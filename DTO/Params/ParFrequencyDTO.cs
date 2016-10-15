using DTO.BaseEntity;

namespace DTO.DTO.Params
{
    public class ParFrequencyDTO : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    }
}