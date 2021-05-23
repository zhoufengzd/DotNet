using System;
using System.Collections.ObjectModel;
using Zen.DBMS.Schema;
using Zen.Common.Def;

namespace Zen.DBMS
{
    //Text (Line)
    //  ==> Line Consumer 
    //ColumnDataPro
    //  Pre Process
    //  String -> Value (by Column Meta Data + Customized Process)
    //  Value -> String  
    //  Post Process


    /// <summary>
    /// Column data processor
    ///   DB centric: Write to column
    /// </summary>
    public sealed class ColumnWriter
    {
        public ColumnWriter(TableSchema schema)
        {
            _tblSchema = schema;
        }

        public object ToColumnValue(int colIndex, string srcData)
        {
            //ColumnMeta = _tblSchema.ColumnDefs[colIndex];
            return srcData;
        }

        #region private functions
        private bool ValidateByType(ColumnMeta fldInfo, object colValue, ref object colValueConverted, ref FieldError fldError)
        {
            // Pre filter out null value & empty value
            if (colValue == null || string.IsNullOrEmpty(colValue.ToString()))
                return HandleNullValue(fldInfo, ref colValue, ref fldError);

            // Convert to string type & check null value again
            DBDataType fldType = fldInfo.DataType;
            string stringValueRaw = null;

            if (!ValueToString(fldInfo, colValue, fldType, ref stringValueRaw, ref fldError))
                return false;

            // Convert raw string to particular field type
            string stringValueConverted = stringValueRaw;
            switch (fldType.TypeEnum)
            {
                case DataTypeEnum.AnsiString:
                case DataTypeEnum.WideString:
                    if (!CheckMaxSize(fldInfo, stringValueConverted, ref fldError))
                    {
                        return false;
                    }
                    colValueConverted = stringValueConverted;
                    return true;

                case DataTypeEnum.DATETIME:
                    colValueConverted = stringValueConverted;
                    return true;

                case DataTypeEnum.Int32:
                    long longValueConverted = 0;
                    try
                    {
                        longValueConverted = Convert.ToInt64(stringValueRaw);
                    }
                    catch (Exception)
                    {
                        fldError.Errors.Add(ErrorType.InvalidDataType);
                        return false;
                    }
                    colValueConverted = longValueConverted;
                    return true;

                case DataTypeEnum.NUMERIC:
                    double doubleValueConverted = 0.0;
                    try
                    {
                        doubleValueConverted = Convert.ToDouble(stringValueRaw);
                    }
                    catch (Exception)
                    {
                        fldError.Errors.Add(ErrorType.InvalidDataType);
                        return false;
                    }
                    colValueConverted = doubleValueConverted;
                    return true;

                default:
                    colValueConverted = stringValueRaw;
                    return true;
            }
        }

        private bool ValueToString(ColumnMeta fldInfo, object colValue, DBDataType fldType, ref string stringValueRaw, ref FieldError fldError)
        {
            Type valueType = colValue.GetType();
            bool isStringType = (valueType == typeof(string));
            bool isCollectionType = (valueType == typeof(Collection<string>));

            Collection<string> collection = null;
            if (isCollectionType)
                collection = (Collection<string>)(colValue);

            if (isStringType)
                stringValueRaw = (string)colValue;

            stringValueRaw = stringValueRaw.Trim();

            return true;
        }

        private bool CheckMaxSize(ColumnMeta fldInfo, string stringValueConverted, ref FieldError fldError)
        {
            if (string.IsNullOrEmpty(stringValueConverted))
                return true;

            if (stringValueConverted.Length > fldInfo.DataType.StringLength)
            {
                fldError.Errors.Add(ErrorType.ExceedsColumnSize);
                return false;
            }

            return true;
        }

        private bool IsNullAllowed(ColumnMeta fldInfo, ref FieldError fldError)
        {
            if (!fldInfo.Nullable)
            {
                fldError.Errors.Add(ErrorType.NonNullValueExpected);
                return false;
            }
            return true;
        }

        private bool HandleNullValue(ColumnMeta fldInfo, ref object colValue, ref FieldError fldError)
        {
            if (!IsNullAllowed(fldInfo, ref fldError))
            {
                return false;
            }
            else
            {
                colValue = null;
                return true;
            }
        }
        #endregion

        #region private data
        private TableSchema _tblSchema;
        #endregion
    }
}
