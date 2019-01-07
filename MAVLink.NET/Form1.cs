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
        private MAVLinkNode node1;

        public Form1()
        {
            InitializeComponent();

            FlightModeComboBox.Items.AddRange(MAVLinkNode.PX4Mode);

            MAVManager = new MAVLinkManager();
            //*
            node1 = MAVManager.RegisterAgent("COM13", 57600);

            MAVManager.Open(0);

            UpdateMAVPosition();
            /*/
            //*/
        }

        private void ArmButton_Click(object sender, EventArgs e)
        {
            node1.ArmDisarmCommand(true, ArmButton);
        }
        
        private void DisarmButton_Click(object sender, EventArgs e)
        {
            node1.ArmDisarmCommand(false, DisarmButton);
        }

        private void TakeoffButton_Click(object sender, EventArgs e)
        {
            node1.TakeoffCommand();
        }

        private void LandButton_Click(object sender, EventArgs e)
        {
            node1.LandCommand();
        }

        private void MissionUploadButton_Click(object sender, EventArgs e)
        {
            // node1.NextWP(0, 0);
            node1.UploadMission();
        }

        private void UpdateMAVPosition()
        {
            System.Threading.Thread thread = new System.Threading.Thread(() => {
                while (true)
                {
                    System.Threading.Thread.Sleep(1000);
                    Vector3 position = node1.Position;
                    try
                    {
                        LatitudeLabel.BeginInvoke((Action) delegate () { LatitudeLabel.Text = String.Format("{0:f6}", position.X); });
                        LongitudeLabel.BeginInvoke((Action) delegate () { LongitudeLabel.Text = String.Format("{0:f6}", position.Y); });
                        AltitudeLabel.BeginInvoke((Action) delegate () { AltitudeLabel.Text = String.Format("{0:f6}", position.Z); });
                        HomeLatitudeLabel.BeginInvoke((Action) delegate () { HomeLatitudeLabel.Text = String.Format("{0:f6}", node1.HomePosition.X); });
                        HomeLongitudeLabel.BeginInvoke((Action) delegate () { HomeLongitudeLabel.Text = String.Format("{0:f6}", node1.HomePosition.Y); });
                        RollLabel.BeginInvoke((Action) delegate () { RollLabel.Text = String.Format("{0:f2}", node1.Roll); });
                        PitchLabel.BeginInvoke((Action) delegate () { PitchLabel.Text = String.Format("{0:f2}", node1.Pitch); });
                        YawLabel.BeginInvoke((Action) delegate () { YawLabel.Text = String.Format("{0:f2}", node1.Yaw); });
                        BatteryLabel.BeginInvoke((Action) delegate () { BatteryLabel.Text = String.Format("{0:d}", node1.BatteryPercentage); });
                        FlightModeLabel.BeginInvoke((Action) delegate () { FlightModeLabel.Text = node1.FlightMode; });
                        SubModeLabel.BeginInvoke((Action) delegate () { SubModeLabel.Text = node1.SubMode; });
                        StatusMessageLabel.BeginInvoke((Action) delegate () { StatusMessageLabel.Text = node1.StatusMessage; });
                        CommandResultMessageLabel.BeginInvoke((Action) delegate () { CommandResultMessageLabel.Text = node1.CommandResultMessage; });
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

        private void ClearMissionButton_Click(object sender, EventArgs e)
        {
            ClearMissionButton.BeginInvoke((Action) delegate () { ClearMissionButton.Enabled = false; });
            node1.ClearMission();
            ClearMissionButton.BeginInvoke((Action) delegate () { ClearMissionButton.Enabled = true; });
        }

        private void MissionStartButton_Click(object sender, EventArgs e)
        {
            node1.StartMission();
        }

        private void FlightModeButton_Click(object sender, EventArgs e)
        {
            uint index = (uint) FlightModeComboBox.SelectedIndex;
            if (index == 0) return;

            node1.SetFlightMode(index);
        }

        private void WaypointButton_Click(object sender, EventArgs e)
        {
            double latitude = double.Parse(LatitudeTextBox.Text);
            double longitude = double.Parse(LongitudeTextBox.Text);

            node1.NextWP(latitude, longitude);
        }

        private void HomeButton_Click(object sender, EventArgs e)
        {
            node1.SetCurrentPositionAsHome();
        }
    }
}