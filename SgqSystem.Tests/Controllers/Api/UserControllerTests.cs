using Dominio.Interfaces.Services;
using DTO.DTO;
using DTO.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;

namespace SgqSystem.Controllers.Api.Tests
{
    [TestClass()]
    public class UserControllerTests
    {

        //private readonly Mock<IUserDomain> _userDomain;

        public UserControllerTests()
        {
            //_userDomain = new Mock<IUserDomain>();
        }

        [TestMethod]
        public void User_Login_Enviando_Usuario_Nulo()
        {
            /*_userDomain.Setup(r => r.AuthenticationLogin(null)).Returns(new GenericReturn<UserDTO>()).Verifiable();
            _userDomain.Object.AuthenticationLogin(null);
            _userDomain.VerifyAll();*/
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

           // _userDomain.Setup(r => r.AuthenticationLogin(user)).Returns(retorno).Verifiable();
           // _userDomain.Object.AuthenticationLogin(user);
           // _userDomain.Verify();
        }

    }
}