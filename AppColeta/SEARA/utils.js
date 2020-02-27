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
    return isNaN(valor) ? 0 : valor;
}

function UmSeForNaN(valor){
    return isNaN(valor) ? 1 : valor;
}

function UmSeForNaNOuNull(valor){
    return isNaN(valor) || valor == null ? 1 : valor;
}

function TracoSeForNaN(valor){
    return isNaN(valor) ? '-' : valor;
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