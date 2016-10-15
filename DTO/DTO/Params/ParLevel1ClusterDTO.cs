using DTO.BaseEntity;

namespace DTO.DTO.Params
{
    public class ParLevel1ClusterDTO : EntityBase
    {
        public int ParLevel1_Id { get; set; }
        public int ParCluster_Id { get; set; }
        public bool NonConformityRule { get; set; }
        public decimal Points { get; set; }
        public bool FixedEvaluetionNumber { get; set; }
        public bool LimitedEvaluetionNumber { get; set; }
        public bool Active { get; set; }
    }
}