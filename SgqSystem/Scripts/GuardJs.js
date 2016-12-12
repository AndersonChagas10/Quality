GuardJs = {

    mascaraCampoCalculado: function (e) {
      if ($(e).val() == "0")
          $(e).val("");
      $(e).inputmask({ mask: '9{1,10}x10^9{1,5}', reverse: true, clearIfNotMatch: true });
    },
    mascaraPorcentegem:  function(e){
     
    },

    message: "One or more fields are requireds: ",

    /*
    Local variables to handle error of validation.
    */
    isValid: true,
    fieldsBlocking: [],

    /*
    Reset validatior.
    */
    resetForValidation: function () {
        GuardJs.isValid = true;
        GuardJs.fieldsBlocking = [];
        GuardJs.message = "One or more fields are requireds: ";
    },

    /*
    Verify if field with date time input have correctly date time format. 
    If has not valid information on fields, push element in GuardJs.fieldsBlocking Array.
    */
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

    },

    /*
    Verify if range date time fields input have correctly date time format. 
    If they are invalid the function will push element in GuardJs.fieldsBlocking Array and make GuardJs.isValid = false.
    */
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

    /*
    Verify if GuardJs.fieldsBlocking has elements, this conditional represents theres something worng in our immediate previous 
    validation, and for each of then (elements in array) will display message in screen.
    */
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

    /*
    Show alert message in alert style.
    */
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

    /*
    Show error message in error style.
    */
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

    /*
    Show sucess message in sucess style.
    */
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

    /*
    Hide any message of styles: error, alert, sucess.
    */
    esconderMensagem: function () {
        $('#divMensagemErro').hide();
        $('#divMensagemAlerta').hide();
        $('#divMensagemSucesso').hide();
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

$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};