function openLogado() {
    openParFrequency();
    //openParDepartment();
    AtualizarVariaveisDeColetas();

    setInterval(function () {
        pingLogado(urlPreffix, enviarColeta, function () { console.log('desconectado') })
    }, 20000);

    AtualizaConstantemente();
}

function AtualizaConstantemente(){
    setInterval(function () {
        $('div[data-falta-sincronizar]').text('(' + globalColetasRealizadas.length + ') N�o sincronizadas');
    }, 1000);
}