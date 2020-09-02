function openLevel2(level1) {

    tempHDL2 = 0;

    if ($('.level2Group').length == 0) {
        openMessageModal(getResource("warning"), getResource("no_families_level"));
        return;
    }

    if (parseInt($(level1).attr('volumealertaindicador')) == 0 && !$(level1).hasClass('VF') && $(level1).attr('hasalert') == "true") {
        openMessageModal(getResource("warning"), getResource("no_volumn_level"));
        return;
    }

    var periodo = parseInt($('.App').attr('period'));

    if (periodo > 1) {
        periodo--;
        var rn = getReauditTempPeriodo(periodo)

        var total = $('.Resultlevel2[level1id=' + $(level1).attr('id') + '][shift=' + $('.App').attr('shift') +
            '][havereaudit=true][reauditnumber=' + rn + '][period=' + periodo + ']').length;
        /*
                if (total > 0) {
                    openMessageModal(getResource("warning"), getResource("reaudit_period"));
                    return;
                }
        */
    }

    //Oculta Lista de Level1
    var level1List = level1.parents('.level1List');
    $('#btnSave').addClass('hide');

    realTimeConsolidationUpdate(level1);
    level1List.hide();

    //Instancia todos os monitoramentos
    var level2List = $('.level2List');
    //Instancia o grupo de Level2 referente ao Level1 selecionado
    var level2Group = level2List.children('.level2Group[level01id=' + level1.attr('id') + ']');
    level2Group.removeClass('hide');
    //Mostra Icone de retorno
    $('.iconReturn').removeClass('hide');

    $('.level3List').hide().addClass('hide');

    //BreadCrumb
    breadCrumb($('.level1List .level1.selected').text());

    var minEvaluateCurrent = null;
    var avaliacao = 0;
    var _level2List;

    //if (_level2 && ultL2Temp == false) {
    //    _level2List = level2Group.find('.level2[id=' + _level2.id + ']').length;
    //} else {
        _level2List = level2Group.find('.level2').length;
    //}

    var maior = 0;
    var totalDeAvaliacoes = 0;

    for (var i = 0; i < _level2List; i++) {

        var level2;
        //if (_level2 && level1.attr('hasgrouplevel2') != 'true' && ultL2Temp == false)
        //    level2 = level2Group.find('.level2[id=' + _level2.id + ']');
        //else
            level2 = level2Group.find($('.level2')[i]);

        updateCounterLinhaLevel2(level1, level2);

        setAvaliationClick(level2);

        setAvaliationLevel2(level2);

        setSampleLevel2(level2);

        if (parseInt($(level2).attr('evaluatecurrent')))
            setAvaliationAndSampleLvl2Line(level2);

        level2.removeAttr('isreaudit');

        var defLineL2;
        if (level1.attr('editlevel2') == "true")
            defLineL2 = updateDefLineL2(level1.attr('id'), level2.attr('id'));

        if (defLineL2 == 0) {
            if ($('.Resultlevel2[level1id=' + level1.attr('id') + '][level2id=' + level2.attr('id') + '][period=' + $('.App').attr('period') + '][shift=' +
                $('.App').attr('shift') + '][reauditnumber=' + getReauditTemp() + ']').attr('defectsl2') > 0)
                defLineL2 = parseInt($('.Resultlevel2[level1id=' + level1.attr('id') + '][level2id=' + level2.attr('id') + '][period=' + $('.App').attr('period') + '][shift=' +
                    $('.App').attr('shift') + '][reauditnumber=' + getReauditTemp() + ']').attr('defectsl2'));
        }

        level2.parent().find('.counters .defectstotal').text(defLineL2);

        if (level2.attr('havereaudit') == "true") {
            level2.parents('.row.list-group-item').addClass('bgLimitExceeded');
        } else {
            level2.parents('.row.list-group-item').removeClass('bgLimitExceeded');
        }

        if (level1.attr('phase1')) {
            level2.children('.levelName').text($(level2).children('.levelName').text().split(' (' + getResource('phase'))[0] + getPhaseLevel2(level1, level2));
        }

        if ($('.level1.selected.CFF').length > 0) {
            if (level1.attr('havereaudit') == 'true' && $('.level1.selected.CFF').attr('isreaudit') != "true") {
                level2.parents('.row.list-group-item').addClass('lightred');
            } else {
                level2.parents('.row.list-group-item').removeClass('lightred');
            }
            if (isNaN(tdefAv) == false) {
                $('.level2Group li .counters .SmpDefects').text(tdefAv.text());
            }
        } else {
            $('.level2Group li[id=' + level1.attr('id') + '] .counters .SmpDefects').addClass('hide')
        }

        if (level2.attr('evaluatecurrent') > level2.attr('evaluate')) {
            level2.parent().find('.na').attr('disabled', 'disabled')
            level2.parent().find('.btnAreaSave').attr('disabled', 'disabled');
            maior = parseInt(level2.attr('evaluatecurrent'));
        }

        if (totalDeAvaliacoes < level2.attr('evaluate')) {
            totalDeAvaliacoes = level2.attr('evaluate');
        }

        //alertas
        if (parseInt($(_level1).attr('avaliacaoultimoalerta')) == parseInt(level2.attr('evaluatecurrent')) - 1) {

            var numerador = 1;

            if ($(_level1).attr('defectstotal') && $(_level1).attr('alertanivel1'))
                numerador += parseFloat($(_level1).attr('defectstotal').replace(',', '.')) / parseFloat($(_level1).attr('alertanivel1').replace(',', '.'));

            $(_level1).attr('metatolerancia', parseFloat($(_level1).attr('alertanivel1').replace(',', '.')) * numerador);
        }
        //fim alertas
    };

    //Mostra a tela do monitoramento
    level2List.removeClass('hide').show();

    level1.attr('numeroAvaliacoes', totalDeAvaliacoes);
    var metaAvaliacao = parseFloat(level1.attr('metadia').replace(',', '.')) / parseFloat(level1.attr('numeroavaliacoes').replace(',', '.'));
    level1.attr('metaavaliacao', metaAvaliacao);

    if (level1.attr('hasgrouplevel2') != 'true') {
        $('.painelLevel02 .form-group .counter[indicador=' + level1.attr('id') + '][headerlevel=level2_header]').removeClass('hide');
        if ($('.counter[indicador=' + level1.attr('id') + '][level=1][headerlevel=level2_header][counter=defects]').length > 0) {
            $('.painelLevel02 .form-group .counter[indicador=52][headerlevel=level2_header] .labelCounter').text('Total Defects')
        }
    }

    if ($('.counter[headerlevel=level2_line][counter=evaluation]').length == 0) {
        $($('.headerCounter .col-xs-3 b')[0]).addClass('hide')
    }

    if ($('.counter[headerlevel=level2_line][counter=sample]').length == 0) {
        $($('.headerCounter .col-xs-3 b')[1]).addClass('hide')
    }

    if ($('.counter[headerlevel=level2_line][counter=defects]').length == 0 && isEUA == false) {
        $($('.headerCounter .col-xs-3 b')[2]).addClass('hide')
    }

    if ($('.counter[headerlevel=level2_line][counter=frequency]').length == 0) {
        $($('.headerCounter .col-xs-3 b')[3]).addClass('hide')
    }

    $('#period').attr('disabled', 'disabled');

    $('#btnShowImage').remove();
    if ($('.level1.selected .levelName').text() == "Carcass Contamination Audit" && $('.level2Group:visible').length > 0) {
        var btnImagemBoi = $('<span id="btnShowImage" style="cursor: pointer; background-color: gray; margin-left: 16px; padding: 14px;">Show image</span>');
        appendDevice(btnImagemBoi, $('.navbar-header .shift'));
    }

    GetUpdateCounter();

    if (arguments.callee.caller.name != "saveResultLevel3" && level1.attr('editlevel2') == "true")
        var defsHL2 = updateDefHeaderL2(level1.attr('id'))
    else
        var defsHL2 = 0;

    $('.Resultlevel2[level1id=' + level1.attr('id') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + '][reauditnumber=' + getReauditTemp() +
        ']').each(function (index, self) {
            defsHL2 += parseInt($(self).attr('defectsl2'));
        });

    $('.level2Group .painelLevel02 .form-group .counter[counter=defects] .value').text(defsHL2);
    tempHDL2 += defsHL2;

    $('.level2Group .reauditFlag').addClass('hide');

    if (level1.attr('isreaudit') == 'true' || level1.attr('reaudminlevel') > 0) {
        $('.level2Group[level01id=' + level1.attr('id') + '] .reauditFlag').removeClass('hide');
    }

    if (level1.attr('editlevel2') == 'true') {
        updateTempSave(level1);
    }

    if ($('.level1.selected.VF').length > 0) {
        $('.sampleCurrentTotal').text($('.VerificacaoTipificacao div[verificacaotipificacaochave*="' + yyyyMMdd() + '"][idunidade=' + $('.App').attr('unidadeid') + ']').length);
    }

    if (level1.attr('havecorrectiveaction') == "true" && $('#local[empresa=jbs][local=brasil]').length > 0) {
        $('#btnCA').removeClass('hide');
    } else {
        $('#btnCA').addClass('hide');
    }

    $('.level2.selected').removeClass('selected');
    setEvaluationByHeadersSelection();

    setupRetrocesso();

    headerWithCheckbox();

    ReauditByHeader.SetupReaudit(level1.attr('id'));

    atualizaCorAgendamento();

    setTimeout(function () {
        //Instancia todos os monitoramentos
        var level2List = $('.level2List');
        //Instancia o grupo de Level2 referente ao Level1 selecionado
        var level2Group = level2List.children('.level2Group[level01id=' + level1.attr('id') + ']');

        var _level2List;
        _level2List = level2Group.find('.level2').length;

        for (var i = 0; i < _level2List; i++) {

            var level2;
            //if (_level2 && level1.attr('hasgrouplevel2') != 'true' && ultL2Temp == false)
            //    level2 = level2Group.find('.level2[id=' + _level2.id + ']');
            //else
            level2 = level2Group.find($('.level2')[i]);

            var linha = level2.parent().find('a');

            if (!!$(linha).attr('parlevel1_id_group'))
                continue;

            var avaliacaoTotal = parseInt(linha.attr('evaluate'));
            var amostraTotal = parseInt(linha.attr('sample'));

            var avaliacao = parseInt(linha.attr('evaluatecurrent'));
            var amostra = parseInt(linha.attr('samplecurrent'));

            var avaliacaoAtual = 0;
            var amostraAtual = 0;

            if (avaliacao > avaliacaoTotal) {
                avaliacaoAtual = avaliacaoTotal;

                if (amostraTotal > 0) {
                    amostraAtual = avaliacaoAtual * amostraTotal;
                }
            } else {
                if (amostra == undefined || avaliacao == undefined) {
                    avaliacaoAtual = parseInt($('.Resultlevel2[level2id=' + linha.attr('id') + '][level1id=' + _level1.id + ']:last').attr('evaluation'));
                    amostraAtual = ((avaliacaoAtual - 1) * amostraTotal) + parseInt(RetornaValor0SeUndefined(parseInt($('.Resultlevel2[level2id=' + linha.attr('id') + '][level1id=' + _level1.id + ']:last').attr('sample'))));
                } else {
                    avaliacaoAtual = RetornaValor0SeUndefined(avaliacao) > 0 ? RetornaValor0SeUndefined(avaliacao) : 1;
                    amostraAtual = ((avaliacaoAtual - 1) * amostraTotal) + parseInt(RetornaValor0SeUndefined(amostra));
                }
            }

            if ($(level1).hasClass("VF")) {
                amostraAtual = $('.ResultsKeysVF div[date="' + getCollectionDate() + '"][unidadeid=' + $('.App').attr('unidadeid') + ']').length + 1;
                if (amostraAtual != amostraTotal)
                    amostraAtual -= 1;
            }

            avaliacaoAtual = isNaN(avaliacaoAtual) ? 0 : avaliacaoAtual;
            amostraAtual = isNaN(amostraAtual) ? 0 : amostraAtual;

            //var proximaAvaliacao = ((amostraAtual / parseInt(amostraTotal)) % 1 == 0) ? 1 : 0;
            //var avaliacaoColetaAtual = Math.ceil(amostraAtual / parseInt(amostraTotal)) + proximaAvaliacao;

            var infinito = '∞';
            var proximaAvaliacao = "";
            var avaliacaoColetaAtual = "";

            if ((amostraAtual / parseInt(amostraTotal)) != Infinity)
                proximaAvaliacao = ((amostraAtual / parseInt(amostraTotal)) % 1 == 0) ? 1 : 0;
            else
                proximaAvaliacao = infinito;

            if (Math.ceil(amostraAtual / parseInt(amostraTotal)) != Infinity)
                avaliacaoColetaAtual = Math.ceil(amostraAtual / parseInt(amostraTotal)) + proximaAvaliacao;
            else
                avaliacaoColetaAtual = avaliacaoAtual;

            if (!(level1.attr('islimitedevaluetionnumber') == "false")) {
                if (avaliacaoColetaAtual != infinito && avaliacaoColetaAtual > 0) {
                    level2.attr('evaluatecurrent', avaliacaoColetaAtual);
                } else {
                    level2.parent().find('.evaluateCurrent').html(Math.ceil(amostraAtual / parseInt(amostraTotal))); //coloca valor na Avaliação
                }
            }

            level2.parent().find('.sampleCurrentTotal').html(amostraAtual);

            if (avaliacaoTotal > 0) {
                level2.parent().find('.sampleXEvaluateTotal').html(avaliacaoTotal * amostraTotal == 0 ? infinito : avaliacaoTotal * amostraTotal);
            } else {
                level2.parent().find('.sampleXEvaluateTotal').html(amostraTotal);

            }

        }

        criarFiltroDeFrequencia();
    }, 100);

    if ($('.level2Group .level2:visible').length == 1 && $('.App').attr('desdobrar-automatico') == "true") {
        $('.level2Group .level2:visible').trigger('click');
    }
}

$(document).on('click', '.level2Group .level2', function (e) {

    _level2 = this;
    //Verifica os cabeçalhos obrigatórios antes de abrir o nível 3
    if (validHeader()) {

        //É avaliação infinita
        var isInfinityAvaliation = !!(parseInt($(this).attr('evaluate')) == 0);

        //Se a av for 0, força inserir um número de Avaliação
        if (isInfinityAvaliation && !parseInt($(this).attr('evaluatecurrent'))) {

            if ($(this).next().hasClass('changeAvNumber')) {
                $(this).next().trigger('click');
            }

            return;
        }

        if (!isInfinityAvaliation &&
            $('.level1.selected').attr('islimitedevaluetionnumber') == "true" &&
            (parseInt($(this).attr('evaluatecurrent')) > parseInt($(this).attr('evaluate')) || $('.level1.selected').attr('hasgrouplevel2') == "true" && parseInt($('.level1.selected').attr('lastevaluate')) > parseInt($(this).attr('evaluate'))) &&
            $(this).attr('isreaudit') != "true" &&
            $('.level1.selected').attr('isreaudit') != "true") {

            openMessageModal(getResource('warning'), getResource('number_of_evaluation_completed'));

            return;
        }

        if (isInfinityAvaliation && $('.level1.selected.VF').length > 0
            && parseInt($('.level2Group:visible .sampleCurrentTotal').text()) >= parseInt($(this).attr('sample'))) {
            openMessageModal(getResource('warning'), getResource('number_of_evaluation_completed'));
            return;
        }

        //Reseta o Level2 selecionado
        $('.level2').removeClass('selected');
        //Instancia o Objeto de Level2
        var level2 = $(this);
        //Define o Level2 selecionado
        level2.addClass('selected');

        //getMultipleList($('.level3Group[level1id='+$('.level1.selected').attr('id')+'][level2id='+$('.level2.selected').attr('id')+'] .painelLevel03 .header select[parfieldtype_id=1] option'));
        $.grep(listHeaders, function (o, i) {
            if (o.level1id == $('.level1.selected').attr('id'))
                listHeadersTemp = o.list;
        });

        //Abre o Level3
        openLevel3(level2);

    }

    _.groupBy(headerResultList, 'ReauditNumber');
});

function confirmButtonLevel02Hide() {
    if ($('.btnAreaSaveConfirm').is(':visible')) {
        $('.overlay').hide();
        $('.btnAreaSaveConfirm').addClass('hide').siblings('.btnAreaSave').removeClass('hide');
    }
}

function level02Complete(level02) {

    level02.each(function (e) {

        var level = $(this);
        var level01 = $('.level01[id=' + level.parents('level02Group').attr('level01id') + ']');

        level.attr('completed', 'completed');
        level.parents('li').addClass('bgCompleted');

        var botaoNa = level.parents('.row').children('.userInfo').children('div').children('.na');
        var botaoSalvarLevel02 = level.parents('.row').children('.userInfo').children('div').children('.btnAreaSave');
        var btnSalvarLevel02Confirm = level.parents('.row').children('.userInfo').children('div').children('.btnAreaSaveConfirm');
        var iconCompleto = level.parents('.row').children('.level02').children('.icons').children('.areaComplete');
        var iconNaoCompleto = level.parents('.row').children('.level02').children('.icons').children('.areaNotComplete');

        iconCompleto.removeClass('hide');
        iconNaoCompleto.addClass('hide');
        botaoNa.addClass('hide');
        botaoSalvarLevel02.addClass('hide').removeAttr('disabled');
        btnSalvarLevel02Confirm.addClass('hide');
        if (level02.is(':visible')) {
            $('#btnSave').addClass('hide');
        }
        //level02ButtonSave(level.parents('.level02Group'));
        if (level01.attr('havephases')) {
            if (level02.attr('completed') && level02.attr('limitexceeded')) {
                botaoSalvarLevel02.removeClass('hide');
                botaoNa.removeClass('hide');
            }
        }

        if (level01)
            //nao precisamos verificar os defeitos se o limitexceeded estiver
            var defectsLevel02 = parseInt($('.painelLevel03 .defects').text());
        var defectsLimit = parseInt(level.attr('levelerrorlimit'));
    });

}

function level02Level03Reset(level02Group) {


    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .defects').text('0');

    $('.level03Group[level01id=' + level02Group.attr('level01id') + '] .level03').each(function (e) {

        $('.level02Group[level01id=' + level02Group.attr('level01id') + '] .level02').removeAttr('level03' + $(this).attr('id'));
    });

    $('.level03Group[level01id=' + level02Group.attr('level01id') + '] .level03').removeClass('lightred').removeAttr('notconform');
    $('.level03Group[level01id=' + level02Group.attr('level01id') + '] .level03 .row div span.response').attr('value', '0').text('Yes');
    $('.level03Group[level01id=' + level02Group.attr('level01id') + '] .level03 textarea').val('');
}

function level02Reset(level02) {

    var level02Group = level02.parents('.level02Group');

    level02Level03Reset(level02Group);

    //CCA
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .totalDefects').text('0');
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .defects').text('0');
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] #selectCattleType').val('0');
    $('#inputChainSpeed').val("");
    $('#inputLotNumber').val("");
    $('#inputMudScore').val("");
    $('#biasedUnbiased').val('0');
    $('.btnAreaSave').removeAttr('disabled');
    $('.btnAreaSaveConfirm').addClass('hide');

    // $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] #inputChainSpeed, .labelPainel[level01id=' + level02Group.attr('level01id') + '] #inputLotNumber, .labelPainel[level01id=' + level02Group.attr('level01id') + '] #inputMudScore').val("");
    $('.painel .labelPainel').removeClass('red');;
    //CFF
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .setsDone').text('0');
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .sideWithErrors').text('0').parents('.labelPainel').removeClass('red');
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .more3Defects').text('0').parents('.labelPainel').removeClass('red');
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .setAtual').text('1');
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .sideAtual').text('1');
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .sideErros').text('0');

    $('.btnNotAvaliable').removeClass('naSelected');
    $('span.auditReaudit').children('.name').text('Audit').siblings('.reauditPeriod').text("");

    $('.level02').siblings('.userInfo').children('div').children('.consecutiveFailure, .defects').text('0');
    level02Group.children('li').children('.row').children('.level02').removeAttr('havecorrectiveaction').removeAttr('limitexceeded').removeAttr('havereaudit');


    level02.removeAttr('completed').removeAttr('reaudit').removeAttr('havereaudit').removeAttr('notavaliable').removeAttr('limitexceeded').attr('defects', '0').parents('.row').children('.userInfo').children('div').children('.defects').text(level02.attr('defects'));
    level02.parents('li').removeClass('bgCompleted').removeClass('bgLimitExceeded').removeClass('bgNoAvaliable');

    var botaoNa = level02.parents('.row').children('.userInfo').children('div').children('.na');
    var botaoSalvarLevel02 = level02.parents('.row').children('.userInfo').children('div').children('.btnAreaSave');
    var iconCompleto = level02.children('.icons').children('.areaComplete');
    var iconNaoCompleto = level02.children('.icons').children('.areaNotComplete');

    botaoNa.removeClass('hide');
    botaoSalvarLevel02.removeClass('hide').removeClass('disabled');
    iconCompleto.addClass('hide');
    iconNaoCompleto.removeClass('hide');
    $('#btnSave, .btnCA').addClass('hide');


}

//param: id do level02
function scrollClick(level02) {
    if ($('#' + level02).length > 0) {
        //pego a posicao do id para colocar para colocar no meio da tela
        var value = $('#' + level02).offset().top - ($(window).height() / 2);

        var body = $('body');
        if (device.platform == "windows")
            body = $('html');

        body.animate({
            scrollTop: value
        }, 'fast');

    }

};

function scrollTop() {

    //pego a posicao do id para colocar para colocar no inicio da tela
    //var value = $('#' + level02).offset().top - ($(window).height() / 2);

    var body = $('body');
    if (device.platform == "windows")
        body = $('html');

    body.animate({
        scrollTop: 0
    }, 'fast');

}

function saveLevel02(Level01Id, Level02Id, unidadeId, date, dateTime, auditorId, shift, period, evaluate, sample, defects, reaudit, reauditNumber, phase, startPhaseDate,
    consecutivefailureLevel, consecutivefailureTotal, notAvaliabled, headerList, alertlevel, sequencial, banda) {

    Level01Id = Level01Id ? Level01Id : $(_level1).attr('id');

    if (notAvaliabled == "notavaliable") {
        notAvaliabled = true;
    }
    else {
        notAvaliabled = false
    }

    if (reaudit != true) {
        reaudit = false
    }

    reauditNumber = reauditNumber ? reauditNumber : 0;
    phase = phase ? phase : 0;
    evaluate = evaluate ? evaluate : 1;
    sample = sample ? sample : 1;
    sequencial = sequencial != null ? sequencial : 0
    banda = banda != null ? banda : 0

    //Comentado pois esta dando problema no BR, validar USA posteriormente
    // if ($('.totalnc:visible').parent().parent().parent().length == 0) {
    //     reaudit = true;
    // }


    return "<div class='level02Result' level01Id='" + Level01Id + "' level02Id='" + Level02Id + "' unidadeId='" + unidadeId + "' date='" + date + "' dateTime='" + dateTime + "' auditorId='" + auditorId
        + "' shift='" + shift + "' period='" + period + "' defects='" + defects + "' reaudit='" + reaudit + "' evaluate='" + evaluate + "' sample='" + sample
        + "' reauditNumber='" + reauditNumber + "' phase='" + phase + "' startPhaseDate='" + startPhaseDate + "' consecutivefailurelevel='" + consecutivefailureLevel
        + "' consecutivefailuretotal='" + consecutivefailureTotal + "' notavaliable='" + notAvaliabled + "' sync='false' alertlevel='" + alertlevel + "' sequential='" + sequencial + "' side='" + banda + "' headerlist='" + headerList + "'></div>";
}

function completeLevel2(level2, evaluateCurrent, evaluateTotal) {

    var avaliacao = parseInt($(level2).attr('evaluate'));
    var amostra = parseInt($(level2).attr('sample'));

    var isInfinityAvaliation = !!(avaliacao == 0 || amostra == 0);

    if (!isInfinityAvaliation && (evaluateCurrent > evaluateTotal)) {
        //Completa o level2 Selecionado.
        level02Complete(level2);
    }
}

$(document).on('click', '.level2 .na', function (e) {
    var botaoSalvar = $(this).siblings('.btnAreaSave');
    var iconCompleto = $(this).parents('li').children('.row').children('.level02').children('.icons').children('.areaComplete');
    var iconNaoCompleto = $(this).parents('li').children('.row').children('.level02').children('.icons').children('.areaNotComplete');
    var level01 = $('.level01.selected');
    var level02 = $(this).parents('li').children('.row').children('.level02');

    if ($(this).hasClass('naSelected')) {
        $(this).removeClass('naSelected');
        botaoSalvar.removeClass('disabled');
        iconCompleto.addClass('hide');
        iconNaoCompleto.removeClass('hide');
        level02.removeAttr('notavaliable').removeAttr('completed');
        level02.parents('li').removeClass('bgNoAvaliable').removeClass('bgCompleted');
        //$('#btnSave').addClass('hide');
    }
    else {
        $(this).addClass('naSelected');
        botaoSalvar.addClass('disabled');
        iconCompleto.removeClass('hide');
        iconNaoCompleto.addClass('hide');
        level02.attr('notavaliable', 'notavaliable');
        level02.parents('li').addClass('bgNoAvaliable');
        level02.parents('li').children('.row').children('.userInfo').children('div').children('.btnAreaSaveConfirm').click();
    }
    if ($('.level01.selected').attr('id') == '1') {
        $('.btnAreaSave').parents('.bgNoAvaliable').find('.btnAreaSave').removeClass('hide');
        $('.btnNotAvaliable').parents('.bgNoAvaliable').find('.btnNotAvaliable').removeClass('hide');
    }

});

function saveResultLevel2() {
    var level1 = $('.level1.selected');
    var period = $('#selectPeriod').val();
    if (period == undefined) {
        period = 1;
    }
    $('.level01Result[level01id=' + level1.attr('id') + '][completedsample=completedsample][period=' + period + ']').attr('completed', 'completed');
    level1.attr('completed', 'completed');
    setValoresLevel1Alertas(level1);
    level1Show(false, clusterAtivo);
}

function acoesSalvar(tipo) {

    var retorno = true;

    switch (tipo) {
        case 1:
            if ($('.level1.selected').attr('hasgrouplevel2') == "true") {

                var listLevel2 = $('.level3Group[level1idgroup="' + $('.level1.selected').attr('id') + '"] .level2');
                var evaluateCurrent = parseInt($('.level3Group[level1idgroup=' + $('.level1.selected').attr('id') + '] .painelLevel03 .evaluateCurrent').text());
                var sampleCurrent = parseInt($('.level3Group[level1idgroup=' + $('.level1.selected').attr('id') + '] .painelLevel03 .sampleCurrent').text());

                if ($('.level1.selected').attr('isreaudit') == "true" && sampleCurrent == $(listLevel2[0]).attr('sample')) {
                    var reauditevaluation = $('.level1.selected').attr('reauditevaluation') == undefined ? 1 : parseInt($('.level1.selected').attr('reauditevaluation'));
                    reauditevaluation = reauditevaluation + 1;
                    $('.level1.selected').attr('reauditevaluation', reauditevaluation);
                }

                for (var i = 0; i < listLevel2.length; i++) {
                    $('.level2.selected').removeClass('selected');
                    $(listLevel2[i]).addClass('selected');
                    saveResultLevel3();
                    $('.level2.selected').removeClass('selected');
                }

                anterior[propertyObjAnteriorDef3] = parseInt(tdef3.text());

                $('.level3Group:visible input.defects').val(0);
                $('.level3Group:visible .level3').removeClass('lightred');

                var level3Group = $('.level3Group[level1idgroup="' + $('.level1.selected').attr('id') + '"]');

                updateEvaluateSample(listLevel2, level3Group, evaluateCurrent, sampleCurrent);
            } else {

                var options = $('.level3Group[level1id=' + $('.level1.selected').attr('id') + '][level2id=' + $('.level2.selected').attr('id') + '] select[checkbox="true"] .selected');

                if (options.length > 0) {
                    var level1 = $('.level1.selected').attr('id');
                    var level2 = $('.level2.selected').attr('id');
                    var _level3Group = $('.level3Group[level1id=' + level1 + '][level2id=' + level2 + ']').clone();
                    options.parent().removeClass('hide');

                    options.each(function (index, elem) {
                        var isVisible = $('.level3Group[level1id=' + level1 + '][level2id=' + level2 + ']:visible').length > 0;
                        if (!isVisible) {
                            $('.level2Group[level01id=' + level1 + '] .level2[id=' + level2 + ']').click();
                        }

                        isVisible = $('.level3Group[level1id=' + level1 + '][level2id=' + level2 + ']:visible').length > 0;
                        if (isVisible) {
                            $(elem).parent().children('option').removeAttr('selected');
                            $(elem).attr('selected', true);

                            $('.level3Group[level1id=' + level1 + '][level2id=' + level2 + '] .level3').each(function (i, e) {
                                var _level3 = $(_level3Group).find('.level3[id=' + $(e).attr('id') + ']').clone();
                                $(e).replaceWith(_level3);
                            });

                            saveResultLevel3();
                        }
                    });
                    options.parent().addClass('hide');
                } else {
                    saveResultLevel3();
                }
                if ($($('.level2')[$('.level2').length - 1]).attr('id') == $(_level2).attr('id')) {
                    scrollTop();
                }

            }
            break;

        case 2:
            saveResultLevel2();
            break;

        case 3:
            saveResultLevel3();
            retorno = false;
            scrollTop();
            break;

        default: break;
    }

    resetCollapseLevel2();

    return retorno;
}
$(document).on('click', '.btnAreaSave', function (e) {

    $('.overlay').show();
    $('.level2.visible').removeClass('selected');
    $(this).parents('.row').children('.level2').addClass('selected');

    $(this).siblings('.btnAreaSaveConfirm').removeClass('hide');

});

$(document).on('click', '.btnNotAvaliableLevel2', function (e) {

    if ($(this).is(':disabled')) {
        return false;
    }

    if (validHeader()) {
        $('.level2').removeClass('selected');
        $(this).parent().siblings('.level2').addClass('selected');
        _level2 = $('.level2.selected')[0];
        var level3Group = $('.level3Group[level1id=' + $('.level1.selected').attr('id') + '][level2id=' + $('.level2.selected').attr('id') + ']');
        if ($('.level1.selected').attr('hasgrouplevel2') == "true") {
            level3Group = $('.level3Group[level1idgroup=' + $('.level1.selected').attr('id') + ']');
        }
        resetLevel3(level3Group);
        if ($('.level2.selected').attr('notavaliable') == "true") {
            $('.level2.selected').removeAttr('notavaliable');
        } else {
            $('.level2.selected').attr('notavaliable', "true");
        }

        level3Group.find('.btnNotAvaliable').click();

        if ($('.level1.selected').attr('editlevel2') == "true") {
            $('#btnSaveTemp').click();
        } else {
            saveResultLevel3();
        }

        scrollClick(_level2.id);
        $('.level2').removeClass('selected');
        $('.button-collapse:visible').click();
        level3Group.find('input.defects').val(0);
    }

});

$(document).on('click', '.btnAreaSaveConfirm', function (e) {

    if ($(this).is(':disabled')) {
        return false;
    }

    if (validHeader()) {

        $(this).addClass('hide');
        $(this).siblings('.btnAreaSave').removeAttr('disabled');

        $('.level2').removeClass('selected');
        $(this).parent().siblings('.level2').addClass('selected');
        _level2 = $('.level2.selected')[0];
        var level3Group = $('.level3Group[level1id=' + $('.level1.selected').attr('id') + '][level2id=' + $('.level2.selected').attr('id') + ']');
        if ($('.level1.selected').attr('hasgrouplevel2') == "true") {
            level3Group = $('.level3Group[level1idgroup=' + $('.level1.selected').attr('id') + ']');
        }
        resetLevel3(level3Group);

        if ($('.level1.selected').attr('editlevel2') == "true") {
            $('#btnSaveTemp').click();
        } else {
            saveResultLevel3();
        }

        scrollClick(_level2.id);
        $('.level2').removeClass('selected');
        $('.button-collapse:visible').click();
        level3Group.find('input.defects').val(0);

        $('.overlay').hide();

    }

});

$(document).on('click', '.level2Group .changeAvNumber', function (e) {

    openMessageConfirmGeneric('Alterar número da avaliação', 'Número: ', getAvaliationNumber, 'number', $(this));

});

//Atualizar o contador geral do indicador
function GetUpdateCounter() {
    $('#total_defects').find('span').text(defectsLevel2Total);
}

function updateDefHeaderL2(level1) {
    var rnumber = getReauditTemp();
    var def = 0;
    var level2 = $.grep(tempLevel2, function (o) {
        if (o.level1id == level1 &&
            o.shift == $('.App').attr('shift') &&
            o.period == $('.App').attr('period') &&
            o.unitid == $('.App').attr('unidadeid') &&
            o.reauditnumber == rnumber &&
            o.date == getCollectionDate()) {
            def += o.defeitos;
        }
    });

    return def;
}

function updateDefLineL2(level1, level2) {
    var rnumber = getReauditTemp();
    var def = 0;
    var level2 = $.grep(tempLevel2, function (o) {
        if (o.level1id == level1 &&
            o.level2id == level2 &&
            o.shift == $('.App').attr('shift') &&
            o.period == $('.App').attr('period') &&
            o.unitid == $('.App').attr('unidadeid') &&
            o.reauditnumber == rnumber &&
            o.date == getCollectionDate()) {
            def += o.defeitos;
        }
    });

    return def;
}

function atualizaCorAgendamento() {
    ParReasonType_Id = null;
    setTimeout(function () {
        $('.level2[frequenciavalor]:visible').each(
            function (i, o) {
                if ($(o).attr('frequenciavalor').length > 0) {

                    var av = 0;
                    var ini = 0;
                    var fim = 0;
                    var situacao = "";

                    $($(o).attr('frequenciavalor').split('|')).each(
                        function (i2, o2) {

                            var frequenciaId = $(o).attr('parfrequency_id');
                            var hour = new Date().getHours();
                            var level1 = $('.level1.selected');
                            var level1Id = level1.attr('id');
                            var unitId = $('.App').attr('unidadeid');
                            var period = $('.App').attr('period');
                            var shift = $('.App').attr('shift');
                            var avaliacaoAtual = $(o).attr('evaluateCurrent') == undefined ? 0 : $(o).attr('evaluateCurrent');
                            var mapeamento = o2.split('-');
                            var level2Id = $(o).attr('id');

                            if (o2.indexOf(":") == 2 || frequenciaId == 10) { //Diário com Intervalo

                                var horaPrimeiraAv;
                                var horaMinutoPrimeiraAv;
                                var horaMinPermitida;
                                var horaMaxPermitida;
                                var reaudnumber = 0;
                                var hoje = new Date();
                                var agora = new Date();
                                var intervalo = mapeamento[0];

                                //isso foi copiado, pra pegar o numero da reauditoria
                                var reaudMax = $('.Resultlevel2[level1id=' + level1Id + '][shift=' + shift + '][period=' + period + '][unitid=' + unitId + '][level2id=' + level2Id + ']:last').attr('reauditnumber')

                                if (reaudMax > 0)
                                    reaudnumber = reaudMax;

                                if (reaudnumber == 0 && (level1.attr('isreaudit') == 'true' || level1.attr('reaudminlevel') > 0)) {
                                    reaudnumber = 1;
                                    reaudMax = 1;
                                }

                                // fim

                                if (!!parseInt(avaliacaoAtual)) {

                                    horaPrimeiraAv = $('.Resultlevel2[level1id=' + level1Id + '][unitid=' + unitId + '][level2id=' + level2Id + '][shift=' + $('.App').attr('shift') +']').attr('horaprimeiraavaliacao');

                                    if (!!horaPrimeiraAv) {
                                        horaMinutoPrimeiraAv = horaPrimeiraAv.split(":");
                                    } else {
                                        return;
                                    }

                                    if (typeof (horaMinutoPrimeiraAv) == 'undefined' || !horaMinutoPrimeiraAv) {
                                        return;
                                    }

                                    var horaAv = parseInt(horaMinutoPrimeiraAv[0]);
                                    var minutoAv = parseInt(horaMinutoPrimeiraAv[1]);

                                    var horaMin = parseInt(intervalo.split(":")[0]) * (avaliacaoAtual - 1);
                                    var minutosMin = parseInt(intervalo.split(":")[1]) * (avaliacaoAtual - 1);

                                    var horaMax = parseInt(intervalo.split(":")[0]) * avaliacaoAtual;
                                    var minutosMax = parseInt(intervalo.split(":")[1]) * avaliacaoAtual;

                                    horaMinPermitida = new Date().setHours((horaAv + horaMin), (minutoAv + minutosMin), 0, 0);
                                    horaMaxPermitida = new Date().setHours((horaAv + horaMax), (minutoAv + minutosMax), 0, 0);

                                    if (agora.getTime() > (horaMinPermitida + (horaMaxPermitida - horaMinPermitida) * 0.5) && agora.getTime() <= horaMaxPermitida) {
                                        situacao += "3"; //Não sei, acho que é ta quase atrasado

                                    } else if (agora.getTime() >= horaMinPermitida && agora.getTime() <= horaMaxPermitida) {
                                        situacao += "2"; //Pode coletar

                                    } else if (agora.getTime() > horaMaxPermitida) {
                                        situacao += "4"; //Atrasado

                                    } else if (agora.getTime() < horaMinPermitida) {
                                        situacao += "1"; //não pode coletar

                                    }

                                } else {
                                    situacao += "2"; //Pode coletar

                                }

                            } else {

                                var avVigente = parseInt(mapeamento[0]);

                                if (!(avaliacaoAtual > 0))
                                    avaliacaoAtual = 1;

                                if (avaliacaoAtual == avVigente) {
                                    av = avVigente;
                                    ini = mapeamento[1];
                                    fim = mapeamento[2];

                                    //diario (controle por horario)
                                    if (frequenciaId == 3) {
                                        var hour = new Date().getTime();

                                        ini = new Date().setHours(mapeamento[1].split(':')[0], mapeamento[1].split(':')[1], 0, 0);
                                        fim = new Date().setHours(mapeamento[2].split(':')[0], mapeamento[2].split(':')[1], 0, 0);

                                        //se o fim for para o outro dia, ou seja, menor que o inicio, soma um dia
                                        if (fim < ini)
                                            fim = new Date(fim).setDate(new Date().getDate() + 1);

                                        //danger  = 4 
                                        //warning = 3
                                        //success = 2
                                        //default = 1

                                        if (hour >= (ini + (fim - ini) * 0.5) && hour <= fim) {
                                            situacao += "3";
                                        } else if (hour >= ini && hour <= fim) {
                                            situacao += "2";
                                        } else if (hour > fim) {
                                            situacao += "4";
                                        } else if (hour < ini) {
                                            situacao += "1";
                                        }

                                    } else if (frequenciaId == 4) { //SEMANAL

                                        // var day = new Date().getDay();
                                        var day = new Date(convertDate($('.atualDate').text())).getDay()

                                        if (day >= (ini + (fim - ini) * 0.5) && day <= fim) {
                                            situacao += "3";
                                        } else if (day >= ini && day <= fim) {
                                            situacao += "2";
                                        } else if (day > fim) {
                                            situacao += "4";
                                        } else if (day < ini) {
                                            situacao += "1";
                                        }

                                    } else if (frequenciaId == 5) { //QUINZENAL

                                        // var day = new Date().getDate() % 15;                               
                                        var day = new Date(convertDate($('.atualDate').text())).getDate() % 15;

                                        if (day >= (ini + (fim - ini) * 0.5) && day <= fim) {
                                            situacao += "3";
                                        } else if (day >= ini && day <= fim) {
                                            situacao += "2";
                                        } else if (day > fim) {
                                            situacao += "4";
                                        } else if (day < ini) {
                                            situacao += "1";
                                        }

                                    } else if (frequenciaId == 6) { //MENSAL

                                        // var day = new Date().getDate();
                                        var day = new Date(convertDate($('.atualDate').text())).getDate();

                                        if (day >= (ini + (fim - ini) * 0.5) && day <= fim) {
                                            situacao += "3";
                                        } else if (day >= ini && day <= fim) {
                                            situacao += "2";
                                        } else if (day > fim) {
                                            situacao += "4";
                                        } else if (day < ini) {
                                            situacao += "1";
                                        }
                                    }
                                }
                            }
                        }
                    );

                    //verificar qual avaliação estou, e verificar todas a frente e pegar o pior caso para apresentar no semaforo

                    //danger  = 4
                    //warning = 3
                    //success = 2
                    //default = 1

                    $(o).find("[data-semaforo]").remove();
                    $(o).attr('data-motivo', false);
                    $(o).attr('data-reasontype', 0);

                    if ($(o).attr('completed') != "completed") {

                        if (situacao.indexOf("4") >= 0) {
                            $(o).append("<div data-semaforo class='btn btn-danger pull-right'>&nbsp</div>");
                            $(o).attr('data-reasontype', 2);
                            $(o).attr('data-motivo', true);
                        } else if (situacao.indexOf("3") >= 0) {
                            $(o).append("<div data-semaforo class='btn btn-warning pull-right'>&nbsp</div>");
                        } else if (situacao.indexOf("2") >= 0) {
                            $(o).append("<div data-semaforo class='btn btn-success pull-right'>&nbsp</div>");
                        } else if (situacao.indexOf("1") >= 0) {
                            $(o).attr('data-motivo', true);
                            $(o).attr('data-reasontype', 1);
                            $(o).append("<div data-semaforo class='btn btn-default pull-right'>&nbsp</div>");
                        }
                    }
                }
            }
        );

        setTimeout(function () {
            if ($('.level2:visible').length > 0)
                atualizaCorAgendamento();
        }, 2000);
    }, 200);
}

function criarFiltroDeFrequencia(){
        if($('.headerCounter > div:last b').hasClass('hide') 
        || $('select[data-filtro="frequencyTotal"]').length > 0)
            return;

        var frequenciasExistentes = [];

        var htmlSelect = "<select data-filtro='frequencyTotal'>";
        htmlSelect += "<option>-</option>";
        $.each($('.counters .frequencyTotal'), function (i,o) {
            var frequencia = $(o).text();
            if(frequenciasExistentes.indexOf(frequencia) < 0){
                frequenciasExistentes.push(frequencia);
                htmlSelect += "<option>"+frequencia+"</option>";
            }
        });
            
        htmlSelect += "</select>";
        $('.headerCounter > div:last').append(htmlSelect);
}

$('body').off('change', 'select[data-filtro]').on('change', 'select[data-filtro]', function () {
    var valorSelecionado = $(this).val();

    //Ocultar todas as linhas
    $('.counters .frequencyTotal')
        .parents('.list-group-item')
        .addClass('hide');

    $.each($('.accordion > .card'),
        function (i, o) {
            $(o).removeClass('hide')
        }
    )

    if (valorSelecionado.length == 1) {
        $('.counters .frequencyTotal')
            .parents('.list-group-item')
            .removeClass('hide');
        return;
    }

    //Mostra linhas que não batem o valor
    $('.counters .frequencyTotal').filter(function () {
        return $(this).text() === valorSelecionado;
    })
        .parents('.list-group-item')
        .removeClass('hide')

    $.each($('.accordion > .card'),
        function (i, o) {
            if ($(o).find('.gabriel:not(.hide)').length == 0) {
                $(o).addClass('hide');
            }
        }
    );
});