using DTO.BaseEntity;

namespace DTO.DTO.Params
{
    public class NxN : EntityBase
    {
        public bool IsSave { get; set; }
        public bool IsInativar { get; set; }
        public bool IsEdit { get; set; }
        public bool IsReativar { get; set; }
    }
}
