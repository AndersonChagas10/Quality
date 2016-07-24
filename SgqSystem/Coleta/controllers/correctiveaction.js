var Utils = {
    GerarData: function () {
        var date = new Date();
        var year = date.getFullYear();
        var month = date.getMonth() + 1;
        var day = date.getDate();
        var minutes = date.getMinutes();
        var hours = date.getHours();
        var yyyymmddhhmm = ("0" + day).slice(-2) + "/" + ("0" + month).slice(-2) + "/" + year + " " + hours + ":" + minutes;
        return yyyymmddhhmm;
    },
};

var storage = window.localStorage;

var idSlaughterLogado = 0;
var idTechinicalLogado = 0;
var timeSlaughterLogado = null;
var timeTechinicalLogado = null;

$("#period").text(storage.getItem("periodo"));
$("#auditor").text(storage.getItem("userId"));
$("#datetime").text(Utils.GerarData());
$("#starttime").text(Utils.GerarData());
$("#shift").text('1');


$("#auditText").text(storage.getItem("indicatorName"));

var AcaoCorretiva = {

    logarUsuarioSlaughter: function () {

        var obj = {
            SlaughterPassword: $("#slaughterPassword").val(),
            SlaughterLogin: $("#slaughterLogin").val()
        };

        $.ajax({
            data: obj,
            url: '../' + '../api/CorrectiveAction/LogarUsuarioSlaughter',
            type: 'POST',
            success: function (data) {
                idSlaughterLogado = data.Id
                $("#slaughter").val(data.Name);
                $("#slaughterDatetime").val(Utils.GerarData());
                timeSlaughterLogado = new Date().toUTCString();
            }
        });

    },

    logarUsuarioTechnical: function () {

        var obj = {
            TechnicalPassword: $("#techinicalPassword").val(),
            TechnicalLogin: $("#techinicalLogin").val()
        };

        $.ajax({
            data: obj,
            url: '../' + '../api/CorrectiveAction/LogarUsuarioTechnical',
            type: 'POST',
            success: function (data) {
                idTechinicalLogado = data.Id;
                $("#techinical").val(data.Name);
                $("#techinicalDatetime").val(Utils.GerarData());
                timeTechinicalLogado = new Date().toUTCString();
            }
        });

    },

    enviarAcaoCorretiva: function () {
            var obj = {
                CorrectiveAction: {
                    DateExecute: new Date().toUTCString(),
                    Auditor: storage.getItem("userId"),
                    Shift: 1,
                    AuditLevel1: storage.getItem("indicatorId"),
                    AuditLevel2: storage.getItem("indicatorId"),
                    AuditLevel3: storage.getItem("indicatorId"),
                    StartTime: new Date().toUTCString(),
                    Period: storage.getItem("periodo"),
                    DescriptionFailure: $("#DescriptionFailure").val(),
                    ImmediateCorrectiveAction: $("#ImmediateCorrectiveAction").val(),
                    ProductDisposition: $("#ProductDisposition").val(),
                    PreventativeMeasure: $("#PreventativeMeasure").val(),
                    Slaughter: idSlaughterLogado,
                    DateTimeSlaughter: timeSlaughterLogado,
                    Techinical: idTechinicalLogado,
                    DateTimeTechinical: timeTechinicalLogado
                }
            };

        $.ajax({
            data: obj,
            url: '../' + '../api/CorrectiveAction/SalvarAcaoCorretiva',
            type: 'POST',
            success: function (data) {
                alert(data);
            }
        });

    },

    verificarAcaoCorretivaIncompleta: function () {

        var obj = {
            CorrectiveAction: {
                Auditor: storage.getItem("userId"),
                Shift: 1,
                AuditLevel1: storage.getItem("indicatorId"),
                AuditLevel2: storage.getItem("indicatorId"),
                AuditLevel3: storage.getItem("indicatorId"),
                Period: storage.getItem("periodo")
            }
        };

        $.ajax({
            data: obj,
            url: '../' + '../api/CorrectiveAction/VerificarAcaoCorretivaIncompleta',
            type: 'POST',
            success: function (data) {
                alert("implementar retorno");
                alert("implementar tratamento de erro");
            }
        });

    },

};


AcaoCorretiva.verificarAcaoCorretivaIncompleta();