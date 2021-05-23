using System;
using System.Collections.Generic;

using Zen.DBMS;
using Zen.DBMS.Schema;
using Zen.Utilities.Generics;
using Zen.Utilities.Proc;

namespace Zen.DBAToolbox
{
    public sealed class AppInstance : AppInstanceBase
    {
        const string SvrConnListFile = "server.connection";
        const string DatasourceListFile = "datasource";

        public AppInstance()
        {
            LoadSvrLists();
        }
        ~AppInstance()
        {
            SaveSvrLists();
        }

        public DBMSContext GetDBMSContext(DBMSPlatformEnum dbms)
        {
            return _contexts[dbms];
        }

        public List<string> GetDataSources(DBMSPlatformEnum dbms)
        {
            if (_svrLists == null)
                _svrLists = new Dictionary<DBMSPlatformEnum, List<string>>();

            List<string> svrList = null;
            if (!_svrLists.TryGetValue(dbms, out svrList))
            {
                svrList = _contexts[dbms].GetDataSources();
                _svrLists.Add(dbms, svrList);
            }
            return svrList;
        }

        /// <summary>
        /// /platform -> serverName -> connection string
        /// </summary>
        public Dictionary<string, string> GetServerConnectionList(DBMSPlatformEnum dbms)
        {
            if (_svrConnectionList == null)
                _svrConnectionList = new Dictionary<DBMSPlatformEnum, Dictionary<string, string>>();

            return Macros.SafeGetNew(_svrConnectionList, dbms);
        }

        #region private functions
        private void LoadSvrLists()
        {
            _contexts = new Dictionary<DBMSPlatformEnum, DBMSContext>();
            _svrConnectionList = new Dictionary<DBMSPlatformEnum, Dictionary<string, string>>();

            Dictionary<string, string> servers = null;
            foreach (DBMSPlatformEnum dbEnum in Enum.GetValues(typeof(DBMSPlatformEnum)))
            {
                if (_configMgr.LoadSysConfig(ToSvrConnectionListFile(dbEnum), out servers))
                    _svrConnectionList.Add(dbEnum, servers);

                DBMSContext ctxt = DBMSFactory.CreateDBMSContext(dbEnum);
                if (ctxt != null)
                    _contexts.Add(dbEnum, ctxt);
            }
        }

        private void SaveSvrLists()
        {
            foreach (KeyValuePair<DBMSPlatformEnum, Dictionary<string, string>> kv in _svrConnectionList)
            {
                _configMgr.SaveSysConfig(kv.Value, ToSvrConnectionListFile(kv.Key));
            }
        }

        private string ToSvrConnectionListFile(DBMSPlatformEnum dbEnum)
        {
            return SvrConnListFile + "." + dbEnum.ToString();
        }
        private string ToDatasourceListFile(DBMSPlatformEnum dbEnum)
        {
            return DatasourceListFile + "." + dbEnum.ToString();
        }
        #endregion

        #region private data
        private Dictionary<DBMSPlatformEnum, DBMSContext> _contexts;
        private Dictionary<DBMSPlatformEnum, List<string>> _svrLists;
        private Dictionary<DBMSPlatformEnum, Dictionary<string, string>> _svrConnectionList;
        #endregion
    }
}