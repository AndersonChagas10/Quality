$('#btnDebug').removeClass('hide');

$(document).on('click', '#btnDebug', function (e) {
    e.preventDefault();
    showDebug();
});

$(document).on('click', '#btnHideDebug', function (e) {
    e.preventDefault();
    hideDebug();
});

$(document).on('click', '#btnClearDebug', function (e) {
    e.preventDefault();
    clearDebug();
});

function showDebug() {
    $('.App').addClass('hide');
    $('.debug').removeClass('hide');
}

function hideDebug() {
    $('.App').removeClass('hide');
    $('.debug').addClass('hide');
}

function debugMessage(message) {
    message = $('<span>[' + (dateTimeFormat()) + '] - ' + message + '</span><br />');
    appendDevice(message, $('.debugMessages'));
}

function clearDebug() {
    $('.debugMessages').empty();
}

