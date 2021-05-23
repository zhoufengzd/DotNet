using System;
using System.Collections.Generic;
using System.Reflection;

namespace Zen.Utilities.Proc
{
    /// <summary>
    /// Contains what normally shows up in the 'About' dialog
    /// </summary>
    public sealed class AssemblyInfo
    {
        public AssemblyInfo()
        {
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string CodeBase
        {
            get { return _codeBase; }
            set { _codeBase = value; }
        }
        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        public string Configuration
        {
            get { return _configuration; }
            set { _configuration = value; }
        }
        public string Company
        {
            get { return _company; }
            set { _company = value; }
        }
        public string Product
        {
            get { return _product; }
            set { _product = value; }
        }
        public string Copyright
        {
            get { return _copyright; }
            set { _copyright = value; }
        }
        public string Trademark
        {
            get { return _trademark; }
            set { _trademark = value; }
        }
        public string Culture
        {
            get { return _culture; }
            set { _culture = value; }
        }

        #region private data
        private string _name;
        private string _codeBase;
        private string _version;
        private string _title;
        private string _description;
        private string _configuration;
        private string _company;
        private string _product;
        private string _copyright;
        private string _trademark;
        private string _culture;
        #endregion
    }

    /// <summary>
    /// This class uses the System.Reflection.Assembly class to
    /// access assembly meta-data
    /// </summary>
    public sealed class AssemblyQueryer
    {
        public AssemblyQueryer(Assembly assembly)
        {
            _assembly = (assembly != null)? assembly:Assembly.GetCallingAssembly();
        }

        public AssemblyInfo AssemblyInfo
        {
            get
            {
                FetchData();
                return _assemblyInfo;
            }
        }

        #region private functions
        private void FetchData()
        {
            if (_assemblyInfo != null)
                return;

            _assemblyInfo = new AssemblyInfo();

            AssemblyName asmName = _assembly.GetName();
            _assemblyInfo.Name = asmName.Name;
            _assemblyInfo.CodeBase = asmName.CodeBase;
            _assemblyInfo.Version = asmName.Version.ToString();

            _assemblyInfo.Copyright = GetCustomAttributeString(typeof(AssemblyCopyrightAttribute));
            _assemblyInfo.Company = GetCustomAttributeString(typeof(AssemblyCompanyAttribute));
            _assemblyInfo.Description = GetCustomAttributeString(typeof(AssemblyDescriptionAttribute));

            _assemblyInfo.Product = GetCustomAttributeString(typeof(AssemblyProductAttribute));
            _assemblyInfo.Title = GetCustomAttributeString(typeof(AssemblyTitleAttribute));
        }

        private string GetCustomAttributeString(Type attributeType)
        {
            // Get assembly attributes
            object[] attributes = _assembly.GetCustomAttributes(attributeType, false);
            if (attributes == null || attributes.Length < 1)
                return null;

            // Get the associated string property
            PropertyInfo[] properties = attributeType.GetProperties();
            if (properties.Length < 1)
                return null;

            // Get the attribute value
            object value = properties[0].GetValue(attributes[0], null);
            return (value != null && (value is System.String)) ? value.ToString() : null;
        }
        #endregion

        #region private data
        private Assembly _assembly;
        private AssemblyInfo _assemblyInfo;
        #endregion
    }

}
