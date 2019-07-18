var sendToReaudit = false;
var clusterAtivo = '';
var clusterAtivoName = '';

//o inicio precisa verificar se o equipamento esta online
function level1Show(inicio, cluster) {

    if (clusterAtivo != '') {
        $($('.level1List').children().children().children().children().children()).each(function (index) {
            $(this).show();

            if ($($(this).children()).attr("parcluster_id") > 0 && $($(this).children()).attr("parcluster_id") != cluster) {
                $(this).hide();
            }
        })
    }

    //quando mudar o formato de sincronização mudar isso tbm
    $('.level01Result').removeClass('selected');
    $('.alert').addClass('hide');

    var levelAtual = $('.level2List');
    if ($('.level3List').is(':visible')) {
        levelAtual = $('.level3List');
    }

    $(levelAtual).hide();

    $(this).addClass('hide');
    $('.level2').removeClass('selected');

    $('#btnSave').addClass('hide');
    $('#btnSaveTemp').addClass('hide');
    $('#btnSaveAllTemp').addClass('hide');
    $('.btnCA').addClass('hide');

    $('.correctiveaction').addClass('hide');
    $('.level1').removeClass('selected');

    $('.iconReturn').addClass('hide');
    $('.level1List').removeClass('hide').show();
    breadCrumb();
    if (clusterAtivo == '') {
        if ($('.ClusterList').find('.cluster').length == 1) {
            $('.ClusterList').find('.cluster').click()
        } else {
            $('.level1List').hide();
            $('.ClusterList').show();
        }
    } else {
        $('.ClusterList').hide();
    }
    $('.App').removeClass('hide').show();

    if (inicio == true) {
        loginFile();
        loginFileResult();
        loginVerificacaoTipificacao();
        loginPhotos();

        getTempFileLevel2();
        getPcc1bFile();

        getListsReprocesso();

        configureZoom();
    }

    sendTotalAvaliacoesPorIndicadorPorAvaliacao(loadTotalAvaliacoesPorIndicadorPorAvaliacao());

    if (inicio == true && connectionServer == true) {
        setTimeout(function (e) {

            if ($('.level02Result[sync=false]').length) {
                if (isEUA == false)
                    $('#btnSync').click();
            } else {
                mensagemSyncHide();
                    reciveResults();
            }

            getCollectionPhase();
            loadTotalAvaliacoesPorIndicadorPorAvaliacao();
            getHeaderResultList();
            getListParMultipleValuesXParCompany();

        }, 1000);

        mockCFF = 0;
        mockCFFSmp = 0;

        $('.level1').removeAttr('isreaudit');

    } else {
        $('.message:visible, .overlay:visible').addClass('hide');
    }

    $('.periodShift').text(getResource('period') + ": " + $('.App').attr('period') + " / " + getResource('shift') + ": " + $('.App').attr('shift'));

    $('.level2').removeAttr('isreaudit');
    $('.level1').removeAttr('reaudminlevel');

    $('#period').not('disabled').removeAttr('disabled');

    if ($('#period').hasClass('disabled')) {
        $('#period').attr('disabled', 'disabled');
    }

    $('.level1').removeClass('bgCompleted');
    $('.level1[completed]').parents('li').addClass('bgCompleted');

    $('.level1').siblings('.userInfo').find('.btnReaudit').addClass('hide');

    $('.level1').each(function (index, self) {

        var resultHeaders = $.grep(headerResultList, function (o, i) {
            return o.HaveReaudit == true && o.ParLevel1_Id == $(self).attr('id') &&
                o.Shift == $('.App').attr('shift') && o.Period == $('.App').attr('period');
        });

        if (resultHeaders.length == 0) {
            if ($('.Resultlevel2[level1id=' + $(self).attr('id') + '][shift=' + $('.App').attr('shift') + '][havereaudit=true]').length > 0) {
                $(self).siblings('.userInfo').find('.btnReaudit').removeClass('hide');
            }
        } else {
            $(self).siblings('.userInfo').find('.btnReaudit').removeClass('hide');
        }

        completeLevel1(self);
        $(self).parents().find('.counter[indicador=' + $(self).attr('id') + '][headerlevel=level1_line]').removeClass('hide');
    });

    updateReaudit(1);

    updateCorrectiveAction();

    $('#btnShowImage').remove();

    if ($('.App').attr('local') == "eua" || $('.App').attr('local') == "canada") {
        isEUA = true;
    }

    mockCFF = 0;
    mockCFFSmp = 0;
    applyRole();
    initializeInputs();
    _level2 = null;
    $('#btnCA').addClass('hide');

    ReauditByHeader.CurrentReauditNumber = 0;
    ReauditByHeader.triggerReaudit = false;

}

function configureZoom() {
    if ($('.zooms').length == 0) {
        $('body').append(
            '<div class="zooms">' +
            '<button class="btn btn-default btn-lg btn-zoom" id="zoomPlus">' +
            '<i class="fa fa-search-plus" aria-hidden="true"></i>' +
            '</button>' +
            '<button class="btn btn-default btn-lg btn-zoom" id="zoomMinus">' +
            '<i class="fa fa-search-minus" aria-hidden="true"></i>' +
            '</button>' +
            '</div>'
        );
    }

    applyZoom();
}

function applyZoom(zoom) {

    if (!localStorage.getItem('zoom') && zoom) {
        localStorage.setItem('zoom', zoom);
    } else if (!zoom) {
        zoom = parseFloat(localStorage.getItem('zoom'));
    } else {
        localStorage.setItem('zoom', parseFloat(localStorage.getItem('zoom')) + zoom);
    }
    $('html').css('zoom', parseFloat($('html').css('zoom')) + zoom);
}

$(document).on('click', '#zoomPlus', function () {
    applyZoom(0.1)
});

$(document).on('click', '#zoomMinus', function () {
    applyZoom(-0.1)
});

function completeLevel1(level1) {
    if (getCompletedLevel1(parseInt($(level1).attr('id')), parseInt($('.App').attr('shift')), parseInt($('.App').attr('period')))) {
        $(level1).parent().addClass('bgCompleted');
        $(level1).attr('completed', 'completed');
    } else {
        $(level1).parent().removeClass('bgCompleted');
        $(level1).removeAttr('completed');
    }
}

function configureLevel01() {

    $('.btnCA').addClass('hide');

    $('.level01').children('.icons').children('.areaComplete').addClass('hide');
    $('.level01').parents('li').removeClass('bgCompleted');

    $('.level01').parents('.row').children('.userInfo').children('div').children('.btnCorrectiveAction').addClass('hide');

    $('.level01, .level01').parents('li').removeClass('bgLimitExceeded');

    //configureLevel
    $('.level01[completed]').children('.icons').children('.areaComplete').removeClass('hide');
    $('.level01[completed]').parents('li').addClass('bgCompleted');

    $('.level01[correctivaction]').parents('.row').children('.userInfo').children('div').children('.btnCorrectiveAction').removeClass('hide');

    //$('.level01[reaudit]').parents('.row').children('.userInfo').children('div').children('.btnReaudit').removeClass('hide');
    // $('.level01[reaudit]').parents('li').removeClass('bgLimitExceeded').addClass('bgCompleted');
    $('.level01[reaudit]').parents('li').removeClass('bgLimitExceeded');
    $('.level01[correctivaction]').parents('li').addClass('bgLimitExceeded').removeClass('bgCompleted');

    $('.level01[reauditNumber]').parents('.row').children('.userInfo').children('div').children('.reauditCount').text($('.level01[reauditNumber]').attr('reauditCount')).removeClass('hide');

    $('.level01[reaudit]').each(function (e) {
        //

        var level01 = $(this);
        var level01geraReaudit = getResultHaveReaudit(level01);

        var level01User = level01.siblings('.userInfo').children('div');

        level01User.children('.btnReaudit').children('.reauditPeriod').text($('#selectPeriod option[value=' + level01geraReaudit.attr('period') + ']').text());

    });
}

$(document).on('click', '.level1List .level1', function (e) {
    _level1 = this;
    if (!sendToReaudit)
        $(this).attr('isreaudit', 'false');

    sendToReaudit = false;
    var valid = false;
    if (parseInt($('#period').val()) > 0) {
        valid = true;
    } else {
        openMessageModal(getResource('warning'), getResource('select_the_period_first'));
        return;
    }

    if (valid) {
        //Reseta Level1 selecionado
        $('.level1').removeClass('selected');
        //Instancia o Objeto de Level1
        var level1 = $(this);
        //Define o Level1 selecionado
        level1.addClass('selected');

        _readFile('level' + level1.attr('id') + '.txt', function (data) {

            $('.level2List').empty();
            $('.level3List').empty();

            //append level2list
            appendDevice($(data).filter('.level2Group'), $('.level2List'));
            //append level3list
            appendDevice($(data).filter('.level3Group'), $('.level3List'));

            openLevel2(level1);

        });
    }

});

function level01Reset(level01) {

    level01.each(function (e) {

        var level = $(this);


        var btnCorrectivAction = level.parents('.row').children('.userInfo').children('div').children('.btnCorrectiveAction');
        var btnReaudit = level.parents('.row').children('.userInfo').children('div').children('.btnReaudit');
        var reauditCount = level.parents('.row').children('.userInfo').children('div').children('.reauditCount');



        btnCorrectivAction.addClass('hide');
        btnReaudit.addClass('hide');
        reauditCount.addClass('hide');
        level.children('.icons').children('.iconsArea').addClass('hide');
        level.removeAttr('completed').removeAttr('correctivaction').removeAttr('startreaudit').removeAttr('reauditnumber').removeAttr('reaudit').removeClass('reauditnumber').parents('li').removeClass('bgLimitExceeded').removeClass('bgCompleted');

    });
}

function saveLevel01(Level01Id, date, unidadeId, shift, period, reaudit, reauditNumber
    , totalEvaluate, sidesWithErros, more3Defects, lastEvaluate, lastSample, biasedUnbiased, evaluate, hashkey) {

    if (reaudit != true) {
        reaudit = false
    }

    Level01Id = Level01Id ? Level01Id : $(_level1).attr('id');

    period = period ? period : 1;

    reauditNumber = reauditNumber ? reauditNumber : 0;
    totalEvaluate = totalEvaluate ? totalEvaluate : 0;
    sidesWithErros = sidesWithErros ? sidesWithErros : 0;
    more3Defects = more3Defects ? more3Defects : 0;
    lastEvaluate = lastEvaluate ? lastEvaluate : 0;
    lastSample = lastSample ? lastSample : 0;
    biasedUnbiased = biasedUnbiased ? biasedUnbiased : 0;
    evaluate = evaluate ? evaluate : 1;


    var date = getCollectionDate();

    return "<div class='level01Result' level01Id='" + Level01Id + "' unidadeid='" + unidadeId + "' date='" + date + "' dateTime='" + dateTimeFormat() + "' shift='" + shift + "' period='" + period + "' reaudit='" + reaudit + "' reauditNumber='" + reauditNumber + "' totalevaluate='" + totalEvaluate + "' sidewitherros='" + sidesWithErros + "' more3Defects='" + more3Defects + "' lastevaluate='" + lastEvaluate + "' lastsample='" + lastSample + "' biasedunbiased='" + biasedUnbiased + "' evaluate='" + evaluate + "' hashkey='" + hashkey + "' sync='false'></div>";
}


//Pega ultimo Resultado atual da data, shift, period que não tem reauditoria
function getAtualResult(level01) {
    var atualResult = $('.level01Result[level01id=' + level01.attr('id') + '][date="' + getCollectionDate() + '"][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + '][reaudit=false]:last');
    return atualResult;
}
//Pega ultimo resultado do shift
function getLastResult(level01) {
    var lastResult = $('.level01Result[level01id=' + level01.attr('id') + '][shift=' + $('.App').attr('shift') + ']:last');
    return lastResult;
}

//Pega Ultimo Resultado do Periodo
function getLastResultPeriod(level01) {
    var atualResult = $('.level01Result[level01id=' + level01.attr('id') + '][date="' + getCollectionDate() + '"][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + ']:last');
    return atualResult;
}
//Pega Resultado do Shift que tem reauditoria
function getResultHaveReaudit(level01) {
    var lastResultReaudit = $('.level01Result[level01id=' + level01.attr('id') + '][shift=' + $('.App').attr('shift') + '][havereaudit]:last');
    return lastResultReaudit;
}
//Pega ultima reauditoria do shift não completa
function getLastReauditPeriodNotCompleted(level01) {
    var lastResultReauditNotCompleted = $('.level01Result[level01id=' + level01.attr('id') + '][shift=' + $('.App').attr('shift') + '][reaudit=true][completed!=completed]:last');
    return lastResultReauditNotCompleted;
}
//Pega ultima Reaudotoria do period
function getLastReauditResultPeriod(level01, period) {
    var lastResultReauditNotCompleted = $('.level01Result[level01id=' + level01.attr('id') + '][shift=' + $('.App').attr('shift') + '][period=' + period + '][date="' + getCollectionDate() + '"][reaudit=true]:last');
    return lastResultReauditNotCompleted;
}

$(document).on('click', '.btnCALevel1', function () {

    var level1 = $(this).parents('.row').children('.level1');

    $(level1).click();
    setTimeout(function () { $('a.main').click() }, 300);

    $('.modal-close-ca').show();

    if (level1.attr('hasgrouplevel2') == "true") {
        if (level1.attr('hascompleteevaluation') == "false") {
            openMessageCA(getResource("corrective_action"), getResource("ca_evaluations_not_completed"), function () {
                correctiveActionOpen(level1.attr('id'), getCollectionDate(), $('.App').attr('shift'), $('.App').attr('period'))
            });
        } else
            correctiveActionOpen(level1.attr('id'), getCollectionDate(), $('.App').attr('shift'), $('.App').attr('period'))
    } else if (!level1.attr('completed')) {
        if (isEUA) {
            openMessageCA(getResource("corrective_action"), getResource("ca_evaluations_not_completed"), function () {
                correctiveActionOpen(level1.attr('id'), getCollectionDate(), $('.App').attr('shift'), $('.App').attr('period'))
            });
        } else {
            correctiveActionOpen(level1.attr('id'), getCollectionDate(), $('.App').attr('shift'), $('.App').attr('period'))
        }
    } else
        correctiveActionOpen(level1.attr('id'), getCollectionDate(), $('.App').attr('shift'), $('.App').attr('period'))
});

$(document).on('click', '#btnCA', function () {

    var level1 = $('.level1.selected');

    $('.modal-close-ca').show();

    if (level1.attr('hasgrouplevel2') == "true") {
        if (level1.attr('hascompleteevaluation') == "false") {
            openMessageCA(getResource("corrective_action"), getResource("ca_evaluations_not_completed"), function () {
                correctiveActionOpen(level1.attr('id'), getCollectionDate(), $('.App').attr('shift'), $('.App').attr('period'))
            });
        } else
            correctiveActionOpen(level1.attr('id'), getCollectionDate(), $('.App').attr('shift'), $('.App').attr('period'))
    } else if (!level1.attr('completed')) {
        if (isEUA) {
            openMessageCA(getResource("corrective_action"), getResource("ca_evaluations_not_completed"), function () {
                correctiveActionOpen(level1.attr('id'), getCollectionDate(), $('.App').attr('shift'), $('.App').attr('period'))
            });
        } else {
            correctiveActionOpen(level1.attr('id'), getCollectionDate(), $('.App').attr('shift'), $('.App').attr('period'))
        }
    } else
        correctiveActionOpen(level1.attr('id'), getCollectionDate(), $('.App').attr('shift'), $('.App').attr('period'))

});

$(document).on('click', '.cluster', function (e) {

    abrirCluster(this)

})

function abrirCluster(cluster) {
    clusterAtivo = cluster.id
    clusterAtivoName = $(cluster).attr('parcluster_name')
    $('.main').html(clusterAtivoName)
    level1Show(false, clusterAtivo)   
}

function voltarCluster() {
    clusterAtivo = ""
    clusterAtivoName = ""
    $('.main').html("")
    level1Show(false, null)
}


