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
            this.SuspendLayout();
            // 
            // SerialPortNameComboBox
            // 
            this.SerialPortNameComboBox.FormattingEnabled = true;
            this.SerialPortNameComboBox.Location = new System.Drawing.Point(88, 52);
            this.SerialPortNameComboBox.Name = "SerialPortNameComboBox";
            this.SerialPortNameComboBox.Size = new System.Drawing.Size(121, 20);
            this.SerialPortNameComboBox.TabIndex = 0;
            // 
            // SerialConnectButton
            // 
            this.SerialConnectButton.Location = new System.Drawing.Point(225, 52);
            this.SerialConnectButton.Name = "SerialConnectButton";
            this.SerialConnectButton.Size = new System.Drawing.Size(75, 23);
            this.SerialConnectButton.TabIndex = 1;
            this.SerialConnectButton.Text = "Connect";
            this.SerialConnectButton.UseVisualStyleBackColor = true;
            // 
            // SerialPortNameLabel
            // 
            this.SerialPortNameLabel.AutoSize = true;
            this.SerialPortNameLabel.Location = new System.Drawing.Point(21, 55);
            this.SerialPortNameLabel.Name = "SerialPortNameLabel";
            this.SerialPortNameLabel.Size = new System.Drawing.Size(38, 12);
            this.SerialPortNameLabel.TabIndex = 2;
            this.SerialPortNameLabel.Text = "Serial Port";
            // 
            // MAVLinkView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 450);
            this.Controls.Add(this.SerialPortNameLabel);
            this.Controls.Add(this.SerialConnectButton);
            this.Controls.Add(this.SerialPortNameComboBox);
            this.Name = "MAVLinkView";
            this.Text = "MAVLinkView - UnitTest";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox SerialPortNameComboBox;
        private System.Windows.Forms.Button SerialConnectButton;
        private System.Windows.Forms.Label SerialPortNameLabel;
    }
}

