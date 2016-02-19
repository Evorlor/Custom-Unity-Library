using UnityEngine;

/// <summary>
/// These are extensions for Transforms
/// </summary>
public static class TransformExtensions
{
    /// <summary>
    /// Activates all children of the transform
    /// </summary>
    public static void ActivateChildren(this Transform transform)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            child.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Deactivates all children of the transform
    /// </summary>
    public static void DeactivateChildren(this Transform transform)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            child.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Finds the child with the specified name at the highest level in the hierarchy.
    /// </summary>
    public static Transform FindDescendant(this Transform transform, string name)
    {
        foreach (Transform child in transform)
        {
            if (child.name == name)
            {
                return child;
            }
        }
        foreach (Transform child in transform)
        {
            return child.FindDescendant(name);
        }
        return null;
    }

    /// <summary>
    /// Rotates the transform to a specified rotation over a set number of seconds.
    /// For an infinite rotation, multiply the degrees by a float to adjust the speed, and set the duration to 0 seconds.
    /// Calling RotateOverTime() or RotateTowardsOverTime() will cancel any pending rotations on this transform.
    /// This method suffers from floating point precision issues, and the rotation will not be precise.
    /// </summary>
    public static void RotateTowardsOverTime(this Transform transform, Vector3 degrees, float seconds)
    {
        var rotationToBeMade = degrees - transform.rotation.eulerAngles;
        if (degrees.z > 270.0f && transform.rotation.eulerAngles.z < 90.0f)
        {
            rotationToBeMade.z = -(360.0f - degrees.z + transform.rotation.eulerAngles.z);
        }
        if (transform.rotation.eulerAngles.z > 270.0f && degrees.z < 90.0f)
        {
            rotationToBeMade.z = 360.0f - transform.rotation.eulerAngles.z + degrees.z;
        }
        RotateOverTime(transform, rotationToBeMade, seconds);
    }

    /// <summary>
    /// Rotates the transform by a specified number of degrees over a set number of seconds.
    /// For an infinite rotation, multiply the degrees by a float to adjust the speed, and set the duration to 0 seconds.
    /// Calling RotateOverTime() or RotateTowardsOverTime() will cancel any pending rotations on this transform.
    /// This method suffers from floating point precision issues, and the rotation will not be precise.
    /// </summary>
    public static void RotateOverTime(this Transform transform, Vector3 degrees, float seconds)
    {
        var oldRotateOverTimeComponents = transform.gameObject.GetComponents<RotateOverTime>();
        foreach (var oldRotateOverTimeComponent in oldRotateOverTimeComponents)
        {
            Object.Destroy(oldRotateOverTimeComponent);
        }

        var rotateOverTimeComponent = transform.gameObject.AddComponent<RotateOverTime>();
        rotateOverTimeComponent.hideFlags = HideFlags.HideInInspector;
        rotateOverTimeComponent.Degrees = degrees;
        rotateOverTimeComponent.Seconds = seconds;
    }
}

class RotateOverTime : MonoBehaviour
{
    public Vector3 Degrees { get; set; }
    public float Seconds { get; set; }

    private Vector3 rotationCompleted = Vector3.zero;
    private Vector3 speed;
    private Vector3 startRotation;

    void Start()
    {
        speed = GetBalancedRotationSpeeds(Degrees, Seconds);
        startRotation = transform.eulerAngles;
    }

    void FixedUpdate()
    {
        UpdateRotation();
        if (IsRotationComplete())
        {
            Destroy(this);
        }
    }

    private Vector3 GetBalancedRotationSpeeds(Vector3 degrees, float seconds)
    {
        if (seconds == 0)
        {
            seconds = 1.0f;
        }
        float degreesWeight = (Degrees.x + Degrees.y + Degrees.z) / 3.0f;
        float speedModifier = degreesWeight / seconds;
        float totalChangeInDegrees = Mathf.Abs(degrees.x) + Mathf.Abs(degrees.y) + Mathf.Abs(degrees.z);
        float xWeight = Mathf.Abs(degrees.x) / totalChangeInDegrees;
        float yWeight = Mathf.Abs(degrees.y) / totalChangeInDegrees;
        float zWeight = Mathf.Abs(degrees.z) / totalChangeInDegrees;
        float xSpeed = xWeight * speedModifier * 3.0f;
        float ySpeed = yWeight * speedModifier * 3.0f;
        float zSpeed = zWeight * speedModifier * 3.0f;
        return new Vector3(xSpeed, ySpeed, zSpeed);
    }

    private void UpdateRotation()
    {
        rotationCompleted += Time.deltaTime * speed;
        var rotation = Quaternion.Euler(rotationCompleted + startRotation).eulerAngles;
        bool rotationIsValid = !(float.IsNaN(rotationCompleted.x) || float.IsNaN(rotationCompleted.y) || float.IsNaN(rotationCompleted.z) && float.IsNaN(startRotation.x) || float.IsNaN(startRotation.y) || float.IsNaN(startRotation.z) || float.IsNaN(rotation.x) || float.IsNaN(rotation.y) || float.IsNaN(rotation.z));
        if (!rotationIsValid)
        {
            Destroy(this);
        }
        transform.eulerAngles = rotation;
    }

    private bool IsRotationComplete()
    {
        bool xRotationIsComplete = Mathf.Abs(rotationCompleted.x) >= Mathf.Abs(Degrees.x);
        bool yRotationIsComplete = Mathf.Abs(rotationCompleted.y) >= Mathf.Abs(Degrees.y);
        bool zRotationIsComplete = Mathf.Abs(rotationCompleted.z) >= Mathf.Abs(Degrees.z);
        return xRotationIsComplete && yRotationIsComplete && zRotationIsComplete && Seconds != 0;
    }
}