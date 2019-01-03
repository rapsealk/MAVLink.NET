namespace MAVLink.NET
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
            this.SuspendLayout();
            // 
            // TakeoffButton
            // 
            this.TakeoffButton.Location = new System.Drawing.Point(237, 357);
            this.TakeoffButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TakeoffButton.Name = "TakeoffButton";
            this.TakeoffButton.Size = new System.Drawing.Size(234, 34);
            this.TakeoffButton.TabIndex = 0;
            this.TakeoffButton.Text = "Takeoff";
            this.TakeoffButton.UseVisualStyleBackColor = true;
            this.TakeoffButton.Click += new System.EventHandler(this.TakeoffButton_Click);
            // 
            // LandButton
            // 
            this.LandButton.Location = new System.Drawing.Point(517, 357);
            this.LandButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.LandButton.Name = "LandButton";
            this.LandButton.Size = new System.Drawing.Size(234, 34);
            this.LandButton.TabIndex = 1;
            this.LandButton.Text = "Land";
            this.LandButton.UseVisualStyleBackColor = true;
            this.LandButton.Click += new System.EventHandler(this.LandButton_Click);
            // 
            // ArmButton
            // 
            this.ArmButton.Location = new System.Drawing.Point(237, 314);
            this.ArmButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ArmButton.Name = "ArmButton";
            this.ArmButton.Size = new System.Drawing.Size(234, 34);
            this.ArmButton.TabIndex = 2;
            this.ArmButton.Text = "Arm";
            this.ArmButton.UseVisualStyleBackColor = true;
            this.ArmButton.Click += new System.EventHandler(this.ArmButton_Click);
            // 
            // DisarmButton
            // 
            this.DisarmButton.Location = new System.Drawing.Point(517, 314);
            this.DisarmButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.DisarmButton.Name = "DisarmButton";
            this.DisarmButton.Size = new System.Drawing.Size(234, 34);
            this.DisarmButton.TabIndex = 3;
            this.DisarmButton.Text = "Disarm";
            this.DisarmButton.UseVisualStyleBackColor = true;
            this.DisarmButton.Click += new System.EventHandler(this.DisarmButton_Click);
            // 
            // LatitudeLabel
            // 
            this.LatitudeLabel.AutoSize = true;
            this.LatitudeLabel.Location = new System.Drawing.Point(237, 454);
            this.LatitudeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LatitudeLabel.Name = "LatitudeLabel";
            this.LatitudeLabel.Size = new System.Drawing.Size(112, 18);
            this.LatitudeLabel.TabIndex = 4;
            this.LatitudeLabel.Text = "LatitudeLabel";
            // 
            // LongitudeLabel
            // 
            this.LongitudeLabel.AutoSize = true;
            this.LongitudeLabel.Location = new System.Drawing.Point(514, 454);
            this.LongitudeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LongitudeLabel.Name = "LongitudeLabel";
            this.LongitudeLabel.Size = new System.Drawing.Size(128, 18);
            this.LongitudeLabel.TabIndex = 5;
            this.LongitudeLabel.Text = "LongitudeLabel";
            // 
            // MissionUploadButton
            // 
            this.MissionUploadButton.Location = new System.Drawing.Point(240, 177);
            this.MissionUploadButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MissionUploadButton.Name = "MissionUploadButton";
            this.MissionUploadButton.Size = new System.Drawing.Size(511, 34);
            this.MissionUploadButton.TabIndex = 6;
            this.MissionUploadButton.Text = "Mission Upload";
            this.MissionUploadButton.UseVisualStyleBackColor = true;
            this.MissionUploadButton.Click += new System.EventHandler(this.MissionUploadButton_Click);
            // 
            // StatusMessageLabel
            // 
            this.StatusMessageLabel.AutoSize = true;
            this.StatusMessageLabel.Location = new System.Drawing.Point(237, 516);
            this.StatusMessageLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.StatusMessageLabel.Name = "StatusMessageLabel";
            this.StatusMessageLabel.Size = new System.Drawing.Size(176, 18);
            this.StatusMessageLabel.TabIndex = 7;
            this.StatusMessageLabel.Text = "StatusMessageLabel";
            // 
            // ClearMissionButton
            // 
            this.ClearMissionButton.Location = new System.Drawing.Point(240, 220);
            this.ClearMissionButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ClearMissionButton.Name = "ClearMissionButton";
            this.ClearMissionButton.Size = new System.Drawing.Size(511, 34);
            this.ClearMissionButton.TabIndex = 8;
            this.ClearMissionButton.Text = "Clear Mission";
            this.ClearMissionButton.UseVisualStyleBackColor = true;
            this.ClearMissionButton.Click += new System.EventHandler(this.ClearMissionButton_Click);
            // 
            // CommandResultMessageLabel
            // 
            this.CommandResultMessageLabel.AutoSize = true;
            this.CommandResultMessageLabel.Location = new System.Drawing.Point(237, 556);
            this.CommandResultMessageLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.CommandResultMessageLabel.Name = "CommandResultMessageLabel";
            this.CommandResultMessageLabel.Size = new System.Drawing.Size(257, 18);
            this.CommandResultMessageLabel.TabIndex = 9;
            this.CommandResultMessageLabel.Text = "CommandResultMessageLabel";
            // 
            // MissionStartButton
            // 
            this.MissionStartButton.Location = new System.Drawing.Point(240, 135);
            this.MissionStartButton.Margin = new System.Windows.Forms.Padding(4);
            this.MissionStartButton.Name = "MissionStartButton";
            this.MissionStartButton.Size = new System.Drawing.Size(511, 34);
            this.MissionStartButton.TabIndex = 10;
            this.MissionStartButton.Text = "Mission Start";
            this.MissionStartButton.UseVisualStyleBackColor = true;
            this.MissionStartButton.Click += new System.EventHandler(this.MissionStartButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1143, 675);
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
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Form1";
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
    }
}

