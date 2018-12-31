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
        private MAVLinkNode node1, node2;

        public Form1()
        {
            InitializeComponent();

            //string[] portNames = System.IO.Ports.SerialPort.GetPortNames();

            MAVManager = new MAVLinkManager();
            node1 = MAVManager.RegisterAgent("COM10", 57600);
            node2 = MAVManager.RegisterAgent("COM9", 57600);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            MAVManager.Open(0);

            System.Threading.Thread thread = new System.Threading.Thread(UpdatePacketSequence);
            thread.Start();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            MAVManager.Open(1);

            System.Threading.Thread thread = new System.Threading.Thread(UpdatePacketSequence2);
            thread.Start();
        }

        private void UpdatePacketSequence()
        {
            while (true)
            {
                label1.BeginInvoke((Action) delegate () { label1.Text = String.Format("seq: {0:d}", node1.PacketSequence); });
                System.Threading.Thread.Sleep(1000);
            }
        }

        private void UpdatePacketSequence2()
        {
            while (true)
            {
                label2.BeginInvoke((Action) delegate () { label2.Text = String.Format("seq: {0:d}", node2.PacketSequence); });
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}