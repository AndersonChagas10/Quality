using System;

namespace Dominio.Entities.BaseEntity
{
    public class EntityBase
    {
        public int Id { get; set; } = 0;
        public DateTime AddDate { get; set; } = DateTime.Now;
        public DateTime? AlterDate { get; set; } = null;

        //public int Id { get; set; } = 0;
        //public DateTime AddedDate { get; set; } = DateTime.Now;
        //public DateTime? LastUpdate { get; set; } = null;
    }
}
