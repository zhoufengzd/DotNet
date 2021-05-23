using System;
using System.Collections.Generic;

namespace Zen.Utilities.Defs
{
    // reference type: class,  delegate, array, or interface

    // value-type:
    //   struct-type
    //     type-name: e.g public struct Xyz {}
    //     simple-type: numeric-type + bool
    //   enum-type
    //     type-name: e.g public enum XyzEnumType { X, Y, Z, }

    public sealed class TypeCategories
    {
        public static readonly Type[] IntTypes = new Type[]
        {
            typeof(SByte), typeof(Byte), typeof(Int16), typeof(UInt16), 
            typeof(Int32), typeof(UInt32), typeof(Int64), typeof(UInt64),
            typeof(Char)
        };
        public static readonly Type[] FloatTypes = new Type[]
        { 
            typeof(Single), typeof(Double)  // float & double
        };
        public static readonly Type[] DecimalTypes = new Type[]
        { 
            typeof(Decimal) // 128-bit data type
        };

        /// <summary>
        /// numeric-type: integral-type + floating-point-type + decimal
        /// </summary>
        public static readonly Type[] NumericTypes = new Type[]
        {
            typeof(SByte), typeof(Byte), typeof(Int16), typeof(UInt16), 
            typeof(Int32), typeof(UInt32), typeof(Int64), typeof(UInt64),
            typeof(Char), 
            typeof(Single), typeof(Double), //floating-point-type
            typeof(Decimal),
        };

        /// <summary>
        /// simple-type: numeric-type + bool
        /// </summary>
        public static readonly Type[] SimpleTypes = new Type[]
        {
            typeof(SByte), typeof(Byte), typeof(Int16), typeof(UInt16), 
            typeof(Int32), typeof(UInt32), typeof(Int64), typeof(UInt64),
            typeof(Char), 
            typeof(Single), typeof(Double), //floating-point-type
            typeof(Decimal),
            typeof(Boolean),
        };
    }
}
