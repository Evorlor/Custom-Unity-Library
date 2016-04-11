using System;

public class EnumUtility
{
    /// <summary>
    /// Gets an array of all names in an enum
    /// </summary>
    public static string[] GetNames<EnumerationType>()
    {
        return Enum.GetNames(typeof(EnumerationType));
    }

    /// <summary>
    /// Gets an array of all values in an enum
    /// </summary>
    public static EnumerationType[] GetValues<EnumerationType>()
    {
        return (EnumerationType[])Enum.GetValues(typeof(EnumerationType));
    }

    /// <summary>
    /// Gets the length of the enum
    /// </summary>
    public static int Count<EnumerationType>()
    {
        return GetNames<EnumerationType>().Length;
    }
}