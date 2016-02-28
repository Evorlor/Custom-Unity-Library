using UnityEngine;

/// <summary>
/// These are extensions for Rigidbody2Ds
/// </summary>
public static class Rigidbody2DExtensions
{
    /// <summary>
    /// Applies a force to the Rigidbody2D such that it will land, if unobstructed, at the target position.
    /// The arch [0.0f, 1.0f] determines the percent of arch to provide between the minimum and maximum arch.
    /// An arch or 0 or 1 will set the trajectory such that the entire force is being applied.
    /// Any value between 0 and 1 will have a reduced force.
    /// If target is out of range, it will still fire, but not reach the target.
    /// Use rigidbody2D.IsWithinRange(Vector2, float) to determine if the target is within range ahead of time, if destired.
    /// This only takes the Y gravity into account, and X gravity will not affect the trajectory.
    /// If there is no gravity, a fixed gravity of (0.0f, -9.8f) will be applied.
    /// </summary>
    public static bool SetTrajectory(this Rigidbody2D rigidbody2D, Vector2 target, float force, float arch = 0.5f)
    {
        arch = Mathf.Clamp(arch, 0, 1);
        var origin = rigidbody2D.position;
        float x = target.x - origin.x;
        float y = target.y - origin.y;
        float gravity = -Physics2D.gravity.y;
        if (gravity == 0)
        {
            var fixedGravity = rigidbody2D.gameObject.AddComponent<FixedGravity2D>();
            gravity = -fixedGravity.GetGravity().y;
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
    /// </summary>
    public static bool IsWithinRange(this Rigidbody2D rigidbody2D, Vector2 target, float force, float arch = 0.5f)
    {
        Mathf.Clamp(arch, 0, 1);
        var origin = rigidbody2D.position;
        float x = target.x - origin.x;
        float y = target.y - origin.y;
        float gravity = -Physics2D.gravity.y;
        if (gravity == 0)
        {
            var temporaryGameObject = new GameObject();
            var temporaryFixedGravity = temporaryGameObject.AddComponent<FixedGravity2D>();
            gravity = -temporaryFixedGravity.GetGravity().y;
            Object.Destroy(temporaryGameObject);
        }
        float b = force * force - y * gravity;
        float discriminant = b * b - gravity * gravity * (x * x + y * y);
        return discriminant >= 0;
    }
}

[RequireComponent(typeof(Rigidbody2D))]
///Applies gravity to a single Rigidbody2D, optionally ignoring the project-wide gravity
public class FixedGravity2D : MonoBehaviour
{
    [Tooltip("The fixed gravity to be applied to the Rigidbody2D")]
    [SerializeField]
    private Vector2 gravity = new Vector2(0, -9.8f);

    [Tooltip("Whether or not to ignore the project's gravity")]
    [SerializeField]
    private bool ignoreProjectGravity = true;

    private Rigidbody2D body2D;
    private float gravityScale;

    void Awake()
    {
        body2D = GetComponent<Rigidbody2D>();
        gravityScale = body2D.gravityScale;
    }

    void FixedUpdate()
    {
        body2D.AddForce(gravity * body2D.mass);
    }

    void OnValidate()
    {
        if (ignoreProjectGravity)
        {
            body2D.gravityScale = 0;
        }
        else
        {
            body2D.gravityScale = gravityScale;
        }
    }

    /// <summary>
    /// Gets the fixed gravity applied to the Rigidbody2D
    /// </summary>
    public Vector2 GetGravity()
    {
        return gravity;
    }


    /// <summary>
    /// Sets the fixed gravity applied to the Rigidbody2D
    /// </summary>
    public void SetGravity(Vector2 gravity)
    {
        this.gravity = gravity;
    }
}