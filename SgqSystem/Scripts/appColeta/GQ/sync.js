var totalObj = 0; //Verifica total de Level02 a ser sincornizado
var objsyncked = 0; //Ojetos j� sincronizados
var qtdeObjetos = 10; //Quantidade de Objetos enviados por sincroniza��o

/// Fun��o que Inicializa a Sincroniza��o
function sendResults() {
    ping();
    if (connectionServer != true) {
        //createLog("Server not found")
        openMessageModal(getResource("server_not_found"), getResource("not_connected_to_network"));
        return true;
    }
    totalObj = $('.level02Result[sync=false]').length;
    totalObj += $('.correctiveAction[sync=false]').length;
    if (totalObj == 0) {
        menssagemSync(getResource("no_data_to_send"), getResource("everything_synchronized_sucessfully") + "!");
        setTimeout(function (e) {
            reciveResults();
        }, 1500);
        return true;
    }
    objsyncked = $('.level02Result[sync=false][send=true]').length; //Zera a variavel de objetos sincronizados
    totalObj = $('.level02Result[sync=false]').length; //verificamos o total de level02 a ser sincronizado
    menssagemSync(getResource("data_collection"), getResource("verifying_the_collected_data")); // Mostra o modal informando que est� verificando as coletas
    $('#btnMessageOk').removeClass('hide');
    initializeSync(100); //Inicializa o Envio dos Resultados
}
function sendResultsOnLine() {
    sincronizarResultadoPCC1B();

    if (terminouDeEnviar)
        return;

    if (isEUA)
        return;

    //clearTimeout(timeoutSendResults); //Paramos o m�todo de enviar automatico pois est� enviando resultado

    preparing(true);
    send(true);

}
function initializeSync(time) {
    setTimeout(function (e) {
        //Prepara a quantidade de objetos que devem ser sincronizados conforme defini��o na vari�vel qtdeObjetos
        preparing(true);
        $('.message .body').html(getResource("preparing_the_data_to_send") + "..."); //Altera mensagem para preaprando envio;

        setTimeout(function (e) {

            objsyncked = objsyncked + $('.level02Result[sync=false][send=true]').length;
            $('.message .body').html(getResource('sending') + " " + getResource('results'));//+ objsyncked + " " + getResource('of') + " " + totalObj + " " + getResource('results').toLowerCase()); //Mostra Qtde de Objeros que j� foram enviados
            $('#btnMessageOk').removeClass('hide');

            setTimeout(function (e) {
                send(); //Envia a informa��o
            }, 1000);
        }, 1000);
    }, time ? time : 60000);
}

function preparing(showMessage) {

    if (terminouDeEnviar) {
        mensagemSyncHide();
        return;
    }

    var arrayResultsSend = $('.level02Result[sync=false]:lt('+qtdeObjetos+')').slice(0, qtdeObjetos);
    arrayResultsSend.attr('send', 'true');

}

var terminouDeEnviar = false;

function send(autoSend, callbackPCC1B, sendImediato) {

    if (terminouDeEnviar) {
        mensagemSyncHide();
        return;
    } else {
        terminouDeEnviar = true;
    }

    var objectSend = "";

    // objectSend['Level02'] = [];
    if (autoSend != true) {
        autoSend = false;
    }

    var quantidadeDeObjetosParaSincronizar = qtdeObjetos;

    $('.level02Result[sync=false][send=true]').each(function (e) {

        //BREAK NO FOREACH PARA LIMITAR A ENVIAR APENAS O NUMERO DEFINIDO DE LEVEL2
        if(quantidadeDeObjetosParaSincronizar == 0)
            return false;
        quantidadeDeObjetosParaSincronizar--;

        var level02Result = "";

        var level02 = $(this);
        var level01 = level02.parents('.level01Result');
        var hasReason = false;

        level02Result += RetornaValor0SeUndefined(level02.attr('level01id'));//[0]
        level02Result += ";" + RetornaValor0SeUndefined(level01.attr('datetime'));//[1]
        level02Result += ";" + RetornaValor0SeUndefined(level02.attr('level02id'));//[2]
        level02Result += ";" + RetornaValor0SeUndefined(level02.attr('datetime'));//[3]
        level02Result += ";" + RetornaValor0SeUndefined(level01.attr('unidadeid'));//[4]
        level02Result += ";" + RetornaValor0SeUndefined(level01.attr('period'));//[5]
        level02Result += ";" + RetornaValor0SeUndefined(level01.attr('shift')); //[6]

        level02Result += ";" + RetornaValor0SeUndefined(level02.attr('auditorid')); //7]ok
        level02Result += ";" + RetornaValor0SeUndefined(level02.attr('phase')); //[8] ok
        level02Result += ";" + RetornaValor0SeUndefined(level02.attr('reaudit')); //[9]ok
        level02Result += ";" + RetornaValor0SeUndefined(level02.attr('startphasedate')); //[10]ok
        level02Result += ";" + RetornaValor0SeUndefined(level02.attr('evaluate'));//[11]ok
        level02Result += ";" + RetornaValor0SeUndefined(level02.attr('sample'));//[12]ok
        level02Result += ";" + RetornaValor0SeUndefined(level02.attr('headerlist'));//[13]ok
        level02Result += ";" + RetornaValor0SeUndefined(level02.attr('isemptylevel3'));//[14]ok
        level02Result += ";" + RetornaValor0SeUndefined(level02.attr('hassampletotal'));//[15]ok
        level02Result += ";"; //[16]ok
        level02Result += ";" + RetornaValor0SeUndefined(level02.attr('consecutivefailurelevel')); //[17] ok
        level02Result += ";" + RetornaValor0SeUndefined(level02.attr('consecutivefailuretotal')); //[18] ok
        level02Result += ";" + RetornaValor0SeUndefined(level02.attr('notavaliable')); //[19]ok
        level02Result += ";" + RetornaValor0SeUndefined(versao); //[20]
        level02Result += ";" + RetornaValor0SeUndefined(baseAmbiente); //[21]

        var level03Result = $(this).children('.level03Result');
        var resultLevel03 = "";

        var ParReason_Id = null;
        var ParReasonType_Id = null;

        var parDepartment_Id = null;

        level03Result.each(function (e) {

            var level03 = $(this);

            var result = RetornaValor0SeUndefined(level03.attr('level03id')); //[0]
            result += "," + RetornaValor0SeUndefined(level03.attr('date')); //[1]


            var valueConfigurado = 0;

            var valorCientifico = level03.attr('value');
            if (valorCientifico.indexOf("x10^") >= 0) {
                valueConfigurado = converteNotacaoBaseDezParaDecimal(RetornaValor0SeUndefined(level03.attr('value'))); //[2]
            } else {
                valueConfigurado = RetornaValor0SeUndefined(level03.attr('value')); //[2]
            }

            result += "," + (valueConfigurado != null ? valueConfigurado.toString().replace(",", ".") : valueConfigurado) //[2]

            var vvalor = parseFloat(valueConfigurado.toString().replace(",", "."));
            var vmin = parseFloat(RetornaValor0SeUndefined(level03.attr('intervalmin')).replace(",", "."));
            var vmax = parseFloat(RetornaValor0SeUndefined(level03.attr('intervalmax')).replace(",", "."));

            if (vvalor >= vmin && vvalor <= vmax && parseFloat((level03.attr('weidefects') ? level03.attr('weidefects') : "").replace(",", ".")) == 0) {

                result += "," + true; //level03.attr('defects');    

            } else {

                try {
                    result += "," + RetornaValor0SeUndefined(level03.attr('conform')); //[3];
                } catch (e) {
                    result += "," + 'false';//level03.attr('defects'); 
                }

            }


            result += "," + RetornaValor0SeUndefined(level03.attr('auditorid')); //[4]
            result += "," + RetornaValor0SeUndefined(level03.attr('totalerror')); //[5]
            result += "," + RetornaValor0SeUndefined(escape(level03.attr('valuetext'))); //[6]
            result += "," + RetornaValor0SeUndefined(level03.attr('id')); //[7]
            result += "," + RetornaValor0SeUndefined(level03.attr('weight')).replace(",", "."); //[8]
            result += ","; //[9] Aqui era o name do level3
            result += "," + RetornaValor0SeUndefined(level03.attr('intervalmin')).replace(",", "."); //[10]
            result += "," + RetornaValor0SeUndefined(level03.attr('intervalmax')).replace(",", "."); //[11]
            result += "," + RetornaValor0SeUndefined(level03.attr('isnotevaluate')); //[12]
            result += "," + RetornaValor0SeUndefined(level03.attr('punishmentvalue')); //[13]

            var vvalor = parseFloat(RetornaValor0SeUndefined(valueConfigurado).toString().replace(",", "."));
            var vmin = parseFloat(RetornaValor0SeUndefined(level03.attr('intervalmin')).replace(",", "."));
            var vmax = parseFloat(RetornaValor0SeUndefined(level03.attr('intervalmax')).replace(",", "."));


            //defects 14
            if (vvalor >= vmin && vvalor <= vmax && parseFloat((level03.attr('weidefects') ? level03.attr('weidefects') : "").replace(",", ".")) == 0) {

                result += "," + '0'; //level03.attr('defects');    

            } else {

                try {
                    result += "," + RetornaValor0SeUndefined(level03.attr('defects'));
                } catch (e) {
                    result += "," + '1';//level03.attr('defects'); 
                }
            }

            try {
                result += "," + RetornaValor0SeUndefined(level03.attr('weievaluation')).replace(",", "."); //[15]
            } catch (e) {
                level02Result += ";" + '0';
            }
            try {
                result += "," + RetornaValor0SeUndefined(level03.attr('weidefects')).replace(",", ".");//[16]
            } catch (e) {
                level02Result += ";" + '0';
            }

            if (level03.attr('ParReasonId')) {
                hasReason = true;
                ParReason_Id = RetornaValor0SeUndefined(level03.attr('ParReasonId'));
                ParReasonType_Id = RetornaValor0SeUndefined(level03.attr('ParReasonType_Id'));
            }

            if (level03.attr('parDepartmentId')) {
                parDepartment_Id = RetornaValor0SeUndefined(level03.attr('parDepartmentId'));
            }

            resultLevel03 += "<level03>" + result + "</level03>";

        });

        level02Result += ";" + resultLevel03; //[22]

        var correctiveActionResult = "";

        level02Result += ";" + RetornaValor0SeUndefined(correctiveActionResult); //[23]
        level02Result += ";" + RetornaValor0SeUndefined(level02.attr('havereaudit')); //[24]
        level02Result += ";" + RetornaValor0SeUndefined(level02.attr('havecorrectiveaction')); //[25]
        level02Result += ";" + RetornaValor0SeUndefined(level02.attr('reauditnumber'));//[26]
        level02Result += ";" + RetornaValor0SeUndefined(level01.attr('alertaatual')); // [27]
        level02Result += ";" + RetornaValor0SeUndefined(level01.attr('completed'));//[28]
        level02Result += ";" + RetornaValor0SeUndefined(level02.attr('havephases')); //r[29]
        level02Result += ";" + RetornaValor0SeUndefined(level02.attr('id')); //r[30]
        level02Result += ";" + RetornaValor0SeUndefined(level02.attr('correctiveactioncomplete')); //r[31]
        level02Result += ";" + RetornaValor0SeUndefined(level02.attr('completereaudit')); //r[32]
        level02Result += ";" + RetornaValor0SeUndefined(level01.attr('hashkey')); // [33]
        level02Result += ";0";//[34]
        try {
            level02Result += ";" + RetornaValor0SeUndefined(level02.attr('weievaluation').replace(",", ".")); //[35]
        } catch (e) {
            level02Result += ";" + '0'; //[35]
        }
        try {
            level02Result += ";" + RetornaValor0SeUndefined(level02.attr('defects').replace(",", ".")); // [36]
        } catch (e) {
            level02Result += ";" + '0';
        }
        try {
            level02Result += ";" + RetornaValor0SeUndefined(level02.attr('weidefects').replace(",", "."));// [37]
        } catch (e) {
            level02Result += ";" + '0';
        }
        try {
            level02Result += ";" + RetornaValor0SeUndefined(level02.attr('totallevel3withdefects').replace(",", ".")); // r[38]
        } catch (e) {
            level02Result += ";" + '0';
        }
        try {
            level02Result += ";" + RetornaValor0SeUndefined(level02.attr('totalLevel3evaluation').replace(",", ".")); // r[39]
        } catch (e) {
            level02Result += ";" + '0';
        }
        //level02Result += ";" + level01.attr('alertaatual').replace(",", "."); // r[40]
        level02Result += ";" + RetornaValor0SeUndefined(level01.attr('avaliacaoultimoalerta')); // r[40]
        level02Result += ";" + RetornaValor0SeUndefined(level02.attr('resultadoavaliado')); // r[41]
        level02Result += ";" + RetornaValor0SeUndefined(level02.attr('resultadodefeitos')); // r[42]
        level02Result += ";" + RetornaValor0SeUndefined(level02.attr('sequential')); // r[43]
        level02Result += ";" + RetornaValor0SeUndefined(level02.attr('side')); // r[44]
        level02Result += ";" + RetornaValor0SeUndefined(level01.attr('monitoramentoultimoalerta')); // r[45]
        level02Result += ";" + RetornaValor0SeUndefined(level02.attr('reauditlevel')); // r[46]
        level02Result += ";" + RetornaValor0SeUndefined(level02.attr('startphaseevaluation')); // r[47]
        level02Result += ";" + RetornaValor0SeUndefined(level02.attr('endphaseevaluation')); // r[48]

        if (level02.attr('objReprocesso'))
            level02Result += ";" + RetornaValor0SeUndefined(level02.attr('objReprocesso')); // r[49]
        else {
            level02Result += ";";
        }

        if (hasReason) {
            level02Result += ";" + RetornaValor0SeUndefined(ParReason_Id); //[50]
            level02Result += ";" + RetornaValor0SeUndefined(ParReasonType_Id); //[50]
        } else {
            level02Result += ";";
            level02Result += ";";
        }

        if (parDepartment_Id) {
            level02Result += ";" + RetornaValor0SeUndefined(parDepartment_Id); //[50]
        } else {
            level02Result += ";";
        }

        objectSend += "<level02>" + level02Result + "</level02>";

    });
    //createLog("Send Json Object");

    $.ajax({
        type: 'POST'
        , url: urlPreffix + '/api/SyncServiceApi/InsertJson'
        , contentType: 'application/json; charset=utf-8'
        , dataType: 'json'
        , data: '{' + "ObjResultJSon: '" + objectSend + "', deviceId: '" + device.uuid + "', autoSend: " + autoSend + ", deviceMac: ''" + '}'
        , async: true //blocks window close
        , headers: token()
        , success: function (data, status) {
            if (data != null && data == "error") {
                //createLog(XMLHttpRequest.responseText);
                mensagemSyncHide();
                if (!autoSend)
                    openMessageModal(getResource('synchronization_error'), getResource('try_again_contact_support'));
            }
            else {
                $('.level02Result[sync=false][send=true]').removeAttr('send').attr('sync', 'true');
                $('.level02Result[sync=true]').remove();
                createFileResult(false);
                if (autoSend != true) {
                    objsyncked = objsyncked + $('.level02Result[sync=false][send=true]').length;
                    $('.message .body').html(getResource('sending') + " " + getResource('results'));//+ " " + objsyncked + "/" + totalObj);
                    menssagemSync(getResource('data_collection'), getResource('verifying_the_collected_data'));
                    $('#btnMessageOk').removeClass('hide');
                    setTimeout(function (e) {
                        if ($('.level02Result[sync=false]').length) {
                            initializeSync(1000);
                        }
                        else {
                            menssagemSync(getResource('consolidation'), getResource('data_consolidated'));
                            setTimeout(function (e) {
                                mensagemSyncHide();
                                reciveResults();
                            }, 100);
                        }
                    }, 1000);//
                }
                else if (autoSend == true) {
                    //Ativa o m�todo de enviar resultados automatico novamente
                    //Se estiver on line primeiro verifica se tem a��o corretiva
                    // if ($('.level3Group.PCC1B').is(':visible')) {
                    //     getPCC1BNext();
                    // }
                    if (sendImediato == true) {
                        console.log("Sincronizado instantaneamente");
                    }
                }

                if ($('.level02Result[sync=false]').length == 0) {
                    ping(sendCorrectiveActionOnLine, sendResultsTimeout);
                    sendResultLevel3Photo();
                }
            }
            terminouDeEnviar = false;
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            terminouDeEnviar = false;
            sendResultsTimeout();
            //createLog(XMLHttpRequest.responseText);
            mensagemSyncHide();
            if (!autoSend)
                openMessageModal(getResource("synchronization_error"), getResource("try_again_contact_support"));
        }
    });
}
function menssagemSync(title, content) {
    var mensagem = $('.message, .overlay');
    $('.message').removeClass('alertRed');
    mensagem.children('.head').html(title);
    mensagem.children('.body').html(content);
    mensagem.addClass('msgSync');
    mensagem.fadeIn("fast");
    mensagem.find('#btnMessageOk').addClass('hide');
}
function mensagemSyncHide() {
    $('.msgSync').removeClass('msgSync').fadeOut("fast").hide();
}
function consolidation() {
    //createLog("Send Consolidation");
    $.ajax({
        type: 'POST'
        , url: urlPreffix + '/api/SyncServiceApi/ProcessJson'
        , contentType: 'application/json; charset=utf-8'
        , dataType: 'json'
        , data: '{' + "device: '" + device.uuid + "', id: 0" + '}'
        //, data: '{' + "obj: '" + objectSend + "', collectionDate : '" + level02.attr('datetime') + "', level01Id: '" + level01.attr('level01Id') + "', level02Id: '" + level02.attr('level02id') + "', unitId: '" + level01.attr('unidadeid') + "', period: '" + level01.attr('period') + "', shift: '" + level01.attr('shift') + "', device: '123', version: '" + versao + "', ambient: '" + baseAmbiente + "'" + '}'
        , async: false //blocks window close
        , headers: token()
        , success: function (data, status) {
            if (data != null && data == "error") {
                //createLog("Consolidation Error:" + XMLHttpRequest.responseText);
                mensagemSyncHide();
                openMessageModal(getResource("synchronization_error"), getResource("try_again_contact_support"));
            }
            else {
                //createLog("Sucess");
                //$('.message .body').html(getResource("ready"), getResource("consolidated_results"));
                openMessageModal(getResource("ready"), getResource("consolidated_results"));
                setTimeout(function (e) {
                    //createLog("Reciving Data");
                    reciveResults();
                }, 1500);
            }

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //createLog("Consolidation Error:" + XMLHttpRequest.responseText);
            mensagemSyncHide();
            openMessageModal(getResource("synchronization_error"), getResource("try_again_contact_support"));
        }
    });
}
function reciveResults() {
    ping();

    if ($('.level02Result[sync=false]').length == 0) {
        ping(sendCorrectiveActionOnLine, sendResultsTimeout);
        sendResultLevel3Photo();
    }

    setTimeout(function (e) {
        if (connectionServer == true) {
            menssagemSync(getResource("synchronizing") + "...", getResource("receiving_data")); // Mostra o modal informando que est� verificando as coletas
            setTimeout(function (e) {
                recivingData();
            }, 1500);
        }
    }, 500);

    // if (connectionServer == true) {
    //     menssagemSync(getResource("synchronizing")+"...", getResource("receiving_data")); // Mostra o modal informando que est� verificando as coletas
    //     recivingData();
    // }
}
/// Recebe os Resultados de Hoje
function recivingData() {
    //createLog("Starting Recieving");

    var date = getCollectionDate();

    if ($('.App').attr('unidadeid')) {
        try {
            $.ajax({
                type: 'POST'
                , url: urlPreffix + '/api/SyncServiceApi/reciveData?unidadeId='+ $('.App').attr('unidadeid') + "&data=" + date
                , contentType: 'application/json; charset=utf-8'
                , dataType: 'json'
                //, data: '{' + "unidadeId: '" + $('.App').attr('unidadeid') + "', data: '" + date + "'" + '}'
                , async: false //blocks window close
                , headers: token()
                , error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //createLog("Reciecing Error");
                    mensagemSyncHide();
                    openMessageModal(getResource("synchronization_error"), getResource("try_again_contact_support"));
                }
                , success: function (data, status) {
                    if (data != null && data == "error") {
                        //createLog("Reciecing Error");
                        mensagemSyncHide();
                        openMessageModal(getResource("synchronization_error"), getResource("try_again_contact_support"));
                    }
                    else {
                        //("Success Recieving");
                        var collections = $(data);
                        $('.ResultsConsolidation').empty();
                        appendDevice(collections, $('.ResultsConsolidation'));

                        emptyEvaluatedCounterLevel1();
                        collections.each(function (index, self) {
                            setEvaluatedCounterLevel1(parseInt($(self).attr('level1id')), parseInt($(self).attr('level2id')), parseInt($(self).attr('evaluation')), parseInt($(self).attr('shift')), parseInt($(self).attr('period')));
                        });


                        $('.message .head').html(getResource("sync_finished"));
                        $('.message .body').html("");
                        setTimeout(function () {
                            mensagemSyncHide();
                        }, 1000);

                        if ($('.level01List').is(':visible')) {
                            $('#selectPeriod').trigger('change');
                        }

                        if ($('.level2List').is(':visible')) {
                            openLevel2($('.level1[id=' + $('.level2Group:visible').attr('level01id') + ']'));
                        }

                        $('.level1').each(function (index, self) {
                            completeLevel1(self);
                        });

                        updateReaudit(1);
                        updateReaudit(2);

                        updateCorrectiveAction();
                        createFileResultConsolidation();

                        getAllVF();

                    }
                },
                timeout: 600000 // sets timeout to 3 seconds
            });
        }
        catch (e) {
            mensagemSyncHide();
            openMessageModal(getResource("synchronization_error"), getResource("try_again_contact_support"));
        }
    }
}

function recivingDataByLevel1(ParLevel1) {
    //createLog("Starting Recieving");

    var date = getCollectionDate();

    try {
        $.ajax({
            type: 'POST'
            , headers: token()
            , url: urlPreffix + "/api/SyncServiceApi/reciveDataByLevel1?ParCompany_Id=" + $('.App').attr('unidadeid') + "&data=" + date + "&ParLevel1_Id=" + ParLevel1.attr('id')
            , contentType: 'application/json; charset=utf-8'
            , dataType: 'json'
            //, data: '{' + "ParCompany_Id: '" + $('.App').attr('unidadeid') + "', data: '" + date + "', ParLevel1_Id: '" + ParLevel1.attr('id') + "'" + '}'
            , async: true //blocks window close
            , error: function (XMLHttpRequest, textStatus, errorThrown) {
                //createLog("Reciecing By Level1 Error");
            }
            , success: function (data, status) {
                if (data != null && data == "error") {
                    //createLog("Reciecing By Level1 Error");
                }
                else {
                    //createLog("Success By Level1 Recieving");
                    var collections = $(data);


                    $('.ResultsConsolidation .Resultlevel2[level1id=' + ParLevel1.attr('id') + ']').remove();
                    appendDevice(collections, $('.ResultsConsolidation'));

                    level2ConsolidationUpdate(ParLevel1);

                }
            }
        });
    }
    catch (e) {
        mensagemSyncHide();
        openMessageModal(getResource("synchronization_error"), getResource("try_again_contact_support"));
    }
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
                    if (r.Retorno != null) {
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
                            var a = '.level01Result[level01Id=' + r.IdSaved + '] .level02Result[level02id=' + o.Level02Id + '][evaluate=' + o.EvaluationNumber + '][shift=' + o.Shift + '][period=' + o.Period + '][date="' + data2 + '"][sample=' + o.Sample + '][phase=' + o.Phase + '][reaudit=' + o.ReauditIs + '][reauditNumber=' + o.ReauditNumber + ']';
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
                        openMessageModal(r.Mensagem + ' - ' + r.MensagemExcecao, null);
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
                if (e.responseJSON)
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