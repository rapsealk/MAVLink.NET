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
            this.WaypointButton = new System.Windows.Forms.Button();
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
            this.LatitudeLabel.Location = new System.Drawing.Point(166, 303);
            this.LatitudeLabel.Name = "LatitudeLabel";
            this.LatitudeLabel.Size = new System.Drawing.Size(80, 12);
            this.LatitudeLabel.TabIndex = 4;
            this.LatitudeLabel.Text = "LatitudeLabel";
            // 
            // LongitudeLabel
            // 
            this.LongitudeLabel.AutoSize = true;
            this.LongitudeLabel.Location = new System.Drawing.Point(360, 303);
            this.LongitudeLabel.Name = "LongitudeLabel";
            this.LongitudeLabel.Size = new System.Drawing.Size(91, 12);
            this.LongitudeLabel.TabIndex = 5;
            this.LongitudeLabel.Text = "LongitudeLabel";
            // 
            // WaypointButton
            // 
            this.WaypointButton.Location = new System.Drawing.Point(168, 147);
            this.WaypointButton.Name = "WaypointButton";
            this.WaypointButton.Size = new System.Drawing.Size(358, 23);
            this.WaypointButton.TabIndex = 6;
            this.WaypointButton.Text = "Waypoint";
            this.WaypointButton.UseVisualStyleBackColor = true;
            this.WaypointButton.Click += new System.EventHandler(this.WaypointButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.WaypointButton);
            this.Controls.Add(this.LongitudeLabel);
            this.Controls.Add(this.LatitudeLabel);
            this.Controls.Add(this.DisarmButton);
            this.Controls.Add(this.ArmButton);
            this.Controls.Add(this.LandButton);
            this.Controls.Add(this.TakeoffButton);
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
        private System.Windows.Forms.Button WaypointButton;
    }
}

