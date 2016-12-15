using DTO.BaseEntity;

namespace DTO.DTO.Params
{
    public class ParGoalDTO : EntityBase
    {
        public int ParLevel1_Id { get; set; }
        public int ParCompany_Id { get; set; }
        public decimal PercentValue { get; set; }
        public bool IsActive { get; set; } = true;
        public ParCompanyDTO ParCompany { get; set; }
    }
}