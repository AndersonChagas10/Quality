using DTO.DTO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SgqSystem.ViewModels
{
    public class UserViewModel : UserDTO
    {

        #region Tela de Perfil

        [StringLength(20, MinimumLength = 3)]
        [DataType(DataType.Password)]
        public string SenhaAntiga { get; set; }

        [StringLength(20, MinimumLength = 3)]
        [DataType(DataType.Password)]
        public string ConfirmarSenha { get; set; }
        
        public List<EmpresaDTO> Empresa { get; set; }

        public string ErrorList { get; set; }

        #endregion

    }

}