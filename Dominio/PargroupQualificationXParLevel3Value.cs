using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class PargroupQualificationXParLevel3Value : BaseModel
    {
        public int Id { get; set; }

        public int PargroupQualification_Id { get; set; }

        public int ParLevel3Value_Id { get; set; }

        public string Value { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("PargroupQualification_Id")]
        public virtual PargroupQualification PargroupQualification { get; set; }

        [ForeignKey("ParLevel3Value_Id")]
        public virtual ParLevel3Value ParLevel3Value { get; set; }
    }
}
