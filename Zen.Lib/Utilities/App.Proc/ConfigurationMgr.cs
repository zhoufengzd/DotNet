using System.IO;
using Zen.Utilities.FileUtil;
using System.Collections.Generic;
using System;

namespace Zen.Utilities.Proc
{
    /// <summary>
    /// SysConfig   ==> "SysConfig"
    /// UserProfile ==> "Profile"
    ///    |___ History: MRU, entries
    ///    Use flat naming schema (no sub folders under "SysConfig" and "Profile"
    /// </summary>
    public sealed class ConfigurationMgr
    {
        const string SysConfig = "SysConfig";
        const string UserProfile = "Profile";
        const string XmlExt = ".xml";

        public ConfigurationMgr()
            :this(SysConfig, UserProfile)
        {
        }
        public ConfigurationMgr(string sysConfig, string userProfile)
        {
            _env = new ProcEnvironInfo();

            _sysDirectory = Path.Combine(_env.WorkingDir, sysConfig);
            Directory.CreateDirectory(_sysDirectory);
            _userDirectory = Path.Combine(_env.WorkingDir, userProfile);
            Directory.CreateDirectory(_userDirectory);

            _settingHistory = new Dictionary<string, string>();
        }

        public void SaveProfile<SettingT>(SettingT settings)
        {
            SaveProfile<SettingT>(settings, typeof(SettingT).FullName + XmlExt);
        }
        public void SaveProfile<SettingT>(SettingT settings, string objName)
        {
            ObjSerializer.Save(GetPath(_userDirectory, objName), settings);
        }
        public void SaveSysConfig<SettingT>(SettingT settings)
        {
            SaveSysConfig<SettingT>(settings, typeof(SettingT).FullName + XmlExt);
        }
        public void SaveSysConfig<SettingT>(SettingT settings, string objName)
        {
            ObjSerializer.Save(GetPath(_sysDirectory, objName), settings);
        }

        public bool LoadProfile<SettingT>(out SettingT obj) where SettingT : new()
        {
            return LoadProfile<SettingT>(typeof(SettingT).FullName + XmlExt, out obj);
        }
        public bool LoadProfile<SettingT>(string objName, out SettingT obj) where SettingT : new()
        {
            string filePath = GetPath(_userDirectory, objName);
            return DoLoad<SettingT>(filePath, out obj);
        }
        public bool LoadSysConfig<SettingT>(out SettingT obj) where SettingT : new()
        {
            return LoadSysConfig<SettingT>(typeof(SettingT).FullName + XmlExt, out obj);
        }
        public bool LoadSysConfig<SettingT>(string objName, out SettingT obj) where SettingT : new()
        {
            string filePath = GetPath(_sysDirectory, objName);
            return DoLoad<SettingT>(filePath, out obj);
        }

        #region private functions
        private string GetPath(string baseDir, string objName)
        {
            return Path.Combine(baseDir, objName + ".xml");
        }

        private bool DoLoad<SettingT>(string filePath, out SettingT obj) where SettingT : new()
        {
            if (FileValidator.IsValid(filePath, 1))
            {
                try
                {
                    obj = ObjSerializer.Load<SettingT>(filePath);
                    return true;
                }
                catch
                {                    
                }
            }

            obj = new SettingT();
            return false;
        }
        #endregion

        #region private data
        private string _sysDirectory;
        private string _userDirectory;
        private ProcEnvironInfo _env;
        private Dictionary<string, string> _settingHistory;
        #endregion
    }
}
