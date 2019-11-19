$(document).on('click', '#btnLogout', function (e) {
    var resultOThersUnits = $('.level02Result[sync=false][unidadeid=' + $('.App').attr('unidadeid') + ']:first');
    if (resultOThersUnits.length > 0) {
        ping(function (e) {
            setTimeout(function (r) {
                sendResults();
            }, 1000);

        }, function (r) {
            openMessageCA(getResource('logout_confirm'), null, function (e) {
                $('.messageConfirm, .overlay').fadeOut("fast");
                deslogar(true);
            });
        });
    } else {
        deslogar(false);
    }
});
// $(document).on('click', '#btnClearDatabase', function (e) {

//     var message = '';

//     if ($('.level02Result[sync=false]').length > 0) {
//         message = getResource("unsyncronized_data");
//     }
//     message += getResource("do_you_want_to_continue");

//     openMessageConfirm(getResource("clear_database"), message,
//         function () {
//             if ($('#passMessageComfirm').val() == "1qazmko0#") {
//                 _writeFile("database.txt", "");
//                 $('.Results').empty();
//                 $('.messageConfirm, .overlay').fadeOut("fast");
//             } else {
//                 $('.messageConfirm, .overlay').children('.body').children('.txtMessage').html('Wrong password.');
//             }
//         }
//     );

// });

function rightMenuHide() {
    if ($('.rightMenu').hasClass('visible')) {
        $(".rightMenu").removeClass('visible').animate({ "right": "-151px" }, "fast", function (e) {
            $('.overlay').hide();
        });
    }
}

$(document).off('click', '#btnSync').on('click', '#btnSync', function (e) {
    //if (connectionServer == true) {
    //    openSyncModal('Sync', 'Syncing the data collection...');
    //    Sync();
    //    //waitingDialog.show('Syncing the results...', { dialogSize: 'sm', progressType: 'primary' }); setTimeout(function () { waitingDialog.hide(); }, 3000);
    //} else {
    //    openMessageModal("Connection", 'The device is not connected to the server.');
    //}
    if (!isDisabledSyncButton) {
        disableSyncButtons();
        sendResults();
    }
});

var btnVolumeClicked = false;
$(document).off('click', '#btnSyncVolume').on('click', '#btnSyncVolume', function (e) {
    if (!isDisabledSyncButton) {
        disableSyncButtons();
        btnVolumeClicked = true;
        ping(getAPPLevelsVolume, paramsUpdate_OffLine);
    }
});

var loadFirst = false;
$(document).on('click', '#btnLoginOffline', function (e) {

    textbtnLoginOffline = $('#btnLoginOffline').text();
    textbtnLoginOnline = $('#btnLoginOnline').text();
    $('#btnLoginOffline').button('loading');
    $('#btnLoginOnline').attr('disabled', 'disabled');
    Geral.esconderMensagem();
    e.preventDefault();
    //offLineLogin();
    ping(onlineLogin, offLineLogin);

    listIndUpdate = '';
    loadFirst = true;

});

$(document).on('click', '#btnLoginOnline', function (e) {

    textbtnLoginOffline = $('#btnLoginOffline').text();
    textbtnLoginOnline = $('#btnLoginOnline').text();
    $('#btnLoginOnline').button('loading');
    $('#btnLoginOffline').attr('disabled', 'disabled');
    Geral.esconderMensagem();
    e.preventDefault();

    //if($('#local').attr('empresa') == 'jbs' && $('#local').attr('local') == 'brasil')
    //onlineLogin();
    //else
    ping(onlineLogin, offLineLogin);

    listIndUpdate = '';
    loadFirst = true;
});

// $(document).on('click', '#btnChangeModule', function (e) {

//     $.LoadingOverlay("show");
//     listIndUpdate = "";
//     $('#btnLoginOnline').click();

// });

$(document).on('click', '.btnCA', function (e) {

    $(this).addClass('caLevel02').addClass('hide');
    if ($('#btnSave').is(':visible')) {
        $('#btnSave').addClass('caLevel02').addClass('hide');
    }
    correctiveActionOpen();
    //$('.level01List').addClass('hide').hide();
    //$('.level02List').addClass('hide').hide();
    //$('.level03List').addClass('hide').hide();
    //$('.breadcrumb').addClass('hide').hide();

    $('.correctiveaction').removeClass('hide').show();
    $(this).addClass('hide');
});

$(document).on('change', '#selectUnit', function (e) {
    //_writeFile("sync.txt", "");
    urlPreffix = "";
    connectionServer = false;
    if ($('#selectUnit :selected').val() == "0") {
        return false;
    }
    urlPreffix = 'http://' + $('#selectUnit :selected').attr('ip');
    ping();
});

function rightMenuShow() {

    if ($('.ClusterList a.cluster').length > 1 && $('#btnVoltarCluster').length == 0) {
        $('#btnLogout').after('<a href="#" id="btnVoltarCluster" class="list-group-item COMP006 hasRole">Trocar Processo</a>');

        $(document).on('click', '#btnVoltarCluster', function (e) {
            voltarCluster()
        });
    }

    $('.overlay').fadeIn('fast');
    $(".rightMenu").animate({
        right: "0px"
    }, "fast", function () {
        $(this).addClass('visible');
    });
}

var mostraDebug = false;
$(document).on('click', '#btnMostrarContadores', function () {
    if (mostraDebug == false) {
        $(this).text('Ocultar Contadores')
        mostraDebug = true;
        mostraDebugAlertas(1);
    }
    else {
        $(this).text('Mostrar Contadores')
        mostraDebug = false;
        mostraDebugAlertas(0);
        //$('.debug, .level2Debug').remove();
    }

});


$(document).on('click', '#btnLog', function (e) {

    var message = "Type the password to continue";

    openMessageConfirm("View log", message,
        function () {
            if ($('#passMessageComfirm').val() == "1qazmko0#") {
                $('.messageConfirm, .overlay').fadeOut("fast");
                menssagemSync("Reading Data", 'This may take some time');

                setTimeout(function (e) {
                    readLog();
                    mensagemSyncHide();
                }, 2000);

            } else {
                $('.messageConfirm, .overlay').children('.body').children('.txtMessage').html('Wrong password.');
            }
        }
    );
});

$(document).on('click', '#btnClearDatabase', function (e) {

    var message = '';

    if ($('.level02Result[sync=false]').length > 0) {
        message = "There still unsynced data in the tablet database.";
    }
    message += " Do you want to continue?";

    openMessageConfirm("Clear database", message,
        function () {
            if ($('#passMessageComfirm').val() == "1qazmko0#") {
                _writeFile("database.txt", "");
                $('.Results').empty();
                $('.messageConfirm, .overlay').fadeOut("fast");
            } else {
                $('.messageConfirm, .overlay').children('.body').children('.txtMessage').html('Wrong password.');
            }
        }
    );
});


$(document).on('click', '#btnCollectDB', function (e) {

    var message = "Type the password to continue";

    openMessageConfirm("View database", message,
        function () {
            if ($('#passMessageComfirm').val() == "1qazmko0#") {
                $('.messageConfirm, .overlay').fadeOut("fast");

                $('.viewModal .body').text($('.Results').html());
                menssagemSync("Reading Data", 'This may take some time');
                setTimeout(function (e) {
                    readDataBase();
                    mensagemSyncHide();
                }, 4000);
            } else {
                $('.messageConfirm, .overlay').children('.body').children('.txtMessage').html('Wrong password.');
            }
        }
    );
});

function readDataBase() {
    _readFile("database.txt", function (result) {
        $('.viewModal .body').text(result);
        $('.viewModal').fadeIn();
    });
}

function deleteLog() {
    _writeFile("log.txt", "");
}

function readLog() {
    _readFile("log.txt", function (result) {
        $('.viewModal .body').text(result);
        $('.viewModal').fadeIn();
    });
}

var flagAutoSend = true;
$(document).on("click", "#btnAutoSend", function () {
    if (flagAutoSend) {
        flagAutoSend = false;
        $(this).text(getResource('auto_send_off'));
        sendResultsTimeout();
    } else {
        flagAutoSend = true;
        $(this).text(getResource('auto_send_on'));
        sendResultsTimeout();
    }
});

function paramsUpdate_OnLine() {
    $.LoadingOverlay("show");
    menssagemSync(getResource("updating_parameters"), getResource("verifying_parameters"));
    getAPPLevelsOnLine();
}

function paramsUpdate_OffLine() {
    openMessageConfirm(getResource("update_parameters_must_be_online"));
}

function deslogar(sync) {
    if (sync) {
        $('.App').fadeOut("fast", function (e) {
            $('.login').removeClass('hide').show();
            $('#inputUserName').val("").focus();
            $('#inputPassword').val("");
            Geral.exibirMensagemErro(getResource('data_not_synced') + " " + getResource('of') + " " + $('.unit').text());
            $("#shift").val("0").change();
            createFileResult(false);
            initializeApp();
        });
    } else {
        $('.App').fadeOut("fast", function (e) {
            $('.login').removeClass('hide').show();
            $('#inputUserName').val("").focus();
            $('#inputPassword').val("");
            $("#shift").val("0").change();
            createFileResult(false);
            initializeApp();
        });
    }
}

var isDisabledSyncButton = false;
function disableSyncButtons() {
    isDisabledSyncButton = true;
    setTimeout(function () { isDisabledSyncButton = false; }, 15000);
}