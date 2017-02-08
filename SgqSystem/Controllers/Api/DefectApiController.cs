using AutoMapper;
using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO;
using SgqSystem.Handlres;
using SgqSystem.ViewModels;
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
        public void MergeDefect(List<DefectDTO> listDefectDTO)
        {
           _defectDomain.MergeDefect(listDefectDTO);
        }

        [Route("GetDefects")]
        [HttpGet]
        public List<DefectDTO> GetDefects(int parCompany_Id)
        {
            return _defectDomain.GetDefects(parCompany_Id);
        }
        
        #endregion
    }
}
