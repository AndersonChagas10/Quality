using Dominio;
using DTO;
using SgqSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.App
{
    [RoutePrefix("api/AppParams")]
    public class AppParamsApiController : ApiController
    {
        private SgqDbDevEntities db;
        [HttpGet]
        [Route("UpdateTelaDoTablet")]
        public RetornoParaTablet UpdateTelaDoTablet()
        {
            GlobalConfig.ParamsDisponiveis = string.Empty;
            GlobalConfig.PaginaDoTablet = new Dictionary<int, string>();

            using (var db = new SgqDbDevEntities())
            {
                var units = db.ParCompany.ToList();
                using (var service = new SyncServices())
                {
                    foreach (var i in units)
                    {

                        var atualizado = service.getAPPLevels(1, i.Id, DateTime.Now);
                        try
                        {
                            GlobalConfig.PaginaDoTablet.Add(i.Id, atualizado);
                            GlobalConfig.ParamsDisponiveis += i.Id.ToString();
                        }
                        catch (Exception e)
                        {
                            new CreateLog(e, i);
                        }

                    }
                }
            }

            return new RetornoParaTablet() { ready = true };
        }

        [HttpGet]
        [Route("ParamsDisponiveis")]
        public Dictionary<int, string> ParamsDisponiveis(int UnitId)
        {
            return GlobalConfig.PaginaDoTablet;
        }


        [HttpGet]
        [Route("GetTela/{UnitId}")]
        public RetornoParaTablet GetTela(int UnitId)
        {
            var retorno = new RetornoParaTablet();
            retorno.ParteDaTela = GlobalConfig.PaginaDoTablet.FirstOrDefault(r => r.Key == UnitId).Value;
            return retorno;
        }
    }

    public class RetornoParaTablet
    {
        public bool ready { get; set; }
        public int pool { get; set; }
        public string ParteDaTela { get; set; }
    }

}