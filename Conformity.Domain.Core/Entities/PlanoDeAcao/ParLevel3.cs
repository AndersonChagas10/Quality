using Conformity.Domain.Core.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Conformity.Domain.Core.Entities.PlanoDeAcao
{
    [Table("ParLevel3")]
    public partial class ParLevel3 : BaseModel, IEntity
    {
        [Required(AllowEmptyStrings = true)]
        [StringLength(1000)]
        [DisplayName("Tarefa")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(1000)]
        public string Description { get; set; }

        public int? ParRiskCharacteristicType_Id { get; set; }

        public bool IsActive { get; set; }

        public bool HasTakePhoto { get; set; }

        public bool? IsPointLess { get; set; }

        public bool? AllowNA { get; set; }

        public int? OrderColumn { get; set; }

        [NotMapped]
        public string IdAndName
        {
            get
            {
                if (Id > 0)
                    return Id + " - " + Name;
                else
                    return "";
            }
            protected internal set
            {

            }
        }
    }
}
