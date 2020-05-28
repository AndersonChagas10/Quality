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

function criaHtmlSelect(titulo,options){
	return '<div class="form-group">'+
	'	<label>'+titulo+'</label>'+
	'	<select class="form-control">'+options+'</select>'+
	'</div>';
}

function ZeroSeForNaN(valor){
    return parseFloat(isNaN(valor) || valor == null ? 0 : parseFloat(valor).toFixed(2));
}

function UmSeForNaN(valor){
    return parseFloat(isNaN(valor) ? 1 : parseFloat(valor).toFixed(2));
}

function UmSeForNaNOuNull(valor){
    return parseFloat(isNaN(valor) || valor == null ? 1 : parseFloat(valor).toFixed(2));
}

function TracoSeForNaN(valor){
    return parseFloat(isNaN(valor) ? '-' : parseFloat(valor).toFixed(2));
}

function serializeFormToObject(divId){

    var obj = {};

    var inputs = $(divId).find('input, select');

    $(inputs).each(function(){
        
        var id = $(this).attr('id');

        if ($(this).prop('type') == 'checkbox' || $(this).prop('type') == 'radio') {
            obj[id].push($(this).prop('checked'));

        } else {
            obj[id] = (this.value || '');
        }

    });

    return obj;

}

function setObjectToForm(objForm) {

    Object.keys(objForm).forEach(function (key) {

        var input = $(document).find('[id=' + key + ']');

        if ($(input).prop('type') == 'checkbox' || $(input).prop('type') == 'radio') {
            $(input).attr('checked', objForm[key]);

        } else {
            $(input).val(objForm[key]);
        }

    });
}

function disableHeaderFields(objForm) {

    Object.keys(objForm).forEach(function (key) {

        var input = $(document).find('[id=' + key + ']');

        $(input).prop('disabled', true);

    });

}

function hederFieldIsValid(formId) {

    var inputs = $(formId).find('input, select');

    var isValid = true;

    $(inputs).each(function () {

        setInputBackGroundColorNone(this);

        if ($(this).prop('type') == 'checkbox' || $(this).prop('type') == 'radio') {

            if ($(this).prop('checked') === 'false') {
                setInputBackGroundColorRed(this);
                isValid = false;
            }
                
        } else {

            if (this.value === null || this.value === undefined || this.value === ""){
                setInputBackGroundColorRed(this);
                isValid = false;
            }

        }
    });

    return isValid;
}

function setInputBackGroundColorRed(input) {

    $(input).css('background-color', '#ffeded');

}

function setInputBackGroundColorNone(input) {

    $(input).css('background-color', '');

}

function getBotaoBuscar() {
    var botaoBuscar = '<div class="pull-right">                     ' +
        '                  <label style="padding-right:5px">Buscar</label> ' +
        '                  <input type="text" onkeyup="buscarItemNaLista(this)" style="height:35px;"/>' +
        '              </div>';
    return botaoBuscar;
}

function buscarItemNaLista(input) {
    $('button.list-group-item').each(function (i, o) {
        var mostrarItem = $(o).text().toLowerCase().includes($(input).val().toLowerCase());
        if (mostrarItem) {
            $(o).show();
        } else {
            $(o).hide();
        }
    })
    if ($('button.list-group-item:visible').length == 0) {
        if ($('span.list-group-item').length == 0) {
            $('.list-group').append("<span class='list-group-item col-xs-12 text-center'>Nenhum resultado encontrado com o termo digitado.</span>");
        } else {
            $('span.list-group-item').show();
        }
    } else {
        if ($('span.list-group-item').length > 0) {
            $('span.list-group-item').hide();
        }
    }
}
