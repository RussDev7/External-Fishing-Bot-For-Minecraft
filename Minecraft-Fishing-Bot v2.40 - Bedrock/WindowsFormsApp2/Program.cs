using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing;


namespace WindowsFormsApp2
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

    public class AudioSession : IDisposable
    {
        private readonly Lazy<Icon> _icon;
        private readonly Lazy<Process> _process;
        private readonly IAudioSessionControl2 _control;

        public AudioSession(IAudioSessionControl2 control)
        {
            _control = control;
            control.GetState(out AudioSessionState state);
            State = state;
            control.GetGroupingParam(out Guid guid);
            GroupingParam = guid;
            IconPath = GetString(control.GetIconPath);
            DisplayName = GetString(control.GetDisplayName);
            _icon = new Lazy<Icon>(GetIcon, true);
            _process = new Lazy<Process>(() => Process.GetProcessById(ProcessId), true);
            Id = GetString(control.GetSessionIdentifier);
            InstanceId = GetString(control.GetSessionInstanceIdentifier);
            control.GetProcessId(out int pid);
            ProcessId = pid;
            IsSystemSounds = control.IsSystemSoundsSession() == 0;
        }

        public AudioSessionState State { get; }
        public string IconPath { get; }
        public string DisplayName { get; }
        public Guid GroupingParam { get; }
        public Icon Icon => _icon.Value;
        public string Id { get; }
        public string InstanceId { get; }
        public int ProcessId { get; }

        public Process Process => _process.Value;
        public bool IsSystemSounds { get; }

        public float[] GetChannelsPeakValues()
        {
            IAudioMeterInformation meter = (IAudioMeterInformation)_control;
            meter.GetMeteringChannelCount(out int channelCount);
            float[] values = new float[channelCount];
            meter.GetChannelsPeakValues(channelCount, values);
            return values;
        }

        private delegate int GetStringFn(out IntPtr ptr);
        private static string GetString(GetStringFn fn)
        {
            fn(out IntPtr ptr);
            if (ptr == IntPtr.Zero)
                return null;

            try
            {
                string s = Marshal.PtrToStringUni(ptr);
                if (!string.IsNullOrWhiteSpace(s) && s.StartsWith("@"))
                {
                    StringBuilder sb = new StringBuilder(256);
                    if (SHLoadIndirectString(s, sb, sb.Capacity, IntPtr.Zero) == 0)
                    {
                        s = sb.ToString();
                    }
                }
                return s;
            }
            finally
            {
                Marshal.FreeCoTaskMem(ptr);
            }
        }

        private Icon GetIcon()
        {
            if (string.IsNullOrWhiteSpace(IconPath))
                return null;

            int index = ParseIconLocationPath(IconPath, out string path);
            // note this may only work if the OS bitness is the same as this process bitness
            IntPtr hIcon = ExtractIcon(IntPtr.Zero, path, index);
            return hIcon == IntPtr.Zero ? null : Icon.FromHandle(hIcon);
        }

        public override string ToString() => DisplayName;
        public void Dispose() => _icon.Value?.Dispose();

        public static float[] GetSpeakersChannelsPeakValues()
        {
            // get the speakers (1st render + multimedia) device
            IMMDeviceEnumerator deviceEnumerator = (IMMDeviceEnumerator)(new MMDeviceEnumerator());
            deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia, out IMMDevice speakers);
            if (speakers == null)
                return new float[0];

            // get meter information
            speakers.Activate(typeof(IAudioMeterInformation).GUID, 0, IntPtr.Zero, out object o);
            IAudioMeterInformation meter = (IAudioMeterInformation)o;
            if (meter == null)
                return new float[0];

            meter.GetMeteringChannelCount(out int count);
            if (count == 0)
                return new float[0];

            float[] values = new float[count];
            meter.GetChannelsPeakValues(count, values);
            return values;
        }

        public static IEnumerable<AudioSession> EnumerateAll()
        {
            // get the speakers (1st render + multimedia) device
            IMMDeviceEnumerator deviceEnumerator = (IMMDeviceEnumerator)(new MMDeviceEnumerator());
            deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia, out IMMDevice speakers);
            if (speakers == null)
                yield break;

            // activate the session manager, we need the enumerator
            speakers.Activate(typeof(IAudioSessionManager2).GUID, 0, IntPtr.Zero, out object o);
            IAudioSessionManager2 sessionManager = (IAudioSessionManager2)o;
            if (sessionManager == null)
                yield break;

            // enumerate sessions for on this device
            sessionManager.GetSessionEnumerator(out IAudioSessionEnumerator sessionEnumerator);
            sessionEnumerator.GetCount(out int count);

            for (int i = 0; i < count; i++)
            {
                sessionEnumerator.GetSession(i, out IAudioSessionControl2 sessionControl);
                if (sessionControl != null)
                {
                    IAudioMeterInformation meter = sessionControl as IAudioMeterInformation;
                    yield return new AudioSession(sessionControl);
                }
            }
        }

        [DllImport("shlwapi", CharSet = CharSet.Unicode)]
        private extern static int SHLoadIndirectString(string pszSource, StringBuilder pszOutBuf, int cchOutBuf, IntPtr ppvReserved);

        [DllImport("shlwapi", CharSet = CharSet.Unicode)]
        private static extern int PathParseIconLocation(string pszIconFile);

        [DllImport("shell32", CharSet = CharSet.Unicode)]
        private static extern IntPtr ExtractIcon(IntPtr ptr, string pszExeFileName, int nIconIndex);

        private static int ParseIconLocationPath(string location, out string path)
        {
            if (location == null)
                throw new ArgumentNullException(nameof(location));

            path = string.Copy(location);
            int index = PathParseIconLocation(path);
            int pos = path.LastIndexOf('\0');
            if (pos >= 0)
            {
                path = path.Substring(0, pos);
            }

            if (path.StartsWith("@"))
            {
                path = path.Substring(1);
            }
            return index;
        }
    }

    [ComImport]
    [Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
    public class MMDeviceEnumerator
    {
    }

    public enum EDataFlow
    {
        eRender,
        eCapture,
        eAll,
        EDataFlow_enum_count
    }

    public enum ERole
    {
        eConsole,
        eMultimedia,
        eCommunications,
        ERole_enum_count
    }

    [Guid("a95664d2-9614-4f35-a746-de8db63617e6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public partial interface IMMDeviceEnumerator
    {
        [PreserveSig]
        int EnumAudioEndpoints(EDataFlow dataFlow, uint dwStateMask, out IntPtr ppDevices);

        [PreserveSig]
        int GetDefaultAudioEndpoint(EDataFlow dataFlow, ERole role, out IMMDevice ppEndpoint);

        [PreserveSig]
        int GetDevice([MarshalAs(UnmanagedType.LPWStr)] string pwstrId, out IMMDevice ppDevice);

        [PreserveSig]
        int RegisterEndpointNotificationCallback(IntPtr pClient);

        [PreserveSig]
        int UnregisterEndpointNotificationCallback(IntPtr pClient);
    }

    [Guid("d666063f-1587-4e43-81f1-b948e807363f"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public partial interface IMMDevice
    {
        [PreserveSig]
        int Activate([MarshalAs(UnmanagedType.LPStruct)] Guid iid, uint dwClsCtx, [In, Out] IntPtr pActivationParams, [MarshalAs(UnmanagedType.IUnknown)] out object ppInterface);

        [PreserveSig]
        int OpenPropertyStore(uint stgmAccess, out IntPtr ppProperties);

        [PreserveSig]
        int GetId(out IntPtr ppstrId);

        [PreserveSig]
        int GetState(out uint pdwState);
    }

    [Guid("C02216F6-8C67-4B5B-9D00-D008E73E0064"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IAudioMeterInformation
    {
        [PreserveSig]
        int GetPeakValue(out float pfPeak);

        [PreserveSig]
        int GetMeteringChannelCount(out int pnChannelCount);

        [PreserveSig]
        int GetChannelsPeakValues(int u32ChannelCount, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] float[] afPeakValues);

        [PreserveSig]
        int QueryHardwareSupport(out int pdwHardwareSupportMask);
    }

    [Guid("77aa99a0-1bd6-484f-8bc7-2c654c9a9b6f"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public partial interface IAudioSessionManager2
    {
        // IAudioSessionManager
        [PreserveSig]
        int GetAudioSessionControl(IntPtr AudioSessionGuid, uint StreamFlags, out IAudioSessionControl2 SessionControl);

        [PreserveSig]
        int GetSimpleAudioVolume(IntPtr AudioSessionGuid, uint StreamFlags, out IntPtr AudioVolume);

        // IAudioSessionManager2
        [PreserveSig]
        int GetSessionEnumerator(out IAudioSessionEnumerator SessionEnum);

        [PreserveSig]
        int RegisterSessionNotification(IntPtr SessionNotification);

        [PreserveSig]
        int UnregisterSessionNotification(IntPtr SessionNotification);

        [PreserveSig]
        int RegisterDuckNotification([MarshalAs(UnmanagedType.LPWStr)] string sessionID, IntPtr duckNotification);

        [PreserveSig]
        int UnregisterDuckNotification(IntPtr duckNotification);
    }

    [Guid("E2F5BB11-0570-40CA-ACDD-3AA01277DEE8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IAudioSessionEnumerator
    {
        [PreserveSig]
        int GetCount(out int SessionCount);

        [PreserveSig]
        int GetSession(int SessionCount, out IAudioSessionControl2 Session);
    }

    public enum AudioSessionState
    {
        AudioSessionStateInactive = 0,
        AudioSessionStateActive = 1,
        AudioSessionStateExpired = 2,
    }

    [Guid("bfb7ff88-7239-4fc9-8fa2-07c950be9c6d"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public partial interface IAudioSessionControl2
    {
        // IAudioSessionControl
        [PreserveSig]
        int GetState(out AudioSessionState pRetVal);

        [PreserveSig]
        int GetDisplayName(out IntPtr pRetVal);

        [PreserveSig]
        int SetDisplayName([MarshalAs(UnmanagedType.LPWStr)] string Value, [MarshalAs(UnmanagedType.LPStruct)] Guid EventContext);

        [PreserveSig]
        int GetIconPath(out IntPtr pRetVal);

        [PreserveSig]
        int SetIconPath([MarshalAs(UnmanagedType.LPWStr)] string Value, [MarshalAs(UnmanagedType.LPStruct)] Guid EventContext);

        [PreserveSig]
        int GetGroupingParam(out Guid pRetVal);

        [PreserveSig]
        int SetGroupingParam([MarshalAs(UnmanagedType.LPStruct)] Guid Override, [MarshalAs(UnmanagedType.LPStruct)] Guid EventContext);

        [PreserveSig]
        int RegisterAudioSessionNotification(IntPtr NewNotifications);

        [PreserveSig]
        int UnregisterAudioSessionNotification(IntPtr NewNotifications);

        // IAudioSessionControl2
        [PreserveSig]
        int GetSessionIdentifier(out IntPtr pRetVal);

        [PreserveSig]
        int GetSessionInstanceIdentifier(out IntPtr pRetVal);

        [PreserveSig]
        int GetProcessId(out int pRetVal);

        [PreserveSig]
        int IsSystemSoundsSession();

        [PreserveSig]
        int SetDuckingPreference(bool optOut);
    }

}