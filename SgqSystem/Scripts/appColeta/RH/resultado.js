var resultColeta = [];
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
	
	$(resultColeta).each(function (i, o) {
		if(o.ParDepartment_Id == parDepartment_Id && o.ParCargo_Id == parCargo_Id){
			obj = o;
		}
	});
	
	return obj;
	
}

function retornaProximasColetasParaSincronizar(){
	
	var count = 1;
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
			&& data[i].CollectionDate == globalColetasRealizadas[j].CollectionDate
			){
				globalColetasRealizadas.splice(j,1);
				break;
			}
		}
	}
}

function enviarColeta(){
	
	if(globalColetasRealizadas.length > 0){
	
		$.ajax({
			data: JSON.stringify(retornaProximasColetasParaSincronizar()),
			url: urlPreffix + '/api/AppColeta/SetCollect',
			type: 'POST',
			contentType: "application/json",
			success: function (data) {
				atualizaColetasAposSincronizacao(data);
				enviarColeta();
			},
			timeout: 600000,
			error: function () {
			}
		});
		
	}
}