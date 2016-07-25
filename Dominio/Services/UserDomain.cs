using AutoMapper;
using Dominio.Entities;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;
using DTO.DTO;
using DTO.Helpers;
using System;

namespace Dominio.Services
{
    public class UserDomain : IUserDomain
    {

        private readonly IUserRepository _userRepo;
        public string falhaGeral { get { return "It was not possible retrieve any data."; } }

        /// <summary>
        /// Construtor para Inversion of Control.
        /// </summary>
        /// <param name="userRepo"> Repositório de Usuario, interface de comunicação com Data. </param>
        public UserDomain(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        /// <summary>
        /// VErifica se existe Usuario e Senha Correspondentes no Banco de dados.
        /// </summary>
        /// <param name="name"> Nome do Usuário. </param>
        /// <param name="password"> Senha do Usuário. </param>
        /// <returns> Retorna o Usuário caso exista, caso não exista retorna exceção com uma mensagem</returns>
        public GenericReturn<UserDTO> AuthenticationLogin(UserDTO userDto)
        {
            try
            {
                if (userDto.IsNull())
                    throw new ExceptionHelper("Username and Password are required.");

                userDto.ValidaObjetoUserDTO(); //Valida Properties do objeto para gravar no banco.

                var user = Mapper.Map<UserDTO, UserSgq>(userDto);
                var isUser = _userRepo.AuthenticationLogin(user);

                if (!isUser.IsNotNull())
                    throw new ExceptionHelper("User not found, please verify Username and Password.");

                var retorno = Mapper.Map<UserSgq, UserDTO>(isUser);
                return new GenericReturn<UserDTO>(retorno);
            }
            catch (Exception e)
            {
                return new GenericReturn<UserDTO>(e, falhaGeral);
            }
        }

    }


}
