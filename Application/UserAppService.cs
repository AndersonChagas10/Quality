using Application.Interface;
using Dominio.Entities;
using Dominio.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class UserAppService :  AppServiceBase<User>, IUserAppService
    {
        
        private readonly IUserService _userService;
        
        public UserAppService(IUserService userService)
            : base(userService)
        {
            _userService = userService;
        }

        public bool Autorizado(User u)
        {
            return _userService.Autorizado(u);
        }

    }
}
