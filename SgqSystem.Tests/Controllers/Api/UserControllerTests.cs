using Application.Interface;
using Dominio.Entities;
using DTO.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace SgqSystem.Controllers.Api.Tests
{
    [TestClass()]
    public class UserControllerTests
    {
        private readonly IUserApp _userApp;

        public UserControllerTests(IUserApp userApp)
        {
            _userApp = userApp;
        }

        [TestMethod]
        public void User_Login_Enviando_Usuario_Nulo()
        {
            var result = _userApp.AuthenticationLogin(null);
            Assert.Equals(result.Mensagem, "");
        }

        [TestMethod]
        [ExpectedException(typeof(ExceptionHelper))]
        public void User_Login_Nao_Encontrado()
        {
            _userApp.AuthenticationLogin(new UserDTO() { Name = "wqewqewqeewqewq", Password = "323132321wqewqewqe" });
        }
       
    }
}