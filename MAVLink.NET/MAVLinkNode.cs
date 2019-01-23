using System;
using System.Collections.Generic;
using System.Numerics;
using MavLink;

namespace MAVLink.NET
{
    /**
     * The class MAVLinkNode is for having communication with UAV (Unmanned Aerial Vehicle) agent.
     * This class is implemented with MAVLink Protocol for PX4 based FCU (Flight Control Unit).
     * If you are not familiar to MAVLink, visit here (https://mavlink.io/en/).
     * 
     * The connection between the agent and this class is built through radio telemetry.
     * 
     * This class supports functions below -
     * 1) Track agent's current flight mode. (based on PX4 compatible modes)
     * 2) 
     */
    class MAVLinkNode
    {
        // PX4_CUSTOM_MAIN_MODE
        public static readonly string[] PX4Mode =
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
        public static readonly string[] PX4SubMode =
        {
            "(EMPTY)", "READY", "TAKEOFF", "LOITER", "MISSION", "RTL", "LAND",
            "RTGS", "FOLLOW_TARGET", "PRECLAND"
        };
        private enum PX4FlightSubMode
        {
            READY=1, TAKEOFF, LOITER, MISSION, RTL, LAND, RTGS, FOLLOW_TARGET, PRECLAND
        };
        // COMMAND_ACK
        private static readonly string[] ResultMessage =
        {
            "ACCEPTED", "TEMPORARILY REJECTED", "DENIED", "UNSUPPORTED", "FAILED", "IN PROGRESS"
        };

        private Mavlink mavlink;

        public System.IO.Ports.SerialPort Serial { get; private set; }
        public System.ComponentModel.BackgroundWorker heartbeatWorker;

        public string PortName;
        public int BaudRate;

        public byte SYSTEM_ID;
        public byte COMPONENT_ID;

        public byte PacketSequence { get; private set; }

        private readonly MAVMessageHandler MessageHandler;

        public bool IsLeader = false;

        private byte _base_mode = 0;
        public byte ArmState = 0;

        public sbyte BatteryPercentage = 0;

        // TODO: List<Msg_mission_item> with no limit.
        private readonly Msg_mission_item[] MissionItems = new Msg_mission_item[32];
        private UInt16 MissionItemCount = 0;
        private UInt16 MissionCurrentSequence = UInt16.MaxValue;
        private UInt16 MissionReachedSequence = UInt16.MaxValue;

        public string FlightMode            = "null";
        public string SubMode               = "null";

        public string StatusMessage         = "null";
        public string CommandResultMessage  = "null";

        public float Roll   = 0f;
        public float Pitch  = 0f;
        public float Yaw    = 0f;

        public short HeadingDirection = 0;

        /**
         * Variables for agent's location.
         */
        public Vector3 Position;
        public Vector3 HomePosition;
        public Vector3 LocalPosition;


        public ulong Gtimestamp     = 0;        // GPS UNIX Timestamp (start from boot)
        public byte SatelliteNumber = 0;        // Number of visible Satellite

        /**
         * Variables for desired direction.
         */
        public Vector3 Direction;


        public MAVLinkNode(string port, int baud)
        {
            mavlink = new Mavlink();
            mavlink.PacketReceived += OnMAVPacketReceive;

            Position        = new Vector3();
            HomePosition    = new Vector3();
            LocalPosition   = new Vector3();
            Direction       = new Vector3();

            MessageHandler = new MAVMessageHandler(this);

            this.PortName = port;
            this.BaudRate = baud;

            Serial = new System.IO.Ports.SerialPort()
            {
                PortName = port,
                BaudRate = baud
            };
            Serial.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(OnSerialReceived);

            heartbeatWorker = new System.ComponentModel.BackgroundWorker();
            heartbeatWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(OnHeartbeat);
        }

        public void OnHeartbeat(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            int sec = 0;    // Thread.sleep(1000);

            while (true)
            {
                Msg_heartbeat hb = new Msg_heartbeat();

                if (sec != DateTime.Now.Second)
                {
                    hb.type = (byte) MAV_TYPE.MAV_TYPE_GCS;
                    hb.system_status = 0;
                    hb.custom_mode = 0;
                    // hb.base_mode = 0;
                    // FIXME: Required?
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

            SYSTEM_ID       = (byte) packet.SystemId;
            COMPONENT_ID    = (byte) packet.ComponentId;
            PacketSequence  = packet.SequenceNumber;

            MessageHandler.Handle(packet.Message);
        }

        private void OnSerialReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            int bsize = Serial.BytesToRead;
            byte[] bytes = new byte[bsize];
            for (int i = 0; i < bsize; i++)
                bytes[i] = (byte) Serial.ReadByte();
            mavlink.ParseBytes(bytes);
        }

        private void SendPacket(MavlinkMessage message)
        {
            MavlinkPacket packet = new MavlinkPacket()
            {
                Message         = message,
                SequenceNumber  = PacketSequence,
                SystemId        = 255,
                ComponentId     = (byte) MAV_COMPONENT.MAV_COMP_ID_MISSIONPLANNER
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

        public bool ArmDisarmCommand(bool target_arm, System.Windows.Forms.Button button = null)
        {
            System.Threading.Thread thread = new System.Threading.Thread(() =>
            {
                if (button != null)
                    button.BeginInvoke((Action) delegate () { button.Enabled = false; });

                Msg_command_long message = new Msg_command_long()
                {
                    command             = (ushort) MAV_CMD.MAV_CMD_COMPONENT_ARM_DISARM,
                    target_system       = SYSTEM_ID,
                    target_component    = COMPONENT_ID,
                    param1              = target_arm ? 1f : 0f  // 1: arm, 0: disarm
                };

                int trial = 0;
                do
                {
                    SendPacket(message);
                    System.Threading.Thread.Sleep(1000);
                } while (target_arm ^ (ArmState == 128 /* 0b1000_0000 */) && ++trial < 5);

                if (button != null)
                    button.BeginInvoke((Action) delegate () { button.Enabled = true; });
            });
            thread.Start();

            return thread.IsAlive;
        }
        
        public void ClearMission()
        {
            Msg_mission_clear_all clearMessage = new Msg_mission_clear_all()
            {
                target_system       = SYSTEM_ID,
                target_component    = (byte) MAV_COMPONENT.MAV_COMP_ID_ALL
            };
            SendPacket(clearMessage);
        }

        /**
         * https://mavlink.io/en/services/mission.html#uploading_mission
         */
        public void SendMission(int index)
        {
            Msg_mission_item message = MissionItems[index];
            message.seq = (ushort) index;
            SendPacket(message);
        }

        public void UploadMission()
        {
            /**
             * https://mavlink.io/en/services/mission.html
             * 
             * The items for the different types of mission..
             * # Flight plans:
             * 1) NAV commands (MAV_CMD_NAV_*) for navigation/movement.
             * 2) DO commands (MAV_CMD_DO_*) for immediate actions like changing speed or activating a servo.
             * 3) CONDITION commands (MAV_CMD_CONDITION_*) for changing the execution of the mission based on a condition;
             *    - e.g. pausing the mission for a time before executing next command.
             * # Geofence mission items:
             * - Prefixed with MAV_CMD_NAV_FENCE_.
             * # Rally point mission items
             * - There is just one rally point MAV_CMD: MAV_CMD_NAV_RALLY_POINT.
             */

            SetCurrentPositionAsHome();

            MAV_CMD[] commands = new MAV_CMD[] { /*MAV_CMD.MAV_CMD_NAV_TAKEOFF,*/ MAV_CMD.MAV_CMD_NAV_WAYPOINT, MAV_CMD.MAV_CMD_NAV_WAYPOINT, MAV_CMD.MAV_CMD_NAV_LAND };
            float[] xs = new float[] { /*37.599158f,*/ 37.599467f, 37.599465f, 37.599465f };
            float[] ys = new float[] { /*126.863608f,*/ 126.863513f, 126.863464f, 126.863464f };
            MissionItemCount = 0;

            for (int i = 0; i < commands.Length; i++)
            {
                MissionItems[MissionItemCount++] = new Msg_mission_item()
                {
                    target_system       = SYSTEM_ID,
                    target_component    = COMPONENT_ID,
                    command             = (ushort) commands[i],
                    frame               = (byte) MAV_FRAME.MAV_FRAME_GLOBAL_RELATIVE_ALT,
                    autocontinue        = 1,
                    current             = (byte) (i == 0 ? 1 : 0),
                    seq                 = (byte) (i + 1),
                    x                   = xs[i],
                    y                   = ys[i],
                    z                   = 5
                };
            }

            // 1) Firstly, GCS sends MISSION_COUNT including the number of mission items to be uploaded.
            //   - A timeout must be started for the GCS to wait on the response from Drone (MISSION_REQUEST_INT).
            Msg_mission_count missionCountMessage = new Msg_mission_count()
            {
                target_system       = SYSTEM_ID,
                target_component    = COMPONENT_ID,
                count               = (ushort) MissionItemCount
            };
            SendPacket(missionCountMessage);

            // 2) Drone receives the message, and prepares to upload mission items 

            // 3) Drone responds with MISSION_REQUEST_INT requesting the first mission items.
        }

        public void UploadMissions(MissionPlanItem[] items)
        {
            MissionItemCount = 0;

            for (int i = 0; i < items.Length; i++)
            {
                MissionPlanItem item = items[i];

                MissionItems[MissionItemCount++] = new Msg_mission_item()
                {
                    target_system       = SYSTEM_ID,
                    target_component    = COMPONENT_ID,
                    command             = (ushort) item.type,
                    frame               = (byte) MAV_FRAME.MAV_FRAME_GLOBAL_RELATIVE_ALT,
                    autocontinue        = 1,
                    current             = (byte) (i == 0 ? 1 : 0),
                    seq                 = (byte) (i + 1),
                    x                   = (float) item.Position.X,
                    y                   = (float) item.Position.Y,
                    z                   = 5f
                };
            }

            Msg_mission_count missionCountMessage = new Msg_mission_count()
            {
                target_system = SYSTEM_ID,
                target_component = COMPONENT_ID,
                count = (ushort) MissionItemCount
            };
            SendPacket(missionCountMessage);
        }

        /**
         * https://mavlink.io/en/services/mission.html#download_mission
         */
        public void DownloadMission()
        {
            Msg_mission_request_list message = new Msg_mission_request_list()
            {
                target_system       = SYSTEM_ID,
                target_component    = COMPONENT_ID
            };
            SendPacket(message);
        }

        /**
         * https://docs.px4.io/en/flight_modes/mission.html
         */
        public void StartMission()
        {
            // To clarify the number of uploaded missions.
            DownloadMission();

            Msg_set_mode offboardMessage = new Msg_set_mode()
            {
                target_system   = SYSTEM_ID,
                base_mode       = (byte) MAV_MODE_FLAG.MAV_MODE_FLAG_CUSTOM_MODE_ENABLED,
                custom_mode     = ((uint) PX4FlightMode.OFFBOARD << 16) + (uint) PX4FlightSubMode.MISSION
            };
            SendPacket(offboardMessage);

            ChangeSpeed(0.5f);

            /**
             * https://mavlink.io/en/messages/common.html#MAV_CMD_MISSION_START
             */
            Msg_command_long message = new Msg_command_long()
            {
                target_system       = SYSTEM_ID,
                target_component    = COMPONENT_ID,
                command             = (ushort) MAV_CMD.MAV_CMD_MISSION_START
                // param1 = first_item: the first mission item to run
                // param2 = last_item: the last mission item to run (after this item is run, the mission ends)
            };
            SendPacket(message);
        }

        public bool HasCompletedMission()
        {
            return (MissionItemCount - 1) == MissionReachedSequence;
        }

        public void ResetMissionReachedSequence()
        {
            MissionReachedSequence = ushort.MaxValue;
        }

        /**
         * https://mavlink.io/en/messages/common.html#MAV_CMD_DO_CHANGE_SPEED
         */
        public void ChangeSpeed(float speed=-1f)
        {
            Msg_command_long message = new Msg_command_long()
            {
                target_system       = SYSTEM_ID,
                target_component    = COMPONENT_ID,
                command             = (ushort) MAV_CMD.MAV_CMD_DO_CHANGE_SPEED,
                param1              = 0,        // Speed type (0 = Airspeed, 1 = Ground Speed, 2 = Climb Speed, 3 = Descent Speed)
                param2              = speed,    // Speed (m/s, -1 indicates no change)
                param3              = -1,       // Throttle (Percent, -1 indicates no change)
                param4              = 0,        // 0 = absolute, 1 = relative
                param5              = 0,        // Empty
                param6              = 0,        // Empty
                param7              = 0         // Empty
            };
            SendPacket(message);
        }

        /**
         * https://mavlink.io/en/messages/common.html#MAV_CMD_DO_SET_HOME
         */
        public void UpdateHomeCommand(float latitude, float longitude, float altitude=5f)
        {
            Msg_command_long message = new Msg_command_long()
            {
                target_system       = SYSTEM_ID,
                target_component    = COMPONENT_ID,
                command             = (byte) MAV_CMD.MAV_CMD_DO_SET_HOME,
                param1              = 1,            // Use current (1 = use current location, 0 = use specified location)
                param2              = 0,            // Empty
                param3              = 0,            // Empty
                param4              = 0,            // Empty
                param5              = latitude,     // Latitude
                param6              = longitude,    // Longitude
                param7              = altitude      // Altitude
            };
            SendPacket(message);
        }
        
        public void SetCurrentPositionAsHome()
        {
            UpdateHomeCommand(Position.X, Position.Y);
        }

        /**
         * https://mavlink.io/en/messages/common.html#MAV_CMD_NAV_TAKEOFF
         */
        public void TakeoffCommand()
        {
            Msg_command_long message = new Msg_command_long()
            {
                command             = (ushort) MAV_CMD.MAV_CMD_NAV_TAKEOFF,
                target_system       = SYSTEM_ID,
                target_component    = COMPONENT_ID,
                param1              = 2.5f, // Minimum pitch
                // param2
                param3              = .1f,   // horizontal navigation by pilot acceptable
                // param4: yaw angle    (not supported)
                param5              = Position.X,   // latitude
                param6              = Position.Y,   // longitude
                param7              = 5             // altitude [meters]
            };
            SendPacket(message);

            ArmDisarmCommand(true);
        }

        /**
         * https://mavlink.io/en/messages/common.html#MAV_CMD_NAV_LAND
         */
        public void LandCommand()
        {
            ClearMission();

            Msg_command_long message = new Msg_command_long()
            {
                command             = (ushort) MAV_CMD.MAV_CMD_NAV_LAND,
                target_system       = SYSTEM_ID,
                target_component    = COMPONENT_ID,
                // param1: Abort Alt
                // param2: Precision land mode. (0 = normal landing, 1 = opportunistic precision landing, 2 = required precision landing)
                // param3: Empty
                // param4: Desired yaw angle. NaN for unchanged.
                param5              = Position.X,   // Latitude
                param6              = Position.Y    // Longitude
                // param7: Altitude (ground level)
            };
            SendPacket(message);
        }

        /**
         * https://mavlink.io/en/messages/common.html#MAV_CMD_NAV_WAYPOINT
         */
        public void NextWP(double latitude, double longitude)
        {
            ClearMission();

            MissionItemCount = 0;

            MissionItems[MissionItemCount++] = new Msg_mission_item()
            {
                target_system       = SYSTEM_ID,
                target_component    = COMPONENT_ID,
                command             = (ushort) MAV_CMD.MAV_CMD_NAV_WAYPOINT,
                frame               = (byte) MAV_FRAME.MAV_FRAME_GLOBAL_RELATIVE_ALT,
                autocontinue        = 1,
                current             = 1,
                seq                 = 1,
                x                   = (float) latitude,
                y                   = (float) longitude,
                z                   = 5
            };

            // Upload 
            Msg_mission_count missionCountMessage = new Msg_mission_count()
            {
                target_system       = SYSTEM_ID,
                target_component    = COMPONENT_ID,
                count               = (ushort) MissionItemCount
            };
            SendPacket(missionCountMessage);
        }

        // FIXME: Update member data variables
        public void UpdatePX4Mode(uint baseMode, uint customMode)
        {
            FlightMode  = PX4Mode[baseMode];
            SubMode     = PX4SubMode[customMode];
            //DatabaseManager.UpdateFlightMode(SYSTEM_ID, FlightMode);
        }

        public void UpdateBaseMode(byte baseMode)
        {
            _base_mode = baseMode;
            ArmState = (byte) (baseMode & (byte) MAV_MODE_FLAG.MAV_MODE_FLAG_SAFETY_ARMED);
        }

        public void UpdateBatteryPercentage(sbyte percentage)
        {
            BatteryPercentage = percentage;
            //DatabaseManager.UpdateBattery(SYSTEM_ID, BatteryPercentage);
        }

        public void UpdateAttitude(float roll, float pitch, float yaw)
        {
            Roll    = roll * Constant.RADIAN;
            Pitch   = pitch * Constant.RADIAN;
            Yaw     = yaw * Constant.RADIAN;
        }

        public void UpdateGpsRaw(int latitude, int longitude, int altitude, ulong usec, byte nsatellites)
        {
            Position.X      = (float) (latitude / Constant.GLOBAL_LOCAL_RATIO);
            Position.Y      = (float) (longitude / Constant.GLOBAL_LOCAL_RATIO);
            Position.Z      = (float) (altitude / Constant.GLOBAL_LOCAL_RATIO);
            Gtimestamp      = usec;
            SatelliteNumber = nsatellites;
            //DatabaseManager.UpdatePosition(SYSTEM_ID, Position.X, Position.Y, Position.Z, SatelliteNumber, Gtimestamp);
        }

        public void UpdateHeading(short heading)
        {
            HeadingDirection = heading;
            //DatabaseManager.UpdateHeadingDirection(SYSTEM_ID, HeadingDirection);
        }

        public void UpdateHomePosition(int latitude, int longitude)
        {
            HomePosition.X = (float) (latitude / Constant.GLOBAL_LOCAL_RATIO);
            HomePosition.Y = (float) (longitude / Constant.GLOBAL_LOCAL_RATIO);
        }

        public void UpdateLocalNED(float x, float y, float z)
        {
            LocalPosition.X = x;
            LocalPosition.Y = y;
            LocalPosition.Z = z;
        }

        public void UpdateCommandAckMessage(int index)
        {
            CommandResultMessage = ResultMessage[index];
        }

        public void UpdateStatusText(byte[] text)
        {
            if (text == null) return;

            int tsize = text.Length;
            char[] c = new char[tsize];
            for (int i = 0; i < tsize; i++) c[i] = (char) text[i];
            StatusMessage = new string(c);
        }

        /**
         * Response to MISSION_REQUEST_LIST.
         */
        public void UpdateMissionCount(UInt16 count)
        {
            MissionItemCount = count;

            Msg_mission_request requestMessage = new Msg_mission_request()
            {
                target_system       = SYSTEM_ID,
                target_component    = COMPONENT_ID,
                seq                 = 0
            };
            SendPacket(requestMessage);
        }

        /**
         * During mission download process.
         */
        public void OnMissionItemMessage(Msg_mission_item item)
        {
            MissionItems[item.seq] = item;

            if (item.seq < MissionItemCount - 1)
            {
                Msg_mission_request requestMessage = new Msg_mission_request()
                {
                    target_system       = SYSTEM_ID,
                    target_component    = COMPONENT_ID,
                    seq                 = (ushort) (item.seq + 1)
                };
                SendPacket(requestMessage);
            }
            else
            {
                Msg_mission_ack ackMessage = new Msg_mission_ack()
                {
                    target_system       = SYSTEM_ID,
                    target_component    = COMPONENT_ID,
                    type                = (byte) MAV_MISSION_RESULT.MAV_MISSION_ACCEPTED
                };
                SendPacket(ackMessage);
            }
        }

        public void OnMissionCurrentMessage(UInt16 sequenceNumber)
        {
            MissionCurrentSequence = sequenceNumber;

            Msg_mission_item currentItem = MissionItems[MissionCurrentSequence];
            
            if (IsLeader && currentItem != null)
            {
                Direction.X = currentItem.x - Position.X;
                Direction.Y = currentItem.y - Position.Y;
                Direction.Z = currentItem.z - Position.Z;
            }
            //DatabaseManager.UpdateNextCommand(SYSTEM_ID, MissionCurrentSequence);
        }

        public void OnMissionRequestMessage(UInt16 sequenceNumber)
        {
            Msg_mission_item itemMessage = MissionItems[sequenceNumber];
            itemMessage.seq = sequenceNumber;
            SendPacket(itemMessage);
        }

        public void OnMissionAckMessage(byte result)
        {
            bool accepted = (result == (byte) MAV_MISSION_RESULT.MAV_MISSION_ACCEPTED);

            // FIXME: Followers set_position as a single mission.
            if (accepted && !IsLeader)
            {
                Msg_command_long startMessage = new Msg_command_long()
                {
                    target_system       = SYSTEM_ID,
                    target_component    = COMPONENT_ID,
                    command             = (ushort) MAV_CMD.MAV_CMD_MISSION_START
                    // param1 = the first mission item to run
                    // param2 = the last mission item to run (after this item is run, the mission ends)
                };
                SendPacket(startMessage);
            }
        }

        public void OnMissionItemReachedMessage(UInt16 sequenceNumber)
        {
            MissionReachedSequence = sequenceNumber;
        }
    }
}
