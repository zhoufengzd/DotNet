using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.IO;

namespace Zen.Search.Util
{
    class ClientInit
    {
        const string DefaultConfigFile = @"Zen.Search.Util.dll.config";

        public static void Initialize()
        {
            lock (SyncRoot)
            {
                if (_Initialized)
                    return;

                ConfigureRemoting();
            }
        }

        private static void ConfigureRemoting()
        {
            try
            {
                if (File.Exists(DefaultConfigFile))
                    RemotingConfiguration.Configure(DefaultConfigFile, false);

                _Initialized = true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static object SyncRoot = new object();
        private static bool _Initialized;
    }
}
