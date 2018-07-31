using Dominio;
using ExcelDataReader;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    public class ImportacaoExcelController : BaseController
    {

        public SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ImportacaoExcel
        public ActionResult Index()
        {
            var listaDeFormatos = db.ImportFormat.Where(x => x.IsActive).ToList();
            ViewBag.Formatos = new SelectList(listaDeFormatos, "Id", "Title");
            return View();
        }

        public ActionResult Importar(HttpPostedFileBase arquivo)
        {

            var dicionarioCabecalho = new Dictionary<int, string>();
            var dicionarioDados = new List<Dictionary<string, string>>();
            DataTable dt = UploadExcelTo(arquivo);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                if (i == 0)
                {
                    if (row[0] != null && row[0].ToString().Length > 0)
                    {
                        for (var j = 0; j < row.ItemArray.Length; j++)
                        {
                            if (row[j].ToString().Length > 0)
                                dicionarioCabecalho.Add(j, row[j].ToString());
                        }
                    }
                }
                else
                {
                    var dicionarioDadosCorpo = new Dictionary<string, string>();
                    foreach (var item in dicionarioCabecalho)
                    {
                        dicionarioDadosCorpo.Add(item.Value, row[item.Key].ToString());
                    }
                    dicionarioDados.Add(dicionarioDadosCorpo);
                }
            }
            var listaDeFormatos = db.ImportFormat.Where(x => x.IsActive).ToList();
            ViewBag.Formatos = new SelectList(listaDeFormatos, "Id", "Title");
            return View("Index", dicionarioDados);

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