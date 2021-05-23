using System;
using System.Collections.Generic;
using System.Text;

namespace Zen.Simulator
{
    [Flags]
    public enum CharcterType
    {
        Unspecified = 0,
        Letter = 0x1,
        Digit = 0x10,
        Punctuation = 0x100,
    }

    public sealed class StringOption
    {
        public StringOption()
        {
            _charTypes = new List<CharcterType>(new CharcterType[] { CharcterType.Letter, });
        }

        public int MinimumLength
        {
            get { return _minLength; }
            set { _minLength = value; AdjustMinMax(); }
        }
        public int MaximumLength
        {
            get { return _maxLength; }
            set { _maxLength = value; AdjustMinMax(); }
        }
        public string Format
        {
            get { return _format; }
            set { _format = value; }
        }
        public List<CharcterType> CharcterTypes
        {
            get { return _charTypes; }
            set { _charTypes = value; }
        }
        public string[] Values
        {
            get { return _values; }
            set { _values = value; }
        }

        #region private functions
        private void AdjustMinMax()
        {
            if (_minLength > _maxLength)
                _maxLength = _minLength;
        }
        #endregion

        #region private data
        private int _minLength = 8;
        private int _maxLength = 8;
        private string _format;
        private List<CharcterType> _charTypes;
        private string[] _values;
        #endregion
    }

    public sealed class RandomString : IRandom<string>
    {
        public RandomString()
            : this(new StringOption())
        {
        }
        public RandomString(StringOption option)
        {
            Init(option);
        }

        public string Next()
        {
            string[] values = _option.Values;
            if (values != null && values.Length > 0)
                return values[_random.Next(values.Length)];

            int bufferLength = _random.Next(_option.MinimumLength, _option.MaximumLength);
            StringBuilder buffer = new StringBuilder(bufferLength);
            for (int index = 0, fmtCharCount = 0; index - fmtCharCount < bufferLength; )
            {
                Char c = (Char)(_random.Next(0, 127));
                if ((_useLetter && Char.IsLetter(c)) || (_useDigit && Char.IsDigit(c)) || (_usePunctuation && Char.IsPunctuation(c)))
                {
                    if (_adjustedFmt != null && _adjustedFmt[index] != '#')
                    {
                        buffer.Append(_adjustedFmt[index]);
                        fmtCharCount++;
                    }
                    else
                    {
                        buffer.Append(c);
                    }
                    index++;
                }
            }

            return buffer.ToString();
        }

        private void Init(StringOption option)
        {
            _option = option;
            _adjustedFmt = string.IsNullOrEmpty(_option.Format) ? null : _option.Format;

            if (_adjustedFmt != null)
            {
                StringBuilder buffer = new StringBuilder();

                int index = 0;
                int valueCharCount = 0;
                while (valueCharCount < _option.MaximumLength)
                {
                    if (index == _adjustedFmt.Length) // re-start over the format index
                    {
                        if (valueCharCount == 0)
                            break;
                        index = 0;
                    }

                    Char c = _adjustedFmt[index++];
                    if (c == '#')
                        valueCharCount++;

                    buffer.Append(c);
                }
                
                _adjustedFmt = (valueCharCount > 0)? buffer.ToString(): null;
            }

            CharcterType ct = (CharcterType)Zen.Utilities.Generics.EnumConverter.ToBitFlag(_option.CharcterTypes);
            _useLetter = (ct & CharcterType.Letter) == CharcterType.Letter;
            _useDigit = (ct & CharcterType.Digit) == CharcterType.Digit;
            _usePunctuation = (ct & CharcterType.Punctuation) == CharcterType.Punctuation;
        }

        #region private data
        private Random _random = new Random();
        private string _adjustedFmt;
        private StringOption _option;

        private bool _useLetter;
        private bool _useDigit;
        private bool _usePunctuation;
        #endregion
    }



}
