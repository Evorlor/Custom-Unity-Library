namespace CustomUnityLibrary
{
    using UnityEngine;

    /// <summary>
    /// These are extensions for Rigidbody2Ds
    /// </summary>
    public static class Rigidbody2DExtensions
    {
        private const float DefaultGravity = -9.8f;

        /// <summary>
        /// Applies a force to the Rigidbody2D such that it will land, if unobstructed, at the target position.
        /// The arch [0.0f, 1.0f] determines the percent of arch to provide between the minimum and maximum arch.
        /// An arch or 0 or 1 will set the trajectory such that the entire force is being applied.
        /// Any value between 0 and 1 will have a reduced force.
        /// If target is out of range, it will still fire, but not reach the target.
        /// Use rigidbody2D.IsWithinRange(Vector2, float) to determine if the target is within range ahead of time, if destired.
        /// This only takes the Y gravity into account, and X gravity will not affect the trajectory.
        /// If there is no gravity and no ConstantForce2D is attached, a ConstantForce2D with a gravity of (0.0f, -9.8f) will be attached.
        /// </summary>
        /// <param name="rigidbody2D">The rigidbody2D to apply the force to</param>
        /// <param name="target">The destination for the rigidbody2D</param>
        /// <param name="force">The force to apply to the rigidbody2D</param>
        /// <param name="arch">The arch to apply to the rigidbody's trajectory</param>
        /// <returns></returns>
        public static bool SetTrajectory(this Rigidbody2D rigidbody2D, Vector2 target, float force, float arch = 0.5f)
        {
            arch = Mathf.Clamp(arch, 0, 1);
            var origin = rigidbody2D.position;
            float x = target.x - origin.x;
            float y = target.y - origin.y;
            float gravity = -Physics2D.gravity.y;
            if (gravity == 0)
            {
                var constantForce2D = rigidbody2D.GetComponent<ConstantForce2D>();
                if (!constantForce2D)
                {
                    Debug.LogWarning("There is no gravity and " + rigidbody2D.name + " does not have a ConstantForce2D attached.  A ConstantForce2D with the default gravity of -9.8f will be added.");
                    constantForce2D = rigidbody2D.gameObject.AddComponent<ConstantForce2D>();
                    constantForce2D.force = new Vector2(0, DefaultGravity);
                }
                gravity = -constantForce2D.force.y;
            }
            float b = force * force - y * gravity;
            float discriminant = b * b - gravity * gravity * (x * x + y * y);
            discriminant = Mathf.Max(0, discriminant);
            float discriminantSquareRoot = Mathf.Sqrt(discriminant);
            float minTime = Mathf.Sqrt((b - discriminantSquareRoot) * 2) / Mathf.Abs(gravity);
            float maxTime = Mathf.Sqrt((b + discriminantSquareRoot) * 2) / Mathf.Abs(gravity);
            float time = (maxTime - minTime) * arch + minTime;
            float vx = x / time;
            float vy = y / time + time * gravity / 2;
            var trajectory = new Vector2(vx, vy);
            trajectory = Vector2.ClampMagnitude(trajectory, force);
            rigidbody2D.AddForce(trajectory, ForceMode2D.Impulse);
            return true;
        }

        /// <summary>
        /// Checks if the Rigidbody2D is within range such that if rigidbody2D.SetTrajectory(Vector2, float) is called on it, it will be able to reach its target.
        /// If there is no gravity and no ConstantForce2D is attached, a default gravity of -9.8f will be assumed.
        /// </summary>
        /// <param name="rigidbody2D">The rigidbody2D whose range will be checked</param>
        /// <param name="target">The target used while checking the range</param>
        /// <param name="force">The force that would be applied for the range check</param>
        /// <param name="arch">The arch that would be used for the range check</param>
        /// <returns>Whether or not the Rigidbody2D would be within range with the supplied parameters</returns>
        public static bool IsWithinRange(this Rigidbody2D rigidbody2D, Vector2 target, float force, float arch = 0.5f)
        {
            arch = Mathf.Clamp(arch, 0, 1);
            var origin = rigidbody2D.position;
            float x = target.x - origin.x;
            float y = target.y - origin.y;
            float gravity = -Physics2D.gravity.y;
            if (gravity == 0)
            {
                var constantForce2D = rigidbody2D.GetComponent<ConstantForce2D>();
                if (!constantForce2D)
                {
                    Debug.LogWarning("There is no gravity and " + rigidbody2D.name + " does not have a ConstantForce2D attached.  The default gravity of -9.8f will be assumed.");
                    gravity = -DefaultGravity;
                }
                else
                {
                    gravity = -constantForce2D.force.y;
                }
            }
            float b = force * force - y * gravity;
            float discriminant = b * b - gravity * gravity * (x * x + y * y);
            return discriminant >= 0;
        }
    }
}