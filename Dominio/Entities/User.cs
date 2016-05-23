using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entities
{
    public class User
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public int Id { get; set; }

        public bool Autorizado(User u)
        {
            return u.Password.Equals("123");
        }
    }
}
