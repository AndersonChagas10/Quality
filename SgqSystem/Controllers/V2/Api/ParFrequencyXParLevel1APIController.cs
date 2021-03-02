using Dominio;
using Dominio.AppViewModel;
using SgqSystem.Controllers.Api;
using SgqSystem.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SgqSystem.Controllers.V2.Api
{
    [RoutePrefix("api")]
    public class ParFrequencyXParLevel1APIController : BaseApiController
    {

        [HttpPost]
        [Route("GetParFrequencyXParLevel1")]
        public IHttpActionResult GetParFrequencyXParLevel1(PlanejamentoColetaViewModel appParametrization)
        {

            List<ParFrequencyAppViewModel> frequencies = new List<ParFrequencyAppViewModel>();

            List<ParLevel1AppViewModel> levels1 = new List<ParLevel1AppViewModel>();

            List<ParVinculoPesoAppViewModel> vinculosPeso = new List<ParVinculoPesoAppViewModel>();

            ParFrequencyAppViewModel frequenciaTemp = new ParFrequencyAppViewModel();

            List<ParFrequencyAppViewModel> frequenciasViewModel = new List<ParFrequencyAppViewModel>();

            using (Dominio.SgqDbDevEntities db = new Dominio.SgqDbDevEntities())
            {
                vinculosPeso = db.ParVinculoPeso
                        .Where(x => x.ParCluster_Id == appParametrization.ParCluster_Id
                        && (x.ParCompany_Id == appParametrization.ParCompany_Id || x.ParCompany_Id == null)
                        && x.IsActive)
                        .Select(x => new ParVinculoPesoAppViewModel()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            ParCompany_Id = x.ParCompany_Id,
                            ParCluster_Id = x.ParCluster_Id,
                            ParFrequency_Id = x.ParFrequencyId,
                            ParLevel1_Id = x.ParLevel1_Id
                        })
                        .ToList();

                levels1 = db.ParLevel1
                    .ToList()
                    .Where(x => vinculosPeso.Any(y => y.ParLevel1_Id == x.Id))
                    .Select(x => new ParLevel1AppViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name
                    })
                    .ToList();

                frequencies = db.ParFrequency
                    .ToList()
                    .Where(x => vinculosPeso.Any(y => y.ParFrequency_Id == x.Id))
                    .Select(x => new ParFrequencyAppViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                    })
                    .ToList();

                foreach (var vinculo in vinculosPeso)
                {
                    var frequenciaVinculada = frequencies.Where(x => x.Id == vinculo.ParFrequency_Id).FirstOrDefault();
                    var indicadorVinculado = levels1.Where(x => x.Id == vinculo.ParLevel1_Id)
                        .Select(x => new ParLevel1AppViewModel()
                        {
                            Id = x.Id,
                            Name = x.Name
                        })
                        .FirstOrDefault();
                    //Valido a frequencia na view model
                    frequenciaTemp = frequenciasViewModel.Where(x => x.Id == frequenciaVinculada.Id).FirstOrDefault();

                    if (frequenciaTemp == null)
                    { 
                        //Cria se não existir e vincula na lista
                        frequenciaTemp = new ParFrequencyAppViewModel() { Id = frequenciaVinculada.Id, Name = frequenciaVinculada.Name };
                        frequenciasViewModel.Add(frequenciaTemp);

                       // frequencyXindicador.frequency.Add(frequenciaTemp);
                    }
                    //Valido o indicador dentro da frequencia
                    if (frequenciaTemp.ParLevel1 != null &&  !frequenciaTemp.ParLevel1.Any(x => x.Id == indicadorVinculado.Id))
                    {
                        // Se não tiver algum indicador na lista do view model
                        frequenciaTemp.ParLevel1.Add(indicadorVinculado);
                    }
                }

            }

            return Ok(frequenciasViewModel);
        }
    }
}
