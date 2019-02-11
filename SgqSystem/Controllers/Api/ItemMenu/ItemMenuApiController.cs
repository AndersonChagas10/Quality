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

            var itensDeMenu = db.ItemMenu/*.Where(r => r.IsActive == true)*/.OrderBy(r => r.Name).ToList();

            return itensDeMenu;
        }

        [HttpPost]
        [Route("UpdateItensMenu")]
        public bool UpdateItensMenu(ListaItemMenu ItensMenu)
        {
            //Setar todos ItemMenu_Id para null antes de salvar

            using (var db = new SgqDbDevEntities())
            {
                var listaItemMenu = db.ItemMenu.ToList();

                foreach (var itemMenu in listaItemMenu)
                {
                    var itemMenuPresenteNaEstrutura = ItensMenu.ItensMenu.FirstOrDefault(x => x.Id == itemMenu.Id);
                    if (itemMenuPresenteNaEstrutura != null) //0 = Id_Root
                    {
                        itemMenu.ItemMenu_Id = itemMenuPresenteNaEstrutura.ItemMenu_Id;
                    }
                    else
                    {
                        itemMenu.ItemMenu_Id = null;
                    }
                    itemMenu.AlterDate = DateTime.Now;
                    db.Entry(itemMenu).Property(x => x.ItemMenu_Id).IsModified = true;
                    db.Entry(itemMenu).Property(x => x.AlterDate).IsModified = true;
                }

                db.SaveChanges();
            }
            return true;
        }

        public class ListaItemMenu
        {
            public List<ItemMenu> ItensMenu { get; set; }
        }
    }
}
