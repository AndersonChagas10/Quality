﻿using Dominio;
using DTO.Helpers;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SgqSystem.Helpers
{
    public class CommonLog
    {
        public static void SaveReport(FormularioParaRelatorioViewModel form, string ReportName)
        {
            using (var db = new SgqDbDevEntities())
            {

                if (form.level1Id > 0)
                    form.level1Name = db.ParLevel1.Where(r => r.Id == form.level1Id).FirstOrDefault().Name;
                if (form.level2Id > 0)
                    form.level2Name = db.ParLevel2.Where(r => r.Id == form.level2Id).FirstOrDefault().Name;
                if (form.level3Id > 0)
                    form.level3Name = db.ParLevel3.Where(r => r.Id == form.level3Id).FirstOrDefault().Name;
                if (form.auditorId > 0)
                    form.auditorName = db.UserSgq.Where(r => r.Id == form.auditorId).FirstOrDefault().Name;
                if (form.unitId > 0)
                    form.unitName = db.ParCompany.Where(r => r.Id == form.unitId).FirstOrDefault().Name;

                var log = new LogJson();
                log.Device_Id = "Web";
                log.callback = ReportName;
                log.AddDate = DateTime.Now;
                log.log = 
                    "Usuario:" + form.auditorId + "|"+
                    "Unidade:"+form.unitId+"|"+
                    "DataInicio:"+form._dataInicio+"|"+
                    "DataFim:"+form._dataFim+"|"+
                    "Indicador:"+form.level1Id+"|"+
                    "Monitoramento:"+form.level2Id+"|"+
                    "Tarefa:"+form.level3Id + "|" +
                    "Usuario:" + form.auditorId + "|" +
                    "UsuarioNome: " + form.auditorName
                    ;
                
                log.result =
                    "Unidade:" + form.unitId + "|" +
                    "UnidadeNome:" + form.unitName + "|" +
                    "DataInicio:" + form._dataInicio + "|" +
                    "DataFim:" + form._dataFim + "|" +
                    "Indicador:" + form.level1Id + "|" +
                    "IndicadorNome:" + form.level1Name + "|" +
                    "Monitoramento:" + form.level2Id + "|" +
                    "MonitoramentoNome:" + form.level2Name + "|" +
                    "Tarefa:" + form.level3Id+
                    "TarefaNome:" + form.level3Name 
                    ;

                db.LogJson.Add(log);
                db.SaveChanges();
            }
        }
    }
}