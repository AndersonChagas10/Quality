using Dominio.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Dominio.Controllers.Api.Tests
{
    [TestClass()]
    public class ResultControllerTests
    {

        #region Testes dados negativo quantidade

        [TestMethod()]
        [ExpectedException(typeof(ExceptionHelper))]
        public void ResultControllertTest_Total_Avalido_Negativo()
        {
            decimal valor1 = -1;
            new ResultOld(0, 1, 2, 3, valor1, 0);

        }

        [TestMethod()]
        public void ResultControllertTest_Total_Nao_Conformidade_Negativo()
        {
            try
            {
                decimal naoConformidade = -1;
                new ResultOld(0, 1, 2, 3, 0, naoConformidade);
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
        public void ResultControllertTest_Id_Tarefa_Invalido()
        {
            try
            {
                int idTarefa = -1;
                new ResultOld(0, idTarefa, 2, 3, 0, 0);
                Assert.Fail("Should throw here");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "O Id da Tarefa está em formato Inválido ou Nulo.");
            }
        }

     

        [TestMethod()]
        public void ResultControllertTest_Id_Monitoramento_Invalido()
        {
            try
            {
                int idMonitoramento = -1;
                new ResultOld(0, 1, 1, idMonitoramento, 0, 0);
                Assert.Fail("Should throw here");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "O Id do Monitoramento está em formato Inválido ou Nulo.");
            }
        }

      

        [TestMethod()]
        public void ResultControllertTest_Id_Operacao_Invalido()
        {
            try
            {
                int idOperacao = -1;
                new ResultOld(0, 1, 1, idOperacao, 0, 0);
                Assert.Fail("Should throw here");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "O Id do Monitoramento está em formato Inválido ou Nulo.");
            }
        }
      

        [TestMethod()]
        [ExpectedException(typeof(ExceptionHelper))]
        public void ResultControllertTest_Id_Invalido()
        {
            new ResultOld(-10, 1, 2, 3, 0, 0);
        }

        #endregion


    }
}