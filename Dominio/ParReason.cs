using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class ParReason : BaseModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Motivo { get; set; }

        public bool IsActive { get; set; }

        public int ParReasonType_Id { get; set; }

        [ForeignKey("ParReasonType_Id")]
        public ParReasonType ParReasonType { get; set; }

    }
}
