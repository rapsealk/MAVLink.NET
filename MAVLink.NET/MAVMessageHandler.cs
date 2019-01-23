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
                    mavlinkNode.UpdateBaseMode(mHeartbeat.base_mode);
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
                    mavlinkNode.UpdateLocalNED(mLocalPositionNED.x, mLocalPositionNED.y, mLocalPositionNED.z);
                    break;
                case MessageType.COMMAND_ACK:
                    mCommandAck = message as Msg_command_ack;
                    mavlinkNode.UpdateCommandResultMessage(mCommandAck.result);
                    break;
                case MessageType.STATUSTEXT:
                    mStatusText = message as Msg_statustext;
                    mavlinkNode.UpdateStatusText(mStatusText.text);
                    break;
                case MessageType.MISSION_COUNT:
                    mMissionCount = message as Msg_mission_count;
                    mavlinkNode.UpdateMissionCount(mMissionCount.count);
                    break;
                case MessageType.MISSION_ITEM:
                    mMissionItem = message as Msg_mission_item;
                    mavlinkNode.OnMissionItemMessage(mMissionItem.seq);
                    break;
                case MessageType.MISSION_CURRENT:
                    mMissionCurrent = message as Msg_mission_current;
                    mavlinkNode.OnMissionCurrentMessage(mMissionCurrent.seq);
                    break;
                case MessageType.MISSION_REQUEST:
                    mMissionRequest = message as Msg_mission_request;
                    mavlinkNode.OnMissionRequestMessage(mMissionRequest.seq);
                    break;
                case MessageType.MISSION_ACK:
                    mMissionAck = message as Msg_mission_ack;
                    mavlinkNode.OnMissionAckMessage(mMissionAck.type);
                    break;
                case MessageType.MISSION_ITEM_REACHED:
                    mMissionItemReached = message as Msg_mission_item_reached;
                    mavlinkNode.OnMissionItemReachedMessage(mMissionItemReached.seq);
                    break;
                default:
                    Console.WriteLine("Message type not found.");
                    break;
            }
        }
    }
}
