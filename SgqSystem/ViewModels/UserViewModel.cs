using SgqSystem.ViewModels.BaseEntityViewModel;

namespace SgqSystem.ViewModels
{
    public class UserViewModel : EntityBaseViewModel
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}