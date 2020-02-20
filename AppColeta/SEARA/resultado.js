var coletasAgrupadas = [];

function getResultEvaluationSample(parLevel1_Id, parLevel2_Id) {

	var obj = {
		Evaluation: 1,
		Sample: 1
	};

	$(coletasAgrupadas).each(function (i, o) {
		if (o.ParLevel1_Id == parLevel1_Id && o.ParLevel2_Id == parLevel2_Id) {
			obj = o;
		}
	});

	return obj;

}

function retornaProximasColetasParaSincronizar() {

	var coletas = [];
	var anterior = null;
	var count = 10;

	for (var i = 0; i < globalColetasRealizadas.length; i++) {

		if (anterior != null) {

			var objAtual = $.extend({}, getCollectionLevel2Obj(globalColetasRealizadas[i]));

			if (coletas.length == 0 || !objIsEquals(getCollectionLevel2Obj(anterior), objAtual)) {

				if (coletas.length > count) {
					return coletas;
				}
			}
		}

		anterior = $.extend({}, globalColetasRealizadas[i]);
		coletas.push(anterior);
	}

	return coletas;
}

function objIsEquals(objAnterior, objAtual) {

	return JSON.stringify(objAnterior) === JSON.stringify(objAtual);

}

function getCollectionLevel2Obj(obj) {
	return {
		ParLevel1_Id: obj.ParLevel1_Id,
		ParLevel2_Id: obj.ParLevel2_Id,
		ParCompany_Id: obj.ParCompany_Id,
		ParCluster_Id: obj.ParCluster_Id,
		ParCargo_Id: obj.ParCargo_Id,
		ParDepartment_Id: obj.ParDepartment_Id,
		Shift: obj.Shift,
		Sample: obj.Sample,
		Evaluation: obj.Evaluation,
	}
}

function atualizaColetasAposSincronizacao(data) {
	for (var i = 0; i < data.length; i++) {
		for (var j = 0; j < globalColetasRealizadas.length; j++) {
			if (data[i].Evaluation == globalColetasRealizadas[j].Evaluation
				&& data[i].Sample == globalColetasRealizadas[j].Sample
				&& data[i].ParDepartment_Id == globalColetasRealizadas[j].ParDepartment_Id
				&& data[i].ParCargo_Id == globalColetasRealizadas[j].ParCargo_Id
				&& data[i].ParLevel1_Id == globalColetasRealizadas[j].ParLevel1_Id
				&& data[i].ParLevel2_Id == globalColetasRealizadas[j].ParLevel2_Id
				&& data[i].ParLevel3_Id == globalColetasRealizadas[j].ParLevel3_Id
				&& data[i].CollectionDate.substr(0, 10) == globalColetasRealizadas[j].CollectionDate.substr(0, 10)
			) {
				globalColetasRealizadas.splice(j, 1);
				break;
			}
		}
	}
	AtualizarArquivoDeColetas();
}


var enviarColetaEmExecucao = false;
function enviarColeta() {

	if (enviarColetaEmExecucao == false && globalColetasRealizadas.length > 0) {
		enviarColetaEmExecucao = true;
		pingLogado(urlPreffix,
			function () {
				$.ajax({
					data: JSON.stringify(retornaProximasColetasParaSincronizar()),
					url: urlPreffix + '/api/AppColeta/SetCollect123',
					type: 'POST',
					contentType: "application/json",
					success: function (data) {
						enviarColetaEmExecucao = false;
						atualizaColetasAposSincronizacao(data);
						enviarColeta();
					},
					timeout: 600000,
					error: function () {
						enviarColetaEmExecucao = false;
					}
				});
			},
			function () {
				console.log('desconectado');
				enviarColetaEmExecucao = false;
			});

	} else {

		enviarAcaoCorretiva();

	}
}

function AtualizarArquivoDeColetas() {
	_writeFile("globalColetasRealizadas.txt", JSON.stringify(globalColetasRealizadas), function () {
	});

	_writeFile("coletasAgrupadas.txt", JSON.stringify(coletasAgrupadas), function () {
	});
}

function AtualizarVariaveisDeColetas() {

	_readFile("globalColetasRealizadas.txt", function (content) {
		if (typeof (content) == 'undefined' || content == "")
			content = '[]';
		globalColetasRealizadas = JSON.parse(content);
	});

	_readFile("coletasAgrupadas.txt", function (content) {
		if (typeof (content) == 'undefined' || content == "")
			content = '[]';
		coletasAgrupadas = JSON.parse(content);
	});

}
