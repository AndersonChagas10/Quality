using ADOFactory;
using AutoMapper;
using Dominio;
using DTO;
using DTO.DTO;
using DTO.Helpers;
using Helper;
using SgqService.ViewModels;
using SgqSystem.Helpers;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{

    [RoutePrefix("api/CorrectiveAction")]
    public class CorrectActApiController : ApiController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        [Route("GetTableCorrectiveAction")]
        [HttpPost]
        public List<CorrectiveAct> GetTableCorrectiveAction([FromBody]FormularioParaRelatorioViewModel model)
        {
            var table = new List<CorrectiveAct>();

            var query = @"SELECT
                            CA.Id as Id
	                       ,CL2.UnitId
                           ,PC.Name AS Unidade
                           ,CL2.ParLevel1_Id
                           ,P1.Name AS Indicador
                           ,CL2.ParLevel2_Id
                           ,P2.Name AS Monitoramento
                           ,CA.PreventativeMeasure
                        FROM correctiveaction AS CA WITH (NOLOCK)
                        LEFT JOIN CollectionLevel2 AS CL2 WITH (NOLOCK)
	                        ON CL2.Id = CA.CollectionLevel02Id
                        LEFT JOIN Result_Level3 AS RL3 WITH (NOLOCK)
	                        ON RL3.Id = CL2.Id
                        LEFT JOIN ParLevel1 AS P1 WITH (NOLOCK)
	                        ON CL2.ParLevel1_Id = P1.Id
                        LEFT JOIN ParLevel2 AS P2 WITH (NOLOCK)
	                        ON CL2.ParLevel2_Id = P2.Id
                        LEFT JOIN ParCompany AS PC WITH (NOLOCK)
	                        ON Pc.Id = CL2.UnitId";
            using (Factory factory = new Factory("DefaultConnection"))
            {
                table = GlobalConfig.CorrectiveAct = factory.SearchQuery<CorrectiveAct>(query).ToList();
            }
            return table;
        }

        [Route("GetCorrectiveAct")]
        [HttpGet]
        public CorrectiveAct GetCorrectiveAct([FromUri] int editar)
        {
            var EditAct = new CorrectiveAct();
            var query = @"SELECT
	                        CA.Id
                           ,CL2.UnitId
                           ,PC.Name AS Unidade
                           ,CL2.ParLevel1_Id
                           ,P1.Name AS Indicador
                           ,CL2.ParLevel2_Id
                           ,P2.Name AS Monitoramento
                           ,CA.PreventativeMeasure
                        FROM correctiveaction AS CA WITH (NOLOCK)
                        LEFT JOIN CollectionLevel2 AS CL2 WITH (NOLOCK)
	                        ON CL2.Id = CA.CollectionLevel02Id
                        LEFT JOIN Result_Level3 AS RL3 WITH (NOLOCK)
	                        ON RL3.Id = CL2.Id
                        LEFT JOIN ParLevel1 AS P1 WITH (NOLOCK)
	                        ON CL2.ParLevel1_Id = P1.Id
                        LEFT JOIN ParLevel2 AS P2 WITH (NOLOCK)
	                        ON CL2.ParLevel2_Id = P2.Id
                        LEFT JOIN ParCompany AS PC WITH (NOLOCK)
	                        ON Pc.Id = CL2.UnitId
                        WHERE CA.Id =" + editar;
            using (Factory factory = new Factory("DefaultConnection"))
            {
                EditAct = GlobalConfig.GetCorrectiveAct = factory.SearchQuery<CorrectiveAct>(query).SingleOrDefault();
            }
            return EditAct;
        }

        [Route("SaveCorrectiveAct")]
        [HttpPost]
        public CorrectiveAct SaveCorrectiveAct([FromBody] CorrectiveAct correctiveAct)
        {
            return null;
        }

        public CorrectActApiController()
        {
            db.Configuration.ProxyCreationEnabled = false;
        }

        [Route("Save")]
        [HttpPost]
        public DTO.DTO.CorrectiveActionDTO Save(DTO.DTO.CorrectiveActionDTO save)
        {
            var objBanco = db.CorrectiveAction.Where(r => r.Id == save.Id).AsNoTracking().FirstOrDefault(); // busca obj do banco pela Id
            objBanco.DescriptionFailure = save.DescriptionFailure;
            objBanco.ImmediateCorrectiveAction = save.ImmediateCorrectiveAction;
            objBanco.ProductDisposition = save.ProductDisposition;
            objBanco.PreventativeMeasure = save.PreventativeMeasure;
            db.SaveChanges();
            return null;
        }

        [Route("GetCorrectiveAction")]
        [HttpPost]
        public List<CorrectiveActionDTO> GetCorrectiveAction([FromBody]FormularioParaRelatorioViewModel model)
        {

            var sql = "select " +
            " \n CorrectiveAction.id as Id, " +
            " \n CorrectiveAction.id as idcorrectiveaction, " +
            " \n UserSgqAuditor.Name as AuditorName, --join " +
            " \n UserSgqSlaugther.Name as NameSlaughter,--join " +
            " \n UserSgqTechinical.Name as NameTechinical, --join " +
            " \n ParLevel1.Name as level01Name, --join " +
            " \n ParLevel2.Name as level02Name, --join " +

            " \n DateTimeSlaughter, " +
            " \n DateTimeTechinical, " +
            " \n CorrectiveAction.AddDate, " +
            " \n CorrectiveAction.AlterDate, " +
            " \n DateCorrectiveAction, " +
            " \n AuditStartTime, " +
            " \n DescriptionFailure, " +
            " \n AuditStartTime, " +
            " \n ImmediateCorrectiveAction, " +
            " \n ProductDisposition, " +
            " \n PreventativeMeasure, " +
            " \n CollectionLevel02Id, " +
            " \n CollectionLevel2_Id " +

            " \n from CorrectiveAction " +

            " \n INNER JOIN CollectionLevel2 ON CorrectiveAction.CollectionLevel02Id = CollectionLevel2.Id " +
            " \n INNER JOIN UserSgq UserSgqAuditor on CorrectiveAction.AuditorId = UserSgqAuditor.Id " +
            " \n LEFT JOIN UserSgq UserSgqSlaugther on CorrectiveAction.SlaughterId = UserSgqSlaugther.Id " +
            " \n LEFT JOIN UserSgq UserSgqTechinical on CorrectiveAction.TechinicalId = UserSgqTechinical.Id " +
            " \n INNER JOIN ParLevel1 on CollectionLevel2.ParLevel1_Id = ParLevel1.Id " +
            " \n INNER JOIN ParLevel2 on CollectionLevel2.ParLevel2_Id = ParLevel2.Id " +

            " \n where " +

            " \n CAST(DateCorrectiveAction as DATE) BETWEEN '" + model._dataInicioSQL + "' and '" + model._dataFimSQL + "' ";

            if (model.unitId > 0)
            {
                sql += " \n and CollectionLevel2.UnitId = " + model.unitId + " ";
            }

            if (model.shift > 0)
            {
                sql += " \n and CollectionLevel2.[Shift] = " + model.shift + " ";
            }

            if (model.period > 0)
            {
                sql += " \n and CollectionLevel2.Period = " + model.period + " ";
            }

            if (model.auditorId > 0)
            {
                sql += " \n and CorrectiveAction.AuditorId = " + model.auditorId + "";
            }

            var dados = new List<CorrectiveActionDTO>();
            using (Factory factory = new Factory("DefaultConnection"))
            {
                dados = factory.SearchQuery<CorrectiveActionDTO>(sql).ToList();
            }

            return dados;
        }

        [Route("GetCorrectiveActionById")]
        [HttpPost]
        public CorrectiveActionDTO GetCorrectiveActionById([FromBody]int id)
        {
            int leve1Id = 0;
            int leve2Id = 0;
            Shift shift = new Shift();
            Period period = new Period();

            CorrectiveAction obj = db.CorrectiveAction.Where(r => r.Id == id).FirstOrDefault();
            CorrectiveActionDTO obj2 = Mapper.Map<CorrectiveAction, CorrectiveActionDTO>(obj);
            obj2.AuditorName = db.UserSgq.Where(r => r.Id == obj.AuditorId).FirstOrDefault().Name;
            obj2.NameSlaughter = db.UserSgq.Where(r => r.Id == obj.SlaughterId).FirstOrDefault().FullName;
            obj2.NameTechinical = db.UserSgq.Where(r => r.Id == obj.TechinicalId).FirstOrDefault().FullName;

            leve1Id = db.CollectionLevel2.Where(r => r.Id == obj.CollectionLevel02Id).FirstOrDefault().ParLevel1_Id;
            leve2Id = db.CollectionLevel2.Where(r => r.Id == obj.CollectionLevel02Id).FirstOrDefault().ParLevel2_Id;

            obj2.Level02Id = leve2Id;
            obj2.level01Name = db.ParLevel1.Where(r => r.Id == leve1Id).FirstOrDefault().Name;
            obj2.level02Name = db.ParLevel2.Where(r => r.Id == leve2Id).FirstOrDefault().Name;

            var level2 = db.CollectionLevel2.Where(r => r.Id == obj.CollectionLevel02Id).FirstOrDefault();
            var pc = db.ParCompany.Where(r => r.Id == level2.UnitId).FirstOrDefault();

            obj2.ShiftName = level2.Shift.ToString();
            obj2.PeriodName = level2.Period.ToString();

            Unit unit = new Unit();
            unit.Code = pc.SIF;
            unit.Name = pc.Name;
            UnitDTO unitDto = Mapper.Map<Unit, UnitDTO>(unit);

            obj2.Unit = unitDto;

            return obj2;
        }

        [Route("SetCorrectiveAction")]
        [HttpPost]
        public List<CorrectiveAction> SetCorrectiveAction([FromBody]List<CorrectiveAction> correctiveActions)
        {
            var correctiveActionsSave = new List<CorrectiveAction>();

            using (var factory = new Factory("DefaultConnection"))
            {
                try
                {

                    //sgq @jbs.com.br
                    //Pi@ui1628
                    //correio.jbs.com.br
                    //587

                    foreach (var correctiveAction in correctiveActions)
                    {

                        //Fazer o update da collectionLevel2 para informar se houve ação corretiva
                        var sql = $@"Select 
                                	*
                            from CollectionLevel2  C2 
                            INNER JOIN CollectionLevel2xParDepartment C2XPD on C2.Id = C2XPD.CollectionLevel2_Id
                            INNER JOIN CollectionLevel2xParCargo C2XPC on C2.Id = C2XPC.CollectionLevel2_Id
                            WHERE C2.ParLevel1_Id = { correctiveAction.CollectionLevel2.ParLevel1_Id }
                            AND C2.ParLevel2_Id = { correctiveAction.CollectionLevel2.ParLevel2_Id }
                            AND C2.UnitId = { correctiveAction.CollectionLevel2.UnitId }
                            AND C2.EvaluationNumber = { correctiveAction.CollectionLevel2.EvaluationNumber }
                            AND C2.Sample = { correctiveAction.CollectionLevel2.Sample }
                            AND C2XPD.ParDepartment_Id = { correctiveAction.CollectionLevel2.ParDepartment_Id }
                            AND C2XPC.ParCargo_Id = { correctiveAction.CollectionLevel2.ParCargo_Id }
                            AND CAST(C2.CollectionDate AS SMALLDATETIME) BETWEEN 
                            '{ correctiveAction.CollectionLevel2.CollectionDate.ToString("yyyy-MM-dd") }' AND 
                            '{ correctiveAction.CollectionLevel2.CollectionDate.AddDays(1).ToString("yyyy-MM-dd") }'";


                        var collectionLevel2 = db.Database.SqlQuery<CollectionLevel2>(sql).FirstOrDefault();

                        if (collectionLevel2 == null)
                        {
                            continue;
                        }

                        var cargo = db.ParCargo.Where(x => x.Id == correctiveAction.CollectionLevel2.ParCargo_Id).FirstOrDefault();
                        var secao = db.ParDepartment.Where(x => x.Id == correctiveAction.CollectionLevel2.ParDepartment_Id).FirstOrDefault();
                        var departamento = db.ParDepartment.Where(x => x.Id == secao.Parent_Id).FirstOrDefault();


                        db.ErrorLog.Add(new Dominio.ErrorLog() { AddDate = DateTime.Now, StackTrace = "emailMock: " + DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.emailMock as string });
                        db.SaveChanges();

                        Task.Run(() => MailSender.SendMail(
                            "sgq@jbs.com.br",
                            "Pi@ui1628",
                            "correio.jbs.com.br",
                            587,
                            false,
                            DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.emailMock as string,
                            "Alerta KO emitido para o Indicador: Risco no uso de EPI´s, Monitoramento: Monitoramento do uso de EPI´s, Tarefa: O colaborador está utilizando luva anticorte sem folga nas pontas dos dedos, apertada ou esticada na palma da mão? Unidade: CGR - JBS S.A. - Campo Grande – CGR (02.916.265/0077-68)",
                            $@"
                                <p>{DateTime.Now.ToString("dd/MM/yyyy HH:mm")}, Alerta emitido para o <u>Indicador</u>: Risco no uso de EPI&acute;s,
                                <u>Monitoramento</u>: Monitoramento do uso de EPI&acute;s, 
                                <u>Tarefa</u>: O colaborador est&aacute; utilizando luva anticorte sem folga nas pontas dos dedos, apertada ou esticada na palma da m&atilde;o? 
                                <u>Frequ&ecirc;ncia</u>: Semanal, <u>Cargo</u>: <strong>{cargo?.Name}</strong>, 
                                <u>Se&ccedil;&atilde;o</u>: <strong>{secao?.Name}</strong> e 
                                <u>Centro de Custo</u>: <strong>{departamento?.Name}</strong> da <u>Unidade</u>: CGR - JBS S.A. - Campo Grande &ndash; CGR (02.916.265/0077-68).<br /> <br /> Alerta KO disparado. N&atilde;o Conformidade em Tarefa de Risco Intoler&aacute;vel. O superior e o SESMT ser&atilde;o notificados para tomada de a&ccedil;&atilde;o corretiva imediata para o Posto de Trabalho.</p>
                                <p>&nbsp;</p>
                                <p><strong>CGR - JBS S.A. - Campo Grande &ndash; CGR (02.916.265/0077-68)</strong></p>
                                <p><strong>Formul&aacute;rio de A&ccedil;&atilde;o Corretiva</strong></p>
                                <table>
                                <tbody>
                                <tr>
                                <td width='566'>
                                <p><strong>A&ccedil;&atilde;o Corretiva Tomada: </strong></p>
                                </td>
                                </tr>
                                <tr>
                                <td width='566'>
                                <p><strong>Data/Hora:</strong>&nbsp;{DateTime.Now.ToString("dd/MM/yyyy HH:mm")}</p>
                                </td>
                                </tr>
                                <tr>
                                <td width='566'>
                                <p><strong>Auditor:&nbsp;</strong>{collectionLevel2.UserSgq?.Name }</p>
                                </td>
                                </tr>
                                <tr>
                                <td width='566'>
                                <p><strong>Frequ&ecirc;ncia:</strong> Semanal</p>
                                </td>
                                </tr>
                                <tr>
                                <td width='566'>
                                <p><strong>&nbsp;</strong></p>
                                </td>
                                </tr>
                                <tr>
                                <td width='566'>
                                <p><strong>Informa&ccedil;&atilde;o da Auditoria:</strong></p>
                                </td>
                                </tr>
                                <tr>
                                <td width='566'>
                                <p><strong>Indicador:&nbsp;</strong>Risco no uso de EPI&acute;s</p>
                                </td>
                                </tr>
                                <tr>
                                <td width='566'>
                                <p><strong>Monitoramento:</strong> Monitoramento do uso de EPI&acute;s</p>
                                </td>
                                </tr>
                                <tr>
                                <td width='566'>
                                <p><strong>Tarefa:</strong> O colaborador est&aacute; utilizando luva anticorte sem folga nas pontas dos dedos, apertada ou esticada na palma da m&atilde;o?</p>
                                </td>
                                </tr>
                                <tr>
                                <td width='566'>
                                <p><strong>&nbsp;</strong></p>
                                </td>
                                </tr>
                                <tr>
                                <td width='566'>
                                <p><strong>Centro de Custo:</strong> {departamento?.Name}
                                
                                </tr>
                                <tr>
                                <td width='566'>
                                <p><strong>Se&ccedil;&atilde;o:&nbsp;</strong>{secao?.Name}</p>
                                </td>
                                </tr>
                                <tr>
                                <td width='566'>
                                <p><strong>Cargo:</strong> {cargo?.Name}</p>
                                </td>
                                </tr>
                                <tr>
                                <td width='566'>
                                <p><strong>&nbsp;</strong></p>
                                </td>
                                </tr>
                                <tr>
                                <td width='566'>
                                <p><strong>Descrição da Falha:</strong> {correctiveAction.DescriptionFailure}
                                
                                </tr>
                                <tr>
                                <td width='566'>
                                <p><strong>Ação Corretiva Imediata;</strong>{correctiveAction.ImmediateCorrectiveAction}</p>
                                </td>
                                </tr>
                                <tr>
                                <td width='566'>
                                <p><strong>Medida Preventiva:</strong> {correctiveAction.PreventativeMeasure}</p>
                                </td>
                                </tr>
                                </tbody>
                                </table>
                                "));

                        CorrectiveAction acaoCorretiva = new CorrectiveAction();

                        acaoCorretiva.CollectionLevel2_Id = collectionLevel2.Id;
                        acaoCorretiva.CollectionLevel02Id = collectionLevel2.Id;
                        acaoCorretiva.ImmediateCorrectiveAction = correctiveAction.ImmediateCorrectiveAction;
                        acaoCorretiva.PreventativeMeasure = correctiveAction.PreventativeMeasure;
                        acaoCorretiva.DescriptionFailure = correctiveAction.DescriptionFailure;
                        acaoCorretiva.ParLevel3_Id = correctiveAction.ParLevel3_Id;
                        acaoCorretiva.AuditorId = correctiveAction.AuditorId;

                        db.CorrectiveAction.Add(acaoCorretiva);
                        db.SaveChanges();

                        collectionLevel2 = db.CollectionLevel2.Where(x => x.Id == collectionLevel2.Id).FirstOrDefault();

                        collectionLevel2.HaveCorrectiveAction = true;
                        db.Entry(collectionLevel2).State = EntityState.Modified;
                        db.SaveChanges();

                        correctiveActionsSave.Add(correctiveAction);

                    }
                }
                catch (Exception ex)
                {
                    //deu ruim;
                    db.ErrorLog.Add(new Dominio.ErrorLog() { AddDate = DateTime.Now, StackTrace = "CA_RH " + ex.ToClient() });
                    db.SaveChanges();
                }
            }

            return correctiveActionsSave;
        }



        [Route("GetCorrectiveActionById2")]
        [HttpPost]
        public CorrectiveActionDTO GetCorrectiveActionById2([FromBody]int id)
        {
            using (var factory = new Factory("DefaultConnection"))
            {

                var correctiveAction = factory.SearchQuery<CorrectiveAction>("select top 1 * from CorrectiveAction where Id = " + id).FirstOrDefault();

                var correctiveActionDTO = Mapper.Map<CorrectiveAction, CorrectiveActionDTO>(correctiveAction);

                var collectionLevel2 = factory.SearchQuery<CollectionLevel2>($@"select top 1 ParLevel1_Id, ParLevel2_Id, Shift, Period, UnitId  from CollectionLevel2 where Id = { correctiveAction.CollectionLevel02Id }").FirstOrDefault();
                var Auditor = db.UserSgq.Where(r => r.Id == correctiveAction.AuditorId).FirstOrDefault();
                var Slaughter = db.UserSgq.Where(r => r.Id == correctiveAction.AuditorId).FirstOrDefault();
                var Techinical = db.UserSgq.Where(r => r.Id == correctiveAction.TechinicalId).FirstOrDefault();

                correctiveActionDTO.AuditorName = Auditor != null ? Auditor.Name : "";
                correctiveActionDTO.NameSlaughter = Slaughter != null ? Slaughter.FullName : "";
                correctiveActionDTO.NameTechinical = Techinical != null ? Techinical.FullName : "";

                if (collectionLevel2 != null)
                {
                    var parCompany = db.ParCompany.Where(r => r.Id == collectionLevel2.UnitId).FirstOrDefault();
                    var parLevel1 = db.ParLevel1.Where(r => r.Id == collectionLevel2.ParLevel1_Id).FirstOrDefault();
                    var parLevel2 = db.ParLevel2.Where(r => r.Id == collectionLevel2.ParLevel2_Id).FirstOrDefault();

                    correctiveActionDTO.level01Name = parLevel1 != null ? parLevel1.Name : "";
                    correctiveActionDTO.level02Name = parLevel2 != null ? parLevel1.Name : "";

                    correctiveActionDTO.ShiftName = collectionLevel2.Shift.ToString();
                    correctiveActionDTO.PeriodName = collectionLevel2.Period.ToString();

                    if (parCompany != null)
                        correctiveActionDTO.Unit = new UnitDTO() { Code = parCompany.SIF, Name = parCompany.Name };
                }


                return correctiveActionDTO;
            }
        }
    }
}
