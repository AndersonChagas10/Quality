var totalObj = 0; //Verifica total de Level02 a ser sincornizado
var objsyncked = 0; //Ojetos já sincronizados
var qtdeObjetos = 500; //Quantidade de Objetos enviados por sincronização

/// Função que Inicializa a Sincronização
function sendResults() {
    ping();
    if (connectionServer != true)
    {
        openMessageModal("No Sync Collection", 'Don´t have collectione for sync now');
        return true;
    }
    totalObj = $('.level02Result[sync=false]').length;
    if (totalObj == 0)
    {
        openMessageModal("No Sync Collection", 'Don´t have collectione for sync now');
        return true;
    }
    objsyncked = 0; //Zera a variavel de objetos sincronizados
    totalObj = $('.level02Result[sync=false]').length; //verificamos o total de level02 a ser sincronizado
    menssagemSync("Sync Collection", 'Cheking Collections'); // Mostra o modal informando que está verificando as coletas
    initializeSync(); //Inicializa o Envio dos Resultados
}
function initializeSync() {
     

    setTimeout(function (e) {
        //Prepara a quantidade de objetos que devem ser sincronizados conforme definição na variável qtdeObjetos
        preparing(qtdeObjetos);
        $('.message .body').html("Preparing to send..."); //Altera mensagem para preaprando envio;
        setTimeout(function (e) {

            $('.message .body').html("Send " + objsyncked + " of " + totalObj + " Results"); //Mostra Qtde de Objeros que já foram enviados
            setTimeout(function (e) {

                send(); //Envia a informação

            }, 1000);
        }, 1000);
    }, 1000);
}
//Prepara os objetos a partir da variavel qtdeObjetos
function preparing(qtdeObjetos)
{
    //Os objetos ganharam uma tag para que possam ser identificados facilmente
    var arrayResultsSend = $('.level02Result[sync=false]').slice(0, qtdeObjetos);
    arrayResultsSend.attr('send', 'true');

    //for (var i = 0; i < qtdeObjetos; i++) {
    //    $('.level02Result[sync=false]:eq(' + i + ')').attr('send', 'true');
    //}
}
function send() {

    var objectSend = "";

   // objectSend['Level02'] = [];

    $('.level02Result[sync=false][send=true]').each(function (e) {
        
        var level02Result = "";

        var level02 = $(this);
        var level01 = level02.parents('.level01Result');

        level02Result += level01.attr('level01id');//[0]
        level02Result += ";" + level01.attr('datetime');//[1]
        level02Result += ";" + level02.attr('level02id');//[2]
        level02Result += ";" + level02.attr('datetime');//[3]
        level02Result += ";" + level01.attr('unidadeid');//[4]
        level02Result += ";" + level01.attr('period');//[5]
        level02Result += ";" + level01.attr('shift'); //[6]
        
        level02Result += ";" + level02.attr('auditorid'); //7]ok
        level02Result += ";" + level02.attr('phase'); //[8] ok
        level02Result += ";" + level02.attr('reaudit'); //[9]ok
        level02Result += ";" + level02.attr('startphasedate'); //[10]ok
        level02Result += ";" + level02.attr('evaluate') ;//[11]ok
        level02Result += ";" + level02.attr('sample');//[12]ok
        level02Result += ";" + level02.attr('cattletype');//[13]ok
        level02Result += ";" + level02.attr('chainspeed');//[14]ok
        level02Result += ";" + level02.attr('lotnumber'); //[15]ok
        level02Result += ";" + level02.attr('mudscore'); //[16]ok
        level02Result += ";" + level02.attr('consecutivefailurelevel'); //[17] ok
        level02Result += ";" + level02.attr('consecutivefailuretotal'); //[18] ok
        level02Result += ";" + level02.attr('notavaliable'); //[19]ok
        level02Result += ";" + versao; //[20]
        level02Result += ";" + baseAmbiente; //[21]

        var level03Result = $(this).children('.level03Result');
        var resultLevel03 = "";

        level03Result.each(function (e) {

            var level03 = $(this);

            var result = level03.attr('level03id');
                result += "," + level03.attr('date');
                result += "," + level03.attr('value');
                result += "," + level03.attr('conform');
                result += "," + level03.attr('auditorid');
                result += "," + level03.attr('totalerror');
                result += "," + escape(level03.attr('valuetext'));
                result += "," + level03.attr('id');

                resultLevel03 += "<level03>" + result + "</level03>";
             
            // object += "]}";
        });
        level02Result += ";" + resultLevel03; //[22]

        var correctiveActionResult = "";
        var correctiveAction = level02.children('.correctiveAction');

        if (correctiveAction.length)
        {
            
            correctiveActionResult = "<correctiveaction>";
            correctiveActionResult += correctiveAction.attr('slaugthersignature');
            correctiveActionResult += "," + correctiveAction.attr('techinicalsignature');
            correctiveActionResult += "," + correctiveAction.attr('datetimeslaughter');
            correctiveActionResult += "," + correctiveAction.attr('datetimetechinical');
            correctiveActionResult += "," + correctiveAction.attr('auditstarttime');
            correctiveActionResult += "," + correctiveAction.attr('datecorrectiveaction');

            correctiveActionResult += "," + escape(correctiveAction.children('.descriptionFailure').text());
            correctiveActionResult += "," + escape(correctiveAction.children('.immediateCorrectiveAction').text());
            correctiveActionResult += "," + escape(correctiveAction.children('.productDisposition').text());
            correctiveActionResult += "," + escape(correctiveAction.children('.preventativeMeasure').text());


            correctiveActionResult += "</correctiveaction>";

        }

        level02Result += ";" + correctiveActionResult; //[23]
        level02Result += ";" + level02.attr('havereaudit'); //[24]
        level02Result += ";" + level02.attr('havecorrectiveaction'); //[25]
        level02Result += ";" + level02.attr('reauditnumber');//[26]
        level02Result += ";" + level01.attr('biasedunbiased');//[27]
        level02Result += ";" + level01.attr('completed');//[28]
        level02Result += ";" + level02.attr('havephases'); //r[29]
        level02Result += ";" + level02.attr('id'); //r[30]
        level02Result += ";" + level02.attr('correctiveactioncomplete'); //r[31]
        level02Result += ";" + level02.attr('completereaudit'); //r[32]

        objectSend += "<level02>" + level02Result + "</level02>";
        


    });
      // console.log(objectSend);
    $.ajax({
        type: 'POST'
            , url: urlPreffix + '/Services/SynService.asmx/InsertJson'
            , contentType: 'application/json; charset=utf-8'
            , dataType: 'json'
            , data: '{' + "ObjResultJSon: '" + objectSend + "', deviceId: '" + device.uuid + "', deviceMac: ''" + '}'
            //, data: '{' + "obj: '" + objectSend + "', collectionDate : '" + level02.attr('datetime') + "', level01Id: '" + level01.attr('level01Id') + "', level02Id: '" + level02.attr('level02id') + "', unitId: '" + level01.attr('unidadeid') + "', period: '" + level01.attr('period') + "', shift: '" + level01.attr('shift') + "', device: '123', version: '" + versao + "', ambient: '" + baseAmbiente + "'" + '}'
            , async: false //blocks window close
        , success: function (data, status) {
            objsyncked = objsyncked + $('.level02Result[sync=false][send=true]').length;

            $('.message .body').html("Send Result " + objsyncked + "/" + totalObj);
            $('.level02Result[sync=false][send=true]').removeAttr('send').attr('sync', 'true');
            createFileResult();
            menssagemSync("Sync Collection", 'Cheking Collections');
            setTimeout(function (e) {
                if ($('.level02Result[sync=false]').length)
                {
                    initializeSync();
                }
                else
                {
                    menssagemSync("Sync Consolidation", 'Consolidation Results');
                    setTimeout(function (e) {

                        consolidation();

                    }, 1500);
                }
            }, 1000);//


        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            mensagemSyncHide();
            openMessageModal("Could not sync. Try again.");
        }
    });
}

function menssagemSync(title, content) {
    var mensagem = $('.message, .overlay');
    mensagem.children('.head').html(title);
    mensagem.children('.body').html(content);
    $('#btnMessageOk').addClass('hide');
    mensagem.addClass('msgSync');
    mensagem.fadeIn("fast");
}
function mensagemSyncHide() {
    $('.msgSync').removeClass('msgSync').fadeOut("fast");

}
function consolidation() {
    $.ajax({
        type: 'POST'
            , url: urlPreffix + '/Services/SynService.asmx/ProcessJson'
            , contentType: 'application/json; charset=utf-8'
            , dataType: 'json'
            , data: '{' + "device: '" + device.uuid + "'" + '}'
        //, data: '{' + "obj: '" + objectSend + "', collectionDate : '" + level02.attr('datetime') + "', level01Id: '" + level01.attr('level01Id') + "', level02Id: '" + level02.attr('level02id') + "', unitId: '" + level01.attr('unidadeid') + "', period: '" + level01.attr('period') + "', shift: '" + level01.attr('shift') + "', device: '123', version: '" + versao + "', ambient: '" + baseAmbiente + "'" + '}'
            , async: false //blocks window close
        , success: function (data, status) {
            $('.message .body').html("Result consolidation completed");
            setTimeout(function (e) {
                reciveResults();
            }, 1500);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            mensagemSyncHide();
            openMessageModal("Could not sync. Try again.");
        }
    });
}
function reciveResults() {
    ping();
    
    setTimeout(function (e) {
        if (connectionServer == true)
        {
            menssagemSync("Recinving Data", 'Reciving results'); // Mostra o modal informando que está verificando as coletas
            setTimeout(function (e) {
                recivingData();

            }, 1500);
        }
    }, 500);
}

function recivingData() {
    try {
        $.ajax({
            type: 'POST'
            , url: urlPreffix + '/Services/SynService.asmx/reciveData'
            , contentType: 'application/json; charset=utf-8'
            , dataType: 'json'
            , data: '{' + "unidadeId: '" + $('.App').attr('unidadeid') + "'" + '}'
            , async: false //blocks window close
            , success: function (data, status) {
                    $('.Results').empty();
                    appendDevice(data.d, $('.Results'));
                    $('.message .body').html("Result reciving sucess");
                    setTimeout(function (e) {
                        mensagemSyncHide();
                    }, 1500);

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                mensagemSyncHide();
                openMessageModal("Could not sync. Try again.");
            }
        });

    }
    catch (e) {
        mensagemSyncHide();
        openMessageModal("Could not sync. Try again.");
    }
}

//function buscaResultadosAtuais() {
//    try {
//        //throw erroDentroBuscaResultadosAtuais;
//        /*AJAX >*/$.get(urlPreffix + '/Merge/DivResults', function (objHtml) {

//            try {

//                appendDevice(objHtml, $('.Results'));
//                $('.message .body').html("Result reciving sucess");
//                setTimeout(function (e) {
//                    mensagemSyncHide();
//                }, 1500);


//            } catch (e) {
//                console.log(e);
//                mensagemSyncHide();
//            }

//        }).fail(function (eee, hhh, xxx) {
//            console.log(eee);
//            mensagemSyncHide();
//        });

//    } catch (e) {
//        console.log(e);
//        //gravaLogTryCatch(e);
//    }
//}
