using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO;
using SgqSystem.Handlres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{
    [HandleApi()]
    [RoutePrefix("api/Role")]
    public class RoleApiController : ApiController
    {
        private IBaseDomain<ScreenComponent, ScreenComponentDTO> _baseDomainScreenComponent;
        private IBaseDomain<RoleSGQ, RoleSGQDTO> _baseDomainRoleSGQ;

        public RoleApiController(
            IBaseDomain<ScreenComponent, ScreenComponentDTO> baseDomainScreenComponent,
            IBaseDomain<RoleSGQ, RoleSGQDTO> baseDomainRoleSGQ)
        {
            _baseDomainScreenComponent = baseDomainScreenComponent;
            _baseDomainRoleSGQ = baseDomainRoleSGQ;
        }
        
        [Route("GetScreenComponent")]
        [HttpGet]
        public ScreenComponentDTO GetScreenComponent(int Id)
        {
            return _baseDomainScreenComponent.GetById(Id);
        }

        [Route("SaveScreenComponent")]
        [HttpPost]
        public ScreenComponentDTO SaveScreenComponent(ScreenComponentDTO screenComponentDto)
        {
            return _baseDomainScreenComponent.AddOrUpdate(screenComponentDto);
        }

        [Route("GetRoleSgq")]
        [HttpGet]
        public RoleSGQDTO GetRoleSgq(int Id)
        {
            return _baseDomainRoleSGQ.GetById(Id);
        }

        [Route("SaveRoleSGQ")]
        [HttpPost]
        public RoleSGQDTO SaveRoleSGQ(RoleSGQDTO roleSgqDto)
        {
            return _baseDomainRoleSGQ.AddOrUpdate(roleSgqDto);
        }
    }
}