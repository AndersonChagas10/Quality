using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class ParLevel3XHelp : BaseModel
    {
        [Key]
        public int Id { get; set; }
        public int ParLevel3_Id { get; set; }
        public string Titulo { get; set; }
        public string Corpo { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("ParLevel3_Id")]
        public virtual ParLevel3 ParLevel3 { get; set; }
    }
}
