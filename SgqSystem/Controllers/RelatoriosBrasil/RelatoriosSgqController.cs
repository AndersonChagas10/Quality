using Dominio.Interfaces.Services;
using SgqSystem.Secirity;
using SgqSystem.ViewModels;
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
            return PartialView(form);
        }

        public ActionResult VisaoGeralDaAreaTbl2()
        {
            return PartialView(form);
        }
    }
}