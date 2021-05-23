using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Zen.Utilities.Generics
{
    /// <summary>
    /// C# enum type converter
    /// </summary>
    public sealed class EnumConverter
    {
        public static string ToName(Enum enumValue)
        {
            return enumValue.ToString();
        }
        public static object ToValue(Type enumType, string enumName)
        {
            return Enum.Parse(enumType, enumName);
        }

        public static IEnumerable<string> ToNames(Type enumType, bool skipNull)
        {
            return ToNames(Enum.GetNames(enumType), skipNull);
        }
        public static IEnumerable<string> ToNames(IEnumerable<int> valueCollection)
        {
            return ToNames((IEnumerable)valueCollection, false);
        }
        public static IEnumerable<string> ToNames(IEnumerable valueCollection)
        {
            return ToNames(valueCollection, false);
        }

        /// <summary> 
        /// Return enum names from the collection. 
        /// Skip null value (Unknown, UnSpecified) if asked.
        /// </summary>
        public static IEnumerable<string> ToNames(IEnumerable valueCollection, bool skipNull)
        {
            List<string> names = new List<string>();
            foreach (object value in valueCollection)
            {
                string enumName = value.ToString();
                if (!skipNull || !_rgx.IsMatch(enumName))
                    names.Add(enumName);
            }

            return names;
        }

        public static IEnumerable ToValues(Type enumType, IEnumerable<string> nameCollection)
        {
            Debug.Assert(TypeInterrogator.IsEnumType(enumType));

            IList values = (IList)GenBuilder.BuildInstance(typeof(List<>), new Type[] { enumType });
            foreach (string name in nameCollection)
                values.Add(Enum.Parse(enumType, name));

            return values;
        }

        /// <summary>
        /// Expect enum collection but C# does not support enum constraint
        /// </summary>
        public static Enum ToBitFlag(IEnumerable valueCollection) // where T: System.Enum
        {
            if (valueCollection == null)
                return BitFlag.None;

            BitFlag combinedFlag = BitFlag.None;
            foreach (object item in valueCollection)
            {
                BitFlag flag = (BitFlag)((int)item);
                combinedFlag = combinedFlag | flag;
            }

            return combinedFlag;
        }

        #region private region
        [Flags]
        private enum BitFlag
        {
            None = 0x0000,
            One = 0x0001,
            Two = 0x0002,
            Four = 0x0004,
            Eight = 0x0008,
            Ten = 0x0010,
            Twenty = 0x0020,
            Fourty = 0x0040,
            Eighty = 0x0080,
            OneHundred = 0x0100,
            TwoHundred = 0x0200,
            FourHundred = 0x0400,
            EightHundred = 0x0800,
            OneThousand = 0x1000,
            TwoThousand = 0x2000,
            FourThousand = 0x4000,
            EightThousand = 0x8000,
        }
        private static Regex _rgx = new Regex("(Unknown)|(Unspecified)");
        #endregion
    }

    public sealed class CollConverter
    {
        public static Array ToArray<ItemT>(IEnumerable<ItemT> source)
        {
            return ToArray(source, TypeInterrogator.GetItemType(source.GetType()));
        }

        public static Array ToArray(IEnumerable source, Type itemT)
        {
            if (TypeInterrogator.IsArrayType(source.GetType()))
                return (Array)source;

            int count = GetCount(source);
            Array target = Array.CreateInstance(itemT, count);
            int ind = 0;
            foreach (object it in source)
                target.SetValue(it, ind++);

            return target;
        }

        public static string[] ToStringArray(IEnumerable source)
        {
            int count = GetCount(source);
            string[] target = new string[count];
            int ind = 0;
            foreach (object it in source)
                target.SetValue(it.ToString(), ind++);

            return target;
        }

        public static IList ToList(IEnumerable source)
        {
            return ToList(source, TypeInterrogator.GetItemType(source.GetType()));
        }
        public static IList ToList<ItemT>(IEnumerable<ItemT> source)
        {
            return ToList(source, TypeInterrogator.GetItemType(source.GetType()));
        }

        public static IList ToList(IEnumerable source, Type itemT)
        {
            if (TypeInterrogator.IsListType(source.GetType()))
                return (IList)source;

            IList target = (IList)GenBuilder.BuildInstance(typeof(List<>), new Type[] { itemT });
            foreach (object it in source)
                target.Add(it);

            return target;
        }

        #region private functions
        public static int GetCount(IEnumerable source)
        {
            ICollection lst = (source as ICollection);
            if (lst != null)
                return lst.Count;

            int count = 0;
            foreach (object it in source)
                count++;

            return count;
        }
        #endregion
    }

    public sealed class StrAdapter
    {
        public StrAdapter(string rawValue)
        {
            _rawValue = rawValue;
        }

        public static implicit operator int(StrAdapter obj)
        {
            return Int32.Parse(obj._rawValue);
        }
        public static implicit operator long(StrAdapter obj)
        {
            return Int64.Parse(obj._rawValue);
        }
        public static implicit operator double(StrAdapter obj)
        {
            return Double.Parse(obj._rawValue);
        }
        public static implicit operator float(StrAdapter obj)
        {
            return Single.Parse(obj._rawValue);
        }
        public static implicit operator DateTime(StrAdapter obj)
        {
            return DateTime.Parse(obj._rawValue);
        }
        public static implicit operator Decimal(StrAdapter obj)
        {
            return (new Decimal(Double.Parse(obj._rawValue)));
        }
        public static implicit operator string(StrAdapter obj)
        {
            return obj._rawValue;
        }

        #region private data
        private string _rawValue;
        #endregion
    }

    public sealed class DictionaryAdapter
    {
        static readonly string SettingFmt = "{0}={1};";
        static readonly string SettingFmtRgx = @"(.+)=(.+)";
        static readonly char[] SettingDelimiter = new char[] { ';' };

        public static List<KVPair<TKey, TValue>> ToList<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            List<KVPair<TKey, TValue>> dictionaryList = new List<KVPair<TKey, TValue>>(dictionary.Count);
            foreach (KeyValuePair<TKey, TValue> kv in dictionary)
                dictionaryList.Add(new KVPair<TKey, TValue>(kv.Key, kv.Value));

            return dictionaryList;
        }

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(List<KVPair<TKey, TValue>> dictionaryList)
        {
            Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>(dictionaryList.Count);
            foreach (KVPair<TKey, TValue> kv in dictionaryList)
                dictionary.Add(kv.Key, kv.Value);

            return dictionary;
        }

        public static string ToString<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            System.Text.StringBuilder buffer = new System.Text.StringBuilder();
            string valueString = null;
            foreach (KeyValuePair<TKey, TValue> kv in dictionary)
            {
                if (kv.Value != null)
                    valueString = kv.Value.ToString();
                if (!string.IsNullOrEmpty(valueString))
                    buffer.AppendFormat(SettingFmt, kv.Key.ToString(), valueString);

            }
            return buffer.ToString();
        }

        public static Dictionary<string, string> ToDictionary(string dictionaryText)
        {
            string[] groups = dictionaryText.Split(SettingDelimiter, StringSplitOptions.RemoveEmptyEntries);

            Regex rgx = new Regex(SettingFmtRgx);
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (string setting in groups)
            {
                Match m = rgx.Match(setting);
                if (m.Success)
                    dictionary.Add(m.Groups[1].Value.ToString().Trim(), m.Groups[2].Value.ToString().Trim());
            }

            return dictionary;
        }
    }
}
