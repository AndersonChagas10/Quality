using AutoMapper;
using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO;
using SgqSystem.Handlres;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{
    [HandleApi()]
    [RoutePrefix("api/UserSgq")]
    public class UserSgqApiController : ApiController
    {
        private IBaseDomain<UserSgq, UserSgqDTO> _baseDomainUserSgq;

        public UserSgqApiController(
            IBaseDomain<UserSgq, UserSgqDTO> baseDomainUserSgq)
        {
            _baseDomainUserSgq = baseDomainUserSgq;
        }

        [Route("Get")]
        [HttpGet]
        public UserSgq Get(int Id)
        {
            UserSgq userSgqDto = Mapper.Map<UserSgq>(_baseDomainUserSgq.GetById(Id));
            return userSgqDto;
        }
    }
}