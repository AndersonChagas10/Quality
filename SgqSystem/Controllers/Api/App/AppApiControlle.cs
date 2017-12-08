using ADOFactory;
using Dominio;
using DTO;
using DTO.Helpers;
using SgqSystem.Helpers;
using SgqSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.App
{
    /// <summary>
    /// 
    /// Gerencia em memória a tela da parametrização dos tablets para unidades.
    /// 
    /// Serviços disponíveis:
    /// 
    /// var root = @Html.Raw(Json.Encode(GlobalConfig.urlPreffixAppColleta));
    /// $.get(root +'/api/AppParams/UpdateTelaDoTablet', { }, function(r) { console.log(r)});
    /// $.get(root +'/api/AppParams/UpdateTelaDoTablet/21', {UnitId: 21 }, function(r) { console.log(r)});
    /// $.get(root +'/api/AppParams/ParamsDisponiveis', { }, function(r) { console.log(r)});
    /// $.get(root +'/api/AppParams/GetTela/21', { }, function(r) { console.log(r)});
    /// 
    /// </summary>
    [RoutePrefix("api/AppParams")]
    public class AppParamsApiController : BaseApiController
    {
        SgqDbDevEntities db;

        public AppParamsApiController()
        {
            db = new SgqDbDevEntities();
        }

        /// <summary>
        /// Sobrescreve a tela do tablet para todas as unidades.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("UpdateTelaDoTablet")]
        public RetornoParaTablet UpdateTelaDoTablet()
        {
            CommonLog.SaveReport("Update_GetTelaAll");

            GlobalConfig.ParamsDisponiveis = string.Empty;
            GlobalConfig.PaginaDoTablet = new Dictionary<int, HtmlDoTablet>();

            var units = db.ParCompany.Where(r => r.IsActive).ToList();
            using (var service = new SyncServices())
            {
                foreach (var i in units)
                {

                    var atualizado = service.getAPPLevels(56, i.Id, DateTime.Now);
                    try
                    {
                        GlobalConfig.PaginaDoTablet.Add(i.Id, new HtmlDoTablet() { Html = atualizado, DataFim = DateTime.Now, DataInicio = DateTime.Now });
                        GlobalConfig.ParamsDisponiveis += i.Id.ToString();
                    }
                    catch (Exception e)
                    {
                        new CreateLog(e, i);
                    }

                }
            }

            return new RetornoParaTablet() { ready = true };

        }

        /// <summary>
        /// Atualiza, se existir, a tela do tablet para determinada unidade.
        /// </summary>
        /// <param name="UnitId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("UpdateTelaDoTablet/{UnitId}")]
        public RetornoParaTablet UpdateTelaDoTablet(int UnitId)
        {
            CommonLog.SaveReport(UnitId, "Update_GetTela");

            if (GlobalConfig.PaginaDoTablet == null)
                GlobalConfig.PaginaDoTablet = new Dictionary<int, HtmlDoTablet>();

            CreateItemIfNotExist(UnitId);
            GlobalConfig.PaginaDoTablet[UnitId].Status = HtmlDoTablet.StatusType.PROCESSANDO;
            GlobalConfig.PaginaDoTablet[UnitId].DataInicio = DateTime.Now;

            using (var service = new SyncServices())
            {
                var atualizado = service.getAPPLevels(56, UnitId, DateTime.Now);/*Cria tela atualizada*/
                try
                {
                    if (GlobalConfig.PaginaDoTablet[UnitId] != null)/*Se ja existir atualiza*/
                    {
                        GlobalConfig.PaginaDoTablet[UnitId].Html = atualizado;
                        GlobalConfig.PaginaDoTablet[UnitId].DataFim = DateTime.Now;
                        GlobalConfig.PaginaDoTablet[UnitId].Status = HtmlDoTablet.StatusType.SUCESSO;
                    }
                    else/*Se nao existir cria*/
                    {
                        GlobalConfig.PaginaDoTablet.Add(UnitId, new HtmlDoTablet() { Html = atualizado, DataFim = DateTime.Now, DataInicio = DateTime.Now, Status = HtmlDoTablet.StatusType.SUCESSO });
                        GlobalConfig.ParamsDisponiveis += UnitId.ToString();
                    }
                }
                catch (Exception e)
                {
                    new CreateLog(e, UnitId);
                }
                System.GC.Collect();
            }

            return GetTela(UnitId);

        }

        /// <summary>
        /// Faz download de todas as telas prontas / atualizadas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ParamsDisponiveis")]
        public Dictionary<int, HtmlDoTablet> ParamsDisponiveis()
        {
            return GlobalConfig.PaginaDoTablet;
        }

        /// <summary>
        /// Responde a tela de uma unidade para o tablet
        /// </summary>
        /// <param name="UnitId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTela/{UnitId}")]
        public RetornoParaTablet GetTela(int UnitId)
        {
            var retorno = new RetornoParaTablet();
            if (GlobalConfig.PaginaDoTablet != null)
            {
                if (GlobalConfig.PaginaDoTablet.ContainsKey(UnitId))
                {
                    retorno.ParteDaTela = GlobalConfig.PaginaDoTablet.FirstOrDefault(r => r.Key == UnitId).Value.Html;
                    return retorno;
                }
            }

            //throw new Exception();

            UpdateTelaDoTablet(UnitId);
            retorno.ParteDaTela = GlobalConfig.PaginaDoTablet.FirstOrDefault(r => r.Key == UnitId).Value.Html;
            return retorno;
        }

        [HttpGet]
        [Route("UpdateDbRemoto/{UnitId}")]
        public void UpdateDbRemoto(int UnitId)
        {
            var user = "sa";
            var pass = "1qazmko0";
            var unidade = db.ParCompany.FirstOrDefault(r => r.Id == UnitId);
            using (var dbADO = new Factory(unidade.IPServer, unidade.DBServer, pass, user))
            {
                //var listaDeUsuarios = UpdateListaDeUsuarios(UnitId);
                var tela = GetTela(UnitId);
                dbADO.InsertUpdateData(tela);

            }
        }

        [HttpGet]
        [Route("GetUnits")]
        public List<int> GetUnits()
        {
            return db.ParCompany.AsNoTracking().Select(r => r.Id).ToList();
        }

        //[HttpGet]
        //[Route("UpdateListaDeUsuarios/{UnitId}")]
        //public Dictionary<int, string> UpdateListaDeUsuarios(int UnitId)
        //{
        //    using (var service = new SyncServices())
        //    {
        //        service.getCompanyUsers(UnitId.ToString()));
        //    }
        //    return retorno;
        //}

        #region Nova Proposta Get Tela

        public class GeneratedUnit
        {
            public List<int> ListUnits { get; set; }
        }

        [HttpGet]
        [Route("GetStackTrace/{id}")]
        public object GetStackTrace(int id)
        {
            if (GlobalConfig.PaginaDoTablet.ContainsKey(id))
            {
                return GlobalConfig.PaginaDoTablet[id].StackTrace;
            }
            return null;
        }

        [HttpPost]
        [Route("GetGeneratedUnits")]
        public object GetGeneratedUnits([FromBody]GeneratedUnit generatedUnit)
        {
            if (GlobalConfig.PaginaDoTablet == null)
                GlobalConfig.PaginaDoTablet = new Dictionary<int, HtmlDoTablet>();

            if (generatedUnit?.ListUnits?.Count > 0)
            {
                foreach (var temp in generatedUnit.ListUnits)
                {
                    if (!GlobalConfig.PaginaDoTablet.ContainsKey(temp))
                    {
                        GlobalConfig.PaginaDoTablet.Add(temp, null);
                    }
                }
                return GlobalConfig.PaginaDoTablet.Select(pt => new { pt.Key, pt.Value?.DataFimStr, pt.Value?.DataInicioStr, pt.Value?.StatusStr });//.Select(t=>new {ID = t.Key, DataInicio = t.Value.DataInicio, DataFim = t.Value.DataFim, Html = t.Value.Html });

            }
            return null;
        }

        private static Semaphore Pool;

        [HttpPost]
        [Route("UpdateGetTelaThread")]
        public void UpdateGetTelaThread([FromBody]GeneratedUnit generatedUnit)
        {
            if (GlobalConfig.PaginaDoTablet == null)
                GlobalConfig.PaginaDoTablet = new Dictionary<int, HtmlDoTablet>();

            if (generatedUnit.ListUnits != null && generatedUnit.ListUnits.Count > 0)
            {
                Queue<Thread> threadBuffer = new Queue<Thread>();

                Pool = new Semaphore(5, 5);

                foreach (int i in generatedUnit.ListUnits)
                {
                    Thread thread = new Thread(() => this.ThreadManager(i));
                    threadBuffer.Enqueue(thread);
                }

                while (threadBuffer.Count > 0)
                {
                    Thread t = threadBuffer.Dequeue();
                    t.Start();
                }
            }
        }

        private void ThreadManager(int id)
        {
            try
            {
                CreateItemIfNotExist(id);

                Pool.WaitOne();
                if (GlobalConfig.PaginaDoTablet != null
                    &&
                    ((GlobalConfig.PaginaDoTablet[id] != null
                        &&
                        (GlobalConfig.PaginaDoTablet[id].DataFim != null
                        || GlobalConfig.PaginaDoTablet[id].DataInicio == null))
                    ||
                    GlobalConfig.PaginaDoTablet[id] == null))
                {
                    UpdateTelaDoTablet(id);
                }
            }
            catch (Exception ex)
            {
                if (GlobalConfig.PaginaDoTablet.ContainsKey(id) && GlobalConfig.PaginaDoTablet[id] != null)
                {
                    GlobalConfig.PaginaDoTablet[id].DataFim = DateTime.Now;
                    GlobalConfig.PaginaDoTablet[id].Status = HtmlDoTablet.StatusType.ERROR;
                    GlobalConfig.PaginaDoTablet[id].StackTrace = ex.StackTrace;

                }
            }
            finally
            {
                Pool.Release();
            }
        }

        private void CreateItemIfNotExist(int id)
        {
            if (!GlobalConfig.PaginaDoTablet.ContainsKey(id))
            {
                GlobalConfig.PaginaDoTablet.Add(id, new HtmlDoTablet() { });
            }
            else
            {
                GlobalConfig.PaginaDoTablet[id] = new HtmlDoTablet() { };
            }
        }

        #endregion


    }


    /// <summary>
    /// Objeto de auxilio para retorno.
    /// </summary>
    public class RetornoParaTablet
    {
        public bool ready { get; set; }
        public int pool { get; set; }
        public string ParteDaTela { get; set; }
    }

}
