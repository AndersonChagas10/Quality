﻿using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO.Params;
using SgqSystem.Handlres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace SgqSystem.Controllers.Api
{
    [HandleApi()]
    [RoutePrefix("api/Manutencao")]
    public class ManutencaoController : ApiController
    {
        [HttpPost]
        [Route("getTabela1")]
        public List<Pacote> getSelectTabela1()
        {
            var lista = new List<Pacote>();

            using (var db = new SgqDbDevEntities())
            {
                var sql = "select distinct(Pacote) as Name from Manutencao WHERE MesAno BETWEEN '20150101' AND '20180101'AND TipoInformacao = 'CustoFixo' order by Pacote";
                
                lista = db.Database.SqlQuery<Pacote>(sql).ToList();

                foreach (var item in lista)
                {
                    sql = " select EmpresaRegional as Regional, ROUND(SUM(DespesaOrcada), 0) AS Orçada,  ROUND(SUM(DespesaRealizada), 0) AS Realizada,";
                    sql += "CASE WHEN SUM(DespesaOrcada) = 0 THEN 0 ELSE ROUND((SUM(DespesaRealizada) / SUM(DespesaOrcada) - 1) * 100, 0) END AS DesvioPorc,";
                    sql += "ROUND(SUM(DespesaRealizada) - SUM(DespesaOrcada), 0) AS DesvioReal";
                    sql += " from Manutencao ";
                    sql += "WHERE MesAno BETWEEN '20150101' AND '20180101' AND TipoInformacao = 'CustoFixo' and Pacote in ( '" + item.Name + "')  group by EmpresaRegional  order by EmpresaRegional asc; ";

                    item.ListaRegionais = db.Database.SqlQuery<Reg>(sql).ToList();

                    sql = " select Pacote as Pacote, ROUND(SUM(DespesaOrcada), 0) AS Orçada,  ROUND(SUM(DespesaRealizada), 0) AS Realizada,";
                    sql += "CASE WHEN SUM(DespesaOrcada) = 0 THEN 0 ELSE ROUND((SUM(DespesaRealizada) / SUM(DespesaOrcada) - 1) * 100, 0) END AS DesvioPorc,";
                    sql += "ROUND(SUM(DespesaRealizada) - SUM(DespesaOrcada), 0) AS DesvioReal";
                    sql += " from Manutencao ";
                    sql += "WHERE MesAno BETWEEN '20150101' AND '20180101' AND TipoInformacao = 'CustoFixo' and Pacote in ( '" + item.Name + "')  group by Pacote  order by Pacote asc; ";
                    item.total = db.Database.SqlQuery<TotalPacote>(sql).FirstOrDefault();

                }

            }

            return lista;
        }

       
        [HttpPost]
        [Route("getTabela2")]
        public List<Pacote> getSelectTabela2([FromBody]string pacote)
        {
            
            var lista = new List<Pacote>();
            var listaSiglas = new List<SiglasUnidades>();

            using (var db = new SgqDbDevEntities())
            {
                var sql = "select distinct(ContaContabil) as Name from Manutencao WHERE MesAno BETWEEN '20150101' AND '20180101'AND TipoInformacao = 'CustoFixo' and Pacote in ('Utilidades') and EmpresaRegional in ('Reg 1 - GO/MG/BA') group by ContaContabil order by ContaContabil";
                lista = db.Database.SqlQuery<Pacote>(sql).ToList();
                var siglas = "select distinct(EmpresaSigla) as Name from Manutencao WHERE MesAno BETWEEN '20150101' AND '20180101'AND TipoInformacao = 'CustoFixo' and Pacote in ('Utilidades') and EmpresaRegional in ('Reg 1 - GO/MG/BA') group by EmpresaSigla order by EmpresaSigla";
                listaSiglas = db.Database.SqlQuery<SiglasUnidades>(siglas).ToList();

                foreach (var item in lista)
                {
                    sql = "select Regional, sum(Orçada) as Orçada, sum(Realizada) as Realizada, sum(DesvioPorc) as DesvioPorc, sum(DesvioReal) as DesvioReal from(select EmpresaSigla as Regional, ";
                    sql += "\n ROUND(SUM(DespesaOrcada), 0) AS Orçada,";
                    sql += "\n ROUND(SUM(DespesaRealizada), 0) AS Realizada,";
                    sql += "\n CASE WHEN SUM(DespesaOrcada) = 0 THEN 0 ELSE ROUND((SUM(DespesaRealizada) / SUM(DespesaOrcada) - 1) * 100, 0) END AS DesvioPorc,";
                    sql += "\n ROUND(SUM(DespesaRealizada) - SUM(DespesaOrcada), 0) AS DesvioReal";
                    sql += "\n from Manutencao";
                    sql += "\n WHERE MesAno BETWEEN '20150101' AND '20180101'";
                    sql += "\n AND TipoInformacao = 'CustoFixo'";
                    sql += "\n and Pacote in ('')";
                    sql += "\n and EmpresaRegional in ('')";
                    sql += "\n and ContaContabil in ('" +item.Name+ "')";
                    sql += "\n group by EmpresaSigla";
                    sql += "\n union all select EmpresaSigla as Regional,  0 AS Orçada,  0 AS Realizada,  0 AS DesvioPorc,  0 AS DesvioReal  from Manutencao";
                    sql += "\n WHERE MesAno BETWEEN '20150101' AND '20180101' AND TipoInformacao = 'CustoFixo' and Pacote in ('Utilidades') and EmpresaRegional in ('Reg 1 - GO/MG/BA') ";
                    sql += "\n group by EmpresaSigla )teste  group by Regional  order by 1 asc";
                    var aux = db.Database.SqlQuery<Reg>(sql);

                    


                    item.ListaRegionais = db.Database.SqlQuery<Reg>(sql).ToList();
                                 
                    
                    
                    sql = " select ContaContabil as Pacote, ROUND(SUM(DespesaOrcada), 0) AS Orçada,  ROUND(SUM(DespesaRealizada), 0) AS Realizada,";
                    sql += "CASE WHEN SUM(DespesaOrcada) = 0 THEN 0 ELSE ROUND((SUM(DespesaRealizada) / SUM(DespesaOrcada) - 1) * 100, 0) END AS DesvioPorc,";
                    sql += "ROUND(SUM(DespesaRealizada) - SUM(DespesaOrcada), 0) AS DesvioReal";
                    sql += " from Manutencao ";
                    sql += " WHERE MesAno BETWEEN '20150101' AND '20180101' AND TipoInformacao = 'CustoFixo'";
                    sql += " and Pacote in ('Utilidades') and EmpresaRegional in ('Reg 1 - GO/MG/BA') and ContaContabil in ( '" + item.Name + "')  group by ContaContabil  order by ContaContabil asc; ";
                    item.total = db.Database.SqlQuery<TotalPacote>(sql).FirstOrDefault();

                }

            }
            //return JsonConvert.SerializeObject("Deu certo");
            return lista;
        }
    }

    public class Pacote
    {
        public string Name { get; set; }
        public List<Reg> ListaRegionais { get; set; }
        public TotalPacote total { get; set; }
    }

    public class SiglasUnidades
    {
        public string Name { get; set; }
    }

    public class Reg
    {
        public string Regional { get; set; }
        public double? Orçada { get; set; }
        public double? Realizada { get; set; }
        public double? DesvioPorc { get; set; }
        public double? DesvioReal { get; set; }
    }

    public class TotalPacote
    {
        public string Pacote { get; set; }
        public double? Orçada { get; set; }
        public double? Realizada { get; set; }
        public double? DesvioPorc { get; set; }
        public double? DesvioReal { get; set; }
    }


}
