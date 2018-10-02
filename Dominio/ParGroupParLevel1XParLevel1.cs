using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class ParGroupParLevel1XParLevel1 : BaseModel
    {
        public int Id { get; set; }

        [DisplayName("Indicador")]
        public int? ParLevel1_Id { get; set; }

        [DisplayName("Grupo de Tipo de Indicador")]
        public int? ParGroupParLevel1_Id { get; set; }

        [DisplayName("Está Ativo")]
        public bool IsActive { get; set; }

        [ForeignKey("ParLevel1_Id")]
        public virtual ParLevel1 ParLevel1 { get; set; }

        [ForeignKey("ParGroupParLevel1_Id")]
        public virtual ParGroupParLevel1 ParGroupParLevel1 { get; set; }
    }
}
