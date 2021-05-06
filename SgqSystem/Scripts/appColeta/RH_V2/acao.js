var currentQtdAcoes = 1;
var currentListaDeColetaComAcao = [];
var index = 0;
var listaAcoes = [];
var listaObjAcoes = [];

function processAction(coletaJson) {

    var listaLevel1Acao = $.grep(parametrization.listaParLevel1, function (o, i) {
        return o.GenerateActionOnNotConformity;
    });
 
    for (var i = 0; i < coletaJson.length; i++) {
        for (var j = 0; j < listaLevel1Acao.length; j++) {

            if (!coletaJson[i].hasOwnProperty('ParHeaderField_Id')
                && parseInt(listaLevel1Acao[j].Id) == parseInt(coletaJson[i].ParLevel1_Id)
                && !coletaJson[i].IsConform
                ) {
                listaAcoes.push(coletaJson[i]);
            }
        }
    }

    montaCorpoFormularioAcao(listaAcoes, 0);
}

function montaCorpoFormularioAcao(listaAcoes, index) {

    var options = '<option value="">Selecione...</option>';

    parametrization.listaAuditor.forEach(function (parMultipleValue) {
        options += '<option value="' + parMultipleValue.Id + '">' + parMultipleValue.Name + '</option>';
    });

    if (listaAcoes.length > 1) {

        btnNext = '<div class="col-sm-3">' +
            '<button class="btn btn-primary" id="next" onclick="proximoElemento(' + index + ')" style="float:right;">Próximo Alerta' +
            ' (' + currentQtdAcoes + "/" + listaAcoes.length + ')</button>' +
            '</div>';

        btnBack = '<div class="col-sm-3">' +
            '<button class="btn btn-primary" id="back" onclick="elementoAnterior(' + index + ')" style="float:left;">Voltar Alerta</button>' +
            '</div>';
    }

    var date = getCurrentDate();

    var htmlAcao = `<div class="container"><div id="bodyModalAcao" style="display:block;">
   <h3 style="font-weight:bold;">Criar Ação</h3>
   <hr>
   <div class="form-group">
      <div class="form-group col-xs-12" style="border: 2px;border-color: azure;border-style: groove;">
         <label class="col-md-4">Data Emissão: ${currentCollectDate.toLocaleDateString()}</label>
         <label class="col-md-3">Hora Emissão: ${currentCollectDate.toLocaleTimeString()}</label>
         <label>Emissor: ${currentLogin.Name}</label>
      </div>
      <div class="form-group col-xs-12" style="border: 2px;border-color: azure;border-style: groove;">
         <p id="actionParCompany_Id">Unidade: ${$.grep(currentLogin.ParCompanyXUserSgq, function (o, i) { return o.ParCompany.Id == currentParCompany_Id })[0].ParCompany.Name}</p>
         <p id="actionParDepartment_Id">Centro de Custo: ${$.grep(parametrization.listaParDepartment, function (o, i) { return o.Id == currentParDepartment_Id })[0].Name}</p>
         <p id="actionParDepartmentParent_Id">Seção/Atividade: ${$.grep(parametrization.listaParDepartment, function (o, i) { return o.Parent_Id == currentParDepartmentParent_Id; })[0].Name}</p>

         <p id="actionParCargo_Id">Item/Cargo: ${$.grep(parametrization.listaParCargo, function (o, i) { return o.Id == currentParCargo_Id })[0].Name} </p>

         <p id="actionParLevel1_Id"
            data-action-level1="${$.grep(parametrization.listaParLevel1, function (o, i) { return o.Id == listaAcoes[index].ParLevel1_Id; })[0].Id}">
            Indicador/Origem: ${$.grep(parametrization.listaParLevel1, function (o, i) { return o.Id == listaAcoes[index].ParLevel1_Id; })[0].Name}</p>

         <p id="actionParLevel2_Id" 
            data-action-level2="${$.grep(parametrization.listaParLevel2, function (o, i) { return o.Id == listaAcoes[index].ParLevel2_Id; })[0].Id}">
            Monitoramento: ${$.grep(parametrization.listaParLevel2, function (o, i) { return o.Id == listaAcoes[index].ParLevel2_Id; })[0].Name}</p>

         <p id="actionParLevel3_Id"
            data-action-level3="${$.grep(parametrization.listaParLevel3, function (o, i) { return o.Id == listaAcoes[index].ParLevel3_Id; })[0].Id}">Desvio/Tarefa: ${$.grep(parametrization.listaParLevel3, function (o, i) { return o.Id == listaAcoes[index].ParLevel3_Id; })[0].Name}</p>
      </div>

      <div class="form-group col-xs-12" style="">
         <p>Não Conformidade/Ocorrencia</p>
         <input id="txtActionNotConformity" class="form-control" type="text">
         <p>Ação</p>
         <input id="txtAction" class="form-control">
      </div>
      <hr>
      <div class="form-group col-xs-12">
         <p><i class="fa fa-camera" aria-hidden="true"></i> Evidencias de Não conformidade</p>
         <label>Ver e Agir</label>
         <input type="checkbox" id="checkVerAgir" />
         <p id="actionsEvidencies" hidden="hidden"><i class="fa fa-camera" aria-hidden="true"></i> Evidencias da Ação Concluida</p>
      </div>
   </div>
   <div class="form-group col-md-12">
      <div class="col-md-4">
         <label>Data da conclusão:</label>
         <input id="actionConclusionDate" type="date" min="${date[0]}" class="form-control">
      </div>
      <div class="col-md-4">
         <label>Hora da conclusão:</label>
         <input id="actionConclusionHour" type="time" class="form-control">
      </div>
      <div class="col-md-4">
         <label>referencia:</label>
         <input id="actionReference" type="text" class="form-control">
      </div>
      <div class="col-md-4">
         <label>Responsavel:</label>
         <select id="actionResponsable" class="form-control">
            ${options}
         </select>
      </div>
      <div class="col-md-4">
         <label>Notificar:</label>
         <select id="actionNotify" class="form-control">
           ${options}
         </select>
      </div>
   </div>
   <div class="col-md-12">
      <div class="col-md-6">
         <button class="btn btn-success" id="btnSave">Salvar</button>
         <button class="btn btn-info" id="btnCloseModal">Fechar</button>
        ${btnNext}
        ${btnBack}
      </div>
   </div>
</div></div>`;

    openModal(htmlAcao, 'white', 'black');

    if (listaObjAcoes != undefined && listaObjAcoes <= 0) {
        listaAcoes.map(function (o, i) {
            createOrUpdateObj(i);
        });
    }

    var currentAction = $.grep(listaObjAcoes, function (o, i) {
        return o.Id == index;
    });

    if (currentAction.length > 0) {
        setCurrentActionValues(currentAction);
    }
}

function elementoAnterior(indexDaListaDeAlerta) {
    if (indexDaListaDeAlerta > 0 && currentQtdAcoes > indexDaListaDeAlerta) {

        createOrUpdateObj(indexDaListaDeAlerta);

        currentQtdAcoes--;
        indexDaListaDeAlerta = indexDaListaDeAlerta - 1;
        montaCorpoFormularioAcao(listaAcoes, indexDaListaDeAlerta);
    }
}

function proximoElemento(indexDaListaDeAlerta) {
    if (indexDaListaDeAlerta < currentQtdAcoes && currentQtdAcoes <= listaAcoes.length) {

        createOrUpdateObj(indexDaListaDeAlerta);

        currentQtdAcoes++;
        indexDaListaDeAlerta++;
        montaCorpoFormularioAcao(listaAcoes, indexDaListaDeAlerta);
    }
}

function createOrUpdateObj(indexDaListaDeAlerta) {

    var currentObjAction = $.grep(listaObjAcoes, function (o, i) {
        return o.Id == indexDaListaDeAlerta;
    });

    if (currentObjAction.length == 0)
        setListaAcoesObj(indexDaListaDeAlerta, null);
    else
        setListaAcoesObj(indexDaListaDeAlerta, currentObjAction);
}

function setListaAcoesObj(indexDaListaDeAlerta, currentObjAction) {

    if (currentObjAction == null) {

        var actionObj = {
            Id:                      indexDaListaDeAlerta,
            ParCompany_Id:           currentParCompany_Id,
            ParDepartment_Id:        currentParDepartment_Id,
            ParDepartmentParent_Id:  currentParDepartmentParent_Id,
            ParCargo_Id:             parCargo_Id,
            ParLevel3_Id:            $("#actionParLevel3_Id").attr('data-action-level3'),
            ParLevel1_Id:            $("#actionParLevel1_Id").attr('data-action-level1'),
            ParLevel2_Id:            $("#actionParLevel2_Id").attr('data-action-level2'),
            Acao_Naoconformidade:    $("#txtActionNotConformity").val(),
            AcaoText:                    $("#txtAction").val(),
            DataConclusao:           $("#actionConclusionDate").val(),
            HoraConclusao:           $("#actionConclusionHour").val(),
            Referencia:              $('#actionReference').val(),
            Responsavel:             $('#actionResponsable :selected').val(),
            Notificar:               $("#actionNotify :selected").val(),
            DataEmissao:             currentCollectDate.toLocaleDateString(),
            HoraEmissao:             currentCollectDate.toLocaleTimeString(),
            Emissor:                 currentLogin.Id
        };

        listaObjAcoes.push(actionObj);

    } else {
        listaObjAcoes[indexDaListaDeAlerta].Id                      = indexDaListaDeAlerta;
        listaObjAcoes[indexDaListaDeAlerta].ParCompany_Id           = currentParCompany_Id;
        listaObjAcoes[indexDaListaDeAlerta].ParDepartment_Id        = currentParDepartment_Id;
        listaObjAcoes[indexDaListaDeAlerta].ParCargo_Id             = parCargo_Id;
        listaObjAcoes[indexDaListaDeAlerta].ParDepartmentParent_Id  = currentParDepartmentParent_Id;
        listaObjAcoes[indexDaListaDeAlerta].ParLevel3_Id            = $("#actionParLevel3_Id").attr('data-action-level3');
        listaObjAcoes[indexDaListaDeAlerta].ParLevel1_Id            = $("#actionParLevel1_Id").attr('data-action-level1');
        listaObjAcoes[indexDaListaDeAlerta].ParLevel2_Id            = $("#actionParLevel2_Id").attr('data-action-level2');
        listaObjAcoes[indexDaListaDeAlerta].Acao_Naoconformidade    = $("#txtActionNotConformity").val();
        listaObjAcoes[indexDaListaDeAlerta].AcaoText                    = $("#txtAction").val();
        listaObjAcoes[indexDaListaDeAlerta].DataConclusao           = $("#actionConclusionDate").val();
        listaObjAcoes[indexDaListaDeAlerta].HoraConclusao           = $("#actionConclusionHour").val();
        listaObjAcoes[indexDaListaDeAlerta].Referencia              = $('#actionReference').val();
        listaObjAcoes[indexDaListaDeAlerta].Responsavel             = $('#actionResponsable :selected').val();
        listaObjAcoes[indexDaListaDeAlerta].Notificar               = $("#actionNotify :selected").val();
        listaObjAcoes[indexDaListaDeAlerta].DataEmissao             = currentCollectDate.toLocaleDateString();
        listaObjAcoes[indexDaListaDeAlerta].HoraEmissao             = currentCollectDate.toLocaleTimeString();
        listaObjAcoes[indexDaListaDeAlerta].Emissor                 = currentLogin.Id;
    }
}

function setCurrentActionValues(currentAction) {

    $("#txtActionNotConformity").val(currentAction[0].Acao_Naoconformidade);
    $("#txtAction").val(currentAction[0].AcaoText);
    $("#actionConclusionDate").val(currentAction[0].DataConclusao);
    $("#actionConclusionHour").val(currentAction[0].HoraConclusao);
    $("#actionReference").val(currentAction[0].Referencia);
    $("#actionResponsable").val(currentAction[0].Responsavel);
    $("#actionNotify").val(currentAction[0].Notificar);

}

function saveAction(){
    $.ajax({
        data: JSON.stringify(listaObjAcoes),
        url: urlPreffix + '/api/AppColeta/SetAction',
        type: 'POST',
        contentType: "application/json",
        success: function (data) {
            console.log('ações salvas');
            currentQtdAcoes = 1;
        },
        timeout: 600000,
        error: function () {
            enviarColetaEmExecucao = false;
        }
    });

}

$('body').off('click', '#btnSave').on('click', '#btnSave', function () {

    saveAction();
});

$('body').off('click', '#btnCloseModal').on('click', '#btnCloseModal', function () {
    listaAcoes = [];
    listaObjAcoes = [];
    currentQtdAcoes = 1;
    saveAction();
    closeModal();
});

$('body').off('click', '#checkVerAgir').on('click', '#checkVerAgir', function () {
    if ($(this).is(":checked")) {
        $("#actionsEvidencies").removeAttr('hidden');
    } else {
        $("#actionsEvidencies").attr('hidden', 'hidden');
    }
});