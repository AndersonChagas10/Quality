//Data string deve ser yyyy-MM-dd HH:MM:SS sendo a hora opicional
function stringToDate(dateString) {

    var dia = parseInt(dateString.substr(8, 2));
    var mes = parseInt(dateString.substr(5, 2)) - 1;
    var ano = parseInt(dateString.substr(0, 4));

    var hora = parseInt(dateString.substr(11, 2)) ? parseInt(dateString.substr(11, 2)) : 0;
    var segundos = parseInt(dateString.substr(14, 2)) ? parseInt(dateString.substr(14, 2)) : 0;
    var mimutos = parseInt(dateString.substr(18, 2)) ? parseInt(dateString.substr(18, 2)) : 0;

    //new Date(year, month, day, hours, minutes, seconds, milliseconds)
    return new Date(ano, mes, dia, hora, mimutos, segundos);
}

function convertDateToJson(date) {

    var newDate = new Date(date);

    return new Date(newDate.setHours(newDate.getHours() - 3)).toJSON();
}

function getCurrentDate() {

    //hora atual
    var hour = new Date().toLocaleTimeString();

    //data collectionDate
    var data = new Date(currentCollectDate).toJSON().substr(0, 10);

    return convertDateToJson(data + " " + hour);
}

function retornaOptionsPeloArray(lista, value, text, defaultText){
    var html = "";
    if(typeof(defaultText) != 'undefined'){
        html += '<option value="">'+defaultText+'</option>';
    }
    $(lista).each(function (i, o) {
        html += '<option value="'+o[value]+'">'+o[text]+'</option>';
    });
    return html;
}

function retornaOptions(lista, value, text, defaultText) {
    var html = "";
    if (typeof (defaultText) != 'undefined') {
        html += '<option value="">' + defaultText + '</option>';
    }
    $(lista).each(function (i, o) {
        if (lista.length == 1) {
            html += '<option value="' + o[value] + '" selected>' + o[text] + '</option>';

            setTimeout(function () {
                var selectCentroCusto = $('body [data-selects-cc] select')[0];
                var selectSecao = $('body [data-selects-cc] select')[1];
                var selectCargo = $('body [data-selects-cargo] select')[0];

                if (!!selectCargo && parseInt($(selectCargo).val()) > 0) {
                    $(selectCargo).trigger('change');
                    $(selectCargo).parent().css('display', 'none');
                } else if (!!selectSecao && parseInt($(selectSecao).val()) > 0) {
                    $(selectSecao).trigger('change');
                    $(selectSecao).parent().css('display', 'none');
                } else if (!!selectCentroCusto && parseInt($(selectCentroCusto).val()) > 0) {
                    $(selectCentroCusto).trigger('change');
                }
                //.css('display', 'none');
            }, 1);
        } else {
            html += '<option value="' + o[value] + '">' + o[text] + '</option>';
        }
    });
    return html;
}

function criaHtmlSelect(titulo,options){
	return '<div class="form-group">'+
	'	<label>'+titulo+'</label>'+
	'	<select class="form-control">'+options+'</select>'+
	'</div>';
}

function dateDiff(date1, date2) {
    var dataInicial = new Date(date1);
    var dataFinal = new Date(date2);
    return (dataFinal - dataInicial) / 1000 / 60 / 60 / 24;
}