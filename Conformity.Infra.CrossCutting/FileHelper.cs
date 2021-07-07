using Conformity.Infra.CrossCutting.Storage;
using System;
using System.IO;
using System.Net;

namespace Conformity.Infra.CrossCutting
{
    public class FileHelper
    {
        public static byte[] DownloadPhoto(string fileAddress
            , string usuarioServidor, string senhaServidor, out Exception exception)
        {
            exception = null;
            FileStream stream;
            byte[] bytes = new byte[0];

            try
            {

                if (string.IsNullOrEmpty(usuarioServidor))
                {
                    stream = File.OpenRead(fileAddress);

                    if (stream != null)
                    {
                        bytes = new byte[stream.Length];
                        stream.Read(bytes, 0, bytes.Length);
                    }
                }
                else
                {
                    NetworkCredential credential = new NetworkCredential(usuarioServidor, senhaServidor);
                    using (new NetworkConnection(Path.GetDirectoryName(fileAddress), credential))
                    {

                        stream = File.OpenRead(fileAddress);

                        if (stream != null)
                        {
                            bytes = new byte[stream.Length];
                            stream.Read(bytes, 0, bytes.Length);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                return null;
            }

            return bytes;
        }

        public static bool SavePhoto(string base64Arquivo, string fileAddress, string fileName
            , string usuarioServidor, string senhaServidor, string urlServidor, out Exception exception)
        {
            exception = null;

            try
            {
                byte[] byteArquivo = Convert.FromBase64String(base64Arquivo);
                if (string.IsNullOrEmpty(usuarioServidor))
                {
                    File.WriteAllBytes(Path.Combine(fileAddress, fileName), byteArquivo);
                }
                else
                {
                    NetworkCredential credential = new NetworkCredential(usuarioServidor, senhaServidor);
                    using (new NetworkConnection(urlServidor, credential))
                    {
                        File.WriteAllBytes(Path.Combine(fileAddress, fileName), byteArquivo);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                exception = ex;
                return false;
            }
        }

        public static void DeleteFile(string file
            , string usuarioServidor, string senhaServidor)
        {

            if (string.IsNullOrEmpty(usuarioServidor))
            {
                if (File.Exists(file))
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }
            }
            else
            {
                NetworkCredential credential = new NetworkCredential(usuarioServidor, senhaServidor);
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

        public static bool FileWasSaved(string file
            , string usuarioServidor, string senhaServidor, string urlServidor)
        {
            if (string.IsNullOrEmpty(usuarioServidor))
            {
                return File.Exists(file);
            }
            else
            {
                NetworkCredential credential = new NetworkCredential(usuarioServidor, senhaServidor);
                using (new NetworkConnection(urlServidor, credential))
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
}
