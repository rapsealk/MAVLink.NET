using System;
using System.Windows.Forms;

namespace UnitTest
{
    public partial class MAVLinkView : Form
    {
        private MAVLinkController Controller;

        public MAVLinkView()
        {
            InitializeComponent();
            SerialPortNameComboBox.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
        }

        public void SetController(MAVLinkController controller)
        {
            Controller = controller;
        }

        public void EnableSerialPortNameComboBox(bool enabled)
        {
            SerialPortNameComboBox.Enabled = enabled;
        }

        public void EnableSerialConnectButton(bool enabled)
        {
            SerialConnectButton.Enabled = enabled;
        }

        public string GetSerialPortName()
        {
            return SerialPortNameComboBox.SelectedItem as string;
        }

        private void SerialConnectButton_Click(object sender, EventArgs e)
        {
            Controller.SerialConnect();
        }

        private void ArmDosarmButton_Click(object sender, EventArgs e)
        {

        }
    }
}