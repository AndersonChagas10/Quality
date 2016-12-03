//var urlServidor = "http://192.168.25.200/SgqMaster";
//var urlLocal = "http://192.168.25.138/sgqGlobal"
////var urlLocal = "http://localhost:63128/"
//var urlPreffix = window.location.host.indexOf("host") > 0 ? urlServidor : urlServidor;

function MakeObject2(element, ListName, objectReturn) {
    //debugMessage('Start MakeObject2');
    objectReturn[ListName] = [];
    element.each(function (e) {

        var level01Result = $(this).getAttributes();
        var level02Results = $(this).children('.level02Result[sync=false]');
        level01Result['nextRoot'] = [];

        level02Results.each(function (e) {

            var level02Result = $(this).getAttributes();
            var level03Results = $(this).children('.level03Result');
            level01Result.nextRoot.push(level02Result);
            level02Result['nextnextRoot'] = [];

            level03Results.each(function (e) {

                var level03Result = $(this).getAttributes();
                level02Result.nextnextRoot.push(level03Result);

            });

        });

        objectReturn[ListName].push(level01Result);

    });

    //debugMessage('Finished MakeObject2');
}

//function MakeObject2(element, ListName, objectReturn) {
//    objectReturn[ListName] = [];
//    var elemens = element;
//    $.each(elemens, function (counter, object) {

//        if ($(object).attr('sync') == "true") { return; }

//        var temp = {};
//        var el = object;
//        for (var i = 0, atts = el.attributes, n = atts.length, arr = []; i < n; i++) {
//            var name = atts[i].nodeName;
//            var value = atts[i].nodeValue;
//            if (name == "datetime") {
//                value = value.slice(0, 16)
//            }
//            if (value == 'undefined' || value == 'null') { continue; }
//            temp[name] = value;
//        }
//        objectReturn[ListName].push(temp);
//        if (!!object.childNodes) {
//            if (object.childNodes.length > 0)
//                MakeObject2(object.childNodes, 'next' + ListName, temp);
//        }
//    });
//}
(function ($) {
    $.fn.getAttributes = function () {
        var attributes = {};
        if (this.length) {
            $.each(this[0].attributes, function (index, attr) {
                if (!(attr.value == "undefined" || attr.value == "null")) { attributes[attr.name] = attr.value; }
            });
        }

        return attributes;
    };
})(jQuery);

//function MakeResult(Obj, selector) {
//    MakeObject2(selector, 'Root', Obj);
//    console.log('Done.');
//    //console.log(Obj);
//}
function MakeResult(Obj, selector) {
    MakeObject2(selector, 'Root', Obj);
    console.log('Done.');
    //console.log(Obj);
}

//$(document).ready(function () {
//    ping();
//});

//function Sync() {
//    var objToSend = { '': $('.App').attr('userid') };
//    $.post(urlPreffix + '/api/Sync/Lock', objToSend, function (r) {

//        try {

//            //console.log(r);
//            if (r == "wait") {
//                //Esperar
//                console.log('Waiting other user to finish Sync.');
//                setTimeout(function (e) {
//                    Sync();
//                }, 800);
//            } else if (r == $('.App').attr('userid')) {
//                //Enviar
//                SyncEnvia();
//            } else {
//                //Caso erro;
//                console.log('Server possible not reached. Cant send data and retrieve, verify your connection.')
//                console.log(r);
//            }

//        } catch (e) {
//            $.post(urlPreffix + '/api/Sync/unLock', {}, function (r) { console.log(r); })
//        }
//        //finally {
//        //    $.post(urlPreffix + '/api/Sync/unLock', {}, function (r) { console.log(r); })
//        //}

//    });
//}
function Sync() {

    SyncEnvia();
    //var objToSend = { '': $('.App').attr('userid') };
    //$.post(urlPreffix + '/api/Sync/Lock', objToSend, function (r) {

    //    try {

    //        //console.log(r);
    //        if (r == "wait") {
    //            //Esperar
    //            console.log('Waiting other user to finish Sync.');
    //            setTimeout(function (e) {
    //                Sync();
    //            }, 800);
    //        } else if (r == $('.App').attr('userid')) {
    //            //Enviar
    //            SyncEnvia();
    //        } else {
    //            //Caso erro;
    //            console.log('Server possible not reached. Cant send data and retrieve, verify your connection.')
    //            console.log(r);
    //        }

    //    } catch (e) {
    //        $.post(urlPreffix + '/api/Sync/unLock', {}, function (r) { console.log(r); })
    //    }
    //    //finally {
    //    //    $.post(urlPreffix + '/api/Sync/unLock', {}, function (r) { console.log(r); })
    //    //}

    //});
}

function SyncEnvia() {

    counter = 0;
    var counterSync = 0;

    statusMessage('Sync started...');
    var ObjListaLevel1 = {};


    var objetoToSend = $('.level01Result');


    //$(objetoToSend).find('.level02Result[sync=true]').remove();

    if ($(objetoToSend).children('div[sync=false]').length) {
        MakeResult(ObjListaLevel1, objetoToSend);
    } else {
        GetToSyncNoOffLineData("SyncEnvia");
    }

    //if (ObjListaLevel1.Root.length == 0) { FirstSync(); }

    $.each(ObjListaLevel1.Root, function (c, o) {

        var smallerObject = {};
        smallerObject['Root'] = [];
        if (!!o.correctiveactioncomplete || o.nextRoot != undefined) {
            if (o.nextRoot != undefined) {
                o['correctiveactioncomplete'] = [];
                $.each(o.nextRoot, function (cc, oo) {
                    if (!!oo.correctiveActionComplete || !!oo.correctiveactioncomplete) {
                        var tempCA = {}
                        var idCorrectiveAction = oo.idcorrectiveaction;
                        if (!idCorrectiveAction) {
                            idCorrectiveAction = oo.idCorrectiveAction;
                        }
                        MakeObject2($('.Results > .correctiveAction[idcorrectiveaction=' + idCorrectiveAction + ']'), 'a', tempCA);
                        o['correctiveactioncomplete'].push(tempCA.a[0]);
                    }
                });
            }
        }

        smallerObject.Root.push(o);
        smallerObject['lockPattern'] = $('.App').attr('userid');
        if (smallerObject.Root[0].nextRoot != undefined && smallerObject.Root[0].nextRoot.length > 0) {

            $.post(urlPreffix + "/api/Sync/SetDataAuditConsolidated", smallerObject, function (r) {
                if (r.Mensagem.indexOf('All Data') > 0) {
                    //$('.level01Result[level01Id=' + r.IdSaved + '] .level02Result').attr('sync', true)
                    if(r.Retorno != null){
                        var listaLevel02ComId = r.Retorno.ListToSave[0].collectionLevel02DTO;
                        listaLevel02ComId.forEach(function (o, c) {
                            var variavel = new Date(o.CollectionDate);
                            var mes = variavel.getUTCMonth() + 1;
                            mes = variavel.getUTCMonth() + 1;
                            mes = mes.toString();
                            if (mes.length < 2) {
                                mes = "0" + mes;
                            }
                            var dia = variavel.getUTCDate().toString()
                            if (dia.length < 2) {
                                dia = "0" + dia;
                            }
                            var data2 = mes + dia + variavel.getUTCFullYear().toString();
                            //var data2 = variavel[1] + variavel[0] + variavel[2];
                            //09132016
                            var a = '.level01Result[level01Id=' + r.IdSaved + '] .level02Result[level02id=' + o.Level02Id + '][evaluate=' + o.EvaluationNumber + '][shift=' + o.Shift + '][period=' + o.Period + '][date=' + data2 + '][sample=' + o.Sample + '][phase=' + o.Phase + '][reaudit=' + o.ReauditIs + '][reauditNumber=' + o.ReauditNumber + ']';
                            $(a).attr('id', o.Id).attr('sync', true);
                            //syncModalPer(counterSync++);
                            if (o.CorrectiveActionSaved != null) {
                                var CA = o.CorrectiveActionSaved;
                                var CaSearch = '.correctiveAction[idcorrectiveaction="' + o.CorrectiveActionId + '"][shift="' + o.Shift + '"][period="' + o.Period + '"][unidadeid="' + r.Retorno.ListToSave[0].UnitId + '"][immediatecorrectiveaction="' + CA.ImmediateCorrectiveAction + '"][preventativemeasure="' + CA.PreventativeMeasure + '"][productdisposition="' + CA.ProductDisposition + '"]';
                                $(CaSearch).attr('CollectionLevel02Id', CA.CollectionLevel02Id).attr('Id', CA.Id).attr("idcorrectiveaction", CA.Id).attr('sync', true);
                                $(a).attr("idcorrectiveaction", CA.Id);
                            }
                            o.collectionLevel03DTO.forEach(function (oo, cc) {
                                $('.level01Result[level01Id=' + r.IdSaved + '] .level02Result[id=' + o.Id + '] .level03Result[level03id=' + oo.Level03Id + ']').attr('id', oo.Id);
                            });
                        });
                    } else {
                        openMessageModal(r.Mensagem + ' - ' + r.MensagemExcecao);
                        $('.level02Result').attr('sync', true);
                    }
                }
                console.log("smallerObject metod start> ", 'background: #222; color: Red')
                //console.log(smallerObject)
                console.log(r.Mensagem);
                console.log(r);
                console.log(r.Result);
                console.log("smallerObject metod end> ", 'background: #222; color: Red')
                counter++;
                if (ObjListaLevel1.Root.length == counter) { GetToSyncNoOffLineData("SyncEnvia"); }
            }).fail(function (e, h, x) {
                if(e.responseJSON)
                    openMessageModal("Error", e.responseJSON.Message);
            });
        }
        else {
            counter++;
            if (ObjListaLevel1.Root.length == counter) { GetToSyncNoOffLineData("SyncEnvia"); }
        }
    });

    setLastSync();

}

//function SyncEnvia() {
//    counter = 0;
//    statusMessage('Sync started...');
//    var ObjListaLevel1 = {};
//    var objetoToSend = $('.level01Result').clone()
//    $(objetoToSend).find('.level02Result[sync=true]').remove();
//    if ($(objetoToSend).children('div').length) {
//        MakeResult(ObjListaLevel1, objetoToSend);
//    } else {
//        GetToSyncNoOffLineData("SyncEnvia");
//    }
//    if (ObjListaLevel1.Root.length == 0) { FirstSync(); }
//    $.each(ObjListaLevel1.Root, function (c, o) {
//        var smallerObject = {};
//        smallerObject['Root'] = [];
//        if (!!o.correctiveactioncomplete || o.nextRoot != undefined) {
//            if (o.nextRoot != undefined) {
//                o['correctiveactioncomplete'] = [];
//                $.each(o.nextRoot, function (cc, oo) {
//                    if (!!oo.correctiveActionComplete || !!oo.correctiveactioncomplete) {
//                        var tempCA = {}
//                        var idCorrectiveAction = oo.idcorrectiveaction;
//                        if (!idCorrectiveAction) {
//                            idCorrectiveAction = oo.idCorrectiveAction;
//                        }
//                        MakeObject2($('.Results > .correctiveAction[idcorrectiveaction=' + idCorrectiveAction + ']'), 'a', tempCA);
//                        o['correctiveactioncomplete'].push(tempCA.a[0]);
//                        //$.each(tempCA.a, function (cc, oo) {
//                        //    if (oo.level01id == o.level01id)
//                        //        o['correctiveactioncomplete'] = oo;
//                        //});
//                    }
//                });
//            }
//        }
//        smallerObject.Root.push(o);
//        smallerObject['lockPattern'] = $('.App').attr('userid');
//        if (smallerObject.Root[0].nextRoot != undefined) {
//            $.post(urlPreffix + "/api/Sync/SetDataAuditConsolidated", smallerObject, function (r) {
//                if (r.Mensagem.indexOf('All Data') > 0) {
//                    //$('.level01Result[level01Id=' + r.IdSaved + '] .level02Result').attr('sync', true)
//                    var listaLevel02ComId = r.Retorno.ListToSave[0].collectionLevel02DTO;
//                    listaLevel02ComId.forEach(function (o, c) {
//                        var variavel = new Date(o.CollectionDate);
//                        var mes = variavel.getUTCMonth() + 1;
//                        mes = variavel.getUTCMonth() + 1;
//                        mes = mes.toString();
//                        if (mes.length < 2) {
//                            mes = "0" + mes;
//                        }
//                        var data2 = mes + variavel.getUTCDate().toString() + variavel.getUTCFullYear().toString();
//                        //var data2 = variavel[1] + variavel[0] + variavel[2];
//                        //09132016
//                        var a = '.level01Result[level01Id=' + r.IdSaved + '] .level02Result[level02id=' + o.Level02Id + '][evaluate=' + o.EvaluationNumber + '][shift=' + o.Shift + '][period=' + o.Period + '][date=' + data2 + '][sample=' + o.Sample + '][phase=' + o.Phase + '][reaudit=' + o.ReauditIs + '][reauditNumber=' + o.ReauditNumber + ']';
//                        $(a).attr('id', o.Id).attr('sync', true);
//                        if (o.CorrectiveActionSaved != null) {
//                            var CA = o.CorrectiveActionSaved;
//                            var CaSearch = '.correctiveAction[idcorrectiveaction="' + o.CorrectiveActionId + '"][shift="' + o.Shift + '"][period="' + o.Period + '"][unidadeid="' + r.Retorno.ListToSave[0].UnitId + '"][immediatecorrectiveaction="' + CA.ImmediateCorrectiveAction + '"][preventativemeasure="' + CA.PreventativeMeasure + '"][productdisposition="' + CA.ProductDisposition + '"]';
//                            $(CaSearch).attr('CollectionLevel02Id', CA.CollectionLevel02Id).attr('Id', CA.Id).attr("idcorrectiveaction", CA.Id)
//                            $(a).attr("idcorrectiveaction", CA.Id);
//                        }
//                        o.collectionLevel03DTO.forEach(function (oo, cc) {
//                            $('.level01Result[level01Id=' + r.IdSaved + '] .level02Result[level02id=' + o.Level02Id + '] .level03Result[level03id=' + oo.Level03Id + ']').attr('id', oo.Id);
//                        });
//                    });
//                }
//                console.log("smallerObject metod start> ", 'background: #222; color: Red')
//                //console.log(smallerObject)
//                console.log(r.Mensagem);
//                console.log(r);
//                console.log(r.Result);
//                console.log("smallerObject metod end> ", 'background: #222; color: Red')
//                counter++;
//                if (ObjListaLevel1.Root.length == counter) { GetToSyncNoOffLineData("SyncEnvia"); }
//            });
//        }
//        else {
//            counter++;
//            if (ObjListaLevel1.Root.length == counter) { GetToSyncNoOffLineData("SyncEnvia"); }
//        }
//    });
//    setLastSync();
//}
function updateWindow() {

    if ($('.level02List').is(':visible')) {
        var level01 = $('.level01.selected');
        var result = $('.level01Result.selected');

        if (!result.length) {

            result = getLastResultPeriod(level01);

        }
        periodReset();
        resultPreencher(result);
        showLevel02(level01);
        //else {
        //    $('.level01.selected').click();
        //}
        //showLevel02($('.level01.selected'));
    }
    else if ($('.level01List').is(':visible')) {
        $('#selectPeriod').trigger('change');
    }

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
        mensagemSyncHide();
        //ok
        //comentei o codigo pois estava interferindo na montagem dos resultados na tela
        //if ($('.level01List').is(':visible')) {
        //    periodReset();
        //}
        console.log("sendHtml end caller: " + caller);
        //$.post(urlPreffix + 'api/Sync/unLock')
    });
}


/********************************************************************************************************
****BUG****BUG****BUG****BUG****BUG****BUG****BUG****BUG****BUG****BUG****BUG****BUG****BUG****BUG*******
********************************************************************************************************/
////////////////function FirstSync() {

////////////////    getOffLineData(true);
////////////////    //var idUnidade = $('.App').attr('unidadeid');
////////////////    //var lockPattern = $('.App').attr('userid');
////////////////    //var objToSend = { 'idUnidade': idUnidade, 'lockPattern': lockPattern }

////////////////    //$.post(urlPreffix + "/api/Sync/GetHtmlLastEntry", objToSend, function (r) {
////////////////    //    console.log(r);
////////////////    //    if (r.Retorno != null) {
////////////////    //        Merge(r);
////////////////    //        sendHtml();
////////////////    //    }
////////////////    //});

////////////////}
/********************************************************************************************************
****FIM BUG****FIM BUG****FIM BUG****FIM BUG****FIM BUG****FIM BUG****FIM BUG****FIM BUG****FIM BUG******
********************************************************************************************************/


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

//função de sincronização comentada. Pessoal da JBS USA solicitou tirar
//var automaticSync;

//function startAutomaticSync() {
//    console.log('Autosyncing...');
//    if (connectionServer == true) {
//        if ($('.level03Group:visible').length == 0 && $('#correctiveActionModal:visible').length == 0) {
//            Sync();
//        }

//        //sincronização a cada trinta segundos
//        automaticSync = setTimeout(startAutomaticSync, 60000);
//    }
//}

//function stopAutomaticSync() {
//    if (automaticSync)
//        clearTimeout(automaticSync);
//}


var connectionServer = false;

var timeoutPing;
function ping() {
    if (urlPreffix == "")
    {
        connectionServer = false;
        return false;
    }
    $.ajax({
        type: "GET",
        url: urlPreffix + '/api/LoginApi/Logado',
        error: function(data)
        {
            connectionServer = false;
        },
        success: function (data) {
            connectionServer = true;
        },
        timeout: 5000 //in milliseconds
    });

   //timeoutPing = setTimeout(function (e) {

   //     $.get(urlPreffix + '/api/LoginApi/Logado', function (data, status) {
   //         if (data == 'ok') {
   //             connectionServer = true;
   //         }
   //         else {
   //             connectionServer = true;
   //         }
   //     }).fail(function () {
   //         connectionServer = false;
   //     });

   // });

    ////connectionServer = true;
    //$.ajax({
    //    type: 'GET',
    //    url: urlPreffix + '/api/LoginApi/Logado',
    //    success: function (data, textStatus, xhr) {
    //        if(data == "ok")
    //        {
    //            connectionServer = true;
    //        }
    //        else
    //        {
    //            connectionServer = false;
    //        }
    //    },
    //    error: function (data, textStatus, xhr) {
    //        connectionServer = false;
    //    },
    //    timeout: 8000 // sets time
    //});

    //setTimeout(ping, 5000);
}


function Merge(r) {

    //Pego os resultados que vem da sincornização.
    var results = $(r.Retorno.html);

    //Se não existe nenhum level01Result.
    if ($('.level01Result').length == 0) {
        //Adiciono os resultados da sincronização aos resultados da tela.

        //coloquei essa variavel pq estou limpando pq tá chegando com level02Result na raiz...verifica pq
        var resultsValidos = results.children('.level01Result, .correctiveAction')

        appendDevice(resultsValidos, $('.Results'));
        removeElementosNaoUtilizados();
        //Saio do método.
        return true;
    }


    //Separamos os objetos que vem a partir da sincronização
    var objResultsLevel01 = results.children('div');

    //Percorro os objetos
    objResultsLevel01.each(function (e) {

        //tem hora que tem resultado de level02Result, então por hora se tiver sai do loop


        //Instanciamos o objeto da lista
        var objLevel1 = $(this);


        //Pegamos o atributo Class
        var classe = $(objLevel1).attr('class');
        //Pegamos o level01Id
        var level01id = $(objLevel1).attr('level01id');

        //if (level01id) {  }

        if (classe == 'level02Result') {

            return true;
        }


        //Se o objeto for uma ação corretiva
        if (classe == 'correctiveAction') {

            var unidadeId = $(objLevel1).attr('unidadeid');
            var auditorId = $(objLevel1).attr('auditorid');
            var shift = $(objLevel1).attr('shift');
            var period = $(objLevel1).attr('period');
            var idCorrectiveAction = $(objLevel1).attr('idcorrectiveaction');
            var idCorrectiveActionDB = $(objLevel1).attr('Id');
            var idLevel02CaSaved = $(objLevel1).attr('collectionlevel02id');

            var searchCA = "";

            //Se Existe o idCorrectiveActionDB
            if (!!idCorrectiveActionDB) {
                //Procuramos pelo id
                searchCA = $('.correctiveAction[id=' + idCorrectiveActionDB + ']');
            } else {
                //Se não existir procuramos pelos atributos
                searchCA = '.correctiveAction[unidadeid="' + unidadeId + '"][auditorid="' + auditorId + '"][shift="' + shift + '"][period="' + period + '"][idcorrectiveaction="' + idCorrectiveAction + '"]';
            }


            var correctiveAction = $(searchCA);


            //Se não existe ação corretiva incluir nos resultadoz
            if (correctiveAction.length == 0) {
                appendDevice(objLevel1, $('.Results'));
                return;
            }

            ////

            ///Colocar o else da ação corretiva para comparar
            ///


        } else {

            //Se o objeto não for uma ação corretiva
            var evaluate = $(objLevel1).attr('evaluate');
            var reaudit = $(objLevel1).attr('reaudit');
            var period = $(objLevel1).attr('period');
            var shift = $(objLevel1).attr('shift');
            var date = $(objLevel1).attr('date');
            var unidadeid = $(objLevel1).attr('unidadeid');

            //Pesquisa pelos atributos
            var searchL01R = '.level01Result[level01id=' + level01id + '][evaluate=' + evaluate + '][reaudit=' + reaudit + '][period=' + period + '][shift=' + shift + '][date=' + date + '][unidadeid=' + unidadeid + ']';
            var level01Result = $(searchL01R);

            //Se não existir o elemento
            if (level01Result.length == 0) {
                //Adiciona nos resultados

                //var level01ResultObjects = $('.level01Result[level01id=' + objLevel1.attr('level01id')  + '][date=' + objLevel1.attr('date') + '][shift=' + objLevel1.attr('shift') + '][period=' + objLevel1.attr('period') + ']');

                //var evaluateObjBanco = parseInt(objLevel1.attr('evaluate'));
                //var reauditObjBanco = objLevel1.attr('reaudit');
                //var reauditNumberObjBanco = parseInt(objLevel1.attr('reauditnumber'));



                //var evalueteResults = 0;
                //var reauditResult = false;
                //var reauditNumberResult = 0;

                //if (reauditNumberObjBanco == "true" && reauditNumberObjBanco == 1)
                //{
                //    reauditResult = false;
                //    reauditNumberResult = 0;
                //}
                //else if (reauditNumberObjBanco == "true" && reauditNumberObjBanco > 1)
                //{
                //    reauditResult = true;
                //    reauditNumberResult = reauditNumberResult - 1;
                //}


                //if (reauditObjBanco == "true" )
                //{
                //    if(evaluateObjBanco > 1)
                //    {
                //        evalueteResults = evaluateObjBanco - 1;
                //    }

                //    if(reauditNumberObjBanco > 1)
                //    {
                //        reauditNumberResult = reauditNumberObjBanco - 1;
                //        reauditResult = true;
                //    }
                //    else
                //    {
                //        reauditResult = false;
                //        reauditNumberResult = 0;
                //    }
                //}



                //meu objeto do banco é um reauditoria e o numero 1 um  ------------tenho quue proceurar um objeto que não é reuaditoria
                //meu objeto é uma reaudit é o numero é maior que 1 ---------------- procuro uma reauditoria menor para colocar o objeto depois
                //tenho que verificar a evaluate para colocar no lugar correto
                //se a avaliação for um eu procuro uma avlaiacao maior
                //se a avaliação for maior que 1 procuro a menor

                //se nao existir essa regra coloca no append mesmo

                //level01ResultObjects.each(function (e) {


                //});

                appendDevice(objLevel1, $('.Results'));
            }
            else if (level01Result.length == 1) {


                if (level01Result[0].isEqualNode(objLevel1[0])) {
                    return;
                }
                else {
                    var substituirlevel01Result = false;

                    var dataAtual = stringToTime(level01Result.attr('datetime'));
                    var dataElementoDb = stringToTime(objLevel1.attr('datetime'));

                    var completedSampleDB = objLevel1.attr('completedsample');
                    var completedSampleResult = level01Result.attr('completedsample');

                    var completeDB = objLevel1.attr('completed');
                    var completeResult = level01Result.attr('completed');


                    var sampleDB = parseInt(objLevel1.attr('lastsample'));
                    var sampleResult = parseInt(level01Result.attr('lastsample'));



                    //Se a amostra for completa no banco e não for completa no resultado.
                    if (!!completedSampleDB && !(!!completedSampleResult)) {
                        //Subistitui o resultado.
                        substituirlevel01Result = true;
                    }

                    //Se o resultado for completo no banco e não for completa no resultado.
                    if (!!completeDB && !(!!completeResult)) {
                        //Subistitui o resultado.
                        substituirlevel01Result = true;
                    }


                    if (substituirlevel01Result == false && sampleDB > sampleResult) {
                        substituirlevel01Result = true;
                    }
                    //Se a dataObjeto for maior
                    if (dataAtual > dataElementoDb) {
                        substituirlevel01Result = false;
                    }
                    if (substituirlevel01Result == true) {
                        //Adiciono o elemento do banco após o elemento do aplicativo.
                        afterDevice(objLevel1, level01Result);
                        //Removo o elemento do aplicado.
                        level01Result.remove();
                    }
                    else {
                        MergeLevel02Result(objLevel1, level01Result);
                    }

                }
                //Se a coleta foi alterada:

                //Se a data do elemento que veio do banco de dados for maior que a data do elemento do resultado no aplicativo.

            }
        }

    });

    removeElementosNaoUtilizados();

}

function removeElementosNaoUtilizados() {
    var level01Check = $('.level01');
    var periods = $('#selectPeriod option[value!=0]');

    level01Check.each(function (e) {

        var level01 = $(this);

        periods.each(function (e) {

            var period = $(this).attr('value');

            if (level01.attr('havephases')) {
                var results = $('.Results .level01Result[level01id=' + level01.attr('id') + '][period=' + period + '][reaudit=false]:not(:last)');
                var resultLast = $('.Results .level01Result[level01id=' + level01.attr('id') + '][period=' + period + '][reaudit=false]:last');

                var resultsReaudit = $('.Results .level01Result[level01id=' + level01.attr('id') + '][period=' + period + '][reaudit=true]:not(:last)');
                var resultLastReaudit = $('.Results .level01Result[level01id=' + level01.attr('id') + '][period=' + period + '][reaudit=true]:last');

                results.removeClass('selected');
                resultLast.removeClass('selected');

                resultsReaudit.removeClass('selected');
                resultLastReaudit.removeClass('selected');


                results.each(function (e) {
                    var result = $(this);
                    if (!result.attr('havecorrectiveaction') && !result.attr('havereaudit') && (result.attr('completed') || (result.attr('completedsample') && resultLast.attr('completed'))) && !result.children('div[sync=false]').length) {
                        result.remove();
                    }
                });

                resultsReaudit.each(function (e) {
                    var result = $(this);
                    if (!resultsReaudit.attr('havecorrectiveaction') && !resultsReaudit.attr('havereaudit') && (resultsReaudit.attr('completed') || (resultsReaudit.attr('completedsample') && resultLastReaudit.attr('completed'))) && !resultsReaudit.children('div[sync=false]').length) {
                        resultsReaudit.remove();
                    }
                });
            }
            else {
                var results = $('.Results .level01Result[level01id=' + level01.attr('id') + '][period=' + period + ']:not(:last)');
                var resultLast = $('.Results .level01Result[level01id=' + level01.attr('id') + '][period=' + period + ']:last');
                results.removeClass('selected');

                results.each(function (e) {
                    var result = $(this);
                    if (!result.attr('havecorrectiveaction') && !result.attr('havereaudit') && (result.attr('completed') || (result.attr('completedsample') && resultLast.attr('completed'))) && !result.children('div[sync=false]').length) {
                        result.remove();
                    }
                });
            }
            //if (results.length > 0) {

            //    results.find('[havecorrectiveaction!=havecorrectiveaction][havereaudit!=havereaudit]').remove();
            //}

        });

        $('.correctiveAction[sync=true]').remove();
    });

    sendHtml("GetToSyncNoOffLineData");
    createFileResult();
    updateWindow();
}

function MergeLevel02Result(objLevel1, level01Result) {
    //Se existe o Level01Result Instanciamos os objetos do level02
    var objectsLevel02 = objLevel1.children('div');
    var atualizaDataLevel01 = false;

    //// Instancio um contador
    //var counter = 0

    //Percorremos os level02Result
    objectsLevel02.each(function (e) {

        //if ($(objLevel1).attr('class') == "correctiveAction") {
        //    return;
        //}

        //Instancio um objeto do level02Result
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
        var evaluate = objLevel2.attr('evaluate');
        var sample = objLevel2.attr("sample");
        //var remove = objLevel2.attr("remove");



        // var search = '.level01Result[evaluate=' + objLevel1.attr('evaluate') + '] .level02Result[level02id=' + id + '][shift=' + shift + '][period=' + period + '][date=' + date + '][sample=' + sample + '][phase=' + phase + '][reaudit=' + reaudit + '][reauditNumber=' + reauditNumber + ']';
        // var elem = $(search);

        //Verifico se o Level01Result te o Level02Result
        var level02Result = level01Result.children('.level02Result[level02id=' + id + '][shift=' + shift + '][period=' + period + '][date=' + date + '][sample=' + sample + '][phase=' + phase + '][reaudit=' + reaudit + '][reauditNumber=' + reauditNumber + ']');
        //Se existir objetos em level01Result > level02Result, IGUAIS aos objetos que vieram do banco level01Result > level02Result entra na conficional

        //Se existe o elemento do Level02Result
        if (level02Result.length > 0) {

            //Caso os dois objetos forem iguais
            if (level02Result[0].isEqualNode(objLevel2[0])) {
                //Não faz nada
            }
            else {


                var dataAtual = stringToTime(level02Result.attr('datetime'));
                var dataElementoDb = stringToTime(objLevel2.attr('datetime'));
                //Se a coleta foi alterada:

                //Se a data do elemento que veio do banco de dados for maior que a data do elemento do resultado no aplicativo.
                if (dataElementoDb > dataAtual) {

                    //Adiciono o elemento do banco após o elemento do aplicativo.
                    afterDevice(objLevel2, level02Result);
                    //Removo o elemento do aplicado.
                    level02Result.remove();
                }
                else {

                }
                //Verificar se precida do bloco de codigo comentado abaixo
                //if (!!level02Result.attr('havecorrectiveaction')) {
                //    if (!!objLevel2.attr('correctiveActionComplete')) {
                //        level02Result.removeAttr('havecorrectiveaction');
                //        level02Result.attr('correctiveactioncomplete', $(objLevel2).attr('correctiveActionComplete'));
                //    }
                //}
                //Fim do bloco de código


            }

        } else {

            //Se não existe o level02Result adiciono ele no local correto

            var sampleObjDB = parseInt(objLevel2.attr('sample'));


            var dataAtual = stringToTime(level01Result.attr('datetime'));
            var dataElementoDb = stringToTime(objLevel1.attr('datetime'));



            var resultados = level01Result.children('.level02Result');
            resultados.each(function (e) {

                var result = $(this);
                var sampleObjResult = parseInt(result.attr('sample'));

                //1
                //2
                if (sampleObjDB < sampleObjResult) {
                    beforeDevice(objLevel2, result);
                    atualizaDataLevel01 = true;
                    return false;
                }
                else if (sampleObjDB > sampleObjResult) {
                    atualizaDataLevel01 = true;
                    afterDevice(objLevel2, result);
                    return false;
                }
                else if (sampleObjDB = sampleObjResult) {
                    atualizaDataLevel01 = true;
                    appendDevice(objLevel2, level01Result);
                    return false;
                }
                else {
                    console.log('Merge não previsto');
                }

            });


            //appendDevice(objLevel2, level01Result);

            //var ordenarIsto = $('.level01Result[level01id=' + $(objLevel1).attr('level01id') + '][evaluate=' + objLevel1.attr('evaluate') + '] .level02Result[level02id=' + id + ']')

            ////if ($(objLevel1).attr('level01id') == '1') {
            //ordenarElementos(ordenarIsto, "sample");
            //ordenarElementos(ordenarIsto, "reaudit");
            //ordenarElementos(ordenarIsto, "phase");
            ////}

            //var level01Ordernar = ordenarIsto.parents('.level01Result');
            //$('.level01Result[level01id=' + $(objLevel1).attr('level01id') + '][evaluate=' + objLevel1.attr('evaluate') + '] .level02Result[level02id=' + id + ']').remove();
            //appendDevice(ordenarIsto, level01Ordernar);

        }

    });
    if (atualizaDataLevel01 == true) {
        level01Result.attr('datetime', objLevel1.attr('datetime'));
    }
    if (objLevel1.attr('correctiveactioncomplete')) {
        level01Result.attr('correctiveactioncomplete', objLevel1.attr('correctiveactioncomplete')).removeAttr('havecorrectiveaction');
    }

    if (objLevel1.attr('completereaudit')) {
        level01Result.attr('completereaudit', objLevel1.attr('completereaudit')).removeAttr('havereaudit');
    }


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
}
function cleanResults() {
    var results = $('.Results .level01Result, .Results .correctiveAction');
    appendDevice(results, $('.Results'));
}