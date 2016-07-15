using Application.Interface;
using Dominio.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace SgqSystem.Controllers.Api.Tests
{
    [TestClass()]
    public class UserControllerTests
    {
        private readonly Mock<IUserAppService> _userApp;

        public UserControllerTests()
        {
            _userApp = new Mock<IUserAppService>();
        }

        [TestMethod]
        public void User_Login_Enviando_Usuario_Nulo()
        {
            _userApp.Setup(r => r.AuthenticationLogin(null)).Throws<ExceptionHelper>();
        }

        [TestMethod]
        public void User_Login_Nao_Encontrado()
        {
            _userApp.Setup(r => r.AuthenticationLogin(new UserSgq("wqewqewqeewqewq", "323132321wqewqewqe"))).Throws<ExceptionHelper>();
        }
       
    }
}