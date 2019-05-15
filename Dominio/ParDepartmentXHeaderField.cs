using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class ParDepartmentXHeaderField : BaseModel
    {
        public int Id { get; set; }

        public int ParDepartment_Id { get; set; }

        public int ParHeaderField_Id { get; set; }

        public bool IsActive { get; set; }

        public bool? IsRequired { get; set; }

        public bool? DefaultSelected { get; set; }

        [StringLength(100)]
        public string HeaderFieldGroup { get; set; }

        [ForeignKey("ParHeaderField_Id")]
        public virtual ParHeaderField ParHeaderField { get; set; }

        [ForeignKey("ParDepartment_Id")]
        public virtual ParDepartment ParDepartment { get; set; }
    }
}
