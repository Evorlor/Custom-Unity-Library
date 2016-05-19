namespace CustomUnityLibrary
{
    using UnityEngine;

    /// <summary>
    /// Contains x and y coordinates.
    /// Similar to Vector3, but stores ints instead of floats.
    /// </summary>
    [System.Serializable]
    public struct Point3
    {
        /// <summary>
        /// Shorthand for writing Point3(0, 0, 0).
        /// </summary>
        public static Point3 zero = new Point3(0, 0, 0);

        /// <summary>
        /// Shorthand for writing Point3(1, 1, 1).
        /// </summary>
        public static Point3 one = new Point3(1, 1, 1);

        /// <summary>
        /// Shorthand for writing Point3(0, 0, -1).
        /// </summary>
        public static Point3 back = new Point3(0, 0, -1);

        /// <summary>
        /// Shorthand for writing Point3(0, -1, 0).
        /// </summary>
        public static Point3 down = new Point3(0, -1, 0);

        /// <summary>
        /// Shorthand for writing Point3(0, 0, 1).
        /// </summary>
        public static Point3 forward = new Point3(0, 0, 1);

        /// <summary>
        /// Shorthand for writing Point3(-1, 0, 0).
        /// </summary>
        public static Point3 left = new Point3(-1, 0, 0);

        /// <summary>
        /// Shorthand for writing Point3(1, 0, 0).
        /// </summary>
        public static Point3 right = new Point3(1, 0, 0);

        /// <summary>
        /// Shorthand for writing Point3(0, 1, 0).
        /// </summary>
        public static Point3 up = new Point3(0, 1, 0);

        /// <summary>
        /// Returns the length of this point
        /// </summary>
        public float magnitude
        {
            get
            {
                return Mathf.Sqrt(x + x * y * y + z * z);
            }
        }

        /// <summary>
        /// Returns this point as a Vector3 with a magnitude of 1
        /// </summary>
        public Vector2 normalized
        {
            get
            {
                return ((Vector3)this).normalized;
            }
        }

        /// <summary>
        /// Returns the squared length of this point
        /// </summary>
        public float sqrMagnitude
        {
            get
            {
                return x * x * y * y + z * z;
            }
        }

        /// <summary>
        /// Access the x, y, z components using [0], [1], [2] respectively.
        /// </summary>
        public int this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return x;
                    case 1:
                        return y;
                    case 2:
                        return z;
                    default:
                        throw new System.IndexOutOfRangeException("Use 0 to access x, use 1 to access y, or use 2 to access z.");
                }
            }
        }

        /// <summary>
        /// X component of the point.
        /// </summary>
        public int x;

        /// <summary>
        /// Y component of the point.
        /// </summary>
        public int y;

        /// <summary>
        /// Z component of the point.
        /// </summary>
        public int z;

        /// <summary>
        /// Creates a new Point
        /// </summary>
        public Point3(int x, int y, int z = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Creates a new Point3 from an existing Point2
        /// </summary>
        public Point3(Point2 point2)
        {
            x = point2.x;
            y = point2.y;
            z = 0;
        }

        /// <summary>
        /// Creates a new Point3 from an existing Point3
        /// </summary>
        public Point3(Point3 point3)
        {
            x = point3.x;
            y = point3.y;
            z = point3.z;
        }

        /// <summary>
        /// Creates a new Point3 from a Vector2
        /// </summary>
        public Point3(Vector2 vector2)
        {
            x = (int)vector2.x;
            y = (int)vector2.y;
            z = 0;
        }

        /// <summary>
        /// Creates a new Point3 from a Vector3
        /// </summary>
        public Point3(Vector3 vector3)
        {
            x = (int)vector3.x;
            y = (int)vector3.y;
            z = (int)vector3.z;
        }

        /// <summary>
        /// Compares two Points for equality
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is Point3 && this == (Point3)obj;
        }

        /// <summary>
        /// Gets the Hash Code of the Point
        /// </summary>
        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
        }

        /// <summary>
        /// Compares two Points for equality
        /// </summary>
        public static bool operator ==(Point3 point0, Point3 point1)
        {
            return point0.x == point1.x && point0.y == point1.y && point0.z == point1.z;
        }

        /// <summary>
        /// Compares two Points for inequality
        /// </summary>
        public static bool operator !=(Point3 point0, Point3 point1)
        {
            return !(point0 == point1);
        }

        /// <summary>
        /// Multiplies each component by a float
        /// </summary>
        public static Vector3 operator *(Point3 point, float number)
        {
            return new Vector3(point.x * number, point.y * number, point.z * number);
        }

        /// <summary>
        /// Multiplies each component by an int
        /// </summary>
        public static Point3 operator *(Point3 point, int number)
        {
            return new Point3(point.x * number, point.y * number, point.z * number);
        }

        /// <summary>
        /// Divides each component by a float
        /// </summary>
        public static Vector3 operator /(Point3 point, float number)
        {
            return new Vector3(point.x / number, point.y / number, point.z / number);
        }

        /// <summary>
        /// Divides each component by an int
        /// </summary>
        public static Point3 operator /(Point3 point, int number)
        {
            return new Point3(point.x / number, point.y / number, point.z / number);
        }

        /// <summary>
        /// Performs component-wise addition between two Points
        /// </summary>
        public static Point3 operator +(Point3 point0, Point3 point1)
        {
            return new Point3(point0.x + point1.x, point0.y + point1.y, point0.z + point1.z);
        }

        /// <summary>
        /// Performs component-wise subtraction between two Points such that the new Point is point0 minus point 1
        /// </summary>
        public static Point3 operator -(Point3 point0, Point3 point1)
        {
            return new Point3(point0.x - point1.x, point0.y - point1.y, point0.z - point1.z);
        }

        /// <summary>
        /// Returns the string format of the Point
        /// </summary>
        public override string ToString()
        {
            return "(" + x + "," + y + "," + z + ")";
        }

        /// <summary>
        /// Allows for cast from Vector3 to Point3.
        /// Truncates any decimals on the Vector3's components.
        /// </summary>
        public static explicit operator Point3(Vector3 vector3)
        {
            return new Point3((int)vector3.x, (int)vector3.y, (int)vector3.z);
        }

        /// <summary>
        /// Allows for cast from Point3 to Vector3
        /// </summary>
        public static implicit operator Vector3(Point3 point3)
        {
            return new Vector3(point3.x, point3.y, point3.z);
        }

        /// <summary>
        /// Allows for cast from Vector2 to Point3.
        /// Truncates any decimals on the Vector3's components.
        /// </summary>
        public static explicit operator Point3(Vector2 vector2)
        {
            return new Point3((int)vector2.x, (int)vector2.y, 0);
        }

        /// <summary>
        /// Allows for cast from Point3 to Vector2
        /// </summary>
        public static explicit operator Vector2(Point3 point3)
        {
            return new Vector2(point3.x, point3.y);
        }

        /// <summary>
        /// Returns the angle in degrees between from and to.
        /// </summary>
        public static float Angle(Point3 point0, Point3 point1)
        {
            return Vector3.Angle(point0, point1);
        }

        /// <summary>
        /// Returns the distance between a and b.
        /// </summary>
        public static float Distance(Point3 point0, Point3 point1)
        {
            return Vector3.Distance(point0, point1);
        }

        /// <summary>
        /// Dot product of two points
        /// </summary>
        public static float Dot(Point3 point0, Point3 point1)
        {
            return Vector3.Dot(point0, point1);
        }

        /// <summary>
        /// Returns a Point3 that is made from the largest components of two Points.
        /// </summary>
        public static Point3 Max(Point3 point0, Point3 point1)
        {
            int maxX = Mathf.Max(point0.x, point1.x);
            int maxY = Mathf.Max(point0.y, point1.y);
            int maxZ = Mathf.Max(point0.z, point1.z);
            return new Point3(maxX, maxY, maxZ);
        }

        /// <summary>
        /// Returns a Point3 that is made from the smallest components of two Points.
        /// </summary>
        public static Point3 Min(Point3 point0, Point3 point1)
        {
            int minX = Mathf.Min(point0.x, point1.x);
            int minY = Mathf.Min(point0.y, point1.y);
            int minZ = Mathf.Min(point0.z, point1.z);
            return new Point3(minX, minY, minZ);
        }

        /// <summary>
        /// Multiplies two points component-wise.
        /// </summary>
        public static Point3 Scale(Point3 point0, Point3 point1)
        {
            return new Point3(point0.x * point1.x, point0.y * point1.y, point0.z * point1.z);
        }

        /// <summary>
        /// Sets the x, y, z components of an existing Point3.
        /// </summary>
        public void Set(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}