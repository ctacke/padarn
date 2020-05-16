using System;

namespace OpenNETCF.WindowsCE
{
    internal static class StringHelper
    {
        public static string ConvertVerbatimLineEndings(string input)
        {
            return input.Replace(@"\r\n", "\r\n");
        }

        public static string RemoveNullCharacters(string s)
        {
            if (s == null)
            {
                return null;
            }
            if (s.IndexOf('\0') > -1)
            {
                return s.Replace("\0", string.Empty);
            }
            return s;
        }

        internal static bool EqualsIgnoreCase(string s1, string s2)
        {
            if (string.IsNullOrEmpty(s1) && string.IsNullOrEmpty(s2))
            {
                return true;
            }
            if (string.IsNullOrEmpty(s1) || string.IsNullOrEmpty(s2))
            {
                return false;
            }
            if (s2.Length != s1.Length)
            {
                return false;
            }
            return (0 == string.Compare(s1, 0, s2, 0, s2.Length, StringComparison.OrdinalIgnoreCase));
        }
    }
}