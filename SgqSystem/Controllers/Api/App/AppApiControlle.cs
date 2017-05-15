using Dominio;
using DTO;
using DTO.Helpers;
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

        [HttpGet]
        [Route("UpdateTelaDoTablet")]
        public RetornoParaTablet UpdateTelaDoTablet()
        {

            GlobalConfig.ParamsDisponiveis = string.Empty;
            GlobalConfig.PaginaDoTablet = new Dictionary<int, string>();

            using (var db = new SgqDbDevEntities())
            {
                var units = db.ParCompany.Where(r => r.IsActive).ToList();
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
        [Route("UpdateTelaDoTablet/{UnitId}")]
        public RetornoParaTablet UpdateTelaDoTablet(int UnitId)
        {

            if (GlobalConfig.PaginaDoTablet == null)
                GlobalConfig.PaginaDoTablet = new Dictionary<int, string>();

            using (var service = new SyncServices())
            {
                var atualizado = service.getAPPLevels(1, UnitId, DateTime.Now);/*Cria tela atualizada*/
                try
                {
                    if (GlobalConfig.PaginaDoTablet.ContainsKey(UnitId))/*Se ja existir atualiza*/
                    {
                        GlobalConfig.PaginaDoTablet[UnitId] = atualizado;
                    }
                    else/*Se nao existir cria*/
                    {
                        GlobalConfig.PaginaDoTablet.Add(UnitId, atualizado);
                        GlobalConfig.ParamsDisponiveis += UnitId.ToString();
                    }
                }
                catch (Exception e)
                {
                    new CreateLog(e, UnitId);
                }
            }

            return GetTela(UnitId);

        }

        //[HttpGet]
        //[Route("UpdateTelaDoTabletByUser/{UserId}")]
        //public RetornoParaTablet UpdateTelaDoTabletByUser(int UserId)
        //{
        //    using (var db = new SgqDbDevEntities())
        //    {
        //        var UnitId = db.UserSgq.FirstOrDefault(r => r.Id == UserId).ParCompany_Id.GetValueOrDefault();
        //        return UpdateTelaDoTablet(UnitId);
        //    }
        //}

        //$.get('http://mtzsvmqsc/Teste/api/AppParams/ParamsDisponiveis', { UnitId: 1 }, function(r) { console.log(r)});
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
            if (GlobalConfig.PaginaDoTablet != null)
            {
                if (GlobalConfig.PaginaDoTablet.ContainsKey(UnitId))
                {
                    retorno.ParteDaTela = GlobalConfig.PaginaDoTablet.FirstOrDefault(r => r.Key == UnitId).Value;
                    return retorno;
                }
            }

            UpdateTelaDoTablet(UnitId);
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
