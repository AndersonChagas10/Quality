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
        public int resultlevel2_id { get; set; }
        public int parlevel1_id { get; set; }
        public decimal WeiDefects { get; set; }
        public decimal WeiEvaluation { get; set; }
        public String parlevel1_name { get; set; }
        public int parlevel2_id { get; set; }
        public String parlevel2_name { get; set; }
        public int parlevel3_id { get; set; }
        public String parlevel3_name { get; set; }
        public decimal weight { get; set; }
        public decimal Evaluation { get; set; }
        public decimal MinValue { get; set; }
        public decimal MaxValue { get; set; }
        public String ValueDone { get; set; }
        public decimal defects { get; set; }
        public decimal CT4Def3 { get; set; }
        public decimal CT4Eva3 { get; set; }
        public DateTime CollectionDate { get; set; }
        public DateTime AddDate { get; set; }
        public int UserId { get; set; }
        public String addip { get; set; }
        public String addmac { get; set; }
        public String addgeograficposition { get; set; }
        
    }

    public class ResultLevel2
    {
        public int id { get; set; }
        public int resultlevel1_id { get; set; }
        public int parlevel1_id { get; set; }
        public String parlevel1_name { get; set; }
        public int parlevel2_id { get; set; }
        public String parlevel2_name { get; set; }
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
    }

    public class ResultLevel1
    {
        public int id { get; set; }
        public int resultlevel1_id { get; set; }
        public int parlevel1_id { get; set; }
        public String parlevel1_name { get; set; }
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
                resultlevel2_id = 1,
                parlevel1_id = 1,
                
            });

            return listaLevel3;
        }

        //private List<ResultLevel2> createListResultLevel2()
        //{
        //    var listaLevel3 = new List<ResultLevel3>();


        //    listaLevel3.Add(new ResultLevel3
        //    {
        //        Id = 1,
        //        resultlevel2_id = 1,
        //        parlevel1_id = 1,

        //    });

        //    return listaLevel3;
        //}

        //private List<ResultLevel1> createListResultLevel1()
        //{
        //    var listaLevel3 = new List<ResultLevel3>();


        //    listaLevel3.Add(new ResultLevel3
        //    {
        //        Id = 1,
        //        resultlevel2_id = 1,
        //        parlevel1_id = 1,

        //    });

        //    return listaLevel3;
        //}

        private List<ParLevel3> createListLevel3()
        {
            var listaLevel3 = new List<ParLevel3>();

            return listaLevel3;
        }

        private List<ParLevel2> createListLevel2()
        {
            var listaLevel2 = new List<ParLevel2>();

            return listaLevel2;
        }

        private List<ParLevel1> createListLevel1()
        {
            var listaLevel1 = new List<ParLevel1>();

            return listaLevel1;
        }

        #endregion

        //http://localhost:63128/ConsolidacaoTestes/Index
        // GET: ConsolidacaoTestes
        public ActionResult Index()
        {
            var listaCom5Level3 = createListResultLevel3();

            return View();
        }

        #region Funções ConsolidationType 

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
        /// Calcula o peso com O número de avaliações
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