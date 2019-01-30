namespace UnitTest
{
    partial class MAVLinkView
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.SerialPortNameComboBox = new System.Windows.Forms.ComboBox();
            this.SerialConnectButton = new System.Windows.Forms.Button();
            this.SerialPortNameLabel = new System.Windows.Forms.Label();
            this.ArmButton = new System.Windows.Forms.Button();
            this._SystemIdLabel = new System.Windows.Forms.Label();
            this._ComponentIdLabel = new System.Windows.Forms.Label();
            this.SystemIdLabel = new System.Windows.Forms.Label();
            this.ComponentIdLabel = new System.Windows.Forms.Label();
            this._PX4ModeLabel = new System.Windows.Forms.Label();
            this._PX4ModeDividerLabel = new System.Windows.Forms.Label();
            this.PX4ModeLabel = new System.Windows.Forms.Label();
            this.PX4SubModeLabel = new System.Windows.Forms.Label();
            this.TakeoffButton = new System.Windows.Forms.Button();
            this.DisarmButton = new System.Windows.Forms.Button();
            this.LandButton = new System.Windows.Forms.Button();
            this.MissionDataGridView = new System.Windows.Forms.DataGridView();
            this.MissionProgressBar = new System.Windows.Forms.ProgressBar();
            this._MissionLabel = new System.Windows.Forms.Label();
            this.MissionUploadButton = new System.Windows.Forms.Button();
            this.MissionDownloadButton = new System.Windows.Forms.Button();
            this.MissionClearButton = new System.Windows.Forms.Button();
            this.MissionStartButton = new System.Windows.Forms.Button();
            this._StatusMessageLabel = new System.Windows.Forms.Label();
            this._CommandResultMessageLabel = new System.Windows.Forms.Label();
            this.StatusMessageLabel = new System.Windows.Forms.Label();
            this.CommandResultMessageLabel = new System.Windows.Forms.Label();
            this._FlightModeLabel = new System.Windows.Forms.Label();
            this.FlightModeComboBox = new System.Windows.Forms.ComboBox();
            this.FlightModeButton = new System.Windows.Forms.Button();
            this._GlobalLocationLabel = new System.Windows.Forms.Label();
            this.LatitudeLabel = new System.Windows.Forms.Label();
            this.LongitudeLabel = new System.Windows.Forms.Label();
            this.AltitudeLabel = new System.Windows.Forms.Label();
            this._LocalPositionLabel = new System.Windows.Forms.Label();
            this.XLabel = new System.Windows.Forms.Label();
            this.YLabel = new System.Windows.Forms.Label();
            this.ZLabel = new System.Windows.Forms.Label();
            this._HomePositionLabel = new System.Windows.Forms.Label();
            this.HomeLatitudeLabel = new System.Windows.Forms.Label();
            this.HomeLongitudeLabel = new System.Windows.Forms.Label();
            this.HomeAltitudeLabel = new System.Windows.Forms.Label();
            this._BatteryPercentageLabel = new System.Windows.Forms.Label();
            this.BatteryPercentageLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.MissionDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // SerialPortNameComboBox
            // 
            this.SerialPortNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SerialPortNameComboBox.FormattingEnabled = true;
            this.SerialPortNameComboBox.Location = new System.Drawing.Point(101, 21);
            this.SerialPortNameComboBox.Name = "SerialPortNameComboBox";
            this.SerialPortNameComboBox.Size = new System.Drawing.Size(138, 20);
            this.SerialPortNameComboBox.TabIndex = 0;
            // 
            // SerialConnectButton
            // 
            this.SerialConnectButton.Location = new System.Drawing.Point(257, 21);
            this.SerialConnectButton.Name = "SerialConnectButton";
            this.SerialConnectButton.Size = new System.Drawing.Size(86, 23);
            this.SerialConnectButton.TabIndex = 1;
            this.SerialConnectButton.Text = "Connect";
            this.SerialConnectButton.UseVisualStyleBackColor = true;
            this.SerialConnectButton.Click += new System.EventHandler(this.SerialConnectButton_Click);
            // 
            // SerialPortNameLabel
            // 
            this.SerialPortNameLabel.AutoSize = true;
            this.SerialPortNameLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.SerialPortNameLabel.Location = new System.Drawing.Point(24, 24);
            this.SerialPortNameLabel.Name = "SerialPortNameLabel";
            this.SerialPortNameLabel.Size = new System.Drawing.Size(74, 12);
            this.SerialPortNameLabel.TabIndex = 2;
            this.SerialPortNameLabel.Text = "Serial Port";
            // 
            // ArmButton
            // 
            this.ArmButton.Location = new System.Drawing.Point(257, 63);
            this.ArmButton.Name = "ArmButton";
            this.ArmButton.Size = new System.Drawing.Size(86, 23);
            this.ArmButton.TabIndex = 3;
            this.ArmButton.Text = "Arm";
            this.ArmButton.UseVisualStyleBackColor = true;
            this.ArmButton.Click += new System.EventHandler(this.ArmButton_Click);
            // 
            // _SystemIdLabel
            // 
            this._SystemIdLabel.AutoSize = true;
            this._SystemIdLabel.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this._SystemIdLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this._SystemIdLabel.Location = new System.Drawing.Point(26, 68);
            this._SystemIdLabel.Name = "_SystemIdLabel";
            this._SystemIdLabel.Size = new System.Drawing.Size(82, 12);
            this._SystemIdLabel.TabIndex = 4;
            this._SystemIdLabel.Text = "System ID: ";
            // 
            // _ComponentIdLabel
            // 
            this._ComponentIdLabel.AutoSize = true;
            this._ComponentIdLabel.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this._ComponentIdLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this._ComponentIdLabel.Location = new System.Drawing.Point(26, 89);
            this._ComponentIdLabel.Name = "_ComponentIdLabel";
            this._ComponentIdLabel.Size = new System.Drawing.Size(107, 12);
            this._ComponentIdLabel.TabIndex = 5;
            this._ComponentIdLabel.Text = "Component ID: ";
            // 
            // SystemIdLabel
            // 
            this.SystemIdLabel.AutoSize = true;
            this.SystemIdLabel.Location = new System.Drawing.Point(133, 68);
            this.SystemIdLabel.Name = "SystemIdLabel";
            this.SystemIdLabel.Size = new System.Drawing.Size(12, 12);
            this.SystemIdLabel.TabIndex = 6;
            this.SystemIdLabel.Text = "0";
            // 
            // ComponentIdLabel
            // 
            this.ComponentIdLabel.AutoSize = true;
            this.ComponentIdLabel.Location = new System.Drawing.Point(133, 89);
            this.ComponentIdLabel.Name = "ComponentIdLabel";
            this.ComponentIdLabel.Size = new System.Drawing.Size(12, 12);
            this.ComponentIdLabel.TabIndex = 7;
            this.ComponentIdLabel.Text = "0";
            // 
            // _PX4ModeLabel
            // 
            this._PX4ModeLabel.AutoSize = true;
            this._PX4ModeLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this._PX4ModeLabel.Location = new System.Drawing.Point(24, 125);
            this._PX4ModeLabel.Name = "_PX4ModeLabel";
            this._PX4ModeLabel.Size = new System.Drawing.Size(51, 12);
            this._PX4ModeLabel.TabIndex = 8;
            this._PX4ModeLabel.Text = "Mode: ";
            // 
            // _PX4ModeDividerLabel
            // 
            this._PX4ModeDividerLabel.AutoSize = true;
            this._PX4ModeDividerLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this._PX4ModeDividerLabel.Location = new System.Drawing.Point(158, 125);
            this._PX4ModeDividerLabel.Name = "_PX4ModeDividerLabel";
            this._PX4ModeDividerLabel.Size = new System.Drawing.Size(19, 12);
            this._PX4ModeDividerLabel.TabIndex = 9;
            this._PX4ModeDividerLabel.Text = "||";
            // 
            // PX4ModeLabel
            // 
            this.PX4ModeLabel.AutoSize = true;
            this.PX4ModeLabel.Location = new System.Drawing.Point(81, 125);
            this.PX4ModeLabel.Name = "PX4ModeLabel";
            this.PX4ModeLabel.Size = new System.Drawing.Size(71, 12);
            this.PX4ModeLabel.TabIndex = 9;
            this.PX4ModeLabel.Text = "PX4 Mode";
            // 
            // PX4SubModeLabel
            // 
            this.PX4SubModeLabel.AutoSize = true;
            this.PX4SubModeLabel.Location = new System.Drawing.Point(183, 125);
            this.PX4SubModeLabel.Name = "PX4SubModeLabel";
            this.PX4SubModeLabel.Size = new System.Drawing.Size(96, 12);
            this.PX4SubModeLabel.TabIndex = 10;
            this.PX4SubModeLabel.Text = "PX4 Submode";
            // 
            // TakeoffButton
            // 
            this.TakeoffButton.Location = new System.Drawing.Point(362, 63);
            this.TakeoffButton.Name = "TakeoffButton";
            this.TakeoffButton.Size = new System.Drawing.Size(86, 23);
            this.TakeoffButton.TabIndex = 11;
            this.TakeoffButton.Text = "Takeoff";
            this.TakeoffButton.UseVisualStyleBackColor = true;
            this.TakeoffButton.Click += new System.EventHandler(this.TakeoffButton_Click);
            // 
            // DisarmButton
            // 
            this.DisarmButton.Location = new System.Drawing.Point(257, 92);
            this.DisarmButton.Name = "DisarmButton";
            this.DisarmButton.Size = new System.Drawing.Size(86, 23);
            this.DisarmButton.TabIndex = 12;
            this.DisarmButton.Text = "Disarm";
            this.DisarmButton.UseVisualStyleBackColor = true;
            this.DisarmButton.Click += new System.EventHandler(this.DisarmButton_Click);
            // 
            // LandButton
            // 
            this.LandButton.Location = new System.Drawing.Point(362, 92);
            this.LandButton.Name = "LandButton";
            this.LandButton.Size = new System.Drawing.Size(86, 23);
            this.LandButton.TabIndex = 13;
            this.LandButton.Text = "Land";
            this.LandButton.UseVisualStyleBackColor = true;
            this.LandButton.Click += new System.EventHandler(this.LandButton_Click);
            // 
            // MissionDataGridView
            // 
            this.MissionDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MissionDataGridView.Location = new System.Drawing.Point(26, 253);
            this.MissionDataGridView.Name = "MissionDataGridView";
            this.MissionDataGridView.RowTemplate.Height = 23;
            this.MissionDataGridView.Size = new System.Drawing.Size(240, 150);
            this.MissionDataGridView.TabIndex = 14;
            // 
            // MissionProgressBar
            // 
            this.MissionProgressBar.Location = new System.Drawing.Point(26, 412);
            this.MissionProgressBar.Name = "MissionProgressBar";
            this.MissionProgressBar.Size = new System.Drawing.Size(342, 23);
            this.MissionProgressBar.TabIndex = 15;
            // 
            // _MissionLabel
            // 
            this._MissionLabel.AutoSize = true;
            this._MissionLabel.ForeColor = System.Drawing.Color.Red;
            this._MissionLabel.Location = new System.Drawing.Point(28, 222);
            this._MissionLabel.Name = "_MissionLabel";
            this._MissionLabel.Size = new System.Drawing.Size(57, 12);
            this._MissionLabel.TabIndex = 16;
            this._MissionLabel.Text = "Mission";
            // 
            // MissionUploadButton
            // 
            this.MissionUploadButton.Location = new System.Drawing.Point(282, 277);
            this.MissionUploadButton.Name = "MissionUploadButton";
            this.MissionUploadButton.Size = new System.Drawing.Size(86, 23);
            this.MissionUploadButton.TabIndex = 17;
            this.MissionUploadButton.Text = "Upload";
            this.MissionUploadButton.UseVisualStyleBackColor = true;
            // 
            // MissionDownloadButton
            // 
            this.MissionDownloadButton.Location = new System.Drawing.Point(282, 318);
            this.MissionDownloadButton.Name = "MissionDownloadButton";
            this.MissionDownloadButton.Size = new System.Drawing.Size(86, 23);
            this.MissionDownloadButton.TabIndex = 18;
            this.MissionDownloadButton.Text = "Download";
            this.MissionDownloadButton.UseVisualStyleBackColor = true;
            // 
            // MissionClearButton
            // 
            this.MissionClearButton.Location = new System.Drawing.Point(282, 358);
            this.MissionClearButton.Name = "MissionClearButton";
            this.MissionClearButton.Size = new System.Drawing.Size(86, 23);
            this.MissionClearButton.TabIndex = 19;
            this.MissionClearButton.Text = "Clear";
            this.MissionClearButton.UseVisualStyleBackColor = true;
            // 
            // MissionStartButton
            // 
            this.MissionStartButton.Location = new System.Drawing.Point(104, 217);
            this.MissionStartButton.Name = "MissionStartButton";
            this.MissionStartButton.Size = new System.Drawing.Size(162, 23);
            this.MissionStartButton.TabIndex = 20;
            this.MissionStartButton.Text = "Start mission";
            this.MissionStartButton.UseVisualStyleBackColor = true;
            // 
            // _StatusMessageLabel
            // 
            this._StatusMessageLabel.AutoSize = true;
            this._StatusMessageLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this._StatusMessageLabel.Location = new System.Drawing.Point(511, 323);
            this._StatusMessageLabel.Name = "_StatusMessageLabel";
            this._StatusMessageLabel.Size = new System.Drawing.Size(56, 12);
            this._StatusMessageLabel.TabIndex = 21;
            this._StatusMessageLabel.Text = "Status: ";
            // 
            // _CommandResultMessageLabel
            // 
            this._CommandResultMessageLabel.AutoSize = true;
            this._CommandResultMessageLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this._CommandResultMessageLabel.Location = new System.Drawing.Point(511, 369);
            this._CommandResultMessageLabel.Name = "_CommandResultMessageLabel";
            this._CommandResultMessageLabel.Size = new System.Drawing.Size(127, 12);
            this._CommandResultMessageLabel.TabIndex = 22;
            this._CommandResultMessageLabel.Text = "Command Result: ";
            // 
            // StatusMessageLabel
            // 
            this.StatusMessageLabel.AutoSize = true;
            this.StatusMessageLabel.Location = new System.Drawing.Point(511, 346);
            this.StatusMessageLabel.Name = "StatusMessageLabel";
            this.StatusMessageLabel.Size = new System.Drawing.Size(134, 12);
            this.StatusMessageLabel.TabIndex = 23;
            this.StatusMessageLabel.Text = "__StatusMessage__";
            // 
            // CommandResultMessageLabel
            // 
            this.CommandResultMessageLabel.AutoSize = true;
            this.CommandResultMessageLabel.Location = new System.Drawing.Point(511, 391);
            this.CommandResultMessageLabel.Name = "CommandResultMessageLabel";
            this.CommandResultMessageLabel.Size = new System.Drawing.Size(200, 12);
            this.CommandResultMessageLabel.TabIndex = 24;
            this.CommandResultMessageLabel.Text = "__CommandResultMessage__";
            // 
            // _FlightModeLabel
            // 
            this._FlightModeLabel.AutoSize = true;
            this._FlightModeLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this._FlightModeLabel.Location = new System.Drawing.Point(22, 163);
            this._FlightModeLabel.Name = "_FlightModeLabel";
            this._FlightModeLabel.Size = new System.Drawing.Size(92, 12);
            this._FlightModeLabel.TabIndex = 26;
            this._FlightModeLabel.Text = "Flight Mode: ";
            // 
            // FlightModeComboBox
            // 
            this.FlightModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FlightModeComboBox.FormattingEnabled = true;
            this.FlightModeComboBox.Location = new System.Drawing.Point(119, 160);
            this.FlightModeComboBox.Name = "FlightModeComboBox";
            this.FlightModeComboBox.Size = new System.Drawing.Size(120, 20);
            this.FlightModeComboBox.TabIndex = 25;
            // 
            // FlightModeButton
            // 
            this.FlightModeButton.Location = new System.Drawing.Point(257, 160);
            this.FlightModeButton.Name = "FlightModeButton";
            this.FlightModeButton.Size = new System.Drawing.Size(86, 23);
            this.FlightModeButton.TabIndex = 27;
            this.FlightModeButton.Text = "set mode";
            this.FlightModeButton.UseVisualStyleBackColor = true;
            // 
            // _GlobalLocationLabel
            // 
            this._GlobalLocationLabel.AutoSize = true;
            this._GlobalLocationLabel.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this._GlobalLocationLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this._GlobalLocationLabel.Location = new System.Drawing.Point(510, 63);
            this._GlobalLocationLabel.Name = "_GlobalLocationLabel";
            this._GlobalLocationLabel.Size = new System.Drawing.Size(57, 12);
            this._GlobalLocationLabel.TabIndex = 29;
            this._GlobalLocationLabel.Text = "Global: ";
            // 
            // LatitudeLabel
            // 
            this.LatitudeLabel.AutoSize = true;
            this.LatitudeLabel.Location = new System.Drawing.Point(591, 63);
            this.LatitudeLabel.Name = "LatitudeLabel";
            this.LatitudeLabel.Size = new System.Drawing.Size(53, 12);
            this.LatitudeLabel.TabIndex = 28;
            this.LatitudeLabel.Text = "latitude";
            // 
            // LongitudeLabel
            // 
            this.LongitudeLabel.AutoSize = true;
            this.LongitudeLabel.Location = new System.Drawing.Point(684, 63);
            this.LongitudeLabel.Name = "LongitudeLabel";
            this.LongitudeLabel.Size = new System.Drawing.Size(65, 12);
            this.LongitudeLabel.TabIndex = 30;
            this.LongitudeLabel.Text = "longitude";
            // 
            // AltitudeLabel
            // 
            this.AltitudeLabel.AutoSize = true;
            this.AltitudeLabel.Location = new System.Drawing.Point(789, 63);
            this.AltitudeLabel.Name = "AltitudeLabel";
            this.AltitudeLabel.Size = new System.Drawing.Size(53, 12);
            this.AltitudeLabel.TabIndex = 31;
            this.AltitudeLabel.Text = "altitude";
            // 
            // _LocalPositionLabel
            // 
            this._LocalPositionLabel.AutoSize = true;
            this._LocalPositionLabel.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this._LocalPositionLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this._LocalPositionLabel.Location = new System.Drawing.Point(510, 92);
            this._LocalPositionLabel.Name = "_LocalPositionLabel";
            this._LocalPositionLabel.Size = new System.Drawing.Size(51, 12);
            this._LocalPositionLabel.TabIndex = 35;
            this._LocalPositionLabel.Text = "Local: ";
            // 
            // XLabel
            // 
            this.XLabel.AutoSize = true;
            this.XLabel.Location = new System.Drawing.Point(591, 92);
            this.XLabel.Name = "XLabel";
            this.XLabel.Size = new System.Drawing.Size(14, 12);
            this.XLabel.TabIndex = 34;
            this.XLabel.Text = "X";
            // 
            // YLabel
            // 
            this.YLabel.AutoSize = true;
            this.YLabel.Location = new System.Drawing.Point(684, 92);
            this.YLabel.Name = "YLabel";
            this.YLabel.Size = new System.Drawing.Size(14, 12);
            this.YLabel.TabIndex = 33;
            this.YLabel.Text = "Y";
            // 
            // ZLabel
            // 
            this.ZLabel.AutoSize = true;
            this.ZLabel.Location = new System.Drawing.Point(789, 92);
            this.ZLabel.Name = "ZLabel";
            this.ZLabel.Size = new System.Drawing.Size(14, 12);
            this.ZLabel.TabIndex = 32;
            this.ZLabel.Text = "Z";
            // 
            // _HomePositionLabel
            // 
            this._HomePositionLabel.AutoSize = true;
            this._HomePositionLabel.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this._HomePositionLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this._HomePositionLabel.Location = new System.Drawing.Point(510, 125);
            this._HomePositionLabel.Name = "_HomePositionLabel";
            this._HomePositionLabel.Size = new System.Drawing.Size(52, 12);
            this._HomePositionLabel.TabIndex = 39;
            this._HomePositionLabel.Text = "Home: ";
            // 
            // HomeLatitudeLabel
            // 
            this.HomeLatitudeLabel.AutoSize = true;
            this.HomeLatitudeLabel.Location = new System.Drawing.Point(591, 125);
            this.HomeLatitudeLabel.Name = "HomeLatitudeLabel";
            this.HomeLatitudeLabel.Size = new System.Drawing.Size(64, 12);
            this.HomeLatitudeLabel.TabIndex = 38;
            this.HomeLatitudeLabel.Text = "home_lat";
            // 
            // HomeLongitudeLabel
            // 
            this.HomeLongitudeLabel.AutoSize = true;
            this.HomeLongitudeLabel.Location = new System.Drawing.Point(684, 125);
            this.HomeLongitudeLabel.Name = "HomeLongitudeLabel";
            this.HomeLongitudeLabel.Size = new System.Drawing.Size(68, 12);
            this.HomeLongitudeLabel.TabIndex = 37;
            this.HomeLongitudeLabel.Text = "home_lon";
            // 
            // HomeAltitudeLabel
            // 
            this.HomeAltitudeLabel.AutoSize = true;
            this.HomeAltitudeLabel.Location = new System.Drawing.Point(789, 125);
            this.HomeAltitudeLabel.Name = "HomeAltitudeLabel";
            this.HomeAltitudeLabel.Size = new System.Drawing.Size(64, 12);
            this.HomeAltitudeLabel.TabIndex = 36;
            this.HomeAltitudeLabel.Text = "home_alt";
            // 
            // _BatteryPercentageLabel
            // 
            this._BatteryPercentageLabel.AutoSize = true;
            this._BatteryPercentageLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this._BatteryPercentageLabel.Location = new System.Drawing.Point(511, 265);
            this._BatteryPercentageLabel.Name = "_BatteryPercentageLabel";
            this._BatteryPercentageLabel.Size = new System.Drawing.Size(84, 12);
            this._BatteryPercentageLabel.TabIndex = 41;
            this._BatteryPercentageLabel.Text = "Battery(%): ";
            // 
            // BatteryPercentageLabel
            // 
            this.BatteryPercentageLabel.AutoSize = true;
            this.BatteryPercentageLabel.Location = new System.Drawing.Point(601, 265);
            this.BatteryPercentageLabel.Name = "BatteryPercentageLabel";
            this.BatteryPercentageLabel.Size = new System.Drawing.Size(19, 12);
            this.BatteryPercentageLabel.TabIndex = 40;
            this.BatteryPercentageLabel.Text = "-1";
            // 
            // MAVLinkView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(950, 450);
            this.Controls.Add(this._BatteryPercentageLabel);
            this.Controls.Add(this.BatteryPercentageLabel);
            this.Controls.Add(this._HomePositionLabel);
            this.Controls.Add(this.HomeLatitudeLabel);
            this.Controls.Add(this.HomeLongitudeLabel);
            this.Controls.Add(this.HomeAltitudeLabel);
            this.Controls.Add(this._LocalPositionLabel);
            this.Controls.Add(this.XLabel);
            this.Controls.Add(this.YLabel);
            this.Controls.Add(this.ZLabel);
            this.Controls.Add(this.AltitudeLabel);
            this.Controls.Add(this.LongitudeLabel);
            this.Controls.Add(this._GlobalLocationLabel);
            this.Controls.Add(this.LatitudeLabel);
            this.Controls.Add(this.FlightModeButton);
            this.Controls.Add(this._FlightModeLabel);
            this.Controls.Add(this.FlightModeComboBox);
            this.Controls.Add(this.CommandResultMessageLabel);
            this.Controls.Add(this.StatusMessageLabel);
            this.Controls.Add(this._CommandResultMessageLabel);
            this.Controls.Add(this._StatusMessageLabel);
            this.Controls.Add(this.MissionStartButton);
            this.Controls.Add(this.MissionClearButton);
            this.Controls.Add(this.MissionDownloadButton);
            this.Controls.Add(this.MissionUploadButton);
            this.Controls.Add(this._MissionLabel);
            this.Controls.Add(this.MissionProgressBar);
            this.Controls.Add(this.MissionDataGridView);
            this.Controls.Add(this.LandButton);
            this.Controls.Add(this.DisarmButton);
            this.Controls.Add(this.TakeoffButton);
            this.Controls.Add(this.PX4SubModeLabel);
            this.Controls.Add(this.PX4ModeLabel);
            this.Controls.Add(this._PX4ModeDividerLabel);
            this.Controls.Add(this._PX4ModeLabel);
            this.Controls.Add(this.ComponentIdLabel);
            this.Controls.Add(this.SystemIdLabel);
            this.Controls.Add(this._ComponentIdLabel);
            this.Controls.Add(this._SystemIdLabel);
            this.Controls.Add(this.ArmButton);
            this.Controls.Add(this.SerialPortNameLabel);
            this.Controls.Add(this.SerialConnectButton);
            this.Controls.Add(this.SerialPortNameComboBox);
            this.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "MAVLinkView";
            this.Text = "MAVLinkView - UnitTest";
            this.Load += new System.EventHandler(this.MAVLinkView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.MissionDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox SerialPortNameComboBox;
        private System.Windows.Forms.Button SerialConnectButton;
        private System.Windows.Forms.Label SerialPortNameLabel;
        private System.Windows.Forms.Button ArmButton;
        private System.Windows.Forms.Label _SystemIdLabel;
        private System.Windows.Forms.Label _ComponentIdLabel;
        private System.Windows.Forms.Label SystemIdLabel;
        private System.Windows.Forms.Label ComponentIdLabel;
        private System.Windows.Forms.Label _PX4ModeLabel;
        private System.Windows.Forms.Label _PX4ModeDividerLabel;
        private System.Windows.Forms.Label PX4ModeLabel;
        private System.Windows.Forms.Label PX4SubModeLabel;
        private System.Windows.Forms.Button TakeoffButton;
        private System.Windows.Forms.Button DisarmButton;
        private System.Windows.Forms.Button LandButton;
        private System.Windows.Forms.DataGridView MissionDataGridView;
        private System.Windows.Forms.ProgressBar MissionProgressBar;
        private System.Windows.Forms.Label _MissionLabel;
        private System.Windows.Forms.Button MissionUploadButton;
        private System.Windows.Forms.Button MissionDownloadButton;
        private System.Windows.Forms.Button MissionClearButton;
        private System.Windows.Forms.Button MissionStartButton;
        private System.Windows.Forms.Label _StatusMessageLabel;
        private System.Windows.Forms.Label _CommandResultMessageLabel;
        private System.Windows.Forms.Label StatusMessageLabel;
        private System.Windows.Forms.Label CommandResultMessageLabel;
        private System.Windows.Forms.Label _FlightModeLabel;
        private System.Windows.Forms.ComboBox FlightModeComboBox;
        private System.Windows.Forms.Button FlightModeButton;
        private System.Windows.Forms.Label _GlobalLocationLabel;
        private System.Windows.Forms.Label LatitudeLabel;
        private System.Windows.Forms.Label LongitudeLabel;
        private System.Windows.Forms.Label AltitudeLabel;
        private System.Windows.Forms.Label _LocalPositionLabel;
        private System.Windows.Forms.Label XLabel;
        private System.Windows.Forms.Label YLabel;
        private System.Windows.Forms.Label ZLabel;
        private System.Windows.Forms.Label _HomePositionLabel;
        private System.Windows.Forms.Label HomeLatitudeLabel;
        private System.Windows.Forms.Label HomeLongitudeLabel;
        private System.Windows.Forms.Label HomeAltitudeLabel;
        private System.Windows.Forms.Label _BatteryPercentageLabel;
        private System.Windows.Forms.Label BatteryPercentageLabel;
        private System.Windows.Forms.BindingSource MissionItemsBindingSource;
    }
}

