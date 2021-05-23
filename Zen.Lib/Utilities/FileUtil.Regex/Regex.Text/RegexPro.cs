using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Zen.Utilities.Text
{
    public sealed class RegexPatterns
    {
        public static readonly string Comma = @",";
        public static readonly string Space = @"\s";
        public static readonly string SpaceOptional = @"\s*";
        public static readonly string SpaceRequired = @"\s+";
        public static readonly string CommaOrSpace1 = @"[,\s]{1}";

        public static readonly string WordFmt = @"\b{0}\b";
        public static readonly string DelimiterWithSpaceFmt = @"\s*{0}\s*";
        public static readonly string NotCharFmt = @"[^{0}]";

        public static readonly string EmptyLine = @"^\s*$";

        public static readonly string Name = @"[A-Z]{1}[a-z-]+"; // Smith and O’Brian and Doe-Ray
        public static readonly string FirstNameLastName = string.Format(@"({0})({1})({2})", RegexPatterns.Name, @"\s+", RegexPatterns.Name);
        public static readonly string LastNameFirstName = string.Format(@"({0})({1})({2})", RegexPatterns.Name, @"\s*,\s*", RegexPatterns.Name);

        /// <example>Provider=SQLOLEDB;Server=LocalHost;</example>
        public static readonly string KeyValueEscapeChars = ";=#";
        public static readonly string KVName = string.Format(@"(?<name>[^{0}]+)", KeyValueEscapeChars); // any char except those escape chars
        public static readonly string KVValue = @"(?<value>.*?)"; // anything as the value, but not greedy
        public static readonly string KVEndMark = @"(?<end>$|(?<!;);$|(?<!;);(?!;))";
        public static readonly string KeyValuePair = string.Format(@"{0}\s*=\s*{1}{2}", KVName, KVValue, KVEndMark); 
    }

    public sealed class RegexPro
    {
        public static string ReplaceMultiple(string text, Dictionary<string, string> replacements)
        {
            string pattern = string.Format("{0}", Delimiter.ToString(replacements.Keys, "|"));
            return Regex.Replace(text, pattern, delegate(Match m) { return replacements[m.Value]; });
        }
        public static string ReplaceMultipleWord(string text, Dictionary<string, string> replacements)
        {
            string pattern = string.Format("\b({0})\b", Delimiter.ToString(replacements.Keys, "|"));
            return Regex.Replace(text, pattern, delegate(Match m) { return replacements[m.Value]; });
        }

        public static string ReplaceDelimiter(string text, string srcDelimiter, string tgtDelimiter)
        {
            string pattern = string.Format(RegexPatterns.DelimiterWithSpaceFmt, srcDelimiter);
            return Regex.Replace(text, pattern, tgtDelimiter);
        }

        /// <summary> FirstName LastName => LastName, FirstName </summary>
        public static string ToLastNameFirstName(string firstNameLastName)
        {
            return Regex.Replace(firstNameLastName, RegexPatterns.FirstNameLastName, "$3, $1");
        }
        /// <summary> LastName, FirstName => FirstName LastName </summary>
        public static string ToFirstNameLastName(string lastNamefirstName)
        {
            return Regex.Replace(lastNamefirstName, RegexPatterns.LastNameFirstName, "$3 $1");
        }

        public static void ParseKeyValuePair(string text, IDictionary<string, string> kvMap)
        {
            foreach (Match match in KeyValueRegex.Matches(text))
            {
                kvMap.Add(match.Groups["name"].Value.Trim(), match.Groups["value"].Value.Replace(";;", ";"));
            }
        }

        #region private functions
        private static string AddSpaceToPattern(string pattern, bool optional)
        {
            return optional ? (RegexPatterns.SpaceOptional + pattern + RegexPatterns.SpaceOptional) : (RegexPatterns.SpaceRequired + pattern + RegexPatterns.SpaceRequired);
        }
        #endregion

        #region private data
        private static Regex KeyValueRegex
            = new Regex(RegexPatterns.KeyValuePair, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        #endregion

    }
}
