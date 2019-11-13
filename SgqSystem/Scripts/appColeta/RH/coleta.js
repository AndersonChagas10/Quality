var coletaJson = [];

function openColeta(levels) {

    coletaJson = [];
    var html = '';
    var coleta = '';

    levels.forEach(function (level1) {

        var hasLevel2 = false;

        level1.ParLevel2.forEach(function (level2) {

            var hasLevel3 = false;
            var striped = true;

            level2.ParLevel3.forEach(function (level3) {

                var inputLevel3 = getInputLevel3(level3, level2, level1, striped);

                if (inputLevel3.length > 0) {

                    if (hasLevel3 == false) {

                        if (hasLevel2 == false) {
                            coleta += getLevel1(level1);
                            coleta += getParHeaderFieldLevel1(level1);
                            hasLevel2 = true;
                        }

                        coleta += getLevel2(level2, level1);
                        coleta += getParHeaderFieldLevel2(level1, level2);
                        hasLevel3 = true;
                    }

                    coleta += inputLevel3;

                    if (inputLevel3)
                        if (striped)
                            striped = false;
                        else
                            striped = true;
                }
            });
        });
    });

    html = getHeader() +
        '<div class="container-fluid">                                                                                                                   ' +
        '	<div class="">                                                                                                                      ' +
        '		<div class="col-xs-12">                                                                                                            ' +
        '			<div class="panel panel-primary">                                                                                              ' +
        '			  <div class="panel-heading">                                                                                                  ' +
        '				<h3 class="panel-title"><a onclick="validaRota(listarParCargo,currentParCargo_Id);" class="btn btn-warning">Voltar</a> Questionario de Coleta</h3>                                   ' +
        '			  </div>                                                                                                                       ' +
        '			  <div class="panel-body">                                                                                                     ' +
        getContador() +
        getParHeaderFieldDeparment() +
        '				<form data-form-coleta style="text-align:justify">                                                                                                    ' +
        coleta +
        '					<button class="btn btn-block btn-primary input-lg col-xs-12" data-salvar style="margin-top:10px">Salvar</button>       ' +
        '				</form>                                                                                                                    ' +
        '			  </div>                                                                                                                       ' +
        '       </div>                                                                                                                             ' +
        '    </div>                                                                                                                                ' +
        '	</div>                                                                                                                                 ' +
        '</div>';

    $('div#app').html(html);

    setBreadcrumbs();

}

$('body')
    .off('click', '[data-collapse-targeter]')
    .on('click', '[data-collapse-targeter]', function () {
        if ($(this).attr('data-targeter-collapsed') == 'true') {
            $('[data-collapse-target^="' + $(this).attr('data-collapse-targeter') + '-"]').removeClass('hide');
            $('[data-collapse-target="' + $(this).attr('data-collapse-targeter') + '"]').removeClass('hide');
            $(this).attr('data-targeter-collapsed', false);
        } else {
            $('[data-collapse-target^="' + $(this).attr('data-collapse-targeter') + '-"]').addClass('hide');
            $('[data-collapse-target="' + $(this).attr('data-collapse-targeter') + '"]').addClass('hide');
            $(this).attr('data-targeter-collapsed', true);
        }
    });

var currentEvaluationSample = {};

function getContador() {
    currentEvaluationSample = getResultEvaluationSample(currentParDepartment_Id, currentParCargo_Id);
    return '<div class="col-xs-12 alert-info" id="divColeta" style="padding-top:10px;padding-bottom:10px">' +
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
    return '<div class="col-xs-12" style="padding-top:5px;padding-bottom:5px;background-color:#edf5fc;" data-collapse-targeter="' + level1.Id + '"><small>' + level1.Name + '</small></div>';
}

function getLevel2(level2, level1) {
    return '<div class="col-xs-12" style="padding-left:18px;padding-top:5px;padding-bottom:5px;background-color:#fcf4e3;" data-collapse-target="' + level1.Id + '" data-collapse-targeter="' + level1.Id + '-' + level2.Id + '"><small>' + level2.Name + '</small></div>';
}

function getLevel3(level3, level2, level1) {
    return '<div class="col-xs-12" style="margin-bottom:10px;margin-top:10px" data-collapse-target="' + level1.Id + '-' + level2.Id + '">' + level3.Name + '</div>';
}

function getInputLevel3(level3, level2, level1, striped) {

    var retorno = "";

    if (level3.ParLevel3InputType && level3.ParLevel3InputType.Id) {

        var colorStriped = "";
        var conforme = "";

        if (striped)
            colorStriped = "background-color: #e9ecef;";

        if (level3.ParLevel3Value.IsRequiredInt == "1")
            conforme = "";
        else
            conforme = level3.ParLevel3Value.IsDefaultAnswerInt;


        retorno += '<div class="col-xs-12" data-linha-coleta ';
        retorno += ' data-collapse-target="' + level1.Id + '-' + level2.Id + '"';
        retorno += ' data-conforme="' + conforme + '"';
        retorno += ' data-default-answer="' + level3.ParLevel3Value.IsDefaultAnswerInt + '"';
        retorno += ' data-min="' + level3.ParLevel3Value.IntervalMin + '"';
        retorno += ' data-max="' + level3.ParLevel3Value.IntervalMax + '"';
        retorno += ' data-level1="' + level1.Id + '"';
        retorno += ' data-level2="' + level2.Id + '"';
        retorno += ' data-level3="' + level3.Id + '"';
        retorno += ' data-peso="' + level3.Peso + '"';
        retorno += ' style="padding-left:10px;' + colorStriped + '">';

        switch (level3.ParLevel3InputType.Id) {

            case 1: //Binário
                retorno += getBinario(level3);
                break;
            case 2: //Numerodedefeitos
                retorno += getNumerodeDefeitos(level3);
                break;
            case 6: //BinárioComTexto
                retorno += getBinarioComTexto(level3);
                break;
            case 3: //Intervalo
                retorno += getIntervalo(level3);
                break;
            case 7: //IntervaloemMinutos
                retorno += getIntervaloemMinutos(level3);
                break;
            case 9: //IntervaloComObservacao
                retorno += getIntervaloComObservacao(level3);
                break;
            case 11: //Observacao
                retorno += getObservacao(level3);
                break;
            case 8: //Likert
                retorno += getLikert(level3);
                break;
            case 5: //Texto
                retorno += getTexto(level3);
                break;

            default:
                retorno += "";
                return '';
        }

        retorno += '</div>';

    }

    return retorno;

}

function getBinario(level3) {

    var btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na>N/A</button>';

    var html = '';

    var respostaPadrao = "";

    if (level3.ParLevel3XHelp)
        html += '<a style="cursor: pointer;" l3id="' + level3.Id + '" data-info><div class="col-xs-6"><small style="font-weight:550 !important">' + level3.Name + ' (Clique aqui)</small></div></a>';

    else
        html += '<div class="col-xs-6"><small style="font-weight:550 !important">' + level3.Name + '</small></div>';

    if (level3.ParLevel3Value.IsRequiredInt) {
        respostaPadrao = "&nbsp;";
        botao = '<button type="button" class ="btn btn-default btn-sm btn-block" data-binario data-required-answer="1" data-positivo="' + level3.ParLevel3BoolTrue.Name + '" data-negativo="' + level3.ParLevel3BoolFalse.Name + '">' + respostaPadrao + '</button>';
    } else {
        if (level3.ParLevel3Value.IsDefaultAnswerInt == "0")
            respostaPadrao = level3.ParLevel3BoolFalse.Name;
        else
            respostaPadrao = level3.ParLevel3BoolTrue.Name;
        botao = '<button type="button" class ="btn btn-default btn-sm btn-block" data-binario data-required-answer="0" data-positivo="' + level3.ParLevel3BoolTrue.Name + '" data-negativo="' + level3.ParLevel3BoolFalse.Name + '">' + respostaPadrao + '</button>';
    }

    html +=
        '<div class="col-xs-6 no-gutters">' +
        '   <div class="col-xs-10">' +
        botao +
        '   </div>' +
        '   <div class="col-xs-2">' + btnNA + '</div>' +
        '</div>' +
        '<div class="clearfix"></div>';

    return html;
}

function getBinarioComTexto(level3) {

    var btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na>N/A</button>';

    var html = '';

    if (level3.ParLevel3XHelp)
        html += '<a style="cursor: pointer;" l3id="' + level3.Id + '" data-info><div class="col-xs-6"><small style="font-weight:550 !important">' + level3.Name + ' (Clique aqui)</small></div></a>';

    else
        html += '<div class="col-xs-6"><small style="font-weight:550 !important">' + level3.Name + '</small></div>';

    html +=
        '<div class="col-xs-6 no-gutters">' +
        '<div class="col-xs-5">' +
        '	<input type="text" class="col-xs-12 input-sm" data-texto/>' +
        '</div>' +
        '<div class="col-xs-5">' +
        '	<button type="button" class="btn btn-default btn-sm btn-block" data-binario data-positivo="' + level3.ParLevel3BoolTrue.Name + '" data-negativo="' + level3.ParLevel3BoolFalse.Name + '">' + level3.ParLevel3BoolTrue.Name + '</button>' +
        '</div>' +
        '<div class="col-xs-2">' + btnNA + '</div>' +
        // btnInfo +
        '</div>' +
        '<div class="clearfix"></div>';

    return html;
}

function getIntervalo(level3) {

    var btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na>N/A</button>';

    var html = '';

    if (level3.ParLevel3XHelp)
        html += '<a style="cursor: pointer;" l3id="' + level3.Id + '" data-info><div class="col-xs-6"><small style="font-weight:550 !important">' + level3.Name + ' (Clique aqui)</small></div></a>';

    else
        html += '<div class="col-xs-6"><small style="font-weight:550 !important">' + level3.Name + '</small></div>';

    var level3LimitLabel = !!level3.ParLevel3Value.ShowLevel3Limits ? ' MIN: ' + level3.ParLevel3Value.IntervalMin + ' | MAX: ' + level3.ParLevel3Value.IntervalMax : '';

    html +=
        '<div class="col-xs-6 no-gutters">' +
        '   <div class="col-xs-2 input-sm" style="font-size: 8px;">' +
        level3LimitLabel +
        '   </div>' +
        '   <div class="col-xs-8 no-gutters">' +
        '       <div class="col-xs-2" style="padding-right: 0;">' +
        '	        <button type="button" class="btn btn-sm btn-primary btn-block" data-minus>-</button>' +
        '       </div>' +
        '       <div class="col-xs-8" style="padding: 0;">' +
        '	        <input type="text" class="col-xs-12 input input-sm" data-valor/>' +
        '       </div>' +
        '       <div class="col-xs-2" style="padding-left: 0;">' +
        '	        <button type="button" class="btn btn-sm btn-primary btn-block" data-plus>+</button>' +
        '       </div>' +
        '   </div>' +
        '   <div class="col-xs-2">' + btnNA + '</div>' +
        // btnInfo +
        '</div>' +
        '<div class="clearfix"></div>';

    return html;
}

function getIntervaloemMinutos(level3) {

    var btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na>N/A</button>';

    var html = '';

    if (level3.ParLevel3XHelp)
        html += '<a style="cursor: pointer;" l3id="' + level3.Id + '" data-info><div class="col-xs-6"><small style="font-weight:550 !important">' + level3.Name + ' (Clique aqui)</small></div></a>';

    else
        html += '<div class="col-xs-6"><small style="font-weight:550 !important">' + level3.Name + '</small></div>';

    var level3LimitLabel = !!level3.ParLevel3Value.ShowLevel3Limits ? ' MIN: ' + level3.ParLevel3Value.IntervalMin + ' | MAX: ' + level3.ParLevel3Value.IntervalMax : '';

    html +=
        '<div class="col-xs-6 no-gutters">' +
        '<div class="col-xs-2 input-sm" style="font-size: 8px;">' +
        level3LimitLabel +
        '</div>' +
        '<div class="col-xs-3">' +
        '	<input type="text" class="col-xs-12 input-sm" data-texto/>' +
        '</div>' +
        '<div class="col-xs-5 no-gutters">' +
        '   <div class="col-xs-2" style="padding-right: 0;">' +
        '	    <button type="button" class="btn btn-sm btn-primary btn-block" data-minus>-</button>' +
        '   </div>' +
        '   <div class="col-xs-8" style="padding: 0;">' +
        '	    <input type="text" class="col-xs-12 input input-sm" data-valor/>' +
        '   </div>' +
        '   <div class="col-xs-2" style="padding-left: 0;">' +
        '	    <button type="button" class="btn btn-sm btn-primary btn-block" data-plus>+</button>' +
        '   </div>' +
        '</div>' +
        '<div class="col-xs-2">' + btnNA + '</div>' +
        // btnInfo +
        '</div>' +
        '<div class="clearfix"></div>';

    return html;
}

function getIntervaloComObservacao(level3) {

    var btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na>N/A</button>';

    var html = '';

    if (level3.ParLevel3XHelp)
        html += '<a style="cursor: pointer;" l3id="' + level3.Id + '" data-info><div class="col-xs-6"><small style="font-weight:550 !important">' + level3.Name + ' (Clique aqui)</small></div></a>';

    else
        html += '<div class="col-xs-6"><small style="font-weight:550 !important">' + level3.Name + '</small></div>';

    var level3LimitLabel = !!level3.ParLevel3Value.ShowLevel3Limits ? ' MIN: ' + level3.ParLevel3Value.IntervalMin + ' | MAX: ' + level3.ParLevel3Value.IntervalMax : '';


    html +=
        '<div class="col-xs-6 no-gutters">' +
        '<div class="col-xs-2 input-sm" style="font-size: 8px;">' +
        level3LimitLabel +
        '</div>' +
        '<div class="col-xs-3">' +
        '	<input type="text" class="col-xs-12 input-sm" data-texto/>' +
        '</div>' +
        '<div class="col-xs-5 no-gutters">' +
        '   <div class="col-xs-2" style="padding-right: 0;">' +
        '	    <button type="button" class="btn btn-sm btn-primary btn-block" data-minus>-</button>' +
        '   </div>' +
        '   <div class="col-xs-8" style="padding: 0;">' +
        '	    <input type="text" class="col-xs-12 input input-sm" data-valor/>' +
        '   </div>' +
        '   <div class="col-xs-2" style="padding-left: 0;">' +
        '	    <button type="button" class="btn btn-sm btn-primary btn-block" data-plus>+</button>' +
        '   </div>' +
        '</div>' +
        '<div class="col-xs-2">' + btnNA + '</div>' +
        // btnInfo +
        '</div>' +
        '<div class="clearfix"></div>';

    return html;
}

function getObservacao(level3) {

    var btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na>N/A</button>';

    var html = '';

    if (level3.ParLevel3XHelp)
        html += '<a style="cursor: pointer;" l3id="' + level3.Id + '" data-info><div class="col-xs-6"><small style="font-weight:550 !important">' + level3.Name + ' (Clique aqui)</small></div></a>';

    else
        html += '<div class="col-xs-6"><small style="font-weight:550 !important">' + level3.Name + '</small></div>';

    html +=
        '<div class="col-xs-6 no-gutters">' +
        '<div class="col-xs-10">' +
        '	<input type="text" class="col-xs-12 input-sm" data-texto/>' +
        '</div>' +
        '<div class="col-xs-2">' + btnNA + '</div>' +
        // btnInfo +
        '</div>' +
        '<div class="clearfix"></div>';

    return html;
}

function getTexto(level3) {

    var btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na>N/A</button>';

    var html = '';

    if (level3.ParLevel3XHelp)
        html += '<a style="cursor: pointer;" l3id="' + level3.Id + '" data-info><div class="col-xs-6"><small style="font-weight:550 !important">' + level3.Name + ' (Clique aqui)</small></div></a>';

    else
        html += '<div class="col-xs-6"><small style="font-weight:550 !important">' + level3.Name + '</small></div>';

    html +=
        '<div class="col-xs-6 no-gutters">' +
        '<div class="col-xs-10">' +
        '	<input type="text" class="col-xs-12 input-sm" data-valor/>' +
        '</div>' +
        '<div class="col-xs-2">' + btnNA + '</div>' +
        // btnInfo +
        '</div>' +
        '<div class="clearfix"></div>';

    return html;
}

function getNumerodeDefeitos(level3) {

    var btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na>N/A</button>';

    var html = '';

    if (level3.ParLevel3XHelp)
        html += '<a style="cursor: pointer;" l3id="' + level3.Id + '" data-info><div class="col-xs-6"><small style="font-weight:550 !important">' + level3.Name + ' (Clique aqui)</small></div></a>';

    else
        html += '<div class="col-xs-6"><small style="font-weight:550 !important">' + level3.Name + '</small></div>';

    html +=
        '<div class="col-xs-6 no-gutters">' +
        '<div class="col-xs-10">' +
        '	<input type="number" class="col-xs-12 input-sm" data-valor/>' +
        '</div>' +
        '<div class="col-xs-2">' + btnNA + '</div>' +
        // btnInfo +
        '</div>' +
        '<div class="clearfix"></div>';

    return html;
}

function getLikert(level3) {

    var btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na>N/A</button>';

    var html = '';

    if (level3.ParLevel3XHelp)
        html += '<a style="cursor: pointer;" l3id="' + level3.Id + '" data-info><div class="col-xs-6"><small style="font-weight:550 !important">' + level3.Name + ' (Clique aqui)</small></div></a>';

    else
        html += '<div class="col-xs-6"><small style="font-weight:550 !important">' + level3.Name + '</small></div>';

    var level3LimitLabel = !!level3.ParLevel3Value.ShowLevel3Limits ? ' MIN: ' + level3.ParLevel3Value.IntervalMin + ' | MAX: ' + level3.ParLevel3Value.IntervalMax : '';

    html +=
        '<div class="col-xs-6 no-gutters">' +
        '   <div class="col-xs-2 input-sm" style="font-size: 8px;">' +
        level3LimitLabel +
        '   </div>' +
        '   <div class="col-xs-8">' +
        '	    <input type="text" class="col-xs-12 input-sm" data-valor/>' +
        '   </div>' +
        '   <div class="col-xs-2">' + btnNA + '</div>' +
        // btnInfo +
        '</div>' +
        '<div class="clearfix"></div>';

    return html;
}

$('body').off('click', '[data-plus]').on('click', '[data-plus]', function (e) {
    var value = parseInt($(this).parent().parent().find('input').val());
    if (isNaN(value))
        value = 1;
    else
        value += 1;
    var input = $(this).parent().parent().find('input');
    input.val(value);
    input.trigger('change');

});

$('body').off('click', '[data-minus]').on('click', '[data-minus]', function (e) {
    var value = parseInt($(this).parent().parent().find('input').val());
    if (isNaN(value))
        value = -1;
    else
        value -= 1;
    var input = $(this).parent().parent().find('input');
    input.val(value);
    input.trigger('change');
});

$('body').off('click', '[data-na]').on('click', '[data-na]', function (e) {
    var linha = $(this).parents('[data-conforme]');
    if (typeof (linha.attr('data-conforme-na')) == 'undefined') {
        resetarLinha(linha);
        linha.addClass('alert-warning');
        linha.attr('data-conforme-na', '');

        var botao = $(linha).find('button[data-required-answer]');
        if (botao.attr('data-required-answer') == "1") {
            linha.attr('data-conforme', "");
            $(botao).text('');
            $(botao).html('&nbsp;');
        }
    } else {
        resetarLinha(linha);
        $(linha).find('input[data-valor]').trigger('change');
    }
});

function validaCampoEmBrancoNA() {
    if (linha.attr('data-default-answer') == "1") {
        linha.attr('data-conforme', " ");
    }
  
}

$('body').off('click', '[data-binario]').on('click', '[data-binario]', function (e) {
    var linha = $(this).parents('[data-conforme]');

    resetarLinha(linha);

    if (linha.attr('data-conforme') == "" || linha.attr('data-conforme') == null) {
        linha.attr('data-conforme', linha.attr('data-default-answer'));

    } else if (linha.attr('data-conforme') == linha.attr('data-default-answer')) {
        linha.attr('data-conforme', linha.attr('data-default-answer') == "0" ? "1" : "0");
    } else {
        linha.addClass('alert-secundary');
        if ($(this).attr('data-required-answer') == "1") {
            linha.attr('data-conforme', "");
        } else {
            linha.attr('data-conforme', linha.attr('data-default-answer'));
        }
    }

    if (linha.attr('data-conforme') == "1") {
        $(this).text($(this).attr('data-positivo'));
    } else if (linha.attr('data-conforme') == "0") {
        $(this).text($(this).attr('data-negativo'));
    } else {
        $(this).text('');
        $(this).html('&nbsp;');
    }

    $(this).addClass('btn-default');
    $(this).removeClass('btn-secundary');

});

$('body').off('change', 'input[data-valor]').on('change', 'input[data-valor]', function (e) {
    var linha = $(this).parents('[data-conforme]');

    if (parseFloat($(this).val()) >= parseFloat($(linha).attr('data-min'))
        && parseFloat($(this).val()) <= parseFloat($(linha).attr('data-max'))) {
        resetarLinha(linha);
        linha.attr('data-conforme', '1');
    } else {
        resetarLinha(linha);
        linha.addClass('alert-secundary');
        linha.attr('data-conforme', '0');
    }
});

$('body').off('click', '[data-info]').on('click', '[data-info]', function (e) {

    var l3Id = $(this).attr('l3id');

    var l3xHelp = $.grep(parametrization.listaParLevel3XHelp, function (obj) {
        return obj.ParLevel3_Id == l3Id;

    })[0];

    var body = l3xHelp.Corpo;
    var title = l3xHelp.Titulo;

    var btnClose = '<button class="btn btn-primary pull-right" onclick="closeModal()">Fechar</button>';
    var modal = '<h4>' + title + '</h4>';
    var corpo =
        '<div class="container">' +
        '<div class="row" style="overflow:auto">' +
        modal +
        '<hr>' +
        '<div style="text-align:center">' +
        body +
        '</div>' +
        '<hr>' +
        btnClose +
        '</div>' +
        '</div>';

    openModal(corpo);

});


function resetarLinha(linha) {
    linha.removeClass('alert-secundary');
    linha.removeClass('alert-warning');
    linha.removeAttr('data-conforme-na');
}

$('body').off('click', '[data-salvar]').on('click', '[data-salvar]', function (e) {

    e.preventDefault();

    if (!HeaderFieldsIsValid()) {
        return false;
    }

    if (!ColetasIsValid()) {
        return false;
    }

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

    //var collectionHeaderFields = getCollectionHeaderFields();
    //console.table(collectionHeaderFields);

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
                ParCompany_Id: currentParCompany_Id,
                IntervalMin: $(data).attr('data-min') == "null" ? null : $(data).attr('data-min'),
                IntervalMax: $(data).attr('data-max') == "null" ? null : $(data).attr('data-max'),
                IsConform: $(data).attr('data-conforme') == "1",
                Value: typeof ($(data).find('input[data-valor]').val()) == 'undefined' ? null : $(data).find('input[data-valor]').val(),
                ValueText: typeof ($(data).find('input[data-texto]').val()) == 'undefined' ? null : $(data).find('input[data-texto]').val(),
                IsNotEvaluate: $(data).attr('data-conforme-na') == "",
                CollectionDate: getCurrentDate(),
                UserSgq_Id: currentLogin.Id,
                Weigth: $(data).attr('data-peso'),
                WeiEvaluation: $(data).attr('data-peso'),
                Defects: $(data).attr('data-conforme') == "1" ? 0 : 1,
                WeiDefects: ($(data).attr('data-conforme') == "1" ? 0 : 1) * parseInt($(data).attr('data-peso')),
                Parfrequency_Id: parametrization.currentParFrequency_Id
                /*
				"Shift_Id":1,
				"Period_Id":1,
				"ParCluster_Id":1,
				"CollectionType":1,
				"PunishimentValue":1,
				"HasPhoto":"0",
				"HaveCorrectiveAction":"0",
				"AlertLevel":"0",
				"ParHeaderField_Id":1,
				"ParHeaderField_Value":""
				*/
            }
        );
    });

    processAlertRole(coletaJson);

    var cabecalhos = getCollectionHeaderFields();

    if (cabecalhos) {
        cabecalhos.forEach(function (cabecalho) {
            //campos de cabeçalhos
            coletaJson.push(cabecalho);
        });
    }

    //Se for a primeira, insere na lista de resultados
    if (coletaAgrupada.Evaluation == 1 && coletaAgrupada.Sample == 1) {
        coletasAgrupadas.push(coletaAgrupada);
    }

    //Salva a coleta realizada numa variavel global
    SalvarColetas(coletaJson);

    //Atualiza para a proxima coleta (se precisar adicionar amostra ou avaliação)
    coletaAgrupada = AtualizaContadorDaAvaliacaoEAmostra(coletaAgrupada);

    //Mostra mensagem de que a coleta foi realizada com sucesso e fecha após 3 segundos
    openMensagem("Amostra salva com sucesso!", "blue", "white");
    closeMensagem(3000);

    if (coletaAgrupada.Sample == 1) {
        //atualiza tela de coleta e contadores
        listarParCargo(true);
    } else {
        listarParLevels();
        $("html, body").animate({ scrollTop: 0 }, "fast");
    }
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

function OpenCorrectiveAction(coleta) {

    var correctiveAction = {};

    //Pegar os dados correntes
    correctiveAction.CollectionLevel2 = {
        ParLevel1_Id: coleta.ParLevel1_Id,
        ParLevel2_Id: coleta.ParLevel2_Id,
        UnitId: coleta.ParCompany_Id,
        //Shift: 1,
        EvaluationNumber: coleta.Evaluation,
        Sample: coleta.Sample,
        ParDepartment_Id: coleta.ParDepartment_Id,
        ParCargo_Id: coleta.ParCargo_Id,
        //ParCluster_Id: 1,
        CollectionDate: getCurrentDate()
    };

    //var tarefa = $.map(parametrization.listaParLevel3, function (val, i) {
    //    if (val.Id == coleta.ParLevel3_Id) {
    //        return val;
    //    }
    //});
    // + '<h4> Tarefa: "' + tarefa[0].Name + '"</h4>' verificar uma forma de mostrar a tarefa que esta nao conforme
    var modal = '<h3 style="font-weight:bold;">Ação Corretiva</h3>';
    var selectUsers = '<option value="">Selecione...</option><option value="1">Pato Donald</option>';

    var date = stringToDate(currentCollectDate.toJSON());

    var body = '<div class="form-group">' +
        '<div class="form-group col-xs-12">' +
        '<strong>Informações</strong>' +
        '<small><br/>Data/Hora: ' + currentCollectDate.toLocaleDateString() + ' ' + currentCollectDate.toLocaleTimeString() +
        '<br/>Monitor: ' + currentLogin.Name +
        '<br/>Tarefa: ' + $.grep(parametrization.listaParLevel3, function (o, i) { return o.Id == coleta.ParLevel3_Id; })[0].Name +
        '<br/>Frequência: ' + $.grep(parametrization.listaParFrequency, function (item) { return item.Id == parametrization.currentParFrequency_Id; })[0].Name +
        '</small></div>' +

        '<div class="form-group col-xs-12">' +
        '<label>Descrição da Falha:</label>' +
        '<input name="DescriptionFailure" id="descriptionFailure" class="col-sx-12 form-control" style="height: 80px;">' +
        '</div>' +
        '<div class="form-group col-xs-12">' +
        '<label for="email">Ação Corretiva Imediata:</label>' +
        '<input name="ImmediateCorrectiveAction" id="immediateCorrectiveAction" class="form-control" style="height: 80px;">' +
        '</div>' +
        '<div class="form-group col-xs-12">' +
        '<label for="email">Ação Preventiva:</label>' +
        '<input name="PreventativeMeasure" id="preventativeMeasure" class="form-control" style="height: 80px;">' +
        '</div>';

    var corpo =
        '<div class="container">' +
        '<div class="row" style="overflow:auto">' +
        modal +
        '<hr>' +
        '<div>' +
        body +
        '</div>' +
        '<hr>' +
        '<div class="form-group col-xs-6">' +
        '<button class="btn btn-primary" id="btnSendCA">Salvar Ação Corretiva</button>' +
        '</div>' +
        '</div>' +
        '</div>';

    openModal(corpo, 'white', 'black');

    $('#btnSendCA').off().on('click', function () {

        //Inserir collectionLevel2 dentro do obj
        correctiveAction.AuditorId = currentLogin.Id;
        correctiveAction.ImmediateCorrectiveAction = $('#immediateCorrectiveAction').val();
        correctiveAction.PreventativeMeasure = $('#preventativeMeasure').val();
        correctiveAction.DescriptionFailure = $('#descriptionFailure').val();

        //Salvar corrective action na lista de correctiveAction
        globalAcoesCorretivasRealizadas.push(correctiveAction);

        closeModal();

    });

}

function getCollectionHeaderFields() {

    var collectionHeaderFied = [];

    $('#headerFieldDepartment input, #headerFieldDepartment select').each(function () {

        $self = $(this);

        //TODO: validar se os campos de cabeçalho obrigatórios foram preenchidos;
        if ($self.val())

            collectionHeaderFied.push({
                ParHeaderField_Id: $self.attr("parheaderfield_id"),
                ParHeaderField_Value: $self.val(),
                Evaluation: currentEvaluationSample.Evaluation,
                Sample: currentEvaluationSample.Sample,
                ParDepartment_Id: currentParDepartment_Id,
                ParCargo_Id: currentParCargo_Id,
                ParCompany_Id: currentParCompany_Id,
                CollectionDate: getCurrentDate(),
                UserSgq_Id: currentLogin.Id,
                Parfrequency_Id: parametrization.currentParFrequency_Id
            });

    });

    $('#headerFieldLevel1 input, #headerFieldLevel1 select').each(function () {

        $self = $(this);

        //TODO: validar se os campos de cabeçalho obrigatórios foram preenchidos;
        if ($self.val())

            collectionHeaderFied.push({
                ParHeaderField_Id: $self.attr("parheaderfield_id"),
                ParHeaderField_Value: $self.val(),
                Evaluation: currentEvaluationSample.Evaluation,
                Sample: currentEvaluationSample.Sample,
                ParDepartment_Id: currentParDepartment_Id,
                ParCargo_Id: currentParCargo_Id,
                ParCompany_Id: currentParCompany_Id,
                CollectionDate: getCurrentDate(),
                UserSgq_Id: currentLogin.Id,
                ParLevel1_Id: $self.parents('#headerFieldLevel1').attr('parLevel1Id'),
                Parfrequency_Id: parametrization.currentParFrequency_Id
            });

    });


    $('#headerFieldLevel2 input, #headerFieldLevel2 select').each(function () {

        $self = $(this);

        //TODO: validar se os campos de cabeçalho obrigatórios foram preenchidos;
        if ($self.val())

            collectionHeaderFied.push({
                ParHeaderField_Id: $self.attr("parheaderfield_id"),
                ParHeaderField_Value: $self.val(),
                Evaluation: currentEvaluationSample.Evaluation,
                Sample: currentEvaluationSample.Sample,
                ParDepartment_Id: currentParDepartment_Id,
                ParCargo_Id: currentParCargo_Id,
                ParCompany_Id: currentParCompany_Id,
                CollectionDate: getCurrentDate(),
                UserSgq_Id: currentLogin.Id,
                ParLevel1_Id: $self.parents('#headerFieldLevel2').attr('parLevel1Id'),
                ParLevel2_Id: $self.parents('#headerFieldLevel2').attr('parLevel2Id'),
                Parfrequency_Id: parametrization.currentParFrequency_Id
            });

    });

    return collectionHeaderFied;
}

function ColetasIsValid() {
    var errorCount = 0;
    $($('form[data-form-coleta] div[data-linha-coleta]')).each(function (i, o) {
        var data = $(o);
        if ($(data).attr('data-conforme-na') != "") {
            if ($(data).attr('data-conforme') == ""
                || $(data).attr('data-conforme') == null
                || $(data).attr('data-conforme') == "undefined") {
                openMensagem("Obrigatório responder todas as Tarefas.", "blue", "white");
                mostraPerguntasObrigatorias(data);
                errorCount++;
                closeMensagem(2000);
            }
        }
    });
    if (errorCount > 0)
        return false;
    else
        return true;
  
}

function mostraPerguntasObrigatorias(data) {

    //verifica se tem campos obrigatorios que nao estao preenchidos e realiza o focus neles
    if ($(data).attr('data-conforme') == 0 || $(data).attr('data-conforme') == "0") {
        $('html, body').animate({
            scrollTop: $(data).parent().offset().top
        }, 300);
        return false;
    }
}

function HeaderFieldsIsValid() {

    retorno = true;

    $('#headerFieldDepartment input, #headerFieldDepartment select, #headerFieldLevel1 input, #headerFieldLevel1 select, #headerFieldLevel2 input, #headerFieldLevel2 select').each(function () {

        $self = $(this);

        $self.css("background-color", "");

        if ($self.attr("data-required") == "true") {

            if ($self.val() == null || $self.val() == undefined || $self.val() == "") {
                $self.css("background-color", "#ffc1c1");
                retorno = false;
            }
        }

    });

    if (!retorno) {
        openMensagem("Campos de cabeçalho obrigatórios não preenchidos", "blue", "white");
        closeMensagem(2000);
    }

    //verifica se tem campos obrigatorios que nao estao preenchidos e realiza o focus neles
    $.each($('[data-required=true]'), function (i, o) {
        if ($(o).val() == 0 || $(o).val() == "") {
            $('html, body').animate({
                scrollTop: $(o).parent().offset().top
            }, 300);
            return false;
        }
    });

    return retorno;
}