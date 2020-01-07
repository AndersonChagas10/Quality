var currentParCompany_Id;
var currentParFrequency_Id;
var parametrization = null;
var currentParDepartment_Id;
var currentParDepartmentParent_Id;
var currentParCargo_Id;
var globalColetasRealizadas = [];
var globalAcoesCorretivasRealizadas = [];
var currentLogin = {};
var globalLoginOnline = false;
var currentCollectDate = new Date();
var appIsOnline = false;
var currentAlerts = [];
var currentAlertsAgrupados = [];
var listaParFrequency = [];
var currentsParDepartments_Ids = [];
var currentPlanejamento = [];

var currentParLevel1_Id;
var currentParLevel2_Id;

var currentTotalEvaluationValue = 0;
var currentTotalSampleValue = 0;

//Script para exibir erros no Mobile
// window.onerror = function (errorMsg, url, lineNumber) {
//     alert('Error: ' + errorMsg + ' Script: ' + url + ' Line: ' + lineNumber);
// }

function onOpenAppColeta() {

    _readFile("login.txt", function (data) {
        if (typeof (data) != 'undefined' && data.length > 0)
            currentLogin = JSON.parse(data);
        currentParCompany_Id = currentLogin.ParCompany_Id;

        _readFile("appParametrization.txt", function (param) {
            if (typeof (param) != 'undefined' && param.length > 0) {
                parametrization = JSON.parse(param);
                listaParFrequency = parametrization.listaParFrequency;
            }
        });
    });
}

function getAppParametrization(frequencyId) {

    sincronizarResultado(frequencyId);

    if (frequencyId != currentParFrequency_Id) {

        currentParFrequency_Id = frequencyId;
        openMensagem('Por favor, aguarde até que seja feito o download do planejamento selecionado', 'blue', 'white');

        $.ajax({
            data: JSON.stringify({
                ParCompany_Id: currentParCompany_Id
                , ParFrequency_Id: currentParFrequency_Id
                , AppDate: currentCollectDate
            }),
            type: 'POST',
            url: urlPreffix + '/api/AppColeta/GetAppParametrization',
            contentType: "application/json",
            success: function (data) {
                data.currentParFrequency_Id = currentParFrequency_Id;
                _writeFile("appParametrization.txt", JSON.stringify(data), function () {
                    parametrization = data;
                    listarParDepartment(0);
                    closeMensagem();
                });
            },
            timeout: 600000,
            error: function () {
                $(this).html($(this).attr('data-initial-text'));
                closeMensagem();
            }
        });

    } else {
        openMensagem('Carregando parametrização', 'blue', 'white');
        _readFile("appParametrization.txt", function (data) {
            if (data)
                parametrization = JSON.parse(data);

            listarParDepartment(0);
            closeMensagem();
        });
    }
}

function sincronizarResultado(frequencyId) {

    openMensagem('Sincronizando resultado', 'blue', 'white');

    $.ajax({
        data: JSON.stringify({
            ParCompany_Id: currentParCompany_Id,
            ParFrequency_Id: currentParFrequency_Id,
            CollectionDate: convertDateToJson(currentCollectDate)
        }),
        url: urlPreffix + '/api/AppColeta/GetResults/',
        type: 'POST',
        contentType: "application/json",
        success: function (data) {
            coletasAgrupadas = data;
            AtualizarArquivoDeColetas();
            closeMensagem();
        },
        timeout: 600000,
        error: function () {
            closeMensagem();
        }
    });
}

function sincronizarColeta() {

    openMensagem('Iniciada sequencia forçada de sincronização', 'orange', 'white');
    enviarColeta();
    closeMensagem(2000);

}

function showAllGlobalVar() {

    console.log("ParCompany:" + currentParCompany_Id);
    console.log("Frequencia: " + currentParFrequency_Id);
    console.log("Departamento: " + currentParDepartment_Id);
    console.log("Cargo: " + currentParCargo_Id);
    console.log("parametrization: " + parametrization);
    console.log("currentParDepartmentParent_Id: " + currentParDepartmentParent_Id);
    console.log("globalColetasRealizadas: " + globalColetasRealizadas);
    console.log("globalAcoesCorretivasRealizadas: " + globalAcoesCorretivasRealizadas);
    console.log("currentLogin: " + currentLogin);
    console.log("globalLoginOnline: " + globalLoginOnline);
    console.log("appIsOnline: " + appIsOnline);
    console.log("currentAlerts: " + currentAlerts);
    console.log("currentAlertsAgrupados: " + currentAlertsAgrupados);
    console.log("listaParFrequency: " + listaParFrequency);
    console.log("currentsParDepartments_Ids: " + currentsParDepartments_Ids);
    console.log("currentPlanejamento: " + currentPlanejamento);

}

function openModalChangeDate() {

    var html = '<div class="form-group row">' +
        '<label for="exemplo">Data: </label>' +
        '<input id="appDate" type="date" class="form-control"/>' +
        '</br>' +
        '<button id="btnChangeDate" type="button" class="btn btn-primary" onclick="changeDate(this)">Alterar Data de coleta</button> | ' +
        '<button id="btnChangeDate" type="button" class="btn btn-primary" onclick="closeModal()">Cancelar</button>' +
        '</div>';

    openModal(html, 'White', 'black');

}

function changeDate(that) {

    var newDate = $(that).parent().find("#appDate").val();

    if (!newDate) {
        return false;
    }

    closeModal();

    if (!appIsOnline) {

        openModal("Você precisa estar online para alterar a data.", "blue", "white");
        closeModal(3000);
        return false;
    }

    if (enviarColetaEmExecucao) {
        openModal("Por favor, aguarde a sincronização das coletas e tente novamente.", "blue", "white");
        closeModal(3000);
        return false;
    }

    if (globalColetasRealizadas.length > 0) {

        setTimeout(function () {
            var titulo = "Não foi possível alterar a data.";
            var mensagem = "Existem coeltas não sincronizadas. Deseja sincronizar os dados?";

            openMessageConfirm(titulo, mensagem, sincronizarColeta, closeModal, "orange", "white");
        }, 500);
        return false;
    }

    openMensagem("Alterando data...", "blue", "White");

    _writeFile("appParametrization.txt", '', function () {
        var oldFrequency_Id = currentParFrequency_Id + 0;
        currentParFrequency_Id = 0;
        getPlanejamentoPorFrequencia(oldFrequency_Id);
        openParFrequency();
    });

    var horas = "00:00:00";

    currentCollectDate = new Date(newDate + " " + horas);

    closeModal(5000);
}

function setBreadcrumbs() {

    var breadcrumb = '<ol class="breadcrumb"><li><a onclick="validaRota(openMenu,null)">Inicio</a></li>';
    var breadcrumbLi = "";
    var isCurrent = true;

    if (currentParLevel2_Id) {
        breadcrumbLi = getBreadcrumb($.grep(parametrization.listaParLevel2, function (item) {
            return item.Id == currentParLevel2_Id;
        })[0].Name, 'validaRota(listarParLevel2)', isCurrent) + breadcrumbLi;

        isCurrent = false;
    }

    if (currentParLevel1_Id) {
        breadcrumbLi = getBreadcrumb($.grep(parametrization.listaParLevel1, function (item) {
            return item.Id == currentParLevel1_Id;
        })[0].Name, 'validaRota(listarParLevel1)', isCurrent) + breadcrumbLi;

        isCurrent = false;
    }

    // if (currentParFrequency_Id) {
    //     breadcrumbLi = getBreadcrumb($.grep(parametrization.listaParFrequency, function (item) {
    //         return item.Id == currentParFrequency_Id;
    //     })[0].Name, 'validaRota(listarParDepartment,0)', isCurrent) + breadcrumbLi;

    //     isCurrent = false;
    // }

    breadcrumb += breadcrumbLi + '</ol>';

    $('.panel-heading').prepend(breadcrumb);

}

function getBreadcrumb(text, link, isCurrent) {

    if (isCurrent) {
        isCurrent = false;

        return '<li class="active">' + text + '</a></li>';
    } else
        return '<li><a onclick="' + link + '">' + text + '</a></li>';

}

function ZeroSeForNaN(valor){
    return isNaN(valor) ? 0 : valor;
}

function TracoSeForNaN(valor){
    return isNaN(valor) ? '-' : valor;
}