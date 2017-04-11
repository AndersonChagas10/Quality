using Dominio;
using Dominio.Interfaces.Services;
using Helper;
using SgqSystem.Secirity;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    [CustomAuthorize]
    [OutputCache(Duration = 20, VaryByParam = "none")]
    public class RelatoriosSgqController : BaseController
    {

        #region Constructor

        private FormularioParaRelatorioViewModel form;


        public RelatoriosSgqController(IRelatorioColetaDomain relatorioColetaDomain)
        {

            form = new FormularioParaRelatorioViewModel();

        }

        #endregion

        [FormularioPesquisa(filtraUnidadePorUsuario = true)]
        public ActionResult Scorecard()
        {
            GetMetaAtualScorecard();
            return View(form);
        }

        private void GetMetaAtualScorecard()
        {
            using (var db = new SgqDbDevEntities())
            {
                var atual = db.ParGoalScorecard.OrderByDescending(r => r.Id).FirstOrDefault();
                if (atual != null)
                {
                    ViewBag.PercentValueMid = atual.PercentValueMid;
                    ViewBag.PercentValueHigh = atual.PercentValueHigh;
                }
                else
                {
                    ViewBag.PercentValueMid = "70";
                    ViewBag.PercentValueHigh = "99";
                }
            }
        }

        public ActionResult ScorecardConfig()
        {
            GetMetaAtualScorecard();
            return View(new ParGoalScorecard());
        }

        [HttpPost]
        public ActionResult ScorecardConfig(ParGoalScorecard parGoalScorecard)
        {
            using (var db = new SgqDbDevEntities())
            {
                parGoalScorecard.InitDate = DateTime.Now;
                db.ParGoalScorecard.Add(parGoalScorecard);
                db.SaveChanges();

                var atual = db.ParGoalScorecard.OrderByDescending(r => r.Id).FirstOrDefault();
                if (atual != null)
                {
                    ViewBag.PercentValueMid = atual.PercentValueMid;
                    ViewBag.PercentValueHigh = atual.PercentValueHigh;
                }
                else
                {
                    ViewBag.PercentValueMid = "70";
                    ViewBag.PercentValueHigh = "99";
                }
            }

            return View(parGoalScorecard);
        }

        [FormularioPesquisa(filtraUnidadePorUsuario = true)]
        public ActionResult RelatorioDiario()
        {
            return View(form);
        }

        [FormularioPesquisa(filtraUnidadePorUsuario = true, parLevel1e2 = true)]
        public ActionResult ApontamentosDiarios()
        {
            return View(form);
        }

        [FormularioPesquisa(filtraUnidadePorUsuario = true)]
        public ActionResult NaoConformidade()
        {
            return View(form);
        }

        [FormularioPesquisa(filtraUnidadePorUsuario = true)]
        public ActionResult ExemploRelatorio()
        {
            return View(form);
        }

        [FormularioPesquisa(filtraUnidadePorUsuario = true)]
        public ActionResult VisaoGeralDaArea()
        {
            return View(form);
        }

        [FormularioPesquisa(filtraUnidadePorUsuario = true)]
        public ActionResult VisaoGeralDaAreaTbl1()
        {
            TabelaDinamicaResultados tabela = MockTabelaVisaoGeralDaArea();
            return View(tabela);
        }

        [FormularioPesquisa(filtraUnidadePorUsuario = true)]
        public ActionResult VisaoGeralDaAreaTbl2()
        {
            TabelaDinamicaResultados tabela = MockTabelaVisaoGeralDaArea(true);
            return PartialView("VisaoGeralDaAreaTbl1", tabela);
        }

        [FormularioPesquisa(filtraUnidadePorUsuario = true)]
        private static TabelaDinamicaResultados MockTabelaVisaoGeralDaArea(bool tbl2 = false)
        {
            var tabela = new TabelaDinamicaResultados();

            #region Cabecalhos

            tabela.trsCabecalho1 = new List<Ths>();
            tabela.trsCabecalho2 = new List<Ths>();
            tabela.trsCabecalho3 = new List<Ths>();
            var thsMeio = new List<Ths>();

            thsMeio.Add(new Ths() { name = "Orçado", coolspan = 1 });
            thsMeio.Add(new Ths() { name = "Real", coolspan = 1 });
            thsMeio.Add(new Ths() { name = "Desvio %", coolspan = 1 });
            thsMeio.Add(new Ths() { name = "Desvio $", coolspan = 1 });

            tabela.trsCabecalho1.Add(new Ths()
            {
                name = "&nbsp"
            });

            tabela.trsCabecalho1.Add(new Ths()
            {
                name = "Pacotes"
            });

            tabela.trsCabecalho2.Add(new Ths()
            {
                name = "reg1",
                coolspan = 4,
                tds = thsMeio
            });

            tabela.trsCabecalho2.Add(new Ths()
            {
                name = "reg2",
                coolspan = 4,
                tds = thsMeio
            });

            tabela.trsCabecalho2.Add(new Ths()
            {
                name = "reg3",
                coolspan = 4,
                tds = thsMeio
            });

            tabela.trsCabecalho3.Add(new Ths()
            {
                name = "Total",
                coolspan = 4,
                tds = thsMeio
            });

            #endregion

            #region Meio

            tabela.trsMeio = new List<Trs>();

            var tdsEsquerda1 = new List<Tds>();
            var tdsEsquerda2 = new List<Tds>();
            var tdsEsquerda3 = new List<Tds>();

            for (int i = 0; i < 4 * 3; i++)
            {
                tdsEsquerda1.Add(new Tds() { valor = (1M * i).ToString(), coolspan = 1, click = tbl2 ? "Grafico3(1);Grafico4(1);" : "GetTabela2()" });

                tdsEsquerda2.Add(new Tds() { valor = (6M * i).ToString(), coolspan = 1, click = tbl2 ? "Grafico3(1);Grafico4(1);" : "GetTabela2()" });

                tdsEsquerda3.Add(new Tds() { valor = (9M * i).ToString(), coolspan = 1, click = tbl2 ? "Grafico3(1);Grafico4(1);" : "GetTabela2()" });
            }

            var tdsDireita1 = new List<Tds>();
            var tdsDireita2 = new List<Tds>();
            var tdsDireita3 = new List<Tds>();

            for (int i = 0; i < 4; i++)
            {
                tdsDireita1.Add(new Tds() { valor = (i * 90M).ToString(), coolspan = 1 });

                tdsDireita2.Add(new Tds() { valor = (i * 900M).ToString(), coolspan = 1 });

                tdsDireita3.Add(new Tds() { valor = (i * 9000M).ToString(), coolspan = 1 });

            }

            tabela.trsMeio.Add(new Trs()
            {
                name = "teste1",
                tdsEsquerda = tdsEsquerda1,
                tdsDireita = tdsDireita1
            });

            tabela.trsMeio.Add(new Trs()
            {
                name = "teste2",
                tdsEsquerda = tdsEsquerda2,
                tdsDireita = tdsDireita2

            });

            tabela.trsMeio.Add(new Trs()
            {
                name = "teste3",
                tdsEsquerda = tdsEsquerda3,
                tdsDireita = tdsDireita3

            });




            #endregion

            #region Rodapé

            tabela.footer = new List<Trs>();

            var tdsDoFooter2 = new List<Tds>();
            for (int i = 0; i < 4 * 3; i++)
            {
                tdsDoFooter2.Add(new Tds()
                {
                    coolspan = 1,
                    valor = (1.2M * i).ToString()
                });
            }

            var tdsDoFooter3 = new List<Tds>();
            for (int i = 0; i < 4 * 1; i++)
            {
                tdsDoFooter3.Add(new Tds()
                {
                    coolspan = 1,
                    valor = (2.2M * i).ToString()
                });
            }

            tabela.footer.Add(new Trs()
            {
                name = "Total:",
                coolspan = 4,
                tdsEsquerda = tdsDoFooter2,
                tdsDireita = tdsDoFooter3

            });

            #endregion
            return tabela;
        }

        [FormularioPesquisa(filtraUnidadePorUsuario = true, parLevel1e2 = true)]
        public ActionResult CartasCep()
        {
            return View(form);
        }

        [FormularioPesquisa(filtraUnidadePorUsuario = true, parLevel1e2 = true)]
        public ActionResult CartasCepP()
        {
            return View(form);
        }

        [FormularioPesquisa(filtraUnidadePorUsuario = true, parLevel1e2 = true)]
        public ActionResult CartasCepX()
        {
            return View(form);
        }

        [FormularioPesquisa(filtraUnidadePorUsuario = true)]
        public ActionResult RelatorioGenerico()
        {
            return View(form);
        }

    }

    public class TabelaDinamicaResultados
    {
        public List<Trs> trsMeio { get; set; }

        public List<Ths> trsCabecalho1 { get; set; }
        public List<Ths> trsCabecalho2 { get; set; }
        public List<Ths> trsCabecalho3 { get; set; }

        public List<Trs> footer { get; set; }
    }

    public class Trs
    {
        public string name { get; set; }
        public int coolspan { get; set; }

        public List<Tds> tdsEsquerda { get; set; }
        public List<Tds> tdsDireita { get; set; }
    }

    public class Tds
    {
        public string valor { get; set; }
        public string click { get; set; }
        public int coolspan { get; set; }
    }

    public class Ths
    {
        public string name { get; set; }
        public int coolspan { get; set; }
        public List<Ths> tds { get; set; }
    }

    //public class Footers
    //{
    //    public decimal valor { get; set; }
    //}

}