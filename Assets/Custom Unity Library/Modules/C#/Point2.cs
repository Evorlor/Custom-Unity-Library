namespace CustomUnityLibrary
{
    using UnityEngine;

    /// <summary>
    /// Contains x and y coordinates.
    /// Similar to Vector2, but stores ints instead of floats.
    /// </summary>
    [System.Serializable]
    public struct Point2
    {
        /// <summary>
        /// Shorthand for writing Point2(0, 0).
        /// </summary>
        public static Point2 zero = new Point2(0, 0);

        /// <summary>
        /// Shorthand for writing Point2(1, 1).
        /// </summary>
        public static Point2 one = new Point2(1, 1);

        /// <summary>
        /// Shorthand for writing Point2(0, -1).
        /// </summary>
        public static Point2 down = new Point2(0, -1);

        /// <summary>
        /// Shorthand for writing Point2(-1, 0).
        /// </summary>
        public static Point2 left = new Point2(-1, 0);

        /// <summary>
        /// Shorthand for writing Point2(1, 0).
        /// </summary>
        public static Point2 right = new Point2(1, 0);

        /// <summary>
        /// Shorthand for writing Point2(0, 1).
        /// </summary>
        public static Point2 up = new Point2(0, 1);

        /// <summary>
        /// X component of the point.
        /// </summary>
        public int x;

        /// <summary>
        /// Y component of the point.
        /// </summary>
        public int y;

        /// <summary>
        /// Returns the length of this point
        /// </summary>
        public float magnitude
        {
            get
            {
                return Mathf.Sqrt(x + x * y * y);
            }
        }

        /// <summary>
        /// Returns this point as a Vector2 with a magnitude of 1
        /// </summary>
        public Vector2 normalized
        {
            get
            {
                return ((Vector2)this).normalized;
            }
        }

        /// <summary>
        /// Returns the squared length of this point
        /// </summary>
        public float sqrMagnitude
        {
            get
            {
                return x * x * y * y;
            }
        }

        /// <summary>
        /// Access the x or y component using [0] or [1] respectively.
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
                    default:
                        throw new System.IndexOutOfRangeException("Use 0 to access x, and use 1 to access y.");
                }
            }
        }

        /// <summary>
        /// Creates a new Point2
        /// </summary>
        public Point2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Creates a new Point2 from an existing Point2
        /// </summary>
        public Point2(Point2 point2)
        {
            x = point2.x;
            y = point2.y;
        }

        /// <summary>
        /// Creates a new Point2 from an existing Point3
        /// </summary>
        public Point2(Point3 point3)
        {
            x = point3.x;
            y = point3.y;
        }

        /// <summary>
        /// Creates a new Point2 from a Vector2
        /// </summary>
        public Point2(Vector2 vector2)
        {
            x = (int)vector2.x;
            y = (int)vector2.y;
        }

        /// <summary>
        /// Creates a new Point2 from a Vector3
        /// </summary>
        public Point2(Vector3 vector3)
        {
            x = (int)vector3.x;
            y = (int)vector3.y;
        }

        /// <summary>
        /// Compares two Points for equality
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is Point2 && this == (Point2)obj;
        }

        /// <summary>
        /// Gets the Hash Code of the Point
        /// </summary>
        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode();
        }

        /// <summary>
        /// Compares two Points for equality
        /// </summary>
        public static bool operator ==(Point2 point0, Point2 point1)
        {
            return point0.x == point1.x && point0.y == point1.y;
        }

        /// <summary>
        /// Compares two Points for inequality
        /// </summary>
        public static bool operator !=(Point2 point0, Point2 point1)
        {
            return !(point0 == point1);
        }

        /// <summary>
        /// Multiplies each component by a float
        /// </summary>
        public static Vector2 operator *(Point2 point, float number)
        {
            return new Vector2(point.x * number, point.y * number);
        }

        /// <summary>
        /// Multiplies each component by an int
        /// </summary>
        public static Point2 operator *(Point2 point, int number)
        {
            return new Point2(point.x * number, point.y * number);
        }

        /// <summary>
        /// Divides each component by a float
        /// </summary>
        public static Vector2 operator /(Point2 point, float number)
        {
            return new Vector2(point.x / number, point.y / number);
        }

        /// <summary>
        /// Divides each component by an int
        /// </summary>
        public static Point2 operator /(Point2 point, int number)
        {
            return new Point2(point.x / number, point.y / number);
        }

        /// <summary>
        /// Performs component-wise addition between two Points
        /// </summary>
        public static Point2 operator +(Point2 point0, Point2 point1)
        {
            return new Point2(point0.x + point1.x, point0.y + point1.y);
        }

        /// <summary>
        /// Performs component-wise subtraction between two Points such that the new Point is point0 minus point 1
        /// </summary>
        public static Point2 operator -(Point2 point0, Point2 point1)
        {
            return new Point2(point0.x - point1.x, point0.y - point1.y);
        }

        /// <summary>
        /// Returns the string format of the Point
        /// </summary>
        public override string ToString()
        {
            return "(" + x + "," + y + ")";
        }

        /// <summary>
        /// Allows for cast from Vector2 to Point2.
        /// Truncates any decimals on the Vector2's components.
        /// </summary>
        public static explicit operator Point2(Vector2 vector2)
        {
            return new Point2((int)vector2.x, (int)vector2.y);
        }

        /// <summary>
        /// Allows for cast from Point2 to Vector2
        /// </summary>
        public static implicit operator Vector2(Point2 point2)
        {
            return new Vector2(point2.x, point2.y);
        }

        /// <summary>
        /// Allows for cast from Vector3 to Point2.
        /// Truncates any decimals on the Vector3's components.
        /// </summary>
        public static explicit operator Point2(Vector3 vector3)
        {
            return new Point2((int)vector3.x, (int)vector3.y);
        }

        /// <summary>
        /// Allows for cast from Point2 to Vector3
        /// </summary>
        public static implicit operator Vector3(Point2 point2)
        {
            return new Vector3(point2.x, point2.y);
        }

        /// <summary>
        /// Returns the angle in degrees between from and to.
        /// </summary>
        public static float Angle(Point2 from, Point2 to)
        {
            return Vector2.Angle(from, to);
        }

        /// <summary>
        /// Returns the distance between two points.
        /// </summary>
        public static float Distance(Point2 point0, Point2 point1)
        {
            return Vector2.Distance(point0, point1);
        }

        /// <summary>
        /// Dot Product of two Points
        /// </summary>
        public static float Dot(Point2 point0, Point2 point1)
        {
            return Vector2.Dot(point0, point1);
        }

        /// <summary>
        /// Returns a Point2 that is made from the largest components of two Points.
        /// </summary>
        public static Point2 Max(Point2 point0, Point2 point1)
        {
            int maxX = Mathf.Max(point0.x, point1.x);
            int maxY = Mathf.Max(point0.y, point1.y);
            return new Point2(maxX, maxY);
        }

        /// <summary>
        /// Returns a Point2 that is made from the smallest components of two Points.
        /// </summary>
        public static Point2 Min(Point2 point0, Point2 point1)
        {
            int minX = Mathf.Min(point0.x, point1.x);
            int minY = Mathf.Min(point0.y, point1.y);
            return new Point2(minX, minY);
        }

        /// <summary>
        /// Multiplies two points component-wise.
        /// </summary>
        public static Point2 Scale(Point2 point0, Point2 point1)
        {
            return new Point2(point0.x * point1.x, point0.y * point1.y);
        }

        /// <summary>
        /// Sets the x and y components of an existing Point2.
        /// </summary>
        public void Set(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}