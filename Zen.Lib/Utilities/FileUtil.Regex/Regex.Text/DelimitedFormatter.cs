using System.Collections.Generic;
using System.Text;

namespace Zen.Utilities.Text
{
    public class Delimiter
    {
        static readonly string DefaultDelimiter = ",";

        public static string ToString<T>(IEnumerable<T> dataList)
        {
            return ToString(dataList, DefaultDelimiter);
        }
        public static string ToString(System.Collections.IEnumerable dataList)
        {
            return ToString((IEnumerable<object>)dataList, DefaultDelimiter);
        }
        public static string ToString<T>(IEnumerable<T> dataList, string delimiter)
        {
            return ToString(dataList, delimiter, null);
        }

        public static string ToString<T>(IEnumerable<T> dataList, string delimiter, string valueFormat)
        {
            StringBuilder buffer = new StringBuilder();

            int counter = 0;
            string formatted = null; 
            foreach (T dtItem in dataList)
            {
                if (counter++ != 0)
                    buffer.Append(delimiter);

                if (dtItem != null)
                {
                    formatted = (valueFormat == null) ? dtItem.ToString() : string.Format(valueFormat, dtItem.ToString());
                    buffer.Append(formatted);
                }
            }

            return buffer.ToString();
        }
    }
}
