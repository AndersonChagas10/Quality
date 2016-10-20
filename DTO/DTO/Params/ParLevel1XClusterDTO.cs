using DTO.BaseEntity;

namespace DTO.DTO.Params
{
    public class ParLevel1XClusterDTO : EntityBase
    {

        public int ParLevel1_Id { get; set; }
        public int ParCluster_Id { get; set; }
        public decimal Points { get; set; }
        public bool Active { get; set; }

    }
}