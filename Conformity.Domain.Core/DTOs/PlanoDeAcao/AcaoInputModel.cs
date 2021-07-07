using System;
using System.Collections.Generic;

namespace Conformity.Domain.Core.DTOs
{
    public class AcaoInputModel
    {
        public int Id { get; set; }
        public string Acao_Naoconformidade { get; set; }
        public string AcaoText { get; set; }
        public DateTime? DataConclusao { get; set; }
        public TimeSpan HoraConclusao { get; set; }
        public string Referencia { get; set; }
        public string Responsavel { get; set; }

        public List<EvidenciaViewModel> ListaEvidencia { get; set; }

        public List<EvidenciaViewModel> ListaEvidenciaConcluida { get; set; }

        public List<AcaoXNotificarAcao> ListaNotificarAcao { get; set; }

        public string Prioridade { get; set; }
        public int UsuarioLogado { get; set; }

        private EAcaoStatus _status;
        public EAcaoStatus Status
        {
            get
            {
                return _status;
            }
            set
            {
                if (!DentroDoPrazo())
                {
                    _status = EAcaoStatus.Atrasada;
                }
                else if (value == EAcaoStatus.Em_Andamento && DentroDoPrazo())
                {
                    _status = EAcaoStatus.Em_Andamento;
                }
                else
                {
                    _status = value;
                }
            }
        }


        public bool DentroDoPrazo()
        {
            DateTime Hoje = DateTime.Now;
            if (Hoje < DataConclusao)
            {
                return true;
            }
            else if (Hoje == DataConclusao
                && Hoje.Hour <= HoraConclusao.Hours
                && Hoje.Minute < HoraConclusao.Minutes)
            {
                return true;
            }
            return false;
        }
    }
}
