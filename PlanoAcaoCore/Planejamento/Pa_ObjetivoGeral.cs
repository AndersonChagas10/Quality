using DTO.Helpers;
using System.Collections.Generic;

namespace PlanoAcaoCore
{
    public class Pa_ObjetivoGeral : Pa_BaseObject, ICrudPa<Pa_ObjetivoGeral>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public int? Pa_IndicadoresDeProjeto_Id { get; set; }
        
        public void IsValid()
        {
            Name = Guard.CheckStringFullSimple(Name);
        }

        public static List<Pa_ObjetivoGeral> Listar()
        {
            var query = "SELECT * FROM Pa_ObjetivoGeral WHERE IsActive = 1";
            return ListarGenerico<Pa_ObjetivoGeral>(query);
        }

        public static Pa_ObjetivoGeral Get(int Id)
        {
            var query = "SELECT * FROM Pa_ObjetivoGeral WHERE Id = " + Id;
            return GetGenerico<Pa_ObjetivoGeral>(query);
        }

        public static List<Pa_ObjetivoGeral> GetObjetivoXIndicadoresProjeto(int id)
        {
            var query = "SELECT * FROM Pa_ObjetivoGeral WHERE IsActive = 1 AND  Pa_IndicadoresDeProjeto_Id = " + id;
            return ListarGenerico<Pa_ObjetivoGeral>(query);
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
