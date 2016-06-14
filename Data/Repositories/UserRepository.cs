using Dominio.Entities;
using Dominio.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {

        public User Autorizado(string name, string password) //Daqui iria pro B.D.
        {
            #region MOCK
            //UserDBMock USUARIO MOCK
            var user = new User { Id = 0, Name = "Admin", Password = "123" };
            var user2 = new User { Id = 2, Name = "User", Password = "123" };

            var listUser = new List<User>();
            listUser.Add(user);
            listUser.Add(user2); 

            #endregion

            var result = listUser.Where(r => r.Name.Equals("Admin") && r.Password.Equals("123")).FirstOrDefault();
            //var result = db.Where(r => r.Name.Equals("Admin") && r.Password.Equals("123")).FirstOrDefault();

            return result; // Retorno para camada Dominio
        }
    }
}
