namespace CustomUnityLibrary
{
    using UnityEngine;

    /// <summary>
    /// Rotates a component by some degrees over a specified number of seconds
    /// This method suffers from floating point precision issues, and the rotation will not be precise.
    /// </summary>
    [DisallowMultipleComponent]
    public class RotationOverTime : MonoBehaviour
    {
        [Tooltip("Number of degrees by which to rotate.  Increase degrees to increase speed for an infinite rotation.")]
        [SerializeField]
        private Vector3 degrees;

        [Tooltip("Number of seconds for which to rotate.  Set to 0 for an infinite rotation.")]
        [SerializeField]
        private float seconds;

        [Tooltip("Whether or not to start the rotation when the component is first added")]
        [SerializeField]
        private bool playOnAwake = true;

        private Vector3 rotationCompleted = Vector3.zero;
        private Vector3 speed;
        private Vector3 startRotation;
        private bool speedsInitialized = false;
        private bool playing = false;

        void Start()
        {
            if (playOnAwake)
            {
                playing = true;
            }
        }

        void FixedUpdate()
        {
            if (playing)
            {
                if (!speedsInitialized)
                {
                    speed = GetBalancedRotationSpeeds(degrees, seconds);
                    startRotation = transform.eulerAngles;
                    speedsInitialized = true;
                }
                UpdateRotation();
                if (IsRotationComplete())
                {
                    Destroy(this);
                }
            }
        }

        /// <summary>
        /// Begins the rotation
        /// </summary>
        public void Play()
        {
            playing = true;
        }

        /// <summary>
        /// Change the degrees by which this Rotation Over Time will rotate
        /// </summary>
        /// <param name="degrees">Degrees to rotate by, or speed if infinite rotation</param>
        public void SetDegrees(Vector3 degrees)
        {
            if (playing)
            {
                Debug.LogWarning("The degrees cannot be changed on a Rotation Over Time after the rotation has begun.", gameObject);
                return;
            }
            this.degrees = degrees;
        }

        /// <summary>
        /// Duration for which the transform will rotate
        /// </summary>
        /// <param name="seconds">Number of seconds to rotate, or 0 for an infinite rotation</param>
        public void SetSeconds(float seconds)
        {
            if (playing)
            {
                Debug.LogWarning("The seconds cannot be chanegd on a Rotation Over Time after the rotation has begun.", gameObject);
                return;
            }
            this.seconds = seconds;
        }

        /// <summary>
        /// Gets the rotation speed for each axis such that they will all complete their rotation at the same time
        /// </summary>
        /// <param name="degrees">Degrees the transform will be rotated</param>
        /// <param name="seconds">Duration for which the transform will rotate</param>
        /// <returns>The rotation speeds required to complete the rotation in the correct amount of time</returns>
        private Vector3 GetBalancedRotationSpeeds(Vector3 degrees, float seconds)
        {
            if (seconds == 0)
            {
                seconds = 1.0f;
            }
            float degreesWeight = (degrees.x + degrees.y + degrees.z) / 3.0f;
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

        /// <summary>
        /// Rotate the transform
        /// </summary>
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

        /// <summary>
        /// Check if the rotation has completed
        /// </summary>
        /// <returns>Whether or not the rotation is complete</returns>
        private bool IsRotationComplete()
        {
            bool xRotationIsComplete = Mathf.Abs(rotationCompleted.x) >= Mathf.Abs(degrees.x);
            bool yRotationIsComplete = Mathf.Abs(rotationCompleted.y) >= Mathf.Abs(degrees.y);
            bool zRotationIsComplete = Mathf.Abs(rotationCompleted.z) >= Mathf.Abs(degrees.z);
            return xRotationIsComplete && yRotationIsComplete && zRotationIsComplete && seconds != 0;
        }
    }
}