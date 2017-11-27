﻿using AutoMapper;
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
        [Route("GetPlanejamentoAcaoRange")]
        public IEnumerable<Pa_Planejamento> GetPlanejamentoAcaoRange(string startDate, string endDate)
        {
            return Pa_Planejamento.GetPlanejamentoAcao(startDate, endDate);
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


        public Pa_Planejamento CreateGenericEstrategicoTatico()
        {
            //db.Database.ExecuteSqlCommand("Insert into pa_planejamento (IsFta, IsTatico) values (1, 0)");
            //db.Database.ExecuteSqlCommand("Insert into pa_planejamento (IsFta, IsTatico, Estrategico_Id) values (1, 1, (select Top 1 Id from pa_planejamento))");

            var estrategico = new PlanoAcaoEF.Pa_Planejamento() { IsTatico = false };
            estrategico.Tatico_Id = null;
            estrategico.Gerencia_Id = 0;
            estrategico.Coordenacao_Id = 0;
            estrategico.Iniciativa_Id = 0;
            estrategico.ObjetivoGerencial_Id = 0;
            estrategico.Responsavel_Projeto = 0;
            estrategico.UnidadeDeMedida_Id = 0;
            estrategico.IndicadoresDeProjeto_Id = 0;
            SavePlanejamentoInDb(estrategico);

            var tatico = new PlanoAcaoEF.Pa_Planejamento() { IsTatico = true, Estrategico_Id = estrategico.Id };
            SavePlanejamentoInDb(tatico);
            tatico.Tatico_Id = tatico.Id;
            SavePlanejamentoInDb(tatico);

            //var cuelho = db.Pa_Planejamento.OrderByDescending(r => r.Id).FirstOrDefault();
            //var cuelho = db.Pa_Planejamento.FirstOrDefault(r => r.Estrategico_Id != null);

            return Mapper.Map<Pa_Planejamento>(tatico);
        }


        public Pa_Planejamento CreateGenericEstrategicoTaticoFta()
        {
            //db.Database.ExecuteSqlCommand("Insert into pa_planejamento (IsFta, IsTatico) values (1, 0)");
            //db.Database.ExecuteSqlCommand("Insert into pa_planejamento (IsFta, IsTatico, Estrategico_Id) values (1, 1, (select Top 1 Id from pa_planejamento))");

            var estrategico = new PlanoAcaoEF.Pa_Planejamento() { IsTatico = false };
            estrategico.Tatico_Id = null;
            estrategico.Gerencia_Id = 0;
            estrategico.Coordenacao_Id = 0;
            estrategico.Iniciativa_Id = 0;
            estrategico.ObjetivoGerencial_Id = 0;
            estrategico.Responsavel_Projeto = 0;
            estrategico.UnidadeDeMedida_Id = 0;
            estrategico.IndicadoresDeProjeto_Id = 0;
            SavePlanejamentoInDb(estrategico);

            var tatico = new PlanoAcaoEF.Pa_Planejamento() { IsTatico = true, Estrategico_Id = estrategico.Id };
            SavePlanejamentoInDb(tatico);
            tatico.Tatico_Id = tatico.Id;
            tatico.IsFta = true;
            SavePlanejamentoInDb(tatico);

            //var cuelho = db.Pa_Planejamento.OrderByDescending(r => r.Id).FirstOrDefault();
            //var cuelho = db.Pa_Planejamento.FirstOrDefault(r => r.Estrategico_Id != null);

            return Mapper.Map<Pa_Planejamento>(tatico);
        }


        private void SavePlanejamentoInDb(PlanoAcaoEF.Pa_Planejamento a)
        {
            if (a.Id > 0)
            {
                db.Pa_Planejamento.Attach(a);
                var entry = db.Entry(a);
                entry.State = System.Data.Entity.EntityState.Modified;
                //entry.Property(e => e.Email).IsModified = true;
                // other changed properties
            }
            else
            {
                db.Pa_Planejamento.Add(a);
            }

            db.SaveChanges();
        }

    }
}
