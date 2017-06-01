﻿using DTO.Helpers;
using Newtonsoft.Json.Linq;
using PlanoAcaoCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace PlanoDeAcaoMVC.Controllers.Api
{
    [RoutePrefix("api/Relatorios")]
    public class ApiRelatoriosController : ApiController
    {
        PlanoAcaoEF.PlanoDeAcaoEntities db;
        public ApiRelatoriosController()
        {
            db = new PlanoAcaoEF.PlanoDeAcaoEntities();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.AutoDetectChangesEnabled = false;
        }

        //Retorna Categorias Series e Dados
        [HttpPost]
        [Route("GetGrafico")]
        public List<RetornoGrafico1> GetGrafico([FromBody] filtros filtro)
        {
            var categoria = filtro.categoria;
            //var series = filtro.serie;
            var series = "[Status]";
            //var filtroCategoria = "";

            var dataInicio = Guard.ParseDateToSqlV2(filtro.dataInicio).ToString("yyyyMMdd");
            var dataFim = Guard.ParseDateToSqlV2(filtro.dataFim).ToString("yyyyMMdd");

            //var dataInicio = DateTime.ParseExact(filtro.dataInicio, "yyyyMMdd", CultureInfo.InvariantCulture); 
            //var dataFim = DateTime.ParseExact(filtro.dataFim, "yyyyMMdd", CultureInfo.InvariantCulture).ToString();

            var where = " where QuandoInicio <= '" + dataFim + "' and QuandoFim <= '" + dataFim + "' ";
            var orderby1 = " order by 1";

            var indicadores = Pa_Qualquer_Name.Listar(categoria);

            var status = Pa_Qualquer_Name.Listar(series);

            var sql1 = "select distinct(" + categoria + ") as valor from pa_acao a left join Pa_acaoXQuem xq on xq.Acao_Id = a.Id left join Pa_Quem q on q.id = xq.Quem_Id";
            sql1 += where;
            sql1 += orderby1;

            var sql2 = "select distinct(" + series + ") as valor from pa_acao";
            sql2 += where;
            sql2 += orderby1;

            var ret1 = Pa_BaseObject.ListarGenerico<RetornoInt>(sql1);
            var ret2 = Pa_BaseObject.ListarGenerico<RetornoInt>(sql2);
            var retorno = new List<RetornoGrafico1>();

            foreach (var i in ret1)
            {
                var temp1 = new RetornoGrafico1() { Indicador_Id = i.valor, Indicador = indicadores.FirstOrDefault(r => r.Id == i.valor)?.Name };
                if (temp1 == null)
                    continue;
                temp1.Status = new List<Status>();

                foreach (var b in ret2)
                {
                    var statusObj = new Status() { Id = b.valor, Name = status.FirstOrDefault(r => r.Id == b.valor).Name };
                    statusObj.QuantidadeAcoes = new List<int>();

                    foreach (var ii in ret1)
                    {
                        var sqlQtd = "select count(1) as valor from Pa_Acao a left join Pa_acaoXQuem xq on xq.Acao_Id = a.Id left join Pa_Quem q on q.id = xq.Quem_Id";
                        sqlQtd += where;
                        sqlQtd += " and " + categoria + " = " + ii.valor + " and " + series + " = " + b.valor;
                        sqlQtd += orderby1;

                        var qtd = Pa_BaseObject.ListarGenerico<RetornoInt>(sqlQtd).FirstOrDefault().valor;
                        statusObj.QuantidadeAcoes.Add(qtd);
                    }
                    temp1.Status.Add(statusObj);
                }
                retorno.Add(temp1);
            }
            return retorno;
        }

        //Retorna apenas Series e Dados
        [HttpPost]
        [Route("GraficoPie")]
        public List<GraficoPieSet> GraficoPie([FromBody] filtros filtro)
        {
            var dataInicio = Guard.ParseDateToSqlV2(filtro.dataInicio).ToString("yyyyMMdd");
            var dataFim = Guard.ParseDateToSqlV2(filtro.dataFim).ToString("yyyyMMdd");
            var listStatus = Pa_Status.Listar();
            var total = Pa_BaseObject.ListarGenerico<RetornoInt>("Select count(*) from Pa_Acao").FirstOrDefault().valor;
            var retorno = new List<GraficoPieSet>();
            foreach (var i in listStatus)
            {
                var queryCount = "select count(id) as valor from pa_acao where [status] = " + i.Id + "and QuandoInicio <= '" + dataFim + "' and QuandoFim <= '" + dataFim + "' "; ;
                var count = Pa_BaseObject.ListarGenerico<RetornoInt>(queryCount).FirstOrDefault().valor;
                retorno.Add(new GraficoPieSet() { name = i.Name, y = count });
            }

            return retorno;
        }

        [HttpPost]
        [Route("NumeroDeAcoesEstabelecidas")]
        public List<JObject> NumeroDeAcoesEstabelecidas(JObject form)
        {
            dynamic teste = form;

             var query = "SELECT DATEPART(mm,QuandoInicio) as Corno, Count(id) as Quantidade FROM [PlanoDeAcao].[dbo].[Pa_Acao] group by  DATEPART(mm,QuandoInicio)";
            List<JObject> items = QueryNinja(query);

            return items;
        }

        private List<JObject> QueryNinja(string query)
        {
            db.Database.Connection.Open();
            var cmd = db.Database.Connection.CreateCommand();
            cmd.CommandText = query;
            var reader = cmd.ExecuteReader();
            List<JObject> items = new List<JObject>();
            while (reader.Read())
            {
                JObject row = new JObject();
                for (int i = 0; i < reader.FieldCount; i++)
                    row[reader.GetName(i)] = reader[i].ToString();

                items.Add(row);
            }

            return items;
        }
    }

    public class GraficoPieSet
    {
        public string name { get; set; }
        public int y { get; set; }
    }

    public class RetornoInt
    {
        public int valor { get; set; }
    }

    public class RetornoGrafico1
    {
        public string Indicador { get; set; }
        public int Indicador_Id { get; set; }
        public List<Status> Status { get; set; }
    }

    public class Status
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public List<int> QuantidadeAcoes { get; set; }
    }

    /// <summary>
    /// Json : 
    /// { 
    ///     diretoria: 1,
    ///     gerencia: 2,
    ///     dataInicio: '20/01/2017'
    ///     dataFim: '20/01/2017'
    /// }
    /// </summary>
    public class filtros
    {
        public int diretoria { get; set; }
        public int gerencia { get; set; }
        public string dataInicio { get; set; }
        public string dataFim { get; set; }
        public string categoria { get; set; }
        public string serie { get; set; }
        public string[] filtroCategoria { get; set; }
        public int relatorio { get; set; }
    }

}
