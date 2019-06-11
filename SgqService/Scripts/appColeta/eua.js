
$(document).on('click', '#btnSalvarLevel02CCA', function (e) {

    //Verifica o level01 Selecionado.
    var level01 = $('.level01.selected');

    //Pega o ultimo resultado (resultado que esta sendo auditado).
    var level01Result = $('.level01Result.selected');

    //Se não encontrar resultado existe algum problema.
    if (!level01Result.length) {
        openMessageModal("Result not found", null);
        return true;
    }

    //Instancia reaudit como falso
    var reaudit = false;
    //Se o level01Result for uma reauditoria.
    if (level01Result.attr('reaudit') == "true") {
        //altero o valor de reauditoria para true.
        reaudit = true;
    }

    //Instancio reauditNumber como 0.
    var reauditNumber = 0;
    //Se a reauditoria é true.
    if (reaudit == true) {
        //Verifica o número da reauditoria pelo resultado.
        reauditNumber = parseInt(level01Result.attr('reauditnumber'));
        //Se o número da reauditoria for zero ou nulo a reauditoria será 1.
        if (reauditNumber == 0 || isNaN(reauditNumber)) {
            reauditNumber = 1;
        }
    }

    //Se todos os itens estão completos no Level02 eu atualizo o level01 para completed.
    if ($('.level02Group[level01id=' + level01.attr('id') + ']').children('li').children('.row').children('.level02[completed!=completed]').length == 0) {

        level01.attr('completed', 'completed');
    }
    else {
        //Se não estiverem, aviso que a reaudotiria deve estar completa.
        openMessageModal('Complete current audit to save ' + $('.level01.selected').children('.name').text(), null);
        return false;
    }

    //Se for uma reautoria preencho os campos NA com o valor informado nos inputs após ter começado a reauditoria.
    if (level01Result.attr('reaudit') == "true") {
        level01Result.children('.level02Result[chainspeed=""]').attr('chainspeed', $('#inputChainSpeed').val());
        level01Result.children('.level02Result[lotnumber=""]').attr('lotnumber', $('#inputLotNumber').val());
        level01Result.children('.level02Result[mudscore=""]').attr('mudscore', $('#inputMudScore').val());
        level01Result.children('.level02Result[cattletype=""]').attr('cattletype', $('#selectCattleType').val());
        level01Result.children('.level02Result[cattletype="0"]').attr('cattletype', $('#selectCattleType').val());

    }

    //Instancio os defeitos totals da auditoria.
    var totalDefects = parseInt($('.painelLevel02 .totalDefects').text());

    //Instancio que falha consecutivas são falsas.
    var haveConsecutiveFailures = false


    if (totalDefects > 22 || $('.level02Group[level01id=' + level01.attr('id') + '] .level02[havereaudit]').length) {
        reauditNumber = 0;
        level01.attr('correctivaction', 'correctivaction').attr('reaudit', 'reaudit').attr('reauditNumber', reauditNumber);

        //level01.parents('.row').children('.userInfo').children('div').children('.reauditCount').children('button').text(reauditNumber + '/' + level01.attr('minreauditnumber'));

        level01Result.attr('havecorrectiveaction', 'havecorrectiveaction');
        if (!level01Result.attr('reaudit') != true) {
            level01Result.attr('havereaudit', 'havereaudit');
            level01.parents('.row').children('.userInfo').children('div').children('.btnReaudit').addClass('hide').siblings('.reauditCount').addClass('hide');
        }
    }

    //Defino o Resultado do level01 como completed.
    level01Result.attr('completed', 'completed').attr('totaldefects', totalDefects);


    //Se o level01 está como reauditoria e meu resultado é uma reauditoria.
    if (level01.attr('startreaudit') || level01Result.attr('reaudit') == "true") {


        //Adiciono os atributos para poder visualizar a reauditoria correta no level01.
        level01.attr('reaudit', 'reaudit').attr('reauditNumber', reauditNumber).removeAttr('startreaudit');

        //Seleciona meu resultado que gerou a reauditoria.
        var resultStarReaudit = getResultHaveReaudit(level01);

        //Informo a quantidade de reauditorias.
        resultStarReaudit.attr('totalreaudits', reauditNumber);

        //Verifico o número mínimo de reauditorias.
        var minreauditNumber = parseInt(level01.attr('minreauditnumber'));

        //Se o número da reauditoria atual for maior que o número minimo de reauditorias.
        if (reauditNumber >= minreauditNumber) {
            //removo a tag reaudit e reauditnumber do level01.
            level01.removeAttr('reaudit').removeAttr('reauditnumber');

            //Adiciono as tags no level01Result que gerou a reauditoria para saber que a reauditoria foi completada.
            resultStarReaudit.removeAttr('havereaudit').attr('completereaudit', 'completereaudit').children('.level02Result[havereaudit]').removeAttr('havereaudit').attr('completereaudit', 'completereaudit');
            //Busco a div que tem os botoes do level01.
            var level01Buttons = level01.siblings('.userInfo').children('div');

            //Zero  o periodo da reauditoria.
            level01Buttons.children('.btnReaudit').addClass('hide').children('.reauditPeriod').text('');

            //Oculto o contador.
            level01Buttons.children('.reauditCount').addClass('hide');

            //if (resultStarReaudit.attr('period') != $('.App').attr('period')) {
            //    var resultAtualLevel = $('.level01Result[level01id=' + level01.attr('id') + '][date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + '][reaudit=false]');
            //    if (resultAtualLevel.length == 0) {
            //        level01.attr('reauditnumber', '0').removeAttr('reaudit').removeAttr('reauditnumber').removeAttr('startreaudit').removeAttr('completed');

            //    }
            //}

            //Informa ao usuário que terminou a reauditoria
            openMessageModal('Re-audit completed', $('#selectPeriod option[value=' + resultStarReaudit.attr('period') + ']').text() + ' re-audit completed!');

        }
    }

    //Se o level01Result tem uma ação corretiva, atribuo ação corretiva no level01 e no resultado
    if (level01Result.children('.level02Result[havecorrectiveaction]').length) {

        level01.attr('correctivaction', 'correctivaction')
        level01Result.attr('havecorrectiveaction', 'havecorrectiveaction');
    }
    //Se o level01Result tem uma reauditoria, atribuo a ação corretiva no level01 e no resultado
    if (level01Result.children('.level02Result[havereaudit]').length) {
        level01.attr('reaudit', 'reaudit');
        level01Result.attr('havereaudit', 'havereaudit');
    }

    //Salva no Banco dados Off Line.
    createFileResult();
    //Retorna ao menu do Level01.

    //Reseto o periodo
    level01Reset(level01);
    periodReset();

    level01Return();



});
$(document).on('click', '#btnSalvarLevel02CFF', function (e) {

    //Verifica o level01 Selecionado
    var level01 = $('.level01.selected');


    var level01Result = $('.level01Result.selected');
    if (!level01Result.length) {
        openMessageModal("Result not found", null);
        return true;
    }


    //Inicio reaudit como falso
    var reaudit = false;
    //se meu level02 for uma reauditoria
    if (level01.attr('startreaudit')) {
        //if (level02.attr('reaudit') == "true") {
        //altero o valor de reauditoria para true;
        reaudit = true;
    }



    //Instancio reauditNumber como 0;
    var reauditNumber = 0;
    //se a reauditoria é true
    if (reaudit == true) {
        //verifico o numero atual de reauditoria pelo nivel02
        reauditNumber = parseInt(level01Result.attr('reauditnumber'));
        if (reauditNumber == 0) {
            reauditNumber = 1;
        }
    }


    var evaluateConf = parseInt($('.level02Group[level01id=' + level01.attr('id') + '] .level02:first').attr('totalsets'));
    var evaluate = parseInt(level01Result.attr('evaluate'));


    //Se todos os itens estão completos no Level02 eu atualizo o level01 para completed
    if (evaluate >= evaluateConf && level01Result.attr('completedsample')) {
        level01.attr('completed', 'completed');

    }
    else {
        //se não estiver envio uma mensagem
        openMessageModal('Complete current audit to save ' + $('.level01.selected').children('.name').text(), null);
        return false;
    }



    //Defino o Resultado do nivel 01 como completed
    var levels01Result = $('.level01Result[level01id=' + level01.attr('id') + '][date="' + level01Result.attr('date') + '"][shift=' + level01Result.attr('shift') + '][period=' + level01Result.attr('period') + '][reaudit=' + level01Result.attr('reaudit') + '][reauditnumber=' + level01Result.attr('reauditnumber') + ']');

    levels01Result.attr('completed', 'completed');

    //Se o level01Result tem uma ação corretiva, atribuo ação corretiva no level01
    if (levels01Result.attr('havecorrectiveaction')) {

        level01.attr('correctivaction', 'correctivaction')
    }


    //Se o level01Result tem uma reauditoria, atribuo a ação corretiva no level01
    if (levels01Result.attr('havereaudit')) {
        level01.attr('reaudit', 'reaudit');
    }


    //Se o level01 está como reauditoria e meu resultado é uma reauditoria.
    if (level01.attr('startreaudit') || level01Result.attr('reaudit') == "true") {


        //Adiciono os atributos para poder visualizar a reauditoria correta no level01.
        level01.attr('reaudit', 'reaudit').attr('reauditNumber', reauditNumber).removeAttr('startreaudit');

        //Seleciona meu resultado que gerou a reauditoria.
        var resultStarReaudit = $('.level01Result[level01id=' + level01.attr('id') + '][shift=' + $('.App').attr('shift') + '][reauditnumber!=' + reauditNumber + '][havereaudit]');

        //Informo a quantidade de reauditorias.
        resultStarReaudit.attr('totalreaudits', reauditNumber);

        //Verifico o número mínimo de reauditorias.
        var minreauditNumber = parseInt(level01.attr('minreauditnumber'));

        //Se o número da reauditoria atual for maior que o número minimo de reauditorias.
        if (reauditNumber >= minreauditNumber) {
            //removo a tag reaudit e reauditnumber do level01.
            level01.removeAttr('reaudit').removeAttr('reauditnumber');

            //Adiciono as tags no level01Result que gerou a reauditoria para saber que a reauditoria foi completada.
            resultStarReaudit.removeAttr('havereaudit').attr('completereaudit', 'completereaudit').children('.level02Result[havereaudit]').removeAttr('havereaudit').attr('completereaudit', 'completereaudit');
            //Busco a div que tem os botoes do level01.
            var level01Buttons = level01.siblings('.userInfo').children('div');

            //Zero  o periodo da reauditoria.
            level01Buttons.children('.btnReaudit').addClass('hide').children('.reauditPeriod').text('');

            //Oculto o contador.
            level01Buttons.children('.reauditCount').addClass('hide');

            //if (resultStarReaudit.attr('period') != $('.App').attr('period')) {
            //    var resultAtualLevel = $('.level01Result[level01id=' + level01.attr('id') + '][date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + '][reaudit=false]');
            //    if (resultAtualLevel.length == 0) {
            //        level01.attr('reauditnumber', '0').removeAttr('reaudit').removeAttr('reauditnumber').removeAttr('startreaudit').removeAttr('completed');

            //    }
            //}

            //Informa ao usuário que terminou a reauditoria
            openMessageModal('Re-audit completed', $('#selectPeriod option[value=' + resultStarReaudit.attr('period') + ']').text() + ' re-audit completed!');

        }
    }



    createFileResult();

    //Reseto o periodo
    level01Reset(level01);
    periodReset();


    level01Return();


});
$(document).on('click', '#btnSalvarLevel02HTP', function (e) {



    //Verifica o level01 Selecionado.
    var level01 = $('.level01.selected');

    //Pega o ultimo resultado (resultado que esta sendo auditado).
    var level01Result = $('.level01Result.selected');

    //Se não encontrar resultado existe algum problema.
    if (!level01Result.length) {
        openMessageModal("Result not found", null);
        return true;
    }

    //Instancia reaudit como falso
    var reaudit = false;
    //Se o level01Result for uma reauditoria.
    if (level01Result.attr('reaudit') == "true") {
        //altero o valor de reauditoria para true.
        reaudit = true;
    }

    //Instancio reauditNumber como 0.
    var reauditNumber = 0;
    //Se a reauditoria é true.
    if (reaudit == true) {
        //Verifica o número da reauditoria pelo resultado.
        reauditNumber = parseInt(level01Result.attr('reauditnumber'));
        //Se o número da reauditoria for zero ou nulo a reauditoria será 1.
        if (reauditNumber == 0 || isNaN(reauditNumber)) {
            reauditNumber = 1;
        }
    }



    //Se todos os itens estão completos no Level02 eu atualizo o level01 para completed.
    if ($('.level02Group[level01id=' + level01.attr('id') + ']').children('li').children('.row').children('.level02[completed!=completed]').length == 0) {

        level01.attr('completed', 'completed');
    }
    else {
        //Se não estiverem, aviso que a reaudotiria deve estar completa.
        openMessageModal('Complete current audit to save ' + $('.level01.selected').children('.name').text(), null);
        return false;
    }

    //Se for uma reautoria preencho os campos NA com o valor informado nos inputs após ter começado a reauditoria.
    //if (level01Result.attr('reaudit') == "true") {
    //    level01Result.children('.level02Result[biasedUnbiased=""]').attr('biasedUnbiased', $('#biasedUnbiased').val());

    //}
    //Se for uma reautoria preencho os campos NA com o valor informado nos inputs após ter começado a reauditoria.
    if (level01Result.attr('reaudit') == "true") {

        level01Result.attr('biasedunbiased', $('#biasedUnbiased').val());

    }

    //Defino o Resultado do level01 como completed.
    level01Result.attr('completed', 'completed');

    //Se o level01 está como reauditoria e meu resultado é uma reauditoria.
    if (level01.attr('startreaudit') || level01Result.attr('reaudit') == "true") {


        //Adiciono os atributos para poder visualizar a reauditoria correta no level01.
        level01.attr('reaudit', 'reaudit').attr('reauditNumber', reauditNumber).removeAttr('startreaudit');

        //Seleciona meu resultado que gerou a reauditoria.
        var resultStarReaudit = getResultHaveReaudit(level01);

        //Informo a quantidade de reauditorias.
        resultStarReaudit.attr('totalreaudits', reauditNumber);

        //Verifico o número mínimo de reauditorias.
        var minreauditNumber = parseInt(level01.attr('minreauditnumber'));

        //Se o número da reauditoria atual for maior que o número minimo de reauditorias.
        if (reauditNumber >= minreauditNumber) {
            //removo a tag reaudit e reauditnumber do level01.
            level01.removeAttr('reaudit').removeAttr('reauditnumber');

            //Adiciono as tags no level01Result que gerou a reauditoria para saber que a reauditoria foi completada.
            resultStarReaudit.removeAttr('havereaudit').attr('completereaudit', 'completereaudit').children('.level02Result[havereaudit]').removeAttr('havereaudit').attr('completereaudit', 'completereaudit');
            //Busco a div que tem os botoes do level01.
            var level01Buttons = level01.siblings('.userInfo').children('div');

            //Zero  o periodo da reauditoria.
            level01Buttons.children('.btnReaudit').addClass('hide').children('.reauditPeriod').text('');

            //Oculto o contador.
            level01Buttons.children('.reauditCount').addClass('hide');

            //if (resultStarReaudit.attr('period') != $('.App').attr('period')) {
            //    var resultAtualLevel = $('.level01Result[level01id=' + level01.attr('id') + '][date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + '][reaudit=false]');
            //    if (resultAtualLevel.length == 0) {
            //        level01.attr('reauditnumber', '0').removeAttr('reaudit').removeAttr('reauditnumber').removeAttr('startreaudit').removeAttr('completed');

            //    }
            //}

            //Informa ao usuário que terminou a reauditoria
            openMessageModal('Re-audit completed', $('#selectPeriod option[value=' + resultStarReaudit.attr('period') + ']').text() + ' re-audit completed!');

        }
    }


    //Se o level01Result tem uma ação corretiva, atribuo ação corretiva no level01 e no resultado
    if (level01Result.children('.level02Result[havecorrectiveaction]').length) {

        level01.attr('correctivaction', 'correctivaction')
        level01Result.attr('havecorrectiveaction', 'havecorrectiveaction');
    }
    //Se o level01Result tem uma reauditoria, atribuo a ação corretiva no level01 e no resultado
    if (level01Result.children('.level02Result[havereaudit]').length) {
        level01.attr('reaudit', 'reaudit');
        level01Result.attr('havereaudit', 'havereaudit');
    }

    //Salva no Banco dados Off Line.
    createFileResult();
    //Retorna ao menu do Level01.

    //Reseto o periodo
    level01Reset(level01);
    periodReset();
    level01Return();

});

//*************CFF*************///////




function resultPreencher(resultPeriod) {

    //Seleciona o Level01.
    var level01 = $('.level01.selected');
    //Instancia o total de defeitos
    var totalDefects = 0;

    //Oculta os botões do level02
    //$('.level02').parents('.row').children('.userInfo').children('div').children('').addClass('hide');
    $('.level02 .btnPhase, .level02 .btnCorrectiveAction, .level02 .reauditCount').addClass('hide');
    //Remove Atributos
    $('.level02').removeAttr('startphasedate').removeAttr('phase').removeAttr('havecorrectiveaction').removeAttr('correctivaction').attr('reauditNumber', '0').attr('defects', '0');
    //Remove Cores de Fundo do Level02
    $('.level02').parents('li').removeClass('bgLimitExceeded');

    //Percorre o resultado
    resultPeriod.each(function (e) {


        var level01Result = $(this);
        if (level01Result.attr('reaudit') == "true" && level01Result.attr('havereaudit') && level01Result.attr('completed') && level01.attr('startreaudit')) {
            return false;
        }
        level01Result.addClass('selected');
        var results = level01Result.children('.level02Result');
        if (level01Result.attr('biasedunbiased')) {
            $('#biasedUnbiased').val(level01Result.attr('biasedunbiased'));
        }

        $("span.sideWithErrors").text((level01Result.attr('sidewitherros')) ? level01Result.attr('sidewitherros') : "0").parents('.labelPainel').removeClass('red');
        $("span.more3Defects").text((level01Result.attr('more3Defects')) ? level01Result.attr('more3Defects') : "0").parents('.labelPainel').removeClass('red');

        var sideWithErros = parseInt($("span.sideWithErrors:first").text());
        if (sideWithErros > 5) {
            $("span.sideWithErrors").parents('.labelPainel').addClass('red');
        }
        var more3Defects = parseInt($("span.more3Defects:first").text());
        if (more3Defects > 0) {
            $("span.more3Defects").parents('.labelPainel').addClass('red');
        }


        results.each(function (e) {


            var result = $(this);
            var level02 = $('.level02Group[level01id=' + results.attr('level01id') + '] .level02[id=' + result.attr('level02id') + ']');
            level02.attr('defects', result.attr('defects')).attr('completed', 'completed');
            level02.parents('.row').children('.userInfo').children('div').children('.defects').text(level02.attr('defects'));
            totalDefects = totalDefects + parseInt(level02.attr('defects'));
            //esses results podem ficar no level01


            //if (level01.attr('consecutivefailure') == "true")
            //{
            //    if (result.attr('consecutivefailuretotal')) {
            //        level02.siblings('.userInfo').children('div').children('.consecutiveFailure').text(result.attr('consecutivefailuretotal'));
            //    }
            //    else {
            //       var resultLast = $('.level02Result[.level01id=' + results.attr('level01id') + '][level02id=' + result.attr('level02id') + ']:last');
            //       level02.siblings('.userInfo').children('div').children('.consecutiveFailure').text(resultLast.attr('consecutivefailuretotal'));

            //    }

            //}

            if (result.attr('cattletype')) {
                $('#selectCattleType').val(result.attr('cattletype'));
            }
            if (result.attr('chainspeed')) {
                $('#inputChainSpeed').val(result.attr('chainspeed'));
            }
            if (result.attr('lotnumber')) {
                $('#inputLotNumber').val(result.attr('lotnumber'));
            }
            if (result.attr('mudscore')) {
                $('#inputMudScore').val(result.attr('mudscore'));
            }

            if (result.attr('notavaliable') == "true") {
                level02.attr('notavaliable', 'notavaliable');
                level02.siblings('.userInfo').children('div').children('.btnNotAvaliable').addClass('naSelected');
            }

            //result.children('.level03Result').each(function (e) {
            //    level02.attr('level03' + $(this).attr('id'), $(this).attr('value'))
            //});


            //if (level01.attr('correctivActionLevel02') && result.attr('havecorrectiveaction')) {
            //    level02.attr('correctivaction', 'correctivaction');
            //    //level02.siblings('.userInfo').children('div').children('.btnCorrectiveAction').removeClass('hide');
            //}
            //else if (!$('#correctiveActionModal').is(':visible') && result.attr('havecorrectiveaction')) {
            //    level02.attr('correctivaction', 'correctivaction');
            //    $('.btnCA').removeClass('hide');
            //}
            //else if (!$('#correctiveActionModal').is(':visible') && result.attr('correctiveactioncomplete')) {
            //    $('.btnCA').removeClass('hide');
            //}

            //if (level01.attr('correctivactionlevel02')) {
            //    $('.btnCA').addClass('hide');

            //}
            if ($('.level01[id=' + result.attr('level01id') + ']').attr('havephases')) {
                var phase = parseInt(result.attr('phase'));
                var phaseNumber = parseInt(result.attr('reauditnumber'));
                var level02Phase = $('.level02[id=' + result.attr('level02id') + ']');
                level02Phase.attr('phase', phase).attr('reauditnumber', phaseNumber);

                if (phase > 0) {

                    level02Phase.children('.levelInfo').text('(Phase ' + phase + ")");
                }


            }

            $('.totalDefects').text(totalDefects);
            if (totalDefects > 22) {
                $('.painel .totalDefects').parents('.labelPainel').addClass('red');
            }
            else {
                $('.painel .totalDefects').parents('.labelPainel').removeClass('red');

            }
            level02.addClass('selected');
            defectLimitCheck();

            if (result.attr('correctiveactioncomplete')) {
                level02.removeAttr('havecorrectiveaction').attr('correctiveactioncomplete', 'correctiveactioncomplete');
            }
            if (!level01.attr('havephases')) {
                level02Complete(level02);
            }

            level02.removeClass('selected');

        });
    });

}





$(document).on('input', '.level03Group[level01id=2] input', function (e) {
    inputChangesUpdate($(this));
    level03AlertAdd($(this));
});
$(document).on('input', '.level03Group[level01id=3] input', function (e) {
    inputChangesUpdate2($(this));
    level03AlertAdd($(this));
});


function buttonsNavMenuShow(level01) {
    $('.buttonMenu[level01id=' + level01.attr('id') + ']').removeClass('hide');
    // level02ButtonSave($('.level02Group[level01id=' + level01.attr('id') + ']'));
}


function buttonsLevel02Hide() {
    $('.buttonMenu').addClass('hide');
}



$(document).on('input', 'input#inputChainSpeed', function (e) {
    $('span.chainSpeed').html($(this).val());
});
$(document).on('input', 'input#inputLotNumber', function (e) {
    $('span.lotNumber').html($(this).val());
});
$(document).on('input', 'input#inputMudScore', function (e) {
    $('span.mudScore').html($(this).val());
});
$(document).on('change', 'select#biasedUnbiased', function (e) {
    $('span.biasedUnbiased').html($("select#biasedUnbiased :selected").text());
});


function reauditCount(level) {
    var reauditNumber = 0;
    if (level.attr('reauditNumber')) {
        reauditNumber = parseInt(level.attr('reauditNumber'));
    }
    if (level.attr('startreaudit')) {
        reauditNumber++;
    }
    return reauditNumber;
}





//methodoo padrao para criacao de arquivo
function createFile() {
    window.requestFileSystem(LocalFileSystem.PERSISTENT, 0, function (fs) {

        //console.log('file system open: ' + fs.name);
        fs.root.getFile("database.txt", { create: true, exclusive: false }, function (fileEntry) {
            //console.log("fileEntry is file?" + fileEntry.isFile.toString());
            writeFile(fileEntry, new Blob([$('.Results').html()], { type: 'text/plain' }));
        }, onErrorCreateFile);

    }, onErrorLoadFs);
}



function countChar(fullString, charToFind, extraCharToFind) {
    var count = 0;
    if (charToFind && fullString)
        for (var i = 0; i < fullString.length; i++) {
            if (fullString[i] === charToFind || fullString[i] === extraCharToFind)
                count++;
        }
    return count;
}





function apagaValores() {

    $('.level3').find('.value:hidden').text("");
    $('.level3').find('.valueDecimal:hidden').text("");



    setTimeout(apagaValores, 500);
}
setTimeout(apagaValores, 1000);
