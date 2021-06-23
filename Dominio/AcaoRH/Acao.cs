﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using static Dominio.Enums.Enums;

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
        public int ParCluster_Id { get; set; }
        public int ParClusterGroup_Id { get; set; }
        public string Acao_Naoconformidade { get; set; }
        public string AcaoText { get; set; }
        public TimeSpan? HoraConclusao { get; set; }
        public DateTime? DataConclusao { get; set; }
        public string Referencia { get; set; }
        public int? Responsavel { get; set; }
        public List<int> Notificar { get; set; }
        public DateTime? DataEmissao { get; set; }
        public TimeSpan? HoraEmissao { get; set; }
        public int Emissor { get; set; }
        public int? Prioridade { get; set; }
        public string PrioridadeText { get; set; }
        public EAcaoStatus Status { get; set; }
        public bool IsActive { get; set; }

        public IEnumerable<string> EvidenciaNaoConformidade { get; set; }
        public IEnumerable<string> EvidenciaAcaoConcluida { get; set; }

        [NotMapped]
        public ParLevel1 ParLevel1 { get; set; }

        [NotMapped]
        public ParLevel2 ParLevel2 { get; set; }

        [NotMapped]
        public ParLevel3 ParLevel3 { get; set; }

        [NotMapped]
        public ParCargo ParCargo { get; set; }

        [NotMapped]
        public ParCompany ParCompany { get; set; }

        [NotMapped]
        public ParDepartment ParDepartment { get; set; }

        [NotMapped]
        public ParDepartment ParDepartmentParent { get; set; }

        [NotMapped]
        public UserSgq ResponsavelUser { get; set; }

        [NotMapped]
        public UserSgq EmissorUser { get; set; }

        [NotMapped]
        public ParCluster ParCluster { get; set; }

        [NotMapped]
        public ParClusterGroup ParClusterGroup { get; set; }
        public IEnumerable<UserSgq> NotificarUsers { get; set; }
    }
}
