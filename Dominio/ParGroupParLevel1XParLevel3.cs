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
    public class ParGroupParLevel1XParLevel3 : BaseModel
    {
        public int Id { get; set; }

        [DisplayName("Vinculo Grupo Indicador x Tarefa")]
        public string Name { get; set; }

        [DisplayName("Grupo de Tipo de Indicador")]
        public int? ParGroupParLevel1Type_Id { get; set; }

        [DisplayName("Monitoramento")]
        public int? ParLevel3_Id { get; set; }

        [DisplayName("Indicador")]
        public int? ParLevel1_Id { get; set; }

        [DisplayName("Está Ativo")]
        public bool IsActive { get; set; }

        [ForeignKey("ParGroupParLevel1Type_Id")]
        public virtual ParGroupParLevel1Type ParGroupParLevel1Type { get; set; }

        [ForeignKey("ParLevel3_Id")]
        public virtual ParLevel3 ParLevel3 { get; set; }

        [ForeignKey("ParLevel1_Id")]
        public virtual ParLevel1 ParLevel1 { get; set; }
    }
}
