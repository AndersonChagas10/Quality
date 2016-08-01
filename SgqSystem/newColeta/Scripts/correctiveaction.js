var Utils = {
    GerarData: function () {
        var date = new Date();
        var year = date.getFullYear();
        var month = date.getMonth() + 1;
        var day = date.getDate();
        var minutes = date.getMinutes();
        var seconds = date.getSeconds();
        var hours = date.getHours();
        //var ddMMyyyyhhmmss = ("0" + day).slice(-2) + "/" + ("0" + month).slice(-2) + "/" + year + " " + ("0" + hours).slice(-2) + ":" + ("0" + minutes).slice(-2) + ":" + ("0" + seconds).slice(-2);
        // return ddMMyyyyhhmmss;
        var MMddyyyyhhmmss = ("0" + month).slice(-2) + "/" + ("0" + day).slice(-2) + "/" + year + " " + ("0" + hours).slice(-2) + ":" + ("0" + minutes).slice(-2) + ":" + ("0" + seconds).slice(-2);
        return MMddyyyyhhmmss;
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

$("#auditText").text(storage.getItem("indicatorName"));

var AcaoCorretiva = {

    abrirModalLoginSlaughter: function () {
        var $temp = $('#modalLoginSlaughter');
        $temp.modal();
    },

    abrirModalLoginTecnical: function () {
        var $temp = $('#modalLoginTechinical');
        $temp.modal();
    },

    logarUsuarioSlaughter: function () {

        Geral.esconderMensagem("#modalLoginSlaughter");

        var obj = {
            SlaughterPassword: $("#slaughterPassword").val(),
            SlaughterLogin: $("#slaughterLogin").val()
        };

        $.ajax({
            data: obj,
            url: '../' + '../api/CorrectiveAction/LogarUsuarioSlaughter',
            type: 'POST',
            success: function (data) {
                if (data.Mensagem != null) {
                    Geral.exibirMensagemAlerta(data.Mensagem, "", "#modalLoginSlaughter");
                }
                else {
                    idSlaughterLogado = data.Retorno.Id
                    $("#slaughter").val(data.Retorno.Name);
                    $("#slaughterDatetime").val(Utils.GerarData());
                    timeSlaughterLogado = Utils.GerarData();
                    var $temp = $('#modalLoginSlaughter');
                    $temp.modal('hide');

                }
            }
        });

    },

    logarUsuarioTechnical: function () {

        Geral.esconderMensagem("#modalLoginTechinical");

        var obj = {
            TechnicalPassword: $("#techinicalPassword").val(),
            TechnicalLogin: $("#techinicalLogin").val()
        };

        $.ajax({
            data: obj,
            url: '../' + '../api/CorrectiveAction/LogarUsuarioTechnical',
            type: 'POST',
            success: function (data) {
                if (data.Mensagem != null) {
                    Geral.exibirMensagemAlerta(data.Mensagem, "", "#modalLoginTechinical");
                }
                else {
                    idTechinicalLogado = data.Retorno.Id;
                    $("#techinical").val(data.Retorno.Name);
                    $("#techinicalDatetime").val(Utils.GerarData());
                    timeTechinicalLogado = Utils.GerarData();
                    var $temp = $('#modalLoginTechinical');
                    $temp.modal('hide');
                }
            }
        });

    },

    enviarAcaoCorretiva: function () {

        Geral.esconderMensagem("#correctiveActionModal");

        var listDefects = new Array();

        var level01Id = parseInt($('.level01.selected').attr('id'));

        if (!$('.level01.selected').length) {

            level01Id = parseInt($('.btnCorrectiveAction.selected').parents('.row').children('.level01').attr('id'));
        }

        $('.level02List .level02Group[level01id=' + level01Id + '] .level02[limitexceeded]').each(function (e) {

            var level02 = $(this);
            var level02Id = parseInt(level02.attr('id'));
            var level02Name = level02.children('span').text();

            $('.level03Group[level01id=' + level01Id + '] .level03').each(function (e) {

                var level03 = $(this);
                var level02errorlimit = parseInt(level02.attr('levelerrorlimit'));
                var level03Defects = level02.attr('level03' + level03.attr('id'));
                var level03Id = parseInt(level03.attr('id'));
                var level03Name = level03.children('.row').children('div').html();


                if (level03.children('.row').children('div').children('span.response').length) {
                    if (level03Defects == 1) {
                        level03Defects = 0
                    }
                    else {
                        level03Defects = 1;
                    }
                }

                if (level03Defects >= level02errorlimit && level03Defects > 0) {

                    var tempDefects = {
                        CorrectiveActionId: 0,
                        AuditLevel01Id: level01Id,
                        AuditLevel02Id: level02Id,
                        AuditLevel03Id: level03Id,
                        Defects: level03Defects
                    };

                    listDefects.push(tempDefects);

                }
            });
        });




        var obj = {
            Conectado: navigator.onLine, 
            CorrectiveAction: {
                Id: idAcaoCorretiva,
                DateExecuteFarmatado: dateExecute,
                AuditorId: 2,
                ShiftId: 1,
                AuditLevel01Id: level01Id,
                StartTimeFarmatado: dateStart,
                PeriodId: 1,
                DescriptionFailure: $("#DescriptionFailure").val(),
                ImmediateCorrectiveAction: $("#ImmediateCorrectiveAction").val(),
                ProductDisposition: $("#ProductDisposition").val(),
                PreventativeMeasure: $("#PreventativeMeasure").val(),
                SlaughterId: idSlaughterLogado,
                NameSlaughter: $("#slaughter").val(),
                DateTimeSlaughterFarmatado: timeSlaughterLogado,
                TechinicalId: idTechinicalLogado,
                NameTechinical: $("#techinical").val(),
                DateTimeTechinicalFarmatado: timeTechinicalLogado,
                CorrectiveActionLevels: listDefects
            }
        };

        // console.log(obj);

        $.ajax({
            data: obj,
            url: '../' + '../api/CorrectiveAction/SalvarAcaoCorretiva',
            type: 'POST',
            success: function (data) {
                if (data.Mensagem != null) {
                    Geral.exibirMensagemAlerta(data.Mensagem, "", "#correctiveActionModal");
                }
                else {
                    window.location.href = '/newColeta/Index.html';
                }
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

                // console.log(data);

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

//AcaoCorretiva.verificarAcaoCorretivaIncompleta();