namespace CustomUnityLibrary
{
    /// <summary>
    /// Utility methods for floats
    /// </summary>
    public static class FloatUtility
    {
        /// <summary>
        /// Maximum by which a float can be inaccurate.
        /// This is the value to use for float comparisons.
        /// For comparisons, Mathf.Approximately(float, float) should be used instead.
        /// </summary>
        public const float MaxInaccuracy = 0.000002f;
    }
}