using SgqSystem.ViewModels;
using System.Collections.Generic;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.RelatoriosBrasil
{
    [RoutePrefix("api/CartasCep")]
    public class CartasCepApiController : ApiController
    {

        [HttpPost]
        [Route("Get")]
        public CartasCepResultSet GetApontamentosDiarios([FromBody] FormularioParaRelatorioViewModel form)
        {
            CartasCepResultSet _mockcartasCep = new CartasCepResultSet();

            decimal[] dados1 = { 4.11M, 4.2M, 3.93M, 3.24M, 3.5M };
            string[] dataAv1 = { "10/05/2015 18:33:56", "10/05/2015 18:34:30", "10/05/2015 18:35:04", "10/05/2015 18:35:36", "10/05/2015 18:36:21" };
            decimal[] lciData1 = { 3.05M, 3.05M, 3.05M, 3.05M, 3.05M };
            //decimal Ics1;
            decimal[] lcsData1 = { 4.25M, 4.25M, 4.25M, 4.25M, 4.25M };
            //decimal media1;
            int[] nivel1Max1 = { 1, 1, 1, 1, 1 };
            int[] nivel1Min1 = { 0, 0, 0, 0, 0 };
            int[] nivel2Max1 = { 2, 2, 2, 2, 2 };
            int[] nivel2Min1 = { 1, 1, 1, 1, 1 };
            int[] nivel3Max1 = { 3, 3, 3, 3, 3 };
            int[] nivel3Min1 = { 2, 2, 2, 2, 2 };

            _mockcartasCep.dados = dados1;
            _mockcartasCep.dataAv = dataAv1;
            _mockcartasCep.lci = 3.05M;
            _mockcartasCep.lciData = lciData1;
            _mockcartasCep.lcs = 4.25M;
            _mockcartasCep.lcsData = lcsData1;
            _mockcartasCep.media = 3.648275862068966M;
            _mockcartasCep.nivel1Max = nivel1Max1;
            _mockcartasCep.nivel1Min = nivel1Min1;
            _mockcartasCep.nivel2Max = nivel2Max1;
            _mockcartasCep.nivel2Min = nivel2Min1;
            _mockcartasCep.nivel3Max = nivel3Max1;
            _mockcartasCep.nivel3Min = nivel3Min1;
            _mockcartasCep.amostras = null;

            //var query = "Select";

            //var query = new ApontamentosDiariosResultSet().Select(form);
            //_list = db.Database.SqlQuery<ApontamentosDiariosResultSet>(query).ToList();
            //return _list;

            return _mockcartasCep;
        }

    }

    public class CartasCepResultSet
    {
        //public List<Chartdata> chartData;
        public decimal[] amostras;
        public decimal[] dados;
        public string[] dataAv;
        public decimal lci;
        public decimal[] lciData;
        public decimal lcs;
        public decimal[] lcsData;
        public decimal media;
        public int[] nivel1Max;
        public int[] nivel1Min;
        public int[] nivel2Max;
        public int[] nivel2Min;
        public int[] nivel3Max;
        public int[] nivel3Min;
    }

    //public class Chartdata
    //{
    //    public List<string> value;
    //    public decimal height;
    //    public string nivel1Maximo;
    //    public string nivel1Minimo;
    //    public string nivel2Maximo;
    //    public string nivel2Minimo;
    //    public string nivel3Maximo;
    //    public string nivel3Minimo;
    //}
}
