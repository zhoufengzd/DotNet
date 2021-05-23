using System;
using System.Collections.Generic;


namespace ModiLib
{
    using MODI;
    public enum LanguageEnum
    {
        //UnSpecified = 0,
        Sysdefault = 1,

        //Chinese_traditional,
        //Chinese_simplified,
        //Czech,
        //Danish,
        //German,
        //Greek,
        English,
        Spanish,
        //Finnish,
        French,
        //Hungarian,
        //Italian,
        //Japanese,
        //Korean,
        //Dutch,
        //Norwegian,
        //Polish,
        //Portuguese,
        //Russian,
        //Swedish,
        //Turkish,
    }

    public sealed class MiLanguageMapper
    {
        public static MiLANGUAGES ToMiLanguage(LanguageEnum language)
        {
            return _languageMap[language];
        }

        #region private functions
        /// <summary>
        /// Will throw exception if LanguageEnum and MiLANGUAGES 
        /// are not in sync
        /// </summary>
        private static Dictionary<LanguageEnum, MiLANGUAGES> LoadMap()
        {
            Array languageEnums = Enum.GetValues(typeof(LanguageEnum));
            Dictionary<LanguageEnum, MiLANGUAGES> dict = new Dictionary<LanguageEnum, MiLANGUAGES>(languageEnums.Length);

            Type mlType = typeof(MiLANGUAGES);
            foreach (LanguageEnum le in languageEnums)
            {
                //if (le == LanguageEnum.UnSpecified)
                //    continue;

                dict.Add(le, (MiLANGUAGES)Enum.Parse(mlType, string.Format("miLANG_{0}", le.ToString().ToUpper())));
            }

            return dict;
        }
        #endregion

        #region private data
        private static Dictionary<LanguageEnum, MiLANGUAGES> _languageMap = LoadMap();
        #endregion

    }
}
