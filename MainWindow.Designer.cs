namespace RaidCrawler
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.ButtonAdvanceDate = new System.Windows.Forms.Button();
            this.CheckEnableFilters = new System.Windows.Forms.CheckBox();
            this.ButtonDisconnect = new System.Windows.Forms.Button();
            this.ButtonConnect = new System.Windows.Forms.Button();
            this.ConnectionStatusText = new System.Windows.Forms.Label();
            this.InputSwitchIP = new System.Windows.Forms.TextBox();
            this.LabelStatus = new System.Windows.Forms.Label();
            this.LabelSwitchIP = new System.Windows.Forms.Label();
            this.LabelLoadedRaids = new System.Windows.Forms.Label();
            this.TeraType = new System.Windows.Forms.TextBox();
            this.LabelTeraType = new System.Windows.Forms.Label();
            this.PID = new System.Windows.Forms.TextBox();
            this.LabelPID = new System.Windows.Forms.Label();
            this.EC = new System.Windows.Forms.TextBox();
            this.LabelEC = new System.Windows.Forms.Label();
            this.Seed = new System.Windows.Forms.TextBox();
            this.LabelSeed = new System.Windows.Forms.Label();
            this.ButtonNext = new System.Windows.Forms.Button();
            this.ButtonPrevious = new System.Windows.Forms.Button();
            this.Area = new System.Windows.Forms.TextBox();
            this.LabelUNK_2 = new System.Windows.Forms.Label();
            this.IVs = new System.Windows.Forms.TextBox();
            this.LabelIVs = new System.Windows.Forms.Label();
            this.ButtonReadRaids = new System.Windows.Forms.Button();
            this.IsEvent = new System.Windows.Forms.CheckBox();
            this.LabelIsEvent = new System.Windows.Forms.Label();
            this.Difficulty = new System.Windows.Forms.TextBox();
            this.LabelDifficulty = new System.Windows.Forms.Label();
            this.Progress = new System.Windows.Forms.ComboBox();
            this.LabelStoryProgress = new System.Windows.Forms.Label();
            this.ButtonViewRAM = new System.Windows.Forms.Button();
            this.Species = new System.Windows.Forms.TextBox();
            this.LabelSpecies = new System.Windows.Forms.Label();
            this.LabelMoves = new System.Windows.Forms.Label();
            this.Move1 = new System.Windows.Forms.TextBox();
            this.Move2 = new System.Windows.Forms.TextBox();
            this.Move4 = new System.Windows.Forms.TextBox();
            this.Move3 = new System.Windows.Forms.TextBox();
            this.LabelGame = new System.Windows.Forms.Label();
            this.Game = new System.Windows.Forms.ComboBox();
            this.LabelEventProgress = new System.Windows.Forms.Label();
            this.EventProgress = new System.Windows.Forms.ComboBox();
            this.Nature = new System.Windows.Forms.TextBox();
            this.LabelNature = new System.Windows.Forms.Label();
            this.Gender = new System.Windows.Forms.TextBox();
            this.LabelGender = new System.Windows.Forms.Label();
            this.StopFilter = new System.Windows.Forms.Button();
            this.Sprite = new System.Windows.Forms.PictureBox();
            this.Ability = new System.Windows.Forms.TextBox();
            this.LabelAbility = new System.Windows.Forms.Label();
            this.GemIcon = new System.Windows.Forms.PictureBox();
            this.ButtonDownloadEvents = new System.Windows.Forms.Button();
            this.ConfigSettings = new System.Windows.Forms.Button();
            this.Rewards = new System.Windows.Forms.Button();
            this.LabelSandwichBonus = new System.Windows.Forms.Label();
            this.RaidBoost = new System.Windows.Forms.ComboBox();
            this.ComboIndex = new System.Windows.Forms.ComboBox();
            this.SendScreenshot = new System.Windows.Forms.Button();
            this.SearchTimer = new System.Windows.Forms.Timer(this.components);
            this.SearchTime = new System.Windows.Forms.Label();
            this.DaySkipSuccessRate = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.Sprite)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GemIcon)).BeginInit();
            this.panel2.SuspendLayout();
            this.myPanel1.SuspendLayout();
            this.myPanel2.SuspendLayout();
            this.myPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonAdvanceDate
            // 
            this.ButtonAdvanceDate.Enabled = false;
            this.ButtonAdvanceDate.Location = new System.Drawing.Point(8, 216);
            this.ButtonAdvanceDate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ButtonAdvanceDate.Name = "ButtonAdvanceDate";
            this.ButtonAdvanceDate.Size = new System.Drawing.Size(198, 33);
            this.ButtonAdvanceDate.TabIndex = 81;
            this.ButtonAdvanceDate.Text = "Advance Date";
            this.toolTip.SetToolTip(this.ButtonAdvanceDate, "Advance Date performs one (1) time set.\r\n\r\nIf Stop Filters are defined, Advance D" +
        "ate\r\ncontinues advancing the date until a stop\r\nfilter has been hit.");
            this.ButtonAdvanceDate.UseVisualStyleBackColor = true;
            this.ButtonAdvanceDate.Click += new System.EventHandler(this.ButtonAdvanceDate_Click);
            // 
            // CheckEnableFilters
            // 
            this.CheckEnableFilters.AutoSize = true;
            this.CheckEnableFilters.Checked = true;
            this.CheckEnableFilters.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckEnableFilters.Location = new System.Drawing.Point(112, 190);
            this.CheckEnableFilters.Name = "CheckEnableFilters";
            this.CheckEnableFilters.Size = new System.Drawing.Size(95, 19);
            this.CheckEnableFilters.TabIndex = 119;
            this.CheckEnableFilters.Text = "Enable Filters";
            this.toolTip.SetToolTip(this.CheckEnableFilters, "Enable Filters enables or disables all filters\r\nentirely.\r\n\r\nEnabled - Advance Da" +
        "te will continue until\r\na match occurs from a filter.\r\n\r\nDisabled - Advance Date" +
        " will only advance\r\none (1) day.");
            this.CheckEnableFilters.UseVisualStyleBackColor = true;
            // 
            // ButtonDisconnect
            // 
            this.ButtonDisconnect.Enabled = false;
            this.ButtonDisconnect.Location = new System.Drawing.Point(112, 52);
            this.ButtonDisconnect.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ButtonDisconnect.Name = "ButtonDisconnect";
            this.ButtonDisconnect.Size = new System.Drawing.Size(97, 27);
            this.ButtonDisconnect.TabIndex = 11;
            this.ButtonDisconnect.Text = "Disconnect";
            this.ButtonDisconnect.UseVisualStyleBackColor = true;
            this.ButtonDisconnect.Click += new System.EventHandler(this.Disconnect_Click);
            // 
            // ButtonConnect
            // 
            this.ButtonConnect.Location = new System.Drawing.Point(8, 52);
            this.ButtonConnect.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ButtonConnect.Name = "ButtonConnect";
            this.ButtonConnect.Size = new System.Drawing.Size(97, 27);
            this.ButtonConnect.TabIndex = 10;
            this.ButtonConnect.Text = "Connect";
            this.ButtonConnect.UseVisualStyleBackColor = true;
            this.ButtonConnect.Click += new System.EventHandler(this.ButtonConnect_Click);
            // 
            // ConnectionStatusText
            // 
            this.ConnectionStatusText.AutoSize = true;
            this.ConnectionStatusText.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ConnectionStatusText.Location = new System.Drawing.Point(79, 35);
            this.ConnectionStatusText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ConnectionStatusText.Name = "ConnectionStatusText";
            this.ConnectionStatusText.Size = new System.Drawing.Size(83, 13);
            this.ConnectionStatusText.TabIndex = 9;
            this.ConnectionStatusText.Text = "Not connected";
            this.ConnectionStatusText.TextChanged += new System.EventHandler(this.ConnectionStatusText_TextChanged);
            // 
            // InputSwitchIP
            // 
            this.InputSwitchIP.Location = new System.Drawing.Point(79, 7);
            this.InputSwitchIP.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.InputSwitchIP.Name = "InputSwitchIP";
            this.InputSwitchIP.Size = new System.Drawing.Size(129, 23);
            this.InputSwitchIP.TabIndex = 8;
            this.InputSwitchIP.Text = "www.www.www.www";
            this.InputSwitchIP.TextChanged += new System.EventHandler(this.InputSwitchIP_Changed);
            // 
            // LabelStatus
            // 
            this.LabelStatus.AutoSize = true;
            this.LabelStatus.Location = new System.Drawing.Point(25, 33);
            this.LabelStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LabelStatus.Name = "LabelStatus";
            this.LabelStatus.Size = new System.Drawing.Size(42, 15);
            this.LabelStatus.TabIndex = 7;
            this.LabelStatus.Text = "Status:";
            // 
            // LabelSwitchIP
            // 
            this.LabelSwitchIP.AutoSize = true;
            this.LabelSwitchIP.Location = new System.Drawing.Point(8, 10);
            this.LabelSwitchIP.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LabelSwitchIP.Name = "LabelSwitchIP";
            this.LabelSwitchIP.Size = new System.Drawing.Size(58, 15);
            this.LabelSwitchIP.TabIndex = 6;
            this.LabelSwitchIP.Text = "Switch IP:";
            // 
            // LabelLoadedRaids
            // 
            this.LabelLoadedRaids.AutoSize = true;
            this.LabelLoadedRaids.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.LabelLoadedRaids.Location = new System.Drawing.Point(9, 6);
            this.LabelLoadedRaids.Name = "LabelLoadedRaids";
            this.LabelLoadedRaids.Size = new System.Drawing.Size(122, 15);
            this.LabelLoadedRaids.TabIndex = 12;
            this.LabelLoadedRaids.Text = "Raids Loaded/Shiny: -";
            this.LabelLoadedRaids.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // TeraType
            // 
            this.TeraType.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TeraType.Location = new System.Drawing.Point(248, 98);
            this.TeraType.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TeraType.Name = "TeraType";
            this.TeraType.ReadOnly = true;
            this.TeraType.Size = new System.Drawing.Size(97, 22);
            this.TeraType.TabIndex = 49;
            // 
            // LabelTeraType
            // 
            this.LabelTeraType.AutoSize = true;
            this.LabelTeraType.Location = new System.Drawing.Point(183, 100);
            this.LabelTeraType.Name = "LabelTeraType";
            this.LabelTeraType.Size = new System.Drawing.Size(58, 15);
            this.LabelTeraType.TabIndex = 48;
            this.LabelTeraType.Text = "Tera Type:";
            this.LabelTeraType.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // PID
            // 
            this.PID.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PID.Location = new System.Drawing.Point(75, 70);
            this.PID.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.PID.Name = "PID";
            this.PID.ReadOnly = true;
            this.PID.Size = new System.Drawing.Size(95, 22);
            this.PID.TabIndex = 47;
            // 
            // LabelPID
            // 
            this.LabelPID.AutoSize = true;
            this.LabelPID.Location = new System.Drawing.Point(40, 72);
            this.LabelPID.Name = "LabelPID";
            this.LabelPID.Size = new System.Drawing.Size(28, 15);
            this.LabelPID.TabIndex = 46;
            this.LabelPID.Text = "PID:";
            this.LabelPID.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // EC
            // 
            this.EC.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.EC.Location = new System.Drawing.Point(75, 42);
            this.EC.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.EC.Name = "EC";
            this.EC.ReadOnly = true;
            this.EC.Size = new System.Drawing.Size(95, 22);
            this.EC.TabIndex = 45;
            // 
            // LabelEC
            // 
            this.LabelEC.AutoSize = true;
            this.LabelEC.Location = new System.Drawing.Point(4, 34);
            this.LabelEC.MaximumSize = new System.Drawing.Size(70, 0);
            this.LabelEC.Name = "LabelEC";
            this.LabelEC.Size = new System.Drawing.Size(64, 30);
            this.LabelEC.TabIndex = 44;
            this.LabelEC.Text = "Encryption Constant:";
            this.LabelEC.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // Seed
            // 
            this.Seed.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Seed.Location = new System.Drawing.Point(75, 14);
            this.Seed.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Seed.Name = "Seed";
            this.Seed.ReadOnly = true;
            this.Seed.Size = new System.Drawing.Size(95, 22);
            this.Seed.TabIndex = 43;
            this.Seed.Click += new System.EventHandler(this.Seed_Clicked);
            // 
            // LabelSeed
            // 
            this.LabelSeed.AutoSize = true;
            this.LabelSeed.Location = new System.Drawing.Point(33, 16);
            this.LabelSeed.Name = "LabelSeed";
            this.LabelSeed.Size = new System.Drawing.Size(35, 15);
            this.LabelSeed.TabIndex = 42;
            this.LabelSeed.Text = "Seed:";
            this.LabelSeed.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ButtonNext
            // 
            this.ButtonNext.Enabled = false;
            this.ButtonNext.Location = new System.Drawing.Point(195, 3);
            this.ButtonNext.Name = "ButtonNext";
            this.ButtonNext.Size = new System.Drawing.Size(33, 27);
            this.ButtonNext.TabIndex = 56;
            this.ButtonNext.Text = ">>";
            this.ButtonNext.UseVisualStyleBackColor = true;
            this.ButtonNext.Click += new System.EventHandler(this.ButtonNext_Click);
            // 
            // ButtonPrevious
            // 
            this.ButtonPrevious.Enabled = false;
            this.ButtonPrevious.Location = new System.Drawing.Point(3, 3);
            this.ButtonPrevious.Name = "ButtonPrevious";
            this.ButtonPrevious.Size = new System.Drawing.Size(33, 27);
            this.ButtonPrevious.TabIndex = 55;
            this.ButtonPrevious.Text = "<<";
            this.ButtonPrevious.UseVisualStyleBackColor = true;
            this.ButtonPrevious.Click += new System.EventHandler(this.ButtonPrevious_Click);
            // 
            // Area
            // 
            this.Area.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Area.Location = new System.Drawing.Point(75, 291);
            this.Area.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Area.Name = "Area";
            this.Area.ReadOnly = true;
            this.Area.Size = new System.Drawing.Size(270, 22);
            this.Area.TabIndex = 61;
            this.Area.Click += new System.EventHandler(this.DisplayMap);
            // 
            // LabelUNK_2
            // 
            this.LabelUNK_2.AutoSize = true;
            this.LabelUNK_2.Location = new System.Drawing.Point(34, 295);
            this.LabelUNK_2.Name = "LabelUNK_2";
            this.LabelUNK_2.Size = new System.Drawing.Size(34, 15);
            this.LabelUNK_2.TabIndex = 60;
            this.LabelUNK_2.Text = "Area:";
            this.LabelUNK_2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // IVs
            // 
            this.IVs.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.IVs.Location = new System.Drawing.Point(75, 182);
            this.IVs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.IVs.Name = "IVs";
            this.IVs.ReadOnly = true;
            this.IVs.Size = new System.Drawing.Size(270, 22);
            this.IVs.TabIndex = 69;
            // 
            // LabelIVs
            // 
            this.LabelIVs.AutoSize = true;
            this.LabelIVs.Location = new System.Drawing.Point(43, 184);
            this.LabelIVs.Name = "LabelIVs";
            this.LabelIVs.Size = new System.Drawing.Size(25, 15);
            this.LabelIVs.TabIndex = 68;
            this.LabelIVs.Text = "IVs:";
            this.LabelIVs.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ButtonReadRaids
            // 
            this.ButtonReadRaids.Enabled = false;
            this.ButtonReadRaids.Location = new System.Drawing.Point(8, 85);
            this.ButtonReadRaids.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ButtonReadRaids.Name = "ButtonReadRaids";
            this.ButtonReadRaids.Size = new System.Drawing.Size(97, 27);
            this.ButtonReadRaids.TabIndex = 80;
            this.ButtonReadRaids.Text = "Read Raids";
            this.ButtonReadRaids.UseVisualStyleBackColor = true;
            this.ButtonReadRaids.Click += new System.EventHandler(this.ButtonReadRaids_Click);
            // 
            // LabelIsEvent
            // 
            this.LabelIsEvent.AutoSize = true;
            this.LabelIsEvent.BackColor = System.Drawing.Color.Transparent;
            this.LabelIsEvent.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.LabelIsEvent.ForeColor = System.Drawing.Color.DodgerBlue;
            this.LabelIsEvent.Location = new System.Drawing.Point(294, 66);
            this.LabelIsEvent.Name = "LabelIsEvent";
            this.LabelIsEvent.Size = new System.Drawing.Size(39, 15);
            this.LabelIsEvent.TabIndex = 84;
            this.LabelIsEvent.Text = "Event";
            this.LabelIsEvent.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // Difficulty
            // 
            this.Difficulty.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Difficulty.Location = new System.Drawing.Point(75, 98);
            this.Difficulty.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Difficulty.Name = "Difficulty";
            this.Difficulty.ReadOnly = true;
            this.Difficulty.Size = new System.Drawing.Size(80, 22);
            this.Difficulty.TabIndex = 86;
            // 
            // LabelDifficulty
            // 
            this.LabelDifficulty.AutoSize = true;
            this.LabelDifficulty.Location = new System.Drawing.Point(11, 101);
            this.LabelDifficulty.Name = "LabelDifficulty";
            this.LabelDifficulty.Size = new System.Drawing.Size(58, 15);
            this.LabelDifficulty.TabIndex = 85;
            this.LabelDifficulty.Text = "Difficulty:";
            this.LabelDifficulty.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // Progress
            // 
            this.Progress.FormattingEnabled = true;
            this.Progress.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.Progress.Location = new System.Drawing.Point(158, 282);
            this.Progress.Name = "Progress";
            this.Progress.Size = new System.Drawing.Size(48, 23);
            this.Progress.TabIndex = 87;
            this.Progress.Text = "w";
            this.Progress.SelectedIndexChanged += new System.EventHandler(this.Progress_SelectedIndexChanged);
            // 
            // LabelStoryProgress
            // 
            this.LabelStoryProgress.AutoSize = true;
            this.LabelStoryProgress.Location = new System.Drawing.Point(8, 285);
            this.LabelStoryProgress.Name = "LabelStoryProgress";
            this.LabelStoryProgress.Size = new System.Drawing.Size(115, 15);
            this.LabelStoryProgress.TabIndex = 88;
            this.LabelStoryProgress.Text = "Story Progress Level:";
            // 
            // ButtonViewRAM
            // 
            this.ButtonViewRAM.Enabled = false;
            this.ButtonViewRAM.Location = new System.Drawing.Point(112, 85);
            this.ButtonViewRAM.Name = "ButtonViewRAM";
            this.ButtonViewRAM.Size = new System.Drawing.Size(97, 27);
            this.ButtonViewRAM.TabIndex = 89;
            this.ButtonViewRAM.Text = "Dump Raid";
            this.ButtonViewRAM.UseVisualStyleBackColor = true;
            this.ButtonViewRAM.Click += new System.EventHandler(this.ViewRAM_Click);
            // 
            // Species
            // 
            this.Species.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Species.Location = new System.Drawing.Point(75, 126);
            this.Species.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Species.Name = "Species";
            this.Species.ReadOnly = true;
            this.Species.Size = new System.Drawing.Size(270, 22);
            this.Species.TabIndex = 93;
            // 
            // LabelSpecies
            // 
            this.LabelSpecies.AutoSize = true;
            this.LabelSpecies.Location = new System.Drawing.Point(19, 128);
            this.LabelSpecies.Name = "LabelSpecies";
            this.LabelSpecies.Size = new System.Drawing.Size(49, 15);
            this.LabelSpecies.TabIndex = 92;
            this.LabelSpecies.Text = "Species:";
            this.LabelSpecies.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // LabelMoves
            // 
            this.LabelMoves.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LabelMoves.AutoSize = true;
            this.LabelMoves.Location = new System.Drawing.Point(24, 255);
            this.LabelMoves.Name = "LabelMoves";
            this.LabelMoves.Size = new System.Drawing.Size(45, 15);
            this.LabelMoves.TabIndex = 94;
            this.LabelMoves.Text = "Moves:";
            this.LabelMoves.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // Move1
            // 
            this.Move1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Move1.Location = new System.Drawing.Point(75, 238);
            this.Move1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Move1.Name = "Move1";
            this.Move1.ReadOnly = true;
            this.Move1.Size = new System.Drawing.Size(133, 22);
            this.Move1.TabIndex = 95;
            this.Move1.Click += new System.EventHandler(this.Move_Clicked);
            // 
            // Move2
            // 
            this.Move2.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Move2.Location = new System.Drawing.Point(212, 238);
            this.Move2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Move2.Name = "Move2";
            this.Move2.ReadOnly = true;
            this.Move2.Size = new System.Drawing.Size(133, 22);
            this.Move2.TabIndex = 96;
            this.Move2.Click += new System.EventHandler(this.Move_Clicked);
            // 
            // Move4
            // 
            this.Move4.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Move4.Location = new System.Drawing.Point(212, 263);
            this.Move4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Move4.Name = "Move4";
            this.Move4.ReadOnly = true;
            this.Move4.Size = new System.Drawing.Size(133, 22);
            this.Move4.TabIndex = 98;
            this.Move4.Click += new System.EventHandler(this.Move_Clicked);
            // 
            // Move3
            // 
            this.Move3.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Move3.Location = new System.Drawing.Point(75, 263);
            this.Move3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Move3.Name = "Move3";
            this.Move3.ReadOnly = true;
            this.Move3.Size = new System.Drawing.Size(133, 22);
            this.Move3.TabIndex = 97;
            this.Move3.Click += new System.EventHandler(this.Move_Clicked);
            // 
            // LabelGame
            // 
            this.LabelGame.AutoSize = true;
            this.LabelGame.Location = new System.Drawing.Point(8, 258);
            this.LabelGame.Name = "LabelGame";
            this.LabelGame.Size = new System.Drawing.Size(41, 15);
            this.LabelGame.TabIndex = 100;
            this.LabelGame.Text = "Game:";
            // 
            // Game
            // 
            this.Game.FormattingEnabled = true;
            this.Game.Items.AddRange(new object[] {
            "Scarlet",
            "Violet"});
            this.Game.Location = new System.Drawing.Point(110, 255);
            this.Game.Name = "Game";
            this.Game.Size = new System.Drawing.Size(96, 23);
            this.Game.TabIndex = 99;
            this.Game.Text = "w";
            this.Game.SelectedIndexChanged += new System.EventHandler(this.Game_SelectedIndexChanged);
            // 
            // LabelEventProgress
            // 
            this.LabelEventProgress.AutoSize = true;
            this.LabelEventProgress.Location = new System.Drawing.Point(7, 313);
            this.LabelEventProgress.Name = "LabelEventProgress";
            this.LabelEventProgress.Size = new System.Drawing.Size(117, 15);
            this.LabelEventProgress.TabIndex = 102;
            this.LabelEventProgress.Text = "Event Progress Level:";
            // 
            // EventProgress
            // 
            this.EventProgress.FormattingEnabled = true;
            this.EventProgress.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.EventProgress.Location = new System.Drawing.Point(158, 311);
            this.EventProgress.Name = "EventProgress";
            this.EventProgress.Size = new System.Drawing.Size(48, 23);
            this.EventProgress.TabIndex = 101;
            this.EventProgress.Text = "w";
            this.EventProgress.SelectedIndexChanged += new System.EventHandler(this.EventProgress_SelectedIndexChanged);
            // 
            // Nature
            // 
            this.Nature.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Nature.Location = new System.Drawing.Point(248, 152);
            this.Nature.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Nature.Name = "Nature";
            this.Nature.ReadOnly = true;
            this.Nature.Size = new System.Drawing.Size(97, 22);
            this.Nature.TabIndex = 106;
            // 
            // LabelNature
            // 
            this.LabelNature.AutoSize = true;
            this.LabelNature.Location = new System.Drawing.Point(195, 154);
            this.LabelNature.Name = "LabelNature";
            this.LabelNature.Size = new System.Drawing.Size(46, 15);
            this.LabelNature.TabIndex = 105;
            this.LabelNature.Text = "Nature:";
            this.LabelNature.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // Gender
            // 
            this.Gender.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Gender.Location = new System.Drawing.Point(75, 154);
            this.Gender.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Gender.Name = "Gender";
            this.Gender.ReadOnly = true;
            this.Gender.Size = new System.Drawing.Size(95, 22);
            this.Gender.TabIndex = 104;
            // 
            // LabelGender
            // 
            this.LabelGender.AutoSize = true;
            this.LabelGender.Location = new System.Drawing.Point(21, 159);
            this.LabelGender.Name = "LabelGender";
            this.LabelGender.Size = new System.Drawing.Size(48, 15);
            this.LabelGender.TabIndex = 103;
            this.LabelGender.Text = "Gender:";
            this.LabelGender.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // StopFilter
            // 
            this.StopFilter.Location = new System.Drawing.Point(9, 187);
            this.StopFilter.Name = "StopFilter";
            this.StopFilter.Size = new System.Drawing.Size(97, 23);
            this.StopFilter.TabIndex = 107;
            this.StopFilter.Text = "Edit Filters";
            this.StopFilter.UseVisualStyleBackColor = true;
            this.StopFilter.Click += new System.EventHandler(this.StopFilter_Click);
            // 
            // Sprite
            // 
            this.Sprite.BackColor = System.Drawing.Color.Transparent;
            this.Sprite.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Sprite.Location = new System.Drawing.Point(279, 3);
            this.Sprite.Name = "Sprite";
            this.Sprite.Size = new System.Drawing.Size(68, 56);
            this.Sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.Sprite.TabIndex = 108;
            this.Sprite.TabStop = false;
            // 
            // Ability
            // 
            this.Ability.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Ability.Location = new System.Drawing.Point(75, 210);
            this.Ability.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Ability.Name = "Ability";
            this.Ability.ReadOnly = true;
            this.Ability.Size = new System.Drawing.Size(132, 22);
            this.Ability.TabIndex = 110;
            // 
            // LabelAbility
            // 
            this.LabelAbility.AutoSize = true;
            this.LabelAbility.Location = new System.Drawing.Point(24, 213);
            this.LabelAbility.Name = "LabelAbility";
            this.LabelAbility.Size = new System.Drawing.Size(44, 15);
            this.LabelAbility.TabIndex = 109;
            this.LabelAbility.Text = "Ability:";
            this.LabelAbility.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // GemIcon
            // 
            this.GemIcon.BackColor = System.Drawing.Color.Transparent;
            this.GemIcon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GemIcon.Location = new System.Drawing.Point(233, 19);
            this.GemIcon.Name = "GemIcon";
            this.GemIcon.Size = new System.Drawing.Size(40, 40);
            this.GemIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.GemIcon.TabIndex = 111;
            this.GemIcon.TabStop = false;
            // 
            // ButtonDownloadEvents
            // 
            this.ButtonDownloadEvents.Enabled = false;
            this.ButtonDownloadEvents.Location = new System.Drawing.Point(112, 118);
            this.ButtonDownloadEvents.Name = "ButtonDownloadEvents";
            this.ButtonDownloadEvents.Size = new System.Drawing.Size(97, 27);
            this.ButtonDownloadEvents.TabIndex = 112;
            this.ButtonDownloadEvents.Text = "Pull Events";
            this.ButtonDownloadEvents.UseVisualStyleBackColor = true;
            this.ButtonDownloadEvents.Click += new System.EventHandler(this.DownloadEvents_Click);
            // 
            // ConfigSettings
            // 
            this.ConfigSettings.Location = new System.Drawing.Point(7, 396);
            this.ConfigSettings.Name = "ConfigSettings";
            this.ConfigSettings.Size = new System.Drawing.Size(199, 33);
            this.ConfigSettings.TabIndex = 115;
            this.ConfigSettings.Text = "Open Settings";
            this.ConfigSettings.UseVisualStyleBackColor = true;
            this.ConfigSettings.Click += new System.EventHandler(this.ConfigSettings_Click);
            // 
            // Rewards
            // 
            this.Rewards.Location = new System.Drawing.Point(77, 308);
            this.Rewards.Name = "Rewards";
            this.Rewards.Size = new System.Drawing.Size(76, 23);
            this.Rewards.TabIndex = 116;
            this.Rewards.Text = "Rewards";
            this.Rewards.UseVisualStyleBackColor = true;
            this.Rewards.Click += new System.EventHandler(this.Rewards_Click);
            // 
            // LabelSandwichBonus
            // 
            this.LabelSandwichBonus.AutoSize = true;
            this.LabelSandwichBonus.Location = new System.Drawing.Point(7, 341);
            this.LabelSandwichBonus.Name = "LabelSandwichBonus";
            this.LabelSandwichBonus.Size = new System.Drawing.Size(120, 15);
            this.LabelSandwichBonus.TabIndex = 118;
            this.LabelSandwichBonus.Text = "Raid Sandwich Boost:";
            // 
            // RaidBoost
            // 
            this.RaidBoost.FormattingEnabled = true;
            this.RaidBoost.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3"});
            this.RaidBoost.Location = new System.Drawing.Point(158, 338);
            this.RaidBoost.Name = "RaidBoost";
            this.RaidBoost.Size = new System.Drawing.Size(48, 23);
            this.RaidBoost.TabIndex = 117;
            this.RaidBoost.Text = "w";
            this.RaidBoost.SelectedIndexChanged += new System.EventHandler(this.RaidBoost_SelectedIndexChanged);
            // 
            // ComboIndex
            // 
            this.ComboIndex.BackColor = System.Drawing.SystemColors.Window;
            this.ComboIndex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboIndex.FormattingEnabled = true;
            this.ComboIndex.Location = new System.Drawing.Point(38, 5);
            this.ComboIndex.Name = "ComboIndex";
            this.ComboIndex.Size = new System.Drawing.Size(155, 23);
            this.ComboIndex.TabIndex = 120;
            this.ComboIndex.SelectedIndexChanged += new System.EventHandler(this.ComboIndex_SelectedIndexChanged);
            // 
            // SendScreenshot
            // 
            this.SendScreenshot.Location = new System.Drawing.Point(9, 367);
            this.SendScreenshot.Name = "SendScreenshot";
            this.SendScreenshot.Size = new System.Drawing.Size(198, 23);
            this.SendScreenshot.TabIndex = 121;
            this.SendScreenshot.Text = "Screenshot Switch";
            this.SendScreenshot.UseVisualStyleBackColor = true;
            this.SendScreenshot.Click += new System.EventHandler(this.SendScreenshot_Click);
            // 
            // SearchTimer
            // 
            this.SearchTimer.Tick += new System.EventHandler(this.SearchTimer_Tick);
            // 
            // SearchTime
            // 
            this.SearchTime.AutoSize = true;
            this.SearchTime.Location = new System.Drawing.Point(49, 21);
            this.SearchTime.Name = "SearchTime";
            this.SearchTime.Size = new System.Drawing.Size(82, 15);
            this.SearchTime.TabIndex = 122;
            this.SearchTime.Text = "Search Time: -";
            this.SearchTime.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // DaySkipSuccessRate
            // 
            this.DaySkipSuccessRate.AutoSize = true;
            this.DaySkipSuccessRate.Location = new System.Drawing.Point(3, 35);
            this.DaySkipSuccessRate.Name = "DaySkipSuccessRate";
            this.DaySkipSuccessRate.Size = new System.Drawing.Size(128, 15);
            this.DaySkipSuccessRate.TabIndex = 123;
            this.DaySkipSuccessRate.Text = "Day skip success rate: -";
            this.DaySkipSuccessRate.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(233, 202);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 15);
            this.label1.TabIndex = 117;
            this.label1.Text = "Size:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ComboIndex);
            this.panel2.Controls.Add(this.ButtonPrevious);
            this.panel2.Controls.Add(this.ButtonNext);
            this.panel2.Location = new System.Drawing.Point(286, 5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(232, 33);
            this.panel2.TabIndex = 127;
            // 
            // Seed
            // 
            this.Seed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Seed.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Seed.Location = new System.Drawing.Point(77, 4);
            this.Seed.Name = "Seed";
            this.Seed.ReadOnly = true;
            this.Seed.Size = new System.Drawing.Size(95, 23);
            this.Seed.TabIndex = 119;
            this.Seed.Click += new System.EventHandler(this.Seed_Clicked);
            // 
            // myPanel1
            // 
            this.myPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.myPanel1.Controls.Add(this.Move4);
            this.myPanel1.Controls.Add(this.Move3);
            this.myPanel1.Controls.Add(this.Move2);
            this.myPanel1.Controls.Add(this.Move1);
            this.myPanel1.Controls.Add(this.Ability);
            this.myPanel1.Controls.Add(this.SizeBox);
            this.myPanel1.Controls.Add(this.Area);
            this.myPanel1.Controls.Add(this.IVs);
            this.myPanel1.Controls.Add(this.Species);
            this.myPanel1.Controls.Add(this.Nature);
            this.myPanel1.Controls.Add(this.Gender);
            this.myPanel1.Controls.Add(this.TeraType);
            this.myPanel1.Controls.Add(this.Difficulty);
            this.myPanel1.Controls.Add(this.PID);
            this.myPanel1.Controls.Add(this.EC);
            this.myPanel1.Controls.Add(this.Seed);
            this.myPanel1.Controls.Add(this.Sprite);
            this.myPanel1.Controls.Add(this.LabelEC);
            this.myPanel1.Controls.Add(this.LabelGender);
            this.myPanel1.Controls.Add(this.label1);
            this.myPanel1.Controls.Add(this.LabelPID);
            this.myPanel1.Controls.Add(this.GemIcon);
            this.myPanel1.Controls.Add(this.LabelNature);
            this.myPanel1.Controls.Add(this.LabelMoves);
            this.myPanel1.Controls.Add(this.LabelUNK_2);
            this.myPanel1.Controls.Add(this.LabelSeed);
            this.myPanel1.Controls.Add(this.LabelAbility);
            this.myPanel1.Controls.Add(this.LabelSpecies);
            this.myPanel1.Controls.Add(this.Rewards);
            this.myPanel1.Controls.Add(this.LabelIVs);
            this.myPanel1.Controls.Add(this.LabelTeraType);
            this.myPanel1.Controls.Add(this.LabelDifficulty);
            this.myPanel1.Controls.Add(this.LabelIsEvent);
            this.myPanel1.Location = new System.Drawing.Point(226, 43);
            this.myPanel1.Name = "myPanel1";
            this.myPanel1.Size = new System.Drawing.Size(350, 338);
            this.myPanel1.TabIndex = 130;
            // 
            // Move4
            // 
            this.Move4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Move4.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Move4.Location = new System.Drawing.Point(214, 253);
            this.Move4.Name = "Move4";
            this.Move4.ReadOnly = true;
            this.Move4.Size = new System.Drawing.Size(133, 23);
            this.Move4.TabIndex = 134;
            this.Move4.Click += new System.EventHandler(this.Move_Clicked);
            // 
            // Move3
            // 
            this.Move3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Move3.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Move3.Location = new System.Drawing.Point(77, 253);
            this.Move3.Name = "Move3";
            this.Move3.ReadOnly = true;
            this.Move3.Size = new System.Drawing.Size(133, 23);
            this.Move3.TabIndex = 133;
            this.Move3.Click += new System.EventHandler(this.Move_Clicked);
            // 
            // Move2
            // 
            this.Move2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Move2.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Move2.Location = new System.Drawing.Point(214, 226);
            this.Move2.Name = "Move2";
            this.Move2.ReadOnly = true;
            this.Move2.Size = new System.Drawing.Size(133, 23);
            this.Move2.TabIndex = 132;
            this.Move2.Click += new System.EventHandler(this.Move_Clicked);
            // 
            // Move1
            // 
            this.Move1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Move1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Move1.Location = new System.Drawing.Point(77, 226);
            this.Move1.Name = "Move1";
            this.Move1.ReadOnly = true;
            this.Move1.Size = new System.Drawing.Size(133, 23);
            this.Move1.TabIndex = 131;
            this.Move1.Click += new System.EventHandler(this.Move_Clicked);
            // 
            // Ability
            // 
            this.Ability.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Ability.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Ability.Location = new System.Drawing.Point(77, 199);
            this.Ability.Name = "Ability";
            this.Ability.ReadOnly = true;
            this.Ability.Size = new System.Drawing.Size(133, 23);
            this.Ability.TabIndex = 130;
            // 
            // SizeBox
            // 
            this.SizeBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.SizeBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.SizeBox.Location = new System.Drawing.Point(279, 199);
            this.SizeBox.Name = "SizeBox";
            this.SizeBox.ReadOnly = true;
            this.SizeBox.Size = new System.Drawing.Size(68, 23);
            this.SizeBox.TabIndex = 129;
            // 
            // Area
            // 
            this.Area.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Area.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Area.Location = new System.Drawing.Point(77, 280);
            this.Area.Name = "Area";
            this.Area.ReadOnly = true;
            this.Area.Size = new System.Drawing.Size(270, 23);
            this.Area.TabIndex = 128;
            this.Area.Click += new System.EventHandler(this.DisplayMap);
            // 
            // IVs
            // 
            this.IVs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.IVs.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.IVs.Location = new System.Drawing.Point(77, 171);
            this.IVs.Name = "IVs";
            this.IVs.ReadOnly = true;
            this.IVs.Size = new System.Drawing.Size(270, 23);
            this.IVs.TabIndex = 127;
            // 
            // Species
            // 
            this.Species.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Species.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Species.Location = new System.Drawing.Point(77, 115);
            this.Species.Name = "Species";
            this.Species.ReadOnly = true;
            this.Species.Size = new System.Drawing.Size(270, 23);
            this.Species.TabIndex = 126;
            // 
            // Nature
            // 
            this.Nature.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Nature.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Nature.Location = new System.Drawing.Point(252, 143);
            this.Nature.Name = "Nature";
            this.Nature.ReadOnly = true;
            this.Nature.Size = new System.Drawing.Size(95, 23);
            this.Nature.TabIndex = 125;
            // 
            // Gender
            // 
            this.Gender.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Gender.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Gender.Location = new System.Drawing.Point(77, 143);
            this.Gender.Name = "Gender";
            this.Gender.ReadOnly = true;
            this.Gender.Size = new System.Drawing.Size(95, 23);
            this.Gender.TabIndex = 124;
            // 
            // TeraType
            // 
            this.TeraType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.TeraType.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TeraType.Location = new System.Drawing.Point(252, 86);
            this.TeraType.Name = "TeraType";
            this.TeraType.ReadOnly = true;
            this.TeraType.Size = new System.Drawing.Size(95, 23);
            this.TeraType.TabIndex = 123;
            // 
            // Difficulty
            // 
            this.Difficulty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Difficulty.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Difficulty.Location = new System.Drawing.Point(77, 85);
            this.Difficulty.Name = "Difficulty";
            this.Difficulty.ReadOnly = true;
            this.Difficulty.Size = new System.Drawing.Size(95, 23);
            this.Difficulty.TabIndex = 122;
            // 
            // PID
            // 
            this.PID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.PID.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PID.Location = new System.Drawing.Point(77, 58);
            this.PID.Name = "PID";
            this.PID.ReadOnly = true;
            this.PID.Size = new System.Drawing.Size(95, 23);
            this.PID.TabIndex = 121;
            // 
            // EC
            // 
            this.EC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.EC.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.EC.Location = new System.Drawing.Point(77, 31);
            this.EC.Name = "EC";
            this.EC.ReadOnly = true;
            this.EC.Size = new System.Drawing.Size(95, 23);
            this.EC.TabIndex = 120;
            // 
            // myPanel2
            // 
            this.myPanel2.Controls.Add(this.LabelLoadedRaids);
            this.myPanel2.Controls.Add(this.DaySkipSuccessRate);
            this.myPanel2.Controls.Add(this.SearchTime);
            this.myPanel2.Location = new System.Drawing.Point(226, 389);
            this.myPanel2.Name = "myPanel2";
            this.myPanel2.Size = new System.Drawing.Size(350, 55);
            this.myPanel2.TabIndex = 131;
            // 
            // myPanel3
            // 
            this.myPanel3.Controls.Add(this.LabelSwitchIP);
            this.myPanel3.Controls.Add(this.ConfigSettings);
            this.myPanel3.Controls.Add(this.LabelStatus);
            this.myPanel3.Controls.Add(this.ButtonViewRAM);
            this.myPanel3.Controls.Add(this.SendScreenshot);
            this.myPanel3.Controls.Add(this.Game);
            this.myPanel3.Controls.Add(this.CheckEnableFilters);
            this.myPanel3.Controls.Add(this.LabelStoryProgress);
            this.myPanel3.Controls.Add(this.InputSwitchIP);
            this.myPanel3.Controls.Add(this.LabelGame);
            this.myPanel3.Controls.Add(this.LabelSandwichBonus);
            this.myPanel3.Controls.Add(this.Progress);
            this.myPanel3.Controls.Add(this.ConnectionStatusText);
            this.myPanel3.Controls.Add(this.EventProgress);
            this.myPanel3.Controls.Add(this.RaidBoost);
            this.myPanel3.Controls.Add(this.ButtonAdvanceDate);
            this.myPanel3.Controls.Add(this.StopFilter);
            this.myPanel3.Controls.Add(this.LabelEventProgress);
            this.myPanel3.Controls.Add(this.ButtonConnect);
            this.myPanel3.Controls.Add(this.ButtonReadRaids);
            this.myPanel3.Controls.Add(this.ButtonDownloadEvents);
            this.myPanel3.Controls.Add(this.ButtonDisconnect);
            this.myPanel3.Location = new System.Drawing.Point(8, 8);
            this.myPanel3.Name = "myPanel3";
            this.myPanel3.Size = new System.Drawing.Size(212, 436);
            this.myPanel3.TabIndex = 132;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 451);
            this.Controls.Add(this.myPanel3);
            this.Controls.Add(this.myPanel2);
            this.Controls.Add(this.myPanel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Sprite)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GemIcon)).EndInit();
            this.panel2.ResumeLayout(false);
            this.myPanel1.ResumeLayout(false);
            this.myPanel1.PerformLayout();
            this.myPanel2.ResumeLayout(false);
            this.myPanel2.PerformLayout();
            this.myPanel3.ResumeLayout(false);
            this.myPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ToolTip toolTip;
        private Button ButtonDisconnect;
        private Button ButtonConnect;
        private Label ConnectionStatusText;
        private TextBox InputSwitchIP;
        private Label LabelStatus;
        private Label LabelSwitchIP;
        private Label LabelLoadedRaids;
        private TextBox TeraType;
        private Label LabelTeraType;
        private TextBox PID;
        private Label LabelPID;
        private TextBox EC;
        private Label LabelEC;
        private TextBox Seed;
        private Label LabelSeed;
        private Button ButtonNext;
        private Button ButtonPrevious;
        private TextBox Area;
        private Label LabelUNK_2;
        private TextBox IVs;
        private Label LabelIVs;
        private Button ButtonReadRaids;
        private Button ButtonAdvanceDate;
        private Label LabelIsEvent;
        private TextBox Difficulty;
        private Label LabelDifficulty;
        private ComboBox Progress;
        private Label LabelStoryProgress;
        private Button ButtonViewRAM;
        private TextBox Species;
        private Label LabelSpecies;
        private Label LabelMoves;
        private TextBox Move1;
        private TextBox Move2;
        private TextBox Move4;
        private TextBox Move3;
        private Label LabelGame;
        private ComboBox Game;
        private Label LabelEventProgress;
        private ComboBox EventProgress;
        private TextBox Nature;
        private Label LabelNature;
        private TextBox Gender;
        private Label LabelGender;
        private Button StopFilter;
        private PictureBox Sprite;
        private TextBox Ability;
        private Label LabelAbility;
        private PictureBox GemIcon;
        private Button ButtonDownloadEvents;
        private Button ConfigSettings;
        private Button Rewards;
        private Label LabelSandwichBonus;
        private ComboBox RaidBoost;
        private CheckBox CheckEnableFilters;
        private ComboBox ComboIndex;
        private Button SendScreenshot;
        private System.Windows.Forms.Timer SearchTimer;
        private Label SearchTime;
        private int DaySkipTries = 0;
        private int DaySkipSuccess = 0;
        private Label DaySkipSuccessRate;
        private Panel panel2;
        private Label label1;
        private Structures.TransparentBackgroundTextBox Seed;
        private Structures.MyPanel myPanel1;
        private Structures.MyPanel myPanel2;
        private Structures.MyPanel myPanel3;
    }
}