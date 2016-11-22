using DTO.BaseEntity;
using System.ComponentModel.DataAnnotations;

namespace DTO.DTO.Params
{
    public class ParLevel1XClusterDTO : EntityBase
    {
        public int ParLevel1_Id { get; set; }
        [Required]
        public int ParCluster_Id { get; set; }
        public decimal Points { get; set; }
        public bool IsActive { get; set; } = true;

        public ParClusterDTO ParCluster { get; set; }

    }
}