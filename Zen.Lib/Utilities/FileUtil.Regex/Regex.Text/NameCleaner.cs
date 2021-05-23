using System;
using System.Collections.Generic;
using System.Text;

namespace Zen.Utilities.Text
{
    public sealed class NameCleaner
    {
        static readonly string InvalidNameChars = string.Format("[{0}-]", new string(System.IO.Path.GetInvalidFileNameChars()));
        static readonly char[] WordDelimiter = new char[] { '_', '-', '.', ' ' };
        static readonly char Space = ' ';
        static readonly Dictionary<string, string> AbbreviationMap = null;

        public static string ToPhrase(string identifier)
        {
            if (string.IsNullOrEmpty(identifier))
                return null;

            StringBuilder buffer = new StringBuilder((int)(identifier.Length * 1.1));
            buffer.Append(Char.ToUpper(identifier[0]));

            bool foundLower = false;
            for (int i = 1; i < identifier.Length; i++)
            {
                char ch = identifier[i];
                if (Char.IsLower(ch))
                    foundLower = true;
                else if (Char.IsUpper(ch) && foundLower)
                    buffer.Append(' ');
                else if (ch == '_' || ch == '-' || ch == '.' || ch == '#')
                    ch = Space;

                buffer.Append(ch);
            }

            return RegexPro.ReplaceMultiple(buffer.ToString(), AbbreviationMap);
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

        #region static initialization: abbreviation map
        static NameCleaner()
        {
            AbbreviationMap = new Dictionary<string, string>();
            AbbreviationMap.Add("Opt", "Options");
        }
        #endregion
    }
}
