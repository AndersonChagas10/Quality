//Data string deve ser yyyy-MM-dd HH:MM:SS sendo a hora opicional
function stringToDate(dateString){

    var dia = parseInt(dateString.substr(8, 2));
    var mes = parseInt(dateString.substr(5, 2));
    var ano = parseInt(dateString.substr(0, 4));

    var hora = parseInt(dateString.substr(11, 2)) ? parseInt(dateString.substr(11, 2)) : 0;
    var segundos = parseInt(dateString.substr(14, 2)) ? parseInt(dateString.substr(14, 2)) : 0;
    var mimutos = parseInt(dateString.substr(18, 2)) ? parseInt(dateString.substr(18, 2)) : 0;

    //new Date(year, month, day, hours, minutes, seconds, milliseconds)
    return new Date(ano, mes, dia, hora, mimutos, segundos);
}

function convertDateToJson(date){

    var newDate = new Date(date);

    return new Date(newDate.setHours(newDate.getHours() - 3)).toJSON();
}