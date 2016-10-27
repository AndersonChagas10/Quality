using DTO.BaseEntity;

namespace DTO.DTO.Params
{
    public class ParNotConformityRuleXLevelDTO : EntityBase
    {
        public int ParNotConformityRule_Id { get; set; }
        public int ParCompany_Id { get; set; }
        public decimal Value { get; set; }
        public int Level { get; set; }
        public int? ParLevel1_Id { get; set; }
        public int? ParLevel2_Id { get; set; }
        public int? ParLevel3_Id { get; set; }
        public bool IsReaudit { get; set; }
        public bool IsActive { get; set; } = true;
        
        public ParCompanyDTO parCompany { get; set; }
        public ParLevel1DTO parLevel1 { get; set; }
        public ParLevel2DTO parLevel2 { get; set; }
        public ParLevel3DTO parLevel3 { get; set; }
        public ParNotConformityRuleDTO parNotConformityRule { get; set; }


    }
}