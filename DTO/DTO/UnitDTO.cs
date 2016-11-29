using DTO.BaseEntity;

namespace DTO.DTO
{
    public class UnitDTO : EntityBase
    {

        public string Name { get; set; }
        public int Number { get; set; }

        public int Code { get; set; }
        public string Ip { get; set; }
    }
}
