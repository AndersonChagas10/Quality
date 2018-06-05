using Dominio;
using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{
    [RoutePrefix("api/ItemMenu")]
    public class ItemMenuApiController : BaseApiController
    {

        [HttpPost]
        [Route("GetListItemMenu")]
        public List<ItemMenu> GetListItemMenu(){

            var db = new SgqDbDevEntities();

            var itensDeMenu = db.ItemMenu.Where(r => r.IsActive == true).OrderBy(r => r.Name).ToList();

            return itensDeMenu;
        }

    }
}
