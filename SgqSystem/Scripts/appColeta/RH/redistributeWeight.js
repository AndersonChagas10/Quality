function RedistributeWeight() {

    var matriz = {};
    var posicaoMatriz = 0;
    var somaPesoNA = 0;
    var qtdTarefas = 0;
    var qtdNA = 0;
    var somaPesoAv = 0;

    function zerarValores() {
        qtdTarefas = 0;
        qtdNA = 0;
        somaPesoNA = 0;
        somaPesoAv = 0;
        return;
    }

    $('form[data-form-coleta] div[data-linha-coleta]').each(function (i, o) {
        var data = $(o);
        var isNA = $(data).attr('data-conforme-na') == "";
        var peso = $(data).attr('data-peso');
        var indicador = $(data).attr('data-level1');
        var monitoramento = $(data).attr('data-level2');

        if ((posicaoMatriz > 0 && (matriz[posicaoMatriz].indicador != indicador || matriz[posicaoMatriz].monitoramento != monitoramento))
            || (posicaoMatriz == 0 && $(matriz[0]).length == 1 && (matriz[0].indicador != indicador || matriz[0].monitoramento != monitoramento))) {
            posicaoMatriz++;
            zerarValores();
        }

        qtdTarefas++;
        if (isNA) {
            qtdNA++;
            somaPesoNA += parseFloat(peso);
        } else {
            somaPesoAv += parseFloat(peso);
        }
        matriz[posicaoMatriz] = { indicador, monitoramento, qtdTarefas, qtdNA, somaPesoNA, somaPesoAv };
    });

    var redistribuirIndicador = {};
    var posicaoIndicador = 0;
    zerarValores()
    for (var i in matriz) {
        somaPesoAv += matriz[i].somaPesoAv;
        var indicador = matriz[i].indicador;
        var redistribuirNivelIndicador = false;
        if (matriz[i].qtdNA == matriz[i].qtdTarefas) {
            redistribuirNivelIndicador = true;
        }
        if (($(posicaoIndicador == 0 && redistribuirIndicador[posicaoIndicador]).length == 1 && redistribuirIndicador[posicaoIndicador].indicador != indicador)
            || (posicaoIndicador > 0 && redistribuirIndicador[posicaoIndicador].indicador != indicador)) {
            posicaoIndicador++;
            zerarValores()
        }
        qtdTarefas += matriz[i].qtdTarefas;
        qtdNA += matriz[i].qtdNA;
        somaPesoNA += matriz[i].somaPesoNA;
        redistribuirIndicador[posicaoIndicador] = { indicador, qtdTarefas, qtdNA, somaPesoNA, somaPesoAv };

        if ($(redistribuirIndicador[posicaoIndicador]).length > 0) {
            redistribuirIndicador[posicaoIndicador].somaPesoAv = somaPesoAv;
            redistribuirIndicador[posicaoIndicador].redistribuirNivelIndicador = redistribuirNivelIndicador;
        }
    }

    var redistribuirCC = {};
    zerarValores()
    for (var i in redistribuirIndicador) {
        if ($(redistribuirIndicador[i]).length > 0) {
            var redistribuirTudo = false;
            if (redistribuirIndicador[i].qtdNA == redistribuirIndicador[i].qtdTarefas) {
                redistribuirTudo = true;
                somaPesoNA += redistribuirIndicador[i].somaPesoNA;
            }
            somaPesoAv += redistribuirIndicador[i].somaPesoAv;
            redistribuirCC[0] = { somaPesoNA, somaPesoAv, redistribuirTudo };
        }
    }

    var pesoTotal = 0;
    var pesoTotalIndicador = {};
    for (var i in matriz) {
        pesoTotal += matriz[i].somaPesoAv + matriz[i].somaPesoNA;
        if (pesoTotalIndicador[matriz[i].indicador] > 0) {
            pesoTotalIndicador[matriz[i].indicador] += matriz[i].somaPesoAv + matriz[i].somaPesoNA;
        } else {
            pesoTotalIndicador[matriz[i].indicador] = matriz[i].somaPesoAv + matriz[i].somaPesoNA;
        }
    }

    function redistribuir(retorno) {
        var level;
        for (var i in retorno) {
            if (retorno == matriz) {
                level = '[data-level2=' + matriz[i].monitoramento + ']';
            }
            else if (retorno == redistribuirIndicador) {
                if ($(redistribuirIndicador[i]).length > 0 && redistribuirIndicador[i].redistribuirNivelIndicador == true) {
                    level = '[data-level1=' + redistribuirIndicador[i].indicador + ']';
                } else {
                    break;
                }
            }
            else if (retorno == redistribuirCC) {
                if ($(redistribuirCC[0]).length > 0 && redistribuirCC[0].redistribuirTudo == true) {
                    level = 'form[data-form-coleta] div[data-linha-coleta]';
                } else {
                    break;
                }
            }
            for (var x in level) {
                var data = $(level)[x];
                var isNA = $(data).attr('data-conforme-na') == "";
                var peso = parseFloat($(data).attr('data-peso'));
                if (!isNA && retorno[i].somaPesoNA > 0) {
                    if (retorno == matriz) {
                        peso += (peso * retorno[i].somaPesoNA) / retorno[i].somaPesoAv;
                    }
                    else if (retorno == redistribuirIndicador) {
                        peso += (peso * retorno[i].somaPesoNA) / (pesoTotalIndicador[retorno[i].indicador] - retorno[i].somaPesoNA);
                    }
                    else if (retorno == redistribuirCC) {
                        peso += (peso * retorno[i].somaPesoNA) / (pesoTotal - retorno[i].somaPesoNA);
                    }

                    $(data).attr('data-peso', peso);
                }
            }
        }
    }

    redistribuir(matriz);
    redistribuir(redistribuirIndicador);
    redistribuir(redistribuirCC);
}