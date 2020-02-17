using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Seara
{
    [Table("SearaFamiliaProdutoXProduto")]
    public class SearaFamiliaProdutoXProduto : BaseModel
    {
        public int Id { get; set; }

        public int? SearaFamiliaProduto_Id { get; set; }
        public int? SearaProduto_Id { get; set; }
        public int? ParCompany_Id { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("ParCompany_Id")]
        public virtual ParCompany ParCompany { get; set; }

        [ForeignKey("SearaProduto_Id")]
        public virtual SearaProduto SearaProduto { get; set; }
    }
}
