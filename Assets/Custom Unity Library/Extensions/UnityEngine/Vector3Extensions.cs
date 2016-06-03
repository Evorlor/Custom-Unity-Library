namespace CustomUnityLibrary
{
    using UnityEngine;

    /// <summary>
    /// These are extensions for Vector3s
    /// </summary>
    public static class Vector3Extensions
    {
        /// <summary>
        /// Casts a cone and checks if the target is within its volume.
        /// </summary>
        /// <param name="vector3">Position at where the point of the cone is and the cone shape starts</param>
        /// <param name="target">Target to check if within the volume of the cone</param>
        /// <param name="degrees">Width of the cone in degrees</param>
        /// <param name="direction">Direction the cone is opening towards</param>
        /// <param name="range">Distance the cone will check before being cut off</param>
        /// <returns>Whether or not the target position was within the volume of the cone</returns>
        public static bool ConeCast(this Vector3 vector3, Vector3 target, float degrees, Vector3 direction, float range = Mathf.Infinity)
        {
            if (Vector3.Distance(vector3, target) > range)
            {
                return false;
            }
            float actualAngle = Vector3.Dot(direction, target - vector3);
            float coneAngle = Mathf.Cos(Mathf.Deg2Rad * degrees);
            return actualAngle >= coneAngle;
        }

        /// <summary>
        /// Returns the Point3 conversion of this Vector3
        /// </summary>
        /// <param name="vector3">Converts the Vector3 to a Point3</param>
        /// <returns>The Point3 version of the Vector3</returns>
        public static Point3 ToPoint(this Vector3 vector3)
        {
            return new Point3((int)vector3.x, (int)vector3.y, (int)vector3.z);
        }
    }
}