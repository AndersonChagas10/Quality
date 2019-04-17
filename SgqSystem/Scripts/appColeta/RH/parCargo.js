function listarParCargo() {

    cleanGlobalVarParCargo();

    if (!parametrization.listaParCargoXDepartment.length > 0) {
        return false;
    }

    var listaParCargoXDepartment = $.grep(parametrization.listaParCargoXDepartment, function (item) {
        return item.ParDepartment_Id == currentParDepartment_Id;
    });

    var listaParCargo = [];

    $(listaParCargoXDepartment).each(function (item, obj) {

        var listaParCargoFilter = $.grep(parametrization.listaParCargo, function (parCargo) {
            return (parCargo.Id == obj.ParCargo_Id || obj.ParCargo_Id == null);
        });

        listaParCargoFilter.forEach(function(item){
			
			var listaEvaluation = $.grep(parametrization.listaParEvaluationXDepartmentXCargoAppViewModel, function (parEvaluation) {
				return (parEvaluation.ParCargo_Id == obj.ParCargo_Id || parEvaluation.ParCargo_Id == null) && (parEvaluation.ParDepartment_Id == currentParDepartment_Id || parEvaluation.ParDepartment_Id == null);
			});		

			if(listaEvaluation.length > 0){
				item['Evaluation'] = listaEvaluation[0];
				listaParCargo.push(item);
			}
        });

    });

    var htmlParCargo = "";

    $(listaParCargo).each(function (i, o) {
		currentEvaluationSample = getResultEvaluationSample(currentParDepartment_Id,o.Id);
		
		var style = '';
		if(!podeRealizarColeta(currentEvaluationSample.Evaluation,o.Evaluation.Evaluation)){
			style = 'style="background-color:#ddd;cursor:not-allowed"';

			htmlParCargo += `<button type="button" ${style} class="list-group-item col-xs-12"
				data-par-cargo-id="${o.Id}" 
				data-total-evaluation="${o.Evaluation.Evaluation}"
				data-total-sample="${o.Evaluation.Sample}"
				data-current-evaluation="${currentEvaluationSample.Evaluation}"
				data-current-sample="${currentEvaluationSample.Sample}">
					<div class="col-sm-4">${o.Name}</div>
					<div class="col-sm-4">&nbsp;</div>
					<div class="col-sm-4">&nbsp;</div>
				</button>`;
		}else{
			htmlParCargo += `<button type="button" class="list-group-item col-xs-12"
				data-par-cargo-id="${o.Id}" 
				data-total-evaluation="${o.Evaluation.Evaluation}"
				data-total-sample="${o.Evaluation.Sample}"
				data-current-evaluation="${currentEvaluationSample.Evaluation}"
				data-current-sample="${currentEvaluationSample.Sample}">
					<div class="col-sm-4">${o.Name}</div>
					<div class="col-sm-4">Av: ${currentEvaluationSample.Evaluation}/${o.Evaluation.Evaluation} </div>
					<div class="col-sm-4">Am: ${currentEvaluationSample.Sample}/${o.Evaluation.Sample} </div>
				</button>`;
		}
    });

    var voltar = `<a onclick="listarParDepartment(${currentParDepartmentParent_Id});">Voltar</a>`;

    html = `
    ${getHeader()}
    <div class="container">
        <div class="row">
            <div class="col-xs-12">
                <div class="panel panel-primary">
                  <div class="panel-heading">
                    <h3 class="panel-title">${voltar}</h3>
                  </div>
                  <div class="panel-body">
                    <div class="list-group">
                        ${htmlParCargo}
                    </div>
                  </div>
                </div>
            </div>
        </div>
    </div>`;

    $('div#app').html(html);

}

function cleanGlobalVarParCargo(){
    currentParCargo_Id = null;
}

function podeRealizarColeta(_currentEvaluation, _currentTotalEvaluation){
	_currentTotalEvaluation = _currentTotalEvaluation > 0 ? _currentTotalEvaluation : 1;
	return !(_currentEvaluation > _currentTotalEvaluation);
}

$('body').on('click', '[data-par-cargo-id]', function (e) {  

	currentTotalEvaluationValue = $(this).attr('data-total-evaluation');
	currentTotalSampleValue = $(this).attr('data-total-sample');
	var currentEvaluationValue = $(this).attr('data-current-evaluation');
	
	if(!podeRealizarColeta(currentEvaluationValue,currentTotalEvaluationValue)){
		alert('Não há mais avaliações disponiveis para realização de coleta para este cargo');
		return;
	}

    currentParCargo_Id = parseInt($(this).attr('data-par-cargo-id'));

    listarParLevels();

});