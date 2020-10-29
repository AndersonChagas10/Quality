using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace IntegrationModule
{
    public class Integration
    {

        public static System.Action RunIntegrationOneValue(string configuration, string script, string tableName, out Exception ex)
        {
            ex = null;
            List<string> scripts = new List<string>();
            Setting setting = new Setting(configuration, script);
            Factory factory = null;
            if (setting.Settings["JustCommand"] == "on")
            {

                if (setting.Settings.ContainsKey("IsLocal") && setting.Settings["IsLocal"] == "on")
                {
                    factory = new Factory("DefaultConnection");
                }
                else
                {
                    factory = new Factory(
                    setting.Settings["DataSource"],
                    setting.Settings["Catalog"],
                    setting.Settings["Password"],
                    setting.Settings["User"]);
                }

                try
                {
                    var sqlCommand = $"{script};SELECT CAST(1 AS int)";
                    SqlCommand cmd = new SqlCommand(sqlCommand);

                    int i = factory.InsertUpdateData(cmd);

                }
                catch (Exception e)
                {
                    ex = e;
                    return null;
                }
                finally
                {
                    factory.Dispose();
                }
            }
            else
            {
                if (setting.Settings.ContainsKey("IsLocal") && setting.Settings["IsLocal"] == "on")
                {
                    factory = new Factory("DefaultConnection");
                }
                else
                {
                    factory = new Factory(
                         setting.Settings["DataSource"],
                         setting.Settings["Catalog"],
                         setting.Settings["Password"],
                         setting.Settings["User"]);
                }

                try
                {
                    var retorno = factory.QueryNinjaADO(script);
                    scripts = setting.CreateInsertScriptOneValue(retorno, tableName);
                }
                catch (Exception e)
                {
                    ex = e;
                    return null;
                }
                finally
                {
                    factory.Dispose();
                }


                if (setting.Settings.ContainsKey("IsLocalServer") && setting.Settings["IsLocalServer"] == "on")
                {
                    factory = new Factory("DefaultConnection");
                }
                else
                {
                    factory = new Factory(
                        setting.Settings["DataSourceServer"],
                        setting.Settings["CatalogServer"],
                        setting.Settings["PasswordServer"],
                        setting.Settings["UserServer"]);
                }

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
                        ex = e;
                        factory.Dispose();
                        return null;
                    }
                    finally
                    {
                        
                    }
                }

                factory.Dispose();

            }

            return null;
        }

    }
}
