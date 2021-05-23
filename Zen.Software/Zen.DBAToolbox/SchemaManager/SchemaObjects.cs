using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;

using Zen.DBMS.Schema;
using Zen.DBMS.Security;

namespace Zen.DBAToolbox
{
    #region enums...
    [Flags]
    public enum PlatformEnum
    {
        NONE = 0x0,
        SQL2K = 0x1,
        SQL2K5 = 0x2,
        ALLSQL = SQL2K | SQL2K5,
        ALL = -1
    }

    public enum IndexTypeEnum
    {
        CLUSTERED = 1,
        NONCLUSTERED = 2,
    }

    public enum IndexCategoryEnum
    {
        NORMAL = 1,
        PRIMARYKEY = 2,
        UNIQUEKEY = 3
    }

    public enum ColumnDefaultValue
    {
        NULL = 0,
        CURRENT_DATE = 1,
        IDENTITY = 2
    }

    public enum CustomScriptRunTimeEnum
    {
        AFTER_UPDATE = 1,
        BEFORE_UPDATE = 2
    }

    public enum SchemaTypeEnum
    {
        UNKNOWN = 0,
        ULM,
        CASE,
    }
    #endregion	enums

    /// <summary>
    /// XML description for SchemaObjects.
    /// Base Object: SchemaObject
    /// </summary>

    public class SchemaPersistent : System.Attribute
    {
    }

    public enum SchemaObjectEventType
    {
        MemberDataChange = 0,
        CollectionMemberChange = 1
    }

    public class SchemaObjectEventArgs
    {
        public SchemaObjectEventArgs(SchemaObjectEventType eType, SchemaObjectEventArgs innerArgs)
        {
            this.eType = eType;
            this.innerArgs = innerArgs;
        }
        public SchemaObjectEventType eType;
        public SchemaObjectEventArgs innerArgs;

    }

    #region SchemaObject
    public abstract class SchemaObject
    {
        public SchemaObject()
        {
            _lOrigMajorVersion = g_lDefaultMajorVer;
            _lOrigMinorVersion = g_lDefaultMinorVer;
            _lLastMajorVersion = g_lDefaultMajorVer;
            _lLastMinorVersion = g_lDefaultMinorVer;
            _strIdentifier = String.Empty;
            _ePlatformBitmask = PlatformEnum.ALL;
            _strDescription = string.Empty;
            _bDirty = false;
        }

        #region Public Functions

        public virtual string DisplayName()
        {
            return _strIdentifier;
        }

        public XmlNode ToXML(XmlDocument doc)
        {
            System.Type myType = GetType();

            string strName = myType.Name;

            XmlElement elem = doc.CreateElement(strName);
            foreach (PropertyInfo info in myType.GetProperties())
            {
                System.Attribute attrib = Attribute.GetCustomAttribute(info, typeof(SchemaPersistent), true);
                if (attrib != null)
                {
                    Debug.Assert(info.CanRead, "Invalid SchemaPersistent property", String.Format("Property {0}.{1} is marked SchemaPersistent but is write-only", myType.Name, info.Name));
                    if (info.CanRead)
                    {
                        ParameterInfo[] parameters = info.GetIndexParameters();
                        object objResult = info.GetValue(this, parameters);
                        if (objResult != null)
                        {
                            IEnumerable enumerable = objResult as IEnumerable;
                            if (objResult.GetType() != typeof(System.String) && enumerable != null)
                            {
                                XmlNode collectionfragment = GetXmlForCollection(doc, info.Name, enumerable);
                                elem.AppendChild(collectionfragment);
                            }
                            else
                            {
                                System.Type valtype = objResult.GetType();
                                if (valtype != typeof(string) && valtype != typeof(string))
                                {
                                    elem.SetAttribute(info.Name, objResult.ToString());
                                }
                                else
                                {
                                    string strValue = objResult.ToString();
                                    if (strValue != String.Empty)
                                    {
                                        XmlNode propertyfrag = doc.CreateElement(info.Name);
                                        propertyfrag.InnerText = strValue;
                                        elem.AppendChild(propertyfrag);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return elem;
        }

        public void FromXml(XmlNode node)
        {
            System.Type myType = GetType();
            if (0 != node.Name.ToUpper().CompareTo(myType.Name.ToUpper()))
            {
                throw new System.Exception(String.Format("XML Node name ({0}) does not match object type ({1})", node.Name, myType.Name));
            }

            foreach (PropertyInfo info in myType.GetProperties())
            {
                System.Attribute attrib = Attribute.GetCustomAttribute(info, typeof(SchemaPersistent), true);
                if (attrib != null)
                {
                    ParameterInfo[] parameters = info.GetIndexParameters();
                    XmlAttribute xmlattrib = node.SelectSingleNode("@" + info.Name) as XmlAttribute;
                    if (xmlattrib != null)
                    {
                        if (info.CanWrite)
                        {
                            info.SetValue(this, DataConvert(info.PropertyType, xmlattrib.Value), parameters);
                        }
                    }

                    else
                    {
                        XmlNode val = node.SelectSingleNode(info.Name);

                        if (val != null)
                        {
                            if (info.CanWrite)
                            {
                                info.SetValue(this, DataConvert(info.PropertyType, val.InnerText), parameters);
                            }
                            else
                            {
                                SchemaObjectCollection arrValues = info.GetValue(this, parameters) as SchemaObjectCollection;
                                if (arrValues != null)
                                {
                                    XmlNodeList nodes = val.SelectNodes(arrValues.ContainedType.Name);
                                    if (nodes != null)
                                    {
                                        ConstructorInfo constructorInfo = arrValues.ContainedType.GetConstructor(new Type[0]);
                                        foreach (XmlNode childnode in nodes)
                                        {
                                            SchemaObject obj = constructorInfo.Invoke(null) as SchemaObject;
                                            obj.FromXml(childnode);

                                            arrValues.Add(obj);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            ClearChangeFlags();
        }


        public void Validate()
        {
            CheckInt(_lOrigMajorVersion, GetType().Name + ".FirstMajorVersion", 0, Int32.MaxValue);
            CheckInt(_lOrigMinorVersion, GetType().Name + ".FirstMinorVersion", 0, Int32.MaxValue);
            CheckInt(_lLastMajorVersion, GetType().Name + ".LastMajorVersion", 0, Int32.MaxValue);
            CheckInt(_lLastMinorVersion, GetType().Name + ".LastMinorVersion", 0, Int32.MaxValue);
            CheckString(_strIdentifier, GetType().Name + ".Identifier", false);

            ValidateMembers();
        }

        [BrowsableAttribute(false)]
        public bool IsDirty
        {
            get
            {
                return _bDirty;
            }
            set
            {
                _bDirty = value;
            }
        }

        [SchemaPersistent, CategoryAttribute("Version"), ReadOnlyAttribute(true)]
        public Int32 FirstMajorVersion
        {
            get
            {
                return _lOrigMajorVersion;
            }
            set
            {
                _lOrigMajorVersion = value;
                if (_lLastMajorVersion == 0)
                {
                    _lLastMajorVersion = value;
                }

            }
        }

        [SchemaPersistent, CategoryAttribute("Version"), ReadOnlyAttribute(true)]
        public Int32 FirstMinorVersion
        {
            get
            {
                return _lOrigMinorVersion;
            }
            set
            {
                _lOrigMinorVersion = value;
                if (_lLastMinorVersion == 0)
                {
                    _lLastMinorVersion = value;
                }

            }
        }

        [SchemaPersistent, CategoryAttribute("Version"), ReadOnlyAttribute(true)]
        public Int32 LastMajorVersion
        {
            get
            {
                return _lLastMajorVersion;
            }
            set
            {
                _lLastMajorVersion = value;

            }
        }

        [SchemaPersistent, CategoryAttribute("Version"), ReadOnlyAttribute(true)]
        public Int32 LastMinorVersion
        {
            get
            {
                return _lLastMinorVersion;
            }
            set
            {
                _lLastMinorVersion = value;

            }
        }

        [SchemaPersistent, CategoryAttribute("General")]
        public string Name
        {
            get
            {
                return _strIdentifier;
            }
        }

        [SchemaPersistent, CategoryAttribute("General")]
        public string Description
        {
            get
            {
                return _strDescription;
            }
            set
            {
                _strDescription = value;

            }
        }

        [SchemaPersistent, CategoryAttribute("General")]
        public PlatformEnum PlatformBitmask
        {
            get
            {
                return _ePlatformBitmask;
            }
            set
            {
                _ePlatformBitmask = value;

            }
        }

        public void ClearChangeFlags()
        {
            _bDirty = false;
        }

        static public Int32 DefaultMajorVersion
        {
            get
            {
                return g_lDefaultMajorVer;
            }
            set
            {
                if (value >= 0)
                {
                    g_lDefaultMajorVer = value;
                }
            }
        }

        static public Int32 DefaultMinorVersion
        {
            get
            {
                return g_lDefaultMinorVer;
            }
            set
            {
                if (value >= 0)
                {
                    g_lDefaultMinorVer = value;
                }
            }
        }
        #endregion

        #region Private Functions
        private XmlNode GetXmlForCollection(XmlDocument doc, string strObjName, IEnumerable objCollection)
        {
            XmlNode frag = doc.CreateElement(strObjName);
            foreach (object obj in objCollection)
            {
                SchemaObject schemaobj = obj as SchemaObject;
                Debug.Assert(schemaobj != null, "Collection object was not a schema object", "Object in collection " + strObjName + " did not derive from SchemaObject");
                if (schemaobj != null)
                {
                    XmlNode subfrag = schemaobj.ToXML(doc);
                    frag.AppendChild(subfrag);
                }
            }

            return frag;
        }

        protected void CheckList(IEnumerable objCollection, string strNameDescriptor)
        {
            if (objCollection != null)
            {
                foreach (object obj in objCollection)
                {
                    SchemaObject schemaobj = obj as SchemaObject;
                    Debug.Assert(schemaobj != null, "Collection object was not a schema object", "Object in collection " + strNameDescriptor + " did not derive from SchemaObject");
                    if (schemaobj == null)
                    {
                        throw new System.Exception(String.Format("Object in collection {0} was not derived from SchemaObject", strNameDescriptor));
                    }

                    schemaobj.Validate();
                }
            }
        }

        protected void CheckInt(Int32 lValue, string strNameDescriptor, Int32 lMinVal, Int32 lMaxVal)
        {
            if (lValue < lMinVal)
            {
                throw new System.Exception(String.Format("Value for {0} ({1}) was lower than minimum value ({2})", strNameDescriptor, lValue, lMinVal));
            }

            if (lValue > lMaxVal)
            {
                throw new System.Exception(String.Format("Value for {0} ({1}) was higher than maximum value ({2})", strNameDescriptor, lValue, lMaxVal));
            }
        }

        protected void CheckString(string strValue, string strNameDescriptor, bool bEmptyOK)
        {
            if (strValue == null)
            {
                throw new System.Exception(String.Format("Value for string {0} was null", strNameDescriptor));
            }

            if (!bEmptyOK && strValue == string.Empty)
            {
                throw new System.Exception(String.Format("Value for string {0} was empty", strNameDescriptor));
            }

        }

        private static object DataConvert(Type type, string strValue)
        {
            if (type == typeof(System.String))
            {
                return strValue;
            }
            else if (type == typeof(System.Int32))
            {
                return Int32.Parse(strValue);
            }
            else if (type.IsEnum)
            {
                return System.Enum.Parse(type, strValue, true);
            }
            else if (type == typeof(System.Boolean))
            {
                return System.Boolean.Parse(strValue);
            }
            else if (type == typeof(Guid))
            {
                return new Guid(strValue);
            }
            else
            {
                return null;
            }
        }

        public event DataChangedEventHandler DataChanged;
        public delegate void DataChangedEventHandler(object sender, SchemaObjectEventArgs e);

        protected virtual void OnDataChanged(SchemaObjectEventArgs e)
        {
            _bDirty = true;

            if (SchemaVersionToInt(_lLastMajorVersion, _lLastMinorVersion) < DefaultSchemaVersionToInt())
            {
                _lLastMajorVersion = SchemaObject.DefaultMajorVersion;
                _lLastMinorVersion = SchemaObject.DefaultMinorVersion;
            }

            if (DataChanged != null)
            {
                DataChanged(this, e);
            }
        }

        protected virtual void OnDataChanged()
        {
            OnDataChanged(new SchemaObjectEventArgs(SchemaObjectEventType.MemberDataChange, null));
        }
        protected void CollectionChanged(object sender, SchemaObjectEventArgs e)
        {
            OnDataChanged(new SchemaObjectEventArgs(SchemaObjectEventType.CollectionMemberChange, e));
        }

        protected void AttachToCollection(SchemaObjectCollection coll)
        {
            coll.DataChanged += new SchemaObjectCollection.DataChangedEventHandler(CollectionChanged);
        }

        protected abstract void ValidateMembers();

        protected static Int32 DefaultSchemaVersionToInt()
        {
            return SchemaVersionToInt(g_lDefaultMajorVer, g_lDefaultMinorVer);
        }

        protected static Int32 SchemaVersionToInt(Int32 iMajorVer, Int32 iMinorVer)
        {
            return iMajorVer * 10000 + iMinorVer;
        }

        #endregion

        #region Protected Variables
        /// <summary>
        /// Current / default major version number
        /// </summary>
        protected static Int32 g_lDefaultMajorVer = 1;
        protected static Int32 g_lDefaultMinorVer = 0;
        protected Int32 _lOrigMajorVersion;
        protected Int32 _lOrigMinorVersion;
        protected Int32 _lLastMajorVersion;
        protected Int32 _lLastMinorVersion;
        protected string _strIdentifier;
        protected PlatformEnum _ePlatformBitmask;
        protected bool _bDirty;
        protected string _strDescription;
        #endregion
    }

    #endregion

    public class SchemaObjectCollection : ICollection, IList, IEnumerable, ICustomTypeDescriptor
    {
        public SchemaObjectCollection(System.Type ContainedType)
        {
            InnerList = new ArrayList();
            Debug.Assert(ContainedType != null, "Contained Type cannot be NULL");
            _ContainedType = ContainedType;
            _tblDataChangedEventHandler = new Hashtable();
        }


        #region IList
        public bool Contains(object obj)
        {
            return InnerList.Contains(obj);
        }

        public void Clear()
        {
            foreach (object obj in InnerList)
            {
                DetachFromSchemaObject(obj);
            }

            InnerList.Clear();

        }

        public void Insert(int iIndex, object obj)
        {
            CheckSchemaObject(obj);
            InnerList.Insert(iIndex, obj);
            AttachToSchemaObject(obj);

        }

        public int IndexOf(object obj)
        {
            return InnerList.IndexOf(obj);
        }

        public void RemoveAt(int iIndex)
        {
            throw new System.Exception("Remove object not allowed for now");
            /*
                        object obj = this[iIndex];

                        InnerList.Remove( iIndex );
                        DetachFromSchemaObject( obj );

			
            */
        }

        public int Add(object obj)
        {
            lock (InnerList.SyncRoot)
            {
                CheckSchemaObject(obj);
                int iLocation = InnerList.Add(obj);
                AttachToSchemaObject(obj);

                return iLocation;
            }
        }

        public void Remove(object obj)
        {
            throw new System.Exception("Remove object not allowed for now");
            /*
                        lock( InnerList.SyncRoot )
                        {
                            CheckSchemaObject( obj );
                            this.InnerList.Remove( obj );
                            DetachFromSchemaObject( obj );
				
                        }
            */
        }

        public object this[int iIndex]
        {
            get
            {
                lock (InnerList.SyncRoot)
                {
                    return this.InnerList[iIndex];
                }
            }
            set
            {
                lock (InnerList.SyncRoot)
                {
                    CheckSchemaObject(value);
                    InnerList[iIndex] = value;
                    AttachToSchemaObject(value);

                }
            }
        }

        public bool IsFixedSize
        {
            get
            {
                return InnerList.IsFixedSize;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return InnerList.IsReadOnly;
            }
        }
        #endregion


        #region IEnumerator

        public IEnumerator GetEnumerator()
        {
            return InnerList.GetEnumerator();
        }

        #endregion
        #region ICollection
        public int Count
        {
            get
            {
                return InnerList.Count;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return InnerList.IsSynchronized;
            }
        }

        public object SyncRoot
        {
            get
            {
                return InnerList.SyncRoot;
            }
        }

        public void CopyTo(Array arrTarget, int iIndex)
        {
            InnerList.CopyTo(arrTarget, iIndex);
        }

        #endregion

        public event DataChangedEventHandler DataChanged;
        public delegate void DataChangedEventHandler(object sender, SchemaObjectEventArgs e);

        protected virtual void OnDataChanged(SchemaObjectEventArgs e)
        {
            if (DataChanged != null)
            {
                DataChanged(this, e);
            }
        }

        protected void OnDataChanged()
        {
            OnDataChanged(new SchemaObjectEventArgs(SchemaObjectEventType.CollectionMemberChange, null));
        }

        protected virtual void ContainedObjectChanged(object sender, SchemaObjectEventArgs e)
        {
            OnDataChanged(new SchemaObjectEventArgs(SchemaObjectEventType.CollectionMemberChange, e));
        }

        protected void CheckSchemaObject(object obj)
        {
            if (obj == null)
            {
                throw new System.Exception("SchemaObjectCollection cannot contain null objects");
            }

            SchemaObject schemaobj = obj as SchemaObject;
            if (schemaobj == null)
            {
                throw new System.Exception("Attempted to add an object that was not a SchemaObject");
            }

            if (obj.GetType() != _ContainedType)
            {
                throw new System.Exception(String.Format("Tried to add a {0} to a collection of {1}", schemaobj.GetType().Name, _ContainedType.Name));
            }
        }

        protected void AttachToSchemaObject(object obj)
        {
            SchemaObject schemaobj = (SchemaObject)obj;
            SchemaObject.DataChangedEventHandler handler = new SchemaObject.DataChangedEventHandler(ContainedObjectChanged);
            schemaobj.DataChanged += handler;
            _tblDataChangedEventHandler[obj] = handler;
        }

        protected void DetachFromSchemaObject(object obj)
        {
            SchemaObject schemaobj = (SchemaObject)obj;
            schemaobj.DataChanged -= (SchemaObject.DataChangedEventHandler)_tblDataChangedEventHandler[obj];
        }

        #region ICustomTypeDescriptor
        public string GetClassName()
        {
            return _ContainedType.Name + "CollectionClass";
        }

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public string GetComponentName()
        {
            return _ContainedType.Name + "CollectionComponent";
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attribs)
        {
            return TypeDescriptor.GetEvents(this, attribs, true);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties(this, attributes, true);
        }

        public PropertyDescriptorCollection GetProperties()
        {
            return TypeDescriptor.GetProperties(this, true);
        }
        #endregion

        public System.Type ContainedType
        {
            get
            {
                return _ContainedType;
            }
        }
        private System.Type _ContainedType;
        ArrayList InnerList;
        Hashtable _tblDataChangedEventHandler;
    }

    public class Column : SchemaObject
    {
        // TODO:
        //		Scale/Precision (reals)

        public Column()
        {
            _bNullable = false;
            _eDataType = DataTypeEnum.Unspecified;
            _lStringLength = 0;
            _eDefaultValue = ColumnDefaultValue.NULL;
            _strExplicitDefaultValue = string.Empty;
        }

        [SchemaPersistent]
        public bool IsNullable
        {
            get
            {
                return _bNullable;
            }
            set
            {
                _bNullable = value;

            }
        }

        [SchemaPersistent]
        public bool IsFullTextIndexed
        {
            get
            {
                return _bFullTextIndexed;
            }
            set
            {
                _bFullTextIndexed = value;

            }
        }

        [SchemaPersistent]
        public string ColumnName
        {
            get
            {
                return _strIdentifier;
            }
            set
            {
                _strIdentifier = value;

            }
        }

        [SchemaPersistent]
        public DataTypeEnum DataType
        {
            get
            {
                return _eDataType;
            }
            set
            {
                _eDataType = value;

            }
        }

        [SchemaPersistent]
        public Int32 MaxStringLength
        {
            get
            {
                return _lStringLength;
            }
            set
            {
                _lStringLength = value;

            }
        }

        [SchemaPersistent]
        public ColumnDefaultValue DefaultValue
        {
            get
            {
                return _eDefaultValue;
            }
            set
            {
                _eDefaultValue = value;

            }
        }

        [SchemaPersistent]
        public string ExplicitDefaultValue
        {
            get
            {
                return _strExplicitDefaultValue;
            }
            set
            {
                _strExplicitDefaultValue = value;

            }
        }

        protected override void ValidateMembers()
        {
            CheckInt((Int32)_eDataType, GetType().Name + ".DataType", 1, (Int32)DataTypeEnum.MAX_VALUE - 1);
            CheckInt(_lStringLength, GetType().Name + ".MaxStringLength", 0, Int32.MaxValue);
            switch (_eDataType)
            {
                case DataTypeEnum.AnsiString:
                case DataTypeEnum.WideString:
                    CheckInt(_lStringLength, GetType().Name + ".MaxStringLength", 1, Int32.MaxValue);
                    break;
                default:
                    break;
            }
        }

        private DataTypeEnum _eDataType;
        private Int32 _lStringLength;
        private bool _bNullable;
        private bool _bFullTextIndexed;
        private ColumnDefaultValue _eDefaultValue;
        private string _strExplicitDefaultValue;
    }

    public class TableFieldValue : SchemaObject
    {
        public TableFieldValue()
        {
            _strFieldName = string.Empty;
            _strFieldValue = string.Empty;
            _bIsNull = false;
        }

        [SchemaPersistent]
        public new string Name
        {
            get
            {
                return _strFieldName;
            }
            set
            {
                _strFieldName = value;
                _strIdentifier = value;
                UpdateIdentifier();

            }
        }

        [SchemaPersistent]
        public bool IsNull
        {
            get
            {
                return _bIsNull;
            }
            set
            {
                _bIsNull = value;

            }
        }

        [SchemaPersistent]
        public string FieldValue
        {
            get
            {
                return _strFieldValue;
            }
            set
            {
                _strFieldValue = value;
                UpdateIdentifier();

            }
        }

        protected override void ValidateMembers()
        {
            CheckString(_strFieldName, GetType().Name + ".FieldName", false);
            if (!_bIsNull)
            {
                CheckString(_strFieldValue, GetType().Name + ".FieldValue", true);
            }
        }

        protected void UpdateIdentifier()
        {
            if (!_bIsNull)
            {
                _strIdentifier = string.Format("{0}: {1}", _strFieldName, _strFieldValue);
            }
            else
            {
                _strIdentifier = string.Format("{0}: <NULL>", _strFieldName);
            }
        }

        private string _strFieldName;
        private string _strFieldValue;
        private bool _bIsNull;
    };

    public class TableFieldValueCollection : SchemaObjectCollection
    {
        public TableFieldValueCollection()
            : base(typeof(TableFieldValue))
        { }

        public void Add(TableFieldValue val)
        {
            base.Add(val);
        }

        public void Remove(TableFieldValue val)
        {
            base.Remove(val);
        }

        public new TableFieldValue this[int iIndex]
        {
            get
            {
                return (TableFieldValue)base[iIndex];
            }
        }
    }

    public class TableRow : SchemaObject
    {
        public TableRow()
        {
            _arrFieldValues = new TableFieldValueCollection();
            AttachToCollection(_arrFieldValues);
        }

        // arraylist of column name/column value pairs
        protected override void ValidateMembers()
        {

        }

        [SchemaPersistent]
        public TableFieldValueCollection FieldValues
        {
            get
            {
                return _arrFieldValues;
            }
        }

        [SchemaPersistent]
        public string RowName
        {
            get
            {
                return _strIdentifier;
            }
            set
            {
                _strIdentifier = value;

            }
        }

        private TableFieldValueCollection _arrFieldValues;
    }

    public class TableRowCollection : SchemaObjectCollection
    {
        public TableRowCollection()
            : base(typeof(TableRow))
        { }

        public void Add(TableRow data)
        {
            base.Add(data);
        }

        public void Remove(TableRow data)
        {
            base.Remove(data);
        }

        public new TableRow this[int iIndex]
        {
            get
            {
                return (TableRow)base[iIndex];
            }
        }
    }

    public class ColumnCollection : SchemaObjectCollection
    {
        public ColumnCollection()
            : base(typeof(Column))
        { }

        public void Add(Column col)
        {
            base.Add(col);
        }

        public void Remove(Column col)
        {
            base.Remove(col);
        }

        public new Column this[int iIndex]
        {
            get
            {
                return (Column)base[iIndex];
            }
        }
    }

    public class SQLRoleCollection : SchemaObjectCollection
    {
        public SQLRoleCollection()
            : base(typeof(SQLRole))
        { }

        public void Add(SQLRole role)
        {
            base.Add(role);
        }

        public void Remove(SQLRole role)
        {
            base.Remove(role);
        }

        public new SQLRole this[int iIndex]
        {
            get
            {
                return (SQLRole)base[iIndex];
            }
        }
    }

    public class SQLRoleAssignmentCollection : SchemaObjectCollection
    {
        public SQLRoleAssignmentCollection()
            : base(typeof(SQLRoleAssignment))
        { }

        public void Add(SQLRoleAssignment role)
        {
            base.Add(role);
        }

        public void Remove(SQLRoleAssignment role)
        {
            base.Remove(role);
        }

        public new SQLRoleAssignment this[int iIndex]
        {
            get
            {
                return (SQLRoleAssignment)base[iIndex];
            }
        }
    }

    public class SchemaString : SchemaObject
    {
        public SchemaString()
        {
            _Value = string.Empty;
        }

        [SchemaPersistent]
        public string Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;

            }
        }

        protected override void ValidateMembers()
        {
            CheckString(_Value, GetType().Name + ".Value", true);
        }
        private string _Value;
    }

    public class StringCollection : SchemaObjectCollection
    {
        public StringCollection()
            : base(typeof(SchemaString))
        {

        }

        public void Add(SchemaString strValue)
        {
            base.Add(strValue);
        }

        public void Remove(SchemaString strValue)
        {
            base.Remove(strValue);
        }

        public new SchemaString this[int iIndex]
        {
            get
            {
                return (SchemaString)base[iIndex];
            }
        }
    }

    public class IndexColumn : SchemaObject
    {
        public IndexColumn()
        {
            _strColumnName = string.Empty;
            _eOrder = SortingOrderEnum.ASCENDING;
        }

        [SchemaPersistent]
        public string ColumnName
        {
            get
            {
                return _strIdentifier;
            }
            set
            {
                _strIdentifier = value;
            }
        }

        [SchemaPersistent]
        public SortingOrderEnum Direction
        {
            get
            {
                return _eOrder;
            }
            set
            {
                _eOrder = value;
            }
        }

        protected override void ValidateMembers()
        {
            CheckString(_strIdentifier, GetType().Name + ".ColumnName", false);
        }

        private string _strColumnName;
        private SortingOrderEnum _eOrder;
    }

    public class IndexColumnCollection : SchemaObjectCollection
    {
        public IndexColumnCollection()
            : base(typeof(IndexColumn))
        {
        }

        public void Add(IndexColumn col)
        {
            base.Add(col);
        }

        public void Remove(IndexColumn col)
        {
            base.Remove(col);
        }

        public new IndexColumn this[int iIndex]
        {
            get
            {
                return (IndexColumn)base[iIndex];
            }
        }
    }

    public class IndexDef : SchemaObject
    {
        public IndexDef()
        {
            _eIndexType = IndexTypeEnum.NONCLUSTERED;
            _eCategory = IndexCategoryEnum.NORMAL;
            _ColumnNames = new IndexColumnCollection();
            AttachToCollection(_ColumnNames);
        }

        [SchemaPersistent]
        public IndexTypeEnum IndexType
        {
            get
            {
                return _eIndexType;
            }
            set
            {
                _eIndexType = value;

            }
        }

        [SchemaPersistent]
        public string IndexName
        {
            get
            {
                return _strIndexName;
            }
            set
            {
                _strIndexName = value;
                _strIdentifier = GetIdentifierFromIndexName(value);

            }
        }

        [SchemaPersistent]
        public IndexCategoryEnum Category
        {
            get
            {
                return _eCategory;
            }
            set
            {
                _eCategory = value;

            }
        }

        [SchemaPersistent]
        public IndexColumnCollection ColumnNames
        {
            get
            {
                return _ColumnNames;
            }
        }

        protected override void ValidateMembers()
        {
            CheckString(_strIndexName, GetType().Name + ".IndexName", false);
            CheckList(_ColumnNames, GetType().Name + ".ColumnNames");
        }

        protected virtual string GetIdentifierFromIndexName(string strValue)
        {
            return strValue;
        }

        private IndexTypeEnum _eIndexType;
        private IndexCategoryEnum _eCategory;
        private IndexColumnCollection _ColumnNames;
        private string _strIndexName;

    }

    public class IndexDefCollection : SchemaObjectCollection
    {
        public IndexDefCollection()
            : base(typeof(IndexDef))
        {
        }

        void Add(IndexDef def)
        {
            base.Add(def);
        }

        void Remove(IndexDef def)
        {
            base.Remove(def);
        }

        public new IndexDef this[int iIndex]
        {
            get
            {
                return (IndexDef)base[iIndex];
            }
        }
    }


    public class ForeignKeyColumn : SchemaObject
    {
        public ForeignKeyColumn()
        {
            _strLocalColumn = string.Empty;
            _strForeignColumn = string.Empty;
        }

        [SchemaPersistent]
        public string LocalColumn
        {
            get
            {
                return _strLocalColumn;
            }
            set
            {
                _strLocalColumn = value;
            }
        }

        [SchemaPersistent]
        public string ForeignColumn
        {
            get
            {
                return _strForeignColumn;
            }
            set
            {
                _strForeignColumn = value;
            }
        }

        protected override void ValidateMembers()
        {
            CheckString(_strLocalColumn, GetType().Name + ".LocalColumn", false);
            CheckString(_strForeignColumn, GetType().Name + ".ForeignColumn", false);
        }

        private string _strLocalColumn;
        private string _strForeignColumn;

    }


    public class ForeignKeyColumnCollection : SchemaObjectCollection
    {
        public ForeignKeyColumnCollection()
            : base(typeof(ForeignKeyColumn))
        { }

        public void Add(ForeignKeyColumn col)
        {
            base.Add(col);
        }

        public void Remove(ForeignKeyColumn col)
        {
            base.Remove(col);
        }

        public new ForeignKeyColumn this[int iIndex]
        {
            get
            {
                return (ForeignKeyColumn)base[iIndex];
            }
        }
    }

    public abstract class ForeignKey : SchemaObject
    {
        public ForeignKey()
        {
            _Columns = new ForeignKeyColumnCollection();
            AttachToCollection(_Columns);

            _strLogicalForeignTable = string.Empty;
            _strPhysicalForeignTable = string.Empty;
            _strForeignKeyName = string.Empty;
        }

        [SchemaPersistent]
        public ForeignKeyColumnCollection Columns
        {
            get
            {
                return _Columns;
            }
        }

        [SchemaPersistent]
        public string ForeignTable
        {
            get
            {
                return _strLogicalForeignTable;
            }
            set
            {
                _strLogicalForeignTable = value;
                _strPhysicalForeignTable = GetPhysicalTableName(_strLogicalForeignTable);

            }
        }

        [SchemaPersistent]
        public string PhysicalForeignTable
        {
            get
            {
                return _strPhysicalForeignTable;
            }
        }

        [SchemaPersistent]
        public string ForeignKeyName
        {
            get
            {
                return _strForeignKeyName;
            }
            set
            {
                _strForeignKeyName = value;
                _strIdentifier = GetPhysicalForeignKeyName(_strForeignKeyName);

            }
        }

        protected override void ValidateMembers()
        {
            CheckString(_strLogicalForeignTable, GetType().Name + ".ForeignTable", false);
            CheckString(_strPhysicalForeignTable, GetType().Name + "PhysicalForeignTable", false);
            CheckString(_strForeignKeyName, GetType().Name + ".ForeignKeyName", false);
        }

        protected abstract string GetPhysicalTableName(string strLogicalTableName);
        protected abstract string GetPhysicalForeignKeyName(string strLogicalForeignKeyName);
        private ForeignKeyColumnCollection _Columns;
        private string _strLogicalForeignTable;
        private string _strPhysicalForeignTable;
        private string _strForeignKeyName;
    }

    public class GlobalForeignKey : ForeignKey
    {
        protected override string GetPhysicalTableName(string strLogicalTableName)
        {
            return GlobalObjectNames.PhysicalTableNameFromLogicalTableName(strLogicalTableName);
        }

        protected override string GetPhysicalForeignKeyName(string strLogicalForeignKeyName)
        {
            return GlobalObjectNames.PhysicalForeignKeyNameFromLogicalForeignKeyName(strLogicalForeignKeyName);
        }
    }

    public class CaseForeignKey : ForeignKey
    {
        protected override string GetPhysicalTableName(string strLogicalTableName)
        {
            return CaseObjectNames.PhysicalTableNameFromLogicalTableName(strLogicalTableName);
        }

        protected override string GetPhysicalForeignKeyName(string strLogicalForeignKeyName)
        {
            return CaseObjectNames.PhysicalForeignKeyNameFromLogicalForeignKeyName(strLogicalForeignKeyName);
        }
    }

    public class GlobalForeignKeyCollection : SchemaObjectCollection
    {
        public GlobalForeignKeyCollection()
            : base(typeof(GlobalForeignKey))
        { }

        public void Add(GlobalForeignKey key)
        {
            base.Add(key);
        }

        public void Remove(GlobalForeignKey key)
        {
            base.Remove(key);
        }

        public new GlobalForeignKey this[int iIndex]
        {
            get
            {
                return (GlobalForeignKey)base[iIndex];
            }
        }
    }

    public class CaseForeignKeyCollection : SchemaObjectCollection
    {
        public CaseForeignKeyCollection()
            : base(typeof(CaseForeignKey))
        { }

        public void Add(CaseForeignKey key)
        {
            base.Add(key);
        }

        public void Remove(CaseForeignKey key)
        {
            base.Remove(key);
        }

        public new CaseForeignKey this[int iIndex]
        {
            get
            {
                return (CaseForeignKey)base[iIndex];
            }
        }
    }

    public class GlobalObjectNames
    {
        public static string PhysicalTableNameFromLogicalTableName(string strLogicalTableName)
        {
            return "SLT_" + strLogicalTableName;
        }
        private GlobalObjectNames() { }

        public static string PhysicalForeignKeyNameFromLogicalForeignKeyName(string strLogicalForeignKeyName)
        {
            return strLogicalForeignKeyName;
        }
    }

    public class CaseObjectNames
    {
        public static string PhysicalTableNameFromLogicalTableName(string strLogicalTableName)
        {
            return "SLT_C[CASEID]_" + strLogicalTableName;
        }

        public static string PhysicalForeignKeyNameFromLogicalForeignKeyName(string strLogicalForeignKeyName)
        {
            return "C[CASEID]_" + strLogicalForeignKeyName;
        }

        private CaseObjectNames() { }
    }

    public abstract class TableDef : SchemaObject
    {
        // TODO:
        //		Foreign key references (in a separate location?)
        //		CHECK CONSTRAINTs

        public TableDef()
        {
            _Columns = new ColumnCollection();
            AttachToCollection(_Columns);
            _strTableName = String.Empty;
            _TableRows = new TableRowCollection();
            AttachToCollection(_TableRows);
            _arrRoles = new SQLRoleAssignmentCollection();
            AttachToCollection(_arrRoles);
            _Indexes = new IndexDefCollection();
            AttachToCollection(_Indexes);
            _GlobalForeignKeys = new GlobalForeignKeyCollection();
            AttachToCollection(_GlobalForeignKeys);
        }

        [SchemaPersistent]
        public GlobalForeignKeyCollection GlobalTableForeignKeys
        {
            get
            {
                return _GlobalForeignKeys;
            }
        }

        [SchemaPersistent]
        public IndexDefCollection Indexes
        {
            get
            {
                return _Indexes;
            }
        }

        [SchemaPersistent]
        public SQLRoleAssignmentCollection Roles
        {
            get
            {
                return _arrRoles;
            }
        }

        [SchemaPersistent, ReadOnlyAttribute(true)]
        public ColumnCollection Columns
        {
            get
            {
                return _Columns;
            }
        }

        [SchemaPersistent, ReadOnlyAttribute(true)]
        public TableRowCollection TableRows
        {
            get
            {
                return _TableRows;
            }
        }

        [SchemaPersistent]
        public string TableName
        {
            get
            {
                return _strTableName;
            }
            set
            {
                _strTableName = value;
                _strIdentifier = GetIdentifierFromTableName(value);

            }
        }

        protected abstract string GetIdentifierFromTableName(string strTableName);

        protected override void ValidateMembers()
        {
            CheckString(_strTableName, GetType().Name + ".TableName", false);
            CheckList(_Columns, GetType().Name + ".Columns");
            CheckList(_arrRoles, GetType().Name + ".Roles");
            CheckList(_TableRows, GetType().Name + "TableRows");
        }

        private ColumnCollection _Columns;
        protected string _strTableName;
        private TableRowCollection _TableRows;
        private SQLRoleAssignmentCollection _arrRoles;
        private IndexDefCollection _Indexes;
        protected GlobalForeignKeyCollection _GlobalForeignKeys;
    }

    public class GlobalTableDef : TableDef
    {
        protected override string GetIdentifierFromTableName(string strTableName)
        {
            return GlobalObjectNames.PhysicalTableNameFromLogicalTableName(strTableName);
        }
    }

    public class CaseTableDef : TableDef
    {
        public CaseTableDef()
        {
            _CaseForeignKeys = new CaseForeignKeyCollection();
            AttachToCollection(_CaseForeignKeys);
        }

        [SchemaPersistent]
        public CaseForeignKeyCollection CaseTableForeignKeys
        {
            get
            {
                return _CaseForeignKeys;
            }
        }

        protected override string GetIdentifierFromTableName(string strTableName)
        {
            return CaseObjectNames.PhysicalTableNameFromLogicalTableName(strTableName);
        }

        private CaseForeignKeyCollection _CaseForeignKeys;
    }

    public class SQLStoredProcedureParameter : SchemaObject
    {
        public SQLStoredProcedureParameter()
        {
            _eDataType = DataTypeEnum.Unspecified;
            _ePlatformBitmask = PlatformEnum.ALLSQL;
            _lStringLength = 0;
            _eDirection = ParameterDirectionEnum.INPUT;
        }

        [SchemaPersistent]
        public new string Name
        {
            set
            {
                if (value != null && value != string.Empty)
                {
                    _strIdentifier = String.Format("{0}{1}", value[0] == '@' ? "" : "@", value);
                }
                else
                {
                    _strIdentifier = value;
                }

            }
            get
            {
                return _strIdentifier;
            }
        }

        [SchemaPersistent]
        public DataTypeEnum DataType
        {
            set
            {
                _eDataType = value;

            }
            get
            {
                return _eDataType;
            }
        }

        [SchemaPersistent]
        public Int32 StringLength
        {
            get
            {
                return _lStringLength;
            }
            set
            {
                _lStringLength = value;

            }
        }

        [SchemaPersistent]
        public ParameterDirectionEnum ParameterDirection
        {
            get
            {
                return _eDirection;
            }
            set
            {
                _eDirection = value;
            }
        }

        protected override void ValidateMembers()
        {
            CheckInt(_lStringLength, GetType().Name + ".StringLength", 0, System.Int32.MaxValue);
            switch (_eDataType)
            {
                case DataTypeEnum.AnsiString:
                case DataTypeEnum.WideString:
                    CheckInt(_lStringLength, GetType().Name + ".StringLength", 1, 4000);
                    break;
            }
        }

        private DataTypeEnum _eDataType;
        private Int32 _lStringLength;
        private ParameterDirectionEnum _eDirection;
    }

    public class SQLStoredProcedureParameterCollection : SchemaObjectCollection
    {
        public SQLStoredProcedureParameterCollection()
            : base(typeof(SQLStoredProcedureParameter))
        { }

        public void Add(SQLStoredProcedureParameter param)
        {
            base.Add(param);
        }

        public void Remove(SQLStoredProcedureParameter param)
        {
            base.Remove(param);
        }

        public new SQLStoredProcedureParameter this[int iIndex]
        {
            get
            {
                return (SQLStoredProcedureParameter)base[iIndex];
            }
        }
    }

    public class SQLStoredProcedure : SchemaObject
    {
        public SQLStoredProcedure()
        {
            _strProcName = String.Empty;
            _arrParameters = new SQLStoredProcedureParameterCollection();
            AttachToCollection(_arrParameters);
            _strProcedureText = string.Empty;
            _arrRoles = new SQLRoleAssignmentCollection();
            AttachToCollection(_arrRoles);
            _ePlatformBitmask = PlatformEnum.ALLSQL;
        }

        [SchemaPersistent]
        public string ProcName
        {
            set
            {
                _strIdentifier = String.Format("[dbo].[{0}]", value);
                _strProcName = value;

            }
            get
            {
                return _strProcName;
            }
        }

        [SchemaPersistent]
        public string ProcedureText
        {
            get
            {
                return _strProcedureText;
            }
            set
            {
                _strProcedureText = value;

            }

        }

        [SchemaPersistent]
        public SQLRoleAssignmentCollection Roles
        {
            get
            {
                return _arrRoles;
            }
        }

        [SchemaPersistent]
        public SQLStoredProcedureParameterCollection Parameters
        {
            get
            {
                return _arrParameters;
            }
        }

        protected override void ValidateMembers()
        {
            CheckString(_strProcName, GetType().Name + ".Name", false);
            CheckString(_strProcedureText, GetType().Name + ".ProcedureText", false);
            CheckList(_arrParameters, GetType().Name + ".Parameters");
            CheckList(_arrRoles, GetType().Name + ".Roles");
        }

        string _strProcName;
        SQLStoredProcedureParameterCollection _arrParameters;
        SQLRoleAssignmentCollection _arrRoles;
        string _strProcedureText;
    }

    public class SQLRole : SchemaObject
    {
        public SQLRole()
        {
            _strRoleName = string.Empty;
            _ePlatformBitmask = PlatformEnum.ALLSQL;
        }

        [SchemaPersistent]
        public string RoleName
        {
            get
            {
                return _strRoleName;
            }
            set
            {
                _strRoleName = value;
                _strIdentifier = value;

            }
        }

        protected override void ValidateMembers()
        {
            CheckString(_strRoleName, GetType().Name + ".RoleName", false);
        }

        private string _strRoleName;
    }

    public class SQLRoleAssignment : SchemaObject
    {
        public SQLRoleAssignment()
        {
            _roleName = string.Empty;
            _eGrantPermission = ObjectPermissionEnum.NONE;
            _eDenyPermission = ObjectPermissionEnum.NONE;
            _ePlatformBitmask = PlatformEnum.ALLSQL;
        }

        [SchemaPersistent]
        public string RoleName
        {
            get
            {
                return _roleName;
            }
            set
            {
                _roleName = value;

            }
        }

        [SchemaPersistent]
        public ObjectPermissionEnum GrantAssignments
        {
            get
            {
                return _eGrantPermission;
            }
            set
            {
                _eGrantPermission = value;

            }
        }

        [SchemaPersistent]
        public ObjectPermissionEnum DenyAssignments
        {
            get
            {
                return _eDenyPermission;
            }
            set
            {
                _eDenyPermission = value;

            }
        }

        protected override void ValidateMembers()
        {
            CheckString(_roleName, GetType().Name + ".Role", false);
        }

        private string _roleName;
        private ObjectPermissionEnum _eGrantPermission;
        private ObjectPermissionEnum _eDenyPermission;
    }

    public class GlobalTableCollection : SchemaObjectCollection
    {
        public GlobalTableCollection()
            : base(typeof(GlobalTableDef))
        {
        }

        public void Add(GlobalTableDef tabdef)
        {
            base.Add(tabdef);
        }

        public void Remove(GlobalTableDef tabdef)
        {
            base.Remove(tabdef);
        }

        public new GlobalTableDef this[int index]
        {
            get
            {
                return (GlobalTableDef)base[index];
            }
        }
    }

    public class CaseTableDefCollection : SchemaObjectCollection
    {
        public CaseTableDefCollection()
            : base(typeof(CaseTableDef))
        { }

        public void Add(CaseTableDef def)
        {
            base.Add(def);
        }

        public void Remove(CaseTableDef def)
        {
            base.Remove(def);
        }

        public new CaseTableDef this[int iIndex]
        {
            get
            {
                return (CaseTableDef)base[iIndex];
            }
        }
    }

    public class View : SchemaObject
    {
        public View()
        {
            _strViewName = string.Empty;
            _strViewText = string.Empty;
            _Roles = new SQLRoleAssignmentCollection();
            AttachToCollection(_Roles);
        }

        [SchemaPersistent]
        public string ViewName
        {
            get
            {
                return _strViewName;
            }
            set
            {
                _strViewName = value;
                _strIdentifier = _strViewName;

            }
        }

        [SchemaPersistent]
        public string ViewText
        {
            get
            {
                return _strViewText;
            }
            set
            {
                _strViewText = value;

            }
        }

        [SchemaPersistent]
        public SQLRoleAssignmentCollection Roles
        {
            get
            {
                return _Roles;
            }
        }

        protected override void ValidateMembers()
        {
            CheckString(_strViewName, GetType().Name + ".ViewName", false);
            CheckString(_strViewText, GetType().Name + ".ViewText", false);
        }

        private string _strViewName;
        private string _strViewText;
        private SQLRoleAssignmentCollection _Roles;
    }

    public class ViewCollection : SchemaObjectCollection
    {
        public ViewCollection()
            : base(typeof(View))
        { }

        public void Add(View view)
        {
            base.Add(view);
        }

        public void Remove(View view)
        {
            base.Remove(view);
        }

        public new View this[int iIndex]
        {
            get
            {
                return (View)base[iIndex];
            }
        }
    }

    public class SQLStoredProcedureCollection : SchemaObjectCollection
    {
        public SQLStoredProcedureCollection()
            : base(typeof(SQLStoredProcedure))
        { }

        public void Add(SQLStoredProcedure proc)
        {
            base.Add(proc);
        }

        public void Remove(SQLStoredProcedure proc)
        {
            base.Remove(proc);
        }

        public new SQLStoredProcedure this[int iIndex]
        {
            get
            {
                return (SQLStoredProcedure)base[iIndex];
            }
        }
    }

    public class CustomScript : SchemaObject
    {

        public CustomScript()
        {
            _strScriptName = string.Empty;
            _strScriptText = string.Empty;
            _eRunTime = CustomScriptRunTimeEnum.AFTER_UPDATE;
            _lAssociatedMajorVer = 0;
            _lAssociatedMinorVer = 0;
        }

        [SchemaPersistent]
        public string ScriptName
        {
            get
            {
                return _strScriptName;
            }
            set
            {
                _strScriptName = value;
                _strIdentifier = _strScriptName;

            }
        }

        [SchemaPersistent]
        public string ScriptText
        {
            get
            {
                return _strScriptText;
            }
            set
            {
                _strScriptText = value;

            }
        }

        [SchemaPersistent]
        public CustomScriptRunTimeEnum RunTime
        {
            get
            {
                return _eRunTime;
            }
            set
            {
                _eRunTime = value;

            }
        }


        [SchemaPersistent]
        public Int32 AssociatedMajorVersion
        {
            get
            {
                return _lAssociatedMajorVer;
            }
            set
            {
                _lAssociatedMajorVer = value;

            }
        }

        [SchemaPersistent]
        public Int32 AssociatedMinorVersion
        {
            get
            {
                return _lAssociatedMinorVer;
            }
            set
            {
                _lAssociatedMinorVer = value;

            }
        }

        protected override void ValidateMembers()
        {
            CheckString(_strScriptText, GetType().Name + ".ScriptText", false);
            CheckString(_strScriptName, GetType().Name + ".ScriptName", false);
            CheckInt(_lAssociatedMajorVer, GetType().Name + ".AssociatedMajorVersion", 0, Int32.MaxValue);
            CheckInt(_lAssociatedMajorVer, GetType().Name + ".AssociatedMinorVersion", 0, Int32.MaxValue);
        }

        private string _strScriptText;
        private string _strScriptName;
        CustomScriptRunTimeEnum _eRunTime;
        private Int32 _lAssociatedMajorVer;
        private Int32 _lAssociatedMinorVer;
    }

    public class CustomScriptCollection : SchemaObjectCollection
    {
        public CustomScriptCollection()
            : base(typeof(CustomScript))
        { }

        public void Add(CustomScript script)
        {
            base.Add(script);
        }

        public void Remove(CustomScript script)
        {
            base.Remove(script);
        }

        public new CustomScript this[int iIndex]
        {
            get
            {
                return (CustomScript)base[iIndex];
            }
        }
    }

    public class SQLUserDefinedFunctionParameter : SchemaObject
    {
        public SQLUserDefinedFunctionParameter()
        {
            _eDataType = DataTypeEnum.Unspecified;
            _ePlatformBitmask = PlatformEnum.ALLSQL;
            _lStringLength = 0;
        }

        [SchemaPersistent]
        public new string Name
        {
            set
            {
                if (value != null && value != string.Empty)
                {
                    _strIdentifier = String.Format("{0}{1}", value[0] == '@' ? "" : "@", value);
                }
                else
                {
                    _strIdentifier = value;
                }

            }
            get
            {
                return _strIdentifier;
            }
        }

        [SchemaPersistent]
        public DataTypeEnum DataType
        {
            set
            {
                _eDataType = value;

            }
            get
            {
                return _eDataType;
            }
        }

        [SchemaPersistent]
        public Int32 StringLength
        {
            get
            {
                return _lStringLength;
            }
            set
            {
                _lStringLength = value;

            }
        }

        protected override void ValidateMembers()
        {
            CheckInt(_lStringLength, GetType().Name + ".StringLength", 0, System.Int32.MaxValue);
            switch (_eDataType)
            {
                case DataTypeEnum.AnsiString:
                case DataTypeEnum.WideString:
                    CheckInt(_lStringLength, GetType().Name + ".StringLength", 1, 4000);
                    break;
            }
        }

        private DataTypeEnum _eDataType;
        private Int32 _lStringLength;
    }

    public class SQLUserDefinedFunctionParameterCollection : SchemaObjectCollection
    {
        public SQLUserDefinedFunctionParameterCollection()
            : base(typeof(SQLUserDefinedFunctionParameter))
        { }

        public void Add(SQLUserDefinedFunctionParameter param)
        {
            base.Add(param);
        }

        public void Remove(SQLUserDefinedFunctionParameter param)
        {
            base.Remove(param);
        }

        public new SQLUserDefinedFunctionParameter this[int iIndex]
        {
            get
            {
                return (SQLUserDefinedFunctionParameter)base[iIndex];
            }
        }
    }

    public class SQLUserDefinedFunction : SchemaObject
    {
        public SQLUserDefinedFunction()
        {
            _ePlatformBitmask = PlatformEnum.ALLSQL;
            _strFunctionName = string.Empty;
            _Roles = new SQLRoleAssignmentCollection();
            AttachToCollection(_Roles);

            _Parameters = new SQLUserDefinedFunctionParameterCollection();
            AttachToCollection(_Parameters);

            _strReturnVal = string.Empty;
        }

        [SchemaPersistent]
        public string FunctionName
        {
            get
            {
                return _strFunctionName;
            }
            set
            {
                _strFunctionName = value;
                _strIdentifier = string.Format("[dbo].[{0}]", _strFunctionName);

            }
        }

        [SchemaPersistent]
        public SQLRoleAssignmentCollection Roles
        {
            get
            {
                return _Roles;
            }
        }

        [SchemaPersistent]
        public SQLUserDefinedFunctionParameterCollection Parameters
        {
            get
            {
                return _Parameters;
            }
        }

        [SchemaPersistent]
        public string ReturnValue
        {
            get
            {
                return _strReturnVal;
            }
            set
            {
                _strReturnVal = value;

            }
        }

        protected override void ValidateMembers()
        {
            CheckString(_strFunctionName, GetType().Name + ".FunctionName", false);
            CheckList(_Roles, GetType().Name + ".Roles");
            CheckList(_Parameters, GetType().Name + ".Parameters");
            CheckString(_strReturnVal, GetType().Name + ".ReturnValue", false);
        }

        private string _strFunctionName;
        private SQLRoleAssignmentCollection _Roles;
        private SQLUserDefinedFunctionParameterCollection _Parameters;
        private string _strReturnVal;
    }

    public class SQLUserDefinedFunctionCollection : SchemaObjectCollection
    {
        public SQLUserDefinedFunctionCollection()
            : base(typeof(SQLUserDefinedFunction))
        { }

        public void Add(SQLUserDefinedFunction fn)
        {
            base.Add(fn);
        }

        public void Remove(SQLUserDefinedFunction fn)
        {
            base.Remove(fn);
        }

        public new SQLUserDefinedFunction this[int iIndex]
        {
            get
            {
                return (SQLUserDefinedFunction)base[iIndex];
            }
        }
    }

    public class CaseSchema : SchemaObject
    {
        public CaseSchema()
        {
            _strIdentifier = "<New Schema>";

            _CaseTables = new CaseTableDefCollection();
            AttachToCollection(_CaseTables);

            _GlobalTables = new GlobalTableCollection();
            AttachToCollection(_GlobalTables);

            _SQLStoredProcs = new SQLStoredProcedureCollection();
            AttachToCollection(_SQLStoredProcs);

            _arrRoles = new SQLRoleCollection();
            AttachToCollection(_arrRoles);

            _Views = new ViewCollection();
            AttachToCollection(_Views);

            _Scripts = new CustomScriptCollection();
            AttachToCollection(_Scripts);

            _SQLUserDefinedFunctions = new SQLUserDefinedFunctionCollection();
            AttachToCollection(_SQLUserDefinedFunctions);

            _SchemaID = Guid.NewGuid();
        }

        public void Load(string strFile)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(strFile);
            FromXml(doc.FirstChild);
        }

        public void Load(Stream xmlStream)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlStream);
            FromXml(doc.FirstChild);
        }

        public void LoadXml(string xmlString)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);
            FromXml(doc.FirstChild);
        }

        public void Save(string strFile)
        {
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(ToXML(doc));
            doc.Save(strFile);
        }

        [SchemaPersistent]
        public Guid SchemaID
        {
            get
            {
                return _SchemaID;
            }
            set
            {
                _SchemaID = value;

            }
        }

        [SchemaPersistent]
        public CustomScriptCollection CustomScripts
        {
            get
            {
                return _Scripts;
            }
        }

        [SchemaPersistent]
        public string SchemaName
        {
            set
            {
                _strIdentifier = value;

            }
            get
            {
                return _strIdentifier;
            }
        }

        [SchemaPersistent]
        public CaseTableDefCollection CaseTables
        {
            get
            {
                return _CaseTables;
            }
        }

        [SchemaPersistent]
        public GlobalTableCollection GlobalTables
        {
            get
            {
                return _GlobalTables;
            }
        }

        [SchemaPersistent]
        public SQLStoredProcedureCollection SQLStoredProcedures
        {
            get
            {
                return _SQLStoredProcs;
            }
        }

        [SchemaPersistent]
        public SQLRoleCollection Roles
        {
            get
            {
                return _arrRoles;
            }
        }


        [SchemaPersistent]
        public ViewCollection Views
        {
            get
            {
                return _Views;
            }
        }

        [SchemaPersistent]
        public SQLUserDefinedFunctionCollection SQLUserDefinedFunctions
        {
            get
            {
                return _SQLUserDefinedFunctions;
            }
        }

        protected override void ValidateMembers()
        {
            CheckList(_CaseTables, GetType().Name + ".CaseTables");
            CheckList(_GlobalTables, GetType().Name + ".GlobalTables");
            CheckList(_SQLStoredProcs, GetType().Name + ".SQLStoredProcedures");
            CheckList(_arrRoles, GetType().Name + ".Roles");
            CheckList(_Views, GetType().Name + ".Views");
            CheckList(_Scripts, GetType().Name + ".CustomScripts");
            CheckList(_SQLUserDefinedFunctions, GetType().Name + ".SQLUserDefinedFunctions");
        }

        private CaseTableDefCollection _CaseTables;
        private GlobalTableCollection _GlobalTables;
        private SQLStoredProcedureCollection _SQLStoredProcs;
        private SQLRoleCollection _arrRoles;
        private ViewCollection _Views;
        private CustomScriptCollection _Scripts;
        private Guid _SchemaID;
        private SQLUserDefinedFunctionCollection _SQLUserDefinedFunctions;
    }

}
