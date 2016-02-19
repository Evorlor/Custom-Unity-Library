using UnityEngine;

/// <summary>
/// These are extensions for Vector2s
/// </summary>
public static class Vector2Extensions
{
    /// <summary>
    /// Returns the value in radians for the Vector2
    /// </summary>
    public static float ToRadians(this Vector2 vector2)
    {
        return Mathf.Atan2(vector2.x, -vector2.y);
    }

    /// <summary>
    /// Returns the value in degrees for the Vector2
    /// </summary>
    public static float ToDegrees(this Vector2 vector2)
    {
        return vector2.ToRadians() * Mathf.Rad2Deg;
    }
}