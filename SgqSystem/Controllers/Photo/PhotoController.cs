using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers.Photo
{
    public class PhotoController : Controller
    {

        public PhotoController()
        {

        }

        public void Get(int id)
        {
            using (var db = new SgqDbDevEntities())
            {
                var photo = db.Result_Level3_Photos
                    .Where(x => x.ID == id)
                    .FirstOrDefault();

                System.Net.WebClient webClient = new System.Net.WebClient();
                string url = photo.Photo;
                byte[] bytes = webClient.DownloadData(url);
                //string fileName = (url.Split('/')[url.Split('/').Length - 1]).Split('.')[0];
                Response.ContentType = "image/png";
                Response.AppendHeader("Content-Disposition", $"attachment; filename={photo.Result_Level3_Id}-{photo.ID}.png");
                Response.BinaryWrite(bytes);
                Response.End();
            }
        }

        // GET: Photo
        public ActionResult Index(int? id)
        {
            using (var db = new SgqDbDevEntities())
            {
                var photo = db.Result_Level3_Photos.OrderByDescending(r => r.ID).FirstOrDefault();
                if (id == null)
                    return View(photo);

                photo = db.Result_Level3_Photos.FirstOrDefault(r => r.ID == id);
                if (photo == null)
                {
                    return HttpNotFound();
                }
                return View(photo);
            }
        }

    }
}