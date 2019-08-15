using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dominio;
using SgqSystem.Handlres;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using DTO;
using ADOFactory;
using System.Threading;
using System.Globalization;
using DTO.Helpers;
using SGQDBContext;
using SgqSystem.Services;
using SgqSystem.Helpers;
using System.Net.Mail;
using System.Text;
using SgqSystem.Controllers.Api.App;
using System.Collections;
using System.Web.Http.Controllers;
using System.Data;
using ServiceModel;

namespace SgqSystem.Controllers.Api
{
    [HandleApi()]
    [RoutePrefix("api/SyncServiceApi")]
    public class SyncServiceApiController : BaseApiController
    {
        public SgqServiceBusiness.Api.SyncServiceApiController business;

        string conexao;
        string conexaoSGQ_GlobalADO;

        protected SqlConnection db;
        protected SqlConnection SGQ_GlobalADO;

        Dominio.SgqDbDevEntities dbEf;

        public SyncServiceApiController()
        {
            conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            if (GlobalConfig.Brasil)
            {
                conexaoSGQ_GlobalADO = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            }

            business = new SgqServiceBusiness.Api.SyncServiceApiController(conexao, conexaoSGQ_GlobalADO);

            db = new SqlConnection(conexao);
            SGQ_GlobalADO = new SqlConnection(conexaoSGQ_GlobalADO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                SGQ_GlobalADO.Dispose();
                db.Dispose();
            }

            base.Dispose(disposing);
        }


        #region Json
        /// <summary>
        /// Método Para Inserir Resultado da Coleta
        /// </summary>
        /// <param name="ObjResultJSon">Objeto para salvar no banco de dados</param>
        /// <param name="deviceId">Uuid do dispositivo no caso do APP</param> //para Web vem nulo
        /// <param name="deviceMac">Mac Adress</param> //O App não manda
        /// <returns></returns>
        /// O resultado da coleta é salvo por Level02
        /// Processo no tablet
        /// 1. Usuário Clica no botão Sync
        /// 2. O Tablet processa o Sync através do método sendResults() quando o usuário Clinca no botão Sync.
        /// 3. O metodo sendResults() processa os resultados não sincronizados e envia pacote de 500 resultados (Pode ser configurado) para gravar na tabla CollectionJson.
        /// 4. O método executa o insert
        /// 5. Após o Insert retorna mensagem para APP.
        /// 6. O processo continua 1 a 5 se repete até finalizar os resultados.
        [HttpPost]
        [Route("InsertJson")]
        public string InsertJson([FromBody] InsertJsonClass insertJsonClass)
        {
            if (string.IsNullOrEmpty(insertJsonClass.ObjResultJSon))
                return null;

            VerifyIfIsAuthorized();
            return business.InsertJson(insertJsonClass);
        }

        private T[] RemoveFrom<T>(T[] source, int index)
        {
            T[] dest = new T[source.Length - 1];
            if (index > 0)
                Array.Copy(source, 0, dest, 0, index);

            if (index < source.Length - 1)
                Array.Copy(source, index + 1, dest, index, source.Length - index - 1);

            return dest;
        }

        /// <summary>
        /// Método que grava o Json nas tabelas de resultados
        /// </summary>
        /// <param name="device">Id do dispositivo</param>
        /// <returns></returns>
        /// Para chamar uma consolidação geral digite [web]
        [HttpPost]
        [Route("ProcessJson")]
        public string ProcessJson(string device, int id, bool filho)
        {
            VerifyIfIsAuthorized();
            return business.ProcessJson(device, id, filho);
        }

        /// <summary>
        /// Metodo que para chamar o recebimento de dados
        /// </summary>
        /// <param name="unidadeId"></param>
        /// <returns></returns>
        /// PORQUE QUE ESSA PORRA DESTA DATA É MESDIAANO?????????????????? (Comentário Gabriel)

        [HttpPost]
        [Route("reciveData")]
        public string reciveData(string unidadeId, string data)
        {
            VerifyIfIsAuthorized();
            return business.reciveData(unidadeId, data);
        }

        [HttpPost]
        [Route("reciveDataByLevel1")]
        public string reciveDataByLevel1(string ParCompany_Id, string data, string ParLevel1_Id)
        {
            VerifyIfIsAuthorized();
            return business.reciveDataByLevel1(ParCompany_Id, data, ParLevel1_Id);
        }
        #endregion

        #region App
        [HttpPost]
        [Route("getAPP")]
        public string getAPP(/*string version*/)
        {
            return getAPP2("");
        }

        [HttpPost]
        [Route("getAPP2")]
        public string getAPP2(string version)
        {
            return business.getAPP2(version);
        }

        [HttpPost]
        [Route("getAPPLevels")]
        public string getAPPLevels(int UserSgq_Id, int ParCompany_Id, DateTime Date, int Shift_Id)
        {
            return business.getAPPLevels(UserSgq_Id, ParCompany_Id, Date, Shift_Id);
        }

        [HttpPost]
        [Route("getAPPLevelsVolume")]
        public string getAPPLevelsVolume([FromBody] GetAPPLevelsVolumeClass getAPPLevelsVolumeClass)
        {
            VerifyIfIsAuthorized();
            return business.getAPPLevelsVolume(getAPPLevelsVolumeClass);
        }

        #endregion

        #region Users

        [HttpPost]
        [Route("getCompanyUsers")]
        public string getCompanyUsers(string ParCompany_Id)
        {
            VerifyIfIsAuthorized();
            return business.getCompanyUsers(ParCompany_Id);
        }

        [HttpPost]
        [Route("UserSGQById")]
        public string UserSGQById(int Id)
        {
            return business.UserSGQById(Id);
        }

        #endregion

        [HttpPost]
        [Route("insertDeviation")]
        public string insertDeviation([FromBody] InsertDeviationClass insertDeviationClass)
        {
            VerifyIfIsAuthorized();
            return business.insertDeviation(insertDeviationClass);
        }

        [HttpPost]
        [Route("sendEmailAlerta")]
        public string sendEmailAlerta()
        {
            VerifyIfIsAuthorized();
            return business.sendEmailAlerta();
        }

        [HttpPost]
        [Route("sendEmail")]
        public string sendEmail(string email, string subject, string body, string email_CopiaOculta = null)
        {
            return business.sendEmail(email, subject, body, email_CopiaOculta);
        }

        [HttpPost]
        [Route("UserCompanyUpdate")]
        public string UserCompanyUpdate(string UserSgq_Id, int ParCompany_Id)
        {
            VerifyIfIsAuthorized();
            return business.UserCompanyUpdate(UserSgq_Id, ParCompany_Id);
        }

        [HttpPost]
        [Route("InsertCorrectiveAction")]
        public string InsertCorrectiveAction([FromBody] InsertCorrectiveActionClass insertCorrectiveActionClass)
        {
            VerifyIfIsAuthorized();
            return business.InsertCorrectiveAction(insertCorrectiveActionClass);
        }

        [HttpPost]
        [Route("getPhaseLevel2")]
        public string getPhaseLevel2(int ParCompany_Id, string date)
        {
            VerifyIfIsAuthorized();
            return business.getPhaseLevel2(ParCompany_Id, date);
        }

        [HttpPost]
        [Route("getResultEvaluationDefects")]
        public string getResultEvaluationDefects(int parCompany_Id, string date, int parLevel1_Id)
        {
            VerifyIfIsAuthorized();
            return business.getResultEvaluationDefects(parCompany_Id, date, parLevel1_Id);
        }

        [HttpPost]
        [Route("getCollectionLevel2Keys")]
        public string getCollectionLevel2Keys([FromBody] GetCollectionLevel2KeysClass getCollectionLevel2KeysClass)
        {
            VerifyIfIsAuthorized();
            return business.getCollectionLevel2Keys(getCollectionLevel2KeysClass);
        }

        [HttpPost]
        [Route("GetLastSampleByCollectionLevel2")]
        public int GetLastSampleByCollectionLevel2(GetLastSampleByCollectionLevel2Class getLastSampleByCollectionLevel2Class)
        {
            VerifyIfIsAuthorized();
            return business.GetLastSampleByCollectionLevel2(getLastSampleByCollectionLevel2Class);
        }
    }
}