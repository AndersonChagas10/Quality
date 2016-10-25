using DTO.BaseEntity;
using System.ComponentModel.DataAnnotations;

namespace DTO.DTO.Params
{
    public class ParLevel1XClusterDTO : EntityBase
    {
        [Required]
        public int ParLevel1_Id { get; set; }
        [Required]
        public int ParCluster_Id { get; set; }
        [Required]
        public decimal Points { get; set; }
        public bool Active { get; set; }

    }
}