using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    [Table("ParGroupParLevel1Type")]
    public class ParGroupParLevel1Type : BaseModel
    {
        public int Id { get; set; }

        [DisplayName("Tipo Indicador")]
        public string Name { get; set; }

        [DisplayName("Está Ativo")]
        public bool IsActive { get; set; }
    }
}
