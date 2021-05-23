using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Zen.Utilities.Text
{
    // RTF Format: Ascii
    // <header>\rtf <charset> \deff? <fonttbl> <filetbl>? <colortbl>?
    //   <stylesheet>? <listtables>? <revtbl>?

    #region internal helpers
    internal sealed class RTFFormat
    {
        public const string MagicNumber = @"{\rtf";
        public const string Ansi = @"\ansi\ansicpg1252";
        public const string DefaultFont = @"\deff0";
        public const string Tail = "}";

        public const string ColorTableFmt = @"{\colortbl;<<#COLORS#>>}";
        public const string ParagraphFmt = @"\par {0} \pard";
    }

    internal sealed class RTFColorTable
    {
        static readonly string RTFColorFmt = @"\red{0}\green{1}\blue{2}";
        static readonly string Black = @"\red0\green0\blue0";

        public RTFColorTable()
        {
            LoadColorTable();
        }

        public string ToRTFColor(string colorName)
        {
            if (_colorTable.ContainsKey(colorName))
                return _colorTable[colorName];

            Debug.Assert(true, "Color is not defined!");
            return Black;
        }

        #region private functions
        private void LoadColorTable()
        {
            if (_colorTable != null)
                return;

            _colorTable = new Dictionary<string, string>();

            Type colorType = typeof(Color);
            PropertyInfo[] properties = colorType.GetProperties(BindingFlags.Public | BindingFlags.Static);
            foreach (PropertyInfo property in properties)
            {
                object obj = property.GetValue(null, null);
                if (obj is Color)
                {
                    Color color = (Color)obj;
                    _colorTable.Add(property.Name, string.Format(RTFColorFmt, color.R, color.G, color.B));
                }
            }
        }
        #endregion

        #region private data
        private Dictionary<string, string> _colorTable;
        #endregion
    }
    #endregion

    /// <summary>
    /// Produce RTF
    /// </summary>
    public sealed class RTFFormatter
    {
        const string DefaultAnsiHeader = RTFFormat.MagicNumber + RTFFormat.Ansi + RTFFormat.DefaultFont;

        static readonly Color[] RedGreenBlue = { Color.Red, Color.Green, Color.Blue };
        static readonly RTFColorTable RtfColorTable = new RTFColorTable();

        public RTFFormatter()
            : this(RedGreenBlue)
        {
        }
        public RTFFormatter(Color[] colors)
        {
            _colors = colors;
            LoadControllers();
        }

        public string WriteHeader()
        {
            return DefaultAnsiHeader + _colorTable;
        }

        public string WriteParagraph(string text)
        {
            return string.Format(RTFFormat.ParagraphFmt, text);
        }

        public string WriteFooter()
        {
            return RTFFormat.Tail;
        }

        public string HighLight(string srcText, Color color)
        {
            if (!_colorIndexMap.ContainsKey(color))
                return srcText;

            return string.Format(@"\cf{0} {1} \cf0", _colorIndexMap[color], srcText);
        }

        #region private functions
        private void LoadControllers()
        {
            int colorCount = _colors.Length;
            _colorIndexMap = new Dictionary<Color, int>(colorCount);
            StringBuilder colorsBuffer = new StringBuilder();

            // \cf0: reserved, by default is black
            for (int ind = 0; ind < colorCount; ind++)
            {
                Color color = _colors[ind];
                _colorIndexMap.Add(color, ind + 1);

                colorsBuffer.Append(RtfColorTable.ToRTFColor(color.Name));
                colorsBuffer.Append(';');
            }

            _colorTable = RTFFormat.ColorTableFmt.Replace("<<#COLORS#>>", colorsBuffer.ToString());
        }
        #endregion

        #region private data
        private Color[] _colors;
        private string _colorTable;
        private Dictionary<Color, int> _colorIndexMap;
        #endregion
    }

    public sealed class RTFInvoker
    {
        public static string HighLight(string srcText, Dictionary<Color, IEnumerable<string>> colorKeywords)
        {
            RTFFormatter fmt = new RTFFormatter();
            Dictionary<string, Color> keyWordColorMap = new Dictionary<string, Color>();
            foreach (KeyValuePair<Color, IEnumerable<string>> kv in colorKeywords)
            {
                foreach (string keyWord in kv.Value)
                    keyWordColorMap.Add(keyWord, kv.Key);
            }

            StringBuilder buffer = new StringBuilder();
            buffer.Append(fmt.WriteHeader());

            string pattern = string.Format(@"\b({0})\b", Delimiter.ToString(keyWordColorMap.Keys, "|"));
            buffer.Append(Regex.Replace(srcText, pattern,
                delegate(Match m) { return fmt.HighLight(m.Value, keyWordColorMap[m.Value]); }));

            buffer.Append(fmt.WriteFooter());
            return buffer.ToString();
        }

    }
}
