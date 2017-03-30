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
        private IBaseDomain<RoleJBS, RoleJBSDTO> _baseDomainRoleJBS;

        public RoleApiController(
            IBaseDomain<ScreenComponent, ScreenComponentDTO> baseDomainScreenComponent,
            IBaseDomain<RoleSGQ, RoleSGQDTO> baseDomainRoleSGQ, IBaseDomain<RoleJBS, RoleJBSDTO> baseDomainRoleJBS)
        {
            _baseDomainScreenComponent = baseDomainScreenComponent;
            _baseDomainRoleSGQ = baseDomainRoleSGQ;
            _baseDomainRoleJBS = baseDomainRoleJBS;
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

        [Route("GetRoleJBS")]
        [HttpGet]
        public RoleJBSDTO GetRoleJBS(int Id)
        {
            return _baseDomainRoleJBS.GetById(Id);
        }

        [Route("SaveRoleJBS")]
        [HttpPost]
        public RoleJBSDTO SaveRoleJBS(RoleJBSDTO roleJbsDto)
        {
            return _baseDomainRoleJBS.AddOrUpdate(roleJbsDto);
        }
    }
}