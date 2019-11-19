//append adaptado para windows
function appendDevice(obj, appendTo) {
    // var platform = device.platform;

    // if (platform == 'windows') {
    //     MSApp.execUnsafeLocalFunction(function () {
    //         $(appendTo).append($(obj));
    //     });
    // }
    // else {
    //     $(appendTo).append($(obj));
    // }

    $(appendTo).append($(obj));
}

//prepend adaptado para windows
function prependDevice(obj, prependTo) {
    // var platform = device.platform;

    // if (platform == 'windows') {
    //     MSApp.execUnsafeLocalFunction(function () {
    //         $(prependTo).prepend($(obj));
    //     });
    // } else {
    //     $(prependTo).prepend($(obj));
    // }

    $(prependTo).prepend($(obj));
}

//replace adaptado para windows
function replaceWithDevice(obj, replace) {
    // var platform = device.platform;

    // if (platform == 'windows') {
    //     MSApp.execUnsafeLocalFunction(function () {
    //         replace.replaceWith(obj);
    //     });
    // }
    // else {
    //     replace.replaceWith(obj);
    // }

    replace.replaceWith(obj);
}

//after adaptado para windows
function afterDevice(obj, afterTo) {
    // var platform = device.platform;

    // if (platform == 'windows') {
    //     MSApp.execUnsafeLocalFunction(function () {
    //         afterTo.after(obj);
    //     });
    // }
    // else {
    //     afterTo.after(obj);
    // }

    afterTo.after(obj);
}

//before adaptado para windows
function beforeDevice(obj, beforeTo) {
    // var platform = device.platform;

    // if (platform == 'windows') {
    //     MSApp.execUnsafeLocalFunction(function () {
    //         beforeTo.before(obj);
    //     });
    // }
    // else {
    //     beforeTo.before(obj);
    // }

    beforeTo.before(obj);
}

//limpeza da tag body adaptado para windows
function bodyEmpty() {
    var extendedSplashScreen = $('.extendedSplashScreen');
    if (extendedSplashScreen.length == 0) {
        extendedSplashScreen = $('.msgSync').not('.overlay'); //pega a mensagem de atualização para a tela não ficar em branco
    }
    $('body').empty();
    appendDevice(extendedSplashScreen, $('body'));
}


function getFrequency(frequency) {

    var range = [];
    var startDate = new Date(insertAt(insertAt(getCollectionDate(), 2, '-'), 5, '-'));
    var endDate = new Date(insertAt(insertAt(getCollectionDate(), 2, '-'), 5, '-'));

    var today = new Date(insertAt(insertAt(getCollectionDate(), 2, '-'), 5, '-'));
    var week = today.getDay();

    var firstDayOfMonth = new Date(today.getFullYear(), today.getMonth(), 1);
    var lastDayOfMonth = new Date(today.getFullYear(), today.getMonth() + 1, 0);


    switch (frequency) {
        case 3: // Diário
            break;
        case 4: // Semanal
            startDate = new Date(today.setDate(today.getDate() - today.getDay()) - week);
            endDate = new Date(today.setDate(today.getDate() - today.getDay() + 6 - week));
            break;
        case 5: // Quinzenal
            if (today.getDate() <= 15) {
                startDate = firstDayOfMonth;
                endDate = new Date(today.getFullYear(), today.getMonth(), 15);
            } else {
                startDate = new Date(today.getFullYear(), today.getMonth(), 16);;
                endDate = lastDayOfMonth;
            }

            break;
        case 6: // Mensal
            startDate = firstDayOfMonth;
            endDate = lastDayOfMonth;
            break;
    }

    startDate.setHours(0, 0, 0, 0);
    endDate.setHours(23, 59, 59, 999);

    range.push(startDate);
    range.push(endDate);

    return range;
}

function isInsideFrequency(currentDate, currentFrequency, effective, period, shift, periodCount, shiftCount) {


    effective = (effective == undefined ? 0 : effective);
    period = (period == undefined ? 0 : period);
    shift = (shift == undefined ? 0 : shift);
    periodCount = (periodCount == undefined ? 0 : periodCount);
    shiftCount = (shiftCount == undefined ? 0 : shiftCount);


    if (!currentDate || typeof currentDate === 'string' || !currentDate instanceof Date) {
        console.log("isInsideFrequency function error: invalid Date");
        return false;
    }

    var startDate = getFrequency(currentFrequency)[0];
    var endDate = getFrequency(currentFrequency)[1];

    var totalPeriod = 4;
    var totalShift = 2;
    var currentPeriod = parseInt($('.App').attr('period'));
    var currentShift = parseInt($('.App').attr('shift'));

    var diff = diffDate(startDate, endDate);
    if (diff < 1) {
        diff = 0;
    }

    if (currentFrequency == 1) {// Por período
        if (effective >= periodCount) {
            return true;
        } else {
            return false;
        }
    }
    else if (currentFrequency == 2) {// Por turno
        if (effective > shiftCount) {
            return true;
        } else {
            return false;
        }
    }

    if (currentDate >= startDate && currentDate <= endDate) {
        return true;
    } else {
        return false;
    }
}

function diffDate(date1, date2) {
    return Math.abs(date2 - date1) / 1000 / 60 / 60 / 24;
}

function insertAt(word, index, string) {
    if (word)
        return word.substr(0, index) + string + word.substr(index);
    else
        return '';
}

function MakeObject2(element, ListName, objectReturn) {
    objectReturn[ListName] = [];
    var elemens = element;
    $.each(elemens, function (counter, object) {

        if ($(object).attr('sync') == "true") { return; }

        if (object == undefined) { return; }

        if ($(object).is('div') == false) { return; }

        var temp = {};
        var el = object;
        for (var i = 0, atts = el.attributes, n = atts.length, arr = []; i < n; i++) {
            var name = atts[i].nodeName;
            var value = atts[i].nodeValue;
            if (value == 'undefined' || value == 'null') { continue; }
            temp[name] = value;
        }
        objectReturn[ListName].push(temp);
        if (!!object.childNodes) {
            if (object.childNodes.length > 0)
                MakeObject2(object.childNodes, 'next' + ListName, temp);
        }
    });
}

var keyUpNumericInProcess = false;

$(document).on('keydown', 'input.numeric', function (event) {
    if(keyUpNumericInProcess){
        event.preventDefault();
    }else{
        keyUpNumericInProcess = true;
    }
});

$(document).on('keyup', 'input.numeric', function (event) {
    ReplaceNegative(event, this);
});

function ReplaceNegative(event, element) {
	
    var $$this = $(element).val().replace(',','.');
	
    var isNumber = !isNaN($$this);
	
	if(!isNumber && ($$this != "-")){
		$(element).val($(element).attr('data-lastvalue'));
	}else{
		$(element).attr('data-lastvalue',$$this)
	}

    keyUpNumericInProcess = false;
}


// function validateNumber(event) {
//     var key = window.event ? event.keyCode : event.which;
//     if (event.keyCode === 8 || event.keyCode === 44) {
//         if(event.target.value.indexOf(',') == -1)
//             return true;
//         else
//             return false;

//         return true;
//     } else if ( key < 48 || key > 57 ) {
//         return false;
//     } else {
//         return true;
//     }
// };

// function validateVirgula(event) {    
//     var key = window.event ? event.keyCode : event.which;
//     if(event.target.value.indexOf(',') == event.target.value.length -1){
//         $(event.target).val($(event.target).val().replace(',', ''))
//     }      
// }