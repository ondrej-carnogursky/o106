using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace o3DLib.Extensions
{
    public static class sysExt
    {
        public static bool IsEven(this int aInt)
        {
            return (double)aInt / 2 == aInt / 2;
        }

        public static void AddMany<T>(this List<T> aList, params T[] aItems)
        {
            foreach (var aItem in aItems)
                aList.Add(aItem);
        }

    }
}
