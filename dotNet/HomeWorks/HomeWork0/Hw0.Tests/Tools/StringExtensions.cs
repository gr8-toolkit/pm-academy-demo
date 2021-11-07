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
        public static string NormalizeOutput(this string str, bool normalizeCase = false)
        {
            if (str is null) return str;

            // Trim whitespaces and NewLine chars
            str = str.Trim().Trim(Environment.NewLine.ToCharArray());

            // Convert to upper case
            if (normalizeCase) str = str.ToUpperInvariant();
            return str;
        }
    }
}
