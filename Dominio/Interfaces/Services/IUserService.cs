﻿using Dominio.Entities;
using Dominio.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Interfaces.Services
{
    public interface IUserService : IServiceBase<User>
    {
        GenericReturn<User> Autorizado(string name, string password);
    }
}
