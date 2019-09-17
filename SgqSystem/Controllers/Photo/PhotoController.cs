using Dominio;
using Helper;
using SgqSystem.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers.Photo
{
    public class PhotoController : BaseController
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

                string url = photo.Photo;

                //Verificar se no web.config a credencial do servidor de fotos
                Exception exception = null;

                byte[] bytes = FileHelper.DownloadPhoto(url
                    , DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.credentialUserServerPhoto
                    , DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.credentialPassServerPhoto
                    , out exception);

                if (exception != null)
                    throw new Exception("Error: " + exception.ToClient());

                if (bytes != null && bytes.Length > 0)
                {
                    Response.ContentType = "image/png";
                    Response.AppendHeader("Content-Disposition", $"attachment; filename={photo.Result_Level3_Id}-{photo.ID}.png");
                    Response.BinaryWrite(bytes);
                    Response.End();

                }
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