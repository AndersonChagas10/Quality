/*Auxiliar*/
function atualizar(callback, url, obj) {

    $.post(url, obj, function (r) {

        //console.log(r);

        callback(r);
    });

}

function InitDatePiker() {
    $('.datepicker').datepicker({
        startDate: 0,
        todayBtn: "linked",
        language: "pt-BR",
        multidate: false,
        todayHighlight: true
    });
}

/*
Retorna apenas elementos unicos do array
*/
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
        return o[prop];
    });
    return arrayRetorno;
}

// Radialize the colors
Highcharts.getOptions().colors = Highcharts.map(Highcharts.getOptions().colors, function (color) {
    return {
        radialGradient: {
            cx: 0.5,
            cy: 0.3,
            r: 0.7
        },
        stops: [
            [0, color],
            [1, Highcharts.Color(color).brighten(-0.3).get('rgb')] // darken
        ]
    };
});

acoesConcluidas = 25;
acoesAtrasadas = 21;
acoesAVencer = 10;
pessoasEnvolvidas = 12;

$('#acoesConcluidas').text(acoesConcluidas);
$('#acoesAtrasadas').text(acoesAtrasadas);
$('#acoesAVencer').text(acoesAVencer);
$('#pessoasEnvolvidas').text(pessoasEnvolvidas);

tituloXBar = 'Status das Ações';
tituloXColumn = "Status das Ações";

tituloYBar = 'Número de ações';
tituloYColumn = 'Número de ações';

tituloBar = 'Status das Ações por Indicadores';
tituloColumn = 'Status das Ações por Responsável';

/*Config Callendar PT-BR*/
$('body .dropdown-toggle').dropdown();

$('.popover-markup>.trigger').popover({
    html: true,
    title: function () {
        return $(this).parent().find('.head').html();
    },
    content: function () {
        return $(this).parent().find('.content').html();
    }
});

var rangesBR = {
    'Hoje': [moment(), moment()],
    'Ontem': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
    'Últimos 7 Dias': [moment().subtract(6, 'days'), moment()],
    'Últimos 30 Dias': [moment().subtract(29, 'days'), moment()],
    'Este mês': [moment().startOf('month'), moment().endOf('month')],
    'Último mês': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
}

moment.locale("Pt-Br");
moment.defaultFormat = "DD/MM/YYYY";

$.fn.daterangepicker('locale', {
    "format": "DD/MM/YYYY",
    "separator": " - ",
    "applyLabel": "Aplicar",
    "cancelLabel": "Cancelar",
    "fromLabel": "de",
    "toLabel": "até",
    "customRangeLabel": "Customizado",
    "weekLabel": "W",
    "daysOfWeek": [
        "Dom",
        "Seg",
        "Ter",
        "Qua",
        "Qui",
        "Sex",
        "Sab"
    ],
    "monthNames": [
        "Janeiro",
        "Fevereiro",
        "Março",
        "Abril",
        "Maio",
        "Junho",
        "Julho",
        "Agosto",
        "Setembro",
        "Outubro",
        "Novembro",
        "Dezembro"
    ],
    //"firstDay": 0
});

rangesCalendar = rangesBR;

$.fn.daterangepicker('ranges', rangesCalendar);

//Start config datepickerrange full
var start = moment();
var end = moment();

function cb(start, end) {
    $('#reportrange span').html(start.format() + ' - ' + end.format());
    enviar['startDate'] = start.format();
    enviar['endDate'] = end.format();
}
var configQueryData = true; // @*@Html.Raw(Json.Encode(GlobalConfig.LanguageBrasil));*@
var configCallendar = {};
if (configQueryData) {
    $('#reportrange').daterangepicker({
        startDate: start,
        endDate: end,
        //opens: "left",
        locale: {
            "format": "DD/MM/YYYY",
            "separator": " - ",
            "applyLabel": "Aplicar",
            "cancelLabel": "Cancelar",
            "customRangeLabel": "Seleção",
        },
        //autoApply: false,
        alwaysShowCalendars: true,
        showDropdowns: true,
        ranges: rangesCalendar,
        linkedCalendars: false
    }, cb);
} else {
    $('#reportrange').daterangepicker({
        startDate: start,
        endDate: end,
        //autoApply: false,
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
//End config datepickerrange full

enviar['startDate'] = moment().format();
enviar['endDate'] = moment().format();
/*Config Callendar FIM*/

/*Auxiliar FIM*/