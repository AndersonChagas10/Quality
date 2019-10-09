using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    [Table("ParDepartmentXRotinaIntegracao")]
    public class ParDepartmentXRotinaIntegracao : BaseModel
    {
        public int Id { get; set; }

        [Column("Name")]
        public string NameRotina { get; set; }

        public int? ParDepartment_Id { get; set; }

        public int? RotinaIntegracao_Id { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("ParDepartment_Id")]
        public virtual ParDepartment ParDepartment { get; set; }

        [ForeignKey("RotinaIntegracao_Id")]
        public virtual RotinaIntegracao RotinaIntegracao { get; set; }
    }
}
