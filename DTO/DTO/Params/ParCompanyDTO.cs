using DTO.BaseEntity;

namespace DTO.DTO.Params
{
    public class ParCompanyDTO : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ParMultipleValues_Id { get; set; }
        public bool IsActive { get; set; } = true;
        public string Initials { get; set; }
        public int SIF { get; set; }
        public int CompanyNumber { get; set; }
        public string IPServer { get; set; }
        public string DBServer { get; set; }
    }
}