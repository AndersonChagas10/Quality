using ADOFactory;
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
            var query = "SELECT * FROM Pa_ObjetivoGeral";
            return ListarGenerico<Pa_ObjetivoGerais>(query);
        }

        public static Pa_ObjetivoGerais Get(int Id)
        {
            var query = "SELECT * FROM Pa_ObjetivoGeral WHERE Id = " + Id;
            return GetGenerico<Pa_ObjetivoGerais>(query);
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
