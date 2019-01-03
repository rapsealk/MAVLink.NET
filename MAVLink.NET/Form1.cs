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
            while (true)
            {
                Vector3 position = node1.Position;
                LatitudeLabel.BeginInvoke((Action) delegate () { LatitudeLabel.Text = String.Format("{0:f}", position.X); });
                LongitudeLabel.BeginInvoke((Action) delegate () { LongitudeLabel.Text = String.Format("{0:f}", position.Y); });
                StatusMessageLabel.BeginInvoke((Action) delegate () { StatusMessageLabel.Text = String.Format("{0:s}", node1.StatusMessage); });
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}