using Application.Interface;
using AutoMapper;
using Dominio.Entities;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ControleColetaController : ApiController
    {

        private readonly IAppServiceBase<ResultOld> _serviceAppBase;

        public ControleColetaController(IAppServiceBase<ResultOld> serviceAppBase)
        {
            _serviceAppBase = serviceAppBase;
        }

        [HttpPost]
        [Route("api/ControleColeta/GetParametrosResult")]
        public GenericReturnViewModel<List<ResultOldViewModel>> GetParametrosResult([FromBody]string date)
        {
            try
            {
                //var date = "2016/07/01";
                var dt = DateTime.ParseExact(date, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                var response = _serviceAppBase.GetAll().Where(r=> r.AddDate >= dt && r.AddDate <= dt.AddDays(1)).ToList();
                var responseViewModel = Mapper.Map<List<ResultOld>, List<ResultOldViewModel>>(response);
                return new GenericReturnViewModel<List<ResultOldViewModel>>() { Retorno = responseViewModel };
            }
            catch (Exception e)
            {
                return new GenericReturnViewModel<List<ResultOldViewModel>>(e, "Cannot get results paramiters");
            }
        }

    }
}
