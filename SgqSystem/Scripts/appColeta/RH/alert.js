var currentRecebeListaDeAlerta = [];
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

function hideAlert() {
    $("#alertObrigatorio").css("display", "none");
    $("#alertOpcional").css("display", "none");
}

function proximoElementoDaListaDeAlertas(indexDaListaDeAlerta){
    if(adicionaObjNaListaDeRespostasDaAcaoCorretiva(indexDaListaDeAlerta,currentlistaSeExisteAlerta) != false){
        indexDaListaDeAlerta = indexDaListaDeAlerta + 1;
        setTimeoutOpenCorrectiveAction(currentRecebeListaDeAlerta, indexDaListaDeAlerta);
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
            if(currentlistaObjCorrectiveAction.length > 0){
                currentlistaObjCorrectiveAction.forEach(function (o) {
                    if(o.objIndex != indexDaListaDeAlerta){
                        currentRespostasDaAcaoCorretiva.ImmediateCorrectiveAction = $('#immediateCorrectiveAction').val();
                        currentRespostasDaAcaoCorretiva.PreventativeMeasure = $('#preventativeMeasure').val();
                        currentRespostasDaAcaoCorretiva.DescriptionFailure = $('#descriptionFailure').val();
                        currentRespostasDaAcaoCorretiva.ParLevel3_Id = $('#parLevel3_Id').val();
                        currentlistaObjCorrectiveAction.push(currentRespostasDaAcaoCorretiva);
                    }
                });
            }
            else{
                currentRespostasDaAcaoCorretiva.ImmediateCorrectiveAction = $('#immediateCorrectiveAction').val();
                currentRespostasDaAcaoCorretiva.PreventativeMeasure = $('#preventativeMeasure').val();
                currentRespostasDaAcaoCorretiva.DescriptionFailure = $('#descriptionFailure').val();
                currentRespostasDaAcaoCorretiva.ParLevel3_Id = $('#parLevel3_Id').val();
                currentlistaObjCorrectiveAction.push(currentRespostasDaAcaoCorretiva);
            }
            return true;
        } 
    }
    return true;
};

function elementoAnteriorDaListaDeAlertas(indexDaListaDeAlerta){
    indexDaListaDeAlerta = indexDaListaDeAlerta - 1;
    pegaValorDoObjDaListaDeAlertas(indexDaListaDeAlerta);
    setTimeoutOpenCorrectiveAction(currentRecebeListaDeAlerta, indexDaListaDeAlerta);
}

function pegaValorDoObjDaListaDeAlertas(indexDaListaDeAlerta){
    currentlistaObjCorrectiveAction.forEach(function (o) {
        if(o.objIndex == indexDaListaDeAlerta){
            setTimeout(function () {
                $('#immediateCorrectiveAction').val(o.ImmediateCorrectiveAction);
                $('#preventativeMeasure').val(o.PreventativeMeasure);
                $('#descriptionFailure').val(o.DescriptionFailure);
                $('#parLevel3_Id').val(o.ParLevel3_Id);
            },100);
        }
    });
}

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
        ListaRespostasAcaoCorretiva: [],
        CollectionDate: getCurrentDate()
    };

    currentRespostasDaAcaoCorretiva = {
        objIndex: index
    };

        var alerta = 
            '<div style="background-color:red; padding:10px;">' +
            '<div>' +
            //'<p>Alerta ' + numeroDeAlertas + ' (' + exists[0].Name + ') foi disparado.</p>' +
            '<p> (' + exists[0].Name + ') no(a) ('+ $.grep(parametrization.listaParLevel1, function (o, i) { return o.Id == coleta.ParLevel1_Id; })[0].Name +')' +
            'para a medida de controle: ('+ $.grep(parametrization.listaParLevel3, function (o, i) { return o.Id == coleta.ParLevel3_Id; })[0].Name +')' +
            ' identificado durante o monitoramento: ('+ $.grep(parametrization.listaParLevel2, function (o, i) { return o.Id == coleta.ParLevel2_Id; })[0].Name +').' +
            '</p>' +
            '</div>' +
            '<div style="text-align:center">' +
            '</div>' +
            '</div>';   

        if(objCorrectiveAction.length > 1){
            btnNext = '<div>' +
                '<button class="btn btn-primary" id="next" onclick="proximoElementoDaListaDeAlertas(' + index + ')" style="float:right;">Próxima Ação Corretiva</button>' +
                '</div>';
        
            btnBack = '<div>' +
            '<button class="btn btn-primary" id="back" onclick="elementoAnteriorDaListaDeAlertas(' + index + ')" style="float:left;">Voltar Ação Corretiva</button>' +
            '</div>';
        }

        if(exists[0].HasCorrectiveAction){
            display = 'block';
            currentlistaSeExisteAlerta = exists;
            fillAcaocorretiva();
        }
        else
        {
            display = 'none';
            btnShowAcaoCorretiva = '<div>' +
                '<button class="btn btn-secundary" data-showAcaoCorretiva style="float:left;margin-left: 350px;">Mostrar Ação Corretiva</button>' +
                '</div>';

            currentlistaSeExisteAlerta = exists;
            fillAcaocorretiva();
        }
        
        function fillAcaocorretiva(){
            modal = '<h3 style="font-weight:bold; display:' + display + ';">Ação Corretiva</h3>';
            body = '<div class="form-group" style="display:' + display + ';">' +
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
                '<div class="container">' +
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
        disableButtons();

        function disableButtons(){
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
        }
        
        $('body').off('click', '[data-showAcaoCorretiva]').on('click', '[data-showAcaoCorretiva]', function (e) {
            display = 'block';
            fillAcaocorretiva();
            openModal(corpo, 'white', 'black');
            disableButtons();
            pegaValorDoObjDaListaDeAlertas(index);
        });

        $('body').off('click', '#btnSendCA').on('click', '#btnSendCA', function () {
            
            //Inserir collectionLevel2 dentro do obj
            correctiveAction.AuditorId = currentLogin.Id;

            adicionaObjNaListaDeRespostasDaAcaoCorretiva(index,currentlistaSeExisteAlerta);

            for (var i = 0; i < currentlistaObjCorrectiveAction.length; i++) {
                correctiveAction.CollectionLevel2.ListaRespostasAcaoCorretiva.push({
                    ImmediateCorrectiveAction: currentlistaObjCorrectiveAction[i].ImmediateCorrectiveAction,
                    PreventativeMeasure: currentlistaObjCorrectiveAction[i].PreventativeMeasure,
                    DescriptionFailure: currentlistaObjCorrectiveAction[i].DescriptionFailure,
                    ParLevel3_Id: currentlistaObjCorrectiveAction[i].ParLevel3_Id
                });    
            }

            //Salvar corrective action na lista de correctiveAction
            globalAcoesCorretivasRealizadas.push(correctiveAction);
            closeModal();
            currentlistaObjCorrectiveAction = null;
        });
}