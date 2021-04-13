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
