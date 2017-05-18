﻿using Dominio;
using Dominio.Interfaces.Services;
using DTO.ResultSet;
using Helper;
using SgqSystem.Secirity;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            //Retorna as Roles do usuário logado para filtrar o botão de edição
            HttpCookie cookie = HttpContext.Request.Cookies.Get("webControlCookie");
            var db = new SgqDbDevEntities();
            List<string> Retorno = new List<string>();

            int _userId = 0;
            if (!string.IsNullOrEmpty(cookie.Values["roles"]))
            {
                _userId = Convert.ToInt32(cookie.Values["userId"].ToString());
            }
                      
            var roles = db.ParCompanyXUserSgq.Where(r => r.UserSgq_Id == _userId).ToList();
            
            foreach (var role in roles)
            {
                Retorno.Add(role.Role);
            }

            ViewBag.Roles = Retorno;
            //Fim da Role

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

        /// <summary>
        /// Cria mock para tabela dinamica Visão Geral da área
        /// 
        /// Params(bool): tbl2: caso seja tabela 2, para alterar o onclick da TD de dados.
        /// 
        /// ----------------------
        /// Query Headers (Region > Cabecalhos):
        /// 
        /// 1º Devem ser declaradas quantas LINHAS possuem no cabeçalho e seus NOMES:
        /// Query:
        ///     Coluna    |   Tipagem
        ///               |
        ///     name      |   string
        ///     coolspan  |   int
        /// Objeto de retorno: Ths.
        /// 
        /// 2º PARA CADA OBJETO DO ITEM 1, DEVE EXISTIR SEU CORRESPONDENTE DESTE ITEM. Query com os valores  > Ths:
        /// Query:
        ///     Coluna    |   Tipagem
        ///               |
        ///     name      |   string
        ///     coolspan  |   int
        ///     
        /// Objeto de retorno: Ths.
        /// 
        /// 
        /// 
        ///     Ex:
        ///     
        ///     
        /// 
        /// ----------------------
        /// 
        /// ----------------------
        /// Query Body (Region > Meio):
        /// 
        /// 
        /// ----------------------
        /// ----------------------
        /// Query Footer (Region > Rodapé):
        /// 
        /// 
        /// ----------------------
        /// 
        /// 
        /// Return "TabelaDinamicaResultados" object.
        /// </summary>
        /// <param name="tbl2"></param>
        /// <returns></returns>
        [FormularioPesquisa(filtraUnidadePorUsuario = true)]
        public static TabelaDinamicaResultados MockTabelaVisaoGeralDaArea(bool tbl2 = false)
        {
            var tabela = new TabelaDinamicaResultados();

            #region Cabecalhos

            /*1º*/
            tabela.trsCabecalho1 = new List<Ths>();

            tabela.trsCabecalho1.Add(new Ths()
            {
                name = " "
            });

            tabela.trsCabecalho1.Add(new Ths()
            {
                name = "Pacotes"
            });
            /*Fim  1º*/

            /*2º CRIANDO CABECALHO DA SEGUNDA TABELA*/
            tabela.trsCabecalho2 = new List<Ths>();
            tabela.trsCabecalho2.Add(new Ths()
            {
                name = "reg1", //TITULO DO AGRUPAMENTO EX: REG1, REG2, ETC...
                coolspan = 4,
                //tds = thsMeio
            });

            tabela.trsCabecalho2.Add(new Ths()
            {
                name = "reg2",
                coolspan = 4,
                //tds = thsMeio
            });

            tabela.trsCabecalho2.Add(new Ths()
            {
                name = "reg3",
                coolspan = 4,
                //tds = thsMeio
            });


            var thsMeio = new List<Ths>();
            thsMeio.Add(new Ths() { name = "Orçado", coolspan = 1 });
            thsMeio.Add(new Ths() { name = "Real", coolspan = 1 });
            thsMeio.Add(new Ths() { name = "Desvio %", coolspan = 1 });
            thsMeio.Add(new Ths() { name = "Desvio $", coolspan = 1 });

            foreach (var i in tabela.trsCabecalho2)
            {
                i.tds = thsMeio; //ESTA PROPERTY DEVE CONTER OS ITENS AGRUPADOS (EX: OÇADO, REAL, DESVIO ETC....)
            }

            tabela.trsCabecalho3 = new List<Ths>();
            tabela.trsCabecalho3.Add(new Ths()
            {
                name = "Total",
                coolspan = 4,
                tds = thsMeio
            });

            /*Fim  2º*/
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

        [FormularioPesquisa(filtraUnidadeDoUsuario = true, parLevel1e2 = true)]
        public ActionResult CartasCep()
        {
            return View(form);
        }

        [FormularioPesquisa(filtraUnidadePorUsuario = true)]
        public ActionResult RelatorioGenerico()
        {
            return View(form);
        }

    }

}