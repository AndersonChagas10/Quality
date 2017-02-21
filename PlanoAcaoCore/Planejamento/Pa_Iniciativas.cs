using ADOFactory;
using DTO.Helpers;
using System.Collections.Generic;
using System.Linq;

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
            var query = "SELECT * FROM Pa_Iniciativa";
            return ListarGenerico<Pa_Iniciativas>(query);
        }

        public static Pa_Iniciativas Get(int Id)
        {
            var query = "SELECT * FROM Pa_Iniciativa WHERE Id = " + Id;
            return GetGenerico<Pa_Iniciativas>(query);
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
