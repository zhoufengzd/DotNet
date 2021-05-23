using System.Collections.Generic;
using System.IO;

namespace Zen.Utilities.FileUtil
{
    /// <summary>
    /// Possible usage: DBA toolbox? automated testing?
    /// </summary>

    public sealed class ScriptInfo
    {
        public ScriptInfo()
        {
        }
        public ScriptInfo(string name, int id, short optionEnum, string parameters)
        {
            _name = name;
            _id = id;
            _optionEnum = optionEnum;
            _parameters = parameters;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public short OptionEnum
        {
            get { return _optionEnum; }
            set { _optionEnum = value; }
        }

        public string Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        #region private data
        private string _name;
        private int _id;
        private string _parameters;
        private short _optionEnum;
        #endregion
    }

    /// <summary>
    /// To load scripts from disk
    /// </summary>
    public sealed class ScriptLoader
    {
        public ScriptLoader(List<ScriptInfo> scriptList)
        {
            _scriptList = scriptList;
        }

        #region private functions
        /// <summary>
        /// Return embedded script resource by script file name
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns>sql script</returns>
        private void LoadScript()
        {
            _scripts = new Dictionary<string, string>(_scriptList.Count);
            foreach (ScriptInfo si in _scriptList)
            {
                StreamReader reader = new StreamReader(si.Name);
                _scripts.Add(si.Name, reader.ReadToEnd());
            }
        }
        #endregion

        #region private data
        private List<ScriptInfo> _scriptList;
        private Dictionary<string, string> _scripts;
        #endregion
    }
}
