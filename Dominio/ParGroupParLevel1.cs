using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class ParGroupParLevel1 : BaseModel
    {
        public int Id { get; set; }

        [DisplayName("Nome")]
        public string Name { get; set; }

        [DisplayName("Grupo de Tipo de Indicador")]
        public int? ParGroupParLevel1Type_Id { get; set; }


        [DisplayName("Está Ativo")]
        public bool IsActive { get; set; }

        [ForeignKey("ParGroupParLevel1Type_Id")]
        public virtual ParGroupParLevel1Type ParGroupParLevel1Type { get; set; }
    }
}
