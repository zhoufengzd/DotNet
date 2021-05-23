using System.IO;
using System.Reflection;
using Zen.Utilities.Generics;

namespace Zen.Utilities.Proc
{

    public sealed class ResourceLoader
    {
        public ResourceLoader()
            : this(Assembly.GetCallingAssembly())
        {
        }

        /// <summary>
        /// Assembly that contains the requested resources
        /// </summary>
        /// <param name="resAssembly"></param>
        public ResourceLoader(Assembly resAssembly)
        {
            _resourceAssembly = resAssembly;
        }

        /// <summary>
        /// Requires either fully qualified name or just a short name
        /// </summary>
        public Stream LoadResource(string resourceName)
        {
            LoadControllers();

            string fullResName = _resourceNames.FindT1(resourceName);
            if (fullResName == null)
                fullResName = _resourceNames.FindT2(resourceName);

            if (fullResName == null)
                return null;

            return _resourceAssembly.GetManifestResourceStream(fullResName);
        }

        private void LoadControllers()
        {
            if (_resourceNames != null)
                return;

            string[] resNames = _resourceAssembly.GetManifestResourceNames();
            _resourceNames = new TwowayMap<string, string>();
            int assemblyFullNameLength = (Path.GetFileNameWithoutExtension(_resourceAssembly.ManifestModule.Name)).Length;
            foreach (string res in resNames)
            {
                _resourceNames.Add(res.Substring(assemblyFullNameLength), res);
            }
        }

        private TwowayMap<string, string> _resourceNames;
        private Assembly _resourceAssembly;
    }
}
