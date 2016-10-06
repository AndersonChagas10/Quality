var urlServidor = "http://192.168.25.200/SgqMaster";
//var urlLocal = "http://192.168.25.138/sgqGlobal"
var urlLocal = "http://localhost:63128/"

var urlPreffix = window.location.host.indexOf("host") > 0 ? urlServidor : urlLocal;

function MakeObject2(element, ListName, objectReturn) {
    objectReturn[ListName] = [];
    var elemens = element;
    $.each(elemens, function (counter, object) {

        if ($(object).attr('sync') == "true") { return; }

        var temp = {};
        var el = object;
        for (var i = 0, atts = el.attributes, n = atts.length, arr = []; i < n; i++) {
            var name = atts[i].nodeName;
            var value = atts[i].nodeValue;
            if (value == 'undefined' || value == 'null') { continue; }
            temp[name] = value;
        }
        objectReturn[ListName].push(temp);
        if (!!object.childNodes) {
            if (object.childNodes.length > 0)
                MakeObject2(object.childNodes, 'next' + ListName, temp);
        }
    });
}

function MakeResult(Obj, selector) {
    MakeObject2(selector, 'Root', Obj);
    console.log('Done.');
    //console.log(Obj);
}

function Sync() {

    counter = 0;

    $('.status').text('Sync started...');
    var ObjListaLevel1 = {};
    MakeResult(ObjListaLevel1, $('.level01Result'));
    $.each(ObjListaLevel1.Root, function (c, o) {
        var smallerObject = {};
        smallerObject['Root'] = [];
        if (!!o.correctiveactioncomplete) {
            var tempCA = {}
            MakeObject2($('.Results > .correctiveAction'), 'a', tempCA)
            $.each(tempCA.a, function (cc, oo) {
                if (oo.level01id == o.level01id)
                    o['correctiveactioncomplete'] = oo;
            });
        }
        smallerObject.Root.push(o);
        console.log(smallerObject)
        $.post(urlPreffix + "/api/Sync/SetDataAuditConsolidated", smallerObject, function (r) {
            $('.level01Result[level01Id=' + r.IdSaved + '] .level02Result').attr('sync', true)
            console.log(r);
            counter++;
            if (ObjListaLevel1.Root.length == counter) { sendHtml(); }
        });
    });


}


function sendHtml() {
    var htmlObject = {};
    htmlObject['html'] = $('.Results').html();
    var stringDateApp = $('.App').attr('date');
    htmlObject['CollectionHtml'] = {
        Html: '',
        Period: parseInt($('.App').attr('period')),
        Shift: parseInt($('.App').attr('shift')),
        CollectionDateFormatado: stringDateApp.substring(0, 2) + "/" + stringDateApp.substring(2, 4) + "/" + stringDateApp.substring(4, 8),
        UnitId: parseInt($('.App').attr('unidadeid')),
    };
    $.post(urlPreffix + "/api/Sync/SaveHtml", htmlObject, function (r) {
        console.log(r);
        if (!r.MensagemExcecao) {
            $('.status').text('Sync finished successfully.');
        } else {
            $('.status').text('Sync failed.');
        }
        GetToSync();
    });
}

function GetSync() {
    $.post(urlPreffix + "/api/Sync/GetLastEntry", {}, function (r) {
        console.log(r);
    });
}

function GetToSync() {
    var unidadeid = $('.App').attr('unidadeid');
    $.post(urlPreffix + "/api/Sync/GetHtmlLastEntry", { '': unidadeid }, function (r) {
        console.log(r);
        if (r.Retorno) {
            $('.Results').empty();
            appendDevice(r.Retorno.html, $('.Results'));
            //$('.status').text('Results recovered successfully.');
            statusMessage('Results recovered successfully.');
        }
    });
}

function statusMessage(message) {
    $('.status').text(message);
    setTimeout(function () {
        $('.status').empty();
    }, 5000);

}

var waitingDialog = waitingDialog || (function ($) {
    'use strict';

    // Creating modal dialog's DOM
    var $dialog = $(
		'<div class="fade" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-hidden="true" style="padding-top:15%; overflow-y:visible;">' +
		'<div class="modal-dialog modal-m">' +
		'<div class="modal-content">' +
			'<div class="modal-header"><h3 style="margin:0;"></h3></div>' +
			'<div class="modal-body">' +
				'<div class="progress progress-striped active" style="margin-bottom:0;"><div class="progress-bar" style="width: 100%"></div></div>' +
			'</div>' +
		'</div></div></div>');

    return {
        /**
		 * Opens our dialog
		 * @param message Custom message
		 * @param options Custom options:
		 * 				  options.dialogSize - bootstrap postfix for dialog size, e.g. "sm", "m";
		 * 				  options.progressType - bootstrap postfix for progress bar type, e.g. "success", "warning".
		 */
        show: function (message, options) {
            // Assigning defaults
            if (typeof options === 'undefined') {
                options = {};
            }
            if (typeof message === 'undefined') {
                message = 'Loading';
            }
            var settings = $.extend({
                dialogSize: 'm',
                progressType: '',
                onHide: null // This callback runs after the dialog was hidden
            }, options);

            // Configuring dialog
            $dialog.find('.modal-dialog').attr('class', 'modal-dialog').addClass('modal-' + settings.dialogSize);
            $dialog.find('.progress-bar').attr('class', 'progress-bar');
            if (settings.progressType) {
                $dialog.find('.progress-bar').addClass('progress-bar-' + settings.progressType);
            }
            $dialog.find('h3').text(message);
            // Adding callbacks
            if (typeof settings.onHide === 'function') {
                $dialog.off('hidden.bs.modal').on('hidden.bs.modal', function (e) {
                    settings.onHide.call($dialog);
                });
            }
            // Opening dialog
            $dialog.modal();
        },
        /**
		 * Closes dialog
		 */
        hide: function () {
            $dialog.modal('hide');
        }
    };

})(jQuery);