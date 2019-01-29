using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using MavLink;

namespace UnitTest
{
    public class MAVLinkModel : Interface.IMAVPublisher
    {
        private readonly List<Interface.IMAVObserver> observers;

        // PX4_CUSTOM_MAIN_MODE
        public static readonly string[] PX4Mode =
        {
            "(EMPTY)", "MANUAL", "ALTCTL", "POSCTL", "AUTO", "ACRO", "OFFBOARD", "STABILIZED", "RATTITUDE",
            "SIMPLE"    // FIXME: unused, but reserved for future use.
        };
        // PX4_CUSTOM_SUB_MODE AUTO
        public static readonly string[] PX4SubMode =
        {
            "(EMPTY)", "READY", "TAKEOFF", "LOITER", "MISSION", "RTL", "LAND",
            "RTGS", "FOLLOW_TARGET", "PRECLAND"
        };
        // COMMAND_ACK
        public static readonly string[] ResultMessage =
        {
            "ACCEPTED", "TEMPORARILY REJECTED", "DENIED", "UNSUPPORTED", "FAILED", "IN PROGRESS"
        };

        private byte SYSTEM_ID;
        private byte COMPONENT_ID;

        private string flightMode;
        private string subFlightMode;

        private byte packetSequence;

        private float roll  = 0f;
        private float pitch = 0f;
        private float yaw   = 0f;

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
            observers = new List<Interface.IMAVObserver>();

            MissionItems = new List<Msg_mission_item>();
        }

        public byte SystemId
        {
            get { return SYSTEM_ID; }
            set
            {
                Console.WriteLine("Set SystemId: " + value);

                if (value < 1 || 255 < value)
                    Console.Error.WriteLine("Invalid System Id.");
                else SYSTEM_ID = value;
                Notify();
            }
        }
        public byte ComponentId
        {
            get { return COMPONENT_ID; }
            set
            {
                Console.WriteLine("Set ComponentId: " + value);

                if (value < 1 || 255 < value)
                    Console.Error.WriteLine("Invalid Component Id.");
                else COMPONENT_ID = value;
                Notify();
            }
        }
        public string FlightMode
        {
            get { return flightMode; }
            set
            {
                if (!PX4Mode.Contains(value))
                    Console.Error.WriteLine("Invalid Flight mode.");
                else flightMode = value;
            }
        }
        public string SubMode
        {
            get { return subFlightMode; }
            set
            {
                if (!PX4SubMode.Contains(value))
                    Console.Error.WriteLine("Invalid Sub flight mode.");
                else subFlightMode = value;
            }
        }
        public byte PacketSequence
        {
            get { return packetSequence; }
            set
            {
                if (value < 0 || 255 < value)
                    Console.Error.WriteLine("Invalid Packet sequence number.");
                else packetSequence = value;
            }
        }

        /*
         * IMAVPublisher
         */
        public void Add(Interface.IMAVObserver observer)
        {
            observers.Add(observer);
        }

        public void Delete(Interface.IMAVObserver observer)
        {
            observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (var observer in observers)
                observer.Update(this);
        }

        public void NotifyArmState(bool isArmed)
        {
            foreach (var observer in observers)
                observer.UpdateArmState(isArmed);
        }
    }
}
