using System;

namespace Hw0.Tests.Tools
{
    /// <summary>
    /// String extension for unit tests.
    /// </summary>
    internal static class StringExtensions
    {
        /// <summary>
        /// Normalizes console output.
        /// </summary>
        public static string NormalizeOutput(
            this string str,
            bool trimWhitespaces = false,
            bool trimNewLineEnding = false,
            bool normalizeCase = false)
        {
            if (str is null)
                return str;

            // Trim whitespaces
            if (trimWhitespaces)
                str = str.Trim();

            // Trim NewLine chars
            if (trimNewLineEnding)
                str = str.TrimEnd(Environment.NewLine.ToCharArray());

            // Convert to upper case
            if (normalizeCase)
                str = str.ToUpperInvariant();

            return str;
        }
    }
}
