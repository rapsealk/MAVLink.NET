using System;
using System.Windows.Forms;

namespace UnitTest
{
    public partial class MAVLinkView : Form, Interface.IMAVObserver
    {
        private Interface.IMAVPublisher publisher = null;

        private MAVLinkController Controller;

        public MAVLinkView()
        {
            InitializeComponent();
            SerialPortNameComboBox.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
        }

        public string SerialPortName
        {
            get { return SerialPortNameComboBox.SelectedItem as string; }
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

        private void SerialConnectButton_Click(object sender, EventArgs e)
        {
            Controller.SerialConnect();
        }

        private void ArmDisarmButton_Click(object sender, EventArgs e)
        {
            Controller.ArmDisarmVehicle();
        }

        public void EnableArmDisarmButton(bool enabled)
        {
            ArmDisarmButton.Enabled = enabled;
        }

        public void SetArmDisarmButton(bool arm)
        {
            ArmDisarmButton.Text = arm ? "Arm" : "Disarm";
        }

        /*
         * IMAVObserver
         */
        public Interface.IMAVPublisher Publisher
        {
            get { return publisher; }
            set {
                if (publisher != null)
                    publisher.Delete(this);
                publisher = value;
                publisher.Add(this);
            }
        }

        public void Update(MAVLinkModel model)
        {
            // TODO: Update view
            Console.WriteLine("Updated model system_id:" + model.SystemId);

            SystemIdLabel.BeginInvoke((Action) delegate () { SystemIdLabel.Text = model.SystemId.ToString(); });
            ComponentIdLabel.BeginInvoke((Action) delegate () { ComponentIdLabel.Text = model.ComponentId.ToString(); });
        }

        public void UpdateArmState(bool isArmed)
        {
            SetArmDisarmButton(isArmed);
        }
    }
}