//Visao Geral dos Projetos
var TipoGrafico = null;

$('#modalVisaoGeralProjetos').on('shown.bs.modal', function () {

    getVisaoGeralProjetos(false);

});

function getVisaoGeralProjetos(filtro) {

    $('#btnFiltraSemPrioridade').addClass('hide');
    $('#btnFiltraPrioridade').addClass('hide');

    Chaveamento = [];
    ChaveamentoObjetivo = [];

    let newDados = dados.slice();

    $('#graficoPorDiretriz').empty();
    $('#graficoPorGerencia').empty();

    let Categories = [];
    let Data = [];

    if (TipoGrafico == "Gerencia") {

        processarChaveamento();

        Categories = Object.values(MapeiaValorParaHC(newDados, "Gerencia").filter(onlyUnique));

        Data = [];

        Categories.forEach(function (c, o) {

            let qtdConcluido = $.grep(newDados, function (r) {
                return ((r.Acao.Status == 3 || r.Acao.Status == 4) && r.Gerencia == c);
            }).length;

            let listaGerencia = $.grep(newDados, function (r) {
                return r.Gerencia == c;
            })

            let qtdDeGerencia = listaGerencia.length;

            let porcConcluidos = qtdConcluido == 0 ? 0 : (qtdConcluido / qtdDeGerencia) * 100

            let gerencia = retornaChaveamentoPorGerenciaId(listaGerencia[0].Gerencia_Id)[0];

            Data.push({ color: gerencia.color, y: porcConcluidos });
        });

        Highcharts.chart('graficoPorGerencia', {

            title: {
                text: Resources('percent_completed_area')
            },
            chart: {
                height: 230
            },
            credits: {
                enabled: false
            },
            lang: {
                noData: "Sem dados"
            },
            subtitle: {
                text: ''
            },
            yAxis: {
                title: {
                    text: Resources('percent_conclusion')
                }
            },
            plotOptions: {
                column: {
                    dataLabels: {
                        enabled: true,
                        format: '{point.y:.0f} %',
                        style: {
                            color: 'black',
                            fontSize: '10px',
                        },

                    },
                    point: {
                        events: {
                            click: function () {
                                filterDados("Gerencia", this.category, getTablesModal, newDados);
                                getGraficoPorProjetos("Gerencia", this.category, true, newDados);
                            }
                        }
                    },
                },
            },
            xAxis: {
                categories: Categories
            },
            tooltip: {
                pointFormat: '{point.y:.0f} %'
            },
            series: [{
                type: 'column',
                colorByPoint: true,
                data: Data,
                showInLegend: false
            }]

        });

    }
    else {

        if (filtro) {

            newDados = $.grep(newDados, (o) => { return o["ObjetivoPriorizado"] == true });
            $('#btnFiltraSemPrioridade').removeClass('hide');
            $('#btnFiltraPrioridade').addClass('hide');

        } else {

            $('#btnFiltraSemPrioridade').addClass('hide');
            $('#btnFiltraPrioridade').removeClass('hide');

        }

        processarChaveamentoObjetivo();

        Categories = Object.values(MapeiaValorParaHC(newDados, "Objetivo").filter(onlyUnique));

        Data = [];

        Categories.forEach(function (c, o) {

            let qtdConcluido = $.grep(newDados, function (r) {
                return ((r.Acao.Status == 3 || r.Acao.Status == 4) && r.Objetivo == c);
            }).length;

            let listaObjetivo = $.grep(newDados, function (r) {
                return r.Objetivo == c;
            });

            let qtdDeObjetivo = listaObjetivo.length;

            let porcConcluidos = qtdConcluido == 0 ? 0 : (qtdConcluido / qtdDeObjetivo) * 100

            let objetivo = retornaChaveamentoPorObjetivo(listaObjetivo[0].Objetivo_Id)[0];
            Data.push({ color: objetivo.color, y: porcConcluidos });

        });

        Highcharts.chart('graficoPorDiretriz', {

            title: {
                text: Resources('percent_completed_area')
            },
            chart: {
                height: 230
            },
            credits: {
                enabled: false
            },
            lang: {
                noData: "Sem dados"
            },
            subtitle: {
                text: ''
            },
            yAxis: {
                title: {
                    text: Resources('percent_conclusion')
                }
            },
            plotOptions: {
                column: {
                    dataLabels: {
                        enabled: true,
                        format: '{point.y:.0f} %',
                        style: {
                            color: 'black',
                            fontSize: '10px',
                        },
                    },
                    point: {
                        events: {
                            click: function () {
                                filterDados("Objetivo", this.category, getTablesModal, newDados);
                                getGraficoPorProjetos("Objetivo", this.category, true, newDados);
                            }
                        }
                    },
                },
            },
            xAxis: {
                categories: Categories
            },
            tooltip: {
                pointFormat: '{point.y:.0f} %'
            },
            series: [{
                type: 'column',
                colorByPoint: true,
                data: Data,
                showInLegend: false
            }]

        });
    }

    getGraficoPorProjetos(null, null, false, newDados);

    getTablesModal(newDados);
}

function vgp(tipoGrafico) {
    TipoGrafico = tipoGrafico;
}

function getAcoesEspecificas(arrayRetorno) {

    let AcoesEspecificas = [];

    arrayRetorno.forEach(function (i) {
        AcoesEspecificas.push(Object.values(i));
    });

    return AcoesEspecificas;
}

function getTableModal(id, dataSet, columnsSet) {

    let table = $('#' + id).DataTable({

        data: dataSet,
        columns: columnsSet,
        destroy: true,
        pageLength: 5,
        lengthMenu: [[5, 10, 20, -1], [5, 10, 20, Resources('all')]],
        language: {
            "sEmptyTable": Resources('no_records_found'),
            "sInfo": Resources('showing_start_to_end_of_total_records'),
            "sInfoEmpty": Resources('showing_0_to_0_of_0_records'),
            "sInfoFiltered": Resources('filtered_max_records'),
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": Resources('menu_results_per_page'),
            "sLoadingRecords": Resources('loading'),
            "sProcessing": Resources('processing'),
            "sZeroRecords": Resources('no_records_found'),
            "sSearch": Resources('search'),
            "oPaginate": {
                "sNext": Resources('next'),
                "sPrevious": Resources('back'),
                "sFirst": Resources('first'),
                "sLast": Resources('last')
            },
            "oAria": {
                "sSortAscending": Resources('sort_columns_ascending'),
                "sSortDescending": Resources('sort_columns_downward')
            }
        }
    });

    return table;
}

function filterDados(prop, value, calback, arr) {

    let dadosfilter = $.grep(arr, function (item) {

        if (item[prop] == value) {

            return item;


        }

    });

    calback(dadosfilter);
}

function getTablesModal(newDados) {

    let renderFunction = function (data) {

        //Gambzinha porque quando converte a data, ela fica com um dia a menos, então tive que somar mais um dia - Renan 22/06/2018
        let dataFormatada = new Date(data);
        dataFormatada.setDate(dataFormatada.getDate() + 1);

        let html = "<span style='display:none'>" + data + "</span>" + dataFormatada.toLocaleDateString();

        return html;
    }

    // Acão Em Andamento
    let AcoesEspecificas = [];

    var arrayRetorno = $.map(newDados, function (o) {

        var valor = o['Acao'].ContramedidaEspecifica;

        if (o['Acao'].Status == 5) {
            return valor;
        } else {
            if (o['Acao'].Id == 0) {
                valor = Resources('no_specific_action');
            }
        }

    });

    Object.values(arrayRetorno.filter(onlyUnique)).forEach(function (i) {
        AcoesEspecificas.push([i]);
    });

    let TituloAcoesEspecificas = [{ title: Resources('specific_action') }]

    var tableAndamento = getTableModal("tableAndamento", AcoesEspecificas, TituloAcoesEspecificas);

    //Acoes Atrasadas
    AcoesEspecificas = [];

    arrayRetorno = $.map(newDados, function (o, c) {

        if (o['Acao'].Status == 1) {

            dataFim = new Date(o['Acao'].QuandoFim);
            dataHoje = new Date();

            var timeDiff = Math.abs(dataHoje.getTime() - dataFim.getTime());
            var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));

            var valor = o['Acao'].ContramedidaEspecifica != null ? o['Acao'].ContramedidaEspecifica : Resources('no_specific_action');

            return { 0: valor, 1: String(diffDays - 1) + " Dias" };
        } else {
            if (o['Acao'].Id == 0) {
                valor = { 0: Resources('no_specific_action'), 1: "" };
            }
        }

    });

    AcoesEspecificas = getAcoesEspecificas(arrayRetorno);

    TituloAcoesEspecificas = [{ title: Resources('specific_action') }, { title: Resources('days_of_delay') }]

    var tableAtrasadas = getTableModal("tableAtrasadas", AcoesEspecificas, TituloAcoesEspecificas);

    //Proximas
    AcoesEspecificas = [];

    arrayRetorno = $.map(newDados, function (o, c) {

        dataInicio = new Date(o['Acao'].QuandoInicio);
        dataHoje = new Date();

        if (dataInicio > dataHoje && o['Acao'].Status != 3 && o['Acao'].Status != 7) {

            var timeDiff = Math.abs(dataHoje.getTime() - dataFim.getTime());
            var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));

            var valor = { 0: o['Acao'].ContramedidaEspecifica, 1: dataInicio.toISOString().slice(0, 10) };

            return valor;
        } else {
            if (o['Acao'].Id == 0) {
                valor = { 0: Resources('no_specific_action'), 1: "" };
            }
        }

    });

    AcoesEspecificas = getAcoesEspecificas(arrayRetorno);

    TituloAcoesEspecificas = [{ title: Resources('specific_action') }, { title: Resources('start_date'), render: renderFunction }]

    var tableProximas = getTableModal("tableProximas", AcoesEspecificas, TituloAcoesEspecificas);

    //Concluidas
    AcoesEspecificas = [];

    arrayRetorno = $.map(newDados, function (o, c) {

        if (o['Acao'].Status == 3 || o['Acao'].Status == 4) {

            let dataConclusao = new Date(o['Acao'].Acompanhamento.AddDate);

            var valor = { 0: o['Acao'].ContramedidaEspecifica, 1: dataConclusao.toISOString().slice(0, 10) };

            return valor;

        } else {

            if (o['Acao'].Id == 0) {
                valor = { 0: Resources('no_specific_action'), 1: "" };
            }
        }
    });

    AcoesEspecificas = getAcoesEspecificas(arrayRetorno);

    TituloAcoesEspecificas = [{ title: Resources('specific_action') }, { title: Resources('date_conclusion'), render: renderFunction }]

    var tableConcluidas = getTableModal("tableConcluidas", AcoesEspecificas, TituloAcoesEspecificas);
}

function getGraficoPorProjetos(prop, value, isFilter, newDados) {

    let Categories = [];

    let dadosInternos = newDados;

    if (isFilter) {

        dadosInternos = $.grep(newDados, function (r) {

            return r[prop] == value;

        });
    }

    Categories = Object.values(MapeiaValorParaHC(dadosInternos, "Iniciativa").filter(onlyUnique));

    Data = [];

    processarChaveamentoObjetivo();
    processarChaveamento();

    let dados2 = newDados;

    if (isFilter) {
        dados2 = $.grep(newDados, function (item) {

            if (item[prop] == value) {

                return item;

            }
        });
    }

    Categories.forEach(function (c, o) {

        let qtdConcluido = $.grep(dados2, function (r) {
            return ((r.Acao.Status == 3 || r.Acao.Status == 4) && r.Iniciativa == c);
        }).length;

        let listaGerencia = $.grep(dados2, function (r) {
            return r.Iniciativa == c;
        });

        let qtdDeGerencia = listaGerencia.length;

        let porcConcluidos = qtdConcluido == 0 ? 0 : (qtdConcluido / qtdDeGerencia) * 100

        if (TipoGrafico == "Gerencia") {
            let gerencia = retornaChaveamentoPorGerenciaId(listaGerencia[0].Gerencia_Id)[0];
            Data.push({ color: gerencia.color, y: porcConcluidos });
        } else {
            let objetivo = retornaChaveamentoPorObjetivo(listaGerencia[0].Objetivo_Id)[0];
            Data.push({ color: objetivo.color, y: porcConcluidos });
        }
    });

    Highcharts.chart('graficoPorProjetos', {

        title: {
            text: Resources('percent_completed_demands')
        },
        credits: {
            enabled: false
        },
        subtitle: {
            text: ''
        },
        lang: {
            noData: "Sem dados"
        },
        yAxis: {
            title: {
                text: Resources('percent_conclusion')
            }
        },
        xAxis: {
            categories: Categories,
        },
        plotOptions: {
            column: {
                dataLabels: {
                    enabled: true,
                    format: '{point.y:.0f} %',
                    style: {
                        color: 'black',
                        fontSize: '10px',
                    },
                },
                point: {
                    events: {
                        click: function () {
                            console.log(this.category)

                            filterDados("Iniciativa", this.category, getTablesModal, dados2)
                        }
                    }
                },
            },
        },
        tooltip: {
            pointFormat: '{point.y:.0f} %'
        },
        series: [{
            type: 'column',
            colorByPoint: true,
            data: Data,
            showInLegend: false
        }]
    });

    let PercentualTotal = Math.floor(Data.reduce((soma, obj) => soma + obj.y, 0) / Categories.length);

    if (PercentualTotal)
        $('#percentualConclusao').html(PercentualTotal + " %");
    else
        $('#percentualConclusao').html("0 %");

}