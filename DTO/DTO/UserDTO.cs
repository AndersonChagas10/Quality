using DTO.BaseEntity;
using DTO.Helpers;

namespace DTO.DTO
{
    public class UserDTO : EntityBase
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public bool isTechnical { get; set; }
        public bool isSlaugther { get; set; }

        /// <summary>
        /// Construtor Padrão
        /// </summary>
        public UserDTO()
        {

        }

        /// <summary>
        /// Validação de nome de Usuario e senha.
        /// </summary>
        /// <param name="name"> Nome do Usuário. </param>
        /// <param name="password"> Senha do Usuário. </param>
        public void ValidaObjetoUserDTO()
        {
            //Verifica se ambos parametros estao nulos.
            if (string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(Password))
                throw new ExceptionHelper("Username and Password are required.");

            SetName(Name);
            SetPassword(Password);
        }


        /// <summary>
        /// Valida e atribui valor a property Name do Usuário.
        /// </summary>
        /// <param name="name"> Nome do Usuário. </param>
        public void SetName(string name)
        {
            Guard.ForNullOrEmpty(name, "The Username is required.");
            Guard.NullOrEmptyValuesCheck(out name, "Username", name, requerido: true, mensagem: "The Username is required.");
            //Name = name;
        }

        /// <summary>
        /// Valida e atribui valor a property Password do Usuário.
        /// </summary>
        /// <param name="name"> Senha do Usuário. </param>
        public void SetPassword(string pass)
        {
            Guard.ForNullOrEmpty(pass, "The Password is required.");
            Guard.VerificaEspacoString(pass, "The Password field should not contains blank spaces."); //A Senha não deve conter espaços
            Guard.NullOrEmptyValuesCheck(out pass, "Password", pass, mensagem: "The Password is required.", requerido: true); //O campo senha deve ser informado.
            //Password = pass;
        }

    }
}
