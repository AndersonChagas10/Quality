
function updateReaudit(level) {

    if (level == 1) {

        $('li:visible > .userInfo > .btnReaudit').addClass('hide');
        var shift = $('.App').attr('shift');
        var period = $('.App').attr('period');
        var unidade = $('.App').attr('unidadeid');

        var existeReaditLevel1 = $('.Resultlevel2[period=' + period + '][shift=' + shift + '][unitid=' + unidade + '][reauditlevel=1][havereaudit=true]');
        var existeReaditLevel2 = $('.Resultlevel2[period=' + period + '][shift=' + shift + '][unitid=' + unidade + '][reauditlevel=2][havereaudit=true]');


        /*--------------------- Level 1 -----------------------------*/
        /*------- Define se exibe ou nao btn reaudit no level 1 -----*/
        existeReaditLevel1.each(function (c, o) {

            if($('.level1[id='+$(o).attr("level1id")+'][reaudit=true]').length > 0) {
                var quantidadeLevel2AvaliadosQuandoEstourou = $('.Resultlevel2[level1id=' + $(o).attr("level1id") + '][period=' + period + '][shift=' + shift + '][reauditnumber=' + parseInt($(existeReaditLevel1).attr('reauditnumber')) + ']').length;
                var quantidadeLevel2AvaliadosAtual = $('.Resultlevel2[level1id=' + $(o).attr("level1id") + '][period=' + period + '][shift=' + shift + '][reauditnumber=' + (parseInt($(existeReaditLevel1).attr('reauditnumber')) + 1) + ']').length;
                var level1 = $('.level1[id=' + $(o).attr('level1id') + ']');
    
                if (quantidadeLevel2AvaliadosQuandoEstourou > quantidadeLevel2AvaliadosAtual) {
                    level1.siblings('.userInfo').children('.btnReaudit').removeClass('hide');
		    $(".level1[id=" + $(o).attr("level1id") + "]").siblings('.userInfo').children('.btnReaudit').removeClass('hide');
                }else if(level1.attr('hasgrouplevel2')== 'true'){
                    level1.siblings('.userInfo').children('.btnReaudit').removeClass('hide');
		    $(".level1[id=" + $(o).attr("level1id") + "]").siblings('.userInfo').children('.btnReaudit').removeClass('hide');
                }
    
                var rnumberatual = parseInt($(o).attr('reauditnumber')) + 1;
                var level2 = $('.level2[id=' + $(o).attr('level2id') + ']');
                level1.addClass('selected');
                // var completo = getCompletedLevel1(level1.attr('id'),shift,period,unidade);
    
                // if(level1.attr('hasgrouplevel2') == 'true') {
                var completo = ReauditCompleta(level1.attr('id'),shift,period,rnumberatual,quantidadeLevel2AvaliadosQuandoEstourou)
                //}
    
                level1.removeClass('selected');
                var prox = $('.Resultlevel2[level1id=' + $(o).attr('level1id') + '][period=' + period + '][shift=' + shift + '][unitid=' + unidade + '][reauditnumber=' +
                    rnumberatual + '][reauditlevel=1][havereaudit=true]:last');
                var temProx = $('.Resultlevel2[level1id=' + $(o).attr('level1id') + '][period=' + period + '][shift=' + shift + '][unitid=' + unidade + '][reauditnumber=' +
                    rnumberatual + ']');
    
                if (level1.attr('hasgrouplevel2') == 'true') {
                    if (prox.length == 0 && temProx.length > 0 && completo){
                        level1.siblings('.userInfo').children('.btnReaudit').addClass('hide');
                	$(o).attr('reauditnumber', 0);
                	$(o).attr('havereaudit', false);
                	$('.level1[id='+$(o).attr("level1id")+']').attr('havereaudit', false);
		    }
                } else {
                    if (prox.length == 0 && temProx.length > 0){
                        level1.siblings('.userInfo').children('.btnReaudit').addClass('hide');
                	$(o).attr('reauditnumber', 0);
                	$(o).attr('havereaudit', false);
                	$('.level1[id='+$(o).attr("level1id")+']').attr('havereaudit', false);
		    }
                }
            } else {

		$(".level1[id=" + $(o).attr("level1id") + "]").siblings('.userInfo').children('.btnReaudit').removeClass('hide');
               
            }
            
        });

        /*--------------------- FIM Level 1 -----------------------------*/
        /*--------------------- Level 2 -----------------------------*/

        if($(existeReaditLevel1[existeReaditLevel1.length-1]).attr('reauditnumber') < $(existeReaditLevel2.filter('[level1id='+existeReaditLevel1.attr('level1id')+
            ']')[existeReaditLevel2.length-1]).attr('reauditnumber') || existeReaditLevel1.filter('[level1id='+existeReaditLevel2.attr('level1id')+
            ']').length == 0){

        existeReaditLevel2.each(function (c, o) {
            if($('.level2[id='+$(o).attr("level2id")+'][reaudit=true]').length > 0) {

                var quantidadeLevel2AvaliadosQuandoEstourou = $('.Resultlevel2[level1id=' + $(o).attr("level1id") + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + '][reauditnumber=' + parseInt($(existeReaditLevel2).attr('reauditnumber')) + ']').length;
                var quantidadeLevel2AvaliadosAtual = $('.Resultlevel2[level1id=' + $(o).attr("level1id") + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + '][reauditnumber=' + (parseInt($(existeReaditLevel2).attr('reauditnumber')) + 1) + ']').length;
                if (quantidadeLevel2AvaliadosQuandoEstourou > quantidadeLevel2AvaliadosAtual) {
                    $(".level1[id=" + $(o).attr("level1id") + "]").siblings('.userInfo').children('.btnReaudit').removeClass('hide');
                }
                var rnumberatual = parseInt($(o).attr('reauditnumber')) + 1;
                var temProx = $('.Resultlevel2[level1id=' + $(o).attr("level1id") + '][period=' + $('.App').attr('period') + '][shift=' + $('.App').attr('shift') + '][unitid=' +
                    $('.App').attr('unidadeid') + '][reauditnumber=' + rnumberatual + ']').length;
                var prox = $('.Resultlevel2[level1id=' + $(o).attr("level1id") + '][period=' + $('.App').attr('period') + '][shift=' + $('.App').attr('shift') + '][unitid=' +
                    $('.App').attr('unidadeid') + '][reauditnumber=' + rnumberatual + '][havereaudit=true]');
                var jaAcabou = $('.Resultlevel2[level1id=' + $(o).attr("level1id") + '][period=' + $('.App').attr('period') + '][shift=' + $('.App').attr('shift') + '][unitid=' +
                    $('.App').attr('unidadeid') + '][reauditnumber=' + (rnumberatual) + ']').length;
                if (prox.length == 0 && temProx > 0) {
                    $(".level1[id=" + $(o).attr("level1id") + "]").siblings('.userInfo').children('.btnReaudit').addClass('hide');
   		    $(o).attr('reauditnumber', 0);
                    $(o).attr('havereaudit', false);
                    $('.level2[id='+$(o).attr("level2id")+']').attr('havereaudit', false);
                } else if (prox.attr('reauditlevel') == '1' && jaAcabou > 0) {// && prox.attr('havereaudit') == 'false') {
                    $(".level1[id=" + $(o).attr("level1id") + "]").siblings('.userInfo').children('.btnReaudit').addClass('hide');
   		    $(o).attr('reauditnumber', 0);
                    $(o).attr('havereaudit', false);
                    $('.level2[id='+$(o).attr("level2id")+']').attr('havereaudit', false);
                } else {
                    $(".level1[id=" + $(o).attr("level1id") + "]").siblings('.userInfo').children('.btnReaudit').removeClass('hide');
                }
            } else {
                $(".level1[id=" + $(o).attr("level1id") + "]").siblings('.userInfo').children('.btnReaudit').removeClass('hide');
		
            }
        });

        }

        /*--------------------- FIM Level 2  -----------------------------*/

    } else if (level == 2) {

        var existeReaditLevel1 = $('.Resultlevel2[period=' + $('.App').attr('period') + '][shift=' + $('.App').attr('shift') + '][unitid=' + $('.App').attr('unidadeid') + '][reauditlevel=1][havereaudit=true]:last');
        var existeReaditLevel2 = $('.Resultlevel2[period=' + $('.App').attr('period') + '][shift=' + $('.App').attr('shift') + '][unitid=' + $('.App').attr('unidadeid') + '][reauditlevel=2]')



        //var existeReaditLevel2 = $('.Resultlevel2[period=' + $('.App').attr('period') + '][shift=' + $('.App').attr('shift') + '][unitid=' + $('.App').attr('unidadeid') + '][reauditlevel=2]')

        ///*---------------------Level 1-----------------------------*/
        ///*Define se exibe ou nao btn reaudit no level 1*/
        //existeReaditLevel1.each(function (c, o) {
        //    var quantidadeLevel2AvaliadosQuandoEstourou = $('.Resultlevel2[level1id=' + $(o).attr("level1id") + '][reauditnumber=' + parseInt($(existeReaditLevel1).attr('reauditnumber')) + ']').length;
        //    var quantidadeLevel2AvaliadosAtual = $('.Resultlevel2[level1id=' + $(o).attr("level1id") + '][reauditnumber=' + (parseInt($(existeReaditLevel1).attr('reauditnumber')) + 1) + ']').length;
        //    if (quantidadeLevel2AvaliadosQuandoEstourou > quantidadeLevel2AvaliadosAtual) {
        //        $('li:visible').removeClass('bgCompleted');
        //        $('li:visible > .userInfo > .btnReaudit').removeClass('hide');
        //    }
        //});

    }
}


$(document).on('click', '.btnReaudit', function (e) {

    //Seleciono o Level01 a partir do botão que cliquei
    var level1 = $(this).parents('.userInfo').siblings('.level1');
    var level2 = $(this).parents('.userInfo').siblings('.level2');
    var existeReaditLevel1 = $('.Resultlevel2[level1id=' + level1.attr('id') + '][period=' + $('.App').attr('period') + '][shift=' + $('.App').attr('shift') + '][unitid=' + $('.App').attr('unidadeid') + '][reauditlevel=1][havereaudit=true]:last');

    if (level1.length > 0) {

        if (($(level1).attr('havereaudit') == 'true') || (existeReaditLevel1.length > 0)) {
            level1.attr('isreaudit', 'true');
            sendToReaudit = true;
            $('.level2Group[level01id=' + level1.attr('id') + '] .level2').attr('havereaudit', 'true');
            var anteriorReauditQueEstorou = $('.Resultlevel2[level1id=' + level1.attr('id') + '][unitid=' + $('.App').attr('unidadeid') + '][reauditnumber=' + (parseInt(existeReaditLevel1.attr('reauditnumber')) - 1) +
                '][havereaudit=true][reauditlevel=2]').length;

            if (anteriorReauditQueEstorou > 0)
                level1.attr('reauditnumber', parseInt(existeReaditLevel1.attr('reauditnumber')) + 1)

            // var reauditnumber = $('.Resultlevel2[reauditnumber]')
            //level1.attr('reauditnumber', 1);
        } else {
            var minR = $('.Resultlevel2[level1id=' + level1.attr('id') + '][havereaudit="true"][reauditlevel="2"][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + ']');
            var min = minR.length == 0 ? 0 : parseInt(minR.filter(':first').attr('reauditnumber'));
            minR.each(function (index, self) {
                if (min > parseInt($(self).attr('reauditnumber'))) {
                    min = parseInt($(self).attr('reauditnumber'));
                }
            });
            min = min + 1;

            level1.attr('reaudminlevel', min);
        }

         

        level1.click();

        ReauditByHeader.triggerReaudit = true;


    } else if (level2.length > 0) {

        level2.attr('isreaudit', true);
        level2.click();

    }
});

function isLevelReaudit() {
    if ($(".level1.selected").attr("isreaudit") == "true" || ($(".level1.selected").attr("isreaudit") != "true" && $(".level2.selected").attr("isreaudit") == "true")) {
        return true;
    } else {
        return false;
    }
}

function ReauditCompleta(level1,shift,period,reauditnumber,quantosTenhoqueAvaliar){
    var total = 0;
    var list = $.grep(counterLevel1, function(o) {
        if(o.parlevel1 == level1 && o.shift == shift && o.period == period){
            total = o.total;
        }
    });
    
    var avaliados = $('.Resultlevel2[level1id='+level1+'][period='+period+'][shift='+shift+'][reauditnumber='+reauditnumber+'][evaluation='+total+']').length
    if(avaliados == quantosTenhoqueAvaliar)
        return true;
    else
        return false;
}