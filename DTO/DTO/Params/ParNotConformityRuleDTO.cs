using DTO.BaseEntity;

namespace DTO.DTO.Params
{
    public class ParNotConformityRuleDTO : EntityBase
    {
        public string Name { get; set; }
        public string Sufix { get; set; }
        public bool IsActive { get; set; } = true;
    }
}