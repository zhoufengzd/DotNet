using Zen.DBMS.Schema;
using System.Collections.Generic;
using Zen.Utilities.Generics;

namespace Zen.DBMS
{
    /// <summary>
    /// Connection KeyWord
    /// </summary>
    public sealed class ConnKWD
    {
        public const string DataSource = "Data Source";
        public const string Server = "Server";
        public const string Database = "Database";
        public const string InitialCatalog = "Initial Catalog";
        public const string UserId = "User ID";
        public const string UID = "UID";
        public const string Password = "Password";
        public const string PWD = "PWD";
        public const string Port = "Port";
    }

    /// <summary>
    /// Main purpose: 
    ///   Exchange info as:
    ///     DBLoginInfo <==> DB Login Dialog
    ///   Serialization  
    ///   Each DBMS maintains its own style
    /// </summary>
    public abstract class DBLoginInfo
    {
        [System.ComponentModel.Browsable(false)]
        public abstract string DataSource { get; set; }

        [System.Xml.Serialization.XmlIgnore, System.ComponentModel.Browsable(false)]
        public Dictionary<string, string> ConnProperties
        {
            get { return _connProperties; }
            set { _connProperties = value; }
        }

        [System.ComponentModel.Browsable(false)]
        public string ConnectionString
        {
            get 
            {
                AdjustConnProperties();
                return DictionaryAdapter.ToString(_connProperties); 
            }
            set
            {
                Dictionary<string, string> rawDictionary = DictionaryAdapter.ToDictionary(value);
                _connProperties.Clear();
                if (rawDictionary == null)
                    return;

                LoadAliasMap();
                foreach (KeyValuePair<string, string> kv in rawDictionary)
                    _connProperties.Add(MapKey(kv.Key), kv.Value);
            }
        }

        #region protected functions
        protected virtual void LoadAliasMap() { }
        /// <summary>
        /// Allow platform dependent login to add / adjust connection properties 
        ///   before construct connection string
        /// </summary>
        protected virtual void AdjustConnProperties() { }
        #endregion

        #region private functions
        private string MapKey(string key)
        {
            string keyMapped = null;
            if (_aliasMap == null || !_aliasMap.TryGetValue(key, out keyMapped))
                return key;

            return keyMapped;
        }
        #endregion

        #region protected data
        protected Dictionary<string, string> _connProperties = new Dictionary<string, string>();
        protected Dictionary<string, string> _aliasMap = null;
        #endregion
    }
}
