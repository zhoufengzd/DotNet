using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Zen.Utilities
{
    public sealed class IniFileHandler
    {
        #region Import 
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern long WritePrivateProfileString(string section,
            string key, string val, string filePath);
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string section,
                 string key, string def, string retVal,
            int size, string filePath);
        #endregion

        public static List<string> GetCategories(string iniFile)
        {
            string returnString = new string(' ', 65536);
            GetPrivateProfileString(null, null, null, returnString, 65536, iniFile);
            List<string> result = new List<string>(returnString.Split('\0'));
            result.RemoveRange(result.Count - 2, 2);
            return result;
        }

        public static List<string> GetKeys(string category, string iniFile)
        {
            string returnString = new string(' ', 32768);
            GetPrivateProfileString(category, null, null, returnString, 32768, iniFile);
            List<string> result = new List<string>(returnString.Split('\0'));
            result.RemoveRange(result.Count - 2, 2);
            return result;
        }

        public static string GetIniFileString(string category, string key, string defaultValue, string iniFile)
        {
            string returnString = new string(' ', 1024);
            GetPrivateProfileString(category, key, defaultValue, returnString, 1024, iniFile);
            return returnString.Split('\0')[0];
        }

        public static bool WriteIniFileString(string category, string key, string value, string iniFile)
        {
            return (WritePrivateProfileString(category, key, value, iniFile) != 0);
        }
    }
}
