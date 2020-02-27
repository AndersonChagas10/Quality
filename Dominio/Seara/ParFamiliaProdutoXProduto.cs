using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Seara
{
    [Table("ParFamiliaProdutoXParProduto")]
    public class ParFamiliaProdutoXParProduto : BaseModel
    {
        public int Id { get; set; }

        public int? ParFamiliaProduto_Id { get; set; }
        public int? ParProduto_Id { get; set; }
        public int? ParCompany_Id { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("ParCompany_Id")]
        public virtual ParCompany ParCompany { get; set; }

        [ForeignKey("ParProduto_Id")]
        public virtual ParProduto ParProduto { get; set; }
    }
}
