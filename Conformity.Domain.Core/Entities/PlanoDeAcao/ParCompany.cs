using Conformity.Domain.Core.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Conformity.Domain.Core.Entities.PlanoDeAcao
{

    [Table("ParCompany")]
    public class ParCompany : BaseModel, IEntity
    {
        [Required(AllowEmptyStrings = true)]
        [StringLength(155)]
        [DisplayName("Empresa")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        [StringLength(50)]
        public string Initials { get; set; }

        [StringLength(155)]
        public string SIF { get; set; }

        public int? CompanyNumber { get; set; }

        [StringLength(155)]
        public string IPServer { get; set; }

        [StringLength(155)]
        public string DBServer { get; set; }

        public string Identification { get; set; }

        public decimal? IntegrationId { get; set; }

        public int? ParCompany_Id { get; set; }
    }
}
