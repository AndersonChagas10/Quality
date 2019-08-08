//document.addEventListener("deviceready", onDeviceReady, false);

function appendDevice(obj, appendTo) {
    // var platform = device.platform;

    // if (platform == 'windows') {
    //     MSApp.execUnsafeLocalFunction(function () {
    //         $(appendTo).append($(obj));
    //     });
    // }
    // else {
    //     $(appendTo).append($(obj));
    // }

    $(appendTo).append($(obj));
}
function prependDevice(obj, prependTo) {
    // var platform = device.platform;

    // if (platform == 'windows') {
    //     MSApp.execUnsafeLocalFunction(function () {
    //         $(prependTo).prepend($(obj));
    //     });
    // }
    // else {
    //     $(prependTo).prepend($(obj));
    // }

    $(prependTo).prepend($(obj));
}

var initialBody = null;

var _level1;
var _level2;
var _totalAvaliacoesPorIndicadorPorAvaliacao = [];
var _totalAvaliacoesPorMonitoramentoPorAvaliacao = [];
var _shift;

//assim que todos os arquivos foram carregados

function resetBody(result) {
    bodyEmpty();
    appendDevice(initialBody, $("body"));
    appendDevice(result, $("body"));
    loadResource();
    //initalizeInputs();
    //loadScripts();
}
function initializeInputs() {

    $('input.interval').inputmask("numeric", { allowMinus: true, radixPoint: ',' });
    $('.defects input').inputmask("integer", { allowMinus: false });
    $('.calculado .input01').inputmask("numeric", { allowMinus: true, radixPoint: ',' });
    $('.calculado .input02').inputmask("numeric", { allowMinus: true, radixPoint: ',', mask: "9" });

    //colocar EUA Mascara de dados'
    $('input[masc=date]').inputmask('99/99/9999');
}
$(document).on("keyup", "input.interval", function (e) {
    var strLength = this.value.length;
    $(this).setCursorPosition(strLength);
});
$.fn.setCursorPosition = function (position) {
    if (this.length == 0) return this;
    return $(this).setSelection(position, position);
}
$.fn.setSelection = function (selectionStart, selectionEnd) {
    if (this.length == 0) return this;
    input = this[0];

    if (input.createTextRange) {
        var range = input.createTextRange();
        range.collapse(true);
        range.moveEnd('character', selectionEnd);
        range.moveStart('character', selectionStart);
        range.select();
    } else if (input.setSelectionRange) {
        input.focus();
        input.setSelectionRange(selectionStart, selectionEnd);
    }

    return this;
}

$.fn.focusEnd = function () {
    this.setCursorPosition(this.val().length);
    return this;
}
function onDeviceReady() {

    if (device.platform == 'browser') {
        navigator.webkitPersistentStorage.requestQuota(
            requestedBytes, function (grantedBytes) {
                console.log('we were granted ', grantedBytes, 'bytes');

            }, function (e) { console.log('Error', e); }
        );
    }

    starterReload();
    initialBody = $("body").html();
    ccaImage = $(".ccaImage")[0].outerHTML;
    _readFile("loginpage.txt");
    _readFile("apppage.txt");
    _readFile("saved.txt");
    _readFile("databaseConsolidation.txt");
    _readFile("database.txt");
    _readFile("users.txt");
    _readFile("lastsync.txt");
    _readFile("sync.txt");
    _readFile("log.txt");
    _readFile("VerificacaoTipificacao.txt");
    _readFile("VerificacaoTipificacaoResultados.txt");
    _readFile("collectionLevel2Keys.txt");
    _readFile("deviations.txt");
    _readFile("Resource.txt");

    meuSelect = '<select id="cb_UrlPreffix1" onChange="abreOApp(this.value);">';
    meuSelect += '<option value="http://mtzsvmqsc/SgqGlobal">JBS</option>';
    meuSelect += '<option value="http://192.168.25.200/SgqMaster">GRT</option>';
    meuSelect += '<option value="http://localhost:8090/SgqSystem">GCN</option>';
    meuSelect += '<option value="http://localhost/SgqSystem">Local</option>';
    meuSelect += '</select>';

    if (localStorage.unit != undefined) {
        $('#selectUnit').val(localStorage.unit);
    }

    ping();

    updateWatch();

    abreOApp(urlPreffix);


}


function abreOApp(valor) {
    urlPreffix = valor;
    ping(getAPPOnLine, getAppOffLine);
    //ping(getAPPOnLine, null);
}

function getAPPOnLine() {
    $.ajax({
        type: 'POST'
        , url: urlPreffix + '/Services/SyncServices.asmx/getAPP2'
        //, contentType: 'application/json; charset=utf-8'
        //, dataType: 'json'
        , dataType: "xml"
        //,contentType: "text/xml; charset=\"utf-8\""
        , data: "version="+versao
        , async: false //blocks window close
        , success: function (data, status) {

            getParReason();

            var Login = $(data).text();
            appendDevice($(Login), $('body'));
            _writeFile("loginpage.txt", Login);

            loadMainSetup();

            $('.Starter').hide();

            $('#version .number, #versionLogin .number').text(versao);
            $('#ambiente .base, #ambienteLogin .base').text(" " + baseAmbiente);

            var language = $(".Resource").attr("language") == undefined ? "default" : $(".Resource").attr("language");
            //loginResource(language);

            $('#shift').trigger('change');
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        }
    });
}

function getAppOffLine() {
    _readFile("loginpage.txt", getLoginPage);
    _readFile("users.txt", function (r) {
        appendDevice($(r), $('body'));
    });
}

function getLoginPage(result) {
    bodyEmpty();
    appendDevice(result, $('body'));
    loadMainSetup();
}

function loadMainSetup() {

    if ($('#local').attr('empresa') == 'ytoara') {
        $('.login .foot').css("background-image", "url(img/grtytoaralogo.png)");
        baseAmbiente = ' Ytoara';
    }
    if ($('#local').attr('empresa') == 'santander') {
        $('.login .foot').css("background-image", "url(img/grtsantanderlogo.png)");
        $('.login .foot').height('30px');
        baseAmbiente = ' Santander';
    }

}

function starterReload() {
    $('#StarterMessage').text('Updating the application from server...');
    $('#StarterIcon').html('<i class="fa fa-spinner fa-pulse fa-2x fa-fw"></i>');
    $('#StarterButton').hide();
}

function starterFail() {
    $('#StarterMessage').text('Error updating the aplication. Connect to a server and try it again.');
    $('#StarterIcon').html('<i class="fa fa-exclamation-triangle" aria-hidden="true"></i>');
    $('#StarterButton').show();
}

$(document).on("change", "#language", function () {
    $("#messageError").addClass("hide");
    if (connectionServer == false) {
        $("#messageError").removeClass("hide").fadeIn();
        $("#mensagemErro").text(getResource("online_to_change_language"));
        setTimeout(function () {
            $("#messageError").fadeOut();
        }, 5000);
        return;
    }

    if ($(".Resource").attr("language") != $(this).val()) {
        //loginResource($(this).val());
    }
});



function AppAppend(App) {

    $(App[0]).find('.level1').each(function (index, self) {
        var levels = '';

        $(App[0]).children('.container').children('.level2List').children('.level2Group[level01id=' + $(self).attr('id') + ']').each(function (index2, self2) {
            levels += $(self2)[0].outerHTML;
        });

        if ($(self).attr('hasgrouplevel2') == 'true') {
            $(App[0]).children('.container').children('.level3List').children('.level3Group[level1idgroup=' + $(self).attr('id') + ']').each(function (index3, self3) {
                levels += $(self3)[0].outerHTML;
            });
        } else {
            $(App[0]).children('.container').children('.level3List').children('.level3Group[level1id=' + $(self).attr('id') + ']').each(function (index3, self3) {
                levels += $(self3)[0].outerHTML;
            });
        }

        _writeFile('level' + $(self).attr('id') + '.txt', levels);

    });

    //$('#period option')

    $(App[0]).find('#period option').each(function (index, option) {
        $(App[0]).children('.container').children('.level2List').children('.level2Group').find('.level2').each(function (index, self) {
            var level1id = parseInt($(self).parents('.level2Group').attr('level01id'));
            if ($(App[0]).children('.container').find('.level1[id=' + level1id + ']').attr('hasgrouplevel2') == "true")
                return;
            var level2id = parseInt($(self).attr('id'));
            var total = parseInt($(self).attr('evaluate'));
            var shift = parseInt($('#shift').val());
            var period = parseInt($(option).val());
            var unidade = $('.App').attr('unidadeid');
            setTotalCounterLevel1(level1id, level2id, total, shift, period, unidade);
        });

        $(App[0]).children('.container').children('.level3List').children('.level3Group').children('.level2').each(function (index, self) {
            var level1id = parseInt($(self).parents('.level3Group').attr('level1idgroup'));
            var level2id = parseInt($(self).attr('id'));
            var total = parseInt($(self).attr('evaluate'));
            var shift = parseInt($('#shift').val());
            var period = parseInt($(option).val());
            var unidade = $('.App').attr('unidadeid');
            setTotalCounterLevel1(level1id, level2id, total, shift, period, unidade);
        });
    });


    $(App[0]).children('.container').children('.level2List').children('.level2Group').remove();
    $(App[0]).children('.container').children('.level3List').children('.level3Group').remove();

    var login = $('body .login');
    var resource = $('body .Resource');
    bodyEmpty();
    appendDevice(login, $('body'));
    $('body .login').removeClass('hide');
    appendDevice(resource, $('body'));
    appendDevice(ccaImage, $('body'));

    _writeFile("Resource.txt", resource[0].outerHTML);

    appendDevice(App, $('body'));

    //$('.App').removeClass('hide');

    if ($('body .login').length > 1) {
        $('body .login:last').remove();
    }

    if ($('body .Resource').length > 1) {
        $('body .Resource:first').remove();
    }

    if ($('body .Resource').length == 0) {
        loadResource();
    }


}

function AppLevelsAppend(App) {

    $(App[0]).find('.level1').each(function (index, self) {
        var levels = '';

        $(App[0]).children('.container').children('.level2List').children('.level2Group[level01id=' + $(self).attr('id') + ']').each(function (index2, self2) {
            levels += $(self2)[0].outerHTML;
        });

        if ($(self).attr('hasgrouplevel2') == 'true') {
            $(App[0]).children('.container').children('.level3List').children('.level3Group[level1idgroup=' + $(self).attr('id') + ']').each(function (index3, self3) {
                levels += $(self3)[0].outerHTML;
            });
        } else {
            $(App[0]).children('.container').children('.level3List').children('.level3Group[level1id=' + $(self).attr('id') + ']').each(function (index3, self3) {
                levels += $(self3)[0].outerHTML;
            });
        }

        _writeFile('level' + $(self).attr('id') + '.txt', levels);


        $('.level1[id=' + $(self).attr('id') + ']').replaceWith(self);

    });


    $(App[0]).children('.container').children('.level2List').children('.level2Group').remove();
    $(App[0]).children('.container').children('.level3List').children('.level3Group').remove();

}

function getAPPLevelsVolume() {

    if (btnVolumeClicked == true)
        openMessageModal("Atualizando Volume...", null);

    var UserSGQ_Id = userlogado.attr('userid');
    var ParCompany_Id = userlogado.attr('unidadeid');
    var date = new Date(insertAt(insertAt(getCollectionDate(), 2, '-'), 5, '-') + " 12:00:00");

    var request = $.ajax({
        data: {
            "UserSgq_Id": UserSGQ_Id,
            "ParCompany_Id": ParCompany_Id,
            //"Date": new Date($.now()).toUTCString(),
            "Date": date.toUTCString(),
            "Level1ListId": listIndUpdate,
            "Shift_Id": parseInt(_shift)
        },
        url: urlPreffix + '/Services/SyncServices.asmx/getAPPLevelsVolume',
        type: 'POST',
        success: function (data) {
            var result = $(data);
            var App = $(result.text());

            AppLevelsAppend(App);

            if (btnVolumeClicked == true) {
                btnVolumeClicked = false;
                $.LoadingOverlay("hide");
                openMessageModal("Volume Atualizado com Sucesso!", null);

            }

        },
        timeout: 600000,
        error: function () {
            request.abort();
            contagem++;
            if (contagem > 2) {
                contagem = 0;
                if (btnVolumeClicked == true) {
                    btnVolumeClicked = false;
                    $('.loadingoverlay').hide();
                    openMessageModal("Erro ao sincronizar Volume.", null);
                }
                //loadInd();
                return;
            }
            setTimeout(function (e) {
                setTimeout(function (e) {
                    getAPPLevelsVolume();
                }, 500)
            }, 1500);
        }
    });
}

var contagem = 0;
function getAPPLevelsOnLine() {

    var UserSGQ_Id = userlogado.attr('userid');
    var ParCompany_Id = userlogado.attr('unidadeid');

    var urlAppLevels = urlPreffix + '/Services/SyncServices.asmx/getAPPLevels';

    // if($('#local').attr('empresa') == "jbs" && $('#local').attr('local') == "brasil"){
    //     urlAppLevels = urlPreffix + '/Services/SyncServices.asmx/getAPPLevelsModulado';
    // }else{
    //     urlAppLevels = urlPreffix + '/Services/SyncServices.asmx/getAPPLevels';
    // }

    var request = $.ajax({

        //data: {
        //    "UserSgq_Id": UserSGQ_Id,
        //    "ParCompany_Id": ParCompany_Id,
        //    "Date": new Date($.now()).toUTCString(),
        //    "Level1ListId": listIndUpdate
        //},

        url: urlPreffix + '/api/AppParams/GetTela/' + ParCompany_Id + "/" + parseInt(_shift),
        type: 'GET',
        success: function (data) {
            var correctiveAction = $('.correctiveAction[sync=false]');
            var result = $(data.ParteDaTela);
            //var App = $(result.text());

            /**/
            AppAppend(result);

            $('body .Results').append(correctiveAction);

            $.LoadingOverlay("hide");

            $('#version .number, #versionLogin .number').text(versao);
            $('#ambiente .base, #ambienteLogin .base').text(" " + baseAmbiente);

            wMessage($('#btnLoginOnline'), getResource('verifying_users'));

            setTimeout(function (e) {
                contagem = 0;
                getCompanyUsers(ParCompany_Id);

            }, 800);
        },
        timeout: 600000,
        error: function () {
            $.LoadingOverlay("hide");
            $('.loadingoverlay').hide();
            openMessageModal(getResource("indicators_error_try_again"), null);
            $('.modalSyncInd').remove();
        }
    });
}

function getAPPLevels1OnLine() {

    var UserSGQ_Id = userlogado.attr('userid');
    var ParCompany_Id = userlogado.attr('unidadeid');

    var urlAppLevels = urlPreffix + '/Services/SyncServices.asmx/getAPPLevels';

    // if($('#local').attr('empresa') == "jbs" && $('#local').attr('local') == "brasil"){
    //     urlAppLevels = urlPreffix + '/Services/SyncServices.asmx/getAPPLevelsModulado';
    // }else{
    //     urlAppLevels = urlPreffix + '/Services/SyncServices.asmx/getAPPLevels';
    // }

    var request = $.ajax({
        //data: {
        //    "UserSgq_Id": UserSGQ_Id,
        //    "ParCompany_Id": ParCompany_Id,
        //    "Date": new Date($.now()).toUTCString(),
        //    "Level1ListId": listIndUpdate
        //},
        url: urlPreffix + '/api/AppParams/GetTela/' + ParCompany_Id + "/" + parseInt($(shift).val()),
        type: 'GET',
        success: function (data) {
            var correctiveAction = $('.correctiveAction[sync=false]');
            var result = $(data.ParteDaTela);
            //var App = $(result.text());

            /**/
            AppAppend(result);

            $('body .Results').append(correctiveAction);

            $.LoadingOverlay("hide");

            $('#version .number, #versionLogin .number').text(versao);
            $('#ambiente .base, #ambienteLogin .base').text(" " + baseAmbiente);

            wMessage($('#btnLoginOnline'), getResource('verifying_users'));
            setTimeout(function (e) {
                contagem = 0;
                getCompanyUsers(ParCompany_Id);
                //getCollectionKeys(ParCompany_Id);               
            }, 800);
        },
        timeout: 600000,
        error: function () {
            request.abort();
            contagem++;
            if (contagem > 2) {
                contagem = 0;
                Geral.exibirMensagemErro(getResource("indicators_error_try_again"));
                return false;
            }
            wMessage($('#btnLoginOnline'), getResource("trying_again"));
            setTimeout(function (e) {
                wMessage($('#btnLoginOnline'), getResource('loading_indicators'));
                setTimeout(function (e) {
                    getAPPLevels1OnLine();
                }, 500)
            }, 1500);
        }
    });
}

var tempoPing = 0;
function updateWatch() {
    tempoPing++;
    if (tempoPing == 5) {
        tempoPing = 0;
        ping();
    }

    $('.atualDate').text(dateTimeWithMinutes(true));
    tempoLogout--;
    if (tempoLogout == 0) {
        $("#btnLogout").click();
        openMessageModal(getResource("inactive"), getResource("inactive_for") + " " + (configTempLogout / 60) + " " + getResource("login_again"));
    }

    setTimeout(updateWatch, 1000);
}

$(document).on('click', function (e) {
    //  confirmButtonLevel02Hide();
    rightMenuHide();
    imageHide();
});

function initializeApp() {
    $('.level03List').addClass('hide');
    $('.level02').removeClass('selected');
    $('.level02List').addClass('hide');
    $('.level01').removeClass('selected');
    $('.level01List').removeClass('hide').show();
    $('#btnLoginOnline').button('reset');
    $('#btnLoginOffline').button('reset');
    //stopAutomaticSync();
}

function AppLevelsAppend(App) {

    $(App[0]).find('.level1').each(function (index, self) {
        var levels = '';

        $(App[0]).children('.container').children('.level2List').children('.level2Group[level01id=' + $(self).attr('id') + ']').each(function (index2, self2) {
            levels += $(self2)[0].outerHTML;
        });

        if ($(self).attr('hasgrouplevel2') == 'true') {
            $(App[0]).children('.container').children('.level3List').children('.level3Group[level1idgroup=' + $(self).attr('id') + ']').each(function (index3, self3) {
                levels += $(self3)[0].outerHTML;
            });
        } else {
            $(App[0]).children('.container').children('.level3List').children('.level3Group[level1id=' + $(self).attr('id') + ']').each(function (index3, self3) {
                levels += $(self3)[0].outerHTML;
            });
        }

        _writeFile('level' + $(self).attr('id') + '.txt', levels);


        $('.level1[id=' + $(self).attr('id') + ']').replaceWith(self);

    });


    $(App[0]).children('.container').children('.level2List').children('.level2Group').remove();
    $(App[0]).children('.container').children('.level3List').children('.level3Group').remove();

    // $('body > div.App > div.container > div.level1List').find('#'+id+'').replaceWith();
    // $('#level1List').replaceWith(novaDiv);
    // $('#level1List').find('li id=2');
}

function ping(callbackOnLine, callbackOffLine) {

    try {

        if (urlPreffix == "") {
            //Se a url preffix estiver em branco
            connectionServer = false;
            if (callbackOffLine != undefined) {
                callbackOffLine();
            }
            return false;
        }

        var dataApp = new Date().toJSON();

        //Chamo o metodo para ver se o APP está conectado no webservice
        $.ajax({
            type: "GET",
            url: urlPreffix + '/api/LoginApi/Logado/?dataApp=' + dataApp,
            error: function (data) {
                //Erro de comunicação
                // connectionServer = false;
                offLine();
                // if (callbackOffLine != undefined) {
                //     callbackOffLine();
                // }

                //Online
                connectionServer = true;
                //onLine();
                if (callbackOnLine != undefined) {
                    callbackOnLine();
                }
            },
            fail: function () {
                //Erro não tratado
                // connectionServer = false;
                offLine();
                // if (callbackOffLine != undefined) {
                //     callbackOffLine();
                // }

                //Online
                connectionServer = true;
                //onLine();
                if (callbackOnLine != undefined) {
                    callbackOnLine();
                }

            },
            success: function (data) {
                if (data == "onLine") {
                    //Online
                    connectionServer = true;
                    onLine();
                    if (callbackOnLine != undefined) {
                        callbackOnLine();
                    }
                }
                else if (data == "dataInvalida") {

                    connectionServer = false;
                    noDataBase();
                    $('body').append('<div style="position:absolute;left:0;width:100%;height:100%;min-height:100%;min-width:100%;background-color:#ff8080;z-index:9999999 !important"><h1 style="text-align:center;color:#fff"> Data do dispositivo Incorreta!</h1><br><br><h3 style="text-align:center;color:#fff">Device date is not correct!</h3></div>');

                } else {

                    //Online mas sem conexão com o banco de dados
                    connectionServer = false;
                    noDataBase();
                    if (callbackOffLine != undefined) {
                        callbackOffLine();
                    }
                }
            },
            timeout: 5000 //in milliseconds não funciona bem
        });
    } catch (e) {
        //Erro 
        console.log(e);
        offLine();
        if (callbackOffLine != undefined) {
            callbackOffLine();
        }
    }
}

var timeoutSendResults;
var tempoSemaforo = 1;

setInterval(function () {
    tempoSemaforo++;
    if (tempoSemaforo >= 5)
        sendResultsTimeout();
}, 1000)

function sendResultsTimeout() {

    if (tempoSemaforo > 3) {
        tempoSemaforo = 0;

        timeoutSendResults = setTimeout(function (e) {
            // ping(sendResultsOnLine, sendResultsOnLine);
            sendResultsOnLine();
        }, 1000);
    }
}

//variavel para controle de tempo de logout
var tempoLogout = configTempLogout;

$(document).on('click', function (e) {
    //atualizacao quando um clique ocorre
    tempoLogout = configTempLogout;
});

$(window).bind('beforeunload', function () {
    return getResource("attention_lose_information");
});

function resetDay() {
    $('.overlay').hide();
    openMessageModal(
        getResource("collection_date"),
        "Essa coleta não foi iniciada hoje. Por favor, faça o login novamente."
        //"This collection didn't start today, it will be recorded normally. Please, login again."
    );

    resetApplication();

    $('#btnLogout').click();
}

function alteraDataRetroativaOnline() {

    var message = getResource("inform_retroactive_date");

    $('#inputDate').val("");

    $('#passMessageComfirm').addClass('hide');
    $('#inputDate').removeClass('hide');

    setTimeout(function (e) {
        $('#inputDate').focus();
    }, 300);

    openMessageConfirm(getResource("alter_collection_date"), message, function () {

        if ($('#inputDate').val() == "" || !validDateInputDate($('#inputDate').val())) {
            $('#inputDate').focus();
            return false;
        }

        var dataRetroativa = new Date(convertDate($('#inputDate').val()));

        if (!retroactiveDateIsValid(dataRetroativa)) {
            openMessageModal(getResource("cant_change_date_greater_than_current"), getResource("input_a_valid_date"));
            return false;
        }

        $('#inputDate, #btnMessageYes, #btnMessageNo').addClass('hide');
        $('.messageConfirm .head').text(getResource("changing_date"));
        $('.messageConfirm .txtMessage').text(getResource("changing_date_please_wait") + '...');

        var dateInput = convertDate($('#inputDate').val());
        var atualDate = dateFormat();

        if (dateInput != atualDate) {
            var dateRetroactive = dateFormateRetroactive(dateInput);
            $('.App').attr('retroactivedata', dateRetroactive);
        }
        else {
            $('.App').removeAttr('retroactivedata');
        }

        setTimeout(function (e) {

            $('#inputDate, #btnMessageYes, #btnMessageNo').removeClass('hide');
            $('#btnMessageNo').click();
            _readFile("saved.txt", getAppRetroativeDate);

        }, 2000);

    });
}

function retroactiveDateIsValid(dataRetroativa) {

    //Data que foi imputada no messageModal
    dataRetroativa = dataRetroativa.setHours(0, 0, 0, 0);

    //data que da ultima sincronização (login online)
    var dataMax = new Date($('.App').attr('datelastsync'));
    dataMax = new Date(dataMax.setDate(dataMax.getDate() + 2)).setHours(0, 0, 0, 0);

    //data do tablet
    var dataMax2 = new Date();
    dataMax2 = new Date(dataMax2.setDate(dataMax2.getDate() + 2)).setHours(0, 0, 0, 0);

    if (dataRetroativa < dataMax &&
        dataRetroativa < dataMax2) {
        return true;

    } else {
        return false;
    }
}

function getAppRetroativeDate(result) {

    //var App = $(result);
    //$("body").empty();
    //appendDevice(App.html(), $("body"));

    initialLogin();
}
function alteraDataRetroativaOffLine() {
    openMessageModal(getResource("you_are_not_online"), getResource("change_date_collection_need_to_be_online"));
}


function resetApplication() {
    //Reseto todos os itens do Level01
    level01Reset($('.level01'));
    //Zero os Selectes do Painel do Level02
    $('.painelLevel02 select').val(0);
    //Reseto os inputs do Painel do Lavel02
    $('.painelLevel02 input').val("");

    //Reseto todos os itens dos Level02
    level02Reset($('.level02List .level02Group .level02'));

    //Atribui o periodo selecionado ao APP.
    $('.App').attr('period', $('#selectPeriod option:selected').val());

    //Coloco o texto o Lebel de Periodo do APP
    $('span.period').html($("select#selectPeriod:visible :selected").text());
}


function cleanResults() {
    var results = $('.Results .level01Result, .Results .correctiveAction');
    $('.Results').empty();
    appendDevice(results, $('.Results'));
}

function createLog(message) {
    var date = dateTimeFormat();
    var div = "<div date='" + date + "'>" + date + ":" + message + "</div>";

    _readFile("log.txt", function (result) {
        _writeFile("log.txt", div + result);
    });
}

$(document).on('keydown', function (e) {
    var atualLevel = '.level1';
    if ($('.level2').is(':visible')) {
        atualLevel = '.level2';
    }
    else if ($('.level3').is(':visible')) {
        atualLevel = '.level3';
    }
    var levelsVisible = $(atualLevel + ':visible');
    var campo = levelsVisible;
    var indice = campo.index($(atualLevel + ':visible.selected'));

    switch (e.which) {
        case 8:
            if (e.ctrlKey) {
                $('.breadcrumb li a:last').click();
            }
            break;
        case 13:
            if (e.ctrlKey) {
                e.preventDefault();
                if ($('#btnSave').is(':visible')) {
                    setTimeout(function (e) {
                        $('#btnSave:visible').click();
                    }, 200);
                }
            }
            break;
        case 38:
            //podemos unificar coma funcao case 40
            if (campo[indice - 1] != null) {
                var proximo = campo[indice - 1];
                setTimeout(function (e) {
                    $(atualLevel + ':visible.selected').removeClass('selected');
                    var itemFocus = $(proximo);
                    itemFocus.addClass('selected')
                    if (itemFocus.find('a').length) {
                        itemFocus.find('a').focus();
                    }
                    else {
                        itemFocus.focus();
                    }
                });
            }
            break;
        case 40:
            //podemos unificar com a funcao de cima
            if (campo[indice + 1] != null) {
                var proximo = campo[indice + 1];
                setTimeout(function (e) {
                    $(atualLevel + ':visible.selected').removeClass('selected');
                    var itemFocus = $(proximo);
                    itemFocus.addClass('selected')
                    if (itemFocus.find('a').length) {
                        itemFocus.find('a').focus();
                    }
                    else {
                        itemFocus.focus();
                    }
                });
            }
            break;
    }

});

$(document).on('keyup', '.interval input, .calculado input, .defects input', function () {
    this.setSelectionRange(this.value.length, this.value.length);
})

$(document).on('click', '.viewModal .close', function (e) {
    $('.viewModal').fadeOut(function (e) {
        $('.viewModal .body').empty();
    });
});


$(document).on('change', '#selectParCompany', function (e) {
    if ($('.level02Result[sync=false]:first').length == 0)
        ping(changeCompany_OnLine, changeUnit_OffLine);
    else {

        //forçar sincronização e fazer a troca apos sincronizar
        $('#btnSync').trigger('click');

        $('#selectParCompany').val($('#selectParCompany').attr('parcompany_id'));
        openMessageModal(getResource("unsyncronized_data"), null);
        return false;
    }
});

function changeCompany_OnLine() {
    var ParCompany_Id_Padrao = $('#selectParCompany').attr('parcompany_id');
    var ParCompany_Id_Selecionada = $('#selectParCompany').val();
    if (ParCompany_Id_Padrao != ParCompany_Id_Selecionada) {
        menssagemSync("<i class='fa fa-spinner fa-spin'></i>" + getResource("changing_unit_preferences"));
        $.ajax({
            data: {
                //Name: username,
                "UserSgq_Id": userlogado.attr('userid'),
                "ParCompany_Id": $('#selectParCompany').val(),
            },
            //    url: urlPreffix + '/api/User/AuthenticationLogin',
            url: urlPreffix + '/Services/SyncServices.asmx/UserCompanyUpdate',
            type: 'POST',
            success: function (data) {

                var d = $(data).text();

                if (d != "") {
                    openMessageModal(getResource("problem_occur"), d);
                }
                else {
                    setTimeout(function (e) {
                        openMessageModal(getResource("unit_changed_login_again"), null);
                        $('#btnLogout').click();
                    }, 1500);
                };
            },
            error: function () {
                changeUnit_OffLine();
            }
        });
    }
}

function changeUnit_OffLine() {
    var ParCompany_Id_Padrao = $('#selectParCompany').attr('ParCompany_Id');
    var ParCompany_Id_Selecionada = $('#selectParCompany').val();
    if (ParCompany_Id_Padrao != ParCompany_Id_Selecionada) {
        $('#selectParCompany').val(ParCompany_Id_Padrao);
        setTimeout(function (e) {
            openMessageModal(getResource("you_are_not_online"), getResource("change_unit_must_be_online"));
        }, 10);
    }
}

function syncButton() {
    var btnSyncInd = $('#btnSyncParam').clone();
    $('#btnSyncParam').after(btnSyncInd);
    btnSyncInd.attr('id', 'btnSyncInd');
    $('#btnSyncInd').text('Sincronizar indicadores');
}

// $(document).on('click', '#btnSyncInd', function(){
//     loadInd();
// });

var intervalo = setInterval(function () {
    $('span.database').css('margin', '2px');
    var notSynced = $('.level02Result[sync=false]').length + $('.VerificacaoTipificacaoResultados div[sync=false]').length;
    if (notSynced == 0) {
        $('span.database').text(getResource('data_synced'));
        //afterDevice($('<i class="fa fa-database" aria-hidden="true"></i>'), $('span.database'));
    } else {
        $('span.database').text(getResource('data_not_synced') + " (" + notSynced + ")");
        //afterDevice($('<i class="fa fa-database" aria-hidden="true"></i>'), $('span.database'));
    }
}, 1000);

function pinga(url) {
    console.log(count++);
    var resp;
    $.get(url + '/api/LoginApi/Logado', function (res) {
        resp = res;
    });
    return resp;
}

var ParReason = [];
function getParReason() {
    $.ajax({
        type: 'GET'
        , url: urlPreffix + '/api/ParReason/Get'
        , contentType: 'application/json; charset=utf-8'
        , dataType: 'json'
        , success: function (data, status) {
            ParReason = data;
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {

        }
    });
}