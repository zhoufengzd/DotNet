
namespace Zen.Utilities.Proc
{
    /// <summary>
    /// 1: Application / AppInstance
    ///   1: ConfigurationMgr
    /// </summary>
    public class AppInstanceBase
    {
        public AppInstanceBase() 
        { 
        }

        public ConfigurationMgr ConfigManager
        {
            get { return _configMgr; }
        }

        #region private functions
        #endregion

        #region private data
        protected ConfigurationMgr _configMgr = new ConfigurationMgr();
        #endregion
    }
}
