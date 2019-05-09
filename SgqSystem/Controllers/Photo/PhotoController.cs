﻿using Dominio;
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

                System.Net.WebClient webClient = new System.Net.WebClient();
                string url = photo.Photo;

                //Verificar se no web.config a credencial do servidor de fotos

                var credentialUserServerPhoto = GetWebConfigSettings("credentialUserServerPhoto");
                var credentialPassServerPhoto = GetWebConfigSettings("credentialPassServerPhoto");
                FileStream file;

                if (credentialUserServerPhoto != null && credentialPassServerPhoto != null)
                {

                    var credential = new NetworkCredential(credentialUserServerPhoto, credentialPassServerPhoto);
                    using (new NetworkConnection(Path.GetDirectoryName(url), credential))
                    {
                        file = System.IO.File.OpenRead(url);
                    }

                }
                else
                {
                    file = System.IO.File.OpenRead(url);
                }

                byte[] bytes = new byte[file.Length];
                file.Read(bytes, 0, bytes.Length);

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