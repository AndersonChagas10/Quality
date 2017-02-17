using System;
using System.Collections.Generic;

namespace PlanoAcaoCore
{
    public class Pa_Acao : Pa_BaseObject, ICrudPa<Pa_Acao>
    {
        public int Unidade_Id { get; set; }
        public int Departamento_Id { get; set; }
        public int AcaoXCausaGenericaXGrupoCausaXContramedidaGenerica { get; set; }
        public int AcaoXQuem { get; set; }
        public DateTime QuandoInicio { get; set; }
        public int DuracaoDias { get; set; }
        public DateTime QuandoFim { get; set; }
        public string ComoPontosimportantes { get; set; }
        public int Predecessora_Id { get; set; }
        public string PraQue { get; set; }
        public decimal QuantoCusta { get; set; }
        public int Status { get; set; }
        public int Panejamento_Id { get; set; }

        public void IsValid()
        {
            //Name = Guard.CheckStringFullSimple(Name);
        }

        public static List<Pa_Acao> Listar()
        {
            var query = "SELECT * FROM Pa_Acao";
            return ListarGenerico<Pa_Acao>(query);
        }

        public static Pa_Acao Get(int Id)
        {
            var query = "SELECT * FROM Pa_Acao WHERE Id = " + Id;
            return GetGenerico<Pa_Acao>(query);
        }

        public void AddOrUpdate()
        {
            //IsValid();
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
