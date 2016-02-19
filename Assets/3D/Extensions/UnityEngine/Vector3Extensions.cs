using UnityEngine;

/// <summary>
/// These are extensions for Vector3s
/// </summary>
public static class Vector3Extensions
{
    /// <summary>
    /// Casts a cone and checks if the target is within its volume. Parameters are the target position, the angle of the cone, the direction to cast the cone, and optionally, the range of the cone.
    /// </summary>
    public static bool ConeCast(this Vector3 vector3, Vector3 target, float degreeOfAccuracy, Vector3 direction, float range = Mathf.Infinity)
    {
        if (Vector3.Distance(vector3, target) > range)
        {
            return false;
        }
        float actualAngle = Vector3.Dot(direction, target - vector3);
        float coneAngle = Mathf.Cos(Mathf.Deg2Rad * degreeOfAccuracy);
        return actualAngle >= coneAngle;
    }
}