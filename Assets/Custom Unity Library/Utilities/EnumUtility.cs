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
        /// <typeparam name="TEnumeration">The enumeration type to get the names for</typeparam>
        /// <returns>An array of the names of the enumeration</returns>
        public static string[] GetNames<TEnumeration>()
        {
            return Enum.GetNames(typeof(TEnumeration));
        }

        /// <summary>
        /// Gets an array of all values in an enum
        /// </summary>
        /// <typeparam name="TEnumeration">The enumeration type to get the values for</typeparam>
        /// <returns>An array of the values of the enumeration</returns>
        public static TEnumeration[] GetValues<TEnumeration>()
        {
            return (TEnumeration[])Enum.GetValues(typeof(TEnumeration));
        }

        /// <summary>
        /// Gets the length of the enum
        /// </summary>
        /// <typeparam name="TEnumeration">The enumeration type to get the count for</typeparam>
        /// <returns>The number of enumerators in the enumeration</returns>
        public static int Count<TEnumeration>()
        {
            return GetNames<TEnumeration>().Length;
        }
    }
}