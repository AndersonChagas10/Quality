using DTO.BaseEntity;

namespace DTO.DTO.Params
{
    public class ParCompanyStructureDTO : EntityBase
    {
        public int ParStructure_Id { get; set; }
        public int ParCompany_Id { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
