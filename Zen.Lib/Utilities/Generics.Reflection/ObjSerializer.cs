using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using Zen.Utilities.Generics;

namespace Zen.Utilities
{
    /// <summary>
    /// </summary>
    public sealed class ObjSerializer
    {
        public static void Save<T>(string fileName, T obj)
        {
            Type objType = typeof(T);
            if (objType == typeof(DataTable))
                DoSave(fileName, ToDataSet((DataTable)(object)obj));
            else if (TypeInterrogator.IsDictionaryType(objType))
                DoSave(fileName, ToList((IDictionary)obj));
            else
                DoSave(fileName, obj);
        }
        public static T Load<T>(string fileName)
        {
            Type objType = typeof(T);
            if (objType == typeof(DataTable))
            {
                DataSet ds = (DataSet)DoLoad(fileName, typeof(DataSet));
                return (T)((object)ToDataTable(ds));
            }
            else if (TypeInterrogator.IsDictionaryType(objType))
            {
                return (T)((object)LoadDictionary(fileName, objType));
            }
            else
            {
                return (T)DoLoad(fileName, objType);
            }
        }

        #region Specialized handling
        public static void SaveDictionary<TKey, TValue>(string fileName, Dictionary<TKey, TValue> dictionary)
        {
            Save(fileName, DictionaryAdapter.ToList(dictionary));
        }
        public static Dictionary<TKey, TValue> LoadDictionary<TKey, TValue>(string fileName)
        {
            List<KVPair<TKey, TValue>> lst = (List<KVPair<TKey, TValue>>)DoLoad(fileName, typeof(List<KVPair<TKey, TValue>>));
            return DictionaryAdapter.ToDictionary(lst);
        }
        #endregion

        #region private & specialized handling
        private static void DoSave<T>(string fileName, T obj)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            using (Stream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                serializer.Serialize(fs, obj);
                fs.Close();
            }
        }
        private static object DoLoad(string fileName, Type ObjType)
        {
            object obj = null;

            XmlSerializer serializer = new XmlSerializer(ObjType);
            using (Stream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                obj = serializer.Deserialize(fs);
                fs.Close();
            }

            return obj;
        }

        private static DataSet ToDataSet(DataTable tbl)
        {
            DataSet ds = null;
            if (tbl.DataSet == null)
            {
                ds = new DataSet("Wrapper");
                ds.Tables.Add(tbl);
            }
            else
            {
                ds = tbl.DataSet;
            }

            return ds;
        }
        private static DataTable ToDataTable(DataSet ds)
        {
            if (ds == null || ds.Tables.Count < 1)
                return null;

            return ds.Tables[0];
        }

        private static IList ToList(IDictionary dictionary)
        {
            Type[] typeArguments = TypeInterrogator.GetItemTypes(dictionary.GetType());
            Type lstkvType = GenBuilder.BuildType(typeof(KVPair<,>), typeArguments);
            IList target = (IList)GenBuilder.BuildInstance(typeof(List<>), new Type[] { lstkvType });
            foreach (DictionaryEntry it in dictionary)
            {
                target.Add(GenBuilder.BuildInstance(lstkvType, new object[] { it.Key, it.Value }));
            }

            return target;
        }
        private static IDictionary LoadDictionary(string fileName, Type dictionaryT)
        {
            Type[] typeArguments = TypeInterrogator.GetItemTypes(dictionaryT);
            Type lstKeyValueType = GenBuilder.BuildType(typeof(KVPair<,>), typeArguments);
            Type lstType = GenBuilder.BuildType(typeof(List<>), new Type[] { lstKeyValueType });
            IList list = (IList)DoLoad(fileName, lstType);

            PropertyInfo keyProperty = lstKeyValueType.GetProperty("Key");
            PropertyInfo valueProperty = lstKeyValueType.GetProperty("Value");
            IDictionary target = (IDictionary)GenBuilder.BuildInstance(typeof(Dictionary<,>), typeArguments);
            foreach (object it in list)
            {
                object key = keyProperty.GetValue(it, null);
                if (key != null)
                    target.Add(key, valueProperty.GetValue(it, null));
            }

            return target;
        }
        #endregion
    }

    /// <summary>
    /// The only intended audience is XmlSerializer
    /// </summary>
    public sealed class KVPair<TKey, TValue>
    {
        public KVPair()
        {
        }
        public KVPair(TKey key, TValue val)
        {
            _key = key;
            _value = val;
        }

        public TKey Key
        {
            get { return _key; }
            set { _key = value; }
        }

        public TValue Value
        {
            get { return _value; }
            set { _value = value; }
        }

        #region private data
        private TKey _key;
        private TValue _value;
        #endregion
    }
}
