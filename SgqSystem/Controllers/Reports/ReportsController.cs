﻿using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO;
using DTO.DTO.Params;
using SgqSystem.Secirity;
using SgqSystem.ViewModels;
using System.Web.Mvc;
namespace SgqSystem.Controllers
{
    //techservices
    [CustomAuthorize()]
    public class ReportsController : BaseController
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

        public ReportsController(IRelatorioColetaDomain relatorioColetaDomain
            , IUserDomain userDomain
            , IBaseDomain<UserSgq, UserDTO> user
            , IBaseDomain<Level01, Level01DTO> level01
            , IBaseDomain<Level02, Level02DTO> level02
            , IBaseDomain<Level03, Level03DTO> level03
            , IBaseDomain<Shift, ShiftDTO> shift
            , IBaseDomain<Period, PeriodDTO> period
            , IBaseDomain<ParCompany, ParCompanyDTO> unit
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
            form.SetUserSelectList(_user.GetAllNoLazyLoad());
            //form.SetUserSelectList(_userDomain.GetAllUserValidationAd(new UserDTO()).Retorno);
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