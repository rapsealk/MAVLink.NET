using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MavLink;
using MAVLink.NET.Model;

namespace MAVLink.NET.Controller
{
    class MAVLinkController
    {
        private Mavlink mavlink;

        private MAVLinkModel model;

        public System.IO.Ports.SerialPort Serial;
        public System.Threading.Thread heartbeatThread;

        private readonly string PortName;
        private readonly int baudRate = 57600;

        private Msg_heartbeat               mHeartbeat              = new Msg_heartbeat();
        private Msg_battery_status          mBatteryStatus          = new Msg_battery_status();
        private Msg_attitude                mAttitude               = new Msg_attitude();
        private Msg_gps_raw_int             mGpsRaw                 = new Msg_gps_raw_int();
        private Msg_gps_rtk                 mGpsRtk                 = new Msg_gps_rtk();
        private Msg_vfr_hud                 mVfr                    = new Msg_vfr_hud();
        private Msg_home_position           mHomePosition           = new Msg_home_position();
        private Msg_local_position_ned      mLocalPositionNED       = new Msg_local_position_ned();
        private Msg_command_ack             mCommandAck             = new Msg_command_ack();
        private Msg_statustext              mStatusText             = new Msg_statustext();
        private Msg_mission_count           mMissionCount           = new Msg_mission_count();
        private Msg_mission_item            mMissionItem            = new Msg_mission_item();
        private Msg_mission_current         mMissionCurrent         = new Msg_mission_current();
        private Msg_mission_request         mMissionRequest         = new Msg_mission_request();
        private Msg_mission_ack             mMissionAck             = new Msg_mission_ack();
        private Msg_mission_item_reached    mMissionItemReached     = new Msg_mission_item_reached();

        private readonly IDictionary<Type, MessageType> mDictionary = new Dictionary<Type, MessageType>();

        private enum MessageType
        {
            HEARTBEAT, BATTERY_STATUS,
            ATTITUDE, GPS_RAW_INT, GPS_RTK, VFR_HUD,
            HOME_POSITION, LOCAL_POSITION_NED,
            COMMAND_ACK, STATUSTEXT,
            MISSION_COUNT, MISSION_ITEM, MISSION_CURRENT, MISSION_REQUEST, MISSION_ACK, MISSION_ITEM_REACHED
        }

        public MAVLinkController(string portName)
        {
            mavlink = new Mavlink();

            model = new MAVLinkModel();
            PortName = portName;

            Serial = new System.IO.Ports.SerialPort()
            {
                PortName = portName,
                BaudRate = baudRate
            };
            Serial.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(OnSerialDataReceive);

            Serial.Open();

            heartbeatThread = new System.Threading.Thread(() => {
                while (true)
                {
                    Msg_heartbeat heartbeatMessage = new Msg_heartbeat()
                    {
                        type            = (byte)MAV_TYPE.MAV_TYPE_GCS
                        /*
                        system_status   = 0,
                        base_mode       = 0,
                        custom_mode     = 0,
                        autopilot       = 0
                        */
                    };
                    SendPacket(heartbeatMessage);
                    System.Threading.Thread.Sleep(1000);
                }
            });
            heartbeatThread.Start();
        }

        ~MAVLinkController()
        {
            Serial.Close();
        }

        private void SendPacket(MavlinkMessage message)
        {
            MavlinkPacket packet = new MavlinkPacket()
            {
                Message         = message,
                SequenceNumber  = model.PacketSequence,
                SystemId        = 255,
                ComponentId     = (byte) MAV_COMPONENT.MAV_COMP_ID_MISSIONPLANNER
            };
            byte[] bytes = mavlink.Send(packet);
            Serial.Write(bytes, 0, bytes.Length);
        }

        private void OnSerialDataReceive(object sender, System.IO.Ports.SerialDataReceivedEventArgs args)
        {
            int bsize = Serial.BytesToRead;
            byte[] bytes = new byte[bsize];
            for (int i = 0; i < bsize; ++i)
                bytes[i] = (byte) Serial.ReadByte();
            mavlink.ParseBytes(bytes);
        }

        private void OnMAVPacketReceive(object sender, MavlinkPacket packet)
        {
            /*
             * FIXME: Sik Radio issue
             * https://groups.google.com/forum/#!msg/drones-discuss/w_iuoVnA7K4/GRr1iUyJAwAJ
             * https://github.com/ArduPilot/SiK/blob/master/Firmware/radio/mavlink.c#L51
             */
            if (packet.SystemId == 51)
            {
                return;
            }

            uint psize = mavlink.PacketsReceived;

            model.SystemId          = (byte) packet.SystemId;
            model.ComponentId       = (byte) packet.ComponentId;

            model.PacketSequence    = packet.SequenceNumber;

            HandleMessage(packet.Message);
        }

        private void HandleMessage(MavlinkMessage message)
        {
            switch (mDictionary[message.GetType()])
            {
                case MessageType.HEARTBEAT:
                    mHeartbeat = message as Msg_heartbeat;
                    //  0000 0000 0000 0000 | 0000 0000 0000 0000
                    // \_custom_/  \_base_/
                    byte baseMode       = mHeartbeat.base_mode;
                    uint customMode     = mHeartbeat.custom_mode;
                    uint offset         = customMode >> 16;
                    uint baseIndex      = offset % 256;
                    uint customIndex    = offset / 256;
                    // FlightMode = PX4Mode[mHeartbeat.custom_mode / (2 ^ 16)];
                    UpdatePX4Mode(baseIndex, customIndex);
                    UpdateArmState(baseMode);
                    break;
                case MessageType.BATTERY_STATUS:
                    mBatteryStatus = message as Msg_battery_status;
                    UpdateBatteryPercentage(mBatteryStatus.battery_remaining);
                    break;
                case MessageType.ATTITUDE:
                    mAttitude = message as Msg_attitude;
                    UpdateAttitude(mAttitude.roll, mAttitude.pitch, mAttitude.yaw);
                    break;
                case MessageType.GPS_RAW_INT:
                    mGpsRaw = message as Msg_gps_raw_int;
                    UpdateGpsRaw(mGpsRaw.lat, mGpsRaw.lon, mGpsRaw.alt, mGpsRaw.time_usec, mGpsRaw.satellites_visible);
                    break;
                case MessageType.GPS_RTK:
                    mGpsRtk = message as Msg_gps_rtk;
                    break;
                case MessageType.VFR_HUD:
                    mVfr = message as Msg_vfr_hud;
                    UpdateHeading(mVfr.heading);
                    break;
                case MessageType.HOME_POSITION:
                    mHomePosition = message as Msg_home_position;
                    UpdateHome(mHomePosition.latitude, mHomePosition.longitude);
                    break;
                case MessageType.LOCAL_POSITION_NED:
                    mLocalPositionNED = message as Msg_local_position_ned;
                    UpdateLocalNED(mLocalPositionNED.x, mLocalPositionNED.y, mLocalPositionNED.z);
                    break;
                case MessageType.COMMAND_ACK:
                    mCommandAck = message as Msg_command_ack;
                    UpdateCommandAckMessage(mCommandAck.result);
                    break;
                case MessageType.STATUSTEXT:
                    mStatusText = message as Msg_statustext;
                    UpdateStatusText(mStatusText.text);
                    break;
                case MessageType.MISSION_COUNT:
                    mMissionCount = message as Msg_mission_count;
                    UpdateMissionCount(mMissionCount.count);
                    break;
                case MessageType.MISSION_ITEM:
                    mMissionItem = message as Msg_mission_item;
                    DownloadMissionItem(mMissionItem);
                    break;
                case MessageType.MISSION_CURRENT:
                    mMissionCurrent = message as Msg_mission_current;
                    UpdateCurrentMission(mMissionCurrent.seq);
                    break;
                case MessageType.MISSION_REQUEST:
                    mMissionRequest = message as Msg_mission_request;
                    UploadMissionItem(mMissionRequest.seq);
                    break;
                case MessageType.MISSION_ACK:
                    mMissionAck = message as Msg_mission_ack;
                    OnMissionAckMessage(mMissionAck.type);
                    break;
                case MessageType.MISSION_ITEM_REACHED:
                    mMissionItemReached = message as Msg_mission_item_reached;
                    UpdateReachedMission(mMissionItemReached.seq);
                    break;
                default:
                    Console.WriteLine("Message type not found.");
                    break;
            }
        }

        /**
         * Messages
         */
        private void SendMissionRequestMessage(ushort sequenceNumber)
        {
            Msg_mission_request missionRequestMessage = new Msg_mission_request()
            {
                target_system       = model.SystemId,
                target_component    = model.ComponentId,
                seq                 = sequenceNumber
            };
            SendPacket(missionRequestMessage);
        }

        private void UpdatePX4Mode(uint baseMode, uint customMode)
        {
            model.FlightMode    = MAVLinkModel.PX4Mode[baseMode];
            model.SubMode       = MAVLinkModel.PX4SubMode[customMode];
            //DatabaseManager.UpdateFlightMode(SYSTEM_ID, FlightMode);
        }

        private void UpdateArmState(byte baseMode)
        {
            model.ArmState = (byte) (baseMode & (byte) MAV_MODE_FLAG.MAV_MODE_FLAG_SAFETY_ARMED);
        }

        private void UpdateBatteryPercentage(sbyte batteryPercentage)
        {
            model.BatteryPercentage = batteryPercentage;
            //DatabaseManager.UpdateBattery(SYSTEM_ID, BatteryPercentage);
        }

        private void UpdateAttitude(float roll, float pitch, float yaw)
        {
            model.Roll  = Constant.RADIAN * roll;
            model.Pitch = Constant.RADIAN * pitch;
            model.Yaw   = Constant.RADIAN * yaw;
        }

        private void UpdateGpsRaw(int latitude, int longitude, int altitude, ulong useconds, byte nSatellites)
        {
            model.GlobalPosition.X  = (float) (latitude / Constant.GLOBAL_LOCAL_RATIO);
            model.GlobalPosition.Y  = (float) (longitude / Constant.GLOBAL_LOCAL_RATIO);
            model.GlobalPosition.Z  = (float) (altitude / Constant.GLOBAL_LOCAL_RATIO);
            model.GpsUnixTimestamp  = useconds;
            model.SatelliteNumber   = nSatellites;
            //DatabaseManager.UpdatePosition(SYSTEM_ID, Position.X, Position.Y, Position.Z, SatelliteNumber, Gtimestamp);
        }

        private void UpdateHeading(short heading)
        {
            model.HeadingDirection = heading;
            //DatabaseManager.UpdateHeadingDirection(SYSTEM_ID, HeadingDirection);
        }

        private void UpdateHome(int latitude, int longitude)
        {
            model.HomePosition.X = (float) (latitude / Constant.GLOBAL_LOCAL_RATIO);
            model.HomePosition.Y = (float) (longitude / Constant.GLOBAL_LOCAL_RATIO);
        }

        private void UpdateLocalNED(float x, float y, float z)
        {
            model.LocalPosition.X = x;
            model.LocalPosition.Y = y;
            model.LocalPosition.Z = z;
        }

        private void UpdateCommandAckMessage(int index)
        {
            model.CommandResultMessage = MAVLinkModel.ResultMessage[index];
        }

        private void UpdateStatusText(byte[] text)
        {
            if (text == null)
                return;

            int tsize = text.Length;
            char[] c = new char[tsize];
            for (int i = 0; i < tsize; ++i)
                c[i] = (char) text[i];
            model.StatusMessage = new string(c);
        }

        // Response to MISSION_REQUEST_LIST.
        private void UpdateMissionCount(ushort count)
        {
            model.MissionItemCount = count;
            SendMissionRequestMessage(0);
        }

        // Mission download process.
        private void DownloadMissionItem(Msg_mission_item item)
        {
            model.MissionItems[item.seq] = item; //model.MissionItems.Add(item);

            // If received item is the last one..
            if (item.seq == model.MissionItemCount - 1)
            {
                Msg_mission_ack missionAckMessage = new Msg_mission_ack()
                {
                    target_system       = model.SystemId,
                    target_component    = model.ComponentId,
                    type                = (byte) MAV_MISSION_RESULT.MAV_MISSION_ACCEPTED
                };
                SendPacket(missionAckMessage);
            }
            else
            {
                SendMissionRequestMessage(++item.seq);
            }
        }

        private void UpdateCurrentMission(ushort sequenceNumber)
        {
            model.MissionCurrentSequence = sequenceNumber;

            Msg_mission_item currentItem = model.MissionItems[sequenceNumber];

            if (currentItem != null && model.IsLeader)
            {
                model.Direction.X = currentItem.x - model.GlobalPosition.X;
                model.Direction.Y = currentItem.y - model.GlobalPosition.Y;
                model.Direction.Z = currentItem.z - model.GlobalPosition.Z;
            }
            //DatabaseManager.UpdateNextCommand(SYSTEM_ID, MissionCurrentSequence);
        }

        private void UploadMissionItem(ushort sequenceNumber)
        {
            Msg_mission_item missionItemMessage = model.MissionItems[sequenceNumber];
            missionItemMessage.seq = sequenceNumber;
            SendPacket(missionItemMessage);
        }

        private void OnMissionAckMessage(byte result)
        {
            bool isAccepted = (result == (byte) MAV_MISSION_RESULT.MAV_MISSION_ACCEPTED);

            // FIXME: Followers are doing set_position as a single mission.
            if (isAccepted && !model.IsLeader)
            {
                Msg_command_long missionStartMessage = new Msg_command_long()
                {
                    target_system = model.SystemId,
                    target_component = model.ComponentId,
                    command = (ushort)MAV_CMD.MAV_CMD_MISSION_START
                    // param1: the first mission item to run
                    // param2: the last mission item to run (after this item is run, the mission ends.)
                };
                SendPacket(missionStartMessage);
            }
        }

        private void UpdateReachedMission(ushort sequenceNumber)
        {
            model.MissionReachedSequence = sequenceNumber;
        }
    }
}
