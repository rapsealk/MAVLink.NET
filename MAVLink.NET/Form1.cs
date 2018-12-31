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

            string[] portNames = System.IO.Ports.SerialPort.GetPortNames();

            MAVManager = new MAVLinkManager();
            MAVManager.RegisterAgent("COM11", 57600);
            // node1 = new MAVLinkNode("COM11", 57600);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                node1.Serial.Open();
            }
            catch (System.IO.IOException ee)
            {
                Console.Error.WriteLine(ee.Message);
                return;
            }
            
            node1.heartbeatWorker.RunWorkerAsync();

            System.Threading.Thread thread = new System.Threading.Thread(UpdatePacketSequence);
            thread.Start();
        }

        private void UpdatePacketSequence()
        {
            while (true)
            {
                textBox1.BeginInvoke((Action)delegate () { textBox1.Text = String.Format("seq: {0:d}", node1.PacketSequence); });
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}