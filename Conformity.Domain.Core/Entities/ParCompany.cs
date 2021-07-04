namespace Conformity.Domain.Core.Entities
{
    using Conformity.Domain.Core.Interfaces;

    public partial class ParCompany : BaseModel, IEntity
    {
        public ParCompany()
        {
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public string Initials { get; set; }

        public string SIF { get; set; }

        public int? CompanyNumber { get; set; }

        public string IPServer { get; set; }

        public string DBServer { get; set; }

        public string Identification { get; set; }

        public decimal? IntegrationId { get; set; }

        public int? ParCompany_Id { get; set; }
    }
}
