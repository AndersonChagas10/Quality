using System;
using System.Data.SqlClient;

namespace Conformity.Application.Helper
{
    public static class ADOHelper
    {
        public static void AddParameterNullable(this SqlCommand cmd, string key, dynamic valor)
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
