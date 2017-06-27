using ADOFactory;
using DTO.DTO;
using DTO.Helpers;
using Newtonsoft.Json.Linq;
using PlanoAcaoCore;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace PlanoDeAcaoMVC.Controllers.Api
{
    [RoutePrefix("api/Pa_User")]
    public class ApiPa_UserController : BaseApiController
    {

        [HttpPost]
        [Route("CheckPass")]
        public JObject CheckPass(JObject form)
        {
            dynamic user = form;
            string pass = user.pass;
            string name = user.name;
            using (var db = new Factory(Conn.dataSource2, Conn.catalog2, Conn.pass2, Conn.user2))
            {
                var userSgq = db.QueryNinjaADO("SELECT * FROM USerSgq where Name = '" + name + "' AND Password = '" + Guard.EncryptStringAES(pass) + "'").FirstOrDefault();
                if (userSgq != null)
                {
                    user.response = "Senha validada com sucesso.";
                    user.isInvalid = false;
                    return user;
                }
                else
                {
                    user.response = "Usuário ou senhas inválidos.";
                    user.isInvalid = true;
                    return user;
                }
            }
        }

        //[HandleApi()]
        [HttpPost]
        [Route("GetUserCookie")]
        public HttpResponseMessage GetUserCookie([FromBody]UserDTO userDto)
        {
            var resp = new HttpResponseMessage();
            var cookie = CreateCookieFromUserDTO(userDto);
            resp.Headers.AddCookies(new CookieHeaderValue[] { cookie });
            return resp;
        }

      

    }
}
