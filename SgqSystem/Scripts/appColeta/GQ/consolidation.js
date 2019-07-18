function updateLevel2Consolidation(
    level2Result,
    evaluateCurrent,
    samplecurrent,
    alertlevell1,
    weievaluationl1,
    evaluatetotall1,
    defectstotall1,
    weidefectsl1,
    totallevel3evaluationl1,
    totallevel3withdefectsl1,
    lastevaluationalertl1,
    lastlevel2alertl1,
    evaluatedresultl1,
    defectsresultl1,
    weievaluationl2,
    resultadodefeitos,
    defectsl2,
    weidefectsl2,
    totallevel3withdefectsl2,
    totallevel3withdefectsl2,
    totallevel3evaluationl2,
    sequential,
    side,
    reauditnumber,
    havereaudit,
    reauditlevel) {

    var level1 = $(_level1)

    if ($('.level3Group.BEA').is(':visible')) {
        if (level2Result.length) {
            level2Result.attr('evaluation', evaluateCurrent).attr('sample', parseInt(samplecurrent));
        } else {
            var level2Result = $("<div class='Resultlevel2' Level1Id='" + $('.level1.selected').attr('id') + "' Level2Id='" + $('.level2.selected').attr('id') + "' UnitId='" + $('.App').attr('unidadeid') + "' Shift='" + $('.App').attr('shift') + "' Period='" + $('.App').attr('period') + "' CollectionDate='" + getCollectionDate() + "' Evaluation='" + $('.painel:visible .evaluateCurrent').text() + "' Sample='" + $('.painel:visible .pecasAvaliadas').val() + "' ></div>");
            appendDevice(level2Result, $('.ResultsConsolidation'))
        }
    } else {
        if (!level2Result.length) {
            // var level2Result = $("<div class='Resultlevel2' Level1Id='" + $('.level1.selected').attr('id') + "' Level2Id='" + $('.level2.selected').attr('id') + "' UnitId='" + $('.App').attr('unidadeid') + "' Shift='" + $('.App').attr('shift') + "' Period='" + $('.App').attr('period') + "' CollectionDate='" + $('.App').attr('date') + "' Evaluation='" + $('.painel:visible .evaluateCurrent').text() + "' Sample='" + $('.painel:visible .sampleCurrent').text() + "' ></div>");
            var level2Result = $("<div class='Resultlevel2' Level1Id='" + $('.level1.selected').attr('id') + "' Level2Id='" + $('.level2.selected').attr('id') + "' UnitId='" + $('.App').attr('unidadeid') + "' Shift='" + $('.App').attr('shift') + "' Period='" + $('.App').attr('period') + "' CollectionDate='" + getCollectionDate() + "'></div>");
            appendDevice(level2Result, $('.ResultsConsolidation'));
        }

        level2Result.attr('evaluation', evaluateCurrent);
        level2Result.attr('sample', samplecurrent);
        level2Result.attr('weievaluationl2', weievaluationl2);
        level2Result.attr('defectsl2', defectsl2);
        level2Result.attr('weidefectsl2', weidefectsl2);
        level2Result.attr('totallevel3withdefectsl2', totallevel3withdefectsl2);
        level2Result.attr('totallevel3evaluationl2', totallevel3evaluationl2);
        level2Result.attr('sequential', sequential);
        level2Result.attr('side', side);
        level2Result.attr('DefectsResultL2', resultadodefeitos);

    }

    if (evaluateCurrent <= 1) {
        var horaAtual = new Date().getHours() + ":" + new Date().getMinutes();
        level2Result.attr('horaprimeiraavaliacao', horaAtual);
    }

    var level2ResultList = $('.Resultlevel2[Level1Id=' + $('.level1.selected').attr('id') + '][UnitId="' + $('.App').attr('unidadeid') + '"][Shift="' + $('.App').attr('shift') + '"][Period="' + $('.App').attr('period') + '"][CollectionDate="' + getCollectionDate() + '"]');

    level2ResultList.attr('alertlevell1', alertlevell1);
    level2ResultList.attr('weievaluationl1', weievaluationl1);
    level2ResultList.attr('evaluatetotall1', evaluatetotall1);
    level2ResultList.attr('totalavaliado', evaluatetotall1);
    level2ResultList.attr('defectstotall1', defectstotall1);
    level2ResultList.attr('weidefectsl1', weidefectsl1);
    level2ResultList.attr('totallevel3evaluationl1', totallevel3evaluationl1);
    level2ResultList.attr('totallevel3withdefectsl1', totallevel3withdefectsl1);
    level2ResultList.attr('lastevaluationalertl1', lastevaluationalertl1);
    level2ResultList.attr('lastlevel2alertl1', lastlevel2alertl1);
    level2ResultList.attr('evaluatedresultl1', evaluatedresultl1);
    level2ResultList.attr('defectsresultl1', defectsresultl1);
    if ($('.level1.selected').attr('havecorrectiveaction')) {
        level2ResultList.attr('havecorrectiveaction', 'true'); //2017.01.02 ultimo commit
    }

    level2Result.attr('havereaudit', havereaudit);
    level2Result.attr('reauditlevel', reauditlevel);
    level2Result.attr('reauditnumber', reauditnumber);

    if (level1.attr('hasgrouplevel2') == 'true' && level1.attr('isreaudit') == 'true') {
        level1.attr('lastsample', samplecurrent);
        level1.attr('lastevaluate', (evaluateCurrent - 1));
    }

}
function level2ConsolidationUpdate(level1) {

    var reaudnumber = 0;
    var reaudMax = $('.Resultlevel2[level1id=' + level1.attr('id') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + '][unitid=' +
        $('.App').attr('unidadeid') + ']:last').attr('reauditnumber')

    if (reaudMax > 0)
        reaudnumber = reaudMax;

    if (reaudnumber == 0 && (level1.attr('isreaudit') == 'true' || level1.attr('reaudminlevel') > 0)) {
        reaudnumber = 1;
        reaudMax = 1;
    }

    var seReaudLevel1 = $('.Resultlevel2[level1id=' + level1.attr('id') + '][unitid=' + $('.App').attr('unidadeid') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period')
        + '][reauditnumber=' + (reaudnumber - 1) + '][reauditlevel=1][havereaudit=true]').length;
    var level2Results = $('.Resultlevel2[level1id=' + level1.attr('id') + '][unitid=' + $('.App').attr('unidadeid') + '][reauditnumber=' + reaudnumber + ']');

    if (seReaudLevel1 > 0) {
        level2ResultsNoPeriodo = $('.Resultlevel2[level1id=' + level1.attr('id') + '][unitid=' + $('.App').attr('unidadeid') + '][reauditnumber=' + reaudMax + '][shift=' +
            $('.App').attr('shift') + '][period=' + $('.App').attr('period') + ']');
        if (level1.attr('hasgrouplevel2') == 'true') {
            var maxEv = $($('.level2')[0]).attr('evaluate');
            var maxSmp = $($('.level2')[0]).attr('sample');
            var level2ResultsNoPeriodoCFF = $('.Resultlevel2[level1id=' + level1.attr('id') + '][unitid=' + $('.App').attr('unidadeid') + '][reauditnumber=' + reaudMax + '][shift=' +
                $('.App').attr('shift') + '][period=' + $('.App').attr('period') + '][evaluation=' + maxEv + '][sample=' + maxSmp + ']');
        }
        if ((level2ResultsNoPeriodo.length == $('.level2').length) || (level1.attr('hasgrouplevel2') == 'true' && level2ResultsNoPeriodoCFF.length == ($('.level2').length - 1))) {
            reaudMax++;
            if ($('.Resultlevel2[havereaudit=true][reauditlevel=1][level1id=' + level1.attr('id') + '][unitid=' + $('.App').attr('unidadeid') + '][period=' + $('.App').attr('period') + '][shift=' + $('.App').attr('shift') + '][reauditnumber=' + (reaudMax - 1) + ']').attr('reauditnumber') > 0)
                level2Results = $('.Resultlevel2[level1id=' + level1.attr('id') + '][unitid=' + $('.App').attr('unidadeid') + '][reauditnumber=' + reaudMax + '][period=' + $('.App').attr('period') + '][shift=' + $('.App').attr('shift') + ']');
            else if ($('.Resultlevel2[reauditlevel=2][havereaudit=true][level1id=' + level1.attr('id') + '][unitid=' + $('.App').attr('unidadeid') + '][period=' + $('.App').attr('period') + '][shift=' + $('.App').attr('shift') + '][reauditnumber=' + (reaudMax - 1) + ']').attr('reauditnumber') > 0)
                level2Results = $('.Resultlevel2[havereaudit!=true][level1id=' + level1.attr('id') + '][unitid=' + $('.App').attr('unidadeid') + '][reauditnumber=' + (reaudMax - 1) + ']');
        } else if (level1.attr('isreaudit') == 'true' && level1.attr('hasgrouplevel2') == 'true') {
            level2Results = level2ResultsNoPeriodo;
        }
    }

    if (reaudMax > 0 && seReaudLevel1 == 0) {
        var count = 0;
        var estourouL1 = $('.Resultlevel2[level1id=' + level1.attr('id') + '][reauditLevel=1][havereaudit=true][period=' + $('.App').attr('period') + '][shift=' + $('.App').attr('shift') + ']:last').length;
        var ultQueEstorouL1 = $('.Resultlevel2[level1id=' + level1.attr('id') + '][reauditLevel=1][havereaudit=true][period=' + $('.App').attr('period') + '][shift=' + $('.App').attr('shift') + ']:last').attr('reauditnumber');
        if (ultQueEstorouL1 > 0) {
            if (ultQueEstorouL1.length > 0)
                count = parseInt(ultQueEstorouL1) + 1;
        } else if (estourouL1 > 0) {
            count++;
        }
        level2Results = $('.Resultlevel2[level1id=' + level1.attr('id') + '][unitid=' + $('.App').attr('unidadeid') +
            '][havereaudit!=true][reauditLevel!=1][reauditnumber=' + count + '][period=' + $('.App').attr('period') + '][shift=' +
            $('.App').attr('shift') + ']');
        count++;
        while (count <= reaudMax) {
            var elemNaReaudit = $('.Resultlevel2[level1id=' + level1.attr('id') + '][unitid=' + $('.App').attr('unidadeid') + '][havereaudit!=true][reauditnumber=' +
                count + '][period=' + $('.App').attr('period') + '][shift=' + $('.App').attr('shift') + ']')
                .each(function (c, o) {
                    level2Results.push(o);
                });
            count++;
        }
    }

    try {
        if (arguments.callee.caller.caller) {
            if (arguments.callee.caller.caller.caller.name == 'saveResultLevel3') {
                var UltRNumber = $('.Resultlevel2[level1id=' + level1.attr('id') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + ']:last').attr('reauditnumber');
                if (UltRNumber > reaudnumber) {
                    $('.Resultlevel2[level1id=' + level1.attr('id') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + '][reauditnumber=' + UltRNumber + ']')
                        .each(
                            function (c, o) {
                                level2Results.push($(o));
                            }
                        );
                }
            }
        }
    } catch (err) {
    }

    if (level1.attr('hasgrouplevel2') == 'true' && level2Results.length == 0 && level1.attr('isreaudit') == 'true') {
        level1.attr('lastsample', '0');
        level1.attr('lastevaluate', '0');
        level1.removeAttr('hascompleteevaluation');
    }

    var level1S = level1;

    level2Results.each(function (e) {

        var level1id = $(this).attr('level1id');
        var level2id = $(this).attr('level2id');

        var unitid = $(this).attr('unitid');
        var shift = $(this).attr('shift');
        var period = $(this).attr('period');
        var collectiondate = $(this).attr('collectiondate');
        var evaluateCurrent = parseInt($(this).attr('evaluation'));
        var sampleCurrent = parseInt($(this).attr('sample'));

        var level1 = $('.level1[id=' + level1id + ']');
        var level2 = $('.level2Group[level01id=' + level1id + '] .level2[id=' + level2id + ']');

        if ($(level2).attr('parfrequency_id') == '1'
            && (parseInt($(this).attr('period')) != parseInt($('.App').attr('period'))
                || (parseInt($(this).attr('period')) == parseInt($('.App').attr('period'))
                    && parseInt($(this).attr('shift')) != parseInt($('.App').attr('shift'))))) { //filtro por periodo
            return;
        }

        if (level1.attr('hasgrouplevel2') == "true" && $(level1).attr('parfrequency_id') == '1'
            && (parseInt($(this).attr('period')) != parseInt($('.App').attr('period'))
                || (parseInt($(this).attr('period')) == parseInt($('.App').attr('period'))
                    && parseInt($(this).attr('shift')) != parseInt($('.App').attr('shift'))))) { //filtro por periodo
            return;
        }

        if ($(level2).attr('parfrequency_id') == '3' && collectiondate != getCollectionDate()) {
            return;
        }


        if (parseInt($(this).attr('shift')) != parseInt($('.App').attr('shift'))) { //filtro por turno
            return;
        }

        if (level1.attr('hasgrouplevel2') == "true") {
            level2 = $('.level3Group[level1idgroup=' + level1id + '] .level2[id=' + level2id + ']');
        }

        if ($(this).attr('havecorrectiveaction') == 'true') {
            level1.attr('havecorrectiveaction', 'true');
        }
        //contadores level1
        //alertlevell1
        level1.attr('alertaatual', $(this).attr('alertlevell1'));
        level1.attr('weievaluation', $(this).attr('weievaluationl1'));
        level1.attr('evaluatetotal', $(this).attr('evaluatetotall1'));
        level1.attr('defectstotal', $(this).attr('defectstotall1'));

        if (level1.attr('hasgrouplevel2') == 'true') {
            var reaudnumber = 0;
            if (isNaN(level1.attr('reauditnumber')) == false)
                reaudnumber = level1.attr('reauditnumber');
            if (level1.attr('isreaudit') == 'true' && reaudnumber == 0)
                reaudnumber++;
            var resultsSampleAnterior = parseInt($('.Resultlevel2[level1id=' + level1.attr('id') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period')
                + '][reauditnumber=' + reaudnumber + ']').length);
            if (level1.attr('havereaudit') == 'true' && level1.attr('havecorrectiveaction') != 'true' && resultsSampleAnterior == 0 &&
                level1.attr('isreaudit') == 'true') {
                level1.attr('lastevaluate', '0');
                level1.attr('lastsample', '0');
                level1.attr('hascompleteevaluation', 'false');
            } else {
                //if(level1.attr('havereaudit') != 'true' && level1.attr('isreaudit') == 'true') {
                if (level1.attr('lastevaluate') == $(this).attr('evaluation')) {
                    if (level1.attr('lastsample') < $(this).attr('sample'))
                        level1.attr('lastsample', $(this).attr('sample'));
                } else if ($(this).attr('sample') == level2.attr('sample')) {
                    level1.attr('lastsample', $(this).attr('sample'));
                    level1.attr('lastevaluate', $(this).attr('evaluation'));
                } else {
                    level1.attr('lastsample', $(this).attr('sample'));
                    level1.attr('lastevaluate', $(this).attr('evaluation'));
                }
            }
        }

        if (level1.attr('parconsolidationtype_id') == 3) {

            level1.attr('totaldefeitos', $(this).attr('defectsresultl1'));
            level1.attr('totalavaliado', $(this).attr('evatuatedresultl1'));

        } else {

            level1.attr('totaldefeitos', $(this).attr('weidefectsl1'));
            level1.attr('totalavaliado', $(this).attr('weievaluationl1'));
        }

        level1.attr('weidefects', $(this).attr('weidefectsl1'));
        level1.attr('totallevel3evaluation', $(this).attr('totallevel3evaluationl1'));
        level1.attr('totallevel3withdefects', $(this).attr('totallevel3withdefectsl1'));
        level1.attr('totallevel3withdefects', $(this).attr('totallevel3withdefectsl1'));

        level1.attr('avaliacaoultimoalerta', $(this).attr('lastevaluationalertl1'));
        level1.attr('monitoramentoultimoalerta', $(this).attr('lastlevel2alertl1'));

        level1.attr('resultadodefeitos', $(this).attr('defectsresultl1'))

        //contadores level2
        level2.attr('weievaluation', $(this).attr('weievaluationl2'));
        level2.attr('evaluatetotal', $(this).attr('totallevel3evaluationl2'));
        level2.attr('defectstotal', $(this).attr('defectsl2'));
        level2.attr('weidefects', $(this).attr('weidefectsl2'));
        level2.attr('totallevel3evaluation', $(this).attr('totallevel3evaluationl2'));
        level2.attr('totalavaliacoes', $(this).attr('evaluatetotall2'));
        level2.attr('totalavaliacoesponderadas', $(this).attr('weievaluationl2'));
        level2.attr('totaldefeitos', $(this).attr('defectstotall2'));

        level2.attr('totaldefeitosponderados', $(this).attr('weidefectsl2'));
        level2.attr('totallevel3avaliados', $(this).attr('totallevel3evaluationl2'));
        level2.attr('totallevel3comdefeitos', $(this).attr('totallevel3withdefectsl2'));
        level2.attr('resultadoavaliado', $(this).attr('evaluatedresultl2'));
        level2.attr('resultadodefeitos', $(this).attr('defectsresultl2'));
        level2.attr('sequential', $(this).attr('sequential'));
        level2.attr('side', $(this).attr('side'));
        level2.attr('defects', $(this).attr('defectsl2'));
        level2.attr('defectscurrentevaluation', 0);

        level2.attr('phase', $(this).attr('phase'));
        level2.attr('startphasedate', $(this).attr('startphasedate'));


        level2.siblings('.counters').find('.defectstotal:first').text(parseInt($(this).attr('defectsl2')));

        var evaluateTotal = parseInt(level2.attr('evaluate'));
        var sampleTotal = parseInt(level2.attr('sample'));
        var defects = parseFloat(level2.attr('defects'));

        var level3Group = $('.level3Group[level1id=' + level1.attr('id') + '][level2id=' + level2.attr('id') + ']');

        var partialSave = false;
        if (level1.attr('ispartialsave') == "true") {
            var level3PartialQtde = $('.Resultlevel2[level1id=' + level1.attr('id') + '][level2id=' + level2.attr('id') + '][collectiondate="' + getCollectionDate() + '"]').children('.r3l2').length;
            var totalInpts = level3Group.find('.level3').find('input').length;

            if (level3PartialQtde < totalInpts) {
                partialSave = true;
            }
        }

        if (level1.attr('hasgrouplevel2') == 'true' && level1.attr('isreaudit') == 'true') {
            sampleCurrent = level1.attr('lastsample');
            evaluateCurrent = level1.attr('lastevaluate');
        }

        if (sampleTotal != 0 && sampleCurrent >= sampleTotal && partialSave != true) {
            sampleCurrent = 0;
            evaluateCurrent++;
        }
        else if (sampleTotal != 0 && partialSave == true) {
            evaluateCurrent = parseInt($(this).attr('evaluation'));
            sampleCurrent = parseInt($(this).attr('sample'));
        }

        completeLevel2(level2, evaluateCurrent, evaluateTotal);

        var isInfinityAv = ($(level2).attr('evaluate') == 0 && $(level2).attr('sample') == 0)

        if (!isInfinityAv)
            updateEvaluateSample(level2, level3Group, evaluateCurrent, sampleCurrent, defects);

        if ($(this).attr('havereaudit') == "true") {
            if ($(this).attr('reauditlevel') == "1") {
                level1.attr('havereaudit', 'true');
                /*var reauditnumber = parseInt($(this).attr('reauditnumber')) + 1;
                level1.attr('reauditnumber', reauditnumber);*/
            }
            else if ($(this).attr('reauditlevel') == "2") {
                level2.attr('havereaudit', 'true');
                var reauditnumber = parseInt($(this).attr('reauditnumber')) + 1;
                level2.attr('reauditnumber', reauditnumber);
            }
        }
    });

}
function level2ConsolidationReset() {
    $('.level1')
        .removeAttr('havecorrectiveaction')
        .removeAttr('alertaatual')
        .removeAttr('weievaluation')
        .removeAttr('evaluatetotal')
        .removeAttr('defectstotal')
        .removeAttr('lastevaluate')
        .removeAttr('lastsample')
        .removeAttr('resultadodefeitos')
        .removeAttr('havereaudit')
        .removeAttr('reauditlevel')
        .removeAttr('reauditnumber')
        .removeAttr('reaudminlevel')
        .removeAttr('isreaudit')
        .attr('totaldefeitos', '0')
        .attr('defectsresultl1', '0');
    $('.level2')
        .removeAttr('weievaluation')
        .removeAttr('evaluatetotal')
        .removeAttr('defectstotal')
        .removeAttr('weidefects')
        .removeAttr('totallevel3evaluation')
        .removeAttr('totalavaliacoes')
        .removeAttr('totalavaliacoesponderadas')
        .removeAttr('totaldefeitos')
        .removeAttr('totaldefeitosponderados')
        .removeAttr('totallevel3avaliados')
        .removeAttr('totallevel3comdefeitos')
        .removeAttr('resultadoavaliado')
        .removeAttr('resultadodefeitos')
        .removeAttr('sequential')
        .removeAttr('side')
        .removeAttr('defects')
        .removeAttr('defectscurrentevaluation')
        .removeAttr('phase')
        .removeAttr('startphasedate')
        .removeAttr('havereaudit')
        .removeAttr('reauditlevel')
        .removeAttr('reauditnumber')
        .removeAttr('evaluatecurrent')
        .removeAttr('samplecurrent')
        .removeAttr('completed')
        .removeAttr('nclocal');

    $('.level2').parents('li').removeClass('bgLimitExceeded').removeClass('bgCompleted');

    $('.btnReaudit').addClass('hide');

    $('.level2').siblings('.counters').children('.evaluateCurrent').text(0);
    $('.level2').siblings('.counters').children('.sampleCurrent').text(0);

}
function realmenteConsolidationResult() {
    realTimeConsolidationUpdate($('.level1.selected'));
}

function realTimeConsolidationUpdate(level1) {
    var haverealtimeconsolidation = level1.attr('haverealtimeconsolidation');
    if (haverealtimeconsolidation == "true") {
        var realtimeconsolitationupdate = level1.attr('realtimeconsolitationupdate') * 10000;
        updateConsolidation(level1, parseInt(realtimeconsolitationupdate))
    }
    else {
        level2ConsolidationUpdate(level1);
    }
}

var realtimeConsolidation;
function updateConsolidation(Level1, updateTime) {
    if (updateTime == 0 || updateTime == undefined) {
        updateTime = 30000;
    }
    if (Level1 != undefined) {
        var resultadosNaoSincronizados = $('.level01Result[level01id=' + Level1.attr('id') + '] .level02Result[sync=false]');

        //Se não existem informações para ser enviadas do Level atual
        if (!resultadosNaoSincronizados.length) {
            clearTimeout(realtimeConsolidation);
            recivingDataByLevel1(Level1);
        }

    }

    realtimeConsolidation = setTimeout(function (e) {
        updateConsolidation(Level1, updateTime);
    }, updateTime);
}

function readFileConsolidation(fileEntry) {
    fileEntry.file(function (file) {
        var reader = new FileReader();
        reader.onloadend = function () {
            if (this.result) {
                $('.ResultsConsolidation').empty();
                appendDevice($(this.result), $('.ResultsConsolidation'));
                cleanResults();
            }
        };
        reader.readAsText(file);
    }, onErrorReadFile);
}

//metodo que grava o resultado do html no bnco local
function createFileResult() {
    _writeFile("database.txt", $('.Results').html());
    createFileResultConsolidation();
}
//metodo padrao que grava dados no arquivo
//metodo que grava o resultado do html no bnco local
function createFileResultConsolidation() {
    _writeFile("databaseConsolidation.txt", $('.ResultsConsolidation').html());
}

function updateConsolidationCollapse(level2Result, sample, evaluate, reauditnumber) {

    if (!level2Result.length) {
        var level2Result = $("<div class='Resultlevel2' Level1Id='" + $('.level1.selected').attr('id') + "' Level2Id='" + $('.level2.selected').attr('id') +
            "' UnitId='" + $('.App').attr('unidadeid') + "' Shift='" + $('.App').attr('shift') + "' Period='" + $('.App').attr('period') + "' CollectionDate='" +
            getCollectionDate() + "' Evaluation='" + evaluate + "' Sample='" + $('.painel:visible .pecasAvaliadas').val() + "' ></div>");
        appendDevice(level2Result, $('.ResultsConsolidation'))
    }

    level2Result.attr('defectsl2', tdef.text());
    level2Result.attr('reauditnumber', reauditnumber);

}