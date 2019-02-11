function yyyyMMdd() {
    var date = new Date();
    var c = getCollectionDate();    
    if(c){
        date.setDate(parseInt(c.slice(2, 4)));
        date.setMonth(parseInt(c.slice(0, 2)) - 1);
        date.setFullYear(parseInt(c.slice(4, 8)));
    }  

    var alterTurningDate = turningTimeCheck(date);
    if (alterTurningDate == true) {
        date = dateDecrease(date);
    }

    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var day = date.getDate();
    var yyyyMMdd = year + ("0" + month).slice(-2) + ("0" + day).slice(-2);

    return yyyyMMdd;
}
function MMddyyyy() {
    var date = new Date();
    var alterTurningDate = turningTimeCheck(date);
    if (alterTurningDate == true) {
        date = dateDecrease(date);
    }

    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var day = date.getDate();
    var MMddyyyy = ("0" + month).slice(-2) + ("0" + day).slice(-2) + year;

    return MMddyyyy;
}
function dateFormat() {
    var date = new Date();
    var alterTurningDate = turningTimeCheck(date);
    if (alterTurningDate == true) {
        date = dateDecrease(date);
    }

    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var day = date.getDate();
    var mmddyyyy = ("0" + month).slice(-2) + "/" + ("0" + day).slice(-2) + "/" + year;

    return mmddyyyy;
}

function dateFormateRetroactive(date) {
    var dateArray = date.split('/');
    return dateArray[0] + dateArray[1] + dateArray[2];
}

function turningTimeCheck(date) {

    var turningTime = $('.App').attr('turningtime');
    if (turningTime != undefined) {

        var turningTimeArray = turningTime.split(':');

        var hour = parseInt(turningTimeArray[0]);
        var minutes = parseInt(turningTimeArray[1]);

        var appHours = date.getHours();
        var appMinutos = date.getMinutes();

        if ((appHours >= 0 && appHours <= 6)) {

            if(appHours < hour)
            {
                return true;
            }
            else if(appMinutos < minutes)
            {
                return true;
            }
        }
    }
    return false;
}
function dateDecrease(date) {

    var newdate = new Date(date);
    newdate.setDate(newdate.getDate() - 1);
    date = newdate;
    return date;
}
function dateReturn() {

    var date = new Date();
    var c = getCollectionDate();
    
    if(c){
        
        date.setMonth(parseInt(c.slice(0, 2)) - 1);
        date.setFullYear(parseInt(c.slice(4, 8)));
	date.setDate(parseInt(c.slice(2, 4)));
    }    
    
    //Se tiver turno que troca no outro dia depois da meia noite verifica aqui
    var alterTurningDate = turningTimeCheck(date);
    if (alterTurningDate == true)
    {
        date = dateDecrease(date);
    }

    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var day = date.getDate();

    var data = ("0" + month).slice(-2) + ("0" + day).slice(-2) + year;
    return data;

}

function dateTimeFormat() {
    var date = new Date();

    var alterTurningDate = turningTimeCheck(date);
    if (alterTurningDate == true) {
        date = dateDecrease(date);
    }

    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var day = date.getDate();
    var hour = date.getHours();
    var minute = date.getMinutes();
    var seconds = date.getSeconds();
    var mileseconds = date.getMilliseconds();

    var mmddyyyyhhmm;
    if ($('.App').attr('retroactivedata')) {

        var retroactiveData = $('.App').attr('retroactivedata');

        month = retroactiveData.substring(0, 2);
        day = retroactiveData.substring(2, 4);
        year = retroactiveData.substring(4, 8);

        mmddyyyyhhmm = ("0" + month).slice(-2) + "/" + ("0" + day).slice(-2) + "/" + year + " " + ("0" + hour).slice(-2) + ":" + ("0" + minute).slice(-2) + ":" + ("00" + seconds).slice(-3) + ":" + ("00" + mileseconds).slice(-3);
    }
    else {
        mmddyyyyhhmm = ("0" + month).slice(-2) + "/" + ("0" + day).slice(-2) + "/" + year + " " + ("0" + hour).slice(-2) + ":" + ("0" + minute).slice(-2) + ":" + ("00" + seconds).slice(-3) + ":" + ("00" + mileseconds).slice(-3);

    }

    return mmddyyyyhhmm;
}
function dateTimeFormatCulture(date, time) {
    if ($('.App').attr('culture') == "pt-br") {
        var dia = date.substring(2, 4);
        var mes = date.substring(0, 2);
        var ano = date.substring(4, 8);

        return dia + "/" + mes + "/" + ano + " " + time;
    }
    else {
        var dia = date.substring(2, 4);
        var mes = date.substring(0, 2);
        var ano = date.substring(4, 8);

        return mes + "/" + dia + "/" + ano + " " + time;

    }
}
function dateTimeWithMinutes(relogio) {

    var date = new Date();

    var alterTurningDate = turningTimeCheck(date);
    if (alterTurningDate == true && relogio != true) {
        date = dateDecrease(date);
    }

    var hour = date.getHours();
    var minute = date.getMinutes();
    var seconds = date.getSeconds();
    var mileseconds = date.getMilliseconds();

    var time = ("0" + hour).slice(-2) + ":" + ("0" + minute).slice(-2);

    if ($('.App').attr('retroactivedata')) {

        var retroactivedata = $('.App').attr('retroactivedata');
        return dateTimeFormatCulture(retroactivedata, time);
    }
    else {

        var date = new Date();

        var alterTurningDate = turningTimeCheck(date);
        if (alterTurningDate == true && relogio != true) {
            date = dateDecrease(date);
        }

        var year = date.getFullYear();
        var month = date.getMonth() + 1;
        var day = date.getDate();
        var mmddyyyyhhmm = ("0" + month).slice(-2) + ("0" + day).slice(-2) + year;

        return dateTimeFormatCulture(mmddyyyyhhmm, time);
    }
}

function dateTimeWithMinutesBR(relogio) {

    var date = new Date();

    var alterTurningDate = turningTimeCheck(date);
    if (alterTurningDate == true && relogio != true) {
        date = dateDecrease(date);
    }

    var hour = date.getHours();
    var minute = date.getMinutes();
    var seconds = date.getSeconds();
    var mileseconds = date.getMilliseconds();

    var time = ("0" + hour).slice(-2) + ":" + ("0" + minute).slice(-2);

    if ($('.App').attr('retroactivedata')) {

        var retroactivedata = $('.App').attr('retroactivedata');
        return dateTimeFormatCulture(retroactivedata, time);
    }
    else {

        var date = new Date();

        var alterTurningDate = turningTimeCheck(date);
        if (alterTurningDate == true && relogio != true) {
            date = dateDecrease(date);
        }

        var year = date.getFullYear();
        var month = date.getMonth() + 1;
        var day = date.getDate();
        var mmddyyyyhhmm = ("0" + day).slice(-2) + ("0" + month).slice(-2) + year;

        return dateTimeFormatCulture(mmddyyyyhhmm, time);
    }
}

///Retorna a data convertida para comparação sem caracteres especiais
function convertDate(date) {
    var dateArray = date.split('/');

    if ($('.App').attr('culture') == "pt-br") {
        var dia = dateArray[0];
        var mes = dateArray[1];
        var ano = dateArray[2];
    }
    else {
        var dia = dateArray[1];
        var mes = dateArray[0];
        var ano = dateArray[2];
    }
    return mes + "/" + dia + "/" + ano;
}

function toDateTime(datetime){
    if(datetime){
        var day = parseInt(datetime.substring(0, 2));
        var month = parseInt(datetime.substring(3,5)) - 1;
        var year = parseInt(datetime.substring(6,10));

         return new Date(year, month, day);
    }else{
        return null;
    }
}

//data formato mm/dd/yyyy
function dateFormat() {

    var date = new Date();

    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var day = date.getDate();
    var mmddyyyy = ("0" + month).slice(-2) + "/" + ("0" + day).slice(-2) + "/" + year;

    return mmddyyyy;
}

//function saveLevel01(Level01Id, date, shift, period, totalSets, totalSides, atualSet, atualSide, totalErros) {
//    return "<div class='level01Result' level01Id='" + Level01Id + "' date='" + date + "' shift='" + shift + "' period='" + period + "' totalSets='" + totalSets + "' totalSides='" + totalSide + "' atualSet='" + atualSet + "' atualSide='" + atualSide + "' totalerros='" + totalErros + "'></div>";
//}
function getCollectionDate() {
    if ($('.App').attr('retroactivedata')) {
        return $('.App').attr('retroactivedata');
    }
    else {
        return $('.App').attr('date');
    }
}

function getCollectionDateFormat() {
    var date = new Date();
    var c = getCollectionDate();
    
    if(c){
        date.setDate(parseInt(c.slice(2, 4)));
        date.setMonth(parseInt(c.slice(0, 2)) - 1);
        date.setFullYear(parseInt(c.slice(4, 8)));
    }   

    return date;
}

function dateDataBaseConvert(date) {
    var month = date.substring(0, 2);
    var day = date.substring(2, 4);
    var year = date.substring(4, 8);

    return year + month + day;
}

function validDateInputDate(date) {
    if (date) {

        var dia = parseInt(date[3] + "" + date[4]);
        var mes = parseInt(date[0] + "" + date[1]);
        var ano = parseInt(date[6] + "" + date[7] + "" + date[8] + "" + date[9]);

        if ($('.App').attr('culture') == "pt-br") {
            dia = parseInt(date[0] + "" + date[1]);
            mes = parseInt(date[3] + "" + date[4]);
        }

        var date = new Date(mes+"/"+dia+"/"+ano); //mm/dd/yyyy
        
        if (date.getDate() != dia) {
            return false;
        }else{
            return true;
        }
    }

    return false;

}

//Format String = "09/05/2016 11:17".
function stringToTime(string) {
    var d1 = string.replace(/[/]/g, '').replace(/[:]/g, '').replace(/[ ]/g, '');
    return parseInt(d1);
}

function dateInternacionalFormat(date){    
    return (new Date(date)).toLocaleDateString("pt-BR")
}

