﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            type = commandType;
        }
    }
}
