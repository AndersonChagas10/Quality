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
        public GenericReturn<ColetaDTO> Salvar([FromBody] ColetaViewModel data)
        {
            return _coletaApp.Salvar(data);
        }

        [Route("api/Result/SalvarLista")]
        public GenericReturn<ColetaDTO> SalvarLista([FromBody] ColetaViewModel data)
        {
            return _coletaApp.SalvarLista(data.listaResultado);
        } 

        #endregion

    }
}
