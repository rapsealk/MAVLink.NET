using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MavLink;

namespace MAVLink.NET
{
    public partial class Form1 : Form
    {
        private MAVLinkManager MAVManager;
        private MAVLinkNode node1;

        public Form1()
        {
            InitializeComponent();

            MAVManager = new MAVLinkManager();
            node1 = MAVManager.RegisterAgent("COM10", 57600);

            MAVManager.Open(0);
        }

        private void ArmButton_Click(object sender, EventArgs e)
        {
            // ArmButton.BeginInvoke((Action) delegate () { ArmButton.Enabled = false; });
            node1.ArmDisarmCommand(true);
            // ArmButton.BeginInvoke((Action) delegate () { ArmButton.Enabled = true; });
        }

        private void DisarmButton_Click(object sender, EventArgs e)
        {
            node1.ArmDisarmCommand(false);
        }

        private void TakeoffButton_Click(object sender, EventArgs e)
        {
            node1.TakeoffCommand();
        }

        private void LandButton_Click(object sender, EventArgs e)
        {
            node1.LandCommand();
        }

        private void WaypointButton_Click(object sender, EventArgs e)
        {
            node1.NextWP(0, 0);
        }

        private void UpdateMAVPosition()
        {
            while (true)
            {
                Vector3 position = node1.Position;
                LatitudeLabel.BeginInvoke((Action) delegate () { LatitudeLabel.Text = String.Format("{0:f}", position.X); });
                LongitudeLabel.BeginInvoke((Action) delegate () { LongitudeLabel.Text = String.Format("{0:f}", position.Y); });
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}