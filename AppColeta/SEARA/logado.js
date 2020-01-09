function openLogado() {
    openMenu();
    AtualizarVariaveisDeColetas();
    AtualizarVariaveisDeAcoesCorretivas();

    setInterval(function () {
        pingLogado(urlPreffix, online, offline)
    }, 20000);

    appIsOnline = true;
    AtualizaConstantemente();
    onlineOffline = '<div class="btn btn-success btn-lg pull-right">ONLINE</div>';
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

        $('div[data-falta-sincronizar]').text('(' + globalColetasRealizadas.length + ') Coletas não sincronizadas');
        $('[data-online-offline]').html(onlineOffline);

        if (globalColetasRealizadas.length > 0)
            $('div[data-falta-sincronizar]').addClass("btn-warning");
        else
            $('div[data-falta-sincronizar]').removeClass("btn-warning");

        //Ação Corretivas
        $('div[data-falta-sincronizar-ca]').text('(' + globalAcoesCorretivasRealizadas.length + ') Ações corretivas não sincronizadas');

        if (globalAcoesCorretivasRealizadas.length > 0)
            $('div[data-falta-sincronizar-ca]').addClass("btn-warning");
        else
            $('div[data-falta-sincronizar-ca]').removeClass("btn-warning");

    }, 300);
}