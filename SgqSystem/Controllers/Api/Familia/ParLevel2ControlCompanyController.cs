﻿using Dominio;
using DTO.Interfaces.Services;
using DTO.DTO.Params;
using DTO.Helpers;
using SgqSystem.Handlres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{
    [HandleApi()]
    [RoutePrefix("api/ParLevel2ControlCompany")]
    public class ParLevel2ControlCompanyController : ApiController
    {

        private IBaseDomain<ParLevel2ControlCompany, ParLevel2ControlCompanyDTO> _baseParLevel2ControlCompany;
        private IBaseDomain<ParLevel1, ParLevel1DTO> _baseParLevel1;
        private IBaseDomain<ParLevel3Level2Level1, ParLevel3Level2Level1DTO> _baseParLevel3Level2Level1;
        private IBaseDomain<ParLevel3Level2, ParLevel3Level2DTO> _baseParLevel3Level2;
        public List<ParLevel2ControlCompanyDTO> _list;

        public ParLevel2ControlCompanyController(IBaseDomain<ParLevel2ControlCompany, ParLevel2ControlCompanyDTO> baseParLevel2ControlCompany,
            IBaseDomain<ParLevel1, ParLevel1DTO> baseParLevel1,
            IBaseDomain<ParLevel3Level2Level1, ParLevel3Level2Level1DTO> baseParLevel3Level2Level1,
            IBaseDomain<ParLevel3Level2, ParLevel3Level2DTO> baseParLevel3Level2
            )
        {
            _baseParLevel2ControlCompany = baseParLevel2ControlCompany;
            _baseParLevel1 = baseParLevel1;
            _baseParLevel3Level2Level1 = baseParLevel3Level2Level1;
            _baseParLevel3Level2 = baseParLevel3Level2;
            _list = new List<ParLevel2ControlCompanyDTO>();
        }

        [HttpPost]
        [Route("Save")]
        public List<ParLevel2ControlCompanyDTO> Save([FromBody]  ParLevel1DTO parLevel1)
        {
            var initDate = Guard.ParseDateToSqlV2(parLevel1.DataInit);

            if (parLevel1.CompanyControl_Id == null || parLevel1.CompanyControl_Id <= 0)
            {
                //desativa os registros já cadastrados do corporativo
                var listaCadastrada = _baseParLevel2ControlCompany.GetAll().Where(r => r.IsActive == true && r.ParCompany_Id == null && r.ParLevel1_Id == parLevel1.Id && r.InitDate == initDate);

                if (listaCadastrada.Count() > 0)
                    foreach (var cadastro in listaCadastrada)
                    {
                        _baseParLevel2ControlCompany.ExecuteSql("Update ParLevel2ControlCompany SET IsActive = 0, AlterDate = getdate() Where Id = " + cadastro.Id + " And Cast(InitDate As Date) = '" + initDate.ToString("yyyy-MM-dd") + "'");
                    }

                if (parLevel1.listLevel2Corporativos != null)
                    foreach (var level2Id in parLevel1.listLevel2Corporativos)
                        _list.Add(_baseParLevel2ControlCompany.AddOrUpdate(new ParLevel2ControlCompanyDTO(level2Id, parLevel1.Id, parLevel1.CompanyControl_Id, initDate)));

                _baseParLevel1.ExecuteSql("Update ParLevel1 SET level2Number = " + parLevel1.level2Number + " Where Id = " + parLevel1.Id);

                if (parLevel1.listLevel2Corporativos == null)
                {
                    var parLevel3Level2Vinculado = _baseParLevel3Level2Level1.GetAll().FirstOrDefault(r => r.ParLevel1_Id == parLevel1.Id && r.Active == true);

                    if (parLevel3Level2Vinculado != null)
                    {
                        var parVinculo = _baseParLevel3Level2.GetAll().FirstOrDefault(r => r.Id == parLevel3Level2Vinculado.ParLevel3Level2_Id && r.IsActive == true);

                        _list.Add(_baseParLevel2ControlCompany.AddOrUpdate(new ParLevel2ControlCompanyDTO(parVinculo.ParLevel2_Id, parLevel1.Id, parLevel1.CompanyControl_Id, initDate, false)));
                    }

                }

            }
            else
            {
                //desativa os registros já cadastrados da unidade
                var listaCadastrada = _baseParLevel2ControlCompany.GetAll().Where(r => r.IsActive == true && r.ParCompany_Id == parLevel1.CompanyControl_Id && r.ParLevel1_Id == parLevel1.Id && r.InitDate == initDate);

                if (listaCadastrada.Count() > 0)
                    foreach (var cadastro in listaCadastrada)
                    {
                        _baseParLevel2ControlCompany.ExecuteSql("Update ParLevel2ControlCompany SET IsActive = 0, AlterDate = getdate() Where Id = " + cadastro.Id + " And Cast(InitDate As Date) = '" + initDate.ToString("yyyy-MM-dd") + "'");
                    }

                if (parLevel1.level2PorCompany != null)
                    foreach (var level2Id in parLevel1.level2PorCompany)
                        _list.Add(_baseParLevel2ControlCompany.AddOrUpdate(new ParLevel2ControlCompanyDTO(level2Id, parLevel1.Id, parLevel1.CompanyControl_Id, initDate)));

                if (parLevel1.listLevel2Corporativos == null)
                {
                    var parLevel3Level2Vinculado = _baseParLevel3Level2Level1.GetAll().FirstOrDefault(r => r.ParLevel1_Id == parLevel1.Id && r.Active == true);

                    if (parLevel3Level2Vinculado != null)
                    {
                        var parVinculo = _baseParLevel3Level2.GetAll().FirstOrDefault(r => r.Id == parLevel3Level2Vinculado.ParLevel3Level2_Id && r.IsActive == true);

                        _list.Add(_baseParLevel2ControlCompany.AddOrUpdate(new ParLevel2ControlCompanyDTO(parVinculo.ParLevel2_Id, parLevel1.Id, parLevel1.CompanyControl_Id, initDate, false)));
                    }

                }
            }
            return _list;
        }
    }
}
