//***************Urls********************************
var urlServidor = "http://192.168.25.200/SgqMaster";
var urlServidorDeveloper = "http://192.168.25.200/SgqMasterDesenvolvimento";

var urlServidorUsaHomologation = "http://10.190.2.34/QualityAssurance"
var urlServidorUsaHomologationName = "http://USTXRI00GRJ01T/QualityAssurance"


var urlServidorUsaProduction = "http://10.160.65.120/QualityAssurance"
var urlServidorExternal = "http://191.34.137.39/SgqMaster"

//***************Urls********************************


//***************Versão********************************
var versao = "1.0.8";

//***************Base Ambiente********************************
//var baseAmbiente = "GRT Local"
var baseAmbiente = "Homologation";
//var baseAmbiente = "Production"

//***************Apontamento Api´s********************************
var urlPreffix = "";

//teste update
//assim que todos os arquivos foram carregados
$(document).ready(function () {
    //clearLastSync();
    $('#selectUnit').val('0')
    if (localStorage.unit != undefined && localStorage.unit != "undefined") {
        $('#selectUnit').val(localStorage.unit);
    }
    $('#selectUnit').trigger('change');
    $('#version .number, #versionLogin .number').text(versao);
    $('#ambiente .base, #ambienteLogin .base').text(baseAmbiente);

    ping();

    //comeca o relogio
    updateWatch();
});
$(document).on('change', '#selectUnit', function (e) {
    //_writeFile("sync.txt", "");
    urlPreffix = "";
    connectionServer = false;
    if ($('#selectUnit :selected').val() == "0")
    {
        return false;
    }
    urlPreffix = 'http://' + $('#selectUnit :selected').attr('ip');
    ping();
});
//assim que o dispositivo estiver pronto
document.addEventListener("deviceready", onDeviceReady, false);

//assim que o dispositivo estiver pronto
function onDeviceReady() {
    //cria o arquivo que grava a ultima sincronizacao
    window.requestFileSystem(LocalFileSystem.PERSISTENT, 0, function (fs) {
        fs.root.getFile("sync.txt", { create: true, exclusive: false }, function (fileEntry) {
        }, onErrorCreateFile);
    }, onErrorLoadFs);

}

//criacao do file plugin
window.addEventListener('filePluginIsReady', function () {
    window.requestFileSystem(LocalFileSystem.PERSISTENT, 0, function (fs) {
        fs.root.getFile("sync.txt", { create: true, exclusive: false }, function (fileEntry) {
        }, onErrorCreateFile);
    }, onErrorLoadFs);
}, false);

//criptografia para a senha
var key = CryptoJS.enc.Utf8.parse('JDS438FDSSJHLWEQ');
var iv = CryptoJS.enc.Utf8.parse('679FDM329IFD23HJ');
var AES = {
    Encrypt: function (text) {
        var encrypted = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(text), key,
            {
                keySize: 128 / 8,
                iv: iv,
                mode: CryptoJS.mode.CBC,
                padding: CryptoJS.pad.Pkcs7
            });
        return encrypted.toString();
    },
};

//fazer login online caso necessario
function onlineLogin(username, password) {
    $.ajax({
        data: {
            Name: username,
            Password: AES.Encrypt(password),
        },
        url: urlPreffix + '/api/User/AuthenticationLogin',
        type: 'POST',
        success: function (data) {
            var $btn = $('#btnLogin');

            if (data.MensagemExcecao) {
                Geral.exibirMensagemErro("Username or password is invalid");
            } else {
                //verificar se o usuario esta ligado a alguma unidade
                //se nao, dar uma mensagem de erro que usuario esta sem permissao de acesso
                insertUserFile(username, AES.Encrypt(password));
            }
            $btn.button('reset');
            return data;
        }
    });
}

function updateWatch() {
    $('.atualDate').text(dateTimeWithMinutes());
    tempoLogout--;
    if (tempoLogout == 0) {
        $("#btnLogout").click();
        openMessageModal("Inactive", "You have not been active for "+(configTempLogout/60)+" minutes. Log in again.");
    }
    setTimeout(updateWatch, 1000);
}

function initialLogin(user) {
    Geral.esconderMensagem();

    localStorage.setItem("unit", $('#selectUnit').val());


    $('.App').attr('userid', user.attr('userid'));

   // $('.App').attr('unidadeid', user.attr('unidadeid'));
    $('.App').attr('unidadeid', $('#selectUnit :selected').val());
    $('.unit').text($('#selectUnit :selected').text());

    $('.App').attr('shift', $('#shift option:selected').val());
    $('.App').attr('date', dateReturn());
    $('.App').attr('logintime', dateTimeFormat());


    $('footer .user').text(user.attr('username'));
    $('.shift').text($('#shift option:selected').text());

    level01Reset($('.level01'));
    $('.painelLevel02 select').val(0);
    $('.painelLevel02 input').val("");

    level02Reset($('.level02List .level02Group .level02'));

    //$('.App').attr('period', $('#selectPeriod option:selected').val());
    //$('span.period').html($("select#selectPeriod:visible :selected").text());

    //periodReset();


    //syncronuza primiero resulta e seta para saber que tem  o primeiro resultao
    //chama usuarios off line
    //start sincronizacao automatica
    //starServices();

    ///antes de mostrar
    showLevel01(true);
}

function starServices() {
    //colocas dados off line na div
    $('.Results').empty();
    //Sync();
   // FirstSync();
    insertUserFile();

}

//variavel para configuração do tempo de logout -- 30 minutos
var configTempLogout = 1800;

//variavel para controle de tempo de logout
var tempoLogout = configTempLogout;

$(document).on('click', function (e) {

    //atualizacao quando um clique ocorre
    tempoLogout = configTempLogout;

});

$(document).on('click', '#btnSync', function (e) {
    //if (connectionServer == true) {
    //    openSyncModal('Sync', 'Syncing the data collection...');
    //    Sync();
    //    //waitingDialog.show('Syncing the results...', { dialogSize: 'sm', progressType: 'primary' }); setTimeout(function () { waitingDialog.hide(); }, 3000);
    //} else {
    //    openMessageModal("Connection", 'The device is not connected to the server.');
    //}
    sendResults();
});

$(document).on('click', '#btnLogin', function (e) {
    var $btn = $('#btnLogin');
    $btn.button('loading');
    e.preventDefault();
    getlogin($('#inputUserName').val(), $('#inputPassword').val(), initialLogin);
});


$(window).bind('beforeunload', function () {
    return "Warning! If you proceed you will loose information";
});

function evaluateDoneReturn(elementConf, lastEvaluate, lastSample) {
    if (elementConf.attr('totalsets')) {
        var evaluateTotal = parseInt(elementConf.attr('totalsets'));
        var sampleTotal = parseInt(elementConf.attr('sidesperset'));

        var evaluate = parseInt(lastEvaluate);
        var sample = parseInt(lastSample);


        if (evaluate < evaluateTotal && sample < sampleTotal) {
            evaluate--;
        }
        return evaluate;
    }
}


$(document).on('click', '.level01List .level01', function (e) {
    //Seleciona o Nivel 01
    var level01 = $(this);

    //Verifica se o level02List não está visivel
    if (!$('.level02List').is(':visible')) {
        //verifica se os itens estão preenchidos
        var itensOk = checkInputsSelect();
        //se não estireverem
        if (itensOk == false) {
            //Paro a execução do sistema para enviar a mensagem
            return false;
        }
    }

    //Zero total de defeitos
    $('.level01Result').removeClass('selected');

    //Verifico se já existe resultado para o periodo.
    //var atualResult = getAtualResult(level01);

    //Instancio configuração de avaliação, por padrão o mínimo é 1;
    var evaluateConf = 1;


    //verifico se trabalho com avaliacoes
    if (level01.attr('haveevaluates')) {

        evaluateConf = parseInt($('.level02Group[level01id=' + level01.attr('id') + '] .level02:first').attr('totalsets'));
    }


    //Verifico o ultimo resultado gerado.
    var resultPeriod = getAtualResult(level01);

    if (!resultPeriod.length && !level01.attr('havephases')) {
        resultPeriod = getLastReauditResultPeriod(level01, $('#selectPeriod').val());
        if (resultPeriod.length && (resultPeriod.attr('date') != $('.App').attr('date')) && (resultPeriod.attr('period') == $('.App').attr('period'))) {
            resultPeriod = getAtualResult(level01);
        }
    }
    //Se o resultado teve uma reauditoria completa
    var reauditPeriod = getLastReauditResultPeriod(level01, resultPeriod.attr('period')); 

    if (resultPeriod.attr('completereaudit') || reauditPeriod.length > 0) {
        //vou buscar a reauditoria como resultado
        resultPeriod = reauditPeriod;
    }


    var resultHaveReaudit = getResultHaveReaudit(level01);
    if (!resultPeriod.length && resultHaveReaudit.length && !level01.attr('startreaudit')) {
        openMessageModal("Pending Audit or Re-audit " + $('#selectPeriod option[value=' + resultHaveReaudit.attr('period') + ']').text(), 'Please, complete to move to next audit');
        return false;
    }
    //Se não foi encontrado resultado para o periodo, mas foi encontrada uma reauditoria.
    if (resultHaveReaudit.length) {
    //if (!resultPeriod.length && resultHaveReaudit.length) {
            //Assuma a reauditoria como resultado do periodo.
        resultPeriod = resultHaveReaudit;
    }

    //Se o resultado do periodo gerou uma reauditoria estou iniciando/continuando uma reauditoria
    if (resultPeriod.attr('havereaudit') && level01.attr('startreaudit')) {
        //Busca o ultimo resultado com reauditoria igual a true
        resultPeriod = getLastReauditResultPeriod(level01, resultPeriod.attr('period'));
    }
    //Reseta o level02
    level02Reset($('.level02Group[level01id=' + level01.attr('id') + '] .level02'));

    //Se está iniciando/continuando uma reauditoria e não tem resultado ou o resultado é uma reauditoria e o resultado do periodo gerou uma reauditoria e inicia/continua a reauditoria
    if ((level01.attr('startreaudit') && !resultPeriod.length) || (resultPeriod.attr('reaudit') == "true" && resultPeriod.attr('havereaudit') && level01.attr('startreaudit'))) {
        //Busca ultimo resultado que tem reauditoria
        resultPeriod = getResultHaveReaudit(level01);

        //Verifica o total de defeitos
        var totalDefects = parseInt(resultPeriod.attr('totaldefects'));


        //Percorremos o resultado do periodo.
        resultPeriod.children('.level02Result').each(function (e) {
            //Regra HTP e CFF
            if (totalDefects > 22 || level01.attr('haveevaluates')) {
                //Se tem mais de 22 defeitos ou tenho mais de uma avaliação.
                return false;
            }
            //Instancia o resultado.
            var result = $(this);
            //Verica o level02.
            var level02 = $('.level02[id=' + result.attr('level02id') + ']');
            //Informa temporáriamente o level01 do resultado como selecionado.
            $('.level01[id=' + result.attr('level01id') + ']').addClass('selected');
            //Informa temporariamente o level01 do resultado como selecionado.
            $('.level02[id=' + result.attr('level02id') + ']').addClass('selected');
            //temos que localizar onde desativa o botao na em outro evento
            var btnNa = level02.siblings('.userInfo').children('div').children('.na');
            btnNa.removeClass('naSelected').removeAttr('disabled');

            //Se o resultado não teve reauditoria.
            if (!result.attr('havereaudit')) {
                //Iniciamos ele como não avaliado.
                //Com isso irá gerar um novo resultado se não tiver outro
                btnNa.click();
            }

            //Removo a classe temporaria do level01 e level02
            $('.level01[id=' + result.attr('level01id') + ']').removeClass('selected');
            $('.level02[id=' + result.attr('level02id') + ']').removeClass('selected');

        });

        //Se level01 tem mais de uma avaliação
        if (level01.attr('haveevaluates')) {
            //Se o resultado do periodo está completo
            if (resultPeriod.attr('completed')) {
                //Verifica o numero da reauditoria.
                var reauditNumber = parseInt(resultPeriod.attr('reauditnumber'));
                //Incrementa o numero
                reauditNumber++;
                //Informa a proxima reauditoria
                level01.attr('reauditnumber', reauditNumber);
            }
            //Reseta o level02
            level02Reset($('.level02Group[level01id=' + level01.attr('id') + '] .level02:first'));
        }
        //Instancia o resultado como o resultado selecionado.
        resultPeriod = $('.level01Result.selected');
    }

    //Limpa atributos do level02
    $('.level02').removeAttr('phase').removeAttr('reauditnumber').children('.levelInfo').text('');
    //Limpa botoes Nao Avaliado
    $('.btnNotAvaliable').removeClass('naSelected').removeAttr('disabled');

    //Preenche os valores do Level02Group caso tenha

    resultPreencher(resultPeriod);
    if (level01.attr('havephases')) {
        getPhases(level01);
    }
    //Mostra o level02Group.
    showLevel02(level01);

});
function getPhases(level01) {
    var resultsLevel01Phase = $('.level01Result[level01id=' + level01.attr('id') + '][reaudit=false][shift=' + $('.App').attr('shift') + '][completed=completed]:last');

    var resultsLevel02Phase = resultsLevel01Phase.children('.level02Result');


    resultsLevel02Phase.each(function (e) {

        var result = $(this);
        var phase = parseInt(result.attr('phase'));
        var phaseNumber = parseInt(result.attr('reauditnumber'));


        if (result.attr('completereaudit')) {
            phase++;
            phaseNumber = 1;
        }
        else {
            phaseNumber++;
        }

        if (phase == 1 && result.attr('date') != $('.App').attr('date')) {
            phase = 0;
        }
        else if (phase == 2 && phaseNumber > 3) {
            phase = 0;
        }
        else if (phase == 3 && phaseNumber > 6) {
            phase = 0;
        }

        if (phase > 3)
        {
            phase = 3;
            phaseNumber = 1;
        }


        var level02 = $('.level02[id=' + result.attr('level02id') + ']');
        if ((!level02.attr('phase') || level02.attr('getphase')) && (phase > 0 || (phase == 0 && result.attr('completereaudit') && result.attr('date') == $('.App').attr('date')))) {
            level02.attr('getphase', 'getphase').attr('phase', phase).attr('reauditnumber', phaseNumber).children('.levelInfo').text('(Phase ' + phase + ")");
        }
        else if ((!level02.attr('phase') || level02.attr('getphase')) && phase == 0) {
            level02.removeAttr('phase').removeAttr('reauditnumber').children('.levelInfo').text('');

        }
    });
    $('.level02[getphase]').removeAttr('getphase');
}
function resultPreencher(resultPeriod) {

    //Seleciona o Level01.
    var level01 = $('.level01.selected');
    //Instancia o total de defeitos
    var totalDefects = 0;

    //Oculta os botões do level02
    //$('.level02').parents('.row').children('.userInfo').children('div').children('').addClass('hide');
    $('.level02 .btnPhase, .level02 .btnCorrectiveAction, .level02 .reauditCount').addClass('hide');
    //Remove Atributos 
    $('.level02').removeAttr('startphasedate').removeAttr('phase').removeAttr('havecorrectiveaction').removeAttr('correctivaction').attr('reauditNumber', '0').attr('defects', '0');
    //Remove Cores de Fundo do Level02
    $('.level02').parents('li').removeClass('bgLimitExceeded');

    //Percorre o resultado
    resultPeriod.each(function (e) {


        var level01Result = $(this);
        if (level01Result.attr('reaudit') == "true" && level01Result.attr('havereaudit') && level01Result.attr('completed') && level01.attr('startreaudit')) {
            return false;
        }
        level01Result.addClass('selected');
        var results = level01Result.children('.level02Result');
        if (level01Result.attr('biasedunbiased')) {
            $('#biasedUnbiased').val(level01Result.attr('biasedunbiased'));
        }

        $("span.sideWithErrors").text((level01Result.attr('sidewitherros')) ? level01Result.attr('sidewitherros') : "0").parents('.labelPainel').removeClass('red');
        $("span.more3Defects").text((level01Result.attr('more3Defects')) ? level01Result.attr('more3Defects') : "0").parents('.labelPainel').removeClass('red');

        var sideWithErros = parseInt($("span.sideWithErrors:first").text());
        if (sideWithErros > 5) {
            $("span.sideWithErrors").parents('.labelPainel').addClass('red');
        }
        var more3Defects = parseInt($("span.more3Defects:first").text());
        if (more3Defects > 0) {
            $("span.more3Defects").parents('.labelPainel').addClass('red');
        }


        results.each(function (e) {


            var result = $(this);
            var level02 = $('.level02Group[level01id=' + results.attr('level01id') + '] .level02[id=' + result.attr('level02id') + ']');
            level02.attr('defects', result.attr('defects')).attr('completed', 'completed');
            level02.parents('.row').children('.userInfo').children('div').children('.defects').text(level02.attr('defects'));
            totalDefects = totalDefects + parseInt(level02.attr('defects'));
            //esses results podem ficar no level01


            //if (level01.attr('consecutivefailure') == "true")
            //{
            //    if (result.attr('consecutivefailuretotal')) {
            //        level02.siblings('.userInfo').children('div').children('.consecutiveFailure').text(result.attr('consecutivefailuretotal'));
            //    }
            //    else {
            //       var resultLast = $('.level02Result[.level01id=' + results.attr('level01id') + '][level02id=' + result.attr('level02id') + ']:last');
            //       level02.siblings('.userInfo').children('div').children('.consecutiveFailure').text(resultLast.attr('consecutivefailuretotal'));

            //    }

            //}

            if (result.attr('cattletype')) {
                $('#selectCattleType').val(result.attr('cattletype'));
            }
            if (result.attr('chainspeed')) {
                $('#inputChainSpeed').val(result.attr('chainspeed'));
            }
            if (result.attr('lotnumber')) {
                $('#inputLotNumber').val(result.attr('lotnumber'));
            }
            if (result.attr('mudscore')) {
                $('#inputMudScore').val(result.attr('mudscore'));
            }

            if (result.attr('notavaliable') == "true") {
                level02.attr('notavaliable', 'notavaliable');
                level02.siblings('.userInfo').children('div').children('.btnNotAvaliable').addClass('naSelected');
            }

          
            if ($('.level01[id=' + result.attr('level01id') + ']').attr('havephases')) {
                var phase = parseInt(result.attr('phase'));
                var phaseNumber = parseInt(result.attr('reauditnumber'));
                var level02Phase = $('.level02[id=' + result.attr('level02id') + ']');
                level02Phase.attr('phase', phase).attr('reauditnumber', phaseNumber);

                if (phase > 0) {

                    level02Phase.children('.levelInfo').text('(Phase ' + phase + ")");
                }


            }

            $('.totalDefects').text(totalDefects);
            if (totalDefects > 22) {
                $('.painel .totalDefects').parents('.labelPainel').addClass('red');
            }
            else {
                $('.painel .totalDefects').parents('.labelPainel').removeClass('red');

            }
            level02.addClass('selected');
            defectLimitCheck();

            if (result.attr('correctiveactioncomplete')) {
                level02.removeAttr('havecorrectiveaction').attr('correctiveactioncomplete', 'correctiveactioncomplete');
            }
            if (!level01.attr('havephases')) {
                level02Complete(level02);
            }

            level02.removeClass('selected');

        });
    });

}
$(document).on('click', '.level02List .level02', function (e) {
    showLevel03($(this));
});

$(document).on('click', '.btnCA', function (e) {

    $(this).addClass('caLevel02').addClass('hide');
    if ($('#btnSave').is(':visible')) {
        $('#btnSave').addClass('caLevel02').addClass('hide');
    }
    correctiveActionOpen();
    //  $('.level01List').addClass('hide').hide();
    // $('.level02List').addClass('hide').hide();
    // $('.level03List').addClass('hide').hide();
    // $('.breadcrumb').addClass('hide').hide();

    //$('.correctiveaction').removeClass('hide').show();
    //$(this).addClass('hide');



});

function correctiveActionOpen(level01Id, date, shift, period) {

    //Seleciona o Id do Level01 caso não tenha recebido nenhum parametro.
    level01Id = level01Id ? level01Id : $('.level01.selected').attr('id');
    //Seleciona a data caso não tenha recebido nenhum parametro.
    date = date ? date : $('.App').attr('date');
    //Seleciona o shift caso não tenha recebido nenhum parametro.
    shift = shift ? shift : $('.App').attr('shift');
    //period = period ? period : $('.App').attr('period');

    //Instancio o Level01.
    var level01 = $('#' + level01Id + '.level01');


    //Pego o resultado selecionado.
    var level01Result = $('.level01Result.selected');
    if (!level01Result.length) {
        level01Result = $('.level01Result[level01id=' + level01.attr('id') + ']');
    }

    //Seleciono um resultado do level02 que tenha Ação Corretiva.
    var level02Result = $('.level01Result[level01id=' + level01.attr('id') + ']').children('.level02Result[havecorrectiveaction]:first');
    if (level01.attr('correctivActionLevel02')) {
        level02Result = $('.level01Result[level01id=' + level01.attr('id') + ']').children('.level02Result[level02id=' + $('.level02.selected').attr('id') + '][havecorrectiveaction]:first');
    }
    //Se o level01 tem avaliações
    if (level01.attr('haveevaluates')) {
        //Quando tenho mais de uma avaliação posso ter ação corretiva em um local um resultado diferente do selecionado
        level02Result = $('.level01Result[level01id=' + level01.attr('id') + '][havecorrectiveaction] .level02Result[havecorrectiveaction]:first');
    }



    if (level01.attr('correctivActionLevel02')) {

        level01Result = level01Result.children('.level02Result[level02id=' + $('.level02.selected').attr('id') + '][havecorrectiveaction]:first');
    }

    $('.correctiveActionSelected').removeClass('correctiveActionSelected');

    level01Result.addClass('correctiveActionSelected');
    level02Result.addClass('correctiveActionSelected');

    var correctiveAction = $('.correctiveAction[idcorrectiveaction=' + level02Result.attr('idcorrectiveaction') + ']');
    if (!level01.attr('correctivactionlevel02')) {
        correctiveAction = $('.correctiveAction[idcorrectiveaction=' + level02Result.parents('.level01Result').children('.level02Result[idcorrectiveaction]').attr('idcorrectiveaction') + ']');
    }

    if (level01.attr('haveevaluates') && !correctiveAction.length) {
        level02Result = $('.level01Result[level01id=' + level02Result.attr('level01id') + '][date=' + level02Result.attr('date') + '][shift=' + level02Result.attr('shift') + '][period=' + level02Result.attr('period') + '][reaudit=' + level02Result.attr('reaudit') + '][reauditnumber=' + level02Result.attr('reauditnumber') + '] .level02Result[idcorrectiveaction]');
        if (level02Result.length) {
            correctiveAction = $('.correctiveAction[idcorrectiveaction=' + level02Result.parents('.level01Result').children('.level02Result[idcorrectiveaction]').attr('idcorrectiveaction') + ']');
        }

    }
    //if (!level02Result.attr('idcorrectiveaction'))
    //{
    //    correctiveAction = $('.correctiveAction[idcorrectiveaction=' + level02Result.attr('idcorrectiveaction') + ']');
    //}
    if (!level01Result.length) {
        openMessageModal("Corrective Action not Found", "Try again");
        return false;
    }
    var correctiveActionModal = $('#correctiveActionModal');


    correctiveActionModal.attr('unidadeid', level01Result.attr('unidadeid'))
                         .attr('auditorid', $('.App').attr('userid'))
                         .attr('date', level01Result.attr('date'))
                         .attr('shift', level01Result.attr('shift'))
                         .attr('period', level01Result.attr('period'))
                         .attr('level01id', level01Result.attr('level01id'))
                         .attr('level02id', level02Result.attr('level02id'));

    $('#CorrectiveActionTaken').children('#datetime').text(dateTimeWithMinutes());
    $('#CorrectiveActionTaken').children('#auditor').text('Admin');
    $('#CorrectiveActionTaken').children('#shift').text($('#shift option[value=' + shift + ']').text());
    $('#AuditInformation').children('#auditText').text(level01.children('.levelName').text());
    $('#AuditInformation').children('#starttime').text(level01Result.attr('datetime').slice(0, 16));
    $('#AuditInformation').children('#correctivePeriod').text($('#selectPeriod option[value=' + level01Result.attr('period') + ']').text());


    $('.overlay').show();
    $('body').addClass('overflowNo');

    correctiveActionModal.removeClass('hide');

    if (!$('.level01.selected').length) {

        level01Id = parseInt($('.btnCorrectiveAction.selected').parents('.row').children('.level01').attr('id'));
    }


    var description = "";


    if (level01Id == 3) {
        description = correctiveActionOpenLevel01Id03();
    }

    else {

        var level02ResultList = level01Result.children('.level02Result[havecorrectiveaction]');
        if (level01.attr('correctivActionLevel02')) {
            level02ResultList = level01Result;
        }

        level02ResultList.each(function (e) {

            //var level02 = $('#' + $(this).attr('level02id') + '.level02');

            var level02 = $('.level02[id=' + $(this).attr('level02id') + ']');


            var level02Id = parseInt(level02.attr('id'));
            var level02Name = level02.children('span.levelName').text();
            var level02errorlimit = parseInt(level02.attr('levelerrorlimit'));

            description = description + level02Name + " - defect limit:" + level02errorlimit;

            $(this).children('.level03Result').each(function (e) {

                var level03 = $('#' + $(this).attr('level03id') + '.level03');;

                var level03Defects = parseInt($(this).attr('value'));
                var level03Id = parseInt(level03.attr('id'));
                var level03Name = level03.children('.row').children('div').html();

                if (level03.children('.row').children('div').children('span.response').length) {
                    var conform = $(this).attr('conform');
                    level03Defects = 0;
                    if (conform == "false") {
                        level03Defects = 1;
                    }
                }
                if (level03Defects >= level02errorlimit && level03Defects > 0) {

                    description = description + "\n" + level03Name + ": " + level03Defects + " Defects";
                }
            });
            description = description + "\n\n";
        });

    }



    var correctiveActionInputs = correctiveActionModal.children('.panel-body').children('.modal-body').children('.row').children('.form-group');

    if (correctiveAction.length) {
        correctiveActionInputs.children('#DescriptionFailure').val(correctiveAction.attr('descriptionFailure'));
        correctiveActionInputs.children('#ImmediateCorrectiveAction').val(correctiveAction.attr('immediateCorrectiveAction'));
        correctiveActionInputs.children('#ProductDisposition').val(correctiveAction.attr('productDisposition'));
        correctiveActionInputs.children('#PreventativeMeasure').val(correctiveAction.attr('preventativeMeasure'));
        correctiveActionModal.attr('idcorrectiveaction', correctiveAction.attr('idcorrectiveaction'));
    }
    else {
        $("#DescriptionFailure").val("");
        $("#DescriptionFailure").val($('#DescriptionFailure').val() + description);
        correctiveActionInputs.children('#ImmediateCorrectiveAction').val("");
        correctiveActionInputs.children('#ProductDisposition').val("");
        correctiveActionInputs.children('#PreventativeMeasure').val("");
        correctiveActionModal.removeAttr('idcorrectiveaction');

        $('.btnSlaugtherSignatureRemove, .btnTechinicalSignatureRemove').click();


    }

    correctiveActionModal.fadeIn("fast");

    $('#DescriptionFailure').focus();


}


function correctiveActionOpenLevel01Id03(level01Id, date, shift, period) {


    //pego level01 por evaluate = set
    var level01ResultByEvaluate = $('.level01Result[level01id="3"][havecorrectiveaction]');

    var string3moreErros = "";
    var string6SidesWithErros = "";
    var Erros6Side = 0;
    var string6SidesWithErrosTemp = "";

    //percorro o level01
    level01ResultByEvaluate.each(function (e) {

        var level01Result = $(this);


        var evaluate = $(this).attr('evaluate');

        //busco a configuracao total de amostrar
        var totalsample = 10;//parseInt($('.level02').attr('sampleTotal'));

        //instancio variaveis de contagem

        //faço um loop pela contagem do total de amostras
        for (var sample = 0; sample < totalsample ; sample++) {

            //procuro todos os levels 2 com a amostra atual no loop
            var level03ResultBySample = level01Result.children('.level02Result[sample=' + sample + ']').children('.level03Result');



            var erro = 0;
            var level02Level03Names = "";
            level03ResultBySample.each(function (e) {

                var level03Result = $(this);
                var level02Result = level03Result.parents('.level02Result');

                var level03Erros = parseInt($(this).attr('value'));
                
                erro = erro + level03Erros;

                if(level03Erros > 0)
                {
                    var level02Name = $('.level02[level02id=' + level02Result.attr('level02id') + '] .levelName:first').text();
                    var level03Name = $('.level03[id=' + level03Result.attr('level03id') + '] .levelName:first').text();

                    var rowLevel02Level03Name = level03Erros + " " + level02Name + " " + level03Name; 

                    if(level02Level03Names == "")
                    {
                        level02Level03Names += rowLevel02Level03Name;
                    }
                    else
                    {
                        level02Level03Names += "," + rowLevel02Level03Name;
                    }
                } 
            });

            if (level02Level03Names)
            {
                level02Level03Names = " - " + level02Level03Names;
            }
            if (erro >= 3) {
                string3moreErros += 'Set ' + evaluate + ' Side ' + sample + ': ' + erro + ' defect(s)' + level02Level03Names + '\n';
                //evaluate, sample  3 erroas ou mais 
                //monto 3 o more defects
                //adiciono 1 defeito no 6ErrosSide
            }
            if (erro > 0) {
                Erros6Side++
                string6SidesWithErrosTemp += 'Set ' + evaluate + ' Side ' + sample + ': ' + erro + ' defect(s)' + level02Level03Names + '\n';
            }
        }   

        if (Erros6Side >= 6) {
            string6SidesWithErros = string6SidesWithErrosTemp;
        }

    });

    if (string3moreErros != "") {
        string3moreErros = "3 or More Defects \n" +
                       string3moreErros;
    }
    if (string6SidesWithErros != "") {
        string6SidesWithErros = "6 Sides With Defects \n" +
                                    string6SidesWithErros;
    }



    return (string3moreErros + "\n" + string6SidesWithErros);

    //alert("string3moreErros:\n" + string3moreErros);

    //alert("string6SidesWithErros:\n" + string6SidesWithErros);
}

function correctiveActionSignature() {

}

function dateFormat() {
    var date = new Date();
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var day = date.getDate();
    var mmddyyyy = ("0" + month).slice(-2) + "/" + ("0" + day).slice(-2) + "/" + year;

    return mmddyyyy;
}
function dateReturn() {

    var date = new Date();
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var day = date.getDate();

    var data = ("0" + month).slice(-2) + ("0" + day).slice(-2) + year;
    return data;

}
function dateTimeFormat() {
    var date = new Date();
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var day = date.getDate();
    var hour = date.getHours();
    var minute = date.getMinutes();
    var seconds = date.getSeconds();
    var mileseconds = date.getMilliseconds();
    var mmddyyyyhhmm = ("0" + month).slice(-2) + "/" + ("0" + day).slice(-2) + "/" + year + " " + ("0" + hour).slice(-2) + ":" + ("0" + minute).slice(-2) + ":" + ("00" + seconds).slice(-3) + ":" + ("00" + mileseconds).slice(-3);

    return mmddyyyyhhmm;
}

function dateTimeWithMinutes() {
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
}

var Geral = {
    exibirMensagemAlerta: function (mensagem, url, container) {
        var page = $("html, body");
        Geral.esconderMensagem(container);
        $('#messageAlert').find('span').text('');
        if (url == undefined || url.length == 0) {
            container = container || '';
            var $divMensagem = $(container + ' #messageAlert');
            $divMensagem.find('#mensagemAlerta').text(mensagem);
            $divMensagem.removeClass('hide');
        } else {
            //alert(mensagem);
            location.href = url;
        }
        $('html,body').animate({ scrollTop: 0 }, 'slow');
    },

    exibirMensagemErro: function (mensagem, url, container) {
        Geral.esconderMensagem(container);
        $('#messageError').find('span').text('');
        if (url == undefined || url.length == 0) {
            container = container || '';
            var $divMensagem = $(container + ' #messageError');
            $divMensagem.find('#mensagemErro').text(mensagem);
            $divMensagem.removeClass('hide');
        } else {
            //alert(mensagem);
            location.href = url;
        }
        $('html,body').animate({ scrollTop: 0 }, 'slow');
    },

    esconderMensagem: function (container) {
        container = container || '';
        $(container + ' #messageError').addClass('hide');
        $(container + ' #messageAlert').addClass('hide');
        $(container + ' #messageSuccess').addClass('hide');
    },

    exibirMensagemSucesso: function (mensagem, url, container) {
        if (mensagem == undefined || mensagem.length == 0) {
            Geral.esconderMensagem(container);
        } else {
            $('#messageSuccess').hide().find('span').text('');
            if (url == undefined || url.length == 0) {
                container = container || '';
                var $divMensagem = $(container + '#messageSuccess');
                $divMensagem.find('span').text(mensagem);
                $divMensagem.show();
            } else {
                //alert(mensagem);
                location.href = url;
            }
            $('html,body').animate({ scrollTop: 0 }, 'slow');
        }
    },
}
function showLevel01(inicio) {
    $('.login').fadeOut("fast", function (e) {
        $('.login').addClass('hide');

        //padding no windows
        if (device.platform == "windows")
            $('.container').css('padding-left', '20px').css('padding-right', '30px');

        //implementar no login
        var loginTime = new Date();
        $('#selectPeriod').val("0");
        $('.level01Result').removeClass('selected');
        configureLevel01();
        level02Reset($('.level02List .level02Group .level02'));
        $('.App').removeClass('hide').show();
        breadCrumb();
        $('.alert').addClass('hide');

        //mostrar link http se desenvolvimento
        if (urlPreffix.indexOf('QualityAssurance') < 0)
            $('.urlPrefix').text(urlPreffix);

        //PeriodHTMLDAO.appendHTML();
        loginFile();

       
        if (inicio == true) {
            //Sync();
            menssagemSync("Cheking Results", 'Syncronize results...');
            setTimeout(function (e) {

                if($('.level02Result[sync=false]').length)
                {
                    var resultOThersUnits = $('.level02Result[sync=false][unidadeid!=' + $('.App').attr('unidadeid') + ']:first');
                    if (resultOThersUnits.length)
                    {
                        openMessageModal('Sync outras unidades', 'Você precisa sincronizar os resultados da unidade ' + $('#selectUnit option[value=' + resultOThersUnits.attr('unidadeid') + ']').text());
                        $('#btnLogout').click();
                        return false;
                    }

                    $('#btnSync').click();
                }
                else
                {
                    mensagemSyncHide();
                    reciveResults();
                }
            }, 2000)
        }

        //startAutomaticSync();

    });
}
function configureLevel01() {


    $('.btnCA').addClass('hide');

    //resetLevel

    $('.level01').children('.icons').children('.areaComplete').addClass('hide');
    $('.level01').parents('li').removeClass('bgCompleted');

    $('.level01').parents('.row').children('.userInfo').children('div').children('.btnCorrectiveAction').addClass('hide');

    // $('.level01').parents('.row').children('.userInfo').children('div').children('.btnReaudit').addClass('hide');

    $('.level01, .level01').parents('li').removeClass('bgLimitExceeded');


    // var reauditCount = $('.reauditCount[count!=0]');

    // reauditCount.children('button').text(reauditCount.attr('count'));
    //$('.level01').parents('.row').children('.userInfo').children('div').children('.reauditCount').removeClass('hide');

    //$('.level01').parents('.row').children('.userInfo').children('div').children('.reauditCount[count!=0]').removeClass('hide').children('button').text();
    // $('.level01').parents('.row').children('.userInfo').children('div').children('.reauditCount').text($('.level01[reauditNumber]').attr('reauditCount')).addClass('hide');

    //configureLevel
    $('.level01[completed]').children('.icons').children('.areaComplete').removeClass('hide');
    $('.level01[completed]').parents('li').addClass('bgCompleted');

    $('.level01[correctivaction]').parents('.row').children('.userInfo').children('div').children('.btnCorrectiveAction').removeClass('hide');

    $('.level01[reaudit]').parents('.row').children('.userInfo').children('div').children('.btnReaudit').removeClass('hide');
    // $('.level01[reaudit]').parents('li').removeClass('bgLimitExceeded').addClass('bgCompleted');
    $('.level01[reaudit]').parents('li').removeClass('bgLimitExceeded');
    $('.level01[correctivaction]').parents('li').addClass('bgLimitExceeded').removeClass('bgCompleted');

    $('.level01[reauditNumber]').parents('.row').children('.userInfo').children('div').children('.reauditCount').text($('.level01[reauditNumber]').attr('reauditCount')).removeClass('hide');

    $('.level01[reaudit]').each(function (e) {
        //

        var level01 = $(this);
        var level01geraReaudit = getResultHaveReaudit(level01);

        var level01User = level01.siblings('.userInfo').children('div');

        level01User.children('.btnReaudit').children('.reauditPeriod').text($('#selectPeriod option[value=' + level01geraReaudit.attr('period') + ']').text());
        //level01User.children('.reauditCount').removeClass('hide'); 9

        //var totalReaudits = 0;
        //if (level01geraReaudit.attr('totalreaudits')) {
        //    totalReaudits = parseInt(level01geraReaudit.attr('totalreaudits'));
        //}


        //level01User.children('.reauditCount').children('button').text(totalReaudits + '/' + level01.attr('minreauditnumber'));

        //level01User.children('.reauditCount');
    });
}


function showLevel02(level01) {

    $('.level01').removeClass('selected');
    $('.painel .labelPainel').addClass('hide');
    level01.addClass('selected');

    $('.level01List').fadeOut("fast", function (e) {
        level01.parents('.level01List').addClass('hide');
        //if (level01.attr('startreaudit')) {
        //    $('span.auditReaudit').children('.name').text('Re-audit');
        //}
        //else {
        //    $('span.auditReaudit').children('.name').text('Audit').siblings('.reauditPeriod').text("");
        //}
        var level02 = $('.level02List');
        $('.level02Group').addClass('hide');

        var level02Group = level02.children('.level02Group[level01id=' + level01.attr('id') + ']');

        level02Group.removeClass('hide');


        $('.level02').parents('li').removeClass('bgCompleted');
        $('.level02').parents('.row').children('.userInfo').children('div').children('.btnCorrectiveAction').addClass('hide');
        $('.level02').parents('li').removeClass('bgLimitExceeded');
        $('.level02').children('.icons').children('.areaComplete').addClass('hide');


        $('.level02[completed]').children('.icons').children('.areaComplete').removeClass('hide');
        $('.level02[completed]').parents('li').addClass('bgCompleted');

        $('.level02[correctivaction]').parents('.row').children('.userInfo').children('div').children('.btnCorrectiveAction').removeClass('hide');



        $('.level02:visible').parents('li').children('.row').children('.level02').children('.icons').children('.areaComplete, .areaNotComplete').addClass('hide').siblings('.areaNotComplete').removeClass('hide');
        $('.level02:visible').parents('.row').children('.userInfo').children('div').children('.btnCorrectiveAction').addClass('hide');

        //var level02Complete = $()
        $('.level02[completed]').parents('li').children('.row').children('.level02').children('.icons').children('.areaComplete').removeClass('hide').siblings('.areaNotComplete').addClass('hide');
        $('.level02[completed]').parents('li').children('.row').children('.userInfo').children('div').children('.na').addClass('hide').siblings('.btnAreaSave').addClass('hide');
        //$('.level02[update]').removeAttr('update');

        // $('.level02[completed]').parents('li').children('.row').children('.userInfo').children('div').children('.areaComplete').removeClass('hide').siblings('.areaNotComplete, .na').addClass('hide').siblings('.btnAreaSave').addClass('hide');
        //$('.level02[notavaliable]').parents('li').children('.row').children('.level02').children('.icons').children('.areaComplete').removeClass('hide').siblings('.areaNotComplete').addClass('hide');
        // $('.level02[notavaliable]').parents('li').children('.row').children('.userInfo').children('div').children('.na').addClass('naSelected').removeClass('hide').siblings('.btnAreaSave').addClass('disabled').removeClass('hide');

        //$('.level02[notavaliable]').parents('li').children('.row').children('.userInfo').children('div').children('.btnAreaSave').addClass('hide').siblings('.btnNotAvaliable').addClass('hide').siblings('.btnReaudit').addClass('hide')
        $('.level02[reaudit]').parents('li').children('.row').children('.userInfo').children('div').children('.na').addClass('hide');

        $('.level02[startphasedate=null]').removeAttr('startphasedate');
        $('.level02[phase=0]').removeAttr('phase');

        //$('.level02[startphasedate], .level02[phase]').parents('li').children('.row').children('.userInfo').children('div').children('.na').removeClass('hide').siblings('.btnAreaSave').removeClass('hide');
        $('.level02:visible[correctivaction]').parents('.row').children('.userInfo').children('div').children('.btnCorrectiveAction').removeClass('hide');
        $('.level02[correctivaction]').parents('li').addClass('bgLimitExceeded').removeClass('bgCompleted');

        $('.level02[limitExceeded]').parents('li').addClass('bgLimitExceeded');
        //$('.level02[phase]').siblings('.userInfo').children('div').children('.na').addClass('hide').siblings('.btnAreaSave').addClass('hide');
        $('.level02[notavaliable="notavaliable"]').parents('li').addClass('bgNoAvaliable').children('.row').children('.userInfo').children('div').children('.btnAreaSave').removeClass('hide').addClass('disabled').siblings('.btnNotAvaliable').removeClass('hide').addClass('naSelected');

        $('.level02[havecorrectiveaction]').siblings('.userInfo').children('div').children('.btnCorrectiveAction').removeClass('hide');
        // btnCorrectiveAction();

        $('.painel .labelPainel[level01id=' + level01.attr('id') + ']').removeClass('hide');
        $('.iconReturn').removeClass('hide');

        if (level01.attr('continuereaudit')) {
            level01.removeAttr('continuereaudit').removeAttr('startreaudit');
        }

        buttonsNavMenuShow(level01);
        breadCrumb($('.level01List .selected').text());

        var level01Result = $('.level01Result.selected');
        $('.level02Group[level01id=' + level01Result.attr('level01id') + '] .btnNotAvaliable').removeAttr('disabled');

        if (level01Result.attr('completed')) {
            $('.level02Group[level01id=' + level01Result.attr('level01id') + '] .btnAreaSave').addClass('hide');
            $('.level02Group[level01id=' + level01Result.attr('level01id') + '] .btnNotAvaliable').attr('disabled', 'disabled');

        }
        $('span.auditReaudit').children('.name').text('Audit');
        if (level01Result.attr('reaudit') == "true" || level01.attr('startreaudit')) {
            $('span.auditReaudit').children('.name').text('Re-audit');

        }
        $('span.Completed').parents('.labelPainel').addClass('hide');

        var evaluate = parseInt(level01Result.attr('evaluate'))

        var evaluateConf = 1;
        if (level01.attr('haveevaluates')) {
            evaluateConf = parseInt($('.level02Group[level01id=' + level01.attr('id') + '] .level02:first').attr('totalsets'));
        }


        if (level01Result.attr('completed')) {
            if (!level01.attr('startreaudit') && evaluate >= evaluateConf) {
                $('span.Completed').parents('.labelPainel').removeClass('hide');
            }

        }
        else if (!level01Result.attr('completed')) {
            evaluate = level01Result.attr('evaluate')
        }
        else {
            evaluate = 0;
            $('.level01Result.selected').removeClass('selected');
        }


        if (level01.attr('haveevaluates')) {

            if (!level01Result.attr('completedsample') && evaluate > 0) {
                evaluate--;
            }
            if (isNaN(evaluate)) {
                evaluate = 0;
            }

            $('span.setsDone').text(evaluate);
        }

        //Hack para resolver ação correctiva no HTP, verificar onde coloca as informações divergentes que coloca o botão de ação corretiva no NotAvaliable e no Completed sem erro
        $('.level02[notavaliable]').removeAttr('havecorrectiveaction');
        $('.level02Group[level01id=1] .level02[defects=0]').removeAttr('havecorrectiveaction');

        //se o overlay estiver visivel
        if ($(level02).hasClass("hide"))
            scrollClick($($('.level02Group[level01id=' + level01.attr('id') + ']').children('.list-group-item').children('.row').children('.level02[completed!=completed]')[0]).attr('id'));

        level02.removeClass('hide').show();


        level02Buttons(level02Group);

        //Hack para resolver ação correctiva no HTP, verificar onde coloca as informações divergentes que coloca o botão de ação corretiva no NotAvaliable e no Completed sem erro

        $('.level02[notavaliable]').removeAttr('havecorrectiveaction').siblings('.userInfo').children('div').children('.btnCorrectiveAction').addClass('hide');
        $('.level02Group[level01id=1] .level02[defects=0]').removeAttr('havecorrectiveaction').siblings('.userInfo').children('div').children('.btnCorrectiveAction').addClass('hide');
        

        //$('.painel.painelLevel02').parent().removeClass('col-xs-12').removeClass('col-xs-9').removeClass('col-xs-6').removeClass('col-xs-3').addClass('col-xs-3').removeClass('hide');

        //var listPanelLevel02 = $('.painel.painelLevel02');

        //for (var i = 0; i < listPanelLevel02.length; i++) {
        //    if ($($('.painel.painelLevel02')[i]).children(':visible').length == 0) {
        //        $($('.painel.painelLevel02')[i]).parent().addClass('hide');
        //    }
        //}

        //$('.painel.painelLevel02:visible:last').parent().addClass('col-xs-' + (($($('.painel.painelLevel02')).parent('.hide').length * 3) + 3)).removeClass('smallPaddingRight');


    });
}


//function level02Configure() {
//    $('.level02[completed]').parents('li').addClass('bgCompleted');
//    $('.level02[limitExceeded]').parents('li').addClass('bgLimitExceeded');
//}
function buttonsNavMenuShow(level01) {
    $('.buttonMenu[level01id=' + level01.attr('id') + ']').removeClass('hide');
    // level02ButtonSave($('.level02Group[level01id=' + level01.attr('id') + ']'));
}
function buttonsLevel02Hide() {
    $('.buttonMenu').addClass('hide');
}
function checkInputsSelect() {


    //All Itens
    var message = '';

    if ($('#selectPeriod option:selected').val() == 0) {

        message = "Select the period<br>";
    }
    if ($('#selectCattleType:visible option:selected').val() == "0") {
        message = message + "Select the cattle type<br>";
    }

    if ($('#inputChainSpeed:visible').val() == "") {
        message = message + "Input the chain speed<br>";
    }

    if ($('#inputLotNumber:visible').val() == "") {
        message = message + "Input the lot number<br>";
    }

    if ($('#inputMudScore:visible').val() == "") {
        message = message + "Input the mud score<br>";
    }

    if ($('#biasedUnbiased:visible option:selected').val() == "0") {
        message = message + "Select if biased or unbiased<br>";
    }

    if (message == '') {
        return true;
    } else {
        openMessageModal('Select the following items to continue:', message);
        return false;
    }

}
function showLevel03(level02) {

    var itensOk = checkInputsSelect();
    if (itensOk == false) {
        return false;
    }

    if (level02.attr('notavaliable')) {
        return true;
    }
    buttonsLevel02Hide();
    $('.level02').removeClass('selected');

    level02.addClass('selected');
    //if (level02.attr('completed'))
    //{
    //    level02.attr('update', 'update');
    //}

    if (!level02.attr('defects')) {
        level02.attr('defects', '0');
    }
    //if (level02.attr('consecutivefailure')) {
    //    $('.painelLevel03 .consecutiveFailure').text(level02.attr('consecutivefailure'));
    //}
    $('.level02List').fadeOut("fast", function (e) {
        level02.parents('.level02List').addClass('hide');
        var level03 = $('.level03List');
        level03.removeClass('hide').show();
        $('.level03Group').addClass('hide');
        $('.painel .defects').text(level02.attr('defects'));
        defectLimitCheck();

        level03.children('.level03Group[level01id=' + $('.level01.selected').attr('id') + ']').removeClass('hide');

        breadCrumb($('.level01List .selected').text(), $('.level02List .level02.selected span.levelName').text());
        $('.btnCA').addClass('hide');

        $('#btnSave').addClass('hide');
        $('.level03Group:visible input').removeAttr('disabled');
        $('.level03Group:visible span.input-group-btn').removeClass('hide');

        if (!$('span.Completed').is(':visible')) {
            $('#btnSave').removeClass('hide');
        }
        else {
            $('.level03Group:visible span.input-group-btn').addClass('hide');
            $('.level03Group:visible input').attr('disabled', 'disabeld');
        }

        // $('.painel.painelLevel03').parent().removeClass('col-xs-12').removeClass('col-xs-9').removeClass('col-xs-6').removeClass('col-xs-3').addClass('col-xs-3').removeClass('hide').add('smallPaddingRight').add('smallPaddingRight');

        //var listPanelLevel03 = $('.painel.painelLevel03');

        //for (var i = 0; i < listPanelLevel03.length; i++) {
        //    if ($($('.painel.painelLevel03')[i]).children(':visible').length == 0) {
        //        $($('.painel.painelLevel03')[i]).parent().addClass('hide');
        //    }
        //}

        //$('.painel.painelLevel03:visible:last').parent().addClass('col-xs-' + (($($('.painel.painelLevel03')).parent('.hide').length * 3) + 3)).removeClass('smallPaddingRight');
    });

}

function breadCrumb(level01, level02) {
    var bdCrumb = $('.breadcrumb');

    bdCrumb.children('li:gt(0)').remove();


    var mainLevel = "<li><a href='#' class='main'>" + $('.App').attr('breadMainLevel') + "</a></li>"
    var level01Li = "";
    if (level01 != null && level01 != undefined) {

        level01Li = "<li><a href='#' class='level01'>" + level01 + "</a></li>";
        if (level02 == null || level02 == undefined) {
            level01Li = "<li class='active'>" + level01 + "</li>";
        }
    }

    var level02Li = "";
    if (level02 != null && level02 != undefined) {
        level02Li = "<li class='active'>" + level02 + "</li>";
    }


    bdCrumb.html(
					mainLevel +
					level01Li +
					level02Li
							);


}

//$(document).on('click', '.breadcrumb', function (e) {
//    $('#messageContent').text('ppppppppp');
//    var $modal = $('#modalMessage');
//    $modal.modal();
//});
$(document).on('click', '#btnMore', function (e) {
    rightMenuShow();
});
$(document).on('click', function (e) {
    //  confirmButtonLevel02Hide();
    rightMenuHide();
    imageHide();
});
$(document).on('click', '.overlay', function (e) {
    if ($('.btnAreaSaveConfirm:visible').length) {
        $(this).hide();
        $('.btnAreaSaveConfirm:visible').addClass('hide').siblings('.btnAreaSave').removeAttr('disabled');
    }
});
function confirmButtonLevel02Hide() {
    if ($('.btnAreaSaveConfirm').is(':visible')) {
        $('.overlay').hide();
        $('.btnAreaSaveConfirm').addClass('hide').siblings('.btnAreaSave').removeClass('hide');
    }
}
$(document).on('click', '#btnShowImage', function (e) {
    imageShow();
});
function rightMenuShow() {
    $('.overlay').fadeIn('fast');
    $(".rightMenu").animate({
        right: "0px"
    }, "fast", function () {
        $(this).addClass('visible');
    });
}
function imageShow() {
    $('.overlay').fadeIn('fast');
    $(".cffImage").animate({
        left: "0px"
    }, "fast", function () {
        $(this).addClass('visible');
    });
}
$(document).on('click', '#btnLogout', function (e) {
    $('.App').fadeOut("fast", function (e) {
        $('.login').removeClass('hide').show();
        $('#inputUserName').val("").focus();
        $('#inputPassword').val("");
        $("#shift").val("0").change();
        createFileResult();
        initializeApp();
    });
});
$(document).on('click', '#btnClearDatabase', function (e) {

    var message = '';

    if($('.level02Result[sync=false]').length > 0){
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
function rightMenuHide() {
    if ($('.rightMenu').hasClass('visible')) {
        $(".rightMenu").removeClass('visible').animate({ "right": "-151px" }, "fast", function (e) {
            $('.overlay').hide();
        });
    }
}
function imageHide() {
    if ($('.cffImage').hasClass('visible')) {
        $(".cffImage").removeClass('visible').animate({ "left": "-256px" }, "fast", function (e) {
            $('.overlay').hide();
        });
    }
}
function initializeApp() {
    $('.level03List').addClass('hide');
    $('.level02').removeClass('selected');
    $('.level02List').addClass('hide');
    $('.level01').removeClass('selected');
    $('.level01List').removeClass('hide').show();
    $('#btnLogin').button('reset');
    //stopAutomaticSync();
}
function level01Return() {

    var levelAtual = '.level02List';
    if (!$('.level03List').hasClass('hide')) {
        levelAtual = '.level03List'
    }

    $(levelAtual).fadeOut("fast", function (e) {
        $(this).addClass('hide');
        $('.level02').removeClass('selected');
        buttonsLevel02Hide();
        configureLevel01();
        $('.level01').removeClass('selected');
        $('#btnSave').addClass('hide');
        $('.level01List').removeClass('hide').show();
        $('.iconReturn').addClass('hide');
        breadCrumb();
    });

    $('.correctiveaction').addClass('hide');
}

function level02Return() {
    var level01 = $('.level01.selected');
    $('.level03List:visible').fadeOut("fast", function (e) {
        $(this).addClass('hide');
        $('#btnSave').addClass('hide');
        $('.level02').removeClass('selected');
        $('.level02[update]').removeAttr('update');
        //$('.level02List').removeClass('hide').show();
        //btnCorrectiveAction();
        // buttonsLevel02Show(level01);
        breadCrumb($('.level01List .selected').text());
        showLevel02(level01);
    });
}
$(document).on('click', '.breadcrumb .main', function (e) {
    level01Return();
});

$(document).on('click', '.breadcrumb .level01', function (e) {
    level02Return();
});
$(document).on('click', 'a.navbar-brand', function (e) {
    $('.breadcrumb li a:last').click();
});
$(document).on('change', '.login #shift', function (e) {
    if ($('.login #shift option:selected').val() == 0) {
        $('#inputUserName, #inputPassword, #btnLogin').attr('disabled', 'disabled');
        $('.login #shift').focus();
    }
    else {
        $('#inputUserName, #inputPassword, #btnLogin').removeAttr('disabled');
        $('#inputUserName').focus();
    }
});
$(document).on('click', '.btn-plus', function (e) {
    var input = $(this).siblings('input');
    var oldValue = input.val();
    var newVal = parseInt(oldValue) + 1;
    input.val(newVal);
    input.trigger('input');
});
$(document).on('click', '.btn-minus', function (e) {
    var input = $(this).siblings('input');
    var oldValue = input.val();
    var newVal = parseInt(oldValue) - 1;
    if (newVal < 0) {
        return false;
    }
    input.val(newVal);
    input.trigger('input');

});

$(document).on('click', '.btnSlaugtherSignatureRemove', function (e) {
    $('.SlaugtherSignature').addClass('hide');
    $('.btnSlaugtherSignature').removeClass('hide');
    $('.SlaugtherSignature').removeAttr('userid');
});

$(document).on('click', '.btnTechinicalSignatureRemove', function (e) {
    $('.TechinicalSignature').addClass('hide');
    $('.btnTechinicalSignature').removeClass('hide');
    $('.TechinicalSignature').removeAttr('userid');
});

$(document).on('click', '.btnSlaugtherSignature', function (e) {
    var correctiveActionModalSignature = $('#modalSignatureCorrectiveAction');
    var heads = correctiveActionModalSignature.children('.panel-body').children('.modal-header');
    heads.children('.head').addClass('hide');
    heads.children('.slaughtersig').removeClass('hide');
    correctiveActionModalSignature.removeClass('hide').attr('signature', 'slaugther');
    correctiveActionSignatureModalOpen();
});

$(document).on('click', '.btnTechinicalSignature', function (e) {
    var correctiveActionModalSignature = $('#modalSignatureCorrectiveAction');
    var heads = correctiveActionModalSignature.children('.panel-body').children('.modal-header');
    heads.children('.head').addClass('hide');
    heads.children('.techinicalsig').removeClass('hide');
    correctiveActionModalSignature.removeClass('hide').attr('signature', 'techinical');
    correctiveActionSignatureModalOpen();

});
function correctiveActionSignatureModalOpen() {
    $('.alert').addClass('hide');
    var correctiveActionModal = $('#correctiveActionModal');
    correctiveActionModal.css('z-index', '9997');
    var correctiveActionModalSignature = $('#modalSignatureCorrectiveAction');
    $('#modalSignatureCorrectiveAction input').val("");
    correctiveActionModalSignature.fadeIn("fast", function (e) {
        $('#modalSignatureCorrectiveAction input:first').focus();
        $(".formCorrectiveAction").animate({
            scrollTop: 0
        }, 100);
    });
}
$(document).on('input', '.level03Group[level01id=2] input', function (e) {
    inputChangesUpdate($(this));
    level03AlertAdd($(this));
});
$(document).on('input', '.level03Group[level01id=3] input', function (e) {
    inputChangesUpdate2($(this));
    level03AlertAdd($(this));
});

function level03AlertAdd(input) {
    var valor = parseInt(input.val());
    if (valor > 0) {
        input.parents('li, div.level03').addClass('bgAlert');
    }
    else {
        input.parents('li, div.level03').removeClass('bgAlert');
    }
}
function inputChangesUpdate(input) {

    var level02 = $('.level02.selected');
    var level03 = input.parents('.level03');

    var valorInput = input.val();
    if (valorInput == "") {
        valorInput = "0";
    }

    var valorInputDefects = parseInt(valorInput);
    var level02Defects = 0;
    $('.level03Group:visible .level03 input').each(function (e) {

        var valor = $(this).val();
        if (valor == "") {
            valor = "0";
        }
        level02Defects = level02Defects + parseInt(valor);
    });

    level02.attr('defects', level02Defects);

    var level02ErrorLimit = parseInt(level02.attr('levelerrorlimit'));
    $('.painel .defects').text(level02Defects);


    if (level02Defects > level02ErrorLimit) {
        $('.painel .defects').parents('.labelPainel').addClass('red');
    }
    else {
        $('.painel .defects').parents('.labelPainel').removeClass('red');
    }


    level02.siblings('.userInfo').children('div').children('.defects').text(level02Defects);

    var level01Defects = 0;

    level02.parents('li').siblings('li').children('.row').children('.level02[defects]').each(function (e) {
        var valor = $(this).attr('defects');
        if (valor == "") {
            valor = "0";
        }
        level01Defects = level01Defects + parseInt(valor);

    });


    level01Defects = level01Defects + level02Defects;

    $('.painel .totalDefects').text(level01Defects);
    if (level01Defects > 22) {
        $('.painel .totalDefects').parents('.labelPainel').addClass('red');
    }
    else {
        $('.painel .totalDefects').parents('.labelPainel').removeClass('red');
    }

}



function inputChangesUpdate2(input) {
    var level02 = $('.level02.selected');
    var level03 = input.parents('.level03');

    var valorInput = input.val();
    if (valorInput == "") {
        valorInput = "0";
    }

    var valorInputDefects = parseInt(valorInput);

    var level03Defects = 0;

    $('.level03Group:visible .level03 input').each(function (e) {

        var valor = $(this).val();
        if (valor == "") {
            valor = "0";
        }
        level03Defects = level03Defects + parseInt(valor);
    });

    var level03Group = $('.level03Group:visible');
    var sidesWithErros = parseInt($('.painelLevel02 .sideWithErrors').text());
    //os defeitos serao calculados por 3 defeitos ou mais e por 6 lados com defeito
    if (level03Defects == 0 && level03Group.attr('firstErrorSide')) {
        sidesWithErros = sidesWithErros - 1;
        level03Group.removeAttr('firstErrorSide');
    }
    else if (level03Defects > 0 && !level03Group.attr('firstErrorSide')) {

        sidesWithErros = sidesWithErros + 1;
        level03Group.attr('firstErrorSide', 'firstErrorSide');
    }

    $('.sideWithErrors').text(sidesWithErros);

    if (sidesWithErros > 5) {
        $('.sideWithErrors').parents('.labelPainel').addClass('red');
    }
    else {
        $('.sideWithErrors').parents('.labelPainel').removeClass('red');
    }

    var sidesWith3ErrosMore = parseInt($('.painelLevel02 .more3Defects').text());

    if (level03Defects < 3 && level03Group.attr('Error3MoreSide')) {
        sidesWith3ErrosMore = sidesWith3ErrosMore - 1;
        level03Group.removeAttr('Error3MoreSide');
        $('.sideErros').parents('.labelPainel').removeClass('red');

    }
    else if (level03Defects > 2 && !level03Group.attr('Error3MoreSide')) {
        sidesWith3ErrosMore = sidesWith3ErrosMore + 1;
        level03Group.attr('Error3MoreSide', 'Error3MoreSide');
        $('.sideErros').parents('.labelPainel').addClass('red');
    }

    $('.more3Defects').parents('.labelPainel').removeClass('red');

    if (sidesWith3ErrosMore > 0) {
        $('.more3Defects').parents('.labelPainel').addClass('red');
    }

    $('.painel .more3Defects').text(sidesWith3ErrosMore);

    var total6Erros = parseInt($('.sideWithErrors:first').text());

    var total3Erros = parseInt($('.more3Defects:first').text())

    //Culpa do Lucas
    if (total6Erros > 5 || total3Erros > 0) {
        level02.attr('defects', "1");
    }



    $('.painel .sideErros').text(level03Defects);

}




function defectLimitCheck() {
    var level02 = $('.level02.selected');

    var defectsLevel02 = parseInt(level02.attr('defects'));
    var defectsLimit = parseInt(level02.attr('levelerrorlimit'));

    var defectsDiv = $('.painelLevel03 .defects').parent('div');
    defectsDiv.removeClass('red');
    var btnNA = level02.siblings('.userInfo').children('div').children('.na');
    //btnNA.addClass('hide');



    if (defectsLevel02 > defectsLimit) {
        defectsDiv.addClass('red');
        level02.attr('limitExceeded', 'limitExceeded');
        if (!level02.attr('correctiveActionComplete'))
            level02.attr('havecorrectiveaction', 'havecorrectiveaction');
        level02.parents('li').addClass('bgLimitExceeded');
    }
    else {
        defectsDiv.removeClass('red');
        level02.removeAttr('limitExceeded');
        level02.parents('li').removeClass('bgLimitExceeded');
    }

}

$(document).on('click', '.modal-close-ca', function (e) {
    $('body').removeClass('overflowNo');
    $('.btnCorrectiveAction').removeClass('selected');
    $('.correctiveActionSelected').removeClass('correctiveActionSelected');
    var ca = $(this).parents('.modal-padrao');
    $('#correctiveActionModal .modal-body .row').scrollTop("0");

    ca.fadeOut("fast", function (e) {

        $(this).addClass('hide');
        $('.overlay').hide();
        if ($('.btnCA').hasClass('caLevel02')) {
            $('.btnCA').removeClass('hide').removeClass('caLevel02');
            if ($('#btnSave').hasClass('caLevel02')) {
                $('#btnSave').removeClass('hide').removeClass('caLevel02');
            }
        }
    });
});


$(document).on('click', '.modal-close-signature', function (e) {
    $('#correctiveActionModal').css('z-index', '9999');
    var ca = $(this).parents('.modal-padrao');
    //$('#correctiveActionModal .modal-body .row').scrollTop("0");

    ca.fadeOut("fast", function (e) {

        $(this).addClass('hide');

    });

});
$(document).on('click', '.button-expand', function (e) {
    $(this).parents('.level03Group').children('.panel-group').children('.panel').children('.collapse').collapse('show');
});
$(document).on('click', '.button-collapse', function (e) {
    $(this).parents('.level03Group').children('.panel-group').children('.panel').children('.in').collapse('hide');
});
//colocar uma forma de identificar campos yes or not
$(document).on('click', '.level03Group[level01id=1] .level03', function (e) {

    var level02 = $('.level02.selected');

    var level03 = $(this);

    var response = level03.children('.row').children('div').children('span.response');
    //acho que demvemos fazer um atrivuto direto no level03 para nao ficar tentando executar para todos
    if (response.length) {

        if (response.attr('value') == '0') {
            response.text('Yes').attr('value', '1');

            //verificar se exite mais level03 excedido
            //se nao existir remove a regra de limite execido do level02 e zero os deveitos
            level03.removeClass('lightred').removeAttr('notconform');
            if ($('.level03Group[level01id=1] .level03[notConform]').length == 0) {
                level02.removeAttr('limitexceeded').parents('li').removeClass('bgLimitExceeded');
            }

        }
        else {

            // $('.panel-group').children('.panel').children('.collapse').collapse('show');
            //$('#accordion').children('.panel').children('.collapse').collapse('show');
            level02.attr('limitexceeded', 'limitexceeded').parents('li').addClass('bgLimitExceeded');
            level03.addClass('lightred').attr('notConform', 'notCoform');
            response.text('No').attr('value', '0');
        }

        level02.attr('defects', $('.level03[notConform]').length);
        level02.attr('level03' + $(this).attr('id'), response.attr('value'));
        // level02Configure();
    }

});
$(document).on('click', '.na', function (e) {
    var itensOk = checkInputsSelect();
    if (itensOk == false) {
        return false;
    }
    var botaoSalvar = $(this).siblings('.btnAreaSave');
    var iconCompleto = $(this).parents('li').children('.row').children('.level02').children('.icons').children('.areaComplete');
    var iconNaoCompleto = $(this).parents('li').children('.row').children('.level02').children('.icons').children('.areaNotComplete');
    var level01 = $('.level01.selected');
    var level02 = $(this).parents('li').children('.row').children('.level02');

    if ($(this).hasClass('naSelected')) {
        $(this).removeClass('naSelected');
        botaoSalvar.removeClass('disabled');
        iconCompleto.addClass('hide');
        iconNaoCompleto.removeClass('hide');
        level02.removeAttr('notavaliable').removeAttr('completed');
        level02.parents('li').removeClass('bgNoAvaliable').removeClass('bgCompleted');
        $('#btnSave').addClass('hide');
        //var result = $('.level01Result[level01id=' + $('.level01.selected').attr('id') + '][shift=' + $('.App').attr('shift') + '][date=' + $('.App').attr('date') + '][period=' + $('.App').attr('period') + ']:last').children('.level02Result[level02id=' + level02.attr('id') + ']:last');
        //result.attr('remove', true).attr('datetime', dateTimeFormat());
        // buttonsLevel02Show(level01);
    }
    else {
        $(this).addClass('naSelected');
        botaoSalvar.addClass('disabled');
        iconCompleto.removeClass('hide');
        iconNaoCompleto.addClass('hide');
        level02.attr('notavaliable', 'notavaliable');
        level02.parents('li').addClass('bgNoAvaliable');
        level02.parents('li').children('.row').children('.userInfo').children('div').children('.btnAreaSaveConfirm').click();
    }
    if ($('.level01.selected').attr('id') == '1') {
        $('.btnAreaSave').parents('.bgNoAvaliable').find('.btnAreaSave').removeClass('hide');
        $('.btnNotAvaliable').parents('.bgNoAvaliable').find('.btnNotAvaliable').removeClass('hide');
    }

});
$(document).on('click', '.btnAreaSave', function (e) {
    var itensOk = checkInputsSelect();
    if (itensOk == false) {
        return false;
    }
    $('.overlay').show();
    $(this).siblings('.btnAreaSaveConfirm').removeClass('hide');
    $(this).attr('disabled', 'disabled');
    $(this).siblings('.defects').addClass('hide');

    //$(this).addClass('disabled', function (e) {
    //    alert('mae');
    //});
    //$(this).parents('button').addClass('hide');

});

$(document).on('click', '.btnAreaSaveConfirm', function (e) {

    if ($('.App').attr('logintime').substring(0, 10).replace("/", "").replace("/", "") == dateReturn()) {

        if ($(this).hasClass('disabled')) {
            return false;
        }
        $('.level02').removeClass('selected');
        var level02 = $(this).parents('li').children('.row').children('.level02');
        level02.addClass('selected');
        //buscar pelo atributo e nao pelo botao
        if (level02.siblings('.userInfo').children('div').children('.btnPhase').is(':visible')) {

        }

        $('.level03Group[level01id=' + level02.parents('.level02Group').attr('level01id') + ']').children('.level03Confirm').click();
        scrollClick(level02.attr('id'));

        $('.overlay').hide();
        $(this).siblings('.defects').removeClass('hide');

    } else {
        resetDay();
    }

});
//param: id do level02
function scrollClick(level02) {
    if (level02) {
        //pego a posicao do id para colocar para colocar no meio da tela
        var value = $('#' + level02).offset().top - ($(window).height() / 2);

        var body = $('body');
        if (device.platform == "windows")
            body = $('html');

        //se o scroll da tela for diferente da localizacao do level02
        //if ($(window).height() > value)

        //faco a animacao para colocar no meio da tela
        body.animate({
            scrollTop: value
        }, 'fast');
    }

};

function level01Reset(level01) {

    level01.each(function (e) {

        var level = $(this);


        var btnCorrectivAction = level.parents('.row').children('.userInfo').children('div').children('.btnCorrectiveAction');
        var btnReaudit = level.parents('.row').children('.userInfo').children('div').children('.btnReaudit');
        var reauditCount = level.parents('.row').children('.userInfo').children('div').children('.reauditCount');



        btnCorrectivAction.addClass('hide');
        btnReaudit.addClass('hide');
        reauditCount.addClass('hide');
        level.children('.icons').children('.iconsArea').addClass('hide');
        level.removeAttr('completed').removeAttr('correctivaction').removeAttr('startreaudit').removeAttr('reauditnumber').removeAttr('reaudit').removeClass('reauditnumber').parents('li').removeClass('bgLimitExceeded').removeClass('bgCompleted');

    });


}
function level02Reset(level02) {

    var level02Group = level02.parents('.level02Group');

    level02Level03Reset(level02Group);

    //CCA
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .totalDefects').text('0');
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .defects').text('0');
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] #selectCattleType').val('0');
    $('#inputChainSpeed').val("");
    $('#inputLotNumber').val("");
    $('#inputMudScore').val("");
    $('#biasedUnbiased').val('0');
    $('.btnAreaSave').removeAttr('disabled');
    $('.btnAreaSaveConfirm').addClass('hide');

    // $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] #inputChainSpeed, .labelPainel[level01id=' + level02Group.attr('level01id') + '] #inputLotNumber, .labelPainel[level01id=' + level02Group.attr('level01id') + '] #inputMudScore').val("");
    $('.painel .labelPainel').removeClass('red');;
    //CFF
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .setsDone').text('0');
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .sideWithErrors').text('0').parents('.labelPainel').removeClass('red');
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .more3Defects').text('0').parents('.labelPainel').removeClass('red');
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .setAtual').text('1');
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .sideAtual').text('1');
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .sideErros').text('0');

    $('.btnNotAvaliable').removeClass('naSelected');
    $('span.auditReaudit').children('.name').text('Audit').siblings('.reauditPeriod').text("");

    $('.level02').siblings('.userInfo').children('div').children('.consecutiveFailure, .defects').text('0');
    level02Group.children('li').children('.row').children('.level02').removeAttr('havecorrectiveaction').removeAttr('limitexceeded').removeAttr('havereaudit');


    level02.removeAttr('completed').removeAttr('reaudit').removeAttr('havereaudit').removeAttr('notavaliable').removeAttr('limitexceeded').attr('defects', '0').parents('.row').children('.userInfo').children('div').children('.defects').text(level02.attr('defects'));
    level02.parents('li').removeClass('bgCompleted').removeClass('bgLimitExceeded').removeClass('bgNoAvaliable');



    var botaoNa = level02.parents('.row').children('.userInfo').children('div').children('.na');
    var botaoSalvarLevel02 = level02.parents('.row').children('.userInfo').children('div').children('.btnAreaSave');
    var iconCompleto = level02.children('.icons').children('.areaComplete');
    var iconNaoCompleto = level02.children('.icons').children('.areaNotComplete');

    botaoNa.removeClass('hide');
    botaoSalvarLevel02.removeClass('hide').removeClass('disabled');
    iconCompleto.addClass('hide');
    iconNaoCompleto.removeClass('hide');
    $('#btnSave, .btnCA').addClass('hide');


}

function level02Level03Reset(level02Group) {


    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .defects').text('0');

    $('.level03Group[level01id=' + level02Group.attr('level01id') + '] .level03').each(function (e) {

        $('.level02Group[level01id=' + level02Group.attr('level01id') + '] .level02').removeAttr('level03' + $(this).attr('id'));
    });

    $('.level03Group[level01id=' + level02Group.attr('level01id') + '] .level03').removeClass('lightred').removeAttr('notconform');
    $('.level03Group[level01id=' + level02Group.attr('level01id') + '] .level03 .row div span.response').attr('value', '0').text('Yes');
    $('.level03Group[level01id=' + level02Group.attr('level01id') + '] .level03 textarea').val('');
}

function level02Complete(level02) {

    level02.each(function (e) {

        var level = $(this);
        var level01 = $('.level01[id=' + level.parents('level02Group').attr('level01id') + ']');

        level.attr('completed', 'completed');
        level.parents('li').addClass('bgCompleted');

        var botaoNa = level.parents('.row').children('.userInfo').children('div').children('.na');
        var botaoSalvarLevel02 = level.parents('.row').children('.userInfo').children('div').children('.btnAreaSave');
        var btnSalvarLevel02Confirm = level.parents('.row').children('.userInfo').children('div').children('.btnAreaSaveConfirm');
        var iconCompleto = level.parents('.row').children('.level02').children('.icons').children('.areaComplete');
        var iconNaoCompleto = level.parents('.row').children('.level02').children('.icons').children('.areaNotComplete');

        iconCompleto.removeClass('hide');
        iconNaoCompleto.addClass('hide');
        botaoNa.addClass('hide');
        botaoSalvarLevel02.addClass('hide').removeAttr('disabled');
        btnSalvarLevel02Confirm.addClass('hide');
        $('#btnSave').addClass('hide');
        //level02ButtonSave(level.parents('.level02Group'));
        if (level01.attr('havephases')) {
            if (level02.attr('completed') && level02.attr('limitexceeded')) {
                botaoSalvarLevel02.removeClass('hide');
                botaoNa.removeClass('hide');
            }
        }

        if (level01)
            //nao precisamos verificar os defeitos se o limitexceeded estiver 
            var defectsLevel02 = parseInt($('.painelLevel03 .defects').text());
        var defectsLimit = parseInt(level.attr('levelerrorlimit'));
    });
    //fazer uma funcao para melhorar    
}


function level02Buttons(level02Group) {

    //Selecione o level01 a partir do grupo informado
    var level01 = $('.level01[id=' + level02Group.attr('level01id') + ']');

    //Verifico o resultado do level01 selecionado
    var level01ResultSelected = $('.level01Result.selected');

    //Instancio level01Completed como false para verificar se o level01 foi todo completo.
    var level01Completed = false;
    //se o resultado do level01Selecionado for completo atribuo level01Completed como true.
    if (level01ResultSelected.attr('completed')) {
        level01Completed = true;
    }

    //Instancio level02Completed como false para verificar se o level02 foi todo completo.
    var level02Complete = false;

    var evaluate = parseInt(level01ResultSelected.attr('evaluate'))


    //Instancio se tem atualizações
    var haveEvaluates = false;
    if (level01.attr('haveevaluates')) {
        haveEvaluates = true;
    }

    //se o resultado do level02Group for completo e não tenho avaliacoes atribuo level02Complete como true.
    if (level02Group.children('li').children('.row').children('.level02[completed!=completed]').length == 0 && level01ResultSelected.length && !level01ResultSelected.attr('completed') && haveEvaluates == false) {
        level02Complete = true;
    }

    else if (haveEvaluates == true && level01ResultSelected.attr('completedsample')) {
        var evaluateConf = 1;
        if (level01.attr('haveevaluates')) {
            evaluateConf = parseInt($('.level02Group[level01id=' + level01.attr('id') + '] .level02:first').attr('totalsets'));
        }

        if (evaluate >= evaluateConf) {
            level02Complete = true;
        }
    }


    //Oculto #btnSave.
    $('#btnSave').addClass('hide');
    //Oculto btnCA
    $('.btnCA').addClass('hide');

    //Se o level01 faz ação corretiva pelo level01 e o resultado tem ação corretiva mostro .btnCA
    //(level01ResultSelected.attr('havecorrectiveaction') || level01ResultSelected.children('.level02Result[havecorrectiveaction]').length)

    if (!level01.attr('correctivactionlevel02') && ($('.level01Result[level01id=' + level01.attr('id') + '][completed][havecorrectiveaction]').length || $('.level01Result[level01id=' + level01.attr('id') + '][completed] .level02Result[havecorrectiveaction]').length)) {

        var level02CA = $('.level01Result[level01id=' + level01.attr('id') + '][completed] .level02Result[havecorrectiveaction]');
        if(level02CA.length && level02CA.parents('.level01Result').attr('correctiveactioncomplete'))
        {
            level02CA.removeAttr('havecorrectiveaction').attr('correctiveactioncomplete', 'correctiveactioncomplete');
        }
        else
        {
            btnCAShow();
        }
    }

    //Se podemos salvar pelo level02 e o level02 está visivel e está completo e não finalizados o resultado do level01 (faltou somente clicar em salvar) mostramos o botão
    if (level01.attr('saveLevel02') && $('.level02Group').is(':visible') && level02Complete == true && level01Completed == false) {
        $('#btnSave').removeClass('hide');
    }

}
//Funcao para mostrar o Botão de Ação Corretiva Suspenso.
function btnCAShow() {
    //Se estamos no level01 ou com uma ação corretiva não podemos mostrar o botão
    if ($('.level01List').is(':visible') || $('#correctiveActionModal').is(':visible')) {
        $('.btnCA').addClass('hide');
        return false
    }

    $('.btnCA').removeClass('hide');
}
$(document).on('click', '.level02Group[level01id=3] .level02', function (e) {



    //Seleciona Level01
    var level01 = $('.level01.selected');
    var level02 = $('.level02.selected');

    var lastResult = getLastResult(level01);
    var returnLevel02 = false;
    //if (lastResult.length)
    //{
    //    var totalEvaluate = parseInt(level02.attr('totalsets'));

    //    if(lastResult.attr('completedsample') && parseInt(lastResult.attr('evaluate')) == totalEvaluate)
    //    {
    //        openMessageModal('Last set finished!', "");
    //        returnLevel02 = true;
    //    }
    //}

    if (returnLevel02 == true) {
        
        setTimeout(function (e)
        {
            level02Return();
        }, 500);
    }
    else
    {
        //Seleciona Level02

        //Inicio reaudit como falso
        var reaudit = false;
        //se meu level02 for uma reauditoria 
        if (level01.attr('startreaudit')) {
            //if (level02.attr('reaudit') == "true") {
            //altero o valor de reauditoria para true;
            reaudit = true;
        }

        //Instancio reauditNumber como 0;
        var reauditNumber = 0;
        //se a reauditoria é true
        if (reaudit == true) {
            //verifico o numero atual de reauditoria pelo nivel02
            reauditNumber = parseInt(level01.attr('reauditnumber'));
            if (reauditNumber == 0 || isNaN(reauditNumber)) {
                reauditNumber = 1;
            }
        }


        var resultsLevel01 = $('.level01Result.selected');


        //var resultsLevel01 = $('.level01Result[level01id=' + level01.attr('id') + '][date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + '][reaudit=' + reaudit + '][reauditnumber=' + reauditNumber + ']:last');
        //if (!resultsLevel01.length && !level01.attr('startreaudit')) {
        //    var resultsLevel01 = $('.level01Result[level01id=' + level01.attr('id') + '][date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + ']:last');
        //}


        var evaluateConf = parseInt($('.level02.selected').attr('totalsets'));
        var sampleConf = parseInt($('.level02.selected').attr('sidesperset'));


        var level02Saved = resultsLevel01.children('.level02Result:last');


        var evaluate = 1;
        var sampleResult = 1;
        var evaluateFinished = evaluate;


        if (resultsLevel01.length) {
            evaluate = parseInt(resultsLevel01.attr('evaluate'));
            evaluateFinished = evaluate;

            if (level02Saved.length) {
                sampleResult = parseInt(level02Saved.attr('sample'));
            }

            if (sampleResult >= sampleConf) {
                evaluate++;
                sampleResult = 1;
            }
            else {
                evaluateFinished--;
                sampleResult++;
            }
        }
        else {
            evaluateFinished = 0;
        }

        $("span.sideWithErrors").text((resultsLevel01.attr('sidewitherros')) ? resultsLevel01.attr('sidewitherros') : "0").parents('.labelPainel').removeClass('red');
        $("span.more3Defects").text((resultsLevel01.attr('more3Defects')) ? resultsLevel01.attr('more3Defects') : "0").parents('.labelPainel').removeClass('red');


        $('.setsDone').text(evaluateFinished);

        $("span.setAtual").text(evaluate);
        $("span.sideAtual").text(sampleResult);

        var sideWithErros = parseInt($("span.sideWithErrors:first").text());
        if (sideWithErros > 5) {
            $("span.sideWithErrors").parents('.labelPainel').addClass('red');
        }
        var more3Defects = parseInt($("span.more3Defects:first").text());
        if (more3Defects > 0) {
            $("span.more3Defects").parents('.labelPainel').addClass('red');
        }

        var level02 = $('.level02Group[level01id=' + level01.attr('id') + '] .level02');

        //level02.removeAttr('completed').parents('li').removeClass('bgCompleted');
        if (resultsLevel01.attr('completed') && (parseInt(resultsLevel01.attr('evaluate')) == evaluateConf) && (parseInt(level02Saved.attr('sample')) == sampleConf)) {
            level02.attr('completed', 'completed');
            level02Complete(level02);
        }
    }

});

$(document).on('click', '.level02Group[level01id=2] .level02', function (e) {

    var level01 = $('.level01.selected');
    var level02 = $('.level02.selected');
    $('.level03Group[level01id=2] .level03 input').val(0).parents('li').removeClass('bgAlert');


    //Inicio reaudit como falso
    var reaudit = false;
    //se meu level02 for uma reauditoria 
    if (level01.attr('startreaudit')) {
        //if (level02.attr('reaudit') == "true") {
        //altero o valor de reauditoria para true;
        reaudit = true;
    }

    //Instancio reauditNumber como 0;
    var reauditNumber = 0;
    //se a reauditoria é true
    if (reaudit == true) {
        //verifico o numero atual de reauditoria pelo nivel02
        reauditNumber = parseInt(level01.attr('reauditnumber'));
        if (reauditNumber == 0 || isNaN(reauditNumber)) {
            reauditNumber = 1;
        }
    }


    $('span.cattleType').text($('#selectCattleType option:selected').text());
    $('span.chainSpeed').text($('#inputChainSpeed').val());
    $('span.lotNumber').text($('#inputLotNumber').val());
    $('span.mudScore').text($('#inputMudScore').val());


    var resultsLevel01 = $('.level01Result.selected');
    //    var resultsLevel01 = $('.level01Result[level01id=' + level01.attr('id') + '][date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + '][reaudit=' + reaudit + '][reauditnumber=' + reauditNumber + ']');
    //if (!resultsLevel01.length && !level01.attr('startreaudit')) {
    //    var resultsLevel01 = $('.level01Result[level01id=' + level01.attr('id') + '][date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + ']:last');
    //}

    var level02Saved = resultsLevel01.children('.level02Result[level02id=' + level02.attr('id') + ']');

    level02Saved.children('.level03Result').each(function (e) {
        var input = $('.level03Group[level01id=2] li#' + $(this).attr('level03id') + '.level03 input');
        input.val($(this).attr('value'));
        var valor = parseInt(input.val());
        if (valor > 0) {
            input.parents('li').addClass('bgAlert');
        }
    });
});
function getPhaseNumber(level01Result, level02) {

    var level02Result = level01Result.children('.level02Result[level02id=' + level02.attr('id') + ']');


    if (level02.attr('phase')) {
        if (level02Result.length) {
            $('.labelPhase').text(level02Result.attr('phase')).parents('.labelPainel').removeClass('hide');
            $('.reauditNumberPhase').text('#' + level02Result.attr('reauditnumber')).attr('value', level02Result.attr('reauditnumber'));
        }
        else {
            $('.labelPhase').text(level02.attr('phase')).parents('.labelPainel').removeClass('hide');
            $('.reauditNumberPhase').text('#' + level02.attr('reauditnumber')).attr('value', level02.attr('reauditnumber'));
        }
    }
}
$(document).on('click', '.level02Group[level01id=1] .level02', function (e) {

    //verifico se estou em uma phase
    //se estiver em uma phase verifico qual reauditoria eu estou
    //se eu encontrar eu abro..
    //se eu nao encontrar eu nao abro nada


    //Seleciona level01.
    var level01 = $('.level01.selected');
    //Seleciona level02.
    var level02 = $('.level02.selected');

    //Informa o valor seleciona no selectbox biasedUnbiased
    $('span.biasedUnbiased').text($('select#biasedUnbiased option:selected').text());

    //Reseta opções do level03
    $('.level03Group[level01id=1] .level03 span.response').text('Yes').attr('value', '1');
    $('.level03Group[level01id=1] .level03 textarea').val('');
    $('.level03Group[level01id=1] .level03').removeClass('lightred').removeAttr('notconform');


    //Fecha os acordeons
    $('.level03Group[level01id=1]').children('div').children('.button-collapse').click();
    $('.level03Group[level01id=1] .panel-group').removeClass('hide');
    $('.level03Group[level01id=1] .level03').removeClass('hide');

    //Mostra somente os itens configurados para o level02 selecionado a partir do atributo level03show.
    if (level02.attr('level03show')) {
        $('.level03Group[level01id=1] .panel-group').addClass('hide');
        $('.level03Group[level01id=1] .level03').addClass('hide');

        var showLevel03List = level02.attr('level03show').split(';');
        for (var i = 0; i < showLevel03List.length; i++) {
            var level03 = $('.level03Group[level01id=1] #' + showLevel03List[i] + '.level03');
            level03.removeClass('hide');
            level03.parents('.panel-group').removeClass('hide');
        }
    }

    //Oculta o others
    $('.txtOthers').removeClass('hide').parents('.panel-group').removeClass('hide');

    //Oculta o span de phases 
    $('.labelPhase').text("").parents('.labelPainel').addClass('hide');
    //Oculta o numero de reauditoria
    $('.reauditNumberPhase').text("").removeAttr('reauditnumber');

    var resultsLevel01 = $('.level01Result.selected');

    getPhaseNumber(resultsLevel01, level02);


    if (resultsLevel01.length) {

        var resultLevel02 = resultsLevel01.children('.level02Result[level02id=' + level02.attr('id') + ']:last');

        //var resultLevel02 = resultsLevel01.children('.level02Result[level02id=' + level02.attr('id') + '][phase=' + $('.labelPhase').text() + '][reauditnumber=' + $('.reauditNumberPhase').attr('value') + ']:last');

        resultLevel02.children('.level03Result').each(function (e) {

            var valueResponse = $(this).attr('conform');

            var response = $('.level03Group[level01id=1] .level03[id=' + $(this).attr('level03id') + '] span.response');

            if (valueResponse == "false" && response.length) {
                response.click();
            }
            else if ($(this).attr('valuetext') != "") {

                var textArea = $('.level03Group[level01id=1] .level03[id=' + $(this).attr('level03id') + '] textarea');
                textArea.val($(this).attr('valuetext'));
                textArea.parents('.level03').attr('notconform', 'notconform');
            }

        });
        setTimeout(function (e) {
            $('.level03[notconform]').parents('.panel-group').children('.panel').children('.collapse').collapse('show');
        }, 500);

    }
});
//function btnCorrectiveAction() {
//    if ($('.level02Group:visible .level02[completed]').length == $('.level02Group:visible .level02').length) {
//        if ($('.level02Group:visible .level02[limitexceeded]').length)
//            $('.btnCA').removeClass('hide');
//    } else {
//        $('.btnCA').addClass('hide');
//    }
//}

$(document).on('click', '#btnSalvarCCA', function (e) {

    //Verifica level01 selecionado.
    var level01 = $('.level01.selected');

    //Verifica level02 selecionado.
    var level02 = $('.level02.selected');

    //Inicial a falha consecutiva do level01.
    var consectiveFailureLevel = 0;
    //Inicia a falalha consecutiva total.
    var consecFailureTotal = 0;

    //Inicio reaudit como falso.
    var reaudit = false;
    //Se meu level01 for uma reauditoria. 
    if (level01.attr('startreaudit')) {
        //Altero o valor de reauditoria para true.
        reaudit = true;
    }


    var date = $('.App').attr('date');
    var period = $('.App').attr('period');


    //Instancio reauditNumber como 0.
    var reauditNumber = 0;
    //se a reauditoria é true.
    if (reaudit == true) {
        //Verifica o numero atual de reauditoria pelo Level01.
        reauditNumber = parseInt(level01.attr('reauditnumber'));
        //Se o número de reauditoria for zero ou nulo defino como 1.
        if (reauditNumber == 0 || isNaN(reauditNumber)) {
            reauditNumber = 1;
        }
        var resultReaudit = getResultHaveReaudit(level01);

        date = resultReaudit.attr('date');
        period = resultReaudit.attr('period');
    }

    //Instancio o level01SaveCadastrado como não cadastrado.
    var level01SaveCadastrado = false;

    //Buscar o resultado do periodo.
    var level01Save = $('.level01Result[level01Id=' + level01.attr('id') + '][date=' + date + '][shift=' + $('.App').attr('shift') + '][period=' + period + '][reaudit=' + reaudit + '][reauditnumber=' + reauditNumber + ']');

    //Se existir resultado level01 atualiza a data.
    if (level01Save.length) {
        //Altero level01SaveCadastrado para true.
        level01SaveCadastrado = true;
        level01Save.attr('datetime', dateTimeFormat());
    } else {

        //Se não existir resultado gera um level01Result novo.
        level01Save = $(saveLevel01(level01.attr('id'), date, $('.App').attr('unidadeid'), $('.App').attr('shift'), period, reaudit, reauditNumber));
    }

    //Removo selected do level01Result.
    $('.level01Result').removeClass('selected');
    //Seleciona o level01Result.
    level01Save.addClass('selected');

    //Instancio o level02SaveCadastrado como não cadastrado.
    var level02SaveCadastrado = false;
    //Procura um level02Result já cadastrado
    var level02Save = level01Save.children('.level02Result[level01id=' + level01.attr('id') + '][level02id=' + level02.attr('id') + '][date=' + date + '][shift=' + $('.App').attr('shift') + '][period=' + period + '][reaudit=' + reaudit + '][reauditnumber=' + reauditNumber + ']')
    //Se existir um level02Result.
    if (level02Save.length) {
        //Altero level02SaveCadastrado para true.
        level02SaveCadastrado = false;
        //Atualiza resultado para refazer sincronização da atualização.
        level02Save.attr('datetime', dateTimeFormat())
                   .attr('auditorid', $('.App').attr('userid'))
                   .attr('defects', level02.attr('defects'))
                   .attr('consecutivefailurelevel', level02.attr('consecutivefailurelevel'))
                   .attr('consecutivefailuretotal', level02.attr('consecutivefailuretotal'))
                   .attr('sync', 'false');

        //Se um resultado avaliado foi alterado para não avaliado. 
        if (level02.attr('notavaliable')) {
            //Remove os resultados do level02 para incluir novamente com os valores padrão (0 defeitos).
            level02Save.children('.level02Result').remove();
            //Informa que o level02 é não avaliado.
            level02Save.attr('notavaliable', 'true');
        }
        else {
            //Se altera um resultado não availado para avaliado, remove a tag somente.
            level02Save.removeAttr('notavaliable');
        }
    }
    else {
        //Se não existir level02Result gera um novo.
        level02Save =
               $(saveLevel02(
                             level01.attr('id'),
                             level02.attr('id'),
                             $('.App').attr('unidadeid'),
                             date,
                             dateTimeFormat(),
                             $('.App').attr('userid'),
                             $('.App').attr('shift'),
                             period,
                             null,
                             null,
                             level02.attr('defects'),
                             reaudit,
                             reauditNumber,
                             null,
                             null,
                             $('#selectCattleType').val(),
                             $('#inputChainSpeed').val(),
                             $('#inputLotNumber').val(),
                             $('#inputMudScore').val(),
                             level02.attr('consecutivefailurelevel'),
                             level02.attr('consecutivefailuretotal'),
                             level02.attr('notavaliable')
                          ));
    }


    //Verifica itens do level03.
    $('.level03Group[level01id=2] .level03 input').each(function (e) {

        //Seleciona o level03 atual.
        var level03 = $(this).parents('.level03');

        //Instancio o level03SaveCadastrado como não cadastrado.
        var level03SaveCadastrado = false;

        //Verifica se existe level03Result.
        var level03Save = level02Save.children('.level03Result[level03id=' + level03.attr('id') + ']');

        //Define o resultado como conforme (padrão).
        var conform = true;

        //Se o valor de defeito for maior que zero o resultado é não conforme.
        if (parseInt($(this).val()) > 0) {
            conform = false;
        }
        //Se tenho level03Result atualizo.
        if (level03Save.length) {

            //Altera level03SaveCadastrado para true.
            level03SaveCadastrado = true;
            //Altera valores necessários.
            level03Save.attr('value', $(this).val())
                       .attr('conform', conform)
                       .attr('auditorId', $('.App').attr('userid'));
        }
        else {
            //Se não tem resultado level03 gera um resultado.
            var level03Save = $(saveLevel03(
                                     level03.attr('id'),
                                     $(this).val(),
                                     conform,
                                     $('.App').attr('userid'),
                                     null
                                   ));
        }

        //Adiciona os resultados no level02 se level03 não está cadastrado.
        if (level03SaveCadastrado == false) {
            appendDevice(level03Save, level02Save);
        }
    });

    //Reseta a tela do level03.
    $('.level03Group[level01id=2] .level03 input').val('0').parents('li').removeClass('bgAlert');

    //Adiciona resultados do level02 para o Level01 se o não estiver cadastrado.
    if (level02SaveCadastrado == false) {
        appendDevice(level02Save, level01Save);
    }

    //Adiciono o level01 aos resultados se o level01 não estiver cadastrado.
    if (level01SaveCadastrado == false) {
        appendDevice(level01Save, $('.Results'));
    }

    //Instancio o botão da Ação Corretiva.
    var btnCorrectiveAction = $('.btnCA');


    //Defeitos do Level02.
    var defects = parseInt($('.painelLevel03 span.defects').text());

    //Limite de defeitos do Level02.
    var defectsLimit = parseInt(level02.attr('levelerrorlimit'));

    //Se o defeito for maior que o limite gera a reauditoria e a ação corretiva no level02Result.
    if (defects > defectsLimit) {
        level02Save.attr('havereaudit', 'havereaudit').attr('havecorrectiveaction', 'havecorrectiveaction');
    }
    else {
        //Se não remove a reauditoria e ação corretiva.
        level02Save.removeAttr('havereaudit').removeAttr('havecorrectiveaction');
    }

    //Verifica limites do level02.
    defectLimitCheck();

    //Completa o Level02 Selecionado.
    level02Complete(level02);
    //Retira o parametro selected do Level02 selecionado.
    level02.removeClass('selected');
    //Gravar o arquivo no banco de dados Off Line.
    createFileResult();
    //Reseta tela de level03
    level02Level03Reset(level02.parents('.level02Group'));

    //Se tiver pelo level03 Clico no BreadCrum.
    if ($('.breadcrumb li a').length > 1) {
        $('.breadcrumb li a:last').click();
    }
    else {
        //Caso contrário executo o comando do mostrar o nivel02.
        showLevel02(level01);
    }
    //PeriodHTMLDAO.insertHTML($('.Results').html());
});


//function saveLevel01(Level01Id, date, shift, period, totalSets, totalSides, atualSet, atualSide, totalErros) {
//    return "<div class='level01Result' level01Id='" + Level01Id + "' date='" + date + "' shift='" + shift + "' period='" + period + "' totalSets='" + totalSets + "' totalSides='" + totalSide + "' atualSet='" + atualSet + "' atualSide='" + atualSide + "' totalerros='" + totalErros + "'></div>";
//}
function saveLevel01(Level01Id, date, unidadeId, shift, period, reaudit, reauditNumber
                    , totalEvaluate, sidesWithErros, more3Defects, lastEvaluate, lastSample, biasedUnbiased, evaluate) {

    if (reaudit != true) {
        reaudit = false
    }

    reauditNumber = reauditNumber ? reauditNumber : 0;
    totalEvaluate = totalEvaluate ? totalEvaluate : 0;
    sidesWithErros = sidesWithErros ? sidesWithErros : 0;
    more3Defects = more3Defects ? more3Defects : 0;
    lastEvaluate = lastEvaluate ? lastEvaluate : 0;
    lastSample = lastSample ? lastSample : 0;
    biasedUnbiased = biasedUnbiased ? biasedUnbiased : 0;
    evaluate = evaluate ? evaluate : 1;


    return "<div class='level01Result' level01Id='" + Level01Id + "' unidadeid='" + unidadeId + "' date='" + $('.App').attr('date') + "' dateTime='" + dateTimeFormat() + "' shift='" + shift + "' period='" + period + "' reaudit='" + reaudit + "' reauditNumber='" + reauditNumber + "' totalevaluate='" + totalEvaluate + "' sidewitherros='" + sidesWithErros + "' more3Defects='" + more3Defects + "' lastevaluate='" + lastEvaluate + "' lastsample='" + lastSample + "' biasedunbiased='" + biasedUnbiased + "' evaluate='" + evaluate + "' sync='false'></div>";
}
function saveLevel02(Level01Id, Level02Id, unidadeId, date, dateTime, auditorId, shift, period, evaluate, sample, defects, reaudit, reauditNumber, phase, startPhaseDate, cattleType,
                     chainSpeed, lotNumber, mudScore, consecutivefailureLevel, consecutivefailureTotal, notAvaliabled) {

    if (notAvaliabled == "notavaliable") {
        notAvaliabled = true;
    }
    else {
        notAvaliabled = false
    }

    if (reaudit != true) {
        reaudit = false
    }

    reauditNumber = reauditNumber ? reauditNumber : 0;
    phase = phase ? phase : 0;
    evaluate = evaluate ? evaluate : 1;
    sample = sample ? sample : 1;


    return "<div class='level02Result' level01Id='" + Level01Id + "' level02Id='" + Level02Id + "' unidadeId='" + unidadeId + "' date='" + date + "' dateTime='" + dateTime + "' auditorId='" + auditorId
            + "' shift='" + shift + "' period='" + period + "' defects='" + defects + "' reaudit='" + reaudit + "' evaluate='" + evaluate + "' sample='" + sample
            + "' reauditNumber='" + reauditNumber + "' phase='" + phase + "' startPhaseDate='" + startPhaseDate + "' cattletype='" + cattleType + "' chainspeed='" + chainSpeed
            + "' lotNumber='" + lotNumber + "' mudScore='" + mudScore + "' consecutivefailurelevel='" + consecutivefailureLevel + "' consecutivefailuretotal='" + consecutivefailureTotal + "' notavaliable='" + notAvaliabled + "' sync='false'></div>";
}
function saveLevel03(Level03Id, value, conform, auditorId, totalError, valuetext) {

    return "<div class='level03Result' level03id='" + Level03Id + "' date='" + dateTimeFormat() + "' value='" + value + "' conform='" + conform + "' auditorId='" + auditorId + "' totalerror='" + totalError + "' valueText='" + valuetext + "'></div>";
}
$(document).on('click', '#btnSalvarCFF', function (e) {
    //verifica level01 selecionado
    var level01 = $('.level01.selected');

    //Verifica level02Group
    var level02Group = $('.level03Group:visible');


    //Seleciona o Cabecalho
    var level02Head = $('.level02.selected');


    // Seleciona o Set Corrente
    var currentSet = parseInt($('.painelLevel03 .setAtual').text());

    //Seleciona o Side Current
    var currentSide = parseInt($('.painelLevel03 .sideAtual').text());


    //Instancia o total de sets
    var totalsets = parseInt(level02Head.attr('totalsets'));

    //Instancia o total de sides
    var totalsides = parseInt(level02Head.attr('totalsides'));

    //Instancia o total de sides por set
    var sidesperset = parseInt(level02Head.attr('sidesperset'));


    //Volta para o Level02 quando salva o level03
    var returnlevel02endset = level02Head.attr('returnlevel02endset');

    //Instancia os sets dones.
    var setsDone = parseInt($('.painelLevel02 .setsDone').text());


    level02Group.removeAttr('firstErrorSide');
    level02Group.removeAttr('Error3MoreSide');

    var date = $('.App').attr('date');
    var period = $('.App').attr('period');

    //Inicio reaudit como falso
    var reaudit = false;
    //se meu level02 for uma reauditoria 
    if (level01.attr('startreaudit')) {
        //if (level02.attr('reaudit') == "true") {
        //altero o valor de reauditoria para true;
        reaudit = true;
    }

    //Instancio reauditNumber como 0;
    var reauditNumber = 0;
    //se a reauditoria é true
    if (reaudit == true) {
        //verifico o numero atual de reauditoria pelo nivel02
        reauditNumber = parseInt(level01.attr('reauditnumber'));
        if (reauditNumber == 0 || isNaN(reauditNumber)) {
            reauditNumber = 1;
        }
        var resultReaudit = getResultHaveReaudit(level01);

        date = resultReaudit.attr('date');
        period = resultReaudit.attr('period');
    }



    //Instancio o level01SaveCadastrado como não cadastrado
    var level01SaveCadastrado = false;
    //  var level01Save = $('.level01Result[level01Id=' + level01.attr('id') + '][date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + '][evaluate=' + currentSet + '][reaudit=' + reaudit + '][reauditnumber=' + reauditNumber + ']');
    var level01Save = $('.level01Result[level01Id=' + level01.attr('id') + '][date=' + date + '][shift=' + $('.App').attr('shift') + '][period=' + period + '][evaluate=' + currentSet + '][reaudit=' + reaudit + '][reauditnumber=' + reauditNumber + ']');

    //se existir resultado level01 atualiza a data

    if (level01Save.length) {
        //Altero level01SaveCadastrado para true.
        level01SaveCadastrado = true;
        level01Save.attr('datetime', dateTimeFormat());
    }
    else {
        //se não existir resultado cria um resultado level01 novo

        level01Save = $(saveLevel01(level01.attr('id'), date, $('.App').attr('unidadeid'), $('.App').attr('shift'), period, reaudit, reauditNumber, $('.setsDone:first').text(), null, null, null, null, null, currentSet));

    }


    $('.level01Result').removeClass('selected');
    level01Save.addClass('selected');

    $('.level03Group[level01id=3] .level02').each(function (e) {

        var level02 = $(this);
        var idLevel02Result = null;
        var level02SaveCadastrado = false;
        //procura resultado do level02

        //Instancio o level02SaveCadastrado como não cadastrado.
        var level02SaveCadastrado = false;
        var level02Save = $('.level02Result[level01id=' + level01.attr('id') + '][level02id=' + level02.attr('level02id') + '][date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + '][evaluate=' + currentSet + '][sample=' + currentSide + '][reaudit=' + reaudit + '][reauditnumber=' + reauditNumber + ']:last')
        if (level02Save.length) {
            //Altero level02SaveCadastrado para true.
            level02SaveCadastrado = false;
            //atualiza resultado para refazer sincronização da atualização

            level02Save.attr('datetime', dateTimeFormat())
                       .attr('auditorid', $('.App').attr('userid'))
                       .attr('defects', $('.sideErros').text())
                       .attr('sync', 'false');

        }
        else {
            //se não existir cria um resultado novo
            level02Save =
             $(saveLevel02(
                           level01.attr('id'),
                           level02.attr('level02id'),
                           $('.App').attr('unidadeid'),
                           date,
                           dateTimeFormat(),
                           $('.App').attr('userid'),
                           $('.App').attr('shift'),
                           period,
                           currentSet,
                           currentSide,
                           $('.sideErros').text(),
                           reaudit,
                           reauditNumber
                        ));
        }


        level02.parents('.panel').children('div').children('.panel-body').children('.level03').each(function (e) {

            var level03 = $(this);
            var input = level03.children('.row').children('div').children('div').children('input');

            //Instancio o level03SaveCadastrado como não cadastrado.
            var level03SaveCadastrado = false;
            var level03Save = level02Save.children('.level03Result[level03id=' + level03.attr('id') + ']:last');

            //verificar para saber quando tem conformidade e nao conformidade

            var conform = true;
            if (parseInt($(this).val()) > 0) {
                conform = false;
            }



            if (level03Save.length) {

                //Altero level03SaveCadastrado para true;
                level03SaveCadastrado = true;
                level03Save.attr('value', input.val())
                           .attr('conform', conform)
                           .attr('auditorId', $('.App').attr('userid'));
            }
            else {
                //se não tenho resultado level03 crio o resultado
                var level03Save = $(saveLevel03(
                                               level03.attr('id'),
                                               input.val(),
                                               conform,
                                               $('.App').attr('userid'),
                                               null
                                             ));

                appendDevice(level03Save, level02Save);
            }
        });
        if (parseInt($('.sideErros:first').text()) > 0 && parseInt($('.more3Defects:first').text()) > 0 || parseInt($('.sideWithErrors:first').text()) > 5) {
            level02Save.attr('havecorrectiveaction', 'havecorrectiveaction');
        }

        appendDevice(level02Save, level01Save);

    });

    // level02Complete(level02Head);

    currentSide = currentSide + 1;

    appendDevice(level01Save, $('.Results'));


    $('.sideErros').text('0').parents('.labelPainel').removeClass('red');
    $('.level03Group:visible input').val(0).parents('.level03').removeClass('bgAlert');
    $('.painelLevel03 .sideAtual').text(currentSide);
    $(this).parents('.level03Group').children('div').children('.button-collapse').click();
    $(document).scrollTop(0);

    if (currentSide > sidesperset) {
        currentSet = currentSet + 1;
        setsDone = setsDone + 1;
        level01Save.attr('completedsample', 'completedsample');

        $('.painelLevel03 .setAtual').text(currentSet);
        $('.setsDone').text(setsDone);
        //verificar se precisa setsdones
        level01Save.attr('totalevaluate', setsDone);
        //aumentar o set

        if (level01Save.children('.level02Result[havereaudit]').length) {
            level01Save.attr('havereaudit', 'havereaudit');
        }
        else {
            //caso não tenha removo
            level01Save.removeAttr('havereaudit');
        }

        //Se o resultado do level02 no level01 tem ação corretiva adiciono ação corretiva no level01
        if (level01Save.children('.level02Result[havecorrectiveaction]').length) {
            level01Save.attr('havecorrectiveaction', 'havecorrectiveaction');
            $('.level01Result[level01id=' + level01.attr('id') + '][completedsample=completedsample][completed!=completed]').attr('havecorrectiveaction', 'havecorrectiveaction');
        }
        else {
            //caso não tenha removo
            level01Save.removeAttr('havecorrectiveaction');
        }

        var sideWithErros = parseInt($('.sideWithErrors:first').text());
        var more3Erros = parseInt($('.more3Defects:first').text());

        if (sideWithErros > 5 || more3Erros > 0)
        {
            level02Head.attr('limitExceeded', 'limitExceeded');
            level02Head.attr('havecorrectiveaction', 'havecorrectiveaction');
            
            level02Head.parents('li').addClass('bgLimitExceeded');
            level02Head.attr('havereaudit', 'havereaudit').attr('havecorrectiveaction', 'havecorrectiveaction');
            level01Save.attr('havereaudit', 'havereaudit').attr('havecorrectiveaction', 'havecorrectiveaction');
            level01Save.children('.level02Result').attr('havereaudit', 'havereaudit');
        }
        else
        {
            level02Head.removeAttr('limitExceeded');
            level02Head.removeAttr('havecorrectiveaction');
            level02Head.parents('li').removeClass('bgLimitExceeded');

        }


        //if (level02Head.attr('limitexceeded')) {
        //    level02Head.attr('havereaudit', 'havereaudit').attr('havecorrectiveaction', 'havecorrectiveaction');
        //    level01Save.attr('havereaudit', 'havereaudit').attr('havecorrectiveaction', 'havecorrectiveaction');
        //}

        $('#btnSave').addClass('hide');
        //createFileResult();
        createFileResult();
        if (setsDone == totalsets) {
            level02Head.attr('completed', 'completed');
            level02Complete(level02Head);
        }
        else {
            level02Head.removeAttr('completed', 'completed');
        }
        level02Return();
        $('.painelLevel03 .sideAtual').text("1");
        return true;
    }
    $('#btnSave').removeClass('hide');
    level01Save.attr('sidewitherros', $('span.sideWithErrors:first').text()).attr('more3Defects', $('span.more3Defects:first').text()).attr('lastevaluate', $('span.setAtual:first').text()).attr('lastsample', $('span.sideAtual:first').text());

    createFileResult();

});
$(document).on('click', '#btnSalvarHTP', function (e) {


    //Verifica level01 selecionado.
    var level01 = $('.level01.selected');

    //Verifica level02 selecionado.
    var level02 = $('.level02.selected');

    //Inicio reaudit como falso.
    var reaudit = false;
    //Se meu level01 for uma reauditoria. 
    if (level01.attr('startreaudit')) {
        //Altero o valor de reauditoria para true.
        reaudit = true;
    }
    //

    var date = $('.App').attr('date');
    var period = $('.App').attr('period');


    //Instancio reauditNumber como 0.
    var reauditNumber = 0;
    //se a reauditoria é true.
    if (reaudit == true) {
        //Verifica o numero atual de reauditoria pelo Level01.
        reauditNumber = parseInt(level01.attr('reauditnumber'));
        //Se o número de reauditoria for zero ou nulo defino como 1.
        if (reauditNumber == 0 || isNaN(reauditNumber)) {
            reauditNumber = 1;
        }
        var resultReaudit = getResultHaveReaudit(level01);

        date = resultReaudit.attr('date');
        period = resultReaudit.attr('period');

    }

    //Instancio o level01SaveCadastrado como não cadastrado.
    var level01SaveCadastrado = false;

    //Buscar o resultado do periodo.
    var level01Save = $('.level01Result[level01Id=' + level01.attr('id') + '][date=' + date + '][shift=' + $('.App').attr('shift') + '][period=' + period + '][reaudit=' + reaudit + '][reauditnumber=' + reauditNumber + ']');


    if (level01Save.length) {
        level01Save.attr('datetime', dateTimeFormat());
    } else {
        level01Save = $(saveLevel01(level01.attr('id'), $('.App').attr('date'), $('.App').attr('unidadeid'), $('.App').attr('shift'), $('.App').attr('period'), reaudit, reauditNumber, null, null, null, null, null, $('#biasedUnbiased option:selected').val()));
    }

    $('.level01Result').removeClass('selected');
    level01Save.addClass('selected');
    var idLevel02Result = null;

    var phase = 0;
    var startPhase = null;
    var atualPhase = parseInt(level02.attr('phase'));
    if(isNaN(atualPhase))
    {
        phase = 0;
    }
    else
    {
        phase = atualPhase;
    }
    if (level01.attr('havephases')) {
        getPhases(level01);
        getPhaseNumber(level01Save, level02);
    }

    if (phase > 0) {
        startPhase = dateTimeFormat();
        reauditNumber = parseInt($('.reauditNumberPhase').attr('value'));
    }
    //var reauditNumber = 0;
    //if ($('.labelPhase').text()) {
    //    phase = parseInt($('.labelPhase').text());
    //    reauditNumber = parseInt($('.reauditNumberPhase').attr('value'));
    //}

    // var level02Save = $('.level02Result[level01id=' + level01.attr('id') + '][level02id=' + level02.attr('id') + '][date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + ']' + attrReaudit + attrReauditNumber + ':last')
    var level02Save = level01Save.children('.level02Result[level02id=' + level02.attr('id') + '][date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + '][phase=' + phase + '][reauditnumber=' + reauditNumber + ']:last')

    if (level01.attr('havephases') && !level02.attr('phase')) {
        level02.attr('phase', '0');
    }

   
    //var reaudit = level02.attr('reaudit');
    //var reauditNumber = 0;
    //if (level02.attr('reaudit')) {
    //    reauditNumber = reauditCount(level02);
    //}

    //if (level02.attr('phase')) {
    //    reauditNumber = level02.attr('reauditnumber');
    //}

    if (level02Save.length) {
        level02Save.attr('datetime', dateTimeFormat())
                   .attr('auditorid', $('.App').attr('userid'))
                   .attr('defects', level02.attr('defects'))
                   .attr('notavaliable', level02.attr('notavaliable'))
                   .attr('sync', false)
                   .attr('startphasedate', startPhase);

        //Se um resultado avaliado foi alterado para não avaliado. 
        if (level02.attr('notavaliable')) {
            //Remove os resultados do level02 para incluir novamente com os valores padrão (0 defeitos).
            level02Save.children('.level02Result').remove();
            //Informa que o level02 é não avaliado.
            level02Save.attr('notavaliable', 'true');
        }
        else {
            //Se altera um resultado não availado para avaliado, remove a tag somente.
            level02Save.removeAttr('notavaliable');
        }
        //idLevel02Result = level02Save.attr('id');
        //level02Save.remove();
    }
    else {
        level02Save =
                $(saveLevel02(
                              //idLevel02Result, 
                              level01.attr('id'),
                              level02.attr('id'),
                              $('.App').attr('unidadeid'),
                              dateReturn(),
                              dateTimeFormat(),
                              $('.App').attr('userid'),
                              $('.App').attr('shift'),
                              $('.App').attr('period'),
                              null,
                              null,
                              level02.attr('defects'),
                              reaudit,
                              reauditNumber,
                              level02.attr('phase'),
                              startPhase,
                              $('#selectCattleType').val(),
                              $('#inputChainSpeed').val(),
                              $('#inputLotNumber').val(),
                              $('#inputMudScore').val(),
                              level02.attr('consecutivefailurelevel'),
                              level02.attr('consecutivefailuretotal'),
                              level02.attr('notavaliable')
                           ));
    }

    var defects = 0;

    $('.level03Group[level01id=1] .level03 span.response').each(function (e) {

        var level03 = $(this).parents('.level03');

        var level03show = level02.attr('level03show').split(';');

        var incluirIntem = false;
        for (var i = 0; i < level03show.length; i++) {
            if (level03show[i] == level03.attr('id')) {
                incluirIntem = true;
                break;
            }
        }

        if (incluirIntem != true) {
            return;
        }

        var level03Save = level02Save.children('.level03Result[level03id=' + level03.attr('id') + ']');

        //precisa verificar onde está setando zero!!
        if ($('.level02Group').is(':visible') || $('.level01List').is(':visible')) {
            $(this).attr('value', '1');

        }

        var conform = true;
        if (parseInt($(this).attr('value')) == 0) {
            conform = false;
            defects++;

        }


        if (level03Save.length) {
            level03Save.attr('value', $(this).val())
                       .attr('conform', conform)
                       .attr('auditorId', $('.App').attr('userid'));
        }
        else {
            var level03Save = $(saveLevel03(
                                           level03.attr('id'),
                                           $(this).val(),
                                           conform,
                                           $('.App').attr('userid'),
                                           null
                                         ));
        }

        appendDevice(level03Save, level02Save);
        // level02Save.append(level03Save);
    });

    //var defectsInOthers = false;

    $('.level03Group[level01id=1] .level03 textarea').each(function (e) {

        var level03 = $(this).parents('.level03');

        var level03Save = level02Save.children('.level03Result[level03id=' + level03.attr('id') + ']');



        //precisa verificar onde está setando zero!!
        if ($('.level02Group').is(':visible') || $('.level01List').is(':visible')) {
            //$(this).attr('value', '');
            $(this).val("");
        }

        var conform = true;
        if ($(this).val().length > 0) {
            conform = false;
            defects++;
            //defectsInOthers = true;
        }

        if (level03Save.length) {

            level03Save.attr('valueText', $(this).val())
                       .attr('conform', conform)
                       .attr('auditorId', $('.App').attr('userid'));
        }
        else {
            var level03Save = $(saveLevel03(
                                           level03.attr('id'),
                                           null,
                                           conform,
                                           $('.App').attr('userid'),
                                           null,
                                           $(this).val()
                                         ));
        }

        appendDevice(level03Save, level02Save);
        // level02Save.append(level03Save);
    });

    appendDevice(level02Save, level01Save);
    appendDevice(level01Save, $('.Results'));


    level02Save.attr('defects', defects);
    //pegar da configuração o total de limite
    if (defects > 0) {


        level02.attr('correctivaction', 'correctivaction').attr('reaudit', 'reaudit').attr('reauditNumber', reauditNumber);
        //level02.parents('.row').children('.userInfo').children('div').children('.btnReaudit').removeClass('hide').siblings('.reauditCount').removeClass('hide');
        level02Save.attr('havecorrectiveaction', 'havecorrectiveaction');
        level02Save.attr('havereaudit', 'havereaudit')

    }
    else {
        level02.removeAttr('correctivaction').removeAttr('reaudit').removeAttr('reauditNumber').removeAttr('havecorrectiveaction');
        level02Save.removeAttr('havecorrectiveaction').removeAttr('havereaudit');

    }

    var phaseConfiguation = $('.phasesreaudits .phase[number=' + phase + ']');

    var phase = parseInt(level02Save.attr('phase'));
    var phaseNumber = parseInt(level02Save.attr('reauditnumber'));

    if (level01.attr('havephases')) {
        if (level02Save.attr('reaudit') == "false" && !level02Save.attr('havereaudit') && phase > 1) {
            var phaseConfiguation = $('.phasesreaudits .phase[number=' + phase + ']');

            var phaseNumbersConf = phaseConfiguation.attr('reaudits');

            if (phaseNumber >= phaseNumbersConf) {
                openMessageModal('Phase ' + phase + ' Completed', level02.children('.levelName').text() + " returned to phase 0");

            }

        }
    }

    createFileResult();

    level02Complete(level02);
    //Se tiver pelo level03 Clico no BreadCrum.
    if ($('.breadcrumb li a').length > 1) {
        $('.breadcrumb li a:last').click();
    }
    else {
        //Caso contrário executo o comando do mostrar o nivel02.
        showLevel02(level01);
    }
    $(this).parents('.level03Group').children('div').children('.button-collapse').click();

});


function resetDay() {
    $('.overlay').hide();
    openMessageModal("New collection", "This collection didn't start today, it will be recorded normally. Please, login again.");

    resetApplication();

    $('#btnLogout').click();

    //$('.App').attr('date', dateReturn());
    //resetApplication();
    ////Reseto o Period para as configurações a partir dos resultados sincronizados
    //periodReset();
    //$('.breadcrumb a.main').click();
  
}


$(document).on('mousedown', '#btnSave', function (e) {
    var that = $(this);

    if ($('.App').attr('logintime').substring(0, 10).replace("/", "").replace("/", "") == dateReturn()) {
        that.addClass('disabled');

        setTimeout(function () {
            $('.level02Group:visible .level02Confirm').click();

            $('.level03Group:visible .level03Confirm').click();
            that.removeClass('disabled');
        }, 250);
    } else {
        resetDay();
    }
});

$(document).on('change', 'select#selectPeriod', function (e) {

    resetApplication();
    //Reseto o Period para as configurações a partir dos resultados sincronizados
    periodReset();
});
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

//Pega ultimo Resultado atual da data, shift, period que não tem reauditoria
function getAtualResult(level01) {
    var atualResult = $('.level01Result[level01id=' + level01.attr('id') + '][date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + '][reaudit=false]:last');
    return atualResult;
}
//Pega ultimo resultado do shift
function getLastResult(level01) {
    var lastResult = $('.level01Result[level01id=' + level01.attr('id') + '][shift=' + $('.App').attr('shift') + ']:last');
    return lastResult;
}

//Pega Ultimo Resultado do Periodo
function getLastResultPeriod(level01) {
    var atualResult = $('.level01Result[level01id=' + level01.attr('id') + '][date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + ']:last');
    return atualResult;
}
//Pega Resultado do Shift que tem reauditoria
function getResultHaveReaudit(level01) {
    var lastResultReaudit = $('.level01Result[level01id=' + level01.attr('id') + '][shift=' + $('.App').attr('shift') + '][havereaudit]:last');
    return lastResultReaudit;
}
//Pega ultima reauditoria do shift não completa
function getLastReauditPeriodNotCompleted(level01) {
    var lastResultReauditNotCompleted = $('.level01Result[level01id=' + level01.attr('id') + '][shift=' + $('.App').attr('shift') + '][reaudit=true][completed!=completed]:last');
    return lastResultReauditNotCompleted;
}
//Pega ultima Reaudotoria do period
function getLastReauditResultPeriod(level01, period) {
    var lastResultReauditNotCompleted = $('.level01Result[level01id=' + level01.attr('id') + '][shift=' + $('.App').attr('shift') + '][period=' + period + '][date=' + $('.App').attr('date') + '][reaudit=true]:last');
    return lastResultReauditNotCompleted;
}

function periodReset() {
    //Zero o total de defeitos geral
    var totalDefects = 0;

    $('.totalDefects').text('0');

    //percorro os Level01   
    $('.level01List .level01').each(function (e) {

        //seleciono o level01 que estou verificando
        var level01 = $(this);

        //Verifico se já existe resultado para o periodo.
        var atualResult = getAtualResult(level01);
        if (!atualResult.length && !level01.attr('havephases')) {
            atualResult = getLastReauditResultPeriod(level01, $('#selectPeriod').val());
        }
        //Verifico o ultimo resultado gerado.
        var lastResult = getLastResult(level01);


        //se não existe resultado para o periodo, verifico o ultimo resultado
        //if(!atualResult.length)
        //{
        //    atualResult = $('.level01Result[level01id=' + level01.attr('id') + '][date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + ']:last');
        //}

        //Instancio a configuração total de evalute
        var evaluateConf = 1;
        if (level01.attr('haveevaluates')) {
            evaluateConf = parseInt($('.level02Group[level01id=' + level01.attr('id') + '] .level02:first').attr('totalsets'));
        }
        //Instancio a avaliação com valor zero
        var evaluate = 0;

        //Se existir resultado para o periodo, data e shit atual
        if (atualResult.length
            && atualResult.attr('date') == $('.App').attr('date')
            && atualResult.attr('shift') == $('.App').attr('shift')
            && atualResult.attr('period') == $('.App').attr('period')) {
            //Atribui o resultado da ultima avaliação a variavel evaluate
            evaluate = parseInt(atualResult.attr('evaluate'));
        }
        else {
            evaluate = 1;
        }

        //verifico se tenho ação corretiva para algum periodo
        var corretivActionsNoCompleted = $('.level01Result[level01id=' + level01.attr('id') + '][date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][havecorrectiveaction]:last');

        //verifico se eu tenho reauditoria para o periodo e data atual
        var reauditNotComplete = $('.level01Result[level01id=' + level01.attr('id') + '][date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][havereaudit]:last');
        if (!reauditNotComplete.length)
        {
            //verifica se tem reauditoria para qualquer period anterior
            reauditNotComplete = $('.level01Result[level01id=' + level01.attr('id') + '][shift=' + $('.App').attr('shift') + '][havereaudit]:last');
        }

        //Instancio [atualResultPeriod] para verificar se o resutlado atual é do periodo seelcionado
        var atualResultPeriod = false;
        if (lastResult.attr('date') == $('.App').attr('date') &&
            lastResult.attr('shift') == $('.App').attr('shift') &&
            lastResult.attr('period') == $('.App').attr('period')) {
            atualResultPeriod = true;
        }

        //Instancio [level01Complete] para verificar o resultado está completo
        var level01Complete = false;
        if (atualResult.length && evaluate >= evaluateConf && atualResult.attr('completed') && atualResultPeriod == true) {
            level01Complete = true;
        }

        //Instancio [correctiveActionInPeriod] para verificar se existe uma ação corretiva
        var correctiveActionInPeriod = false;
        if (corretivActionsNoCompleted.attr('date') == atualResult.attr('date') &&
           corretivActionsNoCompleted.attr('shift') == atualResult.attr('shift') &&
            corretivActionsNoCompleted.attr('period') == atualResult.attr('period')) {
            correctiveActionInPeriod = true;
        }

        //Se tiver ação corretiva e não estiver completado meu level02 mostra o botão de acção corretiva
        if (corretivActionsNoCompleted.length && corretivActionsNoCompleted.attr('completed') && ((correctiveActionInPeriod == true && !atualResult.attr('correctiveactioncomplete')) || !atualResult.length || corretivActionsNoCompleted.attr('havecorrectiveaction'))) {
            //   if (corretivActionsNoCompleted.length && (level01Complete == true || !atualResult.length)) {
            level01.attr('correctivaction', 'correctivaction').parents('.row').children('.userInfo').children('.btnCorrectiveAction').removeClass('hide');
        }

       

        //se tenho um resultado que tem reauditoria nao completa e esse resultado já estiver completo
        if (reauditNotComplete.length && reauditNotComplete.attr('completed')) {

            //Instancio as datas do App e a data da reauditoria
            var dateAPP = $('.App').attr('date');
            var dateReaudit = reauditNotComplete.attr('date');


            //Convertemos as datas do App e da Reauditoria
            var dateAppDate = new Date(dateAPP.substring(4, 8), parseInt(dateAPP.substring(0, 2)) - 1, dateAPP.substring(2, 4)).setHours(0, 0, 0, 0, 0);
            var dateReauditDate = new Date(dateReaudit.substring(4, 8), parseInt(dateReaudit.substring(0, 2)) - 1, dateReaudit.substring(2, 4)).setHours(0, 0, 0, 0, 0);

            
            
            if (dateAppDate >= dateReauditDate) {

                //Verificamos os periodos do App e da Reauditoria
                var periodAPP = $('.App').attr('period');
                var periodReaudit = reauditNotComplete.attr('period');


                //Se a data da auditoria igual a do App e o periodo for igual ou maior ou a Data do App for maior que a data da reauditoria
                //Posso fazer a reauditoria em qualquer  periodo se a data for maior como está hoje
                if ((dateAppDate == dateReauditDate && periodAPP >= periodReaudit) || (dateAppDate > dateReauditDate)) {

                    //Instanciamos o botão de reauditoria
                    var btnReaudit = level01.parents('.row').children('.userInfo').children('div').children('.btnReaudit');
                    //var reauditCount = level01.parents('.row').children('.userInfo').children('div').children('.reauditCount');

                    //Instancio a reauditResult ao resultado que gerou auditoria
                    var readitResult = reauditNotComplete;

                    //Se meu ultimo Resultado for uma reauditoria
                    if (lastResult.attr('reaudit') == "true") {
                        //verifico a configuração de avaliações
                        var evalueteConf = 5;
                        //pega a avaliação da ultima reauditoria
                        var evaluate = parseInt(lastResult.attr('evaluate'));

                        //se tenho avaliações 
                        if (level01.attr('haveevaluates')) {
                            //se terminei as avaliações e a ultima está completo
                            if (evaluate >= evaluateConf && readitResult.attr('completed')) {
                                //altero o resultado da ultima reauditoria com valor dela
                                readitResult.attr('totalreaudits', lastResult.attr('reauditnumber'));
                            }
                        }
                    }
                    //Se o ultimo resultado for uma reauditoria e o ultimo resultado não estiver completo
                    if (lastResult.attr('reaudit') == "true" && !lastResult.attr('completed')) {
                        //Acesso a reauditoria para finalziar ela
                        level01.removeAttr('completed').attr('startreaudit', 'startreaudit');
                    }
                    //se for completa verifica se o numero é igual o total de reauditoris se nao for soma -1
                    //se a reautoria for igual ao totla de reautoriaas que tem que fazer libera a autoria
                    btnReaudit.children('.reauditPeriod').text($('#selectPeriod option[value=' + readitResult.attr('period') + ']').text());
                    //reauditCount.attr('count', readitResult.attr('reauditnumber'));
                    //reauditCount.children('button').text(readitResult.attr('reauditnumber') + "/" + level01.attr('minreauditnumber'));
                    //  btnReaudit.children('.reauditPeriod').text('aaaaaaaaaaaaaaaaaaaaaaa');
                    level01.attr('reaudit', 'reaudit').attr('reauditnumber', readitResult.attr('reauditnumber')).parents('.row').children('.userInfo').children('.btnReaudit').removeClass('hide');

                }
            }


        }
        //Se exister resultado atual e o level01 não for uma reauditoria
        if (atualResult.length && !level01.attr('startreaudit')) {

            //Se 
            //O resultado atual estiver completo e a avaliação for maior ou igual ao numero de avaliação configurado 
            //E a data do resultado for igual a data do App
            if (atualResult.attr('completed') && evaluate >= evaluateConf && atualResult.attr('date') == $('.App').attr('date')) {
                //O resultado do period está completo
                level01.attr('completed', 'completed');
            }

        }

    });
    //Configuro o level01 com o resultado
    configureLevel01();

}

$(document).on('change', 'select#selectCattleType', function (e) {
    $('span.cattleType').html($("select#selectCattleType :selected").text());
});
$(document).on('change', 'input[type=number]', function (e) {
    $(this).attr('oldvalue', $(this).val());
});
$(document).on('input', 'input[type=number]', function (e) {
    if (this.value.length > 0 && isNaN(parseInt(this.value)))
    {
        $(this).val($(this).attr('oldvalue'));
    }
    $(this).val(parseInt($(this).val()));
});
$(document).on('input', 'input#inputChainSpeed', function (e) {
    $('span.chainSpeed').html($(this).val());
});
$(document).on('input', 'input#inputLotNumber', function (e) {
    $('span.lotNumber').html($(this).val());
});
$(document).on('input', 'input#inputMudScore', function (e) {
    $('span.mudScore').html($(this).val());
});
$(document).on('change', 'select#biasedUnbiased', function (e) {
    $('span.biasedUnbiased').html($("select#biasedUnbiased :selected").text());
});



function reauditCount(level) {
    var reauditNumber = 0;
    if (level.attr('reauditNumber')) {
        reauditNumber = parseInt(level.attr('reauditNumber'));
    }
    if (level.attr('startreaudit')) {
        reauditNumber++;
    }
    return reauditNumber;
}
$(document).on('click', '#btnSalvarLevel02CCA', function (e) {

    //Verifica o level01 Selecionado.
    var level01 = $('.level01.selected');

    //Pega o ultimo resultado (resultado que esta sendo auditado).
    var level01Result = $('.level01Result.selected');

    //Se não encontrar resultado existe algum problema.
    if (!level01Result.length) {
        openMessageModal("Result not found");
        return true;
    }

    //Instancia reaudit como falso
    var reaudit = false;
    //Se o level01Result for uma reauditoria. 
    if (level01Result.attr('reaudit') == "true") {
        //altero o valor de reauditoria para true.
        reaudit = true;
    }

    //Instancio reauditNumber como 0.
    var reauditNumber = 0;
    //Se a reauditoria é true.
    if (reaudit == true) {
        //Verifica o número da reauditoria pelo resultado.
        reauditNumber = parseInt(level01Result.attr('reauditnumber'));
        //Se o número da reauditoria for zero ou nulo a reauditoria será 1.
        if (reauditNumber == 0 || isNaN(reauditNumber)) {
            reauditNumber = 1;
        }
    }

    //Se todos os itens estão completos no Level02 eu atualizo o level01 para completed.
    if ($('.level02Group[level01id=' + level01.attr('id') + ']').children('li').children('.row').children('.level02[completed!=completed]').length == 0) {

        level01.attr('completed', 'completed');
    }
    else {
        //Se não estiverem, aviso que a reaudotiria deve estar completa.
        openMessageModal('Complete current audit to save ' + $('.level01.selected').children('.name').text());
        return false;
    }

    //Se for uma reautoria preencho os campos NA com o valor informado nos inputs após ter começado a reauditoria.
    if (level01Result.attr('reaudit') == "true") {
        level01Result.children('.level02Result[chainspeed=""]').attr('chainspeed', $('#inputChainSpeed').val());
        level01Result.children('.level02Result[lotnumber=""]').attr('lotnumber', $('#inputLotNumber').val());
        level01Result.children('.level02Result[mudscore=""]').attr('mudscore', $('#inputMudScore').val());
        level01Result.children('.level02Result[cattletype=""]').attr('cattletype', $('#selectCattleType').val());
        level01Result.children('.level02Result[cattletype="0"]').attr('cattletype', $('#selectCattleType').val());

    }

    //Instancio os defeitos totals da auditoria.
    var totalDefects = parseInt($('.painelLevel02 .totalDefects').text());

    //Instancio que falha consecutivas são falsas.
    var haveConsecutiveFailures = false


    if (totalDefects > 22 || $('.level02Group[level01id=' + level01.attr('id') + '] .level02[havereaudit]').length) {
        reauditNumber = 0;
        level01.attr('correctivaction', 'correctivaction').attr('reaudit', 'reaudit').attr('reauditNumber', reauditNumber);

        //level01.parents('.row').children('.userInfo').children('div').children('.reauditCount').children('button').text(reauditNumber + '/' + level01.attr('minreauditnumber'));

        level01Result.attr('havecorrectiveaction', 'havecorrectiveaction');
        if (!level01Result.attr('reaudit') != true) {
            level01Result.attr('havereaudit', 'havereaudit');
            level01.parents('.row').children('.userInfo').children('div').children('.btnReaudit').addClass('hide').siblings('.reauditCount').addClass('hide');
        }
    }

    //Defino o Resultado do level01 como completed.
    level01Result.attr('completed', 'completed').attr('totaldefects', totalDefects);


    //Se o level01 está como reauditoria e meu resultado é uma reauditoria.
    if (level01.attr('startreaudit') || level01Result.attr('reaudit') == "true") {


        //Adiciono os atributos para poder visualizar a reauditoria correta no level01.
        level01.attr('reaudit', 'reaudit').attr('reauditNumber', reauditNumber).removeAttr('startreaudit');

        //Seleciona meu resultado que gerou a reauditoria.
        var resultStarReaudit = getResultHaveReaudit(level01);

        //Informo a quantidade de reauditorias.
        resultStarReaudit.attr('totalreaudits', reauditNumber);

        //Verifico o número mínimo de reauditorias.
        var minreauditNumber = parseInt(level01.attr('minreauditnumber'));

        //Se o número da reauditoria atual for maior que o número minimo de reauditorias.
        if (reauditNumber >= minreauditNumber) {
            //removo a tag reaudit e reauditnumber do level01.
            level01.removeAttr('reaudit').removeAttr('reauditnumber');

            //Adiciono as tags no level01Result que gerou a reauditoria para saber que a reauditoria foi completada.
            resultStarReaudit.removeAttr('havereaudit').attr('completereaudit', 'completereaudit').children('.level02Result[havereaudit]').removeAttr('havereaudit').attr('completereaudit', 'completereaudit').attr('sync', 'false');
            //Busco a div que tem os botoes do level01.
            var level01Buttons = level01.siblings('.userInfo').children('div');

            //Zero  o periodo da reauditoria.
            level01Buttons.children('.btnReaudit').addClass('hide').children('.reauditPeriod').text('');

            //Oculto o contador.
            level01Buttons.children('.reauditCount').addClass('hide');

            //if (resultStarReaudit.attr('period') != $('.App').attr('period')) {
            //    var resultAtualLevel = $('.level01Result[level01id=' + level01.attr('id') + '][date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + '][reaudit=false]');
            //    if (resultAtualLevel.length == 0) {
            //        level01.attr('reauditnumber', '0').removeAttr('reaudit').removeAttr('reauditnumber').removeAttr('startreaudit').removeAttr('completed');

            //    }
            //}

            //Informa ao usuário que terminou a reauditoria
            openMessageModal('Re-audit completed', $('#selectPeriod option[value=' + resultStarReaudit.attr('period') + ']').text() + ' re-audit completed!');

        }
    }

    //Se o level01Result tem uma ação corretiva, atribuo ação corretiva no level01 e no resultado
    if (level01Result.children('.level02Result[havecorrectiveaction]').length) {

        level01.attr('correctivaction', 'correctivaction')
        level01Result.attr('havecorrectiveaction', 'havecorrectiveaction');
    }
    //Se o level01Result tem uma reauditoria, atribuo a ação corretiva no level01 e no resultado
    if (level01Result.children('.level02Result[havereaudit]').length) {
        level01.attr('reaudit', 'reaudit');
        level01Result.attr('havereaudit', 'havereaudit');
    }

    //Salva no Banco dados Off Line.
    createFileResult();
    //Retorna ao menu do Level01.

    //Reseto o periodo
    level01Reset(level01);
    periodReset();

    level01Return();



});
$(document).on('click', '#btnSalvarLevel02CFF', function (e) {

    //Verifica o level01 Selecionado
    var level01 = $('.level01.selected');


    var level01Result = $('.level01Result.selected');
    if (!level01Result.length) {
        openMessageModal("Result not found");
        return true;
    }


    //Inicio reaudit como falso
    var reaudit = false;
    //se meu level02 for uma reauditoria 
    if (level01.attr('startreaudit')) {
        //if (level02.attr('reaudit') == "true") {
        //altero o valor de reauditoria para true;
        reaudit = true;
    }



    //Instancio reauditNumber como 0;
    var reauditNumber = 0;
    //se a reauditoria é true
    if (reaudit == true) {
        //verifico o numero atual de reauditoria pelo nivel02
        reauditNumber = parseInt(level01Result.attr('reauditnumber'));
        if (reauditNumber == 0) {
            reauditNumber = 1;
        }
    }


    var evaluateConf = parseInt($('.level02Group[level01id=' + level01.attr('id') + '] .level02:first').attr('totalsets'));
    var evaluate = parseInt(level01Result.attr('evaluate'));


    //Se todos os itens estão completos no Level02 eu atualizo o level01 para completed
    if (evaluate >= evaluateConf && level01Result.attr('completedsample')) {
        level01.attr('completed', 'completed');

    }
    else {
        //se não estiver envio uma mensagem
        openMessageModal('Complete current audit to save ' + $('.level01.selected').children('.name').text());
        return false;
    }



    //Defino o Resultado do nivel 01 como completed
    var levels01Result = $('.level01Result[level01id=' + level01.attr('id') + '][date=' + level01Result.attr('date') + '][shift=' + level01Result.attr('shift') + '][period=' + level01Result.attr('period') + '][reaudit=' + level01Result.attr('reaudit') + '][reauditnumber=' + level01Result.attr('reauditnumber') + ']');

    levels01Result.attr('completed', 'completed');

    //Se o level01Result tem uma ação corretiva, atribuo ação corretiva no level01
    if (levels01Result.attr('havecorrectiveaction')) {

        level01.attr('correctivaction', 'correctivaction')
    }


    //Se o level01Result tem uma reauditoria, atribuo a ação corretiva no level01
    if (levels01Result.attr('havereaudit')) {
        level01.attr('reaudit', 'reaudit');
    }


    //Se o level01 está como reauditoria e meu resultado é uma reauditoria.
    if (level01.attr('startreaudit') || level01Result.attr('reaudit') == "true") {


        //Adiciono os atributos para poder visualizar a reauditoria correta no level01.
        level01.attr('reaudit', 'reaudit').attr('reauditNumber', reauditNumber).removeAttr('startreaudit');

        //Seleciona meu resultado que gerou a reauditoria.
        var resultStarReaudit = $('.level01Result[level01id=' + level01.attr('id') + '][shift=' + $('.App').attr('shift') + '][reauditnumber!=' + reauditNumber + '][havereaudit]');

        //Informo a quantidade de reauditorias.
        resultStarReaudit.attr('totalreaudits', reauditNumber);

        //Verifico o número mínimo de reauditorias.
        var minreauditNumber = parseInt(level01.attr('minreauditnumber'));

        //Se o número da reauditoria atual for maior que o número minimo de reauditorias.
        if (reauditNumber >= minreauditNumber) {
            //removo a tag reaudit e reauditnumber do level01.
            level01.removeAttr('reaudit').removeAttr('reauditnumber');

            //Adiciono as tags no level01Result que gerou a reauditoria para saber que a reauditoria foi completada.
            resultStarReaudit.removeAttr('havereaudit').attr('completereaudit', 'completereaudit').children('.level02Result[havereaudit]').removeAttr('havereaudit').attr('completereaudit', 'completereaudit').attr('sync', 'false');
            //Busco a div que tem os botoes do level01.
            var level01Buttons = level01.siblings('.userInfo').children('div');

            //Zero  o periodo da reauditoria.
            level01Buttons.children('.btnReaudit').addClass('hide').children('.reauditPeriod').text('');

            //Oculto o contador.
            level01Buttons.children('.reauditCount').addClass('hide');

            //if (resultStarReaudit.attr('period') != $('.App').attr('period')) {
            //    var resultAtualLevel = $('.level01Result[level01id=' + level01.attr('id') + '][date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + '][reaudit=false]');
            //    if (resultAtualLevel.length == 0) {
            //        level01.attr('reauditnumber', '0').removeAttr('reaudit').removeAttr('reauditnumber').removeAttr('startreaudit').removeAttr('completed');

            //    }
            //}

            //Informa ao usuário que terminou a reauditoria
            openMessageModal('Re-audit completed', $('#selectPeriod option[value=' + resultStarReaudit.attr('period') + ']').text() + ' re-audit completed!');

        }
    }



    createFileResult();

    //Reseto o periodo
    level01Reset(level01);
    periodReset();


    level01Return();


});
$(document).on('click', '#btnSalvarLevel02HTP', function (e) {



    //Verifica o level01 Selecionado.
    var level01 = $('.level01.selected');

    //Pega o ultimo resultado (resultado que esta sendo auditado).
    var level01Result = $('.level01Result.selected');

    //Se não encontrar resultado existe algum problema.
    if (!level01Result.length) {
        openMessageModal("Result not found");
        return true;
    }

    //Instancia reaudit como falso
    var reaudit = false;
    //Se o level01Result for uma reauditoria. 
    if (level01Result.attr('reaudit') == "true") {
        //altero o valor de reauditoria para true.
        reaudit = true;
    }

    //Instancio reauditNumber como 0.
    var reauditNumber = 0;
    //Se a reauditoria é true.
    if (reaudit == true) {
        //Verifica o número da reauditoria pelo resultado.
        reauditNumber = parseInt(level01Result.attr('reauditnumber'));
        //Se o número da reauditoria for zero ou nulo a reauditoria será 1.
        if (reauditNumber == 0 || isNaN(reauditNumber)) {
            reauditNumber = 1;
        }
    }



    //Se todos os itens estão completos no Level02 eu atualizo o level01 para completed.
    if ($('.level02Group[level01id=' + level01.attr('id') + ']').children('li').children('.row').children('.level02[completed!=completed]').length == 0) {

        level01.attr('completed', 'completed');
    }
    else {
        //Se não estiverem, aviso que a reaudotiria deve estar completa.
        openMessageModal('Complete current audit to save ' + $('.level01.selected').children('.name').text());
        return false;
    }

    //Se for uma reautoria preencho os campos NA com o valor informado nos inputs após ter começado a reauditoria.
    //if (level01Result.attr('reaudit') == "true") {
    //    level01Result.children('.level02Result[biasedUnbiased=""]').attr('biasedUnbiased', $('#biasedUnbiased').val());

    //}
    //Se for uma reautoria preencho os campos NA com o valor informado nos inputs após ter começado a reauditoria.
    if (level01Result.attr('reaudit') == "true") {

        level01Result.attr('biasedunbiased', $('#biasedUnbiased').val());

    }

    //Defino o Resultado do level01 como completed.
    level01Result.attr('completed', 'completed');

    //Se o level01 está como reauditoria e meu resultado é uma reauditoria.
    if (level01.attr('startreaudit') || level01Result.attr('reaudit') == "true") {


        //Adiciono os atributos para poder visualizar a reauditoria correta no level01.
        level01.attr('reaudit', 'reaudit').attr('reauditNumber', reauditNumber).removeAttr('startreaudit');

        //Seleciona meu resultado que gerou a reauditoria.
        var resultStarReaudit = getResultHaveReaudit(level01);

        //Informo a quantidade de reauditorias.
        resultStarReaudit.attr('totalreaudits', reauditNumber);

        //Verifico o número mínimo de reauditorias.
        var minreauditNumber = parseInt(level01.attr('minreauditnumber'));

        //Se o número da reauditoria atual for maior que o número minimo de reauditorias.
        if (reauditNumber >= minreauditNumber) {
            //removo a tag reaudit e reauditnumber do level01.
            level01.removeAttr('reaudit').removeAttr('reauditnumber');

            //Adiciono as tags no level01Result que gerou a reauditoria para saber que a reauditoria foi completada.
            resultStarReaudit.removeAttr('havereaudit').attr('completereaudit', 'completereaudit').children('.level02Result[havereaudit]').removeAttr('havereaudit').attr('completereaudit', 'completereaudit').attr('sync', 'false');
            //Busco a div que tem os botoes do level01.
            var level01Buttons = level01.siblings('.userInfo').children('div');

            //Zero  o periodo da reauditoria.
            level01Buttons.children('.btnReaudit').addClass('hide').children('.reauditPeriod').text('');

            //Oculto o contador.
            level01Buttons.children('.reauditCount').addClass('hide');

            //if (resultStarReaudit.attr('period') != $('.App').attr('period')) {
            //    var resultAtualLevel = $('.level01Result[level01id=' + level01.attr('id') + '][date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + '][reaudit=false]');
            //    if (resultAtualLevel.length == 0) {
            //        level01.attr('reauditnumber', '0').removeAttr('reaudit').removeAttr('reauditnumber').removeAttr('startreaudit').removeAttr('completed');

            //    }
            //}

            //Informa ao usuário que terminou a reauditoria
            openMessageModal('Re-audit completed', $('#selectPeriod option[value=' + resultStarReaudit.attr('period') + ']').text() + ' re-audit completed!');

        }
    }


    //Se o level01Result tem uma ação corretiva, atribuo ação corretiva no level01 e no resultado
    if (level01Result.children('.level02Result[havecorrectiveaction]').length) {

        level01.attr('correctivaction', 'correctivaction')
        level01Result.attr('havecorrectiveaction', 'havecorrectiveaction');
    }
    //Se o level01Result tem uma reauditoria, atribuo a ação corretiva no level01 e no resultado
    if (level01Result.children('.level02Result[havereaudit]').length) {
        level01.attr('reaudit', 'reaudit');
        level01Result.attr('havereaudit', 'havereaudit');
    }

    //Salva no Banco dados Off Line.
    createFileResult();
    //Retorna ao menu do Level01.

    //Reseto o periodo
    level01Reset(level01);
    periodReset();
    level01Return();

});
$(document).on('click', '.level01List .btnReaudit', function (e) {

    //Seleciono o Level01 a partir do botão que cliquei
    var level01 = $(this).parents('.row').children('.level01');


    //Se ainda estiver faltando finalizar as ações corretivas eu aviso o usuário
    if (level01.attr('correctivaction')) {
        //alert('Open Corrective Action');
        var title = "Warning";
        var content = "Fill the Corrective Action first.";
        openMessageModal(title, content);
        return true;
    }

    //Iniciamos a reautiroria.
    level01.attr('startReaudit', 'startReaudit').removeAttr('completed');

    var lastResutHaveRaudit = getResultHaveReaudit(level01);

    if (lastResutHaveRaudit.attr('completed')) {
        var reauditNumber = lastResutHaveRaudit.attr('reauditNumber');
        reauditNumber++;
        level01.attr('reauditnumber', reauditNumber);

    }

    //Resetarmos os inputs e selected **colocar em uma função melhor
    $('.painel #selectCattleType').val('0');
    $('#inputChainSpeed').val("");
    $('#inputLotNumber').val("");
    $('#inputMudScore').val("");
    $('#biasedUnbiased').val('0');

    //Resetamos os itens do level02
    level02Reset($('.level02List .level02Group[level01id=' + level01.attr('id') + '] .level02'));


    $('.auditReaudit .reauditPeriod').text(level01.siblings('.userInfo').children('div').children('.btnReaudit').children('.reauditPeriod').text());

    //nao é o 

    //level02Complete($('.level02List .level02Group[level01id=' + level01.attr('id') +'] .level02'));

    level01.click();
});
$(document).on('click', '.level02Group .btnReaudit', function (e) {

    var level02 = $(this).parents('.row').children('.level02');
    level02.attr('startReaudit', 'startReaudit').removeAttr('completed');
    level02.attr('defects', '0');
    $('.painelLevel03 .labelPhase').parents('.labelPainel').addClass('hide');

    //if (level02.attr('havephases') && level02.attr('limitexceeded'))
    //{
    //    $('.painelLevel03 .labelPhase').parents('.labelPainel').removeClass('hide');
    //    level02.attr('prevphasexceeded', 'prevphasexceeded').removeAttr('limitexceeded');

    //}
    //$('.painelLevel03 .labelPhase').text(level02.attr('phase'));

    //$('.level02List .level02Group[level01id=' + level01.attr('id') + '] .level02').removeAttr('completed').removeAttr('limitexceeded').removeAttr('notavaliable');
    //level02Reset($('.level02List .level02Group[level01id=' + level01.attr('id') + '] .level02'));
    level02Level03Reset(level02.parents('.level02Group'));
    //nao é o 
    //level02Complete($('.level02List .level02Group[level01id=' + level01.attr('id') +'] .level02'));
    level02.click();
});


$(document).on('click', '.btnCorrectiveAction', function (e) {

    $('.btnCorrectiveAction').removeClass('selected');

    if ($('.level01List').is(':visible')) {
        $('.level01, .level01Result').removeClass('selected');
    }

    var level01 = $(this).parents('.row').children('.level01');
    if (!level01.length) {
        $('.level02').removeClass('selected');

        level01 = $(this).parents('.row').children('.level02');
    }

    if ($(this).parents('.userInfo').siblings('.level02').length) {
        level01 = $('.level01.selected');
        $('.level02').removeClass('selected');
        $(this).parents('.userInfo').siblings('.level02').addClass('selected');
    }
    else {
        $('.level01').removeClass('selected');

    }
    $(this).addClass('selected');

    //$(this).addClass('hide');

    // level01.removeAttr('correctivaction');
    if (level01.attr('correctivActionLevel02') && $('.level01List').is(':visible')) {
        level01.click();
    }
    else { correctiveActionOpen(level01.attr('id')); }
});

$(document).on('click', '#btnSignatureLogin', function (e) {
    if ($('#modalSignatureCorrectiveAction').attr('signature') == 'slaugther')
        getloginSignature($('#signatureLogin').val(), $('#signaturePassword').val(), slaugtherSignatureLogin);
    else
        getloginSignature($('#signatureLogin').val(), $('#signaturePassword').val(), techinicalSignatureLogin);
});

function slaugtherSignatureLogin(user) {

    if (user.attr('userprofile').indexOf("operations") >= 0) {
        $('#btnSignatureLogin').siblings('.modal-close-signature').click();
        $('.SlaugtherSignature').removeClass('hide');
        $('.SlaugtherSignature').children('.name').text(user.attr('username'));
        $('.SlaugtherSignature').children('.date').text(dateTimeWithMinutes());
        $('.SlaugtherSignature').attr('userid', user.attr('userid'));
        $('.SlaugtherSignature').attr('datetime', dateTimeWithMinutes());
        $('.btnSignature.btnSlaugtherSignature').addClass('hide');
    } else {
        Geral.exibirMensagemErro("The user has no permission to sign this form.");
    }

    
}

function techinicalSignatureLogin(user) {

    if (user.attr('userprofile').indexOf("techservices") >= 0) {
        $('#btnSignatureLogin').siblings('.modal-close-signature').click();
        $('.TechinicalSignature').removeClass('hide');
        $('.TechinicalSignature').children('.name').text(user.attr('username'));
        $('.TechinicalSignature').children('.date').text(dateTimeWithMinutes());
        $('.TechinicalSignature').attr('userid', user.attr('userid'));
        $('.TechinicalSignature').attr('datetime', dateTimeWithMinutes());
        $('.btnSignature.btnTechinicalSignature').addClass('hide');
    } else {
        Geral.exibirMensagemErro("The user has no permission to sign this form.");
    }

    
}

$(document).on('click', '#btnMessageOk', function (e) {
    ////remover modal do body 
    //remover modal do body 
    var mensagem = $('.message');


    $('.overlay').hide();
    mensagem.fadeOut("fast");
});

function areaImage(id) {
    showLevel03($("#" + id + ".level02"));
}

function openMessageConfirm(title, content, callback) {

    var mensagem = $('.messageConfirm, .overlay');

    mensagem.children('.head').html(title);
    mensagem.children('.body').children('.txtMessage').html(content);
    $('#btnMessageYes').removeClass('hide');
    $('#btnMessageNo').removeClass('hide');

    mensagem.fadeIn("fast");

    $('#btnMessageYes').click(function () {
        callback();
    });
    $('#btnMessageNo').click(function () {
        mensagem.fadeOut("fast");
    });
    
}


function openMessageModal(title, content) {

    var mensagem = $('.message, .overlay');

    mensagem.children('.head').html(title);
    mensagem.children('.body').html(content);
    $('#btnMessageOk').removeClass('hide');

    mensagem.fadeIn("fast");
    // var $modal = $('#modalMessage');
    // $modal.children('.modal-header').children('.messageHeader').text(title);
    // $modal.children('.modal-body').children('.message').html(content);
    // $modal.modal();
    ////$('.modal-scrollable, .modal-backdrop').removeClass('hide');
}

function openSyncModal(title, content) {

    var mensagem = $('.message, .overlay');
    mensagem.children('.head').html(title);
    mensagem.children('.body').html("Syncing the data collection... ");
        //"<span class='perc'> 0</span> / " + ($('.level02Result[sync=false]').parents('.level01Result').length));
    $('#btnMessageOk').addClass('hide');

    mensagem.addClass('msgSync');
    mensagem.fadeIn("fast");



    setTimeout(function () {
        if ($('.msgSync').is(':visible')) {
            //poderia colocar uma mensagem que não foi possível sincronizar

            mensagemSyncHide();
        }
    }, 10000);
}
function syncModalPer(per) {
    var mensagem = $('.message, .overlay');
    mensagem.children('.body').children('.perc').html(per);
}


function appendDevice(obj, appendTo) {
    var platform = device.platform;

    if (platform == 'windows') {
        MSApp.execUnsafeLocalFunction(function () {
            appendTo.append(obj);
        });
    }
    else {
        appendTo.append(obj);
    }
}

function afterDevice(obj, afterTo) {
    var platform = device.platform;

    if (platform == 'windows') {
        MSApp.execUnsafeLocalFunction(function () {
            afterTo.after(obj);
        });
    }
    else {
        afterTo.after(obj);
    }
}
function beforeDevice(obj, beforeTo) {
    var platform = device.platform;

    if (platform == 'windows') {
        MSApp.execUnsafeLocalFunction(function () {
            beforeTo.before(obj);
        });
    }
    else {
        beforeTo.before(obj);
    }
}

$(document).on('click', '#btnSendCorrectiveAction', function (e) {

    var message = '';
    var descriptionFailure = $('#DescriptionFailure').val();
    var immediateCorrectiveAction = $('#ImmediateCorrectiveAction').val();
    var productDisposition = $('#ProductDisposition').val();
    var preventativeMeasure = $('#PreventativeMeasure').val();
    var slaugtherSignature = $('.SlaugtherSignature').attr('userid');
    var techinicalSignature = $('.TechinicalSignature').attr('userid');
    var unidadeid = $('.App').attr('unidadeid');
    var auditorid = $('.App').attr('userid');

    if (descriptionFailure == '') {
        message += 'The description of the failure is empty.<br>';
    }
    if (immediateCorrectiveAction == '') {
        message += 'The immediate corrective action is empty.<br>';
    }
    if (productDisposition == '') {
        message += 'The product disposition is empty.<br>';
    }
    if (preventativeMeasure == '') {
        message += 'The preventive measure is empty.<br>';
    }
    if ($('.SlaugtherSignature').hasClass('hide')) {
        message += 'The slaughter signature has not inputed.<br>';
    }
    if ($('.TechinicalSignature').hasClass('hide')) {
        message += 'The techinical signature has not inputed.<br>';
    }

    if (message != '') {
        openMessageModal('The following errors ocurred: ', message);
        return;
    }

    var correctiveActionModal = $('#correctiveActionModal');


    var date = $('.App').attr('date');
    var shift = $('.App').attr('shift');
    var period = $('.App').attr('period');


    var level01 = $('.level01[id=' + correctiveActionModal.attr('level01id') + ']');
    var level02 = $('.level02[id=' + correctiveActionModal.attr('level02id') + ']');

    var level01Result = $('.level01Result.correctiveActionSelected');
    var level02Result = $('.level02Result.correctiveActionSelected');;

    correctiveAction = $(document.createElement('div'));
    correctiveAction.addClass('correctiveAction');
    correctiveAction.attr('slaugtherSignature', slaugtherSignature);
    correctiveAction.attr('techinicalSignature', techinicalSignature);
    //correctiveAction.attr('preventativeMeasure', preventativeMeasure);
    //correctiveAction.attr('productDisposition', productDisposition);
    correctiveAction.attr('unidadeid', unidadeid);
    correctiveAction.attr('auditorid', auditorid);
    correctiveAction.attr('date', date);
    correctiveAction.attr('shift', shift);
    correctiveAction.attr('period', period)
    correctiveAction.attr('level01id', level01Result.attr('level01id'))

    descriptionFailure = $('<div class="descriptionFailure">' + descriptionFailure + '</div>');
    immediateCorrectiveAction = $('<div class="immediateCorrectiveAction">' + immediateCorrectiveAction + '</div>');
    productDisposition = $('<div class="productDisposition">' + productDisposition + '</div>');
    preventativeMeasure = $('<div class="preventativeMeasure">' + preventativeMeasure + '</div>');

    correctiveAction.attr('DateTimeSlaughter', $('.SlaugtherSignature').attr('datetime'));
    correctiveAction.attr('DateTimeTechinical', $('.TechinicalSignature').attr('datetime'));
    correctiveAction.attr('AuditStartTime', $('#AuditInformation').children('span#starttime').text());
    correctiveAction.attr('DateCorrectiveAction', $('#CorrectiveActionTaken').children('span#datetime').text());

    appendDevice(descriptionFailure, correctiveAction);
    appendDevice(immediateCorrectiveAction, correctiveAction);
    appendDevice(productDisposition, correctiveAction);
    appendDevice(preventativeMeasure, correctiveAction);

    var level01ResultsUpdate = $('.level01Result[level01id=' + level01.attr('id') + '][havecorrectiveaction]');
    var level02ResultsUpdate = $('.level01Result[level01id=' + level01.attr('id') + '] .level02Result[havecorrectiveaction]');

    if (level01.attr('correctivactionlevel02')) {

        //level01ResultsUpdate = level01Result;
        level02ResultsUpdate = level02Result;
    }

    level02ResultsUpdate.attr('correctiveactioncomplete', 'correctiveActionComplete').removeAttr('havecorrectiveaction').attr('sync', 'false');
    level02.removeAttr('correctivaction').attr('correctiveactioncomplete', 'correctiveActionComplete');
    level01.removeAttr('correctivaction').attr('correctiveactioncomplete', 'correctiveActionComplete');
    $('.correctiveActionSelected').removeClass('correctiveActionSelected');

    $('.level01Result[level01id=' + level01.attr('id') + '] .level02Result[havecorrectiveaction]').attr('correctiveactioncomplete', 'correctiveActionComplete').removeAttr('havecorrectiveaction');
    $('.level01Result[level01id=' + level01.attr('id') + '][havecorrectiveaction]').attr('correctiveactioncomplete', 'correctiveActionComplete').removeAttr('havecorrectiveaction');

    if (!level01ResultsUpdate.children('.level02Result[havecorrectiveaction]').length) {

        level01ResultsUpdate.attr('correctiveactioncomplete', 'correctiveActionComplete').removeAttr('havecorrectiveaction');
        level02.removeAttr('correctivaction').attr('correctiveactioncomplete', 'correctiveActionComplete');
    }

    appendDevice(correctiveAction, level02Result);

    createFileResult();

    correctiveActionModal.fadeOut("fast", function (e) {
        $('.overlay').hide();
        $('body').removeClass('overflowNo');
        $('#selectPeriod').trigger('change');
    });


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
                    $('.viewModal').fadeIn();
                    mensagemSyncHide();
                }, 4000);

            } else {
                $('.messageConfirm, .overlay').children('.body').children('.txtMessage').html('Wrong password.');
            }
        }
    );

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
$(document).on('click', '.viewModal .close', function (e) {
    $('.viewModal').fadeOut(function (e) {
        $('.viewModal .body').empty();
    });
})

function createLog(message) {
    var div = "<div date='" + dateTimeFormat() + "'>" + message + "</div>";

    _readFile("log.txt", function (result) {
        _writeFile("log.txt", div+result);
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

//var storage = window.localStorage;

//var idAcaoCorretiva = 0;

//var idSlaughterLogado = 0;
//var idTechinicalLogado = 0;
//var timeSlaughterLogado = null;
//var timeTechinicalLogado = null;
//var dateExecute = Utils.GerarData();
//var dateStart = Utils.GerarData();

//$("#period").text(storage.getItem("periodo"));
//$("#auditor").text(storage.getItem("userId"));
//$("#datetime").text(dateExecute);
//$("#starttime").text(dateStart);
//$("#shift").text('1');

//$("#auditText").text(storage.getItem("indicatorName"));

//var AcaoCorretiva = {

//    logarUsuarioSlaughter: function () {

//        var obj = {
//            SlaughterPassword: $("#slaughterPassword").val(),
//            SlaughterLogin: $("#slaughterLogin").val()
//        };

//        $.ajax({
//            data: obj,
//            url: '../' + '../api/CorrectiveAction/LogarUsuarioSlaughter',
//            type: 'POST',
//            success: function (data) {
//                idSlaughterLogado = data.Id
//                $("#slaughter").val(data.Name);
//                $("#slaughterDatetime").val(Utils.GerarData());
//                timeSlaughterLogado = Utils.GerarData();
//            }
//        });

//    },

//    logarUsuarioTechnical: function () {

//        var obj = {
//            TechnicalPassword: $("#techinicalPassword").val(),
//            TechnicalLogin: $("#techinicalLogin").val()
//        };

//        $.ajax({
//            data: obj,
//            url: '../' + '../api/CorrectiveAction/LogarUsuarioTechnical',
//            type: 'POST',
//            success: function (data) {
//                idTechinicalLogado = data.Id;
//                $("#techinical").val(data.Name);
//                $("#techinicalDatetime").val(Utils.GerarData());
//                timeTechinicalLogado = Utils.GerarData();
//            }
//        });

//    },

//    enviarAcaoCorretiva: function () {

//        var obj = {
//            CorrectiveAction: {
//                Id: idAcaoCorretiva,
//                DateExecuteFarmatado: dateExecute,
//                Auditor: storage.getItem("userId"),
//                Shift: 1,
//                AuditLevel1: storage.getItem("indicatorId"),
//                AuditLevel2: storage.getItem("indicatorId"),
//                AuditLevel3: storage.getItem("indicatorId"),
//                StartTimeFarmatado: dateStart,
//                Period: storage.getItem("periodo"),
//                DescriptionFailure: $("#DescriptionFailure").val(),
//                ImmediateCorrectiveAction: $("#ImmediateCorrectiveAction").val(),
//                ProductDisposition: $("#ProductDisposition").val(),
//                PreventativeMeasure: $("#PreventativeMeasure").val(),
//                Slaughter: idSlaughterLogado,
//                NameSlaughter: $("#slaughter").val(),
//                DateTimeSlaughterFarmatado: timeSlaughterLogado,
//                Techinical: idTechinicalLogado,
//                NameTechinical: $("#techinical").val(),
//                DateTimeTechinicalFarmatado: timeTechinicalLogado
//            }
//        };

//        // console.log(obj);

//        $.ajax({
//            data: obj,
//            url: '../' + '../api/CorrectiveAction/SalvarAcaoCorretiva',
//            type: 'POST',
//            success: function (data) {
//                window.location.href = '/Coleta/view/indicators.html';
//            }
//        });

//    },

//    verificarAcaoCorretivaIncompleta: function () {

//        var obj = {
//            CorrectiveAction: {
//                Auditor: storage.getItem("userId"),
//                Shift: 1,
//                AuditLevel1: storage.getItem("indicatorId"),
//                AuditLevel2: storage.getItem("indicatorId"),
//                AuditLevel3: storage.getItem("indicatorId"),
//                Period: storage.getItem("periodo")
//            }
//        };

//        $.ajax({
//            data: obj,
//            url: '../' + '../api/CorrectiveAction/VerificarAcaoCorretivaIncompleta',
//            type: 'POST',
//            success: function (data) {

//                console.log(data);

//                if (data != null) {

//                    if (data.Id != 0) {

//                        idAcaoCorretiva = data.Id;

//                        dateExecute = data.DateExecuteFarmatado;
//                        $("#datetime").text(dateExecute);

//                        dateStart = data.StartTimeFarmatado;
//                        $("#starttime").text(dateStart);

//                        storage.setItem("userId", data.Auditor);
//                        $("#auditor").text(data.Auditor);

//                        $("#shift").text('1');+

//                        storage.setItem("indicatorId", data.AuditLevel1);
//                        storage.setItem("indicatorId", data.AuditLevel2);
//                        storage.setItem("indicatorId", data.AuditLevel3);

//                        storage.setItem("periodo", data.Period);
//                        $("#period").text(data.Period);

//                        $("#DescriptionFailure").val(data.DescriptionFailure);
//                        $("#ImmediateCorrectiveAction").val(data.ImmediateCorrectiveAction);
//                        $("#ProductDisposition").val(data.ProductDisposition);
//                        $("#PreventativeMeasure").val(data.PreventativeMeasure);

//                        idSlaughterLogado = data.Slaughter;
//                        timeSlaughterLogado = data.DateTimeSlaughterFarmatado;
//                        $("#slaughterDatetime").val(timeSlaughterLogado);
//                        $("#slaughter").val(data.NameSlaughter);


//                        idTechinicalLogado = data.Techinical;
//                        timeTechinicalLogado = data.DateTimeTechinicalFarmatado;
//                        $("#techinicalDatetime").val(timeTechinicalLogado);
//                        $("#techinical").val(data.NameTechinical);

//                    }
//                }
//            }
//        });

//    },

//};

//AcaoCorretiva.verificarAcaoCorretivaIncompleta();