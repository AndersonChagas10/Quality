using DTO.BaseEntity;

namespace DTO.DTO.Params
{
    public class ParLocalDTO : EntityBase
    {
        public int Level { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;

    }
}