using System;
using System.Windows.Forms;

namespace UnitTest
{
    public partial class MAVLinkView : Form, Interface.IMAVObserver
    {
        private Interface.IMAVPublisher publisher = null;

        /*
         * IMAVObserver
         */
        public Interface.IMAVPublisher Publisher
        {
            get { return publisher; }
            set
            {
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

            SystemIdLabel.BeginInvoke((Action)delegate () { SystemIdLabel.Text = model.SystemId.ToString(); });
            ComponentIdLabel.BeginInvoke((Action)delegate () { ComponentIdLabel.Text = model.ComponentId.ToString(); });
        }

        public void UpdateArmState(bool isArmed)
        {
            ArmButton.Enabled = !isArmed;
            DisarmButton.Enabled = isArmed;
        }

        /**
         * Callbacks.
         */
        public MAVLinkController Controller;

        public MAVLinkView()
        {
            InitializeComponent();
            SerialPortNameComboBox.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
            FlightModeComboBox.Items.AddRange(MAVLinkModel.PX4Mode);
        }

        private void MAVLinkView_Load(object sender, EventArgs e)
        {
            /*
             * FIXME: (https://docs.microsoft.com/ko-kr/dotnet/framework/winforms/controls/how-to-bind-data-to-the-windows-forms-datagridview-control)
            MissionItemsBindingSource.DataSource = Controller;
            MissionDataGridView.DataSource = MissionItemsBindingSource;
            */
        }

        public string SerialPortName
        {
            get { return SerialPortNameComboBox.SelectedItem as string; }
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

        private void ArmButton_Click(object sender, EventArgs e)
        {
            Controller.ArmDisarmCommand(true);
        }

        private void DisarmButton_Click(object sender, EventArgs e)
        {
            Controller.ArmDisarmCommand(false);
            
            /*
            Button button = sender as Button;
            Console.WriteLine("Button equals to disarmbutton: " + button.Equals(DisarmButton));
            Console.WriteLine("Button.Text: " + button.Text);
            */
        }

        /*
        public void EnableArmDisarmButton(bool enabled)
        {
            ArmButton.Enabled = enabled;
        }
        */

        private void TakeoffButton_Click(object sender, EventArgs e)
        {
            Controller.TakeoffCommand();
        }

        private void LandButton_Click(object sender, EventArgs e)
        {
            Controller.LandCommand();
        }
    }
}