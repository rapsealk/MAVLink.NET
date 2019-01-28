using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using MavLink;

namespace UnitTest
{
    class MAVLinkModel
    {
        // PX4_CUSTOM_MAIN_MODE
        public static readonly string[] PX4Mode =
        {
            "(EMPTY)", "MANUAL", "ALTCTL", "POSCTL", "AUTO", "ACRO", "OFFBOARD", "STABILIZED", "RATTITUDE",
            "SIMPLE"    // FIXME: unused, but reserved for future use.
        };
        private enum PX4FlightMode
        {
            MANUAL = 1, ALTCTL, POSCTL, AUTO, ACRO, OFFBOARD, STABILIZED, RATTITUDE,
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
            READY = 1, TAKEOFF, LOITER, MISSION, RTL, LAND, RTGS, FOLLOW_TARGET, PRECLAND
        };
        // COMMAND_ACK
        public static readonly string[] ResultMessage =
        {
            "ACCEPTED", "TEMPORARILY REJECTED", "DENIED", "UNSUPPORTED", "FAILED", "IN PROGRESS"
        };

        // System Id as a property.
        private byte SYSTEM_ID;
        public byte SystemId
        {
            get { return SYSTEM_ID; }
            set
            {
                if (value < 1 || 255 < value)
                    Console.Error.WriteLine("Invalid System Id.");
                else SYSTEM_ID = value;
            }
        }
        // Component Id as a property.
        private byte COMPONENT_ID;
        public byte ComponentId
        {
            get { return COMPONENT_ID; }
            set
            {
                if (value < 1 || 255 < value)
                    Console.Error.WriteLine("Invalid Component Id.");
                else COMPONENT_ID = value;
            }
        }

        private string _FlightMode;
        public string FlightMode
        {
            get { return _FlightMode; }
            set
            {
                if (!PX4Mode.Contains(value))
                    Console.Error.WriteLine("Invalid Flight mode.");
                else _FlightMode = value;
            }
        }

        private string _SubMode;
        public string SubMode
        {
            get { return _SubMode; }
            set
            {
                if (!PX4SubMode.Contains(value))
                    Console.Error.WriteLine("Invalid Sub flight mode.");
                else _SubMode = value;
            }
        }

        // Packet sequence as a property.
        private byte _PacketSequence;
        public byte PacketSequence
        {
            get { return _PacketSequence; }
            set
            {
                if (value < 0 || 255 < value)
                    Console.Error.WriteLine("Invalid Packet sequence number.");
                else _PacketSequence = value;
            }
        }
        
        private float Roll;
        private float Pitch;
        private float Yaw;

        public bool IsLeader;
        public byte BaseMode;
        public byte ArmState;
        public sbyte BatteryPercentage;
        public short HeadingDirection;
        public ulong GpsUnixTimestamp;
        public byte SatelliteNumber;

        public Vector3 GlobalPosition;
        public Vector3 LocalPosition;
        public Vector3 HomePosition;

        public Vector3 Direction;

        // Mission items.
        public List<Msg_mission_item> MissionItems;
        public ushort MissionItemCount = 0;
        public ushort MissionCurrentSequence = ushort.MaxValue;
        public ushort MissionReachedSequence = ushort.MaxValue;

        public string StatusMessage;

        private string _CommandResultMessage;
        public string CommandResultMessage
        {
            get { return _CommandResultMessage; }
            set
            {
                if (ResultMessage.Contains(value))
                    Console.Error.WriteLine("Invalid Command result message.");
                else _CommandResultMessage = value;
            }
        }

        // string portName, int baudrate = 57600
        public MAVLinkModel()
        {
            MissionItems = new List<Msg_mission_item>();

            Roll = Pitch = Yaw = 0;
        }
    }
}
