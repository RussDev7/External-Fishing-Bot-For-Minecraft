using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
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
        private static readonly string HelloThere = "Hello There Fellow Decompiler, This Program Was Made By Discord:dannyruss.";

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

        private readonly int SaveHP = 63;

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

        private readonly int currentProcessID;

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
            toolStripDropDownButton1.Text = "Version: " + gameVersion;
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
        private void Timer1_Tick(object sender, EventArgs e)
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

                    string VPInput = numericUpDown6.Text;
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
                        Timer1.Stop();
                        Timer1.Enabled = false;
                        if (checkBox9.Checked == false)
                        {
                            //  Check If Save 1 HP Is Enabled
                            if (checkBox3.Checked == true)
                            {
                                //  1 HP Is Enabled
                                //  Check If Scrolling Is Enabled
                                if (checkBox2.Checked == true)
                                {
                                    //  Scrolling Enabled
                                    //  Not Normal (SAVE 1 HP)
                                    //  Check If Scroll Is Needed
                                    if (ScrollReject == 8)
                                    {
                                        //  Finished Last Scroll
                                        //  Check To Close APP
                                        if (RodHealth == (NumericUpDown1.Value - 1))
                                        {
                                            if (checkBox5.Checked == true)
                                            {
                                                //  Pull In Line Before Throw
                                                Thread.Sleep(100);
                                                MainForm.mouse_event(8, 0, 0, 0, 1);
                                                //  RIGHTDOWN
                                                Thread.Sleep(100);
                                                MainForm.mouse_event(16, 0, 0, 0, 1);
                                                //  RIGHTUP
                                                //  Terminate the selected Minecraft process
                                                KillMinecraftProcesses();

                                                //  STOP BOT
                                                //  Stop Time-Out
                                                Timer7.Stop();
                                                Timer7.Enabled = false;
                                                TotalLines++;
                                                //  Enable
                                                if (checkBox11.Checked == true)
                                                {
                                                    Close();
                                                }

                                                //  Stop Timmer
                                                Timer5.Stop();
                                                Timer5.Enabled = false;
                                                GroupBox1.Enabled = false;
                                                GroupBox2.Enabled = false;
                                                GroupBox3.Enabled = false;
                                                GroupBox4.Enabled = false;
                                                GroupBox5.Enabled = false;
                                                checkBox9.Checked = false;
                                                checkBox10.Checked = false;
                                                checkBox11.Checked = false;
                                                Button4.Visible = false;
                                                Button4.Text = "Press To Stop Bot";
                                                Button5.Text = "Press To Start Bot";
                                                Button5.Visible = true;
                                                checkBox1.Checked = false;
                                                checkBox2.Checked = false;
                                                checkBox3.Checked = false;
                                                checkBox4.Checked = false;
                                                checkBox5.Checked = false;
                                                checkBox6.Checked = false;
                                                MessageBox.Show("Bot Has Finished!", "Minecraft Auto-Fishing Bot v" + VersionNumber, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }
                                            else
                                            {
                                                //  Pull In Line Before Throw
                                                Thread.Sleep(100);
                                                MainForm.mouse_event(8, 0, 0, 0, 1);
                                                //  RIGHTDOWN
                                                Thread.Sleep(100);
                                                MainForm.mouse_event(16, 0, 0, 0, 1);
                                                //  RIGHTUP
                                                //  STOP BOT
                                                //  Stop Time-Out
                                                Timer7.Stop();
                                                Timer7.Enabled = false;
                                                TotalLines++;
                                                //  Enable
                                                if (checkBox11.Checked == true)
                                                {
                                                    Close();
                                                }

                                                //  Stop Timmer
                                                Timer5.Stop();
                                                Timer5.Enabled = false;
                                                GroupBox1.Enabled = false;
                                                GroupBox2.Enabled = false;
                                                GroupBox3.Enabled = false;
                                                GroupBox4.Enabled = false;
                                                GroupBox5.Enabled = false;
                                                checkBox9.Checked = false;
                                                checkBox10.Checked = false;
                                                checkBox11.Checked = false;
                                                Button4.Visible = false;
                                                Button4.Text = "Press To Stop Bot";
                                                Button5.Text = "Press To Start Bot";
                                                Button5.Visible = true;
                                                checkBox1.Checked = false;
                                                checkBox2.Checked = false;
                                                checkBox3.Checked = false;
                                                checkBox4.Checked = false;
                                                checkBox5.Checked = false;
                                                checkBox6.Checked = false;
                                                MessageBox.Show("Bot Has Finished!", "Minecraft Auto-Fishing Bot v" + VersionNumber, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }

                                        }
                                        else
                                        {
                                            //  Run Macros
                                            this.RedirrectYes();
                                        }

                                    }
                                    else
                                    {
                                        //  Check if its the last scroll
                                        if (RodHealth == (NumericUpDown1.Value - 1))
                                        {
                                            //  MAX HP
                                            DummyScroll = 1;
                                            //  Scroll To Next Rod
                                            //  Check To Eject First
                                            //  Pull In Line Before Throw
                                            Thread.Sleep(100);
                                            MainForm.mouse_event(8, 0, 0, 0, 1);
                                            //  RIGHTDOWN
                                            Thread.Sleep(100);
                                            MainForm.mouse_event(16, 0, 0, 0, 1);
                                            //  RIGHTUP
                                            //  Scroll Hotbar
                                            this.MouseScroll(false, 1);
                                            // scrolls Down 10 wheel clicks
                                            RodHealth = 0;
                                            ScrollReject++;
                                            //  Run Macros
                                            this.RedirrectYes();
                                        }
                                        else
                                        {
                                            //  Run Macros
                                            this.RedirrectYes();
                                        }

                                    }

                                }
                                else
                                {
                                    //  Scroll Not Enabled
                                    //  Check if its the last throw
                                    if (RodHealth == (NumericUpDown1.Value - 1))
                                    {
                                        // Finished Last Scroll
                                        //  Check To Close APP
                                        if (checkBox5.Checked == true)
                                        {
                                            //  Pull In Line Before Throw
                                            Thread.Sleep(100);
                                            MainForm.mouse_event(8, 0, 0, 0, 1);
                                            //  RIGHTDOWN
                                            Thread.Sleep(100);
                                            MainForm.mouse_event(16, 0, 0, 0, 1);
                                            //  RIGHTUP
                                            //  Terminate the selected Minecraft process
                                            KillMinecraftProcesses();

                                            //  STOP BOT
                                            //  Stop Time-Out
                                            Timer7.Stop();
                                            Timer7.Enabled = false;
                                            TotalLines++;
                                            //  Enable
                                            if (checkBox11.Checked == true)
                                            {
                                                Close();
                                            }

                                            //  Stop Timmer
                                            Timer5.Stop();
                                            Timer5.Enabled = false;
                                            GroupBox1.Enabled = false;
                                            GroupBox2.Enabled = false;
                                            GroupBox3.Enabled = false;
                                            GroupBox4.Enabled = false;
                                            GroupBox5.Enabled = false;
                                            checkBox9.Checked = false;
                                            checkBox10.Checked = false;
                                            checkBox11.Checked = false;
                                            Button4.Visible = false;
                                            Button4.Text = "Press To Stop Bot";
                                            Button5.Text = "Press To Start Bot";
                                            Button5.Visible = true;
                                            checkBox1.Checked = false;
                                            checkBox2.Checked = false;
                                            checkBox3.Checked = false;
                                            checkBox4.Checked = false;
                                            checkBox5.Checked = false;
                                            checkBox6.Checked = false;
                                            MessageBox.Show("Bot Has Finished!", "Minecraft Auto-Fishing Bot v" + VersionNumber, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                        else
                                        {
                                            //  Pull In Line Before Throw
                                            Thread.Sleep(100);
                                            MainForm.mouse_event(8, 0, 0, 0, 1);
                                            //  RIGHTDOWN
                                            Thread.Sleep(100);
                                            MainForm.mouse_event(16, 0, 0, 0, 1);
                                            //  RIGHTUP
                                            //  STOP BOT
                                            //  Stop Time-Out
                                            Timer7.Stop();
                                            Timer7.Enabled = false;
                                            TotalLines++;
                                            //  Enable
                                            if (checkBox11.Checked == true)
                                            {
                                                Close();
                                            }

                                            //  Stop Timmer
                                            Timer5.Stop();
                                            Timer5.Enabled = false;
                                            GroupBox1.Enabled = false;
                                            GroupBox2.Enabled = false;
                                            GroupBox3.Enabled = false;
                                            GroupBox4.Enabled = false;
                                            GroupBox5.Enabled = false;
                                            checkBox9.Checked = false;
                                            checkBox10.Checked = false;
                                            checkBox11.Checked = false;
                                            Button4.Visible = false;
                                            Button4.Text = "Press To Stop Bot";
                                            Button5.Text = "Press To Start Bot";
                                            Button5.Visible = true;
                                            checkBox1.Checked = false;
                                            checkBox2.Checked = false;
                                            checkBox3.Checked = false;
                                            checkBox4.Checked = false;
                                            checkBox5.Checked = false;
                                            checkBox6.Checked = false;
                                            MessageBox.Show("Bot Has Finished!", "Minecraft Auto-Fishing Bot v" + VersionNumber, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }

                                    }
                                    else
                                    {
                                        //  Run Macros
                                        this.RedirrectYes();
                                    }

                                }

                            }
                            else
                            {
                                //  1 HP Disabled
                                //  Check If Scrolling Is Enabled
                                if (checkBox2.Checked == true)
                                {
                                    //  Scrolling Enabled
                                    //  Not Normal (SAVE 1 HP)
                                    //  Check If Scroll Is Needed
                                    if (RodHealth == NumericUpDown1.Value)
                                    {
                                        //  MAX HP
                                        DummyScroll = 1;
                                        //  Check if its the last scroll
                                        if (ScrollReject == 8)
                                        {
                                            //  Finished Last Scroll
                                            //  Check To Close APP
                                            if (checkBox5.Checked == true)
                                            {
                                                //  Check To Eject First
                                                if (checkBox4.Checked == true)
                                                {
                                                    //  Pull In Line Before Throw
                                                    Thread.Sleep(100);
                                                    MainForm.mouse_event(8, 0, 0, 0, 1);
                                                    //  RIGHTDOWN
                                                    Thread.Sleep(100);
                                                    MainForm.mouse_event(16, 0, 0, 0, 1);
                                                    //  RIGHTUP
                                                    //  Press Middle Mouse Button
                                                    Thread.Sleep(100);
                                                    MainForm.mouse_event(32, 0, 0, 0, 1);
                                                    //  MIDDLEDOWN
                                                    Thread.Sleep(100);
                                                    MainForm.mouse_event(64, 0, 0, 0, 1);
                                                    //  MIDDLEUP
                                                }
                                                else
                                                {
                                                    //  Pull In Line Before Throw
                                                    Thread.Sleep(100);
                                                    MainForm.mouse_event(8, 0, 0, 0, 1);
                                                    //  RIGHTDOWN
                                                    Thread.Sleep(100);
                                                    MainForm.mouse_event(16, 0, 0, 0, 1);
                                                    //  RIGHTUP
                                                }

                                                //  Terminate the selected Minecraft process
                                                KillMinecraftProcesses();

                                                //  STOP BOT
                                                //  Stop Time-Out
                                                Timer7.Stop();
                                                Timer7.Enabled = false;
                                                TotalLines++;
                                                //  Enable
                                                if (checkBox11.Checked == true)
                                                {
                                                    Close();
                                                }

                                                //  Stop Timmer
                                                Timer5.Stop();
                                                Timer5.Enabled = false;
                                                GroupBox1.Enabled = false;
                                                GroupBox2.Enabled = false;
                                                GroupBox3.Enabled = false;
                                                GroupBox4.Enabled = false;
                                                GroupBox5.Enabled = false;
                                                checkBox9.Checked = false;
                                                checkBox10.Checked = false;
                                                checkBox11.Checked = false;
                                                Button4.Visible = false;
                                                Button4.Text = "Press To Stop Bot";
                                                Button5.Text = "Press To Start Bot";
                                                Button5.Visible = true;
                                                checkBox1.Checked = false;
                                                checkBox2.Checked = false;
                                                checkBox3.Checked = false;
                                                checkBox4.Checked = false;
                                                checkBox5.Checked = false;
                                                checkBox6.Checked = false;
                                                MessageBox.Show("Bot Has Finished!", "Minecraft Auto-Fishing Bot v" + VersionNumber, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }
                                            else
                                            {
                                                //  Check To Eject First
                                                if (checkBox4.Checked == true)
                                                {
                                                    //  Pull In Line Before Throw
                                                    Thread.Sleep(100);
                                                    MainForm.mouse_event(8, 0, 0, 0, 1);
                                                    //  RIGHTDOWN
                                                    Thread.Sleep(100);
                                                    MainForm.mouse_event(16, 0, 0, 0, 1);
                                                    //  RIGHTUP
                                                    //  Press Middle Mouse Button
                                                    Thread.Sleep(100);
                                                    MainForm.mouse_event(32, 0, 0, 0, 1);
                                                    //  MIDDLEDOWN
                                                    Thread.Sleep(100);
                                                    MainForm.mouse_event(64, 0, 0, 0, 1);
                                                    //  MIDDLEUP
                                                }
                                                else
                                                {
                                                    //  Pull In Line Before Throw
                                                    Thread.Sleep(100);
                                                    MainForm.mouse_event(8, 0, 0, 0, 1);
                                                    //  RIGHTDOWN
                                                    Thread.Sleep(100);
                                                    MainForm.mouse_event(16, 0, 0, 0, 1);
                                                    //  RIGHTUP
                                                }

                                                //  STOP BOT
                                                //  Stop Time-Out
                                                Timer7.Stop();
                                                Timer7.Enabled = false;
                                                TotalLines++;
                                                //  Enable
                                                if (checkBox11.Checked == true)
                                                {
                                                    Close();
                                                }

                                                //  Stop Timmer
                                                Timer5.Stop();
                                                Timer5.Enabled = false;
                                                GroupBox1.Enabled = false;
                                                GroupBox2.Enabled = false;
                                                GroupBox3.Enabled = false;
                                                GroupBox4.Enabled = false;
                                                GroupBox5.Enabled = false;
                                                checkBox9.Checked = false;
                                                checkBox10.Checked = false;
                                                checkBox11.Checked = false;
                                                Button4.Visible = false;
                                                Button4.Text = "Press To Stop Bot";
                                                Button5.Text = "Press To Start Bot";
                                                Button5.Visible = true;
                                                checkBox1.Checked = false;
                                                checkBox2.Checked = false;
                                                checkBox3.Checked = false;
                                                checkBox4.Checked = false;
                                                checkBox5.Checked = false;
                                                checkBox6.Checked = false;
                                                MessageBox.Show("Bot Has Finished!", "Minecraft Auto-Fishing Bot v" + VersionNumber, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }

                                        }
                                        else
                                        {
                                            //  Scroll To Next Rod
                                            //  Check To Eject First
                                            if (checkBox4.Checked == true)
                                            {
                                                //  Pull In Line Before Throw
                                                Thread.Sleep(100);
                                                MainForm.mouse_event(8, 0, 0, 0, 1);
                                                //  RIGHTDOWN
                                                Thread.Sleep(100);
                                                MainForm.mouse_event(16, 0, 0, 0, 1);
                                                //  RIGHTUP
                                                //  Press Middle Mouse Button
                                                Thread.Sleep(100);
                                                MainForm.mouse_event(32, 0, 0, 0, 1);
                                                //  MIDDLEDOWN
                                                Thread.Sleep(100);
                                                MainForm.mouse_event(64, 0, 0, 0, 1);
                                                //  MIDDLEUP
                                            }
                                            else
                                            {
                                                //  Pull In Line Before Throw
                                                Thread.Sleep(100);
                                                MainForm.mouse_event(8, 0, 0, 0, 1);
                                                //  RIGHTDOWN
                                                Thread.Sleep(100);
                                                MainForm.mouse_event(16, 0, 0, 0, 1);
                                                //  RIGHTUP
                                            }

                                            //  Scroll Hotbar
                                            this.MouseScroll(false, 1);
                                            // scrolls Down 10 wheel clicks
                                            RodHealth = 0;
                                            ScrollReject++;
                                            //  Run Macros
                                            this.RedirrectYes();
                                        }

                                    }
                                    else
                                    {
                                        //  Run Macros
                                        this.RedirrectYes();
                                    }

                                }
                                else
                                {
                                    //  Scrolling Not Enabled
                                    //  Normal (NOT SAVE 1 HP)
                                    //  Check If Max HP Was Found
                                    if (RodHealth == NumericUpDown1.Value)
                                    {
                                        //  MAX HP FOUND
                                        //  Eject Item If Needed
                                        if (checkBox4.Checked == true)
                                        {
                                            //  Pull In Line Before Throw
                                            Thread.Sleep(100);
                                            MainForm.mouse_event(8, 0, 0, 0, 1);
                                            //  RIGHTDOWN
                                            Thread.Sleep(100);
                                            MainForm.mouse_event(16, 0, 0, 0, 1);
                                            //  RIGHTUP
                                            //  Press Middle Mouse Button
                                            Thread.Sleep(100);
                                            MainForm.mouse_event(32, 0, 0, 0, 1);
                                            //  MIDDLEDOWN
                                            Thread.Sleep(100);
                                            MainForm.mouse_event(64, 0, 0, 0, 1);
                                            //  MIDDLEUP
                                        }
                                        else
                                        {
                                            //  Pull In Line Before Throw
                                            Thread.Sleep(100);
                                            MainForm.mouse_event(8, 0, 0, 0, 1);
                                            //  RIGHTDOWN
                                            Thread.Sleep(100);
                                            MainForm.mouse_event(16, 0, 0, 0, 1);
                                            //  RIGHTUP
                                        }

                                        //  Check To Close APP
                                        if (checkBox5.Checked == true)
                                        {
                                            //  Terminate the selected Minecraft process
                                            KillMinecraftProcesses();

                                            //  STOP BOT
                                            //  Stop Time-Out
                                            Timer7.Stop();
                                            Timer7.Enabled = false;
                                            TotalLines++;
                                            //  Enable
                                            if (checkBox11.Checked == true)
                                            {
                                                Close();
                                            }

                                            //  Stop Timmer
                                            Timer5.Stop();
                                            Timer5.Enabled = false;
                                            GroupBox1.Enabled = false;
                                            GroupBox2.Enabled = false;
                                            GroupBox3.Enabled = false;
                                            GroupBox4.Enabled = false;
                                            GroupBox5.Enabled = false;
                                            checkBox9.Checked = false;
                                            checkBox10.Checked = false;
                                            checkBox11.Checked = false;
                                            Button4.Visible = false;
                                            Button4.Text = "Press To Stop Bot";
                                            Button5.Text = "Press To Start Bot";
                                            Button5.Visible = true;
                                            checkBox1.Checked = false;
                                            checkBox2.Checked = false;
                                            checkBox3.Checked = false;
                                            checkBox4.Checked = false;
                                            checkBox5.Checked = false;
                                            checkBox6.Checked = false;
                                            MessageBox.Show("Bot Has Finished!", "Minecraft Auto-Fishing Bot v" + VersionNumber, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                        else
                                        {
                                            //  STOP BOT
                                            //  Stop Time-Out
                                            Timer7.Stop();
                                            Timer7.Enabled = false;
                                            TotalLines++;
                                            //  Enable
                                            if (checkBox11.Checked == true)
                                            {
                                                Close();
                                            }

                                            //  Stop Timmer
                                            Timer5.Stop();
                                            Timer5.Enabled = false;
                                            GroupBox1.Enabled = false;
                                            GroupBox2.Enabled = false;
                                            GroupBox3.Enabled = false;
                                            GroupBox4.Enabled = false;
                                            GroupBox5.Enabled = false;
                                            checkBox9.Checked = false;
                                            checkBox10.Checked = false;
                                            checkBox11.Checked = false;
                                            Button4.Visible = false;
                                            Button4.Text = "Press To Stop Bot";
                                            Button5.Text = "Press To Start Bot";
                                            Button5.Visible = true;
                                            checkBox1.Checked = false;
                                            checkBox2.Checked = false;
                                            checkBox3.Checked = false;
                                            checkBox4.Checked = false;
                                            checkBox5.Checked = false;
                                            checkBox6.Checked = false;
                                            MessageBox.Show("Bot Has Finished!", "Minecraft Auto-Fishing Bot v" + VersionNumber, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }

                                    }
                                    else
                                    {
                                        //  Run Macros
                                        this.RedirrectYes();
                                    }

                                }

                            }

                        }
                        else
                        {
                            //  Unlimited Throws Enabled
                            if (checkBox6.Checked == false)
                            {
                                //  Run Macros
                                this.RedirrectYes();
                            }
                            else
                            {
                                //  Stop Bot 
                                //  Pull In Line Before Throw
                                Thread.Sleep(100);
                                MainForm.mouse_event(8, 0, 0, 0, 1);
                                //  RIGHTDOWN
                                Thread.Sleep(100);
                                MainForm.mouse_event(16, 0, 0, 0, 1);
                                //  RIGHTUP
                                //  Check To Close APP
                                if (checkBox5.Checked == true)
                                {
                                    //  Terminate the selected Minecraft process
                                    KillMinecraftProcesses();

                                    //  STOP BOT
                                    //  Stop Time-Out
                                    Timer7.Stop();
                                    Timer7.Enabled = false;
                                    TotalLines++;
                                    //  Enable
                                    if (checkBox11.Checked == true)
                                    {
                                        Close();
                                    }

                                    //  Stop Timmer
                                    Timer5.Stop();
                                    Timer5.Enabled = false;
                                    GroupBox1.Enabled = false;
                                    GroupBox2.Enabled = false;
                                    GroupBox3.Enabled = false;
                                    GroupBox4.Enabled = false;
                                    GroupBox5.Enabled = false;
                                    checkBox9.Checked = false;
                                    checkBox10.Checked = false;
                                    checkBox11.Checked = false;
                                    Button4.Visible = false;
                                    Button4.Text = "Press To Stop Bot";
                                    Button5.Text = "Press To Start Bot";
                                    Button5.Visible = true;
                                    checkBox1.Checked = false;
                                    checkBox2.Checked = false;
                                    checkBox3.Checked = false;
                                    checkBox4.Checked = false;
                                    checkBox5.Checked = false;
                                    checkBox6.Checked = false;
                                    MessageBox.Show("Bot Has Finished!", "Minecraft Auto-Fishing Bot v" + VersionNumber, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    //  STOP BOT
                                    //  Stop Time-Out
                                    Timer7.Stop();
                                    Timer7.Enabled = false;
                                    TotalLines++;
                                    //  Enable
                                    if (checkBox11.Checked == true)
                                    {
                                        Close();
                                    }

                                    //  Stop Timmer
                                    Timer5.Stop();
                                    Timer5.Enabled = false;
                                    GroupBox1.Enabled = false;
                                    GroupBox2.Enabled = false;
                                    GroupBox3.Enabled = false;
                                    GroupBox4.Enabled = false;
                                    GroupBox5.Enabled = false;
                                    checkBox9.Checked = false;
                                    checkBox10.Checked = false;
                                    checkBox11.Checked = false;
                                    Button4.Visible = false;
                                    Button4.Text = "Press To Stop Bot";
                                    Button5.Text = "Press To Start Bot";
                                    Button5.Visible = true;
                                    checkBox1.Checked = false;
                                    checkBox2.Checked = false;
                                    checkBox3.Checked = false;
                                    checkBox4.Checked = false;
                                    checkBox5.Checked = false;
                                    checkBox6.Checked = false;
                                    MessageBox.Show("Bot Has Finished!", "Minecraft Auto-Fishing Bot v" + VersionNumber, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }

                            }

                        }

                        break;
                    }

                }
            }

            //  Look For KillSwitch
            if (checkBox6.Checked == true)
            {
                //  Stop Time-Out
                Timer7.Stop();
                Timer7.Enabled = false;
                Timer1.Stop();
                Timer1.Enabled = false;
                Timer5.Stop();
                Timer5.Enabled = false;
                GroupBox1.Enabled = true;
                GroupBox2.Enabled = true;
                GroupBox3.Enabled = true;
                GroupBox4.Enabled = true;
                GroupBox5.Enabled = true;
                checkBox9.Checked = false;
                checkBox10.Checked = false;
                checkBox11.Checked = false;
                Button4.Visible = false;
                Button4.Text = "Press To Stop Bot";
                Button5.Text = "Press To Start Bot";
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                DummyScroll = 0;
                RodHealth = 3;
                checkBox6.Checked = false;
                Button5.Visible = true;
            }

            //  Look for the selected Minecraft process.
            if (checkBox8.Checked == false)
            {
                //  Stop Time-Out
                Timer7.Stop();
                Timer7.Enabled = false;
                Timer1.Stop();
                Timer1.Enabled = false;
                Timer5.Stop();
                Timer5.Enabled = false;
                GroupBox1.Enabled = false;
                GroupBox2.Enabled = false;
                GroupBox3.Enabled = false;
                GroupBox4.Enabled = false;
                GroupBox5.Enabled = false;
                checkBox9.Checked = false;
                checkBox10.Checked = false;
                checkBox11.Checked = false;
                Button4.Visible = false;
                Button4.Text = "Press To Stop Bot";
                Button5.Text = "Press To Start Bot";
                Button5.Visible = true;
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                checkBox6.Checked = false;
                MessageBox.Show("The selected Minecraft process was stopped!", "Minecraft Auto-Fishing Bot v" + VersionNumber, MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }
        #endregion

        #region Fishing Actions

        /// <summary>
        /// Runs the cast/reel macro after the bot detects a fish bite.
        /// </summary>
        private void RedirrectYes()
        {
            //  Stop Time-Out
            Timer7.Stop();
            Timer7.Enabled = false;
            TotalLines++;
            RodHealth++;
            //  Do Macro

            // Do Swing Offset
            // Get XY
            int OldPosX = Cursor.Position.X;
            int OldPosY = Cursor.Position.Y;
            if (checkBox12.Checked)
            {
                // Move Point
                Cursor.Position = new Point(OldPosX, OldPosY + (int)numericUpDown7.Value);

                //  Wait Line-In Seconds
                Thread.Sleep(100);
            }

            //  Do Not Do if No Pole In INV
            if (DummyScroll == 0)
            {
                int Num3 = Convert.ToInt32(NumericUpDown3.Value);
                Thread.Sleep(Num3);
                //  Wait Line-In Seconds
                Thread.Sleep(100);
                MainForm.mouse_event(8, 0, 0, 0, 1);
                //  RIGHTDOWN
                Thread.Sleep(100);
                MainForm.mouse_event(16, 0, 0, 0, 1);
                //  RIGHTUP
            }
            else
            {
                //  Reset DummyScroll
                DummyScroll = 0;
            }

            int Num4 = Convert.ToInt32(NumericUpDown4.Value);
            Thread.Sleep(Num4);
            //  Normalize Swing Offset 
            if (checkBox12.Checked)
            {
                Thread.Sleep(100);

                // Move Point
                Cursor.Position = new Point(OldPosX, OldPosY - (int)numericUpDown7.Value);

                //  Wait Line-In Seconds
                Thread.Sleep(100);
            }
            //  Wait Line-Out Seconds
            Thread.Sleep(100);
            MainForm.mouse_event(8, 0, 0, 0, 1);
            //  RIGHTDOWN
            Thread.Sleep(100);
            MainForm.mouse_event(16, 0, 0, 0, 1);
            //  RIGHTUP
            int Num5 = Convert.ToInt32(NumericUpDown5.Value);
            Thread.Sleep(Num5);
            //  Wait Kill-Noise Seconds
            //  Start Time-Out
            Timer7.Enabled = true;
            Timer7.Start();
            //  Re-Enable Search
            Timer1.Enabled = true;
            Timer1.Start();
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
            Tmr.Start();
            //  Tool Tips
            ToolTip toolTip1 = new ToolTip
            {
                ShowAlways = true
            };
            toolTip1.SetToolTip(Button4, "Stops the current bot session.");
            toolTip1.SetToolTip(Button5, "Start the bot.");
            toolTip1.SetToolTip(checkBox1, "Choose an custom amount of throws - Default 64.");
            toolTip1.SetToolTip(checkBox2, "Allows to scroll to next rod when broken.");
            toolTip1.SetToolTip(checkBox3, "Save the current rod with 1 HP remaining.");
            toolTip1.SetToolTip(checkBox4, "Auto tosses rod. Must bind toss item as middle-mouse.");
            toolTip1.SetToolTip(checkBox5, "Auto leaves world/server by closing minecraft.");
            toolTip1.SetToolTip(checkBox9, "Unlimited casts of current rod. Great for creative.");
            toolTip1.SetToolTip(checkBox11, "Close the bot when its session is over.");
            toolTip1.SetToolTip(checkBox10, "Attempts to override AFK. Must bind shift as left-mouse.");
            toolTip1.SetToolTip(NumericUpDown1, "Select a custom number.");
            toolTip1.SetToolTip(NumericUpDown2, "Set a custom delay before running bot.");
            toolTip1.SetToolTip(NumericUpDown3, "Set a custom delay before pulling in line.");
            toolTip1.SetToolTip(NumericUpDown4, "Set a custom delay before casting new line.");
            toolTip1.SetToolTip(NumericUpDown5, "Set a custom delay before continuing loop.");
            toolTip1.SetToolTip(numericUpDown6, "Set the level of sound needed to trigger the bot.");
            toolTip1.SetToolTip(checkBox12, "When catching, look down to toss. Minecraft v1.13+");
            toolTip1.SetToolTip(numericUpDown7, "Distance to look down from toss spot.");
        }
        #endregion

        #region Status / Process Polling

        /// <summary>
        /// Refreshes counters, process status, version display, and live UI state.
        /// </summary>
        [Obsolete]
        private void Timer2_Tick(object sender, EventArgs e)
        {
            //  Handles For Lables
            string TL = TotalLines.ToString();
            Label6.Text = TL;

            string RH = RodHealth.ToString();
            Label14.Text = RH;

            string SR = ScrollReject.ToString();
            int SR1 = Int32.Parse(SR) + 1;
            string SR2 = SR1.ToString();
            Label17.Text = SR2;

            //  X, Y coords bottom toolbar
            MyX_MyY.Text = " X:"
                        + Cursor.Position.X + (", Y:" + Cursor.Position.Y);
            //  Count matching Minecraft processes for the selected version.
            int count = GetMinecraftProcessCount();

            string JCount = count.ToString();
            Label3.Text = JCount;
            //  Check for the selected Minecraft process
            if (IsMinecraftRunning())
            {

                ToolStripStatusLabel1.ForeColor = Color.DarkGreen;
                ToolStripStatusLabel1.Text = "Found";
                checkBox8.Checked = true;
                if (CheckGame == 0)
                {
                    Button5.Enabled = true;
                    Button5.Text = "Press To Start Bot";
                    CheckGame = 1;

                    // Minecraft found: enable client controls.

                    GroupBox1.Enabled = true;
                    GroupBox2.Enabled = true;
                    GroupBox3.Enabled = true;
                    GroupBox4.Enabled = true;
                    GroupBox5.Enabled = true;

                }

            }
            else
            {

                ToolStripStatusLabel1.ForeColor = Color.DarkRed;
                ToolStripStatusLabel1.Text = "Not Found";
                Button5.Text = "Looking For Game...";
                CheckGame = 0;
                checkBox8.Checked = false;
                GroupBox1.Enabled = false;
                GroupBox2.Enabled = false;
                GroupBox3.Enabled = false;
                GroupBox4.Enabled = false;
                GroupBox5.Enabled = false;

            }

            //  Get Memory Usage For Current Application
            Process x = Process.GetCurrentProcess();
            string inf;
            inf = "Mem Usage: "
                        + (x.WorkingSet / 1024) + (" K" + "\r\n" + "Paged Memory: "
                        + (x.PagedMemorySize / 1024)
                        + " K");
            Label7.Text = inf;

            //  Get selected Minecraft process version.
            if (TryGetFirstMinecraftProcess(out Process minecraftProcess))
            {
                try
                {
                    string minecraftPath = minecraftProcess.MainModule.FileName;
                    if (System.IO.File.Exists(minecraftPath))
                    {
                        var fileVersion = FileVersionInfo.GetVersionInfo(minecraftPath);
                        Label9.Text = fileVersion.FileVersion;
                    }
                    else
                    {
                        //  Path Does Not Exist
                        Label9.Text = "1.0.0.0";
                    }
                }
                catch
                {
                    // Some processes may deny MainModule access depending on permissions/bitness.
                    Label9.Text = "N/A";
                }
            }
            else
            {
                //  Path Does Not Exist
                Label9.Text = "N/A";
            }

            //  For Eject Rod
            if (NumericUpDown1.Value == 64)
            {
                checkBox4.Enabled = false;
                checkBox4.Checked = false;
            }
            else
            {
                checkBox4.Enabled = true;
            }

        }
        #endregion

        #region Bot Controls

        /// <summary>
        /// Starts a bot session after the selected Minecraft version has been detected.
        /// </summary>
        private void Button5_Click(object sender, EventArgs e)
        {

            if (checkBox8.Checked)
            {

                // Start Bot

                //  
                Button5.Text = "Starting Bot...";
                GroupBox1.Enabled = false;
                GroupBox2.Enabled = false;
                GroupBox3.Enabled = false;
                GroupBox4.Enabled = false;
                GroupBox5.Enabled = false;
                Button4.Enabled = true;
                Button4.Visible = true;
                Button5.Visible = false;
                RodHealth = 1;
                ScrollReject = 0;
                TotalLines = 0;
                Label6.Text = "0";
                int Num2 = Convert.ToInt32(NumericUpDown2.Value);
                System.Threading.Thread.Sleep(Num2);
                //  Wait Start-Delay Seconds
                //  Throw Line
                System.Threading.Thread.Sleep(100);
                mouse_event(8, 0, 0, 0, 1);
                //  RIGHTDOWN
                System.Threading.Thread.Sleep(100);
                mouse_event(16, 0, 0, 0, 1);
                //  RIGHTUP
                int Num5 = Convert.ToInt32(NumericUpDown5.Value);
                System.Threading.Thread.Sleep(Num5);
                //  Wait Noise Reduction Time
                //  Start Anti-AFK
                if (checkBox10.Checked == true)
                {
                    Timer5.Enabled = true;
                    Timer5.Start();
                }

                //  Start Time-Out
                Timer7.Enabled = true;
                Timer7.Start();
                //  Start Bot
                Timer1.Enabled = true;
                Timer1.Start();

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
            if (checkBox1.Checked == true)
            {
                NumericUpDown1.Enabled = true;
                checkBox3.Enabled = false;
                checkBox3.Checked = false;
            }
            else
            {
                checkBox3.Enabled = true;
                NumericUpDown1.Enabled = false;
                NumericUpDown1.Value = 64;
            }

        }

        /// <summary>
        /// Toggles Save 1 HP behavior and updates incompatible options.
        /// </summary>
        private void CheckBox3_CheckedChanged(object sender, EventArgs e)
        {

            // Enable/Disable Eject Rod
            if (checkBox3.Checked == true)
            {
                checkBox4.Enabled = false;
                checkBox1.Enabled = false;
                checkBox4.Checked = false;
            }
            else
            {
                checkBox4.Enabled = true;
                checkBox1.Enabled = true;
            }

        }

        /// <summary>
        /// Toggles the rod-eject option and updates incompatible options.
        /// </summary>
        private void CheckBox4_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBox1.Checked == true)
            {

            }
            else
            {
                //  Enable/Disable Eject Rod
                if (checkBox4.Checked == true)
                {
                    checkBox3.Enabled = false;
                    checkBox3.Checked = false;
                }
                else
                {
                    checkBox3.Enabled = true;
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
            Button4.Enabled = false;
            checkBox6.Checked = true;
            Button4.Text = "Stoping Bot...";

        }

        //  Unlimited Throws
        /// <summary>
        /// Toggles unlimited cast mode and disables incompatible rod-count options.
        /// </summary>
        private void CheckBox9_CheckedChanged(object sender, EventArgs e)
        {

            //  Enable/Disable Unlimited Throws
            if (checkBox9.Checked == true)
            {
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                checkBox11.Checked = false;
                checkBox1.Enabled = false;
                checkBox2.Enabled = false;
                checkBox3.Enabled = false;
                checkBox4.Enabled = false;
                checkBox5.Enabled = false;
                checkBox11.Enabled = false;
            }
            else
            {
                checkBox1.Enabled = true;
                checkBox2.Enabled = true;
                checkBox3.Enabled = true;
                checkBox4.Enabled = true;
                checkBox5.Enabled = true;
                checkBox11.Enabled = true;
            }

        }

        /// <summary>
        /// Runs the optional anti-AFK click action.
        /// </summary>
        private void Timer5_Tick(object sender, EventArgs e)
        {

            //  Left Down
            System.Threading.Thread.Sleep(100);
            mouse_event(2, 0, 0, 0, 1);
            //  RIGHTDOWN
            System.Threading.Thread.Sleep(100);
            mouse_event(4, 0, 0, 0, 1);
            //  RIGHTUP

        }

        /// <summary>
        /// Handles bot timeout/no-rod detection and resets the session if needed.
        /// </summary>
        private void Timer7_Tick(object sender, EventArgs e)
        {

            //  Disable Timmer
            Timer1.Stop();
            Timer1.Enabled = false;
            Timer7.Stop();
            Timer7.Enabled = false;
            if (checkBox2.Checked == true)
            {
                //  Check If Its Already Last Throw
                if (ScrollReject == 8)
                {
                    //  It's GameOver End Bot
                    //  STOP BOT
                    //  Stop Time-Out
                    Timer7.Stop();
                    Timer7.Enabled = false;
                    TotalLines++;
                    //  Enable
                    if (checkBox11.Checked == true)
                    {
                        Close();
                    }

                    //  Stop Time-Out
                    Timer7.Stop();
                    Timer7.Enabled = false;
                    Timer1.Stop();
                    Timer1.Enabled = false;
                    Timer5.Stop();
                    Timer5.Enabled = false;
                    GroupBox1.Enabled = false;
                    GroupBox2.Enabled = false;
                    GroupBox3.Enabled = false;
                    GroupBox4.Enabled = false;
                    GroupBox5.Enabled = false;
                    checkBox9.Checked = false;
                    checkBox10.Checked = false;
                    checkBox11.Checked = false;
                    Button4.Visible = false;
                    Button4.Text = "Press To Stop Bot";
                    Button5.Text = "Press To Start Bot";
                    Button5.Visible = true;
                    checkBox1.Checked = false;
                    checkBox2.Checked = false;
                    checkBox3.Checked = false;
                    checkBox4.Checked = false;
                    checkBox5.Checked = false;
                    checkBox6.Checked = false;
                    MessageBox.Show("No Rod Was Detected! 9th Rod Finished", "Minecraft Auto-Fishing Bot v" + VersionNumber, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    //  Still Good
                    //  Re-Enable Search
                    Timer1.Enabled = false;
                    Timer1.Start();
                    //  Scroll Hotbar
                    MouseScroll(false, 1);
                    // scrolls Down 10 wheel clicks
                    //  Cast Rod
                    int Num4 = Convert.ToInt32(NumericUpDown4.Value);
                    System.Threading.Thread.Sleep(Num4);
                    //  Wait Line-Out Seconds
                    System.Threading.Thread.Sleep(100);
                    mouse_event(8, 0, 0, 0, 1);
                    //  RIGHTDOWN
                    System.Threading.Thread.Sleep(100);
                    mouse_event(16, 0, 0, 0, 1);
                    //  RIGHTUP
                    int Num5 = Convert.ToInt32(NumericUpDown5.Value);
                    System.Threading.Thread.Sleep(Num5);
                    //  Wait Kill-Noise Seconds
                    RodHealth = 0;
                    ScrollReject++;
                    //  Start Time-Out
                    Timer7.Enabled = true;
                    Timer7.Start();
                    //  Re-Enable Search
                    Timer1.Enabled = true;
                    Timer1.Start();
                }

            }
            else
            {
                //  Scroll Is Disabled
                //  STOP BOT
                //  Stop Time-Out
                Timer7.Stop();
                Timer7.Enabled = false;
                Timer1.Stop();
                Timer1.Enabled = false;
                Timer5.Stop();
                Timer5.Enabled = false;
                GroupBox1.Enabled = false;
                GroupBox2.Enabled = false;
                GroupBox3.Enabled = false;
                GroupBox4.Enabled = false;
                GroupBox5.Enabled = false;
                checkBox9.Checked = false;
                checkBox10.Checked = false;
                checkBox11.Checked = false;
                Button4.Visible = false;
                Button4.Text = "Press To Stop Bot";
                Button5.Text = "Press To Start Bot";
                Button5.Visible = true;
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                checkBox6.Checked = false;
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

                toolStripStatusLabel6.Text = "- [Working]";

                // Disable Timer
                timer9.Enabled = true;

                // Action Timer
                timer10.Enabled = true;

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
        private void Timer9_Tick(object sender, EventArgs e)
        {

            timer9.Enabled = false;
            timer10.Enabled = false;
            toolStripStatusLabel6.Text = "N/A";
            toolStripStatusLabel6.Text = "- [Capture]";

        }

        /// <summary>
        /// Displays the current VU level for the selected Minecraft audio session.
        /// </summary>
        private void Timer10_Tick(object sender, EventArgs e)
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

                        string VPInput = numericUpDown6.Text;
                        string s1 = firstTwoChars;
                        int s1int = Int32.Parse(s1);
                        int s2int = Int32.Parse(VPInput);

                        toolStripStatusLabel5.Text = s1;

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
            if (checkBox12.Checked)
            {
                numericUpDown7.Enabled = true;
            }
            else
            {
                numericUpDown7.Enabled = false;
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