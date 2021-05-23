using System;
using System.Diagnostics;

namespace Zen.Utilities.Generics
{
    using XmlIgnore = System.Xml.Serialization.XmlIgnoreAttribute;

    /// <summary>
    /// Identifier for complicated / layered object
    /// </summary>
    public class Identifier
    {
        public Identifier()
            :this(null, -1, Guid.Empty)
        {
        }
        public Identifier(string name, int id, Guid globalId)
        {
            _name = name;
            _internalId = id;
            _globalId = globalId;
        }

        /// <summary>
        /// Dot ('.') is not allowed in name characters since it's reserved as name separator
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; Debug.Assert(_name.IndexOf('.') == -1); }
        }

        public int Id
        {
            get { return _internalId; }
            set { _internalId = value; }
        }
        public Guid GlobalId
        {
            get { return _globalId; }
            set { _globalId = value; }
        }

        #region private data
        protected string _name;
        protected int _internalId;
        protected Guid _globalId;
        #endregion
    }

    /// <summary>
    /// Identifier for complicated / layered object
    /// </summary>
    public sealed class ObjIdentifier : Identifier
    {
        const int MaxPartCount = 10;

        public ObjIdentifier()
            : this(null, null)
        {
        }
        public ObjIdentifier(string objName, ObjIdentifier parentId)
        {
            _name = objName;
            _parent = parentId;
        }

        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        [XmlIgnore]
        public string FullName
        {
            get { return (_parent == null) ? _name : _parent.FullName + "." + _name; }
        }

        public ObjIdentifier Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        #region private data
        private string _displayName;
        private ObjIdentifier _parent;
        #endregion
    }

}
