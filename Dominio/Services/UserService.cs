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

            var retorno = new GenericReturn<User>();

            try 
	        {	        
                var isUser = _userRepo.AuthenticationLogin(name, password);
                if (isUser.IsNotNull())
                {
                    retorno.Retorno = isUser;
                }
                else
                {
                    retorno.MensagemAlerta = "Usuario não encontrado, verifique e-mail e senha.";
                }
	        }
	        catch (Exception ex)
	        {
                return ExceptionHelper<User>.RetornaExcecaoBase(ex, "Ocorreu um erro inesperado");    
	        }

            return retorno;

        }

    }

    
}
