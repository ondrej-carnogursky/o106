using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace o3DLib.Extensions
{
    public static class sysExt
    {

        public const int ACCURRACY = 6;

        public static bool IsEven(this int aInt)
        {
            return (double)aInt / 2 == aInt / 2;
        }

        public static void TryRemove<T>(this IList<T> list, T o)
        {
            if (list.Contains<T>(o))
                list.Remove(o);
        }

        public static Point Middle(this Point p1,Point p2)
        {
            return p1 + (p2 - p1) / 2;
        }

        public static void NotIdioticClear<T>(this System.Collections.ObjectModel.ObservableCollection<T> col)
        {
            while (col.Count > 0)
                col.RemoveAt(col.Count - 1);
        }

        public static void AddMany<T>(this List<T> aList, params T[] aItems)
        {
            foreach (var aItem in aItems)
                aList.Add(aItem);
        }

        public static void AddMany<T>(this List<T> aList, IList<T> aItems)
        {
            foreach (var aItem in aItems)
                aList.Add(aItem);
        }


        public static bool IsEqual(this double value1, double value2)
        {
            return Math.Round(value1, ACCURRACY) == Math.Round(value2, ACCURRACY);
        }

        public static bool IsEqual(this Point value1, Point value2)
        {
            return value1.X.IsEqual(value2.X) && value1.Y.IsEqual(value2.Y);
        }
    }
}
