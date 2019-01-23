using System.Numerics;
using MavLink;

namespace MAVLink.NET
{
    class MissionPlanItem
    {
        public Vector3 Position;
        public MAV_CMD type;

        public MissionPlanItem(Vector3 position, MAV_CMD commandType=MAV_CMD.MAV_CMD_NAV_WAYPOINT)
        {
            Position = position;
            Position.Z = (Position.Z == 0f) ? 5f : Position.Z;
            type = commandType;
        }
    }
}
