using Dominio;
using DTO.Interfaces.Services;
using DTO;
using DTO.DTO;
using DTO.DTO.Params;
using SgqService.Handlres;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Http;

namespace SgqService.Controllers.Api.Params
{
    [HandleApi()]
    [RoutePrefix("api/ParamsApi")]
    public class ParamsApiController : ApiController
    {

        #region Constructor

        private IParamsDomain _paramdDomain;
        private IBaseDomain<ParLevel1, ParLevel1DTO> _baseParLevel1;
        private IBaseDomain<ParLevel2, ParLevel2DTO> _baseParLevel2;
        private IBaseDomain<ParLevel3, ParLevel3DTO> _baseParLevel3;
        private IBaseDomain<ParHeaderField, ParHeaderFieldDTO> _baseParHeaderField;
        private IBaseDomain<ParMultipleValues, ParMultipleValuesDTO> _baseParMultipleValues;

        public ParamsApiController(IParamsDomain paramdDomain
            , IBaseDomain<ParLevel2, ParLevel2DTO> baseParLevel2
            , IBaseDomain<ParLevel3, ParLevel3DTO> baseParLevel3
            , IBaseDomain<ParLevel1, ParLevel1DTO> baseParLevel1
            , IBaseDomain<ParHeaderField, ParHeaderFieldDTO> baseParHeaderField
            , IBaseDomain<ParMultipleValues, ParMultipleValuesDTO> baseParMultipleValues)
        {
            _baseParLevel1 = baseParLevel1;
            _baseParLevel2 = baseParLevel2;
            _baseParLevel3 = baseParLevel3;
            _paramdDomain = paramdDomain;
            _baseParHeaderField = baseParHeaderField;
            _baseParMultipleValues = baseParMultipleValues;
        }

        #endregion

        [HttpGet]
        [Route("GetResource/{language}")]
        public IEnumerable<DictionaryEntry> GetResource(string language)
        {
            if (language.Equals("pt-br") || (language.Equals("default") && GlobalConfig.LanguageBrasil)) //se portugues
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");
            }
            else if (language.Equals("en-us") || (language.Equals("default") && GlobalConfig.LanguageEUA))//inglês
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("");
            }

            System.Resources.ResourceManager resourceManager = Resources.Resource.ResourceManager;

            return resourceManager.GetResourceSet(
                Thread.CurrentThread.CurrentUICulture, true, false).Cast<DictionaryEntry>();
        }
    }
}
