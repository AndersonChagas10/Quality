﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    [Table("ParVinculoPeso")]
    public class ParVinculoPeso : BaseModel
    {
        public int Id { get; set; }

        public int Peso { get; set; }

        [DisplayName("Está Ativo")]
        public bool IsActive { get; set; }

        [DisplayName("Vinculo Grupo Indicador x Tarefa")]
        public string Name { get; set; }

        [DisplayName("Tarefa")]
        public int? ParLevel3_Id { get; set; }

        [DisplayName("Empresa")]
        public int? ParCompany_Id { get; set; }

        [DisplayName("Grupo Indicadores")]
        public int? ParGroupParLevel1_Id { get; set; }

        [DisplayName("Departamento")]
        public int? ParDepartment_Id { get; set; }

        [DisplayName("Indicador")]
        public int? ParLevel1_Id { get; set; }

        [DisplayName("Monitoramento")]
        public int? ParLevel2_Id { get; set; }

        [ForeignKey("ParLevel3_Id")]
        public virtual ParLevel3 ParLevel3 { get; set; }

        [ForeignKey("ParLevel2_Id")]
        public virtual ParLevel2 ParLevel2 { get; set; }

        [ForeignKey("ParLevel1_Id")]
        public virtual ParLevel1 ParLevel1 { get; set; }

        [ForeignKey("ParCompany_Id")]
        public virtual ParCompany ParCompany { get; set; }

        [ForeignKey("ParDepartment_Id")]
        public virtual ParDepartment ParDepartment { get; set; }

        [ForeignKey("ParGroupParLevel1_Id")]
        public virtual ParGroupParLevel1 ParGroupParLevel1 { get; set; }
    }
}
