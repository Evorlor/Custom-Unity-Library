namespace CustomUnityLibrary
{
    using UnityEngine;

    /// <summary>
    /// These are extensions for Vector2s
    /// </summary>
    public static class Vector2Extensions
    {
        /// <summary>
        /// Converts a Vector2 to radians
        /// </summary>
        /// <param name="vector2">The Vector2 to be converted</param>
        /// <returns>The Vector2 in radians</returns>
        public static float ToRadians(this Vector2 vector2)
        {
            return Mathf.Atan2(vector2.x, -vector2.y);
        }

        /// <summary>
        /// Converts a Vector2 to degrees
        /// </summary>
        /// <param name="vector2">The Vector2 to convert to degrees</param>
        /// <returns>The Vector2 in degrees</returns>
        public static float ToDegrees(this Vector2 vector2)
        {
            return vector2.ToRadians() * Mathf.Rad2Deg;
        }

        /// <summary>
        /// Converts the Vector2 to a Point2
        /// </summary>
        /// <param name="vector2">The Vector2 to convert to a Point2</param>
        /// <returns>The Point2 version of the Vector2</returns>
        public static Point2 ToPoint(this Vector2 vector2)
        {
            return new Point2((int)vector2.x, (int)vector2.y);
        }
    }
}