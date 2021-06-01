var listaAcoes = [];
var listaAcoesCurrent = [];
var listaAcoesToSend = [];

function processAction(coletaJson) {

    var listaLevel1Acao = $.grep(parametrization.listaParLevel1, function (o, i) {
        return o.GenerateActionOnNotConformity;
    });
    
    var index = getNextIndex();

    coletaJson.forEach(function (coleta) {

        if (!coleta.hasOwnProperty('ParHeaderField_Id') && !coleta.IsConform) {

            parlevel1 = $.grep(listaLevel1Acao, function (level1) {
                return level1.Id == parseInt(coleta.ParLevel1_Id);
            });

            if (parlevel1.length) {
                createObjAcao(index, coleta);
                index++;
            }
        }
    });

    //atualizar arquivos de acoes pendentes (listaAcoes)
    writeActionFile();

    openAction();

}

function openAction() {

    listaAcoesCurrent = [];

    montaAcoesCurrent();

    montaCorpoFormularioAcao(0);

}

function montaAcoesCurrent() {
    listaAcoesCurrent = $.grep(listaAcoes, function (acao) {
        return acao.ParDepartment_Id == currentParDepartment_Id &&
        acao.ParCompany_Id == currentParCompany_Id &&
        acao.ParDepartmentParent_Id == currentParDepartmentParent_Id &&
        acao.ParCluster_Id == currentParCluster_Id &&
        acao.ParClusterGroup_Id == currentParClusterGroup_Id &&
        acao.ParCargo_Id == currentParCargo_Id &&
        acao.ParFrequency_Id == currentParFrequency_Id;
    });
}

function montaCorpoFormularioAcao(index) {

    if (listaAcoesCurrent.length <= 0) {
        closeModal();
        return;
    }

    //se não tiver proximo indice, volta no 0
    if (listaAcoesCurrent[index] == undefined) {
        index = 0;
    }

    var currentAction = listaAcoesCurrent[index];

    var options = '<option value="">Selecione...</option>';

    parametrization.listaAuditor.forEach(function (auditor) {
        options += '<option value="' + auditor.Id + '">' + auditor.Name + " (" + auditor.SimpleDescription + ")" + '</option>';
    });

    var btnClass = "";
    if (listaAcoesCurrent.length == 1) {
        btnClass = "btn btn-primary disabled";
    } else {
        btnClass = "btn btn-primary";
    }

    btnNext = '<button class="' + btnClass +'" id="next" onclick="proximoElemento(' + index + ')">Próximo Alerta (' + (index + 1) + "/" + listaAcoesCurrent.length + ')</button>';
    btnBack = '<button class="' + btnClass +'" id="back" onclick="elementoAnterior(' + index + ')">Voltar Alerta</button>';

    var date = getCurrentDate();

    var usersNotfy = "";
    currentAction.Notificar.forEach(function (auditor_Id) {
        
        var name = $.grep(parametrization.listaAuditor, function (auditor) {
            return auditor.Id == auditor_Id;
        })[0].Name

        usersNotfy += '<tr><td>' + auditor_Id + '</td><td>' + name + '</td><td><button class="btn btn-danger btn-sm" onclick="removeUserNotify(' + index + ',' + auditor_Id + ')">X</button></td></tr>';
    });


    var btnPhoto = "";
    var btnFile = "";

    if (currentAction.EvidenciaNaoConformidade.length < 2) {
        if (device.platform.toLowerCase() == "android" || device.platform.toLowerCase() == "windows") {
            btnPhoto = '<button class="fa fa-camera btn btn-default" onclick="tirarFotoAcao(' + index + ');" aria-hidden="true"></button>';
        }

        if (device.platform.toLowerCase() == "android" || device.platform.toLowerCase() == "windows") {
            btnFile = '<button class="fa fa-file btn btn-default" onclick="upoadFotoAcaoByLibrary(' + index + ')" aria-hidden="true"></button>';
        }
    }

    var btnPhotoConcluida = "";
    var btnFileConcluida = "";

    if (currentAction.EvidenciaAcaoConcluida.length < 2) {
        if (device.platform.toLowerCase() == "android" || device.platform.toLowerCase() == "windows") {
            btnPhotoConcluida = '<button class="fa fa-camera btn btn-default" onclick="tirarFotoAcaoConcluida(' + index + ');" aria-hidden="true"></button>';
        }

        if (device.platform.toLowerCase() == "android" || device.platform.toLowerCase() == "windows") {
            btnFileConcluida = '<button class="fa fa-file btn btn-default" onclick="upoadFotoAcaoConcluidaByLibrary(' + index + ')" aria-hidden="true"></button>';
        }
    }

    var imagensNC = "";
    var imagensEvidenciaConcluida = "";

    currentAction.EvidenciaNaoConformidade.forEach(function (imagem, i) {
        imagensNC += '<div class="col-xs-6 col-md-4">' +
            '<img src="data:image/jpeg;base64,' + imagem + '" class="img-responsive" style="width:100%" alt="Responsive image">' +
            '<button class="btn btn-danger btn-sm form-control" onclick="removePhotoAcao(' + index + ',' + i + ')">Excluir</button>' +
            '</div>';
    });
    
    currentAction.EvidenciaAcaoConcluida.forEach(function (imagem, i) {
        imagensEvidenciaConcluida += '<div class="col-xs-6 col-md-4">' +
            '<img src="data:image/jpeg;base64,' + imagem + '" class="img-responsive" style="width:100%" alt="Responsive image">' +
            '<button class="btn btn-danger btn-sm form-control" onclick="removePhotoAcaoConcluida(' + index + ',' + i + ')">Excluir</button>' +
            '</div>';
    });
    
    var htmlAcao = '<div class="container-fluid">' +
        '<div id="bodyModalAcao" style="display:block;">' +
        '<h3 style="font-weight:bold;">Criar Ação</h3>' +
        '<hr>' +
        '<div class="form-group">' +
        '   <div class="row" style="border: 2px;border-color: azure;border-style: groove;">' +
        '       <label class="col-xs-4">Data Emissão: ' + currentCollectDate.toLocaleDateString() + '</label>' +
        '       <label class="col-xs-4">Hora Emissão: ' + currentCollectDate.toLocaleTimeString() + '</label>' +
        '       <label class="col-xs-4">Emissor: ' + currentLogin.FullName + '</label>' +
        '   </div>' +
        '   <div class="form-group row" style="border: 2px;border-color: azure;border-style: groove;">' +
        '       <div class="col-xs-12">' +
        '           <p id="actionParCompany_Id">Unidade: ' + currentAction.ParCompany_Name + '</p>' +
        '           <p id="actionParDepartment_Id">Centro de Custo: ' + currentAction.ParDepartment_Name + '</p>' +
        '           <p id="actionParDepartmentParent_Id">Item/Tarefa: ' + currentAction.ParDepartmentParent_Name + '</p>' +
        '           <p id="actionParCargo_Id">Item/Cargo: ' + currentAction.ParCargo_Name + ' </p>' +
        '           <p id="actionParLevel1_Id" data-action-level1="' + currentAction.ParLevel1_Id + '"> Indicador/Origem: ' + currentAction.ParLevel1_Name + '</p>' +
        '           <p id="actionParLevel2_Id" data-action-level2="' + currentAction.ParLevel2_Id + '"> Monitoramento: ' + currentAction.ParLevel2_Name + '</p>' +
        '           <p id="actionParLevel3_Id" data-action-level3="' + currentAction.ParLevel3_Id + '"> Desvio/Medida de Controle: ' + currentAction.ParLevel3_Name + '</p>' +
        '       </div>' +
        '   </div>' +
        '   <div class="form-group row" style="">' +
        '       <div class="col-xs-12">' +
        '           <p>Não Conformidade/Ocorrência</p>' +
        '           <textarea id="txtActionNotConformity" maxlength="900" class="form-control" style="resize: none; height: 100px;"></textarea>' +
        '           <p>Ação</p>' +
        '           <textarea id="txtAction" class="form-control" maxlength="900" style="resize: none; height: 100px;"></textarea>' +
        '       </div>' +
        '   </div>' +
        '   <hr>' +
        '   <div class="form-group row">' +
        '       <div class="col-xs-12">' +
        '           <p>Evidências de Não Conformidade </p>' +
        '           ' + btnPhoto +
        '           ' + btnFile +
        '           <br><br> ' +
        '           <div class="col-xs-12" style="display:table !important;">' + imagensNC + '</div>' +
        '       </div>' +
        '       <hr>' +
        '       <div class="col-xs-12">' +
        '           <p>Ver e Agir <input type="checkbox" onclick="updateAcaoCurrent(' + index + ')" id="checkVerAgir"></p>' +
        '           <div id="actionsEvidencies">' +
        '               <p>Evidências da Ação Concluída</p>' +
        '               ' + btnPhotoConcluida +
        '               ' + btnFileConcluida +
        '               <br><br> ' +
        '               <div class="col-xs-12" style="display:table; !important">' + imagensEvidenciaConcluida + '</div>' +
        '           </div>' +
        '       </div>' +
        '       <hr>' +
        '   </div>' +
        '</div>' +
        '<div class="form-group row">' +
        '   <div class="col-xs-4 hide vereagir">' +
        '       <h4>Data da conclusão:</h4>' +
        '       <input id="actionConclusionDate" type="date" min="' + date.split('T')[0] + '" class="form-control">' +
        '   </div>' +
        '   <div class="col-xs-4 hide vereagir">' +
        '       <h4>Hora da conclusão:</h4>' +
        '       <input id="actionConclusionHour" type="time" class="form-control">' +
        '   </div>' +
        '   <div class="col-xs-4">' +
        '       <h4>Referência:</h4>' +
        '       <input id="actionReference" maxlength="200" type="text" class="form-control">' +
        '   </div>' +
        '   <div class="col-xs-4">' +
        '       <h4>Responsável:</h4>' +
        '       <select id="actionResponsable" class="form-control">' +
        '           ' + options +
        '       </select>' +
        '   </div>' +
        '   <div class="col-xs-4 divActionPriority">' +
        '       <h4>Prioridade:</h4>' +
        '       <select id="actionPriority" class="form-control">' +
        '           <option value="">Selecione...</option>' +
        '           <option value="1">Baixa</option>' +
        '           <option value="2">Média</option>' +
        '           <option value="3">Alta</option>' +
        '       </select>' +
        '   </div>' +
        '</div>' +
        '<div class="form-group row">' +
        '   <div class="col-xs-12 col-md-6">' +
        '       <h4>Notificar:</h4>' + 
        '       <div class="form-inline">' +
        '           <div class="form-group" style="width:60%">' +
        '               <select id="actionNotify" class="form-control">' +
        '                   ' + options +
        '               </select>' +
        '           </div>' +
        '           <button onclick="addUserNotify(0);" class="btn btn-primary">+</button>' +
        '       </div>' +
        '       <table id="tableActionNotify" class="table table-striped" style="font-size:14px;">' +
        '           <thead>' +
        '               <th>Id</th>' +
        '               <th>Nome</th>' +
        '               <th></th>' +
        '           </tr>' +
        '           </thead>' +
        '         ' + usersNotfy +
        '       </table>' +
        '   </div>' +
        '</div>' +
        '<hr>' +
        '<div class="row">' +
        '   <div class="col-xs-12">' +
        '   ' + btnBack +
        '   ' + btnNext +
        // '   </div>' +
        // '   <div class="col-xs-4">' +
        '       <button id="btnSalvarIniciar" class="btn btn-success" onclick="saveAction(' + index + ', 1);">Salvar e iniciar acao</button>' +
        '       <button class="btn btn-success" onclick="saveAction(' + index + ', 2);">Salvar e preencher depois</button>' +
        '   </div>' +
        '</div>' +
        '</div></div>';

    openModal(htmlAcao, 'white', 'black');

    setCurrentActionValues(currentAction);

}

function elementoAnterior(index) {

    if (listaAcoesCurrent.length > 1) {
        updateAcaoCurrent(index, true);
        index--;
        montaCorpoFormularioAcao(index);
    }

}

function proximoElemento(index) {

    if (listaAcoesCurrent.length > 1) {
        updateAcaoCurrent(index, true);
        index++;
        montaCorpoFormularioAcao(index);
    }
    
}

function getNextIndex() {

    if (listaAcoes.length == 0)
        return 1;

    var max = listaAcoes[0];

    for (var i = 1; i < listaAcoes.length; ++i) {
        if (listaAcoes[i].Id > max) {
            max = listaAcoes[i].Id;
        }
    }

    return max++;
}

function createObjAcao(index, coleta){

    var unidade = $.grep(currentLogin.ParCompanyXUserSgq, function (o, i) {
        return o.ParCompany.Id == coleta.ParCompany_Id
    })[0].ParCompany.Name;

    var centroCusto = $.grep(parametrization.listaParDepartment, function (o, i) {
        return o.Id == coleta.ParDepartment_Id;
    })[0].Name;

    var secaoAtividade = $.grep(parametrization.listaParDepartment, function (o, i) {
        return o.Parent_Id == currentParDepartmentParent_Id;
    })[0].Name;

    var itemCargo = $.grep(parametrization.listaParCargo, function (o, i) {
        return o.Id == coleta.ParCargo_Id;
    })[0].Name;

    var level1 = $.grep(parametrization.listaParLevel1, function (o, i) {
        return o.Id == coleta.ParLevel1_Id;
    })[0];

    var level2 = $.grep(parametrization.listaParLevel2, function (o, i) {
        return o.Id == coleta.ParLevel2_Id;
    })[0];

    var level3 = $.grep(parametrization.listaParLevel3, function (o, i) {
        return o.Id == coleta.ParLevel3_Id;
    })[0];

    var parCluster = $.grep(parametrization.listaParCluster, function (o, i) {
        return o.Id == coleta.ParCluster_Id;
    })[0];

    var parClusterGroup = $.grep(parametrization.listaParClusterGroup, function (o, i) {
        return o.Id == currentParClusterGroup_Id;
    })[0];

    var parFrequency = $.grep(parametrization.listaParFrequency, function (o, i) {
        return o.Id == coleta.Parfrequency_Id;
    })[0];

    var actionObj = {
        Id: index,
        ParCompany_Id: coleta.ParCompany_Id,
        ParCompany_Name: unidade,
        ParDepartment_Id: coleta.ParDepartment_Id,
        ParDepartment_Name: centroCusto,
        ParDepartmentParent_Id: currentParDepartmentParent_Id,
        ParDepartmentParent_Name: secaoAtividade,
        ParCargo_Id: coleta.ParCargo_Id,
        ParCargo_Name: itemCargo,
        ParLevel1_Id: parseInt(coleta.ParLevel1_Id),
        ParLevel1_Name: level1.Name,
        ParLevel2_Id: parseInt(coleta.ParLevel2_Id),
        ParLevel2_Name: level2.Name,
        ParLevel3_Id: parseInt(coleta.ParLevel3_Id),
        ParLevel3_Name: level3.Name,
        ParCluster_Id: coleta.ParCluster_Id,
        ParCluster_Name: parCluster.Name,
        ParClusterGroup_Id: parClusterGroup.Id,
        ParClusterGroup_Name: parClusterGroup.Name,
        ParFrequency_Id: parFrequency.Id,
        ParFrequency_Name: parFrequency.Name,
        Acao_Naoconformidade: "",
        AcaoText: "",
        DataConclusao: "",
        HoraConclusao: "",
        Referencia: "",
        Responsavel: "",
        Notificar: [],
        DataEmissao: "",
        HoraEmissao: "",
        Prioridade: "",
        EvidenciaNaoConformidade: [],
        EvidenciaAcaoConcluida: [],
        VerEAgir: false,
        Emissor: currentLogin.Id
    };

    listaAcoes.push(actionObj);
}

function setListaAcoesObj(index, currentObjAction) {

    currentObjAction.Acao_Naoconformidade = $("#txtActionNotConformity").val();
    currentObjAction.AcaoText = $("#txtAction").val();
    currentObjAction.DataConclusao = $("#actionConclusionDate").val();
    currentObjAction.HoraConclusao = $("#actionConclusionHour").val();
    currentObjAction.Referencia = $('#actionReference').val();
    currentObjAction.Responsavel = $('#actionResponsable :selected').val();
    currentObjAction.DataEmissao = currentCollectDate.toJSON();
    currentObjAction.HoraEmissao = currentCollectDate.toLocaleTimeString();
    currentObjAction.Emissor = currentLogin.Id;
    currentObjAction.Prioridade = $('#actionPriority :selected').val();
    currentObjAction.VerEAgir = $('#checkVerAgir').prop('checked');

    listaAcoesCurrent[index] = currentObjAction;

    return currentObjAction;
}

function setCurrentActionValues(currentAction) {

    if (!currentAction.VerEAgir)
        currentAction.EvidenciaAcaoConcluida = [];

    $("#txtActionNotConformity").val(currentAction.Acao_Naoconformidade);
    $("#txtAction").val(currentAction.AcaoText);
    $("#actionConclusionDate").val(currentAction.DataConclusao);
    $("#actionConclusionHour").val(currentAction.HoraConclusao);
    $("#actionReference").val(currentAction.Referencia);
    $("#actionResponsable").val(currentAction.Responsavel);
    $("#actionPriority").val(currentAction.Prioridade);
    $('#checkVerAgir').prop('checked', currentAction.VerEAgir).trigger('change');

}

function retornaStatusAcao(objCriado, status) {

    //1 Pendente
    //2 Em andamento 
    //3 Concluída
    //4 Atrasada 
    //5 Cancelada

    if (objCriado.VerEAgir)
        return 3;
    else
        return status;

}

function updateAcaoCurrent(index, writefile) {

    var objAlterado = setListaAcoesObj(index, listaAcoesCurrent[index]);

    updateAcao(objAlterado);

    if (writefile == true)
        writeActionFile();

    return objAlterado;
}

function updateAcao(objAlterado) {

    var index = $.map(listaAcoes, function(acao){
        return acao.Id;
    }).indexOf(objAlterado.Id);

    listaAcoes[index] = objAlterado;
}

function saveAction(index, status) {

    var objCriado = updateAcaoCurrent(index, false);
    objCriado.Status = retornaStatusAcao(objCriado, status);

    listaAcoesToSend.push(objCriado);
    removeActionCurrentList(index);

    openMensagem('Ação salva.', 'blue', 'white');
    closeMensagem(2000);

    montaCorpoFormularioAcao(index);

}

function removeActionCurrentList(index) {
    removeActionList(listaAcoesCurrent[index]);
    listaAcoesCurrent.splice(index, 1);
}

function removeActionList(objRemovido) {

    var index = $.map(listaAcoes, function(acao){
        return acao.Id;
    }).indexOf(objRemovido.Id);

    listaAcoes.splice(index, 1);

    writeActionFile();
}

function addUserNotify(index) {

    var userNotify = $('#actionNotify :selected').val();

    if (!userNotify)
        return;

    if (listaAcoesCurrent[index].Notificar.indexOf(userNotify) < 0) {
        updateAcaoCurrent(index, true);
        listaAcoesCurrent[index].Notificar.push(userNotify);
        montaCorpoFormularioAcao(index);
    }
    else
        alert("usuário já adicionado");
}

function removeUserNotify(index, user_Id) {
    updateAcaoCurrent(index, true);
    listaAcoesCurrent[index].Notificar.splice(listaAcoesCurrent[index].Notificar.indexOf(user_Id), 1);
    montaCorpoFormularioAcao(index);
}

var indexAcaoFoto = 0;
function tirarFotoAcao(index) {
    indexAcaoFoto = index;
    cameraOptions.sourceType = Camera.PictureSourceType.CAMERA;
    abrirCamera(addPhotoAcao, cameraError, cameraOptions);
}

function tirarFotoAcaoConcluida(index) {
    indexAcaoFoto = index;
    cameraOptions.sourceType = Camera.PictureSourceType.CAMERA;
    abrirCamera(addPhotoAcaoConcluida, cameraError, cameraOptions);
}

function upoadFotoAcaoByLibrary(index) {
    cameraOptions.sourceType = Camera.PictureSourceType.PHOTOLIBRARY;
    indexAcaoFoto = index;
    abrirCamera(addPhotoAcao, cameraError, cameraOptions);

}

function upoadFotoAcaoConcluidaByLibrary(index) {
    cameraOptions.sourceType = Camera.PictureSourceType.PHOTOLIBRARY;
    indexAcaoFoto = index;
    abrirCamera(addPhotoAcaoConcluida, cameraError, cameraOptions);
}

function addPhotoAcao(imageData) {
    listaAcoesCurrent[indexAcaoFoto].EvidenciaNaoConformidade.push(imageData);
    montaCorpoFormularioAcao(indexAcaoFoto);
}

function addPhotoAcaoConcluida(imageData) {
    listaAcoesCurrent[indexAcaoFoto].EvidenciaAcaoConcluida.push(imageData);
    montaCorpoFormularioAcao(indexAcaoFoto);
}

function removePhotoAcao(index, indexAcaoFoto) {
    navigator.notification.confirm("Deseja realmente excluir a imagem?", function (number) {
        if (number == 1) {
            listaAcoesCurrent[index].EvidenciaNaoConformidade.splice(indexAcaoFoto, 1);
            montaCorpoFormularioAcao(index);
        }
    }, "Excluir ?", ["Sim", "Não"]);
}

function removePhotoAcaoConcluida(index, indexAcaoFoto) {
    navigator.notification.confirm("Deseja realmente excluir a imagem?", function (number) {
        if (number == 1) {
            listaAcoesCurrent[index].EvidenciaAcaoConcluida.splice(indexAcaoFoto, 1);
            montaCorpoFormularioAcao(index);
        }
    }, "Excluir ?", ["Sim", "Não"]);
}

$('body')
//.off('click, change', '#checkVerAgir')
.on('click, change', '#checkVerAgir', function () {

    var dataHoje = getCurrentDate().split('T');

    if ($(this).is(":checked")) {
        $("#actionsEvidencies").removeAttr('hidden');
        $('.vereagir').removeClass('hide');
        $('.divActionPriority').addClass('hide');
        $('#actionPriority').val("");
        $('#actionConclusionDate').val(dataHoje[0]);
        $('#actionConclusionHour').val(dataHoje[1].substring(0, 5));
    } else {
        $("#actionsEvidencies").attr('hidden', 'hidden');
        $('.vereagir').addClass('hide');
        $('.divActionPriority').removeClass('hide');
        $('#actionConclusionDate').val("");
        $('#actionConclusionHour').val("");
    }
});

$('body')
// .off('keyup', '#actionConclusionDate')
.on('keyup', '#actionConclusionDate', function () {

    var dataInput = $(this).val();

    if (dataInput) {
        dataInput = parseInt(dataInput.replace('-', '').replace('-', ''))

        var dataHoje = parseInt(getCurrentDate().split('T')[0].replace('-', '').replace('-', ''));

        if (dataInput < dataHoje) {
            $(this).val(getCurrentDate().split('T')[0]);
        }
    }

});


//Ler o arquivo
function readActonFromFile() {

    _readFile("listaAcoes.txt", function (data) {
        if (data)
            listaAcoes = JSON.parse(data);

    });

}

//Escrever no arquivo
function writeActionFile() {

    _writeFile("listaAcoes.txt", JSON.stringify(listaAcoes));

}

function readActonToSendFromFile() {

    _readFile("listaAcoesToSend.txt", function (data) {
        if (data)
            listaAcoesToSend = JSON.parse(data);

    });

}

//Escrever no arquivo
function writeActonToSendFile(callback) {

    _writeFile("listaAcoesToSend.txt", JSON.stringify(listaAcoesToSend), callback);

}

/**
 * SetInterval para enviar acao 
 * 
 * Se tiver coleta para ser enviada, sair da função
 * 
 * se não tiver envia as acoes
 */

var isSendActions = false;
function sendActions() {

    //verificar também coletas para serem enviadas <= 0
    if (!appIsOnline || isSendActions || (listaAcoesToSend && listaAcoesToSend.length <= 0)) {
        return;
    }

    isSendActions = true;

    var acaoToSend = listaAcoesToSend[0];

    $.ajax({
        data: JSON.stringify(acaoToSend),
        url: urlPreffix + '/api/AppColeta/SetAction',
        type: 'POST',
        contentType: "application/json",
        success: function (data) {
            //remover acao da lista de acoesToSend
            removeListAcaoToSend(acaoToSend);
            isSendActions = false;
            writeActonToSendFile(sendActions);
            //console.log(data.Id);
        },
        timeout: 600000,
        error: function () {
            console.log('erro ao salvar ação');
            isSendActions = false;
        }
    });
}


function removeListAcaoToSend(data) {
    listaAcoesToSend = $.grep(listaAcoesToSend, function (acao) {
        return JSON.stringify(acao) !== JSON.stringify(data)
    });

}


//verificar se tem ação 
//grupo de cluster
function getAcoesByClusterGroup(clusterGroup_Id) {
    return $.grep(listaAcoes, function (acao) {
        return acao.ParCompany_Id == currentParCompany_Id &&
            acao.ParClusterGroup_Id == clusterGroup_Id;
    });
}

//cluster
function getAcoesByCluster(cluster_Id){
    return $.grep(listaAcoes, function (acao) {
        return acao.ParCompany_Id == currentParCompany_Id &&
            acao.ParClusterGroup_Id == currentParClusterGroup_Id &&
            acao.ParCluster_Id == cluster_Id;
    });
}

//indicador e frequencia
function getAcoesByLevel1AndFrequency(level1_Id, parFrequency_Id) {
    return $.grep(listaAcoes, function (acao) {
        return acao.ParCompany_Id == currentParCompany_Id &&
            acao.ParClusterGroup_Id == currentParClusterGroup_Id &&
            acao.ParCluster_Id == currentParCluster_Id &&
            acao.ParLevel1_Id == level1_Id && 
            acao.ParFrequency_Id == parFrequency_Id;
    });
}

//Seção
function getAcoesByParDepartment(parDepartment_Id) {

    var listaAcoesReturn = [];

    currentPlanejamento.forEach(function (planejamento) {

        var acao = $.grep(listaAcoes, function (acao) {
            return acao.ParCompany_Id == currentParCompany_Id &&
                acao.ParClusterGroup_Id == currentParClusterGroup_Id &&
                acao.ParCluster_Id == currentParCluster_Id &&
                acao.ParLevel1_Id == planejamento.indicador_Id &&
                acao.ParFrequency_Id == currentParFrequency_Id &&
                acao.ParDepartment_Id == parDepartment_Id &&
                acao.ParDepartmentParent_Id == currentParDepartment_Id;
        });

        if (acao.length > 0)
            listaAcoesReturn.push(acao);

    });

    return listaAcoesReturn;
}

//Centro de custo
function getAcoesByParDepartmentParent(parDepartmentParent_Id) {

    var listaAcoesReturn = [];

    currentPlanejamento.forEach(function (planejamento) {

        var acao = $.grep(listaAcoes, function (acao) {
            return acao.ParCompany_Id == currentParCompany_Id &&
                acao.ParClusterGroup_Id == currentParClusterGroup_Id &&
                acao.ParCluster_Id == currentParCluster_Id &&
                acao.ParLevel1_Id == planejamento.indicador_Id &&
                acao.ParFrequency_Id == currentParFrequency_Id &&
                acao.ParDepartmentParent_Id == parDepartmentParent_Id;
        });

        if (acao.length > 0)
            listaAcoesReturn.push(acao);

    });

    return listaAcoesReturn;
}

//cargo
function getAcoesByParCargo(parCargo_Id) {

    var listaAcoesReturn = [];

    currentPlanejamento.forEach(function (planejamento) {

        var acao = $.grep(listaAcoes, function (acao) {
            return acao.ParCompany_Id == currentParCompany_Id &&
                acao.ParClusterGroup_Id == currentParClusterGroup_Id &&
                acao.ParCluster_Id == currentParCluster_Id &&
                acao.ParLevel1_Id == planejamento.indicador_Id &&
                acao.ParFrequency_Id == currentParFrequency_Id &&
                acao.ParDepartment_Id == currentParDepartment_Id &&
                acao.ParDepartmentParent_Id == currentParDepartmentParent_Id &&
                acao.ParCargo_Id == parCargo_Id;
        });

        if (acao.length > 0)
            listaAcoesReturn.push(acao);

    });

    return listaAcoesReturn;
}

$("#txtActionNotConformity, #txtAction, #actionResponsable, #actionReference, #actionPriority, #actionNotify")
    .off('input change')
    .on('input change', function () {
    if ($("#txtActionNotConformity").val() != ""
        && $("#txtAction").val() != ""
        && $("#actionResponsable").val() != ""
        && $("#actionReference").val() == ""
        && $("#actionPriority").val() == ""
        && $("#tableActionNotify tbody").length == 0) {
        $("#btnSalvarIniciar").removeClass('disabled');
        console.log('habilitar botao salvar e iniciar');
    } else {
        $("#btnSalvarIniciar").addClass('disabled');
        console.log('monter desabilitado');
    }

});
