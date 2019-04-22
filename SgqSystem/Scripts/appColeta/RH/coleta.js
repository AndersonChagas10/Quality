var coletaJson = [];

function openColeta(levels) {

    coletaJson = []
    var html = '';
    var coleta = '';

    levels.forEach(function (level1) {
        coleta += getLevel1(level1);
        level1.ParLevel2.forEach(function (level2) {
            coleta += getLevel2(level2);
            level2.ParLevel3.forEach(function (level3) {
                coleta += getInputLevel3(level3, level2, level1);
            });
        });
    });

    html = getHeader()+
		'<div class="container-fluid">                                                                                                                   '+
		'	<div class="">                                                                                                                      '+
		'		<div class="col-xs-12">                                                                                                            '+
		'			<div class="panel panel-primary">                                                                                              '+
		'			  <div class="panel-heading">                                                                                                  '+
		'				<h3 class="panel-title"><a onclick="listarParCargo(currentParCargo_Id);" class="btn btn-warning">Voltar</a> Questionario de Coleta</h3>                                   '+
		'			  </div>                                                                                                                       '+
		'			  <div class="panel-body">                                                                                                     '+
		getContador()+
		'				<form data-form-coleta>                                                                                                    '+
		coleta+
		'					<button class="btn btn-block btn-primary input-lg col-xs-12" data-salvar style="margin-top:10px">Salvar</button>       '+
		'				</form>                                                                                                                    '+
		'			  </div>                                                                                                                       '+
        '       </div>                                                                                                                             '+
        '    </div>                                                                                                                                '+
		'	</div>                                                                                                                                 '+
		'</div>';

    $('div#app').html(html);


}

var currentEvaluationSample = {};
function getContador() {
    currentEvaluationSample = getResultEvaluationSample(currentParDepartment_Id, currentParCargo_Id);
    return '<div class="col-xs-12 alert-info" style="padding-top:10px;padding-bottom:10px">' +
		'	<div class="col-xs-4">       ' +
		'		Avaliação                ' +
		'	</div>                       ' +
		'	<div class="col-xs-4">       ' +
		'		Amostra                  ' +
		'	</div>                       ' +
		'	<div class="col-xs-4">       ' +
		'		&nbsp;                   ' +
		'	</div>                       ' +
		'	<div class="col-xs-4">       ' +
		'		<strong>' + currentEvaluationSample.Evaluation + '/' + currentTotalEvaluationValue + '</strong>    ' +
		'	</div>                       ' +
		'	<div class="col-xs-4">       ' +
		'		<strong>' + currentEvaluationSample.Sample + '/' + currentTotalSampleValue + '</strong>    ' +
		'	</div>                       ' +
		'	<div class="col-xs-4">       ' +
		'		 &nbsp;                  ' +
		'	</div>                       ' +
		'	<div class="clearfix"></div> ' +
		'</div>                          ';
}

function getLevel1(level1) {
    return '<div class="col-xs-12 input-lg">' + level1.Name + '</div>';
}

function getLevel2(level2) {
    return '<div class="col-xs-12 input-lg" style="padding-left:40px">' + level2.Name + '</div>';
}

function getLevel3(level3) {
    return '<div class="col-xs-12">' + level3.Name + '</div>';
}

function getInputLevel3(level3, level2, level1) {

    var retorno = "";

    if (level3.ParLevel3InputType && level3.ParLevel3InputType.Id) {

        retorno += '<div class="col-xs-12" data-linha-coleta ';
        retorno += ' data-conforme="1"';
        retorno += ' data-min="' + level3.ParLevel3Value.IntervalMin + '"';
        retorno += ' data-max="' + level3.ParLevel3Value.IntervalMax + '"';
        retorno += ' data-level1="' + level1.Id + '"';
        retorno += ' data-level2="' + level2.Id + '"';
        retorno += ' data-level3="' + level3.Id + '"';
        retorno += ' style="padding-left:80px">';

        switch (level3.ParLevel3InputType.Id) {

            case 1: //Binário
                retorno += getBinario(level3);
                break;
            case 6: //BinárioComTexto
                retorno += getBinarioComTexto(level3);
                break;
            case 3: //Intervalo
                retorno += getIntervalo(level3);
                break;
            case 9: //IntervaloComObservacao
                retorno += getIntervaloComObservacao(level3);
                break;
            case 11: //Observacao
                retorno += getObservacao(level3)
                break;
            case 8: //Likert
                retorno += getLikert(level3)
                break;
            case 5: //Texto
                retorno += getTexto(level3)
                break;

            default:
                retorno += ""
                break;
        }

        retorno += '</div>';

    }

    return retorno;

}

function getBinario(level3) {

    var html = '<div class="col-xs-4 input-sm">'+
			level3.Name+
		'</div>                                                                                     '+
		'<div class="col-xs-4 input-sm">                                                            '+
		'</div>                                                                                     '+
		'<div class="col-xs-3">                                                                     '+
		'	<button type="button" class ="btn btn-default btn-sm btn-block"                         '+
        '    data-binario data-positivo="'+level3.ParLevel3BoolTrue.Name+'"                          '+
        '    data-negativo="' + level3.ParLevel3BoolFalse.Name + '">' + level3.ParLevel3BoolTrue.Name + '</button>                    ' +
		'</div>                                                                                     '+
		'<div class="col-xs-1">                                                                     '+
		'	<button type="button" class="btn btn-warning pull-right btn-sm" data-na>N/A</button>    '+
		'</div>                                                                                     '+
		'<div class="clearfix"></div>';
    return html;
}

function getBinarioComTexto(level3) {

    var html = '<div class="col-xs-4 input-sm">'+
        level3.Name+
		'</div>                                                                                            '+
		'<div class="col-xs-2">                                                                            '+
		'</div>                                                                                            '+
		'<div class="col-xs-2">                                                                            '+
		'	<input type="text" class="col-xs-12 input-sm" data-texto/>                                     '+
		'</div>                                                                                            '+
		'<div class="col-xs-3">                                                                            '+
		'	<button type="button" class="btn btn-default btn-sm btn-block"                                 '+
		'data-binario data-positivo="'+level3.ParLevel3BoolTrue.Name+'"                                     '+
		'data-negativo="' + level3.ParLevel3BoolFalse.Name + '">'+level3.ParLevel3BoolTrue.Name+ '</button>                                    ' +
		'</div>                                                                                            '+
		'<div class="col-xs-1">                                                                            '+
		'	<button type="button" class="btn btn-warning pull-right btn-sm" data-na>N/A</button>           '+
		'</div>                                                                                            '+
		'<div class="clearfix"></div>';
    return html;
}

function getIntervalo(level3) {

    var html = '<div class="col-xs-4 input-sm">'+
        level3.Name+
		'</div>                                                                                       '+
		'<div class="col-xs-4 input-sm">                                                              '+
		'	MIN: '+level3.ParLevel3Value.IntervalMin+' | MAX: '+level3.ParLevel3Value.IntervalMax+
		'</div>                                                                                       '+
		'<div class="col-xs-3">                                                                       '+
		'	<button type="button" class="btn btn-sm btn-primary col-xs-2" data-minus>-</button>       '+
		'	<input type="text" class="col-xs-8 input input-sm" data-valor/>                           '+
		'	<button type="button" class="btn btn-sm btn-primary col-xs-2" data-plus>+</button>        '+
		'</div>                                                                                       '+
		'<div class="col-xs-1">                                                                       '+
		'	<button type="button" class="btn btn-warning pull-right btn-sm" data-na>N/A</button>      '+
		'</div>                                                                                       '+
		'<div class="clearfix"></div>';
    return html;
}

function getIntervaloComObservacao(level3) {

    var html = '<div class="col-xs-4 input-sm">'+
        level3.Name+
		'</div>'+
		'<div class="col-xs-2 input-sm">'+
		'	MIN: '+level3.ParLevel3Value.IntervalMin+' | MAX: '+level3.ParLevel3Value.IntervalMax+
		'</div>                                                                                   '+
		'<div class="col-xs-2">                                                                   '+
		'	<input type="text" class="col-xs-12 input-sm" data-texto/>                            '+
		'</div>                                                                                   '+
		'<div class="col-xs-3">                                                                   '+
		'	<button type="button" class="btn btn-sm btn-primary col-xs-2" data-minus>-</button>   '+
		'	<input type="text" class="col-xs-8 input-sm" data-valor/>                             '+
		'	<button type="button" class="btn btn-sm btn-primary col-xs-2" data-plus>+</button>    '+
		'</div>                                                                                   '+
		'<div class="col-xs-1">                                                                   '+
		'	<button type="button" class="btn btn-warning pull-right btn-sm" data-na>N/A</button>  '+
		'</div>                                                                                   '+
		'<div class="clearfix"></div>';
    return html;
}

function getObservacao(level3) {

    var html = '<div class="col-xs-4 input-sm">'+
        level3.Name+
		'</div>                                                                                      '+
		'<div class="col-xs-4">                                                                      '+
		'</div>                                                                                      '+
		'<div class="col-xs-3">                                                                      '+
		'	<input type="text" class="col-xs-12 input-sm" data-texto/>                               '+
		'</div>                                                                                      '+
		'<div class="col-xs-1">                                                                      '+
		'	<button type="button" class="btn btn-warning pull-right btn-sm" data-na>N/A</button>     '+
		'</div>                                                                                      '+
		'<div class="clearfix"></div>';
    return html;
}

function getTexto(level3) {

    var html = '<div class="col-xs-4 input-sm">'+
        level3.Name+
		'</div>                                                                                      '+
		'<div class="col-xs-4">                                                                      '+
		'</div>                                                                                      '+
		'<div class="col-xs-3">                                                                      '+
		'	<input type="text" class="col-xs-12 input-sm" data-valor/>                               '+
		'</div>                                                                                      '+
		'<div class="col-xs-1">                                                                      '+
		'	<button type="button" class="btn btn-warning pull-right btn-sm" data-na>N/A</button>     '+
		'</div>                                                                                      '+
		'<div class="clearfix"></div>';
    return html;
}

function getLikert(level3) {

    var html = '<div class="col-xs-4 input-sm">'+
        level3.Name+
		'</div>                                                                                    '+
		'<div class="col-xs-4 input-sm">                                                           '+
		'	Escala: '+level3.ParLevel3Value.IntervalMin+' - '+level3.ParLevel3Value.IntervalMax+
		'</div>                                                                                    '+
		'<div class="col-xs-3">                                                                    '+
		'	<input type="text" class="col-xs-12 input-sm" data-valor/>                             '+
		'</div>                                                                                    '+
		'<div class="col-xs-1">                                                                    '+
		'	<button type="button" class="btn btn-warning pull-right btn-sm" data-na>N/A</button>   '+
		'</div>                                                                                    '+
		'<div class="clearfix"></div>';
    return html;
}

$('body').off('click', '[data-plus]').on('click', '[data-plus]', function (e) {
    var value = parseInt($(this).parent().find('input').val());
    if (isNaN(value))
        value = 1;
    else
        value += 1;
    var input = $(this).parent().find('input')
    input.val(value);
    input.trigger('change');

});

$('body').off('click', '[data-minus]').on('click', '[data-minus]', function (e) {
    var value = parseInt($(this).parent().find('input').val());
    if (isNaN(value))
        value = -1;
    else
        value -= 1;
    var input = $(this).parent().find('input')
    input.val(value);
    input.trigger('change');
});

$('body').off('click', '[data-na]').on('click', '[data-na]', function (e) {
    var linha = $(this).parents('[data-conforme]');
    if (typeof (linha.attr('data-conforme-na')) == 'undefined') {
        resetarLinha(linha);
        linha.addClass('alert-warning');
        linha.attr('data-conforme-na', '');
    } else {
        resetarLinha(linha);
        $(linha).find('input[data-valor]').trigger('change');
    }
});

$('body').off('click', '[data-binario]').on('click', '[data-binario]', function (e) {
    var linha = $(this).parents('[data-conforme]');
    if (linha.attr('data-conforme') == '0') {
        resetarLinha(linha);
        linha.attr('data-conforme', '1');
        $(this).text($(this).attr('data-positivo'))
        $(this).addClass('btn-default');
        $(this).removeClass('btn-danger');
    } else {
        resetarLinha(linha);
        linha.addClass('alert-danger');
        linha.attr('data-conforme', '0');
        $(this).text($(this).attr('data-negativo'))
        $(this).addClass('btn-danger');
        $(this).removeClass('btn-default');
    }
});

$('body').off('change', 'input[data-valor]').on('change', 'input[data-valor]', function (e) {
    var linha = $(this).parents('[data-conforme]');
    debugger
    if ($(this).val() >= $(linha).attr('data-min') && $(this).val() <= $(linha).attr('data-max')) {
        resetarLinha(linha);
        linha.attr('data-conforme', '1');
    } else {
        resetarLinha(linha);
        linha.addClass('alert-danger');
        linha.attr('data-conforme', '0');
    }
});

function resetarLinha(linha) {
    linha.attr('data-conforme', '1');
    linha.removeClass('alert-danger');
    linha.removeClass('alert-warning');
    linha.removeAttr('data-conforme-na');
}

$('body').off('click', '[data-salvar]').on('click', '[data-salvar]', function (e) {
    e.preventDefault();

    //Verifica se existe coleta já realizada para este cargo.
    var coletaAgrupada = null;
    $(coletasAgrupadas).each(function (i, o) {
        if (o.ParCargo_Id == currentParCargo_Id
		&& o.ParDepartment_Id == currentParDepartment_Id) {
            coletaAgrupada = o;
        }
    });

    //Se não existir, cria uma zerada
    if (coletaAgrupada == null) {
        coletaAgrupada = {
            ParDepartment_Id: currentParDepartment_Id,
            ParCargo_Id: currentParCargo_Id,
            Evaluation: currentEvaluationSample.Evaluation,
            Sample: currentEvaluationSample.Sample
        };
    }

    //Insere valores da coleta
    $($('form[data-form-coleta] div[data-linha-coleta]')).each(function (i, o) {
        var data = $(o);
        coletaJson.push(
			{
			    Evaluation: coletaAgrupada.Evaluation,
			    Sample: coletaAgrupada.Sample,
			    ParDepartment_Id: currentParDepartment_Id,
			    ParCargo_Id: currentParCargo_Id,
			    ParLevel1_Id: $(data).attr('data-level1'),
			    ParLevel2_Id: $(data).attr('data-level2'),
			    ParLevel3_Id: $(data).attr('data-level3'),
			    ParCompany_Id: curretParCompany_Id,
			    IntervalMin: $(data).attr('data-min') == "null" ? null : $(data).attr('data-min'),
			    IntervalMax: $(data).attr('data-max') == "null" ? null : $(data).attr('data-max'),
			    IsConform: $(data).attr('data-conforme') == "1",
			    Value: typeof ($(data).find('input[data-valor]').val()) == 'undefined' ? null : $(data).find('input[data-valor]').val(),
			    ValueText: typeof ($(data).find('input[data-texto]').val()) == 'undefined' ? null : $(data).find('input[data-texto]').val(),
			    IsNotEvaluate: $(data).attr('data-conforme-na') == "",
			    CollectionDate: new Date().toISOString(),

			    /*
				"UserSgq_Id":1,
				"Shift_Id":1,
				"Period_Id":1,
				"ParCluster_Id":1,
				"CollectionType":1,
				"Weigth":1,
				"Defects":0,
				"PunishimentValue":1,
				"WeiEvaluation":1,
				"WeiDefects":0,
				"HasPhoto":"0",
				"HaveCorrectiveAction":"0",
				"Parfrequency_Id":1,
				"AlertLevel":"0",
				"ParHeaderField_Id":1,
				"ParHeaderField_Value":""
				*/
			}
		);
    });

    //Se for a primeira, insere na lista de resultados
    if (coletaAgrupada.Evaluation == 1 && coletaAgrupada.Sample == 1) {
        coletasAgrupadas.push(coletaAgrupada);
    }

    //Salva a coleta realizada numa variavel global
    SalvarColetas(coletaJson);

    //Atualiza para a proxima coleta (se precisar adicionar amostra ou avaliação)
    coletaAgrupada = AtualizaContadorDaAvaliacaoEAmostra(coletaAgrupada);
    //atualiza tela de coleta e contadores
    listarParCargo();
});

function AtualizaContadorDaAvaliacaoEAmostra(coletaAgrupada) {
    coletaAgrupada.Sample++; //Incrementa a amostra
    if (coletaAgrupada.Sample > currentTotalSampleValue) {
        coletaAgrupada.Sample = 1;
        coletaAgrupada.Evaluation++;
        if (coletaAgrupada.Evaluation > currentTotalEvaluationValue) {
            //Acabou as avaliações
        }
    }
    return coletaAgrupada;
}

function SalvarColetas(coletaJson) {
    for (var i = 0; i < coletaJson.length; i++) {
        globalColetasRealizadas.push(coletaJson[i]);
    }
    AtualizarArquivoDeColetas();
}











