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
        public int Inicio { get; set; }
        public int Fim { get; set; }
        public int Av { get; set; }
        public int? Shift_Id { get; set; }
        public int ParEvaluation_Id { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("ParEvaluation_Id")]
        public virtual ParEvaluation ParEvaluation { get; set; }
    }
}
