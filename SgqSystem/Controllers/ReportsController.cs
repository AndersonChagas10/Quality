using Application.Interface;
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
        private readonly IRelatorioColetaApp _relatorioColetaApp;
        private readonly IUserApp _userApp;
        private readonly IBaseApp<Dominio.UserSgq, UserDTO> _user;
        private readonly IBaseApp<Dominio.Level01, Level01DTO> _level01;
        private readonly IBaseApp<Dominio.Level02, Level02DTO> _level02;
        private readonly IBaseApp<Dominio.Level03, Level03DTO> _level03;
        private readonly IBaseApp<Dominio.Shift, ShiftDTO> _shift;
        private readonly IBaseApp<Dominio.Period, PeriodDTO> _period;
        private readonly IBaseApp<Dominio.Unit, UnitDTO> _unit;

        public ReportsController(IRelatorioColetaApp relatorioColetaApp
            , IUserApp userApp
            , IBaseApp<Dominio.UserSgq, UserDTO> user
            , IBaseApp<Dominio.Level01, Level01DTO> level01
            , IBaseApp<Dominio.Level02, Level02DTO> level02
            , IBaseApp<Dominio.Level03, Level03DTO> level03
            , IBaseApp<Dominio.Shift, ShiftDTO> shift
            , IBaseApp<Dominio.Period, PeriodDTO> period
            , IBaseApp<Dominio.Unit, UnitDTO> unit
            )
        {
            _unit = unit;
            _userApp = userApp;
            _level01 = level01;
            _level02 = level02;
            _level03 = level03;
            _shift = shift;
            _period = period;
            _user = user;
            _relatorioColetaApp = relatorioColetaApp;

            form = new FormularioParaRelatorioViewModel();
            form.SetLevel01SelectList(_level01.GetAll());
            form.Setlevel02SelectList(_level02.GetAll());
            form.SetLevel03SelectList(_level03.GetAll());
            form.SetUserSelectList(_user.GetAll());
            form.SetShiftSelectList(/*_shift.GetAll()*/);
            form.SetPeriodSelectList(_period.GetAll());
            form.SetUnitsSelectList(_unit.GetAll());
            form.SetUserSelectList(_userApp.GetAllUserValidationAd(new UserDTO()).Retorno);
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