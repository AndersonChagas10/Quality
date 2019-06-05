$(document).on('click', '#btnDate', function (e) {
    if ($('.level3Group.PCC1B').is(':visible')) {
        addPCC1BSequence('2');
    } else {
        _readFile("usersOffline.txt", function (r) {
            var role, users;
            users = r;
            users = JSON.parse(users);
            if (userlogado.attr('userlogin').toLowerCase() == users.Name.toLowerCase()) {
                if (users.Role)
                    $.grep(users.Role.split(','), function (role, counter) {
                        if (role.toLowerCase() == "admin" || role.toLowerCase() == "backdate" || role.toLowerCase() == "coleta") {
                            //ping(alteraDataRetroativaOnline, alteraDataRetroativaOffLine);                    
                            if (online) {
                                alteraDataRetroativaOnline();
                            } else {
                                alteraDataRetroativaOffLine();
                            }
                        }
                    })
            }
        });
    }
});


$(document).on('click', '#btnShowImage', function (e) {
    imageShow();
});

function areaImage(area) {
    var level2 = $(".level2 .levelName:contains('" + area + "'):first").parent();
    level2.click();
}

function imageShow() {
    $('.overlay').fadeIn('fast');
    $(".ccaImage").animate({
        left: "0px"
    }, "fast", function () {
        $(this).removeClass('hide');
    });
}

function imageHide() {
    if ($('.ccaImage').is(':visible')) {
        $(".ccaImage").addClass('hide').animate({ "left": "-256px" }, "fast", function (e) {
            $('.overlay').hide();
        });
    }
}

$(document).on('click', '#btnMore', function (e) {
    rightMenuShow();
});

$(document).on('click', 'a.navbar-brand', function (e) {
    $('.breadcrumb li a:last').click();
});

$(document).on('change', 'select#selectPeriod', function (e) {

    resetApplication();
    //Reseto o Period para as configurações a partir dos resultados sincronizados
    periodReset();
});

$(document).on('change', '#period', function (e) {
    $(".App").attr('period', $(this).val());
    if ($(this).val() != "0") {
        $(".period").text($('#period option:selected').text());
        level2ConsolidationReset();
        updateReaudit(1);
        updateCorrectiveAction();
        $('.level1').each(function (index, self) {
            completeLevel1(self);
        });
    } else {
        $(".period").text("");
    }

    if ($(this).not(':visible').length > 0) {
        $(".period").addClass('hide');
    }
});

function userAPPDataInsert(user) {

    $('.App').attr('unidadeid', user.attr('unidadeid'));
    localStorage.setItem("unit", user.attr('unidadeid'));

    $('.unit').text(user.attr('unidadename'));
    $('.App').attr('userid', user.attr('userid'));

    if ($('#shift').length) {
        $('.App').attr('shift', $('#shift option:selected').val());

        if ($('#shift option:selected').length == 0)
            $('.App').attr('shift', '1');
    }
    else {
        $('.App').attr('shift', '1');
    }

    if ($('#selectPeriod').length) {
        $('.App').attr('period', $('#selectPeriod').val());
    }
    else {
        $('.App').attr('period', '1');
    }

    $('.App').attr('date', MMddyyyy());
    $('.App').attr('logintime', dateTimeFormat());


    $('footer .user').text(user.attr('username'));

}

$(document).on('click', '#btnSyncParam', function (e) {

    //$('#btnSync').click();
    ping(paramsUpdate_OnLine, paramsUpdate_OffLine);

});

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
            && atualResult.attr('date') == getCollectionDate()
            && atualResult.attr('shift') == $('.App').attr('shift')
            && atualResult.attr('period') == $('.App').attr('period')) {
            //Atribui o resultado da ultima avaliação a variavel evaluate
            evaluate = parseInt(atualResult.attr('evaluate'));
        }
        else {
            evaluate = 1;
        }



        //verifico se tenho ação corretiva para algum periodo
        var corretivActionsNoCompleted = $('.level01Result[level01id=' + level01.attr('id') + '][date="' + getCollectionDate() + '"][shift=' + $('.App').attr('shift') + '][havecorrectiveaction]:last');

        //verifico se eu tenho reauditoria para o periodo e data atual
        var reauditNotComplete = $('.level01Result[level01id=' + level01.attr('id') + '][date="' + getCollectionDate() + '"][shift=' + $('.App').attr('shift') + '][havereaudit]:last');
        if (!reauditNotComplete.length) {
            //verifica se tem reauditoria para qualquer period anterior
            reauditNotComplete = $('.level01Result[level01id=' + level01.attr('id') + '][shift=' + $('.App').attr('shift') + '][havereaudit]:last');
        }

        //Instancio [atualResultPeriod] para verificar se o resutlado atual é do periodo seelcionado
        var atualResultPeriod = false;
        if (lastResult.attr('date') == getCollectionDate() &&
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

        //var reauditInPeriod = false;
        //if(reauditNotComplete.attr('date') == atualResult.attr('date') &&
        //   reauditNotComplete.attr('shift') == atualResult.attr('shift') &&
        //    reauditNotComplete.attr('period') == atualResult.attr('period'))
        //{
        //    reauditInPeriod = true;
        //}




        //se tenho um resultado que tem reauditoria nao completa e esse resultado já estiver completo
        if (reauditNotComplete.length && reauditNotComplete.attr('completed')) {

            //Instancio as datas do App e a data da reauditoria
            var dateAPP = getCollectionDate();
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
            if (atualResult.attr('completed') && evaluate >= evaluateConf && atualResult.attr('date') == getCollectionDate()) {
                //O resultado do period está completo
                level01.attr('completed', 'completed');
            }

        }

    });
    //Configuro o level01 com o resultado
    configureLevel01();

}

function loadParCompanyList(list) {
    $('#selectParCompany').empty();
}