using DTO.Helpers;
using Newtonsoft.Json.Linq;
using SgqSystem.Handlres;
using SgqSystem.Mail;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api
{
    [HandleApi()]
    [RoutePrefix("GlobalConfigApi")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class GlobalConfigApiController : ApiController
    {

        //[HttpPost]
        //[Route("AlterGC/{Id}")]
        //public string AlterGC(int Id)
        //{
        //    return GlobalConfig.AlteraGc(Id);
        //}

        //[HttpPost]
        //[Route("CheckGC")]
        //public string CheckGC()
        //{
        //    return GlobalConfig.CheckGC();
        //}

        [HttpPost]
        [Route("TestaEmail")]
        public void TestaEmail(JObject form)
        {
            dynamic paramiters = form;
            string emailTo = paramiters.email;
            SimpleAsynchronous.SendMailFromDeviationSgqAppTesteBR(emailTo, false);
        }

        [HttpPost]
        [Route("TestaEmailUSA")]
        public void TestaEmailUSA(JObject form)
        {
            dynamic paramiters = form;
            SimpleAsynchronous.SendMailFromDeviationSgqAppTesteUSA(paramiters.email);
        }

        [HttpPost]
        [Route("RecSenha")]
        public string RecSenha([FromBody]string senha)
        {
            return Guard.DecryptStringAES(senha);
        }

        //[HttpPost]
        //[Route("ConverteValorCalculadoString")]
        //public string ConverteValorCalculado()
        //{
        //    //if (!string.IsNullOrEmpty(valor))
        //    //    return Guard.ConverteValorCalculado(valor).ToString();

        //    var dictResult = new Dictionary<string, decimal>();
        //    /*Potencias positivas*/
        //    dictResult.Add("23,99x10^2", Guard.ConverteValorCalculado("23,99x10^2"));//2399 ok
        //    dictResult.Add("-23,99x10^2", Guard.ConverteValorCalculado("-23,99x10^2"));//-2399 ok
        //    dictResult.Add("2399x10^2", Guard.ConverteValorCalculado("2399x10^2")); ;//239900 ok 
        //    dictResult.Add("-2399x10^2", Guard.ConverteValorCalculado("-2399x10^2"));//-239900 ok
        //    dictResult.Add("0,002399x10^2", Guard.ConverteValorCalculado("0,002399x10^2"));//0.2399 ok
        //    dictResult.Add("-0,002399x10^2", Guard.ConverteValorCalculado("-0,002399x10^2"));//-0.2399 ok

        //    /*Potencias Negativas*/
        //    dictResult.Add("23,99x10^-2", Guard.ConverteValorCalculado("23,99x10^-2"));//0.2399 ok
        //    dictResult.Add("-23,99x10^-2", Guard.ConverteValorCalculado("-23,99x10^-2"));//-0.2399 ok
        //    dictResult.Add("2399x10^-2", Guard.ConverteValorCalculado("2399x10^-2"));//23.99 ok 
        //    dictResult.Add("-2399x10^-2", Guard.ConverteValorCalculado("-2399x10^-2"));//-23.99 ok
        //    dictResult.Add("0,002399x10^-2", Guard.ConverteValorCalculado("0,002399x10^-2"));// 0.00002399 ok
        //    dictResult.Add("-0,002399x10^-2", Guard.ConverteValorCalculado("-0,002399x10^-2"));// -0.00002399 ok

        //    var retorno = "";
        //    foreach (var i in dictResult)
        //    {
        //        retorno += "Valor: " + i.Key + "\t\t Calculado: " + i.Value + "\n";
        //    }
        //    return retorno;
        //}

        //[HttpPost]
        //[Route("DesconverteValorCalculadoDecimal")]
        //public string DesconverteValorCalculadoDecimal()
        //{
        //    var dictResult = new Dictionary<decimal, string>();
        //    /*Potencias positivas*/
        //    dictResult.Add(Guard.ConverteValorCalculado("23,99x10^2"), Guard.ConverteValorCalculado(Guard.ConverteValorCalculado("23,99x10^2")));//2,399x10³
        //    dictResult.Add(Guard.ConverteValorCalculado("-23,99x10^2"), Guard.ConverteValorCalculado(Guard.ConverteValorCalculado("-23,99x10^2")));//-2,399x10³
        //    dictResult.Add(Guard.ConverteValorCalculado("2399x10^2"), Guard.ConverteValorCalculado(Guard.ConverteValorCalculado("2399x10^2")));//2,399x10^5
        //    dictResult.Add(Guard.ConverteValorCalculado("-2399x10^2"), Guard.ConverteValorCalculado(Guard.ConverteValorCalculado("-2399x10^2")));//-2,399x10^5
        //    dictResult.Add(Guard.ConverteValorCalculado("0,002399x10^2"), Guard.ConverteValorCalculado(Guard.ConverteValorCalculado("0,002399x10^2")));//0.2399 ok
        //    dictResult.Add(Guard.ConverteValorCalculado("-0,002399x10^2"), Guard.ConverteValorCalculado(Guard.ConverteValorCalculado("-0,002399x10^2")));//-0.2399 ok

        //    /*Potencias Negativas*/
        //    dictResult.Add(Guard.ConverteValorCalculado("21,99x10^-2"), Guard.ConverteValorCalculado(Guard.ConverteValorCalculado("21,99x10^-2")));//0.2399 ok
        //    dictResult.Add(Guard.ConverteValorCalculado("-21,99x10^-2"), Guard.ConverteValorCalculado(Guard.ConverteValorCalculado("-21,99x10^-2")));//-0.2399 ok
        //    dictResult.Add(Guard.ConverteValorCalculado("2199x10^-2"), Guard.ConverteValorCalculado(Guard.ConverteValorCalculado("2199x10^-2")));//23.99 ok 
        //    dictResult.Add(Guard.ConverteValorCalculado("-2199x10^-2"), Guard.ConverteValorCalculado(Guard.ConverteValorCalculado("-2199x10^-2")));//-23.99 ok
        //    dictResult.Add(Guard.ConverteValorCalculado("0,002199x10^-2"), Guard.ConverteValorCalculado(Guard.ConverteValorCalculado("0,002199x10^-2")));//2,399x10^5
        //    dictResult.Add(Guard.ConverteValorCalculado("-0,002199x10^-2"), Guard.ConverteValorCalculado(Guard.ConverteValorCalculado("-0,002199x10^-2")));// -2,399x10^5

        //    var retorno = "";
        //    foreach (var i in dictResult)
        //    {
        //        retorno += "Valor: " + i.Key.ToString() + "\t\t Valor: " + i.Value.ToString() + " \n";
        //    }
        //    return retorno;
        //}

    }
}
