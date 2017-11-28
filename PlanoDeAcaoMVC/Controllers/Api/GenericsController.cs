using PlanoAcaoCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;

namespace PlanoDeAcaoMVC.Controllers.Api
{
    [RoutePrefix("api/Generics")]
    public class GenericsController : ApiController
    {
        [HttpPost]
        [Route("Save")]
        public GenericInsertPa Save(GenericInsertPa valores)
        {
            try
            {

                if (String.IsNullOrEmpty(valores.val))
                {
                    valores.resposta = "Valor informado é invalido.";
                    return valores;
                }

                var retorno = 0;
                var table = string.Empty;
                string fk = string.Empty;

                SwitchParam(valores.param, ref table, ref fk);

                if (valores.predecessor > 0)
                    retorno = Pa_BaseObject.GenericInsertIfNotExists(valores.val, table, valores.predecessor.GetValueOrDefault(), fk);
                else
                    retorno = Pa_BaseObject.GenericInsertIfNotExists(valores.val, table);

                if (retorno == 0)
                {
                    valores.resposta = "Valor informado já existe. Não pode ser duplicado.";
                }

            }
            catch (Exception ex)
            {
                valores.resposta = "Ocorreu um erro durante a tentativa de processar o dado.";
            }

            return valores;

        }
        [HttpPost]
        [Route("Update")]
        public GenericUpdatePa Update(GenericUpdatePa valores)
        {
            try
            {
                var retorno = 0;

                if (String.IsNullOrEmpty(valores.val))
                {
                    valores.resposta = "Valor informado é invalido.";
                    return valores;
                }
                if (!(valores.id > 0))
                {
                    valores.resposta = "Valor do ID é inválido.";
                    return valores;
                }

                var table = string.Empty;
                string fk = string.Empty;

                SwitchParam(valores.param, ref table, ref fk);

                if (valores.predecessor > 0)
                    retorno = Pa_BaseObject.GenericUpdateIfUnique(valores.val, table, valores.isActive, valores.predecessor.GetValueOrDefault(), fk, valores.id);
                else
                    retorno = Pa_BaseObject.GenericUpdateIfUnique(valores.val, table, valores.isActive, valores.id);


                if (retorno == 0)
                {
                    valores.resposta = "Valor informado já existe. Não pode ser duplicado.";
                }

            }
            catch (Exception ex)
            {
                valores.resposta = "Ocorreu um erro durante a tentativa de processar o dado.";
            }

            return valores;

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
                    $@"SELECT [Id],[Name],[IsActive] FROM [dbo].[{ table }] 
                    WHERE { fk } = {valores.predecessor.GetValueOrDefault()}"
                    ).ToList();
            else
                retorno = Pa_BaseObject.ListarGenerico<Pa_BaseObject.Generico>(
                    $@"SELECT [Id],[Name],[IsActive] FROM [dbo].[{ table }] "
                    ).ToList();

            return retorno;

        }


        [HttpPost]
        [Route("Delete")]
        public GenericInsertPa Delete(GenericInsertPa valores)
        {
            var table = string.Empty;
            string fk = string.Empty;

            SwitchParam(valores.param, ref table, ref fk);

            return valores;

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
                case "Gerencia":
                    table = "PA_GERENCIA";
                    break;
                case "Coordenacao":
                    table = "PA_COORDENACAO";
                    fk = "GERENCIA_ID";
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
            public string resposta { get; set; }
        }

        public class GenericUpdatePa : GenericInsertPa
        {
            public bool isActive { get; set; }
            public int id { get; set; } = 0;
        }
    }


}
