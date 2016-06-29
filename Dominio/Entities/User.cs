using Dominio.Entities.BaseEntity;

namespace Dominio.Entities
{
    public class User : EntityBase
    {
        public string Name { get; set; }
        public string Password { get; set; }
       
    }
}
