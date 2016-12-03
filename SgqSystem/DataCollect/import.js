//importar todos os arquivos para dentro da pasta que tem os arquivos temporarios
//os arquivos precisam ser carregados na sequinte ordem
//Methods
//File
//SyncData

//lista de possíveis servidores
var urlServidor = "http://192.168.25.200/SgqMaster";
var urlServidorUsa = "http://10.190.2.34/QualityAssurance"
var urlServidorExternal = "http://grt.brz-s.com:8081/SgqMaster"

//url padrão
var urlPreffix = urlServidor;

//método que faz o download do script methods.js
function setMethodsJS() {
    console.log('Start downloading methods.js');
    window.requestFileSystem(LocalFileSystem.PERSISTENT, 0, function (fs) {
        fs.root.getFile("Methods.js", { create: true, exclusive: false }, function (fileEntry) {
            $.post(urlPreffix + '/File/Download', { fileName: 'Methods.js' }, function (r) {
                fileEntry.createWriter(function (fileWriter) {
                    //gravação no arquivo
                    fileWriter.write(r);
                    console.log('Finish downloading methods.js');
                    //método que faz o download do script file.js
                    setFileJS();
                });
            });
        }, onErrorCreateFile);
    }, onErrorLoadFs);
}

//método que faz o download do script syncdata.js
function setSyncDataJS() {
    console.log('Start downloading syncdata.js');
    window.requestFileSystem(LocalFileSystem.PERSISTENT, 0, function (fs) {
        fs.root.getFile("SyncData.js", { create: true, exclusive: false }, function (fileEntry) {
            $.post(urlPreffix + '/File/Download', { fileName: 'SyncData.js' }, function (r) {
                fileEntry.createWriter(function (fileWriter) {
                    //gravação no arquivo
                    fileWriter.write(r);
                    console.log('Finish downloading syncdata.js');
                    //método que faz a leitura do script methods.js
                    getMethodsJS();
                });
            });
        }, onErrorCreateFile);
    }, onErrorLoadFs);
}

//método que faz o download do script file.js
function setFileJS() {
    console.log('Start downloading file.js');
    window.requestFileSystem(LocalFileSystem.PERSISTENT, 0, function (fs) {
        fs.root.getFile("File.js", { create: true, exclusive: false }, function (fileEntry) {
            $.post(urlPreffix + '/File/Download', { fileName: 'File.js' }, function (r) {
                fileEntry.createWriter(function (fileWriter) {
                    //gravação no arquivo
                    fileWriter.write(r);
                    console.log('Finish downloading file.js');
                    //método que faz o download do script syncdata.js
                    setSyncDataJS();
                });
            });
        }, onErrorCreateFile);
    }, onErrorLoadFs);
}

//método que faz a leitura do script file.js
function getFileJS() {
    console.log('Start setting file.js');
    window.requestFileSystem(LocalFileSystem.PERSISTENT, 0, function (fs) {
        fs.root.getFile("File.js", { create: true, exclusive: false }, function (fileEntry) {
            fileEntry.file(function (file) {
                var reader = new FileReader();
                reader.onloadend = function () {
                    if (this.result) {
                        //execução do script
                        jQuery.globalEval(this.result);
                        //método que faz a leitura do script syncdata.js
                        getSyncDataJS();
                        console.log('Finish setting file.js');
                    }
                };
                reader.readAsText(file);
            }, onErrorReadFile);
        }, onErrorCreateFile);
    });
}

//método que faz a leitura do script methods.js
function getMethodsJS() {
    console.log('Start setting methods.js');
    window.requestFileSystem(LocalFileSystem.PERSISTENT, 0, function (fs) {
        fs.root.getFile("Methods.js", { create: true, exclusive: false }, function (fileEntry) {
            fileEntry.file(function (file) {
                var reader = new FileReader();
                reader.onloadend = function () {
                    if (this.result) {
                        //execução do script
                        jQuery.globalEval(this.result);
                        //método que faz a leitura do script file.js
                        getFileJS();
                        console.log('Finish setting methods.js');
                    } else {
                        setMethodsJS();
                    }
                };
                reader.readAsText(file);
            }, onErrorReadFile);
        }, onErrorCreateFile);
    });
}

//método que faz a leitura do script syncdata.js
function getSyncDataJS() {
    console.log('Start setting syncdata.js');
    window.requestFileSystem(LocalFileSystem.PERSISTENT, 0, function (fs) {
        fs.root.getFile("SyncData.js", { create: true, exclusive: false }, function (fileEntry) {
            fileEntry.file(function (file) {
                var reader = new FileReader();
                reader.onloadend = function () {
                    if (this.result) {
                        //execução do script
                        jQuery.globalEval(this.result);
                        console.log('Finish setting syncdata.js');
                        closeConfFiles();
                    }
                };
                reader.readAsText(file);
            }, onErrorReadFile);
        }, onErrorCreateFile);
    });
}

document.addEventListener("deviceready", onDeviceReady, false);

function onDeviceReady() {
    openConfFiles();
    getMethodsJS();
}

function onErrorLoadFs(e) {
    //openMessageModal( "FileSystem Error: "+e);
    //console.log(e);
}

function onErrorCreateFile(e) {
    //openMessageModal( "FileSystem Error: "+e);
    //console.log(e);
}

function onErrorReadFile(e) {
    //openMessageModal( "FileSystem Error: "+e);
    //console.log(e);
}

function openConfFiles() {

    var mensagem = $('.message, .overlay');

    //mensagem.children('.head').html('Updating the aplication...');
    mensagem.children('.body').html('Updating the aplication...');
    //$('#btnMessageOk').removeClass('hide');

    mensagem.fadeIn("fast");
}

function closeConfFiles() {
    var mensagem = $('.message, .overlay');
    mensagem.fadeOut("fast");
}
