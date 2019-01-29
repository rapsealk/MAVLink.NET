using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MavLink;

namespace UnitTest
{
    public class MAVLinkController
    {
        private readonly MAVLinkModel Model;
        private readonly MAVLinkView View;

        private readonly Mavlink MAVLink;

        public System.IO.Ports.SerialPort SerialPort    = null;
        public System.Threading.Thread HeartbeatThread  = null;

        public MAVLinkController(MAVLinkModel model, MAVLinkView view)
        {
            Model = model;
            View = view;

            View.SetController(this);

            MAVLink = new Mavlink();
        }

        public void SerialConnect()
        {
            string portName = View.GetSerialPortName();

            lock (SerialPort)
            {
                if (SerialPort != null && SerialPort.IsOpen)
                    SerialPort.Close();

                SerialPort = new System.IO.Ports.SerialPort()
                {
                    PortName = portName,
                    BaudRate = 57600
                };
                SerialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(OnSerialReceived);
                SerialPort.Open();
            }

            if (HeartbeatThread != null && HeartbeatThread.IsAlive)
                HeartbeatThread.Interrupt();

            HeartbeatThread = new System.Threading.Thread(OnHeartbeat);
            HeartbeatThread.Start();
        }

        private void OnSerialReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            int bsize = SerialPort.BytesToRead;
            byte[] bytes = new byte[bsize];
            for (int i = 0; i < bsize; ++i)
                bytes[i] = (byte) SerialPort.ReadByte();
            MAVLink.ParseBytes(bytes);
        }

        private void OnHeartbeat()
        {
            while (true)
            {
                Msg_heartbeat heartbeatMessage = new Msg_heartbeat()
                {
                    type            = (byte) MAV_TYPE.MAV_TYPE_GCS,
                    system_status   = 0,
                    base_mode       = 0,
                    custom_mode     = 0,
                    autopilot       = 0
                };
                SendPacket(heartbeatMessage);

                System.Threading.Thread.Sleep(1000);
            }
        }

        private void SendPacket(MavlinkMessage message)
        {
            MavlinkPacket packet = new MavlinkPacket()
            {
                SystemId        = 255,
                ComponentId     = (byte) MAV_COMPONENT.MAV_COMP_ID_MISSIONPLANNER,
                Message         = message,
                SequenceNumber  = Model.PacketSequence
            };
            byte[] bytes = MAVLink.Send(packet);
            SerialPort.Write(bytes, 0, bytes.Length);
        }
    }
}
