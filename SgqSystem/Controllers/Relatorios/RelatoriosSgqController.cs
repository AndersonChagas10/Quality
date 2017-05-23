using Dominio;
using Dominio.Interfaces.Services;
using DTO;
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

        #region Visao Geral da Area

        [FormularioPesquisa(filtraUnidadePorUsuario = true)]
        public ActionResult GetTable(DataCarrierFormulario form)
        {
            TabelaDinamicaResultados tabela;
            if (form.Query.Equals("GetTbl1"))
            {
                //tabela = GetTbl1(form);
            }
            else if (form.Query.Equals("GetTbl2"))
            {
                //tabela = GetTbl2(form);
            }

            tabela = MockTabelaVisaoGeralDaArea();
            tabela.CallBackTableBody = form.CallBackTableBody;
            tabela.Title = form.Title;
            return View(tabela);
        }

        public TabelaDinamicaResultados GetTbl1(DataCarrierFormulario form)
        {
            #region Queryes Trs Meio

            var tabela = new TabelaDinamicaResultados();

            var where = string.Empty;
            where += "";

            if (!string.IsNullOrEmpty(form.ParametroTableRow[0]))
            {
                where += "\n  MACROPROCESSO = '" + form.ParametroTableRow[0] + "' ";
            }
            //if (!string.IsNullOrEmpty(form.ParametroTableRow[1]))
            //{
            //    //where += "\n AND MACROPROCESSO = '" + form.ParametroTableRow[0] + "' ";
            //}
            //if (!string.IsNullOrEmpty(form.ParametroTableRow[2]))
            //{
            //    //where += "\n AND MACROPROCESSO = '" + form.ParametroTableRow[0] + "' ";
            //}

            //Nomes das colunas do corpo da tabela de dados central
            var query0 = "SELECT  Distinct ( MACROPROCESSO ) name, 4 coolspan  FROM NAVEGACAO2";
            
            //Dados das colunas do corpo da tabela de dados central
            var query1 = "SELECT CLASSIFIC_NEGOCIO, MACROPROCESSO, " +
                  "\n CAST (AVG (REAL) AS DECIMAL(5,2)) as REAL," +
                  "\n AVG(META) as ORCADO, " +
                  "\n CAST ((AVG(META) - AVG(REAL))  AS DECIMAL(5,2)) as DESVIO, " +
                  "\n CAST ( ((AVG(META) - AVG(REAL)) / AVG(META) ) AS DECIMAL(5,2)) *100 as \"DESVIOPERCENTUAL\" " +
                  "\n FROM NAVEGACAO2 " +
                  "\n GROUP BY MACROPROCESSO, CLASSIFIC_NEGOCIO " +
                  "\n ORDER BY 1, 2";

            // Total Direita
            var query2 =
            "\n                 SELECT                                                                                                  " +
            "\n         CLASSIFIC_NEGOCIO.CLASSIFIC_NEGOCIO                                                                             " +
            "\n         ,CAST(AVG(REAL)AS DECIMAL(5, 2)) AS REAL                                                                        " +
            "\n         , CAST (AVG(ORCADO)AS DECIMAL(5, 2)) AS ORCADO                                                                  " +
            "\n           , CAST (AVG(REAL) - AVG(ORCADO)AS DECIMAL(5, 2)) AS DESVIO                                                    " +
            "\n               , CAST (((AVG(ORCADO) - AVG(REAL)) / AVG(ORCADO)) AS DECIMAL(5, 2))*100 as \"DESVIOPERCENTUAL\"           " +
            "\n FROM(                                                                                                                   " +
            "\n     SELECT CLASSIFIC_NEGOCIO, MACROPROCESSO,                                                                            " +
            "\n      CAST(AVG(REAL) AS DECIMAL(5, 2)) as REAL,                                                                          " +
            "\n      AVG(META) as ORCADO,                                                                                               " +
            "\n      CAST((AVG(META) - AVG(REAL))  AS DECIMAL(5, 2)) as DESVIO,                                                         " +
            "\n      CAST(((AVG(META) - AVG(REAL)) / AVG(META)) AS DECIMAL(5, 2)) * 100 as \"DESVIOPERCENTUAL\"                         " +
            "\n      FROM NAVEGACAO2                                                                                                    " +
            "\n      GROUP BY MACROPROCESSO, CLASSIFIC_NEGOCIO                                                                          " +
            "\n      ORDER BY 1, 2                                                                                                      " +
            "\n      )CLASSIFIC_NEGOCIO                                                                                                 " +
            "\n GROUP BY                                                                                                                " +
            "\n         CLASSIFIC_NEGOCIO.CLASSIFIC_NEGOCIO                                                                             ";

            // Total Inferior Esquerda
            var query3 =
            "\n                 SELECT                                                                                                  " +
            "\n         MACROPROCESSO.MACROPROCESSO                                                                                     " +
            "\n         ,CAST(AVG(REAL)AS DECIMAL(5, 2)) AS REAL                                                                        " +
            "\n         , CAST (AVG(ORCADO)AS DECIMAL(5, 2)) AS ORCADO                                                                  " +
            "\n           , CAST (AVG(REAL) - AVG(ORCADO)AS DECIMAL(5, 2)) AS DESVIO                                                    " +
            "\n               , CAST (((AVG(ORCADO) - AVG(REAL)) / AVG(ORCADO)) AS DECIMAL(5, 2))*100 as \"DESVIOPERCENTUAL\"           " +
            "\n FROM(                                                                                                                   " +
            "\n     SELECT CLASSIFIC_NEGOCIO, MACROPROCESSO,                                                                            " +
            "\n      CAST(AVG(REAL) AS DECIMAL(5, 2)) as REAL,                                                                          " +
            "\n      AVG(META) as ORCADO,                                                                                               " +
            "\n      CAST((AVG(META) - AVG(REAL))  AS DECIMAL(5, 2)) as DESVIO,                                                         " +
            "\n      CAST(((AVG(META) - AVG(REAL)) / AVG(META)) AS DECIMAL(5, 2)) * 100 as \"DESVIOPERCENTUAL\"                         " +
            "\n      FROM NAVEGACAO2                                                                                                    " +
            "\n      GROUP BY MACROPROCESSO, CLASSIFIC_NEGOCIO                                                                          " +
            "\n      )MACROPROCESSO                                                                                                     " +
            "\n GROUP BY                                                                                                                " +
            "\n         MACROPROCESSO.MACROPROCESSO                                                                                     " +
            "\n       ORDER BY CASE WHEN MACROPROCESSO = 'SEM MACROPROCESSO DEFINIDO' THEN 'Z' ELSE MACROPROCESSO END DESC                  ";

            // Total Inferior Direita
            var query4 = "SELECT " +
                "\n CAST ( AVG(REAL) AS DECIMAL(5,2)) as REAL, " +
                "\n AVG(META) as ORCADO, " +
                "\n CAST ( (AVG(META) - AVG(REAL)) AS DECIMAL(5,2)) as DESVIO, " +
                "\n CAST ( ((AVG(META) - AVG(REAL)) / AVG(META)) AS DECIMAL(5,2)) as \"DESVIOPERCENTUAL\" " +
                "\n FROM NAVEGACAO2";

            //Nome das linhas da tabela esquerda por ex, indicador X, indicador Y (de uma unidade X, y...)
            var query6 = "SELECT DISTINCT CLASSIFIC_NEGOCIO FROM NAVEGACAO2";

            var db = new SgqDbDevEntities();
            var result1 = db.Database.SqlQuery<ResultQuery1>(query1).ToList();
            var result2 = db.Database.SqlQuery<ResultQuery1>(query2).ToList();
            var result3 = db.Database.SqlQuery<ResultQuery1>(query3).ToList();
            var result4 = db.Database.SqlQuery<ResultQuery1>(query4).ToList();
            var queryRowsBody = db.Database.SqlQuery<string>(query6).ToList();

            #endregion

            #region Cabecalhos

            /*1º*/
            tabela.trsCabecalho1 = new List<Ths>();
            tabela.trsCabecalho1.Add(new Ths() { name = "" });
            tabela.trsCabecalho1.Add(new Ths() { name = "Pacotes" });
            /*Fim  1º*/

            #region DESCRIÇÃO
            /*2º CRIANDO CABECALHO DA SEGUNDA TABELA

                  name   | coolspan
                  ------------------
                   Reg1   | 4 
                   Reg2   | 4
                   RegN   | 4

                  coolspan depende do que vai mostrar em Orçado, real, Desvio, etc...
               */
            #endregion
            tabela.trsCabecalho2 = db.Database.SqlQuery<Ths>(query0).OrderBy(r => r.name).ToList();

            var thsMeio = new List<Ths>();
            thsMeio.Add(new Ths() { name = "Real", coolspan = 1 });
            thsMeio.Add(new Ths() { name = "Orçado", coolspan = 1 });
            thsMeio.Add(new Ths() { name = "Desvio $", coolspan = 1 });
            thsMeio.Add(new Ths() { name = "Desvio %", coolspan = 1 });

            foreach (var i in tabela.trsCabecalho2)
                i.tds = thsMeio; //ESTA PROPERTY DEVE CONTER OS ITENS AGRUPADOS (EX: OÇADO, REAL, DESVIO ETC....)

            tabela.trsCabecalho3 = new List<Ths>();
            tabela.trsCabecalho3.Add(new Ths() { name = "Total", coolspan = 4, tds = thsMeio });

            /*Fim  2º*/
            #endregion

            #region Meio

            tabela.trsMeio = new List<Trs>();

            #region DESCRIÇÃO
            /*tdsEsquerda e tdsDireita:

                    LISTA DE TDS, cada row deve ser uma TD, por ex, 
                    uma para REG 1 com os dados para 
                    as Colunas: Real	Desvio %	Desvio $	Orçado, 
                    devem estar em 1 ROW do resultado do SQL, a REG 2,
                    na ROW consecutiva, até REG N.

                   O Resultado Ficara (Query para LINHA Teste1): 

                   Row     | TH   | Col       | valor | coolspan    > new List<Tds>();
                   ----------------------------------------------
                   Teste1  | REG1 | Orçado    | 1     | 1           > new Tds() { valor = 1, coolspan = 1 };
                   Teste1  | REG1 | Real      | 2     | 1           > new Tds() { valor = 2, coolspan = 1 };
                   Teste1  | REG1 | Desvio %  | 3     | 1           .   
                   Teste1  | REG1 | Desvio $  | 4     | 1           .   
                   ----------------------------------------------   .
                   Teste1  | REG2 | Orçado    | 5     | 1
                   Teste1  | REG2 | Real      | 6     | 1
                   Teste1  | REG2 | Desvio %  | 7     | 1
                   Teste1  | REG2 | Desvio $  | 8     | 1
                   ----------------------------------------------
                   Teste1  | REGN | Orçado    | -     | 1
                   Teste1  | REGN | Real      | -    | 1
                   Teste1  | REGN | Desvio %  | -    | 1
                   Teste1  | REGN | Desvio $  | -    | 1
                   ----------------------------------------------
                   Teste2  | REG1 | Orçado    | 1     | 1        
                   Teste2 | REG1 | Real      | 2     | 1        
                   Teste2  | REG1 | Desvio %  | 3     | 1        
                   Teste2  | REG1 | Desvio $  | 4     | 1        
                   ----------------------------------------------
                   Teste2  | REG2 | Orçado    | 5     | 1
                   Teste2  | REG2 | Real      | 6     | 1
                   Teste2  | REG2 | Desvio %  | 7     | 1
                   Teste2  | REG2 | Desvio $  | 8     | 1
                   ----------------------------------------------
                   Teste2  | REGN | Orçado    | 9     | 1
                   Teste2  | REGN | Real      | 10    | 1
                   Teste2  | REGN | Desvio %  | 11    | 1
                   Teste2  | REGN | Desvio $  | 12    | 1

                   OBS: mesmo que a query retorne, para facilitar a coluna TH , col, ROW, o sistema só considera as colunas coolspan e valor.

                   O mesmo para tdsDireita:

                   Row     | TH    | Col        | valor | coolspan
                   ----------------------------------------------
                   Teste1  | TOTAL | Orçado    | 10    | 1
                   Teste1  | TOTAL | Real      | 12    | 1
                   Teste1  | TOTAL | Desvio %  | 14    | 1
                   Teste1  | TOTAL | Desvio $  | 16    | 1

                    */
            //"; 
            #endregion
            foreach (var i in queryRowsBody)
            {

                var filtro = result1.Where(r => r.CLASSIFIC_NEGOCIO.Equals(i)).ToList();
                var Tr = new Trs()
                {
                    name = i,
                    tdsEsquerda = new List<Tds>(),
                    tdsDireita = new List<Tds>()
                };

                #region Result1 

                /*Caso não exista MACROPROCESSO*/
                foreach (var x in tabela.trsCabecalho2)
                    if (!filtro.Any(r => r.MACROPROCESSO.Equals(x.name)))
                        filtro.Add(new ResultQuery1() { MACROPROCESSO = x.name, CLASSIFIC_NEGOCIO = filtro.FirstOrDefault().CLASSIFIC_NEGOCIO });
                filtro = filtro.OrderBy(r => r.MACROPROCESSO).ToList();
                foreach (var ii in filtro)
                {
                    Tr.tdsEsquerda.Add(new Tds() { valor = Decimal.Round(ii.REAL, 2).ToString("G29") });
                    Tr.tdsEsquerda.Add(new Tds() { valor = Decimal.Round(ii.ORCADO, 2).ToString("G29") });
                    Tr.tdsEsquerda.Add(new Tds() { valor = Decimal.Round(ii.DESVIO, 2).ToString("G29") });
                    Tr.tdsEsquerda.Add(new Tds() { valor = Decimal.Round(ii.DESVIOPERCENTUAL, 2).ToString("G29") });
                }

                #endregion

                #region Result2

                filtro = result2.Where(r => r.CLASSIFIC_NEGOCIO.Equals(i)).ToList();
                foreach (var ii in filtro)
                {
                    Tr.tdsDireita.Add(new Tds() { valor = Decimal.Round(ii.REAL, 2).ToString("G29") });
                    Tr.tdsDireita.Add(new Tds() { valor = Decimal.Round(ii.ORCADO, 2).ToString("G29") });
                    Tr.tdsDireita.Add(new Tds() { valor = Decimal.Round(ii.DESVIO, 2).ToString("G29") });
                    Tr.tdsDireita.Add(new Tds() { valor = Decimal.Round(ii.DESVIOPERCENTUAL, 2).ToString("G29") });
                }

                #endregion

                tabela.trsMeio.Add(Tr);
            }

            #endregion

            #region Rodapé

            var queryRowsFooter = new List<string>();// TOTAL por ex.
            queryRowsFooter.Add("Total");
            tabela.footer = new List<Trs>();
            foreach (var i in queryRowsFooter)
            {
                //var filtro = result3.Where(r => r.CLASSIFIC_NEGOCIO.Equals(i)).ToList();
                var Tr = new Trs()
                {
                    name = i,
                    tdsEsquerda = new List<Tds>(),
                    tdsDireita = new List<Tds>()
                };

                #region Result3

                foreach (var ii in result3)
                {
                    Tr.tdsEsquerda.Add(new Tds() { valor = Decimal.Round(ii.REAL, 2).ToString("G29") });
                    Tr.tdsEsquerda.Add(new Tds() { valor = Decimal.Round(ii.ORCADO, 2).ToString("G29") });
                    Tr.tdsEsquerda.Add(new Tds() { valor = Decimal.Round(ii.DESVIO, 2).ToString("G29") });
                    Tr.tdsEsquerda.Add(new Tds() { valor = Decimal.Round(ii.DESVIOPERCENTUAL, 2).ToString("G29") });
                }

                #endregion

                #region Result4

                foreach (var ii in result4)
                {
                    Tr.tdsDireita.Add(new Tds() { valor = Decimal.Round(ii.REAL, 2).ToString("G29") });
                    Tr.tdsDireita.Add(new Tds() { valor = Decimal.Round(ii.ORCADO, 2).ToString("G29") });
                    Tr.tdsDireita.Add(new Tds() { valor = Decimal.Round(ii.DESVIO, 2).ToString("G29") });
                    Tr.tdsDireita.Add(new Tds() { valor = Decimal.Round(ii.DESVIOPERCENTUAL, 2).ToString("G29") });
                }

                #endregion

                tabela.footer.Add(Tr);
            }

            #endregion

            return tabela;
        }

        public TabelaDinamicaResultados GetTbl2(DataCarrierFormulario form)
        {
            #region Queryes Trs Meio
            var tabela = new TabelaDinamicaResultados();

            var where = string.Empty;
            where += "";

            if (!string.IsNullOrEmpty(form.ParametroTableRow[0]))
            {
                where += "\n  MACROPROCESSO = '" + form.ParametroTableRow[0] + "' ";
            }
            //if (!string.IsNullOrEmpty(form.ParametroTableRow[1]))
            //{
            //    //where += "\n AND MACROPROCESSO = '" + form.ParametroTableRow[0] + "' ";
            //}
            //if (!string.IsNullOrEmpty(form.ParametroTableRow[2]))
            //{
            //    //where += "\n AND MACROPROCESSO = '" + form.ParametroTableRow[0] + "' ";
            //}

            //Nomes das colunas do corpo da tabela de dados central
            var query0 = "SELECT  Distinct ( UNIDADE ) name, 4 coolspan  FROM NAVEGACAO2 " + where;

            //Dados das colunas do corpo da tabela de dados central
            var query1 = "SELECT CLASSIFIC_NEGOCIO, UNIDADE AS MACROPROCESSO, " +
                  "\n CAST (AVG (REAL) AS DECIMAL(5,2)) as REAL," +
                  "\n AVG(META) as ORCADO, " +
                  "\n CAST ((AVG(META) - AVG(REAL))  AS DECIMAL(5,2)) as DESVIO, " +
                  "\n CAST ( ((AVG(META) - AVG(REAL)) / AVG(META) ) AS DECIMAL(5,2)) *100 as \"DESVIOPERCENTUAL\" " +
                  "\n FROM NAVEGACAO2 " +
                  where +
                  "\n GROUP BY UNIDADE, CLASSIFIC_NEGOCIO " +
                  "\n ORDER BY 1, 2";

            // Total Direita
            var query2 =
            "\n                 SELECT                                                                                                  " +
            "\n         CLASSIFIC_NEGOCIO.CLASSIFIC_NEGOCIO                                                                             " +
            "\n         ,CAST(AVG(REAL)AS DECIMAL(5, 2)) AS REAL                                                                        " +
            "\n         , CAST (AVG(ORCADO)AS DECIMAL(5, 2)) AS ORCADO                                                                  " +
            "\n           , CAST (AVG(REAL) - AVG(ORCADO)AS DECIMAL(5, 2)) AS DESVIO                                                    " +
            "\n               , CAST (((AVG(ORCADO) - AVG(REAL)) / AVG(ORCADO)) AS DECIMAL(5, 2))*100 as \"DESVIOPERCENTUAL\"           " +
            "\n FROM(                                                                                                                   " +
            "\n     SELECT CLASSIFIC_NEGOCIO, UNIDADE ,                                                                            " +
            "\n      CAST(AVG(REAL) AS DECIMAL(5, 2)) as REAL,                                                                          " +
            "\n      AVG(META) as ORCADO,                                                                                               " +
            "\n      CAST((AVG(META) - AVG(REAL))  AS DECIMAL(5, 2)) as DESVIO,                                                         " +
            "\n      CAST(((AVG(META) - AVG(REAL)) / AVG(META)) AS DECIMAL(5, 2)) * 100 as \"DESVIOPERCENTUAL\"                         " +
            "\n      FROM NAVEGACAO2                                                                                                    " +
            where +
            "\n      GROUP BY UNIDADE, CLASSIFIC_NEGOCIO                                                                          " +
            "\n      ORDER BY 1, 2                                                                                                      " +
            "\n      )CLASSIFIC_NEGOCIO                                                                                                 " +
            "\n GROUP BY                                                                                                                " +
            "\n         CLASSIFIC_NEGOCIO.CLASSIFIC_NEGOCIO                                                                             ";

            // Total Inferior Esquerda
            var query3 =
            "\n                 SELECT                                                                                                  " +
            "\n         UNIDADE.UNIDADE  AS MACROPROCESSO                                                                                    " +
            "\n         ,CAST(AVG(REAL)AS DECIMAL(5, 2)) AS REAL                                                                        " +
            "\n         , CAST (AVG(ORCADO)AS DECIMAL(5, 2)) AS ORCADO                                                                  " +
            "\n           , CAST (AVG(ORCADO) - AVG(REAL)AS DECIMAL(5, 2)) AS DESVIO                                                    " +
            "\n               , CAST (((AVG(ORCADO) - AVG(REAL)) / AVG(ORCADO)) AS DECIMAL(5, 2))*100 as \"DESVIOPERCENTUAL\"           " +
            "\n FROM(                                                                                                                   " +
            "\n     SELECT CLASSIFIC_NEGOCIO, UNIDADE,                                                                            " +
            "\n      CAST(AVG(REAL) AS DECIMAL(5, 2)) as REAL,                                                                          " +
            "\n      AVG(META) as ORCADO,                                                                                               " +
            "\n      CAST((AVG(META) - AVG(REAL))  AS DECIMAL(5, 2)) as DESVIO,                                                         " +
            "\n      CAST(((AVG(META) - AVG(REAL)) / AVG(META)) AS DECIMAL(5, 2)) * 100 as \"DESVIOPERCENTUAL\"                         " +
            "\n      FROM NAVEGACAO2                                                                                                    " +
            where +
            "\n      GROUP BY UNIDADE, CLASSIFIC_NEGOCIO                                                                          " +
            "\n      )UNIDADE                                                                                                     " +
            "\n GROUP BY                                                                                                                " +
            "\n         UNIDADE.UNIDADE                                                                                     " +
            "\n       ORDER BY UNIDADE DESC                   ";

            // Total Inferior Direita
            var query4 = "SELECT " +
                "\n CAST ( AVG(REAL) AS DECIMAL(5,2)) as REAL, " +
                "\n AVG(META) as ORCADO, " +
                "\n CAST ( (AVG(META) - AVG(REAL)) AS DECIMAL(5,2)) as DESVIO, " +
                "\n CAST ( ((AVG(META) - AVG(REAL)) / AVG(META)) AS DECIMAL(5,2)) as \"DESVIOPERCENTUAL\" " +
                "\n FROM NAVEGACAO2" +
                where;

            //Nome das linhas da tabela esquerda por ex, indicador X, indicador Y (de uma unidade X, y...)
            var query6 = "SELECT DISTINCT CLASSIFIC_NEGOCIO FROM NAVEGACAO2 " + where;

            var db = new SgqDbDevEntities();
            var result1 = db.Database.SqlQuery<ResultQuery1>(query1).ToList();
            var result2 = db.Database.SqlQuery<ResultQuery1>(query2).ToList();
            var result3 = db.Database.SqlQuery<ResultQuery1>(query3).ToList();
            var result4 = db.Database.SqlQuery<ResultQuery1>(query4).ToList();
            var queryRowsBody = db.Database.SqlQuery<string>(query6).ToList();

            #endregion

            #region Cabecalhos

            /*1º*/
            tabela.trsCabecalho1 = new List<Ths>();
            tabela.trsCabecalho1.Add(new Ths() { name = "" });
            tabela.trsCabecalho1.Add(new Ths() { name = "Pacotes" });
            /*Fim  1º*/

            #region DESCRIÇÃO
            /*2º CRIANDO CABECALHO DA SEGUNDA TABELA

                  name   | coolspan
                  ------------------
                   Reg1   | 4 
                   Reg2   | 4
                   RegN   | 4

                  coolspan depende do que vai mostrar em Orçado, real, Desvio, etc...
               */
            #endregion
            tabela.trsCabecalho2 = db.Database.SqlQuery<Ths>(query0).OrderBy(r => r.name).ToList();

            var thsMeio = new List<Ths>();
            thsMeio.Add(new Ths() { name = "Real", coolspan = 1 });
            thsMeio.Add(new Ths() { name = "Orçado", coolspan = 1 });
            thsMeio.Add(new Ths() { name = "Desvio $", coolspan = 1 });
            thsMeio.Add(new Ths() { name = "Desvio %", coolspan = 1 });

            foreach (var i in tabela.trsCabecalho2)
                i.tds = thsMeio; //ESTA PROPERTY DEVE CONTER OS ITENS AGRUPADOS (EX: OÇADO, REAL, DESVIO ETC....)

            tabela.trsCabecalho3 = new List<Ths>();
            tabela.trsCabecalho3.Add(new Ths() { name = "Total", coolspan = 4, tds = thsMeio });

            /*Fim  2º*/
            #endregion

            #region Meio

            tabela.trsMeio = new List<Trs>();

            #region DESCRIÇÃO
            /*tdsEsquerda e tdsDireita:

                    LISTA DE TDS, cada row deve ser uma TD, por ex, 
                    uma para REG 1 com os dados para 
                    as Colunas: Real	Desvio %	Desvio $	Orçado, 
                    devem estar em 1 ROW do resultado do SQL, a REG 2,
                    na ROW consecutiva, até REG N.

                   O Resultado Ficara (Query para LINHA Teste1): 

                   Row     | TH   | Col       | valor | coolspan    > new List<Tds>();
                   ----------------------------------------------
                   Teste1  | REG1 | Orçado    | 1     | 1           > new Tds() { valor = 1, coolspan = 1 };
                   Teste1  | REG1 | Real      | 2     | 1           > new Tds() { valor = 2, coolspan = 1 };
                   Teste1  | REG1 | Desvio %  | 3     | 1           .   
                   Teste1  | REG1 | Desvio $  | 4     | 1           .   
                   ----------------------------------------------   .
                   Teste1  | REG2 | Orçado    | 5     | 1
                   Teste1  | REG2 | Real      | 6     | 1
                   Teste1  | REG2 | Desvio %  | 7     | 1
                   Teste1  | REG2 | Desvio $  | 8     | 1
                   ----------------------------------------------
                   Teste1  | REGN | Orçado    | -     | 1
                   Teste1  | REGN | Real      | -    | 1
                   Teste1  | REGN | Desvio %  | -    | 1
                   Teste1  | REGN | Desvio $  | -    | 1
                   ----------------------------------------------
                   Teste2  | REG1 | Orçado    | 1     | 1        
                   Teste2 | REG1 | Real      | 2     | 1        
                   Teste2  | REG1 | Desvio %  | 3     | 1        
                   Teste2  | REG1 | Desvio $  | 4     | 1        
                   ----------------------------------------------
                   Teste2  | REG2 | Orçado    | 5     | 1
                   Teste2  | REG2 | Real      | 6     | 1
                   Teste2  | REG2 | Desvio %  | 7     | 1
                   Teste2  | REG2 | Desvio $  | 8     | 1
                   ----------------------------------------------
                   Teste2  | REGN | Orçado    | 9     | 1
                   Teste2  | REGN | Real      | 10    | 1
                   Teste2  | REGN | Desvio %  | 11    | 1
                   Teste2  | REGN | Desvio $  | 12    | 1

                   OBS: mesmo que a query retorne, para facilitar a coluna TH , col, ROW, o sistema só considera as colunas coolspan e valor.

                   O mesmo para tdsDireita:

                   Row     | TH    | Col        | valor | coolspan
                   ----------------------------------------------
                   Teste1  | TOTAL | Orçado    | 10    | 1
                   Teste1  | TOTAL | Real      | 12    | 1
                   Teste1  | TOTAL | Desvio %  | 14    | 1
                   Teste1  | TOTAL | Desvio $  | 16    | 1

                    */
            //"; 
            #endregion
            foreach (var i in queryRowsBody)
            {

                var filtro = result1.Where(r => r.CLASSIFIC_NEGOCIO.Equals(i)).ToList();
                var Tr = new Trs()
                {
                    name = i,
                    tdsEsquerda = new List<Tds>(),
                    tdsDireita = new List<Tds>()
                };

                #region Result1 

                /*Caso não exista MACROPROCESSO*/
                foreach (var x in tabela.trsCabecalho2)
                    if (!filtro.Any(r => r.MACROPROCESSO.Equals(x.name)))
                        filtro.Add(new ResultQuery1() { MACROPROCESSO = x.name, CLASSIFIC_NEGOCIO = filtro.FirstOrDefault().CLASSIFIC_NEGOCIO });
                filtro = filtro.OrderBy(r => r.MACROPROCESSO).ToList();
                foreach (var ii in filtro)
                {
                    Tr.tdsEsquerda.Add(new Tds() { valor = Decimal.Round(ii.REAL, 2).ToString("G29") });
                    Tr.tdsEsquerda.Add(new Tds() { valor = Decimal.Round(ii.ORCADO, 2).ToString("G29") });
                    Tr.tdsEsquerda.Add(new Tds() { valor = Decimal.Round(ii.DESVIO, 2).ToString("G29") });
                    Tr.tdsEsquerda.Add(new Tds() { valor = Decimal.Round(ii.DESVIOPERCENTUAL, 2).ToString("G29") });
                }

                #endregion

                #region Result2

                filtro = result2.Where(r => r.CLASSIFIC_NEGOCIO.Equals(i)).ToList();
                foreach (var ii in filtro)
                {
                    Tr.tdsDireita.Add(new Tds() { valor = Decimal.Round(ii.REAL, 2).ToString("G29") });
                    Tr.tdsDireita.Add(new Tds() { valor = Decimal.Round(ii.ORCADO, 2).ToString("G29") });
                    Tr.tdsDireita.Add(new Tds() { valor = Decimal.Round(ii.DESVIO, 2).ToString("G29") });
                    Tr.tdsDireita.Add(new Tds() { valor = Decimal.Round(ii.DESVIOPERCENTUAL, 2).ToString("G29") });
                }

                #endregion

                tabela.trsMeio.Add(Tr);
            }

            #endregion

            #region Rodapé

            var queryRowsFooter = new List<string>();// TOTAL por ex.
            queryRowsFooter.Add("Total");
            tabela.footer = new List<Trs>();
            foreach (var i in queryRowsFooter)
            {
                //var filtro = result3.Where(r => r.CLASSIFIC_NEGOCIO.Equals(i)).ToList();
                var Tr = new Trs()
                {
                    name = i,
                    tdsEsquerda = new List<Tds>(),
                    tdsDireita = new List<Tds>()
                };

                #region Result3

                foreach (var ii in result3)
                {
                    Tr.tdsEsquerda.Add(new Tds() { valor = Decimal.Round(ii.REAL, 2).ToString("G29") });
                    Tr.tdsEsquerda.Add(new Tds() { valor = Decimal.Round(ii.ORCADO, 2).ToString("G29") });
                    Tr.tdsEsquerda.Add(new Tds() { valor = Decimal.Round(ii.DESVIO, 2).ToString("G29") });
                    Tr.tdsEsquerda.Add(new Tds() { valor = Decimal.Round(ii.DESVIOPERCENTUAL, 2).ToString("G29") });
                }

                #endregion

                #region Result4

                foreach (var ii in result4)
                {
                    Tr.tdsDireita.Add(new Tds() { valor = Decimal.Round(ii.REAL, 2).ToString("G29") });
                    Tr.tdsDireita.Add(new Tds() { valor = Decimal.Round(ii.ORCADO, 2).ToString("G29") });
                    Tr.tdsDireita.Add(new Tds() { valor = Decimal.Round(ii.DESVIO, 2).ToString("G29") });
                    Tr.tdsDireita.Add(new Tds() { valor = Decimal.Round(ii.DESVIOPERCENTUAL, 2).ToString("G29") });
                }

                #endregion

                tabela.footer.Add(Tr);
            }

            #endregion

            return tabela;
        }

        public class ResultQuery1
        {
            public string CLASSIFIC_NEGOCIO { get; set; }
            public string MACROPROCESSO { get; set; }
            public int ORCADO { get; set; }
            public decimal DESVIO { get; set; }
            public decimal DESVIOPERCENTUAL { get; set; }
            public decimal REAL { get; set; }
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
        public static TabelaDinamicaResultados MockTabelaVisaoGeralDaArea()
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
                tdsEsquerda1.Add(new Tds() { valor = (1M * i).ToString(), coolspan = 1 });

                tdsEsquerda2.Add(new Tds() { valor = (6M * i).ToString(), coolspan = 1 });

                tdsEsquerda3.Add(new Tds() { valor = (9M * i).ToString(), coolspan = 1 });
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

        #endregion

    }

}