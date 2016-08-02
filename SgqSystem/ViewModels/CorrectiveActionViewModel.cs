using DTO.DTO;

namespace SgqSystem.ViewModels
{
    public class CorrectiveActionViewModel : CorrectiveActionDTO
    {

        public UserDTO User { get; set; }
        //private CorrectiveActionDTO _dto;
        //public CorrectiveActionDTO CorrectiveAction
        //{
        //    get
        //    {
        //        return _dto ?? (_dto = new CorrectiveActionDTO());
        //    }
        //    set
        //    {
        //        _dto = value;
        //    }
        //}

        //public string SlaughterPassword { get; set; }
        //public string SlaughterLogin { get; set; }

        //public string TechnicalPassword { get; set; }
        //public string TechnicalLogin { get; set; }

        //public bool Conectado { get; set; }

    }
}