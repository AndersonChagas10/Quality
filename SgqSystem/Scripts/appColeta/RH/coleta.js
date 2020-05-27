var coletaJson = [];
var interacaoComFormulario = 0;

function openColeta(levels) {
    interacaoComFormulario = 0;

    coletaJson = [];
    var html = '';
    var coleta = '';

    levels.forEach(function (level1) {

        var hasLevel2 = false;

        if (level1.ParLevel2 != undefined)
            level1.ParLevel2.forEach(function (level2) {

                var hasLevel3 = false;
                var striped = true;

                if (level2.ParLevel3 != undefined)
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

    //chamar o metodo que valida e monta se pode criar a qualificação 
    $('[data-binario]').parents('[data-conforme]').each(function (i, o) {
        validateShowQualification(o);
    });


    $('.panel-body button, .panel-body input, .panel-body select').off('click').on('click', function (e) {
        interacaoComFormulario++;
    });

    setBreadcrumbs();
}

$('body')
    .off('click', '[data-collapse-targeter]')
    .on('click', '[data-collapse-targeter]', function () {
        if ($(this).attr('data-targeter-collapsed') == 'true') {
            $('[data-collapse-target^="' + $(this).attr('data-collapse-targeter') + '-"]').removeClass('hide');
            $('[data-collapse-target="' + $(this).attr('data-collapse-targeter') + '"]').removeClass('hide');
            $(this).attr('data-targeter-collapsed', false);
            $(this).find('[ data-toggle]').removeClass('fa fa-caret-right').addClass('fa fa-caret-down');
            $('[data-collapse-target="' + $(this).attr('data-collapse-targeter') + '"]').find('[data-toggle]').removeClass('fa fa-caret-right').addClass('fa fa-caret-down');
        } else {
            $('[data-collapse-target^="' + $(this).attr('data-collapse-targeter') + '-"]').addClass('hide');
            $('[data-collapse-target="' + $(this).attr('data-collapse-targeter') + '"]').addClass('hide');
            $(this).attr('data-targeter-collapsed', true);
            $(this).find('[ data-toggle]').removeClass('fa fa-caret-down').addClass('fa fa-caret-right');
        }
    });

//$('body')
//    .off('change', '[data-level3] select:visible')
//    .on('change', '[data-level3] select:visible', function () {

//        var qualificationLevel3Value_Value = $(this).parents('[data-level3]').attr('data-ParQualificationLevel3Value');

//        if (qualificationLevel3Value_Value != null || qualificationLevel3Value_Value != "") {
//            var qualification_Id = $("[data-qualificationSelect] :selected").val();
//        }

//        $("input[data-valor]").trigger('change');
//    });


$('body')
    .off('input', '[data-level3] input:visible')
    .on('input', '[data-level3] input:visible', function () {
        var id = $(this).parents('[data-level3]').attr('data-level3');

        if (id != null || id != "") {
            id = $(this).attr('id');
        }

        $.each($('[data-equacao]:visible'), function (i, o) {

            if ($(o).attr('data-equacao').indexOf('{' + id + '}') >= 0 || $(o).attr('data-equacao').indexOf('{' + id + '?}') >= 0) {

                var equacao = $(o).attr('data-equacao');

                const regex = /{([^}]+)}/g;
                var m;

                while ((m = regex.exec($(o).attr('data-equacao'))) !== null) {
                    // This is necessary to avoid infinite loops with zero-width matches
                    if (m.index === regex.lastIndex) {
                        regex.lastIndex++;
                    }

                    var valor = $('input[id="' + m[1].replace('?', '') + '"]').val();
                    if (valor)
                        equacao = equacao.replace(m[0], valor);
                    else {
                        if (m[1].indexOf('?') >= 0) {
                            equacao = equacao.replace(m[0], 0);
                        }
                    }
                }

                if (equacao.indexOf('{') != -1) {
                    equacao = "";
                }
                else {
                    equacao = eval(equacao);
                }
                $(o).val(equacao);
                $(o).trigger('input');
            }
        });
        $("input[data-valor]").trigger('change');
    });


var currentEvaluationSample = {};

function getContador() {
    currentEvaluationSample = getResultEvaluationSample(currentParDepartment_Id, currentParCargo_Id, currentParCluster_Id);
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
    return '<div class="col-xs-12" style="padding-top:5px;padding-bottom:5px;background-color:#edf5fc;" data-collapse-targeter="' + level1.Id + '"><i class="fa fa-caret-down" data-toggle style="margin-right: 5px;"></i><small>' + level1.Name + '</small></div>';
}

function getLevel2(level2, level1) {
    return '<div class="col-xs-12" style="padding-left:18px;padding-top:5px;padding-bottom:5px;background-color:#fcf4e3;" data-collapse-target="' + level1.Id + '" data-collapse-targeter="' + level1.Id + '-' + level2.Id + '"><i class="fa fa-caret-down" data-toggle style="margin-right: 5px;"></i><small>' + level2.Name + '</small></div>';
}

function getLevel3(level3, level2, level1) {
    return '<div class="col-xs-12" style="margin-bottom:10px;margin-top:10px" data-collapse-target="' + level1.Id + '-' + level2.Id + '">' + level3.Name + '</div>';
}

function getInputLevel3(level3, level2, level1, striped) {

    var retorno = "";

    var htmlLinhaHeaderFieldGeral = getParHeaderFieldGeralLevel3(level1, level2, level3, striped);

    var htmlLinhaParQualification = getParQualification(level1, level2, level3);

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
        retorno += ' data-peso-parametrizado="' + level3.Peso + '"';
        retorno += ' style="padding-left:10px;' + colorStriped + '">';

        switch (level3.ParLevel3InputType.Id) {

            case 1: //Binário
                retorno += getBinario(level3);
                break;
            case 2: //Numerodedefeitos
                retorno += getNumerodeDefeitos(level3);
                break;
            case 15: //NumerodedefeitosComTexto
                retorno += getNumerodeDefeitosComTexto(level3);
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
            case 10: //Resultado
                retorno += getResultado(level3);
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

        retorno += htmlLinhaHeaderFieldGeral;

        retorno += htmlLinhaParQualification;
    }

    return retorno;

}

function getBinario(level3) {

    var btnNA = "";

    if (level3.ParLevel3Value.IsAtiveNA == true) {
        btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na>N/A</button>';
    }

    var html = '';

    var respostaPadrao = "";

    if (level3.ParLevel3XHelp)
        html += '<a style="cursor: pointer;" l3id="' + level3.Id + '" data-info><div class="col-xs-6"><small style="font-weight:550 !important">' + level3.Name + ' (Clique aqui)</small></div></a>';

    else
        html += '<div class="col-xs-6"><small style="font-weight:550 !important">' + level3.Name + '</small></div>';

    if (level3.ParLevel3Value.IsRequiredInt) {
        respostaPadrao = "&nbsp;";
        botao = '<button type="button" class ="btn btn-default btn-sm btn-block" data-binario data-required-answer="1" data-tarefa data-positivo="' + level3.ParLevel3BoolTrue.Name + '" data-negativo="' + level3.ParLevel3BoolFalse.Name + '">' + respostaPadrao + '</button>';
    } else {
        if (level3.ParLevel3Value.IsDefaultAnswerInt == "0")
            respostaPadrao = level3.ParLevel3BoolFalse.Name;
        else
            respostaPadrao = level3.ParLevel3BoolTrue.Name;
        botao = '<button type="button" class ="btn btn-default btn-sm btn-block" data-binario data-required-answer="0" data-tarefa data-positivo="' + level3.ParLevel3BoolTrue.Name + '" data-negativo="' + level3.ParLevel3BoolFalse.Name + '">' + respostaPadrao + '</button>';
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

    var btnNA = "";

    if (level3.ParLevel3Value.IsAtiveNA == true) {
        btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na>N/A</button>';
    }

    var html = '';

    var respostaPadrao = "";

    if (level3.ParLevel3XHelp)
        html += '<a style="cursor: pointer;" l3id="' + level3.Id + '" data-info><div class="col-xs-6"><small style="font-weight:550 !important">' + level3.Name + ' (Clique aqui)</small></div></a>';
    else
        html += '<div class="col-xs-6"><small style="font-weight:550 !important">' + level3.Name + '</small></div>';

    if (level3.ParLevel3Value.IsRequiredInt) {
        respostaPadrao = "&nbsp;";
        botao = '<button type="button" class ="btn btn-default btn-sm btn-block" data-binario data-tarefa data-required-answer="1" data-positivo="' + level3.ParLevel3BoolTrue.Name + '" data-negativo="' + level3.ParLevel3BoolFalse.Name + '">' + respostaPadrao + '</button>';
    } else {
        if (level3.ParLevel3Value.IsDefaultAnswerInt == "0")
            respostaPadrao = level3.ParLevel3BoolFalse.Name;
        else
            respostaPadrao = level3.ParLevel3BoolTrue.Name;
        botao = '<button type="button" class ="btn btn-default btn-sm btn-block" data-binario data-tarefa data-required-answer="0" data-positivo="' + level3.ParLevel3BoolTrue.Name + '" data-negativo="' + level3.ParLevel3BoolFalse.Name + '">' + respostaPadrao + '</button>';
    }
    //html +=
    //    '<div class="col-xs-6 no-gutters">' +
    //    '   <div class="col-xs-10">' +
    //    botao +
    //    '   </div>' +
    //    '   <div class="col-xs-2">' + btnNA + '</div>' +
    //    '</div>' +
    //    '<div class="clearfix"></div>';

    var mensagemPadrao = level3.ParLevel3Value.DefaultMessageText !== null ? level3.ParLevel3Value.DefaultMessageText : "";
    var tamanhoPermitido = level3.ParLevel3Value.StringSizeAllowed !== null ? level3.ParLevel3Value.StringSizeAllowed : 100;
    var input = '<input type="text" class="col-xs-12 input-sm" style="text-align: center;" maxlength="' + tamanhoPermitido + '" placeholder="' + mensagemPadrao + '" data-required-text="' + level3.ParLevel3Value.IsNCTextRequired + '" data-texto/>';


    html +=
        '<div class="col-xs-6 no-gutters">' +
        '<div class="col-xs-5">' +
        botao +
        '</div>' +
        '<div class="col-xs-5">' +
        input +
        '</div>' +
        '<div class="col-xs-2">' + btnNA + '</div>' +
        // btnInfo +
        '</div>' +
        '<div class="clearfix"></div>';

    return html;
}

function getIntervalo(level3) {

    var btnNA = "";

    if (level3.ParLevel3Value.IsAtiveNA == true) {
        btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na>N/A</button>';
    }

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
        '	        <input type="text" class="col-xs-12 input input-sm" data-tarefa data-valor/>' +
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

    var btnNA = "";

    if (level3.ParLevel3Value.IsAtiveNA == true) {
        btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na>N/A</button>';
    }

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
        '	    <input type="text" class="col-xs-12 input input-sm" data-tarefa data-valor/>' +
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

    var btnNA = "";

    if (level3.ParLevel3Value.IsAtiveNA == true) {
        btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na>N/A</button>';
    }

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
        '	    <input type="text" class="col-xs-12 input input-sm" data-tarefa data-valor/>' +
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

    var btnNA = "";

    if (level3.ParLevel3Value.IsAtiveNA == true) {
        btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na>N/A</button>';
    }

    var html = '';

    if (level3.ParLevel3XHelp)
        html += '<a style="cursor: pointer;" l3id="' + level3.Id + '" data-info><div class="col-xs-6"><small style="font-weight:550 !important">' + level3.Name + ' (Clique aqui)</small></div></a>';

    else
        html += '<div class="col-xs-6"><small style="font-weight:550 !important">' + level3.Name + '</small></div>';

    html +=
        '<div class="col-xs-6 no-gutters">' +
        '<div class="col-xs-10">' +
        '	<input type="text" class="col-xs-12 input-sm" data-tarefa data-texto/>' +
        '</div>' +
        '<div class="col-xs-2">' + btnNA + '</div>' +
        // btnInfo +
        '</div>' +
        '<div class="clearfix"></div>';

    return html;
}

function getResultado(level3) {


    var btnNA = "";

    if (level3.ParLevel3Value.IsAtiveNA == true) {
        btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na>N/A</button>';
    }

    var unidadeMedida = '';
    var htmlInputLevel3Resultado = "";

    if (level3.ParLevel3Value.ParMeasurementUnit_Id != null) {
        unidadeMedida = $.map(parametrization.listaParMeasurementUnit, function (val, i) {

            if (val.Id == level3.ParLevel3Value.ParMeasurementUnit_Id) {
                return val.Name;
            }
        });

        htmlInputLevel3Resultado = '<input type="text" class="col-xs-8 input input-sm" data-tarefa data-valor data-equacao="' + level3.ParLevel3Value.DynamicValue + '" style=" text-align: center;" readonly/>' +
            '<input type="text" class="col-xs-2 input input-sm" value="' + unidadeMedida + '"  style=" text-align: center;" disabled />';
    } else {
        htmlInputLevel3Resultado = '<input type="text" class="col-xs-10 input input-sm" data-tarefa data-valor data-equacao="' + level3.ParLevel3Value.DynamicValue + '" style=" text-align: center;" readonly/>'
    }

    var html = '';

    var level3LimitLabel = !!level3.ParLevel3Value.ShowLevel3Limits ? ' MIN: ' + level3.ParLevel3Value.IntervalMin + ' | MAX: ' + level3.ParLevel3Value.IntervalMax : '';

    if (level3.ParLevel3XHelp)
        html += '<a style="cursor: pointer;" l3id="' + level3.Id + '" data-info><div class="col-xs-6"><small style="font-weight:550 !important">' + level3.Name + ' ' + level3LimitLabel + ' (Clique aqui)</small></div></a>';
    else
        html += '<div class="col-xs-6"><small style="font-weight:550 !important">' + level3.Name + ' ' + level3LimitLabel + '</small></div>';

    html +=
        '<div class="col-xs-6 no-gutters">' +
        '   <div class="col-xs-12 no-gutters">' +
        '       <div class="col-xs-12" style="padding: 0;">' +
                    htmlInputLevel3Resultado +
        '       </div>' +
        '   </div>' +
        '   <div class="col-xs-2">' + btnNA + '</div>' +
        // btnInfo +
        '</div>' +
        '<div class="clearfix"></div>';

    return html;
}

function getTexto(level3) {

    var btnNA = "";

    if (level3.ParLevel3Value.IsAtiveNA == true) {
        btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na>N/A</button>';
    }

    var html = '';

    if (level3.ParLevel3XHelp)
        html += '<a style="cursor: pointer;" l3id="' + level3.Id + '" data-info><div class="col-xs-6"><small style="font-weight:550 !important">' + level3.Name + ' (Clique aqui)</small></div></a>';

    else
        html += '<div class="col-xs-6"><small style="font-weight:550 !important">' + level3.Name + '</small></div>';

    var mensagemPadrao = level3.ParLevel3Value.DefaultMessageText !== null ? level3.ParLevel3Value.DefaultMessageText : "";
    var tamanhoPermitido = level3.ParLevel3Value.StringSizeAllowed !== null ? level3.ParLevel3Value.StringSizeAllowed : 100;

    html +=
        '<div class="col-xs-6 no-gutters">' +
        '<div class="col-xs-10">' +
        '<input type="text" class="col-xs-12 input-sm" style="text-align: center;" maxlength="' + tamanhoPermitido + '" placeholder="' + mensagemPadrao + '" data-tarefa data-valor/>' +
        '</div>' +
        '<div class="col-xs-2">' + btnNA + '</div>' +
        // btnInfo +
        '</div>' +
        '<div class="clearfix"></div>';

    return html;
}

function getNumerodeDefeitos(level3) {

    var btnNA = "";

    if (level3.ParLevel3Value.IsAtiveNA == true) {
        btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na>N/A</button>';
    }

    var html = '';

    if (level3.ParLevel3XHelp)
        html += '<a style="cursor: pointer;" l3id="' + level3.Id + '" data-info><div class="col-xs-6"><small style="font-weight:550 !important">' + level3.Name + ' (Clique aqui)</small></div></a>';

    else
        html += '<div class="col-xs-6"><small style="font-weight:550 !important">' + level3.Name + '</small></div>';

    html +=
        '<div class="col-xs-6 no-gutters">' +
        '<div class="col-xs-10">' +
        '	<input type="number" class="col-xs-12 input-sm" data-tarefa data-valor/>' +
        '</div>' +
        '<div class="col-xs-2">' + btnNA + '</div>' +
        // btnInfo +
        '</div>' +
        '<div class="clearfix"></div>';

    return html;
}

function getNumerodeDefeitosComTexto(level3) {

    var btnNA = "";

    if (level3.ParLevel3Value.IsAtiveNA == true) {
        btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na>N/A</button>';
    }

    var html = '';

    if (level3.ParLevel3XHelp)
        html += '<a style="cursor: pointer;" l3id="' + level3.Id + '" data-info><div class="col-xs-6"><small style="font-weight:550 !important">' + level3.Name + ' (Clique aqui)</small></div></a>';

    else
        html += '<div class="col-xs-6"><small style="font-weight:550 !important">' + level3.Name + '</small></div>';
    if (level3.ParLevel3Value.IsRequiredInt) {
        botao = '<input type="number" class="col-xs-12 input-sm" data-tarefa data-valor data-required-answer="1">';
    } else {
        botao = '<input type="number" class="col-xs-12 input-sm" data-tarefa data-valor data-required-answer="0">';
    }
    var mensagemPadrao = level3.ParLevel3Value.DefaultMessageText !== null ? level3.ParLevel3Value.DefaultMessageText : "";
    var tamanhoPermitido = level3.ParLevel3Value.StringSizeAllowed !== null ? level3.ParLevel3Value.StringSizeAllowed : 100;
    var input = '<input type="text" class="col-xs-12 input-sm" style="text-align: center;" maxlength="' + tamanhoPermitido + '" placeholder="' + mensagemPadrao + '" data-required-text="' + level3.ParLevel3Value.IsNCTextRequired + '" data-texto/>';

    html +=
        '<div class="col-xs-6 no-gutters">' +
        '<div class="col-xs-2">' +
        botao +
        '</div>' +
        '<div class="col-xs-8">' +
        input +
        '</div>' +
        '<div class="col-xs-2">' + btnNA + '</div>' +
        '</div>' +
        '<div class="clearfix"></div>';

    return html;
}

function getLikert(level3) {

    var btnNA = "";

    if (level3.ParLevel3Value.IsAtiveNA == true) {
        btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na>N/A</button>';
    }

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
        '	    <input type="text" class="col-xs-12 input-sm" data-tarefa data-valor/>' +
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
        setFieldColorGray($(this));
    } else if (linha.attr('data-conforme') == linha.attr('data-default-answer')) {
        linha.attr('data-conforme', linha.attr('data-default-answer') == "0" ? "1" : "0");
        setFieldColorGray($(this));
    } else {
        linha.addClass('alert-secundary');
        if ($(this).attr('data-required-answer') == "1") {
            linha.attr('data-conforme', "");
            setFieldColorWhite($(this));

        } else {
            linha.attr('data-conforme', linha.attr('data-default-answer'));
            setFieldColorWhite($(this));
        }
    }

    validateShowQualification(linha);

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

$('body').off('click', '[data-duplicate-click-remove]').on('click', '[data-duplicate-click-remove]', function () {
    $(this).parents()[0].remove();
});

$('body').off('click', '[data-duplicate-click-add]').on('click', '[data-duplicate-click-add]', function () {
    var cabecalhos = "";

    var parHeaderField_Id = $(this).attr('data-duplicate-click-add');

    var parLevel1Cabecalhos = $(this).parents('#headerFieldLevel1');

    if (parLevel1Cabecalhos.length > 0) {
        cabecalhos = montarHeaderFieldsPorId(1, parHeaderField_Id, "");
        parLevel1Cabecalhos.append(cabecalhos);
        return;
    }

    var parLevel2Cabecalhos = $(this).parents('#headerFieldLevel2');

    if (parLevel2Cabecalhos.length > 0) {
        cabecalhos = montarHeaderFieldsPorId(2, parHeaderField_Id, "");
        parLevel2Cabecalhos.append(cabecalhos);
        return;
    }

    var parLevel3Cabecalhos = $(this).parents('#headerFieldLevel3');

    if (parLevel3Cabecalhos.length > 0) {
        cabecalhos = montarHeaderFieldsPorId(4, parHeaderField_Id, "pull-right");
        parLevel3Cabecalhos.append(cabecalhos);
        return;
    }

    var parSecaoCabecalhos = $(this).parents('#headerFieldDepartment');

    if (parSecaoCabecalhos.length > 0) {
        cabecalhos = montarHeaderFieldsPorId(3, parHeaderField_Id, "");
        parSecaoCabecalhos.append(cabecalhos);
        return;
    }
});

function montarHeaderFieldsPorId(parLevelHeaderField_Id, parHeaderField_Id, flagPullRight) {
    var html = "";

    var headerFields = $.grep(parametrization.listaParHeaderFieldGeral, function (headerFieldGeral) {

        return headerFieldGeral.ParLevelHeaderField_Id == parLevelHeaderField_Id
            && headerFieldGeral.Id == parHeaderField_Id;

    });

    if (headerFields && headerFields.length)
        headerFields.forEach(function (headerField) {
            html += getInputOrSelect(headerField, flagPullRight);
        });

    return html;

}

function criaLinhaParQualification(level1Id, level2Id, level3Id, linhaLevel3) {

    var retorno = '';

    var listaParQualificationxParLevel3Value = validaParqualification(level1Id, level2Id, level3Id);

    if (listaParQualificationxParLevel3Value.length > 0) {

        listaParQualificationxParLevel3Value.forEach(function (o, i) {

            var qualificationGroupName = '';

            if (parametrization.listaPargroupQualification[i] != undefined) {
                qualificationGroupName = parametrization.listaPargroupQualification[i].Name;
            } else {
                qualificationGroupName = parametrization.listaPargroupQualification[0].Name;
            }


            if ($(linhaLevel3).attr('data-conforme') == o.Value) {
                var options = '';

                parametrization.listaParQualification.forEach(function (obj, i) {
                    options += '<option value="' + obj.Id + '" data-qualification>' + obj.Name + '</option >';
                });

                retorno += ' <div class="col-xs-3 no-gutters pull-right" data-ParQualificationLevel3Value="' + o.Value + '" data-qualification-required="' + o.IsRequired + '">';
                retorno += ' <div class="col-xs-12"><small style="font-weight:550 !important">' + qualificationGroupName + '</small></div>';
                retorno += ' <div class="col-xs-12">';
                retorno += ' <select class="form-control input-sm ddl" data-qualificationSelect>';
                retorno += ' <option value="">Selecione...</option>';
                retorno += options;
                retorno += ' </select>';
                retorno += ' </div>';
                retorno += ' </div>';

            }
        });

        retorno += ' <div class="clearfix"></div>';
    } else
        return '';

    return retorno;
}


function validateShowQualification(linhaLevel3) {

    $(linhaLevel3).siblings('[data-qualificationLevel3Value]').each(function (i, o) {

        if ($(linhaLevel3).attr('data-level1') == $(o).attr('parlevel1Id')
            && $(linhaLevel3).attr('data-level2') == $(o).attr('parlevel2Id')
            && $(linhaLevel3).attr('data-level3') == $(o).attr('parlevel3Id')) {

            var selectsQualificationHtml = criaLinhaParQualification($(o).attr('parlevel1Id'), $(o).attr('parlevel2Id'), $(o).attr('parlevel3Id'), linhaLevel3);

            $(o).html('');
            if (selectsQualificationHtml != "") {
                $(o).append(selectsQualificationHtml);
                $(o).removeClass('hidden');
            } else {
                $(o).AddClass('hidden');
            }
        }
    });
}

function setFieldColorGray(campo) {
    $(campo).css('background-color', '#E8E8E8');
}

function setFieldColorWhite(campo) {
    $(campo).css('background-color', '#FFFFFF');
}

$('body').off('change', 'input[data-valor]').on('change', 'input[data-valor]', function (e) {
    var linha = $(this).parents('[data-conforme]');

    if ((parseFloat($(this).val()) >= parseFloat($(linha).attr('data-min'))
        && parseFloat($(this).val()) <= parseFloat($(linha).attr('data-max')))
        || ($(linha).attr('data-min') == "null" && parseFloat($(this).val()) <= parseFloat($(linha).attr('data-max')))
        || ($(this).val() == "")) {
        resetarLinha(linha);
        linha.attr('data-conforme', '1');
    } else {
        resetarLinha(linha);
        linha.addClass('alert-secundary');
        linha.attr('data-conforme', '0');
    }
    validateShowQualification(linha);
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
            Sample: currentEvaluationSample.Sample,
            ParCluster_Id: currentParCluster_Id
        };
    }

    if (currentRedistributeWeight == true) {
        RedistributeWeight();
    }

    //Insere valores da coleta
    $($('form[data-form-coleta] div[data-linha-coleta]')).each(function (i, o) {
        var data = $(o);
        var isNA = $(data).attr('data-conforme-na') == "";
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
                IsConform: isNA ? 1 : $(data).attr('data-conforme') == "1",
                Value: typeof ($(data).find('input[data-valor]').val()) == 'undefined' ? null : $(data).find('input[data-valor]').val(),
                ValueText: typeof ($(data).find('input[data-texto]').val()) == 'undefined' ? null : $(data).find('input[data-texto]').val(),
                IsNotEvaluate: isNA,
                CollectionDate: getCurrentDate(),
                UserSgq_Id: currentLogin.Id,
                Weigth: $(data).attr('data-peso-parametrizado'),
                WeiEvaluation: isNA ? 0 : $(data).attr('data-peso'),
                Defects: isNA ? 0 : $(data).attr('data-conforme') == "1" ? 0 : 1,
                WeiDefects: isNA ? 0 : ($(data).attr('data-conforme') == "1" ? 0 : 1) * parseFloat($(data).attr('data-peso')),
                Parfrequency_Id: parametrization.currentParFrequency_Id,
                ParCluster_Id: currentParCluster_Id,
                Outros: JSON.stringify({ Qualification_Value: getQualificationCollection($(data).attr('data-level1'), $(data).attr('data-level2'), $(data).attr('data-level3')) })
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
            coletaJson.unshift(cabecalho);
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

function getQualificationCollection(ParLevel1_Id, ParLevel2_Id, ParLevel3_Id) {

    var collectionQualification = [];

    $('[data-qualificationlevel3value] select').each(function () {

        $self = $(this);

        var level1Id = $self.parents('[data-level3]').attr('parlevel1id');
        var level2Id = $self.parents('[data-level3]').attr('parlevel2id');
        var level3Id = $self.parents('[data-level3]').attr('parlevel3id');

        if (level1Id == ParLevel1_Id && level2Id == ParLevel2_Id && level3Id == ParLevel3_Id) {
            //validar se o tem é referente aquela tarefa, para salvar
            collectionQualification.push($self.val());
        } else {
            return "";
        }
    });

    return collectionQualification;
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
                ParCluster_Id: currentParCluster_Id,
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
                ParCluster_Id: currentParCluster_Id,
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
                ParCluster_Id: currentParCluster_Id,
                ParCompany_Id: currentParCompany_Id,
                CollectionDate: getCurrentDate(),
                UserSgq_Id: currentLogin.Id,
                ParLevel1_Id: $self.parents('#headerFieldLevel2').attr('parLevel1Id'),
                ParLevel2_Id: $self.parents('#headerFieldLevel2').attr('parLevel2Id'),
                Parfrequency_Id: parametrization.currentParFrequency_Id
            });

    });


    $('#headerFieldLevel3 input, #headerFieldLevel3 select').each(function () {

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
                ParCluster_Id: currentParCluster_Id,
                ParCompany_Id: currentParCompany_Id,
                CollectionDate: getCurrentDate(),
                UserSgq_Id: currentLogin.Id,
                ParLevel1_Id: $self.parents('#headerFieldLevel3').attr('parLevel1Id'),
                ParLevel2_Id: $self.parents('#headerFieldLevel3').attr('parLevel2Id'),
                ParLevel3_Id: $self.parents('#headerFieldLevel3').attr('parLevel3Id'),
                Parfrequency_Id: parametrization.currentParFrequency_Id
            });

    });

    return collectionHeaderFied;
}

function ColetasIsValid() {
    var linhasDaColeta = $('form[data-form-coleta] div[data-linha-coleta]');
    var inputsDaColeta = $('form[data-form-coleta] div[data-linha-coleta] input[data-texto]');
    var qualification = $('form[data-form-coleta] div[data-qualificationlevel3value] div[data-qualification-required]');
    var selectQualificationColeta = $('form[data-form-coleta] div[data-qualificationlevel3value] select[data-qualificationselect]');

    var errorCount = 0;
    var inputVal;
    var data;
    var linhaQualification;
    var selectQualification;

    for (var i = 0; i < qualification.length; i++) {
        linhaQualification = qualification[i];
        selectQualification = selectQualificationColeta[i];

        if ($(linhaQualification).attr('data-qualification-required') == 'true' && $(selectQualification).length > 0) {
            if ($(selectQualification).val() == null || $(selectQualification).val() == undefined || $(selectQualification).val() == "") {
                $(selectQualification).css("background-color", "#ffc1c1");
                errorCount++;
            } else {
                $(selectQualification).css("background-color", "white");
            }
        }
    }

    for (var i = 0; i < linhasDaColeta.length; i++) {
        data = linhasDaColeta[i];
        inputVal = inputsDaColeta[i];

        if ($(inputVal).attr('data-required-text') == 'true') {

            if ($(data).attr('data-conforme') == "0") {
                if ($(inputVal).val() == null || $(inputVal).val() == undefined || $(inputVal).val() == "") {
                    $(inputVal).css("background-color", "#ffc1c1");
                    errorCount++;
                } else {
                    $(inputVal).css("background-color", "white");
                }
            } else {
                $(inputVal).css("background-color", "white");
            }

        }

        if ($(data).attr('data-conforme-na') != "") {
            if ($(data).attr('data-conforme') == ""
                || $(data).attr('data-conforme') == null
                || $(data).attr('data-conforme') == "undefined") {

                $(data).find("[data-tarefa]").css("background-color", "#ffc1c1");
                errorCount++;
            } else {
                $(data).find("[data-tarefa]").css("background-color", "white");
            }
        }
    }
    if (errorCount > 0) {
        openMensagem("Atenção! Obrigatório responder todas as Tarefas.", "yellow", "black");
        mostraPerguntasObrigatorias(data);
        closeMensagem(2000);
        return false;
    } else
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

    $('#headerFieldDepartment input, #headerFieldDepartment select, '+ 
    '#headerFieldLevel1 input, #headerFieldLevel1 select, ' +
    '#headerFieldLevel2 input, #headerFieldLevel2 select, ' + 
    '#headerFieldLevel3 input, #headerFieldLevel3 select').each(function () {

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
        openMensagem("Atenção! Campos de cabeçalho obrigatórios não preenchidos", "yellow", "black");
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


//$('body').off('click', '.panel-body button, .panel-body input, .panel-body select')
//         .on('click', '.panel-body button, .panel-body input, .panel-body select', function (e) {
//    interacaoComFormulario++;
//});