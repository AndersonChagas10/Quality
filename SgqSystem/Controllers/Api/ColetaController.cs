using Application.Interface;
using AutoMapper;
using DTO.DTO;
using DTO.Helpers;
using SgqSystem.ViewModels;
using System.Web.Http;

namespace SgqSystem.Api.Controllers
{
    public class ColetaController : ApiController
    {

        #region Construtor e atributos

        private readonly IColetaApp _coletaApp;

        public ColetaController(IColetaApp coletaApp)
        {
            _coletaApp = coletaApp;
        } 

        #endregion

        #region Salvar

        [Route("api/Result/Salvar")]
        public GenericReturnViewModel<ColetaViewModel> Salvar([FromBody] ColetaViewModel data)
        {
            var queryResult = _coletaApp.Salvar(data);
            return Mapper.Map<GenericReturn<ColetaDTO>, GenericReturnViewModel<ColetaViewModel>>(queryResult);
        }

        [Route("api/Result/SalvarLista")]
        public GenericReturnViewModel<ColetaViewModel> SalvarLista([FromBody] ColetaViewModel data)
        {
            var queryResult = _coletaApp.SalvarLista(data.listaResultado);
            return Mapper.Map<GenericReturn<ColetaDTO>, GenericReturnViewModel<ColetaViewModel>>(queryResult);
        } 

        #endregion

    }
}
