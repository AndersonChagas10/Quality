using DTO.BaseEntity;

namespace DTO.DTO.Params
{
    public class ParCounterDTO : EntityBase
    {
        public int Level { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;

    }
}