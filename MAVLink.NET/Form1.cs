using System;
using System.Linq;
using System.Windows.Forms;

namespace MAVLink.NET
{
    public partial class Form1 : Form
    {
        private MAVLinkNode[] nodes;

        private string[] FormationModes = { "Triangle", "Row", "Column" };
        private MAVLinkManager.FORMATION[] Formations = { MAVLinkManager.FORMATION.TRIANGLE, MAVLinkManager.FORMATION.ROW, MAVLinkManager.FORMATION.COLUMN };

        public Form1()
        {
            InitializeComponent();

            foreach (ComboBox flightModeComboBox in FlightModeComboBoxes)
                flightModeComboBox.Items.AddRange(MAVLinkNode.PX4Mode);

            FormationModeComboBox.Items.AddRange(FormationModes);

            string[] portNames = System.IO.Ports.SerialPort.GetPortNames();

            foreach (string portName in portNames) Console.WriteLine("po: {0:s}", portName);

            if (portNames.Length < 3)
            {
                Console.Error.WriteLine("Not enough serial ports are ready.");
                Console.Error.WriteLine("Program is gonna be terminated..");

                this.Close();
                return;
            }

            //*
            int count = 0;
            nodes = new MAVLinkNode[3];
            foreach (string portName in portNames.Reverse())
            {
                nodes[count++] = MAVLinkManager.RegisterAgent(portName, 57600);
                if (count == nodes.Length) break;
            }
            for (int i = 0; i < 3; i++) MAVLinkManager.Open(i);
            /*/
            //*/
            UpdateUI();
        }

        ~Form1()
        {
            MAVLinkManager.CloseAll();
        }

        /* TODO: Lambda Generator
        private Func<object, EventArgs, bool> ArmButton_Click_Generator(int index) {
            return (sender, e) => nodes[index].ArmDisarmCommand(true, ArmButtons[index]);
        }
        */

        private void ArmButton_Click_01(object sender, EventArgs e)
        {
            nodes[0].ArmDisarmCommand(true, ArmButton_01);
        }
        private void ArmButton_Click_02(object sender, EventArgs e)
        {
            nodes[1].ArmDisarmCommand(true, ArmButton_02);
        }
        private void ArmButton_Click_03(object sender, EventArgs e)
        {
            nodes[2].ArmDisarmCommand(true, ArmButton_03);
        }
        
        private void DisarmButton_Click_01(object sender, EventArgs e)
        {
            nodes[0].ArmDisarmCommand(false, DisarmButton_01);
        }
        private void DisarmButton_Click_02(object sender, EventArgs e)
        {
            nodes[1].ArmDisarmCommand(false, DisarmButton_02);
        }
        private void DisarmButton_Click_03(object sender, EventArgs e)
        {
            nodes[2].ArmDisarmCommand(false, DisarmButton_03);
        }

        private void TakeoffButton_Click_01(object sender, EventArgs e)
        {
            nodes[0].TakeoffCommand();
        }
        private void TakeoffButton_Click_02(object sender, EventArgs e)
        {
            nodes[1].TakeoffCommand();
        }
        private void TakeoffButton_Click_03(object sender, EventArgs e)
        {
            nodes[2].TakeoffCommand();
        }

        private void LandButton_Click_01(object sender, EventArgs e)
        {
            nodes[0].LandCommand();
        }
        private void LandButton_Click_02(object sender, EventArgs e)
        {
            nodes[1].LandCommand();
        }
        private void LandButton_Click_03(object sender, EventArgs e)
        {
            nodes[2].LandCommand();
        }

        private void MissionUploadButton_Click_01(object sender, EventArgs e)
        {
            // node1.NextWP(0, 0);
            nodes[0].UploadMission();
        }
        private void MissionUploadButton_Click_02(object sender, EventArgs e)
        {
            // node1.NextWP(0, 0);
            nodes[1].UploadMission();
        }
        private void MissionUploadButton_Click_03(object sender, EventArgs e)
        {
            // node1.NextWP(0, 0);
            nodes[2].UploadMission();
        }

        private void ClearMissionButton_Click_01(object sender, EventArgs e)
        {
            ClearMissionButton_01.BeginInvoke((Action) delegate () { ClearMissionButton_01.Enabled = false; });
            nodes[0].ClearMission();
            ClearMissionButton_01.BeginInvoke((Action) delegate () { ClearMissionButton_01.Enabled = true; });
        }
        private void ClearMissionButton_Click_02(object sender, EventArgs e)
        {
            ClearMissionButton_02.BeginInvoke((Action) delegate () { ClearMissionButton_02.Enabled = false; });
            nodes[1].ClearMission();
            ClearMissionButton_02.BeginInvoke((Action) delegate () { ClearMissionButton_02.Enabled = true; });
        }
        private void ClearMissionButton_Click_03(object sender, EventArgs e)
        {
            ClearMissionButton_03.BeginInvoke((Action) delegate () { ClearMissionButton_03.Enabled = false; });
            nodes[2].ClearMission();
            ClearMissionButton_03.BeginInvoke((Action) delegate () { ClearMissionButton_03.Enabled = true; });
        }

        private void MissionStartButton_Click_01(object sender, EventArgs e)
        {
            nodes[0].StartMission();
        }
        private void MissionStartButton_Click_02(object sender, EventArgs e)
        {
            nodes[1].StartMission();
        }
        private void MissionStartButton_Click_03(object sender, EventArgs e)
        {
            nodes[2].StartMission();
        }

        private void FlightModeButton_Click_01(object sender, EventArgs e)
        {
            uint index = (uint) FlightModeComboBox_01.SelectedIndex;
            if (index == 0) return;

            nodes[0].SetFlightMode(index);
        }
        private void FlightModeButton_Click_02(object sender, EventArgs e)
        {
            uint index = (uint) FlightModeComboBox_01.SelectedIndex;
            if (index == 0) return;

            nodes[1].SetFlightMode(index);
        }
        private void FlightModeButton_Click_03(object sender, EventArgs e)
        {
            uint index = (uint) FlightModeComboBox_01.SelectedIndex;
            if (index == 0) return;

            nodes[2].SetFlightMode(index);
        }

        private void UpdateUI()
        {
            System.Threading.Thread thread = new System.Threading.Thread(() => {
                while (true)
                {
                    System.Threading.Thread.Sleep(1000);

                    try
                    {
                        // #1
                        LatitudeLabel_01.BeginInvoke((Action) delegate () { LatitudeLabel_01.Text = String.Format("{0:f6}", nodes[0].Position.X); });
                        LongitudeLabel_01.BeginInvoke((Action) delegate () { LongitudeLabel_01.Text = String.Format("{0:f6}", nodes[0].Position.Y); });
                        AltitudeLabel_01.BeginInvoke((Action) delegate () { AltitudeLabel_01.Text = String.Format("{0:f6}", nodes[0].Position.Z); });
                        LocalXLabel_01.BeginInvoke((Action) delegate () { LocalXLabel_01.Text = String.Format("{0:f2}", nodes[0].LocalPosition.X); });
                        LocalYLabel_01.BeginInvoke((Action) delegate () { LocalYLabel_01.Text = String.Format("{0:f2}", nodes[0].LocalPosition.Y); });
                        LocalYLabel_01.BeginInvoke((Action) delegate () { LocalZLabel_01.Text = String.Format("{0:f2}", nodes[0].LocalPosition.Z); });
                        HomeLatitudeLabel_01.BeginInvoke((Action) delegate () { HomeLatitudeLabel_01.Text = String.Format("{0:f6}", nodes[0].HomePosition.X); });
                        HomeLongitude_01.BeginInvoke((Action) delegate () { HomeLongitude_01.Text = String.Format("{0:f6}", nodes[0].HomePosition.Y); });
                        RollLabel_01.BeginInvoke((Action) delegate () { RollLabel_01.Text = String.Format("{0:f2}", nodes[0].Roll); });
                        PitchLabel_01.BeginInvoke((Action) delegate () { PitchLabel_01.Text = String.Format("{0:f2}", nodes[0].Pitch); });
                        YawLabel_01.BeginInvoke((Action) delegate () { YawLabel_01.Text = String.Format("{0:f2}", nodes[0].Yaw); });
                        BatteryLabel_01.BeginInvoke((Action) delegate () { BatteryLabel_01.Text = String.Format("{0:f2}", nodes[0].BatteryPercentage); });
                        FlightModeLabel_01.BeginInvoke((Action) delegate () { FlightModeLabel_01.Text = nodes[0].FlightMode; });
                        SubModeLabel_01.BeginInvoke((Action) delegate () { SubModeLabel_01.Text = nodes[0].SubMode; });
                        StatusMessageLabel_01.BeginInvoke((Action) delegate () { StatusMessageLabel_01.Text = nodes[0].StatusMessage; });
                        CommandResultMessageLabel_01.BeginInvoke((Action) delegate () { CommandResultMessageLabel_01.Text = nodes[0].CommandResultMessage; });

                        DroneTag_01.BeginInvoke((Action) delegate () { DroneTag_01.Text = String.Format("DroneTag #{0:d}", nodes[0].SYSTEM_ID); });

                        // #2
                        LatitudeLabel_02.BeginInvoke((Action)delegate () { LatitudeLabel_02.Text = String.Format("{0:f6}", nodes[1].Position.X); });
                        LongitudeLabel_02.BeginInvoke((Action)delegate () { LongitudeLabel_02.Text = String.Format("{0:f6}", nodes[1].Position.Y); });
                        AltitudeLabel_02.BeginInvoke((Action)delegate () { AltitudeLabel_02.Text = String.Format("{0:f6}", nodes[1].Position.Z); });
                        LocalXLabel_02.BeginInvoke((Action)delegate () { LocalXLabel_02.Text = String.Format("{0:f2}", nodes[1].LocalPosition.X); });
                        LocalYLabel_02.BeginInvoke((Action)delegate () { LocalYLabel_02.Text = String.Format("{0:f2}", nodes[1].LocalPosition.Y); });
                        LocalYLabel_02.BeginInvoke((Action)delegate () { LocalZLabel_02.Text = String.Format("{0:f2}", nodes[1].LocalPosition.Z); });
                        HomeLatitudeLabel_02.BeginInvoke((Action)delegate () { HomeLatitudeLabel_02.Text = String.Format("{0:f6}", nodes[1].HomePosition.X); });
                        HomeLongitudeLabel_02.BeginInvoke((Action)delegate () { HomeLongitudeLabel_02.Text = String.Format("{0:f6}", nodes[1].HomePosition.Y); });
                        RollLabel_02.BeginInvoke((Action)delegate () { RollLabel_02.Text = String.Format("{0:f2}", nodes[1].Roll); });
                        PitchLabel_02.BeginInvoke((Action)delegate () { PitchLabel_02.Text = String.Format("{0:f2}", nodes[1].Pitch); });
                        YawLabel_02.BeginInvoke((Action)delegate () { YawLabel_02.Text = String.Format("{0:f2}", nodes[1].Yaw); });
                        BatteryLabel_02.BeginInvoke((Action)delegate () { BatteryLabel_02.Text = String.Format("{0:f2}", nodes[1].BatteryPercentage); });
                        FlightModeLabel_02.BeginInvoke((Action)delegate () { FlightModeLabel_02.Text = nodes[1].FlightMode; });
                        SubModeLabel_02.BeginInvoke((Action)delegate () { SubModeLabel_02.Text = nodes[1].SubMode; });
                        StatusMessageLabel_02.BeginInvoke((Action)delegate () { StatusMessageLabel_02.Text = nodes[1].StatusMessage; });
                        CommandResultMessageLabel_02.BeginInvoke((Action)delegate () { CommandResultMessageLabel_02.Text = nodes[1].CommandResultMessage; });

                        DroneTag_02.BeginInvoke((Action)delegate () { DroneTag_02.Text = String.Format("DroneTag #{0:d}", nodes[1].SYSTEM_ID); });

                        // #3
                        LatitudeLabel_03.BeginInvoke((Action)delegate () { LatitudeLabel_03.Text = String.Format("{0:f6}", nodes[2].Position.X); });
                        LongitudeLabel_03.BeginInvoke((Action)delegate () { LongitudeLabel_03.Text = String.Format("{0:f6}", nodes[2].Position.Y); });
                        AltitudeLabel_03.BeginInvoke((Action)delegate () { AltitudeLabel_03.Text = String.Format("{0:f6}", nodes[2].Position.Z); });
                        LocalXLabel_03.BeginInvoke((Action)delegate () { LocalXLabel_03.Text = String.Format("{0:f2}", nodes[2].LocalPosition.X); });
                        LocalYLabel_03.BeginInvoke((Action)delegate () { LocalYLabel_03.Text = String.Format("{0:f2}", nodes[2].LocalPosition.Y); });
                        LocalYLabel_03.BeginInvoke((Action)delegate () { LocalZLabel_03.Text = String.Format("{0:f2}", nodes[2].LocalPosition.Z); });
                        HomeLatitudeLabel_03.BeginInvoke((Action)delegate () { HomeLatitudeLabel_03.Text = String.Format("{0:f6}", nodes[2].HomePosition.X); });
                        HomeLongitudeLabel_03.BeginInvoke((Action)delegate () { HomeLongitudeLabel_03.Text = String.Format("{0:f6}", nodes[2].HomePosition.Y); });
                        RollLabel_03.BeginInvoke((Action)delegate () { RollLabel_03.Text = String.Format("{0:f2}", nodes[2].Roll); });
                        PitchLabel_03.BeginInvoke((Action)delegate () { PitchLabel_03.Text = String.Format("{0:f2}", nodes[2].Pitch); });
                        YawLabel_03.BeginInvoke((Action)delegate () { YawLabel_03.Text = String.Format("{0:f2}", nodes[2].Yaw); });
                        BatteryLabel_03.BeginInvoke((Action)delegate () { BatteryLabel_03.Text = String.Format("{0:f2}", nodes[2].BatteryPercentage); });
                        FlightModeLabel_03.BeginInvoke((Action)delegate () { FlightModeLabel_03.Text = nodes[2].FlightMode; });
                        SubModeLabel_03.BeginInvoke((Action)delegate () { SubModeLabel_03.Text = nodes[2].SubMode; });
                        StatusMessageLabel_03.BeginInvoke((Action)delegate () { StatusMessageLabel_03.Text = nodes[2].StatusMessage; });
                        CommandResultMessageLabel_03.BeginInvoke((Action)delegate () { CommandResultMessageLabel_03.Text = nodes[2].CommandResultMessage; });

                        DroneTag_03.BeginInvoke((Action)delegate () { DroneTag_03.Text = String.Format("DroneTag #{0:d}", nodes[2].SYSTEM_ID); });
                    }
                    catch (InvalidOperationException e)
                    {
                        Console.Error.WriteLine(e.Message);
                        break;
                    }
                }
            });
            thread.Start();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            MAVLinkManager.RunScenario();
        }

        private void DisarmAllButton_Click(object sender, EventArgs e)
        {
            foreach (MAVLinkNode node in nodes)
                node.ArmDisarmCommand(false);
        }

        private void LandAllButton_Click(object sender, EventArgs e)
        {
            foreach (MAVLinkNode node in nodes)
                node.LandCommand();
        }

        private void TakeoffAllButton_Click(object sender, EventArgs e)
        {
            foreach (MAVLinkNode node in nodes)
                node.TakeoffCommand();
        }

        private void FormationModeButton_Click(object sender, EventArgs e)
        {
            MAVLinkManager.FormationMode = Formations[FormationModeComboBox.SelectedIndex];
        }
    }
}