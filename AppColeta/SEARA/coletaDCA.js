var coletaJson = [];
var currentEvaluationDCA = {};
var coletasDCA = [];
var objCabecalhoLevel1 = [];

function openColetaDCA(levels) {

    coletaJson = [];
    var html = '';
    var coleta = '';

    levels.forEach(function (level1) {

        var hasLevel2 = false;

        level1.ParLevel2.forEach(function (level2) {

            var hasLevel3 = false;
            var striped = true;

            level2.ParLevel3.sort((a, b) => a.Name.localeCompare(b.Name));
            level2.ParLevel3.forEach(function (level3) {

                var inputLevel3 = getInputLevel3DCA(level3, level2, level1, striped);

                if (inputLevel3.length > 0) {

                    if (hasLevel3 == false) {

                        if (hasLevel2 == false) {
                            coleta += getLevel1DCA(level1);
                            coleta += getParHeaderFieldLevel1(level1);
                            hasLevel2 = true;
                        }

                        coleta += getLevel2DCA(level2, level1);
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
        '<div class="container-fluid">                                                                                                             ' +
        '	<div class="">                                                                                                                         ' +
        '		<div class="col-xs-12">                                                                                                            ' +
        '			<div class="panel panel-primary">                                                                                              ' +
        '			  <div class="panel-heading">                                                                                                  ' +
        '				<h3 class="panel-title"><a onclick="listarParLevel2DCA();" class="btn btn-warning">Voltar</a> Questionario de Coleta DCA</h3>                                   ' +
        '			  </div>                                                                                                                       ' +
        '			  <div class="panel-body">                                                                                                     ' +
        //getContador() +
        getParHeaderFieldDeparment() +
        '				<form data-form-coleta style="text-align:justify">                                                                                                    ' +
        coleta +
        '					<button class="btn btn-success pull-right" data-salvar-tarefas style="margin:10px 25px">Salvar todas as tarefas</button>       ' +
        '					<button class="btn btn-block btn-primary input-lg col-xs-12" data-salvar-dca style="margin-top:10px" disabled>Salvar</button>       ' +
        '				</form>                                                                                                                    ' +
        '			  </div>                                                                                                                       ' +
        '       </div>                                                                                                                             ' +
        '    </div>                                                                                                                                ' +
        '	</div>                                                                                                                                 ' +
        '</div>';

    $('div#app').html(html);

    if ($("[data-linha-coleta][data-synced='false'][data-amostra-completa='1']").length == $("[data-linha-coleta]").length) {
        habilitaBotaoSalvar();
    }

    atualizaCorSePassarDoLimiteDeNC();
    atualizaPorcentagemDeTarefas();
    setBreadcrumbsDCA();
}

function getLevel1DCA(level1) {
    return '<div class="col-xs-12" style="padding-top:5px;padding-bottom:5px;background-color:#edf5fc;"><small>' + level1.Name + '</small></div>';
}

function getLevel2DCA(level2, level1) {
    var html = '<div class="col-xs-12" style="padding-left:18px;padding-top:5px;padding-bottom:5px;background-color:#fcf4e3;">';
    html += '<div class="row">';
    html += '<div class="col-sm-4"><small>' + level2.Name + '</small></div>';
    html += '<div class="col-sm-4">Conforme <span class="porcentagemTarefa">0</span>%</div>';
    html += '<div class="col-sm-4">% Monitoramento <span class="porcentagemMonitoramento">0</span>% / <span class="porcentagemTotalMonitoramento">' + currentParLevel2DCATotalPorcentagem + '</span>%</div>';
    html += '</div></div>';
    return html;
}

function getLevel3DCA(level3, level2, level1) {
    return '<div class="col-xs-12" style="margin-bottom:10px;margin-top:10px">' + level3.Name + '</div>';
}

function getInputLevel3DCA(level3, level2, level1, striped) {

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

        var amostraNC = getQuantidadeNC(level1, level2, level3);
        var amostraAtual = getAmostraAtual(level1, level2, level3);
        var amostraTotal = getAmostraTotal(level1, level2, level3);
        var synced = getSynced(level1, level2, level3);

        if (amostraTotal == 0)
            return [];

        var amostraCompleta = amostraAtual > amostraTotal ? 1 : 0;

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
        retorno += ' data-qtdeNc="' + amostraNC + '"';
        retorno += ' data-limiteNC="' + level3.ParLevel3Value.LimiteNC + '"';
        retorno += ' data-sample="' + amostraAtual + '"';
        retorno += ' data-sampleMax="' + amostraTotal + '"';
        retorno += ' data-amostra-completa="' + amostraCompleta + '"';
        retorno += ' data-parlevel3inputtype="' + level3.ParLevel3InputType.Id + '"';
        retorno += ' data-synced="' + synced + '"';
        retorno += ' style="padding-left:10px;' + colorStriped + '">';

        switch (level3.ParLevel3InputType.Id) {

            case 1: //Binário
                retorno += getBinarioDCA(level3, amostraAtual, amostraTotal, amostraNC);
                break;
            case 2: //Numerodedefeitos
                retorno += getNumerodeDefeitosDCA(level3, amostraAtual, amostraTotal, amostraNC);
                break;
            case 15: //NumerodedefeitosComTexto
                retorno += getNumerodeDefeitosComTextoDCA(level3, amostraAtual, amostraTotal, amostraNC);
                break;
            case 6: //BinárioComTexto
                retorno += getBinarioComTextoDCA(level3, amostraAtual, amostraTotal, amostraNC);
                break;
            case 3: //Intervalo
                retorno += getIntervaloDCA(level3, amostraAtual, amostraTotal, amostraNC);
                break;
            case 7: //IntervaloemMinutos
                retorno += getIntervaloemMinutosDCA(level3, amostraAtual, amostraTotal, amostraNC);
                break;
            case 9: //IntervaloComObservacao
                retorno += getIntervaloComObservacaoDCA(level3, amostraAtual, amostraTotal, amostraNC);
                break;
            case 11: //Observacao
                retorno += getObservacaoDCA(level3, amostraAtual, amostraTotal, amostraNC);
                break;
            case 8: //Likert
                retorno += getLikertDCA(level3, amostraAtual, amostraTotal, amostraNC);
                break;
            case 5: //Texto
                retorno += getTextoDCA(level3, amostraAtual, amostraTotal, amostraNC);
                break;
            default:
                retorno += "";
                return '';
        }

        retorno += '</div>';

    }

    return retorno;

}

function getBinarioDCA(level3, amostraAtual, amostraTotal, amostraNC) {

    var disabled = amostraAtual > amostraTotal ? 'disabled' : '';
    var btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na-dca ' + disabled + '>N/A</button>';
    var html = '';
    var botao = '';
    var btnColeta = '<button type="button" class="btn btn-success pull-right btn-sm btn-block" data-coleta-dca ' + disabled + '>Salvar</button>';
    //var amostraAtual = 0;
    //var amostraTotal = 10;
    //var amostraNC = 0;
    var respostaPadrao = "";

    html += '<a style="cursor: pointer;" l3id="' + level3.Id + '" data-info-limitenc><div class="col-xs-4"><small style="font-weight:550 !important">' + level3.Name + '</small></div></a>';

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

    var htmlAmostra = '<div class="col-xs-2">Amostras: <spam class="amostra">' + (amostraAtual > amostraTotal ? amostraTotal : amostraAtual) + '</spam>/' + amostraTotal + '</div>';
    var htmlMaxMin = '<div class="col-xs-1">' + 0 + '</div>';
    var htmlAmostraNC = '<div class="col-xs-2 amostras-nc">Amostras NC: <spam class="amostraNC">' + amostraNC + '</spam></div>';
    //var htmlEsconder = '<div class="col-xs-1></div>';

    html +=
        htmlAmostra +
        //htmlMaxMin +
        htmlAmostraNC +
        //htmlEsconder +
        '<div class="col-xs-4 no-gutters">' +
        '   <div class="col-xs-8">' +
        botao +
        '   </div>' +
        '   <div class="col-xs-2" >' + btnNA + '</div>' +
        '   <div class="col-xs-2" >' + btnColeta + '</div>' +
        '</div>' +
        '<div class="clearfix"></div>';

    return html;
}

function getBinarioComTextoDCA(level3, amostraAtual, amostraTotal, amostraNC) {

    var disabled = amostraAtual > amostraTotal ? 'disabled' : '';
    var btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na-dca ' + disabled + '>N/A</button>';
    var btnColeta = '<button type="button" class="btn btn-success pull-right btn-sm btn-block" data-coleta-dca ' + disabled + '>Salvar</button>';
    //var amostraAtual = 0;
    //var amostraTotal = 10;
    //var amostraNC = 0;

    var html = '';

    var respostaPadrao = "";

    html += '<a style="cursor: pointer;" l3id="' + level3.Id + '" data-info-limitenc><div class="col-xs-4"><small style="font-weight:550 !important">' + level3.Name + '</small></div></a>';

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
    //html +=
    //    '<div class="col-xs-6 no-gutters">' +
    //    '   <div class="col-xs-10">' +
    //    botao +
    //    '   </div>' +
    //    '   <div class="col-xs-2">' + btnNA + '</div>' +
    //    '</div>' +
    //    '<div class="clearfix"></div>';

    var htmlAmostra = '<div class="col-xs-2">Amostras: <spam class="amostra">' + (amostraAtual > amostraTotal ? amostraTotal : amostraAtual) + '</spam>/' + amostraTotal + '</div>';
    var htmlMaxMin = '<div class="col-xs-1">' + 0 + '</div>';
    var htmlAmostraNC = '<div class="col-xs-2 amostras-nc">Amostras NC: <spam class="amostraNC">' + amostraNC + '</spam></div>';
    //var htmlEsconder = '<div class="col-xs-1></div>';

    html +=
        htmlAmostra +
        //htmlMaxMin +
        htmlAmostraNC +
        '<div class="col-xs-4 no-gutters">' +
        '   <div class="col-xs-4">' +
        '	    <input type="text" class="col-xs-12 input-sm" data-texto/>' +
        '   </div>' +
        '   <div class="col-xs-4">' +
        botao +
        '   </div>' +
        '   <div class="col-xs-2" >' + btnNA + '</div>' +
        '   <div class="col-xs-2" >' + btnColeta + '</div>' +
        // btnInfo +
        '</div>' +
        '<div class="clearfix"></div>';

    return html;
}

function getIntervaloDCA(level3, amostraAtual, amostraTotal, amostraNC) {

    var disabled = amostraAtual > amostraTotal ? 'disabled' : '';
    var btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na-dca ' + disabled + '>N/A</button>';
    var btnColeta = '<button type="button" class="btn btn-success pull-right btn-sm btn-block" data-coleta-dca ' + disabled + '>Salvar</button>';
    //var amostraAtual = 0;
    //var amostraTotal = 10;
    //var amostraNC = 0;

    var html = '';

    html += '<a style="cursor: pointer;" l3id="' + level3.Id + '" data-info-limitenc><div class="col-xs-4"><small style="font-weight:550 !important">' + level3.Name + '</small></div></a>';

    var level3LimitLabel = !!level3.ParLevel3Value.ShowLevel3Limits ? ' MIN: ' + level3.ParLevel3Value.IntervalMin + ' | MAX: ' + level3.ParLevel3Value.IntervalMax : '';

    var htmlAmostra = '<div class="col-xs-2">Amostras: <spam class="amostra">' + (amostraAtual > amostraTotal ? amostraTotal : amostraAtual) + '</spam>/' + amostraTotal + '</div>';
    var htmlMaxMin = '<div class="col-xs-1">' + 0 + '</div>';
    var htmlAmostraNC = '<div class="col-xs-2 amostras-nc">Amostras NC: <spam class="amostraNC">' + amostraNC + '</spam></div>';
    //var htmlEsconder = '<div class="col-xs-1></div>';

    html +=
        htmlAmostra +
        //htmlMaxMin +
        htmlAmostraNC +
        '<div class="col-xs-4 no-gutters">' +
        '   <div class="col-xs-2 input-sm" style="font-size: 8px;">' +
        level3LimitLabel +
        '   </div>' +
        '   <div class="col-xs-6 no-gutters">' +
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
        '   <div class="col-xs-2" >' + btnNA + '</div>' +
        '   <div class="col-xs-2" >' + btnColeta + '</div>' +
        // btnInfo +
        '</div>' +
        '<div class="clearfix"></div>';

    return html;
}

function getIntervaloemMinutosDCA(level3, amostraAtual, amostraTotal, amostraNC) {

    var disabled = amostraAtual > amostraTotal ? 'disabled' : '';
    var btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na-dca ' + disabled + '>N/A</button>';
    var btnColeta = '<button type="button" class="btn btn-success pull-right btn-sm btn-block" data-coleta-dca ' + disabled + '>Salvar</button>';
    //var amostraAtual = 0;
    //var amostraTotal = 10;
    //var amostraNC = 0;

    var html = '';

    html += '<a style="cursor: pointer;" l3id="' + level3.Id + '" data-info-limitenc><div class="col-xs-4"><small style="font-weight:550 !important">' + level3.Name + '</small></div></a>';

    var level3LimitLabel = !!level3.ParLevel3Value.ShowLevel3Limits ? ' MIN: ' + level3.ParLevel3Value.IntervalMin + ' | MAX: ' + level3.ParLevel3Value.IntervalMax : '';

    var htmlAmostra = '<div class="col-xs-2">Amostras: <spam class="amostra">' + (amostraAtual > amostraTotal ? amostraTotal : amostraAtual) + '</spam>/' + amostraTotal + '</div>';
    var htmlMaxMin = '<div class="col-xs-1">' + 0 + '</div>';
    var htmlAmostraNC = '<div class="col-xs-2 amostras-nc">Amostras NC: <spam class="amostra">' + amostraNC + '</spam></div>';
    //var htmlEsconder = '<div class="col-xs-1></div>';

    html +=
        htmlAmostra +
        //htmlMaxMin +
        htmlAmostraNC +
        '<div class="col-xs-4 no-gutters">' +
        '<div class="col-xs-2 input-sm" style="font-size: 8px;">' +
        level3LimitLabel +
        '</div>' +
        '<div class="col-xs-3">' +
        '	<input type="text" class="col-xs-12 input-sm" data-texto/>' +
        '</div>' +
        '<div class="col-xs-3 no-gutters">' +
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
        '<div class="col-xs-2" >' + btnNA + '</div>' +
        '<div class="col-xs-2" >' + btnColeta + '</div>' +
        // btnInfo +
        '</div>' +
        '<div class="clearfix"></div>';

    return html;
}

function getIntervaloComObservacaoDCA(level3, amostraAtual, amostraTotal, amostraNC) {

    var disabled = amostraAtual > amostraTotal ? 'disabled' : '';
    var btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na-dca ' + disabled + '>N/A</button>';
    var btnColeta = '<button type="button" class="btn btn-success pull-right btn-sm btn-block" data-coleta-dca ' + disabled + '>Salvar</button>';
    //var amostraAtual = 0;
    //var amostraTotal = 10;
    //var amostraNC = 0;

    var html = '';

    html += '<a style="cursor: pointer;" l3id="' + level3.Id + '" data-info-limitenc><div class="col-xs-4"><small style="font-weight:550 !important">' + level3.Name + '</small></div></a>';

    var level3LimitLabel = !!level3.ParLevel3Value.ShowLevel3Limits ? ' MIN: ' + level3.ParLevel3Value.IntervalMin + ' | MAX: ' + level3.ParLevel3Value.IntervalMax : '';


    var htmlAmostra = '<div class="col-xs-2">Amostras: <spam class="amostra">' + (amostraAtual > amostraTotal ? amostraTotal : amostraAtual) + '<spam>/' + amostraTotal + '</div>';
    var htmlMaxMin = '<div class="col-xs-1">' + 0 + '</div>';
    var htmlAmostraNC = '<div class="col-xs-2 amostras-nc">Amostras NC: <spam class="amostraNC">' + amostraNC + '</spam></div>';
    //var htmlEsconder = '<div class="col-xs-1></div>';

    html +=
        htmlAmostra +
        //htmlMaxMin +
        htmlAmostraNC +
        '<div class="col-xs-4 no-gutters">' +
        '<div class="col-xs-2 input-sm" style="font-size: 8px;">' +
        level3LimitLabel +
        '</div>' +
        '<div class="col-xs-3">' +
        '	<input type="text" class="col-xs-12 input-sm" data-texto/>' +
        '</div>' +
        '<div class="col-xs-3 no-gutters">' +
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
        '<div class="col-xs-2" >' + btnNA + '</div>' +
        '<div class="col-xs-2" >' + btnColeta + '</div>' +
        // btnInfo +
        '</div>' +
        '<div class="clearfix"></div>';

    return html;
}

function getObservacaoDCA(level3, amostraAtual, amostraTotal, amostraNC) {

    var disabled = amostraAtual > amostraTotal ? 'disabled' : '';
    var btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na-dca ' + disabled + '>N/A</button>';
    var btnColeta = '<button type="button" class="btn btn-success pull-right btn-sm btn-block" data-coleta-dca ' + disabled + '>Salvar</button>';
    //var amostraAtual = 0;
    //var amostraTotal = 10;
    //var amostraNC = 0;

    var html = '';

    html += '<a style="cursor: pointer;" l3id="' + level3.Id + '" data-info-limitenc><div class="col-xs-4"><small style="font-weight:550 !important">' + level3.Name + '</small></div></a>';

    var htmlAmostra = '<div class="col-xs-2">Amostras: <spam class="amostra">' + (amostraAtual > amostraTotal ? amostraTotal : amostraAtual) + '</spam>/' + amostraTotal + '</div>';
    var htmlMaxMin = '<div class="col-xs-1">' + 0 + '</div>';
    var htmlAmostraNC = '<div class="col-xs-2 amostras-nc">Amostras NC: <spam class="amostraNC">' + amostraNC + '</spam></div>';
    //var htmlEsconder = '<div class="col-xs-1></div>';

    html +=
        htmlAmostra +
        //htmlMaxMin +
        htmlAmostraNC +
        '<div class="col-xs-4 no-gutters">' +
        '<div class="col-xs-8">' +
        '	<input type="text" class="col-xs-12 input-sm" data-texto/>' +
        '</div>' +
        '<div class="col-xs-2" >' + btnNA + '</div>' +
        '<div class="col-xs-2" >' + btnColeta + '</div>' +
        // btnInfo +
        '</div>' +
        '<div class="clearfix"></div>';

    return html;
}

function getTextoDCA(level3, amostraAtual, amostraTotal, amostraNC) {

    var disabled = amostraAtual > amostraTotal ? 'disabled' : '';
    var btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na-dca ' + disabled + '>N/A</button>';
    var btnColeta = '<button type="button" class="btn btn-success pull-right btn-sm btn-block" data-coleta-dca ' + disabled + '>Salvar</button>';
    //var amostraAtual = 0;
    //var amostraTotal = 10;
    //var amostraNC = 0;

    var html = '';

    html += '<a style="cursor: pointer;" l3id="' + level3.Id + '" data-info-limitenc><div class="col-xs-4"><small style="font-weight:550 !important">' + level3.Name + '</small></div></a>';

    var htmlAmostra = '<div class="col-xs-2">Amostras: <spam class="amostra">' + (amostraAtual > amostraTotal ? amostraTotal : amostraAtual) + '</spam>/' + amostraTotal + '</div>';
    var htmlMaxMin = '<div class="col-xs-1">' + 0 + '</div>';
    var htmlAmostraNC = '<div class="col-xs-2 amostras-nc">Amostras NC: <spam class="amostraNC">' + amostraNC + '</spam></div>';
    //var htmlEsconder = '<div class="col-xs-1></div>';

    html +=
        htmlAmostra +
        //htmlMaxMin +
        htmlAmostraNC +
        '<div class="col-xs-4 no-gutters">' +
        '<div class="col-xs-8">' +
        '	<input type="text" class="col-xs-12 input-sm" data-valor/>' +
        '</div>' +
        '<div class="col-xs-2" >' + btnNA + '</div>' +
        '<div class="col-xs-2" >' + btnColeta + '</div>' +
        // btnInfo +
        '</div>' +
        '<div class="clearfix"></div>';

    return html;
}

function getNumerodeDefeitosDCA(level3, amostraAtual, amostraTotal, amostraNC) {
    var disabled = amostraAtual > amostraTotal ? 'disabled' : '';
    var btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na-dca ' + disabled + '>N/A</button>';
    var btnColeta = '<button type="button" class="btn btn-success pull-right btn-sm btn-block" data-coleta-dca ' + disabled + '>Salvar</button>';
    //var amostraAtual = 0;
    //var amostraTotal = 10;
    //var amostraNC = 0;

    var html = '';

    html += '<a style="cursor: pointer;" l3id="' + level3.Id + '" data-info-limitenc><div class="col-xs-4"><small style="font-weight:550 !important">' + level3.Name + '</small></div></a>';

    var htmlAmostra = '<div class="col-xs-2">Amostras: <spam class="amostra hide">' + (amostraAtual > amostraTotal ? amostraTotal : amostraAtual) + '</spam>' + amostraTotal + '</div>';
    var htmlMaxMin = '<div class="col-xs-1">' + 0 + '</div>';
    var htmlAmostraNC = '<div class="col-xs-2 amostras-nc">Amostras NC: <spam class="amostraNC">' + amostraNC + '</spam></div>';
    //var htmlEsconder = '<div class="col-xs-1></div>';

    html +=
        htmlAmostra +
        //htmlMaxMin +
        htmlAmostraNC +
        '<div class="col-xs-4 no-gutters">' +
        '<div class="col-xs-8">' +
        '	<input type="number" class="col-xs-12 input-sm" data-valor/>' +
        '</div>' +
        '<div class="col-xs-2" >' + btnNA + '</div>' +
        '<div class="col-xs-2" >' + btnColeta + '</div>' +
        // btnInfo +
        '</div>' +
        '<div class="clearfix"></div>';

    return html;
}

function getNumerodeDefeitosComTextoDCA(level3, amostraAtual, amostraTotal, amostraNC) {
    var disabled = amostraAtual > amostraTotal ? 'disabled' : '';
    var btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na-dca ' + disabled + '>N/A</button>';
    var btnColeta = '<button type="button" class="btn btn-success pull-right btn-sm btn-block" data-coleta-dca ' + disabled + '>Salvar</button>';

    var html = '';

    html += '<a style="cursor: pointer;" l3id="' + level3.Id + '" data-info-limitenc><div class="col-xs-4"><small style="font-weight:550 !important">' + level3.Name + '</small></div></a>';

    var htmlAmostra = '<div class="col-xs-2">Amostras: <spam class="amostra hide">' + (amostraAtual > amostraTotal ? amostraTotal : amostraAtual) + '</spam>' + amostraTotal + '</div>';
    var htmlAmostraNC = '<div class="col-xs-2 amostras-nc">Amostras NC: <spam class="amostraNC">' + amostraNC + '</spam></div>';

    html +=
        htmlAmostra +
        htmlAmostraNC +
        '<div class="col-xs-4 no-gutters">' +
        '<div class="col-xs-4" style="padding:0 2px 0 2px !important">' +
        '	<input type="number" class="col-xs-12 input-sm" data-valor/>' +
        '</div>' +
        '<div class="col-xs-4" style="padding:0 2px 0 2px !important">' +
        '	<input type="text" class="col-xs-12 input-sm" data-texto placeholder="Observações"/>' +
        '</div>' +
        '<div class="col-xs-2" >' + btnNA + '</div>' +
        '<div class="col-xs-2" >' + btnColeta + '</div>' +
        '</div>' +
        '<div class="clearfix"></div>';

    return html;
}

function getLikertDCA(level3, amostraAtual, amostraTotal, amostraNC) {

    var disabled = amostraAtual > amostraTotal ? 'disabled' : '';
    var btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na-dca ' + disabled + '>N/A</button>';
    var btnColeta = '<button type="button" class="btn btn-success pull-right btn-sm btn-block" data-coleta-dca ' + disabled + '>Salvar</button>';
    //var amostraAtual = 0;
    //var amostraTotal = 10;
    //var amostraNC = 0;

    var html = '';

    html += '<a style="cursor: pointer;" l3id="' + level3.Id + '" data-info-limitenc><div class="col-xs-4"><small style="font-weight:550 !important">' + level3.Name + '</small></div></a>';

    var level3LimitLabel = !!level3.ParLevel3Value.ShowLevel3Limits ? ' MIN: ' + level3.ParLevel3Value.IntervalMin + ' | MAX: ' + level3.ParLevel3Value.IntervalMax : '';

    var htmlAmostra = '<div class="col-xs-2">Amostras: <spam class="amostra">' + amostraAtual + '</spam>/' + amostraTotal + '</div>';
    var htmlMaxMin = '<div class="col-xs-1">' + 0 + '</div>';
    var htmlAmostraNC = '<div class="col-xs-2 amostras-nc">Amostras NC: <spam class="amostraNC">' + amostraNC + '</spam></div>';
    //var htmlEsconder = '<div class="col-xs-1></div>';

    html +=
        htmlAmostra +
        //htmlMaxMin +
        htmlAmostraNC +
        '<div class="col-xs-4 no-gutters">' +
        '   <div class="col-xs-2 input-sm" style="font-size: 8px;">' +
        level3LimitLabel +
        '   </div>' +
        '   <div class="col-xs-6">' +
        '	    <input type="text" class="col-xs-12 input-sm" data-valor/>' +
        '   </div>' +
        '   <div class="col-xs-2" >' + btnNA + '</div>' +
        '   <div class="col-xs-2" >' + btnColeta + '</div>' +
        // btnInfo +
        '</div>' +
        '<div class="clearfix"></div>';

    return html;
}

function validaCampoEmBrancoNA() {
    if (linha.attr('data-default-answer') == "1") {
        linha.attr('data-conforme', " ");
    }
}

// $('body').off('click', '[data-collapse-targeter]').on('click', '[data-collapse-targeter]', function () {
//     if ($(this).attr('data-targeter-collapsed') == 'true') {
//         $('[data-collapse-target^="' + $(this).attr('data-collapse-targeter') + '-"]').removeClass('hide');
//         $('[data-collapse-target="' + $(this).attr('data-collapse-targeter') + '"]').removeClass('hide');
//         $(this).attr('data-targeter-collapsed', false);
//     } else {
//         $('[data-collapse-target^="' + $(this).attr('data-collapse-targeter') + '-"]').addClass('hide');
//         $('[data-collapse-target="' + $(this).attr('data-collapse-targeter') + '"]').addClass('hide');
//         $(this).attr('data-targeter-collapsed', true);
//     }
// });

// $('body').off('click', '[data-plus]').on('click', '[data-plus]', function (e) {
//     var value = parseInt($(this).parent().parent().find('input').val());
//     if (isNaN(value))
//         value = 1;
//     else
//         value += 1;
//     var input = $(this).parent().parent().find('input');
//     input.val(value);
//     input.trigger('change');

// });

// $('body').off('click', '[data-minus]').on('click', '[data-minus]', function (e) {
//     var value = parseInt($(this).parent().parent().find('input').val());
//     if (isNaN(value))
//         value = -1;
//     else
//         value -= 1;
//     var input = $(this).parent().parent().find('input');
//     input.val(value);
//     input.trigger('change');
// });

$('body').off('click', '[data-na-dca]').on('click', '[data-na-dca]', function (e) {

    var linha = $(this).parents('[data-linha-coleta]');

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

// $('body').off('click', '[data-binario]').on('click', '[data-binario]', function (e) {

//     var linha = $(this).parents('[data-conforme]');

//     resetarLinha(linha);

//     if (linha.attr('data-conforme') == "" || linha.attr('data-conforme') == null) {
//         linha.attr('data-conforme', linha.attr('data-default-answer'));

//     } else if (linha.attr('data-conforme') == linha.attr('data-default-answer')) {
//         linha.attr('data-conforme', linha.attr('data-default-answer') == "0" ? "1" : "0");
//     } else {
//         linha.addClass('alert-secundary');
//         if ($(this).attr('data-required-answer') == "1") {
//             linha.attr('data-conforme', "");
//         } else {
//             linha.attr('data-conforme', linha.attr('data-default-answer'));
//         }
//     }

//     if (linha.attr('data-conforme') == "1") {
//         $(this).text($(this).attr('data-positivo'));
//     } else if (linha.attr('data-conforme') == "0") {
//         $(this).text($(this).attr('data-negativo'));
//     } else {
//         $(this).text('');
//         $(this).html('&nbsp;');
//     }

//     $(this).addClass('btn-default');
//     $(this).removeClass('btn-secundary');

// });

// $('body').off('change', 'input[data-valor]').on('change', 'input[data-valor]', function (e) {
//     var linha = $(this).parents('[data-conforme]');

//     if (parseFloat($(this).val()) >= parseFloat($(linha).attr('data-min'))
//         && parseFloat($(this).val()) <= parseFloat($(linha).attr('data-max'))) {
//         resetarLinha(linha);
//         linha.attr('data-conforme', '1');
//     } else {
//         resetarLinha(linha);
//         linha.addClass('alert-secundary');
//         linha.attr('data-conforme', '0');
//     }
// });

$('body').off('click', '[data-info-limitenc]').on('click', '[data-info-limitenc]', function (e) {

    var linha = $(this);
    var title = $(linha).text();

    var btnClose = '<button class="btn btn-primary pull-right" onclick="closeModal()">Fechar</button>';
    var corpo =
        '<div class="container">' +
        '<div class="row" style="overflow:auto">' +
        '<div style="text-align:center">' +
        '<table class="table table-bordered" style="background-color:white">' +
        '<thead>' +
        '    <tr>' +
        '    <th class="text-center" colspan=3>' + title + '</th>' +
        '    </tr>' +
        '</thead>' +
        '<thead>' +
        '    <tr>' +
        '    <th class="col-sm-4 text-center">Padrão</th>' +
        '    <th class="col-sm-4 text-center">Aceitavel</th>' +
        '    <th class="col-sm-4 text-center">Não Aceitavel</th>' +
        '    </tr>' +
        '</thead>' +
        '<tbody>' +
        '    <tr>' +
        '    <td class="btn-success">0</td>' +
        '    <td class="btn-warning">Entre 0 e ' + UmSeForNaNOuNull($(linha).parent().attr('data-limiteNC')) + '</td>' +
        '    <td class="btn-danger">Maior que ' + UmSeForNaNOuNull($(linha).parent().attr('data-limiteNC')) + '</td>' +
        '    </tr>' +
        '</tbody>' +
        '</table>' +
        '</div>' +
        '<hr>' +
        btnClose +
        '</div>' +
        '</div>';

    openModal(corpo);

});


$('body')
    .off('keyup', '[data-linha-coleta][data-parlevel3inputtype="2"] input')
    .on('keyup', '[data-linha-coleta][data-parlevel3inputtype="2"] input', function () {
        var quantidadeDeDefeitos = $(this).val();
        var linhaTarefa = $(this).parents('[data-linha-coleta]');
        var maxSample = parseInt($(linhaTarefa).attr('data-samplemax'));

        $(this).removeClass('btn-danger');
        if (maxSample < quantidadeDeDefeitos || quantidadeDeDefeitos < 0) {
            $(this).addClass('btn-danger');
        }
    });

$('body').off('click', '[data-coleta-dca]').on('click', '[data-coleta-dca]', function () {

    if (!hederFieldIsValid("#headerFieldLevel2")) {
        openMensagem("Existem cabeçalhos obrigatórios não preenchidos!", "yellow", "black");
        setTimeout(closeMensagem, 3000);
        return false;
    }

    var $botaoColeta = $(this);
    var linhaTarefa = $(this).parents('[data-linha-coleta]');
    var currentEvaluation = currentEvaluationDCA.Evaluation;
    var currentSample = parseInt($(linhaTarefa).attr('data-sample'));
    var isNA = $(linhaTarefa).attr('data-conforme-na') == "";
    var qtdeNC = parseInt($(linhaTarefa).attr('data-qtdeNc'));
    var maxSample = parseInt($(linhaTarefa).attr('data-samplemax'));
    var $botaoNa = $(linhaTarefa).find('[data-na-dca]');

    var coletaDCA = {};
    var parParLevel3InputType_Id = linhaTarefa.attr('data-parlevel3inputtype');
    var numeroProximaAmostra = currentSample;

    if (parParLevel3InputType_Id == 2 || parParLevel3InputType_Id == 15) {

        var quantidadeDeDefeitos = linhaTarefa.find('input[type="number"]').val();

        if (maxSample < quantidadeDeDefeitos || quantidadeDeDefeitos < 0) {
            openMensagem("O valor digitado não está de acordo com o número de amostra.", "yellow", "black");
            setTimeout(closeMensagem, 3000);
            return false;
        }

        coletaDCA = {
            Id: 0,
            CollectionDate: getCurrentDate(),
            UserSgq_Id: currentLogin.Id,
            Shift_Id: 1,
            Period_Id: 1,
            ParCompany_Id: currentParCompany_Id,
            ParLevel1_Id: currentParLevel1_Id,
            ParLevel2_Id: currentParLevel2_Id,
            ParLevel3_Id: parseInt($(linhaTarefa).attr('data-level3')),
            Weigth: $(linhaTarefa).attr('data-peso'),
            IntervalMin: $(linhaTarefa).attr('data-min') == "null" ? null : $(linhaTarefa).attr('data-min'),
            IntervalMax: $(linhaTarefa).attr('data-max') == "null" ? null : $(linhaTarefa).attr('data-max'),
            Value: typeof ($(linhaTarefa).find('input[data-valor]').val()) == 'undefined' ? null : $(linhaTarefa).find('input[data-valor]').val(),
            ValueText: typeof ($(linhaTarefa).find('input[data-texto]').val()) == 'undefined' ? null : $(linhaTarefa).find('input[data-texto]').val(),
            IsConform: isNA || !(quantidadeDeDefeitos > 0) ? 1 : 0,
            IsNotEvaluate: isNA,
            Defects: isNA || !(quantidadeDeDefeitos > 0) ? 0 : quantidadeDeDefeitos,
            WeiEvaluation: isNA ? 0 : parseInt($(linhaTarefa).attr('data-peso')) * currentEvaluation,
            Evaluation: currentEvaluation,
            WeiDefects: isNA ? 0 : parseInt($(linhaTarefa).attr('data-peso')),
            Sample: maxSample,
            Outros: '{ParFamiliaProduto_Id:' + currentFamiliaProdutoDCA_Id + ', ParProduto_Id:' + currentProdutoDCA_Id + '}'
        };
        coletaDCA.WeiDefects = isNA ? 0 : coletaDCA.Defects * coletaDCA.WeiDefects;

        coletasDCA.push(coletaDCA);

        //Contar quantidade de NC
        qtdeNC += coletaDCA.WeiDefects;
        numeroProximaAmostra = maxSample + 1;

    } else {
        coletaDCA = {
            Id: 0,
            CollectionDate: getCurrentDate(),
            UserSgq_Id: currentLogin.Id,
            Shift_Id: 1,
            Period_Id: 1,
            ParCompany_Id: currentParCompany_Id,
            ParLevel1_Id: currentParLevel1_Id,
            ParLevel2_Id: currentParLevel2_Id,
            ParLevel3_Id: parseInt($(linhaTarefa).attr('data-level3')),
            Weigth: $(linhaTarefa).attr('data-peso'),
            IntervalMin: $(linhaTarefa).attr('data-min') == "null" ? null : $(linhaTarefa).attr('data-min'),
            IntervalMax: $(linhaTarefa).attr('data-max') == "null" ? null : $(linhaTarefa).attr('data-max'),
            Value: typeof ($(linhaTarefa).find('input[data-valor]').val()) == 'undefined' ? null : $(linhaTarefa).find('input[data-valor]').val(),
            ValueText: typeof ($(linhaTarefa).find('input[data-texto]').val()) == 'undefined' ? null : $(linhaTarefa).find('input[data-texto]').val(),
            IsConform: isNA ? 1 : $(linhaTarefa).attr('data-conforme') == "1",
            IsNotEvaluate: isNA,
            Defects: isNA ? 0 : $(linhaTarefa).attr('data-conforme') == "1" ? 0 : 1,
            WeiEvaluation: isNA ? 0 : parseInt($(linhaTarefa).attr('data-peso')) * currentEvaluation,
            Evaluation: currentEvaluation,
            WeiDefects: isNA ? 0 : ($(linhaTarefa).attr('data-conforme') == "1" ? 0 : 1) * parseInt($(linhaTarefa).attr('data-peso')),
            Sample: currentSample,
            Outros: '{ParFamiliaProduto_Id:' + currentFamiliaProdutoDCA_Id + ', ParProduto_Id:' + currentProdutoDCA_Id + '}'
        };

        coletasDCA.push(coletaDCA);

        //Contar quantidade de NC
        qtdeNC += coletaDCA.WeiDefects;
        numeroProximaAmostra++;
    }

    $(linhaTarefa).attr('data-qtdeNc', qtdeNC);
    $(linhaTarefa).find('.amostraNC').html(qtdeNC);
    $botaoColeta.prop("disabled", true);
    $botaoNa.prop("disabled", true);

    if (numeroProximaAmostra > maxSample) {
        numeroProximaAmostra = maxSample;
        $(linhaTarefa).attr('data-amostra-completa', 1);
        resetarLinha(linhaTarefa);
        verificaSalvar();

    } else {

        setTimeout(function () {

            $botaoColeta.prop("disabled", false);
            $botaoNa.prop("disabled", false);

            resetarLinha(linhaTarefa);

        }, 1000);
    }

    $(linhaTarefa).attr('data-sample', numeroProximaAmostra);
    $(linhaTarefa).find('.amostra').html(numeroProximaAmostra);

    atualizaCorSePassarDoLimiteDeNC();
    atualizaPorcentagemDeTarefas();
});

function resetarLinha(linha) {

    linha.removeClass('alert-secundary');
    linha.removeClass('alert-warning');
    linha.removeAttr('data-conforme-na');

    linha.find('[data-texto]').val('');

}

//$('body').off('click', '[data-salvar]').on('click', '[data-salvar]', function (e) {

//    e.preventDefault();

//    if (!HeaderFieldsIsValid()) {
//        return false;
//    }

//    if (!ColetasIsValid()) {
//        return false;
//    }

//    //Verifica se existe coleta já realizada para este cargo.
//    var coletaAgrupada = null;
//    $(coletasAgrupadas).each(function (i, o) {
//            if (o.ParLevel1_Id == currentParLevel1_Id
//            && o.ParLevel2_Id == currentParLevel2_Id) {
//            coletaAgrupada = o;
//        }
//    });

//    //Se não existir, cria uma zerada
//    if (coletaAgrupada == null) {
//        coletaAgrupada = {
//            ParLevel1_Id: currentParLevel1_Id,
//            ParLevel2_Id: currentParLevel2_Id,
//            Evaluation: currentEvaluationDCA.Evaluation,
//            Sample: currentEvaluationDCA.Sample
//        };
//    }

//    //var collectionHeaderFields = getCollectionHeaderFields();
//    //console.table(collectionHeaderFields);

//    //Insere valores da coleta
//    $($('form[data-form-coleta] div[data-linha-coleta]')).each(function (i, o) {
//        var data = $(o);
//        var isNA = $(data).attr('data-conforme-na') == "";
//        coletaJson.push(
//            {
//                Evaluation: coletaAgrupada.Evaluation,
//                Sample: coletaAgrupada.Sample,
//                ParDepartment_Id: currentParDepartment_Id,
//                ParCargo_Id: currentParCargo_Id,
//                ParLevel1_Id: $(data).attr('data-level1'),
//                ParLevel2_Id: $(data).attr('data-level2'),
//                ParLevel3_Id: $(data).attr('data-level3'),
//                ParCompany_Id: currentParCompany_Id,
//                IntervalMin: $(data).attr('data-min') == "null" ? null : $(data).attr('data-min'),
//                IntervalMax: $(data).attr('data-max') == "null" ? null : $(data).attr('data-max'),
//                IsConform: isNA ? 1 : $(data).attr('data-conforme') == "1",
//                Value: typeof ($(data).find('input[data-valor]').val()) == 'undefined' ? null : $(data).find('input[data-valor]').val(),
//                ValueText: typeof ($(data).find('input[data-texto]').val()) == 'undefined' ? null : $(data).find('input[data-texto]').val(),
//                IsNotEvaluate: isNA,
//                CollectionDate: getCurrentDate(),
//                UserSgq_Id: currentLogin.Id,
//                Weigth: $(data).attr('data-peso'),
//                WeiEvaluation: isNA ? 0 : $(data).attr('data-peso'),
//                Defects: isNA ? 0 : $(data).attr('data-conforme') == "1" ? 0 : 1,
//                WeiDefects: isNA ? 0 : ($(data).attr('data-conforme') == "1" ? 0 : 1) * parseInt($(data).attr('data-peso')),
//                Parfrequency_Id: parametrization.currentParFrequency_Id
//                /*
//				"Shift_Id":1,
//				"Period_Id":1,
//				"ParCluster_Id":1,
//				"CollectionType":1,
//				"PunishimentValue":1,
//				"HasPhoto":"0",
//				"HaveCorrectiveAction":"0",
//				"AlertLevel":"0",
//				"ParHeaderField_Id":1,
//				"ParHeaderField_Value":""
//				*/
//            }
//        );
//    });

//    processAlertRole(coletaJson);

//    var cabecalhos = getCollectionHeaderFields();

//    if (cabecalhos) {
//        cabecalhos.forEach(function (cabecalho) {
//            //campos de cabeçalhos
//            coletaJson.push(cabecalho);
//        });
//    }

//    //Se for a primeira, insere na lista de resultados
//    if (coletaAgrupada.Evaluation == 1 && coletaAgrupada.Sample == 1) {
//        coletasAgrupadas.push(coletaAgrupada);
//    }

//    //Salva a coleta realizada numa variavel global
//    SalvarColetas(coletaJson);

//    //Atualiza para a proxima coleta (se precisar adicionar amostra ou avaliação)
//    coletaAgrupada = AtualizaContadorDaAvaliacaoEAmostra(coletaAgrupada);

//    //Mostra mensagem de que a coleta foi realizada com sucesso e fecha após 3 segundos
//    openMensagem("Amostra salva com sucesso!", "blue", "white");
//    closeMensagem(3000);

//    if (coletaAgrupada.Sample == 1) {
//        //atualiza tela de coleta e contadores
//        listarParLevel2(true);
//    } else {
//        listarParLevels();
//        $("html, body").animate({ scrollTop: 0 }, "fast");
//    }
//});

function AtualizaContadorDaAvaliacao(coletaAgrupada) {
    coletaAgrupada.Evaluation++;
    return coletaAgrupada;
}

function SalvarColetasDCA(coletaJson) {

    for (var i = 0; i < coletaJson.length; i++) {
        globalColetasRealizadas.push(coletaJson[i]);
    }

    AtualizarArquivoDeColetas();
}

// function getCollectionHeaderFields() {

//     var collectionHeaderFied = [];

//     $('#headerFieldDepartment input, #headerFieldDepartment select').each(function () {

//         $self = $(this);

//         //TODO: validar se os campos de cabeçalho obrigatórios foram preenchidos;
//         if ($self.val())

//             collectionHeaderFied.push({
//                 ParHeaderField_Id: $self.attr("parheaderfield_id"),
//                 ParHeaderField_Value: $self.val(),
//                 Evaluation: currentEvaluationDCA.Evaluation,
//                 Sample: currentEvaluationDCA.Sample,
//                 ParDepartment_Id: currentParDepartment_Id,
//                 ParCargo_Id: currentParCargo_Id,
//                 ParCompany_Id: currentParCompany_Id,
//                 CollectionDate: getCurrentDate(),
//                 UserSgq_Id: currentLogin.Id,
//                 Parfrequency_Id: parametrization.currentParFrequency_Id
//             });

//     });

//     $('#headerFieldLevel1 input, #headerFieldLevel1 select').each(function () {

//         $self = $(this);

//         //TODO: validar se os campos de cabeçalho obrigatórios foram preenchidos;
//         if ($self.val())

//             collectionHeaderFied.push({
//                 ParHeaderField_Id: $self.attr("parheaderfield_id"),
//                 ParHeaderField_Value: $self.val(),
//                 Evaluation: currentEvaluationDCA.Evaluation,
//                 Sample: currentEvaluationDCA.Sample,
//                 ParDepartment_Id: currentParDepartment_Id,
//                 ParCargo_Id: currentParCargo_Id,
//                 ParCompany_Id: currentParCompany_Id,
//                 CollectionDate: getCurrentDate(),
//                 UserSgq_Id: currentLogin.Id,
//                 ParLevel1_Id: $self.parents('#headerFieldLevel1').attr('parLevel1Id'),
//                 Parfrequency_Id: parametrization.currentParFrequency_Id
//             });

//     });


//     $('#headerFieldLevel2 input, #headerFieldLevel2 select').each(function () {

//         $self = $(this);

//         //TODO: validar se os campos de cabeçalho obrigatórios foram preenchidos;
//         if ($self.val())

//             collectionHeaderFied.push({
//                 ParHeaderField_Id: $self.attr("parheaderfield_id"),
//                 ParHeaderField_Value: $self.val(),
//                 Evaluation: currentEvaluationDCA.Evaluation,
//                 Sample: currentEvaluationDCA.Sample,
//                 ParDepartment_Id: currentParDepartment_Id,
//                 ParCargo_Id: currentParCargo_Id,
//                 ParCompany_Id: currentParCompany_Id,
//                 CollectionDate: getCurrentDate(),
//                 UserSgq_Id: currentLogin.Id,
//                 ParLevel1_Id: $self.parents('#headerFieldLevel2').attr('parLevel1Id'),
//                 ParLevel2_Id: $self.parents('#headerFieldLevel2').attr('parLevel2Id'),
//                 Parfrequency_Id: parametrization.currentParFrequency_Id
//             });

//     });

//     return collectionHeaderFied;
// }

function ColetasIsValid() {
    var linhasDaColeta = $('form[data-form-coleta] div[data-linha-coleta]');
    for (var i = 0; i < linhasDaColeta.length; i++) {
        var data = linhasDaColeta[i];
        if ($(data).attr('data-conforme-na') != "") {
            if ($(data).attr('data-conforme') == ""
                || $(data).attr('data-conforme') == null
                || $(data).attr('data-conforme') == "undefined") {
                openMensagem("Obrigatório responder todas as Tarefas.", "blue", "white");
                mostraPerguntasObrigatorias(data);
                closeMensagem(2000);
                return false;
            }
        }
    }
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

//DCA coletasDCA
function getQuantidadeNC(parLevel1, parLevel2, parLevel3) {

    var qtdeNC = 0;

    if (coletasDCA.length == 0) {
        return qtdeNC;
    }

    var coletasDCAFilter = $.grep(coletasDCA, function (o) {

        return o.ParLevel1_Id == parLevel1.Id &&
            o.ParLevel2_Id == parLevel2.Id &&
            o.ParLevel3_Id == parLevel3.Id &&
            o.Evaluation == currentEvaluationDCA.Evaluation &&
            o.Outros.indexOf('ParFamiliaProduto_Id:' + currentFamiliaProdutoDCA_Id + ',') > 0;

    });

    if (coletasDCAFilter.length == 0) {
        return qtdeNC;
    }

    coletasDCAFilter.forEach(function (o) {
        qtdeNC += parseInt(o.WeiDefects);
    });

    return qtdeNC;
}

function getAmostraAtual(parLevel1, parLevel2, parLevel3) {

    var amostra = 1;

    if (coletasDCA.length == 0) {
        return amostra;
    }

    var coletasDCAFilter = $.grep(coletasDCA, function (o) {

        return o.ParLevel1_Id == parLevel1.Id &&
            o.ParLevel2_Id == parLevel2.Id &&
            o.ParLevel3_Id == parLevel3.Id &&
            o.Evaluation == currentEvaluationDCA.Evaluation &&
            o.Outros.indexOf('ParFamiliaProduto_Id:' + currentFamiliaProdutoDCA_Id + ',') > 0
    });

    var coletaAgrupadaColetada = getResultEvaluationDCA(parLevel1.Id, parLevel2.Id);
    if(!!coletaAgrupadaColetada.ParFamiliaProduto_Id && coletaAgrupadaColetada.Evaluation >= currentEvaluationDCA.Evaluation){
        return getAmostraTotal(parLevel1, parLevel2, parLevel3)+1;
    }

    //Melhorar essa bosta
    if (coletasDCAFilter.length == 0) {
        return amostra;
    }

    amostra = coletasDCAFilter[coletasDCAFilter.length - 1].Sample;

    return amostra + 1;

}

function getSynced(parLevel1, parLevel2, parLevel3) {

    if (coletasDCA.length == 0) {
        return false;
    }

    var coletasSincronizadas = $.grep(coletasDCA, function (o) {

        return o.ParLevel1_Id == parLevel1.Id &&
            o.ParLevel2_Id == parLevel2.Id &&
            o.ParLevel3_Id == parLevel3.Id &&
            o.Evaluation == currentEvaluationDCA.Evaluation &&
            o.Outros.indexOf('ParFamiliaProduto_Id:' + currentFamiliaProdutoDCA_Id + ',') > 0 &&
            o.Synced == true
    });

    return coletasSincronizadas.length > 0;

}

function getAmostraTotal(parLevel1, parLevel2, parLevel3) {

    var vinculosPeso = $.grep(parVinculosMontarLevel3DCA, function (o) {
        return o.ParLevel1_Id == parLevel1.Id && o.ParLevel2_Id == parLevel2.Id && o.ParLevel3_Id == parLevel3.Id;
    });

    if (vinculosPeso == null || vinculosPeso.length == 0 || !vinculosPeso[0].Sample) {
        return 0;
    }

    return parseInt(vinculosPeso[0].Sample);

}

function avaliacaoIsCompleta() {

    var avIsComplet = $.grep($('[data-linha-coleta]'), function (o) {
        return $(o).attr('data-amostra-completa') == 0;
    }).length == 0 ? true : false;

    return avIsComplet;
}

function habilitaBotaoSalvar() {
    $('button[data-salvar-dca]').attr("disabled", false);
}

function desabilitaBotaoSalvar() {
    $('button[data-salvar-dca]').attr("disabled", true);
}

function verificaSalvar() {

    if (avaliacaoIsCompleta())
        habilitaBotaoSalvar();

}


$('body').off('click', '[data-salvar-tarefas]').on('click', '[data-salvar-tarefas]', function (e) {
    e.preventDefault();
    var linhasDaColeta = $('form[data-form-coleta] div[data-linha-coleta]');
    for (var i = 0; i < linhasDaColeta.length; i++) {
        var data = linhasDaColeta[i];
        var btnSalvarTarefa = $(data).find('[data-coleta-dca]')[0];
        var amostraCompleta = $(data).attr('data-amostra-completa');
        if (amostraCompleta != '1') {
            $(btnSalvarTarefa).trigger('click');
        }
    }
});

$('body').off('click', '[data-salvar-dca]').on('click', '[data-salvar-dca]', function (e) {
    e.preventDefault();

    //TODO: aplicar função para inserir arr de coletas no arr de salvar coletas
    var cabecalhosDCA = getCollectionHeaderFieldsDCA();

    if (cabecalhosDCA) {
        cabecalhosDCA.forEach(function (cabecalho) {
            coletasDCA.unshift(cabecalho);
        });
    }

    var coletasDoMonitoramentoSendoSalvo = $.grep(coletasDCA, function (coleta) {
        return coleta.ParLevel1_Id == currentParLevel1_Id
            && (coleta.ParLevel2_Id == currentParLevel2_Id || coleta.ParLevel2_Id == null)
            && coleta.Outros != null
            && coleta.Outros.indexOf('ParFamiliaProduto_Id:' + currentFamiliaProdutoDCA_Id + ',') > 0
            && coleta.Synced != true
    });

    SalvarColetasDCA(coletasDoMonitoramentoSendoSalvo);

    for (var i = coletasDCA.length - 1; i >= 0; i--) {
        var coleta = coletasDCA[i];
        var exist = $.grep(coletasDoMonitoramentoSendoSalvo, function (coletaSalva) {
            return JSON.stringify(coletaSalva) == JSON.stringify(coleta);
        });
        if (exist.length > 0) {
            coletasDCA[i].Synced = true;
            //coletasDCA.splice(i, 1);
        }
    }

    SalvarColetasAgrupadasDCA();

    openMensagem("Avaliacao salva com sucesso!", "blue", "white");
    closeMensagem(3000);
    desabilitaBotaoSalvar();

});

function getCollectionHeaderFieldsDCA() {

    var collectionHeaderFiedDCA = [];

    $('#headerFieldLevel1 input, #headerFieldLevel1 select').each(function () {

        $self = $(this);

        //TODO: validar se os campos de cabeçalho obrigatórios foram preenchidos;
        if ($self.val())

            collectionHeaderFiedDCA.push({
                ParHeaderField_Id: $self.attr("parheaderfield_id"),
                ParHeaderField_Value: $self.val(),
                Evaluation: currentEvaluationDCA.Evaluation,
                Sample: currentEvaluationDCA.Sample,
                ParCompany_Id: currentParCompany_Id,
                CollectionDate: getCurrentDate(),
                UserSgq_Id: currentLogin.Id,
                ParLevel1_Id: $self.parents('#headerFieldLevel1').attr('parLevel1Id'),
                ParLevel2_Id: $self.parents('#headerFieldLevel1').attr('parLevel2Id'),
                Outros: '{ParFamiliaProduto_Id:' + currentFamiliaProdutoDCA_Id + ', ParProduto_Id:' + currentProdutoDCA_Id + '}'
            });

    });


    $('#headerFieldLevel2 input, #headerFieldLevel2 select').each(function () {

        $self = $(this);

        //TODO: validar se os campos de cabeçalho obrigatórios foram preenchidos;
        if ($self.val())

            collectionHeaderFiedDCA.push({
                ParHeaderField_Id: $self.attr("parheaderfield_id"),
                ParHeaderField_Value: $self.val(),
                Evaluation: currentEvaluationDCA.Evaluation,
                Sample: currentEvaluationDCA.Sample,
                ParCompany_Id: currentParCompany_Id,
                CollectionDate: getCurrentDate(),
                UserSgq_Id: currentLogin.Id,
                ParLevel1_Id: $self.parents('#headerFieldLevel2').attr('parLevel1Id'),
                ParLevel2_Id: $self.parents('#headerFieldLevel2').attr('parLevel2Id'),
                Outros: '{ParFamiliaProduto_Id:' + currentFamiliaProdutoDCA_Id + ', ParProduto_Id:' + currentProdutoDCA_Id + '}'
            });
    });

    return collectionHeaderFiedDCA;
}

function SalvarColetasAgrupadasDCA() {
    //Verifica se existe coleta já realizada para este cargo.
    var coletaAgrupada = null;
    $(coletasAgrupadas).each(function (i, o) {
        if (o.ParLevel1_Id == currentParLevel1_Id
            && o.ParLevel2_Id == currentParLevel2_Id
            && o.ParFamiliaProduto_Id == currentFamiliaProdutoDCA_Id) {
            coletaAgrupada = o;
        }
    });

    //Se não existir, cria uma zerada
    if (coletaAgrupada == null) {
        coletaAgrupada = {
            ParLevel1_Id: currentParLevel1_Id,
            ParLevel2_Id: currentParLevel2_Id,
            Evaluation: currentEvaluationDCA.Evaluation,
            ParFamiliaProduto_Id: currentFamiliaProdutoDCA_Id
        };

        coletasAgrupadas.push(coletaAgrupada);
    } else {
        coletaAgrupada = AtualizaContadorDaAvaliacao(coletaAgrupada);
    }
}

function atualizaCorSePassarDoLimiteDeNC() {
    $('[data-linha-coleta]').each(function (i, o) {
        var qtdeNC = parseInt($(o).attr('data-qtdenc'));
        var limiteNC = UmSeForNaNOuNull($(o).attr('data-limitenc'));

        $(o).find('.amostras-nc').removeClass('btn-danger');
        $(o).find('.amostras-nc').removeClass('btn-success');
        $(o).find('.amostras-nc').removeClass('btn-warning');

        if (qtdeNC > 0 && limiteNC >= qtdeNC) {
            $(o).find('.amostras-nc').addClass('btn-warning');
        } else if (qtdeNC > 0) {
            $(o).find('.amostras-nc').addClass('btn-danger');
        } else {
            $(o).find('.amostras-nc').addClass('btn-success');
        }
    });
}

function atualizaPorcentagemDeTarefas() {
    var quantidadeMaximaAmostraComPeso = 0;
    var quantidadeColetadaAmostraComPeso = 0;
    $('[data-linha-coleta]').each(function (i, o) {
        var amostraAtual = parseInt($(o).attr('data-sample'));
        var amostraNC = parseInt($(o).attr('data-qtdenc'));
        var amostraTotal = parseInt($(o).attr('data-samplemax'));
        var peso = parseInt($(o).attr('data-peso'));


        var amostraCompleta = parseInt($(o).attr('data-amostra-completa'));
        if (amostraCompleta != 1) {
            amostraAtual--;
        }
        if (amostraAtual > amostraTotal) {
            amostraAtual--;
        }

        quantidadeMaximaAmostraComPeso += amostraTotal * peso;
        quantidadeColetadaAmostraComPeso += (amostraAtual - amostraNC) * peso;
    });

    var calculoPorMonitoramento = getCalculoPorMonitoramento(currentParLevel1_Id, currentParLevel2_Id, currentEvaluationDCA.Evaluation);

    $('span.porcentagemTarefa').text(ZeroSeForNaN(calculoPorMonitoramento.Porcentagem));
    var porcentagemTotalMonitoramento = ZeroSeForNaN($('span.porcentagemTotalMonitoramento').text());

    $('span.porcentagemMonitoramento').text(ZeroSeForNaN((ZeroSeForNaN(calculoPorMonitoramento.Porcentagem) / 100) * porcentagemTotalMonitoramento));
}