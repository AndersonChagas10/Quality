using DTO.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanoAcaoCore
{
    public class Pa_Indicadores : Pa_BaseObject, ICrudPa<Pa_Indicadores>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }

        public void IsValid()
        {
            Name = Guard.CheckStringFullSimple(Name);
        }

        public static List<Pa_Indicadores> Listar()
        {
            List<Pa_Indicadores> listReturn;
            var query = "SELECT * FROM Pa_Indicadores";
            using (var db = new FactoryPA(""))
                listReturn = db.SearchQuery<Pa_Indicadores>(query);

            return listReturn;
        }

        public static Pa_Indicadores Get(int Id)
        {
            Pa_Indicadores listReturn;
            var query = "SELECT * FROM Pa_Indicadores WHERE Id = " + Id;

            using (var db = new FactoryPA(""))
                listReturn = db.SearchQuery<Pa_Indicadores>(query).FirstOrDefault();

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
