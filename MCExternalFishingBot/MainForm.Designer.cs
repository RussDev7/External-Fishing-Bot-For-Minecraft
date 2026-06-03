namespace MCExternalFishingBot
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ServerOptions_GroupBox = new System.Windows.Forms.GroupBox();
            this.AntiAFKProtection_CheckBox = new System.Windows.Forms.CheckBox();
            this.CustomWaitValues_GroupBox = new System.Windows.Forms.GroupBox();
            this.NoiseDelayValue_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.NoiseDelay_Label = new System.Windows.Forms.Label();
            this.LineOutDelayValue_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.LineOutDelay_Label = new System.Windows.Forms.Label();
            this.LineInDelayValue_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.LineInDelay_Label = new System.Windows.Forms.Label();
            this.StartBotDelay_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.StartBotDelay_Label = new System.Windows.Forms.Label();
            this.PressToStopBot_Button = new System.Windows.Forms.Button();
            this.PressToStartBot_Button = new System.Windows.Forms.Button();
            this.FishingOptions_GroupBox = new System.Windows.Forms.GroupBox();
            this.SwingOffsetValue_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.SwingOffset_CheckBox = new System.Windows.Forms.CheckBox();
            this.UnlimitedThrows_CheckBox = new System.Windows.Forms.CheckBox();
            this.CloseBotWhenFinished_CheckBox = new System.Windows.Forms.CheckBox();
            this.CustomThrowsValue_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.LeaveWhenFinished_CheckBox = new System.Windows.Forms.CheckBox();
            this.AutoEjectRod_CheckBox = new System.Windows.Forms.CheckBox();
            this.Save1HP_CheckBox = new System.Windows.Forms.CheckBox();
            this.UseHotbarScrolling_CheckBox = new System.Windows.Forms.CheckBox();
            this.CustomThrows_CheckBox = new System.Windows.Forms.CheckBox();
            this.SoundVUSensitivity_GroupBox = new System.Windows.Forms.GroupBox();
            this.TriggerBotAt_Label = new System.Windows.Forms.Label();
            this.TriggerBotAtValue_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.BotStatus_GroupBox = new System.Windows.Forms.GroupBox();
            this.Debug_CheckBox1 = new System.Windows.Forms.CheckBox();
            this.Debug_CheckBox3 = new System.Windows.Forms.CheckBox();
            this.Debug_CheckBox2 = new System.Windows.Forms.CheckBox();
            this.CurrentScrollRodValue_Label = new System.Windows.Forms.Label();
            this.CurrentScrollRod_Label = new System.Windows.Forms.Label();
            this.CurrentRodCastsValue_Label = new System.Windows.Forms.Label();
            this.CurrentRodCasts_Label = new System.Windows.Forms.Label();
            this.ProcessVersionValue_Label = new System.Windows.Forms.Label();
            this.XY_TextBox = new System.Windows.Forms.TextBox();
            this.ProcessVersion_Label = new System.Windows.Forms.Label();
            this.Resources_Label = new System.Windows.Forms.Label();
            this.TotalLinesPulledValue_Label = new System.Windows.Forms.Label();
            this.GameProcessesValue_Label = new System.Windows.Forms.Label();
            this.TotalLinesPulled_Label = new System.Windows.Forms.Label();
            this.GameProcesses_Label = new System.Windows.Forms.Label();
            this.Bot_StatusStrip = new System.Windows.Forms.StatusStrip();
            this.GameStatus_ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.GameStatusValue_ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.Seperator_ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.VULevel_ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.VULevelValue_ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.Capture_ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.Version_ToolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.Java_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Bedrock_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AudioDetectionTimer = new System.Windows.Forms.Timer(this.components);
            this.StatusRefreshTimer = new System.Windows.Forms.Timer(this.components);
            this.AntiAfkTimer = new System.Windows.Forms.Timer(this.components);
            this.RodTimeoutTimer = new System.Windows.Forms.Timer(this.components);
            this.LoadingPleaseWait_Button = new System.Windows.Forms.Button();
            this.VuCaptureTimeoutTimer = new System.Windows.Forms.Timer(this.components);
            this.VuLevelCaptureTimer = new System.Windows.Forms.Timer(this.components);
            this.ServerOptions_GroupBox.SuspendLayout();
            this.CustomWaitValues_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NoiseDelayValue_NumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LineOutDelayValue_NumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LineInDelayValue_NumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartBotDelay_NumericUpDown)).BeginInit();
            this.FishingOptions_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SwingOffsetValue_NumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CustomThrowsValue_NumericUpDown)).BeginInit();
            this.SoundVUSensitivity_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TriggerBotAtValue_NumericUpDown)).BeginInit();
            this.BotStatus_GroupBox.SuspendLayout();
            this.Bot_StatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // ServerOptions_GroupBox
            // 
            this.ServerOptions_GroupBox.Controls.Add(this.AntiAFKProtection_CheckBox);
            this.ServerOptions_GroupBox.Enabled = false;
            this.ServerOptions_GroupBox.Location = new System.Drawing.Point(178, 292);
            this.ServerOptions_GroupBox.Name = "ServerOptions_GroupBox";
            this.ServerOptions_GroupBox.Size = new System.Drawing.Size(154, 48);
            this.ServerOptions_GroupBox.TabIndex = 36;
            this.ServerOptions_GroupBox.TabStop = false;
            this.ServerOptions_GroupBox.Text = "Server Options";
            // 
            // AntiAFKProtection_CheckBox
            // 
            this.AntiAFKProtection_CheckBox.AutoSize = true;
            this.AntiAFKProtection_CheckBox.Location = new System.Drawing.Point(6, 19);
            this.AntiAFKProtection_CheckBox.Name = "AntiAFKProtection_CheckBox";
            this.AntiAFKProtection_CheckBox.Size = new System.Drawing.Size(118, 17);
            this.AntiAFKProtection_CheckBox.TabIndex = 11;
            this.AntiAFKProtection_CheckBox.Text = "Anti-AFK Protection";
            this.AntiAFKProtection_CheckBox.UseVisualStyleBackColor = true;
            // 
            // CustomWaitValues_GroupBox
            // 
            this.CustomWaitValues_GroupBox.Controls.Add(this.NoiseDelayValue_NumericUpDown);
            this.CustomWaitValues_GroupBox.Controls.Add(this.NoiseDelay_Label);
            this.CustomWaitValues_GroupBox.Controls.Add(this.LineOutDelayValue_NumericUpDown);
            this.CustomWaitValues_GroupBox.Controls.Add(this.LineOutDelay_Label);
            this.CustomWaitValues_GroupBox.Controls.Add(this.LineInDelayValue_NumericUpDown);
            this.CustomWaitValues_GroupBox.Controls.Add(this.LineInDelay_Label);
            this.CustomWaitValues_GroupBox.Controls.Add(this.StartBotDelay_NumericUpDown);
            this.CustomWaitValues_GroupBox.Controls.Add(this.StartBotDelay_Label);
            this.CustomWaitValues_GroupBox.Enabled = false;
            this.CustomWaitValues_GroupBox.Location = new System.Drawing.Point(12, 346);
            this.CustomWaitValues_GroupBox.Name = "CustomWaitValues_GroupBox";
            this.CustomWaitValues_GroupBox.Size = new System.Drawing.Size(320, 128);
            this.CustomWaitValues_GroupBox.TabIndex = 35;
            this.CustomWaitValues_GroupBox.TabStop = false;
            this.CustomWaitValues_GroupBox.Text = "Custom Wait Values (Mili-Seconds) --- [1000 = 1 Second]";
            // 
            // NoiseDelayValue_NumericUpDown
            // 
            this.NoiseDelayValue_NumericUpDown.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.NoiseDelayValue_NumericUpDown.Location = new System.Drawing.Point(93, 100);
            this.NoiseDelayValue_NumericUpDown.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.NoiseDelayValue_NumericUpDown.Name = "NoiseDelayValue_NumericUpDown";
            this.NoiseDelayValue_NumericUpDown.Size = new System.Drawing.Size(219, 20);
            this.NoiseDelayValue_NumericUpDown.TabIndex = 15;
            this.NoiseDelayValue_NumericUpDown.Value = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            // 
            // NoiseDelay_Label
            // 
            this.NoiseDelay_Label.AutoSize = true;
            this.NoiseDelay_Label.Location = new System.Drawing.Point(6, 102);
            this.NoiseDelay_Label.Name = "NoiseDelay_Label";
            this.NoiseDelay_Label.Size = new System.Drawing.Size(67, 13);
            this.NoiseDelay_Label.TabIndex = 6;
            this.NoiseDelay_Label.Text = "Noise-Delay:";
            // 
            // LineOutDelayValue_NumericUpDown
            // 
            this.LineOutDelayValue_NumericUpDown.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.LineOutDelayValue_NumericUpDown.Location = new System.Drawing.Point(93, 74);
            this.LineOutDelayValue_NumericUpDown.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.LineOutDelayValue_NumericUpDown.Name = "LineOutDelayValue_NumericUpDown";
            this.LineOutDelayValue_NumericUpDown.Size = new System.Drawing.Size(219, 20);
            this.LineOutDelayValue_NumericUpDown.TabIndex = 14;
            this.LineOutDelayValue_NumericUpDown.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            // 
            // LineOutDelay_Label
            // 
            this.LineOutDelay_Label.AutoSize = true;
            this.LineOutDelay_Label.Location = new System.Drawing.Point(6, 76);
            this.LineOutDelay_Label.Name = "LineOutDelay_Label";
            this.LineOutDelay_Label.Size = new System.Drawing.Size(80, 13);
            this.LineOutDelay_Label.TabIndex = 4;
            this.LineOutDelay_Label.Text = "Line-Out-Delay:";
            // 
            // LineInDelayValue_NumericUpDown
            // 
            this.LineInDelayValue_NumericUpDown.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.LineInDelayValue_NumericUpDown.Location = new System.Drawing.Point(93, 48);
            this.LineInDelayValue_NumericUpDown.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.LineInDelayValue_NumericUpDown.Name = "LineInDelayValue_NumericUpDown";
            this.LineInDelayValue_NumericUpDown.Size = new System.Drawing.Size(219, 20);
            this.LineInDelayValue_NumericUpDown.TabIndex = 13;
            // 
            // LineInDelay_Label
            // 
            this.LineInDelay_Label.AutoSize = true;
            this.LineInDelay_Label.Location = new System.Drawing.Point(6, 50);
            this.LineInDelay_Label.Name = "LineInDelay_Label";
            this.LineInDelay_Label.Size = new System.Drawing.Size(72, 13);
            this.LineInDelay_Label.TabIndex = 2;
            this.LineInDelay_Label.Text = "Line-In-Delay:";
            // 
            // StartBotDelay_NumericUpDown
            // 
            this.StartBotDelay_NumericUpDown.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.StartBotDelay_NumericUpDown.Location = new System.Drawing.Point(93, 22);
            this.StartBotDelay_NumericUpDown.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.StartBotDelay_NumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.StartBotDelay_NumericUpDown.Name = "StartBotDelay_NumericUpDown";
            this.StartBotDelay_NumericUpDown.Size = new System.Drawing.Size(219, 20);
            this.StartBotDelay_NumericUpDown.TabIndex = 12;
            this.StartBotDelay_NumericUpDown.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // StartBotDelay_Label
            // 
            this.StartBotDelay_Label.AutoSize = true;
            this.StartBotDelay_Label.Location = new System.Drawing.Point(6, 24);
            this.StartBotDelay_Label.Name = "StartBotDelay_Label";
            this.StartBotDelay_Label.Size = new System.Drawing.Size(81, 13);
            this.StartBotDelay_Label.TabIndex = 0;
            this.StartBotDelay_Label.Text = "Start-Bot-Delay:";
            // 
            // PressToStopBot_Button
            // 
            this.PressToStopBot_Button.Font = new System.Drawing.Font("Segoe Print", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PressToStopBot_Button.Location = new System.Drawing.Point(12, 12);
            this.PressToStopBot_Button.Name = "PressToStopBot_Button";
            this.PressToStopBot_Button.Size = new System.Drawing.Size(320, 62);
            this.PressToStopBot_Button.TabIndex = 31;
            this.PressToStopBot_Button.Text = "Press To Stop Bot";
            this.PressToStopBot_Button.UseVisualStyleBackColor = true;
            this.PressToStopBot_Button.Visible = false;
            this.PressToStopBot_Button.Click += new System.EventHandler(this.Button4_Click);
            // 
            // PressToStartBot_Button
            // 
            this.PressToStartBot_Button.Font = new System.Drawing.Font("Segoe Print", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PressToStartBot_Button.Location = new System.Drawing.Point(12, 12);
            this.PressToStartBot_Button.Name = "PressToStartBot_Button";
            this.PressToStartBot_Button.Size = new System.Drawing.Size(320, 62);
            this.PressToStartBot_Button.TabIndex = 30;
            this.PressToStartBot_Button.Text = "Press To Start Bot";
            this.PressToStartBot_Button.UseVisualStyleBackColor = true;
            this.PressToStartBot_Button.Click += new System.EventHandler(this.Button5_Click);
            // 
            // FishingOptions_GroupBox
            // 
            this.FishingOptions_GroupBox.Controls.Add(this.SwingOffsetValue_NumericUpDown);
            this.FishingOptions_GroupBox.Controls.Add(this.SwingOffset_CheckBox);
            this.FishingOptions_GroupBox.Controls.Add(this.UnlimitedThrows_CheckBox);
            this.FishingOptions_GroupBox.Controls.Add(this.CloseBotWhenFinished_CheckBox);
            this.FishingOptions_GroupBox.Controls.Add(this.CustomThrowsValue_NumericUpDown);
            this.FishingOptions_GroupBox.Controls.Add(this.LeaveWhenFinished_CheckBox);
            this.FishingOptions_GroupBox.Controls.Add(this.AutoEjectRod_CheckBox);
            this.FishingOptions_GroupBox.Controls.Add(this.Save1HP_CheckBox);
            this.FishingOptions_GroupBox.Controls.Add(this.UseHotbarScrolling_CheckBox);
            this.FishingOptions_GroupBox.Controls.Add(this.CustomThrows_CheckBox);
            this.FishingOptions_GroupBox.Enabled = false;
            this.FishingOptions_GroupBox.Location = new System.Drawing.Point(12, 133);
            this.FishingOptions_GroupBox.Name = "FishingOptions_GroupBox";
            this.FishingOptions_GroupBox.Size = new System.Drawing.Size(160, 207);
            this.FishingOptions_GroupBox.TabIndex = 34;
            this.FishingOptions_GroupBox.TabStop = false;
            this.FishingOptions_GroupBox.Text = "Fishing Options";
            // 
            // SwingOffsetValue_NumericUpDown
            // 
            this.SwingOffsetValue_NumericUpDown.Enabled = false;
            this.SwingOffsetValue_NumericUpDown.Location = new System.Drawing.Point(98, 180);
            this.SwingOffsetValue_NumericUpDown.Maximum = new decimal(new int[] {
            2500,
            0,
            0,
            0});
            this.SwingOffsetValue_NumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SwingOffsetValue_NumericUpDown.Name = "SwingOffsetValue_NumericUpDown";
            this.SwingOffsetValue_NumericUpDown.Size = new System.Drawing.Size(56, 20);
            this.SwingOffsetValue_NumericUpDown.TabIndex = 13;
            this.SwingOffsetValue_NumericUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // SwingOffset_CheckBox
            // 
            this.SwingOffset_CheckBox.AutoSize = true;
            this.SwingOffset_CheckBox.Location = new System.Drawing.Point(6, 182);
            this.SwingOffset_CheckBox.Name = "SwingOffset_CheckBox";
            this.SwingOffset_CheckBox.Size = new System.Drawing.Size(86, 17);
            this.SwingOffset_CheckBox.TabIndex = 12;
            this.SwingOffset_CheckBox.Text = "Swing Offset";
            this.SwingOffset_CheckBox.UseVisualStyleBackColor = true;
            this.SwingOffset_CheckBox.CheckedChanged += new System.EventHandler(this.CheckBox12_CheckedChanged);
            // 
            // UnlimitedThrows_CheckBox
            // 
            this.UnlimitedThrows_CheckBox.AutoSize = true;
            this.UnlimitedThrows_CheckBox.Location = new System.Drawing.Point(6, 113);
            this.UnlimitedThrows_CheckBox.Name = "UnlimitedThrows_CheckBox";
            this.UnlimitedThrows_CheckBox.Size = new System.Drawing.Size(107, 17);
            this.UnlimitedThrows_CheckBox.TabIndex = 11;
            this.UnlimitedThrows_CheckBox.Text = "Unlimited Throws";
            this.UnlimitedThrows_CheckBox.UseVisualStyleBackColor = true;
            this.UnlimitedThrows_CheckBox.CheckedChanged += new System.EventHandler(this.CheckBox9_CheckedChanged);
            // 
            // CloseBotWhenFinished_CheckBox
            // 
            this.CloseBotWhenFinished_CheckBox.AutoSize = true;
            this.CloseBotWhenFinished_CheckBox.Location = new System.Drawing.Point(6, 159);
            this.CloseBotWhenFinished_CheckBox.Name = "CloseBotWhenFinished_CheckBox";
            this.CloseBotWhenFinished_CheckBox.Size = new System.Drawing.Size(145, 17);
            this.CloseBotWhenFinished_CheckBox.TabIndex = 10;
            this.CloseBotWhenFinished_CheckBox.Text = "Close Bot When Finished";
            this.CloseBotWhenFinished_CheckBox.UseVisualStyleBackColor = true;
            // 
            // CustomThrowsValue_NumericUpDown
            // 
            this.CustomThrowsValue_NumericUpDown.Enabled = false;
            this.CustomThrowsValue_NumericUpDown.Location = new System.Drawing.Point(111, 19);
            this.CustomThrowsValue_NumericUpDown.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.CustomThrowsValue_NumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.CustomThrowsValue_NumericUpDown.Name = "CustomThrowsValue_NumericUpDown";
            this.CustomThrowsValue_NumericUpDown.Size = new System.Drawing.Size(43, 20);
            this.CustomThrowsValue_NumericUpDown.TabIndex = 4;
            this.CustomThrowsValue_NumericUpDown.Value = new decimal(new int[] {
            64,
            0,
            0,
            0});
            // 
            // LeaveWhenFinished_CheckBox
            // 
            this.LeaveWhenFinished_CheckBox.AutoSize = true;
            this.LeaveWhenFinished_CheckBox.Location = new System.Drawing.Point(6, 136);
            this.LeaveWhenFinished_CheckBox.Name = "LeaveWhenFinished_CheckBox";
            this.LeaveWhenFinished_CheckBox.Size = new System.Drawing.Size(130, 17);
            this.LeaveWhenFinished_CheckBox.TabIndex = 9;
            this.LeaveWhenFinished_CheckBox.Text = "Leave When Finished";
            this.LeaveWhenFinished_CheckBox.UseVisualStyleBackColor = true;
            // 
            // AutoEjectRod_CheckBox
            // 
            this.AutoEjectRod_CheckBox.AutoSize = true;
            this.AutoEjectRod_CheckBox.Location = new System.Drawing.Point(6, 90);
            this.AutoEjectRod_CheckBox.Name = "AutoEjectRod_CheckBox";
            this.AutoEjectRod_CheckBox.Size = new System.Drawing.Size(98, 17);
            this.AutoEjectRod_CheckBox.TabIndex = 7;
            this.AutoEjectRod_CheckBox.Text = "Auto Eject Rod";
            this.AutoEjectRod_CheckBox.UseVisualStyleBackColor = true;
            this.AutoEjectRod_CheckBox.CheckedChanged += new System.EventHandler(this.CheckBox4_CheckedChanged);
            // 
            // Save1HP_CheckBox
            // 
            this.Save1HP_CheckBox.AutoSize = true;
            this.Save1HP_CheckBox.Location = new System.Drawing.Point(6, 67);
            this.Save1HP_CheckBox.Name = "Save1HP_CheckBox";
            this.Save1HP_CheckBox.Size = new System.Drawing.Size(78, 17);
            this.Save1HP_CheckBox.TabIndex = 6;
            this.Save1HP_CheckBox.Text = "Save 1 HP";
            this.Save1HP_CheckBox.UseVisualStyleBackColor = true;
            this.Save1HP_CheckBox.CheckedChanged += new System.EventHandler(this.CheckBox3_CheckedChanged);
            // 
            // UseHotbarScrolling_CheckBox
            // 
            this.UseHotbarScrolling_CheckBox.AutoSize = true;
            this.UseHotbarScrolling_CheckBox.Location = new System.Drawing.Point(6, 44);
            this.UseHotbarScrolling_CheckBox.Name = "UseHotbarScrolling_CheckBox";
            this.UseHotbarScrolling_CheckBox.Size = new System.Drawing.Size(123, 17);
            this.UseHotbarScrolling_CheckBox.TabIndex = 5;
            this.UseHotbarScrolling_CheckBox.Text = "Use Hotbar Scrolling";
            this.UseHotbarScrolling_CheckBox.UseVisualStyleBackColor = true;
            // 
            // CustomThrows_CheckBox
            // 
            this.CustomThrows_CheckBox.AutoSize = true;
            this.CustomThrows_CheckBox.Location = new System.Drawing.Point(6, 21);
            this.CustomThrows_CheckBox.Name = "CustomThrows_CheckBox";
            this.CustomThrows_CheckBox.Size = new System.Drawing.Size(99, 17);
            this.CustomThrows_CheckBox.TabIndex = 3;
            this.CustomThrows_CheckBox.Text = "Custom Throws";
            this.CustomThrows_CheckBox.UseVisualStyleBackColor = true;
            this.CustomThrows_CheckBox.CheckedChanged += new System.EventHandler(this.CheckBox1_CheckedChanged);
            // 
            // SoundVUSensitivity_GroupBox
            // 
            this.SoundVUSensitivity_GroupBox.Controls.Add(this.TriggerBotAt_Label);
            this.SoundVUSensitivity_GroupBox.Controls.Add(this.TriggerBotAtValue_NumericUpDown);
            this.SoundVUSensitivity_GroupBox.Enabled = false;
            this.SoundVUSensitivity_GroupBox.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.SoundVUSensitivity_GroupBox.Location = new System.Drawing.Point(12, 80);
            this.SoundVUSensitivity_GroupBox.Name = "SoundVUSensitivity_GroupBox";
            this.SoundVUSensitivity_GroupBox.Size = new System.Drawing.Size(320, 47);
            this.SoundVUSensitivity_GroupBox.TabIndex = 29;
            this.SoundVUSensitivity_GroupBox.TabStop = false;
            this.SoundVUSensitivity_GroupBox.Text = "Sound VU Sensitivity (Volume Level) --- [0% - 100%]";
            // 
            // TriggerBotAt_Label
            // 
            this.TriggerBotAt_Label.AutoSize = true;
            this.TriggerBotAt_Label.Location = new System.Drawing.Point(6, 21);
            this.TriggerBotAt_Label.Name = "TriggerBotAt_Label";
            this.TriggerBotAt_Label.Size = new System.Drawing.Size(75, 13);
            this.TriggerBotAt_Label.TabIndex = 17;
            this.TriggerBotAt_Label.Text = "Trigger Bot At:";
            // 
            // TriggerBotAtValue_NumericUpDown
            // 
            this.TriggerBotAtValue_NumericUpDown.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.TriggerBotAtValue_NumericUpDown.Location = new System.Drawing.Point(87, 19);
            this.TriggerBotAtValue_NumericUpDown.Name = "TriggerBotAtValue_NumericUpDown";
            this.TriggerBotAtValue_NumericUpDown.Size = new System.Drawing.Size(225, 20);
            this.TriggerBotAtValue_NumericUpDown.TabIndex = 16;
            this.TriggerBotAtValue_NumericUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // BotStatus_GroupBox
            // 
            this.BotStatus_GroupBox.Controls.Add(this.Debug_CheckBox1);
            this.BotStatus_GroupBox.Controls.Add(this.Debug_CheckBox3);
            this.BotStatus_GroupBox.Controls.Add(this.Debug_CheckBox2);
            this.BotStatus_GroupBox.Controls.Add(this.CurrentScrollRodValue_Label);
            this.BotStatus_GroupBox.Controls.Add(this.CurrentScrollRod_Label);
            this.BotStatus_GroupBox.Controls.Add(this.CurrentRodCastsValue_Label);
            this.BotStatus_GroupBox.Controls.Add(this.CurrentRodCasts_Label);
            this.BotStatus_GroupBox.Controls.Add(this.ProcessVersionValue_Label);
            this.BotStatus_GroupBox.Controls.Add(this.XY_TextBox);
            this.BotStatus_GroupBox.Controls.Add(this.ProcessVersion_Label);
            this.BotStatus_GroupBox.Controls.Add(this.Resources_Label);
            this.BotStatus_GroupBox.Controls.Add(this.TotalLinesPulledValue_Label);
            this.BotStatus_GroupBox.Controls.Add(this.GameProcessesValue_Label);
            this.BotStatus_GroupBox.Controls.Add(this.TotalLinesPulled_Label);
            this.BotStatus_GroupBox.Controls.Add(this.GameProcesses_Label);
            this.BotStatus_GroupBox.Enabled = false;
            this.BotStatus_GroupBox.Location = new System.Drawing.Point(178, 133);
            this.BotStatus_GroupBox.Name = "BotStatus_GroupBox";
            this.BotStatus_GroupBox.Size = new System.Drawing.Size(154, 153);
            this.BotStatus_GroupBox.TabIndex = 33;
            this.BotStatus_GroupBox.TabStop = false;
            this.BotStatus_GroupBox.Text = "Bot Stats:";
            // 
            // Debug_CheckBox1
            // 
            this.Debug_CheckBox1.AutoSize = true;
            this.Debug_CheckBox1.Location = new System.Drawing.Point(133, 19);
            this.Debug_CheckBox1.Name = "Debug_CheckBox1";
            this.Debug_CheckBox1.Size = new System.Drawing.Size(15, 14);
            this.Debug_CheckBox1.TabIndex = 34;
            this.Debug_CheckBox1.UseVisualStyleBackColor = true;
            this.Debug_CheckBox1.Visible = false;
            // 
            // Debug_CheckBox3
            // 
            this.Debug_CheckBox3.AutoSize = true;
            this.Debug_CheckBox3.Location = new System.Drawing.Point(133, 59);
            this.Debug_CheckBox3.Name = "Debug_CheckBox3";
            this.Debug_CheckBox3.Size = new System.Drawing.Size(15, 14);
            this.Debug_CheckBox3.TabIndex = 33;
            this.Debug_CheckBox3.UseVisualStyleBackColor = true;
            this.Debug_CheckBox3.Visible = false;
            // 
            // Debug_CheckBox2
            // 
            this.Debug_CheckBox2.AutoSize = true;
            this.Debug_CheckBox2.Location = new System.Drawing.Point(133, 39);
            this.Debug_CheckBox2.Name = "Debug_CheckBox2";
            this.Debug_CheckBox2.Size = new System.Drawing.Size(15, 14);
            this.Debug_CheckBox2.TabIndex = 32;
            this.Debug_CheckBox2.UseVisualStyleBackColor = true;
            this.Debug_CheckBox2.Visible = false;
            // 
            // CurrentScrollRodValue_Label
            // 
            this.CurrentScrollRodValue_Label.AutoSize = true;
            this.CurrentScrollRodValue_Label.Location = new System.Drawing.Point(103, 132);
            this.CurrentScrollRodValue_Label.Name = "CurrentScrollRodValue_Label";
            this.CurrentScrollRodValue_Label.Size = new System.Drawing.Size(13, 13);
            this.CurrentScrollRodValue_Label.TabIndex = 31;
            this.CurrentScrollRodValue_Label.Text = "0";
            // 
            // CurrentScrollRod_Label
            // 
            this.CurrentScrollRod_Label.AutoSize = true;
            this.CurrentScrollRod_Label.Location = new System.Drawing.Point(6, 132);
            this.CurrentScrollRod_Label.Name = "CurrentScrollRod_Label";
            this.CurrentScrollRod_Label.Size = new System.Drawing.Size(96, 13);
            this.CurrentScrollRod_Label.TabIndex = 30;
            this.CurrentScrollRod_Label.Text = "Current Scroll Rod:";
            // 
            // CurrentRodCastsValue_Label
            // 
            this.CurrentRodCastsValue_Label.AutoSize = true;
            this.CurrentRodCastsValue_Label.Location = new System.Drawing.Point(103, 119);
            this.CurrentRodCastsValue_Label.Name = "CurrentRodCastsValue_Label";
            this.CurrentRodCastsValue_Label.Size = new System.Drawing.Size(13, 13);
            this.CurrentRodCastsValue_Label.TabIndex = 28;
            this.CurrentRodCastsValue_Label.Text = "0";
            // 
            // CurrentRodCasts_Label
            // 
            this.CurrentRodCasts_Label.AutoSize = true;
            this.CurrentRodCasts_Label.Location = new System.Drawing.Point(6, 119);
            this.CurrentRodCasts_Label.Name = "CurrentRodCasts_Label";
            this.CurrentRodCasts_Label.Size = new System.Drawing.Size(96, 13);
            this.CurrentRodCasts_Label.TabIndex = 27;
            this.CurrentRodCasts_Label.Text = "Current Rod Casts:";
            // 
            // ProcessVersionValue_Label
            // 
            this.ProcessVersionValue_Label.AutoSize = true;
            this.ProcessVersionValue_Label.Location = new System.Drawing.Point(88, 35);
            this.ProcessVersionValue_Label.Name = "ProcessVersionValue_Label";
            this.ProcessVersionValue_Label.Size = new System.Drawing.Size(21, 13);
            this.ProcessVersionValue_Label.TabIndex = 26;
            this.ProcessVersionValue_Label.Text = "Na";
            // 
            // XY_TextBox
            // 
            this.XY_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.XY_TextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.XY_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XY_TextBox.Location = new System.Drawing.Point(9, 91);
            this.XY_TextBox.Name = "XY_TextBox";
            this.XY_TextBox.ReadOnly = true;
            this.XY_TextBox.Size = new System.Drawing.Size(91, 14);
            this.XY_TextBox.TabIndex = 19;
            this.XY_TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.XY_TextBox.Visible = false;
            // 
            // ProcessVersion_Label
            // 
            this.ProcessVersion_Label.AutoSize = true;
            this.ProcessVersion_Label.Location = new System.Drawing.Point(6, 35);
            this.ProcessVersion_Label.Name = "ProcessVersion_Label";
            this.ProcessVersion_Label.Size = new System.Drawing.Size(86, 13);
            this.ProcessVersion_Label.TabIndex = 25;
            this.ProcessVersion_Label.Text = "Process Version:";
            // 
            // Resources_Label
            // 
            this.Resources_Label.AutoSize = true;
            this.Resources_Label.Location = new System.Drawing.Point(6, 48);
            this.Resources_Label.Name = "Resources_Label";
            this.Resources_Label.Size = new System.Drawing.Size(59, 13);
            this.Resources_Label.TabIndex = 24;
            this.Resources_Label.Text = "Recources";
            // 
            // TotalLinesPulledValue_Label
            // 
            this.TotalLinesPulledValue_Label.AutoSize = true;
            this.TotalLinesPulledValue_Label.Location = new System.Drawing.Point(101, 106);
            this.TotalLinesPulledValue_Label.Name = "TotalLinesPulledValue_Label";
            this.TotalLinesPulledValue_Label.Size = new System.Drawing.Size(13, 13);
            this.TotalLinesPulledValue_Label.TabIndex = 23;
            this.TotalLinesPulledValue_Label.Text = "0";
            // 
            // GameProcessesValue_Label
            // 
            this.GameProcessesValue_Label.AutoSize = true;
            this.GameProcessesValue_Label.Location = new System.Drawing.Point(92, 22);
            this.GameProcessesValue_Label.Name = "GameProcessesValue_Label";
            this.GameProcessesValue_Label.Size = new System.Drawing.Size(13, 13);
            this.GameProcessesValue_Label.TabIndex = 22;
            this.GameProcessesValue_Label.Text = "0";
            // 
            // TotalLinesPulled_Label
            // 
            this.TotalLinesPulled_Label.AutoSize = true;
            this.TotalLinesPulled_Label.Location = new System.Drawing.Point(6, 106);
            this.TotalLinesPulled_Label.Name = "TotalLinesPulled_Label";
            this.TotalLinesPulled_Label.Size = new System.Drawing.Size(94, 13);
            this.TotalLinesPulled_Label.TabIndex = 21;
            this.TotalLinesPulled_Label.Text = "Total Lines Pulled:";
            // 
            // GameProcesses_Label
            // 
            this.GameProcesses_Label.AutoSize = true;
            this.GameProcesses_Label.Location = new System.Drawing.Point(6, 22);
            this.GameProcesses_Label.Name = "GameProcesses_Label";
            this.GameProcesses_Label.Size = new System.Drawing.Size(90, 13);
            this.GameProcesses_Label.TabIndex = 20;
            this.GameProcesses_Label.Text = "Game Processes:";
            // 
            // Bot_StatusStrip
            // 
            this.Bot_StatusStrip.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Bot_StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.GameStatus_ToolStripStatusLabel,
            this.GameStatusValue_ToolStripStatusLabel,
            this.Seperator_ToolStripStatusLabel,
            this.VULevel_ToolStripStatusLabel,
            this.VULevelValue_ToolStripStatusLabel,
            this.Capture_ToolStripStatusLabel,
            this.Version_ToolStripDropDownButton});
            this.Bot_StatusStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.Bot_StatusStrip.Location = new System.Drawing.Point(0, 483);
            this.Bot_StatusStrip.Name = "Bot_StatusStrip";
            this.Bot_StatusStrip.Size = new System.Drawing.Size(341, 41);
            this.Bot_StatusStrip.SizingGrip = false;
            this.Bot_StatusStrip.TabIndex = 32;
            this.Bot_StatusStrip.Text = "StatusStrip1";
            // 
            // GameStatus_ToolStripStatusLabel
            // 
            this.GameStatus_ToolStripStatusLabel.BackColor = System.Drawing.Color.Transparent;
            this.GameStatus_ToolStripStatusLabel.Name = "GameStatus_ToolStripStatusLabel";
            this.GameStatus_ToolStripStatusLabel.Size = new System.Drawing.Size(76, 15);
            this.GameStatus_ToolStripStatusLabel.Text = "Game Status:";
            // 
            // GameStatusValue_ToolStripStatusLabel
            // 
            this.GameStatusValue_ToolStripStatusLabel.BackColor = System.Drawing.Color.Transparent;
            this.GameStatusValue_ToolStripStatusLabel.ForeColor = System.Drawing.Color.DarkRed;
            this.GameStatusValue_ToolStripStatusLabel.Name = "GameStatusValue_ToolStripStatusLabel";
            this.GameStatusValue_ToolStripStatusLabel.Size = new System.Drawing.Size(64, 15);
            this.GameStatusValue_ToolStripStatusLabel.Text = "Not Found";
            // 
            // Seperator_ToolStripStatusLabel
            // 
            this.Seperator_ToolStripStatusLabel.Name = "Seperator_ToolStripStatusLabel";
            this.Seperator_ToolStripStatusLabel.Size = new System.Drawing.Size(10, 15);
            this.Seperator_ToolStripStatusLabel.Text = "|";
            // 
            // VULevel_ToolStripStatusLabel
            // 
            this.VULevel_ToolStripStatusLabel.Name = "VULevel_ToolStripStatusLabel";
            this.VULevel_ToolStripStatusLabel.Size = new System.Drawing.Size(55, 15);
            this.VULevel_ToolStripStatusLabel.Text = "VU Level:";
            // 
            // VULevelValue_ToolStripStatusLabel
            // 
            this.VULevelValue_ToolStripStatusLabel.Name = "VULevelValue_ToolStripStatusLabel";
            this.VULevelValue_ToolStripStatusLabel.Size = new System.Drawing.Size(29, 15);
            this.VULevelValue_ToolStripStatusLabel.Text = "N/A";
            // 
            // Capture_ToolStripStatusLabel
            // 
            this.Capture_ToolStripStatusLabel.Name = "Capture_ToolStripStatusLabel";
            this.Capture_ToolStripStatusLabel.Size = new System.Drawing.Size(65, 15);
            this.Capture_ToolStripStatusLabel.Text = "- [Capture]";
            this.Capture_ToolStripStatusLabel.Click += new System.EventHandler(this.ToolStripStatusLabel6_Click);
            // 
            // Version_ToolStripDropDownButton
            // 
            this.Version_ToolStripDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Version_ToolStripDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Java_ToolStripMenuItem,
            this.Bedrock_ToolStripMenuItem});
            this.Version_ToolStripDropDownButton.Image = ((System.Drawing.Image)(resources.GetObject("Version_ToolStripDropDownButton.Image")));
            this.Version_ToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Version_ToolStripDropDownButton.Name = "Version_ToolStripDropDownButton";
            this.Version_ToolStripDropDownButton.Size = new System.Drawing.Size(86, 19);
            this.Version_ToolStripDropDownButton.Text = "Version: Java";
            this.Version_ToolStripDropDownButton.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ToolStripDropDownButton1_DropDownItemClicked);
            // 
            // Java_ToolStripMenuItem
            // 
            this.Java_ToolStripMenuItem.Name = "Java_ToolStripMenuItem";
            this.Java_ToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.Java_ToolStripMenuItem.Text = "Java";
            // 
            // Bedrock_ToolStripMenuItem
            // 
            this.Bedrock_ToolStripMenuItem.Name = "Bedrock_ToolStripMenuItem";
            this.Bedrock_ToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.Bedrock_ToolStripMenuItem.Text = "Bedrock";
            // 
            // AudioDetectionTimer
            // 
            this.AudioDetectionTimer.Interval = 1;
            this.AudioDetectionTimer.Tick += new System.EventHandler(this.AudioDetectionTimer_Tick);
            // 
            // StatusRefreshTimer
            // 
            this.StatusRefreshTimer.Enabled = true;
            this.StatusRefreshTimer.Interval = 1;
            this.StatusRefreshTimer.Tick += new System.EventHandler(this.StatusRefreshTimer_Tick);
            // 
            // AntiAfkTimer
            // 
            this.AntiAfkTimer.Interval = 5000;
            this.AntiAfkTimer.Tick += new System.EventHandler(this.AntiAfkTimer_Tick);
            // 
            // RodTimeoutTimer
            // 
            this.RodTimeoutTimer.Interval = 120000;
            this.RodTimeoutTimer.Tick += new System.EventHandler(this.RodTimeoutTimer_Tick);
            // 
            // LoadingPleaseWait_Button
            // 
            this.LoadingPleaseWait_Button.Enabled = false;
            this.LoadingPleaseWait_Button.Font = new System.Drawing.Font("Segoe Print", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoadingPleaseWait_Button.Location = new System.Drawing.Point(12, 12);
            this.LoadingPleaseWait_Button.Name = "LoadingPleaseWait_Button";
            this.LoadingPleaseWait_Button.Size = new System.Drawing.Size(320, 62);
            this.LoadingPleaseWait_Button.TabIndex = 40;
            this.LoadingPleaseWait_Button.Text = "Loading... Please Wait.";
            this.LoadingPleaseWait_Button.UseVisualStyleBackColor = true;
            this.LoadingPleaseWait_Button.Visible = false;
            // 
            // VuCaptureTimeoutTimer
            // 
            this.VuCaptureTimeoutTimer.Interval = 60000;
            this.VuCaptureTimeoutTimer.Tick += new System.EventHandler(this.VuCaptureTimeoutTimer_Tick);
            // 
            // VuLevelCaptureTimer
            // 
            this.VuLevelCaptureTimer.Interval = 1;
            this.VuLevelCaptureTimer.Tick += new System.EventHandler(this.VuLevelCaptureTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 524);
            this.Controls.Add(this.ServerOptions_GroupBox);
            this.Controls.Add(this.CustomWaitValues_GroupBox);
            this.Controls.Add(this.PressToStartBot_Button);
            this.Controls.Add(this.FishingOptions_GroupBox);
            this.Controls.Add(this.SoundVUSensitivity_GroupBox);
            this.Controls.Add(this.BotStatus_GroupBox);
            this.Controls.Add(this.Bot_StatusStrip);
            this.Controls.Add(this.PressToStopBot_Button);
            this.Controls.Add(this.LoadingPleaseWait_Button);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Minecraft Auto-Fishing Bot";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ServerOptions_GroupBox.ResumeLayout(false);
            this.ServerOptions_GroupBox.PerformLayout();
            this.CustomWaitValues_GroupBox.ResumeLayout(false);
            this.CustomWaitValues_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NoiseDelayValue_NumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LineOutDelayValue_NumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LineInDelayValue_NumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartBotDelay_NumericUpDown)).EndInit();
            this.FishingOptions_GroupBox.ResumeLayout(false);
            this.FishingOptions_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SwingOffsetValue_NumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CustomThrowsValue_NumericUpDown)).EndInit();
            this.SoundVUSensitivity_GroupBox.ResumeLayout(false);
            this.SoundVUSensitivity_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TriggerBotAtValue_NumericUpDown)).EndInit();
            this.BotStatus_GroupBox.ResumeLayout(false);
            this.BotStatus_GroupBox.PerformLayout();
            this.Bot_StatusStrip.ResumeLayout(false);
            this.Bot_StatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal System.Windows.Forms.GroupBox ServerOptions_GroupBox;
        internal System.Windows.Forms.CheckBox AntiAFKProtection_CheckBox;
        internal System.Windows.Forms.GroupBox CustomWaitValues_GroupBox;
        internal System.Windows.Forms.NumericUpDown NoiseDelayValue_NumericUpDown;
        internal System.Windows.Forms.Label NoiseDelay_Label;
        internal System.Windows.Forms.NumericUpDown LineOutDelayValue_NumericUpDown;
        internal System.Windows.Forms.Label LineOutDelay_Label;
        internal System.Windows.Forms.NumericUpDown LineInDelayValue_NumericUpDown;
        internal System.Windows.Forms.Label LineInDelay_Label;
        internal System.Windows.Forms.NumericUpDown StartBotDelay_NumericUpDown;
        internal System.Windows.Forms.Label StartBotDelay_Label;
        internal System.Windows.Forms.Button PressToStopBot_Button;
        internal System.Windows.Forms.Button PressToStartBot_Button;
        internal System.Windows.Forms.GroupBox FishingOptions_GroupBox;
        internal System.Windows.Forms.NumericUpDown CustomThrowsValue_NumericUpDown;
        internal System.Windows.Forms.GroupBox SoundVUSensitivity_GroupBox;
        internal System.Windows.Forms.GroupBox BotStatus_GroupBox;
        internal System.Windows.Forms.Label CurrentScrollRodValue_Label;
        internal System.Windows.Forms.Label CurrentScrollRod_Label;
        internal System.Windows.Forms.Label CurrentRodCastsValue_Label;
        internal System.Windows.Forms.Label CurrentRodCasts_Label;
        internal System.Windows.Forms.Label ProcessVersionValue_Label;
        internal System.Windows.Forms.Label ProcessVersion_Label;
        internal System.Windows.Forms.Label Resources_Label;
        internal System.Windows.Forms.Label TotalLinesPulledValue_Label;
        internal System.Windows.Forms.Label GameProcessesValue_Label;
        internal System.Windows.Forms.Label TotalLinesPulled_Label;
        internal System.Windows.Forms.Label GameProcesses_Label;
        internal System.Windows.Forms.TextBox XY_TextBox;
        internal System.Windows.Forms.StatusStrip Bot_StatusStrip;
        internal System.Windows.Forms.ToolStripStatusLabel GameStatus_ToolStripStatusLabel;
        internal System.Windows.Forms.ToolStripStatusLabel GameStatusValue_ToolStripStatusLabel;
        internal System.Windows.Forms.Timer AudioDetectionTimer;
        internal System.Windows.Forms.Timer StatusRefreshTimer;
        internal System.Windows.Forms.Timer AntiAfkTimer;
        internal System.Windows.Forms.Timer RodTimeoutTimer;
        internal System.Windows.Forms.NumericUpDown TriggerBotAtValue_NumericUpDown;
        internal System.Windows.Forms.CheckBox Debug_CheckBox3;
        internal System.Windows.Forms.CheckBox Debug_CheckBox2;
        internal System.Windows.Forms.CheckBox Debug_CheckBox1;
        internal System.Windows.Forms.CheckBox UnlimitedThrows_CheckBox;
        internal System.Windows.Forms.CheckBox CloseBotWhenFinished_CheckBox;
        internal System.Windows.Forms.CheckBox LeaveWhenFinished_CheckBox;
        internal System.Windows.Forms.CheckBox AutoEjectRod_CheckBox;
        internal System.Windows.Forms.CheckBox Save1HP_CheckBox;
        internal System.Windows.Forms.CheckBox UseHotbarScrolling_CheckBox;
        internal System.Windows.Forms.CheckBox CustomThrows_CheckBox;
        internal System.Windows.Forms.Button LoadingPleaseWait_Button;
        private System.Windows.Forms.Label TriggerBotAt_Label;
        private System.Windows.Forms.ToolStripStatusLabel VULevel_ToolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel VULevelValue_ToolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel Capture_ToolStripStatusLabel;
        private System.Windows.Forms.Timer VuCaptureTimeoutTimer;
        private System.Windows.Forms.Timer VuLevelCaptureTimer;
        internal System.Windows.Forms.CheckBox SwingOffset_CheckBox;
        internal System.Windows.Forms.NumericUpDown SwingOffsetValue_NumericUpDown;
        private System.Windows.Forms.ToolStripDropDownButton Version_ToolStripDropDownButton;
        private System.Windows.Forms.ToolStripMenuItem Bedrock_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Java_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel Seperator_ToolStripStatusLabel;
    }
}

