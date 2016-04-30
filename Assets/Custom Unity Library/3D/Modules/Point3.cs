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
}