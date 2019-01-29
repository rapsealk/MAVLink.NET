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
            this.ArmDisarmButton = new System.Windows.Forms.Button();
            this._SystemIdLabel = new System.Windows.Forms.Label();
            this._ComponentIdLabel = new System.Windows.Forms.Label();
            this.SystemIdLabel = new System.Windows.Forms.Label();
            this.ComponentIdLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // SerialPortNameComboBox
            // 
            this.SerialPortNameComboBox.FormattingEnabled = true;
            this.SerialPortNameComboBox.Location = new System.Drawing.Point(101, 52);
            this.SerialPortNameComboBox.Name = "SerialPortNameComboBox";
            this.SerialPortNameComboBox.Size = new System.Drawing.Size(138, 20);
            this.SerialPortNameComboBox.TabIndex = 0;
            // 
            // SerialConnectButton
            // 
            this.SerialConnectButton.Location = new System.Drawing.Point(257, 52);
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
            this.SerialPortNameLabel.Location = new System.Drawing.Point(24, 55);
            this.SerialPortNameLabel.Name = "SerialPortNameLabel";
            this.SerialPortNameLabel.Size = new System.Drawing.Size(74, 12);
            this.SerialPortNameLabel.TabIndex = 2;
            this.SerialPortNameLabel.Text = "Serial Port";
            // 
            // ArmDisarmButton
            // 
            this.ArmDisarmButton.Location = new System.Drawing.Point(257, 94);
            this.ArmDisarmButton.Name = "ArmDisarmButton";
            this.ArmDisarmButton.Size = new System.Drawing.Size(86, 23);
            this.ArmDisarmButton.TabIndex = 3;
            this.ArmDisarmButton.Text = "Arm";
            this.ArmDisarmButton.UseVisualStyleBackColor = true;
            this.ArmDisarmButton.Click += new System.EventHandler(this.ArmDisarmButton_Click);
            // 
            // _SystemIdLabel
            // 
            this._SystemIdLabel.AutoSize = true;
            this._SystemIdLabel.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this._SystemIdLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this._SystemIdLabel.Location = new System.Drawing.Point(26, 99);
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
            this._ComponentIdLabel.Location = new System.Drawing.Point(26, 120);
            this._ComponentIdLabel.Name = "_ComponentIdLabel";
            this._ComponentIdLabel.Size = new System.Drawing.Size(107, 12);
            this._ComponentIdLabel.TabIndex = 5;
            this._ComponentIdLabel.Text = "Component ID: ";
            // 
            // SystemIdLabel
            // 
            this.SystemIdLabel.AutoSize = true;
            this.SystemIdLabel.Location = new System.Drawing.Point(133, 99);
            this.SystemIdLabel.Name = "SystemIdLabel";
            this.SystemIdLabel.Size = new System.Drawing.Size(12, 12);
            this.SystemIdLabel.TabIndex = 6;
            this.SystemIdLabel.Text = "0";
            // 
            // ComponentIdLabel
            // 
            this.ComponentIdLabel.AutoSize = true;
            this.ComponentIdLabel.Location = new System.Drawing.Point(133, 120);
            this.ComponentIdLabel.Name = "ComponentIdLabel";
            this.ComponentIdLabel.Size = new System.Drawing.Size(12, 12);
            this.ComponentIdLabel.TabIndex = 7;
            this.ComponentIdLabel.Text = "0";
            // 
            // MAVLinkView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(536, 450);
            this.Controls.Add(this.ComponentIdLabel);
            this.Controls.Add(this.SystemIdLabel);
            this.Controls.Add(this._ComponentIdLabel);
            this.Controls.Add(this._SystemIdLabel);
            this.Controls.Add(this.ArmDisarmButton);
            this.Controls.Add(this.SerialPortNameLabel);
            this.Controls.Add(this.SerialConnectButton);
            this.Controls.Add(this.SerialPortNameComboBox);
            this.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "MAVLinkView";
            this.Text = "MAVLinkView - UnitTest";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox SerialPortNameComboBox;
        private System.Windows.Forms.Button SerialConnectButton;
        private System.Windows.Forms.Label SerialPortNameLabel;
        private System.Windows.Forms.Button ArmDisarmButton;
        private System.Windows.Forms.Label _SystemIdLabel;
        private System.Windows.Forms.Label _ComponentIdLabel;
        private System.Windows.Forms.Label SystemIdLabel;
        private System.Windows.Forms.Label ComponentIdLabel;
    }
}

