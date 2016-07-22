var date = new Date();
var year = date.getFullYear();
var month = date.getMonth() + 1;
var day = date.getDate();
var minutes = date.getMinutes();
var hours = date.getHours();
var yyyymmddhhmm = ("0" + day).slice(-2) + "/" + ("0" + month).slice(-2) + "/" + year+" "+hours+":"+minutes;
$('#datetime').text(yyyymmddhhmm);


$("#btnSave").mousedown(function () {


    alert('save');

});