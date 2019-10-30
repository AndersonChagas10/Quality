using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogSystem
{
    public static class LogTrackBusiness
    {

        public static void Register(object obj, int json_id, string table_name, int userSgq_Id)
        {
            using (var db = new Dominio.SgqDbDevEntities())
            {
                Dominio.LogTrack logTrack = new Dominio.LogTrack();
                logTrack.Json_Id = json_id;
                logTrack.Json = JsonConvert.SerializeObject(obj);
                logTrack.Tabela = table_name;
                logTrack.UserSgq_Id = userSgq_Id;
                logTrack.AddDate = DateTime.Now;
                db.LogTrack.Add(logTrack);
                db.SaveChanges();
            }
        }

    }
}
