using Dominio.Entities.BaseEntity;

namespace Dominio.Entities
{
    public class ResultCenterRelationShip : EntityBase
    {
        public ResultCenter resultCenter { get; set; }
        public int level { get; set; }
    }
}
