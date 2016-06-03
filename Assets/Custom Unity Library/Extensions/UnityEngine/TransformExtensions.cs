namespace CustomUnityLibrary
{
    using UnityEngine;

    /// <summary>
    /// These are extensions for Transforms
    /// </summary>
    public static class TransformExtensions
    {
        /// <summary>
        /// Activates all children of the transform
        /// </summary>
        /// <param name="transform">The parent whose children will be activated</param>
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
        /// <param name="transform">The parent whose children will be deactived</param>
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
        /// <param name="transform">The parent whose children will be traversed</param>
        /// <param name="name">The name of the target child</param>
        /// <returns>The target child, if found</returns>
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
        /// <param name="transform">The transform to be rotated</param>
        /// <param name="degrees">The degrees by which to rotate this transform</param>
        /// <param name="seconds">The length of time it takes for this transform to complete its rotation</param>
        public static void RotateOverTime(this Transform transform, Vector3 degrees, float seconds)
        {
            var oldRotationOverTimeComponents = transform.gameObject.GetComponents<RotationOverTime>();
            foreach (var oldRotationOverTimeComponent in oldRotationOverTimeComponents)
            {
                Object.Destroy(oldRotationOverTimeComponent);
            }
            var rotateOverTimeComponent = transform.gameObject.AddComponent<RotationOverTime>();
            rotateOverTimeComponent.hideFlags = HideFlags.HideInInspector;
            rotateOverTimeComponent.SetDegrees(degrees);
            rotateOverTimeComponent.SetSeconds(seconds);
        }
    }
}