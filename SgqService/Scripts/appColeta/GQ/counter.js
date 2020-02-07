//Defects na Amostra
var defectsEvaluateSample = 0;
var defects3MoreSample = 0; //******* EUA CFF *********/////
var defectsLevel2Sample = 0;

//Defects na Avaliação
var defectsEvaluateTotal = 0;
var defects3MoreEvaluateTotal = 0; //******* EUA CFF *********/////
var defectsLevel1Total = 0;
var defectsLevel2Total = 0;

function contadorLadoComDefeitos() {

    var notconform = $('.level3Group:visible .level3.boolean').children('.counters').children('span.response[value="0"]');
    notconform.removeClass('lightred').removeAttr('notconform').children('.counters').children('span.response').attr('value', '1');
    notconform.children('.counters').children('span.response').text(notconform.children('.counters').children('span.response:first').attr('booltruename'));

    var defectsEvaluateSampleLocal = defectsEvaluateSample;

    //tem erro
    if (notconform.length) {
        defectsEvaluateSample = 1;
    }
    else {
        defectsEvaluateSample = 0;
    }

    if (defectsEvaluateSampleLocal > defectsEvaluateSample) {
        defectsEvaluateTotal--;
    }
    else if (defectsEvaluateSampleLocal < defectsEvaluateSample) {
        defectsEvaluateTotal++;
    }

    $('.level3Group:visible .defects_evaluate').text(defectsEvaluateTotal);

}
function contador3Defeitos() {

    var notconform = $('.level3Group:visible .level3.boolean').children('.counters').children('span.response[value="0"]');
    notconform.removeClass('lightred').removeAttr('notconform').children('.counters').children('span.response').attr('value', '1');
    notconform.children('.counters').children('span.response').text(notconform.children('.counters').children('span.response:first').attr('booltruename'));

    var defects3MoreSampleLocal = defects3MoreSample;

    //tem erro
    if (notconform.length >= 3) {
        defects3MoreSample = 1;
    }
    else {
        defects3MoreSample = 0;
    }

    if (defects3MoreSampleLocal > defects3MoreSample) {
        defects3MoreEvaluateTotal--;
    }
    else if (defects3MoreSampleLocal < defects3MoreSample) {
        defects3MoreEvaluateTotal++;
    }

    $(_level2).parents('.level2Group').find('.three_more_defects').text(defects3MoreEvaluateTotal);
    $('.level3Group:visible .three_more_defects').text(defects3MoreEvaluateTotal);

}
var mockCFF = 0; mockCFFSmp = 0;
function updateCounters(level2, level3Group) {
    var currentEvaluate = 1;
    var currenteSample = 0;
    var level1 = $('.level1.selected');

    if (level3Group.attr('level1idgroup')) {
        level2 = level3Group.find('.level2:last');
        $('.level2.group[parlevel1_id_group=' + level3Group.attr('level1idgroup') + ']').attr('evaluate', level2.attr('evaluate'));
        $('.level2.group[parlevel1_id_group=' + level3Group.attr('level1idgroup') + ']').attr('sample', level2.attr('sample'));
    }

    if (level2.attr('evaluatecurrent') != undefined) {
        currentEvaluate = parseInt(level2.attr('evaluatecurrent'));
        currenteSample = level2.attr('samplecurrent') != undefined ? parseInt(level2.attr('samplecurrent')) : 0;
    }

    if (level1.attr('hasgrouplevel2') == 'true' && level1.attr('isreaudit') == 'true') {
        currentEvaluate = parseInt(level1.attr('lastevaluate'));
        currenteSample = parseInt(level1.attr('lastsample'));
    }
    var ult = false;
    if (currenteSample == 0) {
        currenteSample = 1;
    } else {

        if (level1.attr('ispartialsave') == "true") {
            var resultPartial = $('.Resultlevel2[level1id=' + level1.attr('id') + '][level2id=' + level2.attr('id') + '][collectiondate="' + $('.App').attr('date') + '"][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + '] .r3l2');
            if (resultPartial.length == level3Group.find('.level3').length) {
                currenteSample = currenteSample + 1;
            }
        }
        else {
            currenteSample = currenteSample + 1;
        }

        if (level1.attr('hasgrouplevel2') == "true") {
            if (currenteSample > parseInt(level2.attr('sample'))) {
                ult = true;
                currenteSample = 1;
                if (level1.attr('isreaudit') == 'true')
                    currentEvaluate++;
            }

        }
    }

    if (level1.attr('hasgrouplevel2') == 'true' && level1.attr('isreaudit') == 'true' && level1.attr('lastevaluate') == '0')
        currentEvaluate = parseInt(level1.attr('lastevaluate')) + 1;

    if (isEUA == true && level1.attr('hasgrouplevel2') == 'true') {// && level1.attr('isreaudit') != "true") {
        if (mockCFF > currentEvaluate)
            currentEvaluate = mockCFF;
        else
            mockCFF = currentEvaluate;
    }
    if (isEUA == true && level1.attr('hasgrouplevel2') == 'true' && level1.attr('isreaudit') == 'true') {
        if (mockCFFSmp > currenteSample)
            currenteSample = mockCFFSmp;
        else
            mockCFFSmp = currenteSample;
    }

    // if (level1.attr('hasgrouplevel2') == 'true' && level1.attr('isreaudit') == 'true' && currenteSample > 1)
    //     currenteSample-- ;
    var sampleInf = level2.attr('sample') == 0 ? '&infin;' : level2.attr('sample');
    var evaluateInf = level2.attr('evaluate') == 0 ? '&infin;' : level2.attr('evaluate');

    level3Group.children('.painelLevel03').children('div').children('div').children('label').children('.evaluateCurrent').text(currentEvaluate);
    level3Group.children('.painelLevel03').children('div').children('div').children('label').children('.sampleCurrent').text(currenteSample);
    level3Group.children('.painelLevel03').children('div').children('div').children('label').children('.evaluateTotal').html(evaluateInf);
    level3Group.children('.painelLevel03').children('div').children('div').children('label').children('.sampleTotal').html(sampleInf);

    if (level1.attr('hasgrouplevel2') == 'true') {
        if (ult) {
            auxCurSmp = (currentEvaluate - 1) * parseInt(level2.attr('sample'));
            //auxCurSmp += currenteSample;
            $('.level2Group li[id=' + level1.attr('id') + '] .counters .sampleCurrentTotal').text(auxCurSmp);
        } else {
            auxCurSmp = (currentEvaluate - 1) * parseInt(level2.attr('sample'));
            auxCurSmp += currenteSample;
            //if(level1.attr('isreaudit') == 'true' && $('.breadcrumb li').length == 2){
            auxCurSmp--;
            //}
            if (auxCurSmp > (parseInt(level2.attr('sample')) * parseInt(level2.attr('evaluate')))) {
                auxCurSmp = parseInt(level2.attr('sample')) * parseInt(level2.attr('evaluate'));
            }
            $('.level2Group li[id=' + level1.attr('id') + '] .counters .sampleCurrentTotal').text((auxCurSmp));
        }
        $('.level2Group li[id=' + level1.attr('id') + '] .counters .evaluateCurrent').text(currentEvaluate - 1);
        if (tdefAv)
            $('.SmpDefects').text(tdefAv.text());
    }
    counterSetSide(level3Group);
    var sampleCurrentTotal = currenteSample;

    var sampleByEvaluate = parseInt(level2.attr('sample'));

    if (currentEvaluate > 1 && currenteSample == 1) {
        sampleCurrentTotal = sampleByEvaluate * (currentEvaluate - 1);
    }
    else if (currentEvaluate > 1) {
        sampleCurrentTotal = ((currentEvaluate - 1) * (sampleByEvaluate)) + (currenteSample - 1);
    }
    else {
        sampleCurrentTotal = currenteSample;
    }

    var sampleCurrentTotalLevel2 = sampleCurrentTotal;

    //var sampleCurrentLevel2 = parseInt(level2.siblings('.counters').children('div').children('.sampleCurrent').text());
    var sampleCurrentLevel2 = level2.attr('sampleCurrent') == undefined ? 0 : parseInt(level2.attr('sampleCurrent'));

    var evaluateCurrentLevel2 = currentEvaluate;
    if (evaluateCurrentLevel2 > 1 && sampleCurrentLevel2 == 0) {
        evaluateCurrentLevel2--;
    }
    else if (evaluateCurrentLevel2 == 1 && sampleCurrentTotalLevel2 == 1) {
        evaluateCurrentLevel2--;
        sampleCurrentTotalLevel2--;
    }
    else if (evaluateCurrentLevel2 == 1) {
        sampleCurrentTotalLevel2--;
    }
    //level2.siblings('.counters').children('div').children('.evaluateCurrent').text(evaluateCurrentLevel2);
    level2.siblings('.counters').children('div').children('.evaluateCurrent').text(evaluateCurrentLevel2);
    level2.siblings('.counters').children('div').children('.sampleCurrentTotal').text(currenteSample);
    //level3Group.children('.painelLevel03').find('.sampleCurrentTotal').text(sampleCurrentTotal);
    //level3Group.children('.painelLevel03').find('.sampleXEvaluateTotal').text(level2.siblings('.counters').children('div').children('.sampleXEvaluateTotal').text());
    //

    var evaluateCompleted = currentEvaluate;

    // if (currenteSample <= parseInt(level2.attr('sample')) && currentEvaluate > 0) {
    //     evaluateCompleted--;
    // }
    level2.parents('.level2Group').find('.EvaluateCompleted').text(evaluateCompleted);
    level3Group.find('.EvaluateCompleted').text(evaluateCompleted);
    var defectsLevel2Current = 0;

    level2.attr('defectsLevel2Current', defectsLevel2Current);
    //$(_level1).attr('lastevaluate', evaluateCompleted);
}

function counterSetSide(level3Group) {
    level3Group.find('.current_evaluation').text(level3Group.find('.evaluateCurrent').text());
    level3Group.find('.current_side').text(level3Group.find('.sampleCurrent').text());
}

function writeCounter(ParLevel1_Id) {
    var rn = $('.level1.selected').attr('reauditnumber');
    var ir = $('.level1.selected').attr('isreaudit');

    if (!ir) {
        rn = 0;
    } else if (ir == 'true' && !rn) {
        rn = 1;
    }

    var r = '';
    $('[level1TdefId=' + ParLevel1_Id + ']').each(function (i, o) {
        r += $('[level1TdefId=' + ParLevel1_Id + ']')[i].outerHTML;
    });
    _writeFile("level1Counter" + ParLevel1_Id + getCollectionDate() + $('.App').attr('unidadeid') + rn + ".txt", r);
}

function readCounter(ParLevel1_Id) {
    var rn = $('.level1.selected').attr('reauditnumber');
    var ir = $('.level1.selected').attr('isreaudit');

    if (!ir) {
        rn = 0;
    } else if (ir == 'true' && !rn) {
        rn = 1;
    }
    _readFile("level1Counter" + ParLevel1_Id + getCollectionDate() + $('.App').attr('unidadeid') + rn + ".txt", function (r) {
        if (!r)
            writeCounter(ParLevel1_Id);
        else {
            $('[level1TdefId=' + ParLevel1_Id + '][id=tdefPeriod' + $('.App').attr('period') + 'Shif' + $('.App').attr('shift') + 'level1TdefId' + ParLevel1_Id + ']').each(function (i, o) {
                // $(o).replaceWith($($(r)[i]));
                var fp = $($('<div>' + r + '</div>').find('[id=' + $(o).attr('id') + ']')[i]);
                $(fp).show();
                $(o).find('span').text($(fp).find('span').text());
            });
        }
    });
}

// Este método esconde o numero de av, am ou defeitos da linha do monitoramento
function updateCounterLinhaLevel2(level1, level2) {
    var evaluationCurrent = 0;
    var sampleCurrent = 0;

   //verifica se exibe a avaliação na linha do monitoramento
    if ($('.counter[indicador=' + level2.attr('id') + '][headerlevel=level2_line][counter=evaluation]').length == 0
       && $('.counter[indicador=' + level1.attr('id') + '][headerlevel=level2_line][counter=evaluation]').length == 0) { //se não tiver, ele esconde

       $('.list-group-item[id=' + level2.attr('id') + '] .evaluateCurrent').addClass('hide');       
       $($('.list-group-item[id=' + level2.attr('id') + '] .separator')[0]).addClass('hide');
       $('.list-group-item[id=' + level2.attr('id') + '] .evaluateTotal').addClass('hide');

   } else { //se tiver, calcula a avaliação atual

       // Este método foi descontinuado para teste

        var lastEvaluation;

        if (level2.attr('parfrequency_id') == 1) {// se for por periodo (USA)
            lastEvaluation = $('.Resultlevel2[level2id=' + level2.attr('id') + '][level1id='+level1.attr('id')+'][period=' + $('.App').attr('period') + '][shift=' + $('.App').attr('shift') + ']:last').attr('evaluation');
        } else {
            lastEvaluation = $('.Resultlevel2[level2id=' + level2.attr('id') + '][level1id='+level1.attr('id')+'][shift=' + $('.App').attr('shift') + ']:last').attr('evaluation');
        }

        if (lastEvaluation > 0) {
            evaluationCurrent = parseInt($('.Resultlevel2[level2id=' + level2.attr('id') + '][level1id='+level1.attr('id')+']:last').attr('evaluation'));
            $('.list-group-item[id=' + level2.attr('id') + '] .evaluateCurrent').text(evaluationCurrent);

        } else
            $('.list-group-item[id=' + level2.attr('id') + '] .evaluateCurrent').text(0);
   }

   if ($('.counter[indicador=' + level2.attr('id') + '][headerlevel=level2_line][counter=sample]').length == 0
       && $('.counter[indicador=' + level1.attr('id') + '][headerlevel=level2_line][counter=sample]').length == 0) { //verifica se exibe a sample na linha do monitoramento

       $('.list-group-item[id=' + level2.attr('id') + '] .sampleCurrentTotal').addClass('hide');
       $($('.list-group-item[id=' + level2.attr('id') + '] .separator')[1]).addClass('hide');
       $('.list-group-item[id=' + level2.attr('id') + '] .sampleXEvaluateTotal').addClass('hide');

   } else {

       // Este método foi comentado para teste

        var lastSample;

        if (level2.attr('parfrequency_id') == 1) {

            lastSample = $('.Resultlevel2[level2id=' + level2.attr('id') + '][level1id='+level1.attr('id')+'][period=' + $('.App').attr('period') + '][shift=' + $('.App').attr('shift') + ']:last').attr('sample');

        } else {

            lastSample = $('.Resultlevel2[level2id=' + level2.attr('id') + '][level1id='+level1.attr('id')+'][shift=' + $('.App').attr('shift') + ']:last').attr('sample');

        }

        if (lastSample > 0) {

            sampleCurrent = parseInt($('.Resultlevel2[level2id=' + level2.attr('id') + '][level1id='+level1.attr('id')+']:last').attr('sample'));
            var total = parseInt(level2.attr('sample'));

            if (evaluationCurrent > 0) {

                sampleCurrent = ((evaluationCurrent - 1) * total) + sampleCurrent + 1;

            }

            $('.list-group-item[id=' + level2.attr('id') + '] .sampleCurrentTotal').text(sampleCurrent);

        } else
            $('.list-group-item[id=' + level2.attr('id') + '] .sampleCurrentTotal').text(0);
   }

    if ($('.counter[indicador=' + level2.attr('id') + '][headerlevel=level2_line][counter=defects][level=2]').length == 0
        && $('.counter[indicador=' + level1.attr('id') + '][headerlevel=level2_line][counter=defects][level=1]').length == 0 && level1.attr('hasgrouplevel2') != 'true') {
        $('.list-group-item[id=' + level2.attr('id') + '] .defectstotal').addClass('hide');
    }

    if ($('.counter[indicador=' + level2.attr('id') + '][headerlevel=level2_line][counter=frequency][level=2]').length == 0
        && $('.counter[indicador=' + level1.attr('id') + '][headerlevel=level2_line][counter=frequency][level=1]').length == 0 && level1.attr('hasgrouplevel2') != 'true') {
        $('.list-group-item[id=' + level2.attr('id') + '] .frequencyTotal').addClass('hide');
    }
    //}
}

function setAvaliationClick(level2) {

    var avaliationNumber = level2.attr('evaluate');
    var sampleNumber = level2.attr('sample');

    //Se for 0 nos dois, ele é do tipo dinâmico que o usuário irá inserir na mão a quantidade
    if (avaliationNumber == 0 || sampleNumber == 0) {
        level2.next().addClass('changeAvNumber');
    }
}

function setAvaliationLevel2(level2) {

    var avaliationNumber = level2.attr('evaluate');

    level2.next().find('.evaluateTotal').html(avaliationNumber);
    if (avaliationNumber == 0)
        level2.next().find('.evaluateTotal').html('&infin;');

}

function setSampleLevel2(level2) {

    var sampleNumber = level2.attr('sample');

    //level2.next().find('.sampleXEvaluateTotal').html(sampleNumber);
    if (sampleNumber == 0)
        level2.next().find('.sampleXEvaluateTotal').html('&infin;');
}

function getAvaliationNumber(that, number) {

    if (!number) {
        return false;
    }

    //Se tiver online
    $.ajax({
        data: JSON.stringify({
            "Shift": $('.App').attr('shift'),
            "ParLevel1_Id": $(_level1).attr('id'),
            "ParLevel2_Id": $(that).prev().attr('id'),
            "UnitId": parseInt($('.App').attr('unidadeid')),
            "EvaluationNumber": number,
            "CollectionDate": getCollectionDateFormat().toJSON()
        }),
        url: urlPreffix + '/api/SyncServiceApi/GetLastSampleByCollectionLevel2',
        headers: token(),
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        async: false,
        success: function (data) {

            var currentSample = data;

            $(that).find('.sampleCurrentTotal').html(currentSample);
            $(that).prev().attr('samplecurrent', currentSample);

            setAvaliationAndSampleOnLvl2($(that).prev(), number, currentSample);
            setAvaliationAndSampleLvl2Line($(that).prev());

        },
        error: function () {
        }
    });

    //Se tiver offline
}

function setAvaliationAndSampleOnLvl2(level2, evaluate, sample) {

    $(level2).attr('evaluatecurrent', evaluate);
    $(level2).attr('samplecurrent', sample);

}

function setAvaliationAndSampleLvl2Line(level2) {

    //var evaluatecurrent = $(level2).attr('evaluatecurrent');
    //$(level2).next().find('.evaluateCurrent').html(evaluatecurrent);
    var evaluatecurrent = $(level2).attr('evaluate');
    if (parseInt(evaluatecurrent) == 0)
        $(level2).next().find('.evaluateCurrent').html($(level2).attr('evaluatecurrent'));
    var samplecurrent = parseInt($(level2).attr('samplecurrent'));
    $(level2).next().find('.sampleCurrentTotal').html(samplecurrent);

}