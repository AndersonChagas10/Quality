using Dominio.Entities;
using Dominio.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class UserRepository :  IUserRepository
    {
        private readonly IRepositoryBase<User> _userRepo;

        public UserRepository(IRepositoryBase<User> userRepo)
        {
            _userRepo = userRepo;
        }

        public User Get(int Id)
        {
            return _userRepo.GetById(Id);
        }

        public User Get(string Name)
        {
            return _userRepo.GetAll().FirstOrDefault(r => r.Name == Name);
        }

        public bool UserNameIsCadastrado(string Name, int id)
        {
            return _userRepo.GetAll().Any(x => x.Id != id && x.Name == Name);
        }

        public void Salvar(User user)
        {
            _userRepo.AddOrUpdate(user);
            _userRepo.Commit();
        }

        public User AuthenticationLogin(string name, string password) //Daqui iria pro B.D.
        {
            #region MOCK
            
            //UserDBMock USUARIO MOCK
            var user = new User { Id = 0, Name = "Admin", Password = "123" };
            var user2 = new User { Id = 2, Name = "User", Password = "123" };

            var listUser = new List<User>();
            listUser.Add(user);
            listUser.Add(user2); 

            #endregion

            var result = listUser.FirstOrDefault(r => r.Name.Equals(name) && r.Password.Equals(password));
            //var result = db.Where(r => r.Name.Equals("Admin") && r.Password.Equals("123")).FirstOrDefault();

            return result; // Retorno para camada Dominio
        }
    }
}
