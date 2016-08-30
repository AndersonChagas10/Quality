using DTO.BaseEntity;

namespace DTO.DTO
{
    public class CollectionHtmlDTO : EntityBase
    {
        public string Html { get; set; }
        public int Period { get; set; }
        public int Shift { get; set; }
    }
}
