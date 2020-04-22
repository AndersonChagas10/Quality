function RedistributeWeight() {

    var nivelMonitoramento = {};
    var posicaoMonitoramento = 0;
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
        var linha = $(o);
        var isNA = $(linha).attr('data-conforme-na') == "";
        var peso = $(linha).attr('data-peso');
        var indicador = $(linha).attr('data-level1');
        var monitoramento = $(linha).attr('data-level2');

        if ((posicaoMonitoramento > 0 && (nivelMonitoramento[posicaoMonitoramento].indicador != indicador || nivelMonitoramento[posicaoMonitoramento].monitoramento != monitoramento))
            || (posicaoMonitoramento == 0 && $(nivelMonitoramento[0]).length == 1 && (nivelMonitoramento[0].indicador != indicador || nivelMonitoramento[0].monitoramento != monitoramento))) {
            posicaoMonitoramento++;
            zerarValores();
        }

        qtdTarefas++;
        if (isNA) {
            qtdNA++;
            somaPesoNA += parseFloat(peso);
        } else {
            somaPesoAv += parseFloat(peso);
        }
        nivelMonitoramento[posicaoMonitoramento] = { indicador, monitoramento, qtdTarefas, qtdNA, somaPesoNA, somaPesoAv };
    });

    var nivelIndicador = {};
    var posicaoIndicador = 0;
    var redistribuirNivelIndicador = false;
    zerarValores()
    for (var i in nivelMonitoramento) {

        somaPesoAv += nivelMonitoramento[i].somaPesoAv;
        var indicador = nivelMonitoramento[i].indicador;

        if (($(posicaoIndicador == 0 && nivelIndicador[posicaoIndicador]).length == 1 && nivelIndicador[posicaoIndicador].indicador != indicador)
            || (posicaoIndicador > 0 && nivelIndicador[posicaoIndicador].indicador != indicador)) {
            posicaoIndicador++;
            redistribuirNivelIndicador = false;
            zerarValores()
        }
        if (nivelMonitoramento[i].qtdNA == nivelMonitoramento[i].qtdTarefas) {
            redistribuirNivelIndicador = true;
            somaPesoNA += nivelMonitoramento[i].somaPesoNA;
        }

        qtdTarefas += nivelMonitoramento[i].qtdTarefas;
        qtdNA += nivelMonitoramento[i].qtdNA;
        nivelIndicador[posicaoIndicador] = { indicador, qtdTarefas, qtdNA, somaPesoNA, somaPesoAv };

        if ($(nivelIndicador[posicaoIndicador]).length > 0) {
            nivelIndicador[posicaoIndicador].somaPesoAv = somaPesoAv;
            nivelIndicador[posicaoIndicador].redistribuirNivelIndicador = redistribuirNivelIndicador;
        }
    }

    var nivelSecao = {};
    var redistribuirTudo = false;
    zerarValores()
    for (var i in nivelIndicador) {
        if ($(nivelIndicador[i]).length > 0) {
            if (nivelIndicador[i].qtdNA == nivelIndicador[i].qtdTarefas) {
                redistribuirTudo = true;
                somaPesoNA += nivelIndicador[i].somaPesoNA;
            }
            somaPesoAv += nivelIndicador[i].somaPesoAv;
            nivelSecao[0] = { somaPesoNA, somaPesoAv, redistribuirTudo };
        }
    }

    var pesoTotalSecao = 0;
    var pesoTotalIndicador = {};
    for (var i in nivelMonitoramento) {
        pesoTotalSecao += nivelMonitoramento[i].somaPesoAv + nivelMonitoramento[i].somaPesoNA;
        if (pesoTotalIndicador[nivelMonitoramento[i].indicador] > 0) {
            pesoTotalIndicador[nivelMonitoramento[i].indicador] += nivelMonitoramento[i].somaPesoAv + nivelMonitoramento[i].somaPesoNA;
        } else {
            pesoTotalIndicador[nivelMonitoramento[i].indicador] = nivelMonitoramento[i].somaPesoAv + nivelMonitoramento[i].somaPesoNA;
        }
    }

    function redistribuir(nivel) {
        var nivelSelecionado;
        for (var i in nivel) {
            if (nivel == nivelMonitoramento) {
                nivelSelecionado = '[data-level2=' + nivelMonitoramento[i].monitoramento + ']';
            }
            else if (nivel == nivelIndicador) {
                if ($(nivelIndicador[i]).length > 0 && nivelIndicador[i].redistribuirNivelIndicador == true) {
                    nivelSelecionado = '[data-level1=' + nivelIndicador[i].indicador + ']';
                }
            }
            else if (nivel == nivelSecao) {
                if ($(nivelSecao[0]).length > 0 && nivelSecao[0].redistribuirTudo == true) {
                    nivelSelecionado = 'form[data-form-coleta] div[data-linha-coleta]';
                } else {
                    break;
                }
            }
            for (var x in nivelSelecionado) {

                var linha = $(nivelSelecionado)[x];
                var isNA = $(linha).attr('data-conforme-na') == "";
                var peso = parseFloat($(linha).attr('data-peso'));

                if (!isNA && nivel[i].somaPesoNA > 0) {
                    if (nivel == nivelMonitoramento && nivel[i].indicador == $(linha).attr('data-level1')) {
                        peso += (peso * nivel[i].somaPesoNA) / nivel[i].somaPesoAv;
                    }
                    else if (nivel == nivelIndicador) {
                        peso += (peso * nivel[i].somaPesoNA) / (pesoTotalIndicador[nivel[i].indicador] - nivel[i].somaPesoNA);
                    }
                    else if (nivel == nivelSecao) {
                        peso += (peso * nivel[i].somaPesoNA) / (pesoTotalSecao - nivel[i].somaPesoNA);
                    }

                    $(linha).attr('data-peso', peso);
                }
            }
        }
    }

    redistribuir(nivelMonitoramento);
    redistribuir(nivelIndicador);
    redistribuir(nivelSecao);
}