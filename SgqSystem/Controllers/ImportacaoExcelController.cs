using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    public class ImportacaoExcelController : Controller
    {
        // GET: ImportacaoExcel
        public ActionResult Index()
        {
            return View();
        }
        public class Produto{
            public string Id { get; set; }
            public string Descricao { get; set; }
            public string Preco { get; set; }
            public string NomeArquivo { get; set; }
        }

        public ActionResult Importar(HttpPostedFileBase arquivo)
        {
            List<Produto> Produtos = new List<Produto>();
            DataTable dt = UploadExcelTo(arquivo);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row = dt.Rows[i];
                    // var linha = i + 2;
                    if (row[0] != null && row[0].ToString().Length > 0)
                    {
                        Produto produto = new Produto();
                        produto.Id = row[0].ToString();
                        produto.Descricao = row[1].ToString();
                        produto.Preco = row[2].ToString();
                        produto.NomeArquivo = arquivo.FileName;

                        Produtos.Add(produto);
                    }
                    else
                    {
                        var vazio = 0;
                    }
                }             
                return View("Index",Produtos);
            }
            else
            {
                return View("Index", ModelState);
            }
        }

        protected DataTable UploadExcelTo(HttpPostedFileBase upload)
        {
            if (upload != null && upload.ContentLength > 0)
            {
                // ExcelDataReader works with the binary Excel file, so it needs a FileStream
                // to get started. This is how we avoid dependencies on ACE or Interop:
                Stream stream = upload.InputStream;

                // We return the interface, so that
                IExcelDataReader reader = null;


                if (upload.FileName.EndsWith(".xls"))
                {
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else if (upload.FileName.EndsWith(".xlsx"))
                {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                else
                {                  
                    ModelState.AddModelError("File", Resources.Resource.file_format_not_supported);
                    return null;
                }

                //reader.IsFirstRowAsColumnNames = true;

                DataSet result = reader.AsDataSet();
                reader.Close();

                if (result.Tables.Count > 0)
                    return result.Tables[0];

                return null;
            }
            else
            {
               ModelState.AddModelError("File", Resources.Resource.no_file_selected);
            }
            return null;
        }
    }
}