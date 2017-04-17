using Dominio;
using SgqSystem.ViewModels;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;

namespace SgqSystem.Controllers.Api.RelatoriosBrasil
{
    //[RoutePrefix("api/RelatorioGenerico")]
    public class RelatorioGenericoApiController : Controller
    {

        [HttpPost]
        //[Route("Get")]
        public ActionResult GetRelatorio(FormularioParaRelatorioViewModel form)
        {

            var body = new List<PropriedadesGenericas>();
            var header = new List<PropriedadesGenericasHeader>();
            var retorno = new Retorno();
            //dynamic retorno = new ExpandoObject();

            var db = new SgqDbDevEntities();
            //var item = db.Database.SqlQuery<object>("").FirstOrDefault();
            //var properties = item.GetType().GetProperties();

            var list = db.UserSgq.ToList();
            var headers = db.Database.SqlQuery<string>("select Column_name from Information_schema.columns where Table_name like 'UserSgq'").ToList();

            foreach (var item in headers)
            {
                header.Add(new PropriedadesGenericasHeader() { title = item, mData = "Col" });
            }

            //body.Add(new PropriedadesGenericas() { Col0 = , Col1 =  });

            foreach (var item in list)
            {
                body.Add(new PropriedadesGenericas()
                {

                    Col0 = item.Id.ToString(),
                    Col1 = item.Name,
                    Col2 = item.Password,
                    Col3 = item.AcessDate.ToString(),
                    Col4 = item.AddDate.ToString(),
                    Col5 = item.AlterDate.ToString(),
                    Col6 = item.Role,
                    Col7 = item.FullName,
                    Col8 = item.Email,
                    Col9 = item.Phone,
                    Col10 = item.ParCompany_Id.ToString(),
                    Col11 = item.PasswordDate.ToString()

                });
        }

            for (int i = 0; i< 10; i++)
            {
                body.Add(new PropriedadesGenericas() { Col0 = "teste coluna 1 " + i, Col1 = "teste coluna 2 " + i, Col2 = "teste coluna 3 " + i });
            }

            retorno.header = header;
            retorno.body = body;


            return PartialView("DataTable", retorno);
}

    }

    public class PropriedadesGenericas
{
    public string Col0 { get; set; }
    public string Col1 { get; set; }
    public string Col2 { get; set; }
    public string Col3 { get; set; }
    public string Col4 { get; set; }
    public string Col5 { get; set; }
    public string Col6 { get; set; }
    public string Col7 { get; set; }
    public string Col8 { get; set; }
    public string Col9 { get; set; }
    public string Col10 { get; set; }
    public string Col11 { get; set; }
    public string Col12 { get; set; }
    public string Col13 { get; set; }
    public string Col14 { get; set; }
    public string Col15 { get; set; }
    public string Col16 { get; set; }
    public string Col17 { get; set; }
    public string Col18 { get; set; }
    public string Col19 { get; set; }
    public string Col20 { get; set; }
    public string Col21 { get; set; }
    public string Col22 { get; set; }
    public string Col23 { get; set; }
    public string Col24 { get; set; }
    public string Col25 { get; set; }
    public string Col26 { get; set; }
    public string Col27 { get; set; }
    public string Col28 { get; set; }
    public string Col29 { get; set; }
    public string Col30 { get; set; }
    public string Col31 { get; set; }
    public string Col32 { get; set; }
    public string Col33 { get; set; }
    public string Col34 { get; set; }
    public string Col35 { get; set; }
    public string Col36 { get; set; }
    public string Col37 { get; set; }
    public string Col38 { get; set; }
    public string Col39 { get; set; }
    public string Col40 { get; set; }
    public string Col41 { get; set; }
    public string Col42 { get; set; }
    public string Col43 { get; set; }
    public string Col44 { get; set; }
    public string Col45 { get; set; }
    public string Col46 { get; set; }
    public string Col47 { get; set; }
    public string Col48 { get; set; }
    public string Col49 { get; set; }
    public string Col50 { get; set; }
}

public class PropriedadesGenericasHeader
{
    public string title { get; set; }
    public string mData { get; set; }
}

public class Retorno
{
    public List<PropriedadesGenericas> body { get; set; }
    public List<PropriedadesGenericasHeader> header { get; set; }
}
}
