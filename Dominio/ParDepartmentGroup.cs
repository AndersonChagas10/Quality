using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    [Table("ParDepartmentGroup")]
    public class ParDepartmentGroup : BaseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        [NotMapped]
        public List<ParDepartment> ParDepartment { get; set; }
    }
}
