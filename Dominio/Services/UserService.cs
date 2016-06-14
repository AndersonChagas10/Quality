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


        public GenericReturn<User> Autorizado(string name, string password)
        {

            var retorno = new GenericReturn<User>();

            try 
	        {	        
		
                var isUser = _userRepo.Autorizado(name, password);
                
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
                return RetornaExcecaoBase(ex, "Ocorreu um erro inesperado");    
	        }

            return retorno;

        }
    }

    
}
