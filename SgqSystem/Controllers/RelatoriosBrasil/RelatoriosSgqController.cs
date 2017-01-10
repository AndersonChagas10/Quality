using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO;
using DTO.DTO.Params;
using SgqSystem.Secirity;
using SgqSystem.ViewModels;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    [CustomAuthorize]
    public class RelatoriosSgqController : BaseController
    {

        #region Constructor

        private FormularioParaRelatorioViewModel form;
        private readonly IRelatorioColetaDomain _relatorioColetaDomain;
        private readonly IUserDomain _userDomain;
        private readonly IBaseDomain<UserSgq, UserDTO> _user;
        private readonly IBaseDomain<Level01, Level01DTO> _level01;
        private readonly IBaseDomain<Level02, Level02DTO> _level02;
        private readonly IBaseDomain<Level03, Level03DTO> _level03;
        private readonly IBaseDomain<Shift, ShiftDTO> _shift;
        private readonly IBaseDomain<Period, PeriodDTO> _period;
        private readonly IBaseDomain<ParCompany, ParCompanyDTO> _unit;
        private readonly IBaseDomain<ParCompanyXUserSgq, ParCompanyXUserSgqDTO> _companyXUserSgq;

        public RelatoriosSgqController(IRelatorioColetaDomain relatorioColetaDomain
            , IUserDomain userDomain
            , IBaseDomain<UserSgq, UserDTO> user
            , IBaseDomain<Level01, Level01DTO> level01
            , IBaseDomain<Level02, Level02DTO> level02
            , IBaseDomain<Level03, Level03DTO> level03
            , IBaseDomain<Shift, ShiftDTO> shift
            , IBaseDomain<Period, PeriodDTO> period
            , IBaseDomain<ParCompany, ParCompanyDTO> unit
            , IBaseDomain<ParCompanyXUserSgq, ParCompanyXUserSgqDTO> companyXUserSgq
            )
        {

            _companyXUserSgq = companyXUserSgq;
            _unit = unit;
            _userDomain = userDomain;
            _level01 = level01;
            _level02 = level02;
            _level03 = level03;
            _shift = shift;
            _period = period;
            _user = user;
            _relatorioColetaDomain = relatorioColetaDomain;

            form = new FormularioParaRelatorioViewModel();
            form.SetLevel01SelectList(_level01.GetAllNoLazyLoad());
            form.Setlevel02SelectList(_level02.GetAllNoLazyLoad());
            form.SetLevel03SelectList(_level03.GetAllNoLazyLoad());
            form.SetUserSelectList(_user.GetAllNoLazyLoad());
            form.SetShiftSelectList(/*_shift.GetAll()*/);
            form.SetPeriodSelectList(_period.GetAllNoLazyLoad());
            form.SetUnitsSelectList(_unit.GetAllNoLazyLoad());
            form.SetUserSelectList(_user.GetAllNoLazyLoad());

        }

        #endregion
        
        [OutputCache(Duration = 20, VaryByParam = "none")]
        public ActionResult Scorecard()
        {
            FiltraUnidades();
            return View(form);
        }

        [OutputCache(Duration = 20, VaryByParam = "none")]
        public ActionResult RelatorioDiario()
        {
            FiltraUnidades();
            return View(form);
        }

        [OutputCache(Duration = 20, VaryByParam = "none")]
        public ActionResult ApontamentosDiarios()
        {
            FiltraUnidades();
            return View(form);
        }

        [OutputCache(Duration = 20, VaryByParam = "none")]
        public ActionResult NaoConformidade()
        {
            FiltraUnidades();
            return View(form);
        }

        [OutputCache(Duration = 20, VaryByParam = "none")]
        public ActionResult ExemploRelatorio()
        {
            FiltraUnidades();
            return View(form);
        }

        #region Auxiliares

        private void FiltraUnidades()
        {
            HttpCookie cookie = HttpContext.Request.Cookies.Get("webControlCookie");
            if (cookie != null)
            {
                var rolesCompany = "";
                var userId = 0;
                if (!string.IsNullOrEmpty(cookie.Values["userId"]))
                    int.TryParse(cookie.Values["userId"].ToString(), out userId);

                if (userId > 0)
                    if (!string.IsNullOrEmpty(cookie.Values["rolesCompany"]))
                    {
                        rolesCompany = cookie.Values["rolesCompany"].ToString();
                        var companyesDoUser = _companyXUserSgq.GetAll().Where(r => r.UserSgq_Id == userId).Select(r => r.ParCompany).ToList().OrderBy(r => r.Name).GroupBy(r => r.Id).Select(group => group.First());
                        form.SetUnitsSelectList(companyesDoUser);
                        ViewBag.UnidadeUsuario = companyesDoUser;
                    }
            }
        } 

        #endregion

    }
}