using UnityEngine;

/// <summary>
/// These are extensions for Rigidbody2Ds
/// </summary>
public static class Rigidbody2DExtensions
{
    /// <summary>
    /// Applies the force to the Rigidbody2D such that it will land, if unobstructed, at the target position.  The arch [0, 1] determines the percent of arch to provide between the minimum and maximum arch.  If target is out of range, it will fail to launch and return false; otherwise, it will launch and return true.  This only takes the Y gravity into account, and X gravity will not affect the trajectory.
    /// </summary>
    public static bool SetTrajectory(this Rigidbody2D rigidbody2D, Vector2 target, float force, float arch = 0.5f)
    {
        Mathf.Clamp(arch, 0, 1);
        var origin = rigidbody2D.position;
        float x = target.x - origin.x;
        float y = target.y - origin.y;
        float gravity = -Physics2D.gravity.y;
        float b = force * force - y * gravity;
        float discriminant = b * b - gravity * gravity * (x * x + y * y);
        if (discriminant < 0)
        {
            return false;
        }
        float discriminantSquareRoot = Mathf.Sqrt(discriminant);
        float minTime = Mathf.Sqrt((b - discriminantSquareRoot) * 2) / Mathf.Abs(gravity);
        float maxTime = Mathf.Sqrt((b + discriminantSquareRoot) * 2) / Mathf.Abs(gravity);
        float time = (maxTime - minTime) * arch + minTime;
        float vx = x / time;
        float vy = y / time + time * gravity / 2;
        var trajectory = new Vector2(vx, vy);
        rigidbody2D.AddForce(trajectory, ForceMode2D.Impulse);
        return true;
    }
}