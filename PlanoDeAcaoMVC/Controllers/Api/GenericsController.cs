using PlanoAcaoCore;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace PlanoDeAcaoMVC.Controllers.Api
{
    [RoutePrefix("api/Generics")]
    public class GenericsController : ApiController
    {
        [HttpPost]
        [Route("Save")]
        public int Save(GenericInsertPa valores)
        {

            var retorno = 0;
            var table = string.Empty;
            string fk = string.Empty;

            SwitchParam(valores.param, ref table, ref fk);

            if (valores.predecessor > 0)
                retorno = Pa_BaseObject.GenericInsertOrUpdate(valores.val, table, valores.predecessor.GetValueOrDefault(), fk);
            else
                retorno = Pa_BaseObject.GenericInsert(valores.val, table);

            return retorno;

        }
        [HttpPost]
        [Route("Update")]
        public int Update(GenericUpdatePa valores)
        {
            var retorno = 0;
            var table = string.Empty;
            string fk = string.Empty;

            SwitchParam(valores.param, ref table, ref fk);

            if (valores.predecessor > 0)
                retorno = Pa_BaseObject.GenericInsertOrUpdate(valores.val, table, valores.predecessor.GetValueOrDefault(), fk);
            else
                retorno = Pa_BaseObject.GenericInsert(valores.val, table);

            return retorno;

        }


        [HttpPost]
        [Route("Get")]
        public List<Pa_BaseObject.Generico> Get(GenericInsertPa valores)
        {
            var table = string.Empty;
            string fk = string.Empty;

            SwitchParam(valores.param, ref table, ref fk);

            var retorno = new List<Pa_BaseObject.Generico>();
            if (valores.predecessor > 0)
                retorno = Pa_BaseObject.ListarGenerico<Pa_BaseObject.Generico>(
                    $@"SELECT [Id],[Name] FROM [dbo].[{ table }] 
                    WHERE { fk } = {valores.predecessor.GetValueOrDefault()}"
                    ).ToList();
            else
                retorno = Pa_BaseObject.ListarGenerico<Pa_BaseObject.Generico>(
                    $@"SELECT [Id],[Name] FROM [dbo].[{ table }] "
                    ).ToList();

            return retorno;

        }

        private void SwitchParam(string param, ref string table, ref string fk)
        {

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
        }

        public class GenericInsertPa
        {
            public string val { get; set; }
            public string param { get; set; }
            public int? predecessor { get; set; } = 0;
        }

        public class GenericUpdatePa
        {
            public string val { get; set; }
            public string param { get; set; }
            public int? predecessor { get; set; } = 0;
            public int id { get; set; } = 0;
        }
    }

  
}
