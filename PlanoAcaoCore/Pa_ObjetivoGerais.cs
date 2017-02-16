using DTO.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace PlanoAcaoCore
{
    public class Pa_ObjetivoGerais : Pa_BaseObject, ICrudPa<Pa_ObjetivoGerais>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }

        public void IsValid()
        {
            Name = Guard.CheckStringFullSimple(Name);
        }

        public static List<Pa_ObjetivoGerais> Listar()
        {
            List<Pa_ObjetivoGerais> listReturn;
            var query = "SELECT * FROM Pa_ObjetivoGerais";
            using (var db = new FactoryPA(""))
                listReturn = db.SearchQuery<Pa_ObjetivoGerais>(query);

            return listReturn;
        }

        public static Pa_ObjetivoGerais Get(int Id)
        {
            Pa_ObjetivoGerais listReturn;
            var query = "SELECT * FROM Pa_ObjetivoGerais WHERE Id = " + Id;

            using (var db = new FactoryPA(""))
                listReturn = db.SearchQuery<Pa_ObjetivoGerais>(query).FirstOrDefault();

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
