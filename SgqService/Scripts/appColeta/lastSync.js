function lastSyncParamCheck(result) {

    if (result == "" || result == undefined) {
        Geral.exibirMensagemErro(getResource("you_need_to_be_connected"));
        $('#btnLoginOnline').button('reset');
        $('#btnLoginOffline').button('reset');
        resetarDateLastSyncParam();
    }
    else {
        //checagem de turnos que podem passar para outro dia mas a coleta continua no dia anterior
        var lastParamSync = $(result);

        _readFile('users.txt', function (r) {
            userlogado = $(r).find('.user[userlogin=' + $('#inputUserName').val() + '][userpass="' + AES.Encrypt($('#inputPassword').val()) + '"]:first');

            if (dateFormat() != lastParamSync.attr('date') || lastParamSync.attr('unitid') != (userlogado ? userlogado.attr('unidadeid') : null)) {
                Geral.exibirMensagemErro(getResource("online_to_login"));
                $('#btnLoginOnline').button('reset');
                $('#btnLoginOffline').button('reset');
                resetarDateLastSyncParam();
            }
            else 
                _readFile("apppage.txt", getAPPLevelsOffLine);
        });


    }
}

function setLastSync() {
    window.requestFileSystem(LocalFileSystem.PERSISTENT, 0, function (fs) {
        fs.root.getFile("sync.txt", { create: true, exclusive: false }, function (fileEntry) {
            var div = '<div class="LastSync" date="' + dateFormat() + '" datetime="' + dateTimeFormat() + '" userid="' + $('.App').attr('userid') + '" unitid="' + $('#selectUnit :selected').val() + '"></div>';
            writeFile(fileEntry, new Blob([div], { type: 'text/plain' }));
        }, onErrorCreateFile);
    }, onErrorLoadFs);
}

function clearLastSync() {
    window.requestFileSystem(LocalFileSystem.PERSISTENT, 0, function (fs) {
        fs.root.getFile("sync.txt", { create: false }, function (fileEntry) {
            fileEntry.remove(function () {
                console.log('File removed.');
            });
            //var div = "123";
            //writeFile(fileEntry, new Blob([div], { type: 'text/plain' }));
        }, onErrorCreateFile);
    }, onErrorLoadFs);
}

//pega a ultima data de sincronismo e decide qual callback executar
function getLastSync(user, callback) {
    window.requestFileSystem(LocalFileSystem.PERSISTENT, 0, function (fs) {
        fs.root.getFile("sync.txt", { create: false, exclusive: false }, function (fileEntry) {
            fileEntry.file(function (file) {
                var reader = new FileReader();
                reader.onloadend = function () {
                    var lastSync = $(this.result);

                    //var str = window.location.href;
                    //var nInicial = str.indexOf("?");
                    //var res = str.substr(nInicial, 50);
                    //var nFinal = res.indexOf("=");

                    //var param = res.substr(1, nFinal - 1);

                    //res = res.substr(nFinal + 1, 50);

                    //if (!isNaN(parseFloat(res)) && isFinite(res) && param == 'ParCompany_Id') {
                    //    //alert(res);
                    //} else {
                    //    res = 0;
                    //}

                    var res = $('.App').attr('unidadeid');

                    if ($(user).attr('unidadeid') == res) {
                        if (lastSync.length) {

                            if (dateFormat() > lastSync.attr('date') && (connectionServer == true) || lastSync.attr('unitid') != $('#selectUnit :selected').val()) {
                                Geral.exibirMensagemErro(getResource("first_synchronization_of_the_day"));
                                $('#btnLoginOffline').button('reset');
                                $('#btnLoginOnline').button('reset');
                            } else {
                                userAPPDataInsert(user);
                                callback(user);
                            }
                        }
                        else {
                            userAPPDataInsert(user);
                            callback(user);
                        }
                    } else {
                        Geral.exibirMensagemErro(getResource("user_doesnt_belong_unit"));
                        if ($('#messageError:visible').length > 0)
                            Geral.exibirMensagemErro(getResource("user_doesnt_belong_unit"));
                        $('#btnLoginOnline').button('reset');
                        $('#btnLoginOffline').button('reset');
                    }
                };
                reader.readAsText(file);
            }, onErrorReadFile);
        }, onErrorCreateFile);
    }, onErrorLoadFs);
}

//arquivo de erro para leitura
function onErrorLoadFs(e) {
}

//cria o arquivo de sincronizacao se nao existir
function onErrorCreateSyncFile(callback, user) {
    window.requestFileSystem(LocalFileSystem.PERSISTENT, 0, function (fs) {
        fs.root.getFile("sync.txt", { create: true, exclusive: false }, function (fileEntry) {
            callback(user);
        }, onErrorCreateFile);
    }, onErrorLoadFs);
}

function gravarLastSyncParam() {
    if (userlogado) {
        var div = '<div class="LastSync" date="' + dateFormat() + '" datetime="' + dateTimeFormat() + '" userid="' + userlogado.attr('userid') + '" unitid="' + userlogado.attr('unidadeid') + '"></div>';
        _writeFile("lastsync.txt", div, gravaBancoDadosOffLine_Concluido);  
    }
}

function gravarLastSyncParam() {
    if (userlogado) {
        var div = '<div class="LastSync" date="' + dateFormat() + '" datetime="' + dateTimeFormat() + '" userid="' + userlogado.attr('userid') + '" unitid="' + userlogado.attr('unidadeid') + '"></div>';
        _writeFile("lastsync.txt", div, gravaBancoDadosOffLine_Concluido);  
    }
}