var coletaJson = [];
var currentEvaluationSample = {};
var coletasDCA = [];

function openColetaDCA(levels) {

    coletaJson = [];
    var html = '';
    var coleta = '';

    levels.forEach(function (level1) {

        var hasLevel2 = false;

        level1.ParLevel2.forEach(function (level2) {

            var hasLevel3 = false;
            var striped = true;

            level2.ParLevel3.forEach(function (level3) {

                var inputLevel3 = getInputLevel3DCA(level3, level2, level1, striped);

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
        '<div class="container-fluid">                                                                                                             ' +
        '	<div class="">                                                                                                                         ' +
        '		<div class="col-xs-12">                                                                                                            ' +
        '			<div class="panel panel-primary">                                                                                              ' +
        '			  <div class="panel-heading">                                                                                                  ' +
        '				<h3 class="panel-title"><a onclick="listarParLevel2DCA();" class="btn btn-warning">Voltar</a> Questionario de Coleta</h3>                                   ' +
        '			  </div>                                                                                                                       ' +
        '			  <div class="panel-body">                                                                                                     ' +
        //getContador() +
        getParHeaderFieldDeparment() +
        '				<form data-form-coleta style="text-align:justify">                                                                                                    ' +
        coleta +
        '					<button class="btn btn-block btn-primary input-lg col-xs-12" data-salvar style="margin-top:10px" disabled>Salvar</button>       ' +
        '				</form>                                                                                                                    ' +
        '			  </div>                                                                                                                       ' +
        '       </div>                                                                                                                             ' +
        '    </div>                                                                                                                                ' +
        '	</div>                                                                                                                                 ' +
        '</div>';

    $('div#app').html(html);

    setBreadcrumbs();

}

//function getContador() {
//    currentEvaluationSample = getResultEvaluationSample(currentParLevel1_Id, currentParLevel2_Id);
//    return '<div class="col-xs-12 alert-info" id="divColeta" style="padding-top:10px;padding-bottom:10px">' +
//        '	<div class="col-xs-4">       ' +
//        '		Avaliação                ' +
//        '	</div>                       ' +
//        '	<div class="col-xs-4">       ' +
//        '		Amostra                  ' +
//        '	</div>                       ' +
//        '	<div class="col-xs-4">       ' +
//        '		&nbsp;                   ' +
//        '	</div>                       ' +
//        '	<div class="col-xs-4">       ' +
//        '		<strong>' + currentEvaluationSample.Evaluation + '/' + currentTotalEvaluationValue + '</strong>    ' +
//        '	</div>                       ' +
//        '	<div class="col-xs-4">       ' +
//        '		<strong>' + currentEvaluationSample.Sample + '/' + currentTotalSampleValue + '</strong>    ' +
//        '	</div>                       ' +
//        '	<div class="col-xs-4">       ' +
//        '		 &nbsp;                  ' +
//        '	</div>                       ' +
//        '	<div class="clearfix"></div> ' +
//        '</div>                          ';
//}

function getLevel1(level1) {
    return '<div class="col-xs-12" style="padding-top:5px;padding-bottom:5px;background-color:#edf5fc;" data-collapse-targeter="' + level1.Id + '"><small>' + level1.Name + '</small></div>';
}

function getLevel2(level2, level1) {
    return '<div class="col-xs-12" style="padding-left:18px;padding-top:5px;padding-bottom:5px;background-color:#fcf4e3;" data-collapse-target="' + level1.Id + '" data-collapse-targeter="' + level1.Id + '-' + level2.Id + '"><small>' + level2.Name + '</small></div>';
}

function getLevel3(level3, level2, level1) {
    return '<div class="col-xs-12" style="margin-bottom:10px;margin-top:10px" data-collapse-target="' + level1.Id + '-' + level2.Id + '">' + level3.Name + '</div>';
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

        var amostraCompleta = amostraAtual == amostraTotal ? true : false; //inativar botão de coleta

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
        retorno += ' data-sample="' + amostraAtual + '"';
        retorno += ' data-sampleMax="' + amostraTotal + '"';
        retorno += ' style="padding-left:10px;' + colorStriped + '">';

        switch (level3.ParLevel3InputType.Id) {

            case 1: //Binário
                retorno += getBinarioDCA(level3, amostraAtual, amostraTotal, amostraNC);
                break;
            case 2: //Numerodedefeitos
                retorno += getNumerodeDefeitosDCA(level3, amostraAtual, amostraTotal, amostraNC);
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

    var btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na>N/A</button>';
    var html = '';
    var botao = '';
    var btnColeta = '<button type="button" class="btn btn-success pull-right btn-sm btn-block" data-coleta>Salvar</button>';
    //var amostraAtual = 0;
    //var amostraTotal = 10;
    //var amostraNC = 0;
    var respostaPadrao = "";

    if (level3.ParLevel3XHelp)
        html += '<a style="cursor: pointer;" l3id="' + level3.Id + '" data-info><div class="col-xs-3"><small style="font-weight:550 !important">' + level3.Name + ' (Clique aqui)</small></div></a>';

    else
        html += '<div class="col-xs-3"><small style="font-weight:550 !important">' + level3.Name + '</small></div>';

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

    var htmlAmostra = '<div class="col-xs-2">Amostras: <spam class="amostra">' + amostraAtual + '</spam>/' + amostraTotal + '</div>';
    var htmlMaxMin = '<div class="col-xs-1">' + 0 + '</div>';
    var htmlAmostraNC = '<div class="col-xs-2">Amostras NC: <spam class="amostraNC">' + amostraNC + '</spam></div>';
    //var htmlEsconder = '<div class="col-xs-1></div>';

    html +=
        htmlAmostra +
        htmlMaxMin +
        htmlAmostraNC +
        //htmlEsconder +
        '<div class="col-xs-4 no-gutters">' +
        '   <div class="col-xs-8">' +
        botao +
        '   </div>' +
        '   <div class="col-xs-2">' + btnColeta + '</div>' +
        '   <div class="col-xs-2">' + btnNA + '</div>' +
        '</div>' +
        '<div class="clearfix"></div>';

    return html;
}

function getBinarioComTextoDCA(level3, amostraAtual, amostraTotal, amostraNC) {

    var btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na>N/A</button>';
    var btnColeta = '<button type="button" class="btn btn-success pull-right btn-sm btn-block" data-coleta>Salvar</button>';
    //var amostraAtual = 0;
    //var amostraTotal = 10;
    //var amostraNC = 0;

    var html = '';

    var respostaPadrao = "";

    if (level3.ParLevel3XHelp)
        html += '<a style="cursor: pointer;" l3id="' + level3.Id + '" data-info><div class="col-xs-3"><small style="font-weight:550 !important">' + level3.Name + ' (Clique aqui)</small></div></a>';
    else
        html += '<div class="col-xs-3"><small style="font-weight:550 !important">' + level3.Name + '</small></div>';

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

    var htmlAmostra = '<div class="col-xs-2">Amostras: <spam class="amostra">' + amostraAtual + '</spam>/' + amostraTotal + '</div>';
    var htmlMaxMin = '<div class="col-xs-1">' + 0 + '</div>';
    var htmlAmostraNC = '<div class="col-xs-2">Amostras NC: <spam class="amostraNC">' + amostraNC + '</spam></div>';
    //var htmlEsconder = '<div class="col-xs-1></div>';

    html +=
        htmlAmostra +
        htmlMaxMin +
        htmlAmostraNC +
        '<div class="col-xs-4 no-gutters">' +
        '   <div class="col-xs-4">' +
        '	    <input type="text" class="col-xs-12 input-sm" data-texto/>' +
        '   </div>' +
        '   <div class="col-xs-4">' +
        botao +
        '   </div>' +
        '   <div class="col-xs-2">' + btnColeta + '</div>' +
        '   <div class="col-xs-2">' + btnNA + '</div>' +
        // btnInfo +
        '</div>' +
        '<div class="clearfix"></div>';

    return html;
}

function getIntervaloDCA(level3, amostraAtual, amostraTotal, amostraNC) {

    var btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na>N/A</button>';
    var btnColeta = '<button type="button" class="btn btn-success pull-right btn-sm btn-block" data-coleta>Salvar</button>';
    //var amostraAtual = 0;
    //var amostraTotal = 10;
    //var amostraNC = 0;

    var html = '';

    if (level3.ParLevel3XHelp)
        html += '<a style="cursor: pointer;" l3id="' + level3.Id + '" data-info><div class="col-xs-3"><small style="font-weight:550 !important">' + level3.Name + ' (Clique aqui)</small></div></a>';

    else
        html += '<div class="col-xs-3"><small style="font-weight:550 !important">' + level3.Name + '</small></div>';

    var level3LimitLabel = !!level3.ParLevel3Value.ShowLevel3Limits ? ' MIN: ' + level3.ParLevel3Value.IntervalMin + ' | MAX: ' + level3.ParLevel3Value.IntervalMax : '';

    var htmlAmostra = '<div class="col-xs-2">Amostras: <spam class="amostra">' + amostraAtual + '</spam>/' + amostraTotal + '</div>';
    var htmlMaxMin = '<div class="col-xs-1">' + 0 + '</div>';
    var htmlAmostraNC = '<div class="col-xs-2">Amostras NC: <spam class="amostraNC">' + amostraNC + '</spam></div>';
    //var htmlEsconder = '<div class="col-xs-1></div>';

    html +=
        htmlAmostra +
        htmlMaxMin +
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
        '   <div class="col-xs-2">' + btnColeta + '</div>' +
        '   <div class="col-xs-2">' + btnNA + '</div>' +
        // btnInfo +
        '</div>' +
        '<div class="clearfix"></div>';

    return html;
}

function getIntervaloemMinutosDCA(level3, amostraAtual, amostraTotal, amostraNC) {

    var btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na>N/A</button>';
    var btnColeta = '<button type="button" class="btn btn-success pull-right btn-sm btn-block" data-coleta>Salvar</button>';
    //var amostraAtual = 0;
    //var amostraTotal = 10;
    //var amostraNC = 0;

    var html = '';

    if (level3.ParLevel3XHelp)
        html += '<a style="cursor: pointer;" l3id="' + level3.Id + '" data-info><div class="col-xs-3"><small style="font-weight:550 !important">' + level3.Name + ' (Clique aqui)</small></div></a>';

    else
        html += '<div class="col-xs-3"><small style="font-weight:550 !important">' + level3.Name + '</small></div>';

    var level3LimitLabel = !!level3.ParLevel3Value.ShowLevel3Limits ? ' MIN: ' + level3.ParLevel3Value.IntervalMin + ' | MAX: ' + level3.ParLevel3Value.IntervalMax : '';

    var htmlAmostra = '<div class="col-xs-2">Amostras: <spam class="amostra">' + amostraAtual + '</spam>/' + amostraTotal + '</div>';
    var htmlMaxMin = '<div class="col-xs-1">' + 0 + '</div>';
    var htmlAmostraNC = '<div class="col-xs-2">Amostras NC: <spam class="amostra">' + amostraNC + '</spam></div>';
    //var htmlEsconder = '<div class="col-xs-1></div>';

    html +=
        htmlAmostra +
        htmlMaxMin +
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
        '<div class="col-xs-2">' + btnColeta + '</div>' +
        '<div class="col-xs-2">' + btnNA + '</div>' +
        // btnInfo +
        '</div>' +
        '<div class="clearfix"></div>';

    return html;
}

function getIntervaloComObservacaoDCA(level3, amostraAtual, amostraTotal, amostraNC) {

    var btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na>N/A</button>';
    var btnColeta = '<button type="button" class="btn btn-success pull-right btn-sm btn-block" data-coleta>Salvar</button>';
    //var amostraAtual = 0;
    //var amostraTotal = 10;
    //var amostraNC = 0;

    var html = '';

    if (level3.ParLevel3XHelp)
        html += '<a style="cursor: pointer;" l3id="' + level3.Id + '" data-info><div class="col-xs-3"><small style="font-weight:550 !important">' + level3.Name + ' (Clique aqui)</small></div></a>';

    else
        html += '<div class="col-xs-3"><small style="font-weight:550 !important">' + level3.Name + '</small></div>';

    var level3LimitLabel = !!level3.ParLevel3Value.ShowLevel3Limits ? ' MIN: ' + level3.ParLevel3Value.IntervalMin + ' | MAX: ' + level3.ParLevel3Value.IntervalMax : '';


    var htmlAmostra = '<div class="col-xs-2">Amostras: <spam class="amostra">' + amostraAtual + '<spam>/' + amostraTotal + '</div>';
    var htmlMaxMin = '<div class="col-xs-1">' + 0 + '</div>';
    var htmlAmostraNC = '<div class="col-xs-2">Amostras NC: <spam class="amostraNC">' + amostraNC + '</spam></div>';
    //var htmlEsconder = '<div class="col-xs-1></div>';

    html +=
        htmlAmostra +
        htmlMaxMin +
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
        '<div class="col-xs-2">' + btnColeta + '</div>' +
        '<div class="col-xs-2">' + btnNA + '</div>' +
        // btnInfo +
        '</div>' +
        '<div class="clearfix"></div>';

    return html;
}

function getObservacaoDCA(level3, amostraAtual, amostraTotal, amostraNC) {

    var btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na>N/A</button>';
    var btnColeta = '<button type="button" class="btn btn-success pull-right btn-sm btn-block" data-coleta>Salvar</button>';
    //var amostraAtual = 0;
    //var amostraTotal = 10;
    //var amostraNC = 0;

    var html = '';

    if (level3.ParLevel3XHelp)
        html += '<a style="cursor: pointer;" l3id="' + level3.Id + '" data-info><div class="col-xs-3"><small style="font-weight:550 !important">' + level3.Name + ' (Clique aqui)</small></div></a>';

    else
        html += '<div class="col-xs-3"><small style="font-weight:550 !important">' + level3.Name + '</small></div>';

    var htmlAmostra = '<div class="col-xs-2">Amostras: <spam class="amostra">' + amostraAtual + '</spam>/' + amostraTotal + '</div>';
    var htmlMaxMin = '<div class="col-xs-1">' + 0 + '</div>';
    var htmlAmostraNC = '<div class="col-xs-2">Amostras NC: <spam class="amostraNC">' + amostraNC + '</spam></div>';
    //var htmlEsconder = '<div class="col-xs-1></div>';

    html +=
        htmlAmostra +
        htmlMaxMin +
        htmlAmostraNC +
        '<div class="col-xs-4 no-gutters">' +
        '<div class="col-xs-8">' +
        '	<input type="text" class="col-xs-12 input-sm" data-texto/>' +
        '</div>' +
        '<div class="col-xs-2">' + btnColeta + '</div>' +
        '<div class="col-xs-2">' + btnNA + '</div>' +
        // btnInfo +
        '</div>' +
        '<div class="clearfix"></div>';

    return html;
}

function getTextoDCA(level3, amostraAtual, amostraTotal, amostraNC) {

    var btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na>N/A</button>';
    var btnColeta = '<button type="button" class="btn btn-success pull-right btn-sm btn-block" data-coleta>Salvar</button>';
    //var amostraAtual = 0;
    //var amostraTotal = 10;
    //var amostraNC = 0;

    var html = '';

    if (level3.ParLevel3XHelp)
        html += '<a style="cursor: pointer;" l3id="' + level3.Id + '" data-info><div class="col-xs-3"><small style="font-weight:550 !important">' + level3.Name + ' (Clique aqui)</small></div></a>';

    else
        html += '<div class="col-xs-3"><small style="font-weight:550 !important">' + level3.Name + '</small></div>';

    var htmlAmostra = '<div class="col-xs-2">Amostras: <spam class="amostra">' + amostraAtual + '</spam>/' + amostraTotal + '</div>';
    var htmlMaxMin = '<div class="col-xs-1">' + 0 + '</div>';
    var htmlAmostraNC = '<div class="col-xs-2">Amostras NC: <spam class="amostraNC">' + amostraNC + '</spam></div>';
    //var htmlEsconder = '<div class="col-xs-1></div>';

    html +=
        htmlAmostra +
        htmlMaxMin +
        htmlAmostraNC +
        '<div class="col-xs-4 no-gutters">' +
        '<div class="col-xs-8">' +
        '	<input type="text" class="col-xs-12 input-sm" data-valor/>' +
        '</div>' +
        '<div class="col-xs-2">' + btnColeta + '</div>' +
        '<div class="col-xs-2">' + btnNA + '</div>' +
        // btnInfo +
        '</div>' +
        '<div class="clearfix"></div>';

    return html;
}

function getNumerodeDefeitosDCA(level3, amostraAtual, amostraTotal, amostraNC) {

    var btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na>N/A</button>';
    var btnColeta = '<button type="button" class="btn btn-success pull-right btn-sm btn-block" data-coleta>Salvar</button>';
    //var amostraAtual = 0;
    //var amostraTotal = 10;
    //var amostraNC = 0;

    var html = '';

    if (level3.ParLevel3XHelp)
        html += '<a style="cursor: pointer;" l3id="' + level3.Id + '" data-info><div class="col-xs-3"><small style="font-weight:550 !important">' + level3.Name + ' (Clique aqui)</small></div></a>';

    else
        html += '<div class="col-xs-3"><small style="font-weight:550 !important">' + level3.Name + '</small></div>';

    var htmlAmostra = '<div class="col-xs-2">Amostras: <spam class="amostra">' + amostraAtual + '</spam>/' + amostraTotal + '</div>';
    var htmlMaxMin = '<div class="col-xs-1">' + 0 + '</div>';
    var htmlAmostraNC = '<div class="col-xs-2">Amostras NC: <spam class="amostraNC">' + amostraNC + '</spam></div>';
    //var htmlEsconder = '<div class="col-xs-1></div>';

    html +=
        htmlAmostra +
        htmlMaxMin +
        htmlAmostraNC +
        '<div class="col-xs-4 no-gutters">' +
        '<div class="col-xs-8">' +
        '	<input type="number" class="col-xs-12 input-sm" data-valor/>' +
        '</div>' +
        '<div class="col-xs-2">' + btnColeta + '</div>' +
        '<div class="col-xs-2">' + btnNA + '</div>' +
        // btnInfo +
        '</div>' +
        '<div class="clearfix"></div>';

    return html;
}

function getLikertDCA(level3, amostraAtual, amostraTotal, amostraNC) {

    var btnNA = '<button type="button" class="btn btn-warning pull-right btn-sm btn-block" data-na>N/A</button>';
    var btnColeta = '<button type="button" class="btn btn-success pull-right btn-sm btn-block" data-coleta>Salvar</button>';
    //var amostraAtual = 0;
    //var amostraTotal = 10;
    //var amostraNC = 0;

    var html = '';

    if (level3.ParLevel3XHelp)
        html += '<a style="cursor: pointer;" l3id="' + level3.Id + '" data-info><div class="col-xs-3"><small style="font-weight:550 !important">' + level3.Name + ' (Clique aqui)</small></div></a>';

    else
        html += '<div class="col-xs-3"><small style="font-weight:550 !important">' + level3.Name + '</small></div>';

    var level3LimitLabel = !!level3.ParLevel3Value.ShowLevel3Limits ? ' MIN: ' + level3.ParLevel3Value.IntervalMin + ' | MAX: ' + level3.ParLevel3Value.IntervalMax : '';

    var htmlAmostra = '<div class="col-xs-2">Amostras: <spam class="amostra">' + amostraAtual + '</spam>/' + amostraTotal + '</div>';
    var htmlMaxMin = '<div class="col-xs-1">' + 0 + '</div>';
    var htmlAmostraNC = '<div class="col-xs-2">Amostras NC: <spam class="amostraNC">' + amostraNC + '</spam></div>';
    //var htmlEsconder = '<div class="col-xs-1></div>';

    html +=
        htmlAmostra +
        htmlMaxMin +
        htmlAmostraNC +
        '<div class="col-xs-4 no-gutters">' +
        '   <div class="col-xs-2 input-sm" style="font-size: 8px;">' +
        level3LimitLabel +
        '   </div>' +
        '   <div class="col-xs-6">' +
        '	    <input type="text" class="col-xs-12 input-sm" data-valor/>' +
        '   </div>' +
        '   <div class="col-xs-2">' + btnColeta + '</div>' +
        '   <div class="col-xs-2">' + btnNA + '</div>' +
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

$('body').off('click', '[data-collapse-targeter]').on('click', '[data-collapse-targeter]', function () {
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

$('body').off('click', '[data-coleta]').on('click', '[data-coleta]', function () {

    $(this).prop("disabled", true);

    var data = $(this).parents('[data-linha-coleta]');
    var currentEvaluation = 1; //$(data).attr('data-evaluation');
    var currentSample = parseInt($(data).attr('data-sample'));
    var isNA = $(data).attr('data-conforme-na') == "";
    var qtdeNC = parseInt($(data).attr('data-qtdeNc'));
    var maxSample = parseInt($(data).attr('data-samplemax'));

    var coletaDCA = {

        Id: 0,
        CollectionDate: currentCollectDate.toJSON(),
        UserSgq_Id: currentLogin.Id,
        Shift_Id: 1,
        Period_Id: 1,
        //ParCargo_Id: 
        ParCompany_Id: currentParCompany_Id,
        //ParDepartment_Id: 
        ParLevel1_Id: currentParLevel1_Id,
        //ParCluster_Id
        ParLevel2_Id: currentParLevel2_Id,
        ParLevel3_Id: parseInt($(data).attr('data-level3')),
        //CollectionType
        Weigth: $(data).attr('data-peso'),
        IntervalMin: $(data).attr('data-min') == "null" ? null : $(data).attr('data-min'),
        IntervalMax: $(data).attr('data-max') == "null" ? null : $(data).attr('data-max'),
        Value: typeof ($(data).find('input[data-valor]').val()) == 'undefined' ? null : $(data).find('input[data-valor]').val(),
        ValueText: typeof ($(data).find('input[data-texto]').val()) == 'undefined' ? null : $(data).find('input[data-texto]').val(),
        IsConform: isNA ? 1 : $(data).attr('data-conforme') == "1",
        IsNotEvaluate: isNA,
        Defects: isNA ? 0 : $(data).attr('data-conforme') == "1" ? 0 : 1,
        //PunishimentValue: 
        WeiEvaluation: isNA ? 0 : $(data).attr('data-peso'),
        Evaluation: currentEvaluation,
        WeiDefects: isNA ? 0 : ($(data).attr('data-conforme') == "1" ? 0 : 1) * parseInt($(data).attr('data-peso')),
        //HasPhoto: 
        Sample: currentSample,
        //HaveCorrectiveAction: 
        Parfrequency_Id: currentParFrequency_Id
        //AlertLevel: 
        //ParHeaderField_Id: 
        //ParHeaderField_Value: 
        //ParHeaderField_Value: 
        //IsProcessed: 

    };

    coletasDCA.push(coletaDCA);

    //Contar quantidade de NC
    qtdeNC += coletaDCA.WeiDefects;

    var numeroProximaAmostra = currentSample + 1;

    $(data).attr('data-qtdeNc', qtdeNC);
    $(data).find('.amostraNC').html(qtdeNC);

    $(data).attr('data-sample', numeroProximaAmostra);
    $(data).find('.amostra').html(numeroProximaAmostra);

    if (numeroProximaAmostra > maxSample) {

        $(data).prop("disabled", true);

    } else {

        $(this).prop("disabled", false);
    }

});

function resetarLinha(linha) {
    linha.removeClass('alert-secundary');
    linha.removeClass('alert-warning');
    linha.removeAttr('data-conforme-na');
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
//            Evaluation: currentEvaluationSample.Evaluation,
//            Sample: currentEvaluationSample.Sample
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
            o.ParLevel3_Id == parLevel3.Id;

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
            o.ParLevel3_Id == parLevel3.Id;
    });

    //Melhorar essa bosta
    if (coletasDCAFilter.length == 0) {
        return amostra;
    }

    amostra = coletasDCAFilter[coletasDCAFilter.length - 1].Evaluation;

    return amostra + 1;

}

function getAmostraTotal(parLevel1, parLevel2, parLevel3) {

    var vinculosPeso = $.grep(parVinculosMontarLevel3, function (o) {
        return o.ParLevel1_Id == parLevel1.Id && o.ParLevel2_Id == parLevel2.Id && o.ParLevel3_Id == parLevel3.Id;
    });

    if (vinculosPeso == null || vinculosPeso.length == 0 || !vinculosPeso[0].Sample) {
        return 0;
    }

    return parseInt(vinculosPeso[0].Sample);

}