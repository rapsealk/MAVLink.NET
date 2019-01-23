using System;
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
        private static readonly string[] PX4SubMode =
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

        private Msg_heartbeat               mHeartbeat              = new Msg_heartbeat();
        private Msg_sys_status              mSysStatus              = new Msg_sys_status();
        private Msg_power_status            mPowerStatus            = new Msg_power_status();
        private Msg_battery_status          mBatteryStatus          = new Msg_battery_status();
        private Msg_attitude                mAttitude               = new Msg_attitude();
        private Msg_gps_raw_int             mGPS                    = new Msg_gps_raw_int();
        private Msg_gps_rtk                 mRTK                    = new Msg_gps_rtk();
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

        public bool _is_leader = false;

        private byte _base_mode = 0;
        public byte _is_armed = 0;

        public sbyte BatteryPercentage = 0;

        private readonly Msg_mission_item[] MissionItems = new Msg_mission_item[32];
        private int MissionItemCount = 0;
        private ushort MissionCurrentSequence = ushort.MaxValue;
        private ushort MissionReachedSequence = ushort.MaxValue;

        public string FlightMode            = "null";
        public string SubMode               = "null";

        public string StatusMessage         = "null";
        public string CommandResultMessage  = "null";

        public float Roll   = 0f;
        public float Pitch  = 0f;
        public float Yaw    = 0f;

        public short HeadingDirection = 0;

        private const float Radian = (float) (180 / Math.PI);

        /**
         * Variables for agent's location.
         */
        public Vector3 Position;
        public Vector3 HomePosition;
        public Vector3 LocalPosition;
        public static double pRatio = 10 * 1000 * 1000; // 10_000_000

        public ulong Gtimestamp     = 0;        // GPS UNIX Timestamp (start from boot)
        public byte SatelliteNumber = 0;        // Number of visible Satellite

        /**
         * Variables for desired direction.
         */
        public Vector3 Direction;


        public MAVLinkNode(string port, int baud, byte SYSTEM_ID=1, byte COMPONENT_ID=1)
        {
            mavlink = new Mavlink();
            mavlink.PacketReceived += OnMAVPacketReceive;

            Position        = new Vector3();
            HomePosition    = new Vector3();
            LocalPosition   = new Vector3();
            Direction       = new Vector3();

            this.PortName = port;
            this.BaudRate = baud;

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
            SYSTEM_ID = (byte) packet.SystemId;
            COMPONENT_ID = (byte) packet.ComponentId;
            PacketSequence = packet.SequenceNumber;

            MavlinkMessage message = packet.Message;

            if (message.GetType() == mHeartbeat.GetType())
            {
                mHeartbeat = (Msg_heartbeat) message;
                //  0000 0000 0000 0000 | 0000 0000 0000 0000
                // \_custom_/  \_base_/
                uint offset         = mHeartbeat.custom_mode >> 16;
                uint base_index     = offset % 256;
                uint custom_index   = offset / 256;
                try
                {
                    // FlightMode = PX4Mode[mHeartbeat.custom_mode / 65536];
                    FlightMode  = PX4Mode[base_index];
                    SubMode     = PX4SubMode[custom_index];
                }
                catch (IndexOutOfRangeException e)
                {
                    Console.Error.WriteLine(e.Message);
                }
                Console.WriteLine("heartbeat.custom_mode: " + mHeartbeat.custom_mode);
                Console.WriteLine("Flight Mode: {0:s}, Sub Mode: {1:s}", FlightMode, SubMode);

                //DatabaseManager.UpdateFlightMode(SYSTEM_ID, FlightMode);
            }
            else if (message.GetType() == mSysStatus.GetType())
            {
                mSysStatus = (Msg_sys_status) message;
            }
            else if (message.GetType() == mPowerStatus.GetType())
            {
                mPowerStatus = (Msg_power_status) message;
            }
            else if (message.GetType() == mBatteryStatus.GetType())
            {
                mBatteryStatus = (Msg_battery_status) message;
                BatteryPercentage = mBatteryStatus.battery_remaining;

                //DatabaseManager.UpdateBattery(SYSTEM_ID, BatteryPercentage);
            }
            else if (message.GetType() == mAttitude.GetType())
            {
                mAttitude = (Msg_attitude) message;
                Roll = mAttitude.roll * Radian;
                Pitch = mAttitude.pitch * Radian;
                Yaw = mAttitude.yaw * Radian;
            }
            else if (message.GetType() == mGPS.GetType())
            {
                mGPS = (Msg_gps_raw_int) message;
                Position.X = mGPS.lat / pRatio;
                Position.Y = mGPS.lon / pRatio;
                Position.Z = mGPS.alt / pRatio;
                Gtimestamp = mGPS.time_usec;
                SatelliteNumber = mGPS.satellites_visible;

                //DatabaseManager.UpdatePosition(SYSTEM_ID, Position.X, Position.Y, Position.Z, SatelliteNumber, Gtimestamp);
            }
            else if (message.GetType() == mRTK.GetType())
            {
                mRTK = (Msg_gps_rtk) message;
            }
            else if (message.GetType() == mVfr.GetType())
            {
                mVfr = (Msg_vfr_hud) message;
                HeadingDirection = mVfr.heading;

                //DatabaseManager.UpdateHeadingDirection(SYSTEM_ID, HeadingDirection);
            }
            else if (message.GetType() == mHomePosition.GetType())
            {
                mHomePosition = (Msg_home_position) message;
                HomePosition.X = mHomePosition.latitude / pRatio;
                HomePosition.Y = mHomePosition.longitude / pRatio;
            }
            else if (message.GetType() == mLocalPositionNED.GetType())
            {
                mLocalPositionNED = (Msg_local_position_ned) message;
                LocalPosition.X = mLocalPositionNED.x;
                LocalPosition.Y = mLocalPositionNED.y;
                LocalPosition.Z = mLocalPositionNED.z;
            }
            else if (message.GetType() == mRawPressure.GetType())
                mRawPressure = (Msg_raw_pressure) message;
            else if (message.GetType() == mScaledPressure.GetType())    // TODO: Log press_abs, temperature, press_diff
                mScaledPressure = (Msg_scaled_pressure) message;
            else if (message.GetType() == mCommandAck.GetType())
            {
                mCommandAck = (Msg_command_ack) message;
                CommandResultMessage = ResultMessage[mCommandAck.result];
            }
            else if (message.GetType() == mStatusText.GetType())        // TODO: System status message
                mStatusText = (Msg_statustext) message;
            else if (message.GetType() == mMissionCount.GetType())      // Response to MISSION_REQUEST_LIST
            {
                mMissionCount = message as Msg_mission_count;
                MissionItemCount = mMissionCount.count;
                Console.WriteLine("[SYSTEM #{0:d}] Msg_mission_count: {1:d}", SYSTEM_ID, mMissionCount.count);

                Msg_mission_request requestMessage = new Msg_mission_request()
                {
                    target_system       = SYSTEM_ID,
                    target_component    = COMPONENT_ID,
                    seq                 = 0
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
                        target_system       = SYSTEM_ID,
                        target_component    = COMPONENT_ID,
                        seq                 = (ushort) (sequenceNumber + 1)
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
                }
            }
            else if (message.GetType() == mMissionCurrent.GetType())
            {
                mMissionCurrent = (Msg_mission_current) message;
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
                Console.WriteLine("[SYSTEM #{0:d}] Mission Ack: " + (mMissionAck.type == (byte) MAV_MISSION_RESULT.MAV_MISSION_ACCEPTED), SYSTEM_ID);
            
                // Follower: set_position as a single mission.
                if (!_is_leader)
                {
                    Msg_command_long smessage = new Msg_command_long()
                    {
                        target_system       = SYSTEM_ID,
                        target_component    = COMPONENT_ID,
                        command             = (ushort) MAV_CMD.MAV_CMD_MISSION_START
                        // param1 = first_item: the first mission item to run
                        // param2 = last_item: the last mission item to run (after this item is run, the mission ends)
                    };
                    SendPacket(smessage);
                }
            }
            else if (message.GetType() == mMissionItemReached.GetType())
            {
                mMissionItemReached = (Msg_mission_item_reached) message;
                MissionReachedSequence = mMissionItemReached.seq;
                Console.WriteLine("[SYSTEM #{0:d}] Mission reached: {1:d}", SYSTEM_ID, MissionReachedSequence);
            }

            _is_armed = (byte) (mHeartbeat.base_mode & (byte) MAV_MODE_FLAG.MAV_MODE_FLAG_SAFETY_ARMED);

            if (mStatusText.text != null)
            {
                int tsize = mStatusText.text.Length;
                char[] c = new char[tsize];
                for (int i = 0; i < tsize; i++) c[i] = (char) mStatusText.text[i];
                StatusMessage = new string(c);
            }

            _base_mode = mHeartbeat.base_mode;
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
                } while (target_arm ^ (_is_armed == 128 /* 0b1000_0000 */) && ++trial < 5);

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
            UpdateHomeCommand((float) Position.X, (float) Position.Y);
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
                param5              = (float) Position.X,   // latitude
                param6              = (float) Position.Y,   // longitude
                param7              = 5                     // altitude [meters]
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
                param5              = (float) Position.X,   // Latitude
                param6              = (float) Position.Y    // Longitude
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
    }
}
