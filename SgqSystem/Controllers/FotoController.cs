using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using ADOFactory;
using ImageCompressor;
using SgqSystem.Jobs;

namespace SgqSystem.Controllers.FotoController
{
    public class FotoController
    {

        public FileStream DownloadPhoto(string fileAddress)
        {
            FileStream stream;

            try
            {

                if (string.IsNullOrEmpty(Config.UserServerGetPhoto))
                {
                    stream = File.OpenRead(fileAddress);
                }
                else
                {
                    var credential = new NetworkCredential(Config.UserServerGetPhoto, Config.PassServerGetPhoto);
                    using (new NetworkConnection(Path.GetDirectoryName(fileAddress), credential))
                    {

                        stream = File.OpenRead(fileAddress);
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return stream;
        }

        //public HttpResponseMessage UploadPhoto(string fileAddress, string urlCompressao)
        //{

        //HttpClient client = new HttpClient();
        //MultipartFormDataContent form = new MultipartFormDataContent();
        //HttpContent content = new StringContent("fileToUpload");
        //HttpResponseMessage response = null;
        //var url = new Uri(urlCompressao);
        //FileStream stream;

        //try
        //{
        //    form.Add(content, "fileToUpload");

        //    if (string.IsNullOrEmpty(Config.UserServerGetPhoto))
        //    {
        //        stream = File.OpenRead(fileAddress);
        //    }
        //    else
        //    {
        //        var credential = new NetworkCredential(Config.UserServerGetPhoto, Config.PassServerGetPhoto);
        //        using (new NetworkConnection(Path.GetDirectoryName(fileAddress), credential))
        //        {

        //            stream = File.OpenRead(fileAddress);
        //        }
        //    }
        //}
        //catch (Exception ex)
        //{
        //    return new HttpResponseMessage() { Content = new StringContent(ex.Message), StatusCode = HttpStatusCode.BadRequest };
        //}

        //try
        //{

        //    content = new StreamContent(stream);

        //    var fileName =
        //        content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
        //        {
        //            Name = "file",
        //            FileName = Path.GetFileName(fileAddress),
        //        };

        //    form.Add(content);

        //    response = (client.PostAsync(url, form)).Result;

        //    response.EnsureSuccessStatusCode();

        //    if (response != null && response.StatusCode == HttpStatusCode.OK)
        //    {
        //        return response;

        //    }

        //    return new HttpResponseMessage() { StatusCode = HttpStatusCode.InternalServerError };

        //}
        //catch (Exception ex)
        //{
        //    return new HttpResponseMessage() { Content = new StringContent(ex.InnerException.StackTrace), StatusCode = HttpStatusCode.InternalServerError };
        //}

        //}

        public bool SavePhoto(FileStream arquivoFoto, string fileAddress, string fileName)
        {
            try
            {
                byte[] buff = ConverteStreamToByteArray(arquivoFoto);

                if (string.IsNullOrEmpty(Config.UserServerSetPhoto))
                {
                    File.WriteAllBytes(Path.Combine(fileAddress, fileName), buff);
                }
                else
                {
                    var credential = new NetworkCredential(Config.UserServerSetPhoto, Config.PassServerSetPhoto);
                    using (new NetworkConnection(Config.UrlSaveImgProcess, credential))
                    {
                        File.WriteAllBytes(Path.Combine(fileAddress, fileName), buff);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<Foto> GetFotosByServer()
        {
            using (Factory factory = new Factory(Config.DataSource, Config.Catalog, Config.Password, Config.User))
            {
                var urlFotos = factory.SearchQuery<Foto>(Config.ScriptSelect).ToList();

                return urlFotos;
            }
        }

        public void SetPhotoIsProcessed(int id, string fileName)
        {

            var newUrl = Path.Combine(Config.UrlSaveImgProcess, fileName);

            using (Factory factory = new Factory(Config.DataSource, Config.Catalog, Config.Password, Config.User))
            {
                factory.ExecuteSql(string.Format(Config.ScriptUpdate, newUrl, id));
            }
        }

        public void DeleteFile(string file)
        {

            if (string.IsNullOrEmpty(Config.UserServerGetPhoto))
            {
                if (File.Exists(file))
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }
            }
            else
            {
                var credential = new NetworkCredential(Config.UserServerGetPhoto, Config.PassServerGetPhoto);
                using (new NetworkConnection(Path.GetDirectoryName(file), credential))
                {

                    try
                    {

                        if (File.Exists(file))
                        {
                            File.SetAttributes(file, FileAttributes.Normal);
                            File.Delete(file);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erro ao excluir arquivo: " + ex.Message);
                    }

                }
            }
        }

        public bool FileWasSaved(string file)
        {
            if (string.IsNullOrEmpty(Config.UserServerSetPhoto))
            {
                return File.Exists(file);
            }
            else
            {
                var credential = new NetworkCredential(Config.UserServerSetPhoto, Config.PassServerSetPhoto);
                using (new NetworkConnection(Config.UrlSaveImgProcess, credential))
                {
                    return File.Exists(file);
                }
            }
        }

        public static byte[] ConverteStreamToByteArray(Stream stream)
        {
            byte[] byteArray = new byte[16 * 1024];
            using (MemoryStream mStream = new MemoryStream())
            {
                int bit;
                while ((bit = stream.Read(byteArray, 0, byteArray.Length)) > 0)
                {
                    mStream.Write(byteArray, 0, bit);
                }
                return mStream.ToArray();
            }
        }
    }

    public class Foto
    {
        public int Id { get; set; }
        public string Photo { get; set; }
    }
}
