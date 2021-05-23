using System.Diagnostics;

namespace Zen.DBMS.Schema
{
    using TwowayMap = Zen.Utilities.Generics.TwowayMap<DataTypeEnum, string>;

    /// <summary>
    /// Provide data type mapping service: DBMS Data Type  -- Local XML Schema Data Type   
    /// </summary>
    public interface IDataTypeDictionary
    {
        string ToString(DataTypeEnum dtType);
        DataTypeEnum ToType(string strTypeName);
        DBMSPlatformEnum DBMSPlatform { get; }
    }

    public abstract class DTDictionaryBase : IDataTypeDictionary
    {
        public string ToString(DataTypeEnum dtType)
        {
            LoadMap();

            string valueTmp = _dtMap.FindT2(dtType);
            return (valueTmp != null) ? valueTmp : string.Empty;
        }

        public DataTypeEnum ToType(string strTypeName)
        {
            LoadMap();

            object valueTmp = _dtMap.FindT1(strTypeName);
            return (valueTmp != null) ? (DataTypeEnum)(valueTmp) : DataTypeEnum.Unspecified;
        }

        public abstract DBMSPlatformEnum DBMSPlatform { get; }

        #region protected memeber function
        protected abstract void LoadMap();
        #endregion

        #region protected data section
        protected TwowayMap _dtMap;
        #endregion
    }

    public class SqlsvrDTDictionary : DTDictionaryBase
    {
        public SqlsvrDTDictionary()
        {
        }

        public override DBMSPlatformEnum DBMSPlatform
        {
            get { return DBMSPlatformEnum.SqlServer; }
        }

        #region private memeber function
        protected override void LoadMap()
        {
            if (_dtMap != null)
                return;

            _dtMap = new TwowayMap();

            _dtMap.Add(DataTypeEnum.Int32, "INT");
            _dtMap.Add(DataTypeEnum.AnsiString, "VARCHAR");
            _dtMap.Add(DataTypeEnum.WideString, "NVARCHAR");
            _dtMap.Add(DataTypeEnum.AnsiCLOB, "TEXT");
            _dtMap.Add(DataTypeEnum.WideCLOB, "NTEXT");
            _dtMap.Add(DataTypeEnum.BLOB, "BINARY");
            _dtMap.Add(DataTypeEnum.Bit, "BIT");
            _dtMap.Add(DataTypeEnum.GUID, "UNIQUEIDENTIFIER");
            _dtMap.Add(DataTypeEnum.DATETIME, "DATETIME");
            _dtMap.Add(DataTypeEnum.NUMERIC, "NUMERIC");
            //_dtMap.Add(DataTypeEnum.MONEY, "MONEY");
        }
        #endregion
    }

    public class OracleDTDictionary : DTDictionaryBase
    {
        public OracleDTDictionary()
        {
        }

        public override DBMSPlatformEnum DBMSPlatform
        {
            get { return DBMSPlatformEnum.Oracle; }
        }

        #region private memeber function
        protected override void LoadMap()
        {
            if (_dtMap != null)
                return;
            
            Debug.Assert(false); // not mapped yet!
            _dtMap = new TwowayMap();
        }
        #endregion
    }
}

