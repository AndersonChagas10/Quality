var currentRecebeListaDeAlerta = [];

function processAlertRole(coletaJson) {

    if (coletaJson.length == 0)
        return;

    var listaParAlertPreFiltrada = $.grep(parametrization.listaParAlert, function (o, i) {
        return (o.ParDepartment_Id == coletaJson[0].ParDepartment_Id || o.ParDepartment_Id == null)
            && (o.ParCargo_Id == coletaJson[0].ParCargo_Id || o.ParCargo_Id == null)
    });

    var objCorrectiveAction = [];
    for (var i = 0; i < coletaJson.length; i++) {

        var coleta = coletaJson[i];

        if (coleta.IsConform)
            continue;

        //retorna se existe alguem alerta vigente para este cenario
        var exists = $.grep(listaParAlertPreFiltrada, function (o, i) {
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

        if (exists.length > 0) {

            numeroDeAlertas++;
            
            var recebeObj = montaObjCorrectiveAction(exists,coleta,numeroDeAlertas);
            objCorrectiveAction.push(recebeObj);
            currentRecebeListaDeAlerta = objCorrectiveAction;

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
    
    if(objCorrectiveAction.length > 0){
        setTimeoutOpenCorrectiveAction(objCorrectiveAction, 0);
    }
}

function montaObjCorrectiveAction(exists,coleta,numeroDeAlertas){
    return {
        exist: exists,
        coleta: coleta,
        numberAlert: numeroDeAlertas
    }
}

function proximoElementLista(indexDaListaDeAlerta){
    indexDaListaDeAlerta = indexDaListaDeAlerta + 1;
    addObjLista(indexDaListaDeAlerta);
    setTimeoutOpenCorrectiveAction(currentRecebeListaDeAlerta, indexDaListaDeAlerta);
 
}

function addObjLista(indexDaListaDeAlerta){
    debugger
    var correctiveActionResult = {};
    if(listaObjCorrectiveAction.length > 0){
        listaObjCorrectiveAction.forEach(function (oi) {
            if(oi.objIndex != indexDaListaDeAlerta){
                correctiveActionResult.ImmediateCorrectiveAction = $('#immediateCorrectiveAction').val();
                correctiveActionResult.PreventativeMeasure = $('#preventativeMeasure').val();
                correctiveActionResult.DescriptionFailure = $('#descriptionFailure').val();
                listaObjCorrectiveAction.push(correctiveActionResult);
            }
        });
    }
    else{
        correctiveActionResult.ImmediateCorrectiveAction = $('#immediateCorrectiveAction').val();
        correctiveActionResult.PreventativeMeasure = $('#preventativeMeasure').val();
        correctiveActionResult.DescriptionFailure = $('#descriptionFailure').val();
        listaObjCorrectiveAction.push(correctiveActionResult);
    }
};

function elementAnteriorLista(indexDaListaDeAlerta){
    indexDaListaDeAlerta = indexDaListaDeAlerta - 1;
    setTimeoutOpenCorrectiveAction(currentRecebeListaDeAlerta, indexDaListaDeAlerta);
}

var listaObjCorrectiveAction = [];
function setTimeoutOpenCorrectiveAction(objCorrectiveAction, index){
    var exists = objCorrectiveAction[index].exist;
    var coleta = objCorrectiveAction[index].coleta;
    var modal = "";
    var body = "";
    var corpo = "";
    var display = "";
    var btnShowAcaoCorretiva = "";
    var btnNext = "";
    var btnBack = "";
    var numeroDeAlertas = objCorrectiveAction[index].numberAlert;

    var correctiveAction = {};
    var correctiveActionResult = {};
    //var listaObjCorrectiveAction = [];

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

    correctiveActionResult = {
        objIndex: index
    };

    setTimeout(function () {

        var alerta = 
            '<div style="background-color:red; padding:10px;">' +
            '<div>' +
            '<p>Alerta ' + numeroDeAlertas + ' (' + exists[0].Name + ') foi disparado.</p>' +
            '</div>' +
            '<div style="text-align:center">' +
            '</div>' +
            '</div>';   

        if(objCorrectiveAction.length > 1){
            btnNext = '<div>' +
                '<button class="btn btn-primary" id="next" onclick="proximoElementLista(' + index + ');" style="float:right;">Próxima Ação Corretiva</button>' +
                '</div>';
        
            btnBack = '<div>' +
            '<button class="btn btn-primary" id="back" onclick="elementAnteriorLista(' + index + ')" style="float:right;">Voltar Ação Corretiva</button>' +
            '</div>';
        }

        if(exists[0].HasCorrectiveAction){
            display = 'block';
            fillAcaocorretiva();
        }
        else
        {
            display = 'none';
            btnShowAcaoCorretiva = '<div>' +
                '<button class="btn btn-secundary" data-showAcaoCorretiva style="float:left;">Mostrar Ação Corretiva</button>' +
                '</div>';
            fillAcaocorretiva();
        }

        function p() {
            alert('show')
        }
        
        function fillAcaocorretiva(){
            modal = '<h3 style="font-weight:bold; display:' + display + ';">Ação Corretiva</h3>';
            body = '<div class="form-group" style="display:' + display + ';">' +
                '<div class="form-group col-xs-12">' +
                '<strong>Informações</strong>' +
                '<small><br/>Data/Hora: ' + currentCollectDate.toLocaleDateString() + ' ' + currentCollectDate.toLocaleTimeString() +
                '<br/>Monitor: ' + currentLogin.Name +
                '<br/>Tarefa: ' + $.grep(parametrization.listaParLevel3, function (o, i) { return o.Id == coleta.ParLevel3_Id; })[0].Name +
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
                '<div class="container">' +
                '<div class="row" style="overflow:auto">' +
                '<div>' +
                alerta +                
                '</div>' +
                btnShowAcaoCorretiva +
                btnNext +
                btnBack +
                '<hr>' +
                modal +
                '<hr>' +
                '<div>' +
                body +
                '</div>' +
                '<hr>' +
                '<div class="form-group col-xs-6">' +
                '<button class="btn btn-primary" id="btnSendCA" style="display:' + display + ';">Salvar Ação Corretiva</button>' +
                '</div>' +
                '</div>' +
                '</div>';
        }

        openModal(corpo, 'white', 'black');

        if(index < objCorrectiveAction.length - 1){
            $("#btnSendCA").attr('disabled', true);
            $("#next").attr('disabled', false);
            $("#back").attr('disabled', true);
        }
        else{
            $("#btnSendCA").attr('disabled', false);
            $("#next").attr('disabled', true);
            $("#back").attr('disabled', false);
        }

        $('body').off('click', '[data-showAcaoCorretiva]').on('click', '[data-showAcaoCorretiva]', function (e) {
            display = 'block';
            fillAcaocorretiva();
            openModal(corpo, 'white', 'black');
        });
      
        
        // $('#back').off().on('click', function () {
        //     listaObjCorrectiveAction.forEach(function (oi) {
        //         if(oi.objIndex == index){
        //             $('#immediateCorrectiveAction').val(oi.ImmediateCorrectiveAction);
        //             $('#preventativeMeasure').val(oi.PreventativeMeasure);
        //             $('#descriptionFailure').val(oi.DescriptionFailure);
        //         }
        //     });
        // });

        $('#btnSendCA').off().on('click', function () {

            //Inserir collectionLevel2 dentro do obj
            correctiveAction.AuditorId = currentLogin.Id;
            correctiveAction.ImmediateCorrectiveAction = $('#immediateCorrectiveAction').val();
            correctiveAction.PreventativeMeasure = $('#preventativeMeasure').val();
            correctiveAction.DescriptionFailure = $('#descriptionFailure').val();
            listaObjCorrectiveAction.push(correctiveAction);

            for (var j = 0; j < listaObjCorrectiveAction.length; j++) {

                //Salvar corrective action na lista de correctiveAction
                //globalAcoesCorretivasRealizadas.push(listaObjCorrectiveAction[j]);
            }
            // no final do salvar limpar a lista de acao respondida listaObjCorrectiveAction;
            closeModal();
        });
    }, 3500);
}