using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Seara
{
    public class CollectionLevel2XParFamiliaProdutoXParProduto : BaseModel
    {
        [Key]
        public int Id { get; set; }

        public int CollectionLevel2_Id { get; set; }

        public int ParFamiliaProduto_Id { get; set; }
        public int? ParProduto_Id { get; set; }
    }
}
