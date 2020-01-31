using System;
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

        public decimal? Peso { get; set; }

        [DisplayName("Está Ativo")]
        public bool IsActive { get; set; }

        [DisplayName("Nome Vinculo")]
        public string Name { get; set; }

        [DisplayName("Tarefa")]
        [Index(IsUnique = true)]
        public int? ParLevel3_Id { get; set; }

        [DisplayName("Empresa")]
        [Index(IsUnique = true)]
        public int? ParCompany_Id { get; set; }

        [DisplayName("Grupo Indicadores")]
        [Index(IsUnique = true)]
        public int? ParGroupParLevel1_Id { get; set; }

        [DisplayName("Departamento")]
        [Index(IsUnique = true)]
        public int? ParDepartment_Id { get; set; }

        [DisplayName("Indicador")]
        [Index(IsUnique = true)]
        public int? ParLevel1_Id { get; set; }

        [DisplayName("Monitoramento")]
        [Index(IsUnique = true)]
        public int? ParLevel2_Id { get; set; }

        [DisplayName("Cluster")]
        [Index(IsUnique = true)]
        public int? ParCluster_Id { get; set; }

        [DisplayName("Grupo")]
        //[Index(IsUnique = true)]
        public int? ParLevel3Group_Id { get; set; }

        public int? ParFrequencyId { get; set; }

        public int? Evaluation { get; set; }

        public int? Sample { get; set; }
         
        [DisplayName("Cargo")]
        //[Index(IsUnique = true)]
        public int? ParCargo_Id { get; set; }

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

        [ForeignKey("ParLevel3Group_Id")]
        public virtual ParLevel3Group ParLevel3Group { get; set; }

        [ForeignKey("ParCargo_Id")]
        public virtual ParCargo ParCargo { get; set; }

        [ForeignKey("ParCluster_Id")]
        public virtual ParCluster ParCluster { get; set; }
    }
}
