using DTO.BaseEntity;

namespace DTO.DTO.Params
{
    public class ParCompanyXStructureDTO : EntityBase
    {
        public int ParStructure_Id { get; set; }
        public int ParCompany_Id { get; set; }
        public bool Active { get; set; } = true;

        public ParStructureDTO ParStructure { get; set; }

    }
}
