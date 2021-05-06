using System;

namespace Dominio
{
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
        public string DataConclusao { get; set; }
        public string Referencia { get; set; }
        public int? Responsavel { get; set; }
        public int? Notificar { get; set; }
        public string DataEmissao { get; set; }
        public string HoraEmissao { get; set; }
        public int Emissor { get; set; }
        public string EvidenciaNaoConformidade { get; set; }
        public string EvidenciaAcaoConcluida { get; set; } 
    }
}
