﻿using Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace SgqSystem.Controllers.Api
{
    //[RoutePrefix("api/File")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class FileController : Controller
    {
        private string ServerVirtualPath { get { return Server.MapPath("~/Arquivos"); } }
        //[Route("GetFiles")]
        public ActionResult GetFiles()
        {
            string[] dirs = Directory.GetFiles(ServerVirtualPath, "*.*");
            var retorno = new files() { filesInDir = new List<string>() };
            foreach (var i in dirs)
                retorno.filesInDir.Add(i);
            return Json(retorno);
        }

        //[Route("Get")]
        public System.Web.Mvc.FileResult Download(string fileName)
        {
            var path = Path.Combine(ServerVirtualPath, fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpPost]
        [HandleController()]
        public string Upload(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                // extract only the filename
                var fileName = Path.GetFileName(file.FileName);
                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(ServerVirtualPath, fileName);
                file.SaveAs(path);
                return "File inserted: " + path;
            }
            return "The file is empity";
        }

    }

    public class files
    {
        public List<string> filesInDir { get; set; }
    }
}
