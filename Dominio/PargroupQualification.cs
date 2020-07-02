using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class PargroupQualification : BaseModel
    {
        public PargroupQualification()
        {
            PargroupQualificationXParQualification = new HashSet<PargroupQualificationXParQualification>();
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        [NotMapped]
        public virtual ICollection<PargroupQualificationXParQualification> PargroupQualificationXParQualification { get; set; }
    }
}
