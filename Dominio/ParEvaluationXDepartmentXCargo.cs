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
    public class ParEvaluationXDepartmentXCargo : BaseModel
    {
        [Key]
        public int Id { get; set; }
        public int? ParCargo_Id { get; set; }
        public int ParDepartment_Id { get; set; }
        public int? ParCompany_Id { get; set; }

        public int Evaluation { get; set; }
        public int Sample { get; set; }

        public int? ParFrequencyId { get; set; }

        [DisplayName("Cluster")]
        [Index(IsUnique = true)]
        public int? ParCluster_Id { get; set; }

        [NotMapped]
        public bool IsAgendamento { get; set; }

        public bool IsActive { get; set; }

        public bool IsParcialCollection { get; set; }

        [ForeignKey("ParCargo_Id")]
        public virtual ParCargo ParCargo { get; set; }

        [ForeignKey("ParDepartment_Id")]
        public virtual ParDepartment ParDepartment { get; set; }

        [ForeignKey("ParCompany_Id")]
        public virtual ParCompany ParCompany { get; set; }

        [ForeignKey("ParCluster_Id")]
        public virtual ParCluster ParCluster { get; set; }

        [NotMapped]
        public virtual List<ParEvaluationSchedule> ParEvaluationSchedule { get; set; }
    }
}
