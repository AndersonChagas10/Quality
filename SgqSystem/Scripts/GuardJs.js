GuardJs = {

    message: "One or more fields are requireds: ",

    isValid: true,

    fieldsBlocking: [],

    resetForValidation: function () {
        GuardJs.isValid = true;
        GuardJs.fieldsBlocking = [];
        GuardJs.message = "One or more fields are requireds: ";
    },

    CheckSingleDateTime: function (dateTimeInString, message) {

        if (dateTimeInString == undefined) {
            GuardJs.fieldsBlocking.push(message);
        }
        else if (dateTimeInString == null) {
            GuardJs.fieldsBlocking.push(message);
        }
        else if (dateTimeInString == NaN) {
            GuardJs.fieldsBlocking.push(message);
        }
        else if (dateTimeInString.length < 10) {
            GuardJs.fieldsBlocking.push(message);
        }
        //else {
        //    if (GuardJs.isMessagemJaExibida()) {

        //    } else {

        //    }
        //}

    },

    CheckRangeDateTime: function (dateTimeInStringInit, dateTimeInStringEnd, nameForInputDateInit, nameForInputDateEnd, messageOptional) {

        if (dateTimeInStringInit == undefined || dateTimeInStringEnd == undefined) {

            if (dateTimeInStringInit == undefined && dateTimeInStringEnd == undefined) {
                GuardJs.fieldsBlocking.push(nameForInputDateInit);
                GuardJs.fieldsBlocking.push(nameForInputDateEnd);
            } else if (dateTimeInStringInit == undefined) {
                GuardJs.fieldsBlocking.push(nameForInputDateInit);
            } else {
                GuardJs.fieldsBlocking.push(nameForInputDateEnd);
            }

            //GuardJs.exibirMensagemAlerta(message);
            GuardJs.isValid = false;
        }
        else if (dateTimeInStringInit == null || dateTimeInStringEnd == null) {

            if (dateTimeInStringInit == null && dateTimeInStringEnd == null) {
                GuardJs.fieldsBlocking.push(nameForInputDateInit);
                GuardJs.fieldsBlocking.push(nameForInputDateEnd);
            } else if (dateTimeInStringInit == null) {
                GuardJs.fieldsBlocking.push(nameForInputDateInit);
            } else {
                GuardJs.fieldsBlocking.push(nameForInputDateEnd);
            }

            //GuardJs.exibirMensagemAlerta(message);
            GuardJs.isValid = false;

        }
        else if (dateTimeInStringInit == NaN || dateTimeInStringEnd == NaN) {

            if (dateTimeInStringInit == NaN && dateTimeInStringEnd == NaN) {
                GuardJs.fieldsBlocking.push(nameForInputDateInit);
                GuardJs.fieldsBlocking.push(nameForInputDateEnd);
            } else if (dateTimeInStringInit == NaN) {
                GuardJs.fieldsBlocking.push(nameForInputDateInit);
            } else {
                GuardJs.fieldsBlocking.push(nameForInputDateEnd);
            }

            //GuardJs.exibirMensagemAlerta(message);
            GuardJs.isValid = false;

        }
        else if (dateTimeInStringInit.length < 10 || dateTimeInStringEnd.length < 10) {

            if (dateTimeInStringInit.length < 10 && dateTimeInStringEnd.length < 10) {
                GuardJs.fieldsBlocking.push(nameForInputDateInit);
                GuardJs.fieldsBlocking.push(nameForInputDateEnd);
            } else if (dateTimeInStringInit.length < 10) {
                GuardJs.fieldsBlocking.push(nameForInputDateInit);
            } else {
                GuardJs.fieldsBlocking.push(nameForInputDateEnd);
            }

            //if (dateTimeInStringInit.length < 10 && dateTimeInStringEnd.length < 10) {
            //    if (dateTimeInStringInit.length > 1 && dateTimeInStringEnd.length > 1)
            //    GuardJs.fieldsBlocking.push(nameForInputDateInit);
            //    GuardJs.fieldsBlocking.push(nameForInputDateEnd);
            //} else if (dateTimeInStringInit.length < 10) {
            //    GuardJs.fieldsBlocking.push(nameForInputDateInit);
            //} else {
            //    GuardJs.fieldsBlocking.push(nameForInputDateEnd);
            //}

            GuardJs.isValid = false;

        }

        GuardJs.ExecuteValidationError();

    },

    ExecuteValidationError: function () {
        if (GuardJs.fieldsBlocking.length > 0) {
            GuardJs.fieldsBlocking.forEach(function (o, c) {
                if (c == 0) {
                    GuardJs.message += '"' + o + '"';
                } else if (c > 0 && c < GuardJs.fieldsBlocking.length - 1) {
                    GuardJs.message += ', "' + o + '"';
                } else if (c == GuardJs.fieldsBlocking.length - 1) {
                    GuardJs.message += ', "' + o + '".';
                }
            });
            GuardJs.exibirMensagemAlerta(GuardJs.message);
        } else {
            GuardJs.esconderMensagem();
        }
    },

    isMessagemJaExibida: function () {

    },

    exibirMensagemAlerta: function (mensagem, url, container) {
        var page = $("html, body");
        GuardJs.esconderMensagem();
        $('#divMensagemAlerta').hide().find('span').text('');
        if (url == undefined || url.length == 0) {
            container = container || '';
            var $divMensagem = $(container + '#divMensagemAlerta');
            $divMensagem.find('span').text(mensagem);
            $divMensagem.show();
        } else {
            alert(mensagem);
            location.href = url;
        }
        $('html,body').animate({ scrollTop: 0 }, 'slow');
    },

    exibirMensagemErro: function (mensagem, url, container) {
        GuardJs.esconderMensagem();
        $('#divMensagemErro').hide().find('span').text('');
        if (url == undefined || url.length == 0) {
            container = container || '';
            var $divMensagem = $(container + '#divMensagemErro');
            $divMensagem.find('#mensagemErro').text(mensagem);
            $divMensagem.show();
        } else {
            alert(mensagem);
            location.href = url;
        }
        $('html,body').animate({ scrollTop: 0 }, 'slow');
    },

    esconderMensagem: function () {
        $('#divMensagemErro').hide();
        $('#divMensagemAlerta').hide();
        $('#divMensagemSucesso').hide();
    },

    exibirMensagemSucesso: function (mensagem, url, container) {
        if (mensagem == undefined || mensagem.length == 0) {
            GuardJs.esconderMensagem();
        } else {
            $('#divMensagemSucesso').hide().find('span').text('');
            if (url == undefined || url.length == 0) {
                container = container || '';
                var $divMensagem = $(container + '#divMensagemSucesso');
                $divMensagem.find('span').text(mensagem);
                $divMensagem.show();
            } else {
                alert(mensagem);
                location.href = url;
            }
            $('html,body').animate({ scrollTop: 0 }, 'slow');
        }
    },

    parseDateTimeWithMinutes: function () {
        var date = new Date();
        var year = date.getFullYear();
        var month = date.getMonth() + 1;
        var day = date.getDate();
        var hour = date.getHours();
        var minute = date.getMinutes();
        var seconds = date.getSeconds();
        var mileseconds = date.getMilliseconds();
        var mmddyyyyhhmm = ("0" + month).slice(-2) + "/" + ("0" + day).slice(-2) + "/" + year + " " + ("0" + hour).slice(-2) + ":" + ("0" + minute).slice(-2);

        return mmddyyyyhhmm;
    },

}