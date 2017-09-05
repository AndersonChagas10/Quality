using Dominio;
using Dominio.Interfaces.Services;
using DTO;
using DTO.DTO;
using SgqSystem.Handlres;
using System.Linq;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{
    [HandleApi()]
    [RoutePrefix("api/UserSgq")]
    public class UserSgqApiController : ApiController
    {
        private IBaseDomain<UserSgq, UserDTO> _baseDomainUserSgq;
        private IBaseDomain<ParCompanyXUserSgq, ParCompanyXUserSgqDTO> _baseDomainParCompanyXUserSgq;
        private SgqDbDevEntities db;

        public UserSgqApiController(
            IBaseDomain<UserSgq, UserDTO> baseDomainUserSgq,
            IBaseDomain<ParCompanyXUserSgq, ParCompanyXUserSgqDTO> baseDomainParCompanyXUserSgq)
        {
            _baseDomainUserSgq = baseDomainUserSgq;
            _baseDomainParCompanyXUserSgq = baseDomainParCompanyXUserSgq;
            db = new SgqDbDevEntities();
        }

        [Route("Get")]
        [HttpGet]
        public UserDTO Get(int Id)
        {
            UserDTO userSgqDto = _baseDomainUserSgq.GetById(Id);
            userSgqDto.ListParCompany_Id = _baseDomainParCompanyXUserSgq.GetAll().Where(r => r.UserSgq_Id == userSgqDto.Id).Select(p => p.ParCompany_Id);

            if (userSgqDto.Role != null)
                userSgqDto.ListRole = userSgqDto.Role.Split(',').Select(p => p.Trim());

            if (userSgqDto != null)
                if (GlobalConfig.Eua)
                {
                    userSgqDto.IsActive = db.Database.SqlQuery<bool>("SELECT IsActive FROM usersgq WHERE Id = " + userSgqDto.Id).FirstOrDefault();
                }

            return userSgqDto;
        }
    }
}