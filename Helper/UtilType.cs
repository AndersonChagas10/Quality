using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public static class UtilType
    {
        public static bool IsPrimitiveAndNullableType(this Type type)
        {
            return
                type == typeof(int) || type == typeof(int?) ||
                type == typeof(long) || type == typeof(long?) ||
                type == typeof(bool) || type == typeof(bool?) ||
                type == typeof(string) ||
                type == typeof(System.Int16) || type == typeof(System.Int16?) ||
                type == typeof(System.Int32) || type == typeof(System.Int32?) ||
                type == typeof(System.Int64) || type == typeof(System.Int64?) ||
                type == typeof(String) ||
                type == typeof(DateTime) || type == typeof(DateTime?) ||
                type == typeof(char) || type == typeof(char?);
        }
    }
}
