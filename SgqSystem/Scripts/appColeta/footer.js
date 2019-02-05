var online = true;

function offLine() {
    online = false;
    $('.footer .status').addClass('offline').removeClass('online').removeClass('databaseoffline').attr('title', "Off-Line");
}

function onLine() {
    online = true;
    $('.footer .status').addClass('online').removeClass('offline').removeClass('databaseoffline').attr('title', "On-Line");
}

function noDataBase() {
    $('.footer .status').addClass('databaseoffline').removeClass('offline').removeClass('online').attr('title', "Data Base Off-Line");
}

$(document).on('click', '.footer', function (e) {
    addPCC1BSequence('1');
});