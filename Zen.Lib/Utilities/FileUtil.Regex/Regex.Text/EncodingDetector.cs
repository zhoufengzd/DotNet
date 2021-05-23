using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Zen.Utilities.Text
{
    public enum CharEncoding
    {
        Unknown,
        AlphabeticPresentationForms,
        Arabic,
        Armenian,
        Arrows,
        BasicLatin,
        Bengali,
        BlockElements,
        Bopomofo,
        BopomofoExtended,
        BoxDrawing,
        BraillePatterns,
        Buhid,
        CJKCompatibility,
        CJKCompatibilityForms,
        CJKCompatibilityIdeographs,
        CJKRadicalsSupplement,
        CJKSymbolsandPunctuation,
        CJKUnifiedIdeographs,
        CJKUnifiedIdeographsExtensionA,
        Cherokee,
        CombiningDiacriticalMarks,
        CombiningDiacriticalMarksforSymbols,
        CombiningHalfMarks,
        CombiningMarksforSymbols,
        ControlPictures,
        CurrencySymbols,
        Cyrillic,
        CyrillicSupplement,
        Devanagari,
        Dingbats,
        EnclosedAlphanumerics,
        EnclosedCJKLettersandMonths,
        Ethiopic,
        GeneralPunctuation,
        GeometricShapes,
        Georgian,
        Greek,
        GreekExtended,
        GreekandCoptic,
        Gujarati,
        Gurmukhi,
        HalfwidthandFullwidthForms,
        HangulCompatibilityJamo,
        HangulJamo,
        HangulSyllables,
        Hanunoo,
        Hebrew,
        HighPrivateUseSurrogates,
        HighSurrogates,
        Hiragana,
        IPAExtensions,
        IdeographicDescriptionCharacters,
        Kanbun,
        KangxiRadicals,
        Kannada,
        Katakana,
        KatakanaPhoneticExtensions,
        Khmer,
        KhmerSymbols,
        Lao,
        LatinExtendedAdditional,
        LetterlikeSymbols,
        Limbu,
        LowSurrogates,
        Malayalam,
        MathematicalOperators,
        MiscellaneousSymbols,
        MiscellaneousSymbolsandArrows,
        MiscellaneousTechnical,
        Mongolian,
        Myanmar,
        NumberForms,
        Ogham,
        OpticalCharacterRecognition,
        Oriya,
        PhoneticExtensions,
        PrivateUse,
        PrivateUseArea,
        Runic,
        Sinhala,
        SmallFormVariants,
        SpacingModifierLetters,
        Specials,
        SuperscriptsandSubscripts,
        SupplementalMathematicalOperators,
        Syriac,
        Tagalog,
        Tagbanwa,
        TaiLe,
        Tamil,
        Telugu,
        Thaana,
        Thai,
        Tibetan,
        UnifiedCanadianAboriginalSyllabics,
        VariationSelectors,
        YiRadicals,
        YiSyllables,
        YijingHexagramSymbols
    }

    public sealed class EncodingDetector
    {
        public EncodingDetector()
        {
            LoadControllers();
        }

        public Dictionary<CharEncoding, int> Detect(string text)
        {
            Dictionary<CharEncoding, int> result = new Dictionary<CharEncoding, int>();
            foreach (KeyValuePair<CharEncoding, Regex> kv in _detectors)
            {
                if (kv.Value.IsMatch(text))
                {
                    if (result.ContainsKey(kv.Key))
                        result[kv.Key] += 1;
                    else
                        result[kv.Key] = 1;
                }
            }

            return result;
        }

        private void LoadControllers()
        {
            const string EncoderRegexFmt = @"\p{Is{0}}";
            Array encodings = Enum.GetValues(typeof(CharEncoding));
            _detectors = new Dictionary<CharEncoding,Regex>(encodings.Length);
            foreach (object encoding in encodings)
            {
                CharEncoding encd = (CharEncoding)encoding;
                if (encd != CharEncoding.Unknown)
                    _detectors.Add(encd, new Regex(string.Format(EncoderRegexFmt, encd.ToString())));
            }
        }

        private Dictionary<CharEncoding, Regex> _detectors;
    }
}
