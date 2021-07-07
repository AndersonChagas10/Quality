using Conformity.Domain.Core.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Conformity.Domain.Core.Entities.PlanoDeAcao
{


    public partial class ParLevel2 : BaseModel, IEntity
    {
        public int? ParFrequency_Id { get; set; }

        public int? ParDepartment_Id { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(155)]
        [DisplayName("Monitoramento")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Description { get; set; }

        public bool IsEmptyLevel3 { get; set; }

        public bool HasShowLevel03 { get; set; }

        public bool HasGroupLevel3 { get; set; }

        public bool IsActive { get; set; }

        public bool? HasSampleTotal { get; set; }

        public bool HasTakePhoto { get; set; }
    }
}
