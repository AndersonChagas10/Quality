//using DTO;
//using DTO.Helpers;
//using SgqSystem.Helpers;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Globalization;
//using System.Linq;
//using System.Threading;

//namespace SgqSystem.Services
//{
//    public class ServiceLevel2
//    {

//        private SqlConnection db { get; set; }

//        public ServiceLevel2(SqlConnection _db)
//        {
//            db = _db;
//        }


//        #region App

//        public string GetResource()
//        {
//            if (GlobalConfig.Brasil)
//            {
//                Thread.CurrentThread.CurrentCulture = new CultureInfo(Guard.LANGUAGE_PT_BR);
//                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Guard.LANGUAGE_PT_BR);
//            }
//            else
//            {
//                Thread.CurrentThread.CurrentCulture = new CultureInfo("");
//                Thread.CurrentThread.CurrentUICulture = new CultureInfo("");
//            }
//            //setup temporário

//            System.Reflection.Assembly assembly = this.GetType().Assembly;

//            System.Resources.ResourceManager resourceManager = Resources.Resource.ResourceManager;

//            var resourceSet = resourceManager.GetResourceSet(
//                Thread.CurrentThread.CurrentUICulture, true, false);

//            string items = "";

//            foreach (var entry in resourceSet.Cast<DictionaryEntry>())
//            {
//                items += "<div res='" + entry.Key.ToString() + "'>" + entry.Value.ToString() + "</div>";
//            }

//            return "<div class='Resource hide'>" + items + "</div>";
//        }

//        public int getEvaluate(SGQDBContext.ParLevel2 parlevel2, IEnumerable<SGQDBContext.ParLevel2Evaluate> ParEvaluateCompany, IEnumerable<SGQDBContext.ParLevel2Evaluate> ParEvaluatePadrao)
//        {
//            int evaluate = 0;
//            var evaluateConf = ParEvaluateCompany.Where(p => p.Id == parlevel2.Id).FirstOrDefault();
//            if (evaluateConf != null)
//            {
//                evaluate = evaluateConf.Evaluate;
//            }
//            else
//            {
//                evaluateConf = ParEvaluatePadrao.Where(p => p.Id == parlevel2.Id).FirstOrDefault();
//                if (evaluateConf != null)
//                {
//                    evaluate = evaluateConf.Evaluate;
//                }
//            }
//            if (evaluate == 0)
//            {
//                evaluate = 0;
//            }
//            return evaluate;
//        }

//        public int getSample(SGQDBContext.ParLevel2 parlevel2, IEnumerable<SGQDBContext.ParLevel2Sample> ParSampleCompany, IEnumerable<SGQDBContext.ParLevel2Sample> ParSamplePadrao)
//        {
//            int sample = 0;
//            var sampleConf = ParSampleCompany.Where(p => p.Id == parlevel2.Id).FirstOrDefault();
//            if (sampleConf != null)
//            {
//                sample = sampleConf.Sample;
//            }
//            else
//            {
//                sampleConf = ParSamplePadrao.Where(p => p.Id == parlevel2.Id).FirstOrDefault();
//                if (sampleConf != null)
//                {
//                    sample = sampleConf.Sample;
//                }
//            }
//            if (sample == 0)
//            {
//                sample = 0;
//            }
//            return sample;
//        }

//        public string getAPPMain(int UserSgq_Id, int ParCompany_Id, DateTime Date)
//        {
//            #region Antes do loop1

//            var html = new Html();
//            string culture;

//            if (GlobalConfig.Brasil)
//            {
//                culture = "pt-br";
//            }
//            else
//            {
//                culture = "en-us";
//            }

//            string breadCrumb = "<ol class=\"breadcrumb\" breadmainlevel=\"Slaughter\"></ol>";

//            string selectPeriod = html.option("1", CommonData.getResource("period").Value.ToString() + " 1") +
//                              html.option("2", CommonData.getResource("period").Value.ToString() + " 2") +
//                              html.option("3", CommonData.getResource("period").Value.ToString() + " 3") +
//                              html.option("4", CommonData.getResource("period").Value.ToString() + " 4");

//            string hide = string.Empty;
//            if (GlobalConfig.Brasil)
//            {
//                hide = "hide";
//            }

//            selectPeriod = html.select(selectPeriod, id: "period", disabled: true, style: "width: 160px");

//            selectPeriod = "<li class='painel list-group-item " + hide + " '>" + selectPeriod + " </li>";

//            #endregion

//            var seiLaLevel1 = GetLevel01(ParCompany_Id: ParCompany_Id, dateCollect: Date); /****** PORQUE ESTA MOKADO ESSA UNIDADE 1? *******/

//            string container = html.div(outerhtml: breadCrumb + selectPeriod + seiLaLevel1, classe: "container");

//            string buttons = " <button id=\"btnSave\" class=\"btn btn-lg btn-warning hide\"><i id=\"saveIcon\" class=\"fa fa-save\"></i><i id=\"loadIcon\" class=\"fa fa-circle-o-notch fa-spin\" style=\"display:none;\"></i></button><!--Save-->" +
//                             " <button class=\"btn btn-lg btn-danger btnCA hide\">" + CommonData.getResource("corrective_action").Value.ToString() + "</button><!--Corrective Action-->";

//            string message = "<div class=\"message padding20\" style=\"display:none\">                                                                                      " +
//                             "   <h1 class=\"head\">Titulo</h1>                                                                                                           " +
//                             "   <div class=\"body font16\">Mensagem</div>                                                                                                " +
//                             "   <div class=\"foot\"><button id=\"btnMessageOk\" class=\"btn btn-lg marginRight30 btn-primary pull-right btnMessage\"> Ok</button></div>      " +
//                             "</div>                                                                                                                                    ";
//            // string messageConfirm = null;
//            //string viewModal = "<div class=\"viewModal\" style=\"display:none;\">                                                                                                                                                         " +
//            //                       "</div>                                                                                                                                                                                                    ";

//            string viewModal = "<div class=\"viewModal\" style=\"display:none;\">" +
//                               "     <div class=\"head\" style=\"height:35px;line-height:35px;padding-left:10px;padding-right:10px\"><div class=\"title\">View </div><a href=\"#\" class=\"pull-right close\" style=\"color: #000;text-decoration:none\">X</a></div> " +
//                               "     <div class=\"body\" style=\"height:565px; overflow-y: auto;padding-left:5px;padding-right:5px;padding-bottom:5px;\"></div>                                                                           " +
//                               "</div>                                                                                                                                                                                                    ";

//            string modalVF = "<div class=\"modalVF panel panel-primary\" style=\"display:none;\"></div>";

//            string modalPCC1B = "<div class=\"modalPCC1B panel panel-primary\" style=\"display:none;\"></div>";

//            string messageConfirm = "<div class=\"messageConfirm padding20\" style=\"display:none\">                                                                                                " +
//                                        "    <h1 class=\"head\">Titulo</h1>                                                                                                                             " +
//                                        "    <div class=\"body font16\"> <div class=\"txtMessage\"></div>                                                                                               " +
//                                        "        <input type=\"password\" id=\"passMessageComfirm\" placeholder=\"Password\" class=\"form-control input-sm\" style=\"max-width:160px;\" />        " +
//                                        "        <input type=\"text\" masc=\"date\" id=\"inputDate\" placeholder=\"99/99/9999\" class=\"form-control input-sm hide\" style=\"max-width:160px;\" /> </div>       " +
//                                        "    <div class=\"foot\"><button id=\"btnMessageYes\" class=\"btn btn-lg marginRight30 btn-primary pull-right btnMessage\"> " + CommonData.getResource("yes").Value.ToString() + " </button></div>                 " +
//                                        "    <div class=\"foot\"><button id=\"btnMessageNo\" class=\"btn btn-lg marginRight30 btn-primary pull-right btnMessage\"> " + CommonData.getResource("no").Value.ToString() + " </button></div>                   " +
//                                        "</div>                                                                                                                                                         ";

//            //string viewModal = "<div class=\"viewModal\" style=\"display:none;\">                                                                                                                                                       " +
//            //                    "    <div class=\"head\" style=\"height:35px;line-height:35px;padding-left:10px;padding-right:10px\">View <a href=\"#\" class=\"pull-right close\" style=\"color:#000;text-decoration:none\">X</a></div> " +
//            //                    "    <div class=\"body\" style=\"height:565px;overflow-y:auto;padding-left:5px;padding-right:5px;padding-bottom:5px;\"></div>                                                                            " +
//            //                    "</div>       

//            string debug = "<div id = 'ControlaDivDebugAlertas' onclick='showHideDivDebugAlerta();'></div> " +

//                           "<div id = 'divDebugAlertas' > " +
//                           "     <p class='titDebugAlertas'>Acompanhamento do indicador</p> " +
//                           "     Indicador: <div id = 'level1' class='clDebugAlertas'></div> " +
//                           "     <br> " +
//                           "     Volume total do indicador: <div id = 'volumeTotal'  class='clDebugAlertas'></div> " +
//                           "     <br> " +
//                           "     Meta %: <div id = 'meta'  class='clDebugAlertas'></div> " +
//                           "     <br> " +
//                           "     Meta Tolerância: <div id = 'metaTolerancia'  class='clDebugAlertas'></div> " +
//                           "     <br> " +
//                           "     Meta Dia: <div id = 'metaDia'  class='clDebugAlertas'></div> " +
//                           "     <br> " +
//                           "     Meta Avaliação: <div id = 'metaAvaliacao'  class='clDebugAlertas'></div> " +
//                           "     <br> " +
//                           "     Total Avaliado: <div id = 'totalAv'  class='clDebugAlertas'></div> " +
//                           "     <br> " +
//                           "     Total NC: <div id = 'totalNc'  class='clDebugAlertas'></div> " +
//                           "     <br> " +
//                           "     Total NC na avaliação: <div id = 'totalNcNaAvalicao'  class='clDebugAlertas'></div> " +

//                           "</div> " +

//                           "<script> " +

//                           "     $('#ControlaDivDebugAlertas').hide(); " +
//                           "     $('#divDebugAlertas').hide(); " +

//                           " </script> ";

//            debug = "<div id = 'ControlaDivDebugAlertas' style='display:none'></div> " +

//                           "<div id = 'divDebugAlertas'  style='display:none'> " +
//                           "     <p class='titDebugAlertas'>Acompanhamento do indicador</p> " +
//                           "     Indicador: <div id = 'level1' class='clDebugAlertas'></div> " +
//                           "     <br> " +
//                           "     Volume total do indicador: <div id = 'volumeTotal'  class='clDebugAlertas'></div> " +
//                           "     <br> " +
//                           "     Meta %: <div id = 'meta'  class='clDebugAlertas'></div> " +
//                           "     <br> " +
//                           "     Meta Tolerância: <div id = 'metaTolerancia'  class='clDebugAlertas'></div> " +
//                           "     <br> " +
//                           "     Meta Dia: <div id = 'metaDia'  class='clDebugAlertas'></div> " +
//                           "     <br> " +
//                           "     Meta Avaliação: <div id = 'metaAvaliacao'  class='clDebugAlertas'></div> " +
//                           "     <br> " +
//                           "     Total Avaliado: <div id = 'totalAv'  class='clDebugAlertas'></div> " +
//                           "     <br> " +
//                           "     Total NC: <div id = 'totalNc'  class='clDebugAlertas'></div> " +
//                           "     <br> " +
//                           "     Total NC na avaliação: <div id = 'totalNcNaAvalicao'  class='clDebugAlertas'></div> " +

//                           "</div> ";


//            return html.div(
//                            outerhtml: navBar(UserSgq_Id, ParCompany_Id) +
//                                       rightMenu() +
//                                       html.div(classe: "overlay", style: "display:none") +
//                                       container +
//                                       buttons +
//                                       footer(),
//                             classe: "App hide",
//                             tags: "breadmainlevel=\"" + CommonData.getResource("slaughter").Value.ToString() + "\" culture=\"" + culture + "\" turningtime=\"03:00\""
//                           ) +
//                           correctiveAction() +
//                           viewModal +
//                           modalVF +
//                           modalPCC1B +
//                           message +
//                           messageConfirm +
//                           debug;
//        }



//        public string navBar(int UserSgq_Id, int ParCompany_Id)
//        {
//            string navBar = "<div class=\"navbar navbar-inverse navbar-fixed-top\">                                                                                                                             " +
//                           "    <div class=\"container\" style=\"padding: 0px !important;\">                                                                                                                                                       " +
//                           "        <div class=\"navbar-header\" style=\"width: 100%\">                                                                                                                         " +
//                           "            <a class=\"navbar-brand\" id=\"SGQName\" href=\"#\"><i class=\"fa fa-chevron-left hide iconReturn\" style=\"margin-left: 8px; font-size: 24px;\" aria-hidden=\"true\"></i> SGQ </a>                 " +
//                           "            <div class=\"buttonMenu navbar-brand hide\" id=\"btnShowImage\" level01id=\"2\">Show Image</div>                                                                        " +
//                           selectUserCompanys(UserSgq_Id, ParCompany_Id) +
//                           //"            <span style='color: #ffffff; margin: 14px 0px;' class='period'></span> / <span style='color: #ffffff; margin: 14px 0px;' class='shift'>shift</span> " +
//                           "            <span style='color: #ffffff; margin: 14px 0px;' class='shift'>shift</span> " +
//                           "            <div id=\"btnMore\" class=\"iconMoreMenu pull-right\" style=\"padding: 12px; margin-right: 10px\"><i class=\"fa fa-ellipsis-v iconMoreMenu\" aria-hidden=\"true\"></i></div><span id='btnDate' style='color: #ffffff; margin: 14px 0px;' class='atualDate pull-right'></span>" +
//                           "        </div>                                                                                                                                                                      " +
//                           "    </div>                                                                                                                                                                          " +
//                           "</div>                                                                                                                                                                              ";

//            return navBar;
//        }
//        public string rightMenu()
//        {
//            string menu = "<div class=\"rightMenu\">                                                                                                  " +
//                           "     <div class=\"list-group list-group-inverse rightMenuList\">                                                           " +
//                           "         <a href=\"#\" id=\"btnSync\" class=\"list-group-item\" style=\"background-color: black; font-weight: bold;\">" + CommonData.getResource("sync_results").Value.ToString() + "</a>                                                  " +
//                           "         <a href=\"#\" id=\"btnSyncParam\" class=\"list-group-item\"  style=\"background-color: black; font-weight: bold;\">" + CommonData.getResource("sync_parameretrization").Value.ToString() + "</a>                                                  " +

//                           "         <a href=\"#\" id=\"btnLogout\" class=\"list-group-item\">" + CommonData.getResource("logout").Value.ToString() + "</a>                                                     " +
//                           "         <a href=\"#\" id=\"btnLog\" class=\"list-group-item\">" + CommonData.getResource("view_log").Value.ToString() + "</a>                                                      " +
//                           "         <a href=\"#\" id=\"btnCollectDB\" class=\"list-group-item\">" + CommonData.getResource("view_db").Value.ToString() + "</a>                                                 " +
//                           "         <a href=\"#\" id=\"btnClearDatabase\" class=\"list-group-item\">" + CommonData.getResource("clean_db").Value.ToString() + "</a>                                            " +
//                           "         <a href=\"#\" id=\"btnMostrarContadores\" class=\"list-group-item\">" + CommonData.getResource("show_counters").Value.ToString() + "</a>                                   " +
//                           "         <a href=\"#\" id=\"btnAutoSend\" class=\"list-group-item\">" + CommonData.getResource("auto_send_on").Value.ToString() + "</a>                                   " +
//                           "         <span id=\"version\" class=\"list-group-item\">" + CommonData.getResource("version").Value.ToString() + ": <span class=\"number\"></span></span>                           " +
//                           "         <span id=\"ambiente\" class=\"list-group-item\"><span class=\"base\"></span></span>                                                                                        " +
//                           "     </div>                                                                                                                                                                         " +
//                           " </div>                                                                                                                                                                             ";

//            return menu;
//        }
//        public string correctiveAction()
//        {
//            string correctiveAction =
//                "<div id=\"correctiveActionModal\" class=\"container panel panel-default modal-padrao\" style=\"display:none\">" +
//                    "<div class=\"panel-body\">" +
//                        "<div class=\"modal-body\">" +
//                            "<h2>" + CommonData.getResource("corrective_action").Value.ToString() + " </h2>" +
//                            "<div id=\"messageAlert\" class=\"alert alert-info hide\" role=\"alert\">" +
//                                "<span id=\"mensagemAlerta\" class=\"icon-info-sign\"></span>" +
//                            "</div>" +
//                            "<div class=\"row formCorrectiveAction\">" +
//                                "<div class=\"panel panel-default\">" +
//                                    "<div class=\"panel-body\">" +
//                                        "<div class=\"row\">" +
//                                            "<div class=\"col-xs-6\" id=\"CorrectiveActionTaken\">" +
//                                                "<b class=\"font16\">" + CommonData.getResource("corrective_action_taken").Value.ToString() + ":<br/></b>" +
//                                                "<b>" + CommonData.getResource("date_time").Value.ToString() + ":</b> <span id=\"datetime\"></span><br/>" +
//                                                "<b>" + CommonData.getResource("auditor").Value.ToString() + ": </b><span id=\"auditor\"></span><br/>" +
//                                                "<b>" + CommonData.getResource("shift").Value.ToString() + ": </b><span id=\"shift\"></span><br/>" +
//                                            "</div>" +
//                                            "<div class=\"col-xs-6\" id=\"AuditInformation\">" +
//                                                "<b class=\"font16\">" + CommonData.getResource("audit_information").Value.ToString() + ":<br/></b>" +
//                                                "<b>" + CommonData.getResource("slaughter").Value.ToString() + ": </b><span id=\"auditText\"></span><br/>" +
//                                                "<b>" + CommonData.getResource("initial_date").Value.ToString() + ":</b><span id=\"starttime\"></span><br/>" +
//                                                "<b>" + CommonData.getResource("period").Value.ToString() + ":</b><span id=\"correctivePeriod\"></span>" +
//                                            "</div>" +
//                                        "</div>" +
//                                    "</div>" +
//                                "</div>" +
//                                "<div class=\"form-group\">" +
//                                    "<label>" + CommonData.getResource("failure_description").Value.ToString() + ":</label>" +
//                                    "<textarea id=\"DescriptionFailure\" class=\"form-control custom-control\" rows=\"3\" style=\"resize:none\"></textarea>" +
//                                "</div>" +
//                                "<div class=\"form-group\">" +
//                                    "<label>" + CommonData.getResource("immediate_corrective_action").Value.ToString() + ":</label>" +
//                                    "<textarea id=\"ImmediateCorrectiveAction\" class=\"form-control custom-control\" rows=\"3\" style=\"resize:none\"></textarea>" +
//                                "</div>" +
//                                "<div class=\"form-group\">" +
//                                    "<label>" + CommonData.getResource("product_disposition").Value.ToString() + ":</label>" +
//                                    "<textarea id=\"ProductDisposition\" class=\"form-control custom-control\" rows=\"3\" style=\"resize:none\"></textarea>" +
//                                "</div>" +
//                                "<div class=\"form-group\">" +
//                                    "<label>" + CommonData.getResource("preventive_measure").Value.ToString() + ":</label>" +
//                                    "<textarea id=\"PreventativeMeasure\" class=\"form-control custom-control\" rows=\"3\" style=\"resize:none\"></textarea>" +
//                                "</div>";
//            if (GlobalConfig.Eua)
//            {
//                correctiveAction +=
//                                "<div class=\"row\">" +
//                                    "<div class=\"col-xs-6\">" +
//                                        "<div class=\"SlaugtherSignature hide\">" +
//                                            "<h4>Slaughter Signature</h4>" +
//                                            "<div class=\"name\">Admin</div>" +
//                                            "<div class=\"date\">08/24/2016 10:31</div>" +
//                                            "<button class=\"btn btn-link btnSlaugtherSignatureRemove\">" + CommonData.getResource("remove_signature").Value.ToString() + "</button>" +
//                                        "</div>" +
//                                    "</div>" +
//                                    "<div class=\"col-xs-6\">" +
//                                        "<div class=\"TechinicalSignature hide\">" +
//                                            "<h4>Technical Signature</h4>" +
//                                            "<div class=\"name\">Admin2</div>" +
//                                            "<div class=\"date\">08/24/2016</div>" +
//                                            "<button class=\"btn btn-link btnTechinicalSignatureRemove\">" + CommonData.getResource("remove_signature").Value.ToString() + "</button>" +
//                                        "</div>" +
//                                    "</div>" +
//                                "</div>";
//            }


//            correctiveAction +=
//                                "<div class=\"modal-footer\">";

//            if (GlobalConfig.Eua)
//            {
//                correctiveAction +=
//                                    "<span class=\"pull-left\">" +
//                                        "<button class=\"btn btn-default btnSignature btnSlaugtherSignature\">" +
//                                            CommonData.getResource("slaughter_signature").Value.ToString() +
//                                        "</button>" +
//                                        "<button class=\"btn btn-default btnSignature btnTechinicalSignature\">" +
//                                            CommonData.getResource("technical_signature").Value.ToString() +
//                                        "</button>" +
//                                    "</span>";
//            }

//            correctiveAction +=
//                                    "<button class=\"btn btn-danger modal-close-ca\">" + CommonData.getResource("close").Value.ToString() + "</button>" +
//                                    "<button class=\"btn btn-primary\" id=\"btnSendCorrectiveAction\">" + CommonData.getResource("send").Value.ToString() + " </button>" +
//                                "</div>" +
//                            "</div>" +
//                        "</div>" +
//                        "</div>" +
//                    "</div>";

//            if (GlobalConfig.Eua)
//            {
//                correctiveAction +=
//                    "<div id=\"modalSignatureCorrectiveAction\" class=\"panel panel-default modal-padrao signature\" style=\"display:none\">" +
//                        "<div class=\"panel-body\">" +
//                            "<div class=\"modal-header\">" +
//                                "<h3 class=\"slaughtersig head hide\">" + CommonData.getResource("slaughter_signature").Value.ToString() + " </h3>" +
//                                "<h3 class=\"techinicalsig head hide\">" + CommonData.getResource("technical_signature").Value.ToString() + " </h3>" +
//                                "<div id=\"messageAlert\" class=\"alert alert-info hide\">" +
//                                    "<span id=\"mensagemAlerta\" class=\"icon-info-sign\"></span>" +
//                                "</div>" +
//                            "</div>" +
//                            "<div class=\"modal-body\">" +
//                                "<div class=\"row\">" +
//                                    "<div class=\"col-xs-12\">" +
//                                        "<div class=\"form-group\">" +
//                                            "<label>" + CommonData.getResource("login").Value.ToString() + ":</label>" +
//                                            "<input type=\"text\" class=\"form-control\" id=\"signatureLogin\">" +
//                                        "</div>" +
//                                        "<div class=\"form-group\">" +
//                                            "<label>" + CommonData.getResource("password").Value.ToString() + ":</label>" +
//                                            "<input type=\"password\" class=\"form-control\" id=\"signaturePassword\">" +
//                                        "</div>" +
//                                        "<div id=\"messageError\" class=\"alert alert-danger hide\">" +
//                                            "<span class=\"icon-remove-sign\"></span><strong>" + CommonData.getResource("error").Value.ToString() + "! </strong><span id=\"mensagemErro\"> </span>" +
//                                        "</div>" +
//                                        "<div id=\"messageAlert\" class=\"alert alert-info hide\">" +
//                                            "<span id=\"mensagemAlerta\" class=\"icon-info-sign\"></span>" +
//                                        "</div>" +
//                                        "<div id=\"messageSuccess\" class=\"alert alert-success hide\">" +
//                                            "<span id=\"mensagemSucesso\" class=\"icon-ok-circle\"></span>" +
//                                        "</div>" +
//                                    "</div>" +
//                                "</div>" +
//                            "</div>" +
//                            "<div class=\"modal-footer\">" +
//                                "<button class=\"btn btn-danger modal-close-signature\">" + CommonData.getResource("close").Value.ToString() + " </button>" +
//                                "<button type=\"button\" class=\"btn btn-primary\" id=\"btnSignatureLogin\">" + CommonData.getResource("sign").Value.ToString() + " </button>" +
//                            "</div>" +
//                        "</div>" +
//                    "</div>" +
//                "</div>";
//            }

//            return correctiveAction;
//        }

//        public string footer()
//        {
//            string foot = "<footer class=\"footer\">                                                                                                                                       " +
//                          "   <p style=\"color:white; margin-left:16px; margin-right:16px; margin-top: 12px;\">                                                                      " +
//                          "       <span class=\"user\">Admin</span> - <span class=\"unit\">Colorado</span> | <span class=\"urlPrefix\"></span>                                          " +
//                          "       <span class=\"status pull-right\"></span>                                                                                                          " +
//                          "   </p>                                                                                                                                                   " +
//                          "</footer>                                                                                                                                                 ";

//            return foot;
//        }
//        /// <summary>
//        /// Recupera Level1 e seus monitoramentos e tarefas relacionados
//        /// </summary>
//        /// <returns></returns>
//        public string GetLevel01(int ParCompany_Id, DateTime dateCollect)
//        {

//            #region Parametros do level 1 e "instancias"

//            ///SE NÃO HOUVER NENHUM LEVEL1, LEVEL2, LEVEL3 INFORMAR QUE NÃO ENCONTROU MONITORAMENTOS
//            var html = new Html();

//            //Instanciamos a Classe ParLevel01 Dapper
//            var ParLevel1DB = new SGQDBContext.ParLevel1(db);
//            var ParCounterDB = new SGQDBContext.ParCounter(db);
//            //Inicaliza ParLevel1VariableProduction
//            var ParLevel1VariableProductionDB = new SGQDBContext.ParLevel1VariableProduction(db);
//            var ParRelapseDB = new SGQDBContext.ParRelapse(db);

//            //Buscamos os ParLevel11 para a unidade selecionada
//            var parLevel1List = ParLevel1DB.getParLevel1ParCriticalLevelList(ParCompany_Id: ParCompany_Id);

//            //Agrupamos o ParLevel1 por ParCriticalLevel
//            var parLevel1GroupByCriticalLevel = parLevel1List.OrderBy(p => p.ParCriticalLevel_Id).GroupBy(p => p.ParCriticalLevel_Id);

//            //Instanciamos uma variável para não gerenciar a utilizar do ParCriticalLevel
//            bool ParCriticalLevel = false;

//            //Instanciamos uma variável para instanciar a lista de level1, level2 e level3
//            //Esses itens podem ser transformados funções menores
//            string listlevel1 = null;
//            string listLevel2 = null;
//            string listLevel3 = null;

//            string excecao = null;
//            #endregion

//            //Percorremos a lista de agrupada
//            foreach (var parLevel1Group in parLevel1GroupByCriticalLevel) //LOOP1
//            {

//                #region instancia

//                //Instanciamos uma variável level01GroupList
//                string level01GroupList = null;
//                //Instanciamos uma variável list parLevel1 para adicionar os parLevel1
//                string parLevel1 = null;
//                //Instanciamos uma variável para verificar o nome do ParCriticalLevel
//                string nameParCritialLevel = null;
//                //Percorremos a Lista dos Agrupamento 

//                #endregion

//                foreach (var parlevel1 in parLevel1Group) //LOOP2
//                {

//                    #region 1 monte de coisa que aparentemente roda rapido....

//                    string tipoTela = "";

//                    var variableList = ParLevel1VariableProductionDB.getVariable(parlevel1.Id).ToList();

//                    if (variableList.Count > 0)
//                    {
//                        tipoTela = variableList[0].Name;
//                    }
//                    //Se o ParLevel1 contem um ParCritialLevel_Id
//                    var ParLevel1AlertasDB = new SGQDBContext.ParLevel1Alertas(db);
//                    var alertas = ParLevel1AlertasDB.getAlertas(parlevel1.Id, ParCompany_Id, dateCollect);

//                    if (parlevel1.ParCriticalLevel_Id > 0)
//                    {
//                        //O ParLevel1 vai estar dentro de um accordon
//                        ParCriticalLevel = true;
//                        //Pego o nome do ParCriticalLevel para não precisar fazer outra pesquisa
//                        nameParCritialLevel = parlevel1.ParCriticalLevel_Name;
//                        //Incremento os itens que estaram no ParLevel1                
//                        //Gera linha Level1

//                        decimal tipoAlerta = parlevel1.tipoAlerta;
//                        decimal valorAlerta = parlevel1.valorAlerta;

//                        decimal alertaNivel1 = 0;
//                        decimal alertaNivel2 = 0;
//                        string alertaNivel3 = "";

//                        decimal volumeAlerta = 0;
//                        decimal meta = 0;

//                        if (tipoAlerta == 1) //JBS por Indicador
//                        {
//                            if (alertas != null)
//                            {
//                                alertaNivel1 = alertas.Nivel1;
//                                alertaNivel2 = alertas.Nivel2;
//                                alertaNivel3 = "a1";
//                                volumeAlerta = alertas.VolumeAlerta;
//                                meta = alertas.Meta;
//                            }
//                        }
//                        else if (tipoAlerta == 2)  //# de NC
//                        {
//                            if (alertas != null)
//                            {
//                                alertaNivel1 = valorAlerta;
//                                alertaNivel2 = valorAlerta;
//                                alertaNivel3 = "a2";
//                                volumeAlerta = alertas.VolumeAlerta;
//                                meta = alertas.Meta;
//                            }
//                        }
//                        else if (tipoAlerta == 3)  //% de NC
//                        {
//                            if (alertas != null)
//                            {
//                                alertaNivel1 = valorAlerta;
//                                alertaNivel2 = valorAlerta;
//                                alertaNivel3 = "a3";
//                                volumeAlerta = alertas.VolumeAlerta;
//                                meta = alertas.Meta;
//                            }
//                        }
//                        else if (tipoAlerta == 4)  //JBS por Monitoramento
//                        {
//                            if (alertas != null)
//                            {
//                                alertaNivel1 = alertas.Nivel1;
//                                alertaNivel2 = alertas.Nivel2;
//                                alertaNivel3 = "a4";
//                                volumeAlerta = alertas.VolumeAlerta;
//                                meta = alertas.Meta;
//                            }
//                        }
//                        else
//                        {
//                            if (alertas != null) //Fica como padrão JBS por indicador
//                            {
//                                alertaNivel1 = alertas.Nivel1;
//                                alertaNivel2 = alertas.Nivel2;
//                                alertaNivel3 = "a0";
//                                volumeAlerta = alertas.VolumeAlerta;
//                                meta = alertas.Meta;
//                            }
//                        }

//                        var listCounter = ParCounterDB.GetParLevelXParCounterList(parlevel1.Id, 0, 1, "level1_line");

//                        string painelCounters = "";

//                        if (listCounter != null)
//                        {
//                            painelCounters = html.painelCounters(listCounter, "margin-top: 40px;font-size: 12px;");
//                        }

//                        if (GlobalConfig.Eua && parlevel1.Name.Contains("CFF"))
//                        {
//                            tipoTela = "CFF";
//                        }

//                        var listParRelapse = ParRelapseDB.getRelapses(parlevel1.Id);

//                        string level01 = html.level1(parlevel1,
//                                                     tipoTela: tipoTela,
//                                                     totalAvaliado: 0,
//                                                     totalDefeitos: 0,
//                                                     alertNivel1: alertaNivel1,
//                                                     alertNivel2: alertaNivel2,
//                                                     alertaNivel3: alertaNivel3,
//                                                     numeroAvaliacoes: 0,
//                                                     metaDia: alertaNivel1 * 3,
//                                                     metaTolerancia: alertaNivel1,
//                                                     metaAvaliacao: 0,
//                                                     alertaAtual: 0,
//                                                     avaliacaoultimoalerta: 0,
//                                                     monitoramentoultimoalerta: 0,
//                                                     volumeAlertaIndicador: volumeAlerta,
//                                                     metaIndicador: meta,
//                                                     IsLimitedEvaluetionNumber: parlevel1.IsLimitedEvaluetionNumber,
//                                                     listParRelapse: listParRelapse);
//                        //Incrementa level1
//                        parLevel1 += html.listgroupItem(parlevel1.Id.ToString(), classe: "row " + excecao, outerhtml: level01 + painelCounters);
//                    }
//                    else
//                    {
//                        //Caso o ParLevel1 não contenha um ParCritialLevel_Id apenas incremento os itens de ParLevel1
//                        parLevel1 += html.listgroupItem(parlevel1.Id.ToString(), outerhtml: parlevel1.Name, classe: excecao);
//                    }
//                    //Instancia variável para receber todos os level3
//                    string level3Group = null;

//                    #endregion

//                    //Busca os Level2 e reforna no level3Group;
//                    listLevel2 += GetLevel02(parlevel1, ParCompany_Id, dateCollect, ref level3Group);

//                    //Incrementa Level3Group
//                    listLevel3 += level3Group;
//                }
//                //Quando termina o loop dos itens agrupados por ParCritialLevel 
//                //Se contem ParCritialLevel

//                if (ParCriticalLevel == true)
//                {
//                    Html.bootstrapcolor? color = null;
//                    if (parLevel1Group.Key == 1)
//                    {
//                        color = Html.bootstrapcolor.danger;
//                    }
//                    else if (parLevel1Group.Key == 2)
//                    {
//                        color = Html.bootstrapcolor.warning;
//                    }
//                    else if (parLevel1Group.Key == 3)
//                    {
//                        color = Html.bootstrapcolor.info;
//                    }
//                    //Adicionamos os itens em um acordeon
//                    parLevel1 = html.accordeon(
//                                                id: parLevel1Group.Key.ToString() + "critivalLevel",
//                                                label: nameParCritialLevel,
//                                                color: color,
//                                                outerhtml: parLevel1,
//                                                aberto: true);
//                }
//                else
//                {
//                    //Adicionamos os itens e um listgroup
//                    level01GroupList = html.listgroup(
//                                                   outerhtml: parLevel1
//                                                );
//                }
//                //Adicionar a lista de level01 agrupados ou não a lsita geral
//                listlevel1 += parLevel1;
//            }
//            //Retona as lista
//            //Podemos gerar uma verificação de atualizações
//            return html.div(
//                            outerhtml: listlevel1,
//                            classe: "level1List"
//                            ) +
//                   html.div(
//                            outerhtml: listLevel2,
//                            classe: "level2List col-xs-12 hide"
//                           ) +
//                   html.div(
//                            outerhtml: listLevel3,
//                            classe: "level3List  List col-xs-12 hide"
//                           );

//        }
//        /// <summary>
//        /// Gera Linhas do level2
//        /// </summary>
//        /// <param name="ParLevel1"></param>
//        /// <param name="ParCompany_Id"></param>
//        /// <param name="level3Group"></param>
//        /// <returns></returns>
//        public string GetLevel02(SGQDBContext.ParLevel1 ParLevel1, int ParCompany_Id, DateTime dateCollect, ref string level3Group)
//        {

//            #region Parametros e "Instancias"

//            //Inicializa ParLevel2
//            var ParLevel2DB = new SGQDBContext.ParLevel2(db);
//            var ParCounterDB = new SGQDBContext.ParCounter(db);
//            //Pega uma lista de ParLevel2
//            //Tem que confirmar a company e colocar na query dentro do método, ainda não foi validado
//            var parlevel02List = ParLevel2DB.getLevel2ByIdLevel1(ParLevel1.Id, ParCompany_Id);

//            //Inicializa Cabecalhos
//            var ParLevelHeaderDB = new SGQDBContext.ParLevelHeader(db);
//            //Inicaliza ParFieldType
//            var ParFieldTypeDB = new SGQDBContext.ParFieldType(db);
//            var ParNCRuleDB = new SGQDBContext.NotConformityRule(db);

//            var reauditFlag = "<li class='painel row list-group-item hide reauditFlag'> Reaudit <span class='reauditnumber'></span></li>";

//            var html = new Html();

//            //Instancia parLevel2List
//            string ParLevel2List = null;
//            //Instancia headerlist
//            string headerList = null;

//            //Inicializa Avaliações e Amostras
//            var ParEvaluateDB = new SGQDBContext.ParLevel2Evaluate(db);
//            var ParSampleDB = new SGQDBContext.ParLevel2Sample(db);

//            //Verifica avaliações padrão
//            var ParEvaluatePadrao = ParEvaluateDB.getEvaluate(ParLevel1: ParLevel1,
//                                                              ParCompany_Id: null);

//            //Verifica avaliações pela company informada
//            var ParEvaluateCompany = ParEvaluateDB.getEvaluate(ParLevel1: ParLevel1,
//                                                               ParCompany_Id: ParCompany_Id);

//            //Verifia amostra padrão
//            var ParSamplePadrao = ParSampleDB.getSample(ParLevel1: ParLevel1,
//                                                        ParCompany_Id: null);

//            //Verifica amostra pela company informada
//            var ParSampleCompany = ParSampleDB.getSample(ParLevel1: ParLevel1,
//                                                        ParCompany_Id: ParCompany_Id);

//            //Variaveis para avaliação de grupos
//            int evaluateGroup = 0;
//            int sampleGroup = 0;

//            string groupLevel3Level2 = null;
//            string painelLevel3 = null;

//            #endregion

//            //Enquando houver lista de level2
//            foreach (var parlevel2 in parlevel02List) //LOOP3
//            {
//                //Verifica se pega avaliações e amostras padrão ou da company
//                int evaluate = getEvaluate(parlevel2, ParEvaluateCompany, ParEvaluatePadrao);
//                int sample = getSample(parlevel2, ParSampleCompany, ParSamplePadrao);

//                //Se agrupar level2 com level3 pego o valor da primeira avaliação e amostra
//                if (ParLevel1.HasGroupLevel2 == true & evaluateGroup == 0)
//                {
//                    evaluateGroup = evaluate;
//                    sampleGroup = sample;
//                }

//                //Colocar função de gerar cabeçalhos por selectbox
//                //Monta os cabecalhos
//                #region Cabecalhos e Contadores
//                string headerCounter =
//                                     html.div(
//                                               outerhtml: "<b>" + CommonData.getResource("ev").Value.ToString() + " </b>",
//                                               classe: "col-xs-6",
//                                               style: "text-align:center"
//                                             ) +
//                                     html.div(
//                                               outerhtml: "<b>" + CommonData.getResource("sd").Value.ToString() + " </b>",
//                                               classe: "col-xs-6",
//                                               style: "text-align:center"
//                                              );
//                //+
//                //                    html.div(
//                //                        outerhtml: "<b>Def.</b>",
//                //                        classe: "col-xs-3",
//                //                        style: "text-align:center"
//                //                    ) +
//                //                    html.div(
//                //                        outerhtml: "<b></b>",
//                //                        classe: "col-xs-3",
//                //                        style: "text-align:center"
//                //                    );

//                //**inserir contadores

//                headerCounter = html.div(
//                                    //aqui vai os botoes
//                                    outerhtml: headerCounter,
//                                    classe: "counters col-xs-4"
//                                    );


//                string classXSLevel2 = " col-xs-5";

//                int totalSampleXEvaluate = evaluate * sample;

//                string counters =
//                                      html.div(
//                                                outerhtml: html.span(outerhtml: "0", classe: "evaluateCurrent") + html.span(outerhtml: " / ", classe: "separator") + html.span(outerhtml: evaluate.ToString(), classe: "evaluateTotal"),
//                                                classe: "col-xs-6",
//                                                style: "text-align:center"
//                                              ) +
//                                      html.div(
//                                                outerhtml: html.span(outerhtml: "0", classe: "sampleCurrent hide") + html.span(outerhtml: "0", classe: "sampleCurrentTotal") + html.span(outerhtml: " / ", classe: "separator") + html.span(outerhtml: sample.ToString(), classe: "sampleTotal hide") + html.span(outerhtml: totalSampleXEvaluate.ToString(), classe: "sampleXEvaluateTotal"),
//                                                classe: "col-xs-6",
//                                                style: "text-align:center"
//                                              );
//                //+
//                //                        html.div(
//                //                                    outerhtml: html.span(outerhtml: "0", classe: "defectsLevel2"),
//                //                                    classe: "col-xs-3",
//                //                                    style: "text-align:center"
//                //                                 ) +
//                //                          html.div(
//                //                                    outerhtml: html.span(outerhtml: "", classe: "newcoutner"),
//                //                                    classe: "col-xs-3",
//                //                                    style: "text-align:center"
//                //                                 );

//                counters = html.div(
//                                    //aqui vai os botoes
//                                    outerhtml: counters,
//                                    classe: "counters col-xs-4"
//                                    );

//                #endregion
//                string buttons = null;
//                string buttonsHeaders = null;
//                //Caso tenha funções de não aplicado, coloca os botões nas respectivas linhas
//                //Como vai ficar se o item tem varias avaliações?vai ter botão salvar na linha do monitoramento?
//                if (ParLevel1.HasNoApplicableLevel2 == true || ParLevel1.HasSaveLevel2 == true)
//                {
//                    string btnNotAvaliable = null;
//                    if (ParLevel1.HasNoApplicableLevel2)
//                    {
//                        btnNotAvaliable = "<button class=\"btn btn-warning btnNotAvaliableLevel2 na\"> " +
//                                           "   <span class=\"cursorPointer iconsArea\">N/A</span> " +
//                                           "</button>                                             ";
//                    }
//                    string btnAreaSave = null;
//                    if (ParLevel1.HasSaveLevel2)
//                    {
//                        btnAreaSave = "<button class=\"btn btn-success hide btnAreaSaveConfirm\">                                                    " +
//                                       "   <span class=\"cursorPointer\"><i class=\"fa fa-check\" aria-hidden=\"true\"></i></span>     " +
//                                       "</button>                                                                                                      " +
//                                       "<button class=\"btn btn-primary btnAreaSave\">                                                                 " +
//                                       "   <span class=\"cursorPointer iconsArea\"><i class=\"fa fa-floppy-o\" aria-hidden=\"true\"></i></span>        " +
//                                       "</button>                                                                                                      ";
//                    }
//                    string btnReaudit = null;
//                    if (parlevel2.IsReaudit)
//                    {
//                        btnReaudit = "<button class=\"btn btn-primary hide btnReaudit\"> " +
//                                      "<span>R</span></button>";
//                    }
//                    buttons = html.div(
//                                 //aqui vai os botoes
//                                 outerhtml: btnReaudit +
//                                            btnAreaSave +
//                                            btnNotAvaliable,
//                                 style: "text-align: right",
//                                 classe: "userInfo col-xs-3"
//                                 );

//                    buttonsHeaders = html.div(
//                                             outerhtml: null,
//                                             classe: "userInfo col-xs-3"
//                                             );
//                }
//                else
//                {
//                    classXSLevel2 = " col-xs-8";
//                    string btnReaudit = null;
//                    if (parlevel2.IsReaudit)
//                    {
//                        btnReaudit = "<button class=\"btn btn-primary hide btnReaudit\"> " +
//                                      "<span>R</span></button>";

//                        buttons = html.div(
//                                     //aqui vai os botoes
//                                     outerhtml: btnReaudit,
//                                     style: "text-align: right",
//                                     classe: "userInfo col-xs-2"
//                                     );

//                        //classXSLevel2 = " col-xs-6";
//                    }

//                }

//                string level02Header = html.div(classe: classXSLevel2) +
//                                       headerCounter +
//                                       buttonsHeaders;

//                headerList = html.listgroupItem(
//                                                classe: "row",
//                                                outerhtml: level02Header
//                                               );

//                var parNCRuleDB = ParNCRuleDB.getParNCRule(parlevel2.ParNotConformityRule_id, parlevel2.Id);
//                decimal ruleValue = 0;

//                if (parNCRuleDB != null)
//                {
//                    ruleValue = parNCRuleDB.Value;
//                }

//                //podemos aplicar os defeitos
//                string level2 = html.level2(id: parlevel2.Id.ToString(),
//                                            label: parlevel2.Name,
//                                            classe: classXSLevel2,
//                                            evaluate: evaluate,
//                                            sample: sample,
//                                            HasSampleTotal: parlevel2.HasSampleTotal,
//                                            IsEmptyLevel3: parlevel2.IsEmptyLevel3,
//                                            RuleId: parlevel2.ParNotConformityRule_id,
//                                            RuleValue: ruleValue.ToString(),
//                                            reaudit: parlevel2.IsReaudit);

//                var listLineCounter = ParCounterDB.GetParLevelXParCounterList(0, parlevel2.Id, 2, "level2_line");

//                string lineCounters = "";

//                if (listLineCounter != null)
//                {
//                    lineCounters = html.painelCounters(listLineCounter, "margin-top: 45px;font-size: 12px;");
//                }

//                //Gera linha do Level2
//                ParLevel2List += html.listgroupItem(
//                                                    id: parlevel2.Id.ToString(),
//                                                    classe: "row",
//                                                    outerhtml: level2 +
//                                                               counters +
//                                                               buttons +
//                                                               html.div(classe: "level2Debug") +
//                                                               lineCounters
//                                                    );


//                //Gera monitoramento do level3
//                string groupLevel3 = GetLevel03(ParLevel1, parlevel2, ParCompany_Id, dateCollect, ref painelLevel3);

//                if (ParLevel1.HasGroupLevel2 == true)
//                {
//                    var othersTags = "defects=\"" + 1 +
//                           "\" evaluate=\"" + evaluate +
//                           "\" sample=\"" + sample +
//                           "\" weievaluation=\"0" +
//                           "\" evaluatetotal=\"0" +
//                           "\" defectstotal=\"0\" weidefects=\"0\"" +
//                           " totallevel3evaluation=\"0\"" +
//                           " totallevel3withdefects=\"0\"" +
//                           " hassampletotal=\"" + parlevel2.HasSampleTotal.ToString().ToLower() + "\"" +
//                           " isemptylevel3=\"" + parlevel2.IsEmptyLevel3.ToString().ToLower()
//                           + "\" ParNotConformityRule_id=\"" + parlevel2.ParNotConformityRule_id
//                           + "\" ParNotConformityRule_value=\"" + ruleValue.ToString()
//                           + "\" AlertValue=\"" + 0
//                           + "\" reaudit=\"" + parlevel2.IsReaudit.ToString().ToLower() + "\"";

//                    groupLevel3 = html.accordeon(
//                                                    id: parlevel2.Id.ToString(),
//                                                    label: parlevel2.Name,
//                                                    classe: "level2 row",
//                                                    outerhtml: groupLevel3,
//                                                    accordeonId: parlevel2.Id,
//                                                    othersTags: othersTags
//                                                );

//                    groupLevel3Level2 += groupLevel3;
//                }
//                else
//                {
//                    level3Group += groupLevel3;
//                }

//            }

//            //Se tiver agrupamentos no ParLevel1
//            if (ParLevel1.HasGroupLevel2 == true)
//            {
//                string parLevel3Group = null;


//                string accordeonbuttons = null;

//                accordeonbuttons = "<button class=\"btn btn-default button-expand marginRight10\"><i class=\"fa fa-expand\" aria-hidden=\"true\"></i> Mostrar Todos</button>" +
//                                   "<button class=\"btn btn-default button-collapse\"><i class=\"fa fa-compress\" aria-hidden=\"true\"></i> Fechar Todos</button>";


//                //painellevel3 = html.listgroupItem(
//                //                                            outerhtml: avaliacoes +
//                //                                                       amostras +
//                //                                                       painelLevel3HeaderListHtml,

//                //                               classe: "painel painelLevel03 row");



//                string panelAccordeon = html.listgroupItem(
//                                                           outerhtml: accordeonbuttons,
//                                                           classe: "painel painelLevel02 row"
//                                                        );


//                if (!string.IsNullOrEmpty(groupLevel3Level2))
//                {
//                    parLevel3Group = html.div(
//                                               classe: "level3Group",
//                                               tags: "level1idgroup=\"" + ParLevel1.Id + "\"",

//                                               outerhtml: painelLevel3 + panelAccordeon +
//                                                          groupLevel3Level2
//                                             );

//                    level3Group += parLevel3Group;
//                }

//                headerList = null;
//                string level2 = html.level2(id: "0",
//                                            label: ParLevel1.Name,
//                                            classe: "group col-xs-12",
//                                            evaluate: evaluateGroup,
//                                            sample: sampleGroup,
//                                            HasSampleTotal: false,
//                                            IsEmptyLevel3: false,
//                                            level1Group_Id: ParLevel1.Id);

//                //Gera linha do Level2
//                ParLevel2List = html.listgroupItem(
//                                                    id: ParLevel1.Id.ToString(),
//                                                    classe: "row",
//                                                    outerhtml: level2 +
//                                                               null +
//                                                               null +
//                                                               html.div(classe: "level2Debug")
//                                                    );
//            }

//            //aqui tem que fazer a pesquisa se tem itens sao do level1 ex: cca,htp
//            //quando tiver cabecalhos tem que replicar no level1

//            ParLevel2List = headerList +
//                            ParLevel2List;

//            var painelLevel2HeaderListHtml = GetHeaderHtml(ParLevelHeaderDB.getHeaderByLevel1(ParLevel1.Id), ParFieldTypeDB, html);


//            if (!string.IsNullOrEmpty(painelLevel2HeaderListHtml))
//            {
//                painelLevel2HeaderListHtml = html.listgroupItem(
//                                                                outerhtml: painelLevel2HeaderListHtml,
//                                                                classe: "row painelLevel02"
//                                                                );
//            }

//            var listCounter = ParCounterDB.GetParLevelXParCounterList(ParLevel1.Id, 0, 1, "level2_header");

//            string painelCounters = "";

//            if (listCounter != null)
//            {
//                painelCounters = html.painelCounters(listCounter);
//            }

//            //Se contem  monitoramentos
//            if (!string.IsNullOrEmpty(ParLevel2List))
//            {
//                //Gera agrupamento dw Level2 para o Level1
//                ParLevel2List = html.listgroup(
//                                                outerhtml: reauditFlag +
//                                                           painelLevel2HeaderListHtml +
//                                                           painelCounters +
//                                                           ParLevel2List,
//                                                tags: "level01Id=\"" + ParLevel1.Id + "\""
//                                               , classe: "level2Group hide");
//            }

//            return ParLevel2List;
//        }

//        public string GetHeaderHtml(IEnumerable<ParLevelHeader> list, ParFieldType ParFieldTypeDB, Html html, int ParLevel1_Id = 0, int ParLevel2_Id = 0, ParLevelHeader ParLevelHeaderDB = null, int ParCompany_id = 0)
//        {
//            string retorno = "";

//            foreach (var header in list) //LOOP7
//            {

//                #region MyRegion

//                if (ParLevel1_Id > 0 && ParLevel2_Id > 0 && ParLevelHeaderDB != null)
//                {
//                    if (ParLevelHeaderDB.isHeaderLeve2Exception(ParLevel1_Id, ParLevel2_Id, header.ParHeaderField_Id))
//                    {
//                        continue;
//                    }
//                }

//                var label = "<label class=\"font-small\">" + header.ParHeaderField_Name + "</label>";

//                var form_control = "";

//                #endregion

//                #region Switch com Loop
//                //ParFieldType 
//                switch (header.ParFieldType_Id)
//                {
//                    //Multipla Escolha
//                    case 1:
//                        var listMultiple = ParFieldTypeDB.getMultipleValues(header.ParHeaderField_Id);
//                        var optionsMultiple = "";
//                        bool hasDefault = false;
//                        foreach (var value in listMultiple) //LOOP8
//                        {
//                            if (value.IsDefaultOption == 1)
//                            {
//                                optionsMultiple += "<option selected=\"selected\" value=\"" + value.Id + "\" PunishmentValue=\"" + value.PunishmentValue + "\">" + value.Name + "</option>";
//                                hasDefault = true;
//                            }
//                            else
//                            {
//                                optionsMultiple += "<option value=\"" + value.Id + "\" PunishmentValue=\"" + value.PunishmentValue + "\">" + value.Name + "</option>";
//                            }
//                        }
//                        if (!hasDefault)
//                            optionsMultiple = "<option selected=\"selected\" value=\"0\">" + CommonData.getResource("select").Value.ToString() + "...</option>" + optionsMultiple;

//                        form_control = "<select class=\"form-control input-sm\" ParHeaderField_Id=\"" + header.ParHeaderField_Id + "\" ParFieldType_Id=\"" + header.ParFieldType_Id + "\">" + optionsMultiple + "</select>";
//                        break;
//                    //Integrações
//                    case 2:
//                        var listIntegration = ParFieldTypeDB.getIntegrationValues(header.ParHeaderField_Id, header.ParHeaderField_Description, ParCompany_id);
//                        var optionsIntegration = "";
//                        bool hasDefaultIntegration = false;
//                        foreach (var value in listIntegration) //LOOP8
//                        {
//                            if (value.IsDefaultOption == 1)
//                            {
//                                optionsIntegration += "<option selected=\"selected\" value=\"" + value.Id + "\" PunishmentValue=\"0\">" + value.Name + "</option>";
//                                hasDefaultIntegration = true;
//                            }
//                            else
//                            {
//                                optionsIntegration += "<option value=\"" + value.Id + "\" PunishmentValue=\"0\">" + value.Name + "</option>";
//                            }
//                        }
//                        if (!hasDefaultIntegration)
//                            optionsIntegration = "<option selected=\"selected\" value=\"0\">" + CommonData.getResource("select").Value.ToString() + "...</option>" + optionsIntegration;

//                        form_control = "<select class=\"form-control input-sm\" ParHeaderField_Id=\"" + header.ParHeaderField_Id + "\" ParFieldType_Id=\"" + header.ParFieldType_Id + "\">" + optionsIntegration + "</select>";
//                        break;
//                        break;
//                    //Binário
//                    case 3:
//                        var listBinario = ParFieldTypeDB.getMultipleValues(header.ParHeaderField_Id);
//                        var optionsBinario = "";
//                        foreach (var value in listBinario) //LOOP8
//                        {
//                            if (listBinario.ElementAt(0) == value)
//                            {
//                                optionsBinario += "<option selected value=\"" + value.Id + "\" PunishmentValue=\"" + value.PunishmentValue + "\">" + value.Name + "</option>";
//                            }
//                            else
//                            {
//                                optionsBinario += "<option value=\"" + value.Id + "\" PunishmentValue=\"" + value.PunishmentValue + "\">" + value.Name + "</option>";
//                            }
//                        }
//                        form_control = "<select class=\"form-control input-sm\" ParHeaderField_Id='" + header.ParHeaderField_Id + "' ParFieldType_Id = '" + header.ParFieldType_Id + "'>" + optionsBinario + "</select>";
//                        break;
//                    //Texto
//                    case 4:
//                        form_control = "<input class=\"form-control input-sm\" type=\"text\" ParHeaderField_Id=\"" + header.ParHeaderField_Id + "\" ParFieldType_Id=\"" + header.ParFieldType_Id + "\">";
//                        break;
//                    //Numérico
//                    case 5:
//                        form_control = "<input class=\"form-control input-sm\" type=\"number\" ParHeaderField_Id=\"" + header.ParHeaderField_Id + "\" ParFieldType_Id=\"" + header.ParFieldType_Id + "\">";
//                        break;
//                    //Data
//                    case 6:
//                        form_control = "<input class=\"form-control input-sm\" type=\"date\" ParHeaderField_Id=\"" + header.ParHeaderField_Id + "\" ParFieldType_Id=\"" + header.ParFieldType_Id + "\">";
//                        break;

//                    //Hora
//                    case 7:
//                        form_control = "<input class=\"form-control input-sm\" type=\"time\" ParHeaderField_Id=\"" + header.ParHeaderField_Id + "\" ParFieldType_Id=\"" + header.ParFieldType_Id + "\">";
//                        break;
//                }

//                var form_group = html.div(
//                                            outerhtml: label + form_control,
//                                            classe: "form-group header",
//                                            tags: header.IsRequired == 1 ? "required" : "",
//                                            style: "margin-bottom: 4px;"
//                                            );

//                retorno += html.div(
//                                            outerhtml: form_group,
//                                            classe: "col-xs-6 col-sm-4 col-md-3 col-lg-2",
//                                            style: "padding-right: 4px !important; padding-left: 4px !important;"
//                                            );


//                #endregion

//            }

//            return retorno;
//        }
//        /// <summary>
//        /// Retorna Level3 
//        /// </summary>
//        /// <param name="ParLevel1"></param>
//        /// <param name="ParLevel2"></param>
//        /// <returns></returns>
//        public string GetLevel03(SGQDBContext.ParLevel1 ParLevel1, SGQDBContext.ParLevel2 ParLevel2, int ParCompany_Id, DateTime dateCollect, ref string painellevel3)
//        {
//            var html = new Html();

//            var reauditFlag = "<li class='painel row list-group-item hide reauditFlag'> Reaudit <span class='reauditnumber'></span></li>";

//            //Inicializa ParLevel3
//            var ParLevel3DB = new SGQDBContext.ParLevel3(db);

//            var ParCounterDB = new SGQDBContext.ParCounter(db);

//            //Inicializa Cabecalhos
//            var ParLevelHeaderDB = new SGQDBContext.ParLevelHeader(db);
//            //Inicaliza ParFieldType
//            var ParFieldTypeDB = new SGQDBContext.ParFieldType(db);
//            //Inicaliza ParLevel1VariableProduction
//            var ParLevel1VariableProductionDB = new SGQDBContext.ParLevel1VariableProduction(db);

//            //Pega uma lista de parleve3
//            //pode colocar par level3 por unidades, como nos eua
//            var parlevel3List = ParLevel3DB.getLevel3ByLevel2(ParLevel1, ParLevel2, ParCompany_Id, dateCollect);

//            string tipoTela = "";

//            var variableList = ParLevel1VariableProductionDB.getVariable(ParLevel1.Id).ToList();

//            var listCounter = ParCounterDB.GetParLevelXParCounterList(0, ParLevel2.Id, 2, "level3_header").ToList();
//            listCounter.AddRange(ParCounterDB.GetParLevelXParCounterList(ParLevel1.Id, 0, 1, "level3_header").ToList());

//            if (variableList.Count > 0)
//            {
//                tipoTela = variableList[0].Name;
//            }
//            string btnNaoAvaliado = html.button(
//                                    label: html.span(
//                                                 classe: "cursorPointer iconsArea",
//                                                 outerhtml: "N/A"
//                                             ),
//                                    classe: "btn-warning btnNotAvaliable na font11"
//                                );


//            bool haveAccordeon = false;

//            int Last_Id = 0;
//            //Tela de bem estar animal
//            if (tipoTela.Equals("BEA"))
//            {
//                #region MyRegion

//                //Instancia uma veriavel para gerar o agrupamento
//                string parLevel3Group = null;

//                foreach (var parLevel3 in parlevel3List) //LOOP4
//                {

//                    if (Last_Id != parLevel3.Id)
//                    {
//                        //Define a qual classe de input pertence o level3
//                        string classInput = null;
//                        //Labels que mostrar informaçãoes do tipo de input
//                        string labelsInputs = null;
//                        //tipo de input
//                        string input = getTipoInputBEA(parLevel3, ref classInput, ref labelsInputs);

//                        string level3List = html.level3(parLevel3, input, classInput, labelsInputs);
//                        parLevel3Group += level3List;

//                        Last_Id = parLevel3.Id;

//                    }
//                }

//                //Avaliações e amostas para painel
//                string avaliacoeshtml = html.div(
//                                    outerhtml: "<label class=\"font-small\" style=\"display:inherit\">" + CommonData.getResource("evaluation").Value.ToString() + " </label><label style=\"display:inline-block; font-size: 20px;\">" + html.span(classe: "evaluateCurrent") + html.span(outerhtml: " / ", classe: "separator") + html.span(classe: "evaluateTotal") + "</label>",
//                                    style: "margin-bottom: 4px;",
//                                    classe: "form-group");
//                string amostrashtml = html.div(
//                                    outerhtml: "<label class=\"font-small\" style=\"display:inherit\">" + CommonData.getResource("samples").Value.ToString() + " </label><label style=\"display:inline-block; font-size: 20px;\">" + html.span(classe: "sampleCurrent") + html.span(outerhtml: " / ", classe: "separator") + html.span(classe: "sampleTotal") + "</label>",
//                                    style: "margin-bottom: 4px;",
//                                    classe: "form-group");

//                string avaliacoes = html.div(
//                                    outerhtml: avaliacoeshtml,
//                                    style: "padding-right: 4px !important; padding-left: 4px !important;",
//                                    classe: "col-xs-6 col-sm-4 col-md-3 col-lg-2");
//                string amostras = html.div(
//                                    outerhtml: amostrashtml,
//                                    style: "padding-right: 4px !important; padding-left: 4px !important;",
//                                    classe: "col-xs-6 col-sm-4 col-md-3 col-lg-2");

//                //Painel
//                //O interessante é um painel só mas no momento está um painel para cada level3group

//                var painelLevel3HeaderListHtml = "";

//                var labelPecas = "<label class='font-small'>Animais Avaliados</label>";
//                var formControlPecas = "<input class='form-control input-sm pecasAvaliadas' type='number'>";
//                var formGroupPecas = html.div(
//                                        outerhtml: labelPecas + formControlPecas,
//                                        classe: "form-group",
//                                        style: "margin-bottom: 4px;"
//                                        );

//                painelLevel3HeaderListHtml += html.div(
//                                                outerhtml: formGroupPecas,
//                                                classe: "col-xs-6 col-sm-4 col-md-3 col-lg-2",
//                                                style: "padding-right: 4px !important; padding-left: 4px !important;"
//                                                );

//                //string HeaderLevel02 = null;
//                painellevel3 = html.listgroupItem(
//                                                     outerhtml: avaliacoes +
//                                                                amostras +
//                                                                painelLevel3HeaderListHtml,

//                                        classe: "painel painelLevel03 row") +
//                              html.painelCounters(listCounter);
//                //          +
//                //html.div(outerhtml: "teste", classe: "painel counters row", style: "background-color: #ff0000");

//                //Se tiver level3 gera o agrupamento no padrão
//                if (!string.IsNullOrEmpty(parLevel3Group))
//                {
//                    parLevel3Group = html.div(
//                                               classe: "level3Group BEA",
//                                               tags: "level1id=\"" + ParLevel1.Id + "\" level2id=\"" + ParLevel2.Id + "\"",

//                                               outerhtml: painellevel3 +
//                                                          parLevel3Group
//                                             );
//                }
//                return parLevel3Group;

//                #endregion
//            }
//            //Tela da verificação da tipificação
//            else if (tipoTela.Equals("VF"))
//            {
//                #region MyRegion
//                //Inicaliza CaracteristicaTipificacao
//                var CaracteristicaTipificacaoDB = new SGQDBContext.CaracteristicaTipificacao(db);
//                //Inicaliza VerificacaoTipificacaoTarefaIntegracao
//                var VerificacaoTipificacaoTarefaIntegracaoDB = new SGQDBContext.VerificacaoTipificacaoTarefaIntegracao(db);

//                //Instancia uma veriavel para gerar o agrupamento
//                string parLevel3Group = null;

//                foreach (var parLevel3 in parlevel3List) // //LOOP4
//                {
//                    if (Last_Id != parLevel3.Id)
//                    {
//                        string tags = null;
//                        string labels = null;

//                        //Gera o level3
//                        string level3 = html.link(
//                                                    outerhtml: html.span(outerhtml: parLevel3.Name, classe: "levelName"),
//                                                    classe: "col-xs-12 col-sm-12 col-md-12"
//                                                    );

//                        #region Switch parLevel3.Name

//                        switch (parLevel3.Name)
//                        {
//                            case "Verificação Tipificação - Falha Operacional":
//                                var listOper = CaracteristicaTipificacaoDB.getCaracteristicasTipificacao(206);
//                                var listOperHtml = "";
//                                foreach (var carac in listOper)
//                                {
//                                    listOperHtml += "<div class='col-xs-2 hide' cNmCaracteristica='" +
//                                                     carac.cNmCaracteristica + "' cIdentificador='" + carac.cIdentificador + "' " +
//                                                     " cNrCaracteristica='" + carac.cNrCaracteristica + "' cSgCaracteristica='" + carac.cSgCaracteristica + "'>" +
//                                                     carac.cSgCaracteristica + "</div>"; ;
//                                }
//                                var CtIdOpe = CaracteristicaTipificacaoDB.getCaracteristicasTipificacaoUnico(206).First().nCdCaracteristica;
//                                var TIdOpe = VerificacaoTipificacaoTarefaIntegracaoDB.getTarefa(Convert.ToInt32(CtIdOpe)).First().TarefaId;
//                                labels += html.div(outerhtml: listOperHtml, classe: "row items", name: "Falha Op.", tags: "listtype = multiple caracteristicatipificacaoid=" + CtIdOpe + " tarefaid=" + TIdOpe);
//                                break;
//                            case "Verificação Tipificação - Gordura":
//                                var listGordura = CaracteristicaTipificacaoDB.getCaracteristicasTipificacao(203);
//                                var listGorduraHtml = "";
//                                foreach (var carac in listGordura)
//                                {
//                                    listGorduraHtml += "<div class='col-xs-2 hide' cNmCaracteristica='" +
//                                                        carac.cNmCaracteristica + "' cIdentificador='" + carac.cIdentificador + "' " +
//                                                        " cNrCaracteristica='" + carac.cNrCaracteristica + "' cSgCaracteristica='" + carac.cSgCaracteristica + "'>" +
//                                                        carac.cSgCaracteristica + "</div>"; ;
//                                }
//                                var CtIdGor = CaracteristicaTipificacaoDB.getCaracteristicasTipificacaoUnico(203).First().nCdCaracteristica;
//                                var TIdGor = VerificacaoTipificacaoTarefaIntegracaoDB.getTarefa(Convert.ToInt32(CtIdGor)).First().TarefaId;
//                                labels += html.div(outerhtml: listGorduraHtml, classe: "row items", name: "Gordura", tags: "listtype = single caracteristicatipificacaoid=" + CtIdGor + " tarefaid=" + TIdGor);
//                                break;
//                            case "Verificação Tipificação - Contusão":
//                                var listContusao = CaracteristicaTipificacaoDB.getCaracteristicasTipificacao(205);
//                                var listContusaoHtml = "";
//                                foreach (var carac in listContusao)
//                                {
//                                    listContusaoHtml += "<div class='col-xs-2 hide' cNmCaracteristica='" +
//                                                        carac.cNmCaracteristica + "' cIdentificador='" + carac.cIdentificador + "' " +
//                                                        " cNrCaracteristica='" + carac.cNrCaracteristica + "' cSgCaracteristica='" + carac.cSgCaracteristica + "'>" +
//                                                        carac.cSgCaracteristica + "</div>"; ;
//                                }
//                                var CtIdCon = CaracteristicaTipificacaoDB.getCaracteristicasTipificacaoUnico(205).First().nCdCaracteristica;
//                                var TIdCon = VerificacaoTipificacaoTarefaIntegracaoDB.getTarefa(Convert.ToInt32(CtIdCon)).First().TarefaId;
//                                labels += html.div(outerhtml: listContusaoHtml, classe: "row items", name: "Contusão", tags: "listtype = multiple caracteristicatipificacaoid=" + CtIdCon + " tarefaid=" + TIdCon);
//                                break;
//                            case "Verificação Tipificação - Idade":
//                                var listIdade = CaracteristicaTipificacaoDB.getCaracteristicasTipificacao(201);
//                                var listIdadeHtml = "";
//                                foreach (var carac in listIdade)
//                                {
//                                    listIdadeHtml += "<div class='col-xs-2 hide' cNmCaracteristica='" +
//                                                        carac.cNmCaracteristica + "' cIdentificador='" + carac.cIdentificador + "' " +
//                                                        " cNrCaracteristica='" + carac.cNrCaracteristica + "' cSgCaracteristica='" + carac.cSgCaracteristica + "'>" +
//                                                        carac.cSgCaracteristica + "</div>"; ;
//                                }
//                                var CtIdIdade = CaracteristicaTipificacaoDB.getCaracteristicasTipificacaoUnico(201).First().nCdCaracteristica;
//                                var TIdIdade = VerificacaoTipificacaoTarefaIntegracaoDB.getTarefa(Convert.ToInt32(CtIdIdade)).First().TarefaId;
//                                labels += html.div(outerhtml: listIdadeHtml, classe: "row items", name: "Maturidade", tags: "listtype = single caracteristicatipificacaoid=" + CtIdIdade + " tarefaid=" + TIdIdade);
//                                break;
//                            case "Verificação Tipificação - Sexo":
//                                var listSexo = CaracteristicaTipificacaoDB.getCaracteristicasTipificacao(207);
//                                var listSexoHtml = "";
//                                foreach (var carac in listSexo)
//                                {
//                                    listSexoHtml += "<div class='col-xs-2 hide' cNmCaracteristica='" +
//                                                    carac.cNmCaracteristica + "' cIdentificador='" + carac.cIdentificador + "' " +
//                                                    " cNrCaracteristica='" + carac.cNrCaracteristica + "' cSgCaracteristica='" + carac.cSgCaracteristica + "'>" +
//                                                    carac.cSgCaracteristica + "</div>"; ;
//                                }
//                                var CtIdSexo = CaracteristicaTipificacaoDB.getCaracteristicasTipificacaoUnico(207).First().nCdCaracteristica;
//                                var TIdSexo = VerificacaoTipificacaoTarefaIntegracaoDB.getTarefa(Convert.ToInt32(CtIdSexo)).First().TarefaId;
//                                labels += html.div(outerhtml: listSexoHtml, classe: "row items", name: "Sexo", tags: "listtype = single caracteristicatipificacaoid=" + CtIdSexo + " tarefaid=" + TIdSexo);
//                                break;
//                        }

//                        #endregion

//                        //gera os labels
//                        labels = html.div(
//                                                outerhtml: labels,
//                                                classe: "col-xs-12 col-sm-12 col-md-12"
//                                            );

//                        //Comandos para intervalos
//                        //tags += " weight=\"" + parLevel3.Weight + "\" intervalmin=\"" + parLevel3.IntervalMin + "\" intervalmax=\"" + parLevel3.IntervalMax + "\"";
//                        tags += " weight=\"" + parLevel3.Weight + "\" intervalmin=\"" + parLevel3.IntervalMin + "\" intervalmax=\"" + parLevel3.IntervalMax + "\" weievaluation=\"0\" inputtype=\"1\"";
//                        //Gera uma linha de level3
//                        string level3List = html.listgroupItem(
//                                                                id: parLevel3.Id.ToString(),
//                                                                classe: "level3 row VF",
//                                                                tags: tags,
//                                                                outerhtml: level3 +
//                                                                            labels
//                                                            );

//                        parLevel3Group += level3List;
//                        Last_Id = parLevel3.Id;

//                    }
//                }

//                var listAreasParticipantes = CaracteristicaTipificacaoDB.getAreasParticipantes();
//                var items = "";

//                foreach (var area in listAreasParticipantes) //LOOP5
//                {
//                    items += "<div class='col-xs-3 hide' cNmCaracteristica='" + area.cNmCaracteristica + "' cIdentificador='" + area.cIdentificador + "' " +
//                            " cNrCaracteristica='" + area.cNrCaracteristica + "' cSgCaracteristica='" + area.cSgCaracteristica + "'>" +
//                            area.cNmCaracteristica + "</div>";
//                }

//                var CtIdAP = CaracteristicaTipificacaoDB.getAreasParticipantesUnico().First().nCdCaracteristica;
//                var TIdAP = VerificacaoTipificacaoTarefaIntegracaoDB.getTarefa(Convert.ToInt32(CtIdAP)).First().TarefaId;
//                var areasParticipantes = html.listgroupItem(
//                                                id: "400",
//                                                classe: "level3 row VF",
//                                                tags: "listtype=multiple",
//                                                outerhtml: html.link(
//                                                                outerhtml: html.span(outerhtml: "Areas Participantes", classe: "levelName"),
//                                                                classe: "col-xs-12 col-sm-12 col-md-12"
//                                                                ) +
//                                                           html.div(
//                                                                outerhtml: html.div(outerhtml: items, classe: "items row", name: "Areas Participantes", tags: "listtype = multiple caracteristicatipificacaoid=" + CtIdAP + " tarefaid=" + TIdAP),
//                                                                classe: "col-xs-12 col-sm-12 col-md-12"
//                                                                )
//                                            );

//                parLevel3Group = areasParticipantes + parLevel3Group;

//                var painelLevel3HeaderListHtml = "";

//                var labelSequencial = "<label class='font-small'>Sequencial</label>";
//                var formControlSequencial = "<input class='form-control input-sm sequencial' style='font-size:30px; height: 50px; text-align:center;' type='number'>";
//                var formGroupSequencial = html.div(
//                                        outerhtml: labelSequencial + formControlSequencial,
//                                        classe: "form-group",
//                                        style: "margin-bottom: 4px;"
//                                        );

//                var labelBanda = "<label class='font-small'>Banda</label>";

//                ////var formControlBanda = "<input class='form-control input-sm banda' style='font-size:30px; height: 50px; text-align:center;' type='number'>";

//                //var formControlBanda = "<select class='form-control input-sm banda' style='font-size:30px; height: 50px; text-align:center;'><option value = '1'>1</option><option value='2'>2</option></select>";


//                var formControlBanda = "<input class='form-control input-sm banda' min='1' max='2' style='font-size:30px; height: 50px; text-align:center;' type='number'>";
//                var formGroupBanda = html.div(
//                                        outerhtml: labelBanda + formControlBanda,
//                                        classe: "form-group",
//                                        style: "margin-bottom: 4px;"
//                                        );

//                painelLevel3HeaderListHtml += html.div(
//                                                outerhtml: formGroupSequencial,
//                                                classe: "col-xs-6 col-sm-4 col-md-3 col-lg-2",
//                                                style: "padding-right: 4px !important; padding-left: 4px !important;"
//                                                );

//                painelLevel3HeaderListHtml += html.div(
//                                                outerhtml: formGroupBanda,
//                                                classe: "col-xs-6 col-sm-4 col-md-3 col-lg-2",
//                                                style: "padding-right: 4px !important; padding-left: 4px !important;"
//                                                );

//                //Avaliações e amostas para painel
//                string avaliacoeshtml = html.div(
//                                    outerhtml: "<label class=\"font-small\" style=\"display:inherit\">" + CommonData.getResource("evaluation").Value.ToString() + " </label><label style=\"display:inline-block; font-size: 20px;\">" + html.span(classe: "evaluateCurrent") + html.span(outerhtml: " / ", classe: "separator") + html.span(classe: "evaluateTotal") + "</label>",
//                                    style: "margin-bottom: 4px;",
//                                    classe: "form-group");
//                string amostrashtml = html.div(
//                                    outerhtml: "<label class=\"font-small\" style=\"display:inherit\">" + CommonData.getResource("samples").Value.ToString() + " </label><label style=\"display:inline-block; font-size: 20px;\">" + html.span(classe: "sampleCurrent") + html.span(outerhtml: " / ", classe: "separator") + html.span(classe: "sampleTotal") + "</label>",
//                                    style: "margin-bottom: 4px;",
//                                    classe: "form-group");

//                string avaliacoes = html.div(
//                                    outerhtml: avaliacoeshtml,
//                                    style: "padding-right: 4px !important; padding-left: 4px !important;",
//                                    classe: "col-xs-6 col-sm-4 col-md-3 col-lg-2");
//                string amostras = html.div(
//                                    outerhtml: amostrashtml,
//                                    style: "padding-right: 4px !important; padding-left: 4px !important;",
//                                    classe: "col-xs-6 col-sm-4 col-md-3 col-lg-2");

//                //Painel
//                //O interessante é um painel só mas no momento está um painel para cada level3group

//                painellevel3 = html.listgroupItem(
//                                                            outerhtml: avaliacoes +
//                                                                       amostras +
//                                                                       painelLevel3HeaderListHtml,

//                                               classe: "painel painelLevel03 row") +

//                               html.painelCounters(listCounter);
//                //+
//                //                html.div(outerhtml: "teste", classe: "painel counters row", style: "background-color: #ff0000");

//                //Se tiver level3 gera o agrupamento no padrão
//                if (!string.IsNullOrEmpty(parLevel3Group) && ParLevel1.HasGroupLevel2 != true)
//                {
//                    parLevel3Group = html.div(
//                                               classe: "level3Group VF",
//                                               tags: "level1id=\"" + ParLevel1.Id + "\" level2id=\"" + ParLevel2.Id + "\"",

//                                                   outerhtml: painellevel3 +
//                                                              parLevel3Group
//                                                 );
//                }
//                return parLevel3Group;
//                #endregion
//            }
//            //Tela do PCC1B
//            else if (tipoTela.Equals("PCC1B"))
//            {
//                #region MyRegion
//                //Instancia uma veriavel para gerar o agrupamento
//                string parLevel3Group = null;

//                foreach (var parLevel3 in parlevel3List) //LOOP4
//                {
//                    if (Last_Id != parLevel3.Id)
//                    {
//                        //Define a qual classe de input pertence o level3
//                        string classInput = null;
//                        //Labels que mostrar informaçãoes do tipo de input
//                        string labelsInputs = null;
//                        //tipo de input
//                        string input = getTipoInput(parLevel3, ref classInput, ref labelsInputs);

//                        string level3List = html.level3(parLevel3, input, classInput, labelsInputs);
//                        parLevel3Group += level3List;
//                        Last_Id = parLevel3.Id;
//                    }
//                }

//                //Avaliações e amostas para painel

//                var painelLevel3HeaderListHtml = "";

//                var labelSequencial = "<label class='font-small'>Sequencial</label>";
//                var formControlSequencial = "<input class='form-control input-sm sequencial' style='font-size:100px; height: 150px; text-align:center;' type='number'>";
//                var formGroupSequencial = html.div(
//                                        outerhtml: labelSequencial + formControlSequencial,
//                                        classe: "form-group",
//                                        style: "margin-bottom: 4px;"
//                                        );

//                var labelBanda = "<label class='font-small'>Banda</label>";
//                var formControlBanda = "<select class='form-control input-sm banda' style='font-size:100px; height: 150px; text-align:center;'><option value='1'>1</option><option value='2'>2</option></select>";
//                var formGroupBanda = html.div(
//                                        outerhtml: labelBanda + formControlBanda,
//                                        classe: "form-group",
//                                        style: "margin-bottom: 4px;"
//                                        );

//                painelLevel3HeaderListHtml += html.div(
//                                                outerhtml: formGroupSequencial,
//                                                classe: "col-xs-8 col-sm-6 col-md-6 col-lg-6",
//                                                style: "padding-right: 4px !important; padding-left: 4px !important;"
//                                                );

//                painelLevel3HeaderListHtml += html.div(
//                                                outerhtml: formGroupBanda,
//                                                classe: "col-xs-2 col-sm-2 col-md-2 col-lg-2",
//                                                style: "padding-right: 4px !important; padding-left: 4px !important;"
//                                                );

//                var button = html.button(classe: "btn btn-lg btn-success pull-right", label: "<i class='fa fa-bookmark' aria-hidden='true'></i>");

//                painelLevel3HeaderListHtml += html.div(
//                                                outerhtml: button,
//                                                classe: "col-xs-2 col-sm-4 col-md-4 col-lg-4",
//                                                style: "padding-right: 4px !important; padding-left: 4px !important;"
//                                                );

//                //Avaliações e amostas para painel
//                string totalnchtml = html.div(
//                                    outerhtml: "<label class=\"font-small text-center\" style=\"display:inherit\">Total NC</label><label class='text-center' style=\"display:inherit; font-size: 20px;\">" + html.span(classe: "totalnc") + "</label>",
//                                    style: "margin-bottom: 4px;",
//                                    classe: "form-group");

//                string ncdianteirohtml = html.div(
//                                    outerhtml: "<label class=\"font-small text-center\" style=\"display:inherit\">NC Dianteiro</label><label class='text-center' style=\"display:inherit; font-size: 20px;\">" + html.span(classe: "ncdianteiro") + "</label>",
//                                    style: "margin-bottom: 4px;",
//                                    classe: "form-group");

//                string nctraseirohtml = html.div(
//                                    outerhtml: "<label class=\"font-small text-center\" style=\"display:inherit\">NC Traseiro</label><label class='text-center' style=\"display:inherit; font-size: 20px;\">" + html.span(classe: "nctraseiro") + "</label>",
//                                    style: "margin-bottom: 4px;",
//                                    classe: "form-group");

//                string niveishtml = html.div(
//                                    outerhtml: "<label class=\"font-small text-center\" style=\"display:inherit\">Níveis</label><label class='text-center' style=\"display:inherit; font-size: 20px;\">" + html.span(classe: "nivel1") + "  -  " + html.span(classe: "nivel2") + "  -  " + html.span(classe: "nivel3") + "</label>",
//                                    style: "margin-bottom: 4px;",
//                                    classe: "form-group");

//                string totalnc = html.div(
//                                    outerhtml: totalnchtml,
//                                    style: "padding-right: 4px !important; padding-left: 4px !important;",
//                                    classe: "col-xs-2");

//                string ncdianteiro = html.div(
//                                    outerhtml: ncdianteirohtml,
//                                    style: "padding-right: 4px !important; padding-left: 4px !important;",
//                                    classe: "col-xs-2");

//                string nctraseiro = html.div(
//                                    outerhtml: nctraseirohtml,
//                                    style: "padding-right: 4px !important; padding-left: 4px !important;",
//                                    classe: "col-xs-2");

//                string niveis = html.div(
//                                    outerhtml: niveishtml,
//                                    style: "padding-right: 4px !important; padding-left: 4px !important;",
//                                    classe: "col-xs-6");

//                string avaliacoeshtml = html.div(
//                                    outerhtml: "<label class=\"font-small\" style=\"display:inherit\">" + CommonData.getResource("evaluation").Value.ToString() + " </label><label style=\"display:inline-block; font-size: 20px;\">" + html.span(classe: "evaluateCurrent") + html.span(outerhtml: " / ", classe: "separator") + html.span(classe: "evaluateTotal") + "</label>",
//                                    style: "margin-bottom: 4px;",
//                                    classe: "form-group");
//                string amostrashtml = html.div(
//                                    outerhtml: "<label class=\"font-small\" style=\"display:inherit\">" + CommonData.getResource("samples").Value.ToString() + " </label><label style=\"display:inline-block; font-size: 20px;\">" + html.span(classe: "sampleCurrent") + html.span(outerhtml: " / ", classe: "separator") + html.span(classe: "sampleTotal") + "</label>",
//                                    style: "margin-bottom: 4px;",
//                                    classe: "form-group");

//                string avaliacoes = html.div(
//                                    outerhtml: avaliacoeshtml,
//                                    style: "padding-right: 4px !important; padding-left: 4px !important;",
//                                    classe: "col-xs-6 col-sm-4 col-md-3 col-lg-2 hide");
//                string amostras = html.div(
//                                    outerhtml: amostrashtml,
//                                    style: "padding-right: 4px !important; padding-left: 4px !important;",
//                                    classe: "col-xs-6 col-sm-4 col-md-3 col-lg-2 hide");

//                painellevel3 = html.listgroupItem(
//                                                            outerhtml: amostras + avaliacoes + totalnc + ncdianteiro + nctraseiro + niveis + painelLevel3HeaderListHtml,
//                                               classe: "painel painelLevel03 row") +

//                              html.painelCounters(listCounter);
//                //+
//                //                  +
//                //html.div(outerhtml: "teste", classe: "painel counters row", style: "background-color: #ff0000");

//                //Se tiver level3 gera o agrupamento no padrão
//                if (!string.IsNullOrEmpty(parLevel3Group))
//                {
//                    parLevel3Group = html.div(
//                                               classe: "level3Group PCC1B",
//                                               tags: "level1id=\"" + ParLevel1.Id + "\" level2id=\"" + ParLevel2.Id + "\"",

//                                               outerhtml: painellevel3 +
//                                                          parLevel3Group
//                                             );
//                }
//                return parLevel3Group;
//                #endregion
//            }
//            //Tela Genérica
//            else
//            {
//                //Instancia uma veriavel para gerar o agrupamento
//                string parLevel3Group = null;

//                var parlevel3GroupByLevel2 = parlevel3List.GroupBy(p => p.ParLevel3Group_Id);

//                foreach (var parLevel3GroupLevel2 in parlevel3GroupByLevel2)//LOOP4
//                {
//                    string accordeonName = null;
//                    string acoordeonId = null;
//                    string level3Group = null;

//                    foreach (var parLevel3 in parLevel3GroupLevel2)//LOOP5
//                    {

//                        if (Last_Id != parLevel3.Id)
//                        {

//                            if (parLevel3.ParLevel3Group_Id > 0)
//                            {
//                                accordeonName = parLevel3.ParLevel3Group_Name;
//                                acoordeonId = parLevel3.ParLevel3Group_Id.ToString() + ParLevel2.Id.ToString();
//                            }

//                            //Define a qual classe de input pertence o level3
//                            string classInput = null;
//                            //Labels que mostrar informaçãoes do tipo de input
//                            string labelsInputs = null;
//                            //tipo de input
//                            string input = getTipoInput(parLevel3, ref classInput, ref labelsInputs);

//                            string level3List = html.level3(parLevel3, input, classInput, labelsInputs);
//                            level3Group += level3List;

//                            Last_Id = parLevel3.Id;
//                        }
//                    }

//                    if (!string.IsNullOrEmpty(acoordeonId))
//                    {
//                        haveAccordeon = true;
//                        level3Group = html.accordeon(
//                                                        id: acoordeonId + "Level3",
//                                                        label: accordeonName,
//                                                        outerhtml: level3Group,
//                                                        classe: "row"
//                                                    );
//                    }

//                    //*inserir contador

//                    parLevel3Group += level3Group;

//                }

//                //< div class="form-group">
//                //      <label for="email" style="
//                //    display: inherit;
//                //">Email:</label>
//                //      <label for="email" style="display: inline-block">Email:</label>
//                //    </div>

//                //Avaliações e amostas para painel
//                string avaliacoeshtml = html.div(
//                                    outerhtml: "<label class=\"font-small\" style=\"display:inherit\">" + CommonData.getResource("evaluation").Value.ToString() + " </label><label style=\"display:inline-block; font-size: 20px;\">" + html.span(classe: "evaluateCurrent") + html.span(outerhtml: " / ", classe: "separator") + html.span(classe: "evaluateTotal") + "</label>",
//                                    style: "margin-bottom: 4px;",
//                                    classe: "form-group");
//                string amostrashtml = html.div(
//                                    outerhtml: "<label class=\"font-small\" style=\"display:inherit\">" + CommonData.getResource("samples").Value.ToString() + " </label><label style=\"display:inline-block; font-size: 20px;\">" + html.span(classe: "sampleCurrent") + html.span(outerhtml: " / ", classe: "separator") + html.span(classe: "sampleTotal") + "</label>",
//                                    style: "margin-bottom: 4px;",
//                                    classe: "form-group");

//                string avaliacoes = html.div(
//                                    outerhtml: avaliacoeshtml,
//                                    style: "padding-right: 4px !important; padding-left: 4px !important;",
//                                    classe: "col-xs-6 col-sm-4 col-md-3 col-lg-2");
//                string amostras = html.div(
//                                    outerhtml: amostrashtml,
//                                    style: "padding-right: 4px !important; padding-left: 4px !important;",
//                                    classe: "col-xs-6 col-sm-4 col-md-3 col-lg-2");

//                //Painel
//                //O interessante é um painel só mas no momento está um painel para cada level3group

//                var painelLevel3HeaderListHtml = GetHeaderHtml(
//                    ParLevelHeaderDB.getHeaderByLevel1Level2(ParLevel1.Id, ParLevel2.Id), ParFieldTypeDB, html, ParLevel1.Id, ParLevel2.Id, ParLevelHeaderDB, ParCompany_Id);

//                //string HeaderLevel02 = null;

//                string accordeonbuttons = null;
//                if (haveAccordeon == true)
//                {
//                    accordeonbuttons = "<button class=\"btn btn-default button-expand marginRight10\"><i class=\"fa fa-expand\" aria-hidden=\"true\"></i> Mostrar Todos</button>" +
//                                       "<button class=\"btn btn-default button-collapse\"><i class=\"fa fa-compress\" aria-hidden=\"true\"></i> Fechar Todos</button>";
//                }

//                painellevel3 = html.listgroupItem(
//                                                            outerhtml: avaliacoes +
//                                                                       amostras +
//                                                                       painelLevel3HeaderListHtml,

//                                               classe: "painel painelLevel03 row") +
//                              html.painelCounters(listCounter);
//                //+
//                //                                html.div(outerhtml: "teste", classe: "painel counters row", style: "background-color: #ff0000");

//                var botoesTodos = "";



//                string panelButton = html.listgroupItem(
//                                                        outerhtml: botoesTodos,
//                                                        classe: "painel row"
//                                                    );

//                //Se tiver level3 gera o agrupamento no padrão
//                if (!string.IsNullOrEmpty(parLevel3Group) && ParLevel1.HasGroupLevel2 != true)
//                {
//                    parLevel3Group = html.div(
//                                               classe: "level3Group",
//                                               tags: "level1id=\"" + ParLevel1.Id + "\" level2id=\"" + ParLevel2.Id + "\"",

//                                               outerhtml: reauditFlag +
//                                                          painellevel3 + panelButton +
//                                                          parLevel3Group
//                                             );
//                }
//                return parLevel3Group;
//            }


//        }
//        /// <summary>
//        /// Gera o input para level3
//        /// </summary>
//        /// <param name="parLevel3">ParLevel3</param>
//        /// <param name="classInput">Classe de Input</param>
//        /// <param name="labels">Labels do Input</param>
//        /// <returns></returns>
//        public string getTipoInput(SGQDBContext.ParLevel3 parLevel3, ref string classInput, ref string labels)
//        {
//            var html = new Html();
//            string input = null;
//            if (parLevel3.ParLevel3InputType_Id == 1)
//            {
//                classInput = " boolean";
//                input = html.campoBinario(parLevel3.Id.ToString(), parLevel3.ParLevel3BoolTrue_Name, parLevel3.ParLevel3BoolFalse_Name);
//            }
//            else if (parLevel3.ParLevel3InputType_Id == 2)
//            {
//                classInput = " defects";
//                labels = html.div(
//                                           outerhtml: "<b>Max: </b>" + parLevel3.IntervalMax.ToString("G29"),
//                                           classe: "font10",
//                                           style: "font-size: 11px; margin-top:7px;"
//                                       );

//                input = html.campoNumeroDeDefeitos(id: parLevel3.Id.ToString(),
//                                                intervalMin: parLevel3.IntervalMin,
//                                                intervalMax: parLevel3.IntervalMax,
//                                                unitName: parLevel3.ParMeasurementUnit_Name);
//            }
//            else if (parLevel3.ParLevel3InputType_Id == 3)
//            {
//                classInput = " interval";
//                labels = html.div(
//                                           outerhtml: "<b>Min: </b>" + parLevel3.IntervalMin.ToString("G29") + " ~ <b>Max: </b>" + parLevel3.IntervalMax.ToString("G29") + " " + parLevel3.ParMeasurementUnit_Name,
//                                           classe: "font10",
//                                           style: "font-size: 11px; margin-top:7px;"
//                                       );

//                input = html.campoIntervalo(id: parLevel3.Id.ToString(),
//                                                intervalMin: parLevel3.IntervalMin,
//                                                intervalMax: parLevel3.IntervalMax,
//                                                unitName: parLevel3.ParMeasurementUnit_Name);
//            }
//            else if (parLevel3.ParLevel3InputType_Id == 4)
//            {
//                classInput = " calculado";

//                var intervalMin = Guard.ConverteValorCalculado(parLevel3.IntervalMin);
//                var intervalMax = Guard.ConverteValorCalculado(parLevel3.IntervalMax);

//                labels = html.div(
//                                           outerhtml: "<b>Min: </b> " + Guard.ConverteValorCalculado(parLevel3.IntervalMin) + " ~ <b>Max: </b>" + Guard.ConverteValorCalculado(parLevel3.IntervalMax) + " " + parLevel3.ParMeasurementUnit_Name,
//                                           classe: "font10",
//                                           style: "font-size: 11px; margin-top:7px;"
//                                       );

//                input = html.campoCalculado(id: parLevel3.Id.ToString(),
//                                                intervalMin: parLevel3.IntervalMin,
//                                                intervalMax: parLevel3.IntervalMax,
//                                                unitName: parLevel3.ParMeasurementUnit_Name);
//            }
//            else if (parLevel3.ParLevel3InputType_Id == 5)
//            {
//                classInput = " texto naoValidarInput";
//                labels = html.div(
//                                           outerhtml: "",
//                                           classe: "font10",
//                                           style: "font-size: 11px; margin-top:7px;"
//                                       );

//                input = html.campoTexto(id: parLevel3.Id.ToString());
//            }
//            else
//            {
//                ///Campo interval está repetindo , falta o campo defeitos
//                classInput = " interval";

//                labels = html.div(
//                                    outerhtml: "<b>Min: </b>" + parLevel3.IntervalMin.ToString("G29") + " ~ <b>Max: </b>" + parLevel3.IntervalMax.ToString("G29") + " " + parLevel3.ParMeasurementUnit_Name,
//                                    classe: "font10",
//                                    style: "font-size: 11px; margin-top:7px;"
//                                );

//                input = html.campoIntervalo(id: parLevel3.Id.ToString(),
//                                                intervalMin: parLevel3.IntervalMin,
//                                                intervalMax: parLevel3.IntervalMax,
//                                                unitName: parLevel3.ParMeasurementUnit_Name);
//            }
//            return input;
//        }
//        public string getTipoInputBEA(SGQDBContext.ParLevel3 parLevel3, ref string classInput, ref string labels)
//        {
//            var html = new Html();
//            string input = null;
//            classInput = " defects";
//            labels = html.div(
//                                       classe: "font10",
//                                       style: "font-size: 11px; margin-top:7px;"
//                                   );

//            input = html.campoNumeroDeDefeitos(id: parLevel3.Id.ToString(),
//                                            intervalMin: parLevel3.IntervalMin,
//                                            intervalMax: parLevel3.IntervalMax,
//                                            unitName: parLevel3.ParMeasurementUnit_Name);
//            return input;
//        }

//        //public string GetLevel03_novo(SGQDBContext.ParLevel1 ParLevel1, SGQDBContext.ParLevel2 ParLevel2)
//        //{
//        //    var html = new Html();


//        //    var parlevel3List = ParLevel3DB.getLevel3ByLevel2(ParLevel2.Id);

//        //    string btnNaoAvaliado = html.button(
//        //                               label: html.span(
//        //                                                 classe: "cursorPointer iconsArea",
//        //                                                 outerhtml: "N/A"
//        //                                                ),
//        //                               classe: "btn-warning btnNotAvaliable na font11"
//        //                           );

//        //    string parLevel3Group = null;

//        //string panelButton = html.listgroupItem(outerhtml: "<button id='btnAllNA' class='btn btn-warning btn-sm pull-right'> Todos N/A </button>",
//        //                                            classe: "painel painelLevel02 row"
//        //                                        );



//        //    foreach (var parLevel3 in parlevel3List)
//        //    {

//        //        string classInput = null;
//        //        string tags = null;
//        //        string labels = null;
//        //        string input = null;

//        //        if (parLevel3.ParLevel3InputType_Id == 1)
//        //        {
//        //            classInput = " boolean";
//        //            input = html.campoBinario(parLevel3.Id.ToString(), parLevel3.ParLevel3BoolTrue_Name, parLevel3.ParLevel3BoolFalse_Name);

//        //        }
//        //        else
//        //        {
//        //            classInput = " interval";
//        //            tags = "intervalmin=\"" + parLevel3.IntervalMin + "\" intervalmax=\"" + parLevel3.IntervalMax + "\"";

//        //            labels = html.div(
//        //                             outerhtml: "<b>Min: </b>" + parLevel3.IntervalMin.ToString() + " ~ <b>Max: </b>" + parLevel3.IntervalMax.ToString() + " " + parLevel3.ParMeasurementUnit_Name,
//        //                             classe: "font10",
//        //                             style: "font-size: 11px; margin-top:7px;"
//        //                           );

//        //            input = html.campoIntervalo(id: parLevel3.Id.ToString(),
//        //                                           intervalMin: parLevel3.IntervalMin,
//        //                                           intervalMax: parLevel3.IntervalMax,
//        //                                           unitName: parLevel3.ParMeasurementUnit_Name);

//        //        }

//        //        string level3 = html.link(
//        //                                   outerhtml: html.span(outerhtml: parLevel3.Name, classe: "levelName"),
//        //                                   classe: "col-xs-4"
//        //                                  );
//        //        labels = html.div(
//        //                                outerhtml: labels,
//        //                                classe: "col-xs-3"
//        //                            );
//        //        string counters = html.div(
//        //                                    outerhtml: input,
//        //                                    classe: "col-xs-3 counters cursorPointer"
//        //                                  );
//        //        string buttons = html.div(
//        //                                   outerhtml: btnNaoAvaliado,
//        //                                   classe: "col-xs-2",
//        //                                   style: "text-align:right"
//        //                                 );

//        //        tags += " weight=\"" + parLevel3.Weight + "\" intervalmin=\"" + parLevel3.IntervalMin + "\" intervalmax=\"" + parLevel3.IntervalMax + "\"";

//        //        string level3List = html.listgroupItem(
//        //                                              id: parLevel3.Id.ToString(),
//        //                                              classe: "level3 row" + classInput,
//        //                                              tags: tags,
//        //                                              outerhtml: level3 +
//        //                                                         labels +
//        //                                                         counters +
//        //                                                         buttons
//        //                                            );

//        //        parLevel3Group += level3List;
//        //    }

//        //    string avaliacoes = html.div(
//        //                      outerhtml: "<b style=\"width:100px;display:inline-block\">Avaliações</b>" + html.span(classe: "evaluateCurrent") + " / " + html.span(classe: "evaluateTotal"),
//        //                    style: "font-size: 16px");
//        //    string amostrar = html.div(
//        //                                  outerhtml: "<b style=\"width:100px;display:inline-block\">Amostras</b>" + html.span(classe: "sampleCurrent") + " / " + html.span(classe: "sampleTotal"),
//        //                                style: "font-size: 16px");


//        //    string painellevel3 = html.listgroupItem(
//        //                                                outerhtml: avaliacoes +
//        //                                                           amostrar,

//        //                                   classe: "painel painelLevel03 row");



//        //    return parLevel3Group;

//        //}
//        public string GetLoginAPP()
//        {
//            var html = new Html();
//            string head = html.div(classe: "head");

//            //Verifica as configurações iniciais da tela
//            var ParConfSGQDB = new SGQDBContext.ParConfSGQContext(db);
//            var configuracoes = ParConfSGQDB.get();


//            #region form

//            #region Unit
//            bool inputsDesabilitados = false;

//            string selectUnit = null;
//            if (configuracoes != null && configuracoes.HaveUnitLogin == true)
//            {
//                inputsDesabilitados = true;
//                //coloca as unidades vindo do banco ou mocado eua, podemos colocar um arquivo para carregar
//                selectUnit = html.option("1", "Unit 1", tags: "ip=\"192.168.25.200/SgqMaster\"");
//                selectUnit = html.select(selectUnit, "selectUnit");
//            }

//            #endregion

//            #region shift
//            string selectShit = null;
//            if (configuracoes != null && configuracoes.HaveShitLogin == true)
//            {
//                inputsDesabilitados = true;
//                selectShit = html.option("0", CommonData.getResource("select_the_shift").Value.ToString()) +
//                              html.option("1", CommonData.getResource("shift_a").Value.ToString()) +
//                              html.option("2", CommonData.getResource("shift_b").Value.ToString());

//                selectShit = html.select(selectShit, id: "shift");
//            }
//            #endregion

//            string selectUrlPreffix = "";
//            //                          html.option("http://mtzsvmqsc/SgqGlobal", "JBS") +
//            //                          html.option("http://192.168.25.200/SgqMaster", "GRT") +
//            //                          html.option("http://localhost:8090/SgqSystem", "GCN");

//            string formOuterHtml = html.head(Html.h.h2, outerhtml: CommonData.getResource("login").Value.ToString()) +
//                                  selectUnit +
//                                  selectShit +
//                                  html.label(labelfor: "inputUserName", classe: "sr-only", outerhtml: CommonData.getResource("username").Value.ToString()) +
//                                  html.input(id: "inputUserName", placeholder: CommonData.getResource("username").Value.ToString(), required: true, disabled: inputsDesabilitados) +
//                                  html.label(labelfor: "inputPassword", classe: "sr-only", outerhtml: CommonData.getResource("password").Value.ToString()) +
//                                  html.input(type: Html.type.password, id: "inputPassword", placeholder: CommonData.getResource("password").Value.ToString(), required: true, disabled: inputsDesabilitados) +
//                                  html.button(label: CommonData.getResource("enter").Value.ToString(), id: "btnLogin", classe: "btn-lg btn-primary btn-block marginTop10", dataloading: "<i class='fa fa-spinner fa-spin'></i> <span class='wMessage' style='font-size:14px;'>" + CommonData.getResource("authenticating").Value.ToString() + "</span>") +

//                                  html.div(id: "messageError", classe: "alert alert-danger hide", tags: "role=\"alert\"",
//                                           outerhtml: html.span(classe: "icon-remove-sign") + "<strong>Erro! </strong>" + html.span(id: "mensagemErro")) +

//                                  html.div(classe: "divLoadFiles",
//                                           outerhtml: html.span(classe: "messageLoading")) +

//                                  html.div(id: "messageAlert",
//                                           classe: "alert alert-info hide",
//                                           tags: "role=\"alert\"",
//                                           outerhtml: html.span(id: "mensagemAlerta", classe: "icon-info-sign")) +

//                                  html.div(id: "messageSuccess",
//                                           classe: "alert alert-success hide",
//                                           tags: "role=\"alert\"",
//                                           outerhtml: html.span(id: "mensagemSucesso", classe: "icon-ok-circle"));

//            //html.select(selectUrlPreffix, "cb_UrlPreffix", "\" onChange='abreOApp(this.value);' \"");

//            string form = html.form(
//                                    outerhtml: formOuterHtml
//                                    , classe: "form-signin");

//            #endregion

//            #region foot
//            string footOuterHtml = html.br() +
//                                   html.br() +
//                                   html.br() +
//                                   html.span(
//                                              outerhtml: CommonData.getResource("version").Value.ToString() +
//                                                         html.span(classe: "number")
//                                             , id: "versionLogin") +
//                                   html.span(
//                                               outerhtml: html.span(classe: "base")
//                                             , id: "ambienteLogin"

//                                            );

//            string foot = html.div(
//                                    outerhtml: footOuterHtml
//                                    , classe: "foot", style: "text-align:center");

//            #endregion

//            return html.div(
//                                outerhtml: head +
//                                           form +
//                                           foot

//                                , classe: "login"
//                            );
//        }
//        #endregion


//    }
//}