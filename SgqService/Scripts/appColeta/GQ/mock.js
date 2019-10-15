

var countHeaderFieldGroup = 0;

function preenchePCC1b(){
    //debugger
    if(_level1 && _level1.id.replace('98789','|').split('|')[1] == 3){
        $('#DescriptionFailure').val('Foi encontrado fezes e/ou ingesta no quarto.');
        $('#ImmediateCorrectiveAction').val('A nória foi paralizada e fez-se a remoção da porção contaminada.');
        $('#ProductDisposition').val('Verificou-se a carcaça novamente e ela foi liberada.');
        $('#PreventativeMeasure').val('Será analisado junto com o supervisor');
    }
}

$(document).on('click','#btnMessageOk', function(e){
    preenchePCC1b(); 
});

function clonarHF(a){ 
    var headerFieldGroupVisiveis = $('[hfg]:visible').not('[data-vinculo]');
    countHeaderFieldGroup++;
    headerFieldGroupVisiveis = $.grep(headerFieldGroupVisiveis, function(o, c){ return $(o).attr('hfg') == $(a).attr('hfg') }); 
    $.each(headerFieldGroupVisiveis,function(i,o){
        if(!$(o).parent().attr('data-vinculo')){
            var elementoClonado = $(o).parent().clone(true, true);
            elementoClonado.attr('data-vinculo',countHeaderFieldGroup);
            elementoClonado.insertAfter($(o).parent());
        }
    });
}

function removerHF(a){ 
    var headerFieldGroupVisiveis = $('[data-vinculo='+$(a).parent().attr('data-vinculo')+']:visible');
    $.each(headerFieldGroupVisiveis,function(i,o){
        $(o).remove();
    });
}

$(document).ready(function(){
    $('body')
        .off('input', 'input.interval:visible, input.likert:visible, input.texto[type="date"], input.texto[type="time"]')
        .on('input', 'input.interval:visible, input.likert:visible, input.texto[type="date"], input.texto[type="time"]', function () {

        var id = $(this).parents('li').attr('id');
        $.each($('input[resultado]:visible'), function(i, o){
            if ($(o).attr('resultado').indexOf('{' + id + '}') >= 0 || $(o).attr('resultado').indexOf('{' + id + '?}') >= 0){
                var resultado = $(o).attr('resultado');

                const regex = /{([^}]+)}/g;
                var m;

                while ((m = regex.exec($(o).attr('resultado'))) !== null)
                {
                    // This is necessary to avoid infinite loops with zero-width matches
                    if (m.index === regex.lastIndex)
                    {
                        regex.lastIndex++;
                    }

                    var valor = $('li[id="' + m[1].replace('?','') + '"] input.interval').val();
                    if(valor)
                        resultado = resultado.replace(m[0],valor);
                    else{
                        var valor = $('li[id="' + m[1].replace('?','') + '"] input.likert').val();
                        if (valor)
                            resultado = resultado.replace(m[0], valor);
                        else {
                            var valor = $('li[id="' + m[1].replace('?', '') + '"] input.texto[type="date"]').val();
                            if (valor)
                                resultado = resultado.replace(m[0], valor);
                            else {
                                var valor = $('li[id="' + m[1].replace('?', '') + '"] input.texto[type="time"]').val();
                                if (valor)
                                    resultado = resultado.replace(m[0], valor);
                                else {
                                    if (m[1].indexOf('?') >= 0) {
                                        resultado = resultado.replace(m[0], 0);
                                    }
                                }
                            }
                        }
                    }
                }

                if (resultado.indexOf('{') != -1)
                {
                    resultado = "";
                }
                else
                {
                    resultado = eval(resultado);
                }
                $(o).val(resultado);
                $(o).trigger('input');

            }
        });
    });

    $('body').on('click','.level2',function(){
        var self = this;
        PesoHB(self);
    });
});

var mediaPesoHB = [];

function CalculoMediaPesoHB(){
    var sum = 0;
    for( var i = 0; i < mediaPesoHB.length; i++ ){
        sum += parseFloat( mediaPesoHB[i] ); //don't forget to add the base
    }

    if(mediaPesoHB.length == 0){
        return 0;
    }

    var media = sum/mediaPesoHB.length;

    if($(_level2).attr('sample') == 20){
        media -= 0.640;
    }else if($(_level2).attr('sample') == 32){
        media -= 0.485;
    }else if($(_level2).attr('sample') == 80){
        media -= 0.295;
    }

    return media;
}

function ResetaCorMediaPesoHB(timeout){
    timeout = timeout ? timeout : 100;
    setTimeout(function(){
        if(CalculoMediaPesoHB() < parseFloat($('#'+getDicionario('IdTarefaPesoHB')+'.level3').attr('intervalmin'))){
            $('.level3List .calculoPesoHB').addClass('lightred');
        }else{
            $('.level3List .calculoPesoHB').removeClass('lightred');
        }
    }, timeout);
}

function PesoHB(self){
    var level1 = $(_level1).attr('id').split('98789');
    if(level1[1] == parseInt(getDicionario('IdIndicadorPesoHB'))){
        var id = $(self).attr('id');
        var cluster_level2 = id.split('98789');
        if(cluster_level2.length > 0)
            id = cluster_level2[1];
		
        setTimeout(
            function(){
                console.log('AQUI VC FAZ AS REGRAS DO HAMBURGUER');


                var minimo = parseInt($('#'+getDicionario('IdTarefaPesoHB')+'.level3').attr('intervalmin'));
                var tara = parseFloat($('#'+getDicionario('IdCabecalhoTaraPesoHB')).val());
                if (isNaN(tara))
                    tara = 0;
					
                $('.level3List .calculoPesoHB').remove();
                var ultimoLevel3 = $('.level3List .level3:last').clone();
                $(ultimoLevel3).addClass('calculoPesoHB');
                $(ultimoLevel3).find('.col-xs-4 .levelName').text('Média peso HB');
                $(ultimoLevel3).find('.col-xs-3 .levelName').text('Min: ' + (minimo) + 'g');


                if(parseInt($('span.sampleCurrent:visible').text()) <= 1)
                    mediaPesoHB = [];

                $(ultimoLevel3).find('.col-xs-3.counters').addClass('medicaCalculoPesoHB').text('Média: ' + CalculoMediaPesoHB() + 'g');
                $(ultimoLevel3).find('.col-xs-2').html('');
					
                $('.level3List').off('blur', '#'+getDicionario('IdCabecalhoTaraPesoHB'));
                $('.level3List').on('blur', '#'+getDicionario('IdCabecalhoTaraPesoHB'), function(){
                    PesoHB(self);
                });
					
                $('.level3List').off('change', '#'+getDicionario('IdCabecalhoQuantidadeAmostraPesoHB'));
                $('.level3List').on('change', '#'+getDicionario('IdCabecalhoQuantidadeAmostraPesoHB'), function(){
                    var text = $(this).find(':selected').text();
                    //Nº Amostrar
                    $(_level2).attr('sample', text);
                    $('span.sampleTotal:visible').text(text);
                    PesoHB(self);
                });
					
                $('.level3List').append(ultimoLevel3);
                ResetaCorMediaPesoHB(400);
            }
        ,100);
		
    }
}

function validaNumeroEscalaLikert(evt, that)
{
    var e = event || evt; 
    var charCode = e.which || e.keyCode;

    $(that).parents('li').css('background-color', '');

    if(!(charCode == 45 && $(that).val().length == 0)){
        if (charCode > 31 && (charCode < 48 || charCode > 57)){
            e.preventDefault();
            return false;
        }
    }

    aplicaCorAoInput(that);

    return true;
}

function aplicaCorAoInput(input) {

    var paramns = $(input).attr('paramns')

    var properties = paramns.split('|');
    var arr = [];

    properties.forEach(function(property) {
        var tup = property.split(':');
        arr[tup[0]] = [tup[1],tup[2]];
    });

    var value = $(input).val();
    if(!(typeof(arr[value]) == 'undefined') && !(typeof(arr[value][0]) == 'undefined')){
        var color = arr[value][0];
        var valueText = arr[value][1];

        $(input).parents('li').attr('value', valueText);
        $(input).parents('li').css('background-color', color);
    }
}

function validaValoresValidosEscalaLikert(input) {

    if((!(typeof($(input).val()) == 'undefined') && $(input).val().length <= 0) || parseInt($(input).attr('min')) > $(input).val()
        || parseInt($(input).attr('max')) < $(input).val()){
        $(input).val('');
        $(input).parents('li').css('background-color', '');
        $(input).trigger('input');
    }

}

function calcularSensorial(list){

    var attributes = list

    //declare attribute and point counter
    var noOf5 = 0
    var noOf4and6 = 0
    var noOf3and7 = 0
    var noOf2and8 = 0
    var noOf1and9 = 0
    var noOfElem = 0 
    var addPoint5_85 = 0
    var addPoint5_60 = 0
    var addPoint4and6 = 0
    var CategoryScore_calc = 0

    for (i = 0; i < attributes.length; i++){

        if (attributes[i] == 1 || attributes[i] == 9){
            noOf1and9 = noOf1and9 + 1
        }else if (attributes[i] == 2 || attributes[i] == 8){
            noOf2and8 = noOf2and8 + 1
        }else if (attributes[i] == 3 || attributes[i] == 7){
            noOf3and7 = noOf3and7 + 1
        }else if (attributes[i] == 4 || attributes[i] == 6){
            noOf4and6 = noOf4and6 + 1
        }else if (attributes[i] == 5){
            noOf5 = noOf5 + 1
        }

    }

    noOfElem = 1 > (noOf4and6 + noOf3and7 + noOf5 - 1) ? 1 : (noOf4and6 + noOf3and7 + noOf5 - 1)

    addPoint5_85 = (10 * noOf5 / noOfElem)
    addPoint5_60 = (20 * noOf5 / noOfElem)
    addPoint4and6 = (10 * noOf4and6 / noOfElem)

    if (noOf1and9 > 0) {
        CategoryScore_calc = 0
    }
    else if (noOf2and8 > 0) {
        CategoryScore_calc = 25
    }
    else if (noOf3and7 > 0) {
        CategoryScore_calc = 60 + addPoint5_60 + addPoint4and6
    }
    else if (noOf4and6 > 0) {
        CategoryScore_calc = 85 + addPoint5_85
    }
    else if (noOf5 = 0) {
        CategoryScore_calc = 0
    }
    else {
        CategoryScore_calc = 100
    } 

    //imprimir na tela
    return  Math.round( CategoryScore_calc)

}

function RetornaValor0SeUndefined(valor){
    if(typeof(valor) == 'undefined' || valor == 'undefined'){
        return 0;
    }else{
        return valor
    }
}

function datedif(data1, data2) {
    var d1 = new Date(data1);
    var d2 = new Date(data2);

    var timeDifference = d2.getTime() - d1.getTime();
    var DaysDifference = timeDifference / 86400000;

    return DaysDifference;
}

function timedif(hora1, hora2) {
    var shora1 = hora1.split(":");
    var shora2 = hora2.split(":");

    var min1 = (parseInt(shora1[0]) * 60) + parseInt(shora1[1]);
    var min2 = (parseInt(shora2[0]) * 60) + parseInt(shora2[1]);

    return min2 - min1;
}