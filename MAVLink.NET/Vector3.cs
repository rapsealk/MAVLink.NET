using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAVLink.NET
{
    class Vector3
    {
        public double X, Y, Z;

        public Vector3(double x = 0, double y = 0, double z = 0)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public static Vector3 operator +(Vector3 one, Vector3 other)
        {
            return new Vector3(one.X + other.X, one.Y + other.Y, one.Z + other.Z);
        }

        public static Vector3 operator -(Vector3 one, Vector3 other)
        {
            return new Vector3(one.X - other.X, one.Y - other.Y, one.Z - other.Z);
        }

        public static Vector3 operator *(Vector3 vector, double scalar)
        {
            return new Vector3(vector.X * scalar, vector.Y * scalar, vector.Z * scalar);
        }

        public static Vector3 operator /(Vector3 vector, double scalar)
        {
            return new Vector3(vector.X / scalar, vector.Y / scalar, vector.Z / scalar);
        }

        public double Size()
        {
            return Math.Abs(Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2)));
        }

        public void Normalize()
        {
            double size = this.Size();
            this.X /= size;
            this.Y /= size;
            this.Z /= size;
        }

        public Vector3 Normalized()
        {
            double size = this.Size();
            return new Vector3(X / size, Y / size, Z / size);
        }
    }
}
