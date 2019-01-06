//#define MYSQL
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

        public System.IO.Ports.SerialPort Serial;
        public System.ComponentModel.BackgroundWorker heartbeatWorker;

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
        private Msg_vfr_hud                 mVfr                    = new Msg_vfr_hud();   // heading, altitude
        private Msg_raw_pressure            mRawPressure            = new Msg_raw_pressure();
        private Msg_scaled_pressure         mScaledPressure         = new Msg_scaled_pressure();
        private Msg_command_ack             mCommandAck             = new Msg_command_ack();
        private Msg_statustext              mStatusText             = new Msg_statustext();
        private Msg_mission_count           mMissionCount           = new Msg_mission_count();
        private Msg_mission_request         mMissionRequest         = new Msg_mission_request();
        private Msg_mission_ack             mMissionAck             = new Msg_mission_ack();
        private Msg_mission_item_reached    mMissionItemReached     = new Msg_mission_item_reached();

        public bool _is_leader = false;

        private byte _base_mode = 0;
        public byte _is_armed = 0;

        public sbyte BatteryPercentage = 0;

        private Msg_mission_item[] MissionItems = new Msg_mission_item[32];
        private int MissionItemCount = 0;

        public string FlightMode            = "null";
        public string SubMode               = "null";

        public string StatusMessage         = "null";
        public string CommandResultMessage  = "null";

        public float Roll   = 0f;
        public float Pitch  = 0f;
        public float Yaw    = 0f;

        private static float Radian = (float) (180 / Math.PI);

        /**
         * Variables for agent's location.
         */
        public Vector3 Position;
        public static double pRatio = 10 * 1000 * 1000; // 10_000_000

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
            //if (packet.SystemId != SYSTEM_ID || packet.ComponentId != COMPONENT_ID)
            //    return;

            uint psize = mavlink.PacketsReceived;
            SYSTEM_ID = (byte) packet.SystemId;
            COMPONENT_ID = (byte) packet.ComponentId;
            PacketSequence = packet.SequenceNumber;
            // Console.WriteLine("Sequence #: " + PacketSequence);

            MavlinkMessage message = packet.Message;

            if (message.GetType() == mHeartbeat.GetType())
            {
                mHeartbeat = (Msg_heartbeat) message;
                //  0000 0000 0000 0000 | 0000 0000 0000 0000
                // \_custom_/  \_base_/
                uint offset = mHeartbeat.custom_mode >> 16;
                uint base_index = offset % 256;
                uint custom_index = offset / 256;
                try
                {
                    // FlightMode = PX4Mode[mHeartbeat.custom_mode / 65536];
                    FlightMode = PX4Mode[base_index];
                    SubMode = PX4SubMode[custom_index];
                }
                catch (IndexOutOfRangeException e)
                {
                    // FIXME
                }
                Console.WriteLine("heartbeat.custom_mode: " + mHeartbeat.custom_mode);
                Console.WriteLine("Flight Mode: {0:s}, Sub Mode: {1:s}", FlightMode, SubMode);
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
#if MYSQL
                // MySQL Update Query
                MySql.Data.MySqlClient.MySqlConnection conn = DatabaseManager.GetConnection();
                try
                {
                    conn.Open();
                    MySql.Data.MySqlClient.MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand()
                    {
                        Connection = conn,
                        CommandText = "UPDATE realtime SET Lattitude=@lat, Longitude=@lon WHERE UAV_ID=@id"
                    };
                    command.Parameters.Add("@lat", MySql.Data.MySqlClient.MySqlDbType.Double);
                    command.Parameters.Add("@lon", MySql.Data.MySqlClient.MySqlDbType.Double);
                    command.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.Int32);
                    command.Parameters[0].Value = Position.X;
                    command.Parameters[1].Value = Position.Y;
                    command.Parameters[2].Value = SYSTEM_ID;
                    command.ExecuteNonQuery();
                    conn.Close();
                }
                catch (MySql.Data.MySqlClient.MySqlException e)
                {
                    Console.Error.WriteLine(e.Message);
                }
#endif
            }
            else if (message.GetType() == mRTK.GetType())
            {
                mRTK = (Msg_gps_rtk) message;
            }
            else if (message.GetType() == mVfr.GetType())
                mVfr = (Msg_vfr_hud) message;
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
            //else if (message.GetType() == mMissionCount.GetType())      // TODO: Handle mission
            //    mMissionCount = (Msg_mission_count) message;
            else if (message.GetType() == mMissionRequest.GetType())
            {
                mMissionRequest = (Msg_mission_request) message;
                Console.WriteLine("mMissionRequest.seq: " + mMissionRequest.seq);
                SendMission(mMissionRequest.seq);
            }
            else if (message.GetType() == mMissionAck.GetType())
            {
                mMissionAck = (Msg_mission_ack) message;
                Console.WriteLine("Mission Ack: " + (mMissionAck.type == (byte) MAV_MISSION_RESULT.MAV_MISSION_ACCEPTED));
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
                } while (target_arm ^ (_is_armed == 0b1000_0000) && ++trial < 5);

                if (button != null)
                    button.BeginInvoke((Action) delegate () { button.Enabled = true; });
            });
            thread.Start();

            return thread.IsAlive;
        }

        /*
        public void SingleNavCommand(MAV_CMD command)
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
                command         = (byte) command,
                current         = 1,
                frame           = (byte) MAV_FRAME.MAV_FRAME_GLOBAL_RELATIVE_ALT,
                seq             = 1
            };
            SendPacket(message);
        }
        */
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

            MAV_CMD[] commands = new MAV_CMD[] { MAV_CMD.MAV_CMD_NAV_TAKEOFF, MAV_CMD.MAV_CMD_NAV_WAYPOINT, MAV_CMD.MAV_CMD_NAV_WAYPOINT, MAV_CMD.MAV_CMD_NAV_LAND };
            float[] xs = new float[] { 37.599158f, 37.599202f, 37.599246f, 37.599290f };
            float[] ys = new float[] { 126.863608f, 126.863422f, 126.863236f, 126.863050f };
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
            /* Takeoff
            MissionItems[MissionItemCount++] = new Msg_mission_item()
            {
                target_system       = SYSTEM_ID,
                target_component    = COMPONENT_ID,
                command             = (ushort) MAV_CMD.MAV_CMD_NAV_TAKEOFF,
                frame               = (byte) MAV_FRAME.MAV_FRAME_GLOBAL_RELATIVE_ALT,
                autocontinue        = 1,
                current             = 1,
                seq                 = 1
            };
            for (int i = 0; i < xs.Length; i++)
            {
                MissionItems[MissionItemCount++] = new Msg_mission_item()
                {
                    target_system       = SYSTEM_ID,
                    target_component    = COMPONENT_ID,
                    command             = (ushort) MAV_CMD.MAV_CMD_NAV_WAYPOINT,
                    frame               = (byte) MAV_FRAME.MAV_FRAME_GLOBAL_RELATIVE_ALT,
                    autocontinue        = 1,
                    current             = 0, // (byte) (i == 0 ? 1 : 0),
                    seq                 = (byte) (i + 2), // (i + 1),
                    x                   = xs[i],
                    y                   = ys[i],
                    z                   = 5
                };
            }
            MissionItems[0].param1 = 5; // minimum pitch
            // Land
            MissionItems[MissionItemCount++] = new Msg_mission_item()
            {
                target_system       = SYSTEM_ID,
                target_component    = COMPONENT_ID,
                command             = (ushort) MAV_CMD.MAV_CMD_NAV_LAND,
                frame               = (byte) MAV_FRAME.MAV_FRAME_GLOBAL_RELATIVE_ALT,
                autocontinue        = 1,
                current             = 0,
                seq                 = (ushort) (xs.Length + 2)
            };
            */

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

        /**
         * https://docs.px4.io/en/flight_modes/mission.html
         */
        public void StartMission()
        {
            Msg_set_mode message = new Msg_set_mode()
            {
                target_system   = SYSTEM_ID,
                base_mode       = (byte) MAV_MODE_FLAG.MAV_MODE_FLAG_CUSTOM_MODE_ENABLED,
                custom_mode     = ((uint) PX4FlightMode.OFFBOARD << 16) + (uint) PX4FlightSubMode.MISSION
            };
            SendPacket(message);
        }

        /*
        public void UpdateHomeCommand(float latitude, float longitude, float altitude=5f)
        {
            Msg_command_long message = new Msg_command_long()
            {
                target_system       = SYSTEM_ID,
                target_component    = COMPONENT_ID,
                command             = (byte) MAV_CMD.MAV_CMD_DO_SET_HOME,
                param1              = 0,
                param2              = 0,
                param3              = 0,
                param4              = 0
            };
        }
        */

        /**
         * https://mavlink.io/en/messages/common.html#MAV_CMD_DO_SET_HOME
         */
        private void SetCurrentPositionAsHome()
        {
            Msg_command_long message = new Msg_command_long()
            {
                target_system       = SYSTEM_ID,
                target_component    = COMPONENT_ID,
                command             = (byte) MAV_CMD.MAV_CMD_DO_SET_HOME,
                param1              = 1     // Use current (1 = use current location, 0 = use specified location)
                // param2: Empty
                // param3: Empty
                // param4: Empty
                // param5: Latitude,
                // param6: Longitude,
                // param7: Altitude
            };
            SendPacket(message);
        }

        /**
         * https://mavlink.io/en/messages/common.html#MAV_CMD_NAV_TAKEOFF
         */
        public void TakeoffCommand()
        {
            //*
            Msg_command_long message = new Msg_command_long()
            {
                command             = (ushort) MAV_CMD.MAV_CMD_NAV_TAKEOFF,
                target_system       = SYSTEM_ID,
                target_component    = COMPONENT_ID,
                param1              = 2.5f, // Minimum pitch
                // param2
                param3              = 1f,   // horizontal navigation by pilot acceptable
                // param4: yaw angle    (not supported)
                // param5: latitude     (not supported)
                // param6: longitude    (not supported)
                param7              = 5     // altitude [meters]
            };
            /*/
            Msg_command_long message = new Msg_command_long()
            {
                command=(ushort)MAV_CMD.MAV_CMD_NAV_TAKEOFF_LOCAL,
                target_system=SYSTEM_ID,
                target_component=COMPONENT_ID,
                // param1: Minimum pitch (if airspeed sensor present), desired pitch without sensor [rad]
                // param2: Empty
                // param3: Takeoff ascend rate [ms^-1]
                // param4: Yaw angle [rad] (if magnetometer or another yaw estimation source present), ignored without one of these
                // param5: Y-axis position [m]
                // param6: X-axis position [m]
                // param7: Z-axis position [m]
                param7 = 5
            };
            //*/
            SendPacket(message);

            ArmDisarmCommand(true);
        }

        /**
         * https://mavlink.io/en/messages/common.html#MAV_CMD_NAV_LAND
         */
        public void LandCommand()
        {
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
            /*
            Msg_mission_item message = new Msg_mission_item()
            {
                seq             = 1,
                command         = (byte) MAV_CMD.MAV_CMD_NAV_WAYPOINT,
                frame           = (byte) MAV_FRAME.MAV_FRAME_GLOBAL_RELATIVE_ALT,
                autocontinue    = 1,
                current         = 0,
                param1          = 2,
                x               = (float) Position.X + 1,
                y               = (float) Position.Y + 1,
                z               = 5
            };
            /*/
            Msg_command_long message = new Msg_command_long()
            {
                command             = (ushort) MAV_CMD.MAV_CMD_NAV_WAYPOINT,
                target_system       = SYSTEM_ID,
                target_component    = COMPONENT_ID,
                // param1: Hold time in decimal seconds. (ignored by fixed wing, time to stay at waypoint for rotary wing)
                // param2: Acceptance radius in meters (if the sphere with this radius is hit, the waypoint counts as reached)
                // param3: 0 to pass through the WP, if > 0 radius in meters to pass by WP. Positive value for clockwise orbit, negative value for counter-clockwise orbit. Allows trajectory control.
                // param4: Desired yaw angle at waypoint (rotary wing). NaN for unchanged.
                param5              = (float) latitude,     // Latitude
                param6              = (float) longitude,    // Longitude
                param7              = 5                     // Altitude
            };
            //*/
            SendPacket(message);
        }
    }
}
