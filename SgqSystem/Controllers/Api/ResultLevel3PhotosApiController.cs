using ADOFactory;
using Dominio;
using DTO;
using DTO.DTO;
using SGQDBContext;
using SgqSystem.Handlres;
using SgqSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{

    [HandleApi()]
    [RoutePrefix("api/ResultLevel3PhotosApi")]
    public class ResultLevel3PhotosApiController : BaseApiController
    {

        string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        SgqServiceBusiness.Api.ResultLevel3PhotosApiController business;
        public ResultLevel3PhotosApiController()
        {
            business = new SgqServiceBusiness.Api.ResultLevel3PhotosApiController(conexao);
        }

        [HttpPost]
        public IHttpActionResult Insert([FromBody] List<Result_Level3_PhotosDTO> Fotos)
        {
            if (Fotos == null || Fotos != null && Fotos.Count == 0)
            {
                return BadRequest("Nenhuma foto enviada!");
            }
            VerifyIfIsAuthorized();
            return business.Insert(Fotos);
        }
    }
}
