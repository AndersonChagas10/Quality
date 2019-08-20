using ADOFactory;
using Dominio;
using DTO;
using DTO.Helpers;
using Newtonsoft.Json;
using ServiceModel;
using SgqServiceBusiness.Helpers;
using SgqServiceBusiness.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;

namespace SgqServiceBusiness.Api.App
{
    public class AppParamsApiController : IDisposable
    {
        SgqDbDevEntities db;
        private List<Shift> listaDeShift;
        private string conexao;

        public AppParamsApiController(string conexao)
        {
            this.conexao = conexao;
            db = new SgqDbDevEntities();
            listaDeShift = db.Shift.ToList();
        }

        /// <summary>
        /// Sobrescreve a tela do tablet para todas as unidades.
        /// </summary>
        /// <returns></returns>
        public RetornoParaTablet UpdateTelaDoTablet()
        {
            CommonLog.SaveReport("Update_GetTelaAll");

            GlobalConfig.ParamsDisponiveis = string.Empty;

            if (GlobalConfig.PaginaDoTablet == null)
                GlobalConfig.PaginaDoTablet = new Dictionary<int, HtmlDoTablet>();

            var units = db.ParCompany.Where(r => r.IsActive).ToList();

            var shifts = db.Shift.ToList();
            shifts.Insert(0, new Shift());

            var service = new SyncServiceApiController(conexao, conexao);

            foreach (var shift in shifts)
            {
                foreach (var i in units)
                {
                    var atualizado = service.getAPPLevels(56, i.Id, DateTime.Now, shift.Id);
                    try
                    {
                        this.SaveFile(i.Id, atualizado, shift.Id);
                        GlobalConfig.PaginaDoTablet.Add(i.Id, new HtmlDoTablet() { /*Html = atualizado,*/ DataFim = DateTime.Now, DataInicio = DateTime.Now });
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
        public RetornoParaTablet UpdateTelaDoTablet(int UnitId)
        {
            CommonLog.SaveReport(UnitId, "Update_GetTela");

            if (GlobalConfig.PaginaDoTablet == null)
                GlobalConfig.PaginaDoTablet = new Dictionary<int, HtmlDoTablet>();

            CreateItemIfNotExist(UnitId);
            GlobalConfig.PaginaDoTablet[UnitId].Status = HtmlDoTablet.StatusType.PROCESSANDO;
            GlobalConfig.PaginaDoTablet[UnitId].DataInicio = DateTime.Now;

            var shifts = this.listaDeShift;
            shifts.Insert(0, new Shift());

            var service = new SyncServiceApiController(conexao, conexao);

            for (int i = 0; i < shifts.Count; i++)
            {
                var shift = shifts[i];
                var atualizado = service.getAPPLevels(56, UnitId, DateTime.Now, shift.Id);/*Cria tela atualizada*/

                try
                {
                    if (GlobalConfig.PaginaDoTablet[UnitId] != null)/*Se ja existir atualiza*/
                    {
                        GlobalConfig.PaginaDoTablet[UnitId].DataFim = DateTime.Now;
                        //GlobalConfig.PaginaDoTablet[UnitId].Status = HtmlDoTablet.StatusType.SUCESSO;
                    }
                    else/*Se nao existir cria*/
                    {
                        GlobalConfig.PaginaDoTablet.Add(UnitId, new HtmlDoTablet()
                        {
                            DataFim = DateTime.Now,
                            DataInicio = DateTime.Now,
                            //Status = HtmlDoTablet.StatusType.SUCESSO
                        });
                        GlobalConfig.ParamsDisponiveis += UnitId.ToString();
                    }

                    this.SaveFile(UnitId, atualizado, shift.Id);

                    if (shifts.Count - 1 == i)
                    {
                        GlobalConfig.PaginaDoTablet[UnitId].Status = HtmlDoTablet.StatusType.SUCESSO;
                    }
                }
                catch (Exception e)
                {
                    new CreateLog(e, UnitId);
                }

                System.GC.Collect();
            }

            return null;// GetTela(UnitId);

        }

        /// <summary>
        /// Faz download de todas as telas prontas / atualizadas
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, HtmlDoTablet> ParamsDisponiveis()
        {
            return GlobalConfig.PaginaDoTablet;
        }

        /// <summary>
        /// Responde a tela de uma unidade para o tablet
        /// </summary>
        /// <param name="UnitId"></param>
        /// <returns></returns>
        public RetornoParaTablet GetTela(int UnitId, int ShiftId = 0)
        {
            var retorno = new RetornoParaTablet();

            var shifts = db.Shift.ToList();
            shifts.Insert(0, new Shift());

            try
            {
                if (GlobalConfig.PaginaDoTablet != null)
                {
                    if (GlobalConfig.PaginaDoTablet.ContainsKey(UnitId))
                    {

                        retorno.ParteDaTela = this.GetFile(UnitId, ShiftId);

                        if (retorno.ParteDaTela != null && GlobalConfig.PaginaDoTablet[UnitId]?.DataFim > DateTime.Now.Date.AddHours(-3))
                            return retorno;
                    }
                }

                UpdateTelaDoTablet(UnitId);

                //foreach (var shift in shifts)
                //{
                retorno.ParteDaTela = this.GetFile(UnitId, ShiftId);
                //}
            }
            catch (Exception ex)
            {
                new CreateLog(new Exception("GetTela - " + ex.Message, ex), UnitId);
            }
            return retorno;
        }

        public void UpdateDbRemoto(int UnitId)
        {
            var user = "sa";
            var pass = "1qazmko0";
            var unidade = db.ParCompany.FirstOrDefault(r => r.Id == UnitId);
            using (var dbADO = new Factory(unidade.IPServer, unidade.DBServer, pass, user))
            {
                //var listaDeUsuarios = UpdateListaDeUsuarios(UnitId);
                var tela = GetTela(UnitId, 1);
                dbADO.InsertUpdateData(tela);

            }
        }

        public List<int> GetUnits()
        {
            return db.ParCompany.AsNoTracking().Select(r => r.Id).ToList();
        }

        public string GetFiles()
        {

            var app = DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.AppFiles;

            return new AppScriptsController().GetByVersion(app);
        }

        public string GetAPP()
        {
            var html = new Html();

            var teste = new SyncServiceApiController(conexao, conexao);

            string login = teste.GetLoginAPP();

            string resource = teste.GetResource();

            return login + resource;
        }

        #region Nova Proposta Get Tela

        public object GetStackTrace(int id)
        {
            if (GlobalConfig.PaginaDoTablet.ContainsKey(id))
            {
                return GlobalConfig.PaginaDoTablet[id].StackTrace;
            }
            return null;
        }

        public object GetGeneratedUnits(GeneratedUnit generatedUnit)
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
                return GlobalConfig.PaginaDoTablet.Select(pt => new { pt.Key, pt.Value?.DataFimStr, pt.Value?.DataInicioStr, pt.Value?.StatusStr });

            }
            return null;
        }

        public void UpdateGetTelaThread(GeneratedUnit generatedUnit)
        {
            if (GlobalConfig.PaginaDoTablet == null)
                GlobalConfig.PaginaDoTablet = new Dictionary<int, HtmlDoTablet>();

            if (generatedUnit.ListUnits != null && generatedUnit.ListUnits.Count > 0)
            {
                Queue<Thread> threadBuffer = new Queue<Thread>();

                var Pool = GlobalConfig.PoolSemaphore;

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

                GlobalConfig.PoolSemaphore.WaitOne();
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
                    GlobalConfig.PaginaDoTablet[id].StackTrace = ex.Message + " -> " + ex.StackTrace;
                }
            }
            finally
            {
                GlobalConfig.PoolSemaphore.Release();
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

        private void SaveFile(int id, string html, int Shift_Id)
        {
            //html = System.Text.RegularExpressions.Regex.Replace(html, @"[ ]{2,}", String.Empty);
            var fileName = $"HTMLTabletUnidade{id}_Shift{Shift_Id}.txt";
            if (Shift_Id == 0)
                fileName = $"HTMLTabletUnidade{id}.txt";
            var path = Path.Combine(@AppDomain.CurrentDomain.BaseDirectory, "appParametrization", fileName);
            Directory.CreateDirectory(path.Substring(0, path.LastIndexOf("\\")));
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.Write(html);
            }
        }

        private string GetFile(int id, int Shift_Id)
        {
            //Código para Ler o arquivo
            string file;
            var fileName = $"HTMLTabletUnidade{id}_Shift{Shift_Id}.txt";
            if (Shift_Id == 0)
                fileName = $"HTMLTabletUnidade{id}.txt";
            var path = Path.Combine(@AppDomain.CurrentDomain.BaseDirectory, "appParametrization", fileName);
            Directory.CreateDirectory(path.Substring(0, path.LastIndexOf("\\")));
            try
            {
                file = File.ReadAllText(@path);
            }
            catch (Exception ex)
            {
                file = null;
            }
            return file;
        }

        #endregion

        public void Dispose()
        {
            db.Dispose();
        }

        public Dictionary<string, string> GetImages()
        {
            var image = new Dictionary<string, string>();

            image.Add("logo", DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.systemLogo);

            return image;
        }

    }
}
