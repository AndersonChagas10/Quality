function openLogado() {
    openParFrequency();
    //openParDepartment();
    AtualizarVariaveisDeColetas();

    setInterval(function () {
        pingLogado(urlPreffix, online, offline)
    }, 20000);

    AtualizaConstantemente();
    onlineOffline = '<div class="btn btn-success btn-lg pull-right">ONLINE</div>';
    appIsOnline = true;
}

var onlineOffline = "";
function online() {
    enviarColeta();
    appIsOnline = true;
    onlineOffline = '<div class="btn btn-success btn-lg pull-right">ONLINE</div>';
}

function offline() {
    console.log('desconectado')
    onlineOffline = '<div class="btn btn-danger btn-lg pull-right">OFFLINE</div>';
    appIsOnline = false;
}

function AtualizaConstantemente() {
    setInterval(function () {
        $('div[data-falta-sincronizar]').text('(' + globalColetasRealizadas.length + ') Não sincronizadas');
        $('[data-online-offline]').html(onlineOffline);

        if (globalColetasRealizadas.length > 0)
            $('div[data-falta-sincronizar]').addClass("btn-warning");
        else
            $('div[data-falta-sincronizar]').removeClass("btn-warning");

    }, 300);
}