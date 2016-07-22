using Application.Interface;
using AutoMapper;
using Dominio;
using DTO.DTO;
using DTO.Helpers;
using SgqSystem.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BetaApiController : ApiController
    {

        private readonly IBetaAppService _betaAppService;
        private readonly IAppServiceBase<Coleta> _AppServiceBaseCoelta;
        private readonly IAppServiceBase<Level1> _AppServiceBaseLevel1;
        private readonly IAppServiceBase<Level2> _AppServiceBaseLevel2;
        private readonly IAppServiceBase<Level3> _AppServiceBaseLevel3;
        private readonly IAppServiceBase<UserSgq> _AppServiceBaseUserSgq;

        public BetaApiController(IBetaAppService betaAppService,
            IAppServiceBase<Coleta> AppServiceBaseCoelta,
            IAppServiceBase<Level1> AppServiceBaseLevel1,
            IAppServiceBase<Level2> AppServiceBaseLevel2,
            IAppServiceBase<Level3> AppServiceBaseLevel3,
            IAppServiceBase<UserSgq> AppServiceBaseUserSgq)
        {
            _betaAppService = betaAppService;
            _AppServiceBaseCoelta = AppServiceBaseCoelta;
            _AppServiceBaseLevel1 = AppServiceBaseLevel1;
            _AppServiceBaseLevel2 = AppServiceBaseLevel2;
            _AppServiceBaseLevel3 = AppServiceBaseLevel3;
            _AppServiceBaseUserSgq = AppServiceBaseUserSgq;
        }

        #region Coleta de Dados SP1 BEta

        [Route("api/Result/Salvar")]
        public GenericReturnViewModel<ColetaViewModel> Post([FromBody] ColetaViewModel data)
        {
            var queryResult = _betaAppService.Salvar(data);
            return Mapper.Map<GenericReturn<ColetaDTO>, GenericReturnViewModel<ColetaViewModel>>(queryResult);
        }

        [Route("api/Result/SalvarLista")]
        public GenericReturnViewModel<ColetaViewModel> SalvarLista([FromBody] ColetaViewModel data)
        {
            var queryResult = _betaAppService.SalvarLista(data.listaResultado);
            return Mapper.Map<GenericReturn<ColetaDTO>, GenericReturnViewModel<ColetaViewModel>>(queryResult);
        }

        #endregion

        #region Retorno de Dados

        [Route("api/RelatorioBetaApi/GetNcPorIndicador")]
        public GenericReturnViewModel<List<ColetaViewModel>> GetNcPorIndicador(int idIndicador, string dateInit, string dateEnd)
        {
            var queryResult = _betaAppService.GetNcPorIndicador(idIndicador, dateInit, dateEnd);
            return Mapper.Map<GenericReturn<List<ColetaDTO>>, GenericReturnViewModel<List<ColetaViewModel>>>(queryResult);
        }

        [Route("api/RelatorioBetaApi/GetNcPorLevel2")]
        public GenericReturnViewModel<List<ColetaViewModel>> GetNcPorLevel2(int idIndicador, string dateInit, string dateEnd)
        {
            var queryResult = _betaAppService.GetNcPorLevel2(idIndicador, dateInit, dateEnd);
            return Mapper.Map<GenericReturn<List<ColetaDTO>>, GenericReturnViewModel<List<ColetaViewModel>>>(queryResult);
        }

        [Route("api/RelatorioBetaApi/GetNcPorLevel2Jelsafa")]
        public GenericReturnViewModel<List<ColetaViewModel>> GetNcPorLevel2Jelsafa(int idIndicador, string dateInit, string dateEnd)
        {
            var queryResult = _betaAppService.GetNcPorLevel2Jelsafa(idIndicador, dateInit, dateEnd);
            return Mapper.Map<GenericReturn<List<ColetaDTO>>, GenericReturnViewModel<List<ColetaViewModel>>>(queryResult);
        }

        [Route("api/RelatorioBetaApi/GetNcPorLevel3")]
        public GenericReturnViewModel<List<ColetaViewModel>> GetNcPorLevel3(int indicadorId, int idLevel2, string dateInit, string dateEnd)
        {
            var queryResult = _betaAppService.GetNcPorLevel3(indicadorId, idLevel2, dateInit, dateEnd);
            return Mapper.Map<GenericReturn<List<ColetaDTO>>, GenericReturnViewModel<List<ColetaViewModel>>>(queryResult);
        }

        #endregion

        #region Retorno de dados, Tabelas Level1, 2 e 3

        //[Route("api/RelatorioBetaApi/GetNcPorIndicador")]
        //public GenericReturnViewModel<List<ColetaViewModel>> GetDadosLevel1(int idIndicador, string dateInit, string dateEnd)
        //{
        //    var queryResult = _betaAppService.GetDadosLevel1();
        //}


        //[Route("api/RelatorioBetaApi/GetDadosLevel2")]
        //public GenericReturnViewModel<List<ColetaViewModel>> GetDadosLevel2(int idIndicador, string dateInit, string dateEnd)
        //{
        //    var queryResult = _betaAppService.GetDadosLevel2();
        //}


        //[Route("api/RelatorioBetaApi/GetDadosLevel3")]
        //public GenericReturnViewModel<List<ColetaViewModel>> GetDadosLevel3(int idIndicador, string dateInit, string dateEnd)
        //{
        //    var queryResult = _betaAppService.GetDadosLevel3();
        //}

        #endregion

        #region Recebe Dados para Syncronizar o banco de dados

        //public GenericReturnViewModel<List<ColetaViewModel>> GetDadosLevel1(int idIndicador, string dateInit, string dateEnd)
        //{
        //    var queryResult = _betaAppService.GetDadosLevel1();
        //}

        #endregion

        #region Envia Dados Para Syncronizar a plataforma remota

        public GenericReturnViewModel<SyncViewModel> GetDataToSincyAudit()
        {
            var objReturn = new GenericReturnViewModel<SyncViewModel>(GetSync);
            return objReturn;
        }

        public SyncViewModel GetSync()
        {
            var resultColeta = Mapper.Map<List<Coleta>, List<ColetaDTO>>(_AppServiceBaseCoelta.GetAll().ToList());
            var resultLevel1 = Mapper.Map<List<Level1>, List<Level1DTO>>(_AppServiceBaseLevel1.GetAll().ToList());
            var resultLevel2 = Mapper.Map<List<Level2>, List<Level2DTO>>(_AppServiceBaseLevel2.GetAll().ToList());
            var resultLevel3 = Mapper.Map<List<Level3>, List<Level3DTO>>(_AppServiceBaseLevel3.GetAll().ToList());
            var resultUserSgq = Mapper.Map<List<UserSgq>, List<UserDTO>>(_AppServiceBaseUserSgq.GetAll().ToList());

            return new SyncViewModel()
            {
                Coleta = resultColeta,
                Level1 = resultLevel1,
                Level2 = resultLevel2,
                Level3 = resultLevel3,
                UserSgq = resultUserSgq
            };
        }
    #endregion
}
}
