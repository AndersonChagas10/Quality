using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class PargroupQualificationXParQualification : BaseModel
    {
        public int Id { get; set; }

        public bool IsActive { get; set; }

        public int? PargroupQualification_Id { get; set; }

        public int? ParQualification_Id { get; set; }

        [ForeignKey("ParQualification_Id")]
        public virtual ParQualification ParQualification { get; set; }
    }
}
