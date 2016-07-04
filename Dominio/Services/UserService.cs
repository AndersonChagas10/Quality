using Dominio.Entities;
using Dominio.Helpers;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;
using System;

namespace Dominio.Services
{
    public class UserService :  IUserService
    {

        private readonly IUserRepository _userRepo;

        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public GenericReturn<User> AuthenticationLogin(string name, string password)
        {

            try 
	        {	        
                var isUser = _userRepo.AuthenticationLogin(name, password);
                if (isUser.IsNotNull())
                {
                    return new GenericReturn<User>(isUser);
                }
                else
                {
                    return new GenericReturn<User>() { MensagemAlerta = "Usuario não encontrado, verifique e-mail e senha." };
                }
	        }
	        catch (Exception ex)
	        {
                throw new GenericReturn<User>(ex, "Ocorreu um erro inesperado", "Não foi possível localizar Usuario / Senha, por favor tente novamente.");    
	        }

        }

    }

    
}
