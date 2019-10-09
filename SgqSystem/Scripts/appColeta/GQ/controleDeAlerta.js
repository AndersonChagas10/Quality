var alertlevel = null, ncL1 = 0, ncL2 = 0, maxL1 = 0, maxL2 = 0;
function AlertaNumeroNC(level, nc) {
    var controleDeAlerta = 0;
    var max = 0, controle = false;

    if (level == 1)
        if (nc > parseInt($(_level1).attr('alertanivel2'))) {
            controleDeAlerta = 1;
            ncL1 = nc;
            maxL1 = parseInt($(_level1).attr('alertanivel2'));
            /*$(_level1).attr('havereaudit','true');
            zeraLevel2();*/
            atribuiReauditLevel1();
            $('.Resultlevel2:last').attr('reauditlevel','1')
            $('.level02Result:last').attr('reauditlevel','1');
            alertlevel = 1;
        }
    if (level == 2)
        if ($(_level2).length > 0)
            if (nc > parseInt($(_level2).attr('parnotconformityrule_value'))) {
                controleDeAlerta = 1;
                ncL2 = nc;
                maxL2 = parseInt($(_level2).attr('parnotconformityrule_value'));
                $(_level1).removeAttr('havereaudit','false');
                $(_level2).attr('havereaudit', 'true');
                $('.Resultlevel2:last').attr('reauditlevel','2').attr('havereaudit', 'true').attr('havecorrectiveaction', 'true');
                $('.level02Result:last').attr('reauditlevel','2');
              /*  $(_level2).attr('havereaudit','true');
                $(_level1).attr('havereaudit','false');*/
                alertlevel = 2;
            }

    //alert('alerta Núm NC : L1 = ' + controleDeAlerta[0] + ' e L2 = ' + controleDeAlerta[1]);
    if (controleDeAlerta == 1) {
        if (level == 1) {
            $(_level1).attr('havereaudit', 'true');
            var mensagem = getResource("nc_target_exceed") + " (" + ncL1 + ") " + getResource("nc_of") + " (" + maxL1 + ") " + getResource("allowed");
            openMessageModal(getResource("warning") + " " + getResource("fired"), mensagem, 'alerta');
            ///ZerarContadorDefeito();
        } else {
            var mensagem = getResource("nc_target_exceed") + " (" + ncL2 + ") " + getResource("nc_of") + " (" + maxL2 + ") " + getResource("allowed");
            openMessageModal(getResource("warning") + " " + getResource("fired"), mensagem, 'alerta');
            //ZerarContadorDefeito();
        }
        ZerarContadorDefeito();
        return controleDeAlerta;
    }
    ZerarContadorDefeito();
    return controleDeAlerta;
}
//tira o havereaudit dos leveis 2
function atribuiReauditLevel1(){
    var level2ComReaudit = $('.level2Group[level01id=' + $('.level1.selected').attr('id') + '] .level2');
    level2ComReaudit.each(function(e){
    	$(this).attr('reauditlevel','1');
    });
}
function zeraLevel2(){
    var level2ComReaudit = $('.level2');
    level2ComReaudit.each(function(e){
    	$(this).attr('reauditlevel','0');
    });
}
function AlertaPercNC(level, nc, volume) {
    var controleDeAlerta = 0;
    var max = 0, controle = false;

    if (level == 1)
        if (((nc / volume) * 100) > parseInt($(_level1).attr('alertanivel2'))) {
            controleDeAlerta = 1;
            ncL1 = nc;
            maxL1 = parseInt($(_level1).attr('alertanivel2'));
            if (isEUA)
                alertlevel = 'level 1';
            else
                alertlevel = 'Indicador';
        }
    if (level == 2)
        if ($(_level2).length > 0)
            if (((nc / volume) * 100) > parseInt($(_level2).attr('parnotconformityrule_value'))) {
                controleDeAlerta = 1;
                ncL2 = nc;
                maxL2 = parseInt($(_level2).attr('parnotconformityrule_value'));
                if (alertlevel == null) {
                    if (isEUA)
                        alertlevel = 'level 2';
                    else
                        alertlevel = 'Monitoramento';
                }
            }
    //alert('alerta Perc NC : L1 = ' + controleDeAlerta[0] + ' e L2 = ' + controleDeAlerta[1]);
    if (controleDeAlerta == 1) {
        if (level == 1) {
            $(_level1).attr('havereaudit', 'true');
            var mensagem = getResource("nc_target_exceed") + " (" + ncL1 + ") " + getResource("nc_of") + " (" + maxL1 + ") " + getResource("allowed");
            openMessageModal(getResource("warning") + " " + getResource("fired"), mensagem, 'alerta');
            //ZerarContadorDefeito();
        } else {
            $(_level2).attr('havereaudit', 'true');
            var mensagem = getResource("nc_target_exceed") + " (" + ncL2 + ") " + getResource("nc_of") + " (" + maxL2 + ") " + getResource("allowed");
            openMessageModal(getResource("warning") + " " + getResource("fired"), mensagem, 'alerta');
            //ZerarContadorDefeito();
        }
        ZerarContadorDefeito();
        return controleDeAlerta;
    }
    ZerarContadorDefeito();
    return controleDeAlerta;
}

function AlertaJBSNC(level, nc, parLevel1) {
    var controleDeAlerta = [];
    controleDeAlerta[0] = 0;
    controleDeAlerta[1] = 0;

    //preenche Método com regras JBS

    //alert('alerta JBS NC : L1 = ' + controleDeAlerta[0] + ' e L2 = ' + controleDeAlerta[1]);
    //ZerarContadorDefeito();
    return controleDeAlerta;
}

function alertaCFF() {
    var menor, level2, controleAlertaCFF = false;
    var collapseList = $('.level2:visible');
    collapseList.each(function (e) {
        if (parseInt($(this).attr('parnotconformityrule_id')) > 0) {
            if (e == 0) {
                menor = parseInt($(this).attr('parnotconformityrule_value'));
                level2 = $(this);
            } else if (menor > $(this).attr('parnotconformityrule_value')) {
                menor = parseInt($(this).attr('parnotconformityrule_value'));
                level2 = $(this);
            }
        } else if (isNaN(menor))
            menor = 0;
    });

    if (tdefAv.text() > parseInt($(_level1).attr('alertanivel2'))) {
        controleAlertaCFF = true;
        if ($(_level1).attr('reaudit') == 'true') {
            $(_level1).attr('haveReaudit', 'true');
            $(_level1).attr('reauditlevel', '1');
        }
    }
    if (controleAlertaCFF == false) {
        if (menor > 0)
            if ((tdef.text() > menor)) {
                controleAlertaCFF = true;
                if ($(_level1).attr('reaudit') == 'true') {
                    $(_level1).attr('haveReaudit', 'true');
                    $(_level1).attr('reauditlevel', '1');
                }
            }
    }

    if (controleAlertaCFF) {
        mensagem = getResource("nc_target_exceed") + getResource("allowed");
        openMessageModal(getResource("warning") + " " + getResource("fired"), mensagem, 'alerta');
        $('.Resultlevel2:last').attr('reauditlevel','1').attr('havereaudit','true');
        $('.level02Result:last').attr('reauditlevel','1').attr('havereaudit','true');
    }
    ZerarContadorDefeito();
}

/**
 * Método que controla alerta de todos os tipos no sgq
 * 
 * Passo 1 : Verifico se tem defeito nos leveis 3 selecionados
 * Passo 2a : Aciono o método que consolida os meus defeitos atuais no level 1
 * Passo 2b : Aciono o método que consolida os meus defeitos atuais no level 2
 * 
 * Passo 3 : Crio a var local defeitos[] que será setada no momento  que definir o tipo de consolidação
 * ( na posição 0 será o Valor de defeitos no level 1 e na posição 1 será o valor de defeitos no level 2) Valor padrão = [{0},{0}]
 * Passo 4 : Crio a var local avaliações[] que será setada no momento  que definir o tipo de consolidação.
 * ( na posição 0 será o Valor de avaliações no level 1 e na posição 1 será o valor de avaliações no level 2) Valor padrão = [{0},{0}]
 * Passo 5 : Crio a var local volume[] que será setada no momento que escolhermos o indicador.
 * ( na posição 0 será o Valor de volume no level 1 e na posição 1 será o valor de volume no level 2) Valor padrão = [{0},{0}]
 * 
 * Passo 6 : Crio o case para o tipo de consolidação
 * Passo 6a : Caso tipo de consolidação = 1 (soma simples) então : 
 * var defeitos[0] = weidefectsL1, defeitos[1] = weidefectsL2
 * var avaliações[0] = weievaluationL1, avaliações[1] = weievaluationL2
 * Passo 6b : Caso tipo de consolidação = 2 (soma de tarefas únicas(BEA)) então : 
 * var defeitos[0] = weidefectsL1, defeitos[1] = weidefectsL2
 * var avaliações[0] = weievaluationL1, avaliações[1] = weievaluationL2
 * Passo 6c : Caso tipo de consolidação = 3 (soma por peças) então : 
 * var defeitos[0] = defectsResultL1, defeitos[1] = defectsResultL2
 * var avaliações[0] = evaluatedResultL1, avaliações[1] = evaluatedResultL2
 * Passo 6d : Caso tipo de consolidação = 4 (por amostra) então : 
 * var defeitos[0] = defectsResultL1, defeitos[1] = defectsResultL2
 * var avaliações[0] = evaluatedResultL1, avaliações[1] = evaluatedResultL2
 * Passo 6e : Caso tipo de consolidação = 5 (soma de defeitos) então : 
 * Definir
 * 
 * Passo 7 : crio o case para Regra de alerta
 * Passo 7a : Caso regraDeAlerta = 1 (JBS) então :
 * var controlaAlertas = alertaJBSNC(var defeitos) : bool;
 * 
 * Passo 7b : Caso regraDeAlerta = 2 (núm de Defeitos) então :
 * var controlaAlertas = alertaNumeroNC(var defeitos) : bool;
 * 
 * Passo 7c : Caso regraDeAlerta = 3 (% de Defeitos) então :
 * var controlaAlertas = alertaPercNC(var defeitos, volume) : bool;
 * 
 * Passo 8 : Controlar e disparar os alertas
 */

function controleDeAlerta(level2id) {

    var evaluateCurrent = $('.level3Group[level2id=' + $('.level2.selected').attr('id') + '] .painelLevel03 .evaluateCurrent').text();
    var sampleCurrent = $('.level3Group[level2id=' + $('.level2.selected').attr('id') + '] .painelLevel03 .sampleCurrent').text();
    var def = 0, lvl2Id = 0, contador = 0;;
    if ($('.level1.selected').attr('hasgrouplevel2') == "true") {
        evaluateCurrent = $('.level3Group[level1idgroup=' + $('.level1.selected').attr('id') + '] .painelLevel03 .evaluateCurrent').text();
        sampleCurrent = $('.level3Group[level1idgroup=' + $('.level1.selected').attr('id') + '] .painelLevel03 .sampleCurrent').text();
    }

    var numeroDeLevel2;

    if($(_level1).attr('hasgrouplevel2') == 'true'){
        numeroDeLevel2 = $('.level01Result.selected').children('.level02Result[level01id=' + $('.level1.selected').attr('id')
        + '][level02id=' + level2id + '][evaluate=' + evaluateCurrent + '][sample=' + sampleCurrent + ']');
    } else {
        numeroDeLevel2 = $('.level01Result.selected').children('.level02Result[level02id=' + $('.level2.selected').attr('id')
        + '][evaluate=' + evaluateCurrent + '][sample=' + sampleCurrent + ']');
    }
    
    //***Para bem estar animal ***////
    //****importante colocar outras opcoes aqui ///****
    if ($('.level1.BEA').hasClass('selected')) {
        numeroDeLevel2 = $('.level01Result.selected').children('.level02Result:last');

    }

    if (parseInt($(_level1).attr('parconsolidationtype_id')) == 6) {
        numeroDeLevel2.each(function (e) {
            var level2Resultado = $(this);
            var numeroDeLevel3 = level2Resultado.children('.level03Result');

            //Soma dos atributos do Level3
            numeroDeLevel3.each(function (e) {
                var level3 = $(this);
                if (parseInt(level3.attr('defects')) > 0)
                    contador++;
            });
        });
    }
    
    numeroDeLevel2.each(function (e) {
        var level2Resultado = $(this);
        var numeroDeLevel3 = level2Resultado.children('.level03Result');
        //Soma dos atributos do Level3
        var booleano = false;
        numeroDeLevel3.each(function (e) {
            var level3 = $(this);

            if (booleano == false)
                if (parseInt(level3.attr('defects')) > 0) {
                    def = parseInt(level3.attr('defects'));
                    lvl2Id = level2Resultado.attr('level02id');
                    booleano = true;
                    return controleDeAlertaSgq(def, lvl2Id, contador);
                }
        });

    });

}

function controleDeAlertaSgq(defeitosAtuais, level2Id, contador) {

    //Passo 1 : Verifico se tem defeito nos leveis 3 selecionados
    if (defeitosAtuais > 0) {

        //Passo 2a : Aciono o método que consolida os meus defeitos atuais no level 1
        var resultadoL1 = emitirAlerta($(_level1).attr('id'), $('.App').attr('period'), $('.App').attr('shift'));
        var resultadoL2;
        //Passo 2b : Aciono o método que consolida os meus defeitos atuais no level 2
        $.grep(resultadoL1.DefeitosLevel2, function (level2, counter) {
            if (level2.level2id == level2Id);
            resultadoL2 = level2;
        });

        //Passo 3: Crio a var local defeitos[] que será setada no momento  que definir o tipo de consolidação
        // ( na posição 0 será o Valor de defeitos no level 1 e na posição 1 será o valor de defeitos no level 2) Valor padrão = [{0},{0}]

        //var defeitosLevel1 = resultadoL1.defectsl2;
        //var defeitosLevel2 = resultadoL2.defectsl2;

        //if(isNaN(defeitosLevel2))

        var defeitos = [];
        defeitos[0] = 0;
        defeitos[1] = 0;

        // Passo 4 : Crio a var local avaliações[] que será setada no momento  que definir o tipo de consolidação.
        // ( na posição 0 será o Valor de avaliações no level 1 e na posição 1 será o valor de avaliações no level 2) Valor padrão = [{0},{0}]

        var avaliacoes = [];
        avaliacoes[0] = 0;
        avaliacoes[1] = 0;

        //Passo 5 : Crio a var local volume que será setada no momento que escolhermos o indicador. Valor padrão = [{0},{0}]

        var volume = $(_level1).attr('volumealertaindicador');

        // listaDefeitosL1.forEach(
        //     function (o) {
        //         defeitosLevel1 += o;
        //     }
        // );

        // listaDefeitosL2.forEach(
        //     function (o) {
        //         defeitosLevel2 += o;
        //     }
        // );

        var controlaAlertas;

        //Passo 6 : Crio o case para o tipo de consolidação 
        var tipoDeConsolidacao = parseInt($(_level1).attr('parconsolidationtype_id'));
        switch (tipoDeConsolidacao) {
            /*Passo 6a : Caso tipo de consolidação = 1 (soma simples) então : 
            * var defeitos[0] = weidefectsL1, defeitos[1] = weidefectsL2
            * var avaliações[0] = weievaluationL1, avaliações[1] = weievaluationL2*/
            case 1:
                var seila = defeitos[1] = parseInt(resultadoL2.weidefectsl2);
                if (seila > parseInt($(_level2).attr('totalavaliacoes')))
                    seila = parseInt($(_level2).attr('totalavaliacoes'));
                defeitos[0] = parseInt(resultadoL1.weidefectsl2), defeitos[1] = seila;
                avaliacoes[0] = resultadoL1.weievaluationl2, avaliacoes[1] = resultadoL2.weievaluationl2;
                break;
            /* Passo 6b : Caso tipo de consolidação = 2 (soma de tarefas únicas(BEA)) então : 
            * var defeitos[0] = weidefectsL1, defeitos[1] = weidefectsL2
            * var avaliações[0] = weievaluationL1, avaliações[1] = weievaluationL2*/
            case 2:
                defeitos[0] = parseInt(resultadoL1.weidefectsl2), defeitos[1] = parseInt(resultadoL2.weidefectsl2);
                avaliacoes[0] = resultadoL1.weievaluationl2, avaliacoes[1] = resultadoL2.weievaluationl2;
                break;
            /* Passo 6c : Caso tipo de consolidação = 3 (soma por peças) então : 
            * var defeitos[0] = defectsResultL1, defeitos[1] = defectsResultL2
            * var avaliações[0] = evaluatedResultL1, avaliações[1] = evaluatedResultL2*/
            case 3:
                defeitos[0] = parseInt(resultadoL1.defectsresultl2), defeitos[1] = parseInt(resultadoL2.defectsresultl2);
                avaliacoes[0] = resultadoL1.totalavaliado, avaliacoes[1] = resultadoL2.evaluatedresultl2;
                break;
            /* Passo 6d : Caso tipo de consolidação = 4 (por amostra) então : 
            * var defeitos[0] = defectsResultL1, defeitos[1] = defectsResultL2
            * var avaliações[0] = evaluatedResultL1, avaliações[1] = evaluatedResultL2*/
            case 4:
                alertaCFF();
                return;
            // defeitos[0] = parseInt(resultadoL1.defectsresultl2), defeitos[1] = parseInt(resultadoL2.defectsresultl2);
            // avaliacoes[0] = resultadoL1.totalavaliado, avaliacoes[1] = resultadoL2.evaluatedresultl2;
            // break;
            /* Passo 6e : Caso tipo de consolidação = 5 (soma de defeitos) então : 
            * Definir*/
            case 5:
                defeitos[0] = parseInt(resultadoL1.weidefectsl2), defeitos[1] = parseInt(resultadoL2.weidefectsl2);
                avaliacoes[0] = resultadoL1.weievaluationl2, avaliacoes[1] = resultadoL2.weievaluationl2;
                break;
            case 6:
                defeitos[0] = resultadoL1.totallevel3withdefectsl2, defeitos[1] = contador;
                avaliacoes[0] = resultadoL1.totalavaliado, avaliacoes[1] = resultadoL2.evaluatedresultl2;
                break;
            default:
                break;
        }

        var regraDeAlerta = [];
        var ar = [];
        ar[0] = $(_level1).attr('alertanivel3');
        ar[1] = $(_level1).attr('alertanivel2');
        regraDeAlerta[0] = ar;
        if (isNaN($(_level2).attr('parnotconformityrule_id')) == false) {
            arlevel2 = [];
            arlevel2[0] = $(_level2).attr('parnotconformityrule_id');
            arlevel2[1] = $(_level2).attr('parnotconformityrule_value');
            regraDeAlerta[1] = arlevel2;
        }

        var tipo1 = null, tipo2 = null;
        //Passo 7 : crio o case para Regra de alerta
        switch (regraDeAlerta[0][0]) {
            case 'a0':
                tipo1 = 4;
                break;
            case 'a1':
                tipo1 = 1;
                break;
            case 'a2':
                tipo1 = 2;
                break;
            case 'a3':
                tipo1 = 3;
                break;
            default:
                break;
        }
        if (regraDeAlerta.length > 1) {
            var caso = parseInt(regraDeAlerta[1][0]);
            switch (caso) {
                case 1:
                    tipo2 = 1;
                    break;
                case 2:
                    tipo2 = 2;
                    break;
                case 3:
                    tipo2 = 3;
                    break;
                default: break;
            }
        }
        var retorno = [];
        retorno[0] = chamaAlertaL1(tipo1, defeitos, volume, avaliacoes);
        if (retorno[0] == 0) {
            retorno[1] = chamaAlertaL2(tipo2, defeitos, volume, avaliacoes);
        }
        return retorno;
    }
}

function chamaAlertaL1(tipo1, defeitos, volume, avaliacoes) {

    switch (tipo1) {
        case 1:
            return AlertaJBSNC(1, defeitos[0]);
        case 2:
            return AlertaNumeroNC(1, defeitos[0]);
        case 3:
            return AlertaPercNC(1, defeitos[0], volume);
        case 4:
            return AlertaNumeroNC(1, defeitos[0]);
        default: return null;
    }
}

function chamaAlertaL2(tipo2, defeitos, volume, avaliacoes) {
    switch (tipo2) {
        case 1:
            return AlertaJBSNC(2, defeitos[1]);
        case 2:
            return AlertaNumeroNC(2, defeitos[1]);
        case 3:
            return AlertaPercNC(2, defeitos[1], volume);
        default: return null;
    }
}