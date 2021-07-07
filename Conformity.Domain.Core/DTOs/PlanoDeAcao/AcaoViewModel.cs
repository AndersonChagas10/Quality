using System;
using System.Collections.Generic;

namespace Conformity.Domain.Core.DTOs
{
    public class AcaoViewModel
    {
        public int Id { get; set; }
        public int ParLevel1_Id { get; set; }
        public string ParLevel1_Name { get; set; }
        public int ParLevel2_Id { get; set; }
        public string ParLevel2_Name { get; set; }
        public int ParLevel3_Id { get; set; }
        public string ParLevel3_Name { get; set; }
        public int ParCompany_Id { get; set; }
        public string ParCompany_Name { get; set; }
        public int ParDepartment_Id { get; set; }
        public string ParDepartment_Name { get; set; }
        public int ParDepartmentParent_Id { get; set; }
        public string ParDepartmentParent_Name { get; set; }
        public string ParCargo_Name { get; set; }
        public int ParCargo_Id { get; set; }
        public string Acao_Naoconformidade { get; set; }
        public string AcaoText { get; set; }
        public DateTime? DataEmissao { get; set; }
        public string _DataEmissao { get { return DataEmissao?.ToShortDateString(); } }
        public DateTime? DataConclusao { get; set; }
        public string _DataConclusao { get { return DataConclusao?.ToShortDateString(); } }
        public TimeSpan HoraEmissao { get; set; }
        public TimeSpan HoraConclusao { get; set; }
        public string Referencia { get; set; }
        public string Responsavel { get; set; }
        public string Notificar { get; set; }
        public int Emissor { get; set; }
        public string EmissorNome { get; set; }
        public int UsuarioLogado { get; set; }
        public int ParCluster_Id { get; set; }
        public int ParClusterGroup_Id { get; set; }

        public List<EvidenciaViewModel> ListaEvidencia { get; set; }

        public List<EvidenciaViewModel> ListaEvidenciaConcluida { get; set; }

        public List<AcaoXNotificarAcao> ListaNotificarAcao { get; set; }

        public List<string> EvidenciaNaoConformidade { get; set; }

        public List<string> EvidenciaAcaoConcluida { get; set; }
        public string Prioridade { get; set; }
        public int Status { get; set; }
        public bool IsActive { get; set; }
        public string Responsavel_Name { get; set; }
        public bool PermiteEditar { get => Emissor == UsuarioLogado && Status == (int)EAcaoStatus.Pendente || Status == (int)EAcaoStatus.Em_Andamento; }
    }
}
