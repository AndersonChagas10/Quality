using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanoAcaoCore.Acao
{
    public class Pa_Acompanhamento : Pa_BaseObject
    {
        public string Description { get; set; }
        public int Acao_Id { get; set; }
        public int? Order { get; set; }
        public int Status_Id { get; set; }
        public int MailTo { get; set; }
        public string Name { get; set; }
        public int Author_Id { get; set; }

        public static List<Pa_Acompanhamento> Listar()
        {
            var query = "SELECT * FROM Pa_Acompanhamento";
            return ListarGenerico<Pa_Acompanhamento>(query);
        }

        public static Pa_Acompanhamento Get(int Id)
        {
            var query = "SELECT * FROM Pa_Acompanhamento WHERE Id = " + Id;
            return GetGenerico<Pa_Acompanhamento>(query);
        }

        public static List<Pa_Acompanhamento> GetByAcaoId(int Acao_Id)
        {
            var query = "SELECT * FROM Pa_Acompanhamento Where Acao_Id = " + Acao_Id;
            return ListarGenerico<Pa_Acompanhamento>(query);
        }

        public Pa_Status _Status
        {
            get
            {
                if (Status_Id >= 0)
                    return Pa_Status.Get(Status_Id);
                else
                    return new Pa_Status();
            }
        }

        public List<int> _MailTo { get; set; }

        public List<Pa_Quem> _Quem
        {
            get
            {
                var PQP = new List<Pa_Quem>();
                if (_MailTo != null)
                {
                    if (_MailTo.Count > 0)
                    {
                        foreach (var i in _MailTo)
                        {
                            PQP.Add(Pa_Quem.Get(i));
                        }
                    }
                }
                return PQP;
            }
        }
    }
}
