using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace o3DLib.Extensions
{
    public static class sysExt
    {

        public const int ACCURRACY = 6;

        public static bool IsEven(this int aInt)
        {
            return (double)aInt / 2 == aInt / 2;
        }

        public static void AddMany<T>(this List<T> aList, params T[] aItems)
        {
            foreach (var aItem in aItems)
                aList.Add(aItem);
        }

        public static bool IsEqual(this double value1, double value2)
        {
            return Math.Round(value1, ACCURRACY) == Math.Round(value2, ACCURRACY);
        }

    }
}
