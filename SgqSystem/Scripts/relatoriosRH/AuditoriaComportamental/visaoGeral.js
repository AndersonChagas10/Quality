﻿$(document).ready(function () {
   
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

});

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

function montaCards(listaAuditoria) {

    var totalAuditorias = agrupaPor(listaAuditoria, 'CollectionL2_Id');

    $("#labelTotalAuditorias").text(totalAuditorias.length);

    var totalAuditores = totalAuditorias.filter((o, i) => o.HeaderFieldListL1.hasOwnProperty("Auditor") == true).length;
    $("#labelTotalAuditores").text(totalAuditores);

    var totalTarefasConforme = listaAuditoria.filter((o, i) => o.Conforme == "C").length;
    $("#labelTotalConforme").text(totalTarefasConforme);

    var totalTarefasNaoConforme = listaAuditoria.filter((o, i) => o.Conforme == "NC").length;
    $("#labelTotalNaoConforme").text(totalTarefasNaoConforme);

    var totalAuditoresObservados = totalAuditorias.filter((o, i) => o.HeaderFieldListL1.hasOwnProperty("Nº de pessoas observadas") == true);

    var somaObservados = 0;
    totalAuditoresObservados.map(function (o, i) {
        if (o.HeaderFieldListL1 != null) {
            somaObservados += parseInt(o.HeaderFieldListL1["Nº de pessoas observadas"]);
        }
    });
    //var somaObservados = totalAuditoresObservados.reduce(function (a, b) { return parseInt(a.HeaderFieldListL1["Nº de pessoas observadas"]) + parseInt(b.HeaderFieldListL1["Nº de pessoas observadas"]); });

    $("#labelTotalAuditoresObservados").text(somaObservados);

    var ips = (1 - (totalTarefasNaoConforme / somaObservados)) * 100;
    $("#labelIps").text(parseInt(ips.toFixed(2)) + "%");
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

    var totalPlanejado = totalAuditorias * 2 * totalSemanas; //total de coletas * a meta * a quantidade de semanas  = total de coletas q representam 100%
    if (totalAuditorias / totalPlanejado * 100 == 100)
        $("#lblTotalRealizadoAcompanhamento").text(parseInt(totalAuditorias / totalPlanejado * 100) + "%").css('color', 'green');
    else
        $("#lblTotalRealizadoAcompanhamento").text(parseInt(totalAuditorias / totalPlanejado * 100) + "%").css('color', 'red');

    var totalTarefasAvalidas = totalConforme + totalNaoConforme;
    var porcentagemTotalConforme = totalConforme / totalTarefasAvalidas * 100;
    $("#lblTotalConformeAcompanhamento").text(parseInt(porcentagemTotalConforme.toFixed(2)) + "%").css('color', 'green');


    var porcentagemTotalNaoconforme = totalNaoConforme / totalTarefasAvalidas * 100;
    $("#lblTotalNaoConformeAcompanhamento").text(parseInt(porcentagemTotalNaoconforme.toFixed(2)) + "%").css('color', 'red');
}

function montaListaObjGenericosPorcentagem(lista, propriedadeName, propriedadeValue1, propriedadeValue2) {

    var listaFormatada = [];
    var contPropriedadeValue1= 0;
    var contPropriedadeValue2 = 0;

    lista.forEach(function (o, i) {
        if (o['HeaderFieldListL1'][propriedadeName] == propriedadeValue1) {
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

    var listaAvaliacaoAtividade = lista.filter((o, i) => o.HeaderFieldListL1.hasOwnProperty("Avaliação da Atividade") == true);

    return montaListaObjGenericosPorcentagem(listaAvaliacaoAtividade, "Avaliação da Atividade", "Insegura", "Segura");
}

function montaListaPessoaAvaliada(lista) {

    var listaFormatada = [];
    var contFuncionario = 0;
    var contTerceiro = 0;

    var listaPessoaAvaliada = lista.filter((o, i) => o.HeaderFieldListL1.hasOwnProperty("Pessoa avaliada") == true);

    listaPessoaAvaliada.forEach(function (o, i) {
        if (o['HeaderFieldListL1']["Pessoa avaliada"] == "Funcionário") {
            contFuncionario += parseInt(o['HeaderFieldListL1']["Nº de pessoas observadas"]);
        } else {
            contTerceiro += parseInt(o['HeaderFieldListL1']["Nº de pessoas observadas"]);
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

    var listaTarefaRealizada = lista.filter((o, i) => o.HeaderFieldListL1.hasOwnProperty("Tipo de Tarefa Realizada") == true);

    listaTarefaRealizada.forEach(function (o, i) {
        if (o['HeaderFieldListL1']["Tipo de Tarefa Realizada"] == "Individual") {
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

function montaGraficos(data) {

    var listaFormatada = [];

    for (var i = 0; i < data.length; i++) {

        var item = data[i];
        var listaObjTarefa = [];

        var listaAgrupada = data.filter((o, i) => o.Monitoramento == item.Monitoramento);

        if (listaAgrupada != null) {
            var valorNCTotal = 0;
            for (var j = 0; j < listaAgrupada.length; j++) {

                var valorNC = 0;
                if (listaAgrupada[j].Conforme == 'C') {
                    valorNC++;
                    valorNCTotal++;
                }

                listaObjTarefa.push([{ name: listaAgrupada[j].Tarefa, nc: valorNC, color:'#f8cc9d' }]);
                i += j;
            }
        } 

        listaFormatada.push({
            name: listaAgrupada[0].Monitoramento, totalNc: valorNCTotal, color: '#f08513', Tarefa: listaObjTarefa });

    }

    var listaFinal = [];
    listaFormatada.forEach(function (o, i) {

        listaFinal.push({ name: o.name, y: o.totalNc, color: o.color });

        for (var j = 0; j < o.Tarefa.length; j++) {
            listaFinal.push({ name: o.Tarefa[j][0].name.substring(0,10), y: o.Tarefa[j][0].nc, color: o.Tarefa[j][0].color });
        }

    });

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

function enviarFiltro() {

    //verificar o nivel para montar os items na tela
    let nivelVisao;

    $("#pills-tab li").each(function (i, o) {
        if ($(o).hasClass('active'))
            nivelVisao = $(o).children().attr('val');
    });

    switch (nivelVisao) {
        case '1':

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

                montaCards(data);
                montaGraficosPizza(data);
                montaGraficos(data);

                closeLoader();
            }).fail(function (jqXHR, textStatus, msg) {
                console.log(msg);
                //preencheRetornoGrafico("Ocorreu um erro ao buscar os dados. Erro: " + msg);
                closeLoader();
            });

            $("#divTableUnidade").addClass('hidden');
            $("#divAcompanhamento").addClass('hidden');
            $("#conteudoGraficos").removeClass('hidden');
            $("#cardsVisaoGeralUnidade").removeClass('hidden');
            break;
        case '2':
            $("#divTableUnidade").removeClass('hidden');
            $("#divAcompanhamento").addClass('hidden');
            $("#conteudoGraficos").removeClass('hidden');
            $("#cardsVisaoGeralUnidade").removeClass('hidden');

            $('#tblVisaoUnidade').DataTable();
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

                    openLoader('Aguarde...');

                    let acompanhamentoObj = JSON.parse(JSON.stringify(data));
                    var listaDeSemanas = [];

                    listaDeSemanas = montaListaSemanas(data);

                    var colunas = [
                        { title: "Grupo de Empresa", mData: "GrupoEmpresa" },
                        { title: "Regional", mData: "GrupoEmpresa" },
                        { title: "Unidade", mData: "Unidade" },
                        { title: "Auditor", mData: "Auditor Cabecalho" },
                        { title: "Total", mData: "total" },
                        {
                            title: "% Realizado", mData: null, mRender: function (acompanhamentoObj, type, full) {

                                var semanasZeradas = 0;
                                var meta = 2;
                                var total = 0;

                                for (var i = 0; i < listaDeSemanas.length; i++) {

                                    total = parseInt(total)
                                        + parseInt(acompanhamentoObj[listaDeSemanas[i].title] > meta ? meta : acompanhamentoObj[listaDeSemanas[i].title]);
                                }

                                var porcentagemTotal = total / (meta * listaDeSemanas.length) * 100;

                                return parseInt(porcentagemTotal.toFixed(2)) > 100 ? "100%" : parseInt(porcentagemTotal.toFixed(2)) + "%";
                            }
                        },
                        {
                            title: "% Seguro", mData: null, mRender: function (acompanhamentoObj, type, full) {
                                var porcentagemTotal = acompanhamentoObj.C / acompanhamentoObj.total * 100;

                                return parseInt(porcentagemTotal.toFixed(2)) > 100 ? "100%" : parseInt(porcentagemTotal.toFixed(2)) + "%";
                            }
                        },
                        {
                            title: "% Inseguro", mData: null, mRender: function (acompanhamentoObj, type, full) {
                                var porcentagemTotal = acompanhamentoObj.NC / acompanhamentoObj.total * 100;

                                return parseInt(porcentagemTotal.toFixed(2)) > 100 ? "100%" : parseInt(porcentagemTotal.toFixed(2)) + "%";
                            }
                        }
                    ];
                    
                    listaDeSemanas.filter((o, i) => colunas.splice(4, 0, o));

                    $("#tblAcompanhamento").removeClass('hidden');
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
                                if (i == row.cells.length - 3 && parseFloat(row.cells[i].innerHTML) <= 99)
                                    $('td:eq(' + i + ')', row).css("background-color", "#e37f7f").css("color", "white").css("font-weight", "bold"); //vermelho
                                else if (i == row.cells.length - 3 && parseFloat(row.cells[i].innerHTML) == 100)
                                    $('td:eq(' + i + ')', row).css("background-color", "#abebae").css("color", "black").css("font-weight", "bold"); //verde

                                //regra para pintar as duas ultimas colunas de meta de porcentagem 30 por 70
                                if (i == row.cells.length - 3) {
                                    var maximo = parseFloat(row.cells[i + 2].innerHTML);
                                    var minimo = parseFloat(row.cells[i + 1].innerHTML);
                                    if ((parseFloat(row.cells[i].innerHTML) <= minimo && parseFloat(row.cells[i].innerHTML) <= maximo)
                                        || parseFloat(row.cells[i].innerHTML) >= minimo && parseFloat(row.cells[i].innerHTML) <= maximo) {
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
                    $("#cardsAcompanhamento").show();
                    $('#message').html('');
                } else {
                    $('#message').html("<div class='alert alert-info'>  Não existem dados no período selecionado.  </div>");
                    $("#tblAcompanhamento").addClass('hidden');
                    $("#cardsAcompanhamento").hide();
                }
                closeLoader();
            }).fail(function (jqXHR, textStatus, msg) {
                console.log(msg);
                closeLoader();
            });

            $("#divTableUnidade").addClass('hidden');
            $("#divAcompanhamento").removeClass('hidden');
            $("#conteudoGraficos").addClass('hidden');
            $("#cardsVisaoGeralUnidade").addClass('hidden');
            break;
        default:
            break;
    }
}

$("#visaoGeral, #visaoUnidade, #acompanhamentoAuditoria").on('click', function () {
    //chama o enviar para buscar pelo nivel selecionado
    setTimeout(function () { enviarFiltro(); }, 1000);
});