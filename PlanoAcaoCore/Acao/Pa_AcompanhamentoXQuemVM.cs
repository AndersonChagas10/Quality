using System.Collections.Generic;

namespace PlanoAcaoCore.Acao
{
    public class Pa_AcompanhamentoXQuemVM : Pa_BaseObject
    {
        public int Acompanhamento_Id { get; set; }
        public int Quem_Id { get; set; }

        public static List<Pa_AcompanhamentoXQuemVM> GetByAcompanhamentoId(int Acompanhamento_Id)
        {
            var query = "select * from Pa_AcompanhamentoXQuem where Acompanhamento_Id = " + Acompanhamento_Id;
            return ListarGenerico<Pa_AcompanhamentoXQuemVM>(query);
        }
    }    
}
