﻿using PlanoAcaoCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Mvc;

namespace PlanoDeAcaoMVC.Controllers
{
    //[CustomAuthorizeAttribute]
    [IntegraSgq]
    public class Pa_PlanejamentoController : Controller
    {
        public Pa_PlanejamentoController()
        {

            Jobs.UpdateStatus();

            //select* from Pa_Dimensao
            //select* from[Pa_Objetivo]
            //select* from Pa_IndicadoresDiretriz
            ViewBag.Dimensao = Pa_Dimensao.Listar();
            ViewBag.Objetivo = Pa_Objetivo.Listar();
            ViewBag.IndicadoresDiretriz = Pa_IndicadoresDiretriz.Listar();

            ViewBag.Diretoria = Pa_Diretoria.Listar();
            ViewBag.Gerencia = Pa_Gerencia.Listar();
            ViewBag.Coordenacao = Pa_Coordenacao.Listar();
            ViewBag.Missao = Pa_Missao.Listar();
            ViewBag.Visao = Pa_Visao.Listar();
            //ViewBag.Quem = Pa_Quem.Listar();
            ViewBag.TemaAssunto = Pa_TemaAssunto.Listar();
            ViewBag.IndicadoresDeProjeto = Pa_IndicadoresDeProjeto.Listar();
            ViewBag.Iniciativa = Pa_Iniciativas.Listar();
            ViewBag.ObjetivoGerencial = Pa_ObjetivoGeral.Listar();
            ViewBag.UnidadeMedida = Pa_UnidadeMedida.Listar();
            ViewBag.TemaProjeto = Pa_TemaProjeto.Listar();
            ViewBag.TipoProjeto = Pa_TipoProjeto.Listar();

            if (ViewBag.Quem == null)
                ViewBag.Quem = Pa_Quem.Listar();
        }

        // GET: Pa_Planejamento
        public ActionResult Index(int? id = 0)
        {
            ViewBag.urlSend = Url.Action("Save", "api/Pa_Planejamento");
            ViewBag.urlList = Url.Action("List", "api/Pa_Planejamento");
            ViewBag.Coordenacao = new List<Pa_Coordenacao>();
            var model = new Pa_Planejamento();

            if(id.GetValueOrDefault() > 0)
               model = Pa_Planejamento.Get(id.GetValueOrDefault());

            return PartialView("Index", model);
        }

        public ActionResult Details(int? id = 0)
        {
            if (id > 0)
            {
                var model = Pa_Planejamento.GetTatico(id.GetValueOrDefault());
                if (model == null)
                    model = Pa_Planejamento.GetEstrategico(id.GetValueOrDefault());
                if (model != null)
                    return PartialView("Details", model);
            }

            return null;
        }

        public ActionResult Buscar()
        {
            return PartialView("Buscar");
        }

        [HttpPost]
        public ActionResult Filtrar(Pa_Planejamento filtro)
        {
            var lista = Pa_Planejamento.Listar();
            

            if (filtro.Diretoria_Id > 0)
                lista = lista.Where(r => r.Diretoria_Id == filtro.Diretoria_Id).ToList();

            if (filtro.Missao_Id > 0)
                lista = lista.Where(r => r.Missao_Id == filtro.Gerencia_Id).ToList();

            if (filtro.Visao_Id > 0)
                lista = lista.Where(r => r.Visao_Id == filtro.Coordenacao_Id).ToList();

            if (filtro.Dimensao_Id > 0)
                lista = lista.Where(r => r.Dimensao_Id == filtro.TemaAssunto_Id).ToList();

            if (filtro.Objetivo_Id > 0)
                lista = lista.Where(r => r.Objetivo_Id == filtro.ObjetivoGerencial_Id).ToList();

            if (filtro.IndicadoresDiretriz_Id > 0)
                lista = lista.Where(r => r.IndicadoresDiretriz_Id == filtro.IndicadoresDeProjeto_Id).ToList();

            if (filtro.Responsavel_Diretriz > 0)
                lista = lista.Where(r => r.Responsavel_Diretriz == filtro.IndicadoresDiretriz_Id).ToList();

            if (filtro.IsfiltrarAcao.GetValueOrDefault())
                lista = lista.Where(r => r.Estrategico_Id != null).ToList();
            else
                lista = lista.Where(r => r.Diretoria_Id > 0 && r.Missao_Id > 0 && r.Dimensao_Id > 0).ToList();

            ViewBag.Filtradas = lista;

            return PartialView("Filtrar");
        }

        [HttpGet]
        public ActionResult Editar(int id, bool? isTatico )
        {
            //var model = Pa_Planejamento.Listar().FirstOrDefault();
            var model = new Pa_Planejamento();
            if(isTatico == true)
            {
                model = Pa_Planejamento.GetTatico(id);
            }
            else
            {
                model = Pa_Planejamento.Get(id);
            }
                

            if(model.DataInicio != null)
                model._DataInicio = model.DataInicio.GetValueOrDefault().ToString("dd/MM/yyyy");
            if (model.DataFim != null)
                model._DataFim = model.DataFim.GetValueOrDefault().ToString("dd/MM/yyyy");
            if (model.ValorDe > 0)
                model._ValorDe = model.ValorDe.ToString("G29");
            if (model.ValorPara > 0)
                model._ValorPara = model.ValorPara.ToString("G29");
            if(model.Gerencia_Id > 0)
                ViewBag.Coordenacao = Pa_Coordenacao.Listar().Where(r=> r.GERENCIA_ID == model.Gerencia_Id);
            if (model.Iniciativa_Id > 0)
                ViewBag.IndicadoresDeProjeto = Pa_IndicadoresDeProjeto.Listar().Where(r => r.Pa_Iniciativa_Id == model.Iniciativa_Id);
            if (model.IndicadoresDeProjeto_Id > 0)
                ViewBag.ObjetivoGerencial = Pa_ObjetivoGeral.Listar().Where(r => r.Pa_IndicadoresDeProjeto_Id == model.IndicadoresDeProjeto_Id);

            return PartialView("Index", model);
        }

        #region PArtial de DDL's

        /// <summary>
        /// Partial de DDL de objetivos
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GETObjetivo(int id)
        {
            if (id > 0)
                ViewBag.Disabled = "false";
            else
                ViewBag.Disabled = "true";
            ViewBag.DdlName = "Objetivo_Id";

            var results = Pa_Objetivo.GetObjetivoXDimensao(id);
            if (results == null)
                results = new List<Pa_Objetivo>();


            ViewBag.Ddl = new SelectList(results, "Id", "Name");

            return PartialView("_DdlGenerica");
        }


        public ActionResult GETIndicadoresDiretriz(int id)
        {
            if (id > 0)
                ViewBag.Disabled = "false";
            else
                ViewBag.Disabled = "true";
            ViewBag.DdlName = "IndicadoresDiretriz_Id";

            var results = Pa_IndicadoresDiretriz.GetIndicadoresDiretrizXObjetivo(id);
            if (results == null)
                results = new List<Pa_IndicadoresDiretriz>();

            ViewBag.Ddl = new SelectList(results, "Id", "Name");

            return PartialView("_DdlGenerica");
        }

        public ActionResult GETIndicadoresProjetoIniciativa(int id)
        {
            if (id > 0)
                ViewBag.Disabled = "false";
            else
                ViewBag.Disabled = "true";
            ViewBag.DdlName = "IndicadoresDeProjeto_Id";

            var results = Pa_IndicadoresDeProjeto.GetIndicadoresProjetoXiniciativa(id);
            if (results == null)
                results = new List<Pa_IndicadoresDeProjeto>();

            ViewBag.Ddl = new SelectList(results, "Id", "Name");

            return PartialView("_DdlGenerica");
        }

        public ActionResult GETCoordenacaoByGerencia(int id)
        {
            if (id > 0)
                ViewBag.Disabled = "false";
            else
                ViewBag.Disabled = "true";
            ViewBag.DdlName = "Coordenacao_Id";

            var results = Pa_Coordenacao.GetCoordenacaoByGerencia(id);
            if (results == null)
                results = new List<Pa_Coordenacao>();

            ViewBag.Ddl = new SelectList(results, "Id", "Name");

            return PartialView("_DdlGenerica");
        }


        public ActionResult GETObjetivosGerenciais(int id)
        {
            if (id > 0)
                ViewBag.Disabled = "false";
            else
                ViewBag.Disabled = "true";
            ViewBag.DdlName = "ObjetivoGerencial_Id";

            var results = Pa_ObjetivoGeral.GetObjetivoXIndicadoresProjeto(id);
            if (results == null)
                results = new List<Pa_ObjetivoGeral>();

            ViewBag.Ddl = new SelectList(results, "Id", "Name");

            return PartialView("_DdlGenerica");
        }


        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {

            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");

            try
            {

                System.Resources.ResourceManager resourceManager = ResourcesPA.Resource.ResourceManager;

                ViewBag.Resources = resourceManager.GetResourceSet(
                    Thread.CurrentThread.CurrentUICulture, true, false).Cast<DictionaryEntry>();

            }
            catch (Exception ex)
            {
            }

            base.Initialize(requestContext);
        }

        #endregion
    }
}
