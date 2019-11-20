function listarParLevel2(isVoltar) {
	
	currentParLevel2_Id = null;

	var listaParLevel2 = retornaParLevel2(currentParLevel1_Id);

	var htmlLista = "";

	$(listaParLevel2).each(function (i, o) {
		currentEvaluationSample = getResultEvaluationSample(currentParLevel1_Id, o.Id);

        //FIX para trabalhar de forma correta os valores 
        //que são recebidos do backend com os resultados
        if (currentEvaluationSample.Sample > o.Evaluation.Sample){
            currentEvaluationSample.Evaluation += 1;
            currentEvaluationSample.Sample = 1;
        }

        var style = '';
        if (!podeRealizarColeta(currentEvaluationSample.Evaluation, o.Evaluation.Evaluation)) {
            style = 'style="background-color:#ddd;cursor:not-allowed"';

            htmlLista += '<button type="button" ' + style + ' class="list-group-item col-xs-12" ' +
			'" data-par-level2-id="' + o.Id + '" '+
                'data-total-evaluation="' + o.Evaluation.Evaluation + '"                             ' +
                'data-total-sample="' + o.Evaluation.Sample + '"                                     ' +
                'data-current-evaluation="' + currentEvaluationSample.Evaluation + '"                ' +
                'data-current-sample="' + currentEvaluationSample.Sample + '">                       ' +
                '	<div class="col-xs-3">' + o.Name + '</div>                                      ' +
                '	<div class="col-xs-1">&nbsp;</div>                                                                  ' +
                '	<div class="col-xs-4">Av: ' + o.Evaluation.Evaluation + '/' + o.Evaluation.Evaluation + ' </div>      ' +
                '	<div class="col-xs-4">Am: ' + o.Evaluation.Sample + '/' + o.Evaluation.Sample + ' </div>  ' +
                '</button>';
        } else {
            htmlLista += '<button type="button" class="list-group-item col-xs-12"                                       ' +
			'" data-par-level2-id="' + o.Id + '" '+
                'data-total-evaluation="' + o.Evaluation.Evaluation + '"                                                         ' +
                'data-total-sample="' + o.Evaluation.Sample + '"                                                                 ' +
                'data-current-evaluation="' + currentEvaluationSample.Evaluation + '"                                            ' +
                'data-current-sample="' + currentEvaluationSample.Sample + '">                                                   ' +
                '	<div class="col-xs-3">' + o.Name + '</div>                                                                  ' +
                '	<div class="col-xs-1">&nbsp;</div>                                                                  ' +
                '	<div class="col-xs-4">Av: ' + currentEvaluationSample.Evaluation + '/' + o.Evaluation.Evaluation + ' </div>      ' +
                '	<div class="col-xs-4">Am: ' + currentEvaluationSample.Sample + '/' + o.Evaluation.Sample + ' </div>              ' +
                '</button>';
            atualizaCorAgendamento(o, currentEvaluationSample);
		};
	});

	var voltar = "";

    var voltar = '<a onclick="listarParLevel1(' + isVoltar +'  );" class="btn btn-warning">Voltar</a>';

	html = getHeader() +
		'<div class="container-fluid">                                           ' +
		'	<div class="">                                              ' +
		'		<div class="col-xs-12">                                    ' +
		'                                                                  ' +
		'			<div class="panel panel-primary">                      ' +
		'			  <div class="panel-heading">                          ' +
		'				<h3 class="panel-title">' + voltar + ' Selecione o centro de custo desejado</h3>            ' +
		'			  </div>                                               ' +
		'			  <div class="panel-body">                             ' +
		'				<div class="list-group">                           ' +
		htmlLista +
		'				</div>                                             ' +
		'			  </div>                                               ' +
		'			</div>                                                 ' +
		'                                                                  ' +
		'		</div>                                                     ' +
		'	</div>                                                         ' +
		'</div>';

    $('div#app').html(html);

    setBreadcrumbs();

    if ($(".list-group button").length == 1 && (isVoltar == false || isVoltar == undefined)) {
        $("[data-par-department-id]").trigger('click');
    }
}

$('body').off('click', '[data-par-level2-id]').on('click', '[data-par-level2-id]', function (e) {

	currentParLevel2_Id = parseInt($(this).attr('data-par-level2-id'));
	
	currentTotalEvaluationValue = $(this).attr('data-total-evaluation');
    currentTotalSampleValue = $(this).attr('data-total-sample');
    var currentEvaluationValue = $(this).attr('data-current-evaluation');

    if (!podeRealizarColeta(currentEvaluationValue, currentTotalEvaluationValue)) {
        //alert('Não há mais avaliações disponiveis para realização de coleta para este cargo');

        openMensagem('Não há mais avaliações disponiveis para realização de coleta para este cargo', 'red', 'white');
        closeMensagem(2000);
        return;
    }

    listarParLevels();

});

function retornaParLevel2(parLevel1Id) {
    var listaLevel2 = $.grep(parametrization.listaParVinculoPeso, function (item) {
        return item.ParLevel1_Id == parLevel1Id;
    });

    var parLevel2Ids = $.map(listaLevel2, function (obj) {
        return obj.ParLevel2_Id;
    });

    parLevel2Ids = parLevel2Ids.sort();

    parLevel2Ids = $.unique(parLevel2Ids);

    var listaParLevel2 = [];

    $(parLevel2Ids).each(function (index, parLevel2Id) {

        var listaParLevel2Filter = $.grep(parametrization.listaParLevel2, function (obj) {
            return (obj.Id == parLevel2Id);
        });

        listaParLevel2Filter.forEach(function (item) {

            //Verificar primeiramente os que existem ParCargo e ParCompany obs (frequencia e departamento são obrigatorios)
            var listaEvaluation = [];

            //Caso não existir, buscar os que possuem todas as unidades
			listaEvaluation = $.grep(parametrization.listaParVinculoPeso, function (parEvaluation) {
				return parEvaluation.ParLevel1_Id == parLevel1Id
			});

            if (listaEvaluation.length > 0) {
                item['Evaluation'] = listaEvaluation[0]; //o correto é que retorne somente um, mas caso retorne mais do que um, não pode dar erro
                listaParLevel2.push(item);
            }
        });
    });

    return listaParLevel2;
}