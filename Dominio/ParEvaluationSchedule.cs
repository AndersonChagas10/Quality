using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    [Table("ParEvaluationSchedule")]
    public partial class ParEvaluationSchedule : BaseModel
    {
        [Key]
        public int Id { get; set; }
        public string Inicio { get; set; }
        public string Fim { get; set; }
        public int Av { get; set; }
        public int? Shift_Id { get; set; }
        public int? ParEvaluation_Id { get; set; }
        public bool IsActive { get; set; }
        public string Intervalo { get; set; }
        public int? ParEvaluationXDepartmentXCargo_Id { get; set; }

        [NotMapped]
        public bool isDeletar { get; set; }

        [ForeignKey("ParEvaluation_Id")]
        public virtual ParEvaluation ParEvaluation { get; set; }
    }
}
