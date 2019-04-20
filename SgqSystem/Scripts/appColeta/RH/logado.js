function openLogado() {
    openParFrequency();
    //openParDepartment();
    AtualizarVariaveisDeColetas();

    setInterval(function () {
        pingLogado(urlPreffix, online, offline)
    }, 20000);

    AtualizaConstantemente();
	onlineOffline = '<div class="btn btn-success btn-lg pull-right">ONLINE</div>';
}

var onlineOffline = "";
function online(){
	enviarColeta();
	onlineOffline = '<div class="btn btn-success btn-lg pull-right">ONLINE</div>';
}

function offline(){
	console.log('desconectado')
	onlineOffline = '<div class="btn btn-danger btn-lg pull-right">OFFLINE</div>';
}

function AtualizaConstantemente(){
    setInterval(function () {
        $('div[data-falta-sincronizar]').text('(' + globalColetasRealizadas.length + ') Não sincronizadas');
		$('[data-online-offline]').html(onlineOffline);
		
		if(globalColetasRealizadas.length > 0)
			$('div[data-falta-sincronizar]').addClass("btn-warning");
		else
			$('div[data-falta-sincronizar]').removeClass("btn-warning");
		
    }, 300);
}