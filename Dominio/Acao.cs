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
        public int Acao_Naoconformidade { get; set; }
        public int AcaoText { get; set; }
        public int HoraConclusao { get; set; }
        public int Referencia { get; set; }
        public int Responsavel { get; set; }
        public int Notificar { get; set; }
        public int DataEmissao { get; set; }
        public int HoraEmissao { get; set; }
        public int Emissor { get; set; }
        public int EvidenciaNaoConformidade { get; set; }
        public int EvidenciaAcaoConcluida { get; set; } 
    }
}
