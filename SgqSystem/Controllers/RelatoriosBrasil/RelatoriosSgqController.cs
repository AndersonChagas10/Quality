using Dominio.Interfaces.Services;
using SgqSystem.Secirity;
using SgqSystem.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    [CustomAuthorize]
    [FormularioPesquisa(filtraUnidadePorUsuario = true)]
    [OutputCache(Duration = 20, VaryByParam = "none")]
    public class RelatoriosSgqController : BaseController
    {

        #region Constructor

        private FormularioParaRelatorioViewModel form;
        //private readonly IRelatorioColetaDomain _relatorioColetaDomain;
        //private readonly IUserDomain _userDomain;
        //private readonly IBaseDomain<UserSgq, UserDTO> _user;
        //private readonly IBaseDomain<Level01, Level01DTO> _level01;
        //private readonly IBaseDomain<Level02, Level02DTO> _level02;
        //private readonly IBaseDomain<Level03, Level03DTO> _level03;
        //private readonly IBaseDomain<Shift, ShiftDTO> _shift;
        //private readonly IBaseDomain<Period, PeriodDTO> _period;
        //private readonly IBaseDomain<ParCompany, ParCompanyDTO> _unit;
        //private readonly IBaseDomain<ParCompanyXUserSgq, ParCompanyXUserSgqDTO> _companyXUserSgq;

        public RelatoriosSgqController(IRelatorioColetaDomain relatorioColetaDomain
            //, IUserDomain userDomain
            //, IBaseDomain<UserSgq, UserDTO> user
            //, IBaseDomain<Level01, Level01DTO> level01
            //, IBaseDomain<Level02, Level02DTO> level02
            //, IBaseDomain<Level03, Level03DTO> level03
            //, IBaseDomain<Shift, ShiftDTO> shift
            //, IBaseDomain<Period, PeriodDTO> period
            //, IBaseDomain<ParCompany, ParCompanyDTO> unit
            //, IBaseDomain<ParCompanyXUserSgq, ParCompanyXUserSgqDTO> companyXUserSgq
            )
        {

            //_companyXUserSgq = companyXUserSgq;
            //_unit = unit;
            //_userDomain = userDomain;
            //_level01 = level01;
            //_level02 = level02;
            //_level03 = level03;
            //_shift = shift;
            //_period = period;
            //_user = user;
            //_relatorioColetaDomain = relatorioColetaDomain;

            form = new FormularioParaRelatorioViewModel();
            //form.SetLevel01SelectList(_level01.GetAllNoLazyLoad());
            //form.Setlevel02SelectList(_level02.GetAllNoLazyLoad());
            //form.SetLevel03SelectList(_level03.GetAllNoLazyLoad());
            //form.SetUserSelectList(_user.GetAllNoLazyLoad());
            //form.SetShiftSelectList(/*_shift.GetAll()*/);
            //form.SetPeriodSelectList(_period.GetAllNoLazyLoad());
            //form.SetUnitsSelectList(_unit.GetAllNoLazyLoad());
            //form.SetUserSelectList(_user.GetAllNoLazyLoad());

        }

        #endregion

        public ActionResult Scorecard()
        {
            return View(form);
        }

        //[FormularioPesquisa(filtraUnidadePorUsuario = false)]
        public ActionResult RelatorioDiario()
        {
            return View(form);
        }

        public ActionResult ApontamentosDiarios()
        {
            return View(form);
        }

        public ActionResult NaoConformidade()
        {
            return View(form);
        }

        public ActionResult ExemploRelatorio()
        {
            return View(form);
        }

        public ActionResult VisaoGeralDaArea()
        {
            return View(form);
        }

        public ActionResult VisaoGeralDaAreaTbl1()
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
            for (int i = 0; i < 4*3; i++)
            {
                tdsDoFooter2.Add(new Tds() {
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

            return View(tabela);
        }

        public ActionResult VisaoGeralDaAreaTbl2()
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

            tabela.trsMeio.Add(new Trs()
            {
                name = "teste3",
                tdsEsquerda = tdsEsquerda3,
                tdsDireita = tdsDireita3

            });

            tabela.trsMeio.Add(new Trs()
            {
                name = "teste3",
                tdsEsquerda = tdsEsquerda3,
                tdsDireita = tdsDireita3

            });

            tabela.trsMeio.Add(new Trs()
            {
                name = "teste3",
                tdsEsquerda = tdsEsquerda3,
                tdsDireita = tdsDireita3

            });

            tabela.trsMeio.Add(new Trs()
            {
                name = "teste3",
                tdsEsquerda = tdsEsquerda3,
                tdsDireita = tdsDireita3

            });

            tabela.trsMeio.Add(new Trs()
            {
                name = "teste3",
                tdsEsquerda = tdsEsquerda3,
                tdsDireita = tdsDireita3

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

            return View(tabela);
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