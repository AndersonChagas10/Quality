var currentParCompany_Id;
var currentUserSgq_Id;
var currentParFrequency_Id;
var globalDicionarioEstatico = {};
var parametrization = null;
var currentParDepartment_Id;
var currentParDepartmentParent_Id;
var currentParCargo_Id;
var currentParCluster_Id;
var currentParClusterGroup_Id;
var globalColetasRealizadas = [];
var globalAcoesCorretivasRealizadas = [];
var currentLogin = {};
var globalLoginOnline = false;
var currentCollectDate = new Date();
var appIsOnline = false;
var currentAlerts = [];
var currentAlertsAgrupados = [];
var listaParFrequency = [];
var listaParClusterGroup = [];
var currentsParDepartments_Ids = [];
var currentPlanejamento = [];
var globalLogo = null;

var currentTotalEvaluationValue = 0;
var currentTotalSampleValue = 0;
var currentRedistributeWeight = false;
var currentIsPartialSave = false;
var currentBaixarGetResultadoAposEnviarOsDadosColetados = false;

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
                currentParCompany_Id = parametrization.ParCompany_Id;
                listaParFrequency = parametrization.listaParFrequency;
                atualizarVariaveisCurrent(parametrization);
            }
        });
    });
}

function atualizarVariaveisCurrent(parametrization) {
    currentParCluster_Id = parametrization.currentParCluster_Id;
    currentParFrequency_Id = parametrization.currentParFrequency_Id;
    currentParClusterGroup_Id = parametrization.currentParClusterGroup_Id;
}

function aposLimparDadosDaParametrizacao() {
    currentParCluster_Id = null;
    currentParFrequency_Id = null;
    currentParClusterGroup_Id = null;
}

function sincronizarResultado() {

    if(globalColetasRealizadas.length > 0){
        openMensagem('Favor solicitar a sincronização dos resultados após o termino do envio dos dados coletados.', 'yellow', 'black');
        closeMensagem(2000);
        return;
    }

    openMensagem('Sincronizando resultado', 'blue', 'white');
    currentBaixarGetResultadoAposEnviarOsDadosColetados = false;

    setTimeout(function(){
        $.ajax({
            data: JSON.stringify({
                ParCompany_Id: currentParCompany_Id,
                ParFrequency_Id: currentParFrequency_Id,
                ParCluster_Id: currentParCluster_Id,
                CollectionDate: convertDateToJson(currentCollectDate)
            }),
            url: urlPreffix + '/api/AppColeta/GetResults/',
            type: 'POST',
            contentType: "application/json",
            success: function (data) {
                coletasAgrupadas = data;
                orderBySampleEvaluation();
                AtualizarArquivoDeColetas();
                closeMensagem();
            },
            timeout: 600000,
            error: function () {
                closeMensagem();
            }
        });
    },5000);
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
        chamaGetAppParametrization();
    });

    var horas = "00:00:00";

    currentCollectDate = new Date(newDate + " " + horas);

    closeModal(5000);
}

function setBreadcrumbs() {

    var breadcrumb = '<ol class="breadcrumb"><li><a onclick="validaRota(openMenu,null)">Inicio</a></li>';
    var breadcrumbLi = "";
    var isCurrent = true;

    if (currentParCompany_Id) {

        breadcrumbLi += getBreadcrumb($.grep(currentLogin.ParCompanyXUserSgq, function (item) {
            return item.ParCompany_Id == currentParCompany_Id;
        })[0].ParCompany.Name, 'validaRota(listarParCompany,0)', false);

        isCurrent = false;
    }

    if (currentParClusterGroup_Id) {

        breadcrumbLi += getBreadcrumb($.grep(listaParClusterGroup, function (item) {
            return item.Id == currentParClusterGroup_Id;
        })[0].Name, 'validaRota(listarParClusterGroup,0)', false);

        isCurrent = false;
    }

    if (currentParCluster_Id) {
        if (parametrization) {
            breadcrumbLi += getBreadcrumb($.grep(parametrization.listaParCluster, function (item) {
                return item.Id == currentParCluster_Id;
            })[0].Name, 'validaRota(listarParCluster,0)', isCurrent);
        } else {
            breadcrumbLi += getBreadcrumb($.grep(listaParCluster, function (item) {
                return item.Id == currentParCluster_Id;
            })[0].Name, 'validaRota(listarParCluster,0)', isCurrent);
        }

        isCurrent = false;
    }

    if (currentParFrequency_Id) {
        breadcrumbLi = getBreadcrumb($.grep(parametrization.listaParFrequency, function (item) {
            return item.Id == currentParFrequency_Id;
        })[0].Name, 'validaRota(listarParDepartment,0)', isCurrent) + breadcrumbLi;

        isCurrent = false;
    }

    if (currentParCargo_Id) {
        breadcrumbLi = getBreadcrumb($.grep(parametrization.listaParCargo, function (item) {
            return item.Id == currentParCargo_Id;
        })[0].Name, function () { }, isCurrent) + breadcrumbLi;

        isCurrent = false;
    }

    //Aqui vou ter que pegar uma lista de Departamentos e fazer um foreach 
    if (currentParDepartment_Id) {

        var deparment = "";
        isCurrent = false;

        currentsParDepartments_Ids.forEach(function (department_Id, index) {

            if (!currentParCargo_Id && (index + 1) == currentsParDepartments_Ids.length) {
                isCurrent = true;
            }

            if (department_Id) {

                deparment += getBreadcrumb($.grep(parametrization.listaParDepartment, function (item) {
                    return item.Id == department_Id;
                })[0].Name, 'validaRota(listarParDepartment,' + department_Id + ')', isCurrent);
            }

        });

        breadcrumbLi = deparment + breadcrumbLi;
        isCurrent = false;
    }

    breadcrumb += breadcrumbLi + '</ol>';

    $('.panel-heading').append(breadcrumb);

}

function getBreadcrumb(text, link, isCurrent) {

    if (isCurrent) {
        isCurrent = false;

        return '<li class="active">' + text + '</a></li>';
    } else
        return '<li><a onclick="' + link + '">' + text + '</a></li>';

}

function validaParqualification(level1Id, level2Id, level3Id) {

    var listaParLevel3ValueFiltrada = [];
    var listaParQualificationxParLevel3Value = [];

    parametrization.listaParLevel3Value.forEach(function (o, i) {
        if (o.ParLevel1_Id == level1Id && o.ParLevel2_Id == level2Id && o.ParLevel3_Id == level3Id)
            listaParLevel3ValueFiltrada.push(o);
    });

    if (listaParLevel3ValueFiltrada.length > 0) {
        
        listaParQualificationxParLevel3Value = 
        $.grep(parametrization.listaPargroupQualificationXParLevel3Value, function (element, index) {
            if (element.ParLevel3Value_Id == listaParLevel3ValueFiltrada[0].Id)
                return element;
        });
    }

    return listaParQualificationxParLevel3Value;
}

 function orderBySampleEvaluation(){
	 
	coletasAgrupadas = coletasAgrupadas.sort(function(a, b) {
        var x = a['Sample']; var y = b['Sample'];
        return ((x < y) ? -1 : ((x > y) ? 1 : 0));
    });
	
	coletasAgrupadas = coletasAgrupadas.sort(function(a, b) {
        var x = a['Evaluation']; var y = b['Evaluation'];
        return ((x < y) ? -1 : ((x > y) ? 1 : 0));
    });
 }