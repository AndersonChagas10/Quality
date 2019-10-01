$(document).on('change', '.painelLevel03 select.selectComponente', function () {

    processComponente(this);

});

function processComponente(thiss) {


    if (!headerIsValid($(thiss).attr('Componente_id'))) {
        return false;
    }

    var arrColumName = getColumNames($(thiss).attr('Componente_id'));

    //pegar os limites usando a função getLimits
    var limites = getComponenteGenericoValor(arrColumName);

    if (!limites) {
        return false;
    }

    //buscar as tarefas com os limites correspondentes
    var limtesArr = limites.Valor.split('|');

    if (limtesArr && limtesArr.length > 0) {

        limtesArr.forEach(function (limite) {

            //trocar os limites
            var tarefa_Id = limite.split(':')[0];
            var limiteInferior = limite.split(':')[1];
            var limiteSuperior = limite.split(':')[2];

            changeLimitLevel3(tarefa_Id, limiteSuperior, limiteInferior)
        });
    }
}

function changeLimitLevel3(tarefa_Id, limiteSuperior, limiteInferior) {

    if (!tarefa_Id) {
        return false;
    }

    trocarLimitesLinhaLevel3(tarefa_Id, limiteSuperior, limiteInferior)
}

function trocarLimitesLinhaLevel3(tarefa_Id, limiteSuperior, limiteInferior) {

    //Intervalo
    if (!isNaN(parseFloat(limiteSuperior)) && !isNaN(parseFloat(limiteInferior))) {


        //Trocar os limites da tarefa intervalo
        $('.level3[id="' + tarefa_Id + '"]').attr('intervalmin', limiteInferior);
        $('.level3[id="' + tarefa_Id + '"]').attr('intervalmax', limiteSuperior);

        var posicaoLimiteInferior = $($('.level3[id="' + tarefa_Id + '"]').find('.levelName')[1]).html().split('~')[0].indexOf('</b>') + 4;
        var posicaoLimteSuperior = $($('.level3[id="' + tarefa_Id + '"]').find('.levelName')[1]).html().split('~')[1].indexOf('</b>') + 4;
        var textoLimiteInferior = $($('.level3[id="' + tarefa_Id + '"]').find('.levelName')[1]).html().split('~')[0].substring(0, posicaoLimiteInferior);
        var textoLimiteSuperior = $($('.level3[id="' + tarefa_Id + '"]').find('.levelName')[1]).html().split('~')[1].substring(0, posicaoLimteSuperior);
        var textoFinal = $($('.level3[id="' + tarefa_Id + '"]').find('.levelName')[1]).html().split('~')[1].substring(posicaoLimteSuperior);
        var unidadeMedida = textoFinal.substring(textoFinal.indexOf(' '), textoFinal.length);

        $($('.level3[id="' + tarefa_Id + '"]').find('.levelName')[1]).html(textoLimiteInferior + limiteInferior + " ~" + textoLimiteSuperior + limiteSuperior + unidadeMedida);

    } else if (!isNaN(parseFloat(limiteSuperior)) && isNaN(parseFloat(limiteInferior))) { //Deve ser Menor que

        $('.level3[id="' + tarefa_Id + '"]').attr('intervalmax', limiteSuperior);

        var posicaoLimteSuperior = $($('.level3[id="' + tarefa_Id + '"]').find('.levelName')[1]).html().indexOf('</b>') + 4;
        var textoLimiteSuperior = $($('.level3[id="' + tarefa_Id + '"]').find('.levelName')[1]).html().substring(0, posicaoLimteSuperior);
        var textoFinal = $($('.level3[id="' + tarefa_Id + '"]').find('.levelName')[1]).html().substring(posicaoLimteSuperior);
        var unidadeMedida = textoFinal.substring(textoFinal.indexOf(' '), textoFinal.length);

        $($('.level3[id="' + tarefa_Id + '"]').find('.levelName')[1]).html(textoLimiteSuperior + limiteSuperior + unidadeMedida);


    } else if (!isNaN(parseFloat(limiteInferior)) && isNaN(parseFloat(limiteSuperior))) { //Deve ser Maior que 

        $('.level3[id="' + tarefa_Id + '"]').attr('intervalmin', limiteInferior);

        //Trocar os limite inferior da tarefa
        var posicaoLimiteInferior = $($('.level3[id="' + tarefa_Id + '"]').find('.levelName')[1]).html().indexOf('</b>') + 4;
        var textoLimiteInferior = $($('.level3[id="' + tarefa_Id + '"]').find('.levelName')[1]).html().substring(0, posicaoLimiteInferior);
        var textoFinal = $($('.level3[id="' + tarefa_Id + '"]').find('.levelName')[1]).html().substring(posicaoLimiteInferior);
        var unidadeMedida = textoFinal.substring(textoFinal.indexOf(' '), textoFinal.length);

        $($('.level3[id="' + tarefa_Id + '"]').find('.levelName')[1]).html(textoLimiteInferior + limiteInferior + unidadeMedida);

    }

}

function getComponenteGenericoValor(arrColumName) {

    var valor = $.grep(listComponenteGenericoValores, function (obj) {
        return eval("(obj.ComponenteGenericoTipoColuna_Id == 8 || obj.Name == " + arrColumName.join(" || obj.Name == ") + ")");
    });

    if (valor.length > arrColumName.length)
        return $.grep(valor, function (obj) {
            return obj.ComponenteGenericoTipoColuna_Id == 8;
        })[0];

    else
        return null
}

function headerIsValid(componente_Id) {

    var retorno = true;

    $('.painelLevel03 select.selectComponente[componente_id="' + componente_Id + '"]').each(function () {

        var valorSelecionado = $(this).find('option:selected').val();

        if (valorSelecionado == undefined || valorSelecionado == null || valorSelecionado == "") {
            retorno = false;
        }

    });

    return retorno;
}

function getColumNames(componente_Id) {

    var arrReturn = [];

    $('.painelLevel03 select.selectComponente[componente_id="' + componente_Id + '"]').each(function () {

        arrReturn.push('\'' + $(this).attr('componenteGenericoColuna') + '\'');

    });

    return arrReturn;

}