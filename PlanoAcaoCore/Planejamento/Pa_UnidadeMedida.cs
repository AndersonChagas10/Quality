using DTO.Helpers;
using System.Collections.Generic;

namespace PlanoAcaoCore
{
    public class Pa_UnidadeMedida : Pa_BaseObject, ICrudPa<Pa_ObjetivoGeral>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }

        public void IsValid()
        {
            Name = Guard.CheckStringFullSimple(Name);
        }

        public static List<Pa_UnidadeMedida> Listar()
        {
            var query = "SELECT * FROM Pa_UnidadeMedida";
            return ListarGenerico<Pa_UnidadeMedida>(query);
        }

        public static Pa_UnidadeMedida Get(int Id)
        {
            var query = "SELECT * FROM Pa_UnidadeMedida WHERE Id = " + Id;
            return GetGenerico<Pa_UnidadeMedida>(query);
        }

        public void AddOrUpdate()
        {
            IsValid();
        }

    }
}
