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

        /// <summary>
        /// Construtor para Inversion of Control.
        /// </summary>
        /// <param name="userRepo"> Repositório de Usuario, interface de comunicação com Data. </param>
        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        /// <summary>
        /// VErifica se existe Usuario e Senha Correspondentes no Banco de dados.
        /// </summary>
        /// <param name="name"> Nome do Usuário. </param>
        /// <param name="password"> Senha do Usuário. </param>
        /// <returns> Retorna o Usuário caso exista, caso não exista retorna exceção com uma mensagem</returns>
        public GenericReturn<User> AuthenticationLogin(User user)
        {
            if (user.IsNull())
                throw new ExceptionHelper("Nome de Usuario e Senha devem ser informados.");

            var isUser = _userRepo.AuthenticationLogin(user);

            if (isUser.IsNotNull())
                return new GenericReturn<User>(isUser);
            else
                throw new ExceptionHelper("Usuario não encontrado, verifique e-mail e senha.");
        }

    }

    
}
