namespace CustomUnityLibrary
{
    using System;

    /// <summary>
    /// Utility methods for Enumerations
    /// </summary>
    public static class EnumUtility
    {
        /// <summary>
        /// Gets an array of all names in an enum
        /// </summary>
        public static string[] GetNames<TEnumeration>()
        {
            return Enum.GetNames(typeof(TEnumeration));
        }

        /// <summary>
        /// Gets an array of all values in an enum
        /// </summary>
        public static TEnumeration[] GetValues<TEnumeration>()
        {
            return (TEnumeration[])Enum.GetValues(typeof(TEnumeration));
        }

        /// <summary>
        /// Gets the length of the enum
        /// </summary>
        public static int Count<TEnumeration>()
        {
            return GetNames<TEnumeration>().Length;
        }
    }
}