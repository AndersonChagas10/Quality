
var listaAcoes = [];
var listaObjAcoes = [];

function processAction(coletaJson) {

    var listaLevel1Acao = $.grep(parametrization.listaParLevel1, function (o, i) {
        return o.GenerateActionOnNotConformity;
    });

    coletaJson.forEach(function (coleta) {

        if (!coleta.hasOwnProperty('ParHeaderField_Id') && !coleta.IsConform) {

            parlevel1 = $.grep(listaLevel1Acao, function (level1) {
                return level1.Id == parseInt(coleta.ParLevel1_Id);
            });

            if (parlevel1.length)
                listaAcoes.push(coleta);
        }
    });

    montaObjAcao(listaAcoes);

    montaCorpoFormularioAcao(listaObjAcoes, 0);
}

function montaObjAcao(listaAcoes) {

    if (listaObjAcoes != undefined && listaObjAcoes <= 0) {
        listaAcoes.forEach(function (o, i) {
            createOrUpdateObj(i);
        });
    }

}

function montaCorpoFormularioAcao(listaObjAcoes, index) {

    if (listaObjAcoes.length <= 0) {
        fecharModalAcao();
        return;
    }

    //se não tiver proximo indice, volta no 0
    if (listaObjAcoes[index] == undefined) {
        index = 0;
    }

    var currentAction = listaObjAcoes[index];

    var options = '<option value="">Selecione...</option>';

    parametrization.listaAuditor.forEach(function (auditor) {
        options += '<option value="' + auditor.Id + '">' + auditor.Name + " (" + auditor.SimpleDescription + ")" + '</option>';
    });

    btnNext = '<button class="btn btn-primary" id="next" onclick="proximoElemento(' + index + ')">Próximo Alerta (' + (index + 1) + "/" + listaObjAcoes.length + ')</button>';
    btnBack = '<button class="btn btn-primary" id="back" style="margin-right: 10px;" onclick="elementoAnterior(' + index + ')">Voltar Alerta</button>';

    var date = getCurrentDate();

    var unidade = $.grep(currentLogin.ParCompanyXUserSgq, function (o, i) {
        return o.ParCompany.Id == currentParCompany_Id
    })[0].ParCompany.Name;

    var centroCusto = $.grep(parametrization.listaParDepartment, function (o, i) {
        return o.Id == currentParDepartment_Id
    })[0].Name;

    var secaoAtividade = $.grep(parametrization.listaParDepartment, function (o, i) {
        return o.Parent_Id == currentParDepartmentParent_Id;
    })[0].Name;

    var itemCargo = $.grep(parametrization.listaParCargo, function (o, i) {
        return o.Id == currentParCargo_Id
    })[0].Name;

    var level1 = $.grep(parametrization.listaParLevel1, function (o, i) {
        return o.Id == currentAction.ParLevel1_Id;
    })[0];

    var level2 = $.grep(parametrization.listaParLevel2, function (o, i) {
        return o.Id == currentAction.ParLevel2_Id;
    })[0];

    var level3 = $.grep(parametrization.listaParLevel3, function (o, i) {
        return o.Id == currentAction.ParLevel3_Id;
    })[0];

    var htmlAcao = '<div class="container-fluid">' +
        '<div id="bodyModalAcao" style="display:block;">' +
        '<h3 style="font-weight:bold;">Criar Ação</h3>' +
        '<hr>' +
        '<div class="form-group">' +
        '   <div class="row" style="border: 2px;border-color: azure;border-style: groove;">' +
        '       <label class="col-xs-4">Data Emissão: ' + currentCollectDate.toLocaleDateString() + '</label>' +
        '       <label class="col-xs-4">Hora Emissão: ' + currentCollectDate.toLocaleTimeString() + '</label>' +
        '       <label class="col-xs-4">Emissor: ' + currentLogin.Name + '</label>' +
        '   </div>' +
        '   <div class="form-group row" style="border: 2px;border-color: azure;border-style: groove;">' +
        '       <div class="col-xs-12">' +
        '           <p id="actionParCompany_Id">Unidade: ' + unidade + '</p>' +
        '           <p id="actionParDepartment_Id">Centro de Custo: ' + centroCusto + '</p>' +
        '           <p id="actionParDepartmentParent_Id">Seção/Atividade: ' + secaoAtividade + '</p>' +
        '           <p id="actionParCargo_Id">Item/Cargo: ' + itemCargo + ' </p>' +
        '           <p id="actionParLevel1_Id" data-action-level1="' + level1.Id + '"> Indicador/Origem: ' + level1.Name + '</p>' +
        '           <p id="actionParLevel2_Id" data-action-level2="' + level2.Id + '"> Monitoramento: ' + level2.Name + '</p>' +
        '           <p id="actionParLevel3_Id" data-action-level3="' + level3.Id + '"> Desvio/Tarefa: ' + level3.Name + '</p>' +
        '       </div>' +
        '   </div>' +
        '   <div class="form-group row" style="">' +
        '       <div class="col-xs-12">' +
        '           <p>Não Conformidade/Ocorrencia</p>' +
        '           <textarea id="txtActionNotConformity" class="form-control" style="resize: none; height: 100px;"></textarea>' +
        '           <p>Ação</p>' +
        '           <textarea id="txtAction" class="form-control" style="resize: none; height: 100px;"></textarea>' +
        '       </div>' +
        '   </div>' +
        '   <hr>' +
        '   <div class="form-group row">' +
        '       <div class="col-xs-12">' +
        '           <p><i class="fa fa-camera" aria-hidden="true"></i> Evidencias de Não conformidade</p>' +
        '           <label>Ver e Agir</label>' +
        '           <input type="checkbox" id="checkVerAgir" />' +
        '           <p id="actionsEvidencies" hidden="hidden"><i class="fa fa-camera" aria-hidden="true"></i> Evidencias da Ação Concluida</p>' +
        '       </div>' +
        '   </div>' +
        '</div>' +
        '<div class="form-group row">' +
        '   <div class="col-md-4">' +
        '       <label>Data da conclusão:</label>' +
        '       <input id="actionConclusionDate" type="date" min="' + date.split('T')[0] + '" class="form-control">' +
        '   </div>' +
        '   <div class="col-md-4">' +
        '       <label>Hora da conclusão:</label>' +
        '       <input id="actionConclusionHour" type="time" class="form-control">' +
        '   </div>' +
        '   <div class="col-md-4">' +
        '       <label>referencia:</label>' +
        '       <input id="actionReference" type="text" class="form-control">' +
        '   </div>' +
        '   <div class="col-md-4">' +
        '       <label>Responsavel:</label>' +
        '       <select id="actionResponsable" class="form-control">' +
        '           ' + options +
        '       </select>' +
        '   </div>' +
        '   <div class="col-md-4">' +
        '       <label>Notificar:</label>' +
        '       <select id="actionNotify" class="form-control">' +
        '           ' + options +
        '       </select>' +
        '   </div>' +
        '</div>' +
        '<hr>' +
        '<div class="row">' +
        '   <div class="col-md-6">' +
        '   ' + btnBack +
        '   ' + btnNext +
        '   </div>' +
        '   <div class="col-md-6">' +
        '       <button class="btn btn-success pull-right" style="margin-right: 10px;" onclick="saveAction(' + index + ');">Salvar esta ação</button>' +
        '   </div>' +
        '</div>' +
        '</div></div>';

    openModal(htmlAcao, 'white', 'black');

    if (currentAction != null) {
        setCurrentActionValues(currentAction);
    }
}

function elementoAnterior(index) {

    if (listaObjAcoes.length > 1) {
        createOrUpdateObj(index);
        index--;
        montaCorpoFormularioAcao(listaObjAcoes, index);
    }

}

function proximoElemento(index) {

    if (listaObjAcoes.length > 1) {
        createOrUpdateObj(index);
        index++;
        montaCorpoFormularioAcao(listaObjAcoes, index);
    }
    
}

function createOrUpdateObj(index) {

    return setListaAcoesObj(index, listaObjAcoes[index]);

}

function setListaAcoesObj(index, currentObjAction) {

    if (currentObjAction == null) {

        var actionObj = {
            ParCompany_Id: listaAcoes[index].ParCompany_Id,
            ParDepartment_Id: listaAcoes[index].ParDepartment_Id,
            ParDepartmentParent_Id: currentParDepartmentParent_Id,
            ParCargo_Id: listaAcoes[index].ParCargo_Id,
            ParLevel1_Id: parseInt(listaAcoes[index].ParLevel1_Id),
            ParLevel2_Id: parseInt(listaAcoes[index].ParLevel2_Id),
            ParLevel3_Id: parseInt(listaAcoes[index].ParLevel3_Id),
            Acao_Naoconformidade: "",
            AcaoText: "",
            DataConclusao: "",
            HoraConclusao: "",
            Referencia: "",
            Responsavel: "",
            Notificar: "",
            DataEmissao: "",
            HoraEmissao: "",
            Emissor: currentLogin.Id
        };

        listaObjAcoes.push(actionObj);

    } else {
        listaObjAcoes[index].Acao_Naoconformidade = $("#txtActionNotConformity").val();
        listaObjAcoes[index].AcaoText = $("#txtAction").val();
        listaObjAcoes[index].DataConclusao = $("#actionConclusionDate").val();
        listaObjAcoes[index].HoraConclusao = $("#actionConclusionHour").val();
        listaObjAcoes[index].Referencia = $('#actionReference').val();
        listaObjAcoes[index].Responsavel = $('#actionResponsable :selected').val();
        listaObjAcoes[index].Notificar = $("#actionNotify :selected").val();
        listaObjAcoes[index].DataEmissao = currentCollectDate.toLocaleDateString();
        listaObjAcoes[index].HoraEmissao = currentCollectDate.toLocaleTimeString();
        listaObjAcoes[index].Emissor = currentLogin.Id;
    }

    return listaObjAcoes[index];
}

function setCurrentActionValues(currentAction) {

    $("#txtActionNotConformity").val(currentAction.Acao_Naoconformidade);
    $("#txtAction").val(currentAction.AcaoText);
    $("#actionConclusionDate").val(currentAction.DataConclusao);
    $("#actionConclusionHour").val(currentAction.HoraConclusao);
    $("#actionReference").val(currentAction.Referencia);
    $("#actionResponsable").val(currentAction.Responsavel);
    $("#actionNotify").val(currentAction.Notificar);

}

function saveAction(index) {

    var objCriado = createOrUpdateObj(index);

    $.ajax({
        data: JSON.stringify([objCriado]),
        url: urlPreffix + '/api/AppColeta/SetAction',
        type: 'POST',
        contentType: "application/json",
        success: function (data) {
            console.log('ações salvas');
            removeActionList(index);
            montaCorpoFormularioAcao(listaObjAcoes, index);
        },
        timeout: 600000,
        error: function () {
            console.log('erro ao salvar ações');
        }
    });

}

function fecharModalAcao() {
    listaAcoes = [];
    listaObjAcoes = [];
    closeModal();
}

function removeActionList(index) {
    listaObjAcoes.splice(index, 1);
}

function getActionById(indexId) {

    var objAcao = $.grep(listaObjAcoes, function (objAcao) {
        return objAcao.Id == indexId;
    });

    if (objAcao.length > 0)
        return objAcao[0];

    return null;

}

$('body').off('click', '#checkVerAgir').on('click', '#checkVerAgir', function () {
    if ($(this).is(":checked")) {
        $("#actionsEvidencies").removeAttr('hidden');
    } else {
        $("#actionsEvidencies").attr('hidden', 'hidden');
    }
});

$('body').off('keyup', '#actionConclusionDate').on('keyup', '#actionConclusionDate', function () {

    var dataInput = $(this).val();

    debugger

    if (dataInput) {
        dataInput = parseInt(dataInput.replace('-', '').replace('-', ''))

        var dataHoje = parseInt(getCurrentDate().split('T')[0].replace('-', '').replace('-', ''));

        if (dataInput < dataHoje) {
            $(this).val(getCurrentDate().split('T')[0]);
        }

    }

});