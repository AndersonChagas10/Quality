using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Seara
{
    public class ParProdutoResultSet : BaseModel
    {
        public int? ParProduto_Id { get; set; }
        public string ParProduto { get; set; }

    }
}
