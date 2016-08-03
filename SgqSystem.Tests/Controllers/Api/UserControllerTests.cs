using Application.Interface;
using DTO.DTO;
using DTO.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace SgqSystem.Controllers.Api.Tests
{
    [TestClass()]
    public class UserControllerTests
    {

        private readonly Mock<IUserApp> _userApp;

        public UserControllerTests()
        {
            _userApp = new Mock<IUserApp>();
        }

        [TestMethod]
        public void User_Login_Enviando_Usuario_Nulo()
        {
            _userApp.Setup(r => r.AuthenticationLogin(null)).Returns(new GenericReturn<UserDTO>()).Verifiable();
            _userApp.Object.AuthenticationLogin(null);
            _userApp.VerifyAll();
        }

        [TestMethod]
        public void User_Login_Nao_Encontrado()
        {
           // var user = new UserDTO() { Name = "sadasdsadsa", Password = "sadsdsad" };
           // var userVm = new UserViewModel() { Name = "sadasdsadsa", Password = "sadsdsad" };
           // var retorno = new GenericReturn<UserDTO>() { Mensagem = "Username and Password are required.", MensagemExcecao = "Username and Password are required." };

           // var mock = new Mock<IUserRepository>();
           // mock.Setup(r => r.AuthenticationLogin(user)).Returns(retorno);
           //var objsender = new UserDomain(mock.Object).AuthenticationLogin(user);

           // UserController controller = new UserController(mock.Object);
           // var response = controller.Post(userVm);

           // Assert.AreEqual(response, retorno);

           // _userApp.Setup(r => r.AuthenticationLogin(user)).Returns(retorno).Verifiable();
           // _userApp.Object.AuthenticationLogin(user);
           // _userApp.Verify();
        }

    }
}