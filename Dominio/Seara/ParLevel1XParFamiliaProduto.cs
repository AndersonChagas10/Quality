using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Seara
{
    public class ParLevel1XParFamiliaProduto : BaseModel
    {
        public int Id { get; set; }

        public int ParLevel1_Id { get; set; }

        public int ParFamiliaProduto_Id { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("ParLevel1_Id")]
        public ParLevel1 ParLevel1 { get; set; }

        [ForeignKey("ParFamiliaProduto_Id")]
        public ParFamiliaProduto ParFamiliaProduto { get; set; }

    }
}
