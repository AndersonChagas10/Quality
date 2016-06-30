using Dominio.Entities.BaseEntity;

namespace Dominio.Entities
{
    public class Audit : DataCollectionBase
    {

        public virtual AuditCenter auditCenter { get; set; }

    }
}
