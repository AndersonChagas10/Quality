using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO.Params;
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
        public List<ParLevel2ControlCompanyDTO> _list;

        public ParLevel2ControlCompanyController(IBaseDomain<ParLevel2ControlCompany, ParLevel2ControlCompanyDTO> baseParLevel2ControlCompany,
            IBaseDomain<ParLevel1, ParLevel1DTO> baseParLevel1)
        {
            _baseParLevel2ControlCompany = baseParLevel2ControlCompany;
            _baseParLevel1 = baseParLevel1;
            _list = new List<ParLevel2ControlCompanyDTO>();
        }

        [HttpPost]
        [Route("Save")]
        public List<ParLevel2ControlCompanyDTO> Save([FromBody]  ParLevel1DTO parLevel1)
        {

            var initDate = DateTime.Now;
            if (parLevel1.CompanyControl_Id == null || parLevel1.CompanyControl_Id <= 0)
            {
                //desativa os registros já cadastrados do corporativo
                var listaCadastrada = _baseParLevel2ControlCompany.GetAll().Where(r => r.IsActive == true && r.ParCompany_Id == null);

                if(listaCadastrada.Count() > 0)
                    foreach(var cadastro in listaCadastrada)
                    {
                        _baseParLevel2ControlCompany.ExecuteSql("Update ParLevel2ControlCompany SET IsActive = 0, AlterDate = getdate() Where Id = " + cadastro.Id);
                    }

                if (parLevel1.listLevel2Corporativos != null)
                    foreach (var level2Id in parLevel1.listLevel2Corporativos)
                        _list.Add(_baseParLevel2ControlCompany.AddOrUpdate(new ParLevel2ControlCompanyDTO(level2Id, parLevel1.Id, parLevel1.CompanyControl_Id, initDate)));

                _baseParLevel1.ExecuteSql("Update ParLevel1 SET level2Number = " + parLevel1.level2Number + " Where Id = " + parLevel1.Id);
            }
            else
            {
                //desativa os registros já cadastrados da unidade
                var listaCadastrada = _baseParLevel2ControlCompany.GetAll().Where(r => r.IsActive == true && r.ParCompany_Id == parLevel1.CompanyControl_Id);

                if (listaCadastrada.Count() > 0)
                    foreach (var cadastro in listaCadastrada)
                    {
                        _baseParLevel2ControlCompany.ExecuteSql("Update ParLevel2ControlCompany SET IsActive = 0, AlterDate = getdate() Where Id = " + cadastro.Id);
                    }

                if (parLevel1.level2PorCompany != null)
                    foreach (var level2Id in parLevel1.level2PorCompany)
                        _list.Add(_baseParLevel2ControlCompany.AddOrUpdate(new ParLevel2ControlCompanyDTO(level2Id, parLevel1.Id, parLevel1.CompanyControl_Id, initDate)));
            }
            return _list;
        }



    }
}
