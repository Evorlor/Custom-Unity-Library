using UnityEngine;

/// <summary>
/// Contains x and y coordinates.
/// Similar to Vector3, but stores ints instead of floats.
/// </summary>
[System.Serializable]
public struct Point3
{
    /// <summary>
    /// Coordinates of Point
    /// </summary>
    public int x, y, z;

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
    /// Gets the Vector3 form of the point
    /// </summary>
    public Vector3 ToVector()
    {
        return new Vector3(x, y, z);
    }
}