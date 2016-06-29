using System;

namespace Dominio.Entities.BaseEntity
{
    public class EntityBase
    {
        public int  Id { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime AlterDate { get; set; }
    }
}
