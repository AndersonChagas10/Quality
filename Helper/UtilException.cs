using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace SgqSystem.Helpers
{
    public static class UtilSqlCommand
    {
        public static void AddParameterNullable(SqlCommand cmd, string key, dynamic valor)
        {
            if (valor == null)
            {
                cmd.Parameters.Add(new SqlParameter(key, DBNull.Value));
            }
            else
            {
                cmd.Parameters.Add(new SqlParameter(key, valor));
            }
        }
    }
}