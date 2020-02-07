$(document).on('change', '.painelLevel03:visible select.selectComponente', function () {

    processComponente(this);

});

function processComponente(thiss) {

    showAllLevel3();

    if (!headerIsValid($(thiss).attr('Componente_id'))) {
        zeraLimitesComponentes();      
        return false;
    }

    var arrColumName = getColumNames($(thiss).attr('Componente_id'));

    //pegar os limites usando a função getLimits
    var limites = getComponenteGenericoValor(arrColumName);

    if (!limites) {
        zeraLimitesComponentes();
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

    hideLevel3ToHide(limites);

}

function changeLimitLevel3(tarefa_Id, limiteSuperior, limiteInferior) {

    if (!tarefa_Id) {
        return false;
    }

    if ($('.level3[id="' + tarefa_Id + '"]:visible').length == 0) {
        return false;
    }

    if ($('.level3[id="' + tarefa_Id + '"]:visible').attr('inputtype') != 3) {
        return false;
    }

    trocarLimitesLinhaLevel3(tarefa_Id, limiteSuperior, limiteInferior)
}

function trocarLimitesLinhaLevel3(tarefa_Id, limiteSuperior, limiteInferior) {

    //Intervalo
    if (!isNaN(parseFloat(limiteSuperior)) && !isNaN(parseFloat(limiteInferior))) {


        //Trocar os limites da tarefa intervalo
        $('.level3[id="' + tarefa_Id + '"]:visible').attr('intervalmin', limiteInferior);
        $('.level3[id="' + tarefa_Id + '"]:visible').attr('intervalmax', limiteSuperior);

        var posicaoLimiteInferior = $($('.level3[id="' + tarefa_Id + '"]:visible').find('.levelName')[1]).html().split('~')[0].indexOf('</b>') + 4;
        var posicaoLimteSuperior = $($('.level3[id="' + tarefa_Id + '"]:visible').find('.levelName')[1]).html().split('~')[1].indexOf('</b>') + 4;
        var textoLimiteInferior = $($('.level3[id="' + tarefa_Id + '"]:visible').find('.levelName')[1]).html().split('~')[0].substring(0, posicaoLimiteInferior);
        var textoLimiteSuperior = $($('.level3[id="' + tarefa_Id + '"]:visible').find('.levelName')[1]).html().split('~')[1].substring(0, posicaoLimteSuperior);
        var textoFinal = $($('.level3[id="' + tarefa_Id + '"]:visible').find('.levelName')[1]).html().split('~')[1].substring(posicaoLimteSuperior);
        var unidadeMedida = textoFinal.substring(textoFinal.indexOf(' '), textoFinal.length);

        $($('.level3[id="' + tarefa_Id + '"]:visible').find('.levelName')[1]).html(textoLimiteInferior + limiteInferior + " ~" + textoLimiteSuperior + limiteSuperior + unidadeMedida);

    } else if (!isNaN(parseFloat(limiteSuperior)) && isNaN(parseFloat(limiteInferior))) { //Deve ser Menor que

        $('.level3[id="' + tarefa_Id + '"]').attr('intervalmax', limiteSuperior);

        var posicaoLimteSuperior = $($('.level3[id="' + tarefa_Id + '"]:visible').find('.levelName')[1]).html().indexOf('</b>') + 4;
        var textoLimiteSuperior = $($('.level3[id="' + tarefa_Id + '"]:visible').find('.levelName')[1]).html().substring(0, posicaoLimteSuperior);
        var textoFinal = $($('.level3[id="' + tarefa_Id + '"]:visible').find('.levelName')[1]).html().substring(posicaoLimteSuperior);
        var unidadeMedida = textoFinal.substring(textoFinal.indexOf(' '), textoFinal.length);

        $($('.level3[id="' + tarefa_Id + '"]:visible').find('.levelName')[1]).html(textoLimiteSuperior + limiteSuperior + unidadeMedida);


    } else if (!isNaN(parseFloat(limiteInferior)) && isNaN(parseFloat(limiteSuperior))) { //Deve ser Maior que 

        $('.level3[id="' + tarefa_Id + '"]').attr('intervalmin', limiteInferior);

        //Trocar os limite inferior da tarefa
        var posicaoLimiteInferior = $($('.level3[id="' + tarefa_Id + '"]:visible').find('.levelName')[1]).html().indexOf('</b>') + 4;
        var textoLimiteInferior = $($('.level3[id="' + tarefa_Id + '"]:visible').find('.levelName')[1]).html().substring(0, posicaoLimiteInferior);
        var textoFinal = $($('.level3[id="' + tarefa_Id + '"]:visible').find('.levelName')[1]).html().substring(posicaoLimiteInferior);
        var unidadeMedida = textoFinal.substring(textoFinal.indexOf(' '), textoFinal.length);

        $($('.level3[id="' + tarefa_Id + '"]:visible').find('.levelName')[1]).html(textoLimiteInferior + limiteInferior + unidadeMedida);

    }

}

function getComponenteGenericoValor(arrColumName) {

    var where = $.map(arrColumName, function (obj) {
        return "(obj.Name == '"+obj.Name+"' && obj.Valor =='"+obj.Valor+"')";
    });

    var valor = $.grep(listComponenteGenericoValores, function (obj) {
        return eval(where.join(" || "));
    });

    var saveIdDosObjetosFiltrados = $.map(valor, function (obj) {
        return obj.SaveId
    });

    saveIdDosObjetosFiltrados = $.unique(saveIdDosObjetosFiltrados.sort());

    var idSelecionado = $.grep(saveIdDosObjetosFiltrados, function (idFiltrado) {
        var objetosPorId = $.grep(valor, function (obj) {
            return obj.SaveId == idFiltrado;
        }).length;
        return arrColumName.length == objetosPorId;
    });

    if(idSelecionado.length > 0){
        valor = $.grep(listComponenteGenericoValores, function (obj) {
            return obj.SaveId == idSelecionado[0];
        });
    }

    if (valor.length > arrColumName.length)
        return $.grep(valor, function (obj) {
            return obj.ComponenteGenericoTipoColuna_Id == 8;
        })[0];

    else
        return null;
}

function distinct(value, index, self) {
    return self.indexOf(value) === index;
}

function headerIsValid(componente_Id) {

    var retorno = true;

    $('.painelLevel03:visible select.selectComponente[componente_id="' + componente_Id + '"]').each(function () {

        var valorSelecionado = $(this).find('option:selected').val();

        if (valorSelecionado == undefined || valorSelecionado == null || valorSelecionado == "") {
            retorno = false;
        }

    });

    return retorno;
}

function getColumNames(componente_Id) {

    var arrReturn = [];

    $('.painelLevel03:visible select.selectComponente[componente_id="' + componente_Id + '"]').each(function () {

        var obj = { Name: $(this).attr('componenteGenericoColuna'), Valor: $(this).find('option:selected').val()  }

        arrReturn.push(obj);

    });

    return arrReturn;

}


// Como é apresentado na lista, as opções com os valores de componente generico, temos todos os limites cadastrados na tela. Sendo todos os limites
// , temos todas as tarefas. Deve ser montado uma listagem com todas as tarefas possiveis que foram cadastradas no limite.
// Ao ir fazendo a combinação, esta listagem será percorrida e ao não encontrar alguma tarefa a mesma será considerada como NA. 
// O limite quando NA deverá ser min:0 e max:0

//TODO: função que troque os limites das tarefas para 0 e 0
function zeraLimitesComponentes() {
    
    //pegar todos os campos de cabeçalho
    var componentes_Ids = [];

    $(".selectComponente").each(function () {

        componentes_Ids.push(parseInt($(this).attr('componente_id')));

    });

    componentes_Ids = $.unique(componentes_Ids.sort());

    if(componentes_Ids.length == 0)
        return false;

    var objLimites = $(listComponenteGenericoValores).filter(function(i, o){
    
        return (componentes_Ids.includes(parseInt(o.ComponenteGenerico_Id)) && o.ComponenteGenericoTipoColuna_Id == 8);
    
    });

    $(objLimites).each(function(i, limites){

        var limtesArr = limites.Valor.split('|');

        if (limtesArr && limtesArr.length > 0) {
    
            limtesArr.forEach(function (limite) {
    
                //trocar os limites
                var tarefa_Id = limite.split(':')[0];
                var limiteInferior = 0;
                var limiteSuperior = 0;
    
                changeLimitLevel3(tarefa_Id, limiteSuperior, limiteInferior);
            });
        }
    });

}


//TODO: função que faça todos os tipos de combinações
function hideLevel3ToHide(limites) {
    
    var limtesArr = limites.Valor.split('|');

    var level3Arr = $.map(limtesArr, function(limite) {

        return parseInt(limite.split(':')[0]);
    });

    var arrTodosLimites = $(listComponenteGenericoValores).filter(function(i, o){
    
        return (parseInt(limites.ComponenteGenerico_Id) === parseInt(o.ComponenteGenerico_Id) && 
        parseInt(o.ComponenteGenericoTipoColuna_Id) === 8 && 
        parseInt(o.SaveId) !== parseInt(limites.SaveId));
    
    });

    if(arrTodosLimites.length == 0)
        return false;

    var arrTodosLevel3 = $.map(arrTodosLimites, function(limite) {
        return parseInt(limite.Valor.split(':')[0]);
    });

    var arrExclusivo = $.grep(arrTodosLevel3, function (item) {
        return !level3Arr.includes(item);
    });

    arrExclusivo.forEach(function(o){
        hideLevel3ById(parseInt(o));
    });

}

//TODO: função que esconda as tarefas
function hideLevel3ById(level3Id) {
    setLevel3NA(level3Id);
    $('.level3Group').find('[id="' + level3Id + '"]:visible').hide();
}

//TODO: função que exiba todas as tarefas
function showAllLevel3() {
    $('.level3Group').find('li').show();
    removeLevel3NA();
}

function setLevel3NA(level3Id) {

    var $tarefa = $('.level3Group').find('[id="' + level3Id + '"]:visible');

    if (!$tarefa.hasClass('bgNoAvaliable'))
        $tarefa.find('.btnNotAvaliable').trigger('click');

}

function removeLevel3NA() {

    $('.level3Group li').each(function () {

        var $tarefa = $(this);

        if ($tarefa.hasClass('bgNoAvaliable'))
            $tarefa.find('.btnNotAvaliable').trigger('click');

    });
}