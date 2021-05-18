using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio
{
    [Table("PA.Acao")]
    public class Acao
    {
        public int Id { get; set; }
        public int ParLevel1_Id { get; set; }
        public int ParLevel2_Id { get; set; }
        public int ParLevel3_Id { get; set; }
        public int ParCompany_Id { get; set; }
        public int ParDepartment_Id { get; set; }
        public int ParDepartmentParent_Id { get; set; }
        public int ParCargo_Id { get; set; }
        public string Acao_Naoconformidade { get; set; }
        public string AcaoText { get; set; }
        public string HoraConclusao { get; set; }
        public DateTime? DataConclusao { get; set; }
        public string Referencia { get; set; }
        public int? Responsavel { get; set; }
        public int? Notificar { get; set; }
        public DateTime? DataEmissao { get; set; }
        public string HoraEmissao { get; set; }
        public int Emissor { get; set; }
        public int Prioridade { get; set; }
        //public string EvidenciaNaoConformidade { get; set; }
        //public string EvidenciaAcaoConcluida { get; set; }

        [ForeignKey("ParLevel1_Id")]
        public ParLevel1 ParLevel1 { get; set; }

        [ForeignKey("ParLevel2_Id")]
        public ParLevel2 ParLevel2 { get; set; }

        [ForeignKey("ParLevel3_Id")]
        public ParLevel3 ParLevel3 { get; set; }

        [ForeignKey("ParCargo_Id")]
        public ParCargo ParCargo { get; set; }

        [ForeignKey("ParCompany_Id")]
        public ParCompany ParCompany { get; set; }

        [ForeignKey("ParDepartment_Id")]
        public ParDepartment ParDepartment { get; set; }

        [NotMapped]
        public List<string> EvidenciaNaoConformidade { get; set; }

        [NotMapped] 
        public List<string> EvidenciaAcaoConcluida { get; set; }
    }
}
