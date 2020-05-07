using PlanoAcaoCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;

namespace PlanoDeAcaoMVC.Controllers.Api
{
    [RoutePrefix("api/Generics")]
    public class GenericsController : BaseApiController
    {
        [HttpPost]
        [Route("Save")]
        public GenericInsertPa Save(GenericInsertPa valores)
        {
            var idSalvo = 0;
            var table = string.Empty;
            string fk = string.Empty;

            try
            {
                if (String.IsNullOrEmpty(valores.NomeDoItem))
                {
                    valores.Resposta = "Valor informado é invalido.";
                    return valores;
                }

                EncontraTabelaParametrosParaQuery(valores.ParametroDeBusca, ref table, ref fk);

                if (valores.Id > 0)
                    idSalvo = UpdateGenericInsertPa(valores, table, fk);
                else
                    idSalvo = SaveGenericInsertPa(valores, table, fk);

                if (idSalvo > 0)
                    valores.Id = idSalvo;
                else
                    valores.Resposta = "Valor informado já existe. Não pode ser duplicado.";
            }
            catch (Exception)
            {
                valores.Resposta = "Ocorreu um erro durante a tentativa de processar o dado.";
            }

            return valores;

        }

        [HttpPost]
        [Route("Get")]
        public dynamic Get(GenericInsertPa valores)
        {
            var table = string.Empty;
            string fk = string.Empty;
            string isPriority = string.Empty;

            EncontraTabelaParametrosParaQuery(valores.ParametroDeBusca, ref table, ref fk);

            var retorno = new List<Pa_BaseObject.Generico>();

            if (table == "Pa_Objetivo")
            {
                isPriority = ",[IsPriority]";
            }

            if (valores.PredecessorId > 0)
                retorno = Pa_BaseObject.ListarGenerico<Pa_BaseObject.Generico>(
                    $@"SELECT [Id],[Name],[IsActive] { isPriority } FROM [dbo].[{ table }] 
                    WHERE { fk } = {valores.PredecessorId.GetValueOrDefault()}"
                    ).ToList();
            else
                retorno = Pa_BaseObject.ListarGenerico<Pa_BaseObject.Generico>(
                    $@"SELECT [Id],[Name],[IsActive] { isPriority } FROM [dbo].[{ table }] "
                    ).ToList();
            return retorno;

        }

        [HttpPost]
        [Route("Delete")]
        public GenericInsertPa Delete(GenericInsertPa valores)
        {
            throw new NotImplementedException("Ação não implementada.");
        }

        private static int SaveGenericInsertPa(GenericInsertPa valores, string table, string fk)
        {
            int idSalvo;
            if (valores.PredecessorId > 0)
                idSalvo = Pa_BaseObject.GenericInsertIfNotExists(valores.NomeDoItem, table, valores.PredecessorId.GetValueOrDefault(), fk, valores.IsPriority);
            else
                idSalvo = Pa_BaseObject.GenericInsertIfNotExists(valores.NomeDoItem, table);
            return idSalvo;
        }

        private static int UpdateGenericInsertPa(GenericInsertPa valores, string table, string fk)
        {
            int idSalvo;
            if (valores.PredecessorId > 0)
                idSalvo = Pa_BaseObject.GenericUpdateIfUnique(valores.NomeDoItem, table, valores.IsActive, valores.PredecessorId.GetValueOrDefault(), fk, valores.Id, valores.IsPriority);
            else
                idSalvo = Pa_BaseObject.GenericUpdateIfUnique(valores.NomeDoItem, table, valores.IsActive, valores.Id);
            return idSalvo;
        }

        private void EncontraTabelaParametrosParaQuery(string param, ref string table, ref string fk)
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
                case "TemaProjeto":
                    table = "Pa_TemaProjeto";
                    break;
                case "Diretoria":
                    table = "Pa_Diretoria";
                    break;
                case "Missao":
                    table = "Pa_Missao";
                    break;
                case "Visao":
                    table = "Pa_Visao";
                    break;
                case "TipoProjeto":
                    table = "Pa_TipoProjeto";
                    break;
                default:
                    break;
            }
        }

        public class GenericInsertPa
        {
            public string NomeDoItem { get; set; }
            public string ParametroDeBusca { get; set; }
            public int? PredecessorId { get; set; } = 0;
            public string Resposta { get; set; }
            public bool IsActive { get; set; }
            public int Id { get; set; } = 0;
            public bool? IsPriority { get; set; }
        }

        [HttpPost]
        [Route("GetDiretriz/{id}")]
        public Pa_Objetivo GetDiretriz(int id)
        {
            return Pa_Objetivo.Get(id);
        }

    }


}
