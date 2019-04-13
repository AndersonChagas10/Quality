using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationModule
{
    public class Integration
    {

        public static System.Action RunIntegration(string configuration, string script, string tableName)
        {
            List<string> scripts = new List<string>();
            Setting setting = new Setting(configuration, script);
            using (var factory = new Factory(
                setting.Settings["DataSource"],
                setting.Settings["Catalog"],
                setting.Settings["Password"],
                setting.Settings["User"]))
            {
                var retorno = factory.QueryNinjaADO(script);
                scripts = setting.CreateInsertScript(retorno, tableName);
            }

            using (var factory = new Factory(
                setting.Settings["DataSourceServer"],
                setting.Settings["CatalogServer"],
                setting.Settings["PasswordServer"],
                setting.Settings["UserServer"]))
            {
                foreach (var item in scripts)
                {
                    var sqlCommand = $"{item};SELECT CAST(1 AS int)";
                    SqlCommand cmd = new SqlCommand(sqlCommand);

                    int i = factory.InsertUpdateData(cmd);

                }
            }
            return null;
        }

        public static System.Action RunIntegrationOneValue(string configuration, string script, string tableName)
        {
            List<string> scripts = new List<string>();
            Setting setting = new Setting(configuration, script);
            using (var factory = new Factory(
                setting.Settings["DataSource"],
                setting.Settings["Catalog"],
                setting.Settings["Password"],
                setting.Settings["User"]))
            {
                try
                {
                    var retorno = factory.QueryNinjaADO(script);
                    scripts = setting.CreateInsertScriptOneValue(retorno, tableName);
                }
                catch (Exception e)
                {
                }
            }

            using (var factory = new Factory(
                setting.Settings["DataSourceServer"],
                setting.Settings["CatalogServer"],
                setting.Settings["PasswordServer"],
                setting.Settings["UserServer"]))
            {
                foreach (var item in scripts)
                {
                    try
                    {
                        var sqlCommand = $"{item};SELECT CAST(1 AS int)";
                        SqlCommand cmd = new SqlCommand(sqlCommand);

                        int i = factory.InsertUpdateData(cmd);

                    }
                    catch (Exception e)
                    {

                    }
                }
            }
            return null;
        }
    }
}
