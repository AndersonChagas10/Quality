using AutoMapper;
using DTO.Helpers;
using Helper;
using PlanoAcaoCore;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace PlanoDeAcaoMVC.Controllers.Api
{
    [RoutePrefix("api/Pa_Planejamento")]
    public class ApiPa_PlanejamentoController : ApiController
    {
        PlanoAcaoEF.PlanoDeAcaoEntities db;

        public ApiPa_PlanejamentoController()
        {
            db = new PlanoAcaoEF.PlanoDeAcaoEntities();
        }

        [HttpGet]
        [Route("List")]
        public IEnumerable<Pa_Planejamento> List()
        {
            return Pa_Planejamento.Listar();
        }

        [HttpGet]
        [Route("GET")]
        public Pa_Planejamento Get(int id)
        {
            var planejamento = Pa_Planejamento.Get(id);
            if (planejamento.Estrategico_Id.GetValueOrDefault() > 0)
            {
                //planejamento.Tatico_Id = Pa_BaseObject.ListarGenerico<Pa_Planejamento>("Select * from Pa_Planejamento where Estrategico_Id = " + planejamento.Estrategico_Id.GetValueOrDefault()).FirstOrDefault().Id;
            }
            return planejamento;
        }

        [HttpGet]
        [HttpPost]
        [Route("GetPlanejamentoAcao")]
        public IEnumerable<Pa_Planejamento> GetPlanejamentoAcao()
        {
            var retorno = Pa_Planejamento.GetPlanejamentoAcao();
            foreach (var i in retorno)
            {

                if (i.Estrategico_Id.GetValueOrDefault() > 0)
                {
                    //i.Tatico_Id = Pa_BaseObject.ListarGenerico<Pa_Planejamento>("Select * from Pa_Planejamento where Estrategico_Id = " + i.Tatico_Id.GetValueOrDefault()).FirstOrDefault().Id;
                }
            }
            return retorno;
        }

        [HttpPost]
        [Route("Save")]
        public Pa_Planejamento Save([FromBody]Pa_Planejamento planejamento)
        {
           
                planejamento.IsValid();

                planejamento.IsfiltrarAcao = null;

                if (planejamento.Estrategico_Id.GetValueOrDefault() > 0)
                {
                    if (!string.IsNullOrEmpty(planejamento._ValorDe))
                        planejamento.ValorDe = NumericExtensions.CustomParseDecimal(planejamento._ValorDe).GetValueOrDefault();
                    if (!string.IsNullOrEmpty(planejamento._ValorPara))
                        planejamento.ValorPara = NumericExtensions.CustomParseDecimal(planejamento._ValorPara).GetValueOrDefault();
                    planejamento.DataInicio = Guard.ParseDateToSqlV2(planejamento._DataInicio, Guard.CultureCurrent.BR);
                    planejamento.DataFim = Guard.ParseDateToSqlV2(planejamento._DataFim, Guard.CultureCurrent.BR);
                }

                if (!planejamento.IsTatico)
                {
                    planejamento.Tatico_Id = null;
                    planejamento.Gerencia_Id = 0;
                    planejamento.Coordenacao_Id = 0;
                    planejamento.Iniciativa_Id = 0;
                    planejamento.ObjetivoGerencial_Id = 0;
                    planejamento.Responsavel_Projeto = 0;
                    planejamento.UnidadeDeMedida_Id = 0;
                    planejamento.IndicadoresDeProjeto_Id = 0;
                }
                else if (planejamento.IsTatico && planejamento.Tatico_Id.GetValueOrDefault() > 0)
                {
                    planejamento.Id = planejamento.Tatico_Id.GetValueOrDefault();
                }

                //Pa_BaseObject.SalvarGenerico(planejamento);
                var a = Mapper.Map<PlanoAcaoEF.Pa_Planejamento>(planejamento);

                if (a.Id > 0)
                {
                    db.Pa_Planejamento.Attach(a);
                    var entry = db.Entry(a);
                    entry.State = System.Data.Entity.EntityState.Modified;
                    //entry.Property(e => e.Email).IsModified = true;
                    // other changed properties
                    db.SaveChanges();
                }
                else
                {
                    db.Pa_Planejamento.Add(a);
                    db.SaveChanges();
                }

                #region GAMBIARRA FDP

                if (planejamento.IsTatico)
                {
                    a.Tatico_Id = a.Id;
                    //Pa_BaseObject.SalvarGenerico(planejamento);

                    db.Pa_Planejamento.Attach(a);
                    var entry = db.Entry(a);
                    entry.State = System.Data.Entity.EntityState.Modified;
                    //entry.Property(e => e.Email).IsModified = true;
                    // other changed properties
                    db.SaveChanges();

                }

            #endregion

            return planejamento;
        }

        //[HttpPost]
        //[Route("GetPlanejamentosFTA")]
        //public List<PlanoAcaoEF.Pa_Planejamento> GetPlanejamentosFTA()
        //{
        //    return db.Pa_Planejamento.Where(r => r.IsFta == true).ToList();
        //}


    }
}
