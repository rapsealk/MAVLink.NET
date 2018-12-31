using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MavLink;

namespace MAVLink.NET
{
    class MAVLinkNode
    {
        // PX4_CUSTOM_MAIN_MODE
        private static readonly string[] PX4Mode =
        {
            "(EMPTY)", "MANUAL", "ALTCTL", "POSCTL", "AUTO", "ACRO", "OFFBOARD", "STABILIZED", "RATTITUDE",
            "SIMPLE"    // FIXME: unused, but reserved for future use.
        };
        private enum PX4FlightMode
        {
            MANUAL=1, ALTCTL, POSCTL, AUTO, ACRO, OFFBOARD, STABILIZED, RATTITUDE,
            SIMPLE
        }
        // PX4_CUSTOM_SUB_MODE AUTO
        private static readonly string[] PX4SubMode =
        {
            "(EMPTY)", "READY", "TAKEOFF", "LOITER", "MISSION", "RTL", "LAND",
            "RTGS", "FOLLOW_TARGET", "PRECLAND"
        };
        // COMMAND_ACK
        private static readonly string[] ResultMessage =
        {
            "ACCEPTED", "TEMPORARILY REJECTED", "DENIED", "UNSUPPORTED", "FAILED", "IN PROGRESS"
        };

        private Mavlink mavlink;

        public System.IO.Ports.SerialPort Serial;
        public System.ComponentModel.BackgroundWorker heartbeatWorker;

        public byte SYSTEM_ID;
        public byte COMPONENT_ID;

        public byte PacketSequence { get; private set; }

        private Msg_heartbeat       mHeartbeat      = new Msg_heartbeat();
        private Msg_sys_status      mSysStatus      = new Msg_sys_status();
        private Msg_power_status    mPowerStatus    = new Msg_power_status();
        private Msg_attitude        mAttitude       = new Msg_attitude();
        private Msg_gps_raw_int     mGPS            = new Msg_gps_raw_int();
        private Msg_vfr_hud         mVfr            = new Msg_vfr_hud();   // heading, altitude
        private Msg_raw_pressure    mRawPressure    = new Msg_raw_pressure();
        private Msg_scaled_pressure mScaledPressure = new Msg_scaled_pressure();
        private Msg_command_ack     mCommandAck     = new Msg_command_ack();
        private Msg_statustext      mStatusText     = new Msg_statustext();
        private Msg_mission_count   mMissionCount   = new Msg_mission_count();

        public bool _is_leader = false;

        private byte _base_mode = 0;
        public byte _is_armed = 0;

        /**
         * Variables for agent's location.
         */
        public Vector3 Position;
        //public Position_t Position;
        public double pRatio = 10000;

        /**
         * Variables for desired direction.
         */
        public Vector3 Direction;

        public MAVLinkNode(string port, int baud, byte SYSTEM_ID=1, byte COMPONENT_ID=1)
        {
            Console.WriteLine("MAVLinkNode::Constructor");

            mavlink = new Mavlink();
            mavlink.PacketReceived += OnMAVPacketReceive;

            Position = new Vector3();
            //Position = new Position_t();
            Direction = new Vector3();

            this.SYSTEM_ID      = SYSTEM_ID;
            this.COMPONENT_ID   = COMPONENT_ID;

            Serial = new System.IO.Ports.SerialPort()
            {
                PortName = port,
                BaudRate = baud
            };
            Serial.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(OnSerialReceived);
            

            heartbeatWorker = new System.ComponentModel.BackgroundWorker();
            heartbeatWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(OnHeartbeat);
            //heartbeatWorker.RunWorkerAsync();

            //System.Threading.Thread thread = new System.Threading.Thread(OnHeartbeat);
            //thread.Start();
        }

        public void OnHeartbeat(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            int sec = 0;    // Thread.sleep(1000);

            while (true)
            {
                Msg_heartbeat hb = new Msg_heartbeat();
                // Console.WriteLine("OnHeartbeat@" + ++sec);
                if (sec != DateTime.Now.Second)
                {
                    hb.type = (byte) MAV_TYPE.MAV_TYPE_GCS;
                    hb.system_status = 0;
                    hb.custom_mode = 0;
                    // hb.base_mode = 0;
                    hb.base_mode = _base_mode;
                    hb.autopilot = 0;

                    SendPacket(hb);
                    sec = DateTime.Now.Second;
                }
                // System.Threading.Thread.Sleep(1000);
            }
        }

        private void OnMAVPacketReceive(object sender, MavlinkPacket packet)
        {
            //if (packet.SystemId != SYSTEM_ID || packet.ComponentId != COMPONENT_ID)
            //    return;

            uint psize = mavlink.PacketsReceived;
            SYSTEM_ID = (byte) packet.SystemId;
            COMPONENT_ID = (byte) packet.ComponentId;
            PacketSequence = packet.SequenceNumber;
            // Console.WriteLine("Sequence #: " + PacketSequence);

            MavlinkMessage message = packet.Message;

            if (message.GetType() == mHeartbeat.GetType())
                mHeartbeat = (Msg_heartbeat)message;
            else if (message.GetType() == mSysStatus.GetType())
                mSysStatus = (Msg_sys_status)message;
            else if (message.GetType() == mPowerStatus.GetType())
                mPowerStatus = (Msg_power_status)message;
            else if (message.GetType() == mAttitude.GetType())
                mAttitude = (Msg_attitude)message;
            else if (message.GetType() == mGPS.GetType())
            {
                mGPS = (Msg_gps_raw_int) message;
                Position.X = mGPS.lat / pRatio;
                Position.Y = mGPS.lon / pRatio;
                Position.Z = mGPS.alt / pRatio;
                //Position.Latitude   = (double) mGPS.lat / pRatio;
                //Position.Longitude  = (double) mGPS.lon / pRatio;
                //Position.Altitude   = (double) mGPS.alt / pRatio;
            }
            else if (message.GetType() == mVfr.GetType())
                mVfr = (Msg_vfr_hud)message;
            else if (message.GetType() == mRawPressure.GetType())
                mRawPressure = (Msg_raw_pressure)message;
            else if (message.GetType() == mScaledPressure.GetType())    // TODO: Log press_abs, temperature, press_diff
                mScaledPressure = (Msg_scaled_pressure)message;
            else if (message.GetType() == mCommandAck.GetType())
                mCommandAck = (Msg_command_ack)message;
            else if (message.GetType() == mStatusText.GetType())        // TODO: System status message
                mStatusText = (Msg_statustext)message;
            else if (message.GetType() == mMissionCount.GetType())      // TODO: Handle mission
                mMissionCount = (Msg_mission_count)message;


            _is_armed = (byte) (mHeartbeat.base_mode & (byte) MAV_MODE_FLAG.MAV_MODE_FLAG_SAFETY_ARMED);
            // Console.WriteLine("[ARMED]: " + (_is_armed != 0));

            if (mStatusText.text != null)
            {
                int tsize = mStatusText.text.Length;
                char[] c = new char[tsize];
                for (int i = 0; i < tsize; i++) c[i] = (char) mStatusText.text[i];
                Console.WriteLine("[STATUS_MESSAGE]: " + new string(c));
            }
            Console.WriteLine("[COMMAND_RESULT_MESSAGE]: " + ResultMessage[mCommandAck.result]);

            _base_mode = mHeartbeat.base_mode;
        }

        private void OnSerialReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            int bsize = Serial.BytesToRead;
            byte[] bytes = new byte[bsize];
            for (int i = 0; i < bsize; i++)
                bytes[i] = (byte)Serial.ReadByte();
            mavlink.ParseBytes(bytes);
        }

        private void SendPacket(MavlinkMessage message)
        {
            MavlinkPacket packet = new MavlinkPacket()
            {
                Message = message,
                SequenceNumber = (byte) PacketSequence,
                SystemId = 255,
                ComponentId = (byte) MAV_COMPONENT.MAV_COMP_ID_MISSIONPLANNER
            };
            byte[] bytes = mavlink.Send(packet);
            Serial.Write(bytes, 0, bytes.Length);
        }

        public void SetFlightMode(uint mode)
        {
            Msg_set_mode message = new Msg_set_mode()
            {
                base_mode       = (byte) MAV_MODE_FLAG.MAV_MODE_FLAG_CUSTOM_MODE_ENABLED,
                custom_mode     = mode << 16,
                target_system   = SYSTEM_ID
            };
            SendPacket(message);
        }

        public void ArmDisarmCommand(bool target_arm)
        {
            Msg_command_long message = new Msg_command_long()
            {
                command             = (ushort) MAV_CMD.MAV_CMD_COMPONENT_ARM_DISARM,
                target_system       = SYSTEM_ID,
                target_component    = COMPONENT_ID,
                param1              = target_arm ? 1f : 0f  // 1: arm, 0: disarm
            };
            
            do
            {
                SendPacket(message);
                System.Threading.Thread.Sleep(1000);
            } while (target_arm ^ (_is_armed == 0b1000_0000));
        }

        public void TakeoffCommand()
        {
            Msg_mission_clear_all clearMessage = new Msg_mission_clear_all()
            {
                target_system       = SYSTEM_ID,
                target_component    = COMPONENT_ID
            };
            SendPacket(clearMessage);

            Msg_mission_item message = new Msg_mission_item()
            {
                autocontinue    = 1,
                command         = (byte) MAV_CMD.MAV_CMD_NAV_TAKEOFF,
                current         = 0,
                frame           = (byte) MAV_FRAME.MAV_FRAME_GLOBAL_RELATIVE_ALT,
                seq             = 1
            };
            SendPacket(message);
        }

        public void LandCommand()
        {
            Msg_mission_clear_all clearMessage = new Msg_mission_clear_all()
            {
                target_system       = SYSTEM_ID,
                target_component    = COMPONENT_ID
            };
            SendPacket(clearMessage);

            Msg_mission_item message = new Msg_mission_item()
            {
                autocontinue    = 1,
                command         = (byte) MAV_CMD.MAV_CMD_NAV_LAND,
                current         = 0,
                frame           = (byte) MAV_FRAME.MAV_FRAME_GLOBAL_RELATIVE_ALT,
                seq             = 1
            };
            SendPacket(message);
        }

        public void NextWP()
        {
            Msg_mission_item message = new Msg_mission_item()
            {
                seq             = 1,
                command         = (byte) MAV_CMD.MAV_CMD_NAV_WAYPOINT,
                frame           = (byte) MAV_FRAME.MAV_FRAME_GLOBAL_RELATIVE_ALT,
                autocontinue    = 1,
                current         = 0,
                param1          = 2,
                x               = 0,
                y               = 0,
                z               = 5
            };
            SendPacket(message);
        }
    }
}
