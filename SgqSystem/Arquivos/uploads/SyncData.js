//var urlServidor = "http://192.168.25.200/SgqMaster";
//var urlLocal = "http://192.168.25.138/sgqGlobal"
////var urlLocal = "http://localhost:63128/"

//var urlPreffix = window.location.host.indexOf("host") > 0 ? urlServidor : urlServidor;
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
            if (name == "datetime") {
                value = value.slice(0, 16)
            }
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
    var objToSend = { '': $('.App').attr('userid') };
    $.post(urlPreffix + '/api/Sync/Lock', objToSend, function (r) {

        try {

            //console.log(r);
            if (r == "wait") {
                //Esperar
                console.log('Waiting other user to finish Sync.');
                setTimeout(function (e) {
                    Sync();
                }, 800);
            } else if (r == $('.App').attr('userid')) {
                //Enviar
                SyncEnvia();
            } else {
                //Caso erro;
                console.log('Server possible not reached. Cant send data and retrieve, verify your connection.')
                console.log(r);
            }

        } catch (e) {
            $.post(urlPreffix + '/api/Sync/unLock', {}, function (r) { console.log(r); })
        }
        //finally {
        //    $.post(urlPreffix + '/api/Sync/unLock', {}, function (r) { console.log(r); })
        //}

    });
}

function SyncEnvia() {

    counter = 0;

    statusMessage('Sync started...');
    var ObjListaLevel1 = {};

    var objetoToSend = $('.level01Result').clone()
    $(objetoToSend).find('.level02Result[sync=true]').remove();

    if ($(objetoToSend).children('div').length) {
        MakeResult(ObjListaLevel1, objetoToSend);
    } else {
        GetToSyncNoOffLineData("SyncEnvia");
    }

    if (ObjListaLevel1.Root.length == 0) { FirstSync(); }

    $.each(ObjListaLevel1.Root, function (c, o) {

        var smallerObject = {};
        smallerObject['Root'] = [];
        if (!!o.correctiveactioncomplete || o.nextRoot != undefined) {
            if (o.nextRoot != undefined) {
                o['correctiveactioncomplete'] = [];
                $.each(o.nextRoot, function (cc, oo) {
                    if (!!oo.correctiveactioncomplete) {
                        var tempCA = {}
                        MakeObject2($('.Results > .correctiveAction[idcorrectiveaction=' + oo.idcorrectiveaction + ']'), 'a', tempCA);
                        o['correctiveactioncomplete'].push(tempCA.a[0]);
                        //$.each(tempCA.a, function (cc, oo) {
                        //    if (oo.level01id == o.level01id)
                        //        o['correctiveactioncomplete'] = oo;
                        //});
                    }
                });
            }
        }

        smallerObject.Root.push(o);
        smallerObject['lockPattern'] = $('.App').attr('userid');
        if (smallerObject.Root[0].nextRoot != undefined) {

            $.post(urlPreffix + "/api/Sync/SetDataAuditConsolidated", smallerObject, function (r) {
                if (r.Mensagem.indexOf('All Data') > 0) {
                    //$('.level01Result[level01Id=' + r.IdSaved + '] .level02Result').attr('sync', true)

                    var listaLevel02ComId = r.Retorno.ListToSave[0].collectionLevel02DTO;
                    listaLevel02ComId.forEach(function (o, c) {
                        var variavel = new Date(o.CollectionDate).toLocaleDateString().split('/')
                        if (variavel[0].length < 2) {
                            variavel[0] = "0" + variavel[0];
                        }
                        var data2 = variavel[0] + variavel[1] + variavel[2];
                            //09132016
                        var a = '.level01Result[level01Id=' + r.IdSaved + '] .level02Result[level02id=' + o.Level02Id + '][evaluate=' + o.EvaluationNumber + '][shift=' + o.Shift + '][period=' + o.Period + '][date=' + data2 + '][sample=' + o.Sample + '][phase=' + o.Phase + '][reaudit=' + o.ReauditIs + '][reauditNumber=' + o.ReauditNumber + ']';
                        $(a).attr('id', o.Id).attr('sync', true);
                        if (o.CorrectiveActionSaved != null) {
                            var CA = o.CorrectiveActionSaved;
                            var CaSearch = '.correctiveAction[idcorrectiveaction="' + o.CorrectiveActionId + '"][shift="' + o.Shift + '"][period="' + o.Period + '"][unidadeid="' + r.Retorno.ListToSave[0].UnitId + '"][immediatecorrectiveaction="' + CA.ImmediateCorrectiveAction + '"][preventativemeasure="' + CA.PreventativeMeasure + '"][productdisposition="' + CA.ProductDisposition + '"]';
                            $(CaSearch).attr('CollectionLevel02Id', CA.CollectionLevel02Id).attr('Id', CA.Id)
                        }
                        o.collectionLevel03DTO.forEach(function (oo, cc) {
                            $('.level01Result[level01Id=' + r.IdSaved + '] .level02Result[level02id=' + o.Level02Id + '] .level03Result[level03id=' + oo.Level03Id + ']').attr('id', oo.Id);
                        });
                    });
                }
                console.log("smallerObject metod start> ", 'background: #222; color: Red')
                //console.log(smallerObject)
                console.log(r.Mensagem);
                console.log(r);
                console.log(r.Result);
                console.log("smallerObject metod end> ", 'background: #222; color: Red')
                counter++;
                if (ObjListaLevel1.Root.length == counter) { GetToSyncNoOffLineData("SyncEnvia"); }
            });
        }
        else {
            counter++;
            if (ObjListaLevel1.Root.length == counter) { GetToSyncNoOffLineData("SyncEnvia"); }
        }
    });

    setLastSync();

}

function sendHtml(caller) {

    var htmlObject = {};
    htmlObject['html'] = $('.Results').html();
    htmlObject['idUnidade'] = $('.App').attr('unidadeid');
    htmlObject['lockPattern'] = $('.App').attr('userid');

    var stringDateApp = $('.App').attr('date');
    htmlObject['CollectionHtml'] = {
        Html: '',
        Period: parseInt($('.App').attr('period')),
        Shift: parseInt($('#shift option:selected').val()),
        CollectionDateFormatado: !!stringDateApp ? stringDateApp.substring(0, 2) + "/" + stringDateApp.substring(2, 4) + "/" + stringDateApp.substring(4, 8) : '',
        UnitId: parseInt($('.App').attr('unidadeid')),
    };

    $.post(urlPreffix + "/api/Sync/SaveHtml", htmlObject, function (r) {
        //console.log(r);
        //if (!r.MensagemExcecao) {
        //    $('.status').text('Sync finished successfully.');
        //} else {
        //    $('.status').text('Sync failed.');
        //} 
        console.log("sendHtml start caller: " + caller);
        if (!!r.Mensagem && r.MensagemExcecao == null) {
            console.log(r.Mensagem);
            statusMessage('Successfully synced.');
        }
        if (!!r.MensagemExcecao) {
            console.log(r);
            statusMessage('Sync failed.');
            return false;
        }

        if ($('.level02List').is(':visible')) {
            $('.level01.selected').click();
            //showLevel02($('.level01.selected'));
        }
        //ok
        //comentei o codigo pois estava interferindo na montagem dos resultados na tela
        //if ($('.level01List').is(':visible')) {
        //    periodReset();
        //}
        console.log("sendHtml end caller: " + caller);
        //$.post(urlPreffix + 'api/Sync/unLock')
    });
}

function FirstSync() {

    getOffLineData(true);
    //var idUnidade = $('.App').attr('unidadeid');
    //var lockPattern = $('.App').attr('userid');
    //var objToSend = { 'idUnidade': idUnidade, 'lockPattern': lockPattern }

    //$.post(urlPreffix + "/api/Sync/GetHtmlLastEntry", objToSend, function (r) {
    //    console.log(r);
    //    if (r.Retorno != null) {
    //        Merge(r);
    //        sendHtml();
    //    }
    //});

}

function GetToSyncNoOffLineData(caller) {

    var idUnidade = $('.App').attr('unidadeid');
    var lockPattern = $('.App').attr('userid');
    var objToSend = { 'idUnidade': idUnidade, 'lockPattern': lockPattern }
    objToSend['CollectionHtml'] = {
        //Html: $('.App').attr('unidadeid'),
        //Period: $('.App').attr('period'),
        Shift: $('#shift option:selected').val(),
        UnitId: $('.App').attr('unidadeid')
    }

    $.post(urlPreffix + "/api/Sync/GetHtmlLastEntry", objToSend, function (r) {
        console.log("GetToSyncNoOffLineData start caller: " + caller);
        console.log(r);
        if (r.Retorno != null) {
            Merge(r);
            sendHtml("GetToSyncNoOffLineData");
        }
        //automaticSync = setTimeout(startAutomaticSync, 120000);
        console.log("GetToSyncNoOffLineData end caller: " + caller);
    });
}

function statusMessage(message) {
    $('.status').text(message);
    setTimeout(function () {
        $('.status').empty();
    }, 10000);

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
         *                options.dialogSize - bootstrap postfix for dialog size, e.g. "sm", "m";
         *                options.progressType - bootstrap postfix for progress bar type, e.g. "success", "warning".
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


var automaticSync;

function startAutomaticSync() {
    console.log('Autosyncing...');
    if (connectionServer == true) {
       // Sync();
    }
    //sincronização a cada dois minutos
    automaticSync = setTimeout(startAutomaticSync, 30000);
}

function stopAutomaticSync() {
    if (automaticSync)
        clearTimeout(automaticSync);
}

var connectionServer = false;

function ping() {
    $.ajax({
        url: urlPreffix,
        success: function (result) {
            if (result)
                connectionServer = true;
            else
                connectionServer = false;
        },
        error: function (result) {
            connectionServer = false;
        }
    });

    setTimeout(ping, 1000);
}


function Merge(r) {

    var results = $(r.Retorno.html);

    if ($('.level01Result').length == 0) {
        appendDevice(results.html(), $('.Results'));
        return;
    }

    var objResultsLevel01 = results.children('div');

    objResultsLevel01.each(function (e) {

        var objLevel1 = $(this);

        var classe = $(objLevel1).attr('class');
        var level01id = $(objLevel1).attr('level01id');

        if (classe == 'correctiveAction') {

            var slaugthersignature = $(objLevel1).attr('slaugthersignature');
            var techinicalsignature = $(objLevel1).attr('techinicalsignature');
            var preventativemeasure = $(objLevel1).attr('preventativemeasure');
            var productdisposition = $(objLevel1).attr('productdisposition');
            var unidadeid = $(objLevel1).attr('unidadeid');
            var auditorid = $(objLevel1).attr('auditorid');
            var shift = $(objLevel1).attr('shift');
            var period = $(objLevel1).attr('period');
            //var descriptionfailure = $(objLevel1).attr('descriptionfailure');
            //var Specks = $(objLevel1).attr('Specks');
            var immediatecorrectiveaction = $(objLevel1).attr('immediatecorrectiveaction');
            var idcorrectiveaction = $(objLevel1).attr('idcorrectiveaction');
            //[descriptionfailure = ' + descriptionfailure + ']
            //[Specks = ' + Specks + ']
            var idCaSaved = $(objLevel1).attr('Id');
            var idLevel02CaSaved = $(objLevel1).attr('collectionlevel02id');

            var search = "";
            if (!!idCaSaved) {
                search = $('.correctiveAction[id=' + idCaSaved + ']');
            } else {
                search = '.correctiveAction[unidadeid="' + unidadeid + '"][auditorid="' + auditorid + '"][shift="' + shift + '"][period="' + period + '"][idcorrectiveaction="' + idcorrectiveaction + '"]';
            }

            var elem = $(search);

            if (elem.length == 0) {
                appendDevice($(objLevel1), $('.Results'));
                return;
            }

        } else {

            //var leve01id = $(objLevel1).attr('level01id');
            var evaluate = $(objLevel1).attr('evaluate');
            var reaudit = $(objLevel1).attr('reaudit');
            var period = $(objLevel1).attr('period');
            var shift = $(objLevel1).attr('shift');
            var date = $(objLevel1).attr('date');
            var unidadeid = $(objLevel1).attr('unidadeid');

            var search = '.level01Result[level01id=' + level01id + '][evaluate=' + evaluate + '][reaudit=' + reaudit + '][period=' + period + '][shift=' + shift + '][date=' + date + '][unidadeid=' + unidadeid + ']';
            var elem = $(search);

            if (elem.length == 0) {
                appendDevice($(objLevel1), $('.Results'));
                return;
            }
        }

        var objectsLevel02 = objLevel1.children('div');
        counter = 0
        objectsLevel02.each(function (e) {
            if ($(objLevel1).attr('class') == "correctiveAction") {
                return;
            }

            var objLevel2 = $(this);

            //console.log($(this));
            //console.log(counter++);

            var id = objLevel2.attr("level02id");
            var shift = objLevel2.attr("shift");
            var period = objLevel2.attr("period");
            var date = objLevel2.attr("date");
            var reaudit = objLevel2.attr("reaudit");
            var phase = objLevel2.attr("phase");
            var reauditNumber = objLevel2.attr("reauditNumber");
            var sample = objLevel2.attr("sample");
            var remove = objLevel2.attr("remove");
            
            var search = '.level01Result[evaluate=' + objLevel1.attr('evaluate') + '] .level02Result[level02id=' + id + '][shift=' + shift + '][period=' + period + '][date=' + date + '][sample=' + sample + '][phase=' + phase + '][reaudit=' + reaudit + '][reauditNumber=' + reauditNumber + ']';
            var elem = $(search);
            //Se existir objetos em level01Result > level02Result, IGUAIS aos objetos que vieram do banco level01Result > level02Result entra na conficional
            if (elem.length > 0) {

                if (elem[0].isEqualNode(objLevel2[0])) {

                }
                else {
                    //Se o elemento existe, tenho que verificar se somente o horario foi atualizado da coleta, se sim , é pq foi alterado.
                    var dataAtual = stringToTime(elem.attr('datetime'));;
                    var dataElementoDb = stringToTime($(objLevel2).attr('datetime'));
                    //Se a coleta foi alterada:
                    if (dataElementoDb > dataAtual) {
                        afterDevice($(objLevel2), elem);
                        elem.remove();
                    }
                    //o meu tem CA incompleta
                    if (!!elem.attr('havecorrectiveaction')) {
                        if (!!objLevel2.attr('correctiveActionComplete')) {
                            elem.removeAttr('havecorrectiveaction');
                            elem.attr('correctiveactioncomplete', $(objLevel2).attr('correctiveActionComplete'));
                        }
                    }
                }

            } else {

                appendDevice($(objLevel2), $('.level01Result[level01id=' + $(objLevel1).attr('level01id') + '][evaluate=' + objLevel1.attr('evaluate') + ']'));

                var ordenarIsto = $('.level01Result[level01id=' + $(objLevel1).attr('level01id') + '][evaluate=' + objLevel1.attr('evaluate') + '] .level02Result[level02id=' + id + ']')

                //if ($(objLevel1).attr('level01id') == '1') {
                ordenarElementos(ordenarIsto, "sample");
                ordenarElementos(ordenarIsto, "reaudit");
                ordenarElementos(ordenarIsto, "phase");
                //}

                var level01Ordernar = ordenarIsto.parents('.level01Result');
                $('.level01Result[level01id=' + $(objLevel1).attr('level01id') + '][evaluate=' + objLevel1.attr('evaluate') + '] .level02Result[level02id=' + id + ']').remove();
                appendDevice(ordenarIsto, level01Ordernar);

            }

        });

    });

}

function ordenarElementos(elemento, parametro) {
    elemento.sort(function (a, b) {
        var contentA = parseInt($(a).attr(parametro));
        var contentB = parseInt($(b).attr(parametro));
        return (contentA < contentB) ? -1 : (contentA > contentB) ? 1 : 0;
    });
}
//Format String = "09/05/2016 11:17".
function stringToTime(string) {
    var d1 = string.replace(/[/]/g, '').replace(/[:]/g, '').replace(/[ ]/g, '');
    return parseInt(d1);
    //
    //
}
//Deprecated
//function GetSync() {
//    $.post(urlPreffix + "/api/Sync/GetLastEntry", {}, function (r) {
//        console.log(r);
//    });
//}