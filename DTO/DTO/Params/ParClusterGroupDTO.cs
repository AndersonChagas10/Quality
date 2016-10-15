using DTO.BaseEntity;

namespace DTO.DTO.Params
{
    public class ParClusterGroupDTO : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ParClusterGroupParent_Id { get; set; }
        public bool Active { get; set; }
    }
}