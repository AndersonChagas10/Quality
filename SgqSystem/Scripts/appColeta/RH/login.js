var token = function () {
    return {
        "token": currentLogin.Name + "|" + currentLogin.Password
    };
};

function getImages() {

    pingLogado(urlPreffix, function () {

        $.ajax({
            data: JSON.stringify({}),
            type: 'POST',
            url: urlPreffix + '/api/AppParams/GetImages',
            contentType: "application/json",
            success: function (data) {

                _writeFile("images.txt", JSON.stringify(data), function () {
                    getLogo(setLogin);
                });

            },
            timeout: 600000,
            error: function () {
                getLogo(setLogin);
            }
        });

    }, getLogo(setLogin))

}

function getLogo(callback) {

    _readFile('images.txt', function (data) {

        if (data) {

            data = JSON.parse(data)

            if (data.logo)

                callback(data.logo);

            else {
                callback();
            }
        }
        else {

            callback();
        }

    });
}

function openLogin() {

    getImages();

}

function setLogin(logo) {

    cleanGlobalVarLogin();

    var systemLogo = "";

    if (logo)
        systemLogo = 'background-image: url(' + logo + ')';

    var html = '';

    html = '<div id="" class="login" name="" style="">                                                                                                              ' +
        '<div id="" class="head" name="" style="' + systemLogo + '"></div>                                                                                                            ' +
        '    <form id="" class="form-signin" name="" style="">                                                                                                      ' +
        '        <h2 id="" class="" name="" style="">Entrar</h2>                                                                                                                                     ' +
        '        <label for="inputUserName" class="sr-only" style="">Usuário</label>                                                                                ' +
        '        <input type="text" id="inputUserName" class=" form-control" placeholder="Usuário" required="" >                                                    ' +
        '        <label for="inputPassword" class="sr-only" style="">Senha</label>                                                                                  ' +
        '        <input type="password" id="inputPassword" class=" form-control" placeholder="Senha" required="" >                                                  ' +
        '        <button type="submit" id="btnLogin" class="btn-lg btn-primary btn-block marginTop10 btn"                                                           ' +
        '        data-loading-text="<i class=\'fa fa-spinner fa-spin\'></i> <span class=\'wMessage\' style=\'font-size:14px;\'>Validando...</span>"                       ' +
        'data-initial-text="Entrar" style="" >Entrar</button>                                                                                                       ' +
        '        <div id="messageError" class="alert alert-danger hide" name="" style="" role="alert"><span id="" class="icon-remove-sign" name="" style="">        ' +
        '</span><strong>Erro! </strong><span id="mensagemErro" class="" name="" style=""></span></div>                                                              ' +
        '        <div id="" class="divLoadFiles" name="" style=""><span id="" class="messageLoading" name="" style=""></span></div>                                 ' +
        '        <div id="messageAlert" class="alert alert-info hide" name="" style="" role="alert">                                                                ' +
        '<span id="mensagemAlerta" class="icon-info-sign" name="" style=""></span></div>                                                                            ' +
        '        <div id="messageSuccess" class="alert alert-success hide" name="" style="" role="alert">                                                           ' +
        '<span id="mensagemSucesso" class="icon-ok-circle" name="" style=""></span></div>                                                                           ' +
        '    </form>                                                                                                                                                ' +
        '    <div id="" class="" name="" style="max-width:320px; margin: 0 auto; padding-right:15px; padding-left:15px">                                            ' +
        '        <button type="submit" id="btnChangeHost" class="btn-lg btn-default btn-sm btn-block btn" style="">Atualizar o APP</button>                         ' +
        '    </div>                                                                                                                                                 ' +
        '    <div id="" class="foot" name="" style="text-align:center">                                                                                             ' +
        '        <br>                                                                                                                                               ' +
        '        <br>                                                                                                                                               ' +
        '        <br><span id="local" class="hide" name="" style="" empresa="jbs" local="brasil"></span>                                                            ' +
        '<span id="versionLogin" class="" name="" style="">Versão<span id="" class="number" name="" style=""> 2.0.48 Android</span></span>                          ' +
        '    <span id="ambienteLogin" class="" name="" style=""><span id="" class="base" name="" style=""> JBS </span></span>                                       ' +
        '    </div>                                                                                                                                                 ' +
        '</div>';

    $('div#app').html(html);
}

$('body').off('click', '#btnLogin').on('click', '#btnLogin', function (event) {

    event.preventDefault();

    var btnLogin = this;

    $(this).html($(this).attr('data-loading-text'));

    if (currentLogin.Id > 0) {
        if ($('#inputUserName').val() == currentLogin.Name
            && AES.Encrypt($('#inputPassword').val()) == currentLogin.Password) {
            globalLoginOnline = false;
            //currentParFrequency_Id = parametrization.currentParFrequency_Id;
            loginSuccess(currentLogin);
            return;
        }
    }

    $.ajax({
        data: {
            app: true,
            Name: $('#inputUserName').val(),
            Password: AES.Encrypt($('#inputPassword').val()),
        },
        url: urlPreffix + '/api/User/AuthenticationLogin',
        type: 'POST',
        headers: token(),
        success: function (data) {

            //se for usuários diferentes, zera a parametrização
            if (currentLogin.Id != data.Retorno.Id) {

                parametrization = null;
                currentPlanejamento = [];

                _writeFile("appParametrization.txt", '', function () {
                });

                _writeFile("planejamento.txt", '', function () {
                });
            }

            _writeFile("login.txt", JSON.stringify(data.Retorno), function () {
                globalLoginOnline = true;
                loginSuccess(data.Retorno);
            });
        },
        timeout: 600000,
        error: function () {
            $(btnLogin).html($(btnLogin).attr('data-initial-text'));
        }
    });
});

function loginSuccess(data) {
    currentParCompany_Id = data.ParCompany_Id;
    currentLogin = data;
    openLogado();
}

function cleanGlobalVarLogin() {

    currentParFrequency_Id = null;
    currentParCompany_Id = null;
    currentParDepartment_Id = null;
    currentParCargo_Id = null;

}

function logout() {
    if (globalColetasRealizadas.length > 0) {

        setTimeout(function(){
            var titulo = "Há coletas que não foram sincronizadas.";
            var mensagem = "Não é possivel sair até que todas as coletas tenham sido sincronizadas.<br/>";
            mensagem += "[Não] Fecha a mensagem [Sim] Força sincronização das coletas (esteja online).";

            openMessageConfirm(titulo, mensagem, sincronizarColeta, closeMensagemImediatamente, "orange", "white");
        },500);
        return false;
    }

    //_writeFile("login.txt", '', function () {
    openLogin();
    // });
}

$(window).on('beforeunload', function () {
    _writeFile("login.txt", '', function () { });
});