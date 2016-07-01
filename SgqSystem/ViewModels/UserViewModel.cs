using SgqSystem.ViewModels.BaseEntityViewModel;
using System;

namespace SgqSystem.ViewModels
{
    public class UserViewModel : EntityBaseViewModel
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public DateTime AcessDate { get; set; }
    }
}