using System;
using System.Data;

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Zen.DBMS
{
    /// <summary>
    /// Validate field data based on FieldInfo [SLT_FieldInfo]
    /// </summary>
    class CtsFieldDataValidator
    {
        public CtsFieldDataValidator(IMeTblInterpreterMgr meMgr)
        {
            _meMgr = meMgr;
        }

        public bool Validate(FieldInfo fldInfo, object colValue, out object colValueConverted, ref FieldError fldError)
        {
            colValueConverted = null;

            // Validate Type: conversion, length, nullability
            if (!ValidateByType(fldInfo, colValue, ref colValueConverted, ref fldError))
                return false;

            if (!ValidateByColName(fldInfo, colValue, ref colValueConverted, ref fldError))
                return false;

            return true;
        }

        #region private functions
        private bool ValidateByType(FieldInfo fldInfo, object colValue, ref object colValueConverted, ref FieldError fldError)
        {
            // Pre filter out null value & empty value
            if (colValue == null || string.IsNullOrEmpty(colValue.ToString()))
                return HandleNullValue(fldInfo, ref colValue, ref fldError);

            // Convert to string type & check null value again
            FieldType fldType = fldInfo.FieldType;
            string stringValueRaw = null;

            if (!ValueToString(fldInfo, colValue, fldType, ref stringValueRaw, ref fldError))
                return false;

            // Convert raw string to particular field type
            string stringValueConverted = stringValueRaw;
            switch (fldType)
            {
                case FieldType.FixedLengthText:
                case FieldType.LongText:
                case FieldType.MultiEntry:
                    if (!CheckMaxSize(fldInfo, stringValueConverted, ref fldError))
                    {
                        return false;
                    }
                    colValueConverted = stringValueConverted;
                    return true;

                case FieldType.Date:
                    stringValueConverted = SWDateConverter.ToDateString(stringValueRaw);
                    if (string.IsNullOrEmpty(stringValueConverted))
                    {
                        fldError.Errors.Add(ErrorType.InvalidDataType);
                        return false;
                    }
                    colValueConverted = stringValueConverted;
                    return true;

                case FieldType.Time:
                    stringValueConverted = SWTimeConverter.ToTimeString(stringValueRaw);
                    if (string.IsNullOrEmpty(stringValueConverted))
                    {
                        fldError.Errors.Add(ErrorType.InvalidDataType);
                        return false;
                    }
                    colValueConverted = stringValueConverted;
                    return true;

                case FieldType.Int32:
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

                case FieldType.Real:
                case FieldType.Double:
                case FieldType.Currency:
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

        private bool ValidateByColName(FieldInfo fldInfo, object colValue, ref object colValueConverted, ref FieldError fldError)
        {
            return true;
        }

        private bool ValidateMultiEntry(FieldInfo fldInfo, IEnumerable<string> multiEntries, int meCount, ref FieldError fldError)
        {
            if (meCount < 1)
                return true;

            MeTblInterpreter meInterpreter = _meMgr.GetMeTblInterpreter(fldInfo.FieldName);
            return meInterpreter.ValidateMultiEntry(multiEntries, meCount, ref fldError);
        }

        private bool ValueToString(FieldInfo fldInfo, object colValue, FieldType fldType, ref string stringValueRaw, ref FieldError fldError)
        {
            Type valueType = colValue.GetType();
            bool isStringType = (valueType == typeof(string));
            bool isCollectionType = (valueType == typeof(Collection<string>));

            Collection<string> collection = null;
            if (isCollectionType)
                collection = (Collection<string>)(colValue);

            if (isStringType)
                stringValueRaw = (string)colValue;

            if (fldType == FieldType.MultiEntry)
            {
                string multiEntryString = null;
                if (isCollectionType)
                {
                    if (!ValidateMultiEntry(fldInfo, collection, collection.Count, ref fldError))
                        return false;

                    multiEntryString = MultiEntryConverter.ToMeString(collection);
                }
                else if (isStringType)
                {
                    MultiEntry me = MultiEntryConverter.ToMultiEntry(stringValueRaw, _meMgr.MultiEntryDelimiter);
                    if (!ValidateMultiEntry(fldInfo, me.MultiEntryList, me.MultiEntryList.Length, ref fldError))
                        return false;

                    multiEntryString = MultiEntryConverter.ToMeString(me);
                }
                stringValueRaw = multiEntryString;      
            }

            stringValueRaw = stringValueRaw.Trim();  
              
            return true;
        }

        private bool CheckMaxSize(FieldInfo fldInfo, string stringValueConverted, ref FieldError fldError)
        {
            if (string.IsNullOrEmpty(stringValueConverted))
                return true;

            if (stringValueConverted.Length > fldInfo.MaxSize)
            {
                fldError.Errors.Add(ErrorType.ExceedsColumnSize);
                return false;
            }

            return true;
        }

        private bool IsNullAllowed(FieldInfo fldInfo, ref FieldError fldError)
        {
            if (!fldInfo.IsNullable)
            {
                fldError.Errors.Add(ErrorType.NonNullValueExpected);
                return false;
            }
            return true;
        }

        private bool HandleNullValue(FieldInfo fldInfo, ref object colValue, ref FieldError fldError)
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
        private IMeTblInterpreterMgr _meMgr;
        #endregion
    }
}