var Utils = {
    GerarData: function () {
        var date = new Date();
        var year = date.getFullYear();
        var month = date.getMonth() + 1;
        var day = date.getDate();
        var minutes = date.getMinutes();
        var seconds = date.getSeconds();
        var hours = date.getHours();
        var yyyymmddhhmm = ("0" + day).slice(-2) + "/" + ("0" + month).slice(-2) + "/" + year + " " + ("0" + hours).slice(-2) + ":" + ("0" + minutes).slice(-2) + ":" + ("0" + seconds).slice(-2);
        return yyyymmddhhmm;
    },
};

var storage = window.localStorage;

var idAcaoCorretiva = 0;

var idSlaughterLogado = 0;
var idTechinicalLogado = 0;
var timeSlaughterLogado = null;
var timeTechinicalLogado = null;
var dateExecute = Utils.GerarData();
var dateStart = Utils.GerarData();

$("#period").text(storage.getItem("periodo"));
$("#auditor").text(storage.getItem("userId"));
$("#datetime").text(dateExecute);
$("#starttime").text(dateStart);
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
                timeSlaughterLogado = Utils.GerarData();
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
                timeTechinicalLogado = Utils.GerarData();
            }
        });

    },

    enviarAcaoCorretiva: function () {

            var obj = {
                CorrectiveAction: {
                    Id: idAcaoCorretiva,
                    DateExecuteFarmatado: dateExecute,
                    Auditor: storage.getItem("userId"),
                    Shift: 1,
                    AuditLevel1: storage.getItem("indicatorId"),
                    AuditLevel2: storage.getItem("indicatorId"),
                    AuditLevel3: storage.getItem("indicatorId"),
                    StartTimeFarmatado: dateStart,
                    Period: storage.getItem("periodo"),
                    DescriptionFailure: $("#DescriptionFailure").val(),
                    ImmediateCorrectiveAction: $("#ImmediateCorrectiveAction").val(),
                    ProductDisposition: $("#ProductDisposition").val(),
                    PreventativeMeasure: $("#PreventativeMeasure").val(),
                    Slaughter: idSlaughterLogado,
                    NameSlaughter: $("#slaughter").val(),
                    DateTimeSlaughterFarmatado: timeSlaughterLogado,
                    Techinical: idTechinicalLogado,
                    NameTechinical: $("#techinical").val(),
                    DateTimeTechinicalFarmatado: timeTechinicalLogado
                }
            };

            console.log(obj);

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

                console.log(data);

                if (data != null) {

                    if (data.Id != 0) {

                        idAcaoCorretiva = data.Id;

                        dateExecute = data.DateExecuteFarmatado;
                        $("#datetime").text(dateExecute);

                        dateStart = data.StartTimeFarmatado;
                        $("#starttime").text(dateStart);

                        storage.setItem("userId", data.Auditor);
                        $("#auditor").text(data.Auditor);

                        $("#shift").text('1');

                        storage.setItem("indicatorId", data.AuditLevel1);
                        storage.setItem("indicatorId", data.AuditLevel2);
                        storage.setItem("indicatorId", data.AuditLevel3);

                        storage.setItem("periodo", data.Period);
                        $("#period").text(data.Period);

                        $("#DescriptionFailure").val(data.DescriptionFailure);
                        $("#ImmediateCorrectiveAction").val(data.ImmediateCorrectiveAction);
                        $("#ProductDisposition").val(data.ProductDisposition);
                        $("#PreventativeMeasure").val(data.PreventativeMeasure);

                        idSlaughterLogado = data.Slaughter;
                        timeSlaughterLogado = data.DateTimeSlaughterFarmatado;
                        $("#slaughterDatetime").val(timeSlaughterLogado);
                        $("#slaughter").val(data.NameSlaughter);


                        idTechinicalLogado = data.Techinical;
                        timeTechinicalLogado = data.DateTimeTechinicalFarmatado;
                        $("#techinicalDatetime").val(timeTechinicalLogado);
                        $("#techinical").val(data.NameTechinical);

                    }
                }
            }
        });

    },

};

AcaoCorretiva.verificarAcaoCorretivaIncompleta();