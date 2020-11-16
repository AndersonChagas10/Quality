using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogSystem
{
    public class LogTrackBusiness : LogSystem
    {

        public static void RegisterIfNotExist(object obj, int json_id, string table_name, int userSgq_Id)
        {
            if(GetLogTrack(table_name,json_id).Count() == 0)
            {
                Register(obj, json_id, table_name, userSgq_Id);
            }
        }

        public static void Register(object obj, int json_id, string table_name, int userSgq_Id)
        {
            using (var db = new Dominio.SgqDbDevEntities())
            {
                Dominio.LogTrack logTrack = new Dominio.LogTrack();
                logTrack.Json_Id = json_id;
                logTrack.Json = ToJson(obj);
                logTrack.Tabela = table_name;
                logTrack.UserSgq_Id = userSgq_Id;
                logTrack.AddDate = DateTime.Now;
                db.LogTrack.Add(logTrack);
                db.SaveChanges();
            }
        }

        public static void Register(object obj, int json_Id, string table_name, int userSgq_Id, int parReason_Id, string motivo)
        {
            using (var db = new Dominio.SgqDbDevEntities())
            {
                Dominio.LogTrack logTrack = new Dominio.LogTrack();
                logTrack.Json_Id = json_Id;
                logTrack.Json = ToJson(obj);
                logTrack.Tabela = table_name;
                logTrack.UserSgq_Id = userSgq_Id;
                logTrack.AddDate = DateTime.Now;
                logTrack.ParReason_Id = parReason_Id;
                logTrack.Motivo = motivo;
                db.LogTrack.Add(logTrack);
                db.SaveChanges();
            }
        }

        public static IEnumerable<object> GetLogTrack(string table_name, int json_Id)
        {
            using (var db = new Dominio.SgqDbDevEntities())
            {
                var logs = db.LogTrack
                    .Include("UserSgq")
                    .Include("ParReason")
                    .Where(x => x.Tabela == table_name && x.Json_Id == json_Id)
                    .OrderByDescending(x => x.AddDate)
                    .ToList()
                    .Select(x => new
                    {
                        obj = JsonConvert.DeserializeObject<object>(x.Json),
                        addDate = x.AddDate.ToString("dd/MM/yyyy HH:mm"),
                        userSgq_Name = x.UserSgq?.Name ?? "",
                        parReason = x.ParReason?.Motivo ?? "",
                        motivo = x.Motivo ?? ""
                    })
                    .ToList();
                return logs;
            }
        }

        public static IEnumerable<object> GetLogTrack(string table_name, List<int> json_Ids)
        {
            using (var db = new Dominio.SgqDbDevEntities())
            {

                List<object> logTrack = new List<object>();

                var logs = db.LogTrack
                    .Include("UserSgq")
                    .Include("ParReason")
                    .Where(x => x.Tabela == table_name && json_Ids.Contains(x.Json_Id))
                    .OrderByDescending(x => x.AddDate)
                    .ToList()
                    .Select(x => new {
                        obj = JsonConvert.DeserializeObject<object>(x.Json),
                        addDate = x.AddDate.ToString("dd/MM/yyyy HH:mm"),
                        userSgq_Name = x.UserSgq?.Name ?? "",
                        parReason = x.ParReason?.Motivo ?? "",
                        motivo = x.Motivo ?? ""
                    })
                    .ToList();

                foreach (var item in logs)
                {
                    JObject json = item.obj as JObject;

                    if (json != null && int.Parse(json.SelectToken("ParFieldType_Id").ToString()) == 12){

                        var userId = int.Parse(json.SelectToken("Value").ToString());
                        var user = db.UserSgq.Where(x => x.Id == userId).Select(x => x.FullName).FirstOrDefault();
                        json.Add("FullName", user);

                        logTrack.Add(new {
                            obj = JsonConvert.DeserializeObject<object>(json.ToString()),
                            addDate = item.addDate.ToString(),
                            userSgq_Name = item.userSgq_Name ?? "",
                            parReason = item.parReason ?? "",
                            motivo = item.motivo ?? "",
                        });

                    }
                    else
                    {
                        logTrack.Add(json);
                    }

                }

                return logTrack;
            }
        }

    }
}
