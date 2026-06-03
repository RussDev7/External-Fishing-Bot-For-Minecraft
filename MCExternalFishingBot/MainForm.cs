using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Linq;
using System;

namespace MCExternalFishingBot
{
    /// <summary>
    /// Main WinForms window for the external Minecraft auto-fishing bot.
    /// </summary>
    public partial class MainForm : Form
    {
        #region Constants and State

        // Say Hello To Decompilers
        #pragma warning disable CS0414
        private static readonly string HelloThere = "Hello There Fellow Decompiler, This Program Was Made By Discord:dannyruss.";
        #pragma warning restore CS0414

        private string VersionNumber => GetFileVersionDisplay();

        /// <summary>
        /// Supported Minecraft editions. Java checks both javaw.exe and java.exe; Bedrock checks Minecraft.Windows.exe.
        /// </summary>
        private enum MinecraftGameVersion
        {
            Java,
            Bedrock
        }

        private const string JavaWindowedProcessName = "javaw";
        private const string JavaConsoleProcessName = "java";
        private const string BedrockProcessName = "Minecraft.Windows";

        private MinecraftGameVersion _selectedGameVersion = MinecraftGameVersion.Java;

        private int RodHealth = 1;

        // private readonly int SaveHP = 63;

        private int TotalLines = 0;

        private int ScrollReject = 0;

        private int CheckGame = 0;

        private int DummyScroll = 0;

        private const int MOUSEEVENTF_WHEEL = 2048;

        // The amount of movement is specified in mouseData.
        private const int MOUSEEVENTF_HWHEEL = 4096;

        private const int VK_ESCAPE = 27;

        private const int KEYEVENTF_EXTENDEDKEY = 1;

        private const int KEYEVENTF_KEYDOWN = 0;

        private const int KEYEVENTF_KEYUP = 2;

        private const int MOUSEEVENTF_LEFTDOWN = 2;

        public delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);

        // private readonly int currentProcessID;

        #endregion

        #region Native Methods

        [DllImport("user32.dll", EntryPoint = "mouse_event")]
        static extern void mouse_event(Int32 dwFlags, Int32 dx, Int32 dy, Int32 cButtons, Int32 dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern int ShowWindow(IntPtr handle, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        internal static class NativeMethods
        {
            [DllImport("user32.dll")]
            public static extern bool RegisterHotKey(IntPtr windowHandle, int hotkeyId, uint modifierKeys, uint virtualKey);

            [DllImport("user32.dll")]
            public static extern bool UnregisterHotKey(IntPtr windowHandle, int hotkeyId);
        }

        public enum MouseEventFlags : System.UInt32
        {

            MOUSEEVENTF_ABSOLUTE = 32768,

            MOUSEEVENTF_LEFTDOWN = 2,

            MOUSEEVENTF_LEFTUP = 4,

            MOUSEEVENTF_MIDDLEDOWN = 32,

            MOUSEEVENTF_MIDDLEUP = 64,

            MOUSEEVENTF_MOVE = 1,

            MOUSEEVENTF_RIGHTDOWN = 8,

            MOUSEEVENTF_RIGHTUP = 16,

            MOUSEEVENTF_XDOWN = 128,

            MOUSEEVENTF_XUP = 256,

            MOUSEEVENTF_WHEEL = 2048,

            MOUSEEVENTF_HWHEEL = 4096,
        }

        /// <summary>
        /// Sends mouse wheel input.
        /// </summary>
        /// <param name="up">True to scroll up; false to scroll down.</param>
        /// <param name="clicks">Number of wheel clicks to send.</param>
        private void MouseScroll(bool up, int clicks)
        {
            if (up)
            {
                MainForm.mouse_event(MOUSEEVENTF_WHEEL, 0, 0, clicks * 120, 0);
            }
            else
            {
                MainForm.mouse_event(MOUSEEVENTF_WHEEL, 0, 0, clicks * 120 * -1, 0);
            }

        }
        #endregion

        #region Construction

        /// <summary>
        /// Initializes the main form and all designer controls.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            SetMinecraftGameVersion(MinecraftGameVersion.Java);
        }
        #endregion

        #region Minecraft Process Helpers

        /// <summary>
        /// Gets the process names that should be treated as Minecraft for the selected game version.
        /// </summary>
        private string[] GetSelectedMinecraftProcessNames()
        {
            switch (_selectedGameVersion)
            {
                case MinecraftGameVersion.Bedrock:
                    return new[] { BedrockProcessName };

                case MinecraftGameVersion.Java:
                default:
                    return new[] { JavaWindowedProcessName, JavaConsoleProcessName };
            }
        }

        /// <summary>
        /// Returns true when a process name belongs to the selected Minecraft version.
        /// </summary>
        private bool IsSelectedMinecraftProcessName(string processName)
        {
            if (string.IsNullOrEmpty(processName))
            {
                return false;
            }

            return GetSelectedMinecraftProcessNames()
                .Any(name => string.Equals(name, processName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Safely checks whether a process belongs to the selected Minecraft version.
        /// </summary>
        private bool IsMinecraftProcess(Process process)
        {
            if (process == null)
            {
                return false;
            }

            try
            {
                return IsSelectedMinecraftProcessName(process.ProcessName);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets all running Minecraft processes that match the selected game version.
        /// </summary>
        private Process[] GetMinecraftProcesses()
        {
            return Process.GetProcesses()
                .Where(IsMinecraftProcess)
                .ToArray();
        }

        /// <summary>
        /// Returns true when the selected Minecraft version is currently running.
        /// </summary>
        private bool IsMinecraftRunning()
        {
            return GetMinecraftProcesses().Length > 0;
        }

        /// <summary>
        /// Attempts to get the first running Minecraft process for the selected game version.
        /// </summary>
        private bool TryGetFirstMinecraftProcess(out Process process)
        {
            process = GetMinecraftProcesses().FirstOrDefault();
            return process != null;
        }

        /// <summary>
        /// Counts matching Minecraft processes for display in the form.
        /// </summary>
        private int GetMinecraftProcessCount()
        {
            return GetMinecraftProcesses().Length;
        }

        /// <summary>
        /// Kills all matching Minecraft processes for the currently selected game version.
        /// </summary>
        /// <remarks>
        /// NOTE: This preserves the original "Auto leave world/server" behavior, but now respects
        /// the selected Java/Bedrock version instead of hard-coding javaw.exe.
        /// </remarks>
        private void KillMinecraftProcesses()
        {
            foreach (Process process in GetMinecraftProcesses())
            {
                try
                {
                    process.Kill();
                }
                catch
                {
                    // Ignore failed kills so one protected/exited process does not block bot shutdown cleanup.
                }
            }
        }

        /// <summary>
        /// Updates the selected Minecraft game version from the status-strip dropdown.
        /// </summary>
        private void SetMinecraftGameVersion(MinecraftGameVersion gameVersion)
        {
            _selectedGameVersion = gameVersion;
            CheckGame = 0;
            Version_ToolStripDropDownButton.Text = "Version: " + gameVersion;
        }
        #endregion

        #region Version Helpers

        /// <summary>
        /// Gets the file version from the compiled executable's assembly metadata.
        /// This reflects Project Properties > Assembly Information > File version.
        /// </summary>
        private static string GetFileVersionDisplay()
        {
            try
            {
                string fileVersion = FileVersionInfo
                    .GetVersionInfo(Application.ExecutablePath)
                    .FileVersion;

                if (string.IsNullOrWhiteSpace(fileVersion))
                    return "0.0";

                // Optional: Convert "2.41.0.0" into "2.41"
                if (Version.TryParse(fileVersion, out Version version))
                {
                    if (version.Build == 0 && version.Revision == 0)
                        return $"{version.Major}.{version.Minor}";

                    if (version.Revision == 0)
                        return $"{version.Major}.{version.Minor}.{version.Build}";
                }

                return fileVersion;
            }
            catch
            {
                return "0.0";
            }
        }
        #endregion

        #region Audio Detection / Main Bot Loop

        /// <summary>
        /// Polls the selected Minecraft audio session and triggers the fishing macro when a sound spike is detected.
        /// </summary>
        private async void AudioDetectionTimer_Tick(object sender, EventArgs e)
        {

            // Monitor the selected Minecraft audio session.
            // If you want the whole speakers peak values, use the AudioSession.GetSpeakersChannelsPeakValues() method
            foreach (AudioSession session in AudioSession.EnumerateAll())
            {
                if (IsMinecraftProcess(session.Process))
                {
                    float[] values = session.GetChannelsPeakValues();
                    if (values.Length == 0) continue;

                    // Type Values To Console
                    //Console.WriteLine(string.Join(" ", values.Select(v => v.ToString("00%"))));

                    // Send Values To GUI
                    var Label100String = string.Join(" ", values.Select(v => v.ToString("00%")));
                    string firstTwoChars = Label100String.Length <= 2 ? Label100String : Label100String.Substring(0, 2);

                    string VPInput = TriggerBotAtValue_NumericUpDown.Text;
                    string s1 = firstTwoChars;
                    int s1int = Int32.Parse(s1);
                    int s2int = Int32.Parse(VPInput);

                    // toolStripStatusLabel5.Text = s1;

                    // this will give result false as  
                    // both s1 and s2 are different 

                    // this will give result false as  
                    // both s1 and s2 are different 
                    if (s1int <= s2int)
                    {
                    }
                    else
                    {

                        //  Sound Spike Was Found, Run Code
                        AudioDetectionTimer.Stop();
                        AudioDetectionTimer.Enabled = false;
                        if (UnlimitedThrows_CheckBox.Checked == false)
                        {
                            //  Check If Save 1 HP Is Enabled
                            if (Save1HP_CheckBox.Checked == true)
                            {
                                //  1 HP Is Enabled
                                //  Check If Scrolling Is Enabled
                                if (UseHotbarScrolling_CheckBox.Checked == true)
                                {
                                    //  Scrolling Enabled
                                    //  Not Normal (SAVE 1 HP)
                                    //  Check If Scroll Is Needed
                                    if (ScrollReject == 8)
                                    {
                                        //  Finished Last Scroll
                                        //  Check To Close APP
                                        if (RodHealth == (CustomThrowsValue_NumericUpDown.Value - 1))
                                        {
                                            if (LeaveWhenFinished_CheckBox.Checked == true)
                                            {
                                                //  Pull In Line Before Throw
                                                await Task.Delay(100);
                                                MainForm.mouse_event(8, 0, 0, 0, 1);
                                                //  RIGHTDOWN
                                                await Task.Delay(100);
                                                MainForm.mouse_event(16, 0, 0, 0, 1);
                                                //  RIGHTUP
                                                //  Terminate the selected Minecraft process
                                                KillMinecraftProcesses();

                                                //  STOP BOT
                                                //  Stop Time-Out
                                                RodTimeoutTimer.Stop();
                                                RodTimeoutTimer.Enabled = false;
                                                TotalLines++;
                                                //  Enable
                                                if (CloseBotWhenFinished_CheckBox.Checked == true)
                                                {
                                                    Close();
                                                }

                                                //  Stop Timmer
                                                AntiAfkTimer.Stop();
                                                AntiAfkTimer.Enabled = false;
                                                BotStatus_GroupBox.Enabled = false;
                                                SoundVUSensitivity_GroupBox.Enabled = false;
                                                FishingOptions_GroupBox.Enabled = false;
                                                CustomWaitValues_GroupBox.Enabled = false;
                                                ServerOptions_GroupBox.Enabled = false;
                                                UnlimitedThrows_CheckBox.Checked = false;
                                                AntiAFKProtection_CheckBox.Checked = false;
                                                CloseBotWhenFinished_CheckBox.Checked = false;
                                                PressToStopBot_Button.Visible = false;
                                                PressToStopBot_Button.Text = "Press To Stop Bot";
                                                PressToStartBot_Button.Text = "Press To Start Bot";
                                                PressToStartBot_Button.Visible = true;
                                                CustomThrows_CheckBox.Checked = false;
                                                UseHotbarScrolling_CheckBox.Checked = false;
                                                Save1HP_CheckBox.Checked = false;
                                                AutoEjectRod_CheckBox.Checked = false;
                                                LeaveWhenFinished_CheckBox.Checked = false;
                                                Debug_CheckBox1.Checked = false;
                                                MessageBox.Show("Bot Has Finished!", "Minecraft Auto-Fishing Bot v" + VersionNumber, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }
                                            else
                                            {
                                                //  Pull In Line Before Throw
                                                await Task.Delay(100);
                                                MainForm.mouse_event(8, 0, 0, 0, 1);
                                                //  RIGHTDOWN
                                                await Task.Delay(100);
                                                MainForm.mouse_event(16, 0, 0, 0, 1);
                                                //  RIGHTUP
                                                //  STOP BOT
                                                //  Stop Time-Out
                                                RodTimeoutTimer.Stop();
                                                RodTimeoutTimer.Enabled = false;
                                                TotalLines++;
                                                //  Enable
                                                if (CloseBotWhenFinished_CheckBox.Checked == true)
                                                {
                                                    Close();
                                                }

                                                //  Stop Timmer
                                                AntiAfkTimer.Stop();
                                                AntiAfkTimer.Enabled = false;
                                                BotStatus_GroupBox.Enabled = false;
                                                SoundVUSensitivity_GroupBox.Enabled = false;
                                                FishingOptions_GroupBox.Enabled = false;
                                                CustomWaitValues_GroupBox.Enabled = false;
                                                ServerOptions_GroupBox.Enabled = false;
                                                UnlimitedThrows_CheckBox.Checked = false;
                                                AntiAFKProtection_CheckBox.Checked = false;
                                                CloseBotWhenFinished_CheckBox.Checked = false;
                                                PressToStopBot_Button.Visible = false;
                                                PressToStopBot_Button.Text = "Press To Stop Bot";
                                                PressToStartBot_Button.Text = "Press To Start Bot";
                                                PressToStartBot_Button.Visible = true;
                                                CustomThrows_CheckBox.Checked = false;
                                                UseHotbarScrolling_CheckBox.Checked = false;
                                                Save1HP_CheckBox.Checked = false;
                                                AutoEjectRod_CheckBox.Checked = false;
                                                LeaveWhenFinished_CheckBox.Checked = false;
                                                Debug_CheckBox1.Checked = false;
                                                MessageBox.Show("Bot Has Finished!", "Minecraft Auto-Fishing Bot v" + VersionNumber, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }

                                        }
                                        else
                                        {
                                            //  Run Macros
                                            await this.RedirrectYes();
                                        }

                                    }
                                    else
                                    {
                                        //  Check if its the last scroll
                                        if (RodHealth == (CustomThrowsValue_NumericUpDown.Value - 1))
                                        {
                                            //  MAX HP
                                            DummyScroll = 1;
                                            //  Scroll To Next Rod
                                            //  Check To Eject First
                                            //  Pull In Line Before Throw
                                            await Task.Delay(100);
                                            MainForm.mouse_event(8, 0, 0, 0, 1);
                                            //  RIGHTDOWN
                                            await Task.Delay(100);
                                            MainForm.mouse_event(16, 0, 0, 0, 1);
                                            //  RIGHTUP
                                            //  Scroll Hotbar
                                            this.MouseScroll(false, 1);
                                            // scrolls Down 10 wheel clicks
                                            RodHealth = 0;
                                            ScrollReject++;
                                            //  Run Macros
                                            await this.RedirrectYes();
                                        }
                                        else
                                        {
                                            //  Run Macros
                                            await this.RedirrectYes();
                                        }

                                    }

                                }
                                else
                                {
                                    //  Scroll Not Enabled
                                    //  Check if its the last throw
                                    if (RodHealth == (CustomThrowsValue_NumericUpDown.Value - 1))
                                    {
                                        // Finished Last Scroll
                                        //  Check To Close APP
                                        if (LeaveWhenFinished_CheckBox.Checked == true)
                                        {
                                            //  Pull In Line Before Throw
                                            await Task.Delay(100);
                                            MainForm.mouse_event(8, 0, 0, 0, 1);
                                            //  RIGHTDOWN
                                            await Task.Delay(100);
                                            MainForm.mouse_event(16, 0, 0, 0, 1);
                                            //  RIGHTUP
                                            //  Terminate the selected Minecraft process
                                            KillMinecraftProcesses();

                                            //  STOP BOT
                                            //  Stop Time-Out
                                            RodTimeoutTimer.Stop();
                                            RodTimeoutTimer.Enabled = false;
                                            TotalLines++;
                                            //  Enable
                                            if (CloseBotWhenFinished_CheckBox.Checked == true)
                                            {
                                                Close();
                                            }

                                            //  Stop Timmer
                                            AntiAfkTimer.Stop();
                                            AntiAfkTimer.Enabled = false;
                                            BotStatus_GroupBox.Enabled = false;
                                            SoundVUSensitivity_GroupBox.Enabled = false;
                                            FishingOptions_GroupBox.Enabled = false;
                                            CustomWaitValues_GroupBox.Enabled = false;
                                            ServerOptions_GroupBox.Enabled = false;
                                            UnlimitedThrows_CheckBox.Checked = false;
                                            AntiAFKProtection_CheckBox.Checked = false;
                                            CloseBotWhenFinished_CheckBox.Checked = false;
                                            PressToStopBot_Button.Visible = false;
                                            PressToStopBot_Button.Text = "Press To Stop Bot";
                                            PressToStartBot_Button.Text = "Press To Start Bot";
                                            PressToStartBot_Button.Visible = true;
                                            CustomThrows_CheckBox.Checked = false;
                                            UseHotbarScrolling_CheckBox.Checked = false;
                                            Save1HP_CheckBox.Checked = false;
                                            AutoEjectRod_CheckBox.Checked = false;
                                            LeaveWhenFinished_CheckBox.Checked = false;
                                            Debug_CheckBox1.Checked = false;
                                            MessageBox.Show("Bot Has Finished!", "Minecraft Auto-Fishing Bot v" + VersionNumber, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                        else
                                        {
                                            //  Pull In Line Before Throw
                                            await Task.Delay(100);
                                            MainForm.mouse_event(8, 0, 0, 0, 1);
                                            //  RIGHTDOWN
                                            await Task.Delay(100);
                                            MainForm.mouse_event(16, 0, 0, 0, 1);
                                            //  RIGHTUP
                                            //  STOP BOT
                                            //  Stop Time-Out
                                            RodTimeoutTimer.Stop();
                                            RodTimeoutTimer.Enabled = false;
                                            TotalLines++;
                                            //  Enable
                                            if (CloseBotWhenFinished_CheckBox.Checked == true)
                                            {
                                                Close();
                                            }

                                            //  Stop Timmer
                                            AntiAfkTimer.Stop();
                                            AntiAfkTimer.Enabled = false;
                                            BotStatus_GroupBox.Enabled = false;
                                            SoundVUSensitivity_GroupBox.Enabled = false;
                                            FishingOptions_GroupBox.Enabled = false;
                                            CustomWaitValues_GroupBox.Enabled = false;
                                            ServerOptions_GroupBox.Enabled = false;
                                            UnlimitedThrows_CheckBox.Checked = false;
                                            AntiAFKProtection_CheckBox.Checked = false;
                                            CloseBotWhenFinished_CheckBox.Checked = false;
                                            PressToStopBot_Button.Visible = false;
                                            PressToStopBot_Button.Text = "Press To Stop Bot";
                                            PressToStartBot_Button.Text = "Press To Start Bot";
                                            PressToStartBot_Button.Visible = true;
                                            CustomThrows_CheckBox.Checked = false;
                                            UseHotbarScrolling_CheckBox.Checked = false;
                                            Save1HP_CheckBox.Checked = false;
                                            AutoEjectRod_CheckBox.Checked = false;
                                            LeaveWhenFinished_CheckBox.Checked = false;
                                            Debug_CheckBox1.Checked = false;
                                            MessageBox.Show("Bot Has Finished!", "Minecraft Auto-Fishing Bot v" + VersionNumber, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }

                                    }
                                    else
                                    {
                                        //  Run Macros
                                        await this.RedirrectYes();
                                    }

                                }

                            }
                            else
                            {
                                //  1 HP Disabled
                                //  Check If Scrolling Is Enabled
                                if (UseHotbarScrolling_CheckBox.Checked == true)
                                {
                                    //  Scrolling Enabled
                                    //  Not Normal (SAVE 1 HP)
                                    //  Check If Scroll Is Needed
                                    if (RodHealth == CustomThrowsValue_NumericUpDown.Value)
                                    {
                                        //  MAX HP
                                        DummyScroll = 1;
                                        //  Check if its the last scroll
                                        if (ScrollReject == 8)
                                        {
                                            //  Finished Last Scroll
                                            //  Check To Close APP
                                            if (LeaveWhenFinished_CheckBox.Checked == true)
                                            {
                                                //  Check To Eject First
                                                if (AutoEjectRod_CheckBox.Checked == true)
                                                {
                                                    //  Pull In Line Before Throw
                                                    await Task.Delay(100);
                                                    MainForm.mouse_event(8, 0, 0, 0, 1);
                                                    //  RIGHTDOWN
                                                    await Task.Delay(100);
                                                    MainForm.mouse_event(16, 0, 0, 0, 1);
                                                    //  RIGHTUP
                                                    //  Press Middle Mouse Button
                                                    await Task.Delay(100);
                                                    MainForm.mouse_event(32, 0, 0, 0, 1);
                                                    //  MIDDLEDOWN
                                                    await Task.Delay(100);
                                                    MainForm.mouse_event(64, 0, 0, 0, 1);
                                                    //  MIDDLEUP
                                                }
                                                else
                                                {
                                                    //  Pull In Line Before Throw
                                                    await Task.Delay(100);
                                                    MainForm.mouse_event(8, 0, 0, 0, 1);
                                                    //  RIGHTDOWN
                                                    await Task.Delay(100);
                                                    MainForm.mouse_event(16, 0, 0, 0, 1);
                                                    //  RIGHTUP
                                                }

                                                //  Terminate the selected Minecraft process
                                                KillMinecraftProcesses();

                                                //  STOP BOT
                                                //  Stop Time-Out
                                                RodTimeoutTimer.Stop();
                                                RodTimeoutTimer.Enabled = false;
                                                TotalLines++;
                                                //  Enable
                                                if (CloseBotWhenFinished_CheckBox.Checked == true)
                                                {
                                                    Close();
                                                }

                                                //  Stop Timmer
                                                AntiAfkTimer.Stop();
                                                AntiAfkTimer.Enabled = false;
                                                BotStatus_GroupBox.Enabled = false;
                                                SoundVUSensitivity_GroupBox.Enabled = false;
                                                FishingOptions_GroupBox.Enabled = false;
                                                CustomWaitValues_GroupBox.Enabled = false;
                                                ServerOptions_GroupBox.Enabled = false;
                                                UnlimitedThrows_CheckBox.Checked = false;
                                                AntiAFKProtection_CheckBox.Checked = false;
                                                CloseBotWhenFinished_CheckBox.Checked = false;
                                                PressToStopBot_Button.Visible = false;
                                                PressToStopBot_Button.Text = "Press To Stop Bot";
                                                PressToStartBot_Button.Text = "Press To Start Bot";
                                                PressToStartBot_Button.Visible = true;
                                                CustomThrows_CheckBox.Checked = false;
                                                UseHotbarScrolling_CheckBox.Checked = false;
                                                Save1HP_CheckBox.Checked = false;
                                                AutoEjectRod_CheckBox.Checked = false;
                                                LeaveWhenFinished_CheckBox.Checked = false;
                                                Debug_CheckBox1.Checked = false;
                                                MessageBox.Show("Bot Has Finished!", "Minecraft Auto-Fishing Bot v" + VersionNumber, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }
                                            else
                                            {
                                                //  Check To Eject First
                                                if (AutoEjectRod_CheckBox.Checked == true)
                                                {
                                                    //  Pull In Line Before Throw
                                                    await Task.Delay(100);
                                                    MainForm.mouse_event(8, 0, 0, 0, 1);
                                                    //  RIGHTDOWN
                                                    await Task.Delay(100);
                                                    MainForm.mouse_event(16, 0, 0, 0, 1);
                                                    //  RIGHTUP
                                                    //  Press Middle Mouse Button
                                                    await Task.Delay(100);
                                                    MainForm.mouse_event(32, 0, 0, 0, 1);
                                                    //  MIDDLEDOWN
                                                    await Task.Delay(100);
                                                    MainForm.mouse_event(64, 0, 0, 0, 1);
                                                    //  MIDDLEUP
                                                }
                                                else
                                                {
                                                    //  Pull In Line Before Throw
                                                    await Task.Delay(100);
                                                    MainForm.mouse_event(8, 0, 0, 0, 1);
                                                    //  RIGHTDOWN
                                                    await Task.Delay(100);
                                                    MainForm.mouse_event(16, 0, 0, 0, 1);
                                                    //  RIGHTUP
                                                }

                                                //  STOP BOT
                                                //  Stop Time-Out
                                                RodTimeoutTimer.Stop();
                                                RodTimeoutTimer.Enabled = false;
                                                TotalLines++;
                                                //  Enable
                                                if (CloseBotWhenFinished_CheckBox.Checked == true)
                                                {
                                                    Close();
                                                }

                                                //  Stop Timmer
                                                AntiAfkTimer.Stop();
                                                AntiAfkTimer.Enabled = false;
                                                BotStatus_GroupBox.Enabled = false;
                                                SoundVUSensitivity_GroupBox.Enabled = false;
                                                FishingOptions_GroupBox.Enabled = false;
                                                CustomWaitValues_GroupBox.Enabled = false;
                                                ServerOptions_GroupBox.Enabled = false;
                                                UnlimitedThrows_CheckBox.Checked = false;
                                                AntiAFKProtection_CheckBox.Checked = false;
                                                CloseBotWhenFinished_CheckBox.Checked = false;
                                                PressToStopBot_Button.Visible = false;
                                                PressToStopBot_Button.Text = "Press To Stop Bot";
                                                PressToStartBot_Button.Text = "Press To Start Bot";
                                                PressToStartBot_Button.Visible = true;
                                                CustomThrows_CheckBox.Checked = false;
                                                UseHotbarScrolling_CheckBox.Checked = false;
                                                Save1HP_CheckBox.Checked = false;
                                                AutoEjectRod_CheckBox.Checked = false;
                                                LeaveWhenFinished_CheckBox.Checked = false;
                                                Debug_CheckBox1.Checked = false;
                                                MessageBox.Show("Bot Has Finished!", "Minecraft Auto-Fishing Bot v" + VersionNumber, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }

                                        }
                                        else
                                        {
                                            //  Scroll To Next Rod
                                            //  Check To Eject First
                                            if (AutoEjectRod_CheckBox.Checked == true)
                                            {
                                                //  Pull In Line Before Throw
                                                await Task.Delay(100);
                                                MainForm.mouse_event(8, 0, 0, 0, 1);
                                                //  RIGHTDOWN
                                                await Task.Delay(100);
                                                MainForm.mouse_event(16, 0, 0, 0, 1);
                                                //  RIGHTUP
                                                //  Press Middle Mouse Button
                                                await Task.Delay(100);
                                                MainForm.mouse_event(32, 0, 0, 0, 1);
                                                //  MIDDLEDOWN
                                                await Task.Delay(100);
                                                MainForm.mouse_event(64, 0, 0, 0, 1);
                                                //  MIDDLEUP
                                            }
                                            else
                                            {
                                                //  Pull In Line Before Throw
                                                await Task.Delay(100);
                                                MainForm.mouse_event(8, 0, 0, 0, 1);
                                                //  RIGHTDOWN
                                                await Task.Delay(100);
                                                MainForm.mouse_event(16, 0, 0, 0, 1);
                                                //  RIGHTUP
                                            }

                                            //  Scroll Hotbar
                                            this.MouseScroll(false, 1);
                                            // scrolls Down 10 wheel clicks
                                            RodHealth = 0;
                                            ScrollReject++;
                                            //  Run Macros
                                            await this.RedirrectYes();
                                        }

                                    }
                                    else
                                    {
                                        //  Run Macros
                                        await this.RedirrectYes();
                                    }

                                }
                                else
                                {
                                    //  Scrolling Not Enabled
                                    //  Normal (NOT SAVE 1 HP)
                                    //  Check If Max HP Was Found
                                    if (RodHealth == CustomThrowsValue_NumericUpDown.Value)
                                    {
                                        //  MAX HP FOUND
                                        //  Eject Item If Needed
                                        if (AutoEjectRod_CheckBox.Checked == true)
                                        {
                                            //  Pull In Line Before Throw
                                            await Task.Delay(100);
                                            MainForm.mouse_event(8, 0, 0, 0, 1);
                                            //  RIGHTDOWN
                                            await Task.Delay(100);
                                            MainForm.mouse_event(16, 0, 0, 0, 1);
                                            //  RIGHTUP
                                            //  Press Middle Mouse Button
                                            await Task.Delay(100);
                                            MainForm.mouse_event(32, 0, 0, 0, 1);
                                            //  MIDDLEDOWN
                                            await Task.Delay(100);
                                            MainForm.mouse_event(64, 0, 0, 0, 1);
                                            //  MIDDLEUP
                                        }
                                        else
                                        {
                                            //  Pull In Line Before Throw
                                            await Task.Delay(100);
                                            MainForm.mouse_event(8, 0, 0, 0, 1);
                                            //  RIGHTDOWN
                                            await Task.Delay(100);
                                            MainForm.mouse_event(16, 0, 0, 0, 1);
                                            //  RIGHTUP
                                        }

                                        //  Check To Close APP
                                        if (LeaveWhenFinished_CheckBox.Checked == true)
                                        {
                                            //  Terminate the selected Minecraft process
                                            KillMinecraftProcesses();

                                            //  STOP BOT
                                            //  Stop Time-Out
                                            RodTimeoutTimer.Stop();
                                            RodTimeoutTimer.Enabled = false;
                                            TotalLines++;
                                            //  Enable
                                            if (CloseBotWhenFinished_CheckBox.Checked == true)
                                            {
                                                Close();
                                            }

                                            //  Stop Timmer
                                            AntiAfkTimer.Stop();
                                            AntiAfkTimer.Enabled = false;
                                            BotStatus_GroupBox.Enabled = false;
                                            SoundVUSensitivity_GroupBox.Enabled = false;
                                            FishingOptions_GroupBox.Enabled = false;
                                            CustomWaitValues_GroupBox.Enabled = false;
                                            ServerOptions_GroupBox.Enabled = false;
                                            UnlimitedThrows_CheckBox.Checked = false;
                                            AntiAFKProtection_CheckBox.Checked = false;
                                            CloseBotWhenFinished_CheckBox.Checked = false;
                                            PressToStopBot_Button.Visible = false;
                                            PressToStopBot_Button.Text = "Press To Stop Bot";
                                            PressToStartBot_Button.Text = "Press To Start Bot";
                                            PressToStartBot_Button.Visible = true;
                                            CustomThrows_CheckBox.Checked = false;
                                            UseHotbarScrolling_CheckBox.Checked = false;
                                            Save1HP_CheckBox.Checked = false;
                                            AutoEjectRod_CheckBox.Checked = false;
                                            LeaveWhenFinished_CheckBox.Checked = false;
                                            Debug_CheckBox1.Checked = false;
                                            MessageBox.Show("Bot Has Finished!", "Minecraft Auto-Fishing Bot v" + VersionNumber, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                        else
                                        {
                                            //  STOP BOT
                                            //  Stop Time-Out
                                            RodTimeoutTimer.Stop();
                                            RodTimeoutTimer.Enabled = false;
                                            TotalLines++;
                                            //  Enable
                                            if (CloseBotWhenFinished_CheckBox.Checked == true)
                                            {
                                                Close();
                                            }

                                            //  Stop Timmer
                                            AntiAfkTimer.Stop();
                                            AntiAfkTimer.Enabled = false;
                                            BotStatus_GroupBox.Enabled = false;
                                            SoundVUSensitivity_GroupBox.Enabled = false;
                                            FishingOptions_GroupBox.Enabled = false;
                                            CustomWaitValues_GroupBox.Enabled = false;
                                            ServerOptions_GroupBox.Enabled = false;
                                            UnlimitedThrows_CheckBox.Checked = false;
                                            AntiAFKProtection_CheckBox.Checked = false;
                                            CloseBotWhenFinished_CheckBox.Checked = false;
                                            PressToStopBot_Button.Visible = false;
                                            PressToStopBot_Button.Text = "Press To Stop Bot";
                                            PressToStartBot_Button.Text = "Press To Start Bot";
                                            PressToStartBot_Button.Visible = true;
                                            CustomThrows_CheckBox.Checked = false;
                                            UseHotbarScrolling_CheckBox.Checked = false;
                                            Save1HP_CheckBox.Checked = false;
                                            AutoEjectRod_CheckBox.Checked = false;
                                            LeaveWhenFinished_CheckBox.Checked = false;
                                            Debug_CheckBox1.Checked = false;
                                            MessageBox.Show("Bot Has Finished!", "Minecraft Auto-Fishing Bot v" + VersionNumber, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }

                                    }
                                    else
                                    {
                                        //  Run Macros
                                        await this.RedirrectYes();
                                    }

                                }

                            }

                        }
                        else
                        {
                            //  Unlimited Throws Enabled
                            if (Debug_CheckBox1.Checked == false)
                            {
                                //  Run Macros
                                await this.RedirrectYes();
                            }
                            else
                            {
                                //  Stop Bot 
                                //  Pull In Line Before Throw
                                await Task.Delay(100);
                                MainForm.mouse_event(8, 0, 0, 0, 1);
                                //  RIGHTDOWN
                                await Task.Delay(100);
                                MainForm.mouse_event(16, 0, 0, 0, 1);
                                //  RIGHTUP
                                //  Check To Close APP
                                if (LeaveWhenFinished_CheckBox.Checked == true)
                                {
                                    //  Terminate the selected Minecraft process
                                    KillMinecraftProcesses();

                                    //  STOP BOT
                                    //  Stop Time-Out
                                    RodTimeoutTimer.Stop();
                                    RodTimeoutTimer.Enabled = false;
                                    TotalLines++;
                                    //  Enable
                                    if (CloseBotWhenFinished_CheckBox.Checked == true)
                                    {
                                        Close();
                                    }

                                    //  Stop Timmer
                                    AntiAfkTimer.Stop();
                                    AntiAfkTimer.Enabled = false;
                                    BotStatus_GroupBox.Enabled = false;
                                    SoundVUSensitivity_GroupBox.Enabled = false;
                                    FishingOptions_GroupBox.Enabled = false;
                                    CustomWaitValues_GroupBox.Enabled = false;
                                    ServerOptions_GroupBox.Enabled = false;
                                    UnlimitedThrows_CheckBox.Checked = false;
                                    AntiAFKProtection_CheckBox.Checked = false;
                                    CloseBotWhenFinished_CheckBox.Checked = false;
                                    PressToStopBot_Button.Visible = false;
                                    PressToStopBot_Button.Text = "Press To Stop Bot";
                                    PressToStartBot_Button.Text = "Press To Start Bot";
                                    PressToStartBot_Button.Visible = true;
                                    CustomThrows_CheckBox.Checked = false;
                                    UseHotbarScrolling_CheckBox.Checked = false;
                                    Save1HP_CheckBox.Checked = false;
                                    AutoEjectRod_CheckBox.Checked = false;
                                    LeaveWhenFinished_CheckBox.Checked = false;
                                    Debug_CheckBox1.Checked = false;
                                    MessageBox.Show("Bot Has Finished!", "Minecraft Auto-Fishing Bot v" + VersionNumber, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    //  STOP BOT
                                    //  Stop Time-Out
                                    RodTimeoutTimer.Stop();
                                    RodTimeoutTimer.Enabled = false;
                                    TotalLines++;
                                    //  Enable
                                    if (CloseBotWhenFinished_CheckBox.Checked == true)
                                    {
                                        Close();
                                    }

                                    //  Stop Timmer
                                    AntiAfkTimer.Stop();
                                    AntiAfkTimer.Enabled = false;
                                    BotStatus_GroupBox.Enabled = false;
                                    SoundVUSensitivity_GroupBox.Enabled = false;
                                    FishingOptions_GroupBox.Enabled = false;
                                    CustomWaitValues_GroupBox.Enabled = false;
                                    ServerOptions_GroupBox.Enabled = false;
                                    UnlimitedThrows_CheckBox.Checked = false;
                                    AntiAFKProtection_CheckBox.Checked = false;
                                    CloseBotWhenFinished_CheckBox.Checked = false;
                                    PressToStopBot_Button.Visible = false;
                                    PressToStopBot_Button.Text = "Press To Stop Bot";
                                    PressToStartBot_Button.Text = "Press To Start Bot";
                                    PressToStartBot_Button.Visible = true;
                                    CustomThrows_CheckBox.Checked = false;
                                    UseHotbarScrolling_CheckBox.Checked = false;
                                    Save1HP_CheckBox.Checked = false;
                                    AutoEjectRod_CheckBox.Checked = false;
                                    LeaveWhenFinished_CheckBox.Checked = false;
                                    Debug_CheckBox1.Checked = false;
                                    MessageBox.Show("Bot Has Finished!", "Minecraft Auto-Fishing Bot v" + VersionNumber, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }

                            }

                        }

                        break;
                    }

                }
            }

            //  Look For KillSwitch
            if (Debug_CheckBox1.Checked == true)
            {
                //  Stop Time-Out
                RodTimeoutTimer.Stop();
                RodTimeoutTimer.Enabled = false;
                AudioDetectionTimer.Stop();
                AudioDetectionTimer.Enabled = false;
                AntiAfkTimer.Stop();
                AntiAfkTimer.Enabled = false;
                BotStatus_GroupBox.Enabled = true;
                SoundVUSensitivity_GroupBox.Enabled = true;
                FishingOptions_GroupBox.Enabled = true;
                CustomWaitValues_GroupBox.Enabled = true;
                ServerOptions_GroupBox.Enabled = true;
                UnlimitedThrows_CheckBox.Checked = false;
                AntiAFKProtection_CheckBox.Checked = false;
                CloseBotWhenFinished_CheckBox.Checked = false;
                PressToStopBot_Button.Visible = false;
                PressToStopBot_Button.Text = "Press To Stop Bot";
                PressToStartBot_Button.Text = "Press To Start Bot";
                CustomThrows_CheckBox.Checked = false;
                UseHotbarScrolling_CheckBox.Checked = false;
                Save1HP_CheckBox.Checked = false;
                AutoEjectRod_CheckBox.Checked = false;
                LeaveWhenFinished_CheckBox.Checked = false;
                DummyScroll = 0;
                RodHealth = 3;
                Debug_CheckBox1.Checked = false;
                PressToStartBot_Button.Visible = true;
            }

            //  Look for the selected Minecraft process.
            if (Debug_CheckBox3.Checked == false)
            {
                //  Stop Time-Out
                RodTimeoutTimer.Stop();
                RodTimeoutTimer.Enabled = false;
                AudioDetectionTimer.Stop();
                AudioDetectionTimer.Enabled = false;
                AntiAfkTimer.Stop();
                AntiAfkTimer.Enabled = false;
                BotStatus_GroupBox.Enabled = false;
                SoundVUSensitivity_GroupBox.Enabled = false;
                FishingOptions_GroupBox.Enabled = false;
                CustomWaitValues_GroupBox.Enabled = false;
                ServerOptions_GroupBox.Enabled = false;
                UnlimitedThrows_CheckBox.Checked = false;
                AntiAFKProtection_CheckBox.Checked = false;
                CloseBotWhenFinished_CheckBox.Checked = false;
                PressToStopBot_Button.Visible = false;
                PressToStopBot_Button.Text = "Press To Stop Bot";
                PressToStartBot_Button.Text = "Press To Start Bot";
                PressToStartBot_Button.Visible = true;
                CustomThrows_CheckBox.Checked = false;
                UseHotbarScrolling_CheckBox.Checked = false;
                Save1HP_CheckBox.Checked = false;
                AutoEjectRod_CheckBox.Checked = false;
                LeaveWhenFinished_CheckBox.Checked = false;
                Debug_CheckBox1.Checked = false;
                MessageBox.Show("The selected Minecraft process was stopped!", "Minecraft Auto-Fishing Bot v" + VersionNumber, MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }
        #endregion

        #region Fishing Actions

        /// <summary>
        /// Runs the cast/reel macro after the bot detects a fish bite.
        /// </summary>
        private async Task RedirrectYes()
        {
            //  Stop Time-Out
            RodTimeoutTimer.Stop();
            RodTimeoutTimer.Enabled = false;
            TotalLines++;
            RodHealth++;
            //  Do Macro

            // Do Swing Offset
            // Get XY
            int OldPosX = Cursor.Position.X;
            int OldPosY = Cursor.Position.Y;
            if (SwingOffset_CheckBox.Checked)
            {
                // Move Point
                Cursor.Position = new Point(OldPosX, OldPosY + (int)SwingOffsetValue_NumericUpDown.Value);

                //  Wait Line-In Seconds
                await Task.Delay(100);
            }

            //  Do Not Do if No Pole In INV
            if (DummyScroll == 0)
            {
                int Num3 = Convert.ToInt32(LineInDelayValue_NumericUpDown.Value);
                await Task.Delay(Num3);
                //  Wait Line-In Seconds
                await Task.Delay(100);
                MainForm.mouse_event(8, 0, 0, 0, 1);
                //  RIGHTDOWN
                await Task.Delay(100);
                MainForm.mouse_event(16, 0, 0, 0, 1);
                //  RIGHTUP
            }
            else
            {
                //  Reset DummyScroll
                DummyScroll = 0;
            }

            int Num4 = Convert.ToInt32(LineOutDelayValue_NumericUpDown.Value);
            await Task.Delay(Num4);
            //  Normalize Swing Offset 
            if (SwingOffset_CheckBox.Checked)
            {
                await Task.Delay(100);

                // Move Point
                Cursor.Position = new Point(OldPosX, OldPosY - (int)SwingOffsetValue_NumericUpDown.Value);

                //  Wait Line-In Seconds
                await Task.Delay(100);
            }
            //  Wait Line-Out Seconds
            await Task.Delay(100);
            MainForm.mouse_event(8, 0, 0, 0, 1);
            //  RIGHTDOWN
            await Task.Delay(100);
            MainForm.mouse_event(16, 0, 0, 0, 1);
            //  RIGHTUP
            int Num5 = Convert.ToInt32(NoiseDelayValue_NumericUpDown.Value);
            await Task.Delay(Num5);
            //  Wait Kill-Noise Seconds
            //  Start Time-Out
            RodTimeoutTimer.Enabled = true;
            RodTimeoutTimer.Start();
            //  Re-Enable Search
            AudioDetectionTimer.Enabled = true;
            AudioDetectionTimer.Start();
        }
        #endregion

        #region Form Lifecycle

        /// <summary>
        /// Initializes runtime-only form settings and control tooltips after the designer controls are created.
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            // Adjust Timer1
            // Timer1.Interval = 250;

            this.KeyPreview = true;
            //  Tool Tips
            ToolTip toolTip1 = new ToolTip
            {
                ShowAlways = true
            };
            toolTip1.SetToolTip(PressToStopBot_Button, "Stops the current bot session.");
            toolTip1.SetToolTip(PressToStartBot_Button, "Start the bot.");
            toolTip1.SetToolTip(CustomThrows_CheckBox, "Choose an custom amount of throws - Default 64.");
            toolTip1.SetToolTip(UseHotbarScrolling_CheckBox, "Allows to scroll to next rod when broken.");
            toolTip1.SetToolTip(Save1HP_CheckBox, "Save the current rod with 1 HP remaining.");
            toolTip1.SetToolTip(AutoEjectRod_CheckBox, "Auto tosses rod. Must bind toss item as middle-mouse.");
            toolTip1.SetToolTip(LeaveWhenFinished_CheckBox, "Auto leaves world/server by closing minecraft.");
            toolTip1.SetToolTip(UnlimitedThrows_CheckBox, "Unlimited casts of current rod. Great for creative.");
            toolTip1.SetToolTip(CloseBotWhenFinished_CheckBox, "Close the bot when its session is over.");
            toolTip1.SetToolTip(AntiAFKProtection_CheckBox, "Attempts to override AFK. Must bind shift as left-mouse.");
            toolTip1.SetToolTip(CustomThrowsValue_NumericUpDown, "Select a custom number.");
            toolTip1.SetToolTip(StartBotDelay_NumericUpDown, "Set a custom delay before running bot.");
            toolTip1.SetToolTip(LineInDelayValue_NumericUpDown, "Set a custom delay before pulling in line.");
            toolTip1.SetToolTip(LineOutDelayValue_NumericUpDown, "Set a custom delay before casting new line.");
            toolTip1.SetToolTip(NoiseDelayValue_NumericUpDown, "Set a custom delay before continuing loop.");
            toolTip1.SetToolTip(TriggerBotAtValue_NumericUpDown, "Set the level of sound needed to trigger the bot.");
            toolTip1.SetToolTip(SwingOffset_CheckBox, "When catching, look down to toss. Minecraft v1.13+");
            toolTip1.SetToolTip(SwingOffsetValue_NumericUpDown, "Distance to look down from toss spot.");
        }
        #endregion

        #region Status / Process Polling

        /// <summary>
        /// Refreshes counters, process status, version display, and live UI state.
        /// </summary>
        // [Obsolete]
        private void StatusRefreshTimer_Tick(object sender, EventArgs e)
        {
            //  Handles For Lables
            string TL = TotalLines.ToString();
            TotalLinesPulledValue_Label.Text = TL;

            string RH = RodHealth.ToString();
            CurrentRodCastsValue_Label.Text = RH;

            string SR = ScrollReject.ToString();
            int SR1 = Int32.Parse(SR) + 1;
            string SR2 = SR1.ToString();
            CurrentScrollRodValue_Label.Text = SR2;

            //  X, Y coords bottom toolbar
            XY_TextBox.Text = " X:"
                        + Cursor.Position.X + (", Y:" + Cursor.Position.Y);
            //  Count matching Minecraft processes for the selected version.
            int count = GetMinecraftProcessCount();

            string JCount = count.ToString();
            GameProcessesValue_Label.Text = JCount;
            //  Check for the selected Minecraft process
            if (IsMinecraftRunning())
            {

                GameStatusValue_ToolStripStatusLabel.ForeColor = Color.DarkGreen;
                GameStatusValue_ToolStripStatusLabel.Text = "Found";
                Debug_CheckBox3.Checked = true;
                if (CheckGame == 0)
                {
                    PressToStartBot_Button.Enabled = true;
                    PressToStartBot_Button.Text = "Press To Start Bot";
                    CheckGame = 1;

                    // Minecraft found: enable client controls.

                    BotStatus_GroupBox.Enabled = true;
                    SoundVUSensitivity_GroupBox.Enabled = true;
                    FishingOptions_GroupBox.Enabled = true;
                    CustomWaitValues_GroupBox.Enabled = true;
                    ServerOptions_GroupBox.Enabled = true;

                }

            }
            else
            {

                GameStatusValue_ToolStripStatusLabel.ForeColor = Color.DarkRed;
                GameStatusValue_ToolStripStatusLabel.Text = "Not Found";
                PressToStartBot_Button.Text = "Looking For Game...";
                CheckGame = 0;
                Debug_CheckBox3.Checked = false;
                BotStatus_GroupBox.Enabled = false;
                SoundVUSensitivity_GroupBox.Enabled = false;
                FishingOptions_GroupBox.Enabled = false;
                CustomWaitValues_GroupBox.Enabled = false;
                ServerOptions_GroupBox.Enabled = false;

            }

            //  Get Memory Usage For Current Application
            Process x = Process.GetCurrentProcess();
            string inf;
            inf = "Mem Usage: "
                        + (x.WorkingSet64 / 1024) + (" K" + "\r\n" + "Paged Memory: "
                        + (x.PagedMemorySize64 / 1024)
                        + " K");
            Resources_Label.Text = inf;

            //  Get selected Minecraft process version.
            if (TryGetFirstMinecraftProcess(out Process minecraftProcess))
            {
                try
                {
                    string minecraftPath = minecraftProcess.MainModule.FileName;
                    if (System.IO.File.Exists(minecraftPath))
                    {
                        var fileVersion = FileVersionInfo.GetVersionInfo(minecraftPath);
                        ProcessVersionValue_Label.Text = fileVersion.FileVersion;
                    }
                    else
                    {
                        //  Path Does Not Exist
                        ProcessVersionValue_Label.Text = "1.0.0.0";
                    }
                }
                catch
                {
                    // Some processes may deny MainModule access depending on permissions/bitness.
                    ProcessVersionValue_Label.Text = "N/A";
                }
            }
            else
            {
                //  Path Does Not Exist
                ProcessVersionValue_Label.Text = "N/A";
            }

            //  For Eject Rod
            if (CustomThrowsValue_NumericUpDown.Value == 64)
            {
                AutoEjectRod_CheckBox.Enabled = false;
                AutoEjectRod_CheckBox.Checked = false;
            }
            else
            {
                AutoEjectRod_CheckBox.Enabled = true;
            }

        }
        #endregion

        #region Bot Controls

        /// <summary>
        /// Starts a bot session after the selected Minecraft version has been detected.
        /// </summary>
        private async void Button5_Click(object sender, EventArgs e)
        {

            if (Debug_CheckBox3.Checked)
            {

                // Start Bot

                //  
                PressToStartBot_Button.Text = "Starting Bot...";
                BotStatus_GroupBox.Enabled = false;
                SoundVUSensitivity_GroupBox.Enabled = false;
                FishingOptions_GroupBox.Enabled = false;
                CustomWaitValues_GroupBox.Enabled = false;
                ServerOptions_GroupBox.Enabled = false;
                PressToStopBot_Button.Enabled = true;
                PressToStopBot_Button.Visible = true;
                PressToStartBot_Button.Visible = false;
                RodHealth = 1;
                ScrollReject = 0;
                TotalLines = 0;
                TotalLinesPulledValue_Label.Text = "0";
                int Num2 = Convert.ToInt32(StartBotDelay_NumericUpDown.Value);
                await Task.Delay(Num2);
                //  Wait Start-Delay Seconds
                //  Throw Line
                await Task.Delay(100);
                mouse_event(8, 0, 0, 0, 1);
                //  RIGHTDOWN
                await Task.Delay(100);
                mouse_event(16, 0, 0, 0, 1);
                //  RIGHTUP
                int Num5 = Convert.ToInt32(NoiseDelayValue_NumericUpDown.Value);
                await Task.Delay(Num5);
                //  Wait Noise Reduction Time
                //  Start Anti-AFK
                if (AntiAFKProtection_CheckBox.Checked == true)
                {
                    AntiAfkTimer.Enabled = true;
                    AntiAfkTimer.Start();
                }

                //  Start Time-Out
                RodTimeoutTimer.Enabled = true;
                RodTimeoutTimer.Start();
                //  Start Bot
                AudioDetectionTimer.Enabled = true;
                AudioDetectionTimer.Start();

            }
            else
            {

                // MC Was Not Started
                MessageBox.Show("Please Start Minecraft.", "Minecraft Auto-Fishing Bot v" + VersionNumber, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }

        }
        #endregion

        #region Form Closing

        /// <summary>
        /// Handles form shutdown. Currently reserved for future cleanup.
        /// </summary>
        private void Form1_Closing(object sender, EventArgs e)
        {
            //  Terminate SndVol

        }
        #endregion

        #region Option Controls

        /// <summary>
        /// Enables custom throw-count controls and disables Save 1 HP when custom throws are active.
        /// </summary>
        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {

            //  Enable/Disable NumericUpDown
            if (CustomThrows_CheckBox.Checked == true)
            {
                CustomThrowsValue_NumericUpDown.Enabled = true;
                Save1HP_CheckBox.Enabled = false;
                Save1HP_CheckBox.Checked = false;
            }
            else
            {
                Save1HP_CheckBox.Enabled = true;
                CustomThrowsValue_NumericUpDown.Enabled = false;
                CustomThrowsValue_NumericUpDown.Value = 64;
            }

        }

        /// <summary>
        /// Toggles Save 1 HP behavior and updates incompatible options.
        /// </summary>
        private void CheckBox3_CheckedChanged(object sender, EventArgs e)
        {

            // Enable/Disable Eject Rod
            if (Save1HP_CheckBox.Checked == true)
            {
                AutoEjectRod_CheckBox.Enabled = false;
                CustomThrows_CheckBox.Enabled = false;
                AutoEjectRod_CheckBox.Checked = false;
            }
            else
            {
                AutoEjectRod_CheckBox.Enabled = true;
                CustomThrows_CheckBox.Enabled = true;
            }

        }

        /// <summary>
        /// Toggles the rod-eject option and updates incompatible options.
        /// </summary>
        private void CheckBox4_CheckedChanged(object sender, EventArgs e)
        {

            if (CustomThrows_CheckBox.Checked == true)
            {

            }
            else
            {
                //  Enable/Disable Eject Rod
                if (AutoEjectRod_CheckBox.Checked == true)
                {
                    Save1HP_CheckBox.Enabled = false;
                    Save1HP_CheckBox.Checked = false;
                }
                else
                {
                    Save1HP_CheckBox.Enabled = true;
                }

            }

        }
        #endregion

        #region Bot Stop / Safety Timers

        /// <summary>
        /// Stops the current bot session and resets the UI to an idle state.
        /// </summary>
        private void Button4_Click(object sender, EventArgs e)
        {

            //  Send Stop State
            PressToStopBot_Button.Enabled = false;
            Debug_CheckBox1.Checked = true;
            PressToStopBot_Button.Text = "Stoping Bot...";

        }

        //  Unlimited Throws
        /// <summary>
        /// Toggles unlimited cast mode and disables incompatible rod-count options.
        /// </summary>
        private void CheckBox9_CheckedChanged(object sender, EventArgs e)
        {

            //  Enable/Disable Unlimited Throws
            if (UnlimitedThrows_CheckBox.Checked == true)
            {
                CustomThrows_CheckBox.Checked = false;
                UseHotbarScrolling_CheckBox.Checked = false;
                Save1HP_CheckBox.Checked = false;
                AutoEjectRod_CheckBox.Checked = false;
                LeaveWhenFinished_CheckBox.Checked = false;
                CloseBotWhenFinished_CheckBox.Checked = false;
                CustomThrows_CheckBox.Enabled = false;
                UseHotbarScrolling_CheckBox.Enabled = false;
                Save1HP_CheckBox.Enabled = false;
                AutoEjectRod_CheckBox.Enabled = false;
                LeaveWhenFinished_CheckBox.Enabled = false;
                CloseBotWhenFinished_CheckBox.Enabled = false;
            }
            else
            {
                CustomThrows_CheckBox.Enabled = true;
                UseHotbarScrolling_CheckBox.Enabled = true;
                Save1HP_CheckBox.Enabled = true;
                AutoEjectRod_CheckBox.Enabled = true;
                LeaveWhenFinished_CheckBox.Enabled = true;
                CloseBotWhenFinished_CheckBox.Enabled = true;
            }

        }

        /// <summary>
        /// Runs the optional anti-AFK click action.
        /// </summary>
        private async void AntiAfkTimer_Tick(object sender, EventArgs e)
        {

            //  Left Down
            await Task.Delay(100);
            mouse_event(2, 0, 0, 0, 1);
            //  RIGHTDOWN
            await Task.Delay(100);
            mouse_event(4, 0, 0, 0, 1);
            //  RIGHTUP

        }

        /// <summary>
        /// Handles bot timeout/no-rod detection and resets the session if needed.
        /// </summary>
        private async void RodTimeoutTimer_Tick(object sender, EventArgs e)
        {

            //  Disable Timmer
            AudioDetectionTimer.Stop();
            AudioDetectionTimer.Enabled = false;
            RodTimeoutTimer.Stop();
            RodTimeoutTimer.Enabled = false;
            if (UseHotbarScrolling_CheckBox.Checked == true)
            {
                //  Check If Its Already Last Throw
                if (ScrollReject == 8)
                {
                    //  It's GameOver End Bot
                    //  STOP BOT
                    //  Stop Time-Out
                    RodTimeoutTimer.Stop();
                    RodTimeoutTimer.Enabled = false;
                    TotalLines++;
                    //  Enable
                    if (CloseBotWhenFinished_CheckBox.Checked == true)
                    {
                        Close();
                    }

                    //  Stop Time-Out
                    RodTimeoutTimer.Stop();
                    RodTimeoutTimer.Enabled = false;
                    AudioDetectionTimer.Stop();
                    AudioDetectionTimer.Enabled = false;
                    AntiAfkTimer.Stop();
                    AntiAfkTimer.Enabled = false;
                    BotStatus_GroupBox.Enabled = false;
                    SoundVUSensitivity_GroupBox.Enabled = false;
                    FishingOptions_GroupBox.Enabled = false;
                    CustomWaitValues_GroupBox.Enabled = false;
                    ServerOptions_GroupBox.Enabled = false;
                    UnlimitedThrows_CheckBox.Checked = false;
                    AntiAFKProtection_CheckBox.Checked = false;
                    CloseBotWhenFinished_CheckBox.Checked = false;
                    PressToStopBot_Button.Visible = false;
                    PressToStopBot_Button.Text = "Press To Stop Bot";
                    PressToStartBot_Button.Text = "Press To Start Bot";
                    PressToStartBot_Button.Visible = true;
                    CustomThrows_CheckBox.Checked = false;
                    UseHotbarScrolling_CheckBox.Checked = false;
                    Save1HP_CheckBox.Checked = false;
                    AutoEjectRod_CheckBox.Checked = false;
                    LeaveWhenFinished_CheckBox.Checked = false;
                    Debug_CheckBox1.Checked = false;
                    MessageBox.Show("No Rod Was Detected! 9th Rod Finished", "Minecraft Auto-Fishing Bot v" + VersionNumber, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    //  Still Good
                    //  Re-Enable Search
                    AudioDetectionTimer.Enabled = false;
                    AudioDetectionTimer.Start();
                    //  Scroll Hotbar
                    MouseScroll(false, 1);
                    // scrolls Down 10 wheel clicks
                    //  Cast Rod
                    int Num4 = Convert.ToInt32(LineOutDelayValue_NumericUpDown.Value);
                    await Task.Delay(Num4);
                    //  Wait Line-Out Seconds
                    await Task.Delay(100);
                    mouse_event(8, 0, 0, 0, 1);
                    //  RIGHTDOWN
                    await Task.Delay(100);
                    mouse_event(16, 0, 0, 0, 1);
                    //  RIGHTUP
                    int Num5 = Convert.ToInt32(NoiseDelayValue_NumericUpDown.Value);
                    await Task.Delay(Num5);
                    //  Wait Kill-Noise Seconds
                    RodHealth = 0;
                    ScrollReject++;
                    //  Start Time-Out
                    RodTimeoutTimer.Enabled = true;
                    RodTimeoutTimer.Start();
                    //  Re-Enable Search
                    AudioDetectionTimer.Enabled = true;
                    AudioDetectionTimer.Start();
                }

            }
            else
            {
                //  Scroll Is Disabled
                //  STOP BOT
                //  Stop Time-Out
                RodTimeoutTimer.Stop();
                RodTimeoutTimer.Enabled = false;
                AudioDetectionTimer.Stop();
                AudioDetectionTimer.Enabled = false;
                AntiAfkTimer.Stop();
                AntiAfkTimer.Enabled = false;
                BotStatus_GroupBox.Enabled = false;
                SoundVUSensitivity_GroupBox.Enabled = false;
                FishingOptions_GroupBox.Enabled = false;
                CustomWaitValues_GroupBox.Enabled = false;
                ServerOptions_GroupBox.Enabled = false;
                UnlimitedThrows_CheckBox.Checked = false;
                AntiAFKProtection_CheckBox.Checked = false;
                CloseBotWhenFinished_CheckBox.Checked = false;
                PressToStopBot_Button.Visible = false;
                PressToStopBot_Button.Text = "Press To Stop Bot";
                PressToStartBot_Button.Text = "Press To Start Bot";
                PressToStartBot_Button.Visible = true;
                CustomThrows_CheckBox.Checked = false;
                UseHotbarScrolling_CheckBox.Checked = false;
                Save1HP_CheckBox.Checked = false;
                AutoEjectRod_CheckBox.Checked = false;
                LeaveWhenFinished_CheckBox.Checked = false;
                Debug_CheckBox1.Checked = false;
                MessageBox.Show("No Rod Was Detected!", "Minecraft Auto-Fishing Bot v" + VersionNumber, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        #endregion

        #region VU Capture

        /// <summary>
        /// Starts a short VU-level capture for the selected Minecraft version.
        /// </summary>
        private void ToolStripStatusLabel6_Click(object sender, EventArgs e)
        {

            //  Check for the selected Minecraft process
            if (IsMinecraftRunning())
            {

                Capture_ToolStripStatusLabel.Text = "- [Working]";

                // Disable Timer
                VuCaptureTimeoutTimer.Enabled = true;

                // Action Timer
                VuLevelCaptureTimer.Enabled = true;

            }
            else
            {

                // MC Was Not Started
                MessageBox.Show("Please Start Minecraft.", "Minecraft Auto-Fishing Bot v" + VersionNumber, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }

        }

        /// <summary>
        /// Stops the temporary VU-level capture timers.
        /// </summary>
        private void VuCaptureTimeoutTimer_Tick(object sender, EventArgs e)
        {

            VuCaptureTimeoutTimer.Enabled = false;
            VuLevelCaptureTimer.Enabled = false;
            Capture_ToolStripStatusLabel.Text = "N/A";
            Capture_ToolStripStatusLabel.Text = "- [Capture]";

        }

        /// <summary>
        /// Displays the current VU level for the selected Minecraft audio session.
        /// </summary>
        private void VuLevelCaptureTimer_Tick(object sender, EventArgs e)
        {
            //  Check for the selected Minecraft process
            if (IsMinecraftRunning())
            {
                // Capture Sound
                foreach (AudioSession session in AudioSession.EnumerateAll())
                {
                    if (IsMinecraftProcess(session.Process))
                    {
                        float[] values = session.GetChannelsPeakValues();
                        if (values.Length == 0) continue;

                        // Type Values To Console
                        //Console.WriteLine(string.Join(" ", values.Select(v => v.ToString("00%"))));

                        // Send Values To GUI
                        var Label100String = string.Join(" ", values.Select(v => v.ToString("00%")));
                        string firstTwoChars = Label100String.Length <= 2 ? Label100String : Label100String.Substring(0, 2);

                        string VPInput = TriggerBotAtValue_NumericUpDown.Text;
                        string s1 = firstTwoChars;
                        int s1int = Int32.Parse(s1);
                        int s2int = Int32.Parse(VPInput);

                        VULevelValue_ToolStripStatusLabel.Text = s1;

                    }

                }

            }
            else
            {

                // MC Was Not Started
                MessageBox.Show("Please Start Minecraft.", "Minecraft Auto-Fishing Bot v" + VersionNumber, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }

        }
        #endregion

        #region Swing Offset

        /// <summary>
        /// Enables or disables the swing-offset distance control.
        /// </summary>
        private void CheckBox12_CheckedChanged(object sender, EventArgs e)
        {
            if (SwingOffset_CheckBox.Checked)
            {
                SwingOffsetValue_NumericUpDown.Enabled = true;
            }
            else
            {
                SwingOffsetValue_NumericUpDown.Enabled = false;
            }
        }
        #endregion

        #region Game Version Selector

        /// <summary>
        /// Changes which Minecraft edition the bot should look for.
        /// </summary>
        /// <remarks>
        /// Java mode intentionally checks both javaw.exe and java.exe, so the old Java-process
        /// selector is no longer needed. Bedrock mode checks Minecraft.Windows.exe.
        /// </remarks>
        private void ToolStripDropDownButton1_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "Java")
            {
                SetMinecraftGameVersion(MinecraftGameVersion.Java);
            }
            else if (e.ClickedItem.Text == "Bedrock")
            {
                SetMinecraftGameVersion(MinecraftGameVersion.Bedrock);
            }
        }
        #endregion
    }
}