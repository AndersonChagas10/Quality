using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanoAcaoCore
{
    public class Pa_Planejamento : Pa_BaseObject, ICrudPa<Pa_Planejamento>
    {
       public int Diretoria_Id { get; set; }
       public int Gerencia_Id { get; set; }
       public int Coordenacao_Id { get; set; }
       public int Missao_Id { get; set; }
       public int Visao_Id { get; set; }
       public int TemaAssunto_Id { get; set; }
       public int Indicadores_Id { get; set; }
       public int Iniciativa_Id { get; set; }
       public int ObjetivoGerencial_Id { get; set; }
       public string Dimensao { get; set; }
       public string Objetivo { get; set; }
       public decimal ValorDe { get; set; }
       public decimal ValorPara { get; set; }
       public DateTime DataInicio { get; set; }
       public DateTime DataFim { get; set; }

        public void IsValid()
        {
            //Name = Guard.CheckStringFullSimple(Name);
        }

        public static List<Pa_Planejamento> Listar()
        {
            List<Pa_Planejamento> listReturn;
            var query = "SELECT * FROM Pa_Planejamento";
            using (var db = new FactoryPA(""))
                listReturn = db.SearchQuery<Pa_Planejamento>(query);

            return listReturn;
        }

        public static Pa_Planejamento Get(int Id)
        {
            Pa_Planejamento listReturn;
            var query = "SELECT * FROM Pa_Planejamento WHERE Id = "  + Id;

            using (var db = new FactoryPA(""))
                listReturn = db.SearchQuery<Pa_Planejamento>(query).FirstOrDefault();

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
