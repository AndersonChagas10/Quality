using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO.Params;
using Helper;
using SgqSystem.Secirity;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    public class ManDataCollectITController : Controller
    {

        private IBaseDomain<ParFrequency, ParFrequencyDTO> _baseDomainParFrequency;
        private IBaseDomain<ParMeasurementUnit, ParMeasurementUnitDTO> _baseDomainParMeasurementUnit;
        private IBaseDomain<ParCompany, ParCompanyDTO> _baseDomainParCompany;

        public ManDataCollectITController(IBaseDomain<ParFrequency, ParFrequencyDTO> baseDomainParFrequency,
                    IBaseDomain<ParMeasurementUnit, ParMeasurementUnitDTO> baseDomainParMeasurementUnit,
                    IBaseDomain<ParCompany, ParCompanyDTO> baseDomainParCompany
                    )
        {

            _baseDomainParFrequency = baseDomainParFrequency;
            _baseDomainParMeasurementUnit = baseDomainParMeasurementUnit;
            _baseDomainParCompany = baseDomainParCompany;

            ViewBag.listaParFrequency = _baseDomainParFrequency.GetAll();
            ViewBag.listaParMeasurementUnit = _baseDomainParMeasurementUnit.GetAll();
            ViewBag.listaParCompany = baseDomainParCompany.GetAll();

        }

        public ActionResult Index()
        {
            return View();
        }
    }
}