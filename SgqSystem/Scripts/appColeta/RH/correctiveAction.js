function atualizaAcoesCorretivasAposSincronizacao(data) {

	for (var i = 0; i < data.length; i++) {
		for (var j = 0; j < globalAcoesCorretivasRealizadas.length; j++) {
			if (data[i].CollectionLevel2.Evaluation == globalAcoesCorretivasRealizadas[j].CollectionLevel2.Evaluation
				&& data[i].CollectionLevel2.Sample == globalAcoesCorretivasRealizadas[j].CollectionLevel2.Sample
				&& data[i].CollectionLevel2.ParDepartment_Id == globalAcoesCorretivasRealizadas[j].CollectionLevel2.ParDepartment_Id
				&& data[i].CollectionLevel2.ParCargo_Id == globalAcoesCorretivasRealizadas[j].CollectionLevel2.ParCargo_Id
				&& data[i].CollectionLevel2.ParLevel1_Id == globalAcoesCorretivasRealizadas[j].CollectionLevel2.ParLevel1_Id
				&& data[i].CollectionLevel2.ParLevel2_Id == globalAcoesCorretivasRealizadas[j].CollectionLevel2.ParLevel2_Id
				&& data[i].CollectionLevel2.ParLevel3_Id == globalAcoesCorretivasRealizadas[j].CollectionLevel2.ParLevel3_Id
				&& data[i].CollectionLevel2.CollectionDate.substr(0, 10) == globalAcoesCorretivasRealizadas[j].CollectionLevel2.CollectionDate.substr(0, 10)
			) {
				globalAcoesCorretivasRealizadas.splice(j, 1);
				break;
			}
		}
    }
    
	AtualizarArquivoDeAcoesCorretivas();
}

var enviarAcaoCorretivaEmExecucao = false;

function enviarAcaoCorretiva() {

	if (enviarAcaoCorretivaEmExecucao == false && globalAcoesCorretivasRealizadas.length > 0) {

        enviarAcaoCorretivaEmExecucao = true;
        
        pingLogado(urlPreffix, function () {

				$.ajax({

					data: JSON.stringify(retornaProximasAcoesCorretivasParaSincronizar()),
					url: urlPreffix + '/api/CorrectiveAction/SetCorrectiveAction',
					type: 'POST',
					contentType: "application/json",
					success: function (data) {

						enviarAcaoCorretivaEmExecucao = false;
						atualizaAcoesCorretivasAposSincronizacao(data);
                        //enviarAcaoCorretiva();
                        enviarColeta();
					},
					timeout: 600000,
					error: function () {
						enviarAcaoCorretivaEmExecucao = false;
					}
				});
			},
			function () {
				console.log('desconectado');
				enviarAcaoCorretivaEmExecucao = false;
			});
	}
}

function AtualizarArquivoDeAcoesCorretivas() {

	_writeFile("globalAcoesCorretivasRealizadas.txt", JSON.stringify(globalAcoesCorretivasRealizadas), function () {});

}

function AtualizarVariaveisDeAcoesCorretivas() {

	_readFile("globalAcoesCorretivasRealizadas.txt", function (content) {
		if (typeof (content) == 'undefined')
            content = '[]';
            
			globalAcoesCorretivasRealizadas = JSON.parse(content);
	});

}

function retornaProximasAcoesCorretivasParaSincronizar() {

	var acoesCorretivas = [];
	var anterior = null;
	var count = 10;

	for (var i = 0; i < globalAcoesCorretivasRealizadas.length; i++) {

		if (anterior != null) {
			var objAtual = Object.assign({}, globalAcoesCorretivasRealizadas[i]);
		}

		if (acoesCorretivas.length == 0 || !objIsEquals(anterior, objAtual)) {

			if (acoesCorretivas.length > count) {
				return;
			}
		}

		anterior = Object.assign({}, globalAcoesCorretivasRealizadas[i]);
		acoesCorretivas.push(Object.assign({}, anterior));
	}

	return acoesCorretivas;
}