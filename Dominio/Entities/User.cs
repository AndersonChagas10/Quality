using Dominio.Entities.BaseEntity;
using System;

namespace Dominio.Entities
{
    public class User : EntityBase
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public DateTime AcessDate { get; set; }
       
    }
}
