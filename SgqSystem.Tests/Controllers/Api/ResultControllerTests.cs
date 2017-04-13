using Dominio.Interfaces.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
using System;

namespace Dominio.Controllers.Api.Tests
{
    [TestClass()]
    public class ResultControllerTests
    {

        //private readonly Coleta _result ;
        //private readonly Mock<IBetaRepository> _resultRepo;

        #region Testes dados negativo quantidade

        [TestMethod()]
        [ExpectedException(typeof(ExceptionHelper))]
        public void ResultControllertTest_Total_Avalido_Negativo()
        {
            //decimal valor1 = -1;
            //new Coleta(0, 1, 2, 3, valor1, 0);

        }

        [TestMethod()]
        public void ResultControllertTest_Total_Nao_Conformidade_Negativo()
        {
            try
            {
                //decimal naoConformidade = -1;
                //new Coleta(0, 1, 2, 3, 0, naoConformidade);
                Assert.Fail("Should throw here");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "O Total Avaliado não pode ser negativo!");
            }
        }

        #endregion

        #region Teste de Ids Fks e Pks

        [TestMethod()]
        public void ResultControllertTest_Id_Level3_Invalido()
        {
            try
            {
                //int idLevel3 = -1;
                //new Coleta(0, idLevel3, 2, 3, 0, 0);
                Assert.Fail("Should throw here");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "O Id da Level3 está em formato Inválido ou Nulo.");
            }
        }

        [TestMethod()]
        public void ResultControllertTest_Id_Level2_Invalido()
        {
            try
            {
                //int idLevel2 = -1;
                //new Coleta(0, 1, 1, idLevel2, 0, 0);
                Assert.Fail("Should throw here");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "O Id do Level2 está em formato Inválido ou Nulo.");
            }
        }

        [TestMethod()]
        public void ResultControllertTest_Id_Level1_Invalido()
        {
            try
            {
                //int idLevel1 = -1;
                //new Coleta(0, 1, 1, idLevel1, 0, 0);
                Assert.Fail("Should throw here");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "O Id do Level2 está em formato Inválido ou Nulo.");
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(ExceptionHelper))]
        public void ResultControllertTest_Id_Invalido()
        {
            //new Coleta(-10, 1, 2, 3, 0, 0);
        }

        [TestMethod()]
        public void ResultControllertTest_Id_Invalido_Level1_Insercao_Banco_Dados()
        {
            //var result = new Coleta(100, 2, 2, 3, 0, 0);
            //_resultRepo.Setup(r => r.Salvar(result)).Throws<ExceptionHelper>();
            //var service = new BetaService(_resultRepo.Object);
            //service.Salvar(result);
            //_resultRepo.Verify(r => r.Salvar(result), Times.Never);
        }

        [TestMethod()]
        public void ResultControllertTest_Id_Invalido_Level2_Insercao_Banco_Dados()
        {
            //var result = new Coleta(2, 100, 2, 3, 0, 0);
            //_resultRepo.Setup(r => r.Salvar(result)).Throws<ExceptionHelper>();
            //var service = new BetaService(_resultRepo.Object);
            //service.Salvar(result);
            //_resultRepo.Verify(r => r.Salvar(result), Times.Never);
        }

        [TestMethod()]
        public void ResultControllertTest_Id_Invalido_Level3_Insercao_Banco_Dados()
        {
            //var result = new Coleta(2, 2, 100, 3, 0, 0);
            //_resultRepo.Setup(r => r.Salvar(result)).Throws<ExceptionHelper>();
            //var service = new BetaService(_resultRepo.Object);
            //service.Salvar(result);
            //_resultRepo.Verify(r => r.Salvar(result), Times.Never);
        }



        #endregion


    }
}