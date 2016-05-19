﻿namespace CustomUnityLibrary
{
    using System.Text.RegularExpressions;

    /// <summary>
    /// These are extensions for Strings
    /// </summary>
    public static class StringExtensions
    {
        private const string SpacingRegex = @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))";
        private const string SpacingSpace = " $0";

        /// <summary>
        /// Removes the substring at the end of a string
        /// </summary>
        public static string TrimEnd(this string source, string value)
        {
            if (!source.EndsWith(value))
                return source;

            return source.Remove(source.LastIndexOf(value));
        }

        /// <summary>
        /// Adds the spacing to a string such that each word or acronym will be separated by spaces.
        /// </summary>
        public static string AddSpacing(this string source)
        {
            return Regex.Replace(source, SpacingRegex, SpacingSpace);
        }
    }
}