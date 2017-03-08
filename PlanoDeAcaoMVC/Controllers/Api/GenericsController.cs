using PlanoAcaoCore;
using System.Web.Http;

namespace PlanoDeAcaoMVC.Controllers.Api
{
    [RoutePrefix("api/Generics")]
    public class GenericsController : ApiController
    {
        [HttpGet]
        [Route("Save/{val}/{param}/{predecessor}")]
        public int Save(string val, string param, int? predecessor = 0)
        {

            var retorno = 0;
            var table = string.Empty;
            string fk = string.Empty;

            switch (param)
            {
                case "TemaAssunto":
                    table = "Pa_TemaAssunto";
                    break;
                case "Objetivo":
                    table = "Pa_Objetivo";
                    fk = "Pa_Dimensao_Id";
                    break;
                case "Indicadores":
                    table = "Pa_Indicadores";
                    break;
                case "ObjetivoGerencial":
                    table = "Pa_ObjetivoGeral";
                    fk = "Pa_IndicadoresDeProjeto_Id";
                    break;
                case "IndicadoresDeProjeto":
                    table = "Pa_IndicadoresDeProjeto";
                    fk = "Pa_Iniciativa_Id";
                    break;
                case "IndicadoresDiretriz":
                    table = "Pa_IndicadoresDiretriz";
                    fk = "Pa_Objetivo_Id";
                    break;
                case "Iniciativa":
                    table = "Pa_Iniciativa";
                    break;
                default:
                    break;
            }

            if(predecessor > 0)
                retorno = Pa_BaseObject.GenericInsert(val, table, predecessor.GetValueOrDefault(), fk);
            else
                retorno = Pa_BaseObject.GenericInsert(val, table);

            return retorno;

        }
    }

  
}
