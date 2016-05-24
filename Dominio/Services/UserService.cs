using Dominio.Entities;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Services
{
    public class UserService : ServiceBase<User>, IUserService
    {
        private readonly IUserRepository _userRepo;

        public UserService(IUserRepository userRepo)
            : base(userRepo)
        {
            _userRepo = userRepo;
        }
        public bool Autorizado(string name, string password)
        {
            return _userRepo.Autorizado(name, password);
        }
    }
}
