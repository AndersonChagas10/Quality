using DTO.BaseEntity;

namespace DTO.DTO.Params
{
    public class ParSampleDTO : EntityBase
    {
        public int ParCompany_Id { get; set; }
        public int ParLevel2_Id { get; set; }
        public int Number { get; set; }
        public bool IsActive { get; set; } = true;
    }
}