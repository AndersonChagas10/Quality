function processAction() {

    var options = '<option value="">Selecione...</option>';

    parametrization.listaAuditor.forEach(function (parMultipleValue) {
        options += '<option value="' + parMultipleValue.Id + '">' + parMultipleValue.Name + '</option>';
    });

    var htmlAcao = `<div class="container"><div id="bodyModalAcao" style="display:block;">
   <h3 style="font-weight:bold;">Criar Ação</h3>
   <hr>
   <div class="form-group">
      <div class="form-group col-xs-12" style="border: 2px;border-color: azure;border-style: groove;">
         <label class="col-md-4">Data Emissão: ${ currentCollectDate.toLocaleDateString()}</label>
         <label class="col-md-3">Hora Emissão: ${currentCollectDate.toLocaleTimeString()}</label>
         <label>Emissor: ${currentLogin.Name}</label>
      </div>
      <div class="form-group col-xs-12" style="border: 2px;border-color: azure;border-style: groove;">
         <p>Unidade: ${$.grep(currentLogin.ParCompanyXUserSgq, function (o, i) { return o.ParCompany.Id == currentParCompany_Id })[0].ParCompany.Name}</p>
         <p>Centro de Custo: ${$.grep(parametrization.listaParDepartment, function (o, i) { return o.Id == currentParDepartment_Id })[0].Name}</p>
         <p>Seção/Atividade: ${$.grep(parametrization.listaParDepartment, function (o, i) { return o.Parent_Id == currentParDepartmentParent_Id })[0].Name}</p>
         <p>Item/Tarefa: tarefa</p>
         <p>Indicado/Origem: indicador</p>
         <p>Monitoramento: monitoramento</p>
         <p>Desvio: desvio</p>
      </div>
      <div class="form-group col-xs-12" style="">
         <p>Não Conformidade/Ocorrencia</p>
         <input class="form-control" type="text">
         <p>Ação</p>
         <input class="form-control">
      </div>
      <hr>
      <div class="form-group col-xs-12">
         <p><i class="fa fa-camera" aria-hidden="true"></i> Evidencias de Não conformidade</p>
         <ul>
            <li>Ver e agir</li>
         </ul>
         <p><i class="fa fa-camera" aria-hidden="true"></i> Evidencias da Ação Concluida</p>
      </div>
   </div>
   <div class="form-group col-md-12">
      <div class="col-md-4">
         <label>Data da conclusão:</label>
         <input type="date" class="form-control">
      </div>
      <div class="col-md-4">
         <label>Hora da conclusão:</label>
         <input type="time" class="form-control">
      </div>
      <div class="col-md-4">
         <label>referencia:</label>
         <input type="text" class="form-control">
      </div>
      <div class="col-md-4">
         <label>Responsavel:</label>
         <select class="form-control">
            ${options}
         </select>
      </div>
      <div class="col-md-4">
         <label>Responsavel:</label>
         <select class="form-control">
           ${options}
         </select>
      </div>
   </div>
   <div class="col-md-12">
      <div class="col-md-6">
         <button class="btn btn-success">Salvar</button>
      </div>
   </div>
</div></div>`;

    openModal(htmlAcao, 'white', 'black');
}

//function processAction(coletaJson) {

//    if (coletaJson.length == 0)
//        return;

//    var listaParAlertPreFiltrada = $.grep(parametrization.listaParAlert, function (o, i) {
//        return (o.ParDepartment_Id == coletaJson[0].ParDepartment_Id || o.ParDepartment_Id == null)
//            && (o.ParCargo_Id == coletaJson[0].ParCargo_Id || o.ParCargo_Id == null)
//    });

//    currentListaDeColetaComAlertaEAcaoCorretiva = [];
//    for (var i = 0; i < coletaJson.length; i++) {

//        var coleta = coletaJson[i];

//        if (coleta.IsConform)
//            continue;

//        //retorna se existe algum alerta vigente para este cenario
//        var listaAlertasVigente = $.grep(listaParAlertPreFiltrada, function (o, i) {
//            return (o.ParCompany_Id == coleta.ParCompany_Id || o.ParCompany_Id == null)
//                && (o.ParLevel1_Id == coleta.ParLevel1_Id || o.ParLevel1_Id == null)
//                && (o.ParLevel2_Id == coleta.ParLevel2_Id || o.ParLevel2_Id == null)
//                && o.ParLevel3_Id == coleta.ParLevel3_Id
//                && o.ParAlertType_Id == 1
//                && (o.ParSecao_Ids == currentParDepartment_Id || o.ParSecao_Ids == null);
//        });

//        //retorna todos os alertas para somar o numero do alerta
//        var numeroDeAlertas = $.grep(currentAlertsAgrupados, function (o, i) {
//            return (o.ParDepartment_Id == coleta.ParDepartment_Id || o.ParDepartment_Id == null)
//                && (o.ParCargo_Id == coleta.ParCargo_Id || o.ParCargo_Id == null)
//                && (o.ParCompany_Id == coleta.ParCompany_Id || o.ParCompany_Id == null)
//                && (o.ParLevel1_Id == coleta.ParLevel1_Id || o.ParLevel1_Id == null)
//                && (o.ParLevel2_Id == coleta.ParLevel2_Id || o.ParLevel2_Id == null)
//                && o.ParLevel3_Id == coleta.ParLevel3_Id
//        }).length;

//        if (listaAlertasVigente.length > 0) {

//            numeroDeAlertas++;

//            currentListaDeColetaComAlertaEAcaoCorretiva.push({
//                listaAlertasVigente: listaAlertasVigente,
//                coleta: coleta,
//                numberAlert: numeroDeAlertas
//            });

//            currentAlertsAgrupados.push({
//                ParDepartment_Id: coleta.ParDepartment_Id,
//                ParCargo_Id: coleta.ParCargo_Id,
//                ParCompany_Id: coleta.ParCompany_Id,
//                ParLevel1_Id: coleta.ParLevel1_Id,
//                ParLevel2_Id: coleta.ParLevel2_Id,
//                ParLevel3_Id: coleta.ParLevel3_Id,
//                Number: numeroDeAlertas
//            });
//        }
//    }

//    if (currentListaDeColetaComAlertaEAcaoCorretiva.length > 0) {
//        abreModalCorrectiveAction(currentListaDeColetaComAlertaEAcaoCorretiva, 0);
//    }
//}

function abreModalCorrectiveAction(coletasAgrupadas, index) {


    var corpo = montaHtmlModalAcaoCorretiva(coletasAgrupadas, index);
    openModal(corpo, 'white', 'black');
    disableButtons(index);
}

function montaHtmlModalAcaoCorretiva(listaDeColetaComAlertaEAcaoCorretiva, index) {
    var modal = "";
    var body = "";
    var corpo = "";
    var display = "";
    var btnShowAcaoCorretiva = "";
    var btnNext = "";
    var btnBack = "";
    var secao;
    var centroCusto;

    for (var i = 0; i < listaDeColetaComAlertaEAcaoCorretiva.length; i++) {
        secao = $.grep(parametrization.listaParDepartment, function (o, j) {
            if (o.Id == listaDeColetaComAlertaEAcaoCorretiva[i].ParDepartment_Id)
                return o;
        });

        centroCusto = $.grep(parametrization.listaParDepartment, function (o, j) {
            if (o.Id == secao.Parent_Id)
                return o;
        });
    }

    var alerta = montaHtmlModalAlerta(listaDeColetaComAlertaEAcaoCorretiva, coleta);

    if (listaAlertasVigente[0].HasCorrectiveAction) {
        display = 'block';
    }
    else {
        display = 'none';
        btnShowAcaoCorretiva = '<div class="col-sm-3">' +
            '<button class="btn btn-secundary" data-showAcaoCorretiva>Preencher Ação Corretiva</button>' +
            '</div>';
    }
    currentlistaSeExisteAlerta = listaAlertasVigente;

    if (listaDeColetaComAlertaEAcaoCorretiva.length > 1) {

        btnNext = '<div class="col-sm-3">' +
            '<button class="btn btn-primary" id="next" onclick="proximoElementoDaListaDeAlertas(' + index + ')" style="float:right;">Próximo Alerta' +
            ' (' + currentQtdAlerta + "/" + listaDeColetaComAlertaEAcaoCorretiva.length + ')</button>' +
            '</div>';

        btnBack = '<div class="col-sm-3">' +
            '<button class="btn btn-primary" id="back" onclick="elementoAnteriorDaListaDeAlertas(' + index + ')" style="float:left;">Voltar Alerta</button>' +
            '</div>';
    }

    modal = '<h3 style="font-weight:bold;">Ação Corretiva</h3>';
    body = '<div class="form-group">' +
        '<div class="form-group col-xs-12">' +
        '<strong><small>Unidade: ' + $.grep(currentLogin.ParCompanyXUserSgq, function (o, i) { return o.ParCompany.Id == currentParCompany_Id })[0].ParCompany.Name +
        '<br/>Data/Hora: ' + currentCollectDate.toLocaleDateString() + ' ' + currentCollectDate.toLocaleTimeString() +
        '<br/>Monitor: ' + currentLogin.Name + '</strong>' +
        '<input type="hidden" id="parLevel3_Id" value="' + $.grep(parametrization.listaParLevel3, function (o, i) { return o.Id == coleta.ParLevel3_Id; })[0].Id + '">' +
        '<br/><br/><strong>Informações</strong>' +
        '<br/>Cluster: ' + $.grep(parametrization.listaParCluster, function (o, i) { return o.Id == currentParCluster_Id; })[0].Name +
        '<br/>Frequência: ' + $.grep(parametrization.listaParFrequency, function (item) { return item.Id == parametrization.currentParFrequency_Id; })[0].Name +
        '<br/>Centro de Custo: ' + centroCusto[0].Name +
        '<br/>Seção: ' + secao[0].Name +
        '<br/>Cargo: ' + $.grep(parametrization.listaParCargo, function (o, i) { return o.Id == coleta.ParCargo_Id; })[0].Name +
        '<br/>Indicador: ' + $.grep(parametrization.listaParLevel1, function (o, i) { return o.Id == coleta.ParLevel1_Id; })[0].Name +
        '<br/>Monitoramento: ' + $.grep(parametrization.listaParLevel2, function (o, i) { return o.Id == coleta.ParLevel2_Id; })[0].Name +
        '<br/>Medida de Controle: ' + $.grep(parametrization.listaParLevel3, function (o, i) { return o.Id == coleta.ParLevel3_Id; })[0].Name +
        '<br/>Alerta: KO - ' + listaAlertasVigente[0].Name +
        '</small></div>' +

        '<div class="form-group col-xs-12">' +
        '<label>Descrição da Falha:</label>' +
        '<input name="DescriptionFailure" id="descriptionFailure" class="col-sx-12 form-control" style="height: 80px;">' +
        '</div>' +
        '<div class="form-group col-xs-12">' +
        '<label for="email">Ação Corretiva Imediata:</label>' +
        '<input name="ImmediateCorrectiveAction" id="immediateCorrectiveAction" class="form-control" style="height: 80px;">' +
        '</div>' +
        '<div class="form-group col-xs-12">' +
        '<label for="email">Ação Preventiva:</label>' +
        '<input name="PreventativeMeasure" id="preventativeMeasure" class="form-control" style="height: 80px;">' +
        '</div>';

    corpo =
        '<div class="container" id="modalAcaoCorretiva">' +
        '<div class="row" style="overflow:auto">' +
        '<div id="alertObrigatorio" class="alert alert-warning alert-dismissible" style="display: none;" role="alert">' +
        '<button type="button" class="close" onclick="hideAlert()"><span aria-hidden="true">&times;</span></button>' +
        '<strong>Esse alerta é obrigatório, preencher todos os campos para continuar!</strong>' +
        '</div>' +
        '<div id="alertOpcional" class="alert alert-warning alert-dismissible" style="display: none;" role="alert">' +
        '<button type="button" class="close" onclick="hideAlert()"><span aria-hidden="true">&times;</span></button>' +
        '<strong>Esse alerta é opcional, preencha todos os campos ou não preencha nenhum campo para continuar!</strong>' +
        '</div>' +
        '<div>' +
        alerta +
        '</div>' +
        '<div style="padding-top: 10px;">' +
        btnBack +
        btnShowAcaoCorretiva +
        '<div class="col-sm-3"><button class="btn btn-primary" id="btnSendCA" data-index="' + index + '">Salvar e Fechar Ações Corretivas</button></div>' +
        btnNext +
        '</div>' +
        '<div style="margin-top:60px;">' +
        '<hr>' +
        '</div>' +
        '<div id="bodyModalAcaoCorretiva" style="display:' + display + ';">' +
        modal +
        '<hr>' +
        body +
        '<hr>' +
        '<div class="form-group col-xs-6">' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>';

    return corpo;
}

function montaHtmlModalAlerta(listaAlertasVigente, coleta) {
    var alerta = '<div style="background-color:red; color:#ffffff; padding:10px;">' +
        '<div>' +
        '<p> (' + listaAlertasVigente[0].Name + ') no(a) (' + $.grep(parametrization.listaParLevel1, function (o, i) { return o.Id == coleta.ParLevel1_Id; })[0].Name + ')' +
        ' para a medida de controle: (' + $.grep(parametrization.listaParLevel3, function (o, i) { return o.Id == coleta.ParLevel3_Id; })[0].Name + ')' +
        ' identificado durante o monitoramento: (' + $.grep(parametrization.listaParLevel2, function (o, i) { return o.Id == coleta.ParLevel2_Id; })[0].Name + ').' +
        '</p>' +
        '</div>' +
        '<div style="text-align:center">' +
        '</div>' +
        '</div>';
    return alerta;
}
