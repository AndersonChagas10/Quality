var currentListaDeColetaComAlertaEAcaoCorretiva = [];
var currentRespostasDaAcaoCorretiva = {};
var currentlistaObjCorrectiveAction = [];
var currentlistaSeExisteAlerta = [];

function processAlertRole(coletaJson) {

    if (coletaJson.length == 0)
        return;

    var listaParAlertPreFiltrada = $.grep(parametrization.listaParAlert, function (o, i) {
        return (o.ParDepartment_Id == coletaJson[0].ParDepartment_Id || o.ParDepartment_Id == null)
            && (o.ParCargo_Id == coletaJson[0].ParCargo_Id || o.ParCargo_Id == null)
    });

    currentListaDeColetaComAlertaEAcaoCorretiva = [];
    for (var i = 0; i < coletaJson.length; i++) {

        var coleta = coletaJson[i];

        if (coleta.IsConform)
            continue;

        //retorna se existe algum alerta vigente para este cenario
        var listaAlertasVigente = $.grep(listaParAlertPreFiltrada, function (o, i) {
            return (o.ParCompany_Id == coleta.ParCompany_Id || o.ParCompany_Id == null)
                && (o.ParLevel1_Id == coleta.ParLevel1_Id || o.ParLevel1_Id == null)
                && (o.ParLevel2_Id == coleta.ParLevel2_Id || o.ParLevel2_Id == null)
                && o.ParLevel3_Id == coleta.ParLevel3_Id
                && o.ParAlertType_Id == 1;
        });

        //retorna todos os alertas para somar o numero do alerta
        var numeroDeAlertas = $.grep(currentAlertsAgrupados, function (o, i) {
            return (o.ParDepartment_Id == coleta.ParDepartment_Id || o.ParDepartment_Id == null)
                && (o.ParCargo_Id == coleta.ParCargo_Id || o.ParCargo_Id == null)
                && (o.ParCompany_Id == coleta.ParCompany_Id || o.ParCompany_Id == null)
                && (o.ParLevel1_Id == coleta.ParLevel1_Id || o.ParLevel1_Id == null)
                && (o.ParLevel2_Id == coleta.ParLevel2_Id || o.ParLevel2_Id == null)
                && o.ParLevel3_Id == coleta.ParLevel3_Id
        }).length;

        if (listaAlertasVigente.length > 0) {

            numeroDeAlertas++;
            
            currentListaDeColetaComAlertaEAcaoCorretiva.push({
                listaAlertasVigente: listaAlertasVigente,
                coleta: coleta,
                numberAlert: numeroDeAlertas
            });

            currentAlertsAgrupados.push({
                ParDepartment_Id: coleta.ParDepartment_Id,
                ParCargo_Id: coleta.ParCargo_Id,
                ParCompany_Id: coleta.ParCompany_Id,
                ParLevel1_Id: coleta.ParLevel1_Id,
                ParLevel2_Id: coleta.ParLevel2_Id,
                ParLevel3_Id: coleta.ParLevel3_Id,
                Number: numeroDeAlertas
            });
        }
    }
    
    if(currentListaDeColetaComAlertaEAcaoCorretiva.length > 0){
        abreModalCorrectiveAction(currentListaDeColetaComAlertaEAcaoCorretiva, 0);
    }
}

function abreModalCorrectiveAction(listaDeColetaComAlertaEAcaoCorretiva, index){
    var listaAlertasVigente = listaDeColetaComAlertaEAcaoCorretiva[index].listaAlertasVigente;
    var coleta = listaDeColetaComAlertaEAcaoCorretiva[index].coleta;

    var correctiveAction = {};

    //Pegar os dados correntes
    correctiveAction.CollectionLevel2 = {
        ParLevel1_Id: coleta.ParLevel1_Id,
        ParLevel2_Id: coleta.ParLevel2_Id,
        UnitId: coleta.ParCompany_Id,
        //Shift: 1,
        EvaluationNumber: coleta.Evaluation,
        Sample: coleta.Sample,
        ParDepartment_Id: coleta.ParDepartment_Id,
        ParCargo_Id: coleta.ParCargo_Id,
        //ParCluster_Id: 1,
        CollectionDate: getCurrentDate()
    };

    currentRespostasDaAcaoCorretiva = correctiveAction;
    currentRespostasDaAcaoCorretiva.objIndex = index;

    var corpo = montaHtmlModalAcaoCorretiva(listaDeColetaComAlertaEAcaoCorretiva, listaAlertasVigente, index, coleta);
    openModal(corpo, 'white', 'black');
    disableButtons(index);
}
        
function montaHtmlModalAcaoCorretiva(listaDeColetaComAlertaEAcaoCorretiva, listaAlertasVigente, index,coleta){
    var modal = "";
    var body = "";
    var corpo = "";
    var display = "";
    var btnShowAcaoCorretiva = "";
    var btnNext = "";
    var btnBack = "";
    
    var alerta = montaHtmlModalAlerta(listaAlertasVigente,coleta);

    if(listaAlertasVigente[0].HasCorrectiveAction){
        display = 'block';
    }
    else
    {
        display = 'none';
        btnShowAcaoCorretiva = '<div>' +
            '<button class="btn btn-secundary" data-showAcaoCorretiva style="float:left;margin-left: 350px;">Mostrar Ação Corretiva</button>' +
            '</div>';
    }
    currentlistaSeExisteAlerta = listaAlertasVigente;

    if(listaDeColetaComAlertaEAcaoCorretiva.length > 1){
        btnNext = '<div>' +
            '<button class="btn btn-primary" id="next" onclick="proximoElementoDaListaDeAlertas(' + index + ')" style="float:right;">Próxima Ação Corretiva</button>' +
            '</div>';
    
        btnBack = '<div>' +
        '<button class="btn btn-primary" id="back" onclick="elementoAnteriorDaListaDeAlertas(' + index + ')" style="float:left;">Voltar Ação Corretiva</button>' +
        '</div>';
    }

    modal = '<h3 style="font-weight:bold;">Ação Corretiva</h3>';
    body = '<div class="form-group">' +
        '<div class="form-group col-xs-12">' +
        '<strong>Informações</strong>' +
        '<small><br/>Data/Hora: ' + currentCollectDate.toLocaleDateString() + ' ' + currentCollectDate.toLocaleTimeString() +
        '<br/>Monitor: ' + currentLogin.Name +
        '<br/>Tarefa: ' + $.grep(parametrization.listaParLevel3, function (o, i) { return o.Id == coleta.ParLevel3_Id; })[0].Name +
        '<input type="hidden" id="parLevel3_Id" value="' + $.grep(parametrization.listaParLevel3, function (o, i) { return o.Id == coleta.ParLevel3_Id; })[0].Id + '">' +
        '<br/>Frequência: ' + $.grep(parametrization.listaParFrequency, function (item) { return item.Id == parametrization.currentParFrequency_Id; })[0].Name +
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
        '<button type="button" class="close" onclick="hideAlert()"><span aria-hidden="true">&times;</span></button>'+
        '<strong>Esse alerta é obrigatório, preencher todos os campos para continuar!</strong>'  +
        '</div>' +
        '<div id="alertOpcional" class="alert alert-warning alert-dismissible" style="display: none;" role="alert">' +
        '<button type="button" class="close" onclick="hideAlert()"><span aria-hidden="true">&times;</span></button>'+
        '<strong>Esse alerta é opcional, preencha todos os campos ou não preencha nenhum campo para continuar!</strong>'  +
        '</div>' +
        '<div>' +
        alerta +                
        '</div>' +
        '<div style="padding-top: 10px;">' +
        btnBack +
        btnShowAcaoCorretiva +
        btnNext +
        '</div>' +
        '<div style="margin-top:60px;">' +
        '<hr>' +
        '</div>' +
        '<div id="bodyModalAcaoCorretiva" style="display:'+ display +';">' +
        modal +
        '<hr>' +
        body +
        '<hr>' +
        '<div class="form-group col-xs-6">' +
        '<button class="btn btn-primary" id="btnSendCA" data-index="' + index + '">Salvar Ação Corretiva</button>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>';

    return corpo;
}

function hideAlert() {
    $("#alertObrigatorio").css("display", "none");
    $("#alertOpcional").css("display", "none");
}

function proximoElementoDaListaDeAlertas(indexDaListaDeAlerta){
    if(adicionaObjNaListaDeRespostasDaAcaoCorretiva(indexDaListaDeAlerta,currentlistaSeExisteAlerta) != false){
        indexDaListaDeAlerta++;
        abreModalCorrectiveAction(currentListaDeColetaComAlertaEAcaoCorretiva, indexDaListaDeAlerta);
        pegaValorDoObjDaListaDeAlertas(indexDaListaDeAlerta);
    }
}

function adicionaObjNaListaDeRespostasDaAcaoCorretiva(indexDaListaDeAlerta,currentlistaSeExisteAlerta){
    if(currentlistaSeExisteAlerta[0].HasCorrectiveAction != false 
        || ($('#immediateCorrectiveAction').val() != "" 
            || $('#preventativeMeasure').val() != "" 
            || $('#descriptionFailure').val() != ""
        ))
    {
        if(currentlistaSeExisteAlerta[0].HasCorrectiveAction == false && ($('#immediateCorrectiveAction').val() == "" || $('#preventativeMeasure').val() == "" || $('#descriptionFailure').val() == "")){
            $("#alertOpcional").css("display", "block");
            return false;
        }
        else if(currentlistaSeExisteAlerta[0].HasCorrectiveAction == true && ($('#immediateCorrectiveAction').val() == "" || $('#preventativeMeasure').val() == "" || $('#descriptionFailure').val() == "")){
            $("#alertObrigatorio").css("display", "block");
            return false;
        }
        else{
            var novaAcaoCorretiva = true;
            if(currentlistaObjCorrectiveAction.length > 0){
                currentlistaObjCorrectiveAction.forEach(function (o) {
                    if(o.objIndex == indexDaListaDeAlerta){
                        novaAcaoCorretiva = false;
                        currentRespostasDaAcaoCorretiva = preencheObjetoAcaoCorretiva(currentRespostasDaAcaoCorretiva);
                    }
                });
            }
            if(novaAcaoCorretiva){
                currentRespostasDaAcaoCorretiva = preencheObjetoAcaoCorretiva(currentRespostasDaAcaoCorretiva);
                currentlistaObjCorrectiveAction.push(currentRespostasDaAcaoCorretiva);
            }
            return true;
        } 
    }
    return true;
};

function elementoAnteriorDaListaDeAlertas(indexDaListaDeAlerta){
    adicionaObjNaListaDeRespostasDaAcaoCorretiva(indexDaListaDeAlerta,currentlistaSeExisteAlerta)
    indexDaListaDeAlerta = indexDaListaDeAlerta - 1;
    abreModalCorrectiveAction(currentListaDeColetaComAlertaEAcaoCorretiva, indexDaListaDeAlerta);
    pegaValorDoObjDaListaDeAlertas(indexDaListaDeAlerta);
}

function pegaValorDoObjDaListaDeAlertas(indexDaListaDeAlerta){
    currentlistaObjCorrectiveAction.forEach(function (o) {
        if(o.objIndex == indexDaListaDeAlerta){
            $('#immediateCorrectiveAction').val(o.ImmediateCorrectiveAction);
            $('#preventativeMeasure').val(o.PreventativeMeasure);
            $('#descriptionFailure').val(o.DescriptionFailure);
            $('#parLevel3_Id').val(o.ParLevel3_Id);
        }
    });
}

function montaHtmlModalAlerta(listaAlertasVigente,coleta){
    var alerta = '<div style="background-color:red; padding:10px;">' +
    '<div>' +
    '<p> (' + listaAlertasVigente[0].Name + ') no(a) ('+ $.grep(parametrization.listaParLevel1, function (o, i) { return o.Id == coleta.ParLevel1_Id; })[0].Name +')' +
    'para a medida de controle: ('+ $.grep(parametrization.listaParLevel3, function (o, i) { return o.Id == coleta.ParLevel3_Id; })[0].Name +')' +
    ' identificado durante o monitoramento: ('+ $.grep(parametrization.listaParLevel2, function (o, i) { return o.Id == coleta.ParLevel2_Id; })[0].Name +').' +
    '</p>' +
    '</div>' +
    '<div style="text-align:center">' +
    '</div>' +
    '</div>'; 
    return alerta;
}

function disableButtons(index){
    if(index < currentListaDeColetaComAlertaEAcaoCorretiva.length - 1){
        $("#modalAcaoCorretiva #btnSendCA").attr('disabled', true);
        $("#modalAcaoCorretiva #next").attr('disabled', false);
        $("#modalAcaoCorretiva #back").attr('disabled', true);
    }
    else{
        $("#modalAcaoCorretiva #btnSendCA").attr('disabled', false);
        $("#modalAcaoCorretiva #next").attr('disabled', true);
        $("#modalAcaoCorretiva #back").attr('disabled', false);
    }
}

$('body').off('click', '[data-showAcaoCorretiva]').on('click', '[data-showAcaoCorretiva]', function (e) {
    display = 'block';
    $("#bodyModalAcaoCorretiva").show();
});

$('body').off('click', '#btnSendCA').on('click', '#btnSendCA', function () {
    
    var index = $(this).attr('data-index');
    //Inserir collectionLevel2 dentro do obj

    adicionaObjNaListaDeRespostasDaAcaoCorretiva(index,currentlistaSeExisteAlerta);

    for (var i = 0; i < currentlistaObjCorrectiveAction.length; i++) {
        //Salvar corrective action na lista de correctiveAction
        currentlistaObjCorrectiveAction[i].AuditorId = currentLogin.Id;
        globalAcoesCorretivasRealizadas.push(currentlistaObjCorrectiveAction[i]);
    }

    closeModal();
    currentlistaObjCorrectiveAction = null;
});

function preencheObjetoAcaoCorretiva(acaoCorretiva){
    acaoCorretiva.ImmediateCorrectiveAction = $('#immediateCorrectiveAction').val();
    acaoCorretiva.PreventativeMeasure = $('#preventativeMeasure').val();
    acaoCorretiva.DescriptionFailure = $('#descriptionFailure').val();
    acaoCorretiva.ParLevel3_Id = $('#parLevel3_Id').val();
    return acaoCorretiva;
}