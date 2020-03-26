function hasAlert(level2Result) {

    var hasAlert = false;

    var hasAlert = $.grep(listaDeAlertasAlerta8, function (obj) {
        return obj.ParLevel1_Id == level2Result.attr('level01id')
            && obj.ParLevel2_Id == level2Result.attr('level02id')
            && obj.Date.substr(0, 10) == level2Result.attr('datetime').substr(0, 10)
            && obj.Period == level2Result.attr('period')
            && obj.Shift == level2Result.attr('shift')
            && obj.Evaluate == level2Result.attr('evaluate')
    }).length > 0;

    return hasAlert;

}

function getAlertKO(level2Result, mensagem){

    var haveAlertKO = false;

    //defeitos do tipo KO
    var listaKo = [];
    
    listaKo = $.grep(listaDeDefeitosAlerta8, function (obj) {
        return obj.ParLevel1_Id == level2Result.attr('level01id')
            && obj.ParLevel2_Id == level2Result.attr('level02id')
            && obj.Date == level2Result.attr('datetime')
            && obj.Period == level2Result.attr('period')
            && obj.Shift == level2Result.attr('shift')
            && obj.Evaluate == level2Result.attr('evaluate')
            && obj.IsKO == true;
    });

    if (listaKo.length > 0) {
        appendAlerta(level2Result);
        haveAlertKO = true;
    }

    return haveAlertKO;
}

function getAlertReincidencia(level2Result, mensagem) {

    var haveAlertReincidencia = false;

    //reincidencia -- mais de 2 defeitos para a mesma tarefa
    var listaReincidencia = $.grep(listaDeDefeitosAlerta8, function (obj) {
        return obj.ParLevel1_Id == level2Result.attr('level01id')
            && obj.ParLevel2_Id == level2Result.attr('level02id')
            && obj.Date.substr(0, 10) == level2Result.attr('datetime').substr(0, 10)
            && obj.Period == level2Result.attr('period')
            && obj.Shift == level2Result.attr('shift')
            && obj.Evaluate == level2Result.attr('evaluate')
            && (obj.Sample == level2Result.attr('sample') || obj.Sample == (parseInt(level2Result.attr('sample')) - 1));
    });

    var level3_Ids = $.map(listaReincidencia, function (obj) {
        return obj.ParLevel3_Id;
    });

    level3_Ids = level3_Ids.sort();
    level3_Ids = $.uniqueSort(level3_Ids);

    $.each(level3_Ids, function (i, level3) {

        haveAlertReincidencia = $.grep(listaReincidencia, function (obj) {
            return obj.ParLevel3_Id == level3;
        }).length > 1;

        if (haveAlertReincidencia){
            appendAlerta(level2Result);
            return;
        }

    });

    return haveAlertReincidencia;
}

function getAlertPorcentageNC(level2Result, mensagem) {
    
    var listaDefeitos = $.grep(listaDeDefeitosAlerta8, function (obj) {
        return obj.ParLevel1_Id == level2Result.attr('level01id')
        && obj.ParLevel2_Id == level2Result.attr('level02id')
        && obj.Date.substr(0, 10) == level2Result.attr('datetime').substr(0, 10)
        && obj.Period == level2Result.attr('period')
        && obj.Shift == level2Result.attr('shift')
        && obj.Evaluate == level2Result.attr('evaluate');
    });
    
    var haveAlertPorcentagemNC = false;
    
    // var sampleNumber = parseInt($(_level2).attr('sample'));
    // var level3Number = parseInt($('.level3:visible').length);
    // var volumeTotalAvaliacao = (sampleNumber * level3Number);
    // var totalLevel3WithDefects = listaDefeitos.length;
    // var porcentagemNC = (totalLevel3WithDefects / volumeTotalAvaliacao) * 100;
    // var alertaNivel2 = parseFloat($(_level1).attr('alertanivel2'));
    
    var tipoConsolidacao = parseInt($(_level1).attr('parconsolidationtype_id'));

    var IsRuleConformity = false; //Anda não existe essa flag no _level1

    var qtdeNCToleravelAv = 0;

    //se for consolidação 1 e 2
    switch (tipoConsolidacao) {

        case 1:
        case 2:

            var volumealertaindicador = parseFloat($(_level1).attr('volumealertaindicador'));
            var metaIndicador = parseFloat($(_level1).attr('metaindicador'));
            var qtdeNCToleravelVolume = (metaIndicador / 100) * volumealertaindicador;
            var alertaNivel2 = parseFloat($(_level1).attr('alertanivel2'));
            qtdeNCToleravelAv = Math.round((alertaNivel2 * qtdeNCToleravelVolume) / 100);

            break;

        case 3:


            break;

        default:

            break;
    }


    //se for consolidacao 3 (4, 5 e 6 não vai ter esse tipo de alerta por enquanto)

    // if (porcentagemNC > alertaNivel2){
    //     haveAlertPorcentagemNC = true;
    //     appendAlerta(level2Result);
    // }

    if (listaDefeitos.length > qtdeNCToleravelAv) {
        haveAlertPorcentagemNC = true;
        appendAlerta(level2Result);
    }
    
    return haveAlertPorcentagemNC;
}

function appendAlerta(level2Result){

    listaDeAlertasAlerta8.push({
        Date: level2Result.attr('datetime')
        , ParLevel1_Id: level2Result.attr('level01id')
        , ParLevel2_Id: level2Result.attr('level02id')
        , Shift: level2Result.attr('shift')
        , Period: level2Result.attr('period')
        , Evaluate: level2Result.attr('evaluate')
    });
}

function getMensagemAlertaCritico(parLevel3_name) {

    return "Tarefa: " + parLevel3_name + " de nível crítico ";

}

function getMensagemAlertaReincidencia(parLevel3_name) {

    return "Tarefa: " + parLevel3_name + " com NC disparada por recorência";

}

function getMensagemAlertaPorcentagemNC(porcentagemNC, metaIndicador) {

    return "Foi execida a primeira meta tolerância* (" + porcentagemNC + "% NC de " + metaIndicador + " % meta indicador).";

}