  
/*
Retorna apenas elementos unicos do array
*/
function onlyUnique(value, index, self) {
    return self.indexOf(value) === index;
}


/*API de SUM para DataTable

Exemplo: 

// Simply get the sum of a column
  var table = $('#example').DataTable();
  table.column( 3 ).data().sum();

  // Insert the sum of a column into the columns footer, for the visible
  // data on each draw
  $('#example').DataTable( {
    drawCallback: function () {
      var api = this.api();
      $( api.table().footer() ).html(
        api.column( 4, {page:'current'} ).data().sum()
      );
    }
  } );

*/
jQuery.fn.dataTable.Api.register('sum()', function () {
    return this.flatten().reduce(function (a, b) {
        if (typeof a === 'string') {
            a = a.replace(/[^\d.-]/g, '') * 1;
        }
        if (typeof b === 'string') {
            b = b.replace(/[^\d.-]/g, '') * 1;
        }

        return a + b;
    }, 0);
});

/*Ajax tratado e com suporte a loader, utilizar ao montar graficos e tabelas*/
function EasyAjax(url, dados, callback, loader, toggle) {

    if (!!loader)
        $('#' + loader).empty().addClass('loader');

    if (dados == undefined)
        dados = {};

    //AJAX
    $.post(url, dados, function (r) {
        try {
            if (toggle != undefined)
                $('#menu-toggle').click();

            if (!!loader)
                $('#' + loader).removeClass('loader');

            callback(r);

        } catch (e) {
            console.log(e);
        } finally {
            $btn.button('reset');
        }
    }).fail(function (e, h, x) {
        $btn.button('reset');
        if (e.status == 0) {
            GuardJs.exibirMensagemAlerta("Não foi possivel buscar os dados: " + e.statusText);
        } else {
            GuardJs.exibirMensagemAlerta("Não foi possivel buscar os dados: " + e.responseJSON.Message);
        }
    }).always(function () {
        if (!!loader)
            $('#' + loader).removeClass('loader');
    });
}

function InitiMasksDefaults() {

    /*Input Mask*/
    $('.integer').each(function (index) {
        $(this).inputmask("integer", { rightAlign: false });
    });
    $('.decimal').each(function (index) {
        $(this).inputmask("decimal", { rightAlign: false });
    });

    $('.integer-direita').each(function (index) {
        $(this).inputmask("integer", { rightAlign: true });
    });
    $('.decimal-direita').each(function (index) {
        $(this).val($(this).val().replace(',', '.'));
        $(this).inputmask("decimal", { rightAlign: true });
    });

    $('.integer-esquerda').each(function (index) {
        $(this).inputmask("integer", { rightAlign: false });
    });
    $('.decimal-esquerda').each(function (index) {
        $(this).val($(this).val().replace(',', '.'));
        $(this).inputmask("decimal", { rightAlign: false });
    });
    /*FIM Input Mask*/

    /*Select 2*/
    $('.select2ddl').each(function (index) {
        $(this).select2();
    });

    /*FIM Select 2*/

    $('.DataPiker').daterangepicker({
        singleDatePicker: true,
        showDropdowns: true,
        'locale': {
            "format": "DD/MM/YYYY",
        }
    });

}

/*Mascaras e instancias de Select 2 por classe*/
$(document).ready(function () {

    InitiMasksDefaults();

})


function getCookie(name) {
    var value = "; " + document.cookie;
    var parts = value.split("; " + name + "=");
    if (parts.length == 2) return parts.pop().split(";").shift().split('&');
}

Array.prototype.max = function () {
    return Math.max.apply(null, this);
};

Array.prototype.min = function () {
    return Math.min.apply(null, this);
};

/*
    tableId [type=string] Ex: "Table1",
    isInLine [type=bool] Ex: true,
    isInColumn [type=bool] Ex: "Table1",
    startIndex [type=int] Ex: 1, (Representa o índice do 1° elemento da tabela a ser interado, a primeira td é 0).
    delimiterIndex [type=int] Ex: 3, (Representa o índice da ultima TD aonde começa a zona de repetição dos dados).
*/
function heatMap(tableId, isInLine, isInColumn, startIndex, delimiterIndex) {

    var startIndexFixed = startIndex;

    //Se for por TR.
    if (isInLine) {

        //Se for In Line > para cada TR procura indice e calcula o percentual.
        $('#fixBody1 > table tr').each(function (c, o) {

            //Para manejar contadores por TR
            startIndex = startIndexFixed;
            var elems = [];

            //Insere no array todos os elementos indicados em startIndex, insere o elemento Jquery "td" e o valor da "td".
            while (!!$(o).find('td:eq(' + startIndex + ')')[0]) {
                elems.push({
                    //obj javascript > td
                    td: $(o).find('td:eq(' + startIndex + ')') 
                    // Extrai valor numerico da TD.
                    , valor: $(o).find('td:eq(' + startIndex + ')')[0].textContent.match(/\d+(\.\d{1,2})?/g)[0] 
                });
                //Proximo index de TD a se inserir.
                startIndex += delimiterIndex + 1; 
            }

            //Valor minimo e maximo encontrados na TR.
            var valorMaximo = Math.max.apply(Math, elems.map(function (o) { return o.valor; }))
            var valorMinimo = Math.min.apply(Math, elems.map(function (o) { return o.valor; }))

            elems.forEach(function (oo, cc) {
                //Encontra valor em 1 percentual dentre os valores escolhidos, maior valor é 100% e menos é 0%
                var percentual = parseFloat((((oo["valor"] * 100) / valorMaximo) / 100).toFixed(2));
                //Atribui o Style com a cor de acordo com o valor percentual obtido acima.
                oo.td[0].style.backgroundColor = buscaCor(percentual);
            });
        })
    }
}

function percentToRGB(percent, order) {

    listaCorPura = [];
    listaCorPura.push('#00FF00');
    listaCorPura.push('#11FF00');
    listaCorPura.push('#22FF00');
    listaCorPura.push('#77FF00');
    listaCorPura.push('#88FF00');
    listaCorPura.push('#99FF00');
    listaCorPura.push('#AAFF00');
    listaCorPura.push('#BBFF00');
    listaCorPura.push('#CCFF00');
    listaCorPura.push('#DDFF00');
    listaCorPura.push('#EEFF00');
    listaCorPura.push('#FFFF00');
    listaCorPura.push('#FFEE00');
    listaCorPura.push('#FFDD00');
    listaCorPura.push('#FFCC00');
    listaCorPura.push('#FFBB00');//60%
    listaCorPura.push('#FFBB00');//64
    listaCorPura.push('#FFBB00');//68
    listaCorPura.push('#FFBB00');//72
    listaCorPura.push('#FFBB00');//76
    listaCorPura.push('#ff6901');//80
    listaCorPura.push('#ff6901');//84
    listaCorPura.push('#ff6901');//88
    listaCorPura.push('#ed3b1c');//92
    listaCorPura.push('#ed3b1c');
    listaCorPura.push('#ed3b1c');

    var listaCor = [];
    listaCorPura.forEach(function (o, c) {
        for (var i = 0; i < 4; i++) {
            listaCor.push(o);
        }
    });

    return listaCor[parseInt(percent)];

}

/*DESCONTINUAR ESTES METODOS< E UTILIZAR APENA INSTANCIA POR CLASSE*/
Inputmask.extendAliases({
    'numeric': {
        allowPlus: false,
        allowMinus: false
    }
});

Inputmask.extendAliases({
    'numericoPositivo': {
        alias: "numeric",
        placeholder: '',
        allowPlus: false,
        allowMinus: false
    }
});

Inputmask.extendAliases({
    'mascaraNumericaPositivaNegativa': {
        alias: "numeric",
        placeholder: '',
        allowPlus: false,
        allowMinus: true
    }
});

Inputmask.extendAliases({
    'mascaraNumericaInteira': {
        alias: "numeric",
        placeholder: '',
        allowPlus: false,
        allowMinus: false
    }
});
Inputmask.extendAliases({
    'campoCalculado': {
        mask: '[-][+]9{1,16}[.][9]{15}x10^[-][+]9{1,5}'
            , placeholder: ' '
            , showMaskOnFocus: false
            , clearMaskOnLostFocus: true
            , showMaskOnHover: false
            , reverse: true
            , clearIfNotMatch: true
            , clearIncomplete: true
        //, jitMasking: true
        //, showTooltip: true
    }
});

Inputmask.extendAliases({
    'procentagem': {
        radixPoint: ".",
        suffix: "%",
        clearMaskOnLostFocus: false,
        placeholder: ' ',
        showMaskOnFocus: true,
        showMaskOnHover: false,
        clearIfNotMatch: true
    }
});

Inputmask.extendAliases({
    'numerico': {
        radixPoint: ".",
        //suffix: "%",
        clearMaskOnLostFocus: false,
        placeholder: ' ',
        showMaskOnFocus: true,
        showMaskOnHover: false,
        clearIfNotMatch: true
    }
});
/*FIM DESCONTINUAR ESTES METODOS< E UTILIZAR APENA INSTANCIA POR CLASSE*/

/*Transforma array Row / Col para array unidirecional ROW (entrada [{a:1},{b:2}] saida [1,2])*/
function MapeiaValorParaHC(array, prop) {
    var arrayRetorno = $.map(array, function (o, c) {
        return o[prop];
    });
    return arrayRetorno;
}

function loadSelect2() {
    $.fn.select2.defaults.set("theme", "classic");
    $('.select2').css("max-height", "400px");
    //$('.select2').select2({ matcher: modelMatcher });
    $('.select2-container .select2-selection--single').css('height', '34px');
    $('.select2-container--classic .select2-selection--single .select2-selection__rendered').css('line-height', '34px');
    $('.select2-container--classic .select2-selection--single .select2-selection__arrow').css('height', '32px');
}

//Auxiliares para abas de bootstrap e divs em geral.
var DivManager = {
    //Cria div dentro de um elemento, retorna Id da div criada.
    // Parans: idString: Id do elemento aonde sera adicionada a nova Div, por ex, uma div,
    //         Contador: Numero sequencial concatenado ao Id da Div criada, por ex "IdNovo" + 1 = IdNovo1, Idnovo2, etc...,
    //         Tag: Complemento concatenado ao IdNovo, este não é sequencial,
    //         W: Width da div.
    // Returns: Id da Div criada.
    criaDiv: function (idString, contador, tag, w) {
        var novoId = idString + contador + (tag != undefined ? tag : "");
        document.getElementById(idString).appendChild(document.createElement("div"));
        $("#" + idString).children().last().attr("id", novoId);
        if (w != undefined)
            $("#" + idString).children().last().width(w);
        return $("#" + idString).children().last().attr("id");
    },

};

GuardJs = {

    /*DESCONTINUAR ESTES METODOS< E UTILIZAR APENA INSTANCIA POR CLASSE*/
    mascaraNumericaPositiva: function (e) {
        $(e).inputmask("numericoPositivo");
    },

    mascaraNumericaPositivaNegativa: function () {
        $(e).inputmask("mascaraNumericaPositivaNegativa");
    },

    mascaraCampoCalculado: function (e) {
        if ($(e).val() == "0")
            $(e).val("");
        $(e).inputmask("campoCalculado");
    },

    mascaraPorcentegem: function (e) {
        $(e).inputmask("numerico");
    },

    mascaraNumerica: function (e) {
        $(e).inputmask("numeric");
    },

    mascaraInteger: function(e) {
        $(e).inputmask("integer", { rightAlign: false });  
    },
    /*FIM DESCONTINUAR ESTES METODOS< E UTILIZAR APENA INSTANCIA POR CLASSE*/

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