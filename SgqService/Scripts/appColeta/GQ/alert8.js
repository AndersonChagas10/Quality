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

function getAlertKO(level2Result){

    var alerta8 = {};
    var mensagem = "";
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
        mensagem = getMensagemAlertaCritico();
        appendAlerta(level2Result);
        haveAlertKO = true;
    }

    alerta8.haveAlert = haveAlertKO;
    alerta8.mensagem = mensagem;

    return alerta8;
}

function getAlertReincidencia(level2Result) {

    var alerta8 = {};
    var mensagem = "";
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
            mensagem = getMensagemAlertaReincidencia();
            appendAlerta(level2Result);
            return;
        }

    });

    alerta8.haveAlert = haveAlertReincidencia;
    alerta8.mensagem = mensagem;

    return alerta8;
}

function getAlertPorcentageNC(level2Result) {
    
    var haveAlertPorcentagemNC = false;   
    var tipoConsolidacao = parseInt($(_level1).attr('parconsolidationtype_id'));
    var IsRuleConformity = $(_level1).attr('isruleconformity') == 'true';
    var qtdeNCToleravelAv = 0;
    var quantidadeDefeitos = 0;
    var alerta8 = {};
    var mensagem = "";

    var listaDefeitos = $.grep(listaDeDefeitosAlerta8, function (obj) {
        return obj.ParLevel1_Id == level2Result.attr('level01id')
        && obj.ParLevel2_Id == level2Result.attr('level02id')
        && obj.Date.substr(0, 10) == level2Result.attr('datetime').substr(0, 10)
        && obj.Period == level2Result.attr('period')
        && obj.Shift == level2Result.attr('shift')
        && obj.Evaluate == level2Result.attr('evaluate');
    });
    
    switch (tipoConsolidacao) {
        //se for consolidação 1 e 2
        case 1:
        case 2:

            var volumeAlertaIndicador = parseFloat($(_level1).attr('volumealertaindicador'));
            var metaIndicador = parseFloat($(_level1).attr('metaindicador'));
            metaIndicador = IsRuleConformity ? (100 - metaIndicador) : metaIndicador;
            var qtdeNCToleravelVolume = (metaIndicador / 100) * volumeAlertaIndicador;
            var alertaNivel2 = parseFloat($(_level1).attr('alertanivel2'));
            qtdeNCToleravelAv = Math.round((alertaNivel2 * qtdeNCToleravelVolume) / 100);

            quantidadeDefeitos = listaDefeitos.length;

            break;

        //se for consolidação 3
        case 3:

            var volumeAlertaIndicador = parseFloat($(_level2).attr('evaluate')) * parseFloat($(_level2).attr('sample'));
            var metaIndicador = parseFloat($(_level1).attr('metaindicador'));
            metaIndicador = IsRuleConformity ? (100 - metaIndicador) : metaIndicador;
            var qtdeNCToleravelVolume = (metaIndicador / 100) * volumeAlertaIndicador;
            var alertaNivel2 = parseFloat($(_level1).attr('alertanivel2'));
            qtdeNCToleravelAv = Math.round((alertaNivel2 * qtdeNCToleravelVolume) / 100);

            var samples = $.map(listaDefeitos, function (obj) {
                return obj.Sample;
            });

            samples = samples.sort();
            samples = $.uniqueSort(samples);

            quantidadeDefeitos = samples.length;

            break;

        //se for consolidação 4, 5 ou 6 (não vai existir por enquanto)
        default:

            quantidadeDefeitos = 0;
            break;
    }

    if (quantidadeDefeitos > qtdeNCToleravelAv) {

        var porcentagemDefeitosAvaliacao = ((qtdeNCToleravelAv * 100) / volumeAlertaIndicador);

        mensagem = getMensagemAlertaPorcentagemNC(porcentagemDefeitosAvaliacao, metaIndicador);

        haveAlertPorcentagemNC = true;
        appendAlerta(level2Result);
    }
    
    alerta8.haveAlert = haveAlertPorcentagemNC;
    alerta8.mensagem = mensagem;

    return alerta8;
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

    // return "A Tarefa: " + parLevel3_name + " de nível crítico <br>O Surpervisor da área será notificado e deverá tomar ações corretivas ";
    return "A Tarefa de nível crítico <br>O Surpervisor da área será notificado e deverá tomar ações corretivas ";

}

function getMensagemAlertaReincidencia(parLevel3_name) {

    // return "Tarefa: " + parLevel3_name + " com NC disparada por recorência";
    return "Tarefa: com NC disparada por recorência";

}

function getMensagemAlertaPorcentagemNC(porcentagemNC, metaIndicador) {

    return "Foi execida a primeira meta tolerância* (" + porcentagemNC + "% NC de " + metaIndicador + " % meta indicador).";

}