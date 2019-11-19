function correctiveActionOpen(level01Id, date, shift, period) {

    //Seleciona o Id do Level01 caso não tenha recebido nenhum parametro.
    level01Id = level01Id ? level01Id : $('.level1.selected').attr('id');
    //Seleciona a data caso não tenha recebido nenhum parametro.
    date = date ? date : getCollectionDate();
    //Seleciona o shift caso não tenha recebido nenhum parametro.
    shift = shift ? shift : $('.App').attr('shift');
    //period = period ? period : $('.App').attr('period');
    period = period ? period : 1;

    //Instancio o Level01.
    var level01 = $('.level1[id=' + level01Id + ']');

    var correctiveActionModal = $('#correctiveActionModal');

    //Shit e Period estão pegando do APP para o Brasil, mas para o os EUA tem que pegar da consolidação

    correctiveActionModal.attr('unidadeid', $('.App').attr('unidadeid'))
    correctiveActionModal.attr('auditorid', $('.App').attr('userid'))
    correctiveActionModal.attr('shift', shift)
    correctiveActionModal.attr('period', period)
    correctiveActionModal.attr('period', period)

    $('#CorrectiveActionTaken').children('#datetime').text(dateTimeWithMinutes());
    $('#CorrectiveActionTaken').children('#auditor').text(userlogado[0].getAttribute('username')); //Colocar o Usuário Atual
    $('#CorrectiveActionTaken').children('#shift').text($('#shift option[value=' + shift + ']').text());
    $('#AuditInformation').children('#auditText').text(level01.children('.levelName').text());

    var ConsolidationResult = $('.ResultsConsolidation .Resultlevel2[level1id=' + level01.attr('id') + '][havecorrectiveaction=true][unitid=' +
        $('.App').attr('unidadeid') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + ']:first');

    if (isEUA == true) { //Só serve para CCA e CFF
        ConsolidationResult = $('.ResultsConsolidation .Resultlevel2[level1id=' + level01.attr('id') + '][havecorrectiveaction=true][unitid=' +
            $('.App').attr('unidadeid') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + '][havereaudit=true]:last');
    }


    /*
        if (!ConsolidationResult.length) {
            openMessageModal("Corrective Action not Found", "Try again");
            return false;
        }
    */
    ConsolidationResult.addClass('selected');

    $('#AuditInformation').children('#starttime').text(dateTimeWithMinutes().slice(0, 16));
    correctiveActionModal.attr('level01id', ConsolidationResult.attr('level1id'));
    correctiveActionModal.attr('level02id', ConsolidationResult.attr('level2id'));
    correctiveActionModal.attr('evaluationnumber', ConsolidationResult.attr('evaluation'));
    correctiveActionModal.attr('collectionlevel2_id', ConsolidationResult.attr('collectionlevel2_id_correctiveaction'));
    if (ConsolidationResult.attr('period')) {
        correctiveActionModal.attr('period', ConsolidationResult.attr('period'));
    }
    correctiveActionModal.attr('parfrequency_id', level01.attr('parfrequency_id'));
    //verificar data retroativa
    correctiveActionModal.attr('date', dateTimeWithMinutes());

    $('#AuditInformation').children('#correctivePeriod').text($('#period option[value=' + ConsolidationResult.attr('period') + ']').text());

    $('.overlay').show();
    $('body').addClass('overflowNo');
    correctiveActionModal.removeClass('hide');

    correctiveActionModal.fadeIn("fast");

    $('#DescriptionFailure, #ImmediateCorrectiveAction, #ProductDisposition, #PreventativeMeasure').val("");

    // $.post(urlPreffix + "/api/ApontamentosDiarios/GetRL/"+ConsolidationResult.attr('collectionlevel2_id_correctiveaction'), 
    //     function(r) {
    //     var falhas ="";
    //     var count = 0;
    //     while(count < r.length){
    //         falhas += getResource("description_failure")+": "+r[count].ParLevel3_Name+" \n";
    //         falhas += getResource("defects")+": "+r[count].Defects+"\n";
    //         count++;
    //     }
    //     falhas+= getResource("defectsTotal")+": "+ ConsolidationResult.attr('defectstotall2');
    //     $('#DescriptionFailure').val(falhas);
    // });

    if (level01.attr('hasgrouplevel2') == 'true')
        descreveFalhaCFF(level01);
    else
        descreveFalhaOffline(level01);

    $('#DescriptionFailure').focus();
}

function correctiveActionOpenPesoHB(level01Id, date, shift, period) {

    //Seleciona o Id do Level01 caso não tenha recebido nenhum parametro.
    level01Id = level01Id ? level01Id : $('.level1.selected').attr('id');
    //Seleciona a data caso não tenha recebido nenhum parametro.
    date = date ? date : getCollectionDate();
    //Seleciona o shift caso não tenha recebido nenhum parametro.
    shift = shift ? shift : $('.App').attr('shift');
    //period = period ? period : $('.App').attr('period');
    period = period ? period : 1;

    //Instancio o Level01.
    var level01 = $('.level1[id=' + level01Id + ']');

    var correctiveActionModal = $('#correctiveActionModal');

    //Shit e Period estão pegando do APP para o Brasil, mas para o os EUA tem que pegar da consolidação

    correctiveActionModal.attr('unidadeid', $('.App').attr('unidadeid'))
    correctiveActionModal.attr('auditorid', $('.App').attr('userid'))
    correctiveActionModal.attr('shift', shift)
    correctiveActionModal.attr('period', period)
    correctiveActionModal.attr('period', period)

    $('#CorrectiveActionTaken').children('#datetime').text(dateTimeWithMinutes());
    $('#CorrectiveActionTaken').children('#auditor').text(userlogado[0].getAttribute('username')); //Colocar o Usuário Atual
    $('#CorrectiveActionTaken').children('#shift').text($('#shift option[value=' + shift + ']').text());
    $('#AuditInformation').children('#auditText').text(level01.children('.levelName').text());

    // var ConsolidationResult = $('.ResultsConsolidation .Resultlevel2[level1id=' + level01.attr('id') + '][havecorrectiveaction=true][unitid=' +
    //     $('.App').attr('unidadeid') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + ']:first');

    // ConsolidationResult.addClass('selected');

    $('#AuditInformation').children('#starttime').text(dateTimeWithMinutes().slice(0, 16));
    correctiveActionModal.attr('level01id', $(_level1).attr('id'));
    correctiveActionModal.attr('level02id', $(_level2).attr('id'));
	correctiveActionModal.attr('evaluationnumber', $(_level2).attr('evaluatecurrent'));
    // correctiveActionModal.attr('collectionlevel2_id', ConsolidationResult.attr('collectionlevel2_id_correctiveaction'));

    if (period) {
        correctiveActionModal.attr('period', period);
    }

    correctiveActionModal.attr('parfrequency_id', level01.attr('parfrequency_id'));
    //verificar data retroativa
    correctiveActionModal.attr('date', dateTimeWithMinutes());

    $('#AuditInformation').children('#correctivePeriod').text($('#period option[value=' + period + ']').text());

    $('.overlay').show();
    $('body').addClass('overflowNo');
    correctiveActionModal.removeClass('hide');

    correctiveActionModal.fadeIn("fast");

    $('#DescriptionFailure, #ImmediateCorrectiveAction, #ProductDisposition, #PreventativeMeasure').val("");

    if (level01.attr('hasgrouplevel2') == 'true')
        descreveFalhaCFF(level01);
    else
        descreveFalhaOffline(level01);

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
        for (var sample = 0; sample < totalsample; sample++) {

            //procuro todos os levels 2 com a amostra atual no loop
            var level03ResultBySample = level01Result.children('.level02Result[sample=' + sample + ']').children('.level03Result');



            var erro = 0;
            var level02Level03Names = "";
            level03ResultBySample.each(function (e) {

                var level03Result = $(this);
                var level02Result = level03Result.parents('.level02Result');

                var level03Erros = parseInt($(this).attr('value'));

                erro = erro + level03Erros;

                if (level03Erros > 0) {
                    var level02Name = $('.level02[level02id=' + level02Result.attr('level02id') + '] .levelName:first').text();
                    var level03Name = $('.level03[id=' + level03Result.attr('level03id') + '] .levelName:first').text();

                    var rowLevel02Level03Name = level03Erros + " " + level02Name + " " + level03Name;

                    if (level02Level03Names == "") {
                        level02Level03Names += rowLevel02Level03Name;
                    }
                    else {
                        level02Level03Names += "," + rowLevel02Level03Name;
                    }
                }
            });

            if (level02Level03Names) {
                level02Level03Names = " - " + level02Level03Names;
            }
            if (erro >= 3) {
                string3moreErros += 'Set ' + evaluate + ' Side ' + sample + ': ' + erro + ' defect(s)' + level02Level03Names + '\n';
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
}
$(document).on('click', '.btnSlaugtherSignatureRemove', function (e) {
    removeSlaughterSignature();
});

$(document).on('click', '.btnTechinicalSignatureRemove', function (e) {
    removeTechnicalSignature();
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
        getSignatureLogin($('#signatureLogin').val(), $('#signaturePassword').val(), 'slaughter');
    else
        getSignatureLogin($('#signatureLogin').val(), $('#signaturePassword').val(), 'technical');
});

function getSignatureLogin(username, password, permission) {
    $.ajax({
        data: {
            app: true,
            Name: username,
            Password: AES.Encrypt(password),
        },
        url: urlPreffix + '/api/User/AuthenticationLogin',
        type: 'POST',
        success: function (data) {

            if (data.MensagemExcecao) {
                Geral.exibirMensagemErro(getResource("invalid_user_or_password"));
                $('#btnLoginOnline').button('reset');
                $('#btnLoginOffline').button('reset');
            } else {
                if (data.Retorno.Role.toLowerCase().indexOf(permission) >= 0) {
                    if (permission.toLowerCase() == "slaughter") {
                        $('#btnSignatureLogin').siblings('.modal-close-signature').click();
                        $('.SlaugtherSignature').removeClass('hide');
                        $('.SlaugtherSignature').children('.name').text(data.Retorno.FullName);
                        $('.SlaugtherSignature').children('.date').text(dateTimeWithMinutes());
                        $('.SlaugtherSignature').attr('userid', data.Retorno.Id);
                        $('.SlaugtherSignature').attr('datetime', dateTimeWithMinutes());
                        $('.btnSignature.btnSlaugtherSignature').addClass('hide');
                    } else if (permission.toLowerCase() == "technical") {
                        $('#btnSignatureLogin').siblings('.modal-close-signature').click();
                        $('.TechinicalSignature').removeClass('hide');
                        $('.TechinicalSignature').children('.name').text(data.Retorno.FullName);
                        $('.TechinicalSignature').children('.date').text(dateTimeWithMinutes());
                        $('.TechinicalSignature').attr('userid', data.Retorno.Id);
                        $('.TechinicalSignature').attr('datetime', dateTimeWithMinutes());
                        $('.btnSignature.btnTechinicalSignature').addClass('hide');
                    } else {
                        Geral.exibirMensagemErro(getResource("user_has_no_permission_fill_form"));
                    }
                } else {
                    Geral.exibirMensagemErro(getResource("user_has_no_permission_fill_form"));
                }
            }
        },
        timeout: 600000,
        error: function (e) {
            console.log(e);
            if (permission.toLowerCase() == "slaughter") {
                getlogin(username, password, slaugtherSignatureLogin);
            } else if (permission.toLowerCase() == "technical") {
                getlogin(username, password, techinicalSignatureLogin);
            }
        }
    });
}

// $(document).on('click', '#btnSignatureLogin', function (e) {
//     if ($('#modalSignatureCorrectiveAction').attr('signature') == 'slaugther')
//         getlogin($('#signatureLogin').val(), $('#signaturePassword').val(), slaugtherSignatureLogin);
//     else
//         getlogin($('#signatureLogin').val(), $('#signaturePassword').val(), techinicalSignatureLogin);
// });

function slaugtherSignatureLogin(user) {

    _readFile("usersUnidade.txt", function (r) {
        var users = JSON.parse(r);
        var userSlaughter = $.grep(users, function (obj, counter) {
            return obj.Id == user.attr('userid');
        });
        if (userSlaughter.length) {
            $.grep(userSlaughter[0].Role.split(','), function (role, counter) {
                if (role == "Slaughter") {
                    $('#btnSignatureLogin').siblings('.modal-close-signature').click();
                    $('.SlaugtherSignature').removeClass('hide');
                    $('.SlaugtherSignature').children('.name').text(user.attr('username'));
                    $('.SlaugtherSignature').children('.date').text(dateTimeWithMinutes());
                    $('.SlaugtherSignature').attr('userid', user.attr('userid'));
                    $('.SlaugtherSignature').attr('datetime', dateTimeWithMinutes());
                    $('.btnSignature.btnSlaugtherSignature').addClass('hide');
                } else
                    Geral.exibirMensagemErro(getResource("user_has_no_permission_fill_form"));
            });
        }
    });
}

function techinicalSignatureLogin(user) {
    _readFile("usersUnidade.txt", function (r) {
        var users = JSON.parse(r);
        var userSlaughter = $.grep(users, function (obj, counter) {
            return obj.Id == user.attr('userid');
        });
        if (userSlaughter.length) {
            $.grep(userSlaughter[0].Role.split(','), function (role, counter) {
                if (role == "Technical") {
                    $('#btnSignatureLogin').siblings('.modal-close-signature').click();
                    $('.TechinicalSignature').removeClass('hide');
                    $('.TechinicalSignature').children('.name').text(user.attr('username'));
                    $('.TechinicalSignature').children('.date').text(dateTimeWithMinutes());
                    $('.TechinicalSignature').attr('userid', user.attr('userid'));
                    $('.TechinicalSignature').attr('datetime', dateTimeWithMinutes());
                    $('.btnSignature.btnTechinicalSignature').addClass('hide');
                } else
                    Geral.exibirMensagemErro(getResource("user_has_no_permission_fill_form"));
            });
        }
    });
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

    if ($('.App').attr('local') == "eua" || $('.App').attr('local') == "canada") {
        if (slaugtherSignature == null) {
            openMessageModal(getResource("insert_slaughter_signature"), null);
            return;
        } else
            if (techinicalSignature == null) {
                openMessageModal(getResource("insert_technical_signature"), null);
                return;
            }
    }

    if (descriptionFailure == '') {
        message += getResource("fail_description_is_empty") + '<br>';
    }
    if (immediateCorrectiveAction == '') {
        message += getResource("immediate_corrective_action_is_empty") + '.<br>';
    }
    //if (productDisposition == '') {
    //    message += 'A disposição do produto está vazia<br>';
    //}
    //if (preventativeMeasure == '') {
    //    message += 'A medida preventiva está vazia<br>';
    //}
    if (message != '') {
        openMessageModal(getResource("some_problems_occur") + ': ', message);
        return;
    }

    var correctiveActionModal = $('#correctiveActionModal');

    var date = getCollectionDate();
    var shift = $('.App').attr('shift');
    var period = $('.App').attr('period');

    var level01 = $('.level1[id=' + correctiveActionModal.attr('level01id') + ']:first');
    //var level02 = $('.level2[id=' + correctiveActionModal.attr('level02id') + ']:first');

    var correctiveAction = $(document.createElement('div'));
    correctiveAction.addClass('correctiveAction');
    correctiveAction.attr('slaugtherSignature', slaugtherSignature);
    correctiveAction.attr('techinicalSignature', techinicalSignature);

    correctiveAction.attr('unidadeid', unidadeid);
    correctiveAction.attr('auditorid', auditorid);
    correctiveAction.attr('date', date);
    correctiveAction.attr('shift', shift);
    correctiveAction.attr('period', period)


    //    correctiveAction.attr('level01id', level01Result.attr('level01id'))
    correctiveAction.attr('level01id', correctiveActionModal.attr('level01id'))
    correctiveAction.attr('level02id', correctiveActionModal.attr('level02id'))


    descriptionFailure = $('<div class="descriptionFailure">' + descriptionFailure + '</div>');
    immediateCorrectiveAction = $('<div class="immediateCorrectiveAction">' + immediateCorrectiveAction + '</div>');
    productDisposition = $('<div class="productDisposition">' + productDisposition + '</div>');
    preventativeMeasure = $('<div class="preventativeMeasure">' + preventativeMeasure + '</div>');

    correctiveAction.attr('DateTimeSlaughter', $('.SlaugtherSignature').attr('datetime'));
    correctiveAction.attr('DateTimeTechinical', $('.TechinicalSignature').attr('datetime'));
    correctiveAction.attr('AuditStartTime', $('#AuditInformation').children('span#starttime').text());
    correctiveAction.attr('DateCorrectiveAction', $('#CorrectiveActionTaken').children('span#datetime').text());


    correctiveAction.attr('collectionlevel2_id', $('#correctiveActionModal').attr('collectionlevel2_id'));
    correctiveAction.attr('evaluationnumber', $('#correctiveActionModal').attr('evaluationnumber'));
    correctiveAction.attr('parfrequency_id', $('#correctiveActionModal').attr('parfrequency_id'));

    correctiveAction.attr('sync', 'false');
    var reauditnumber = $('.Resultlevel2[level1id=' + level01.attr('id') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') +
        '][havecorrectiveaction=true]:last').attr('reauditnumber');
    correctiveAction.attr('reauditnumber', reauditnumber);
    appendDevice(descriptionFailure, correctiveAction);
    appendDevice(immediateCorrectiveAction, correctiveAction);
    appendDevice(productDisposition, correctiveAction);
    appendDevice(preventativeMeasure, correctiveAction);

    appendDevice(correctiveAction, $('.Results'));

    //gravar banco de dados
    //$('.btnCA').hasClass('caLevel02')
    $('.modal-close-ca').click();
    $('.ResultsConsolidation .Resultlevel2.selected').removeAttr('havecorrectiveaction');

    // var level01 = $('.level1.selected');
    level01.removeAttr('havecorrectiveaction');
    //$('div[havecorrectiveaction]').removeAttr('havecorrectiveaction');
    level01.removeAttr('havecorrectiveaction');
    $('.Resultlevel2[level1id=' + level01.attr('id') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + ']').removeAttr('havecorrectiveaction');

    createFileResultConsolidation();

    $('.btnAllNA').text('Todos N/A');


    $('.btnCA').removeClass('caLevel02');
    $('.modal-close-ca').click();

    updateCorrectiveAction();
    if (isEUA) {
        var reauditnumber;
        if (isNaN(level01.attr('reauditnumber')) == false)
            reauditnumber = level01.attr('reauditnumber');
        if (parseInt($('.level2[completed=completed][reauditevaluation]').length) != parseInt($('.level2').length) && isNaN(reauditnumber) == false)
            reauditnumber++;
        if (isNaN(reauditnumber) == false)
            level01.attr('reauditnumber', reauditnumber);
    }

    if (alertlevel == 2) {
        level01.attr('havereaudit', 'false');
        level01.removeAttr('isreaudit');
    }
    removeSlaughterSignature();
    removeTechnicalSignature();

    $('#btnCA').addClass('hide');
});

function sendCorrectiveActionOnLine() {

    var CorrectiveActions = $('.correctiveAction[sync=false]');
    if (CorrectiveActions.length > 0) {
        CorrectiveActions.each(function (e) {

            var correctiveAction = $(this);

            var DateCorrectiveAction = dateTimeWithMinutes();
            var AuditStartTime = dateTimeWithMinutes();


            if (isEUA == false) {
                correctiveAction.attr('datetimeslaughter', dateTimeWithMinutesBR());
                correctiveAction.attr('datetimetechinical', dateTimeWithMinutesBR());
                DateCorrectiveAction = dateTimeWithMinutesBR();
                AuditStartTime = dateTimeWithMinutesBR();
            }

            // correctiveAction.attr('datetimeslaughter', dateTimeWithMinutes());
            // correctiveAction.attr('datetimetechinical', dateTimeWithMinutes());

            var dados = {
                "CollectionLevel2_Id": correctiveAction.attr('collectionlevel2_id') != undefined ? correctiveAction.attr('collectionlevel2_id') : 0,
                "ParLevel1_Id": correctiveAction.attr('level01id'),
                "ParLevel2_Id": correctiveAction.attr('level02id'),
                "Shift": correctiveAction.attr('shift'),
                "Period": correctiveAction.attr('period'),
                "ParCompany_Id": correctiveAction.attr('unidadeid'),
                "EvaluationNumber": correctiveAction.attr('evaluationnumber'),
                "ParFrequency_Id": correctiveAction.attr('parfrequency_id'),
                "data": correctiveAction.attr('date'),
                "AuditorId": correctiveAction.attr('auditorid'),
                "SlaughterId": correctiveAction.attr('slaugthersignature') == undefined ? correctiveAction.attr('auditorid') : correctiveAction.attr('slaugthersignature'),
                "TechinicalId": correctiveAction.attr('techinicalsignature') == undefined ? correctiveAction.attr('auditorid') : correctiveAction.attr('techinicalsignature'),
                "DateTimeSlaughter": correctiveAction.attr('datetimeslaughter'),
                "DateTimeTechinical": correctiveAction.attr('datetimetechinical'),
                "DateCorrectiveAction": DateCorrectiveAction,
                "AuditStartTime": AuditStartTime,
                "DescriptionFailure": escape(correctiveAction.children('.descriptionFailure').html()),
                "ImmediateCorrectiveAction": escape(correctiveAction.children('.immediateCorrectiveAction').html()),
                "ProductDisposition": escape(correctiveAction.children('.productDisposition').html()),
                "PreventativeMeasure": escape(correctiveAction.children('.preventativeMeasure').html()),
                "reauditnumber": correctiveAction.attr('reauditnumber') == undefined ? 0 : correctiveAction.attr('reauditnumber')
            };

            $.ajax({
                data: dados,
                //    url: urlPreffix + '/api/User/AuthenticationLogin',
                headers: token(),
                url: urlPreffix + '/api/SyncServiceApi/InsertCorrectiveAction',
                type: 'POST',
                success: function (data) {
                    $('.Results .correctiveAction[sync=false][reauditnumber=' + correctiveAction.attr('reauditnumber') + ']').attr('sync', 'true');
                    correctiveAction.remove();
                    createFileResultConsolidation();
                    createFileResult();
                    $('.btnAllNA').text('Todos N/A');

                },
                error: function (e) {
                    console.log(e);
                }
            });

        });
        removeSlaughterSignature();
        removeTechnicalSignature();
        var reauditnumber;

    }
    sendVerificacaoTipificacao();
    sendResultsTimeout();
}
function removeSlaughterSignature() {
    $('.SlaugtherSignature').addClass('hide');
    $('.btnSlaugtherSignature').removeClass('hide');
    $('.SlaugtherSignature').removeAttr('userid');
}
function removeTechnicalSignature() {
    $('.TechinicalSignature').addClass('hide');
    $('.btnTechinicalSignature').removeClass('hide');
    $('.TechinicalSignature').removeAttr('userid');
}
function updateCorrectiveAction() {

    $('.level1:visible').each(function (index, self) {
        var reaudMax = 0;
        if ($('.Resultlevel2[level1id=' + $(self).attr('id') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + '][unitid=' +
            $('.App').attr('unidadeid') + ']:last').attr('reauditnumber') > 0)
            reaudMax = $('.Resultlevel2[level1id=' + $(self).attr('id') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + '][unitid=' +
                $('.App').attr('unidadeid') + ']:last').attr('reauditnumber');

        if ($('.Resultlevel2[level1id=' + $(self).attr('id') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + '][reauditnumber=' +
            reaudMax + '][havecorrectiveaction=true]').length > 0) {
            $(self).siblings('.userInfo').children('.btnCALevel1').removeClass('hide');
            $(self).siblings('.userInfo').children('.btnReaudit').attr('disabled', 'disabled');
        } else {
            $(self).siblings('.userInfo').children('.btnCALevel1').addClass('hide');
            $(self).siblings('.userInfo').children('.btnReaudit').removeAttr('disabled');
        }
    });

    $('.level2:visible').each(function (index, self) {
        if ($('.Resultlevel2[level1id=' + $('level1.selected').attr('id') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + '][havecorrectiveaction=true]').length > 0) {
            $(self).siblings('.userInfo').children('.btnReaudit').attr('disabled', 'disabled');
        } else {
            $(self).siblings('.userInfo').children('.btnReaudit').removeAttr('disabled');
        }
    });

}

function descreveFalhaOffline(level01) {
    var reauditnumber = 0;
    var UltReauditnumber = $('.Resultlevel2[level1id=' + level01.attr('id') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') +
        '][collectiondate=' + getCollectionDate() + '][havereaudit=true]:last').attr('reauditnumber');
    if (UltReauditnumber > 0)
        reauditnumber = UltReauditnumber;
    var mockAva = 0; mockAmo = 0;
    if (level01.attr('havereaudit') == 'true') {
        var res = $('.Resultlevel2[defectsl2!=0][level1id=' + level01.attr('id') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') +
            '][collectiondate=' + getCollectionDate() + '][reauditnumber=' + reauditnumber + ']');
    } else {
        var res = $('.Resultlevel2[havereaudit=true][level1id=' + level01.attr('id') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') +
            '][reauditnumber=' + reauditnumber + ']');
    }
    var defs = "";
    res.each(function (e) {

        if ($(this).attr('defectsl2') == 0)
            return;

        if ($(this).attr('evaluation') > mockAva || $(this).attr('sample') > mockAmo) {
            mockAva = $(this).attr('evaluation');
            mockAmo = $(this).attr('sample');
            defs += "" + getResource('evaluation') + " " + $(this).attr('evaluation') + " " + getResource('sample') + " " + $(this).attr('sample');
        }
        var level2 = $('.level2[id=' + $(this).attr('level2id') + ']');
        // level 2
        defs += "\n     " + $('.level2[id=' + $(this).attr('level2id') + ']>span').text();
        defs += ": " + parseInt($('.level2[id=' + $(this).attr('level2id') + ']').attr('parnotconformityrule_value')) + " " +
            getResource('allowed') + " \n";

        var leveis3s = JSON.parse(localStorage.getItem('defeitosl3s'));
        var defl3s = [];

        $.grep(leveis3s, function (def, counter) {
            if (def.idL2 == level2.attr('id') && def.data == getCollectionDate() && def.shift == $('.App').attr('shift') && def.period == $('.App').attr('period')
                && def.rnumber == reauditnumber)
                defl3s.push(def);
        })
        defl3s.forEach(function (obj, e) {
            defs += "       " + $($('.level3[id=' + obj.idL3 + '] > a > span')[0]).text();
            defs += " - " + obj.defeitos + " " + getResource('defects') + "\n";
        });
    });

    $('#DescriptionFailure').val(defs);
}
function descreveFalhaCFF(level01) {
    var ev = 0, smp = 0;
    var reauditnumber = 0;
    var UltReauditnumber = $('.Resultlevel2[level1id=' + level01.attr('id') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') +
        '][collectiondate=' + getCollectionDate() + '][havereaudit=true]:last').attr('reauditnumber');
    if (UltReauditnumber > 0)
        reauditnumber = UltReauditnumber;
    var defs = " ";

    var leveis3s = JSON.parse(localStorage.getItem('defeitosl3s'));
    var amostra = 0, avaliacao = 0;
    var defl3s = [];
    $.grep(leveis3s, function (def, counter) {
        if (def.idL1 == level01.attr('id') && def.data == getCollectionDate() && def.shift == $('.App').attr('shift') && def.period == $('.App').attr('period')
            && def.rnumber == reauditnumber)
            defl3s.push(def);
    });

    parseInt($('.level2[id=' + $(this).attr('level2id') + ']').attr('parnotconformityrule_value')) + " " + getResource('allowed') + " \n";
    var mockl2 = '0';
    var totalDefs = 0;

    defl3s.forEach(function (obj, e) {
        totalDefs += parseInt(obj.defeitos);
    });

    defl3s.forEach(function (obj, e) {
        if (obj.avaliacao > avaliacao || obj.amostra > amostra) {
            avaliacao = obj.avaliacao;
            amostra = obj.amostra;
            defs += "\n" + getResource('evaluation') + " " + avaliacao + " " + getResource('sample') + " " + amostra;
            defs += " - " + "total " + getResource('defects') + " " + totalDefs + "\n";
        }
        if (obj.idL2 != mockl2) {
            mockl2 = obj.idL2;
            defs += "    " + $('.level2[id=' + obj.idL2 + '] >.panel >.panel-heading > h4 >a').text() + "\n";
            //defs += " " + parseInt($('.level2[id=' + obj.idL2 + ']').attr('parnotconformityrule_value')) + " " + getResource('allowed') + " \n";
        }
        defs += "       " + $($('.level3[id=' + obj.idL3 + '] > a > span')[0]).text();
        defs += " - " + obj.defeitos + " " + getResource('defects') + "\n";
    });
    $('#DescriptionFailure').val(defs);
}