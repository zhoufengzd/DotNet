using System;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CTSummation.Enterprise.Utilities
{
    public static class FieldTypeConverter
    {
        private const string Replacement = @"$1 $2 $4";
        public const string SummationDateFormat = "yyyyMMdd";

        private static readonly IFormatProvider DateTimeFormatInfo;
        private static readonly Regex DateRegularExpression;
        private static readonly string[] DateFormatListOne;
        private static readonly string[] DateFormatListTwo;

        static FieldTypeConverter()
        {
            DateRegularExpression = new Regex(@"^([A-Za-z\s]+)([1-9]|[12][0-9]|3[01])(nd|rd|th)\s+(\d{4})$", RegexOptions.None);
            //DateFormatListOne = new string[3] {
            //    "yyyyMMdd",
            //    "dd/MM/yyyy",
            //    "MMM-dd-yyyy"
            //};
            DateFormatListOne = new string[3] {
                "yyyyMMdd",
                "MM/dd/yyyy",
                "MMM-dd-yyyy"
            };
            DateFormatListTwo = new string[2] {
                "MMM d yyyy",
                "dddd MMMM d yyyy"
            };
            DateTimeFormatInfo = CultureInfo.CurrentCulture.DateTimeFormat;
        }

        public static bool TryConvertDate(string value, out string convertedDate)
        {
            DateTime result;
            bool isValid = DateTime.TryParseExact(value, DateFormatListOne, DateTimeFormatInfo,
                    DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal, out result);
            if (!isValid)
            {
                bool isMatch = DateRegularExpression.IsMatch(value);
                if (isMatch)
                {
                    value = DateRegularExpression.Replace(value, Replacement);
                    isValid = DateTime.TryParseExact(value, DateFormatListTwo, DateTimeFormatInfo,
                        DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal, out result);
                }
            }
            convertedDate = isValid ? result.ToString(SummationDateFormat, DateTimeFormatInfo) : String.Empty;
            return isValid;
        }
    }
}
