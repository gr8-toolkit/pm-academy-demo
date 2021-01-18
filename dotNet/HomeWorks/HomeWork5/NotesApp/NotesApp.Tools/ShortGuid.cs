// D.Maximov for PM Tech Academy (c)
using System;
using System.Text;

namespace NotesApp.Tools
{
    /// <summary>
    /// Short GUID extensions.
    /// Short GUID - it's guid encoded to base64, 
    /// with replaced '/' -> '_' and '+' -> '-', 
    /// and without equals '==' tail.
    /// </summary>
    public static class ShortGuid
    {
        private const string FormatError = "Given string is not a GUID or short base64 GUID";
        private const int ShortIdLength = 22;
        private const int ShortIdExtLength = 24;

        /// <summary>
        /// Encode GUID to short GUID.
        /// Short GUID - it's guid encoded to base64, 
        /// with replaced '/' -> '_' and '+' -> '-', 
        /// and without equals '==' tail.
        /// </summary>
        /// <returns>Returns encoded string</returns>
        public static string ToShortId(this Guid guid)
        {
            var sb = new StringBuilder(Convert.ToBase64String(guid.ToByteArray()));
            sb.Replace("/", "_").Replace("+", "-");
            return sb.ToString(0, ShortIdLength);
        }

        /// <summary>
        /// Decode short GUID.
        /// Short GUID - it's guid encoded to base64, 
        /// with replaced '/' -> '_' and '+' -> '-', 
        /// and without equals '==' tail.
        /// </summary>
        /// <returns>Returns decoded string or null if given string is null.</returns>
        /// <exception cref="FormatException">Throws when given string is not a valid GUID or short GUID.</exception>
        public static Guid? FromShortId(this string guid)
        {
            if (guid == null) return default;
            if (Guid.TryParse(guid, out var result)) return result;

            if (guid.Length != ShortIdLength && guid.Length != ShortIdExtLength)
            {
                throw new FormatException(FormatError);
            }

            var sb = new StringBuilder(guid, ShortIdExtLength);
            sb.Replace('_', '/').Replace('-', '+');
            if (guid.Length == ShortIdLength) sb.Append('=', 2);

            try
            {
                byte[] buffer = Convert.FromBase64String(sb.ToString());
                return new Guid(buffer);
            }
            catch(FormatException ex)
            {
                throw new FormatException(FormatError, ex);
            }
        }
    }
}
