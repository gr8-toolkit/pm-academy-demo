// D.Maximov for PM Tech Academy (c)
using System;
using System.Text;

namespace NotesApp.Tools
{
    /// <summary>
    /// String generators.
    /// </summary>
    public static class StringGenerator
    {
        private static readonly Random Rnd = new Random();

        /// <summary>
        /// Generates random numbers string.
        /// </summary>
        /// <param name="length">Resulted string length.</param>
        /// <param name="allowLeadingZero">Allow leading zeros in resulted string.</param>
        /// <exception cref="ArgumentOutOfRangeException">Throws when length is negative value.</exception>
        /// <returns>Returns generated string.</returns>
        public static string GenerateNumbersString(int length, bool allowLeadingZero)
        {
            if (length == 0) return string.Empty;
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Length must be positive value");
            }

            var bytes = new byte[length];

            do
            {
                Rnd.NextBytes(bytes);
            }
            while (!allowLeadingZero && ((bytes[0] + 10) % 10 == 0));

            var builder = new StringBuilder(length) { Length = length };
            var offset = Convert.ToInt32('0');
            for (var i = 0; i < length; i++)
            {
                builder[i] = Convert.ToChar(((bytes[i] + 10) % 10) + offset);
            }

            return builder.ToString();
        }
    }
}
