using Dominio.Entities.BaseEntity;

namespace Dominio.Entities
{
    public class Result : DataCollectionBase
    {
        public int Id_AuditCenter { get; set; }
        public virtual AuditCenter auditCenter { get; set; }
    }
}
