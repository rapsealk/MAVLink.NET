using System;
using System.Collections.Generic;
using MavLink;

namespace MAVLink.NET
{
    class MAVMessageHandler
    {
        private MAVLinkNode mavlinkNode;

        private Msg_heartbeat               mHeartbeat              = new Msg_heartbeat();
        private Msg_battery_status          mBatteryStatus          = new Msg_battery_status();
        private Msg_attitude                mAttitude               = new Msg_attitude();
        private Msg_gps_raw_int             mGpsRaw                 = new Msg_gps_raw_int();
        private Msg_gps_rtk                 mGpsRtk                 = new Msg_gps_rtk();
        private Msg_vfr_hud                 mVfr                    = new Msg_vfr_hud();
        private Msg_home_position           mHomePosition           = new Msg_home_position();
        private Msg_local_position_ned      mLocalPositionNED       = new Msg_local_position_ned();
        private Msg_raw_pressure            mRawPressure            = new Msg_raw_pressure();
        private Msg_scaled_pressure         mScaledPressure         = new Msg_scaled_pressure();
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
            RAW_PRESSURE, SCALED_PRESSURE,
            COMMAND_ACK, STATUSTEXT,
            MISSION_COUNT, MISSION_ITEM, MISSION_CURRENT, MISSION_REQUEST, MISSION_ACK, MISSION_ITEM_REACHED
        }

        public MAVMessageHandler(MAVLinkNode node)
        {
            mavlinkNode = node;

            /**
             * Table for switch-case statement.
             */
            mDictionary[mHeartbeat.GetType()]           = MessageType.HEARTBEAT;
            mDictionary[mBatteryStatus.GetType()]       = MessageType.BATTERY_STATUS;
            mDictionary[mAttitude.GetType()]            = MessageType.ATTITUDE;
            mDictionary[mGpsRaw.GetType()]              = MessageType.GPS_RAW_INT;
            mDictionary[mGpsRtk.GetType()]              = MessageType.GPS_RTK;
            mDictionary[mVfr.GetType()]                 = MessageType.VFR_HUD;
            mDictionary[mHomePosition.GetType()]        = MessageType.HOME_POSITION;
            mDictionary[mLocalPositionNED.GetType()]    = MessageType.LOCAL_POSITION_NED;
            mDictionary[mRawPressure.GetType()]         = MessageType.RAW_PRESSURE;
            mDictionary[mScaledPressure.GetType()]      = MessageType.SCALED_PRESSURE;
            mDictionary[mCommandAck.GetType()]          = MessageType.COMMAND_ACK;
            mDictionary[mStatusText.GetType()]          = MessageType.STATUSTEXT;
            mDictionary[mMissionCount.GetType()]        = MessageType.MISSION_COUNT;
            mDictionary[mMissionItem.GetType()]         = MessageType.MISSION_ITEM;
            mDictionary[mMissionCurrent.GetType()]      = MessageType.MISSION_CURRENT;
            mDictionary[mMissionRequest.GetType()]      = MessageType.MISSION_REQUEST;
            mDictionary[mMissionAck.GetType()]          = MessageType.MISSION_ACK;
            mDictionary[mMissionItemReached.GetType()]  = MessageType.MISSION_ITEM_REACHED;
        }

        public void Handle(MavlinkMessage message)
        {
            switch (mDictionary[message.GetType()])
            {
                case MessageType.HEARTBEAT:
                    mHeartbeat = message as Msg_heartbeat;
                    //  0000 0000 0000 0000 | 0000 0000 0000 0000
                    // \_custom_/  \_base_/
                    uint offset         = mHeartbeat.custom_mode >> 16;
                    uint base_index     = offset % 256;
                    uint custom_index   = offset / 256;
                    // FlightMode = PX4Mode[mHeartbeat.custom_mode / 65536];
                    mavlinkNode.UpdatePX4Mode(base_index, custom_index);
                    break;
                case MessageType.BATTERY_STATUS:
                    mBatteryStatus = message as Msg_battery_status;
                    mavlinkNode.UpdateBatteryPercentage(mBatteryStatus.battery_remaining);
                    break;
                case MessageType.ATTITUDE:
                    mAttitude = message as Msg_attitude;
                    mavlinkNode.UpdateAttitude(mAttitude.roll, mAttitude.pitch, mAttitude.yaw);
                    break;
                case MessageType.GPS_RAW_INT:
                    mGpsRaw = message as Msg_gps_raw_int;
                    mavlinkNode.UpdateGpsRaw(mGpsRaw.lat, mGpsRaw.lon, mGpsRaw.alt, mGpsRaw.time_usec, mGpsRaw.satellites_visible);
                    break;
                case MessageType.GPS_RTK:
                    mGpsRtk = message as Msg_gps_rtk;
                    break;
                case MessageType.VFR_HUD:
                    mVfr = message as Msg_vfr_hud;
                    mavlinkNode.UpdateHeading(mVfr.heading);
                    break;
                case MessageType.HOME_POSITION:
                    mHomePosition = message as Msg_home_position;
                    mavlinkNode.UpdateHomePosition(mHomePosition.latitude, mHomePosition.longitude);
                    break;
                case MessageType.LOCAL_POSITION_NED:
                    mLocalPositionNED = message as Msg_local_position_ned;
                    break;
                case MessageType.RAW_PRESSURE:
                    break;
                case MessageType.SCALED_PRESSURE:
                    break;
                case MessageType.COMMAND_ACK:
                    break;
                case MessageType.STATUSTEXT:
                    break;
                case MessageType.MISSION_COUNT:
                    break;
                case MessageType.MISSION_ITEM:
                    break;
                case MessageType.MISSION_CURRENT:
                    break;
                case MessageType.MISSION_REQUEST:
                    break;
                case MessageType.MISSION_ACK:
                    break;
                case MessageType.MISSION_ITEM_REACHED:
                    break;
                default:
                    Console.WriteLine("Message type not found.");
                    break;
            }
            else if (message.GetType() == mLocalPositionNED.GetType())
            {
                mLocalPositionNED = (Msg_local_position_ned)message;
                LocalPosition.X = mLocalPositionNED.x;
                LocalPosition.Y = mLocalPositionNED.y;
                LocalPosition.Z = mLocalPositionNED.z;
            }
            else if (message.GetType() == mRawPressure.GetType())
                mRawPressure = (Msg_raw_pressure)message;
            else if (message.GetType() == mScaledPressure.GetType())    // TODO: Log press_abs, temperature, press_diff
                mScaledPressure = (Msg_scaled_pressure)message;
            else if (message.GetType() == mCommandAck.GetType())
            {
                mCommandAck = (Msg_command_ack)message;
                CommandResultMessage = ResultMessage[mCommandAck.result];
            }
            else if (message.GetType() == mStatusText.GetType())        // TODO: System status message
                mStatusText = (Msg_statustext)message;
            else if (message.GetType() == mMissionCount.GetType())      // Response to MISSION_REQUEST_LIST
            {
                mMissionCount = message as Msg_mission_count;
                MissionItemCount = mMissionCount.count;
                Console.WriteLine("[SYSTEM #{0:d}] Msg_mission_count: {1:d}", SYSTEM_ID, mMissionCount.count);

                Msg_mission_request requestMessage = new Msg_mission_request()
                {
                    target_system = SYSTEM_ID,
                    target_component = COMPONENT_ID,
                    seq = 0
                };
                SendPacket(requestMessage);
            }
            else if (message.GetType() == mMissionItem.GetType())
            {
                mMissionItem = message as Msg_mission_item;

                ushort sequenceNumber = mMissionItem.seq;
                MissionItems[sequenceNumber] = mMissionItem;

                if (sequenceNumber < MissionItemCount - 1)
                {
                    Msg_mission_request requestMessage = new Msg_mission_request()
                    {
                        target_system = SYSTEM_ID,
                        target_component = COMPONENT_ID,
                        seq = (ushort)(sequenceNumber + 1)
                    };
                    SendPacket(requestMessage);
                }
                else
                {
                    Msg_mission_ack ackMessage = new Msg_mission_ack()
                    {
                        target_system = SYSTEM_ID,
                        target_component = COMPONENT_ID,
                        type = (byte)MAV_MISSION_RESULT.MAV_MISSION_ACCEPTED
                    };
                }
            }
            else if (message.GetType() == mMissionCurrent.GetType())
            {
                mMissionCurrent = (Msg_mission_current)message;
                MissionCurrentSequence = mMissionCurrent.seq;
                Msg_mission_item currentItem = MissionItems[MissionCurrentSequence];
                if (_is_leader && currentItem != null)
                {
                    Direction.X = currentItem.x - Position.X;
                    Direction.Y = currentItem.y - Position.Y;
                    Direction.Z = currentItem.z - Position.Z;
                }
                Console.WriteLine("[SYSTEM #{0:d}] MissionCurrentSequence: " + MissionCurrentSequence, SYSTEM_ID);
                //DatabaseManager.UpdateNextCommand(SYSTEM_ID, MissionCurrentSequence);
            }
            else if (message.GetType() == mMissionRequest.GetType())
            {
                mMissionRequest = message as Msg_mission_request;
                ushort index = mMissionRequest.seq;
                Msg_mission_item itemMessage = MissionItems[index];
                itemMessage.seq = index;
                SendPacket(itemMessage);
            }
            else if (message.GetType() == mMissionAck.GetType())
            {
                mMissionAck = message as Msg_mission_ack;
                Console.WriteLine("[SYSTEM #{0:d}] Mission Ack: " + (mMissionAck.type == (byte)MAV_MISSION_RESULT.MAV_MISSION_ACCEPTED), SYSTEM_ID);

                // Follower: set_position as a single mission.
                if (!_is_leader)
                {
                    Msg_command_long smessage = new Msg_command_long()
                    {
                        target_system = SYSTEM_ID,
                        target_component = COMPONENT_ID,
                        command = (ushort)MAV_CMD.MAV_CMD_MISSION_START
                        // param1 = first_item: the first mission item to run
                        // param2 = last_item: the last mission item to run (after this item is run, the mission ends)
                    };
                    SendPacket(smessage);
                }
            }
            else if (message.GetType() == mMissionItemReached.GetType())
            {
                mMissionItemReached = (Msg_mission_item_reached)message;
                MissionReachedSequence = mMissionItemReached.seq;
                Console.WriteLine("[SYSTEM #{0:d}] Mission reached: {1:d}", SYSTEM_ID, MissionReachedSequence);
            }
        }
    }
}
