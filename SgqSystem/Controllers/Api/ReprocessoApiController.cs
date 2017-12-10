﻿using ADOFactory;
using Dominio;
using SgqSystem.Handlres;
using SgqSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{
    [HandleApi()]
    [RoutePrefix("api/Reprocesso")]
    public class ReprocessoApiController : ApiController
    {
        public class RetrocessoReturn
        {
           public List<ParReprocessoHeaderOP> parReprocessoHeaderOPs { get; set; }
           public List<ParReprocessoCertificadosSaidaOP> parReprocessoCertificadosSaidaOP { get; set; }
           public List<ParReprocessoSaidaOP> parReprocessoSaidaOPs { get; set; }
           public List<ParReprocessoEntradaOP> parReprocessoEntradaOPs { get; set; }
            public List<Header> headerFields { get; set; }
        }

        public class ParReprocessoHeaderOP
        {
            public int nCdOrdemProducao { get; set; }
            public int nCdEmpresa { get; set; }
            public DateTime dLancamento { get; set; }
            public int nCdUsuario { get; set; }
            public String cCdRastreabilidade { get; set; }
            public int nCdHabilitacao { get; set; }
        }

        public class ParReprocessoCertificadosSaidaOP
        {
            public int nCdOrdemProducao { get; set; }
            public int nCdCertificacao { get; set; }
        }

        public class ParReprocessoSaidaOP
        {
            public int nCdOrdemProducao { get; set; }
            public int iItem { get; set; }
            public double nCdProduto { get; set; }
            public int iQtdeValor { get; set; }
            public String cQtdeTipo { get; set; }
            public int nCdLocalEstoque { get; set; }
        }

        public class ParReprocessoEntradaOP
        {
            public int nCdOrdemProducao { get; set; }
            public int nCdProduto { get; set; }
            public DateTime dProduto { get; set; }
            public DateTime dEmbalagem { get; set; }
            public DateTime dValidade { get; set; }
            public int nCdLocalEstoque { get; set; }
            public String cCdOrgaoRegulador { get; set; }
            public String cCdRastreabilidade { get; set; }
            public int iVolume { get; set; }
            public double nPesoLiquido { get; set; }
            public Produto produto { get; set; }
        }

        public class Produto
        {
            public decimal nCdProduto { get; set; }
            public String cNmProduto { get; set; }
            public String cDescricaoDetalhada { get; set; }
        }

        public class Header
        {
            public String Id { get; set; }
        }

        [Route("Get/{ParCompany_Id}")]
        [HttpGet]
        public RetrocessoReturn Get(int ParCompany_Id)
        {
            Factory factorySgq = new Factory("DbContextSgqEUA");
            
            Factory factoryParReprocessoHeaderOP = new Factory("CONN_ParReprocessoHeaderOP");
            Factory factoryParReprocessoCertificadosSaidaOP = new Factory("CONN_ParReprocessoCertificadosSaidaOP");
            Factory factoryParReprocessoEntradaOP = new Factory("CONN_ParReprocessoEntradaOP");
            Factory factoryParReprocessoSaidaOP = new Factory("CONN_ParReprocessoSaidaOP");

            SgqDbDevEntities sgqDbDevEntities = new SgqDbDevEntities();            

            var parCompany = sgqDbDevEntities.ParCompany.FirstOrDefault(r => r.Id == ParCompany_Id);

            if (parCompany != null)
            {
                return new RetrocessoReturn
                {
                    parReprocessoHeaderOPs = factoryParReprocessoHeaderOP.SearchQuery<ParReprocessoHeaderOP>("EXEC " + AppSettingsWebConfig.GetValue("PROC_ParReprocessoHeaderOP") + " " + parCompany.CompanyNumber),
                    parReprocessoCertificadosSaidaOP = factoryParReprocessoCertificadosSaidaOP.SearchQuery<ParReprocessoCertificadosSaidaOP>("EXEC " + AppSettingsWebConfig.GetValue("PROC_ParReprocessoCertificadosSaidaOP")),
                    parReprocessoSaidaOPs = factoryParReprocessoSaidaOP.SearchQuery<ParReprocessoSaidaOP>("EXEC " + AppSettingsWebConfig.GetValue("PROC_ParReprocessoSaidaOP")),
                    parReprocessoEntradaOPs =
                    factoryParReprocessoEntradaOP.SearchQuery<ParReprocessoEntradaOP>("EXEC " + AppSettingsWebConfig.GetValue("PROC_ParReprocessoEntradaOP")).Select(r =>
                    {
                        r.produto = factorySgq.SearchQuery<Produto>("SELECT * FROM Produto WHERE nCdProduto = " + r.nCdProduto).FirstOrDefault();
                        return r;
                    }).ToList(),
                    headerFields = factorySgq.SearchQuery<Header>("SELECT 'cb'+ CAST(id AS VARCHAR(400)) AS Id FROM ParHeaderField WHERE Description like 'Reprocesso%'")
                };
                
            }

            return null;

        }
    }
}
