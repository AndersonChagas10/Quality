using DTO.BaseEntity;

namespace DTO.DTO.Params
{
    public class ParConsolidationTypeDTO : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }

        //public virtual ICollection<ParLevel1> ParLevel1 { get; set; }
    }
}