using Dominio.Interfaces.Services;
using DTO.DTO;
using SgqSystem.Secirity;
using SgqSystem.ViewModels;
using System.Web.Mvc;
namespace SgqSystem.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class ReportsController : Controller
    {

        #region Constructor

        private FormularioParaRelatorioViewModel form;
        private readonly IRelatorioColetaDomain _relatorioColetaDomain;
        private readonly IUserDomain _userDomain;
        private readonly IBaseDomain<Dominio.UserSgq, UserDTO> _user;
        private readonly IBaseDomain<Dominio.Level01, Level01DTO> _level01;
        private readonly IBaseDomain<Dominio.Level02, Level02DTO> _level02;
        private readonly IBaseDomain<Dominio.Level03, Level03DTO> _level03;
        private readonly IBaseDomain<Dominio.Shift, ShiftDTO> _shift;
        private readonly IBaseDomain<Dominio.Period, PeriodDTO> _period;
        private readonly IBaseDomain<Dominio.Unit, UnitDTO> _unit;

        public ReportsController(IRelatorioColetaDomain relatorioColetaDomain
            , IUserDomain userDomain
            , IBaseDomain<Dominio.UserSgq, UserDTO> user
            , IBaseDomain<Dominio.Level01, Level01DTO> level01
            , IBaseDomain<Dominio.Level02, Level02DTO> level02
            , IBaseDomain<Dominio.Level03, Level03DTO> level03
            , IBaseDomain<Dominio.Shift, ShiftDTO> shift
            , IBaseDomain<Dominio.Period, PeriodDTO> period
            , IBaseDomain<Dominio.Unit, UnitDTO> unit
            )
        {
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
            form.SetLevel01SelectList(_level01.GetAll());
            form.Setlevel02SelectList(_level02.GetAll());
            form.SetLevel03SelectList(_level03.GetAll());
            form.SetUserSelectList(_user.GetAll());
            form.SetShiftSelectList(/*_shift.GetAll()*/);
            form.SetPeriodSelectList(_period.GetAll());
            form.SetUnitsSelectList(_unit.GetAll());
            form.SetUserSelectList(_userDomain.GetAllUserValidationAd(new UserDTO()).Retorno);
        }

        #endregion

        #region DataCollectionReport

        public ActionResult DataCollectionReport()
        {
            return View(form);
        }

        #endregion

        #region CorrectiveActionReport

        public ActionResult CorrectiveActionReport()
        {
            return View(form);
        }

        #endregion

    }
}