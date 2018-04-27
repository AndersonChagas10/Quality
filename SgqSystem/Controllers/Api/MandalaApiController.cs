using Dominio;
using DTO;
using Newtonsoft.Json.Linq;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SgqSystem.Controllers.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Mandala")]
    public class MandalaApiController : BaseApiController
    {

        [HttpPost]
        [Route("MostrarEmpresaMandala")]
        public List<DTO.Mandala> MostrarEmpresaMandala([FromBody] FormularioParaRelatorioViewModel form)
        {       
            var listaEmpresas = BuscarListaEmpresasDisponiveis(form);
            var listaUnidades = GlobalConfig.MandalaUnidade?.Where(x => listaEmpresas.Contains(x.ParCompany_id)).ToList();

            return listaUnidades;
        }


        [HttpPost]
        [Route("MostrarIndicadorMandala")]
        public List<DTO.Mandala> MostrarIndicadorMandala([FromBody] FormularioParaRelatorioViewModel form)
        {
            var empresaSelecionada = form.unitName;
            var listaIndicadores = GlobalConfig.MandalaIndicador?.Where(x => x.ParCompany_Name == empresaSelecionada).ToList();

            return listaIndicadores;
        }

        [HttpPost]
        [Route("MostrarMonitoramentoMandala")]
        public List<DTO.Mandala> MostrarMonitoramentoMandala([FromBody] FormularioParaRelatorioViewModel form)
        {
            var processoSelecionado = form.level1Name;
            var empresaSelecionada = form.unitName;

            var listaMonitoramentos = GlobalConfig.MandalaMonitoramento?.Where(x => x.ParCompany_Name == empresaSelecionada).Where(x => x.ParLevel1_name == processoSelecionado).ToList();

            return listaMonitoramentos;
        }

        private List<int> GetUserUnits(int User)
        {
            using (var db = new SgqDbDevEntities())
            {
                return db.ParCompanyXUserSgq.Where(r => r.UserSgq_Id == User).Select(r => r.ParCompany_Id).ToList();
            }
        }

        private List<int> BuscarListaEmpresasDisponiveis(FormularioParaRelatorioViewModel form)
        {
            var empresas = GetUserUnits(form.auditorId);

            var listaEmpresa = GetUserUnits(form.auditorId);

            return listaEmpresa;
        }

    }
}


