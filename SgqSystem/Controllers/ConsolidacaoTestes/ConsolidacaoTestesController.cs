using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers.ConsolidacaoTestes
{

    #region MOCK Classes Model

    public class ResultLevel3
    {
        public int Id { get; set; }
        public int ResultLevel2Id { get; set; }
        public int ParLevel1Id { get; set; }
        public decimal WeiDefects { get; set; }
        public decimal WeiEvaluation { get; set; }
        public String ParLevel1Name { get; set; }
        public int ParLevel2Id { get; set; }
        public String ParLevel2Name { get; set; }
        public int ParLevel3Id { get; set; }
        public String ParLevel3Name { get; set; }
        public decimal Weight { get; set; }
        public decimal Evaluation { get; set; }
        public decimal MinValue { get; set; }
        public decimal MaxValue { get; set; }
        public String ValueDone { get; set; }
        public decimal Defects { get; set; }
        public decimal CT4Def3 { get; set; }
        public decimal CT4Eva3 { get; set; }
        public DateTime CollectionDate { get; set; }
        public DateTime AddDate { get; set; }
        public int UserId { get; set; }
        public String AddIp { get; set; }
        public String AddMac { get; set; }
        public String AddGeograficPosition { get; set; }
        public int UnidadeId { get; set; }

    }

    public class ResultLevel2
    {
        public int Id { get; set; }
        public int ResultLevel1Id { get; set; }
        public int ParLevel1Id { get; set; }
        public String ParLevel1Name { get; set; }
        public int ParLevel2Id { get; set; }
        public String ParLevel2Name { get; set; }
	    public int Punishment { get; set; }
	    public decimal CT1Eva2 { get; set; }
	    public decimal CT1Def2 { get; set; }
	    public decimal CT2Eva2 { get; set; }
	    public decimal CT2Def2 { get; set; }
	    public decimal CT3Eva2 { get; set; }
	    public decimal CT3Def2 { get; set; }
	    public decimal CT4Eva2 { get; set; }
	    public decimal CT4Def2 { get; set; }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           
	    public decimal CT5Eva2 { get; set; }
	    public decimal CT5Def2 { get; set; }
        public DateTime AddDate { get; set; }
        public int UnidadeId { get; set; }
    }

    public class ResultLevel1
    {
        public int Id { get; set; }
        public int ResultLevel1Id { get; set; }
        public int ParLevel1Id { get; set; }
        public String ParLevel1Name { get; set; }
        public decimal CT1Eva1 { get; set; }
	    public decimal CT1Def1 { get; set; }
	    public decimal CT2Eva1 { get; set; }
	    public decimal CT2Def1 { get; set; }
	    public decimal CT3Eva1 { get; set; }
	    public decimal CT3Def1 { get; set; }
        public decimal CT3Def1_2 { get; set; }
        public decimal CT4Eva1 { get; set; }
	    public decimal CT4Def1 { get; set; }
	    public decimal CT5Eva1 { get; set; }
	    public decimal CT5Def1 { get; set; }
        public DateTime AddDate { get; set; }
        public int UnidadeId { get; set; }
    }

    #endregion

    public class ConsolidacaoTestesController : Controller
    {

        #region MOCKS Lists

        private List<ResultLevel3> createListResultLevel3()
        {
            var listaLevel3 = new List<ResultLevel3>();


            listaLevel3.Add(new ResultLevel3
            {
                Id = 1,
                ResultLevel2Id = 1,
                ParLevel1Id = 1,
                ParLevel1Name = "Indicador 1",
                ParLevel2Id = 1,
                ParLevel2Name = "Monitoramento 1",
                ParLevel3Id = 1,
                ParLevel3Name = "Tarefa 1",
                Weight = 1,
                WeiEvaluation = calcWeightEvaluation(1, 2),
                Evaluation = 1,
                MinValue = 1,
                MaxValue = 1,
                ValueDone = "",
                Defects = 1,
                WeiDefects = calcWeightDefects(1, 1, 1),
                CT4Eva3 = CT4Eva3(1),
                CT4Def3 = CT4Def3(1),
            });

            return listaLevel3;
        }

        private List<ResultLevel2> createListResultLevel2(List<ResultLevel3> listaLevel3)
        {
            var listaLevel2 = new List<ResultLevel2>();

            listaLevel2.Add(new ResultLevel2
            {
                Id = 1,
                ResultLevel1Id = 1,
                ParLevel1Name = "",
                ParLevel2Id = 1,
                ParLevel2Name = "",
                Punishment = 1,
                CT1Eva2 = CT1Eva2(1, listaLevel3),
                CT1Def2 = CT1Def2(1, 2, listaLevel3),
                CT2Eva2 = CT2Eva2(1, listaLevel3),
                CT2Def2 = CT2Def2(1, listaLevel3),
                CT3Eva2 = CT3Eva2(1, listaLevel3),
                CT3Def2 = CT3Def2(1, listaLevel3),
                CT4Eva2 = CT4Eva2(1, listaLevel3),
                CT4Def2 = CT4Def2(1, listaLevel3),
                CT5Eva2 = CT5Eva2(1, listaLevel3),
                CT5Def2 = CT5Def2(1, listaLevel3),
            });

            return listaLevel2;
        }

        private List<ResultLevel1> createListResultLevel1(List<ResultLevel2> listaLevel2)
        {
            var listaLevel1 = new List<ResultLevel1>();


            listaLevel1.Add(new ResultLevel1
            {
                Id = 1,
                ResultLevel1Id =1,
                ParLevel1Id = 1,
                ParLevel1Name = "",
                CT1Eva1   = CT1Eva1(1, listaLevel2),
                CT1Def1   = CT1Def1(1, listaLevel2),
                CT2Eva1   = CT2Eva1(1, listaLevel2),
                CT2Def1   = CT2Def1(1, listaLevel2),
                CT3Eva1   = CT3Eva1(1, listaLevel2),
                CT3Def1   = CT3Def1(1, listaLevel2),
                CT3Def1_2 = CT3Def1_2(1, listaLevel2),
                CT4Eva1   = CT4Eva1(1, listaLevel2),
                CT4Def1   = CT4Def1(1, listaLevel2),
                CT5Eva1   = CT5Eva1(1, listaLevel2),
                CT5Def1   = CT5Def1(1, listaLevel2),
            });

            return listaLevel1;
        }
        
        #endregion

        //http://localhost:63128/ConsolidacaoTestes/Index
        // GET: ConsolidacaoTestes
        public ActionResult Index()
        {
            var listResultLevel3 = createListResultLevel3();

            var listResultLevel2 = createListResultLevel2(listResultLevel3);

            var listResultLevel1 = createListResultLevel1(listResultLevel2);

            return View();
        }

        #region Funções ConsolidationType ResultLevel1

        /// <summary>
        /// Calcula a soma do campo CT1Eva2 da lista da tabela ResultLevel2 que possuem o campo
        /// ResultLevel1Id igual ao parametro ResultLevel1Id e retorna o valor
        /// </summary>
        /// <param name="ResultLevel1Id">O parametro vem do campo Id da tabela ResultLevel1</param>
        /// <param name="listLevel2">Lista criada a partir da tabela ResultLevel2</param>
        /// <returns></returns>
        private decimal CT1Eva1(int ResultLevel1Id, List<ResultLevel2> listLevel2)
        {
            decimal sum = 0;

            for (int i = 0; i < listLevel2.Count; i++)
            {
                if (ResultLevel1Id == listLevel2[i].ResultLevel1Id)
                {
                    sum += listLevel2[i].CT1Eva2;
                }
            }

            return sum;
        }

        /// <summary>
        /// Calcula a soma do campo CT1Def2 da lista da tabela ResultLevel2 que possuem o campo
        /// ResultLevel1Id igual ao parametro ResultLevel1Id e retorna o valor
        /// </summary>
        /// <param name="ResultLevel1Id">O parametro vem do campo Id da tabela ResultLevel1</param>
        /// <param name="listLevel2">Lista criada a partir da tabela ResultLevel2</param>
        /// <returns></returns>
        private decimal CT1Def1(int ResultLevel1Id, List<ResultLevel2> listLevel2)
        {
            decimal sum = 0;

            for (int i = 0; i < listLevel2.Count; i++)
            {
                if (ResultLevel1Id == listLevel2[i].ResultLevel1Id)
                {
                    sum += listLevel2[i].CT1Def2;
                }
            }

            return sum;
        }

        /// <summary>
        /// Calcula a soma do campo CT2Eva2 da lista da tabela ResultLevel2 que possuem o campo
        /// ResultLevel1Id igual ao parametro ResultLevel1Id e retorna o valor
        /// </summary>
        /// <param name="ResultLevel1Id">O parametro vem do campo Id da tabela ResultLevel1</param>
        /// <param name="listLevel2">Lista criada a partir da tabela ResultLevel2</param>
        /// <returns></returns>
        private decimal CT2Eva1(int ResultLevel1Id, List<ResultLevel2> listLevel2)
        {
            decimal sum = 0;

            for (int i = 0; i < listLevel2.Count; i++)
            {
                if (ResultLevel1Id == listLevel2[i].ResultLevel1Id)
                {
                    sum += listLevel2[i].CT2Eva2;
                }
            }

            return sum;
        }

        /// <summary>
        /// Calcula a soma do campo CT2Def2 da lista da tabela ResultLevel2 que possuem o campo
        /// ResultLevel1Id igual ao parametro ResultLevel1Id e retorna o valor
        /// </summary>
        /// <param name="ResultLevel1Id">O parametro vem do campo Id da tabela ResultLevel1</param>
        /// <param name="listLevel2">Lista criada a partir da tabela ResultLevel2</param>
        /// <returns></returns>
        private decimal CT2Def1(int ResultLevel1Id, List<ResultLevel2> listLevel2)
        {
            decimal sum = 0;

            for (int i = 0; i < listLevel2.Count; i++)
            {
                if (ResultLevel1Id == listLevel2[i].ResultLevel1Id)
                {
                    sum += listLevel2[i].CT2Def2;
                }
            }

            return sum;
        }

        /// <summary>
        /// Calcula a soma do campo CT2Eva2 da lista da tabela ResultLevel2 que possuem o campo
        /// ResultLevel1Id igual ao parametro ResultLevel1Id 
        /// Se o resultado dessa soma for maior que zero 
        /// retorno 1
        /// senão
        /// retorno 0
        /// </summary>
        /// <param name="ResultLevel1Id">O parametro vem do campo Id da tabela ResultLevel1</param>
        /// <param name="listLevel2">Lista criada a partir da tabela ResultLevel2</param>
        /// <returns></returns>
        private decimal CT3Eva1(int ResultLevel1Id, List<ResultLevel2> listLevel2)
        {
            decimal sum = 0;

            for (int i = 0; i < listLevel2.Count; i++)
            {
                if (ResultLevel1Id == listLevel2[i].ResultLevel1Id)
                {
                    sum += listLevel2[i].CT2Eva2;
                }
            }

            if(sum > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Calcula a soma do campo CT3Def2 da lista da tabela ResultLevel2 que possuem o campo
        /// ResultLevel1Id igual ao parametro ResultLevel1Id 
        /// Se o resultado dessa soma for maior que zero 
        /// retorno 1
        /// senão
        /// retorno 0
        /// </summary>
        /// <param name="ResultLevel1Id">O parametro vem do campo Id da tabela ResultLevel1</param>
        /// <param name="listLevel2">Lista criada a partir da tabela ResultLevel2</param>
        /// <returns></returns>
        private decimal CT3Def1(int ResultLevel1Id, List<ResultLevel2> listLevel2)
        {
            decimal sum = 0;

            for (int i = 0; i < listLevel2.Count; i++)
            {
                if (ResultLevel1Id == listLevel2[i].ResultLevel1Id)
                {
                    sum += listLevel2[i].CT3Def2;
                }
            }

            if (sum > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Calcula a soma do campo CT3Def2 da lista da tabela ResultLevel2 que possuem o campo
        /// ResultLevel1Id igual ao parametro ResultLevel1Id 
        /// </summary>
        /// <param name="ResultLevel1Id">O parametro vem do campo Id da tabela ResultLevel1</param>
        /// <param name="listLevel2">Lista criada a partir da tabela ResultLevel2</param>
        /// <returns></returns>
        private decimal CT3Def1_2(int ResultLevel1Id, List<ResultLevel2> listLevel2)
        {
            decimal sum = 0;

            for (int i = 0; i < listLevel2.Count; i++)
            {
                if (ResultLevel1Id == listLevel2[i].ResultLevel1Id)
                {
                    sum += listLevel2[i].CT3Def2;
                }
            }

            return sum;
        }

        /// <summary>
        /// Calcula a soma do campo CT4Eva2 da lista da tabela ResultLevel2 que possuem o campo
        /// ResultLevel1Id igual ao parametro ResultLevel1Id e retorna o valor
        /// </summary>
        /// <param name="ResultLevel1Id">O parametro vem do campo Id da tabela ResultLevel1</param>
        /// <param name="listLevel2">Lista criada a partir da tabela ResultLevel2</param>
        /// <returns></returns>
        private decimal CT4Eva1(int ResultLevel1Id, List<ResultLevel2> listLevel2)
        {
            decimal sum = 0;

            for (int i = 0; i < listLevel2.Count; i++)
            {
                if (ResultLevel1Id == listLevel2[i].ResultLevel1Id)
                {
                    sum += listLevel2[i].CT4Eva2;
                }
            }

            return sum;
        }

        /// <summary>
        /// Calcula a soma do campo CT4Def2 da lista da tabela ResultLevel2 que possuem o campo
        /// ResultLevel1Id igual ao parametro ResultLevel1Id e retorna o valor
        /// </summary>
        /// <param name="ResultLevel1Id">O parametro vem do campo Id da tabela ResultLevel1</param>
        /// <param name="listLevel2">Lista criada a partir da tabela ResultLevel2</param>
        /// <returns></returns>
        private decimal CT4Def1(int ResultLevel1Id, List<ResultLevel2> listLevel2)
        {
            decimal sum = 0;

            for (int i = 0; i < listLevel2.Count; i++)
            {
                if (ResultLevel1Id == listLevel2[i].ResultLevel1Id)
                {
                    sum += listLevel2[i].CT4Def2;
                }
            }

            return sum;
        }

        /// <summary>
        /// Calcula a soma do campo CT5Eva2 da lista da tabela ResultLevel2 que possuem o campo
        /// ResultLevel1Id igual ao parametro ResultLevel1Id e retorna o valor
        /// </summary>
        /// <param name="ResultLevel1Id">O parametro vem do campo Id da tabela ResultLevel1</param>
        /// <param name="listLevel2">Lista criada a partir da tabela ResultLevel2</param>
        /// <returns></returns>
        private decimal CT5Eva1(int ResultLevel1Id, List<ResultLevel2> listLevel2)
        {
            decimal sum = 0;

            for (int i = 0; i < listLevel2.Count; i++)
            {
                if (ResultLevel1Id == listLevel2[i].ResultLevel1Id)
                {
                    sum += listLevel2[i].CT5Eva2;
                }
            }

            return sum;
        }

        /// <summary>
        /// Calcula a soma do campo CT5Def2 da lista da tabela ResultLevel2 que possuem o campo
        /// ResultLevel1Id igual ao parametro ResultLevel1Id e retorna o valor
        /// </summary>
        /// <param name="ResultLevel1Id">O parametro vem do campo Id da tabela ResultLevel1</param>
        /// <param name="listLevel2">Lista criada a partir da tabela ResultLevel2</param>
        /// <returns></returns>
        private decimal CT5Def1(int ResultLevel1Id, List<ResultLevel2> listLevel2)
        {
            decimal sum = 0;

            for (int i = 0; i < listLevel2.Count; i++)
            {
                if (ResultLevel1Id == listLevel2[i].ResultLevel1Id)
                {
                    sum += listLevel2[i].CT5Def2;
                }
            }

            return sum;
        }

        #endregion

        #region Funções ConsolidationType ResultLevel2

        /// <summary>
        /// Calcula a soma do campo WeiEvaluation da lista da tabela ResultLevel3 que possuem o campo
        /// ResultLevel2Id igual ao parametro ResultLevel2Id e retorna o valor
        /// </summary>
        /// <param name="ResultLevel2Id">O parametro vem do campo Id da tabela ResultLevel2</param>
        /// <param name="listLevel3">Lista criada a partir da tabela ResultLevel3</param>
        /// <returns></returns>
        private decimal CT1Eva2(int ResultLevel2Id, List<ResultLevel3> listLevel3)
        {
            decimal sum = 0;

            for (int i = 0; i < listLevel3.Count; i++)
            {
                if (ResultLevel2Id == listLevel3[i].ResultLevel2Id)
                {
                    sum += listLevel3[i].WeiEvaluation;
                }
            }

            return sum;
        }

        /// <summary>
        /// Calcula a soma do campo WeiDefects da lista listLevel3 que tem o campo ResultLevel2Id
        /// igual ao parametro ResultLevel2Id
        /// Se o resultado dessa soma for maior ao campo CT1Eva2 
        /// o valor do campo CT1Eva2 será retornado
        /// senão
        /// o valor da soma será retornado
        /// </summary>
        /// <param name="ResultLevel2Id">Parametro vem do campo Id da tabela ResultLevel2</param>
        /// <param name="CT1Eva2">Parametro vem do campo CT1Eva2 da tabela ResultLevel2</param>
        /// <param name="listLevel3">Lista criada a partir da tabela ResultLevel3</param>
        /// <returns></returns>
        private decimal CT1Def2(int ResultLevel2Id, decimal CT1Eva2, List<ResultLevel3> listLevel3)
        {
            decimal sum = 0;

            for (int i = 0; i < listLevel3.Count; i++)
            {
                if (ResultLevel2Id == listLevel3[i].ResultLevel2Id)
                {
                    sum += listLevel3[i].WeiDefects;
                }
            }

            if (sum > CT1Eva2)
            {
                return CT1Eva2;
            }
            else
            {
                return sum;
            }

        }

        /// <summary>
        /// Calcula a soma do campo WeiEvaluation da lista listLevel3 que tem o campo ResultLevel2Id
        /// igual ao parametro ResultLevel2Id
        /// Se o resultado dessa soma for maior que zero 
        /// retorno 1
        /// senão
        /// retorno 0
        /// </summary>
        /// <param name="ResultLevel2Id">Parametro vem do campo Id da tabela ResultLevel2</param>
        /// <param name="listLevel3">Lista criada a partir da tabela ResultLevel3</param>
        /// <returns></returns>
        private int CT2Eva2(int ResultLevel2Id, List<ResultLevel3> listLevel3)
        {
            decimal sum = 0;

            for (int i = 0; i < listLevel3.Count; i++)
            {
                if (ResultLevel2Id == listLevel3[i].ResultLevel2Id)
                {
                    sum += listLevel3[i].WeiEvaluation;
                }
            }

            if (sum > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Calcula a soma do campo WeiDefects da lista listLevel3 que tem o campo ResultLevel2Id
        /// igual ao parametro ResultLevel2Id
        /// Se o resultado dessa soma for maior que zero 
        /// retorno 1
        /// senão
        /// retorno 0
        /// </summary>
        /// <param name="ResultLevel2Id">Parametro vem do campo Id da tabela ResultLevel2</param>
        /// <param name="listLevel3">Lista criada a partir da tabela ResultLevel3</param>
        /// <returns></returns>
        private int CT2Def2(int ResultLevel2Id, List<ResultLevel3> listLevel3)
        {
            decimal sum = 0;

            for (int i = 0; i < listLevel3.Count; i++)
            {
                if (ResultLevel2Id == listLevel3[i].ResultLevel2Id)
                {
                    sum += listLevel3[i].WeiDefects;
                }
            }

            if (sum > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Calcula a soma do campo WeiEvaluation da lista listLevel3 que tem o campo ResultLevel2Id
        /// igual ao parametro ResultLevel2Id
        /// Se o resultado dessa soma for maior que zero 
        /// retorno 1
        /// senão
        /// retorno 0
        /// </summary>
        /// <param name="ResultLevel2Id">Parametro vem do campo Id da tabela ResultLevel2</param>
        /// <param name="listLevel3">Lista criada a partir da tabela ResultLevel3</param>
        /// <returns></returns>
        private int CT3Eva2(int ResultLevel2Id, List<ResultLevel3> listLevel3)
        {
            decimal sum = 0;

            for (int i = 0; i < listLevel3.Count; i++)
            {
                if (ResultLevel2Id == listLevel3[i].ResultLevel2Id)
                {
                    sum += listLevel3[i].WeiEvaluation;
                }
            }

            if (sum > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Calcula a soma do campo WeiDefects da lista da tabela ResultLevel3 que possuem o campo
        /// ResultLevel2Id igual ao parametro ResultLevel2Id e retorna o valor
        /// </summary>
        /// <param name="ResultLevel2Id">Parametro vem do campo Id da tabela ResultLevel2</param>
        /// <param name="listLevel3">Lista criada a partir da tabela ResultLevel3</param>
        /// <returns></returns>
        private decimal CT3Def2(int ResultLevel2Id, List<ResultLevel3> listLevel3)
        {
            decimal sum = 0;

            for (int i = 0; i < listLevel3.Count; i++)
            {
                if (ResultLevel2Id == listLevel3[i].ResultLevel2Id)
                {
                    sum += listLevel3[i].WeiDefects;
                }
            }

            return sum;
        }

        /// <summary>
        /// Calcula a soma do campo CT4Eva3 da lista da tabela ResultLevel3 que possuem o campo
        /// ResultLevel2Id igual ao parametro ResultLevel2Id e retorna o valor
        /// </summary>
        /// <param name="ResultLevel2Id">Parametro vem do campo Id da tabela ResultLevel2</param>
        /// <param name="listLevel3">Lista criada a partir da tabela ResultLevel3</param>
        /// <returns></returns>
        private decimal CT4Eva2(int ResultLevel2Id, List<ResultLevel3> listLevel3)
        {
            decimal sum = 0;

            for (int i = 0; i < listLevel3.Count; i++)
            {
                if (ResultLevel2Id == listLevel3[i].ResultLevel2Id)
                {
                    sum += listLevel3[i].CT4Eva3;
                }
            }

            return sum;
        }

        /// <summary>
        /// Calcula a soma do campo CT4Def3 da lista da tabela ResultLevel3 que possuem o campo
        /// ResultLevel2Id igual ao parametro ResultLevel2Id e retorna o valor
        /// </summary>
        /// <param name="ResultLevel2Id">Parametro vem do campo Id da tabela ResultLevel2</param>
        /// <param name="listLevel3">Lista criada a partir da tabela ResultLevel3</param>
        /// <returns></returns>
        private decimal CT4Def2(int ResultLevel2Id, List<ResultLevel3> listLevel3)
        {
            decimal sum = 0;

            for (int i = 0; i < listLevel3.Count; i++)
            {
                if (ResultLevel2Id == listLevel3[i].ResultLevel2Id)
                {
                    sum += listLevel3[i].CT4Def3;
                }
            }

            return sum;
        }

        /// <summary>
        /// Calcula a soma do campo WeiEvaluation da lista da tabela ResultLevel3 que possuem o campo
        /// ResultLevel2Id igual ao parametro ResultLevel2Id e retorna o valor
        /// </summary>
        /// <param name="ResultLevel2Id">Parametro vem do campo Id da tabela ResultLevel2</param>
        /// <param name="listLevel3">Lista criada a partir da tabela ResultLevel3</param>
        /// <returns></returns>
        private decimal CT5Eva2(int ResultLevel2Id, List<ResultLevel3> listLevel3)
        {
            decimal sum = 0;

            for (int i = 0; i < listLevel3.Count; i++)
            {
                if (ResultLevel2Id == listLevel3[i].ResultLevel2Id)
                {
                    sum += listLevel3[i].WeiEvaluation;
                }
            }

            return sum;
        }

        /// <summary>
        /// Calcula a soma do campo WeiDefects da lista da tabela ResultLevel3 que possuem o campo
        /// ResultLevel2Id igual ao parametro ResultLevel2Id e retorna o valor
        /// </summary>
        /// <param name="ResultLevel2Id">Parametro vem do campo Id da tabela ResultLevel2</param>
        /// <param name="listLevel3">Lista criada a partir da tabela ResultLevel3</param>
        /// <returns></returns>
        private decimal CT5Def2(int ResultLevel2Id, List<ResultLevel3> listLevel3)
        {
            decimal sum = 0;

            for (int i = 0; i < listLevel3.Count; i++)
            {
                if (ResultLevel2Id == listLevel3[i].ResultLevel2Id)
                {
                    sum += listLevel3[i].WeiDefects;
                }
            }

            return sum;
        }
        
        #endregion
        
        #region Funções ConsolidationType ResultLevel3

        /// <summary>
        /// Se o peso das avaliações da tabela resultlevel3
        /// for maior que zero, retorna 1
        /// se igual a zero, retorno 0
        /// </summary>
        /// <param name="weightEvaluation">weightEvaluation da tabela resultlevel3</param>
        /// <returns>inteiro</returns>
        private int CT4Eva3(decimal weightEvaluation)
        {
            if (weightEvaluation > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Se o peso dos defeitos da tabela resultlevel3
        /// for maior que zero, retorna 1
        /// se igual a zero, retorno 0
        /// </summary>
        /// <param name="weightDefects">weightDefects da tabela resultlevel3</param>
        /// <returns></returns>
        private int CT4Def3(decimal weightDefects)
        {
            if (weightDefects > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        #endregion

        #region Calculos

        /// <summary>
        /// Calcula o peso com o número de defeitos e valor da punição
        /// </summary>
        /// <param name="weight"></param>
        /// <param name="evaluation"></param>
        /// <returns> weight*(defects+punishment)</returns>
        private decimal calcWeightDefects(decimal weight, decimal evaluation, decimal punishment)
        {
            return weight * (evaluation+punishment);
        }

        

        /// <summary>
        /// Calcula o peso com o número de avaliações
        /// </summary>
        /// <param name="weight"></param>
        /// <param name="evaluation"></param>
        /// <returns> weight * evaluation </returns>
        private decimal calcWeightEvaluation(decimal weight, decimal evaluation)
        {
            return weight * evaluation;
        }

        #endregion
                

    }

}