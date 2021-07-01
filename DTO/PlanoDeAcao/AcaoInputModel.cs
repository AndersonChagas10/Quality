using Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DTO.PlanoDeAcao
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
        public string Status { get; set; }
    }
}
