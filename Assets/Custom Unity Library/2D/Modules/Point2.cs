using UnityEngine;

/// <summary>
/// Contains x and y coordinates.
/// Similar to Vector2, but stores ints instead of floats.
/// </summary>
[System.Serializable]
public struct Point2
{
    /// <summary>
    /// Coordinates of Point
    /// </summary>
    public int x, y;

    /// <summary>
    /// Creates a new Point
    /// </summary>
    public Point2(int x, int y)
    {
        this.x = x;
        this.y = y;
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
}