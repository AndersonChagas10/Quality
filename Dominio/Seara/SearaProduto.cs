using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Seara
{
    [Table("SearaProduto")]
    public partial class SearaProduto : BaseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ParCompany_Id { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("ParCompany_Id")]
        public virtual ParCompany ParCompany { get; set; }
    }
}
