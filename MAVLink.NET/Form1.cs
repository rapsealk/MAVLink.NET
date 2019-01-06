using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MAVLink.NET
{
    public partial class Form1 : Form
    {
        private MAVLinkManager MAVManager;
        private MAVLinkNode[] nodes;

        public Form1()
        {
            InitializeComponent();

            foreach (ComboBox flightModeComboBox in FlightModeComboBoxes)
                flightModeComboBox.Items.AddRange(MAVLinkNode.PX4Mode);

            MAVManager = new MAVLinkManager();

            string[] portNames = System.IO.Ports.SerialPort.GetPortNames();

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
                nodes[count++] = MAVManager.RegisterAgent(portName, 57600);
                if (count == nodes.Length) break;
            }
            /*/
            UpdateUI();
            //*/
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
            nodes[1].ArmDisarmCommand(true, ArmButton_01);
        }
        private void ArmButton_Click_03(object sender, EventArgs e)
        {
            nodes[2].ArmDisarmCommand(true, ArmButton_01);
        }
        
        private void DisarmButton_Click_01(object sender, EventArgs e)
        {
            nodes[0].ArmDisarmCommand(false, DisarmButton_01);
        }
        private void DisarmButton_Click_02(object sender, EventArgs e)
        {
            nodes[1].ArmDisarmCommand(false, DisarmButton_01);
        }
        private void DisarmButton_Click_03(object sender, EventArgs e)
        {
            nodes[2].ArmDisarmCommand(false, DisarmButton_01);
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
            uint index = (uint)FlightModeComboBox_01.SelectedIndex;
            if (index == 0) return;

            nodes[0].SetFlightMode(index);
        }
        private void FlightModeButton_Click_02(object sender, EventArgs e)
        {
            uint index = (uint)FlightModeComboBox_01.SelectedIndex;
            if (index == 0) return;

            nodes[1].SetFlightMode(index);
        }
        private void FlightModeButton_Click_03(object sender, EventArgs e)
        {
            uint index = (uint)FlightModeComboBox_01.SelectedIndex;
            if (index == 0) return;

            nodes[2].SetFlightMode(index);
        }

        private void UpdateUI()
        {
            System.Threading.Thread thread = new System.Threading.Thread(() => {
                while (true)
                {
                    System.Threading.Thread.Sleep(1000);

                    for (int i = 0; i < nodes.Length; i++)
                    {
                        Vector3 position = nodes[i].Position;

                        try
                        {
                            LatitudeLabels[i].BeginInvoke((Action) delegate () { LatitudeLabels[i].Text = String.Format("{0:f6}", position.X); });
                            LongitudeLabels[i].BeginInvoke((Action) delegate () { LongitudeLabels[i].Text = String.Format("{0:f6}", position.Y); });
                            AltitudeLabels[i].BeginInvoke((Action) delegate () { AltitudeLabels[i].Text = String.Format("{0:f6}", position.Z); });
                            RollLabels[i].BeginInvoke((Action) delegate () { RollLabels[i].Text = String.Format("{0:f2}", nodes[i].Roll); });
                            PitchLabels[i].BeginInvoke((Action) delegate () { PitchLabels[i].Text = String.Format("{0:f2}", nodes[i].Pitch); });
                            YawLabels[i].BeginInvoke((Action) delegate () { YawLabels[i].Text = String.Format("{0:f2}", nodes[i].Yaw); });
                            BatteryLabels[i].BeginInvoke((Action) delegate () { BatteryLabels[i].Text = String.Format("{0:f2}", nodes[i].BatteryPercentage); });
                            FlightModeLabels[i].BeginInvoke((Action) delegate () { FlightModeLabels[i].Text = nodes[i].FlightMode; });
                            SubModeLabels[i].BeginInvoke((Action) delegate () { SubModeLabels[i].Text = nodes[i].SubMode; });
                            StatusMessageLabels[i].BeginInvoke((Action) delegate () { StatusMessageLabels[i].Text = nodes[i].StatusMessage; });
                            CommandResultMessageLabels[i].BeginInvoke((Action) delegate () { CommandResultMessageLabels[i].Text = nodes[i].CommandResultMessage; });
                        }
                        catch (InvalidOperationException e)
                        {
                            Console.Error.WriteLine(e.Message);
                            // break;
                        }
                    }
                }
            });
            thread.Start();
        }
    }
}