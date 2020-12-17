$(document).ready(function () {
    $('#visaoGeral').click();
    graficosMocados();
});

function graficosMocados() {


    Highcharts.chart('container2', {
        title: {
            text: 'Auditorias por Grupo de Empresa'
        },
        xAxis: {
            categories: ['Apples', 'Oranges', 'Pears', 'Bananas', 'Plums']
        },
        labels: {
            items: [{
                html: 'Total fruit consumption',
            }]
        },
        series: [{
            type: 'column',
            name: 'Jane',
            data: [3, 2, 1, 3, 4]
        }, {
            type: 'column',
            name: 'John',
            data: [2, 3, 5, 7, 6]
        }, {
            type: 'spline',
            name: 'Average',
            data: [3, 2.67, 3, 6.33, 3.33],
            marker: {
                lineWidth: 2,
                lineColor: Highcharts.getOptions().colors[3],
                fillColor: 'white'
            }
        }]
    });

    Highcharts.chart('container3', {
        title: {
            text: 'Auditorias por Regional'
        },
        xAxis: {
            categories: ['Apples', 'Oranges', 'Pears', 'Bananas', 'Plums']
        },
        labels: {
            items: [{
                html: 'Total fruit consumption',
            }]
        },
        series: [{
            type: 'column',
            name: 'Jane',
            data: [3, 2, 1, 3, 4]
        }, {
            type: 'column',
            name: 'John',
            data: [2, 3, 5, 7, 6]
        }, {
            type: 'spline',
            name: 'Average',
            data: [3, 2.67, 3, 6.33, 3.33],
            marker: {
                lineWidth: 2,
                lineColor: Highcharts.getOptions().colors[3],
                fillColor: 'white'
            }
        }]
    });

    Highcharts.chart('container4', {
        title: {
            text: 'Auditorias por Unidade'
        },
        xAxis: {
            categories: ['Apples', 'Oranges', 'Pears', 'Bananas', 'Plums']
        },
        labels: {
            items: [{
                html: 'Total fruit consumption',
            }]
        },
        series: [{
            type: 'column',
            name: 'Jane',
            data: [3, 2, 1, 3, 4]
        }, {
            type: 'column',
            name: 'John',
            data: [2, 3, 5, 7, 6]
        }, {
            type: 'spline',
            name: 'Average',
            data: [3, 2.67, 3, 6.33, 3.33],
            marker: {
                lineWidth: 2,
                lineColor: Highcharts.getOptions().colors[3],
                fillColor: 'white'
            }
        }]
    });

    Highcharts.chart('container5', {
        title: {
            text: 'IPS - Indice de Prática Segura'
        },
        xAxis: {
            categories: ['Apples', 'Oranges', 'Pears', 'Bananas', 'Plums']
        },
        labels: {
            items: [{
                html: 'Total fruit consumption',
            }]
        },
        series: [{
            type: 'column',
            name: 'Jane',
            data: [3, 2, 1, 3, 4]
        }, {
            type: 'column',
            name: 'John',
            data: [2, 3, 5, 7, 6]
        }, {
            type: 'spline',
            name: 'Average',
            data: [3, 2.67, 3, 6.33, 3.33],
            marker: {
                lineWidth: 2,
                lineColor: Highcharts.getOptions().colors[3],
                fillColor: 'white'
            }
        }]
    });

    Highcharts.chart('container6', {
        chart: {
            type: 'line'
        },
        title: {
            text: 'Evolução de Nº Desvios por Categoria '
        },
        xAxis: {
            categories: ['Apples', 'Bananas', 'Oranges']
        },
        yAxis: {
            title: {
                text: 'Fruit eaten'
            }
        },
        series: [{
            name: 'Jane',
            data: [1, 0, 4]
        }, {
            name: 'John',
            data: [5, 7, 3]
        }]
    });
}

function agrupaPor(array, propriedade) {

    var arrayAgrupado = [];
    var arrayAgrupadoObjetos = [];

    for (var i = 0; i < array.length; i++) {
        var item = array[i];
        if (!arrayAgrupado[item[propriedade]]) {
            arrayAgrupado[item[propriedade]] = item;
        }
    }

    var keys = Object.keys(arrayAgrupado);
    for (var i = 0; i < keys.length; i++) {
        var item = keys[i];
        arrayAgrupadoObjetos.push(arrayAgrupado[item]);
    }

    return arrayAgrupadoObjetos;
}

function montaCardsVisaoUnidade(listaAuditoria) {

    var totalAuditorias = agrupaPor(listaAuditoria, 'CollectionL2_Id');

    $("#divVisaoUnidade #labelTotalAuditorias").text(listaAuditoria.filter((o, i) => o.total).length);

    var totalAuditores = listaAuditoria.filter((o, i) => o['Auditor Cabecalho']).length;
    $("#divVisaoUnidade #labelTotalAuditores").text(totalAuditores);

    var totalTarefasConforme = listaAuditoria.filter((o, i) => o['C'] > '0').length;
    $("#divVisaoUnidade #labelTotalConforme").text(totalTarefasConforme);

    var totalTarefasNaoConforme = listaAuditoria.filter((o, i) => o['NC'] > '0').length;
    $("#divVisaoUnidade #labelTotalNaoConforme").text(totalTarefasNaoConforme);

    var somaObservados = 0;
    listaAuditoria.map(function (o, i) {
        if (o['pessoas observadas'] != null) {
            somaObservados += parseInt(o['pessoas observadas']);
        }
    });
    $("#divVisaoUnidade #labelTotalAuditoresObservados").text(somaObservados);

    var ips = (1 - (totalTarefasNaoConforme / somaObservados)) * 100;
    $("#divVisaoUnidade #labelIps").text(parseInt(ips.toFixed(2)) + "%");
}

function montaCardsVisaoGeral(listaAuditoria) {

    var totalAuditorias = agrupaPor(listaAuditoria, 'CollectionL2_Id');

    $("#divVisaoGeral #labelTotalAuditorias").text(listaAuditoria.filter((o, i) => o.total).length);

    var totalAuditores = listaAuditoria.filter((o, i) => o['Auditor Cabecalho']).length;
    $("#divVisaoGeral #labelTotalAuditores").text(totalAuditores);

    var totalTarefasConforme = listaAuditoria.filter((o, i) => o['C'] > '0').length;
    $("#divVisaoGeral #labelTotalConforme").text(totalTarefasConforme);

    var totalTarefasNaoConforme = listaAuditoria.filter((o, i) => o['NC'] > '0').length;
    $("#divVisaoGeral #labelTotalNaoConforme").text(totalTarefasNaoConforme);

    var somaObservados = 0;
    listaAuditoria.map(function (o, i) {
        if (o['pessoas observadas'] != null) {
            somaObservados += parseInt(o['pessoas observadas']);
        }
    });
    $("#divVisaoGeral #labelTotalAuditoresObservados").text(somaObservados);

    var ips = (1 - (totalTarefasNaoConforme / somaObservados)) * 100;
    $("#divVisaoUnidade #labelIps").text(parseInt(ips.toFixed(2)) + "%");
}

function montaCardsAcompanhamento(listaAuditoria, totalSemanas) {

    var totalAuditorias = 0;
    var totalAuditores = 0;
    var totalConforme = 0;
    var totalNaoConforme = 0;
    listaAuditoria.map(function (o, i) {

        totalAuditorias += parseInt(o.total);

        if (o['Auditor Cabecalho'] != "") {
            totalAuditores++;
        }

        if (o['C'] != "") {
            totalConforme += parseInt(o['C']);
        }
        if (o['NC'] != "") {
            totalNaoConforme += parseInt(o['NC']);
        }

    });

    $("#lblTotalAuditoriasAcompanhamento").text(totalAuditorias);

    $("#lblTotalAuditoresAcompanhamento").text(totalAuditores);

    if (globalTotalRealizado / totalAuditores >= 100)
        $("#lblTotalRealizadoAcompanhamento").text(100 + "%").css('color', 'green');
    else
        $("#lblTotalRealizadoAcompanhamento").text((globalTotalRealizado / totalAuditores).toFixed(2) + "%").css('color', 'red');

    var totalTarefasAvalidas = totalConforme + totalNaoConforme;
    var porcentagemTotalConforme = totalConforme / totalTarefasAvalidas * 100;
    $("#lblTotalConformeAcompanhamento").text(porcentagemTotalConforme.toFixed(2) + "%").css('color', 'green');


    var porcentagemTotalNaoconforme = totalNaoConforme / totalTarefasAvalidas * 100;
    $("#lblTotalNaoConformeAcompanhamento").text(porcentagemTotalNaoconforme.toFixed(2) + "%").css('color', 'red');
}

function montaListaObjGenericosPorcentagem(lista, propriedadeName, propriedadeValue1, propriedadeValue2) {

    var listaFormatada = [];
    var contPropriedadeValue1= 0;
    var contPropriedadeValue2 = 0;

    //lista.forEach(function (o, i) {
    //    if (o['HeaderFieldListL1'][propriedadeName] == propriedadeValue1) {
    //        contPropriedadeValue1++;
    //    } else {
    //        contPropriedadeValue2++;
    //    }
    //});

    lista.forEach(function (o, i) {
        if (o[propriedadeName] == propriedadeValue1) {
            contPropriedadeValue1++;
        } else {
            contPropriedadeValue2++;
        }
    });

    listaFormatada.push({ name: propriedadeValue1, y: (parseInt(contPropriedadeValue1) / lista.length) * 100, color: '#df6357' });
    listaFormatada.push({ name: propriedadeValue2, y: (parseInt(contPropriedadeValue2) / lista.length) * 100, color: '#6ec979' });

    return listaFormatada;
}

function montaListaSeguroInseguro(lista) {

    var listaAvaliacaoAtividade = lista.filter((o, i) => o["Avaliação da Atividade"] != "");

    return montaListaObjGenericosPorcentagem(listaAvaliacaoAtividade, "Avaliação da Atividade", "Insegura", "Segura");
}

function montaListaPessoaAvaliada(lista) {

    var listaFormatada = [];
    var contFuncionario = 0;
    var contTerceiro = 0;

    var listaPessoaAvaliada = lista.filter((o, i) => o["Pessoa avaliada"] != "");

    listaPessoaAvaliada.forEach(function (o, i) {
        if (o["Pessoa avaliada"] == "Funcionário") {
            contFuncionario += parseInt(o["pessoas observadas"]);
        } else {
            contTerceiro += parseInt(o["pessoas observadas"]);
        }
    });

    listaFormatada.push({ name: 'Funcionários', y: contFuncionario, color:'#7f96ec' });
    listaFormatada.push({ name: 'Terceiros', y: contTerceiro, color:'#c9a25c' });

    return listaFormatada;
}

function montaListaTarefaRealizada(lista) {

    var listaFormatada = [];
    var contIndividual = 0;
    var contEquipe = 0;

    var listaTarefaRealizada = lista.filter((o, i) => o["Tipo de Tarefa Realizada"] != "");

    listaTarefaRealizada.forEach(function (o, i) {
        if (o["Tipo de Tarefa Realizada"] == "Individual") {
            contIndividual++;
        } else {
            contEquipe++;
        }
    });

    listaFormatada.push({ name: 'Individual', y: contIndividual, color: '#7f96ec'  });
    listaFormatada.push({ name: 'Grupo/Equipe', y: contEquipe, color: '#c9a25c' });

    return listaFormatada;
}

function montaGraficosPizza(data) {

    Highcharts.chart('containerPie1', {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        title: {
            text: 'Avaliação de Atividade'
        },
        tooltip: {
            pointFormat: `<span style="color:{series.color}">{series.name}:</span> <strong>{point.percentage:.1f}%</strong>`,
            shared: true
        },
        accessibility: {
            point: {
                valueSuffix: '%'
            }
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '<b>{point.name}</b>: {point.percentage:.1f} %'
                }
            }
        },
        series: [{
            name: '% NC',
            colorByPoint: true,
            data: montaListaSeguroInseguro(data)
        }]
    });


    Highcharts.chart('containerPie2', {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        title: {
            text: 'Pessoa Avaliada'
        },
        tooltip: {
            pointFormat: '{series.name}: {point.y} <b></b>'
        },
        accessibility: {
            point: {
                valueSuffix: ''
            }
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '<b>{point.name}: {point.y} </b>'
                }
            }
        },
        series: [{
            data: montaListaPessoaAvaliada(data)
        }]
    });

    Highcharts.chart('containerPie3', {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        title: {
            text: 'Tipo de Tarefa Realizada'
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.y}</b>'
        },
        accessibility: {
            point: {
                valueSuffix: ''
            }
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '<b>{point.name}</b>: {point.y}'
                }
            }
        },
        series: [{
            data:montaListaTarefaRealizada(data) 
        }]
    });
}

function formataListaObj(data, propriedadeAgrupadora, propriedadeAgrupada, listaMonitoramentoNames) {

    var listaFormatada = [];

    for (var i = 0; i < listaMonitoramentoNames.length; i++) {
        var listaObjTarefa = [];
        var valorNCTotal = 0;

        for (var j = 0; j < data.length; j++) {
            var item = data[j];
            if (listaMonitoramentoNames[i] == item[propriedadeAgrupadora]) {

                var valorNC = 0;
                if (item.NC > '0') {
                    valorNC += parseInt(item.NC);
                    valorNCTotal += valorNC;
                    listaObjTarefa.push([{ name: item[propriedadeAgrupada], nc: valorNC, color: '#f8cc9d' }]);
                }
            }
        }

        listaFormatada.push({
            name: listaMonitoramentoNames[i], totalNc: valorNCTotal, color: '#f08513', tarefa: listaObjTarefa
        });
    }

    var listaFinal = [];
    listaFormatada.forEach(function (o, i) {

        listaFinal.push({ name: o.name, y: o.totalNc, color: o.color });

        for (var j = 0; j < o.tarefa.length; j++) {
            listaFinal.push({ name: o.tarefa[j][0].name.substring(0, 10), y: o.tarefa[j][0].nc, color: o.tarefa[j][0].color });
        }

    });

    return listaFinal;
}

function montaGraficos(data) {

    var listaFinal = formataListaObj(data);

    Highcharts.chart('container1', {
        chart: {
            type: 'column'
        },
        title: {
            text: 'Desvios - Total'
        },
        xAxis: {
            categories: listaFinal.map(function (i, o) { return i.name; })
        },
        yAxis: {
            min: 0,
            title: {
                text: 'nc'
            }
        },
        series: [{
            name: 'Total Desvios',
            data: listaFinal
        }]
    });
}

function montaGraficosUnidade(data) {

    var listaAgrupadaMonitoramento = agrupaPor(data, "monitoramento");

    var listaMonitoramentoNames = listaAgrupadaMonitoramento.map(function (o, i) {
        return o.monitoramento;
    });

    var listaFinal = formataListaObj(data, 'monitoramento', 'tarefa', listaMonitoramentoNames);

    var listaUnidades = data.map(function (o, i) {
        return o.Unidade;
    });

    Highcharts.chart('container1Unidade', {
        chart: {
            reflow: true,
            type: 'column'
        },
        title: {
            text: 'Desvios - Total por Unidade'
        },
        subtitle: {
            text: 'Unidade:' + listaUnidades.filter((x, i, a) => a.indexOf(x) == i)
        },
        xAxis: {
            categories: listaFinal.map(function (i, o) { return i.name; })
        },
        yAxis: {
            min: 0,
            title: {
                text: ''
            }
        },
        series: [{
            data: listaFinal,
            showInLegend: false
        }]
    });

    var listaAgrupadaSetor = agrupaPor(data, "CentroCusto");

    var listaSetorNames = listaAgrupadaSetor.map(function (o, i) {
        return o.CentroCusto;
    });

    var conformidade = [];
    var naoConformidade = [];
    var totalColeta = [];

    for (var j = 0; j < listaSetorNames.length; j++) {
        var totalNC = 0;
        var totalC = 0;

        for (var i = 0; i < data.length; i++) {

            if (data[i].CentroCusto == listaSetorNames[j]) {
                if (data[i].C > 0) {
                    totalC++;
                } else if (data[i].NC > 0) {
                    totalNC++;
                }
            }

            if (i == data.length - 1) {
                conformidade.push({ name: listaSetorNames[j], y: totalC, color: '#B5F599' });
                naoConformidade.push({ name: listaSetorNames[j], y: totalNC, color: '#F07573' });
                totalColeta.push({ name: listaSetorNames[j], y: totalNC + totalC, color: '#070D0F' });
            }
        }
    }

    Highcharts.chart('container2Unidade', {
        chart: {
            reflow: true,
            type: 'column'
        },
        title: {
            text: 'Auditorias por Setor'
        },
        xAxis: {
            categories: conformidade.map(function (i, o) { return i.name; })
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Total'
            }
        },
        series: [{
            name: 'Comportamento Seguro',
            data: conformidade,
            color: '#B5F599'
        }, {
                name: 'Total Desvios',
                data: naoConformidade,
                color: '#F07573'
            },
        {
            type: 'spline',
            name: 'total auditorias',
            data: totalColeta,
            color: '#070D0F'
            }]
    });
}

function montaListaSemanas(data) {

    var semanas = [];
    var semanasName = [];
    var colunasRemover = ["Seguro", "Inseguro","total", "C", "NC", "NA", "pessoas observadas","Auditor Cabecalho" ,"GrupoEmpresa", "Regional", "Secao", "GrupoEmpresa", "Unidade", "Auditor", "Indicador", "Monitoramento", "Tarefa", "Conforme", "Cargo", "ValorDescricaoTarefa", "ClusterName", "HeaderFieldListL1", "HeaderFieldListL2", "HeaderFieldListL3"];

    for (var i = 0; i < data.length; i++) {
        for (var j = 0; j < colunasRemover.length; j++) {
            if (data[i].hasOwnProperty(colunasRemover[j])) {
                delete data[i][colunasRemover[j]];
            }
        }
    }

    var semanaKeys = Object.keys(data[0]);
    var lista = [];

    for (var j = 0; j < semanaKeys.length; j++) {
        lista.push({ title: semanaKeys[j], mData: semanaKeys[j] });
    }

    return lista;
}

function montaGraficosDesviosPorSetor(data) {

    var listaUnidades = data.map(function (o, i) {
        return o.Unidade;
    });

    var listaAgrupadaMonitoramento = agrupaPor(data, "monitoramento");

    var listaMonitoramentoNames = listaAgrupadaMonitoramento.map(function (o, i) {
        return o.monitoramento;
    });


    var listaFinalSetor = formataListaObj(data, 'monitoramento', 'tarefa', listaMonitoramentoNames);

    Highcharts.chart('container3Unidade', {
        chart: {
            reflow: true,
            type: 'column'
        },
        title: {
            text: 'Desvios - Total por Setor'
        },
        subtitle: {
            text: 'Unidade:' + listaUnidades.filter((x, i, a) => a.indexOf(x) == i)
        },
        xAxis: {
            categories: listaFinalSetor.map(function (i, o) { return i.name; })
        },
        yAxis: {
            min: 0,
            title: {
                text: 'nc'
            }
        },
        series: [{
            showInLegend: false,
            data: listaFinalSetor
        }]
    });
}

function enviarFiltro(nivelVisao) {
    if (!nivelVisao) {
        $("#pills-tab li").each(function (i, o) {
            if ($(o).hasClass('active'))
                nivelVisao = $(o).children().attr('val');
        });
    }

    $('#message').html('');
    $("#divVisaoGeral").addClass('hidden');
    $("#divVisaoUnidade").addClass('hidden');
    $("#divAcompanhamento").addClass('hidden');

    openLoader('Aguarde...');

    switch (nivelVisao) {
        case '1':

            $('#message').html("<div class='alert alert-warning'><marquee width='60%' direction='left' height='100px'><h3>Tela em construção.</h3></marquee></div>");
            closeLoader();
            return;

            $.ajax({
                url: urlGet,
                type: 'post',
                data: JSON.stringify(objFiltro),
                dataType: "JSON",
                contentType: "APPLICATION/JSON; CHARSET=UTF-8",
                beforeSend: function () {
                }
            }).done(function (data) {

                for (var i = 0; i < data.length; i++) {
                    data[i]["HeaderFieldListL1"] = JSON.parse(data[i]["HeaderFieldListL1"]);
                    data[i]["HeaderFieldListL2"] = JSON.parse(data[i]["HeaderFieldListL2"]);
                    data[i]["HeaderFieldListL3"] = JSON.parse(data[i]["HeaderFieldListL3"]);
                }

                montaCardsVisaoGeral(data);
                montaGraficosPizza(data);
                montaGraficos(data);

                closeLoader();
                $("#divVisaoGeral").removeClass('hidden');
            }).fail(function (jqXHR, textStatus, msg) {
                console.log(msg);
                closeLoader();
                $("#divVisaoGeral").removeClass('hidden');
            });
            break;
        case '2':
            $.ajax({
                url: urlGetUnidade,
                type: 'post',
                data: JSON.stringify(objFiltro),
                dataType: "JSON",
                contentType: "APPLICATION/JSON; CHARSET=UTF-8",
                beforeSend: function () {
                }
            }).done(function (data) {
                
                montaCardsVisaoUnidade(data);
                montaGraficosPizza(data);
                montaGraficosUnidade(data);

                var colunas = [
                    { title: "Grupo de Empresa", mData: "grupoempresa" },
                    { title: "Regional", mData: "regional" },
                    { title: "Unidade", mData: "Unidade" },
                    { title: "Setor", mData: "CentroCusto" },
                    { title: "Auditor Cabecalho", mData: "Auditor Cabecalho" },
                    { title: "Data", mData: "Data" },
                    { title: "Tipo de Tarefa Realizada", mData: "Tipo de Tarefa Realizada" },
                    { title: "pessoas observadas", mData: "pessoas observadas" },
                    { title: "Avaliação da Atividade", mData: "Avaliação da Atividade" },
                    { title: "Descrição do Desvio", mData: "valordescricaotarefa" }
                    
                ];

                var initDatatable = function () {

                    $('#loading').hide();
                    setTimeout(function (e) {
                        var oTable = $('#tblVisaoUnidade').dataTable();
                        if (oTable.length > 0) {
                            oTable.fnAdjustColumnSizing();
                            tableEdit = $('.dataTable').DataTable();
                        }
                    }, 100);
                };

                var table = datatableGRT.Inicializar({
                    idTabela: "tblVisaoUnidade",
                    listaDeDados: data,
                    colunaDosDados: colunas,
                    numeroLinhasNaTabela: 25,
                    aplicarResponsividade: true,
                    tamanhosDoMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "-"]],
                    loadingRecords: true,
                    destroy: true,
                    info: true,
                    initComplete: initDatatable
                });

                closeLoader();
                $("#divVisaoUnidade").removeClass('hidden');
            }).fail(function (jqXHR, textStatus, msg) {
                console.log(msg);
                //preencheRetornoGrafico("Ocorreu um erro ao buscar os dados. Erro: " + msg);
                closeLoader();
                $("#divVisaoUnidade").removeClass('hidden');
            });

            $.ajax({
                url: urlGetUnidadePorSetor,
                type: 'post',
                data: JSON.stringify(objFiltro),
                dataType: "JSON",
                contentType: "APPLICATION/JSON; CHARSET=UTF-8",
                beforeSend: function () {
                }
            }).done(function (retorno) {

                montaGraficosDesviosPorSetor(retorno);

            }).fail(function (jqXHR, textStatus, msg) {
                console.log(msg);
                //preencheRetornoGrafico("Ocorreu um erro ao buscar os dados. Erro: " + msg);
                closeLoader();
                });

            $("#divTableUnidade").removeClass('hidden');

            $('.dataTables_length').addClass('bs-select');
            //fazer a requisição por unidade
            break;
        case '3':

            $.ajax({
                url: urlGetAcompanhamento,
                type: 'post',
                data: JSON.stringify(objFiltro),
                dataType: "JSON",
                contentType: "APPLICATION/JSON; CHARSET=UTF-8",
                beforeSend: function () {
                }
            }).done(function (data) {
                $("#collapseFiltrosReference").collapse('hide');
                if (data != null && data.length > 0) {

                    globalTotalRealizado = 0;
                    let acompanhamentoObj = JSON.parse(JSON.stringify(data));
                    var listaDeSemanas = [];

                    listaDeSemanas = montaListaSemanas(data);

                    var colunas = [
                        { title: "Grupo de Empresa", mData: "GrupoEmpresa" },
                        { title: "Regional", mData: "Regional" },
                        { title: "Unidade", mData: "Unidade" },
                        { title: "Auditor", mData: "Auditor Cabecalho" },
                        { title: "Total", mData: "total" },
                        {
                            title: "% Realizado", mData: null, mRender: function (acompanhamentoObj, type, full) {

                                var meta = 2;
                                var total = 0;

                                for (var i = 0; i < listaDeSemanas.length; i++) {

                                    total = parseInt(total)
                                        + parseInt(acompanhamentoObj[listaDeSemanas[i].title] > meta ? meta : acompanhamentoObj[listaDeSemanas[i].title]);
                                }

                                var porcentagemTotal = total / (meta * listaDeSemanas.length) * 100;

                                return parseInt(porcentagemTotal.toFixed(2)) > 100 ? "100%" : porcentagemTotal.toFixed(2) + "%";
                            }
                        },
                        {
                            title: "% Seguro", mData: null, mRender: function (acompanhamentoObj, type, full) {
                                var porcentagemTotal = acompanhamentoObj.C / acompanhamentoObj.total * 100;

                                return parseInt(porcentagemTotal.toFixed(2)) > 100 ? "100%" : porcentagemTotal.toFixed(2) + "%";
                            }
                        },
                        {
                            title: "% Inseguro", mData: null, mRender: function (acompanhamentoObj, type, full) {
                                var porcentagemTotal = acompanhamentoObj.NC / acompanhamentoObj.total * 100;

                                return parseInt(porcentagemTotal.toFixed(2)) > 100 ? "100%" : porcentagemTotal.toFixed(2) + "%";
                            }
                        }
                    ];
                    
                    listaDeSemanas.filter((o, i) => colunas.splice(4, 0, o));

                    var initDatatable = function () {
                        $('#loading').hide();
                        setTimeout(function (e) {
                            var oTable = $('#tblAcompanhamento').dataTable();
                            if (oTable.length > 0) {
                                oTable.fnAdjustColumnSizing();
                                tableEdit = $('.dataTable').DataTable();
                            }
                        }, 100);
                    };

                    var table = datatableGRT.Inicializar({
                        idTabela: "tblAcompanhamento",
                        listaDeDados: acompanhamentoObj,
                        colunaDosDados: colunas,
                        numeroLinhasNaTabela: 25,
                        aplicarResponsividade: true,
                        tamanhosDoMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "-"]],
                        loadingRecords: true,
                        destroy: true,
                        info: true,
                        initComplete: initDatatable,
                        createdRow: function (row, data, index) {
                            for (var i = 0; i < row.cells.length; i++) {
                                if (i == row.cells.length - 3 && parseFloat(row.cells[i].innerHTML) <= 99) {
                                    $('td:eq(' + i + ')', row).css("background-color", "#e37f7f").css("color", "white").css("font-weight", "bold"); //vermelho
                                    globalTotalRealizado += parseFloat(row.cells[i].innerHTML);
                                } else if (i == row.cells.length - 3 && parseFloat(row.cells[i].innerHTML) == 100) {
                                    $('td:eq(' + i + ')', row).css("background-color", "#abebae").css("color", "black").css("font-weight", "bold"); //verde
                                    globalTotalRealizado += parseFloat(row.cells[i].innerHTML);
                                }

                                //regra para pintar as duas ultimas colunas de meta de porcentagem 30 por 70
                                if (i == row.cells.length - 3) {

                                    var maximo = parseFloat(row.cells[i + 2].innerHTML);
                                    var minimo = parseFloat(row.cells[i + 1].innerHTML);

                                    if (minimo <= 30 && maximo >= 70) {
                                        //verde
                                        $('td:eq(' + (i + 2) + ')', row).css("background-color", "#abebae").css("color", "black").css("font-weight", "bold");
                                        $('td:eq(' + (i + 1) + ')', row).css("background-color", "#abebae").css("color", "black").css("font-weight", "bold");
                                    } else {
                                        //vermelho
                                        $('td:eq(' + (i + 2) + ')', row).css("background-color", "#e37f7f").css("color", "white").css("font-weight", "bold");
                                        $('td:eq(' + (i + 1) + ')', row).css("background-color", "#e37f7f").css("color", "white").css("font-weight", "bold");
                                    }
                                }
                            }
                        }
                    });

                    montaCardsAcompanhamento(acompanhamentoObj, listaDeSemanas.length);
                    $("#divAcompanhamento").removeClass('hidden');
                } else {
                    $("#divAcompanhamento").addClass('hidden');
                    $('#message').html("<div class='alert alert-info'>  Não existem dados no período selecionado.  </div>");
                }
                closeLoader();
            }).fail(function (jqXHR, textStatus, msg) {
                console.log(msg);
                closeLoader();
                $("#divAcompanhamento").removeClass('hidden');
            });

            break;
        default:
            break;
    }
}

$("#visaoGeral, #visaoUnidade, #acompanhamentoAuditoria").on('click', function () {
    //chama o enviar para buscar pelo nivel selecionado
    var nivelVisao = $(this).attr('val');
    enviarFiltro(nivelVisao);
});