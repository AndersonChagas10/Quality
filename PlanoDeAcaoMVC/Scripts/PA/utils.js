function getDateRange(campo) {
    var datas = campo.split(' - ');
    var dataPTInicio = datas[0].split("/");
    var dataPTFim = datas[1].split("/");

    dataInicio = new Date(dataPTInicio[2] + ' ' + dataPTInicio[1] + ' ' + dataPTInicio[0]);
    dataFim = new Date(dataPTFim[2] + ' ' + dataPTFim[1] + ' ' + dataPTFim[0]);
}

function getDateUSA(campo) {
    var dataPTInicio = campo.split("/");
    data = new Date(dataPTInicio[2] + ' ' + dataPTInicio[1] + ' ' + dataPTInicio[0]);
    if (data != 'Invalid Date') {
        return data;
    }
}

function sortFunction(a, b) {
    if (a[0] === b[0]) {
        return 0;
    }
    else {
        return (a[0] < b[0]) ? -1 : 1;
    }
}

function onlyUnique(value, index, self) {
    return self.indexOf(value) === index;
}

function MapeiaValorParaHC(array, prop, isInteger) {

    var arrayRetorno = $.map(array, function (o, c) {

        if (isInteger) {
            var valor = parseInt(o[prop]);
            if (isNaN(valor)) return 0;
            return valor;
        }

        var propArray = prop.split('.');

        if (propArray.length == 2) {

            if (propArray[1] == "TipoIndicador") {

                var value = o[propArray[0]][propArray[1]];

                if (value == 0)
                    value = Resources("no_operational_planning");

                else if (value == 1)
                    value = Resources("guidelines");

                else if (value == 2)
                    value = "Scorecard";

                return value;

            } else {

                return o[propArray[0]][propArray[1]] == "" ? null : o[propArray[0]][propArray[1]];

            }

        } else {

            return o[prop];

        }

    });

    return arrayRetorno;
}

function orderArray(array) {

    array.sort(function (a, b) {

        if (a.name > b.name) {
            return 1;
        }

        if (a.name < b.name) {
            return -1;
        }

        return 0;
    });

    return array;
}

function pintaStatus(seriesFilter, serieArrFinal) {

    if (seriesFilter == 'Status' && serieArrFinal != '') {

        Highcharts.setOptions({
            colors: ['red', 'pink', 'blue', 'orange', 'green', 'black', "yellow", "gray"]
        });

        var serieAux;
        var listaVazia;
        var aux = 0;
        serieAux = orderArray(serie);
        serie = [];

        for (var i = 0; i < listaStatus.length; i++) {

            if (serieAux[aux].name == listaStatus[i]) {

                serie.push(serieAux[aux]);

                if (serieAux.length > aux + 1) {
                    aux++;
                }

            } else {

                listaVazia = { name: listaStatus[i], data: null, visible: false };
                serie.push(listaVazia);

            }
        }

        return serie;

    } else {

        serieArrFinal.forEach(function (c, o) {

            if (c.name == Resources("late")) {
                c["color"] = atrasadaColor;

            } else if (c.name == Resources("completed")) {
                c["color"] = concluidoColor;

            } else if (c.name == Resources("completed_late")) {
                c["color"] = concluidoAtrasoColor;

            } else if (c.name == Resources("in_progress")) {
                c["color"] = andamentoColor;

            } else if (c.name == Resources("canceled")) {
                c["color"] = canceladoColor;

            } else if (c.name == Resources("_return")) {
                c["color"] = retornoColor;

            } else if (c.name == Resources("finished")) {
                c["color"] = finalizadaColor;

            } else if (c.name == Resources("finished_late")) {
                c["color"] = finalizadaComAtrasoColor;

            } else if (c.name == Resources("not_started")) {
                c["color"] = naoIniciadoColor;
            }
        });

        return serieArrFinal;
    }
}

function btnOrder(campo1, campo2, painel, btn) {

    if (btnOrderFilter == "Asc")
        btnOrderFilter = "";

    else if (btnOrderFilter == "")
        btnOrderFilter = "Asc";

    var titleSpan = "";
    var classSpan = "";

    switch (btnOrderFilter) {
        case "":
            classSpan = "glyphicon glyphicon-sort-by-attributes-alt";
            titleSpan = Resources("order_by_value");
            break;

        case "Asc":
            titleSpan = Resources("alphabetical_order");
            classSpan = "glyphicon glyphicon-sort-by-alphabet";
            break;
    }

    $('#' + btn + ' span').removeClass().addClass(classSpan);

    document.getElementById(btn).title = titleSpan;

    filtraDadosParaGerarGraficoPanel5Panel6($('#' + campo1 + ' option:selected').val(), $('#' + campo2 + ' option:selected').val(), btnOrderFilter, painel)

}

function randomColor() {
    color = 'rgb(' + Math.round(Math.random() * 255) + ',' + Math.round(Math.random() * 255) + ',' + Math.round(Math.random() * 255) + ')';
    return color;
}

function Resources(key) {
    return $.grep(ResourcesPAFile, function (a) { return a.Key == key })[0].Value
}

function FiltraColunas(array, arrFiltro) {

    let novoArr = [];

    array.forEach(function (o, c) {

        obj = {};
        objt = {};
        arrFiltro.forEach(function (oo, cc) {

            obj[oo] = o[oo];
            objt[oo] = o.Acao[oo];

            if (cc == "TipoIndocador") {
                if (objt.TipoIndicador == 1) {
                    $('');
                }
            }
        });

        novoArr.push(obj);
        novoArr.push(objt);
    });

    return novoArr;
}

//Função para buscas nos Select2
//function matchStart(params, data) {

//    if ($.trim(params.term) === '') {
//        return data;
//    }

//    if (data.text.indexOf(params.term) > 0) {
//        return data;
//    } else {
//        return null;
//    }

//}

//$("#campo1Filtro").select2({
//    matcher: matchStart
//});

$(function () {

    var rangesBR = {
        'Hoje': [moment(), moment()],
        'Ontem': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
        'Últimos 7 Dias': [moment().subtract(6, 'days'), moment()],
        'Últimos 30 Dias': [moment().subtract(29, 'days'), moment()],
        'Este mês': [moment().startOf('month'), moment().endOf('month')],
        'Último mês': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
    };

    moment.locale("Pt-Br");
    moment.defaultFormat = "DD/MM/YYYY";

    $.fn.daterangepicker('locale', {
        "format": "DD/MM/YYYY",
        "separator": " - ",
        "applyLabel": Resources("apply"),
        "cancelLabel": Resources("cancel"),
        "fromLabel": Resources("of"),
        "toLabel": Resources("until"),
        "customRangeLabel": Resources("customized"),
        "weekLabel": "W",
        "daysOfWeek": [
            Resources("sun"),
            Resources("mon"),
            Resources("tue"),
            Resources("wed"),
            Resources("thur"),
            Resources("fri"),
            Resources("sat")
        ],
        "monthNames": [
            Resources("jan2"),
            Resources("feb2"),
            Resources("mar2"),
            Resources("apr2"),
            Resources("may2"),
            Resources("june2"),
            Resources("july2"),
            Resources("aug2"),
            Resources("sept2"),
            Resources("oct2"),
            Resources("nov2"),
            Resources("dec2")
        ]
    });

    rangesCalendar = rangesBR;

    $.fn.daterangepicker('ranges', rangesCalendar);

    function cb(start, end) {
        $('#reportrange span').html(start.format() + ' - ' + end.format());
        enviar['startDate'] = start.format();
        enviar['endDate'] = end.format();
    }

    var configQueryData = true;
    var configCallendar = {};

    if (configQueryData) {

        $('input[name="daterange"]').daterangepicker({
            startDate: start,
            endDate: end,
            locale: {
                "format": "DD/MM/YYYY",
                "separator": " - ",
                "applyLabel": Resources("apply"),
                "cancelLabel": Resources("cancel"),
                "customRangeLabel": Resources("selection"),
            },
            alwaysShowCalendars: true,
            showDropdowns: true,
            ranges: rangesCalendar,
            linkedCalendars: false
        }, cb);

    } else {

        $('input[name="daterange"]').daterangepicker({
            startDate: start,
            endDate: end,
            locale: {
                "format": "MM/DD/YYYY",
                "separator": " - ",
            },
            alwaysShowCalendars: true,
            showDropdowns: true,
            ranges: rangesCalendar,
            linkedCalendars: false
        }, cb);
    }

    cb(start, end);

    enviar['startDate'] = moment().add('month', -1).format();
    enviar['endDate'] = moment().format();

});

$(document).ajaxSend(function () {
    $.LoadingOverlay("show");
    counter++;
});

$(document).ajaxStop(function () {
    $.LoadingOverlay("hide", true);
    counter--;
});

$(document).ajaxError(function (e, h, x) {
    console.log(e);
    console.log(h);
    console.log(x);
    $.LoadingOverlay("hide");
    counter--;
});