using DTO.BaseEntity;
using DTO.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DTO.DTO
{
    public class UserDTO : EntityBase
    {
        public virtual List<ParCompanyXUserSgqDTO> ParCompanyXUserSgq { get; set; }

        //[Required]
        [StringLength(150)]
        [Display(Name = "Username: ")]
        public string Name { get; set; }

        //[Required]
        [StringLength(20, MinimumLength = 3)]
        [Display(Name = "Password: ")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool isTechnical { get; set; }
        public bool isSlaugther { get; set; }
        public string[] Roles { get; set; }
        public string FullName { get; set; }
        public List<UnitUserDTO> UnitUser { get; set; }

        public string Role { get; set; }
        public bool isBrasil { get; set; }
        public Nullable<int> ParCompany_Id { get; set; }

        public DateTime? PasswordDate { get; set; }

        public bool ChangePassword { get; set; } = false;

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
            Name = Regex.Replace(Name, @"\s+", "");
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
            Guard.CheckStringFull(out name, "Username", name, requerido: true, mensagem: "The Username is required.");

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
            Guard.CheckStringFull(out pass, "Password", pass, mensagem: "The Password is required.", requerido: true); //O campo senha deve ser informado.
            //Password = pass;
        }

        public bool app { get; set; }


        public string ConfirmPassword { get; set; }
        public Nullable<System.DateTime> AcessDate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsWeb { get; set; }
        public IEnumerable<int> ListParCompany_Id { get; set; }
        public IEnumerable<string> ListRole { get; set; }
        

    }
}
