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

function onDeviceReady() {
    getVersionNumber();

    onOpenAppColeta();

    if (device.platform == 'browser') {
        navigator.webkitPersistentStorage.requestQuota(
            requestedBytes, function (grantedBytes) {
                console.log('we were granted ', grantedBytes, 'bytes');

            }, function (e) { console.log('Error', e); }
        );
    }

    starterReload();

    $('body').prepend('<div class="" id="app" style="padding-bottom:50px;"></div>');
    $('body').css('padding-bottom', '20px');

    criarMensagem();
    criarModal();

    $('#btn-reload').attr('style', $('#btn-reload').attr('style') + ";z-index:9999");

    openLogin();
}

function starterReload() {
    $('#StarterMessage').text('Updating the application from server...');
    $('#StarterIcon').html('<i class="fa fa-spinner fa-pulse fa-2x fa-fw"></i>');
    $('#StarterButton').hide();
}