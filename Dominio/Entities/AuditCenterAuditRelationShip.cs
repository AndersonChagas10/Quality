using Dominio.Entities.BaseEntity;

namespace Dominio.Entities
{
    public class AuditCenterRelationShip : EntityBase
    {
        public AuditCenter auditCenter { get; set; }
        public int level { get; set; }
    }
}
