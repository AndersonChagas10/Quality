using DTO.DTO;
using System.Linq;
using System.Security.Principal;

namespace SgqSystem.Secirity
{
    public class CustomPrincipal : IPrincipal
    {
        private UserDTO UserVm;
        private string token;
        private string cookie;

        //private readonly IUserDomain _userApp;

        public CustomPrincipal(string userName)
        {
            //var teste = new UserApp(_userApp);
            //if (UserVm == null)
            //    UserVm = teste.GetByName(userName).Retorno;
            Identity = new GenericIdentity(userName);

        }

        public IIdentity Identity
        {
            get;
            set;
        }

        public bool IsInRole(string role)
        {
            var roles = role.Split(new char[] { ',' });
            return roles.Any(r => this.UserVm.Roles.Contains(r));
        }
    }
}