var coletasAgrupadas = [];
/*
{
	ParDepartment_Id,
	ParCargo_Id,
	Evaluation,
	Sample
}
*/

function getResultEvaluationSample(parDepartment_Id, parCargo_Id){
	
	var obj = {
		ParDepartment_Id : parDepartment_Id,
		ParCargo_Id : parCargo_Id,
		Evaluation : 1,
		Sample : 1
	};
	
	$(coletasAgrupadas).each(function (i, o) {
		if(o.ParDepartment_Id == parDepartment_Id && o.ParCargo_Id == parCargo_Id){
			obj = o;
		}
	});
	
	return obj;
	
}

function retornaProximasColetasParaSincronizar(){
	
	var count = 10;
	var amostras = [];
	for(var i = 0; i < globalColetasRealizadas.length;i++){
		var amostra = globalColetasRealizadas[i];
		amostras.push(amostra);
		
		if(amostras.length >= count)
			break;
	}
	return amostras;

}

function atualizaColetasAposSincronizacao(data){
	for(var i = 0; i < data.length;i++){
		for(var j = 0; j < globalColetasRealizadas.length;j++){
			if(data[i].Evaluation == globalColetasRealizadas[j].Evaluation
			&& data[i].Sample == globalColetasRealizadas[j].Sample
			&& data[i].ParDepartment_Id == globalColetasRealizadas[j].ParDepartment_Id
			&& data[i].ParCargo_Id == globalColetasRealizadas[j].ParCargo_Id
			&& data[i].ParLevel1_Id == globalColetasRealizadas[j].ParLevel1_Id
			&& data[i].ParLevel2_Id == globalColetasRealizadas[j].ParLevel2_Id
			&& data[i].ParLevel3_Id == globalColetasRealizadas[j].ParLevel3_Id
			&& data[i].CollectionDate.substr(0,10) == globalColetasRealizadas[j].CollectionDate.substr(0,10)
			){
				globalColetasRealizadas.splice(j,1);
				break;
			}
		}
	}
	AtualizarArquivoDeColetas();
}

var enviarColetaEmExecucao = false;

function enviarColeta(){

	if(enviarColetaEmExecucao == false && globalColetasRealizadas.length > 0){

		enviarColetaEmExecucao = true;

	    pingLogado(urlPreffix, function () {

                $.ajax({
                    data: JSON.stringify(retornaProximasColetasParaSincronizar()),
                    url: urlPreffix + '/api/AppColeta/SetCollect',
                    type: 'POST',
                    contentType: "application/json",
                    success: function (data) {					
						atualizaColetasAposSincronizacao(data);
						enviarColetaEmExecucao = false;
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
	}
}

function AtualizarArquivoDeColetas(){
	_writeFile("globalColetasRealizadas.txt", JSON.stringify(globalColetasRealizadas), function () {
	});
	
	_writeFile("coletasAgrupadas.txt", JSON.stringify(coletasAgrupadas), function () {
	});
}

function AtualizarVariaveisDeColetas(){
	_readFile("globalColetasRealizadas.txt", function (content) {
		if(typeof(content) == 'undefined')
			content = '[]';
			globalColetasRealizadas = JSON.parse(content);
	});
	
	_readFile("coletasAgrupadas.txt", function (content) {
		if(typeof(content) == 'undefined')
			content = '[]';
		coletasAgrupadas = JSON.parse(content);
	});
}