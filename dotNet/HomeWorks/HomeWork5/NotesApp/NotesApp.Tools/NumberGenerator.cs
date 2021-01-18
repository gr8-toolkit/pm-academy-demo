// D.Maximov for PM Tech Academy (c)
using System;

namespace NotesApp.Tools
{
    /// <summary>
    /// String generators.
    /// </summary>
    public static class NumberGenerator
    {
        /// <summary>
        /// Generate random positive <see cref="long"/>.
        /// </summary>
        /// <param name="length">
        /// Resulted string representation length.
        /// Should be between 1..18
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">Throws when length is out range.</exception>
        /// <returns>Returns generated number.</returns>
        public static long GeneratePositiveLong(int length)
        {
            if (length < 1 || length > 18)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Length must be positive value");
            }
            var str = StringGenerator.GenerateNumbersString(length, false);
            return long.Parse(str);
        }
    }
}