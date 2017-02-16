using DTO.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanoAcaoCore
{
    public class Pa_Iniciativas : Pa_BaseObject, ICrudPa<Pa_Iniciativas>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }

        public void IsValid()
        {
            Name = Guard.CheckStringFullSimple(Name);
        }

        public static List<Pa_Iniciativas> Listar()
        {
            List<Pa_Iniciativas> listReturn;
            var query = "SELECT * FROM Pa_Iniciativas";
            using (var db = new FactoryPA(""))
                listReturn = db.SearchQuery<Pa_Iniciativas>(query);

            return listReturn;
        }

        public static Pa_Iniciativas Get(int Id)
        {
            Pa_Iniciativas listReturn;
            var query = "SELECT * FROM Pa_Iniciativas WHERE Id = " + Id;

            using (var db = new FactoryPA(""))
                listReturn = db.SearchQuery<Pa_Iniciativas>(query).FirstOrDefault();

            return listReturn;
        }

        public void AddOrUpdate()
        {
            IsValid();
            string query;
            if (Id > 0)
            {
                query = "";
                Update(query);
            }
            else
            {
                query = "";
                Salvar(query);
            }
        }

    }
}
