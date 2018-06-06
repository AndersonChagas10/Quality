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
        public List<ItemMenu> GetListItemMenu()
        {

            var db = new SgqDbDevEntities();

            var itensDeMenu = db.ItemMenu.Where(r => r.IsActive == true).OrderBy(r => r.Name).ToList();

            return itensDeMenu;
        }

        [HttpPost]
        [Route("UpdateItensMenu")]
        public bool UpdateItensMenu([FromBody] List<ItemMenu> ItensMenu)
        {

            //Setar todos ItemMenu_Id para null antes de salvar

            foreach (var itemMenu in ItensMenu)
            {
                if (itemMenu.Id > 0) //0 = Id_Root
                {
                    using (var db = new SgqDbDevEntities())
                    {
                        db.ItemMenu.Attach(itemMenu);
                        db.Entry(itemMenu).Property(x => x.ItemMenu_Id).IsModified = true;
                        db.SaveChanges();
                    }
                }
            }

            return true;
        }
    }
}
