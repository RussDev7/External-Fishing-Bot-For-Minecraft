using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;
using System.IO;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        // Say Hello To Decompilers
        private static readonly string HelloThere = "Hello There Fellow Decompiler, This Program Was Made By dannyruss (xXCrypticNightXx).";

        private string VersionNumber = "2.41";

        private string JavaName = "javaw";

        private int RodHealth = 1;

        private int SaveHP = 63;

        private int TotalLines = 0;

        private int ScrollReject = 0;

        private int CheckJava = 0;

        private int DummyScroll = 0;

        private const int MOUSEEVENTF_WHEEL = 2048;

        // The amount of movement is specified in mouseData.
        private const int MOUSEEVENTF_HWHEEL = 4096;

        // Not available for 2000 or XP
        private void MouseScroll(bool up, int clicks)
        {
            if (up)
            {
                Form1.mouse_event(MOUSEEVENTF_WHEEL, 0, 0, (clicks * 120), 0);
            }
            else
            {
                Form1.mouse_event(MOUSEEVENTF_WHEEL, 0, 0, ((clicks * 120) * -1), 0);
            }

        }

        public delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);

        private int currentProcessID;

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

        private const int VK_ESCAPE = 27;

        private const int KEYEVENTF_EXTENDEDKEY = 1;

        private const int KEYEVENTF_KEYDOWN = 0;

        private const int KEYEVENTF_KEYUP = 2;

        private const int MOUSEEVENTF_LEFTDOWN = 2;

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

        public Form1()
        {
            InitializeComponent();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {

            // Here, I'm just monitoring chrome, one of the possible sessions.
            // If you want the whole speakers peak values, use the AudioSession.GetSpeakersChannelsPeakValues() method
            foreach (AudioSession session in AudioSession.EnumerateAll())
            {
                if (session.Process?.ProcessName == JavaName)
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
                                                Form1.mouse_event(8, 0, 0, 0, 1);
                                                //  RIGHTDOWN
                                                Thread.Sleep(100);
                                                Form1.mouse_event(16, 0, 0, 0, 1);
                                                //  RIGHTUP
                                                //  Terminate javaw
                                                foreach (System.Diagnostics.Process myProc in System.Diagnostics.Process.GetProcesses())
                                                {
                                                    if (myProc.ProcessName == JavaName)
                                                    {
                                                        myProc.Kill();
                                                    }
                                                }

                                                //  STOP BOT
                                                //  Stop Time-Out
                                                Timer7.Stop();
                                                Timer7.Enabled = false;
                                                TotalLines = (TotalLines + 1);
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
                                                Form1.mouse_event(8, 0, 0, 0, 1);
                                                //  RIGHTDOWN
                                                Thread.Sleep(100);
                                                Form1.mouse_event(16, 0, 0, 0, 1);
                                                //  RIGHTUP
                                                //  STOP BOT
                                                //  Stop Time-Out
                                                Timer7.Stop();
                                                Timer7.Enabled = false;
                                                TotalLines = (TotalLines + 1);
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
                                            Form1.mouse_event(8, 0, 0, 0, 1);
                                            //  RIGHTDOWN
                                            Thread.Sleep(100);
                                            Form1.mouse_event(16, 0, 0, 0, 1);
                                            //  RIGHTUP
                                            //  Scroll Hotbar
                                            this.MouseScroll(false, 1);
                                            // scrolls Down 10 wheel clicks
                                            RodHealth = 0;
                                            ScrollReject = (ScrollReject + 1);
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
                                            Form1.mouse_event(8, 0, 0, 0, 1);
                                            //  RIGHTDOWN
                                            Thread.Sleep(100);
                                            Form1.mouse_event(16, 0, 0, 0, 1);
                                            //  RIGHTUP
                                            //  Terminate javaw
                                            foreach (System.Diagnostics.Process myProc in System.Diagnostics.Process.GetProcesses())
                                            {
                                                if (myProc.ProcessName == JavaName)
                                                {
                                                    myProc.Kill();
                                                }
                                            }

                                            //  STOP BOT
                                            //  Stop Time-Out
                                            Timer7.Stop();
                                            Timer7.Enabled = false;
                                            TotalLines = (TotalLines + 1);
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
                                            Form1.mouse_event(8, 0, 0, 0, 1);
                                            //  RIGHTDOWN
                                            Thread.Sleep(100);
                                            Form1.mouse_event(16, 0, 0, 0, 1);
                                            //  RIGHTUP
                                            //  STOP BOT
                                            //  Stop Time-Out
                                            Timer7.Stop();
                                            Timer7.Enabled = false;
                                            TotalLines = (TotalLines + 1);
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
                                                    Form1.mouse_event(8, 0, 0, 0, 1);
                                                    //  RIGHTDOWN
                                                    Thread.Sleep(100);
                                                    Form1.mouse_event(16, 0, 0, 0, 1);
                                                    //  RIGHTUP
                                                    //  Press Middle Mouse Button
                                                    Thread.Sleep(100);
                                                    Form1.mouse_event(32, 0, 0, 0, 1);
                                                    //  MIDDLEDOWN
                                                    Thread.Sleep(100);
                                                    Form1.mouse_event(64, 0, 0, 0, 1);
                                                    //  MIDDLEUP
                                                }
                                                else
                                                {
                                                    //  Pull In Line Before Throw
                                                    Thread.Sleep(100);
                                                    Form1.mouse_event(8, 0, 0, 0, 1);
                                                    //  RIGHTDOWN
                                                    Thread.Sleep(100);
                                                    Form1.mouse_event(16, 0, 0, 0, 1);
                                                    //  RIGHTUP
                                                }

                                                //  Terminate javaw
                                                foreach (System.Diagnostics.Process myProc in System.Diagnostics.Process.GetProcesses())
                                                {
                                                    if (myProc.ProcessName == JavaName)
                                                    {
                                                        myProc.Kill();
                                                    }
                                                }

                                                //  STOP BOT
                                                //  Stop Time-Out
                                                Timer7.Stop();
                                                Timer7.Enabled = false;
                                                TotalLines = (TotalLines + 1);
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
                                                    Form1.mouse_event(8, 0, 0, 0, 1);
                                                    //  RIGHTDOWN
                                                    Thread.Sleep(100);
                                                    Form1.mouse_event(16, 0, 0, 0, 1);
                                                    //  RIGHTUP
                                                    //  Press Middle Mouse Button
                                                    Thread.Sleep(100);
                                                    Form1.mouse_event(32, 0, 0, 0, 1);
                                                    //  MIDDLEDOWN
                                                    Thread.Sleep(100);
                                                    Form1.mouse_event(64, 0, 0, 0, 1);
                                                    //  MIDDLEUP
                                                }
                                                else
                                                {
                                                    //  Pull In Line Before Throw
                                                    Thread.Sleep(100);
                                                    Form1.mouse_event(8, 0, 0, 0, 1);
                                                    //  RIGHTDOWN
                                                    Thread.Sleep(100);
                                                    Form1.mouse_event(16, 0, 0, 0, 1);
                                                    //  RIGHTUP
                                                }

                                                //  STOP BOT
                                                //  Stop Time-Out
                                                Timer7.Stop();
                                                Timer7.Enabled = false;
                                                TotalLines = (TotalLines + 1);
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
                                                Form1.mouse_event(8, 0, 0, 0, 1);
                                                //  RIGHTDOWN
                                                Thread.Sleep(100);
                                                Form1.mouse_event(16, 0, 0, 0, 1);
                                                //  RIGHTUP
                                                //  Press Middle Mouse Button
                                                Thread.Sleep(100);
                                                Form1.mouse_event(32, 0, 0, 0, 1);
                                                //  MIDDLEDOWN
                                                Thread.Sleep(100);
                                                Form1.mouse_event(64, 0, 0, 0, 1);
                                                //  MIDDLEUP
                                            }
                                            else
                                            {
                                                //  Pull In Line Before Throw
                                                Thread.Sleep(100);
                                                Form1.mouse_event(8, 0, 0, 0, 1);
                                                //  RIGHTDOWN
                                                Thread.Sleep(100);
                                                Form1.mouse_event(16, 0, 0, 0, 1);
                                                //  RIGHTUP
                                            }

                                            //  Scroll Hotbar
                                            this.MouseScroll(false, 1);
                                            // scrolls Down 10 wheel clicks
                                            RodHealth = 0;
                                            ScrollReject = (ScrollReject + 1);
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
                                            Form1.mouse_event(8, 0, 0, 0, 1);
                                            //  RIGHTDOWN
                                            Thread.Sleep(100);
                                            Form1.mouse_event(16, 0, 0, 0, 1);
                                            //  RIGHTUP
                                            //  Press Middle Mouse Button
                                            Thread.Sleep(100);
                                            Form1.mouse_event(32, 0, 0, 0, 1);
                                            //  MIDDLEDOWN
                                            Thread.Sleep(100);
                                            Form1.mouse_event(64, 0, 0, 0, 1);
                                            //  MIDDLEUP
                                        }
                                        else
                                        {
                                            //  Pull In Line Before Throw
                                            Thread.Sleep(100);
                                            Form1.mouse_event(8, 0, 0, 0, 1);
                                            //  RIGHTDOWN
                                            Thread.Sleep(100);
                                            Form1.mouse_event(16, 0, 0, 0, 1);
                                            //  RIGHTUP
                                        }

                                        //  Check To Close APP
                                        if (checkBox5.Checked == true)
                                        {
                                            //  Terminate javaw
                                            foreach (System.Diagnostics.Process myProc in System.Diagnostics.Process.GetProcesses())
                                            {
                                                if (myProc.ProcessName == JavaName)
                                                {
                                                    myProc.Kill();
                                                }
                                            }

                                            //  STOP BOT
                                            //  Stop Time-Out
                                            Timer7.Stop();
                                            Timer7.Enabled = false;
                                            TotalLines = (TotalLines + 1);
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
                                            TotalLines = (TotalLines + 1);
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
                                Form1.mouse_event(8, 0, 0, 0, 1);
                                //  RIGHTDOWN
                                Thread.Sleep(100);
                                Form1.mouse_event(16, 0, 0, 0, 1);
                                //  RIGHTUP
                                //  Check To Close APP
                                if (checkBox5.Checked == true)
                                {
                                    //  Terminate javaw
                                    foreach (System.Diagnostics.Process myProc in System.Diagnostics.Process.GetProcesses())
                                    {
                                        if (myProc.ProcessName == JavaName)
                                        {
                                            myProc.Kill();
                                        }
                                    }

                                    //  STOP BOT
                                    //  Stop Time-Out
                                    Timer7.Stop();
                                    Timer7.Enabled = false;
                                    TotalLines = (TotalLines + 1);
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
                                    TotalLines = (TotalLines + 1);
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

            //  Look For javaw.exe
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
                MessageBox.Show("The java Process Was Stoped!", "Minecraft Auto-Fishing Bot v" + VersionNumber, MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        //  Found Fish
        private void RedirrectYes()
        {
            //  Stop Time-Out
            Timer7.Stop();
            Timer7.Enabled = false;
            TotalLines = (TotalLines + 1);
            RodHealth = (RodHealth + 1);
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
                Form1.mouse_event(8, 0, 0, 0, 1);
                //  RIGHTDOWN
                Thread.Sleep(100);
                Form1.mouse_event(16, 0, 0, 0, 1);
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
            Form1.mouse_event(8, 0, 0, 0, 1);
            //  RIGHTDOWN
            Thread.Sleep(100);
            Form1.mouse_event(16, 0, 0, 0, 1);
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

        //  Form Load Commands
        private void Form1_Load(object sender, EventArgs e)
        {
            // Adjust Timer1
            // Timer1.Interval = 250;

            this.KeyPreview = true;
            Tmr.Start();
            //  Tool Tips
            ToolTip toolTip1 = new ToolTip();
            toolTip1.ShowAlways = true;
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

        //  1  Interval Clock
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
            MyX_MyY.Text = (" X:"
                        + (Cursor.Position.X + (", Y:" + Cursor.Position.Y)));
            //  Count Java Processes
            int count = 0;
            foreach (System.Diagnostics.Process myProc in System.Diagnostics.Process.GetProcesses())
            {
                if (myProc.ProcessName == JavaName)
                {
                    count = count + 1;
                }
            }

            string JCount = count.ToString();
            Label3.Text = JCount;
            //  Check For Java Process
            if (Process.GetProcessesByName(JavaName).Length > 0)
            {

                ToolStripStatusLabel1.ForeColor = Color.DarkGreen;
                ToolStripStatusLabel1.Text = "Found";
                checkBox8.Checked = true;
                if (CheckJava == 0)
                {
                    Button5.Enabled = true;
                    Button5.Text = "Press To Start Bot";
                    CheckJava = 1;

                    // JavaFound Enable Client

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
                CheckJava = 0;
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
            inf = ("Mem Usage: "
                        + ((x.WorkingSet / 1024) + (" K" + ("\r\n" + ("Paged Memory: "
                        + ((x.PagedMemorySize / 1024)
                        + " K"))))));
            Label7.Text = inf;

            //  Get Java Version
            if (Process.GetProcessesByName(JavaName).Length > 0)
            {
                Process javaProc = Process.GetProcessesByName(JavaName).First();
                string javaPath = javaProc.MainModule.FileName;
                if (System.IO.File.Exists(javaPath))
                {
                    // Specify the directory you want to manipulate.
                    var JreVersion = FileVersionInfo.GetVersionInfo(javaPath);
                    Label9.Text = JreVersion.FileVersion;
                }
                else
                {
                    //  Path Does Not Exist
                    Label9.Text = "1.0.0.0";
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

        //  Start Bot
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

        //  Form Closing
        private void Form1_Closing(object sender, EventArgs e)
        {
            //  Terminate SndVol

        }

        //  Custom Throws
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

        //  Save 1 HP
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

        //  Auto Eject Rod
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

        //  Stop Bot
        private void Button4_Click(object sender, EventArgs e)
        {

            //  Send Stop State
            Button4.Enabled = false;
            checkBox6.Checked = true;
            Button4.Text = "Stoping Bot...";

        }

        //  Unlimited Throws
        private void checkBox9_CheckedChanged(object sender, EventArgs e)
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

        //  ANTI-AFK
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

        //  Fishing Time-Out
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
                    TotalLines = (TotalLines + 1);
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
                    ScrollReject = (ScrollReject + 1);
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

        // Get Sound Level For 1 Minuite
        private void toolStripStatusLabel6_Click(object sender, EventArgs e)
        {

            //  Check For Java Process
            if (Process.GetProcessesByName(JavaName).Length > 0)
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

        // Disable Timmer
        private void timer9_Tick(object sender, EventArgs e)
        {

            timer9.Enabled = false;
            timer10.Enabled = false;
            toolStripStatusLabel6.Text = "N/A";
            toolStripStatusLabel6.Text = "- [Capture]";

        }

        // Find Sound
        private void timer10_Tick(object sender, EventArgs e)
        {
            //  Check For Java Process
            if (Process.GetProcessesByName(JavaName).Length > 0)
            {
                // Capture Sound
                foreach (AudioSession session in AudioSession.EnumerateAll())
                {
                    if (session.Process?.ProcessName == JavaName)
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

        // Adjust Swing Offset
        private void checkBox12_CheckedChanged(object sender, EventArgs e)
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

        // Adjust the dropdown option.
        private void toolStripDropDownButton1_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "Javaw")
            {
                JavaName = "javaw";
                toolStripDropDownButton1.Text = "JavaName: \"Javaw\"";
            }
            else if (e.ClickedItem.Text == "Java")
            {
                JavaName = "java";
                toolStripDropDownButton1.Text = "JavaName: \"Java\"";
            }
        }
    }

}