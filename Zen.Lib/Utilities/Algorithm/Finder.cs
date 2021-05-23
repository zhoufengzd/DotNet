using System;
using System.Collections.Generic;

using Zen.Utilities.Generics;

namespace Zen.Utilities.Algorithm
{
    public sealed class Finder
    {
        /// <summary>
        /// Find and return the first matched one
        /// </summary>
        public static bool Find<T>(IEnumerable<T> enumerable, T searched) where T : IComparable<T>
        {
            return Find(enumerable, searched, CompareType.EQL);
        }
        public static bool Find<T>(IEnumerable<T> enumerable, T searched, CompareType ct) where T : IComparable<T>
        {
            if (enumerable == null)
                return false;

            bool found = false;
            foreach (T item in enumerable)
            {
                switch (ct)
                {
                    case CompareType.GT:
                        found = (searched.CompareTo(item) > 0); break;
                    case CompareType.GT_EQL:
                        found = (searched.CompareTo(item) >= 0); break;
                    case CompareType.EQL:
                        found = (searched.CompareTo(item) == 0); break;
                    case CompareType.LT_EQL:
                        found = (searched.CompareTo(item) <= 0); break;
                    case CompareType.LT:
                        found = (searched.CompareTo(item) < 0); break;
                }

                if (found)
                    return true;
            }

            return false;
        }

    }
}
