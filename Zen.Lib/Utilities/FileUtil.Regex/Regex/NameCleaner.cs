using System;
using System.Collections.Generic;
using System.Text;

namespace Zen.Utilities
{
    public sealed class NameCleaner
    {
        static readonly string InvalidNameChars = string.Format("[{0}-]", new string(System.IO.Path.GetInvalidFileNameChars()));
        static readonly char[] WordDelimiter = new char[] { '_', '-', '.', ' ' };

        public static string ToPhrase(string identifier)
        {
            if (string.IsNullOrEmpty(identifier))
                return null;

            StringBuilder buffer = new StringBuilder((int)(identifier.Length * 1.1));
            buffer.Append(Char.ToUpper(identifier[0]));

            for (int i = 1; i < identifier.Length; i++)
            {
                if (Char.IsUpper(identifier[i]))
                    buffer.Append(' ');

                buffer.Append(identifier[i]);
            }

            return buffer.ToString();
        }

        public static string ToIdentifier(IEnumerable<string> words)
        {
            StringBuilder result = new StringBuilder();
            foreach (string wd in words)
                result.Append(ToPascalCase(wd));

            return result.ToString();
        }

        public static string ToPascalCase(string wd)
        {
            StringBuilder buffer = new StringBuilder(wd);
            for (int i = 0; i < buffer.Length; i++)
            {
                if (i > 0)
                    buffer[i] = Char.ToLower(buffer[i]);
                else
                    buffer[i] = Char.ToUpper(buffer[i]);
            }
            return buffer.ToString();
        }

        public static string Clean(string rawName, string replacement)
        {
            return System.Text.RegularExpressions.Regex.Replace(rawName, InvalidNameChars, replacement);
        }
    }
}
