using ADOFactory;
using DTO.Helpers;
using System.Collections.Generic;
using System.Linq;

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
            var query = "SELECT * FROM Pa_Indicadores";
            return ListarGenerico<Pa_Indicadores>(query);
        }

        public static Pa_Indicadores Get(int Id)
        {
            var query = "SELECT * FROM Pa_Indicadores WHERE Id = " + Id;
            return GetGenerico<Pa_Indicadores>(query);
        }

        public void AddOrUpdate()
        {
            IsValid();
            //string query;
            //if (Id > 0)
            //{
            //    query = "";
            //    Update(query);
            //}
            //else
            //{
            //    query = "";
            //    Salvar(query);
            //}
        }
      
    }
}
