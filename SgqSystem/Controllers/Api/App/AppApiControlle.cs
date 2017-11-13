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
    public class AppParamsApiController : ApiController
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
            GlobalConfig.PaginaDoTablet = new Dictionary<int, string>();

            var units = db.ParCompany.Where(r => r.IsActive).ToList();
            using (var service = new SyncServices())
            {
                foreach (var i in units)
                {

                    var atualizado = service.getAPPLevels(56, i.Id, DateTime.Now);
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
            DateTime dtStart = DateTime.Now;
            CommonLog.SaveReport(UnitId, "Update_GetTela");

            if (GlobalConfig.PaginaDoTablet == null)
                GlobalConfig.PaginaDoTablet = new Dictionary<int, string>();

            using (var service = new SyncServices())
            {
                var atualizado = service.getAPPLevels(56, UnitId, DateTime.Now);/*Cria tela atualizada*/
                double sec = (DateTime.Now - dtStart).TotalMilliseconds;
                try
                {
                    if (GlobalConfig.PaginaDoTablet.ContainsKey(UnitId))/*Se ja existir atualiza*/
                    {
                        GlobalConfig.PaginaDoTablet[UnitId] = sec.ToString();// atualizado;
                    }
                    else/*Se nao existir cria*/
                    {
                        GlobalConfig.PaginaDoTablet.Add(UnitId, sec.ToString()/*atualizado*/);
                        GlobalConfig.ParamsDisponiveis += UnitId.ToString();
                    }
                }
                catch (Exception e)
                {
                    new CreateLog(e, UnitId);
                }
            }

            GetTela(UnitId);
            return null;

        }

        /// <summary>
        /// Faz download de todas as telas prontas / atualizadas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ParamsDisponiveis")]
        public Dictionary<int, string> ParamsDisponiveis()
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
                    retorno.ParteDaTela = GlobalConfig.PaginaDoTablet.FirstOrDefault(r => r.Key == UnitId).Value;
                    return retorno;
                }
            }

            //throw new Exception();

            UpdateTelaDoTablet(UnitId);
            retorno.ParteDaTela = GlobalConfig.PaginaDoTablet.FirstOrDefault(r => r.Key == UnitId).Value;
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


        [HttpGet]
        [Route("GetGeneratedUnits")]
        public Dictionary<int,string> GetGeneratedUnits()
        {
            return GlobalConfig.PaginaDoTablet;//?.Select(u => u.Key).ToList();
        }

        private static Semaphore Pool;

        [HttpGet]
        [Route("UpdateGetTelaThread")]
        public void UpdateGetTelaThread()
        {
            //_runningThread = 0;
            int index = 0;
            List<int> listUnitIds = GetUnits();

            for (int i = 0; i < GlobalConfig.PaginaDoTablet?.Count; i++)
                GlobalConfig.PaginaDoTablet[i] = string.Empty;

            //listUnitIds.ForEach(u => new Thread(() => this.ThreadManager(u)).Start());

            Queue<Thread> threadBuffer = new Queue<Thread>();

            Pool = new Semaphore(20,20);

            foreach(int i in listUnitIds)
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

        private void ThreadManager(int id)
        {
            try
            {
                Pool.WaitOne();
                UpdateTelaDoTablet(id);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                Pool.Release();
            }

            /*bool run;
            while (true)
            {
                run = false;
                Thread.Sleep(new Random().Next(200, 500));
                if (_maxRunningThread > _runningThread)
                {
                    ++_runningThread;
                    run = true;
                }

                if (run)
                {
                    UpdateTelaDoTablet(id);
                    --_runningThread;
                    return;
                }else
                {
                    Thread.Sleep(new Random().Next(500, 1000));
                }
            }*/
        }

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
