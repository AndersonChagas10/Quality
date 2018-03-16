using Dominio;
using Newtonsoft.Json.Linq;
using SgqSystem.Helpers;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.RelatoriosBrasil
{
    [RoutePrefix("api/VerificacaoTipificacao2")]
    public class VerificacaoTipificacaoApiController : ApiController
    {
        [HttpPost]
        [Route("Get")]
        public dynamic Get([FromBody] FormularioParaRelatorioViewModel form)
        {
            CommonLog.SaveReport(form, "Report_Verificacao_Tipificacao");

            var db = new SgqDbDevEntities();

            var query =
            "\n select vp.[Id] " +
            "\n ,ParCompany.Name as 'Unidade' " +
            "\n ,UserSgq.Name as 'Monitor' " +
            "\n ,Convert(varchar(10), CollectionDate, 103) as 'Data da Coleta' " +
            "\n ,Convert(char(5), CollectionDate, 108) as 'Hora da Coleta' " +
            "\n ,REPLACE(REPLACE([cIdentificadorTipificacao], '>', ''), '<','') as  'Indentificador da Tipificação' " +
            "\n ,[Sequencial] as 'Sequencial' " +
            "\n ,[Banda] as 'Banda' " +
            "\n ,cp.cNmCaracteristica as 'SGQ' " +
            "\n ,cpjbs.cNmCaracteristica as 'JBS' " +
            "\n ,case [ResultadoComparacaoGRT_JBS] " +
            "\n         WHEN 'True' THEN 'SIM' ELSE 'NÃO' END as 'Conforme?' " +
            "\n FROM VerificacaoTipificacaoV2 vp " +
            "\n INNER JOIN ParCompany on ParCompany.Id = vp.[ParCompany_Id] " +
            "\n INNER JOIN UserSgq on UserSgq.Id = vp.UserSgq_Id " +
            "\n LEFT JOIN CaracteristicaTipificacao cp ON cp.nCdCaracteristica = vp.GRT_nCdCaracteristicaTipificacao " +
            "\n LEFT JOIN CaracteristicaTipificacao cpjbs ON cpjbs.nCdCaracteristica = vp.JBS_nCdCaracteristicaTipificacao" +
            "\n WHERE CAST(CollectionDate as DATE) BETWEEN '" + form._dataInicioSQL + "' AND '" + form._dataFimSQL + "' " +
            "\n AND[cIdentificadorTipificacao] NOT IN ('<DIF>', '<AREA>') " +
            "\n AND vp.[ParCompany_Id] = " + form.unitId + "";

            var retorno = GenericTable.QueryNinja(db, query);

            return retorno;
        }

        //    [HttpPost]
        //    [Route("Edit")]
        //    public string Edit(int Id)
        //    {
        //        return VerificacaoTipificacaoResult.GetById(Id); ;
        //    }
        //}

        //public class VerificacaoTipificacaoResult
        //{
        //    public static string GetById(int id)
        //    {
        //        VerificacaoTipificacaoV2 vt;
        //        //List<VerificacaoTipificacaoV2> verificacaoTipificacao2;

        //        using (var databaseSgq = new SgqDbDevEntities())
        //        {
        //            vt = databaseSgq.VerificacaoTipificacaoV2.FirstOrDefault(r => r.Id == id);

        //            //verificacaoTipificacao2 = databaseSgq.VerificacaoTipificacaoV2.Where(r => r.Key == vt.Key && r.cIdentificadorTipificacao == vt.cIdentificadorTipificacao).ToList();

        //            switch (vt.cIdentificadorTipificacao)
        //            {
        //                case "<AREA>":
        //                    return mountHtmlArea(vt);
        //                case "<CONTUSAO>":
        //                    return mountHtmlContusao(vt);
        //                case "<FALHAOP>":
        //                    return mountHtmlFalhaOp(vt);
        //                case "<GORDURA>":
        //                    return mountHtmlGordura(vt);
        //                case "<IDADE>":
        //                    return mountHtmlIdade(vt);
        //                case "<SEXO>":
        //                    return mountHtmlSexo(vt);
        //                default:
        //                    return null;
        //            }
        //        }
        //    }

        //    public static string mountHtmlArea(VerificacaoTipificacaoV2 verificacaoTipificacao2)
        //    {
        //        return mountQuery(205, true);
        //    }

        //    public static string mountHtmlContusao(VerificacaoTipificacaoV2 verificacaoTipificacao2)
        //    {
        //        return mountQuery(205);
        //    }

        //    public static string mountHtmlFalhaOp(VerificacaoTipificacaoV2 verificacaoTipificacao2)
        //    {
        //        return mountQuery(206);
        //    }

        //    public static string mountHtmlGordura(VerificacaoTipificacaoV2 verificacaoTipificacao2)
        //    {
        //        return mountQuery(203);
        //    }

        //    public static string mountHtmlIdade(VerificacaoTipificacaoV2 verificacaoTipificacao2)
        //    {
        //        return mountQuery(201);
        //    }

        //    public static string mountHtmlSexo(VerificacaoTipificacaoV2 verificacaoTipificacao2)
        //    {
        //        return mountQuery(207);
        //    }

        //    public static string mountQuery(int number, bool isArea = false)
        //    {
        //        var result = new List<CaracteristicaTipificacao>();
        //        var retorno = "";

        //        using (var db = new SgqDbDevEntities())
        //        {
        //            var sql = "";

        //            if (isArea)
        //            {
        //                sql = "\n select CP.nCdCaracteristica " +
        //                     "\n , CP.cNmCaracteristica " +
        //                     "\n , CP.cNrCaracteristica " +
        //                     "\n , CP.cSgCaracteristica " +
        //                     "\n , CP.cIdentificador from AreasParticipantes CP  (nolock) " +
        //                     "\n where LEN(cNrCaracteristica) >= 5";
        //            }
        //            else
        //            {
        //                sql = "\n select CP.nCdCaracteristica " +
        //                            "\n , CP.cNmCaracteristica " +
        //                            "\n , CP.cNrCaracteristica " +
        //                            "\n , CP.cSgCaracteristica " +
        //                            "\n , CP.cIdentificador from CaracteristicaTipificacao CP (nolock) " +
        //                            "\n where LEN(CP.cNrCaracteristica) >= 5 and SUBSTRING(CP.cNrCaracteristica, 1, 3) = '" + number + "'";
        //            }

        //            result = db.Database.SqlQuery<CaracteristicaTipificacao>(sql).ToList();

        //        }

        //        retorno = "<ul class=\"list-group\"> ";

        //        foreach (var item in result)
        //        {
        //            retorno += "<li class=\"list-group-item item\" id=\"" + item.cNrCaracteristica + "\"><label class=\"checkbox-inline\"><input type=\"checkbox\" value=\"\">" + item.cNmCaracteristica + "</label></li> ";
        //        }

        //        retorno += "</ul>";

        //        return retorno;
        //    }
    }
}
