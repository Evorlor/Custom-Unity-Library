/// <summary>
/// These are extensions for Strings
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Removes the substring at the end of a string
    /// </summary>
    public static string TrimEnd(this string source, string value)
    {
        if (!source.EndsWith(value))
            return source;

        return source.Remove(source.LastIndexOf(value));
    }
}