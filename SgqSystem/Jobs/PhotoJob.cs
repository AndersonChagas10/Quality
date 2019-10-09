using DTO;
using Quartz;
using SgqSystem.Controllers.FotoController;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace SgqSystem.Jobs
{
    public class PhotoJob : IJob
    {
        public static bool Executing { get; set; } = false;
        public void Execute(IJobExecutionContext context)
        {
            PhotoJobFunction(null);
        }

        public static void PhotoJobFunction(object stateInfo)
        {
            while (true)
            {
                if (Executing)
                    return;

                Executing = true;
                Execute();
                Executing = false;
            }
        }

        private static void Execute()
        {
            if (ConfigurationManager.AppSettings["PhotoJob"] == "on")
            {
                try
                {
                    var fotoController = new FotoController();

                    var fotos = fotoController.GetFotosByServer();

                    foreach (var foto in fotos)
                    {
                        var arquivoFoto = fotoController.DownloadPhoto(foto.Photo);

                        fotoController.SavePhoto(arquivoFoto, Config.UrlSaveImgProcess, Path.GetFileName(foto.Photo));

                        //verificar se a foto foi salva, se foi altera e deleta
                        if (fotoController.FileWasSaved(Config.UrlSaveImgProcess + "\\" + Path.GetFileName(foto.Photo)))
                        {
                            fotoController.SetPhotoIsProcessed(foto.Id, Path.GetFileName(foto.Photo));

                            if (bool.Parse(Config.DeleteFile))
                            {
                                fotoController.DeleteFile(foto.Photo);
                            }

                            //Console.WriteLine("File: " + Path.GetFileName(foto.Photo) + " was Processed");
                        }
                    }
                }
                catch (Exception ex)
                {
                    new CreateLog(new Exception("Erro no metodo [PhotoJobFunction]", ex));
                }
            }
        }
    }

    public static class Config
    {
        public static string DataSource { get; set; }
        public static string Catalog { get; set; }
        public static string User { get; set; }
        public static string Password { get; set; }
        public static string UrlCompressao { get; set; }
        public static string UrlSaveImgProcess { get; set; }
        public static string ScriptSelect { get; set; }
        public static string ScriptUpdate { get; set; }
        public static int QtdeFotosPorVez { get; set; }
        public static string UserServerGetPhoto { get; set; }
        public static string PassServerGetPhoto { get; set; }
        public static string UserServerSetPhoto { get; set; }
        public static string PassServerSetPhoto { get; set; }
        public static string DeleteFile { get; set; }

        public static void Initialize()
        {
            //string text;

            ////pegar as configurações no banco de dados
            //var fileStream = new FileStream($@"{@AppDomain.CurrentDomain.BaseDirectory}\config.txt", FileMode.Open, FileAccess.Read);

            //using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            //{
            //    text = streamReader.ReadToEnd();
            //}

            //var config = text.Split('|');

            //foreach (var item in config)
            //{
            //    var leo = new string[] { "::" };
            //    var keyValue = item.Split(leo, StringSplitOptions.None);
            //    typeof(Config).GetProperty(keyValue[0]).SetValue(null, keyValue[1]);
            //}

            DataSource = "192.168.25.200";
            Catalog = "dbGQualidade";
            User = "sa";
            Password = "1qazmko0";
            UrlSaveImgProcess = "\\Servergrt\\C\\uploadFiles\\photos_comprimidas";
            ScriptSelect = "select top 100 Id, Photo from Result_Level3_Photos order by id desc";
            ScriptUpdate = "update Result_Level3_Photos set photo = '{0}' where id = {1}";
            QtdeFotosPorVez = 1;
            UserServerGetPhoto = "";
            PassServerGetPhoto  = "";
            UserServerSetPhoto  = "GRT1";
            PassServerSetPhoto = "1qazmko0";
            DeleteFile = "false";
        }

    }
}