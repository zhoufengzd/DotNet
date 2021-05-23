using System;
using System.Reflection;
using Zen.DBMS.Schema;
using Zen.Utilities;
using System.Data;

namespace Zen.DBMS
{
    /// <summary>
    /// Object  <==> Relational Database 
    /// </summary>
    public sealed class ORMGenerator
    {
        public string GenerateDDL(Type tp, SchemaContext context)
        {
            // Fix type name => '.' to '_'
            TableMeta tableMeta = new TableMeta(tp.Name.Replace('.', '_'), context);

            // Loop through all obj properties
            // Leaf -> Column
            // Complex -> Build path / Cache mapping 
            PropertyInfo[] properties = tp.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo pi in properties)
            {
                Type unerLyingType = TypeInterrogator.GetUnderlyingType(pi.PropertyType);
                //if (IsSkippedType(unerLyingType))
                //    return;

                if (TypeInterrogator.IsSingleValueType(unerLyingType))
                {
                    tableMeta.ColMetaList.Add(BuildColumnMeta(pi, unerLyingType));
                }
                //else if (TypeInterrogator.IsCollectionType(unerLyingType))
                //    BuildCollectionCtrl(pi, pi.PropertyType, typeObj);
                //else
                //    BuildCompositeCtrl(unerLyingType, pi.GetValue(typeObj, null));
            }

            return tableMeta.BuildCreateDDL();
        }

        // To do: unify TableShema with TableMeta
        public DataTable GenerateDataTable(Type tp)
        {
            // Type => Table Schema (TableMeta) => DataTable
            return null;
        }

        private ColumnMeta BuildColumnMeta(PropertyInfo pi, Type tp)
        {
            ColumnMeta colMeta = null; 
            if (TypeInterrogator.IsNumericType(tp))
            {
                colMeta = new ColumnMeta(pi.Name, DataTypeEnum.NUMERIC, new SqlsvrDTDictionary());
                colMeta.DataType.Precision = 10;
                colMeta.DataType.Scale = 2;
            }
            else if (TypeInterrogator.IsBoolType(tp))
            {
                colMeta = new ColumnMeta(pi.Name, DataTypeEnum.Bit, new SqlsvrDTDictionary());
            }
            else if (TypeInterrogator.IsDateTimeType(tp))
            {
                colMeta = new ColumnMeta(pi.Name, DataTypeEnum.DATETIME, new SqlsvrDTDictionary());
            }
            else if (TypeInterrogator.IsEnumType(tp))
            {
                colMeta = new ColumnMeta(pi.Name, DataTypeEnum.Int32, new SqlsvrDTDictionary());
                //colMeta.DataType.Scale = 2;
            }
            else if (TypeInterrogator.IsStringType(tp))
            {
                colMeta = new ColumnMeta(pi.Name, DataTypeEnum.AnsiString, new SqlsvrDTDictionary());
                colMeta.DataType.StringLength = 256;
            }

            return colMeta;
        }

        // depends on mapping
        public object TableToObject()
        {
            return null;
        }
    }
}
