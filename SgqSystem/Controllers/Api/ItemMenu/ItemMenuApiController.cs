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
        public bool UpdateItensMenu(MeuRabo ItensMenu)
        {

            //Setar todos ItemMenu_Id para null antes de salvar

            using (var db = new SgqDbDevEntities())
            {
                var sql = $@"UPDATE ItemMenu set ItemMenu_Id = null";

                db.Database.ExecuteSqlCommand(sql);
                db.SaveChanges();
            }

            foreach (var itemMenu in ItensMenu.ItensMenu)
            {
                if (itemMenu.Id > 0) //0 = Id_Root
                {
                    using (var db = new SgqDbDevEntities())
                    {
                        try
                        {
                            itemMenu.AlterDate = DateTime.Now;
                            itemMenu.Name = "Gambs"; //Gambzinha para não dizer que o "Name" é obrigatório, porém não será alterado
                            db.ItemMenu.Attach(itemMenu);
                            db.Entry(itemMenu).Property(x => x.ItemMenu_Id).IsModified = true;
                            db.Entry(itemMenu).Property(x => x.AlterDate).IsModified = true;
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }

                    }
                }
            }

            return true;
        }

        public class MeuRabo
        {
            public List<ItemMenu> ItensMenu { get; set; }
        }
    }
}
