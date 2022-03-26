using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace WebApplicationNeoPharm.Extentions
{
    public static class ClsStringExtentions
    {

        /// <summary>
        /// check if a string is null or whitespaces
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string str) => string.IsNullOrWhiteSpace(str);
        /// <summary>
        /// Convert string to base 64 string
        /// </summary>
        /// <param name="stringToencode"></param>
        /// <returns></returns>
        public static string ToBase64String(this string stringToencode) => Convert.ToBase64String(Encoding.UTF8.GetBytes(stringToencode));
        /// <summary>
        /// does this string is "true" (not case sensative)
        /// </summary>
        /// <param name="boolString"></param>
        /// <returns></returns>
        public static bool IsTrue(this string boolString) => bool.TrueString.Equals(boolString, StringComparison.OrdinalIgnoreCase);
        /// <summary>
        /// Check if the string starts with atleast one of the string provided
        /// </summary>
        /// <param name="str"></param>
        /// <param name="stringsToCheck"></param>
        /// <returns></returns>
        public static bool StartsWithRange(this string str, IEnumerable<string> stringsToCheck)
        {
            if (stringsToCheck == null)
            {
                return false;
            }
            foreach (string startStr in stringsToCheck)
            {
                if (str.StartsWith(startStr))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Removes ALL whitespaces from a string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string TrimAll(string str) => Regex.Replace(str, @"\s+", "");
        public static string Truncate(this string value, int width)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= width ? value : value.Substring(0, width);
        }

        public static string CalculateHash(this string value)
        {
            ulong hashedValue = 3074457345618258791ul;
            for (int i = 0; i < value.Length; i++)
            {
                hashedValue += value[i];
                hashedValue *= 3074457345618258799ul;
            }
            return hashedValue.ToString("X");
        }

        // Add padding to string while also trancating the string to the width
        public static string PadWidth(this string value, int width, char pad = ' ')
        {
            return value.Truncate(width).PadLeft(width, pad);
        }
        // Add padding to string while also trancating the string to the width
        public static string PadWidthRight(this string value, int width, char pad = ' ')
        {
            return value.Truncate(width).PadRight(width, pad);
        }
    }
}
