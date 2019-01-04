﻿namespace MAVLink.NET
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resource being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.TakeoffButton = new System.Windows.Forms.Button();
            this.LandButton = new System.Windows.Forms.Button();
            this.ArmButton = new System.Windows.Forms.Button();
            this.DisarmButton = new System.Windows.Forms.Button();
            this.LatitudeLabel = new System.Windows.Forms.Label();
            this.LongitudeLabel = new System.Windows.Forms.Label();
            this.MissionUploadButton = new System.Windows.Forms.Button();
            this.StatusMessageLabel = new System.Windows.Forms.Label();
            this.ClearMissionButton = new System.Windows.Forms.Button();
            this.CommandResultMessageLabel = new System.Windows.Forms.Label();
            this.MissionStartButton = new System.Windows.Forms.Button();
            this.FlightModeTag = new System.Windows.Forms.Label();
            this.FlightModeLabel = new System.Windows.Forms.Label();
            this.FlightModeComboBox = new System.Windows.Forms.ComboBox();
            this.FlightModeButton = new System.Windows.Forms.Button();
            this.LatitudeTag = new System.Windows.Forms.Label();
            this.LongitudeTag = new System.Windows.Forms.Label();
            this.SubModeTag = new System.Windows.Forms.Label();
            this.SubModeLabel = new System.Windows.Forms.Label();
            this.RollTag = new System.Windows.Forms.Label();
            this.PitchTag = new System.Windows.Forms.Label();
            this.YawTag = new System.Windows.Forms.Label();
            this.RollLabel = new System.Windows.Forms.Label();
            this.PitchLabel = new System.Windows.Forms.Label();
            this.YawLabel = new System.Windows.Forms.Label();
            this.BatteryTag = new System.Windows.Forms.Label();
            this.BatteryLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // TakeoffButton
            // 
            this.TakeoffButton.Location = new System.Drawing.Point(166, 238);
            this.TakeoffButton.Name = "TakeoffButton";
            this.TakeoffButton.Size = new System.Drawing.Size(164, 23);
            this.TakeoffButton.TabIndex = 0;
            this.TakeoffButton.Text = "Takeoff";
            this.TakeoffButton.UseVisualStyleBackColor = true;
            this.TakeoffButton.Click += new System.EventHandler(this.TakeoffButton_Click);
            // 
            // LandButton
            // 
            this.LandButton.Location = new System.Drawing.Point(362, 238);
            this.LandButton.Name = "LandButton";
            this.LandButton.Size = new System.Drawing.Size(164, 23);
            this.LandButton.TabIndex = 1;
            this.LandButton.Text = "Land";
            this.LandButton.UseVisualStyleBackColor = true;
            this.LandButton.Click += new System.EventHandler(this.LandButton_Click);
            // 
            // ArmButton
            // 
            this.ArmButton.Location = new System.Drawing.Point(166, 209);
            this.ArmButton.Name = "ArmButton";
            this.ArmButton.Size = new System.Drawing.Size(164, 23);
            this.ArmButton.TabIndex = 2;
            this.ArmButton.Text = "Arm";
            this.ArmButton.UseVisualStyleBackColor = true;
            this.ArmButton.Click += new System.EventHandler(this.ArmButton_Click);
            // 
            // DisarmButton
            // 
            this.DisarmButton.Location = new System.Drawing.Point(362, 209);
            this.DisarmButton.Name = "DisarmButton";
            this.DisarmButton.Size = new System.Drawing.Size(164, 23);
            this.DisarmButton.TabIndex = 3;
            this.DisarmButton.Text = "Disarm";
            this.DisarmButton.UseVisualStyleBackColor = true;
            this.DisarmButton.Click += new System.EventHandler(this.DisarmButton_Click);
            // 
            // LatitudeLabel
            // 
            this.LatitudeLabel.AutoSize = true;
            this.LatitudeLabel.Location = new System.Drawing.Point(202, 277);
            this.LatitudeLabel.Name = "LatitudeLabel";
            this.LatitudeLabel.Size = new System.Drawing.Size(80, 12);
            this.LatitudeLabel.TabIndex = 4;
            this.LatitudeLabel.Text = "LatitudeLabel";
            // 
            // LongitudeLabel
            // 
            this.LongitudeLabel.AutoSize = true;
            this.LongitudeLabel.Location = new System.Drawing.Point(338, 277);
            this.LongitudeLabel.Name = "LongitudeLabel";
            this.LongitudeLabel.Size = new System.Drawing.Size(91, 12);
            this.LongitudeLabel.TabIndex = 5;
            this.LongitudeLabel.Text = "LongitudeLabel";
            // 
            // MissionUploadButton
            // 
            this.MissionUploadButton.Location = new System.Drawing.Point(168, 118);
            this.MissionUploadButton.Name = "MissionUploadButton";
            this.MissionUploadButton.Size = new System.Drawing.Size(358, 23);
            this.MissionUploadButton.TabIndex = 6;
            this.MissionUploadButton.Text = "Mission Upload";
            this.MissionUploadButton.UseVisualStyleBackColor = true;
            this.MissionUploadButton.Click += new System.EventHandler(this.MissionUploadButton_Click);
            // 
            // StatusMessageLabel
            // 
            this.StatusMessageLabel.AutoSize = true;
            this.StatusMessageLabel.Location = new System.Drawing.Point(164, 358);
            this.StatusMessageLabel.Name = "StatusMessageLabel";
            this.StatusMessageLabel.Size = new System.Drawing.Size(124, 12);
            this.StatusMessageLabel.TabIndex = 7;
            this.StatusMessageLabel.Text = "StatusMessageLabel";
            // 
            // ClearMissionButton
            // 
            this.ClearMissionButton.Location = new System.Drawing.Point(168, 147);
            this.ClearMissionButton.Name = "ClearMissionButton";
            this.ClearMissionButton.Size = new System.Drawing.Size(358, 23);
            this.ClearMissionButton.TabIndex = 8;
            this.ClearMissionButton.Text = "Clear Mission";
            this.ClearMissionButton.UseVisualStyleBackColor = true;
            this.ClearMissionButton.Click += new System.EventHandler(this.ClearMissionButton_Click);
            // 
            // CommandResultMessageLabel
            // 
            this.CommandResultMessageLabel.AutoSize = true;
            this.CommandResultMessageLabel.Location = new System.Drawing.Point(164, 380);
            this.CommandResultMessageLabel.Name = "CommandResultMessageLabel";
            this.CommandResultMessageLabel.Size = new System.Drawing.Size(183, 12);
            this.CommandResultMessageLabel.TabIndex = 9;
            this.CommandResultMessageLabel.Text = "CommandResultMessageLabel";
            // 
            // MissionStartButton
            // 
            this.MissionStartButton.Location = new System.Drawing.Point(168, 90);
            this.MissionStartButton.Name = "MissionStartButton";
            this.MissionStartButton.Size = new System.Drawing.Size(358, 23);
            this.MissionStartButton.TabIndex = 10;
            this.MissionStartButton.Text = "Mission Start";
            this.MissionStartButton.UseVisualStyleBackColor = true;
            this.MissionStartButton.Click += new System.EventHandler(this.MissionStartButton_Click);
            // 
            // FlightModeTag
            // 
            this.FlightModeTag.AutoSize = true;
            this.FlightModeTag.Location = new System.Drawing.Point(166, 303);
            this.FlightModeTag.Name = "FlightModeTag";
            this.FlightModeTag.Size = new System.Drawing.Size(75, 12);
            this.FlightModeTag.TabIndex = 11;
            this.FlightModeTag.Text = "FlightMode: ";
            // 
            // FlightModeLabel
            // 
            this.FlightModeLabel.AutoSize = true;
            this.FlightModeLabel.ForeColor = System.Drawing.Color.Red;
            this.FlightModeLabel.Location = new System.Drawing.Point(247, 303);
            this.FlightModeLabel.Name = "FlightModeLabel";
            this.FlightModeLabel.Size = new System.Drawing.Size(98, 12);
            this.FlightModeLabel.TabIndex = 12;
            this.FlightModeLabel.Text = "FlightModeLabel";
            // 
            // FlightModeComboBox
            // 
            this.FlightModeComboBox.FormattingEnabled = true;
            this.FlightModeComboBox.Location = new System.Drawing.Point(362, 300);
            this.FlightModeComboBox.Name = "FlightModeComboBox";
            this.FlightModeComboBox.Size = new System.Drawing.Size(121, 20);
            this.FlightModeComboBox.TabIndex = 13;
            // 
            // FlightModeButton
            // 
            this.FlightModeButton.Location = new System.Drawing.Point(489, 300);
            this.FlightModeButton.Name = "FlightModeButton";
            this.FlightModeButton.Size = new System.Drawing.Size(75, 23);
            this.FlightModeButton.TabIndex = 14;
            this.FlightModeButton.Text = "Set Mode";
            this.FlightModeButton.UseVisualStyleBackColor = true;
            this.FlightModeButton.Click += new System.EventHandler(this.FlightModeButton_Click);
            // 
            // LatitudeTag
            // 
            this.LatitudeTag.AutoSize = true;
            this.LatitudeTag.Location = new System.Drawing.Point(166, 277);
            this.LatitudeTag.Name = "LatitudeTag";
            this.LatitudeTag.Size = new System.Drawing.Size(30, 12);
            this.LatitudeTag.TabIndex = 15;
            this.LatitudeTag.Text = "Lat: ";
            // 
            // LongitudeTag
            // 
            this.LongitudeTag.AutoSize = true;
            this.LongitudeTag.Location = new System.Drawing.Point(298, 277);
            this.LongitudeTag.Name = "LongitudeTag";
            this.LongitudeTag.Size = new System.Drawing.Size(34, 12);
            this.LongitudeTag.TabIndex = 16;
            this.LongitudeTag.Text = "Lon: ";
            // 
            // SubModeTag
            // 
            this.SubModeTag.AutoSize = true;
            this.SubModeTag.Location = new System.Drawing.Point(166, 325);
            this.SubModeTag.Name = "SubModeTag";
            this.SubModeTag.Size = new System.Drawing.Size(89, 12);
            this.SubModeTag.TabIndex = 17;
            this.SubModeTag.Text = "CustomMode: ";
            // 
            // SubModeLabel
            // 
            this.SubModeLabel.AutoSize = true;
            this.SubModeLabel.ForeColor = System.Drawing.Color.Red;
            this.SubModeLabel.Location = new System.Drawing.Point(247, 325);
            this.SubModeLabel.Name = "SubModeLabel";
            this.SubModeLabel.Size = new System.Drawing.Size(90, 12);
            this.SubModeLabel.TabIndex = 18;
            this.SubModeLabel.Text = "SubModeLabel";
            // 
            // RollTag
            // 
            this.RollTag.AutoSize = true;
            this.RollTag.Location = new System.Drawing.Point(23, 220);
            this.RollTag.Name = "RollTag";
            this.RollTag.Size = new System.Drawing.Size(30, 12);
            this.RollTag.TabIndex = 19;
            this.RollTag.Text = "roll: ";
            // 
            // PitchTag
            // 
            this.PitchTag.AutoSize = true;
            this.PitchTag.Location = new System.Drawing.Point(23, 238);
            this.PitchTag.Name = "PitchTag";
            this.PitchTag.Size = new System.Drawing.Size(40, 12);
            this.PitchTag.TabIndex = 20;
            this.PitchTag.Text = "pitch: ";
            // 
            // YawTag
            // 
            this.YawTag.AutoSize = true;
            this.YawTag.Location = new System.Drawing.Point(23, 256);
            this.YawTag.Name = "YawTag";
            this.YawTag.Size = new System.Drawing.Size(37, 12);
            this.YawTag.TabIndex = 21;
            this.YawTag.Text = "yaw: ";
            // 
            // RollLabel
            // 
            this.RollLabel.AutoSize = true;
            this.RollLabel.ForeColor = System.Drawing.Color.RoyalBlue;
            this.RollLabel.Location = new System.Drawing.Point(59, 220);
            this.RollLabel.Name = "RollLabel";
            this.RollLabel.Size = new System.Drawing.Size(57, 12);
            this.RollLabel.TabIndex = 22;
            this.RollLabel.Text = "RollLabel";
            // 
            // PitchLabel
            // 
            this.PitchLabel.AutoSize = true;
            this.PitchLabel.ForeColor = System.Drawing.Color.RoyalBlue;
            this.PitchLabel.Location = new System.Drawing.Point(59, 238);
            this.PitchLabel.Name = "PitchLabel";
            this.PitchLabel.Size = new System.Drawing.Size(64, 12);
            this.PitchLabel.TabIndex = 23;
            this.PitchLabel.Text = "PitchLabel";
            // 
            // YawLabel
            // 
            this.YawLabel.AutoSize = true;
            this.YawLabel.ForeColor = System.Drawing.Color.RoyalBlue;
            this.YawLabel.Location = new System.Drawing.Point(59, 256);
            this.YawLabel.Name = "YawLabel";
            this.YawLabel.Size = new System.Drawing.Size(61, 12);
            this.YawLabel.TabIndex = 24;
            this.YawLabel.Text = "YawLabel";
            // 
            // BatteryTag
            // 
            this.BatteryTag.AutoSize = true;
            this.BatteryTag.Location = new System.Drawing.Point(23, 311);
            this.BatteryTag.Name = "BatteryTag";
            this.BatteryTag.Size = new System.Drawing.Size(37, 12);
            this.BatteryTag.TabIndex = 25;
            this.BatteryTag.Text = "Battery: ";
            // 
            // BatteryLabel
            // 
            this.BatteryLabel.AutoSize = true;
            this.BatteryLabel.ForeColor = System.Drawing.Color.Red;
            this.BatteryLabel.Location = new System.Drawing.Point(75, 311);
            this.BatteryLabel.Name = "BatteryLabel";
            this.BatteryLabel.Size = new System.Drawing.Size(37, 12);
            this.BatteryLabel.TabIndex = 26;
            this.BatteryLabel.Text = "0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.BatteryLabel);
            this.Controls.Add(this.BatteryTag);
            this.Controls.Add(this.YawLabel);
            this.Controls.Add(this.PitchLabel);
            this.Controls.Add(this.RollLabel);
            this.Controls.Add(this.YawTag);
            this.Controls.Add(this.PitchTag);
            this.Controls.Add(this.RollTag);
            this.Controls.Add(this.SubModeLabel);
            this.Controls.Add(this.SubModeTag);
            this.Controls.Add(this.LongitudeTag);
            this.Controls.Add(this.LatitudeTag);
            this.Controls.Add(this.FlightModeButton);
            this.Controls.Add(this.FlightModeComboBox);
            this.Controls.Add(this.FlightModeLabel);
            this.Controls.Add(this.FlightModeTag);
            this.Controls.Add(this.MissionStartButton);
            this.Controls.Add(this.CommandResultMessageLabel);
            this.Controls.Add(this.ClearMissionButton);
            this.Controls.Add(this.StatusMessageLabel);
            this.Controls.Add(this.MissionUploadButton);
            this.Controls.Add(this.LongitudeLabel);
            this.Controls.Add(this.LatitudeLabel);
            this.Controls.Add(this.DisarmButton);
            this.Controls.Add(this.ArmButton);
            this.Controls.Add(this.LandButton);
            this.Controls.Add(this.TakeoffButton);
            this.Name = "Form1";
            this.Text = "MAVLink.NET";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button TakeoffButton;
        private System.Windows.Forms.Button LandButton;
        private System.Windows.Forms.Button ArmButton;
        private System.Windows.Forms.Button DisarmButton;
        private System.Windows.Forms.Label LatitudeLabel;
        private System.Windows.Forms.Label LongitudeLabel;
        private System.Windows.Forms.Button MissionUploadButton;
        private System.Windows.Forms.Label StatusMessageLabel;
        private System.Windows.Forms.Button ClearMissionButton;
        private System.Windows.Forms.Label CommandResultMessageLabel;
        private System.Windows.Forms.Button MissionStartButton;
        private System.Windows.Forms.Label FlightModeTag;
        private System.Windows.Forms.Label FlightModeLabel;
        private System.Windows.Forms.ComboBox FlightModeComboBox;
        private System.Windows.Forms.Button FlightModeButton;
        private System.Windows.Forms.Label LatitudeTag;
        private System.Windows.Forms.Label LongitudeTag;
        private System.Windows.Forms.Label SubModeTag;
        private System.Windows.Forms.Label SubModeLabel;
        private System.Windows.Forms.Label RollTag;
        private System.Windows.Forms.Label PitchTag;
        private System.Windows.Forms.Label YawTag;
        private System.Windows.Forms.Label RollLabel;
        private System.Windows.Forms.Label PitchLabel;
        private System.Windows.Forms.Label YawLabel;
        private System.Windows.Forms.Label BatteryTag;
        private System.Windows.Forms.Label BatteryLabel;
    }
}

