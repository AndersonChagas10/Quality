using System;

namespace PlanoAcaoCore
{
    public class Pa_BaseObject
    {
        public int Id { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime AlterDate { get; set; }


        public void Update(string query)
        {
            using (var db = new FactoryPA(""))
                db.ExecuteSql(query);
        }

        public void Salvar(string query)
        {
            using (var db = new FactoryPA(""))
                Id = db.ExecuteSql(query);
        }
    }
}