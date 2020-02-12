var plataforma = device.platform; //platform Android
var versao = "";
var versionNumber = "";

function getVersionAPI() {
    $.ajax({
        type: 'GET'
        , url: urlPreffix + '/Config/GetAppVersionIsUpdated?versionNumber=' + versionNumber
        , contentType: 'application/json; charset=utf-8'
        , dataType: 'json'
        , async: false //blocks window close
        , success: function (data, status) {
            if (data.updated != true) {
                OpenAppMustBeUpdated(data);
            } else {
                versionNumber = data.versionNumber;
                saveVersionNumber();
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        }
    });
}

function OpenAppMustBeUpdated(data) {
    setTimeout(function () {
        navigator.notification.alert('Nova atualização disponivel. A aplicação será atualizada!',
            ReloadAfterUpdatVersion,
            'Atualização',
            'OK');
    }, 500);
    versionNumber = data.versionNumber;
}

function ReloadAfterUpdatVersion() {
    saveVersionNumber();
    Reload();
}

function getVersionNumber() {
    _readFile("version.txt", function (r) {
        if (!!r) {
            versionNumber = r;
        }
        versao = versionNumber + " " + plataforma;
        getVersionAPI();
    });
}

function saveVersionNumber() {
    _writeFile("version.txt", versionNumber);
    versao = versionNumber + " " + plataforma;
}