using System;
using System.Collections.Generic;
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

        public bool IsActive { get; set; }

        [ForeignKey("ParCargo_Id")]
        public virtual ParCargo ParCargo { get; set; }

        [ForeignKey("ParDepartment_Id")]
        public virtual ParDepartment ParDepartment { get; set; }

        [ForeignKey("ParCompany_Id")]
        public virtual ParCompany ParCompany { get; set; }

        [NotMapped]
        public virtual List<ParEvaluationSchedule> ParEvaluationSchedule { get; set; }
    }
}
