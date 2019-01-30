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

        private readonly Mavlink mavlink;

        public System.IO.Ports.SerialPort SerialPort    = new System.IO.Ports.SerialPort();
        public System.Threading.Thread HeartbeatThread  = null;

        private MavlinkMessage[] messages = new MavlinkMessage[]
        {
            new Msg_heartbeat(),
            new Msg_battery_status(),
            new Msg_attitude(),
            new Msg_gps_raw_int(),
            new Msg_gps_rtk(),
            new Msg_vfr_hud(),
            new Msg_home_position(),
            new Msg_local_position_ned(),
            new Msg_command_ack(),
            new Msg_statustext(),
            new Msg_mission_count(),
            new Msg_mission_item(),
            new Msg_mission_current(),
            new Msg_mission_request(),
            new Msg_mission_ack(),
            new Msg_mission_item_reached()
        };

        /*
        private Msg_heartbeat               mHeartbeat          = new Msg_heartbeat();
        private Msg_battery_status          mBatteryStatus      = new Msg_battery_status();
        private Msg_attitude                mAttitude           = new Msg_attitude();
        private Msg_gps_raw_int             mGpsRaw             = new Msg_gps_raw_int();
        private Msg_gps_rtk                 mGpsRtk             = new Msg_gps_rtk();
        private Msg_vfr_hud                 mVfr                = new Msg_vfr_hud();
        private Msg_home_position           mHomePosition       = new Msg_home_position();
        private Msg_local_position_ned      mLocalPositionNED   = new Msg_local_position_ned();
        private Msg_command_ack             mCommandAck         = new Msg_command_ack();
        private Msg_statustext              mStatusText         = new Msg_statustext();
        private Msg_mission_count           mMissionCount       = new Msg_mission_count();
        private Msg_mission_item            mMissionItem        = new Msg_mission_item();
        private Msg_mission_current         mMissionCurrent     = new Msg_mission_current();
        private Msg_mission_request         mMissionRequest     = new Msg_mission_request();
        private Msg_mission_ack             mMissionAck         = new Msg_mission_ack();
        private Msg_mission_item_reached    mMissionItemReached = new Msg_mission_item_reached();
        */

        private readonly IDictionary<Type, MessageType> mDictionary = new Dictionary<Type, MessageType>();

        private enum MessageType
        {
            HEARTBEAT,
            BATTERY_STATUS,
            ATTITUDE,
            GPS_RAW_INT,
            GPS_RTK,
            VFR_HUD,
            HOME_POSITION,
            LOCAL_POSITION_NED,
            COMMAND_ACK,
            STATUSTEXT,
            MISSION_COUNT,
            MISSION_ITEM,
            MISSION_CURRENT,
            MISSION_REQUEST,
            MISSION_ACK,
            MISSION_ITEM_REACHED
        }

        public MAVLinkController(MAVLinkModel model, MAVLinkView view)
        {
            Model = model;
            View = view;

            View.Controller = this;
            View.Publisher = Model;

            mavlink = new Mavlink();
            mavlink.PacketReceived += OnMAVPacketReceive;

            /*
            mDictionary[mHeartbeat.GetType()]           = MessageType.HEARTBEAT;
            mDictionary[mBatteryStatus.GetType()]       = MessageType.BATTERY_STATUS;
            mDictionary[mAttitude.GetType()]            = MessageType.ATTITUDE;
            mDictionary[mGpsRaw.GetType()]              = MessageType.GPS_RAW_INT;
            mDictionary[mGpsRtk.GetType()]              = MessageType.GPS_RTK;
            mDictionary[mVfr.GetType()]                 = MessageType.VFR_HUD;
            mDictionary[mHomePosition.GetType()]        = MessageType.HOME_POSITION;
            mDictionary[mLocalPositionNED.GetType()]    = MessageType.LOCAL_POSITION_NED;
            mDictionary[mCommandAck.GetType()]          = MessageType.COMMAND_ACK;
            mDictionary[mStatusText.GetType()]          = MessageType.STATUSTEXT;
            mDictionary[mMissionCount.GetType()]        = MessageType.MISSION_COUNT;
            mDictionary[mMissionItem.GetType()]         = MessageType.MISSION_ITEM;
            mDictionary[mMissionCurrent.GetType()]      = MessageType.MISSION_CURRENT;
            mDictionary[mMissionRequest.GetType()]      = MessageType.MISSION_REQUEST;
            mDictionary[mMissionAck.GetType()]          = MessageType.MISSION_ACK;
            mDictionary[mMissionItemReached.GetType()]  = MessageType.MISSION_ITEM_REACHED;
            */
            for (int i = 0; i < messages.Length; ++i)
                mDictionary[messages[i].GetType()] = (MessageType) i;
        }

        public void SerialConnect()
        {
            //View.EnableSerialConnectButton(false);
            
            string portName = View.SerialPortName;

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

        private void OnSerialReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            int bsize = SerialPort.BytesToRead;
            byte[] bytes = new byte[bsize];
            for (int i = 0; i < bsize; ++i)
                bytes[i] = (byte) SerialPort.ReadByte();
            mavlink.ParseBytes(bytes);
        }

        private void OnMAVPacketReceive(object sender, MavlinkPacket packet)
        {
            uint psize = mavlink.PacketsReceived;

            /*
             * FIXME: Sik Radio issue
             * https://groups.google.com/forum/#!msg/drones-discuss/w_iuoVnA7K4/GRr1iUyJAwAJ
             * https://github.com/ArduPilot/SiK/blob/master/Firmware/radio/mavlink.c#L51
             */
            if (packet.SystemId == 51)
            {
                return;
            }

            Model.SystemId          = (byte) packet.SystemId;
            Model.ComponentId       = (byte) packet.ComponentId;
            Model.PacketSequence    = packet.SequenceNumber;

            Console.WriteLine("Model.SystemId: " + packet.SystemId);
            Console.WriteLine("Model.ComponentId: " + packet.ComponentId);
            Console.WriteLine("Model.PacketSequence: " + packet.SequenceNumber);

            HandleMessage(packet.Message);
        }

        private void HandleMessage(MavlinkMessage message)
        {
            Type messageType = message.GetType();

            Console.WriteLine("messageType: {0:s}", messageType);

            if (!mDictionary.Keys.Contains(messageType))
                return;

            switch (mDictionary[messageType])
            {
                case MessageType.HEARTBEAT:
                    Msg_heartbeat heartbeatMessage = message as Msg_heartbeat;
                    //  0000 0000 0000 0000 | 0000 0000 0000 0000
                    //  \_custom_/ \_base_/
                    byte baseMode   = heartbeatMessage.base_mode;
                    uint customMode = heartbeatMessage.custom_mode;
                    uint offset         = customMode >> 16;
                    uint baseIndex      = offset % 256;
                    uint customIndex    = offset / 256;
                    // FlightMode = PX4Mode[mHeartbeat.custom_mode / (2 ^ 16)];
                    //UpdatePX4Mode(baseIndex, customIndex);
                    //UpdateArmState(baseMode);
                    break;
                case MessageType.BATTERY_STATUS:
                    Msg_battery_status batteryStatusMessage = message as Msg_battery_status;
                    //UpdateBatteryPercentage(mBatteryStatus.battery_remaining);
                    break;
                case MessageType.ATTITUDE:
                    Msg_attitude attitudeMessage = message as Msg_attitude;
                    //UpdateAttitude(mAttitude.roll, mAttitude.pitch, mAttitude.yaw);
                    break;
                case MessageType.GPS_RAW_INT:
                    Msg_gps_raw_int gpsRawIntMessage = message as Msg_gps_raw_int;
                    //UpdateGpsRaw(mGpsRaw.lat, mGpsRaw.lon, mGpsRaw.alt, mGpsRaw.time_usec, mGpsRaw.satellites_visible);
                    break;
                case MessageType.GPS_RTK:
                    Msg_gps_rtk gpsRtkMessage = message as Msg_gps_rtk;
                    break;
                case MessageType.VFR_HUD:
                    Msg_vfr_hud vfrHudMessage = message as Msg_vfr_hud;
                    //UpdateHeading(mVfr.heading);
                    break;
                case MessageType.HOME_POSITION:
                    Msg_home_position homePositionMessage = message as Msg_home_position;
                    //UpdateHome(mHomePosition.latitude, mHomePosition.longitude);
                    break;
                case MessageType.LOCAL_POSITION_NED:
                    Msg_local_position_ned localPositionNed = message as Msg_local_position_ned;
                    //UpdateLocalNED(mLocalPositionNED.x, mLocalPositionNED.y, mLocalPositionNED.z);
                    break;
                case MessageType.COMMAND_ACK:
                    Msg_command_ack commandAckMessage = message as Msg_command_ack;
                    //UpdateCommandAckMessage(mCommandAck.result);
                    break;
                case MessageType.STATUSTEXT:
                    Msg_statustext statusTextMessage = message as Msg_statustext;
                    //UpdateStatusText(mStatusText.text);
                    break;
                case MessageType.MISSION_COUNT:
                    Msg_mission_count missionCountMessage = message as Msg_mission_count;
                    //UpdateMissionCount(mMissionCount.count);
                    break;
                case MessageType.MISSION_ITEM:
                    Msg_mission_item missionItemMessage = message as Msg_mission_item;
                    //DownloadMissionItem(mMissionItem);
                    break;
                case MessageType.MISSION_CURRENT:
                    Msg_mission_current missionCurrentMessage = message as Msg_mission_current;
                    //UpdateCurrentMission(mMissionCurrent.seq);
                    break;
                case MessageType.MISSION_REQUEST:
                    Msg_mission_request missionRequestMessage = message as Msg_mission_request;
                    //UploadMissionItem(mMissionRequest.seq);
                    break;
                case MessageType.MISSION_ACK:
                    Msg_mission_ack missionAckMessage = message as Msg_mission_ack;
                    //OnMissionAckMessage(mMissionAck.type);
                    break;
                case MessageType.MISSION_ITEM_REACHED:
                    Msg_mission_item_reached missionItemReachedMessage = message as Msg_mission_item_reached;
                    //UpdateReachedMission(mMissionItemReached.seq);
                    break;
                default:
                    Console.WriteLine("Message type not found.");
                    break;
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
            byte[] bytes = mavlink.Send(packet);
            SerialPort.Write(bytes, 0, bytes.Length);
        }

        public void ArmDisarmCommand(bool doArm)
        {
            bool isArmed = (Model.ArmState == (byte) MAV_MODE_FLAG.MAV_MODE_FLAG_SAFETY_ARMED);
            if (isArmed == doArm) return;

            System.Threading.Thread thread = new System.Threading.Thread(() =>
            {
                //View.EnableArmDisarmButton(false);

                Msg_command_long message = new Msg_command_long()
                {
                    command             = (ushort) MAV_CMD.MAV_CMD_COMPONENT_ARM_DISARM,
                    target_system       = Model.SystemId,
                    target_component    = Model.ComponentId,
                    param1              = doArm ? 1f : 0f  // 1: arm, 0: disarm
                };

                int trial = 0;
                do
                {
                    SendPacket(message);
                    System.Threading.Thread.Sleep(1000);
                } while (doArm ^ (Model.ArmState == (byte) MAV_MODE_FLAG.MAV_MODE_FLAG_SAFETY_ARMED) && ++trial < 5);

                //View.EnableArmDisarmButton(true);
            });
            thread.Start();
        }

        /**
         * https://mavlink.io/en/messages/common.html#MAV_CMD_NAV_TAKEOFF
         */
        public void TakeoffCommand(float altitudeInMeters = 5f)
        {
            Msg_command_long takeoffMessage = new Msg_command_long()
            {
                command = (ushort)MAV_CMD.MAV_CMD_NAV_TAKEOFF,
                target_system       = Model.SystemId,
                target_component    = Model.ComponentId,
                param1              = 2.5f, // Minimum pitch
                // param2
                param3              = .1f,   // horizontal navigation by pilot acceptable
                // param4: yaw angle    (not supported)
                param5              = Model.GlobalPosition.X,   // latitude
                param6              = Model.GlobalPosition.Y,   // longitude
                param7              = altitudeInMeters          // altitude [meters]
            };
            SendPacket(takeoffMessage);

            ArmDisarmCommand(true);
        }

        /**
         * https://mavlink.io/en/messages/common.html#MAV_CMD_NAV_LAND
         */
        public void LandCommand()
        {
            Msg_command_long landMessage = new Msg_command_long()
            {
                command             = (ushort) MAV_CMD.MAV_CMD_NAV_LAND,
                target_system       = Model.SystemId,
                target_component    = Model.ComponentId,
                // param1: Abort Alt
                // param2: Precision land mode. (0 = normal landing, 1 = opportunistic precision landing, 2 = required precision landing)
                // param3: Empty
                // param4: Desired yaw angle. NaN for unchanged.
                param5              = Model.GlobalPosition.X,   // Latitude
                param6              = Model.GlobalPosition.Y    // Longitude
                // param7: Altitude (ground level)
            };
            SendPacket(landMessage);
        }
    }
}
