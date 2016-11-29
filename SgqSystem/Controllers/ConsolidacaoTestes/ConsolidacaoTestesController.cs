using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers.ConsolidacaoTestes
{
    public class ConsolidacaoTestesController : Controller
    {
        //http://localhost:63128/ConsolidacaoTestes/Index
        // GET: ConsolidacaoTestes
        public ActionResult Index()
        {
            var listaCom5Level3 = preecnheListaCom5Nivel3();

            return View();
        }

        private void consolidaPorX()
        {

        }

        private List<ObjetoNivel3> preecnheListaCom5Nivel3()
        {
            var listaLevel3 = new List<ObjetoNivel3>();


            listaLevel3.Add(new ObjetoNivel3
            {
                valor = 1,
                conforme = true
            });
            listaLevel3.Add(new ObjetoNivel3
            {
                valor = 1,
                conforme = true
            }); listaLevel3.Add(new ObjetoNivel3
            {
                valor = 1,
                conforme = true
            }); listaLevel3.Add(new ObjetoNivel3
            {
                valor = 1,
                conforme = true
            }); listaLevel3.Add(new ObjetoNivel3
            {
                valor = 1,
                conforme = true
            });
            return listaLevel3;
        }

    }

    public class ObjetoNivel3 {
        public int valor { get; set; }
        public bool conforme { get; set; }
    }
}