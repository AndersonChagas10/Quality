using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationModule
{
    internal class Setting
    {
        public Dictionary<string, string> Settings { get; set; }
        //DataSource:SERVERGRT|Catalog:hl_7deSetembro|User:sa|Password:1qazmko0|DataSourceServer:SERVERGRT|CatalogServer:HL_GRT|UserServer:sa|PasswordServer:1qazmko0

        public string Script { get; set; }

        public Setting(string configuration, string script)
        {
            var config = configuration.Split('|');
            Settings = new Dictionary<string, string>();
            foreach (var item in config)
            {
                var keyValue = item.Split(':');
                Settings.Add(keyValue[0], keyValue[1]);
            }

            Script = script;
        }

        public List<string> CreateInsertScript(List<Dictionary<string, object>> retorno, string nomeTabela)
        {
            List<string> listScripts = new List<string>();
            StringBuilder script = new StringBuilder();
            if (!(retorno.Count > 0))
            {
                return listScripts;
            }

            for (int i = 0; i < retorno.Count; i++)
            {
                if (i % 500 == 0)
                {
                    listScripts.Add(script.ToString());
                    script = new StringBuilder();
                    script.Append($"INSERT INTO {MontaNomeTabela(nomeTabela)} (");
                    script.Append(string.Join(",", retorno.FirstOrDefault().Select(x => $"[{x.Key}]")));
                    script.AppendLine(") values ");
                }
                else
                {
                    script.Append(",");
                }

                var estruturaValores = $"(@{string.Join(",@", retorno.FirstOrDefault().Select(x => "[" + x.Key + "]"))})";

                var linha = retorno[i];

                var scriptLinha = estruturaValores;
                foreach (var item in linha)
                {
                    var value = Convert.ChangeType(item.Value, item.Value.GetType()).ToString();
                    if (item.Value.GetType() == typeof(string))
                    {
                        value = $"'{value.Replace("'", "''")}'";
                    }
                    else if (item.Value.GetType() == typeof(DateTime))
                    {
                        value = $"'{(Convert.ToDateTime(item.Value)).ToString("yyyy-MM-dd HH:mm:ss")}'";
                    }

                    scriptLinha = scriptLinha.Replace($"@[{item.Key}]", $"{(value?.Length > 0 ? value.Replace(',', '.') : "null")}");
                }
                script.AppendLine(scriptLinha);

            }

            //Adiciona o ultimo script gerado
            listScripts.Add(script.ToString());

            return listScripts;
        }

        public List<string> CreateInsertScriptOneValue(List<Dictionary<string, object>> retorno, string nomeTabela)
        {
            List<string> listScripts = new List<string>();
            StringBuilder script = new StringBuilder();
            if (!(retorno.Count > 0))
            {
                return listScripts;
            }

            for (int i = 0; i < retorno.Count; i++)
            {
                if (i % 1 == 0)
                {
                    listScripts.Add(script.ToString());
                    script = new StringBuilder();
                    script.Append($"INSERT INTO {MontaNomeTabela(nomeTabela)} (");
                    script.Append(string.Join(",", retorno.FirstOrDefault().Select(x => $"[{x.Key}]")));
                    script.AppendLine(") values ");
                }
                else
                {
                    script.Append(",");
                }

                var estruturaValores = $"(@{string.Join(",@", retorno.FirstOrDefault().Select(x => "[" + x.Key + "]"))})";

                var linha = retorno[i];

                var scriptLinha = estruturaValores;
                foreach (var item in linha)
                {
                    var value = Convert.ChangeType(item.Value, item.Value.GetType()).ToString();
                    if (item.Value.GetType() == typeof(string))
                    {
                        value = $"'{value.Replace("'", "''")}'";
                    }
                    else if (item.Value.GetType() == typeof(DateTime))
                    {
                        value = $"'{(Convert.ToDateTime(item.Value)).ToString("yyyy-MM-dd HH:mm:ss")}'";
                    }

                    scriptLinha = scriptLinha.Replace($"@[{item.Key}]", $"{(value?.Length > 0 ? value.Replace(',', '.') : "null")}");
                }
                script.AppendLine(scriptLinha);

            }

            //Adiciona o ultimo script gerado
            listScripts.Add(script.ToString());

            return listScripts;
        }

        private string MontaNomeTabela(string nomeTabela)
        {
            var nome = "";
            foreach (var item in nomeTabela.Split('.'))
            {
                if (!string.IsNullOrEmpty(nome))
                    nome += ".";
                nome += "[" + item + "]";
            }
            return nome;
        }
    }
}
