var userlogado;
//fazer login online caso necessario
function onlineLogin(username, password) {
    Geral.esconderMensagem();
    $.ajax({
        data: {
            app: true,
            Name: $('#inputUserName').val(),
            Password: AES.Encrypt($('#inputPassword').val()),
        },
        url: urlPreffix + '/api/User/AuthenticationLogin',
        type: 'POST',
        success: function (data) {
            if (data.MensagemExcecao) {
                Geral.exibirMensagemErro(getResource('invalid_user_or_password'));
                $('#btnLoginOnline').button('reset');
                $('#btnLoginOffline').button('reset');
            } else {

                //Verificar se já sincronizou no dia atual
                var IsSyncToday = false;

                //current shift
                _shift = $('#shift :selected').val();

                _readFile('dateLastSync.txt', function (json) {

                    if (json == "") {

                        onLineLogin(data);

                    } else {

                        var obj = null;

                        try {
                            obj = JSON.parse(json);
                        } catch (e) { }

                        if (obj && obj.Data)
                            IsSyncToday = (
                                (obj.Data == new Date().toLocaleDateString()) &&
                                obj.Shift == parseInt($(shift).val()) &&
                                obj.Unit == data.Retorno.ParCompany_Id);

                        if (IsSyncToday) {
                            offLineLogin();

                        } else {
                            onLineLogin(data);
                        }
                    }
                });

            }
        },
        timeout: 600000,
        error: function () {
            offLineLogin();
            // Geral.exibirMensagemErro(getResource("login_error_try_again"));
            // $('#btnLoginOnline').button('reset');
            // $('#btnLoginOffline').button('reset');
        }
    });
}

function onLineLogin(data) {
    //verifica se tem dado não sinc
    if ($('.level02Result[sync=false]:last').length > 0) {
        //verifica se o usuário da coleta anterior é da mesma unidade que o atual
        if ($('.level02Result[sync=false]:last').attr('unidadeid') == data.Retorno.ParCompany_Id) {
            users = JSON.stringify(data.Retorno);
            gravarUsuariosRetornoOffLine();
            getInfoUserById(data.Retorno.Id);
        } else
            verificaConexaoUnidadeAnterior(data.Retorno.ParCompany_Id, data.Retorno.Id);
    } else {
        users = JSON.stringify(data.Retorno);
        gravarUsuariosRetornoOffLine();
        getInfoUserById(data.Retorno.Id);
    }
}

//recuperar o usuário pelo id
function getInfoUserById(Id) {
    Geral.esconderMensagem();
    $.ajax({
        data: {
            Id: Id,
        },
        url: urlPreffix + '/Services/SyncServices.asmx/UserSGQById',
        type: 'POST',
        success: function (data) {
            var user = $($(data).text());
            userlogado = user;
            if (!user.hasClass('user')) {
                Geral.exibirMensagemErro($(data).text());
            }
            else {
                wMessage($('#btnLoginOnline'), getResource('loading_indicators'));
                setTimeout(function (e) {
                    getAPPLevels1OnLine();
                }, 500)
            }
        },
        timeout: 600000,
        error: function () {
            Geral.exibirMensagemErro(getResource("user_information_error_try_again"));
        }
    });
}

function getCompanyUsers(ParCompany_Id) {
    var request = $.ajax({
        data: {
            "ParCompany_Id": ParCompany_Id
        },
        url: urlPreffix + '/Services/SyncServices.asmx/getCompanyUsers',
        type: 'POST',
        success: function (data) {

            $('Users').empty();

            var users = $(data).text();
            appendDevice(users, $('.Users'));

            wMessage($('#btnLoginOnline'), getResource('verifying_keys'));
            wMessage($('#btnLoginOffline'), getResource('verifying_keys'));

            setTimeout(function (e) {
                contagem = 0;
                //getCollectionKeys(ParCompany_Id);
                gravaBancoDadosOffLine();
            }, 500);
        },
        timeout: 600000,
        error: function () {
            request.abort();
            contagem++;
            if (contagem > 2) {
                contagem = 0;
                Geral.exibirMensagemErro(getResource("user_load_error_try_again"));
                return false;
            }
            wMessage($('#btnLoginOnline'), getResource("trying_again"));
            wMessage($('#btnLoginOffline'), getResource("trying_again"));
            setTimeout(function (e) {
                wMessage($('#btnLoginOnline'), getResource('verifying_users'));
                wMessage($('#btnLoginOffline'), getResource('verifying_users'));
                setTimeout(function (e) {
                    getCompanyUsers(ParCompany_Id);
                }, 800);
            }, 1500);
        }
    });
}

var textbtnLogin = "";
var textbtnLoginOnline = "";

function initialLogin() {
    Geral.esconderMensagem();

    clusterAtivo = '';
    clusterAtivoName = '';
    //initializeInputs();

    $('.shift').text($('#shift option:selected').text());

    level01Reset($('.level01'));
    $('.painelLevel02 select').val(0);
    $('.painelLevel02 input').val("");

    level02Reset($('.level02List .level02Group .level02'));

    //melhorar verificacao de true no inicio
    var login = $('.login');
    login.hide();

    login.addClass('hide');
    //padding no windows
    if (device.platform == "windows") { $('.App .container').css('padding-left', '20x').css('padding-right', '35px'); }

    var loginTime = new Date();
    //aqui verificar o period se for um deixa 1
    $('#selectPeriod').val("0");

    //mostrar link http se desenvolvimento
    if (urlPreffix.indexOf('QualityAssurance') < 0) { $('.urlPrefix').text(urlPreffix); }

    //aqui pode conter o sync ou depois que mostrar o level01
    level1Show(true, clusterAtivo);

    $('#btnLoginOffline').text(textbtnLoginOffline);
    $('#btnLoginOnline').text(textbtnLoginOnline);
    $('#btnLoginOffline').button('reset');
    $('#btnLoginOnline').button('reset');
    $('#period').not('disabled').removeAttr('disabled').trigger('change');

    userAPPDataInsert(userlogado);

    applyRole();

    if ($('.level1.CFF').length > 0) {
        getDefectEvaluationOnline($('.level1.CFF').attr('id'));
    }

    sendResultsTimeout();

    setupParCompanies();

    setTimeout(function (e) {
        updateReaudit(1);
        updateReaudit(2);
        updateCorrectiveAction();
        setDateLastSync();
    }, 500);

    getAPPLevelsVolume();
    getCollectionKeys($('.App').attr('unidadeid'));
    loginVerificacaoKeys();

}

//GERA OS CLIQUES PARA CRIAR VARIAS PONTEIRO PARA CONTROLE DE ALERTAS

$('.level1').on('click', function () {
    _level1 = this;
});

$('.level2').on('click', function () {
    _level2 = this;
});

$(document).on('change', '.login #shift', function (e) {
    if ($('.login #shift option:selected').val() == 0) {
        $('#inputUserName, #inputPassword').attr('disabled', 'disabled');
        $('#btnLoginOnline').attr('disabled', 'disabled');
        $('#btnLoginOffline').attr('disabled', 'disabled');
        $('.login #shift').focus();
    }
    else {
        $('#inputUserName, #inputPassword').removeAttr('disabled');
        $('#btnLoginOnline').removeAttr('disabled');
        $('#btnLoginOffline').removeAttr('disabled');
        $('#inputUserName').focus();
    }
});

/// <summary>Método que faz o Login</summary>
/// username: Nome de  usuário
/// password: Senha
/// method: call-back (função que vai ser chamada após a execução do método)
function getlogin(username, password, callback) {

    var u = username;
    var p = AES.Encrypt(password);

    var user = $('.user[userlogin="' + u.toLowerCase() + '"][userpass="' + p + '"]');

    if (user.length) {
        callback(user);
    } else {
        Geral.exibirMensagemErro(getResource("invalid_user_or_password"));
        $('#btnLoginOnline').button('reset');
        $('#btnLoginOffline').button('reset');
    }
}

function offLineLogin() {
    _readFile("lastsync.txt", lastSyncParamCheck);
}

function firstLoginWithoutFile() {
    if (connectionServer == true) {
        $('#btnSync').click();
    }
}

//cria o arquivo de database se ainda nao existir
function loginFile() {
    _readFile("database.txt", function (r) {
        if (r) {
            $('.Results').empty();
            appendDevice($(r), $('.Results'));
            //cleanResults();
        }
    });
}

function loginFileResult() {
    _readFile("databaseConsolidation.txt", function (r) {
        if (r) {
            $('.ResultsConsolidation').empty();
            appendDevice($(r), $('.ResultsConsolidation'));
            //cleanResults();
        }
    });
}

function loginPhotos() {
    _readFile("level3Photos.json", function (r) {
        if (r) {
            level3Photos = JSON.parse(r);
        }
    });
}

function usersOffLineCheck(result) {
    var users = $(result);
    $('.Users').empty();
    appendDevice($(users.html()), $('.Users'));

    if (!$('.Users .user').length) {
        Geral.exibirMensagemErro(getResource("online_to_login"));
        $('#btnLoginOnline').button('reset');
        $('#btnLoginOffline').button('reset');
    }
    else {
        userlogado = $('.Users .user[userlogin=' + $('#inputUserName').val() + '][userpass="' + AES.Encrypt($('#inputPassword').val()) + '"]');
        if (!userlogado.length) {
            Geral.exibirMensagemErro(getResource("username_or_password_are_incorrect"));
            $('#btnLoginOnline').button('reset');
            $('#btnLoginOffline').button('reset');
        }
        else {
            connectionServer = false;
            gravaBancoDadosOffLine_Concluido();
        }
    }
}

function getAPPLevelsOffLine(result) {
    var App = $(result);
    AppAppend(App);
    _readFile("users.txt", usersOffLineCheck);
}

function gravaBancoDadosOffLine() {
    _writeFile("apppage.txt", $('body').html(), gravarUsuariosOffLine);
    setTimeout(function (e) {
        initialLogin();
    }, 1000);
}
function gravarUsuariosOffLine() {
    var userHtml = "<div></div>"
    if ($('.Users').length > 0)
        userHtml = $('.Users')[0].outerHTML;

    _writeFile("users.txt", userHtml, gravarLastSyncParam);
    var url = urlPreffix + '/api/User/GetAllUserByUnit/';
    $.post(url + userlogado.attr('unidadeid'), function (r) {
        var listaUsers = JSON.stringify(r);
        _writeFile("usersUnidade.txt", listaUsers, null);
    });
}

function gravarUsuariosRetornoOffLine() {
    _writeFile("usersOffline.txt", users, gravarLastSyncParam);
}

function gravaBancoDadosOffLine_Concluido() {
    wMessage($('#btnLoginOnline'), getResource("logging_in"));
    wMessage($('#btnLoginOffline'), getResource("logging_in"));
    userAPPDataInsert(userlogado);
    if (connectionServer == false)
        setTimeout(function (e) {
            connectionServer = false;
            initialLogin();
        }, 500);
}

//metodo padrao que le o arquivo no hmtl de results
function readFile(fileEntry) {
    fileEntry.file(function (file) {
        var reader = new FileReader();
        reader.onloadend = function () {

        };
        reader.readAsText(file);
    }, onErrorReadFile);
}

function verificaConexaoUnidadeAnterior(ParCompany_Id, user_Id) {
    $.get(urlPreffix + '/api/Company/getCompany/', { id: ParCompany_Id }, function (r) {
        if (r.IPServer != null) {
            var prefixAntigo = urlPreffix;
            urlPreffix = r.IPServer + '/SgqSystem';
            ping(function (r) {
                sendResults();
                urlPreffix = prefixAntigo;
                ping(getInfoUserById(user_Id), offLineLogin);
            }, function (r) {
                openMessageModal(getResource('data_not_synced'));
                urlPreffix = prefixAntigo;
                $('#btnLoginOnline').button('reset');
                return false;
            });
        }
    });
}

function setupParCompanies() {
    _readFile("usersOffline.txt", function (r) {
        //var user  = JSON.parse(localStorage.getItem('UserLogado'));
        var user = JSON.parse(r);
        $('#selectParCompany').attr('parcompany_id', user.ParCompany_Id)

        var arraySelectCompanyUser = $.map(user.ParCompanyXUserSgq, function (option) {
            return option.ParCompany_Id.toString();
        });

        var arraySelectCompanyFull = $.map($('#selectParCompany option'), function (option) {
            return option.value;
        });

        var difference = [];

        jQuery.grep(arraySelectCompanyFull, function (el) {
            if (jQuery.inArray(el, arraySelectCompanyUser) == -1) difference.push(el);
        });

        $('#selectParCompany').val(user.ParCompany_Id).change();

        difference.forEach(function (o) {
            $('#selectParCompany option[value="' + o + '"]').hide();
        });
    });
}

function setDateLastSync() {
    if($('.App').attr('serverdate')){

        var dataServidor = convertDate($('.App').attr('serverdate'));
        const objLastSync = { Shift: parseInt($(shift).val()), Data: new Date(dataServidor).toLocaleDateString(), Unit: parseInt($('.App').attr('unidadeid')) }

        _writeFile("dateLastSync.txt", JSON.stringify(objLastSync));
    
        $('.App').attr('datelastsync', dataServidor);
    }
}

function resetarDateLastSyncParam() {
    _writeFile("dateLastSync.txt", "");  
    _writeFile("lastsync.txt", "");  
}