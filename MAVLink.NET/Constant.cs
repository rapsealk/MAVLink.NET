using System;

namespace MAVLink.NET
{
    class Constant
    {
        public static readonly float RADIAN = (float) (180 / Math.PI);

        public static readonly double GLOBAL_LOCAL_RATIO = 10 * 1000 * 1000; // 10_000_000
    }
}
