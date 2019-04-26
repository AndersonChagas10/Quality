var curretParCompany_Id;
var currentParFrequency_Id;
var parametrization = null;
var currentParDepartment_Id;
var currentParDepartmentParent_Id;
var currentParCargo_Id;
var globalColetasRealizadas = [];
var currentLogin = {};
var globalLoginOnline = false;
var currentCollectDate = new Date();
var appIsOnline = false;
var currentAlerts = [];
var currentAlertsAgrupados = [];

var currentTotalEvaluationValue = 0;
var currentTotalSampleValue = 0;

function onOpenAppColeta() {
    _readFile("login.txt", function (data) {
        if (typeof (data) != 'undefined' && data.length > 0)
            currentLogin = JSON.parse(data);

        _readFile("appParametrization.txt", function (param) {
            if (typeof (param) != 'undefined' && param.length > 0) {
                parametrization = JSON.parse(param);
                currentParFrequency_Id = parametrization.currentParFrequency_Id;
            }
        });
    });
}

function getAppParametrization(frequencyId) {

    if (currentParFrequency_Id > 0 && frequencyId == currentParFrequency_Id) {
        if (confirm('Deseja refazer o download da parametrização desta frequencia?'))
            currentParFrequency_Id = 0;
    }

    sincronizarResultado(frequencyId);

    if (frequencyId != currentParFrequency_Id) {
        currentParFrequency_Id = frequencyId;
        openMensagem('Por favor, aguarde até que seja feito o download do planejamento selecionado', 'blue', 'white');
        $.ajax({
            data: JSON.stringify({
                ParCompany_Id: curretParCompany_Id
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
                });
                closeMensagem();
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
            ParCompany_Id: currentLogin.ParCompanyXUserSgq[0].ParCompany.Id,
            CollectionDate: new Date(currentCollectDate).toISOString()
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
    console.log("ParCompany:" + curretParCompany_Id);
    console.log("Frequencia: " + currentParFrequency_Id);
    console.log("Departamento: " + currentParDepartment_Id);
    console.log("Cargo: " + currentParCargo_Id);
}

function openModalChangeDate() {

    var html = '<div class="form-group row">' +
        '<label for="exemplo">Data: </label>' +
        '<input id="appDate" type="date" class="form-control"/>' +
        '</br>' +
        '<button id="btnChangeDate" type="button" class="btn btn-primary" onclick="changeDate(this)">Alterar Data de coleta</button> | ' +
        '<button id="btnChangeDate" type="button" class="btn btn-primary" onclick="closeModal()">Cancelar</button>' +
        '</div>';

    openModal(html);

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

        var titulo = "Não foi possível alterar a data.";
        var mensagem = "Existem coeltas não sincronizadas. Deseja sincronizar os dados?";

        openMessageConfirm(titulo, mensagem, sincronizarColeta, function () { }, "blue", "white");

        return false;
    }

    openMensagem("Alterando data...", "blue", "White");
    _writeFile("appParametrization.txt", '', function () { });

    currentCollectDate = new Date(newDate);

    openParFrequency();
    closeModal(5000);
}