using AutoMapper;
using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO;
using SgqSystem.Handlres;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{
    [HandleApi()]
    [RoutePrefix("api/Defect")]
    public class DefectApiController : ApiController
    {

        #region Construtor para injeção de dependencia

        private IDefectDomain _defectDomain;
        
        public DefectApiController(IDefectDomain defectDomain)
        {
            _defectDomain = defectDomain;
        }

        #endregion

        #region Metodos disponíveis na API

        [Route("MergeDefect")]
        [HttpPost]
        public List<Defect> MergeDefect(List<DefectDTO> listDefectDTO)
        {
           return _defectDomain.MergeDefect(listDefectDTO);
        }
        
        #endregion
    }
}
