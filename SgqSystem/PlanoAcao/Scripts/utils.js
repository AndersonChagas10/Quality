var rootUrl = window.location.host + "/";

var dismiss = '[data-dismiss="alert"]';
var waitingForBool = false;
var waitingForCount = 0;


var Alert = function (el) {
    $(el).on('click', dismiss, this.addClass("hidden"))
}

var Geral = {

    //Requisição ajax com tratamento de erro para APIs
    ajaxRequestApi: function (config) {

        console.log("Chamou" + config.url);

        waitingForCount++;
        Geral.waitingForShow(config.url);

        $.ajax({
            dataType: 'json',
            data: config.dataObj,
            url: config.url, //Obrigatorio
            type: config.type || 'POST', //Post Default
            async: config.async == undefined || config.async, // true Default 
            success: function (result) {

                waitingForCount--;
                Geral.waitingForHide(config.url);

                if (result.IsSucesso) {
                    if (config.success != undefined) {
                        config.success(result);
                    }
                }
                else {
                    if (config.error != undefined) {
                        config.error(result);
                    }
                }

                if (result.IdMensagem != null) {
                    if (result.IdMensagem.length != 0) {
                        $('#alert' + result.IdMensagem).removeClass('hidden');
                        var $divMensagem = $('#alert' + result.IdMensagem);
                        $divMensagem.find('span').text(result.Mensagem);
                        $('html,body').animate({ scrollTop: 0 }, 'slow');
                    }
                }

            },
            error: function (xhr, ajaxOptions, thrownError) {

                console.log("xhr" + xhr);
                console.log("ajaxOptions" + ajaxOptions);
                console.log("thrownError" + thrownError);

                waitingForCount--;
                Geral.waitingForHide(config.url);

                var $divMensagem = $('#alertdanger');
                $divMensagem.find('span').text('Ocorreu um erro inesperado. Por favor entre em contato com o administrador do sistema.');
                $('html,body').animate({ scrollTop: 0 }, 'slow');
                if (config.error != undefined) {
                    config.error();
                }
                return null;
            }
        }).done(function (result) {
            if (config.done != undefined) {
                config.done();
            }
            return result;
        });

    },

    //Protótip print file via Canvas para print em PDF direto.
    printable: function printablePdf(id) {
        //Params:  id , id do elemento a ser impresso

        //var pdf = new jsPDF();
        //pdf.addHTML($('#' + id)[0], function () {
        //    var blob = pdf.output("blob");
        //    window.open(URL.createObjectURL(blob));
        //});

        var pdf = new jsPDF('l', 'pt', 'a4');
        pdf.addHTML($('#' + id)[0], NaN, NaN, "", function () {
            var blob = pdf.output("blob");
            window.open(URL.createObjectURL(blob));
        });

    },

    waitingForShow: function (url) {
        if (waitingForBool == false) {
            waitingForBool = true;
            //  console.log("Travou" + url);
            waitingDialog.show("Carregando...");
        }
    },

    waitingForHide: function (url) {
        if (waitingForBool == true && waitingForCount == 0) {
            waitingForBool = false;
            //  console.log("Destravou" + url);
            waitingDialog.hide();
        }
    },

};

var FunctionDate = {


    calcularDiferencaDataInicioFim: function (inicio, fim) {
        //formato do brasil 'pt-br'
        moment.locale('pt-br');
        //setando data1
        var data1 = moment(inicio, 'DD/MM/YYYY');
        //setando data2
        var data2 = moment(fim, '/DD/MM/YYYY');

        //tirando a diferenca da data2 - data1 em dias
        var diff = data2.diff(data1, 'days');

        if (isNaN(diff)) {
            data2 = moment(fim, 'DD/MM/YYYY');
            diff = data2.diff(data1, 'days');
        }

        return diff;

    },


    //Gera data atual
    convertDateNowToString: function (formato, dataVindo) {

        var date = new Date();
        if (dataVindo != undefined) {
            date = dataVindo;
        }
        var year = date.getFullYear();
        var month = date.getMonth() + 1;
        var day = date.getDate();
        var minutes = date.getMinutes();
        var seconds = date.getSeconds();
        var hours = date.getHours();


        if (formato == 'ddMMyyyy') {
            var ddMMyyyy = ("0" + day).slice(-2) + "/" + ("0" + month).slice(-2) + "/" + year;
            return ddMMyyyy;
        }
        else if (formato == 'ddMMyyyyhhmmss') {
            var ddMMyyyyhhmmss = ("0" + day).slice(-2) + "/" + ("0" + month).slice(-2) + "/" + year + " " + ("0" + hours).slice(-2) + ":" + ("0" + minutes).slice(-2) + ":" + ("0" + seconds).slice(-2);
            return ddMMyyyyhhmmss;
        }
        else if (formato == 'MMddyyyyhhmmss') {
            var MMddyyyyhhmmss = ("0" + month).slice(-2) + "/" + ("0" + day).slice(-2) + "/" + year + " " + ("0" + hours).slice(-2) + ":" + ("0" + minutes).slice(-2) + ":" + ("0" + seconds).slice(-2);
            return MMddyyyyhhmmss;
        }

    },

    //Converte string no formato dd/MM/yyyy para formato datetime
    convertStringToDate: function (dataString) {
        var parts = dataString.split('/');
        var mydate = new Date(parts[2], parts[1] - 1, parts[0]);
        return mydate;
    },

    //Limita data do datepiker com a data de hoje.
    limitDateTimeField: function (idData) {
        $("#" + idData).on('click', function () {
            if ($("#" + idData).val() == '') {
                FromEndDate = new Date();
                $("#" + idData).datepicker('setEndDate', FromEndDate);
            }
        });
    },

    //Recomendo usar a function 'convertStringToDate' no campo 'dataSetada'
    setDateId: function (idData, dataSetada) {
        setTimeout(function () {
            $("#" + idData).data("datepicker-initialized", true).not(":hidden").each(function () { $(this).click(); $(this).val($.date(dataSetada)); })
        }, 800);
    },

    //Limitar campos data fim e data inicio com data atual
    limitDateInitEnd: function (dataInicio, DataFim) {
        $("#" + dataInicio).on('click', function () {
            if ($("#" + dataInicio).val() == '' && $("#" + DataFim).val() == '') {
                FromEndDate = new Date();
                $("#" + dataInicio).datepicker('setEndDate', FromEndDate);
            }
        });

        $("#" + DataFim).on('click', function () {
            if ($("#" + DataFim).val() == '') {
                FromEndDate = new Date();
                $("#" + DataFim).datepicker('setEndDate', FromEndDate);
            }
        });

        $("#" + dataInicio).on('changeDate', function (selected) {
            startDate = new Date(selected.date.valueOf());
            startDate.setDate(startDate.getDate(new Date(selected.date.valueOf())));
            $("#" + DataFim).datepicker('setStartDate', startDate);
        });

        $("#" + DataFim).on('changeDate', function (selected) {
            FromEndDate = new Date(selected.date.valueOf());
            FromEndDate.setDate(FromEndDate.getDate(new Date(selected.date.valueOf())));
            $("#" + dataInicio).datepicker('setEndDate', FromEndDate);
        });
    },

    validarCampoData: function (idData) {
        if ($("#" + idData).val() != "") {
            if (!(/^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$/.test($("#" + idData).val()))) {
                //  Geral.exibirMensagemErro("Dados do campo Data inválido!");
                alert("tratar mensagem");
                return false;
            }
        }
        return true;
    },

    //Retorna nº de dias em determinado mes, use Date.GetMounth() , Date.GetFullYear() jquery para parametros.
    daysInMonth: function daysInMonth(dateObject) {
        return new Date(dateObject.getFullYear(), dateObject.getMonth() + 1, 0).getDate();
    },

};

var Auxiliares = {
    //Limpa DropDown por id
    clearDropDown: function (DropListId) {
        var select = document.getElementById(DropListId);
        var i;
        for (i = select.options.length - 1; i >= 0; i--) {
            select.remove(i);
        }
    },

    //Limpa todos campos
    clearAllFields: function (elementoPai) {
        $(':input', '#' + elementoPai)
            .not(':button, :submit, :reset, :hidden')
            .val('')
            .removeAttr('selected');
    },

    //Converte valores de properties para um objeto igual json.
    convertObjectToJson: function (data) {
        var retorno = {};

        for (i = 0; i < data.length; i++)
            retorno[data[i]['name']] = data[i]['value'];

        return retorno;
    },

    //Retorna um Elemento Jquery a partir de uma 'string' do id, ou '#string_do_id' (seletor).
    seletorOrStringToSeletor: function (btn) {
        var btnValid = btn || undefined;
        btnValid = (btnValid == undefined) ? undefined : (typeof btnValid == "object") ? btnValid : $('#' + btnValid.slice(btnValid.indexOf('#') + 1, btnValid.length));
        return btnValid;
    },

};

var URL_HELPER = {

    criarUrlApi: function (nomeController, nomeAcao, api) {
        if (api)
            return '../api/' + nomeController + '/' + nomeAcao;
        else
            return '../' + nomeController + '/' + nomeAcao;
    }
};

//Auxiliares para abas
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

//Dias da semana em portugues para highcharts.
var HighChardAddConfig = {

    dataPortugues: function () {
        Highcharts.setOptions({
            lang: {
                loading: 'Aguarde...',
                months: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
                weekdays: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'],
                shortMonths: ['Jan', 'Feb', 'Mar', 'Abr', 'Maio', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
                exportButtonTitle: "Exportar",
                printButtonTitle: "Imprimir",
                rangeSelectorFrom: "De",
                rangeSelectorTo: "Até",
                rangeSelectorZoom: "Periodo",
                downloadPNG: 'Download imagem PNG',
                downloadJPEG: 'Download imagem JPEG',
                downloadPDF: 'Download documento PDF',
                downloadSVG: 'Download imagem SVG'
                // resetZoom: "Reset",
                // resetZoomTitle: "Reset,
                // thousandsSep: ".",
                // decimalPoint: ','
            }
        });
    },

};

var DATATABLE_HELPER = {

    montarTabela: function (config) {

        //$('#tarefaTable').empty().DataTable({
        //    "processing": true,
        //    "data": dataTemporario,
        //    "columns": colunaOrdenada,
        //    "destroy": true,
        //    "fixedHeader": true,
        //    "scrollX": true,
        //    "bScrollCollapse": true
        //});



        var tabela = $('#' + config.idTabela).empty().DataTable({
            "oLanguage": {
                "sSearch": config.search || "Busca:",
                "sLengthMenu": config.lengthMenu || 'Mostrando _MENU_ registros por página',
                "sZeroRecords": config.msgNenhumRegistroEncontrado || 'Nenhum resultado encontrado',
                "sInfo": config.info || 'Mostrando _START_ a _END_ de _TOTAL_ resultados',
                "sInfoEmpty": config.infoEmpty || 'Mostrando 0 a 0 de 0 resultados',
                "sInfoFiltered": config.infoFiltered || '(filtrados _MAX_ registros)',
                "sLoadingRecords": config.loadingRecords || "Carregando...",
                "sProcessing": "Processando...",
                "oPaginate": {
                    "sFirst": config.first || '←',
                    "sLast": config.last || '→',
                    "sNext": config.next || '»',
                    "sPrevious": config.previous || '«'
                }
            },
            "processing": config.processing == undefined || config.processing,
            "data": config.data,
            "columns": config.columns,
            "destroy": config.destroy == undefined || config.destroy,
            "fixedHeader": config.fixedHeader == undefined || config.fixedHeader,
            "scrollX": config.scrollX == undefined || config.scrollX,
            "scrollY": config.scrollY || "",
            "scrollCollapse": config.scrollCollapse == undefined || config.scrollCollapse,
            "order": config.order || [[0, 'asc']],
            "lengthMenu": config.lengthMenu || [[10, 25, 50, -1], [10, 25, 50, "Todos"]],
            "pageLength": config.pageLength || 10,
            "bAutoWidth": config.bAutoWidth == undefined || config.bAutoWidth,
            "bFilter": config.bFilter == undefined || config.bFilter,
            "fnDrawCallback": config.fnDrawCallback,
            "searching": config.searching == undefined || config.searching,
            "paginate": config.paginate == undefined || config.paginate,
            "paging": config.paging == undefined || config.paging,
            //"serverSide": !config.serverSide == undefined || config.serverSide,
            "fixedColumns": {
                "leftColumns": config.leftColumns || 0,
                "rightColumns": config.rightColumns || 0,
            },
            "columnDefs": config.columnDefs,
            "initComplete": config.initComplete
        });

        
        var buttonsConfigLast = [
                {
                    extend: 'print',
                    text: 'Imprimir',
                    customize: function (win) {
                        $(win.document.body).find('table')
                            .addClass('compact')
                            .css('font-size', 'inherit');
                    },
                    exportOptions: {
                        columns: ':visible'
                    }
                },
                {
                    extend: 'excelHtml5',
                    text: 'Excel',
                    exportOptions: {
                        columns: ':visible'
                    }
                },
                //'excel',
                 {
                     extend: 'pdf',
                     text: 'PDF',
                     exportOptions: {
                         columns: ':visible'
                     }
                 },
                  {
                      extend: 'colvis',
                      text: 'Colunas Visíveis',
                      exportOptions: {
                          columns: ':visible'
                      }
                  },
                //, 'print'
        ];

        //var first = ["a", "b", "c"];
        //var second = ["d", "e", "f"];

        //if (config.buttonsCfg != undefined) {
        //    $.merge($.merge([], config.buttonsCfg), buttonsConfigLast);
        //}

        new $.fn.dataTable.Buttons(tabela, {
            buttons: buttonsConfigLast
        });

        tabela.buttons(0, null).container().prependTo(
            tabela.table().container()
        );

    },

};

var Cookie = {

    writeCookie: function (name, value, days) {
        var date, expires;
        expires = "";
        if (days) {
            date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            expires = "; expires=" + date.toGMTString();
        }
        document.cookie = name + "=" + value + expires + "; path=/";
        console.log("saved!");
    },

    readCookie: function (name) {
        var i, c, ca, nameEQ = name + "=";
        ca = document.cookie.split(';');
        for (i = 0; i < ca.length; i++) {
            c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1, c.length);
            }
            if (c.indexOf(nameEQ) == 0) {
                return c.substring(nameEQ.length, c.length);
            }
        }
        return '';
    },

};



$("input[alt='az']").each(function (index) {
    $(this).bind('keyup keypress keydown change mouseover', function () {
        var input = $(this);
        input.val(input.val().replace(/[^a-zA-Z]/g, ''));
    });
});

$('[mask]').each(function (e) {
    $(this).inputmask($(this).attr('mask'));
});

$('input[alt="telefone"]').bind("keyup paste blur focus", function () {
    $(this).inputmask({
        mask: ['(99) 9{4}-9{4}', '(99) 9{5}-9{4}'],
        removeMaskOnSubmit: true,
        placeholder: ' '
    });
});

$('input[alt="rf"]').bind("keyup paste blur focus", function () {
    $(this).inputmask({
        mask: ['999.999-9'],
        removeMaskOnSubmit: true,
        placeholder: ' '
    });
});

$('input[alt="moeda"]').bind("keyup paste blur focus", function () {
    var $camposMaskMoney = $(this);

    $camposMaskMoney.maskMoney({
        prefix: 'R$ ',
        allowZero: true,
        thousands: '.',
        decimal: ',',
        affixesStay: true
    });

    var $camposParaAplicarMaskMoney = $camposMaskMoney.filter(':not([value=""])');
    $camposParaAplicarMaskMoney.each(function () {
        var $this = $(this);
        var valor = $this.val();
        if (valor.indexOf(',') == -1) {
            $this.val(valor + ',00');
        }
    });
    $camposParaAplicarMaskMoney.maskMoney('mask');
});