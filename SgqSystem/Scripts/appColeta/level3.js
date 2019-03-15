var anterior = {};
var seletorTdef;
var propertyObjAnterior;
var tdef;
var tdefAv;
var tdef3;
var propertyObjAnteriorDef3, tempHDL2;
var motivoAtrasoSelected_Id = null;
var parDepartmentSelected_Id = null;

function HourToMinutes(hour) {
    if (hour.length > 0 && hour.indexOf(':') > 0) {
        var arrHour = hour.split(':');
        return parseInt(arrHour[0] * 60) + parseInt(arrHour[1]);
    }
    return 0;
}

function openLevel3(level2) {

    mockCFFSmp = 0;

    /*if ($(level2).find('.btn-default').length > 0) {
        openMessageModal('Por Favor, aguardar a liberação para realizar a coleta', 'Coleta bloqueada por estar fora do prazo agendado de coleta.')
        return;
    }*/

    abrirDepartamentoNoLevel3();

    $('input.likert').trigger('blur');
    $('.level3 input, .level3 textarea').val('');

    $('.level3Group[level2id=' + level2.attr('id') + '] .painelLevel02').remove();

    var painelClone = $('.level2Group[level01id=' + $('.level1.selected').attr('id') + '] .painelLevel02:visible').clone();

    var selects = $('.level2Group[level01id=' + $('.level1.selected').attr('id') + '] .painelLevel02:visible').find("select");
    $(selects).each(function (i) {
        var select = this;
        $(painelClone).find("select").eq(i).val($(select).val());
    });

    var avaliacoesLevel1 = $('.level2Group[level01id=' + $('.level1.selected').attr('id') + ']').find('.evaluateCurrent');
    var amostrasTotalLevel1 = $('.level2Group[level01id=' + $('.level1.selected').attr('id') + ']').find('.sampleTotal');
    var amostrasAtualLevel1 = $('.level2Group[level01id=' + $('.level1.selected').attr('id') + ']').find('.sampleCurrent');

    var reduzir = false;

    var maior = 0;
    var menor = 0;

    for (var i = 0; i < avaliacoesLevel1.length; i++) {

        parseFloat(amostrasAtualLevel1[i].textContent)

        var avaliacaoAtual = parseFloat(avaliacoesLevel1[i].textContent);

        avaliacaoAtual = parseFloat(amostrasAtualLevel1[i].textContent) == 0 ? avaliacaoAtual + 1 : avaliacaoAtual;

        var amostrasRestantes = parseFloat(amostrasTotalLevel1[i].textContent) - parseFloat(amostrasAtualLevel1[i].textContent);

        if (avaliacaoAtual > maior) {

            maior = avaliacaoAtual;
        }

        if (avaliacaoAtual < menor || menor == 0) {
            menor = avaliacaoAtual;

            reduzir = true;
        }

    }

    if (reduzir) {
        menor--;
    }

    //Oculta Lista de Level2
    var level2List = level2.parents('.level2List');

    var level1 = $('.level1.selected');
    var level2 = $('.level2.selected');
    $('.btnCA').addClass('hide');

    //setup reaudit level 1
    if (level1.attr('isreaudit') == "true") {
        $('.reauditFlag').addClass('hide');
        $('.level3Group[level2id=' + $('.level2.selected').attr('id') + '] .reauditFlag').removeClass('hide');
        $('.level3Group[level1idgroup=' + $('.level1.selected').attr('id') + '] .reauditFlag').removeClass('hide');

        level2.attr('evaluatecurrent', level1.attr('lastevaluate'));
        level2.attr('samplecurrent', 0);

        $('.level3Group[level2id=' + $('.level2.selected').attr('id') + '] .reauditFlag .reauditnumber').text(level1.attr('reauditnumber'));

        if (level1.attr('hasgrouplevel2') == 'true') {
            var listLevel2 = $('.level3Group[level1idgroup=' + $('.level1.selected').attr('id') + '] .level2');

            var reauditevaluation = level2.attr('reauditevaluation') == undefined ? 1 : parseInt(level2.attr('reauditevaluation'));

            listLevel2.attr('evaluatecurrent', parseInt(level1.attr('lastevaluate')) + 1);
            listLevel2.attr('samplecurrent', 0);

            level2.attr('evaluatecurrent', parseInt(level1.attr('lastevaluate')) + 1);
            level2.attr('samplecurrent', 0);

            var avalCorrente = parseInt($(_level1).attr('lastevaluate'));
            var sampleAux = parseInt(level1.attr('lastsample')) + 1;
            if (avalCorrente > parseInt(level2.attr('evaluate')) && level1.attr('islimitedevaluetionnumber') == "true") {
                level1.attr('hascompleteevaluation', 'true');
                // $(_level1).attr('hascompleteevaluation', 'true');
                // var number = parseInt(level1.attr('reauditnumber')) + 1;
                // level1.attr('reauditnumber', number);
                level1.attr('lastevaluate', level1.attr('reauditevaluation'));
                openMessageModal(getResource("evaluation_complete"), null);
                return openLevel2(level1);
            }
        }
    }

    //setup reaudit level 2
    if (level2.attr('isreaudit') == "true") {
        $('.reauditFlag').addClass('hide');
        $('.level3Group[level2id=' + $('.level2.selected').attr('id') + '] .reauditFlag').removeClass('hide');

        var reauditevaluation = level2.attr('reauditevaluation') == undefined ? 1 : parseInt(level2.attr('reauditevaluation'));

        level2.attr('evaluatecurrent', reauditevaluation);
        level2.attr('reauditevaluation', reauditevaluation);
        //level2.attr('samplecurrent', 0);

        $('.level3Group[level2id=' + $('.level2.selected').attr('id') + '] .reauditFlag .reauditnumber').text(level2.attr('reauditnumber'));
    }

    level2List.hide();
    //Oculta os grupos de tarefas
    $('.level3Group').addClass('hide');
    //Instancia todas tarefas
    var level3List = $('.level3List');
    //Instancia o grupo de Level3 referente ao Level2 selecionado
    var level3Group = level3List.children('.level3Group[level1id=' + level1.attr('id') + '][level2id=' + level2.attr('id') + ']');
    if (level1.attr('hasgrouplevel2') == "true") {
        level3Group = level3List.children('.level3Group[level1idgroup=' + level1.attr('id') + ']');
    }
    //
    //Mosta o grupo selecionado
    resetLevel3(level3Group);

    var evaluationCurrent = parseInt(level2.attr('evaluatecurrent'));
    if (!evaluationCurrent) {
        evaluationCurrent = 1;
    }
    var evaluationUltimoAlerta = parseInt(level1.attr('avaliacaoultimoalerta'));
    if (!evaluationUltimoAlerta) {
        evaluationUltimoAlerta = 0;
    }

    if (menor == 0) {
        textoMenor = 1;
    } else {
        textoMenor = menor + 1;
    }

    /*
    CONTROLE DE AVALIAÇOES POR MONITORAMENTO
    Se for consolidação por tarefas, não posso passar para a proxiam ava sem terminar tudo.
    Se for por peças, posso
    */

    if ($('.level2.selected').attr('isreaudit') != "true") {

        if ($(_level1).attr('hascompleteevaluation') == "true") {

            if (menor != maior && evaluationCurrent - 1 > menor && evaluationCurrent != 1) {
                openMessageModal(getResource("evaluation") + " " + textoMenor + " " + getResource("not_finished"), getResource("mornitorings_pedding") + textoMenor);
                openLevel2(level1);
                return true;
            }

        }

        if ($('.App').attr('local') == 'brasil') {
            if (
                (level1.attr('hascompleteevaluation') == 'true'
                    && level1.attr('havecorrectiveaction')
                    && evaluationUltimoAlerta > 0
                    && evaluationCurrent > evaluationUltimoAlerta)

                ||

                (level1.attr('hascompleteevaluation') == 'false'
                    && parseInt(level2.attr('defects')) > 0
                    && level1.attr('havecorrectiveaction')
                    && evaluationUltimoAlerta > 0
                    && evaluationCurrent > evaluationUltimoAlerta)

            ) {
                openMessageModal(getResource("corrective_action_reports") + evaluationUltimoAlerta, getResource("ca_pending") + " " + evaluationUltimoAlerta);
                openLevel2(level1);
                return true;
            }
        } else {
            if (level1.attr('havecorrectiveaction') && evaluationUltimoAlerta > 0 && evaluationCurrent > evaluationUltimoAlerta && level1.attr('hasgrouplevel2') != 'true') {
                openMessageModal(getResource("corrective_action_reports") + evaluationUltimoAlerta, getResource("ca_pending") + " " + evaluationUltimoAlerta);
                openLevel2(level1);
                return true;
            }
        }
    }

    var LastEvaluationL1 = parseInt($(_level1).attr('lastevaluate'));
    var CurrentEvaluationL1 = parseInt(level3Group.find('.evaluateCurrent').text());

    var avalCorrente = parseInt($(_level1).attr('lastevaluate')) + 1;
    var sampleAux = parseInt($($('.level2')[1]).attr('samplecurrent'));

    var smpCorr = parseInt($(_level1).attr('lastsample')) + 1;
    // if(sampleAux == parseInt($('.level2.selected').attr('sample')))
    //     avalCorrente++;

    if (avalCorrente > parseInt(level2.attr('evaluate')) && smpCorr > level2.attr('sample') && level1.attr('islimitedevaluetionnumber') == "true"
        && level1.attr('hasgrouplevel2') == "true") {
        level1.attr('hascompleteevaluation', 'true');
        openMessageModal(getResource("evaluation_complete"), null);
        return openLevel2(level1);
    }

    defectsLevel2Total = $(_level2).attr('totaldefeitos') != undefined ? parseInt($(_level2).attr('totaldefeitos')) : 0;

    if (CurrentEvaluationL1 > LastEvaluationL1) {
        defectsLevel1Total = 0;
        defectsLevel2Total = 0;
        defects3MoreEvaluateTotal = 0;
        defectsEvaluateTotal = 0;
    }


    level3Group.find('.total_defects').text(defectsLevel1Total);
    level3Group.find('.level2_defects').text(defectsLevel2Total);
    level3Group.find('.defects_evaluate').text(defectsEvaluateTotal);

    counterSetSide(level3Group);

    level3Group.removeClass('hide');

    breadCrumb($('.level1List .level1.selected').text(), $('.level2List .level2.selected span.levelName').text());

    $('#btnSave').addClass('hide');
    if (!$('span.Completed').is(':visible')) {
        $('#btnSave').removeClass('hide');
    }
    else {
        $('.level3Group[level2id=' + $('.level1.selected').attr('id') + '] span.input-group-btn').addClass('hide');
        $('.level3Group[level2id=' + $('.level1.selected').attr('id') + '] input').attr('disabled', 'disabeld');

        $('.level3Group[level2id=' + $('.level1.selected').attr('id') + '] span.input-group-btn').addClass('hide');
        $('.level3Group[level2id=' + $('.level1.selected').attr('id') + '] input').attr('disabled', 'disabeld');

    }

    resetHeaderLevel3(level3Group);
    //Mostra a tela de tarefa
    level3List.removeClass('hide').show();

    var ncAtual = parseFloat($(_level1).attr('totaldefeitos'));

    var avAtual = $('.level3Group[level2id=' + $('.level1.selected').attr('id') + '] .painel .evaluateCurrent').text();
    var avAnterior = $(_level2).siblings('.counters').find('.evaluateCurrent').text();
    var metaTolAtual = parseFloat($(_level1).attr('metatolerancia').replace(',', '.'));

    var confirmaNovaMetaTolerancia = false;

    if (parseFloat($(_level1).attr('parconsolidationtype_id')) == 3) {

        if ($(_level1).attr('monitoramentoultimoalerta') != _level2.id || avAtual > $(_level1).attr('avaliacaoultimoalerta')) {
            confirmaNovaMetaTolerancia = true;
        }

    } else {

        var defeitoUltimaAv = 0;

        for (var i = 0; i < _totalAvaliacoesPorIndicadorPorAvaliacao.length; i++) {
            if (_totalAvaliacoesPorIndicadorPorAvaliacao[i].ParLevel1_Id == _level1.id &&
                _totalAvaliacoesPorIndicadorPorAvaliacao[i].CurrentEvaluation < avAtual) {
                defeitoUltimaAv += _totalAvaliacoesPorIndicadorPorAvaliacao[i].Defects;
            }
        }

        if (avAtual > 1 && avAtual > avAnterior && menor == avAnterior && avAtual == (parseFloat($(_level1).attr('avaliacaoultimoalerta')) + 1) && defeitoUltimaAv > metaTolAtual) {
            confirmaNovaMetaTolerancia = true;
        }
    }

    if (confirmaNovaMetaTolerancia) {

        if (metaTolAtual < ncAtual) {
            var valor = Math.ceil(ncAtual / parseFloat($(_level1).attr('alertaNivel1').replace(',', '.'))) * parseFloat($(_level1).attr('alertaNivel1').replace(',', '.'));
            $(_level1).attr('metatolerancia', valor);
        }
    }

    if (level1.attr('ispartialsave') == "true") {

        var level3Group = $('.level3Group[level1id=' + level1.attr('id') + '][level2id=' + level2.attr('id') + ']');

        level3Group.find('.level3').show();
        level3Group.find('.panel').show();


        var resultLevel2 = $('.Resultlevel2[level1id=' + level1.attr('id') + '][level2id=' + level2.attr('id') + '][collectiondate="' + getCollectionDate() + '"][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + ']');
        if (resultLevel2.length) {
            var resultLevel3InLevel2 = resultLevel2.children('.r3l2');

            resultLevel3InLevel2.each(function (e) {


                var level3 = level3Group.find('.level3[id=' + $(this).attr('id') + ']');
                level3.hide();

                var level3InAccordeon = level3.parents('.panel');
                if (!level3InAccordeon.find('ul.list-group').children('.level3:visible').length) {
                    level3InAccordeon.hide();
                }
            });
        }

        if (!level3Group.find('.level3:visible').length) {

            level3Group.find('.level3').show();
            level3Group.find('.panel').show();
        }
    }

    if ($('.level3Group.PCC1B').is(':visible')) {
        trocarSequencial = true;
        getPCC1BNext(true);
    }

    loadHeaders();

    //loadMasks();

    if ($('.level1.selected').attr('isreaudit') == "true") {
        var evaluateCurrent = $('.level3Group[level2id=' + $('.level2.selected').attr('id') + '] .painelLevel03 .evaluateCurrent');
        var sampleCurrent = $('.level3Group[level2id=' + $('.level2.selected').attr('id') + '] .painelLevel03 .sampleCurrent');
        var evaluateTotal = $('.level3Group[level2id=' + $('.level2.selected').attr('id') + '] .painelLevel03 .evaluateTotal');
        var sampleTotal = $('.level3Group[level2id=' + $('.level2.selected').attr('id') + '] .painelLevel03 .sampleTotal');

        evaluateTotal.text('1');
    }

    //painelClone.find('input, select').attr('disabled', 'disabled'); //DESABILITA O CAMPO DE CABEÇALHO DO MONITORAMENTO
    //painelClone.find('input, select');
    painelClone.find("div").removeClass("header");

	$('.painelLevel03:visible').prepend(painelClone);
	
	/*Paleativo permanente para os tipos de cabeçalho do tipo data. Algum bug
	que precisou fazer a gambiarra abaixo para conseguir setar o valor corretamente*/
	$('.painelLevel03:visible').find('input[type="date"]').each(function (i, e) {
		var element = $('.painelLevel03:visible')
		.find('input[parheaderfield_id="'+$(e).attr('parheaderfield_id')+'"]')
		element.attr('type',"text");
		element.attr('value',$(e).val());
		element.attr('type',"date");
	});
		
    //beforeDevice(painelClone, $('.painelLevel03:visible'));

    $('#period').attr('disabled', 'disabled');
    $('input.defects').val(0);

    if ($('.level1.selected').attr('hasgrouplevel2') == "true") {

        var listLevel2 = $('.level3Group[level1idgroup="' + $('.level1.selected').attr('id') + '"] .level2');
        var level3Group = $('.level3Group[level1idgroup="' + $('.level1.selected').attr('id') + '"]');

        if (isEUA == true && level1.attr('hasgrouplevel2') == 'true') {
            var reaudnumber = 0;
            reaudnumber = $('.Resultlevel2[level1id=' + level1.attr('id') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + '][unitid=' +
                $('.App').attr('unidadeid') + ']:last').attr('reauditnumber');

            if (reaudnumber == 0 && level1.attr('isreaudit') == 'true') {
                reaudnumber = 1;
            }

            if ($(_level1).attr('reauditnumber') > reaudnumber) {
                reaudnumber = parseInt($(_level1).attr('reauditnumber'));
            }

            if ($('.Resultlevel2[level1id=' + $('.level1.selected').attr('id') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') +
                '][reauditnumber=' + reaudnumber + ']:first').length > 0) {
                var evaluateVisible = $('.Resultlevel2[level1id=' + $('.level1.selected').attr('id') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') +
                    '][reauditnumber=' + reaudnumber + ']:first').attr('evaluation');
                var sampleVisible = $('.Resultlevel2[level1id=' + $('.level1.selected').attr('id') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') +
                    '][reauditnumber=' + reaudnumber + ']:first').attr('sample');
            } else {
                var evaluateVisible = 1;
                var sampleVisible = 0;
            }
            updateEvaluateSample(listLevel2, level3Group, evaluateVisible, sampleVisible);
        } else {
            if ($('.Resultlevel2[level1id=' + $('.level1.selected').attr('id') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + ']:first').length > 0) {
                var evaluateVisible = $('.Resultlevel2[level1id=' + $('.level1.selected').attr('id') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + ']:first').attr('evaluate');
                var sampleVisible = $('.Resultlevel2[level1id=' + $('.level1.selected').attr('id') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + ']:first').attr('sample');

                updateEvaluateSample(listLevel2, level3Group, evaluateVisible, sampleVisible);
            }
        }

    }

    $('#btnShowImage').remove();

    AtruibuiEventoCounterTipoCFF();

    initializeInputs();

    resetCollapseLevel2();

    readCounter(parseInt(level1.attr('id')));

    if (level1.attr('editlevel2') == 'true') {
        getTempLevel2(level1.attr('id'), level2.attr('id'), 0);

        $('#btnSaveTemp').removeClass('hide');
        $('#btnSave').addClass('hide');
        $('#btnSaveAllTemp').addClass('hide');
    }
    // if (isEUA) {
    $('.painelLevel02 .form-group .counter .value:visible').text(tempHDL2);
    if ($('.counter[indicador=' + level1.attr('id') + '][headerlevel=level3_header][counter=defects]').length == 0
        && $('.counter[indicador=' + level2.attr('id') + '][headerlevel=level3_header][counter=defects]').length == 0
        && level1.attr('hasgrouplevel2') != 'true') {
        $('.painelLevel03 .defects').parent().parent().parent().addClass('hide');
    } else if ($('.counter[indicador=' + level1.attr('id') + '][headerlevel=level3_header][counter=defects][level=1]').length > 0
        && $('.counter[indicador=' + level2.attr('id') + '][headerlevel=level3_header][counter=defects][level=2]').length == 0
        && level1.attr('hasgrouplevel2') != 'true') {
        $('.painelLevel03 .defects').parent().parent().parent().find('.font-small:visible').text('Total Defects');
    }

    if ($('.counter[indicador=' + level1.attr('id') + '][headerlevel=level3_header][counter=evaluation]').length == 0 &&
        $('.counter[indicador=' + level2.attr('id') + '][headerlevel=level3_header][counter=evaluation]').length == 0
        && level1.attr('hasgrouplevel2') != 'true') {
        $('.painelLevel03 .evaluateCurrent').parent().parent().parent().addClass('hide');
    }

    if ($('.counter[indicador=' + level1.attr('id') + '][headerlevel=level3_header][counter=sample]').length == 0 &&
        $('.counter[indicador=' + level2.attr('id') + '][headerlevel=level3_header][counter=sample]').length == 0
        && level1.attr('hasgrouplevel2') != 'true') {
        $('.painelLevel03 .sampleCurrent').parent().parent().parent().addClass('hide');
    }

    if (level1.attr('hasgrouplevel2') == 'true') {
        $('.painelLevel03 .defects').parent().parent().parent().addClass('hide');
    }

    if ($('.level1.selected.VF').length > 0) {
        updateVerificacaoHeaders();
        setAreasParticipantes();

        $($('.painelLevel03 div')[0]).removeClass('hide');
        $($('.painelLevel03 div')[2]).removeClass('hide');

        $('.sampleCurrent:visible').text($('.ResultsKeysVF div[date="' + getCollectionDate() + '"][unidadeid=' + $('.App').attr('unidadeid') + ']').length + 1);
        $('.evaluateCurrent:visible').text('1');

        $('.level1.selected').attr('samplecurrent', $('.sampleCurrent:visible').text());
    }

    $('#btnCA').addClass('hide');

    displayPhotoButtons();

    setEvaluationByHeadersSelection();

    setupRetrocesso();

    headerWithCheckbox();

    configVinculoAvAm();

    if ($(level2).attr('data-motivo') == "true") {

        openMessageConfirmAtraso(function () {

            motivoAtrasoSelected_Id = $("#slcMotivo :selected").val();

        });

    } else {
        motivoAtrasoSelected_Id = null;
    }

    removeFotosNaoSalvas();

    $('.level3 input').val('').trigger('input');

    if ($('#btnAllNA').text() == 'Todos A')
        $('#btnAllNA').trigger('click');

}

function configVinculoAvAm() {

    var level1Id = parseInt($('.level1.selected').attr('id'));
    var level2Id = parseInt($('.level2.selected').attr('id'));
    var avaliacao = parseInt($('.level3Group[level1id=' + level1Id + '][level2id=' + level2Id + '] .evaluateCurrent').text());
    var amostra = parseInt($('.level3Group[level1id=' + level1Id + '][level2id=' + level2Id + '] .sampleCurrent').text());

    $('.level3Group[level1id=' + level1Id + '][level2id=' + level2Id + '] .level3').each(function (i, e) {
        var level3Id = parseInt($(e).attr('id'));

        var level1Id_Id = level1Id.toString().split('98789');
        var l1_id = 0
        if (level1Id_Id.length > 1) {
            l1_id = parseInt(level1Id_Id[1])
        } else {
            l1_id = parseInt(level1Id_Id[0])
        }

        var level3 = getLevel3Vinculado(l1_id, level2Id, level3Id);

        $(e).removeClass('hide');
        if (level3) {
            if (level3TemVinculo(l1_id, level2Id, level3Id, avaliacao, amostra, level3)) {
                $(e).removeClass('hide');
                if ($(e).parent().parent().parent().find('.level3:visible').length > 0)
                    $(e).parent().parent().parent().show();
            } else {
                $(e).addClass('hide');
                if ($(e).parent().parent().parent().find('.level3:visible').length == 0)
                    $(e).parent().parent().parent().hide();
            }
        }
    });
}

function level3TemVinculo(level1Id, level2Id, level3Id, avaliacao, amostra, level3) {
    var _level3 = level3 ? level3 : getLevel3Vinculado(level1Id, level2Id, level3Id);

    if (_level3) {
        if ((_level3.EvaluationInterval.length == 0 || parseInt(_level3.EvaluationInterval) == 0) && parseInt(amostra) <= _level3.SampleNumber) {
            return true;
        } else {
            if (_level3.EvaluationInterval.split(',').indexOf(amostra.toString()) >= 0
                && avaliacao <= _level3.EvaluationNumber) {
                return true;
            }
            return false;
        }
    }
    return false;
}

function getLevel3Vinculado(level1Id, level2Id, level3Id) {
    var _level3 = null;

    level2Id = parseInt(level2Id.toString().split('98789')[1]); //tirar o cluster
    if (typeof listaParLevel3Vinculado != 'undefined') {
        _level3 = _.find(listaParLevel3Vinculado, {
            'ParLevel1_Id': level1Id,
            'ParLevel2_Id': level2Id,
            'ParLevel3_Id': level3Id
        });

        if (!_level3) {
            _level3 = _.find(listaParLevel3Vinculado, {
                'ParLevel1_Id': level1Id,
                'ParLevel2_Id': 0,
                'ParLevel3_Id': level3Id
            });
        }

        if (!_level3) {
            _level3 = _.find(listaParLevel3Vinculado, {
                'ParLevel1_Id': 0,
                'ParLevel2_Id': level2Id,
                'ParLevel3_Id': level3Id
            });
        }

        if (!_level3) {
            _level3 = _.find(listaParLevel3Vinculado, {
                'ParLevel1_Id': 0,
                'ParLevel2_Id': 0,
                'ParLevel3_Id': level3Id
            });
        }
    }

    return _level3;
}

function resetHeaderLevel3(levelGroup) {
    levelGroup.children('.painel').find('input').val("");
    levelGroup.children('.painel').find('select').val("");
    levelGroup.children('.painel').find('.productNamelabel').html("");
}

function resetBooleanInput(inputs) {
    inputs.each(function (e) {
        $(this).attr("value", "1");
        $(this).text($(this).attr('booltruename'));
    });
}

function saveLevel03(Level03Id, value, conform, auditorId, totalError, valuetext, weight, punishment, intervalmin, intervalmax, isnotevaluate, defects) {

    if (isnotevaluate != "true") {
        isnotevaluate = "false";
    }

    if (defects == undefined) {
        defects = 0;
    }

    ativaFotosSalvasRelacionadasAColeta();

    return "<div class='level03Result' level03id='" + Level03Id + "' date='" + dateTimeFormat()
        + "' value='" + value + "' conform='" + conform + "' auditorId='" + auditorId + "' totalerror='" + totalError
        + "' valueText='" + valuetext + "' weight='" + weight + "' punishmentvalue='" + punishment
        + "' intervalmin='" + intervalmin + "' intervalmax='" + intervalmax + "' isnotevaluate='" + isnotevaluate
        + "' defects='" + defects + "' motivoAtrasoId='" + motivoAtrasoSelected_Id
        + "' parDepartmentId='" + parDepartmentSelected_Id + "'></div>";
}

function validaCamposLevel3(tipo) {

    var validacao = 0;
    var retorno = true;

    if (!validHeader()) {
        return retorno;
    }

    if (tipo == 4) {  //tipo para todos NA

        var level3 = $('.level2:visible').find('.level3');
        var botaoNA = $(this);

        level3.each(

            function (index) {

                if ($($('.level2:visible').find('.level3')[index]).attr('notavaliable') == "true")
                    if (botaoNA.text() == 'Todos A') {
                        $('.level2:visible').find('.level3 .btnNotAvaliable')[index].click();
                    }

                    else
                        if (botaoNA.text() == 'Todos A') {
                            //console.log("Tarefa Não avaliada");
                        } else {
                            $('.level2:visible').find('.level3 .btnNotAvaliable')[index].click();
                        }

            }

        )
    }

    //Percorrer todos os level3 da página aberta
    var hasgroup = $(_level1).attr('hasgrouplevel2');

    if (hasgroup == true) {
        $($('.level2:visible').find('.level3 input')).not('.naoValidarInput').each(

            function (index, self) {

                if ($($('.level2:visible').find('.level3')[index]).hasClass('calculado')) {

                    if ($($('.level2:visible').find('.level3')[index]).attr('notavaliable')) {

                        $($('.level2:visible').find('.level3')[index].children[2].children[0].children[0]).val(0)
                        $($('.level2:visible').find('.level3')[index].children[2].children[0].children[2]).val(1)

                    } else {

                        if ($($('.level2:visible').find('.level3')[index].children[2].children[0].children[0]).val() != "" && $($('.level2:visible').find('.level3')[index].children[2].children[0].children[2]).val() == "") {

                            $($('.level2:visible').find('.level3')[index].children[2].children[0].children[2]).val(0);

                        }

                    }

                } else {

                    if ($(self).val() == "") {

                        if ($($('.level2:visible').find('.level3')[index]).attr('notavaliable')) {

                            //console.log("Campo sem valor, porém é NA " + index);

                            $(self).val("0");
                            //console.log($(self).val());

                        } else {

                            //console.log("Campo sem valor " + index);
                            validacao++;
                            return;

                        }

                    } else {
                        //console.log("Valor do campo " + index + ": " + $(self).val());
                    }
                }
            }
        )
    } else {
        //Percorrer todos os level3 da página aberta
        $($('.level3:visible input')).not('.naoValidarInput').each(

            function (index, self) {

                if ($($('.level3:visible input')[index]).parents('.level3').hasClass('calculado')) {

                    if ($($('.level3:visible input')[index]).parents('li').attr('notavaliable')) {

                        $($($('.level3:visible input')[index]).parents('li').find('input')[0]).val(0);
                        $($($('.level3:visible input')[index]).parents('li').find('input')[1]).val(1);

                    } else {

                        if ($($($('.level3:visible input')[index]).parents('li').find('input')[0]).val() != "" && $($($('.level3:visible input')[index]).parents('li').find('input')[1]).val() == "") {

                            $($($('.level3:visible input')[index]).parents('li').find('input')[1]).val(0);

                        } else if ($($($('.level3:visible input')[index]).parents('li').find('input')[0]).val() == "" && $($($('.level3:visible input')[index]).parents('li').find('input')[1]).val() == "") {
                            validacao++;
                            return;
                        }

                    }

                } else {

                    if ($(self).val() == "") {

                        if ($($('.level3:visible input')[index]).parents('li').attr('notavaliable')) {

                            //console.log("Campo sem valor, porém é NA " + index);

                            $(self).val("0");
                            //console.log($(self).val());

                        } else {

                            //console.log("Campo sem valor " + index);
                            validacao++;
                            return;

                        }

                    } else {
                        //console.log("Valor do campo " + index + ": " + $(self).val());
                    }
                }
            }
        )
    }

    if (validacao == 0) {

        if ($('.level3Group.BEA').is(':visible')) {
            if (parseInt($('.pecasAvaliadas:visible').val())) {
                var sampleCurrent = parseInt($('.sampleCurrent:visible').text()) + parseInt($('.pecasAvaliadas:visible').val());
                var sampleTotal = parseInt($('.sampleTotal:visible').text());
                if (_level1.id != 42) {
                    if (sampleCurrent > sampleTotal + 1) {
                        openMessageModal(getResource("total_evaluated_parts_exceed"), null);
                        return retorno;
                    }
                }
            } else {
                openMessageModal(getResource("the_number_parts_filled"), null);
                return retorno;
            }
            if (parseInt($('.pecasAvaliadas:visible').val()) < parseInt($('.levelValue:visible').val() ? $('.levelValue:visible').val() : 0)) {
                openMessageModal(getResource("number_of_animals"), null);
                return retorno;
            }

            var validDefects = true;

            $('input.defects:visible').each(function (index, self) {
                //console.log(index+' '+self)
                if (parseInt($(self).val()) < 0) {
                    validDefects = false;
                }

            });

            if (!validDefects) {
                openMessageModal(getResource("number_of_animals"), null);
                return retorno;
            }

        }
        if ($('.level3Group.VF').is(':visible')) {

            if (!parseInt($('.sequencial:visible').val())) {
                openMessageModal(getResource("sequential_must_be_filled"), null);
                return retorno;
            }

            if (!parseInt($('.banda:visible').val())) {
                openMessageModal(getResource("side_must_be_filled"), null);
                return retorno;
            }

            var validItems = true;

            $('.level3.VF:visible').each(function (index, self) {
                var id = $(self).attr('id');
                if (id == 400 || id == 1060 || id == 1061 || id == 1058) {
                    if (!$('.level3.VF[id=' + id + '] .items div').is(':visible')) {
                        validItems = false;
                    }
                }
            });

            if (validItems == false) {
                openMessageModal(getResource("required_items_not_selected"), null);
                return false;
            }

            var chave = $('.App').attr('unidadeid') + "" + $(".level3Group.VF").attr('level1id') + "" + $('.sequencial:visible').val() + "" + $('.banda:visible').val() + "" + yyyyMMdd();



            $('.VerificacaoTipificacaoResultados div[chave=' + chave + ']').remove();

            $(".level3.VF .items").children("div:visible").each(function (index, self) {
                var tarefaid = $(self).parent('.items').attr('tarefaid');
                var areaparticipantesid = null;
                var caracteristicatipificacaoid = null;
                if ($(self).parents('.level3').attr('id') == "400")
                    areaparticipantesid = $(self).attr("cnrcaracteristica");
                else
                    caracteristicatipificacaoid = $(self).attr("cnrcaracteristica");

                var vtr = $("<div tarefaid='" + tarefaid
                    + "' areaparticipantesid='" + areaparticipantesid
                    + "' caracteristicatipificacaoid='" + caracteristicatipificacaoid
                    + "' chave='" + chave
                    + "' sync='" + false
                    + "'></div>");

                appendDevice(vtr, $(".VerificacaoTipificacaoResultados"));

            });

            $('.level3:visible').each(function (index, self) {
                createVFConsolidation($('.App').attr('unidadeid'), $(".level3Group.VF").attr('level1id'), $('.sequencial:visible').val(), $('.banda:visible').val(),
                    yyyyMMdd(), $(self).attr('id'), $('.level3Group.VF').attr('level2id'));
            });

            _writeFile("VerificacaoTipificacaoResultados.txt", $(".VerificacaoTipificacaoResultados").html());
            _writeFile("VerificacaoTipificacao.txt", $(".VerificacaoTipificacao").html());
            _writeFile("VerificacaoTipificacaoKeys.txt", $('.ResultsKeysVF').html());

            $('.level3.VF').find('.items').children('div:visible').addClass("hide");
            $('.sequencial:visible').val("");
            $('.banda:visible').val("");

            updateVerificacaoHeaders();
            setAreasParticipantes();

            var count = $('.ResultsKeysVF div[date="' + getCollectionDate() + '"][unidadeid=' + $('.App').attr('unidadeid') + ']').length + 1;

            $('.sampleCurrent:visible').text(count);

            if (count > $('.level2.selected').attr('sample')) {
                openLevel2($('.level1.selected'));
            }

            return;
        }

        var that = $(this);

        that.addClass('disabled');

        $('.level3').find('.value').text("");
        $('.level3').find('.valueDecimal').text("");

        retorno = acoesSalvar(1);
        that.removeClass('disabled');

        showDebugAlertas();

    } else {
        if (tipo == 1) {

            if ($(_level1).attr('ispartialsave') != "true") {
                openMessageModal(getResource("warning"), getResource("required_fields_unfilled"));
            }
            else if ($(_level1).attr('ispartialsave') == "true") {
                var totalInputs = $('.level3Group:visible input').length;
                var inputsEmpty = checkInputEmpty($('.level3Group:visible'));

                if (totalInputs == inputsEmpty) {
                    openLevel2($(_level1));
                }
                else {
                    retorno = acoesSalvar(tipo);
                }
            }

        }
        else if ($(_level1).attr('ispartialsave') != "true") {

            openMessageModal(getResource("action_not_allowed"), getResource("Existem campos de preenchimento obrigatórios neste monitoramento. Já deixamos a tela aberta para você preenche-los corretamente."));
            retorno = acoesSalvar(3);
        }
    }
    return retorno;
}

$(document).on('click', '#btnAllNC', function (e) {

    var notconform = $('.level3Group:visible .level3.boolean').children('.counters').children('span.response[value="0"]').parent().parent();

    var defectsRemove = 0;
    if (notconform.length > 0) {
        defectsRemove = notconform.length;
    }

    //var conform = $('.level3Group:visible .level3.boolean').children('.counters').children('span.response:not([value="0"])').parent().parent();

    //notconform.removeClass('lightred').removeAttr('notconform').children('.counters').children('span.response').attr('value', '1');
    //conform.addClass('lightred').attr('notConform', 'notCorform').children('.counters').children('span.response').attr('value', '0');

    //notconform.children('.counters').children('span.response').text(notconform.children('.counters').children('span.response:first').attr('booltruename'));
    //conform.children('.counters').children('span.response').text(conform.children('.counters').children('span.response:first').attr('boolfalsename'));

    $('.level3 a').click();

    var level02 = $('.level2.selected');

    level02.attr('defects', $('.level03[notConform]').length);

    var level3 = $(".level3Group:visible .level03");
    level3.each(function (index, self) {
        level02.attr('level03' + $(self).attr('id'), $(self).children('.counters').children('span.response').attr('value'));
    });

    if ($('.level3Group:visible .level03[notConform]').length == 0) {
        level02.removeAttr('limitexceeded').parents('li').removeClass('bgLimitExceeded');
    }

    var defects = $('.level3Group:visible .level3.boolean').children('.counters').children('span.response[value="0"]').length;

    var defectsTotalL1 = parseInt($('.level3Group:visible .total_defects').text());
    var defectsTotalL2 = parseInt($('.level3Group:visible .Defects').text());

    defectsTotalL1 += defects;
    defectsTotalL1 -= defectsRemove;

    defectsTotalL2 += defects;
    defectsTotalL2 -= defectsRemove;
    //
    $('.level3Group:visible .total_defects').text(defectsTotalL1);
    $('.level3Group:visible .Defects').text(defectsTotalL2);

    ////
});

$(document).on('click', '#btnAllNA', function (e) {

    setAllNA();

    var botaoNA = $(this);

    if (botaoNA.text() == 'Todos A') {
        $('.level3Group[level2id=' + $('.level2.selected').attr('id') + '] .level3[notavaliable="true"]').removeClass('bgNoAvaliable').removeAttr('notavaliable').children(".counters").find("input").val("");
    } else {
        $('.level3Group[level2id=' + $('.level2.selected').attr('id') + '] .level3:not([notavaliable])')
            .addClass('bgNoAvaliable').attr('notavaliable', 'true');
    }


    if ($($('.level3Group[level2id=' + $('.level2.selected').attr('id') + '] .level3:first')).attr('notavaliable') == "true") {
        $(this).text('Todos A');
    } else {
        $(this).text('Todos N/A');
    }

});

function setAllNA() {
    var defectsTotalL1 = parseInt($('.level3Group[level2id=' + $('.level2.selected').attr('id') + '] .total_defects').text());
    var defectsTotalL2 = parseInt($('.level3Group[level2id=' + $('.level2.selected').attr('id') + '] .Defects').text());

    var notconform = $('.level3Group[level2id=' + $('.level2.selected').attr('id') + '] .level3.boolean').children('.counters').children('span.response[value="0"]').parent().parent();

    var defectsRemove = 0;
    if (notconform.length > 0) {
        defectsRemove = notconform.length;
    }
    notconform.removeClass('lightred').removeAttr('notconform').children('.counters').children('span.response').attr('value', '1');

    notconform.children('.counters').children('span.response').text(notconform.children('.counters').children('span.response:first').attr('booltruename'));

    var level3 = $('.level3Group[level2id=' + $('.level2.selected').attr('id') + '] .level3');

    defectsTotalL1 -= defectsRemove;
    defectsTotalL2 -= defectsRemove;

    $('.level3Group[level2id=' + $('.level2.selected').attr('id') + '] .total_defects').text(defectsTotalL1);
    $('.level3Group[level2id=' + $('.level2.selected').attr('id') + '] .Defects').text(defectsTotalL2);
}

$(document).on('click', '.level3 .btnNotAvaliable', function (e) {

    var notconform = $(this).parents('.level3').children('.counters').children('span.response[value="0"]').parent().parent();

    var defectsRemove = 0;
    if (notconform.length > 0) {
        defectsRemove = notconform.length;
    }

    notconform.removeClass('lightred').removeAttr('notconform').children('.counters').children('span.response').attr('value', '1');
    notconform.children('.counters').children('span.response').text(notconform.children('.counters').children('span.response:first').attr('booltruename'));

    var defectsTotalL1 = parseInt($('.level3Group:visible .total_defects').text());
    var defectsTotalL2 = parseInt($('.level3Group:visible .Defects').text());

    defectsTotalL1 -= defectsRemove;
    defectsTotalL2 -= defectsRemove;

    var level3 = $(this).parents('.level3');
    if (level3.attr('notavaliable') == "true") {
        level3.removeClass('bgNoAvaliable').removeAttr('notavaliable');
        $(level3).children(".counters").find("input").val("");
    }
    else {
        level3.addClass('bgNoAvaliable').attr('notavaliable', 'true');
    }

    $('.level3Group:visible .total_defects').text(defectsTotalL1);
    $('.level3Group:visible .Defects').text(defectsTotalL2);

    $('input.likert').trigger('blur');

});

function saveResultLevel3() {

    var level1 = $(_level1).attr('id').split('98789');
    if (level1[1] == parseInt(getDicionario('IdIndicadorPesoHB'))) {

        mediaPesoHB.push(parseInt($('#' + getDicionario('IdTarefaPesoHB') + '.level3 input[type="text"]').val()));
        $('.level3List .calculoPesoHB .medicaCalculoPesoHB').text("Média: " + CalculoMediaPesoHB() + "g");

        if (!(typeof (ResetaCorMediaPesoHB) == "undefined"))
            ResetaCorMediaPesoHB();

        if (parseInt($('span.sampleTotal:visible').text()) <= parseInt($('span.sampleCurrent:visible').text())) {
            if (CalculoMediaPesoHB() < parseInt($('#' + getDicionario('IdTarefaPesoHB') + '.level3').attr('intervalmin'))) {
                var alertaatual = isNaN($(_level1).attr('alertaatual')) ? 1 : parseInt($(_level1).attr('alertaatual'));
                var mensagemHtml = "A Quantidade Média (g) foi menor do que 498g.";
                alertaatual++;
                openMessageModal(getResource("warning") + " " + alertaatual + " " + getResource("fired"), mensagemHtml, 'alerta');
                $(_level1).attr('alertaatual', alertaatual);
                setGravaAlertaDBLocal(_level1, alertaatual, 1, mensagemHtml);

                //força abertura da Ação Corretiva
                correctiveActionOpenPesoHB();
            }
            mediaPesoHB = [];
        }
    }

    //Level1 Selecionado
    var level1 = $('.level1.selected');
    var level2 = $('.level2.selected');


    if (!level1.attr('id') || level1.attr('id').replace('98789', '|').split('|').length < 2) {
        level1.attr('id', clusterAtivo.toString() + '98789' + level1.attr('id'))
    }

    if (!level2.attr('id') || level2.attr('id').replace('98789', '|').split('|').length < 2) {
        level2.attr('id', clusterAtivo.toString() + '98789' + level2.attr('id'))
    }

    updateHeaderCollection();

    if ($('.level1.VF.selected').length > 0)
        sendVer = true;

    //Verifica Total de defeitos no level2
    var defects = level2.attr('defects');
    if (level1.attr('hasgrouplevel2') != 'true')
        defects = 0;

    var isPartialSave = partialSaveCheck($('.level3Group[level2id=' + $('.level2.selected').attr('id') + ']'));

    //data da coleta    
    var date = getCollectionDate();
    //periodo da coleta
    var period = $('.App').attr('period');
    if (isNaN(period)) {
        period = 1;
    }
    var level01SaveCadastrado = false;
    //Buscar o resultado do periodo.

    ///// Seleciona o Set Corrente
    var evaluateTotal = parseInt(level2.attr('evaluate'));
    //Seleciona o Side Current
    var sampleTotal = parseInt(level2.attr('sample'));

    var updateSample = false;
    var evaluateCurrent = level2.attr('evaluatecurrent') != undefined ? parseInt(level2.attr('evaluatecurrent')) : 1;

    if (level2.attr('isreaudit') == "true") {
        evaluateCurrent = parseInt(level2.attr('reauditevaluation'));
    }

    var sampleCurrent = parseInt($('.level3Group[level1idgroup=' + $('.level1.selected').attr('id') + '] .painelLevel03 .sampleCurrent').text());
    if (isNaN(sampleCurrent))
        var sampleCurrent = level2.attr('samplecurrent');

    if ($('.level3Group.BEA').is(':visible')) {
        if (!sampleCurrent) {
            sampleCurrent = 0;
        }
    } else {
        if (!sampleCurrent) {
            sampleCurrent = 0;
        } else {
            if (isPartialSave && sampleCurrent) {
                sampleCurrent--;
            }
        }
        if (sampleCurrent < sampleTotal && level1.attr('hasgrouplevel2') != "true")
            sampleCurrent++;
    }

    var sequential = 0;
    var side = 0;

    if ($('.level3Group.PCC1B').is(':visible')) {
        sequential = $('.sequencial:visible').val();
        side = $('.banda:visible').val();
    }

    //Inicio reaudit como falso.
    var reaudit = false;
    //se for reauditoria

    var reauditNumber = level2.attr('reauditnumber') == undefined ? 0 : parseInt(level2.attr('reauditnumber'));

    if (level2.attr('isreaudit') == "true" || level1.attr('isreaudit') == "true") {
        reaudit = true;
    }

    var evaluateCurrentCheck = evaluateCurrent;
    var sampleDuplicatedCheck = sampleCurrent;
    var hashkey = $('.level1.selected').attr('hashkey');

    if (level1.attr('hashkey') == "1") {
        evaluateCurrentCheck = sequential;
        sampleDuplicatedCheck = side;
    }

    var reauditNumberVisible = parseInt($('.level3Group[level1id=' + $('.level1.selected').attr('id') + '][level2id=' + $('.level2.selected').attr('id') + '] .reauditFlag .reauditnumber').text());

    if ($(_level1).attr('isreaudit') == 'true') {
        if (isNaN($(_level1).attr('reauditnumber')) == false) {
            reauditNumberVisible = parseInt($(_level1).attr('reauditnumber'));
            //reauditNumberVisible++;
        }
    } else if (level2.attr('isreaudit') == 'true') {
        if (level2.attr('reauditnumber') > 0)
            reauditNumberVisible = level2.attr('reauditnumber');
        else
            reauditNumberVisible = 1;
    }

    if (level1.attr('hasgrouplevel2') == 'true' && level1.attr('isreaudit') == 'true') {
        evaluateCurrentCheck = parseInt(level1.attr('lastevaluate'));
    }

    var duplicatedKey = keyDuplicatedCheck(level1, level2, evaluateCurrentCheck, sampleDuplicatedCheck, date, reaudit, reauditNumberVisible);
    if (($(level1).hasClass('PCC1B') || $(level1).hasClass('VF')) && duplicatedKey == true && level1.attr('hashkey') != "1" && $(_level1).attr('ispartialsave') != "true") {

        openMessageModal(getResource("duplicated_collection"), getResource("collection_dropped"));
        updateEvaluateSample(level2, $('.level3Group[level2id=' + level2.attr('id') + ']'), evaluateCurrentCheck, sampleDuplicatedCheck);
        if (currenteSample == sampleTotal) {
            //evaluateCurrent = evaluateCurrent + 1;
            sampleCurrent = 0;
            completeLevel2(level2, evaluateCurrentCheck, sampleDuplicatedCheck);
            $('#btnSave').addClass('hide');
            $('.level3Group[level2id=' + level2.attr('id') + ']')
            resetLevel3($('.level3Group[level2id=' + level2.attr('id') + ']'));
            setTimeout(function (e) {
                openLevel2(level1);
            }, 200);

        }
        return true;
    }
    else if (($(level1).hasClass('PCC1B') || $(level1).hasClass('VF')) && duplicatedKey == true && level1.attr('hashkey') == "1" && $(_level1).attr('ispartialsave') != "true") {
        openMessageModal("Coleta Duplicada", "Verificamos que a coleta que está fazendo já foi lançada. A coleta antiga será substituida pela nova coleta");
    }

    //aqui vai evaluate
    var level01Save = $('.level01Result[level01Id=' + level1.attr('id') + '][date="' + date + '"][shift=' + $('.App').attr('shift') + '][unidadeid=' + $('.App').attr('unidadeid') + '][period=' + period + '][evaluate=' + evaluateCurrent + '][reaudit=' + reaudit + '][reauditnumber=' + reauditNumber + ']');

    if (level01Save.length) {
        level01SaveCadastrado = true;
        level01Save.attr('datetime', dateTimeFormat());
    }
    else {
        //atualizar outros itens que tem no saveLevel01
        //Se não existir resultado gera um level01Result novo.

        level01Save = $(saveLevel01(level1.attr('id'), date, $('.App').attr('unidadeid'), $('.App').attr('shift'), period, reaudit, reauditNumber, null, null, null, null, null, null, evaluateCurrent, hashkey));
    }

    //Removo selected do level01Result.
    $('.level01Result').removeClass('selected');
    //Seleciona o level01Result.
    level01Save.addClass('selected');

    var phase = 0;

    var startPhaseDate = level2.attr('startphasedate');

    if (!startPhaseDate) {
        startPhaseDate = getCollectionDate();
    } else if (startPhaseDate == "01010001") {
        startPhaseDate = getCollectionDate();
    }

    var level02Save = "";

    var level2Group = $('.level2.selected');

    level2Group.each(function (e) {

        level2 = $(this);

        var sampleVisible = parseInt($('.level3Group[level2id=' + $('.level2.selected').attr('id') + '] .painelLevel03 .sampleCurrent').text());
        var evaluateVisible = parseInt($('.level3Group[level2id=' + $('.level2.selected').attr('id') + '] .painelLevel03 .evaluateCurrent').text());

        if (!reauditNumberVisible) {
            reauditNumberVisible = 0;
        }

        if ($('.level1.selected').attr('hasgrouplevel2') == "true") {
            sampleVisible = parseInt($('.level3Group[level1idgroup=' + $('.level1.selected').attr('id') + '] .painelLevel03 .sampleCurrent').text());
            evaluateVisible = parseInt($('.level3Group[level1idgroup=' + $('.level1.selected').attr('id') + '] .painelLevel03 .evaluateCurrent').text());
        }

        if (reaudit == false && (level1.attr('reaudminlevel') > 0))
            reaudit = true;

        level02Save = level01Save.children('.level02Result[level01id=' + level1.attr('id') + '][level02id=' + level2.attr('id') + '][unidadeid=' + $('.App').attr('unidadeid') + '][date="' + date + '"][shift=' + $('.App').attr('shift') + '][period=' + period + '][evaluate=' + evaluateVisible + '][sample=' + sampleVisible + '][reaudit=' + reaudit + '][reauditnumber=' + reauditNumberVisible + '][phase=' + phase + ']');

        if ($('.level1.PCC1B.selected').length > 0) {
            level02Save = level01Save.children('.level02Result[level01id=' + level1.attr('id') + '][level02id=' + level2.attr('id') + '][unidadeid=' + $('.App').attr('unidadeid') + '][date="' + date + '"][shift=' + $('.App').attr('shift') + '][period=' + period + '][sequential=' + $('.sequencial:visible').val() + '][side=' + $('.banda:visible').val() + '][reaudit=' + reaudit + '][reauditnumber=' + reauditNumberVisible + '][phase=' + phase + ']');
        }

        if (($(level02Save).attr('havereaudit') == "true") || (level1.attr('reauditnumber') > 0)) {
            level02Save = $("#CriaUmaNova");
            reaudit = true;
            if (reauditNumberVisible == undefined || reauditNumberVisible == 0)
                reauditNumberVisible = 1;

            if (($('.level02Result[level02id=' + level2.attr('id') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + ']:last').attr('reauditnumber') > 0)
                && level1.attr('isreaudit') != 'true')
                reauditNumberVisible = parseInt($('.level02Result[level02id=' + level2.attr('id') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + ']:last').attr('reauditnumber')) + 1;

        } else if (level02Save.length == 0 && reaudit == true) {

            var UltRNumberLevel2 = $('.Resultlevel2[level2id=' + level2.attr('id') + '][havereaudit=true][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period')
                + ']:last').attr('reauditnumber');

            if ($('.Resultlevel2[level2id=' + level2.attr('id') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + '][reauditnumber=' +
                (UltRNumberLevel2 + 1) + ']').length == 0)
                UltRNumberLevel2++;

            if (UltRNumberLevel2 > 0)
                reauditNumberVisible = UltRNumberLevel2;
        }

        //Se existir um level02Result.
        var level02SaveCadastrado = false;

        if (level02Save.length) {
            //Altero level02SaveCadastrado para true.
            level02SaveCadastrado = true;
            //Atualiza resultado para refazer sincronização da atualização.
            level02Save.attr('datetime', dateTimeFormat())
                .attr('auditorid', $('.App').attr('userid'))
                .attr('defects', level2.attr('defects'))
                .attr('consecutivefailurelevel', level2.attr('consecutivefailurelevel'))
                .attr('consecutivefailuretotal', level2.attr('consecutivefailuretotal'))
                .attr('sync', 'false');

            //Se um resultado avaliado foi alterado para não avaliado.
            if (level2.attr('notavaliable')) {
                //Remove os resultados do level2 para incluir novamente com os valores padrão (0 defeitos).
                level02Save.children('.level02Result').remove();
                //Informa que o level2 é não avaliado.
                level02Save.attr('notavaliable', 'true');
            }
            else {
                //Se altera um resultado não availado para avaliado, remove a tag somente.
                level02Save.removeAttr('notavaliable');
            }
        }
        else {

            var headerList = "";

            //montar array
            //fazer header de level2
            $('.painelLevel02 .form-group select:visible, .painelLevel02 .form-group input:visible, .painelLevel03 .form-group select:visible, .painelLevel03 .form-group input:visible').each(function (index, self) {
                if ($(self).val() && $(self).val().length > 0) {
                    var type = $(self).attr('parfieldtype_id');
                    var value = $(self).val().replace(",", ".");
                    var id = $(self).attr('parheaderfield_id');

                    if (value != undefined && value != "") {
                        var hashkey = $(self).children('option:selected').attr('hashkey');
                        if (!!hashkey && $(self).attr('parfieldtype_id') == "1") {
                            value = hashkey;
                        }

                        var evaluation = 0;
                        var sample = 0;
                        var defectsTotal = 0;
                        var reauditIs = 0;
                        var reauditNumber = 0;
                        var haveReaudit = 0;

                        if ($(self).attr('linknumberevaluetion') == "true") {
                            evaluation = $('.level3Group[level1id=' + $('.level1.selected').attr('id') + '][level2id=' + $('.level2.selected').attr('id') + '] .headerEvaluation .headerEvaluationCount').text();
                            sample = $('.level3Group[level1id=' + $('.level1.selected').attr('id') + '][level2id=' + $('.level2.selected').attr('id') + '] .headerSample .headerSampleCount').text();

                            var headersResult = $.grep(getFilteredHeaderResultList(), function (o, i) {
                                return o.ParLevel1_Id == $('.level1.selected').attr('id')
                                    && o.Shift == $('.App').attr('shift') && o.Period == period
                                    && o.ParLevel2_Id == $('.level2.selected').attr('id')
                                    && o.Evaluation == evaluation
                                    && o.Sample == sample
                                    && o.ReauditNumber == ReauditByHeader.CurrentReauditNumber;
                            });

                            headersResult.forEach(function (o) {
                                defectsTotal = parseInt(o.Defects);
                                reauditIs = o.ReauditIs ? "1" : "0";
                                reauditNumber = ReauditByHeader.CurrentReauditNumber;
                                haveReaudit = o.HaveReaudit ? "1" : "0";
                            });

                        }

                        headerList += "<header>" + id + "," + type + "," + value + "," + evaluation + "," + sample + "," + defectsTotal + "," + reauditIs + "," + reauditNumber + "," + haveReaudit + "</header>";
                    }
                }

            });

            if ($('.level3Group.BEA').is(':visible')) {
                //Se não existir level02Result gera um novo.
                level02Save =
                    $(saveLevel02(
                        level1.attr('id'),
                        level2.attr('id'),
                        $('.App').attr('unidadeid'),
                        date,
                        dateTimeFormat(),
                        $('.App').attr('userid'),
                        $('.App').attr('shift'),
                        period,
                        $('.level3Group[level2id=' + $('.level2.selected').attr('id') + '] .painelLevel03 .evaluateCurrent').text(),
                        parseInt(sampleVisible) - 1 + parseInt($('.pecasAvaliadas:visible').val()),
                        level2.attr('defects'),
                        reaudit,
                        reauditNumberVisible,
                        phase,
                        startPhaseDate,
                        level2.attr('consecutivefailurelevel'),
                        level2.attr('consecutivefailurehtotal'),
                        level2.attr('notavaliable'),
                        headerList
                    ));
            } else {
                //Se não existir level02Result gera um novo.
                level02Save =
                    $(saveLevel02(
                        level1.attr('id'),
                        level2.attr('id'),
                        $('.App').attr('unidadeid'),
                        date,
                        dateTimeFormat(),
                        $('.App').attr('userid'),
                        $('.App').attr('shift'),
                        period,
                        evaluateVisible,
                        sampleVisible,
                        level2.attr('defects'),
                        reaudit,
                        reauditNumberVisible,
                        phase,
                        startPhaseDate,
                        level2.attr('consecutivefailurelevel'),
                        level2.attr('consecutivefailuretotal'),
                        level2.attr('notavaliable'),
                        headerList,
                        null,
                        sequential,
                        side
                    ));
            }
        }

        var punishmentvalue = getPunishment();

        level02Save.attr('hassampletotal', level2.attr('hassampletotal'));

        var accordeonId = "";

        if (level1.attr('hasgrouplevel2') == "true") {

            accordeonId = '.level2[id=' + level2.attr('id') + ']';
        }

        var inputList = $('.level3Group[level2id=' + $('.level2.selected').attr('id') + '] ' + accordeonId + ' .level3:not(".hide") input')

        if ($('.level1.selected').attr('hasgrouplevel2') == "true") {
            inputList = $('.level2[id=' + $('.level2.selected').attr('id') + '] .level3 input');
        }

        inputList.each(function (e) {

            //Se tem avaliação parcial
            if ($(_level1).attr('ispartialsave') == "true" && $(this).val() == "") {
                return;
            }

            //se for o input 2 do campo caculado pula ele
            if ($(this).hasClass('input02')) {
                return;
            }

            ///***********ainda nao contempla notação********////
            //Seleciona o level3 atual.
            var level3 = $(this).parents('.level3');

            //Instancio o level03SaveCadastrado como não cadastrado.
            var level03SaveCadastrado = false;

            //Verifica se existe level03Result.
            var level03Save = level02Save.children('.level03Result[level03id=' + level3.attr('id') + ']');

            //Define o resultado como conforme (padrão).
            var conform = true;

            var value = ReplaceVirgula($(this).val());

            //Se o valor de defeito for maior que zero o resultado é não conforme.
            if (parseFloat(value) > 0) {
                conform = false;
            }

            if (!parseFloat(value)) {
                value = 0;
            }

            //var inputType = parseInt($('.level3[id=' + level3.attr('level03id') + ']').attr('inputtype'));

            /*****
            SE FOR CAMPO CALCULADO, PRECISAMOS REVERTER O VALOR PARA DECIMAL
            *****/

            var inputType = parseInt($(this).parents('.level3').attr('inputtype'));

            //Calcula valor do Campo Calculado
            if (inputType == 4) {

                var valor1 = $(this).val().replace(',', '.');
                var valor2 = $(this).siblings('.input02').val().replace(',', '.');

                value = converteNotacaoBaseDezParaDecimal(valor1, valor2);
                //value = replaceWithDevice(value);
            } else if (inputType == 6) {
                level3.attr('value', $(level3).find('input').val());
                value = !level3.attr('notconform');
                conform = value;
            } else if (inputType == 7) {

                var antes = $(level3).find('.antes').val();
                var depois = $(level3).find('.depois').val();
                level3.attr('value', antes + "|" + depois);
                value = HourToMinutes(depois) - HourToMinutes(antes);
                conform = (value >= parseFloat(level3.attr('intervalmin')) && value <= parseFloat(level3.attr('intervalmax')));
            } else if (inputType == 9) {
                level3.attr('value', $(level3).find('.levelValueNotes').val());
                value = (level3).find('.levelValue').val()
                conform = (value >= parseFloat(level3.attr('intervalmin')) && value <= parseFloat(level3.attr('intervalmax')));
            }

            //Se tenho level03Result atualizo.
            if (level03Save.length) {

                //Altera level03SaveCadastrado para true.
                level03SaveCadastrado = true;
                //Altera valores necessários.
                if (inputType != 7 && inputType != 9) {
                    level03Save.attr('value', value)
                        .attr('conform', conform)
                        .attr('auditorId', $('.App').attr('userid'))
                        .attr('weight', level3.attr('weight'));
                }

            }
            else {
                var valorDefeitoTexto = 0;

                if ($(this).hasClass('texto') && $(this).val() != "") {
                    valorDefeitoTexto = parseFloat(1);
                    value = 1;
                    conform = false;
                } else if ((inputType == 7 || inputType == 6) && conform == false) {
                    valorDefeitoTexto = parseFloat(1);
                } else {
                    valorDefeitoTexto = parseFloat($(this).val());
                }

                defects += valorDefeitoTexto;

                //defects += parseFloat($(this).val());

                var level03Save = $(saveLevel03(
                    level3.attr('id'),
                    value,
                    conform,
                    $('.App').attr('userid'),
                    null,
                    level3.attr('value'),
                    level3.attr('weight'),
                    punishmentvalue,
                    level3.attr('intervalmin'),
                    level3.attr('intervalmax'),
                    level3.attr('notavaliable'),
                    valorDefeitoTexto));
            }

            //Adiciona os resultados no level2 se level3 não está cadastrado.
            if (level03SaveCadastrado == false) {
                appendDevice(level03Save, level02Save);
            }

            // if($('.level1.CFF.selected').length > 0){  
            //     setDefectsByEvaluation($('.level1.CFF.selected').attr('id'), getCollectionDate(), evaluateCurrent, sampleCurrent,
            //                                 $('.App').attr('period'), $('.App').attr('shift'), parseInt(value));
            // }

            if (inputType == 2)
                $(this).val(0);
            else if (inputType == 4) {
                $(this).val("");
                $(this).siblings('.input02').val("");
            } else if (inputType == 9) {
                $(this).val("");
                $(this).siblings('textarea').val("")
            } else
                $(this).val("");

        });

        var responseList = $('.level3Group[level2id=' + $('.level2.selected').attr('id') + '] ' + accordeonId + ' .level3:not(".hide") span.response');

        if ($('.level1.selected').attr('hasgrouplevel2') == "true") {
            responseList = $('.level2[id=' + $('.level2.selected').attr('id') + '] .level3 span.response');
        }

        responseList.each(function (e) {

            var valorDefeito = 0;

            var level3 = $(this).parents('.level3');
            //var level03show = level02.attr('level03show').split(';');

            var level03Save = level02Save.children('.level03Result[level03id=' + level3.attr('id') + ']');

            //precisa verificar onde está setando zero!!
            if (level1.attr('editlevel2') != "true")
                if ($('.level2Group').is(':visible') || $('.level1List').is(':visible')) {
                    $(this).attr('value', '1');
                }

            var conform = true;
            //Se tem defeitos
            if (parseInt($(this).attr('value')) == 0) {
                conform = false;
                valorDefeito++;
            }

            if (level03Save.length) {
                level03Save.attr('value', $(this).val())
                    .attr('conform', conform)
                    .attr('auditorId', $('.App').attr('userid'));
            }
            else {
                var level03Save = $(saveLevel03(
                    level3.attr('id'),
                    $(this).val(),
                    conform,
                    $('.App').attr('userid'),
                    null,
                    null,
                    level3.attr('weight'),
                    punishmentvalue,
                    level3.attr('intervalmin'),
                    level3.attr('intervalmax'),
                    level3.attr('notavaliable'),
                    valorDefeito));
            }
            appendDevice(level03Save, level02Save);


        });

        if (level1.hasClass('reprocesso')) {
            var objReprocesso = {};

            var value = $('.level3Group[level1id=' + $('.level1.selected').attr('id') + '][level2id=' + $('.level2.selected').attr('id') + '] .header.reprocesso:first select').val();

            objReprocesso.parReprocessoCertificadosSaidaOP =
                $.grep(reprocessoLists.parReprocessoCertificadosSaidaOP, function (elem) {
                    return elem.nCdOrdemProducao == value;
                });

            objReprocesso.parReprocessoEntradaOPs =
                $.grep(reprocessoLists.parReprocessoEntradaOPs, function (elem) {
                    return elem.nCdOrdemProducao == value;
                });

            objReprocesso.parReprocessoHeaderOPs =
                $.grep(reprocessoLists.parReprocessoHeaderOPs, function (elem) {
                    return elem.nCdOrdemProducao == value;
                });

            objReprocesso.parReprocessoSaidaOPs =
                $.grep(reprocessoLists.parReprocessoSaidaOPs, function (elem) {
                    return elem.nCdOrdemProducao == value;
                });

            level02Save.attr('objReprocesso', JSON.stringify(objReprocesso));
            console.log(objReprocesso);
        }

        if (level02SaveCadastrado == false) {
            level02Save.attr('defects', defects);
            appendDevice(level02Save, level01Save);
        }

        //Soma Amostra
        //Atualiza a Amostra no level2
        //Adiciona resultados do level2 para o level1 se o não estiver cadastrado.

        //Adiciono o level1 aos resultados se o level1 não estiver cadastrado.
        if (level01SaveCadastrado == false) {
            appendDevice(level01Save, $('.Results'));
        }

        if ($('.level3Group.BEA').is(':visible')) {
            sampleCurrent = parseInt(sampleCurrent) + parseInt($('.pecasAvaliadas:visible').val());
        }

        var evString = JSON.stringify(evaluateCurrent);
        var smpString = JSON.stringify(sampleCurrent);
        gravaDefeitosLeveis3(evString, smpString, level2, reaudit);

        setStartPhase(level02Save);

        setValoresLevel1Alertas(level1, level2, level02Save);

        var haveReaudit = level02Save.attr('havereaudit') == undefined ? false : level02Save.attr('havereaudit');
        var reauditLevel = level02Save.attr('reauditlevel') == undefined ? 0 : level02Save.attr('reauditlevel');

        var reauditNumber = 0;
        if (reaudit) {
            reauditNumber = reauditNumberVisible;
        }

        if (level1.attr('hasgrouplevel2') == 'true' && level1.attr('isreaudit') == 'true') {
            if ($('.Resultlevel2[level1id=' + level1.attr('id') + '][reauditnumber=' + reauditNumber + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period')
                + '][havereaudit=true]').length > 0) {
                haveReaudit = true;
                reauditLevel = 1;
            }
        }

        var level2ConsolidationResults = $('.Resultlevel2[level1id=' + level1.attr('id') + '][level2id=' + level2.attr('id') + '][collectiondate="' + date +
            '"][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + '][unitid=' + $('.App').attr('unidadeid') + '][reauditnumber=' +
            reauditNumber + ']');

        if (level2ConsolidationResults.length > 0) {
            if (level1.attr('isreaudit') != "true" && level2.attr('isreaudit') != "true" && level2ConsolidationResults.attr('havereaudit') == "true") {
                haveReaudit = true;
                level02Save.attr('havereaudit', true);
                reauditLevel = level2ConsolidationResults.attr('reauditlevel');
                level02Save.attr('reauditlevel', reauditLevel);
                reauditNumber = level2ConsolidationResults.attr('reauditnumber');
                level02Save.attr('reauditNumber', reauditNumber);
            } else if (level1.attr('isreaudit') == "true" && $('.level2Group[level01id=' + level1.attr('id') + '] .level2[controlereaud]').length == 1) {
                if ($('.level2Group[level01id=' + level1.attr('id') + '] .level2[havereaudit=true]').length > 0) {
                    reauditNumber = parseInt(level1.attr('reauditnumber'));
                    haveReaudit = true;

                    level02Save.attr('reauditlevel', '1');
                    level02Save.attr('reauditNumber', reauditNumber);
                    level02Save.attr('havereaudit', true);
                    level1.removeAttr('isreaudit');
                }
            } else if (level1.attr('isreaudit') == "true" && level1.attr('hasgrouplevel2') != 'true') {
                level02Save.removeAttr('havereaudit', '0');
                haveReaudit = false;
            }
        }

        var level3ComDefeitos = 0;
        var numeroDeLevel2;

        if (reauditNumber > 0) {
            numeroDeLevel2 = $('.level01Result.selected').children('.level02Result[level02id=' + $('.level2.selected').attr('id')
                + '][evaluate=' + evaluateCurrent + '][sample=' + sampleCurrent + '][reauditnumber=' + reauditNumber + ']');
        } else {
            numeroDeLevel2 = $('.level01Result.selected').children('.level02Result[level02id=' + $('.level2.selected').attr('id')
                + '][evaluate=' + evaluateCurrent + '][sample=' + sampleCurrent + ']');
        }

        //***Para bem estar animal ***////
        //****importante colocar outras opcoes aqui ///****
        if ($('.level1.BEA').hasClass('selected'))
            numeroDeLevel2 = $('.level01Result.selected').children('.level02Result:last');

        numeroDeLevel2.each(function (e) {
            var level2Resultado = $(this);
            var numeroDeLevel3 = level2Resultado.children('.level03Result');

            //Soma dos atributos do Level3
            numeroDeLevel3.each(function (e) {
                var level3 = $(this);
                if (parseInt(level3.attr('defects')) > 0)
                    level3ComDefeitos++;
            });
        });

        level2.attr('totallevel3withdefects', level3ComDefeitos);

        updateLevel2Consolidation(
            level2ConsolidationResults,                              //  level2Result,
            evaluateCurrent,                                         //  evaluateCurrent,
            sampleCurrent,                                           //  samplecurrent,
            level1.attr('alertaatual'),                              //  alertlevell1,
            level1.attr('weievaluation'),                            //  weievaluationl1,
            level1.attr('totalavaliado'),                            //  evaluatetotall1,
            level1.attr('totaldefeitos'),                             //  defectstotall1,
            level1.attr('weidefects'),                               //  weidefectsl1,
            level1.attr('totallevel3evaluation'),                    //  totallevel3evaluationl1,
            level1.attr('totallevel3withdefects'),                   //  totallevel3withdefectsl1,
            level1.attr('avaliacaoultimoalerta'),                    //  lastevaluationalertl1,
            level1.attr('monitoramentoultimoalerta'),
            level1.attr('totalavaliado'),                             //  evaluatedresultl1,
            level1.attr('totaldefeitos'),                             //  defectsresultl1,
            level2.attr('weievaluation'),                            //  weievaluationl2,
            level2.attr('resultadodefeitos'),                        //  resultadodefeitos,
            level2.attr('totaldefeitos'),                            //  defectsl2,
            level02Save.attr('weidefects'),                          //  weidefectsl2,
            level2.attr('totallevel3withdefects'),                   //  totallevel3withdefectsl2,
            level2.attr('totallevel3withdefects'),                   //  totallevel3withdefectsl2,
            level2.attr('totallevel3evaluation'),                    //  totallevel3evaluationl2,
            sequential,                                              //  sequential,
            side,
            reauditNumber,
            haveReaudit,
            reauditLevel);

        level02Save.children('.level03Result').each(function (e) {
            partialSave(level1, level2, $(this).attr('level03id'));
        });

    });

    if (isEUA) {
        cacheGlobalAlerta.reauditNumber = $('.Resultlevel2[level1id=' + $('.level1.selected').attr('id') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + ']:last').attr('reauditnumber');
        MakeObject2($('.Resultlevel2[level1id=' + $('.level1.selected').attr('id') + '][period=' + $('.App').attr('period') + '][shift=' + $('.App').attr('shift') + '][reauditnumber=' + cacheGlobalAlerta.reauditNumber + ']'), 'DefeitosLevel2', cacheGlobalAlerta.resultLevel2);
        controleDeAlerta(level2.attr('id'));
    }

    level2 = $('.level2.selected');

    level01Save.attr('totalavaliado', level1.attr('totalavaliado'));
    //level01Save.attr('totalavaliado', level1.attr('totalavaliado'));

    level01Save.attr('alertanivel1', level1.attr('alertanivel1'));
    level01Save.attr('alertanivel2', level1.attr('alertanivel2'));
    level01Save.attr('alertanivel3', level1.attr('alertanivel3'));

    level01Save.attr('alertaatual', level1.attr('alertaatual'));
    level01Save.attr('avaliacaoultimoalerta', level1.attr('avaliacaoultimoalerta'));
    level01Save.attr('monitoramentoultimoalerta', level1.attr('monitoramentoultimoalerta'));

    if (level1.attr('havecorrectiveaction') == 'true') {
        level01Save.attr('havecorrectiveaction', 'havecorrectiveaction');
        level02Save.attr('havecorrectiveaction', 'havecorrectiveaction');
    }

    if (evaluateTotal != 0 && evaluateCurrent > evaluateTotal && level1.attr('islimitedevaluetionnumber') == "true") {
        level01Save.attr('hascompleteevaluation', 'true');
        openMessageModal(getResource("evaluation_complete"), null);
        return openLevel2(level1);
    }
    else {
        level02Save.attr('isemptylevel3', 'false');
    }

    $('.level3Group[level2id=' + $('.level2.selected').attr('id') + '] .level3').removeClass('selected');

    var evaluateCurrentTemp = evaluateCurrent;
    var sampleCurrentTemp = sampleCurrent;

    if ($('.level1.selected').attr('hasgrouplevel2') == "true") {
        $('.button-collapse:visible').click();
    }

    if (level1.attr('phase1')) {
        checkPhase($('.level1.selected').attr('id'), $('.level2.selected').attr('id'), $('.App').attr('shift'), $('.App').attr('period'), parseInt(level2.attr('phase')));
    }

    setEndPhase(level02Save);

    var controle = false;

    if (parseInt(sampleTotal) != 0) {

        if (parseInt(sampleCurrent) == parseInt(sampleTotal)) {

            if (level1.attr('hasgrouplevel2') != "true") {
                evaluateCurrent = evaluateCurrent + 1;
            }

            if (level1.attr('hasgrouplevel2') == "true" && level2.attr('id') == $('.level2:visible:last').attr('id')) {
                evaluateCurrent = evaluateCurrent + 1;
            }

            if (evaluateCurrent > evaluateTotal && level1.attr('islimitedevaluetionnumber') == "true" &&
                level1.attr('hasgrouplevel2') == "true") {
                level1.attr('hascompleteevaluation', 'true');
                if (level1.attr('isreaudit'))
                    level1.attr('lastevaluate', level1.attr('reauditevaluation'));
            }

            sampleCurrent = 0;
            level01Save.attr('completedsample', 'completedsample');
            level01Save.attr('totalevaluate', evaluateCurrent);
            level2.attr('evaluatecurrent', evaluateCurrent);
            updateEvaluateSample(level2, $('.level3Group[level2id=' + $('.level2.selected').attr('id') + ']'), evaluateCurrent, sampleCurrent);
            resetLevel3($('.level3Group[level2id=' + $('.level2.selected').attr('id') + ']'));
            createFileResult();

            if (isPartialSave == true) {

                if (sampleCurrentTemp == 0)
                    level2.removeAttr('evaluatecurrent').removeAttr('samplecurrent');

            } else {

                if (level2.attr('isreaudit') != "true") {
                    completeLevel2(level2, evaluateCurrent, evaluateTotal);

                    if ($('.level1.selected').attr('isreaudit') != 'true' && level2.attr('isreaudit') != 'true')
                        setEvaluatedCounterLevel1(parseInt($('.level1.selected').attr('id')), parseInt(level2.attr('id')), evaluateCurrentTemp, parseInt($('.App').attr('shift')), parseInt($('.App').attr('period')));

                } else {
                    var reauditevaluation = parseInt(level2.attr('reauditevaluation')) + 1;
                    level2.attr('reauditevaluation', reauditevaluation);
                }
            }
            var evalAux = evaluateCurrent;

            if (evalAux > evaluateTotal && level1.attr('hasgrouplevel2') == 'true') {
                createFileResultConsolidation();
                $('#btnAllNA').text('Todos N/A');
                controle = true;
                //level1Show();
            }
            if (level2.attr('isreaudit') == 'true' && defects == 0 && evaluateCurrent > level2.attr('evaluate')) {

                if (isNaN(level2.attr('reauditnumber')) == false) {
                    var rnum = parseInt(level2.attr('reauditnumber')) + 1;
                    $('.level2Group[level01id=' + $('.level1.selected').attr('id') + '] .level2[id=' + level2.attr('id') + ']').attr('reauditnumber', rnum);
                    console.log($('.level2Group[level01id=' + $('.level1.selected').attr('id') + '] .level2[id=' + level2.attr('id') + ']').attr('reauditnumber'));
                }

                level2.attr('havereaudit', 'false');
            }

            if (level1.attr('hasgrouplevel2') == "true" && level2.attr('id') == $('.level2:visible:last').attr('id')) {
                openLevel2(level1);
            }

            if (sampleTotal != 0 && level1.attr('hasgrouplevel2') != "true") {

                if (level1.attr('editlevel2') != "true") {
                    openLevel2(level1);
                    scrollClick(parseInt(_level2.id));
                } else {
                    if (ultL2Temp) {
                        openLevel2(level1);
                        scrollClick(parseInt(_level2.id));
                    }
                }
            }
        }
        else if ($(_level1).attr('ispartialsave') == "true") {
            openLevel2(level1);
        }

    } else {

        sampleCurrent = parseInt(sampleCurrent) + 1;

        if (level1.attr('ispartialsave') == "true") {
            openLevel2(level1);
            scrollClick(parseInt(_level2.id));
        }
    }

    var level3GroupVisible = $('.level3Group[level2id=' + $('.level2.selected').attr('id') + ']');

    if (level3GroupVisible.length == 0)
        level3GroupVisible = $('.level3Group[level1idgroup=' + $('.level1.selected').attr('id') + ']');

    if ($('.level1.selected').attr('hasgrouplevel2') != "true") {
        resetLevel3(level3GroupVisible);
        updateEvaluateSample(level2, level3GroupVisible, evaluateCurrent, sampleCurrent);
        writeCounter(parseInt($('.level1.selected').attr('id')));
    }

    createFileResultConsolidation();
    $('#btnAllNA').text('Todos N/A');

    if ($('.level1.PCC1B.selected').length > 0 && parseInt($('.banda:visible').val()) == 2) {
        trocarSequencial = true;
        openModalPCC1BProximoSequencial();
        getPCC1BNext(true);
    }
    else if ($('.level1.PCC1B.selected').length > 0 && parseInt($('.banda:visible').val()) == 1) {
        openModalPCC1BProximoSequencial();
        offLinePCC1B();
    }

    if (level1.attr('hasgrouplevel2') == 'true') {
        if (level2.attr('id') == $($('.level2')[$('.level2').length - 1]).attr('id')) {
            resetCollapseLevel2();
            writeCounter(parseInt($('.level1.selected').attr('id')));
            if (parseInt(tdefAv.text()) == 0) {
                level1.attr('havereaudit', 'false');
                if (evaluateCurrent > level2.attr('evaluate'))
                    if (isNaN(level1.attr('reauditnumber')) == false) {
                        var rnum = parseInt(level1.attr('reauditnumber')) + 1;
                        level1.attr('reauditnumber', rnum);
                    }
            } if (controle)
                level1Show(false, clusterAtivo);
        }
    }

    $('.level3:visible').find('.camera-button').removeClass('btn-default').addClass('btn-danger');

    setEvaluationByHeadersSelection();

    configVinculoAvAm();

    $('input.likert').trigger('blur');

    exibirLevel3PorDepartamento();

}

function resetLevel3(level3Group) {

    var level2 = $('.level2.selected');
    var booleanInputs = level3Group.find('.level3').removeClass('lightred').removeClass('bgNoAvaliable').removeAttr('notavaliable').children('.counters').children('.response[value=0]');
    level3Group.find('.level3').removeClass('lightred').removeClass('bgNoAvaliable').removeAttr('notavaliable').children('.counters').children('.response[value=0]').parents('.level3').children('a');
    resetBooleanInput(booleanInputs);
    level3Group.find('.level3.texto').removeAttr('value');
    level3Group.find('.total_defects').text(defectsLevel1Total);
    level3Group.find('.defects_evaluate').text(defectsEvaluateTotal);

    //Resta Contadores da Amostra
    defectsEvaluateSample = 0;
    defects3MoreSample = 0;
    defectsLevel2Sample = 0;
    $('.level3Group[level2id=' + $('.level1.selected').attr('id') + '] .current_sample_defects').text(defectsLevel2Sample);
    updateCounters(level2, level3Group);

    getPadraoNumero();
}

$(document).on('click', '.button-expand', function (e) {
    $(this).parents('.level3Group').find(".panel-collapse.collapse").collapse("show");
});

$(document).on('click', '.button-collapse', function (e) {
    $(this).parents('.level3Group').find(".panel-collapse.in").collapse("hide");
});

$(document).on('focusout', 'input.sequencial:visible', function () {
    var chave = $('.App').attr('unidadeid') + "" + $(".level3Group.VF").attr('level1id') + "" + $('.sequencial:visible').val() + "" + $('.banda:visible').val() + "" + yyyyMMdd();
    if ($('.level1.selected.VF').length > 0 && $('select.banda:visible').val() != "") {
        if ($('.ResultsKeysVF').children('div[key=' + chave + ']').length > 0)
            openMessageModal(getResource("sequential_side_already_filled"), null);
    }
});

$(document).on('focusout', 'select.banda:visible', function () {
    var chave = $('.App').attr('unidadeid') + "" + $(".level3Group.VF").attr('level1id') + "" + $('.sequencial:visible').val() + "" + $('.banda:visible').val() + "" + yyyyMMdd();
    if ($('.level1.selected.VF').length > 0 && $('input.sequencial:visible').val() != "") {
        if ($('.ResultsKeysVF').children('div[key=' + chave + ']').length > 0)
            openMessageModal(getResource("sequential_side_already_filled"), null);
    }
});

function getPadraoNumero() {

    var tamanho = $('input.defects:visible').length;
    for (var i = 0; i < tamanho; i++) {
        $('input.defects:visible')[i].value = $($('input.defects:visible')[i]).attr('value');
    }

}

$(document).on('input', '.interval input, .calculado input, .defects input', function (e) {

    var parentElement = $(this).parents('.interval, .calculado, .defects, .inputMinutes');

    var antes = $(parentElement).find('.antes').val();
    var depois = $(parentElement).find('.depois').val();

    if (antes != undefined) {
        parentElement.attr('value', antes + "|" + depois);
        value = HourToMinutes(depois) - HourToMinutes(antes);
    } else {

        var value = parseFloat(ReplaceVirgula(this.value));
    }

    var intervalMin = parseFloat(ReplaceVirgula(parentElement.attr('intervalmin')));
    var intervalMax = parseFloat(ReplaceVirgula(parentElement.attr('intervalmax')));

    if (intervalMin > intervalMax) {
        var intervalMaxOld = intervalMax;
        intervalMax = intervalMin;
        intervalMin = intervalMaxOld;
    }


    if (parentElement.hasClass('calculado')) {
        var valor1 = parentElement.find('input.input01').val() == "" ? "0" : parentElement.find('input.input01').val();

        var valor2 = parentElement.find('input.input02').val() == "" ? "0" : parentElement.find('input.input02').val();

        var value = converteNotacaoBaseDezParaDecimal(valor1.replace(',', '.'), valor2.replace(',', '.'));

        var valueBase10 = converteDecimalParaNotacaoBaseDez(value);

        parentElement.find('.valueDecimal').text("   ou   " + valueBase10);
        parentElement.find('.value').text("Resultado: " + value);
    }

    if (value < intervalMin || value > intervalMax) {
        if (!parentElement.hasClass('lightred')) {
            defectsLevel1Total++;
            defectsLevel2Total++;
            defectsLevel2Sample++;
        }
    } else {
        if (parentElement.hasClass('lightred')) {
            defectsLevel1Total--;
            defectsLevel2Total--;
            defectsLevel2Sample--;
        }
    }

    parentElement.removeClass('lightred');
    if (value < intervalMin) {
        parentElement.addClass('lightred');
    }
    else if (value > intervalMax) {
        parentElement.addClass('lightred');
    }

    // Atualizar contador pela triger
    contadorLadoComDefeitos();
    contador3Defeitos();

    defectsLevel1Total++;

    $('.level3Group:visible .total_defects').text(defectsLevel1Total);
    $('.level3Group:visible .level2_defects').text(defectsLevel2Total);
    $('.level3Group:visible .current_sample_defects').text(defectsLevel2Sample);
    GetDefectsGroup();

    if ($('.level2Group .painelLevel02 .form-group .counter[counter=defects]').length > 0 ||
        (($('.counter[indicador=' + _level2.id + '][headerlevel=level2_line][counter=defects]').length > 0 ||
            $('.counter[indicador=' + _level1.id + '][headerlevel=level3_header][counter=defects]').length > 0) &&
            $('.painelLevel03 .defects:visible').length > 0)) {

        var defAtualHL2 = 0;

        $('.defects input:visible').each(function (index, self) {
            defAtualHL2 += parseFloat(ReplaceVirgula(self.value));
        });

        $('.interval input:visible').each(function (index, self) {
            var parent = $(this).parents('.interval');
            var min = parseFloat(ReplaceVirgula(parentElement.attr('intervalmin')));
            var max = parseFloat(ReplaceVirgula(parentElement.attr('intervalmax')));
            var value = parseFloat(ReplaceVirgula(self.value));

            if (value > max || value < min) {
                defAtualHL2 += 1;
            }
        });

        var defAtualHL3 = defAtualHL2;
        defAtualHL2 += tempHDL2;
        $('.counter[counter=defects] .value:visible').text(defAtualHL2);
        $('.level2Group .painelLevel02 .form-group .counter[counter=defects] .value').text(defAtualHL2);
    }

    if ($('.painelLevel03 .defects:visible').length > 0) {
        $('.painelLevel03 .defects:visible').text(defectsLevel2Total);
    }
});



//colocar uma forma de identificar campos yes or not
$(document).on('click', '.level3.boolean a, .level3.boolean .counters', function (e) {

    var level02 = $('.level2.selected');
    var level03 = $(this).parents('.level3');

    if (level03.attr('notavaliable') == "true") {
        return false;
    }

    var response = level03.children('.counters').children('span.response');

    //acho que demvemos fazer um atrivuto direto no level03 para nao ficar tentando executar para todos
    if (response.length) {
        if (response.attr('value') == '0') {
            response.text(response.attr('booltruename')).attr('value', '1');
            level03.removeClass('lightred').removeAttr('notconform');
            if ($('.level3Group .level03[notConform]').length == 0) {
                level02.removeAttr('limitexceeded').parents('li').removeClass('bgLimitExceeded');
            }
            defectsLevel1Total--;
            defectsLevel2Total--;
            defectsLevel2Sample--;
        }
        else {
            level03.addClass('lightred').attr('notConform', 'notCoform');
            response.text(response.attr('boolfalsename')).attr('value', '0');
            defectsLevel1Total++;
            defectsLevel2Total++;
            defectsLevel2Sample++;
        }
    }

    contadorLadoComDefeitos();
    contador3Defeitos();

    $('.level3Group:visible .total_defects').text(defectsLevel1Total);
    $('.level3Group:visible .level2_defects').text(defectsLevel2Total);
    $('.level3Group:visible .current_sample_defects').text(defectsLevel2Sample);

    GetDefectsGroup();

    if ($('.level2Group .painelLevel02 .form-group .counter[counter=defects]').length > 0) {
        var defAtualHL2 = 0;
        $('.defects input:visible').each(function (index, self) {
            defAtualHL2 += parseFloat(ReplaceVirgula(self.value));
        });
        //$('.counter[counter=defects] .value:visible').css({ 'color': 'red' }).text(defAtualHL2);
        var defAtualHL3 = defAtualHL2;
        defAtualHL2 += tempHDL2;
        $('.level2Group .painelLevel02 .form-group .counter[counter=defects] .value').text(defAtualHL2);
    }

    if ($('.painelLevel03 .defects:visible').length > 0) {
        $('.painelLevel03 .defects:visible').text(defectsLevel2Total);
        //$('.painelLevel03 .defects:visible').css({ 'color': 'red' });
    }
});

//CUIDAR DO CAMPO VAZIO
$(document).on('input', '.texto input', function (e) {

    var level03 = $(this).parents('.texto');

    var value = $(this).val();

    level03.removeAttr('value');
    level03.removeClass('lightred').removeAttr('notconform');
    level03.removeClass('lightred');
    if (value != "") {
        level03.attr('value', value);
    }

    if (value != "" && !level03.hasClass('naoValidarInput')) {
        level03.addClass('lightred');
        level03.addClass('lightred').attr('notConform', 'notCoform');
    }

});

function partialSaveCheck(level3Group) {

    if ($(_level1).attr('ispartialsave') == "true") {
        var totalInputs = level3Group.find('.level3').find('input').length;
        var inputsEmpty = checkInputEmpty(level3Group);

        if (totalInputs == inputsEmpty)
            return false;
        else
            return true;
    }
    else
        return false;

}
function checkInputEmpty(level3Group) {

    var inputs = level3Group.find('.level3').find('input:visible');

    var inputsEmpyts = 0;
    inputs.each(function (e) {

        if ($(this).val() == "") {
            inputsEmpyts++;
        }

    });
    return inputsEmpyts;
}


function defectLimitCheck() {
    var level02 = $('.level02.selected');

    var defectsLevel02 = parseInt(level02.attr('defects'));
    var defectsLimit = parseInt(level02.attr('levelerrorlimit'));

    var defectsDiv = $('.painelLevel03 .defects').parent('div');
    defectsDiv.removeClass('red');
    var btnNA = level02.siblings('.userInfo').children('div').children('.na');
    //btnNA.addClass('hide');

    if (defectsLevel02 > defectsLimit) {
        defectsDiv.addClass('red');
        level02.attr('limitExceeded', 'limitExceeded');
        if (!level02.attr('correctiveActionComplete'))
            level02.attr('havecorrectiveaction', 'havecorrectiveaction');
        level02.parents('li').addClass('bgLimitExceeded');
    }
    else {
        defectsDiv.removeClass('red');
        level02.removeAttr('limitExceeded');
        level02.parents('li').removeClass('bgLimitExceeded');
    }

}

function inputChangesUpdate2(input) {
    var level02 = $('.level02.selected');
    var level03 = input.parents('.level03');

    var valorInput = input.val();
    if (valorInput == "") {
        valorInput = "0";
    }

    var valorInputDefects = parseInt(valorInput);

    var level03Defects = 0;

    $('.level03Group:visible .level03 input').each(function (e) {

        var valor = $(this).val();
        if (valor == "") {
            valor = "0";
        }
        level03Defects = level03Defects + parseInt(valor);
    });

    var level03Group = $('.level03Group:visible');
    var sidesWithErros = parseInt($('.painelLevel02 .sideWithErrors').text());
    //os defeitos serao calculados por 3 defeitos ou mais e por 6 lados com defeito
    if (level03Defects == 0 && level03Group.attr('firstErrorSide')) {
        sidesWithErros = sidesWithErros - 1;
        level03Group.removeAttr('firstErrorSide');
    }
    else if (level03Defects > 0 && !level03Group.attr('firstErrorSide')) {

        sidesWithErros = sidesWithErros + 1;
        level03Group.attr('firstErrorSide', 'firstErrorSide');
    }

    $('.sideWithErrors').text(sidesWithErros);

    if (sidesWithErros > 5) {
        $('.sideWithErrors').parents('.labelPainel').addClass('red');
    }
    else {
        $('.sideWithErrors').parents('.labelPainel').removeClass('red');
    }

    var sidesWith3ErrosMore = parseInt($('.painelLevel02 .more3Defects').text());

    if (level03Defects < 3 && level03Group.attr('Error3MoreSide')) {
        sidesWith3ErrosMore = sidesWith3ErrosMore - 1;
        level03Group.removeAttr('Error3MoreSide');
        $('.sideErros').parents('.labelPainel').removeClass('red');

    }
    else if (level03Defects > 2 && !level03Group.attr('Error3MoreSide')) {
        sidesWith3ErrosMore = sidesWith3ErrosMore + 1;
        level03Group.attr('Error3MoreSide', 'Error3MoreSide');
        $('.sideErros').parents('.labelPainel').addClass('red');
    }

    $('.more3Defects').parents('.labelPainel').removeClass('red');

    if (sidesWith3ErrosMore > 0) {
        $('.more3Defects').parents('.labelPainel').addClass('red');
    }

    $('.painel .more3Defects').text(sidesWith3ErrosMore);

    var total6Erros = parseInt($('.sideWithErrors:first').text());

    var total3Erros = parseInt($('.more3Defects:first').text())

    //Culpa do Lucas
    if (total6Erros > 5 || total3Erros > 0) {
        level02.attr('defects', "1");
    }



    $('.painel .sideErros').text(level03Defects);

}

function inputChangesUpdate(input) {

    var level02 = $('.level02.selected');
    var level03 = input.parents('.level03');

    var valorInput = input.val();
    if (valorInput == "") {
        valorInput = "0";
    }

    var valorInputDefects = parseInt(valorInput);
    var level02Defects = 0;
    $('.level03Group:visible .level03 input').each(function (e) {

        var valor = $(this).val();
        if (valor == "") {
            valor = "0";
        }
        level02Defects = level02Defects + parseInt(valor);
    });

    level02.attr('defects', level02Defects);

    var level02ErrorLimit = parseInt(level02.attr('levelerrorlimit'));
    $('.painel .defects').text(level02Defects);

    if (level02Defects > level02ErrorLimit) {
        $('.painel .defects').parents('.labelPainel').addClass('red');
    }
    else {
        $('.painel .defects').parents('.labelPainel').removeClass('red');
    }


    level02.siblings('.userInfo').children('div').children('.defects').text(level02Defects);

    var level01Defects = 0;

    level02.parents('li').siblings('li').children('.row').children('.level02[defects]').each(function (e) {
        var valor = $(this).attr('defects');
        if (valor == "") {
            valor = "0";
        }
        level01Defects = level01Defects + parseInt(valor);

    });

    level01Defects = level01Defects + level02Defects;

    $('.painel .totalDefects').text(level01Defects);
    if (level01Defects > 22) {
        $('.painel .totalDefects').parents('.labelPainel').addClass('red');
    }
    else {
        $('.painel .totalDefects').parents('.labelPainel').removeClass('red');
    }

}

$(document).on('click', '.btn-plus', function (e) {
    var input = $(this).siblings('input');
    var oldValue = input.val();
    if (input.val() == "") {
        oldValue = 0;
    }
    var newVal = parseInt(oldValue) + 1;
    input.val(newVal);
    input.trigger('input');

    GetDefectsGroup();
});

$(document).on('click', '.btn-minus', function (e) {
    var input = $(this).siblings('input');
    var oldValue = input.val();
    if (input.val() == "") {
        oldValue = 0;
    }
    var newVal = parseInt(oldValue) - 1;
    input.val(newVal);
    input.trigger('input');
});

$.fn.sum = function () {
    var sum = 0;
    this.each(function () {
        sum += 1 * ($(this).val());
    });
    return sum;
};

function updateEvaluateSample(level2, level3Group, evaluateCurrent, sampleCurrent) {
    level2.attr('evaluatecurrent', evaluateCurrent);
    level2.attr('samplecurrent', sampleCurrent);
    // level2.parents('.list-group-item').children('.counters').children('div').children('.evaluateCurrent').text(evaluateCurrent);
    level2.parents('.list-group-item').children('.counters').children('div').children('.sampleCurrent').text(sampleCurrent);

    currentEvaluate = parseInt(level2.attr('evaluatecurrent'));
    currenteSample = parseInt(level2.attr('samplecurrent'));
    currenteSample = currenteSample + 1;

    if ($(_level1).attr('hasgrouplevel2') == 'true' && $(_level1).attr('isreaudit') == 'true') {
        currenteSample = parseInt($(_level1).attr('lastsample'));
        currentEvaluate = parseInt($(_level1).attr('lastevaluate'));
        // if (isNaN($(_level1).attr('reauditevaluation')) == false)
        //     currentEvaluate = parseInt($(_level1).attr('reauditevaluation'));
        if (currenteSample == parseInt(level2.attr('sample'))) {
            level2.attr('evaluateCurrent', currentEvaluate);
            level2.attr('sampleCurrent', currenteSample);
            currentEvaluate++;
        } else
            currenteSample++;

        if ($('.level2:visible').length == 1)
            currenteSample = $(_level1).attr('lastsample');
    }

    level3Group.find('.evaluateCurrent').text(currentEvaluate);
    level3Group.find('.sampleCurrent').text(currenteSample);

    $('.pecasAvaliadas:visible').val("");

    updateCounters(level2, level3Group);
}

function partialSave(level1, level2, level3_Id) {
    if (level1.attr('ispartialsave') == "true") {
        var resultConsolidation = $('.Resultlevel2[level1id=' + level1.attr('id') + '][level2id=' + level2.attr('id') + '][collectiondate="' + getCollectionDate() + '"][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + ']');

        if (resultConsolidation.length) {
            var resultLevel3 = $('<div id=' + level3_Id + ' class="r3l2"></div>');
            appendDevice(resultLevel3, resultConsolidation);
            //resultConsolidation.append(resultLevel3);

        }
    }
}

$(document).on('click', '#btnSave', function (e) {

    $('body').scrollTop(0);

    var btnSave = $(this);

    btnSave.attr('disabled', 'disabled');
    btnSave.children('#saveIcon').hide();
    btnSave.children('#loadIcon').show();

    if ($('.level2Group:visible').length > 0) {
        if ($('.level2Group[level01id=' + $('.level1.selected').attr('id') + '] .level2').not('[completed]').length > 0) {
            return;
        }
        $('.level2Group[level01id=' + $('.level1.selected').attr('id') + '] .level2').each(function (index, self) {
            $(self).attr('.sampleTotal')
        });
        $('.level1.selected').removeAttr('isreaudit');
        $('.level2Group[level01id=' + $('.level1.selected').attr('id') + '] .level2').removeAttr('isreaudit');
        level1Show(false, clusterAtivo);
    } else {

        setTimeout(function (e) {

            validaCamposLevel3(1);

            btnSave.children('#loadIcon').hide();
            btnSave.children('#saveIcon').show();
            btnSave.removeAttr('disabled');

        }, 100);
    }

});


function getPunishment() {
    //verificar se itens cabecalho level2 level3 juntos 
    var valor = 0;
    $('.header select:visible option:selected').each(function (e) {
        if ($(this).attr('punishmentvalue')) {
            var value = ReplaceVirgula($(this).attr('punishmentvalue').replace(',', '.'));
            var valorPunicao = parseFloat(value);
            valor += valorPunicao;
        }
    });
    return valor;
}

function ReplaceVirgula(value) {
    return value.replace(',', '.');
}

function resetCollapseLevel2() {
    var collapses = $('.level2.panel-group:visible');
    collapses.each(function (i, o) {
        $(o).find('input.defects').val(0);
        $(o).find('input.texto').val("");
        $(o).find('input.texto').parent().parent().parent().removeAttr('value');
        $(o).find('span.response').text(($(o).find('span.response').attr('booltruename')));
        $(o).find('span.response').attr('value', 1);
        $(o).find('.lightred').removeClass('lightred');
        $(o).find('.levelValue').text('');
        $(o).find('.level3').removeAttr('notconform');
        $(o).find('.level3').removeAttr('notavaliable');
        $(o).find('.level3').removeClass('bgNoAvaliable');
    });

    collapses = $('.level3List:visible')
    collapses.each(function (i, o) {
        $(o).find('input.defects').val(0);
        $(o).find('input.texto').val("");
        $(o).find('input.texto').parent().parent().parent().removeAttr('value');
        $(o).find('span.response').text(($(o).find('span.response').attr('booltruename')));
        $(o).find('span.response').attr('value', 1);
        $(o).find('.lightred').removeClass('lightred');
        $(o).find('.levelValue').text('');
        $(o).find('.level3').removeAttr('notconform');
        $(o).find('.level3').removeAttr('notavaliable');
        $(o).find('.level3').removeClass('bgNoAvaliable');
    });

    $('span[booltruename]').each(function (i, o) { $(o).html($(o).attr('booltruename')) });

}

//Atualizar contadores quando a amostra for agrupada
//Atualizar contadores quando for amostra
function GetDefectsGroup() {
    //var totalDefeito = parseInt($('#tdef').find('span').text());
    //var totalInicial = parseInt($('#tdef').find('span').text());
    //var quantidadeDefeitos = parseInt($('#tdefav').find('span').text());
    //$('.level2:visible').find('li').on('click', function () {
    //    quantidadeDefeitos = $('.level2:visible').find('li.lightred').length
    //    $('#tdefav').find('span').text(quantidadeDefeitos)
    //    $('#tdef').find('span').text(totalInicial + parseInt(quantidadeDefeitos))
    //});
    //$('.level2List:visible').find('li').on('click', function () {
    //    quantidadeDefeitos = $('.level2List:visible').find('li.lightred').length
    //    $('#tdefav').find('span').text(quantidadeDefeitos)
    //    $('#tdef').find('span').text(totalInicial + parseInt(quantidadeDefeitos))
    //});
    //$('.level3List:visible').find('li').on('click', function () {
    //    quantidadeDefeitos = $('.level3List:visible').find('li.lightred').length
    //    $('#tdefav').find('span').text(quantidadeDefeitos)
    //    $('#tdef').find('span').text(totalInicial + parseInt(quantidadeDefeitos))
    //});

    //$('salvar').on('click', function () {
    //    totalInicial = parseInt($('#tdef').find('span').text())
    //    $('#tdefav').find('span').text(o);
    //})

    //totalDefeito ++;
    //quantidadeDefeitos= $('.evaluateCurrent:visible').text();
    //$('#tdef').find('span').text(totalDefeito);
    //$('#tdefav').find('span').text(quantidadeDefeitos);
}
function GetDefectsGroupDecrement() {
    var totalDefeito = parseInt(tdef.text());
    var quantidadeDefeitos = parseInt(tdefAv.text());

    totalDefeito--;
    quantidadeDefeitos = $('.evaluateCurrent:visible').text();
    tdef.text(totalDefeito);
    tdefAv.text(quantidadeDefeitos);
}

//Zerar contador da amostra ao salvar
function ZerarContadorDefeito() {
    try {
        var totalDefeito;
        if (isNaN(tdef.text()) == false)
            totalDefeito = parseInt(tdef.text());
        if (isNaN(tdefAv.text()) == false)
            anterior[propertyObjAnterior] = parseInt(tdefAv.text());
        if (totalDefeito > 0) {
            totalDefeito = 0;
            tdef.text(totalDefeito);
        }
    }
    catch (err) {
        //
    }
}


function AtruibuiEventoCounterTipoCFF() {

    //Atualizar contadores 
    var period = $('#period :selected').val();
    var shift = $('#shift :selected ').val();
    var l1Id = _level1.id;
    tdef = $($('[id="tdefPeriod' + period + 'Shif' + shift + 'level1TdefId' + l1Id + '"]')[0]).find('span');
    tdefAv = $($('[id="tdefPeriod' + period + 'Shif' + shift + 'level1TdefId' + l1Id + '"]')[1]).find('span');
    tdef3 = $($('[id="tdefPeriod' + period + 'Shif' + shift + 'level1TdefId' + l1Id + '"]')[2]).find('span');

    var number = 0;
    if (isNaN($(_level1).attr('reauditnumber')) == false)
        number = $(_level1).attr('reauditnumber');
    if ($(_level1).attr('isreaudit') == 'true' && isNaN($(_level1).attr('reauditnumber')))
        number = 1;
    propertyObjAnterior = "Unit" + $('#selectParCompany :selected').val() + "Level1" + _level1.id + "Period" + $('#period :selected').val() + "Shift" +
        $('#shift :selected ').val() + "Date" + getCollectionDate() + "r" + number;

    propertyObjAnteriorDef3 = "Unit" + $('#selectParCompany :selected').val() + "Level1" + _level1.id + "Period" + $('#period :selected').val() + "Shift" +
        $('#shift :selected ').val() + "Date" + getCollectionDate() + "r" + number + "Def3";


    tdef.text($('.level2:visible li[inputtype=2] input[type="text"]').sum());
    if (anterior[propertyObjAnterior] != undefined)
        tdefAv.text(anterior[propertyObjAnterior]);
    else
        anterior[propertyObjAnterior] = parseInt(tdefAv.text());

    if (anterior[propertyObjAnteriorDef3] != undefined)
        tdef3.text(anterior[propertyObjAnteriorDef3]);
    else
        anterior[propertyObjAnteriorDef3] = parseInt(tdef3.text());

    tdef.parent().show();
    tdefAv.parent().show();
    tdef3.parent().show();

    $('.level2:visible li[inputtype=2] , li[inputtype=3] input[type="text"]').off('input').on('input', function () {
        var tryInputType1 = $(this).attr("inputtype");
        var tryInputType2 = $(this).parents('li').attr("inputtype");
        var inputType;
        if (tryInputType1 != undefined) {
            inputType = tryInputType1;
        } else if (tryInputType2 != undefined) {
            inputType = tryInputType2;
        }

        if (inputType == 2) { /*Numero de defeitos*/
            var elementos = $('.level2:visible li[inputtype=2] input[type="text"]');
            var novoValor = elementos.sum();
            novoValor += $('.level2:visible li[inputtype="1"].lightred, li[inputtype="3"].lightred').length
            tdef.text(novoValor);
            if (novoValor > 0) {
                haveChange = true;
                var tmpAnterior = anterior[propertyObjAnterior] + 1;
                tdefAv.text(tmpAnterior)
            }
            else if (novoValor == 0 && $(this).find('input[type="text"]').val() >= 0) {
                if ($(this).find('input[type="text"]').val() == 0 && (tdefAv.text() == anterior[propertyObjAnterior]))
                    return;

                var tmpAnterior = parseInt(tdefAv.text()) - 1;
                tdefAv.text(tmpAnterior)
            }

            if ($('.level2:visible li[inputtype=2] input[type="text"]').sum() >= 3 &&
                anterior[propertyObjAnteriorDef3] == parseInt(tdef3.text())) {
                var tmpAnterior = anterior[propertyObjAnteriorDef3] + 1;
                tdef3.text(tmpAnterior);
            }
            else if ($('.level2:visible li[inputtype=2] input[type="text"]').sum() < 3 &&
                anterior[propertyObjAnteriorDef3] < parseInt(tdef3.text())) {
                var tmpAnterior = anterior[propertyObjAnteriorDef3];
                tdef3.text(tmpAnterior)
            }
        }
    });
}
$('body > div.App > div.container > ol > li:nth-child(2) > a').off('click').on('click', function () {
    tdefAv.text(anterior[propertyObjAnterior]);
    tdef.parent().hide();
    tdefAv.parent().hide();
});

$('body > div.App > div.container > ol > li:nth-child(1) > a').off('click').on('click', function () {
    tdefAv.text(anterior[propertyObjAnterior]);
    tdef.parent().hide();
    tdefAv.parent().hide();
});

function gravaDefeitosLeveis3(avaliacao, amostra, level2, reaudit) {

    var leveis3s = localStorage.getItem('defeitosl3s');

    if (leveis3s == null || leveis3s == undefined)
        leveis3s = []
    else
        leveis3s = JSON.parse(leveis3s);
    var reauditNumber = 0;

    if (isNaN($(_level1).attr('reauditnumber')) == false)
        reauditNumber = parseInt($(_level1).attr('reauditnumber'));

    if (reauditNumber == 0 && reaudit)
        reauditNumber = 1;

    if ($('.level1.selected').attr('hasgrouplevel2') == 'true') {
        if ($(_level1).attr('lastsample') == level2.attr('sample') && amostra == 1)
            avaliacao++;

        $('.level02Result[level01id=' + $('.level1.selected').attr('id') + '][level02id=' + level2.attr('id') + '][evaluate=' + avaliacao + '][sample=' + amostra
            + '][date=' + getCollectionDate() + '][period=' + $('.App').attr('period') + '][shift=' + $('.App').attr('shift') + '][reauditnumber=' + reauditNumber +
            '] >.level03Result[defects!=0]').each(
                function (e) {
                    var obj;
                    $.grep(leveis3s, function (def, counter) {
                        if (def.idL1 == $('.level1.selected').attr('id') && def.idL2 == level2.attr('id') && def.data == getCollectionDate() && def.shift == $('.App').attr('shift') && def.period == $('.App').attr('period')
                            && def.rnumber == reauditNumber && def.avaliacao == avaliacao && def.amostra == amostra && def.idL3 == $(this).attr('level03id'))
                            obj = def;
                    });

                    if (obj == undefined) {
                        obj = {
                            'idL1': $('.level1.selected').attr('id'), 'idL2': $('.level2.selected').attr('id'), 'idL3': $(this).attr('level03id'), 'amostra': amostra, 'avaliacao': avaliacao, 'defeitos': $(this).attr('defects'),
                            'data': getCollectionDate(), 'shift': $('.App').attr('shift'), 'period': $('.App').attr('period'), 'rnumber': reauditNumber
                        };
                        leveis3s.push(obj);
                    }
                });
    } else {
        $('.level02Result[level02id=' + $('.level2.selected').attr('id') + '][evaluate=' + avaliacao + '][sample=' + amostra + '][date=' + getCollectionDate() +
            '][period=' + $('.App').attr('period') + '][shift=' + $('.App').attr('shift') + '][reauditnumber=' + reauditNumber + '] >.level03Result[defects!=0]').each(function (e) {
                if ($(this).attr('defects') > 0) {
                    var obj = {
                        'idL2': $('.level2.selected').attr('id'), 'idL3': $(this).attr('level03id'), 'amostra': amostra, 'avaliacao': avaliacao, 'defeitos': $(this).attr('defects'),
                        'data': getCollectionDate(), 'shift': $('.App').attr('shift'), 'period': $('.App').attr('period'), 'rnumber': reauditNumber
                    };
                    leveis3s.push(obj);
                }
            });
    }
    localStorage.setItem('defeitosl3s', JSON.stringify(leveis3s));
}

function gravaDefeitosLeveis3CFF(avaliacao, amostra, level2) {
    var leveis3s = localStorage.getItem('defeitosl3s');
    if (leveis3s == null || leveis3s == undefined)
        leveis3s = []
    else
        leveis3s = JSON.parse(leveis3s);
}

function abrirDepartamentoNoLevel3() {
    if (!(typeof (listaParLevel3XParDepartment) == "undefined")) {
        var listLevel3ComDepartamento = $.grep(listaParLevel3XParDepartment, function (o, i) {
            return o.ParLevel1_Id == $(_level1).attr("Id").split('98789')[1]
                && o.ParLevel2_Id == $(_level2).attr("Id").split('98789')[1]
        });

        parDepartmentSelected_Id = null;
        if (listLevel3ComDepartamento.length > 0) {
            //abre modal pra seleção
            $('select#selectSelecionarDepartamento').html('');


            //verificar se quantidade na lista é a mesma que a quantidade de tarefa. se não for mostrar opção de todos.
            /*var listaPorTarefa = [];
            $(listLevel3ComDepartamento).each(function (i, o) {
                var exist = $.grep(listaPorTarefa, function (o1, i1) {
                    return o1.ParLevel3_Id == o.ParLevel3_Id;
                });
                
                if(exist.length <= 0){
                    listaPorTarefa.push(o);
                }
            });*/

            var optionsDepartamento = [];
            optionsDepartamento.push('<option value="0">' + getResource('select') + '</option>');
            $(listLevel3ComDepartamento).each(function (i, o) {
                var option = '<option value="' + o.ParDepartment_Id + '">' + o.ParDepartment_Name + '</option>';
                if (optionsDepartamento.indexOf(option) < 0)
                    optionsDepartamento.push(option);
            });

            $('select#selectSelecionarDepartamento').html(optionsDepartamento.join(''));
            openMessageSelecionarLevel3PorDepartamento(function () {
                //após seleção apresenta apenas os departamentos vinculados
                parDepartmentSelected_Id = $("#selectSelecionarDepartamento :selected").val();
                //remove os level3 não vinculados ao departamento
                exibirLevel3PorDepartamento();
            });
        }
    }
}

function exibirLevel3PorDepartamento() {
    if (!(typeof (listaParLevel3XParDepartment) == "undefined")) {
        if (parDepartmentSelected_Id > 0) {
            var listaLevel3Ativos = $.grep(listaParLevel3XParDepartment, function (o, i) {
                return o.ParLevel1_Id == $(_level1).attr("Id").split('98789')[1]
                    && o.ParLevel2_Id == $(_level2).attr("Id").split('98789')[1]
                    && o.ParDepartment_Id == parDepartmentSelected_Id
            });

            $('.level3').each(function (i, o) {
                var existe = $.grep(listaLevel3Ativos, function (o1, i1) {
                    return o1.ParLevel1_Id == $(_level1).attr("Id").split('98789')[1]
                        && o1.ParLevel2_Id == $(_level2).attr("Id").split('98789')[1]
                        && o1.ParLevel3_Id == $(o).attr('id');
                });

                if (!(existe.length > 0))
                    $(o).addClass('hide');
            });
        } else {
            var listaLevel3Ativos = $.grep(listaParLevel3XParDepartment, function (o, i) {
                return o.ParLevel1_Id == $(_level1).attr("Id").split('98789')[1]
                    && o.ParLevel2_Id == $(_level2).attr("Id").split('98789')[1]
            });

            $('.level3').each(function (i, o) {
                var existe = $.grep(listaLevel3Ativos, function (o1, i1) {
                    return o1.ParLevel1_Id == $(_level1).attr("Id").split('98789')[1]
                        && o1.ParLevel2_Id == $(_level2).attr("Id").split('98789')[1]
                        && o1.ParLevel3_Id == $(o).attr('id');
                });

                if (existe.length > 0)
                    $(o).addClass('hide');
            });
        }
    }
}