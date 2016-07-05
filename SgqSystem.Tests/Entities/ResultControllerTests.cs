using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Dominio.Entities.Tests
{
    [TestClass()]
    public class ResultControllerTests
    {

        
        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void NewResult_Total_Avalido_Negativo()
        {
            decimal valor1 = -1;
            new ResultOld(0, 1, 2, 3, valor1, 0);

        }

        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void NewResult_Total_Nao_Conformidade_Negativo()
        {
            decimal valor1 = -1;
            new ResultOld(0, 1, 2, 3, 0, valor1);
        }

        #region Teste de Ids Fks e Pks

       
        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void NewResult_Id_Tarefa_Invalido()
        {
            new ResultOld(0, -1, 2, 3, 0, 0);
        }

     

        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void NewResult_Id_Monitoramento_Invalido()
        {
            new ResultOld(0, 1, 1, -1, 0, 0);
        }

      

        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void NewResult_Id_Operacao_Invalido()
        {
            new ResultOld(0, 1, -2, 3, 0, 0);
        }
      

        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void NewResult_Id_Invalido()
        {
            new ResultOld(-10, 1, 2, 3, 0, 0);
        }

        #endregion


    }
}