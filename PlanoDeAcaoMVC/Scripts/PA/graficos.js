
function graficoEstoque() {

    dadosEstoque = [];

    jQuery.each(dados, function (i, val) {
        dadosEstoque.push(dados[i]);
    });

    //MOCK Acompanhamento
    dadosEstoque.forEach(function (o, c) {
        var tamanho = o.Acao._Acompanhamento.length
        if (tamanho == 0) {
            o.Acao['Acompanhamento'] = { AddDate: "1900-01-01" };
        } else {
            o.Acao['Acompanhamento'] = { AddDate: o.Acao._Acompanhamento[tamanho - 1].AddDate };
        }
    })

    let categoriesArr = [];
    var data = new Date();
    data = new Date(data.getFullYear(), data.getMonth(), 1);

    data.setMonth(data.getMonth() - 23);

    categoriesArr.push(new Date(data.getFullYear(), data.getMonth(), 1));
    data.setMonth(data.getMonth() + 1);
    data = new Date(data.getFullYear(), data.getMonth(), 1);
    categoriesArr.push(new Date(data.getFullYear(), data.getMonth(), 1));
    data.setMonth(data.getMonth() + 1);
    data = new Date(data.getFullYear(), data.getMonth(), 1);
    categoriesArr.push(new Date(data.getFullYear(), data.getMonth(), 1));
    data.setMonth(data.getMonth() + 1);
    data = new Date(data.getFullYear(), data.getMonth(), 1);
    categoriesArr.push(new Date(data.getFullYear(), data.getMonth(), 1));
    data.setMonth(data.getMonth() + 1);
    data = new Date(data.getFullYear(), data.getMonth(), 1);
    categoriesArr.push(new Date(data.getFullYear(), data.getMonth(), 1));
    data.setMonth(data.getMonth() + 1);
    data = new Date(data.getFullYear(), data.getMonth(), 1);
    categoriesArr.push(new Date(data.getFullYear(), data.getMonth(), 1));
    data.setMonth(data.getMonth() + 1);
    data = new Date(data.getFullYear(), data.getMonth(), 1);
    categoriesArr.push(new Date(data.getFullYear(), data.getMonth(), 1));
    data.setMonth(data.getMonth() + 1);
    data = new Date(data.getFullYear(), data.getMonth(), 1);
    categoriesArr.push(new Date(data.getFullYear(), data.getMonth(), 1));
    data.setMonth(data.getMonth() + 1);
    data = new Date(data.getFullYear(), data.getMonth(), 1);
    categoriesArr.push(new Date(data.getFullYear(), data.getMonth(), 1));
    data.setMonth(data.getMonth() + 1);
    data = new Date(data.getFullYear(), data.getMonth(), 1);
    categoriesArr.push(new Date(data.getFullYear(), data.getMonth(), 1));
    data.setMonth(data.getMonth() + 1);
    data = new Date(data.getFullYear(), data.getMonth(), 1);
    categoriesArr.push(new Date(data.getFullYear(), data.getMonth(), 1));
    data.setMonth(data.getMonth() + 1);
    data = new Date(data.getFullYear(), data.getMonth(), 1);
    categoriesArr.push(new Date(data.getFullYear(), data.getMonth(), 1));
    data.setMonth(data.getMonth() + 1);
    data = new Date(data.getFullYear(), data.getMonth(), 1);

    categoriesArr.push(new Date(data.getFullYear(), data.getMonth(), 1));
    data.setMonth(data.getMonth() + 1);
    data = new Date(data.getFullYear(), data.getMonth(), 1);
    categoriesArr.push(new Date(data.getFullYear(), data.getMonth(), 1));
    data.setMonth(data.getMonth() + 1);
    data = new Date(data.getFullYear(), data.getMonth(), 1);
    categoriesArr.push(new Date(data.getFullYear(), data.getMonth(), 1));
    data.setMonth(data.getMonth() + 1);
    data = new Date(data.getFullYear(), data.getMonth(), 1);
    categoriesArr.push(new Date(data.getFullYear(), data.getMonth(), 1));
    data.setMonth(data.getMonth() + 1);
    data = new Date(data.getFullYear(), data.getMonth(), 1);

    categoriesArr.push(new Date(data.getFullYear(), data.getMonth(), 1));
    data.setMonth(data.getMonth() + 1);
    data = new Date(data.getFullYear(), data.getMonth(), 1);
    categoriesArr.push(new Date(data.getFullYear(), data.getMonth(), 1));
    data.setMonth(data.getMonth() + 1);
    data = new Date(data.getFullYear(), data.getMonth(), 1);
    categoriesArr.push(new Date(data.getFullYear(), data.getMonth(), 1));
    data.setMonth(data.getMonth() + 1);
    data = new Date(data.getFullYear(), data.getMonth(), 1);
    categoriesArr.push(new Date(data.getFullYear(), data.getMonth(), 1));
    data.setMonth(data.getMonth() + 1);
    data = new Date(data.getFullYear(), data.getMonth(), 1);
    categoriesArr.push(new Date(data.getFullYear(), data.getMonth(), 1));
    data.setMonth(data.getMonth() + 1);
    data = new Date(data.getFullYear(), data.getMonth(), 1);
    categoriesArr.push(new Date(data.getFullYear(), data.getMonth(), 1));
    data.setMonth(data.getMonth() + 1);
    data = new Date(data.getFullYear(), data.getMonth(), 1);
    categoriesArr.push(new Date(data.getFullYear(), data.getMonth(), 1));
    data.setMonth(data.getMonth() + 1);
    data = new Date(data.getFullYear(), data.getMonth(), 1);
    categoriesArr.push(new Date(data.getFullYear(), data.getMonth(), 1));
    data.setMonth(data.getMonth() + 1);
    data = new Date(data.getFullYear(), data.getMonth(), 1);

    let arrayAndamento = [];

    let arrayFechadas = [];

    let arrayEstoque = [];

    let Aux = [];

    let apenasAcoesAbertas = [];

    let apenasAcoesFechadas = [];

    for (var i = 0; i < categoriesArr.length; i++) {

        arrayAndamento[i] = $.grep(dadosEstoque, function (o, j) {
            return new Date(o.Acao.QuandoInicio).getFullYear() == categoriesArr[i].getFullYear() && new Date(o.Acao.QuandoInicio).getMonth() == categoriesArr[i].getMonth() && o.Acao._StatusName.indexOf('Cancelada') < 0;
        }).length;

    }

    for (var i = 0; i < categoriesArr.length; i++) {

        arrayFechadas[i] = $.grep(dadosEstoque, function (o, j) {
            return new Date(o.Acao.Acompanhamento.AddDate).getFullYear() == categoriesArr[i].getFullYear() && new Date(o.Acao.Acompanhamento.AddDate).getMonth() == categoriesArr[i].getMonth() && o.Acao._StatusName && (o.Acao._StatusName.indexOf('Concluído') >= 0 || o.Acao._StatusName.indexOf('Finalizada')) >= 0;
        }).length;

    }

    arrayEstoque[0] = 0;

    for (var i = 0; i < categoriesArr.length; i++) {
        if (i == 0) {
            arrayEstoque[0] = arrayAndamento[0] - arrayFechadas[0];
        } else {
            arrayEstoque[i] = arrayEstoque[i - 1] + (arrayAndamento[i] - arrayFechadas[i]);
        }

    }

    let serieArrFinal = [{
        type: 'column',
        name: Resources('open'),
        data: arrayAndamento,
        color: andamentoColor
    }, {
        type: 'column',
        name: Resources('closed'),
        data: arrayFechadas,
        color: concluidoColor
    }, {
        type: 'spline',
        name: Resources('stock'),
        data: arrayEstoque,
        color: atrasadaColor,
        marker: {
            lineWidth: 2,
            lineColor: atrasadaColor,
            fillColor: atrasadaColor
        }
    }];


    for (var i = 0; i < categoriesArr.length; i++) {

        retorno = '';

        switch (categoriesArr[i].getMonth()) {
            case 0:
                retorno = Resources('jan');
                break;
            case 1:
                retorno = Resources('feb');
                break;
            case 2:
                retorno = Resources('mar');
                break;
            case 3:
                retorno = Resources('apr');
                break;
            case 4:
                retorno = Resources('may');
                break;
            case 5:
                retorno = Resources('june');
                break;
            case 6:
                retorno = Resources('july');
                break;
            case 7:
                retorno = Resources('aug');
                break;
            case 8:
                retorno = Resources('sept');
                break;
            case 9:
                retorno = Resources('oct');
                break;
            case 10:
                retorno = Resources('nov');
                break;
            case 11:
                retorno = Resources('dec');
                break;
        }

        categoriesArr[i] = retorno + '-' + categoriesArr[i].getFullYear();

    }

    makeChart('panel4', categoriesArr, serieArrFinal, 'column', '', {});

    $('select:not(#valor1Filtro)').select2({
        dropdownAutoWidth: true,
        width: '140px',
        matcher: matchCustom
    })

    $('select#valor1Filtro').select2({
        dropdownAutoWidth: true,
        width: '250px',
        matcher: matchCustom
    })

}

//Instancia HighCharts em um grafico padrão, aceita options para sobrescrita
function makeChart(id, categoriesArr, seriesArr, type, yAxisTitle, optionsDef) {

    if (id == 'panel5') {
        $('#semDados1').hide();
    }

    if (id == 'panel6') {
        $('#semDados2').hide();
    }

    if (seriesArr[0] != undefined) {

        var semDados = false;

        for (var h = 0; h < seriesArr.length; h++) {
            if (seriesArr[h].data.length == 1 && seriesArr[h].data[0] == 0) {
                semDados = false;
            } else {
                semDados = true;
                break;
            }

        }

        if (!semDados) {
            if (id == 'panel5') {
                $('#semDados1').show();
                return;
            }

            if (id == 'panel6') {
                $('#semDados2').show();
                return;
            }
        }
    }

    let options = {
        chart: {
            type: type,
            zoomType: 'xy'
        },
        title: {
            text: false
        },
        subtitle: {
            text: false
        },
        lang: {
            noData: "Sem dados"
        },
        xAxis: {

            categories: categoriesArr,
            title: {
                text: null
            },
            labels: {

                style: {
                    fontSize: '8px',
                    fontFamily: 'Verdana, sans-serif'
                }
            },
            tickInterval: 1,
            allowDecimals: false
        },
        yAxis: {
            allowDecimals: false,
            title: {
                text: GetPropertyRealName(yAxisTitle),
                align: 'high',
                style: {
                    fontSize: '8px',
                    fontFamily: 'Verdana, sans-serif'
                }
            },
            labels: {
                overflow: 'justify',

                style: {
                    fontSize: '8px',
                    fontFamily: 'Verdana, sans-serif'
                }

            }
        },
        tooltip: {
            valueSuffix: ''
        },
        plotOptions: {
            series: {
                stacking: 'normal',
                events: {
                    click: function (event) {
                        filterBar1ForDataTable(this.name, event.point.category, id)

                    }
                }

            },
            bar: {
                dataLabels: {
                    enabled: false
                }
            }
        },
        legend: {
            layout: 'horizontal',
            align: 'center',
            verticalAlign: 'bottom',
            floating: false,
            itemStyle: {
                fontSize: '8px',
                fontFamily: 'Verdana, sans-serif'
            }
        },
        credits: {
            enabled: false
        },

        series: seriesArr
    };

    if (optionsDef)
        Object.assign(options, optionsDef);

    Highcharts.chart(id, options);
}

function geraData1() {

    data1 = [];

    dados.sort(function (a, b) {
        return a.Acao.Status - b.Acao.Status;
    });

    jQuery.each(dados, function (i, val) {

        var campo = '';
        var cor = '';

        switch (dados[i].Acao.Status) {
            case 0:
                campo = Resources('without_status');
                cor = '#fdffb2';
                break;
            case 1:
                campo = Resources('late');
                cor = atrasadaColor;
                break;
            case 2:
                campo = Resources('cancel');
                cor = canceladoColor;
                break;
            case 3:
                campo = Resources('completed');
                cor = concluidoColor;
                break;
            case 4:
                campo = Resources('completed_late');
                cor = concluidoAtrasoColor;
                break;
            case 5:
                campo = Resources('in_progress');
                cor = andamentoColor;
                break;
            case 6:
                campo = Resources('_return');
                cor = retornoColor;
                break;
            case 7:
                campo = Resources('finished');
                cor = finalizadaColor;
                break;
            case 8:
                campo = Resources('finished_late');
                cor = finalizadaComAtrasoColor;
                break;
            case 9:
                campo = Resources('not_started');
                cor = naoIniciadoColor;
                break;
            default:
                campo = Resources('status');
                cor = 'black';
                break;

        }

        if (data1[0] == undefined) {
            if (dados[i].Acao.Status != 0) {
                data1.push({ name: campo, y: 1, color: cor });
            }
        } else if (data1[data1.length - 1].name == campo) {
            data1[data1.length - 1].y = data1[data1.length - 1].y + 1;
        } else if (dados[i].Acao.Status != 0) {
            data1.push({ name: campo, y: 1, color: cor });
        }
    });

}

function geraData2(dadosFiltrados) {

    data1 = [];

    dadosFiltrados.sort(function (a, b) {
        return a.Acao.Status - b.Acao.Status;
    });


    jQuery.each(dadosFiltrados, function (i, val) {

        var campo = '';
        var cor = '';

        switch (dadosFiltrados[i].Acao.Status) {
            case 0:
                campo = Resources('without_status');
                cor = '#fdffb2';
                break;
            case 1:
                campo = Resources('late');
                cor = atrasadaColor;
                break;
            case 2:
                campo = Resources('cancel');
                cor = canceladoColor;
                break;
            case 3:
                campo = Resources('completed');
                cor = concluidoColor;
                break;
            case 4:
                campo = Resources('completed_late');
                cor = concluidoAtrasoColor;
                break;
            case 5:
                campo = Resources('in_progress');
                cor = andamentoColor;
                break;
            case 6:
                campo = Resources('_return');
                cor = retornoColor;
                break;
            case 7:
                campo = Resources('finished');
                cor = finalizadaColor;
                break;
            case 8:
                campo = Resources('finished_late');
                cor = finalizadaComAtrasoColor;
                break;
            case 9:
                campo = Resources('not_started');
                cor = naoIniciadoColor;
                break;
            default:
                campo = Resources('status');
                cor = 'black';
                break;

        }

        if (data1[0] == undefined) {
            if (dadosFiltrados[i].Acao.Status != 0) {
                data1.push({ name: campo, y: 1, color: cor });
            }
        } else if (data1[data1.length - 1].name == campo) {
            data1[data1.length - 1].y = data1[data1.length - 1].y + 1;
        } else if (dadosFiltrados[i].Acao.Status != 0) {
            data1.push({ name: campo, y: 1, color: cor });
        }
    });

    getGraphPanel2(data1);
}

function getGraphPanel1(meuDado) {
    Highcharts.chart('panel1', {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        credits: {
            enabled: false
        },
        title: {
            text: false
        },
        tooltip: {
            pointFormat: '<b>{point.percentage:.1f}%</b>'
        },
        lang: {
            noData: "Sem dados"
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '{point.percentage:.1f} %',
                    distance: -10,
                    style: {
                        //color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'white',
                        color: 'black',
                        fontSize: '10px'
                    },
                    connectorColor: 'silver'
                }
            },
            series: {
                cursor: 'pointer',
                events: {
                    click: function (event) {
                        filterPieForDataTable(event.point.name)
                    }
                }
            }
        },
        series: [{
            name: '',
            data: meuDado
        }]
    });
}

function getGraphPanel2(meuDado) {
    Highcharts.chart('panel2', {
        chart: {
            backgroundColor: 'rgba(255, 255, 255, 0.0)',
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        credits: {
            enabled: false
        },
        title: {
            text: false
        },
        lang: {
            noData: "Sem dados"
        },
        tooltip: {
            pointFormat: '<b>{point.percentage:.1f}%</b>'
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '{point.percentage:.1f} %',
                    distance: -10,
                    style: {
                        //color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'white',
                        color: 'black',
                        fontSize: '10px'
                    },
                    connectorColor: 'silver'
                }
            },
            series: {
                cursor: 'pointer',
                events: {
                    click: function (event) {
                        filterPie2ForDataTable(event.point.name);
                    }
                }
            }
        },
        series: [{
            name: '',
            data: meuDado
        }]
    });
}

Highcharts.chart('panel4', {
    title: {
        text: false
    },
    legend: {
        enabled: true,
        verticalAlign: 'top',
        itemStyle: {
            fontSize: '8px',
            fontFamily: 'Verdana, sans-serif'
        }
    },
    credits: {
        enabled: false
    },
    lang: {
        noData: "Sem dados"
    },
    xAxis: {
        tickInterval: 1,
        categories: categories4,
        labels: {

            style: {
                fontSize: '8px',
                fontFamily: 'Verdana, sans-serif'
            }
        }
    },
    yAxis: {
        enabled: false,
        min: 0,
        title: {
            text: false,
        },
        labels: {

            style: {
                fontSize: '8px',
                fontFamily: 'Verdana, sans-serif'
            }
        }
    },
    plotOptions: {
        series: {
            stacking: 'normal',
            events: {
                click: function (event) {
                    filterBar1ForDataTable(event.point.name, event.point.category)
                }
            }
        },
        bar: {
            dataLabels: {
                enabled: false
            }
        }
    },
    series: data4
});

Highcharts.chart('panel5', {
    chart: {
        type: 'bar'
    },
    title: {
        text: false
    },
    lang: {
        noData: "Sem dados"
    },
    subtitle: {
        text: false
    },
    xAxis: {
        tickInterval: 1,

        categories: categories5,
        title: {
            text: null
        },
        labels: {

            style: {
                fontSize: '8px',
                fontFamily: 'Verdana, sans-serif'
            }
        }
    },
    yAxis: {
        min: 0,
        title: {
            text: Resources('number_action'),
            align: 'high',
            style: {
                fontSize: '8px',
                fontFamily: 'Verdana, sans-serif'
            },
            allowDecimals: false
        },
        labels: {
            overflow: 'justify',

            style: {
                fontSize: '8px',
                fontFamily: 'Verdana, sans-serif'
            }

        }
    },
    tooltip: {
        valueSuffix: ''
    },
    plotOptions: {
        series: {
            stacking: 'normal',
            events: {
                click: function (event) {
                    filterBar1ForDataTable(event.point.name, event.point.category)
                }
            }
        },
        bar: {
            dataLabels: {
                enabled: false
            }
        }
    },
    legend: {
        layout: 'horizontal',
        align: 'center',
        verticalAlign: 'bottom',
        floating: false,
        itemStyle: {
            fontSize: '8px',
            fontFamily: 'Verdana, sans-serif'
        }
    },
    credits: {
        enabled: false
    },

    series: data5
});

Highcharts.chart('panel6', {
    chart: {
        type: 'bar'
    },
    title: {
        text: false
    },
    subtitle: {
        text: false
    },
    lang: {
        noData: "Sem dados"
    },
    xAxis: {
        tickInterval: 1,
        categories: categories6,
        title: {
            text: null
        },
        labels: {

            style: {
                fontSize: '8px',
                fontFamily: 'Verdana, sans-serif'
            }
        },
        allowDecimals: false
    },
    yAxis: {
        min: 0,
        title: {
            text: Resources('number_action'),
            align: 'high',
            style: {
                fontSize: '8px',
                fontFamily: 'Verdana, sans-serif'
            }
        },
        labels: {
            overflow: 'justify',

            style: {
                fontSize: '8px',
                fontFamily: 'Verdana, sans-serif'
            }

        }
    },
    tooltip: {
        valueSuffix: ' millions'
    },
    plotOptions: {
        series: {
            stacking: 'normal'
        },
        bar: {
            dataLabels: {
                enabled: false
            }
        }
    },
    legend: {
        layout: 'horizontal',
        align: 'center',
        verticalAlign: 'bottom',
        floating: false,
        itemStyle: {
            fontSize: '8px',
            fontFamily: 'Verdana, sans-serif'
        }
    },
    credits: {
        enabled: false
    },

    series: data6
});