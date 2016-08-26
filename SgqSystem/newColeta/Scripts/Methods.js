$(document).ready(function () {
    //PeriodHTMLDAO.createTable();
    //PeriodHTMLDAO.selectTable(list);
    
});

function initialLogin(user)
{
    $('.App').attr('userid', user.attr('userid'));
    $('footer .user').text(user.attr('username'));
    showLevel01();
}

$(document).on('click', '#btnLogin', function (e) {
    e.preventDefault();
    getlogin($('#inputUserName').val(), $('#inputPassword').val(), initialLogin);
    //auth();
});
$(window).bind('beforeunload', function () {
    return "Warning! If you proceed you will loose information";
});

$(document).on('click', '.level01List .level01', function (e) {

    var level01 = $(this);
    var itensOk = checkInputsSelect();
    if (itensOk == false) {
        return false;
    }
    if (!$(this).attr('completed') && $(this).attr('reaudit') && !$(this).attr('startreaudit'))
    {
        openMessageModal("Pending re-audit", 'Please, complete re-audit prior to move to next audit');
        return false;
    }
    var totalDefects = 0;

    var attrReaudit = "[reaudit=false]";
    var attrReauditNumber = "";

    var reauditNumber = 0;
    var reaudit = level01.attr('reaudit');
    if (reaudit == "reaudit") {
        if (level01.attr('reauditnumber')) {
            reauditNumber = parseInt(level01.attr('reauditnumber'));
            if (level01.attr('startreaudit')) {
                reauditNumber++;
            }
            if(reauditNumber > 0)
            {
                attrReaudit = "[reaudit=true]";
                attrReauditNumber = "[reauditnumber=" + reauditNumber + "]";
            }
        }

    }

    var resultsLevel01 = $('.level01Result[date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + '][level01id=' + level01.attr('id') + ']' + attrReaudit + attrReauditNumber + ':last');

    level02Reset($('.level02Group[level01id=' + level01.attr('id') + '] .level02'));

    if (level01.attr('getresultlevel01')) {


        $("span.setsDone").text((resultsLevel01.attr('totalevaluate')) ? resultsLevel01.attr('totalevaluate') : "0");

       
        $("span.sideWithErrors").text((resultsLevel01.attr('sidewitherros')) ? resultsLevel01.attr('sidewitherros') : "0").parents('.labelPainel').removeClass('red');
        $("span.more3Defects").text((resultsLevel01.attr('more3Defects')) ? resultsLevel01.attr('more3Defects') : "0").parents('.labelPainel').removeClass('red');
        $("span.setAtual").text((resultsLevel01.attr('lastevaluate')) ? resultsLevel01.attr('lastevaluate') : "1");
        $("span.sideAtual").text((resultsLevel01.attr('lastsample')) ? resultsLevel01.attr('lastsample') : "1");

        var sideWithErros = parseInt($("span.sideWithErrors:first").text());
        if (sideWithErros > 5)
        {
            $("span.sideWithErrors").parents('.lavelPainel').addClass('red');
        }
        var more3Defects = parseInt($("span.more3Defects:first").text());
        if (more3Defects > 0)
        {
            $("span.more3Defects").parents('.labelPainel').addClass('red');
        }

        var level02 = $('.level02Group[level01id=' + level01.attr('id') + '] .level02');

        //level02.removeAttr('completed').parents('li').removeClass('bgCompleted');
        if(resultsLevel01.attr('completed'))
        {
            level02.attr('completed', 'completed');
            level02Complete(level02);
        }
    }
    else
    {

        resultsLevel01.each(function (e) {

            var level01Result = $(this);
            var results = level01Result.children('.level02Result');
            if (level01Result.attr('biasedunbiased'))
            {
                $('#biasedUnbiased').val(level01Result.attr('biasedunbiased'));
            }

            results.each(function (e) {

                var result = $(this);
                var level02 = $('.level02Group[level01id=' + results.attr('level01id') + '] .level02[id=' + result.attr('level02id') + ']');
                level02.attr('defects', result.attr('defects')).attr('completed', 'completed');
                level02.parents('.row').children('.userInfo').children('div').children('.defects').text(level02.attr('defects'));
                totalDefects = totalDefects + parseInt(level02.attr('defects'));
                //esses results podem ficar no level01
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

                result.children('.level03Result').each(function (e) {
                    level02.attr('level03' + $(this).attr('id'), $(this).attr('value'))
                });

                $('.totalDefects').text(totalDefects);
                level02.addClass('selected');
                defectLimitCheck();
                if (!level02.attr('havephases'))
                {
                    level02Complete(level02);
                }
                level02.removeClass('selected');
            });

            //var level01 = $('.level01[id=' + level01Result.attr('level01id') + ']');
            //if (level01Result.attr('completed')) {
            //    level01.addClass('selected');
            //    $('.level02Group[level01id=' + level01Result.attr('level01id') + '] .level02Confirm').click();
            //}
        });
    }

    showLevel02($(this));
});

$(document).on('click', '.level02List .level02', function (e) {
    showLevel03($(this));
});

$(document).on('click', '.btnCA', function (e) {

    $(this).addClass('caLevel02').addClass('hide');
    correctiveActionOpen();
    //  $('.level01List').addClass('hide').hide();
    // $('.level02List').addClass('hide').hide();
    // $('.level03List').addClass('hide').hide();
    // $('.breadcrumb').addClass('hide').hide();

    //$('.correctiveaction').removeClass('hide').show();
    //$(this).addClass('hide');

 

});

function correctiveActionOpen(level01Id, date, shift, period)
{
    
    level01Id = level01Id ? level01Id : $('.level01.selected').attr('id');
    date = date ? date : $('.App').attr('date');
    shift = shift ? shift : $('.App').attr('shift');
    period = period ? period : $('.App').attr('period');

    var level01 = $('#' + level01Id + '.level01');
    var correctiActionSelected = $('.level01Result[level01id=' + level01Id + '][date=' + date + '][shift=' + shift + '][period=' + period + '][havecorrectiveaction]');

    var correctiveActionModal = $('#correctiveActionModal');

    correctiveActionModal.attr('unidadeid', correctiActionSelected.attr('unidadeid'))
                         .attr('auditorid', $('.App').attr('userid'))
                         .attr('date', correctiActionSelected.attr('date'))
                         .attr('shift', correctiActionSelected.attr('shift'))
                         .attr('period', correctiActionSelected.attr('period'));

    $('#CorrectiveActionTaken').children('#datetime').text(dateTimeFormat());
    $('#CorrectiveActionTaken').children('#auditor').text('Admin');
    $('#CorrectiveActionTaken').children('#shift').text($('#shift option[value=' + shift + ']').text());
    $('#AuditInformation').children('#auditText').text(level01.children('.levelName').text());
    $('#AuditInformation').children('#starttime').text(correctiActionSelected.attr('datetime'));
    $('#AuditInformation').children('#correctivePeriod').text($('#selectPeriod option[value=' + period + ']').text());
    

    $('.overlay').show();
    $('body').addClass('overflowNo');
    correctiveActionModal.removeClass('hide');

 //   var level01Id = parseInt($('.level01.selected').attr('id'));

    if (!$('.level01.selected').length) {

        level01Id = parseInt($('.btnCorrectiveAction.selected').parents('.row').children('.level01').attr('id'));
    }

    var description = "";

     correctiActionSelected.children('.level02Result[havecorrectiveaction]').each(function (e) {

    

        var level02 = $('#' + $(this).attr('level02id') + '.level02');


        var level02Id = parseInt(level02.attr('id'));
        var level02Name = level02.children('span.levelName').text();
        var level02errorlimit = parseInt(level02.attr('levelerrorlimit'));

        description = description + level02Name + " error limit = " + level02errorlimit;

        $(this).children('.level03Result').each(function (e) {

            var level03 = $('#' + $(this).attr('level03id') + '.level03');;

            var level03Defects = $(this).attr('value');
            var level03Id = parseInt(level03.attr('id'));
            var level03Name = level03.children('.row').children('div').html();

            if (level03.children('.row').children('div').children('span.response').length) {
                if (level03Defects == 1) {
                    level03Defects = 0
                }
                else {
                    level03Defects = 1;
                }
            }

            if (level03Defects >= level02errorlimit && level03Defects > 0) {

                description = description + "\n" + level03Name + ": " + level03Defects + " Defects";
            }
        });

        description = description + "\n\n";

    });


    $("#DescriptionFailure").val(description);

    correctiveActionModal.fadeIn("fast");

}
function auth() {
    var userTest = { Name: $("#inputUserName").val(), Password: $("#inputPassword").val() };
    var urlGetUser = '../api/User';

    $.ajax({
        type: "POST",
        dataType: "json",
        url: urlGetUser,
        data: userTest,
        success: function (data) {
            if (!!data.MensagemErro) {
                Geral.exibirMensagemErro(data.MensagemErro);
            }
            if (!!data.MensagemAlerta) {
                Geral.exibirMensagemAlerta(data.MensagemAlerta);
            }
            if (!!data.MensagemExcecao) {
                Geral.exibirMensagemErro(data.MensagemExcecao);
            }
            if (!!data.Retorno) {

                if ($('#shift').val() > parseInt('0')) {
                    $('.App').attr('userid', data.Retorno.Id);
                    //$('.App').attr('userName', data.Retorno.Name);
                    $('.App').attr('logintime', dateTimeFormat());
                    $('.App').attr('shift', $('#shift').val());
                    $('.App').attr('shiftname', $('#shift option:selected').text());
                    $('.shift').text($('.App').attr('shiftname'));
                    $('.atualDate').text(dateTimeFormat());
                    $('.userName').text(data.Retorno.Name);
                    Geral.esconderMensagem(container);
                    showLevel01();

                } else {
                    Geral.exibirMensagemErro('Select the shift.');
                }
            }
            if (!!data.listRetorno && data.listRetorno.length > 0) {
                Geral.exibirMensagemAlerta(data.listRetorno);
            }
        },
        error: function (error) {
            return false;
        },
        complete: function () {
            console.log('Aways Execute this one "Complete"');
        }
    });

}
function correctiveActionSignature()
{
      
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

    var data = ("0" + month).slice(-2)  + ("0" + day).slice(-2) + year;
    return data;
    
}
function dateTimeFormat() {
    var date = new Date();
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var day = date.getDate();
    var hour = date.getHours();
    var minute = date.getMinutes();
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
function showLevel01() {
    $('.login').fadeOut("fast", function (e) {
        $('.login').addClass('hide');

        //implementar no login
        var loginTime = new Date();
        $('#selectPeriod').val("0");
        $('.App').attr('shift', $('#shift option:selected').val()).attr('userid', '1');
        $('.App').attr('date', dateReturn());
        $('.App').attr('logintime', dateTimeFormat());
        $('.App').attr('unidadeid', '1');
        $('.atualDate').text($('.App').attr('logintime'));
        $('.App').attr('monitorid', '1');
        configureLevel01();
        level02Reset($('.level02List .level02Group .level02'));
        $('.App').removeClass('hide').show();
        breadCrumb();
        $('.alert').addClass('hide');

        //PeriodHTMLDAO.appendHTML();
        loginFile();

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

        var level01 = $(this);
        var level01geraReaudit = $('.level01Result[level01id=' + $(this).attr('id') + '][shift=' + $('.App').attr('shift') + '][reaudit=false][havereaudit]:last');

        var level01User = level01.siblings('.userInfo').children('div');

        level01User.children('.btnReaudit').children('.reauditPeriod').text($('#selectPeriod option[value=' + level01geraReaudit.attr('period') + ']').text());

        var totalReaudits = 0;
        if (level01geraReaudit.attr('totalreaudits'))
        {
            totalReaudits = parseInt(level01geraReaudit.attr('totalreaudits'));
        }


        level01User.children('.reauditCount').children('button').text(totalReaudits + '/' + level01.attr('minreauditnumber'));

        //level01User.children('.reauditCount');
    });
}

function showLevel02(level01) {

    $('.level01').removeClass('selected');
    $('.painel .labelPainel').addClass('hide');
    level01.addClass('selected');

    $('.level01List').fadeOut("fast", function (e) {
        level01.parents('.level01List').addClass('hide');

        if (level01.attr('startreaudit'))
        {
            $('span.auditReaudit').children('.name').text('Re-audit');
        }
        else
        {
            $('span.auditReaudit').children('.name').text('Audit').siblings('.reauditPeriod').text("");
        }
        var level02 = $('.level02List');
        level02.removeClass('hide').show();
        $('.level02Group').addClass('hide');

        var level02Group = level02.children('.level02Group[level01id=' + level01.attr('id') + ']');

        level02Group.removeClass('hide');

        level02ButtonSave(level02Group);

        $('.level02:visible').parents('li').children('.row').children('.level02').children('.icons').children('.areaComplete, .areaNotComplete').addClass('hide').siblings('.areaNotComplete').removeClass('hide');

        //var level02Complete = $()
        $('.level02[completed]').parents('li').children('.row').children('.level02').children('.icons').children('.areaComplete').removeClass('hide').siblings('.areaNotComplete').addClass('hide');
        $('.level02[completed]').parents('li').children('.row').children('.userInfo').children('div').children('.na').addClass('hide').siblings('.btnAreaSave').addClass('hide');
        //$('.level02[update]').removeAttr('update');

        // $('.level02[completed]').parents('li').children('.row').children('.userInfo').children('div').children('.areaComplete').removeClass('hide').siblings('.areaNotComplete, .na').addClass('hide').siblings('.btnAreaSave').addClass('hide');
        $('.level02[notavaliable]').parents('li').children('.row').children('.level02').children('.icons').children('.areaComplete').removeClass('hide').siblings('.areaNotComplete').addClass('hide');
        $('.level02[notavaliable]').parents('li').children('.row').children('.userInfo').children('div').children('.na').addClass('naSelected').removeClass('hide').siblings('.btnAreaSave').addClass('disabled').removeClass('hide');

        //$('.level02[notavaliable]').parents('li').children('.row').children('.userInfo').children('div').children('.btnAreaSave').addClass('hide').siblings('.btnNotAvaliable').addClass('hide').siblings('.btnReaudit').addClass('hide')
        $('.level02[reaudit]').parents('li').children('.row').children('.userInfo').children('div').children('.na').addClass('hide');

        $('.level02[startphasedate]').parents('li').children('.row').children('.userInfo').children('div').children('.na').removeClass('hide').siblings('.btnAreaSave').removeClass('hide');;

        btnCorrectiveAction();

        $('.painel .labelPainel[level01id=' + level01.attr('id') + ']').removeClass('hide');
        $('.iconReturn').removeClass('hide');
        buttonsLevel02Show(level01);
        breadCrumb($('.level01List .selected').text());
    });
}


function level02Configure() {
    $('.level02[completed]').parents('li').addClass('bgCompleted');
    $('.level02[limitExceeded]').parents('li').addClass('bgLimitExceeded');


}
function buttonsLevel02Show(level01) {
    $('.buttonMenu[level01id=' + level01.attr('id') + ']').removeClass('hide');
    level02ButtonSave($('.level02Group[level01id=' + level01.attr('id') + ']'));
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
    if ($('#selectCattleType:visible option:selected').val() == "0")
    {
        message =  message + "Select the cattle type<br>";
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
    if (itensOk == false)
    {
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
    if (level02.attr('consecutivefailure'))
    {
        $('.painelLevel03 .consecutiveFailure').text(level02.attr('consecutivefailure'));
    }
    $('.level02List').fadeOut("fast", function (e) {
        level02.parents('.level02List').addClass('hide');

        var level03 = $('.level03List');
        level03.removeClass('hide').show();
        $('.level03Group').addClass('hide');
        $('.painel .defects').text(level02.attr('defects'));
        defectLimitCheck();

        level03.children('.level03Group[level01id=' + $('.level01.selected').attr('id') + ']').removeClass('hide');

        breadCrumb($('.level01List .selected').text(), $('.level02List .level02.selected span.levelName').text());
        $('#btnSave').removeClass('hide');

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
        $(this).addClass('hide');
        $('.btnAreaSaveConfirm:visible').addClass('hide').siblings('.btnAreaSave').removeAttr('disabled');
    }
    $('.defects').removeClass('hide');
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
$(document).on('click', '#btnSync', function (e) {
    Sync();
});
$(document).on('click', '#btnLogout', function (e) {
    $('.App').fadeOut("fast", function (e) {
        $('.login').removeClass('hide').show();
        $('#inputUserName').val("").focus();
        $('#inputPassword').val("");
        $("#shift").val("0").change();
        initializeApp();
    });
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
        btnCorrectiveAction();
        buttonsLevel02Show(level01);
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
function correctiveActionSignatureModalOpen()
{
    $('.alert').addClass('hide');
    var correctiveActionModal = $('#correctiveActionModal');
    correctiveActionModal.css('z-index', '9997');
    var correctiveActionModalSignature = $('#modalSignatureCorrectiveAction');
    $('#modalSignatureCorrectiveAction input').val("");
    correctiveActionModalSignature.fadeIn("fast", function (e) {
        $('#modalSignatureCorrectiveAction input:first').focus();
    });
}
$(document).on('input', '.level03Group[level01id=3] input', function (e) {
    inputChangesUpdate($(this));
    level03AlertAdd($(this));
});
$(document).on('input', '.level03Group[level01id=6] input', function (e) {
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
    $('.painel .defects').text(level02Defects);
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
    defectLimitCheck();


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
    btnNA.addClass('hide');
   


    if (defectsLevel02 > defectsLimit) {
        defectsDiv.addClass('red');
        level02.attr('limitExceeded', 'limitExceeded');
        level02.parents('li').addClass('bgLimitExceeded');
        if (level02.parents('.level02Group').is(':visible'))
        {
            $('.btnCA').addClass('hide');
        }
    }
    else {
        defectsDiv.removeClass('red');
        level02.removeAttr('limitExceeded');
        level02.parents('li').removeClass('bgLimitExceeded');
        if (defectsLevel02 == 0 && !level02.attr('reaudit')) {
            btnNA.removeClass('hide');
        }
    }

}
$(document).on('click', '.modal-close-ca', function (e) {
    $('body').removeClass('overflowNo');
    var ca = $(this).parents('.modal-padrao');
    $('#correctiveActionModal .modal-body .row').scrollTop("0");

    ca.fadeOut("fast", function(e){

        $(this).addClass('hide');
        $('.overlay').hide();
        if($('.btnCA').hasClass('caLevel02'))
        {
            $('.btnCA').removeClass('hide').removeClass('caLevel02');
        }
    });
});


$(document).on('click', '.modal-close-signature', function (e) {
    $('#correctiveActionModal').css('z-index', '9999');
    var ca = $(this).parents('.modal-padrao');
    //$('#correctiveActionModal .modal-body .row').scrollTop("0");

    ca.fadeOut("fast", function(e){

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
$(document).on('click', '.level03Group[level01id=2] .level03', function (e) {

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
            if ($('.level03Group[level01id=2] .level03[notConform]').length == 0) {
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

    if ($(this).hasClass('disabled')) {
        return false;
    }
    var level02 = $(this).parents('li').children('.row').children('.level02');
    level02.addClass('selected');
    //buscar pelo atributo e nao pelo botao
    if (level02.siblings('.userInfo').children('div').children('.btnPhase').is(':visible'))
    {

    }

    $('.level03Group[level01id=' + level02.parents('.level02Group').attr('level01id') + ']').children('.level03Confirm').click();
    $('.overlay').hide();
    $(this).siblings('.defects').removeClass('hide');

});

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
        level.removeAttr('completed').removeAttr('correctivaction').removeAttr('reaudit').removeClass('reauditnumber').parents('li').removeClass('bgLimitExceeded').removeClass('bgCompleted');

    });


}

function level02Reset(level02) {

    var level02Group = level02.parents('.level02Group');

    level02Level03Reset(level02Group);

    //CCA
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .totalDefects').text('0');
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .defects').text('0');
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] #selectCattleType').val('0');
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] #inputChainSpeed, .labelPainel[level01id=' + level02Group.attr('level01id') + '] #inputLotNumber, .labelPainel[level01id=' + level02Group.attr('level01id') + '] #inputMudScore').val("");

    //CFF
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .setsDone').text('0');
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .sideWithErrors').text('0').parents('.labelPainel').removeClass('red');
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .more3Defects').text('0').parents('.labelPainel').removeClass('red');
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .setAtual').text('1');
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .sideAtual').text('1');
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .sideErros').text('0');


    $('span.auditReaudit').children('.name').text('Audit').siblings('.reauditPeriod').text("");


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

    $('.level03Group[level01id=' + level02Group.attr('level01id') + '] .level03').each(function (e) {

        $('.level02Group[level01id=' + level02Group.attr('level01id') + '] .level02').removeAttr('level03' + $(this).attr('id'));
    });

    $('.level03Group[level01id=' + level02Group.attr('level01id') + '] .level03').removeClass('lightred').removeAttr('notconform');
    $('.level03Group[level01id=' + level02Group.attr('level01id') + '] .level03 .row div span.response').attr('value', '0').text('Yes');
}

function level02Complete(level02) {

    level02.each(function (e) {

        var level = $(this);

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
        level02ButtonSave(level.parents('.level02Group'));
        if (level02.attr('havephases'))
        {
            if(level02.attr('completed') && level02.attr('limitexceeded'))
            {
                botaoSalvarLevel02.removeClass('hide');
                botaoNa.removeClass('hide');
            }
        }

        //nao precisamos verificar os defeitos se o limitexceeded estiver 
        var defectsLevel02 = parseInt($('.painelLevel03 .defects').text());
        var defectsLimit = parseInt(level.attr('levelerrorlimit'));
    });
    //fazer uma funcao para melhorar    
}
function level02ButtonSave(level02Group) {
    var level01 = $('.level01[id=' + level02Group.attr('level01id') + ']');

    //level01.attr('saveLevel02') && level02Group.children('.row').children('.level02[complete]')
    //activ button save - fazer uma regra
    if(level01.attr('saveLevel02') && level02Group.is(':visible') && (level02Group.children('li').children('.row').children('.level02[completed!=completed]').length ==  0 || (level01.attr('update') && level01.attr('completed'))))
    {
        $('#btnSave').removeClass('hide');
    }
}
$(document).on('click', '.level02Group[level01id=3] .level02', function (e) {

    var level01 = $('.level01.selected');
    var level02 = $('.level02.selected');
    $('.level03Group[level01id=3] .level03 input').val(0).parents('li').removeClass('bgAlert');

    var attrReaudit = "[reaudit=false]";
    var attrReauditNumber = "";

    var reauditNumber = 0;
    var reaudit = level01.attr('reaudit');
    if (reaudit == "reaudit") {
        if (level01.attr('reauditnumber')) {
            reauditNumber = parseInt(level01.attr('reauditnumber'));
            if (level01.attr('startreaudit')) {
                reauditNumber++;
            }
            if (reauditNumber > 0) {
                attrReaudit = "[reaudit=true]";
                attrReauditNumber = "[reauditnumber=" + reauditNumber + "]";

            }
        }
    }

    var resultsLevel01 = $('.level01Result[date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + ']' + attrReaudit + attrReauditNumber);
    if(!resultsLevel01.length && !level01.attr('startreaudit'))
    {
        var resultsLevel01 = $('.level01Result[date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + ']:last');
    }

    var level02Saved = resultsLevel01.children('.level02Result[level02id=' + level02.attr('id') + ']');

    level02Saved.children('.level03Result').each(function (e) {
        var input = $('.level03Group[level01id=3] li#' + $(this).attr('level03id') + '.level03 input');
        input.val($(this).attr('value'));
        var valor = parseInt(input.val());
        if (valor > 0) {
            input.parents('li').addClass('bgAlert');
        }
    });
});
$(document).on('click', '.level02Group[level01id=2] .level02', function (e) {

    var level01 = $('.level01.selected');
    var level02 = $('.level02.selected');
    var level02Group = level02.parents('.level02Group');

    $('.level03Group[level01id=2] .level03 span.response').text('Yes').attr('value', '1');
    $('.level03Group[level01id=2] .level03').removeClass('lightred').removeAttr('notconform');
    $('.level03Group[level01id=2]').children('div').children('.button-collapse').click();

    //var auditReauditLabel = $('.painelLevel03 .labelPainel[level01id=' + level02Group.attr('level01id') + '] .auditReaudit');
    //if (level02.attr('startReaudit')) {
    //    auditReauditLabel.text('Reaudit');
    //}
    //else {
    //    auditReauditLabel.children('.name').text('Audit').siblings('.reauditPeriod').text("");
    //}

    //aqui vai a verificação da reauditoria
    var attrReaudit = "[reaudit=false]";
    var attrReauditNumber = "";


    var reauditNumber = 0;
    var reaudit = level02.attr('reaudit');
    if (reaudit == "reaudit") {
        if (level02.attr('reauditnumber')) {
            reauditNumber = parseInt(level02.attr('reauditnumber'));
            if (level02.attr('startreaudit')) {
                reauditNumber++;
            }
            if (reauditNumber > 0) {
                attrReaudit = "[reaudit=true]";
                attrReauditNumber = "[reauditnumber=" + reauditNumber + "]";

            }
        }
    }

    var resultsLevel01 = $('.level01Result[date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + ']' + attrReaudit + attrReauditNumber);
    if (!resultsLevel01.length && !level02.attr('startreaudit')) {
        var resultsLevel01 = $('.level01Result[date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + ']:last');
    }

    var level02Saved = resultsLevel01.children('.level02Result[level02id=' + level02.attr('id') + ']' + attrReaudit  + attrReauditNumber);

    level02Saved.children('.level03Result').each(function (e) {

        var valueResponse = $(this).attr('conform');
        var response = $('.level03Group[level01id=2] .level03[id=' + $(this).attr('level03id') + '] span.response');

        if(valueResponse == "false")
        {
            response.click();
        }

    });


    setTimeout(function (e) {
        $('.level03[notconform]').parents('.panel-group').children('.panel').children('.collapse').collapse('show');
    }, 500);


});
function btnCorrectiveAction() {
    if ($('.level02Group:visible .level02[limitexceeded]').length) {
        $('.btnCA').removeClass('hide');
    }
    else if (!$('.level02Group:visible .level02[limitexceeded]').length) {
        $('.btnCA').addClass('hide');
    }
}
$(document).on('click', '#btnSalvarCCA', function (e) {

    var level01 = $('.level01.selected');
    var level02 = $('.level02.selected');

    var consectiveFailureLevel = 0;
    var consecFailureTotal = 0;

    

    var attrReaudit = "[reaudit=false]";
    var attrReauditNumber = "";

    var reauditNumber = 0;
    var reaudit = level01.attr('reaudit');
    if (reaudit == "reaudit") {
        if (level01.attr('reauditnumber')) {
            reauditNumber = parseInt(level01.attr('reauditnumber'));
            if (level01.attr('startreaudit')) {
                reauditNumber++;
            }

            if(reauditNumber > 0)
            {
                attrReaudit = "[reaudit=true]";
                attrReauditNumber = "[reauditnumber=" + reauditNumber + "]";

            }
        }

    }

    var level01Save = $('.level01Result[level01Id=' + level01.attr('id') + '][date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + ']' + attrReaudit + attrReauditNumber);

    if (!level01Save.length) {
        level01Save = $(
                            saveLevel01(level01.attr('id'), $('.App').attr('date'), $('.App').attr('unidadeid'), $('.App').attr('shift'), $('.App').attr('period'), reaudit, reauditNumber)
                        );
    }

    var level02Save = $('.level02Result[level01id=' + level01.attr('id') + '][level02id=' + level02.attr('id') + '][date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + ']' + attrReaudit + attrReauditNumber)
    if (level02Save.length) {
        level02Save.remove();
    }

    level02Save =
                $(saveLevel02(
                              level01.attr('id'),
                              level02.attr('id'),
                              $('.App').attr('unidadeid'),
                              dateReturn(),
                              dateTimeFormat(),
                              $('.App').attr('auditorid'),
                              $('.App').attr('shift'),
                              $('.App').attr('period'),
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

    if (level01.attr('consecutivefailure')) {

        if (level02.attr('consecutivefailurelevel')) {
            consectiveFailureLevel = parseInt(level02.attr('consecutivefailurelevel'));
        }

        if (level02.attr('consecutivefailuretotal')) {
            consecFailureTotal = parseInt(level02.attr('consecutivefailuretotal'));
        }

        var defects = parseInt(level02.attr('defects'));
        var defectsLimit = parseInt(level02.attr('levelerrorlimit'));

        if (level02.attr('completed')) {
            if (defects > defectsLimit) {
                if (consectiveFailureLevel == 0) {
                    consectiveFailureLevel = 1;
                    var level02Last = $('.level02Result[level01id=' + level01.attr('id') + '][level02id=' + level02.attr('id') + '][shift=' + $('.App').attr('shift') + '][period!=' + $('.App').attr('period') + ']:last');

                    var consecFailureLast = 0;
                    if (level02Last.attr('consecutivefailuretotal')) {
                        consecFailureLast = parseInt(level02Last.attr('consecutivefailuretotal'));
                    }
                    consecFailureTotal = consectiveFailureLevel + consecFailureLast;
                }
                level01Save.attr('havecorrectiveaction', 'havecorrectiveaction');
                level02Save.attr('havecorrectiveaction', 'havecorrectiveaction');
            }
            else if (defects <= defectsLimit) {
                consectiveFailureLevel = 0;
                consecFailureTotal = 0;
            }
            //se eu alterei e continua com mais defeitos nao faço nada
            //se eu alterei e nao tem defeitos irei zerar o contador ottal
            //se eu alte+rei e nao tem total de defeitos e estouruo os defietos
            //vreifica o numero de defeitos anterior e icremento mais 1 no defeitos total e 1 no defeito da areax   
        }
        else if (defects > defectsLimit && !level02.attr('completed')) {
            consectiveFailureLevel = 1
            consecFailureTotal++;
            level01Save.attr('havecorrectiveaction', 'havecorrectiveaction');
            level02Save.attr('havecorrectiveaction', 'havecorrectiveaction');
        }
        
        else {
            consecFailureTotal = 0;
            consectiveFailureLevel = 0;
        }

        $('.painelLevel03 .consecutiveFailure').text(consecFailureTotal);
        level02.attr('consecutivefailurelevel', consectiveFailureLevel);
        level02.attr('consecutivefailuretotal', consecFailureTotal);
        level02.parents('.row').children('.userInfo').children('div').children('.consecutiveFailure').text(consecFailureTotal);
    }

    

    $('.level03Group[level01id=3] .level03 input').each(function (e) {

        var level03 = $(this).parents('.level03');
        //level02.attr('level03' + level03.attr('id'), $(this).val());

        //verificar para saber quando tem conformidade e nao conformidade

        var conform = true;
        if (parseInt($(this).val()) > 0) {
            conform = false;
        }
        var level03Save = $(saveLevel03(
                                       level03.attr('id'),
                                       $(this).val(),
                                       conform,
                                       $('.App').attr('auditorid'),
                                       null
                                     ));

        appendDevice(level03Save, level02Save);

    });
    $('.level03Group[level01id=3] .level03 input').val('0').parents('li').removeClass('bgAlert');

    appendDevice(level02Save, level01Save);

    appendDevice(level01Save, $('.Results'));


    //testar quando editar para nao ter erro o que acontece com os atributos
    if (consecFailureTotal == 3) {
        level02.attr('reaudit', 'reaudit').attr('havereaudit', 'havereaudit');
        level02.parents('.row').children('.userInfo').children('.pull-right').children('.btnReaudit').removeClass('hide');
    }

    level02Complete(level02);
    level02.removeClass('selected');

    if ($('.breadcrumb li a').length > 1) {
        $('.breadcrumb li a:last').click();
    }
    else {
        showLevel02(level01);
    }
    //PeriodHTMLDAO.insertHTML($('.Results').html());

    createFileResult();

});



//function saveLevel01(Level01Id, date, shift, period, totalSets, totalSides, atualSet, atualSide, totalErros) {
//    return "<div class='level01Result' level01Id='" + Level01Id + "' date='" + date + "' shift='" + shift + "' period='" + period + "' totalSets='" + totalSets + "' totalSides='" + totalSide + "' atualSet='" + atualSet + "' atualSide='" + atualSide + "' totalerros='" + totalErros + "'></div>";
//}
function saveLevel01(Level01Id, date, unidadeId, shift, period, reaudit, reauditNumber
                    , totalEvaluate, sidesWithErros, more3Defects, lastEvaluate, lastSample, biasedUnbiased, evaluate) {

    if (reaudit == "reaudit")
    {
        reaudit = true
    }
    else
    {
        reaudit = false;
    }

    reauditNumber = reauditNumber ? reauditNumber : 0;
    totalEvaluate = totalEvaluate ? totalEvaluate : 0;
    sidesWithErros = sidesWithErros ? sidesWithErros : 0;
    more3Defects = more3Defects ? more3Defects : 0;
    lastEvaluate = lastEvaluate ? lastEvaluate : 0;
    lastSample = lastSample ? lastSample : 0;
    biasedUnbiased = biasedUnbiased ? biasedUnbiased : 0;
    evaluate = evaluate ? evaluate : 1;
    

    return "<div class='level01Result' level01Id='" + Level01Id + "' unidadeid='" + unidadeId + "' date='" + $('.App').attr('date') + "' dateTime='" + dateTimeFormat() + "' shift='" + shift + "' period='" + period + "' reaudit='" + reaudit + "' reauditNumber='" + reauditNumber + "' totalevaluate='" + totalEvaluate + "' sidewitherros='" + sidesWithErros + "' more3Defects='" + more3Defects + "' lastevaluate='" + lastEvaluate + "' lastsample='" + lastSample + "' biasedunbiased='" + biasedUnbiased + "' evaluate='" + evaluate + "'></div>";
}
function saveLevel02(Level01Id, Level02Id, unidadeId, date, dateTime, auditorId, shift, period, evaluate, sample, defects, reaudit, reauditNumber, phase, startPhaseDate, cattleType,
                     chainSpeed, lotNumber, mudScore, consecutivefailureLevel, consecutivefailureTotal, notAvaliabled) {

    if (notAvaliabled == "notavaliable")
    {
        notAvaliabled = true;
    }
    else
    {
        notAvaliabled = false
    }

    if (reaudit == "reaudit") {
        reaudit = true
    }
    else {
        reaudit = false;
    }
    if (reauditNumber == null) {
        reauditNumber = 0;
    }


    return "<div class='level02Result' level01Id='" + Level01Id + "' level02Id='" + Level02Id + "' unidadeId='" + unidadeId + "' date='" + date + "' dateTime='" + dateTime + "' auditorId='" + auditorId
            + "' shift='" + shift + "' period='" + period + "' defects='" + defects + "' reaudit='" + reaudit + "' evaluate='" + evaluate + "' sample='" + sample
            + "' reauditNumber='" + reauditNumber + "' phase='" + phase + "' startPhaseDate='" + startPhaseDate + "' cattletype='" + cattleType + "' chainspeed='" + chainSpeed
            + "' lotNumber='" + lotNumber + "' mudScore='" + mudScore + "' consecutivefailurelevel='" + consecutivefailureLevel + "' consecutivefailuretotal='" + consecutivefailureTotal + "' notavaliable='" + notAvaliabled + "'></div>";
}
function saveLevel03(Level03Id, value, conform, auditorId, totalError) {
    return "<div class='level03Result' level03id='" + Level03Id + "' date='" + dateTimeFormat() + "' value='" + value + "' conform='" + conform + "' auditorId='" + auditorId + "' totalerror='" + totalError + "'></div>";
}
$(document).on('click', '#btnSalvarCFF', function (e) {
    var level01 = $('.level01.selected');
    var level02Group = $('.level03Group:visible');

    var level02Head = $('.level02.selected');

    var level02Head = $('.level02.selected');

    var currentSet = parseInt($('.painelLevel03 .setAtual').text());
    var currentSide = parseInt($('.painelLevel03 .sideAtual').text());

    var totalsets = parseInt(level02Head.attr('totalsets'));
    var totalsides = parseInt(level02Head.attr('totalsides'));
    var sidesperset = parseInt(level02Head.attr('sidesperset'));

    var returnlevel02endset = level02Head.attr('returnlevel02endset');

    var setsDone = parseInt($('.painelLevel02 .setsDone').text());

    level02Group.removeAttr('firstErrorSide');
    level02Group.removeAttr('Error3MoreSide');    

    var attrReaudit = "[reaudit=false]";
    var attrReauditNumber = "";

    var reauditNumber = 0;
    var reaudit = level01.attr('reaudit');
    if (reaudit == "reaudit") {
        if (level01.attr('reauditnumber')) {
            reauditNumber = parseInt(level01.attr('reauditnumber'));
            if (level01.attr('startreaudit')) {
                reauditNumber++;
            }
            if (reauditNumber > 0) {
                attrReaudit = "[reaudit=true]";
                attrReauditNumber = "[reauditnumber=" + reauditNumber + "]";

            }
        }
    }

    var level01Save = $('.level01Result[level01Id=' + level01.attr('id') + '][date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + '][evaluate=' + currentSet + ']' + attrReaudit + attrReauditNumber);

    if (!level01Save.length) {
        level01Save = $(
                            saveLevel01(level01.attr('id'), $('.App').attr('date'), $('.App').attr('unidadeid'), $('.App').attr('shift'), $('.App').attr('period'), reaudit, reauditNumber, $('.setsDone:first').text(), null, null, null, null, null, currentSet)
                        );
    }

    $('.level03Group[level01id=6] .level02').each(function (e) {

        var level02 = $(this);

        var level02Save = $('.level02Result[level01id=' + level01.attr('id') + '][level02id=' + level02.attr('id') + '][date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + ']' + attrReaudit + attrReauditNumber)
        if (level02Save.length) {
            level02Save.remove();
        }

        var level02Save =
              $(saveLevel02(
                            level01.attr('id'),
                            level02.attr('level02id'),
                            $('.App').attr('unidadeid'),
                            dateReturn(),
                            dateTimeFormat(),
                            $('.App').attr('auditorid'),
                            $('.App').attr('shift'),
                            $('.App').attr('period'),
                            currentSet,
                            currentSide,
                            level02.attr('defects'),
                            level02.attr('reaudit')
                         ));

        level02.parents('.panel').children('div').children('.panel-body').children('.level03').each(function (e) {

            var level03 = $(this);
            var input = level03.children('.row').children('div').children('div').children('input');

            //verificar para saber quando tem conformidade e nao conformidade

            var conform = true;
            if (parseInt($(this).val()) > 0) {
                conform = false;
            }

            var level03Save = $(saveLevel03(
                                           level03.attr('id'),
                                           input.val(),
                                           conform,
                                           $('.App').attr('auditorid'),
                                           null
                                         ));

            appendDevice(level03Save, level02Save);

        });
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

        if (setsDone == totalsets) {
            level01Save.attr('completed', 'completed');
            level02Head.attr('completed', 'completed');
            level02Complete(level02Head);
        }
        $('.painelLevel03 .setAtual').text(currentSet);
        $('.setsDone').text(setsDone);
        //verificar se precisa setsdones
        level01Save.attr('totalevaluate', setsDone);
        //aumentar o set
        defectLimitCheck();
        if (level02Head.attr('limitexceeded'))
        {
            level02Head.attr('havereaudit', 'havereaudit');
            level01Save.attr('havereaudit', 'havereaudit');
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

    var level01 = $('.level01.selected');
    var level02 = $('.level02.selected');

    var attrReaudit = "[reaudit=false]";
    var attrReauditNumber = "";

    var reauditNumber = 0;
    var reaudit = level02.attr('reaudit');
    if (reaudit == "reaudit") {
        if (level02.attr('reauditnumber')) {
            reauditNumber = parseInt(level02.attr('reauditnumber'));
            if (level02.attr('startreaudit')) {
                reauditNumber++;
            }

            if (reauditNumber > 0) {
                attrReaudit = "[reaudit=true]";
                attrReauditNumber = "[reauditnumber=" + reauditNumber + "]";

            }
        }

    }

    var level01Save = $('.level01Result[level01Id=' + level01.attr('id') + '][date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + ']');

    if (!level01Save.length) {
        level01Save = $(
                            saveLevel01(level01.attr('id'), $('.App').attr('date'), $('.App').attr('unidadeid'), $('.App').attr('shift'), $('.App').attr('period'), null, null, null, null, null, null, null, $('#biasedUnbiased option:selected').val()))
                        ;
    }

    var level02Save = $('.level02Result[level01id=' + level01.attr('id') + '][level02id=' + level02.attr('id') + '][date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + ']' + attrReaudit + attrReauditNumber)
    if (level02Save.length) {
        level02Save.remove();
    }

    if (level02.attr('havephases') && !level02.attr('phase'))
    {
        level02.attr('phase', '0');
    }

    level02Save =
                $(saveLevel02(
                              level01.attr('id'),
                              level02.attr('id'),
                              $('.App').attr('unidadeid'),
                              dateReturn(),
                              dateTimeFormat(),
                              $('.App').attr('auditorid'),
                              $('.App').attr('shift'),
                              $('.App').attr('period'),
                              null,
                              null,
                              level02.attr('defects'),
                              reaudit,
                              reauditNumber,
                              level02.attr('phase'),
                              dateTimeFormat(),
                              $('#selectCattleType').val(),
                              $('#inputChainSpeed').val(),
                              $('#inputLotNumber').val(),
                              $('#inputMudScore').val(),
                              level02.attr('consecutivefailurelevel'),
                              level02.attr('consecutivefailuretotal'),
                              level02.attr('notavaliable')
                           ));


    var reauditNumber = reauditCount(level02);

    $('.level03Group[level01id=2] .level03 span.response').each(function (e) {

        var level03 = $(this).parents('.level03');

        var conform = true;
        if (parseInt($(this).attr('value')) == 0) {
            conform = false;
        }

        var level03Save = $(saveLevel03(
                                       level03.attr('id'),
                                       $(this).val(),
                                       conform,
                                       $('.App').attr('auditorid'),
                                       null
                                     ));
                                
        appendDevice(level03Save, level02Save);                            
        // level02Save.append(level03Save);
    });

    appendDevice(level02Save, level01Save);
    appendDevice(level01Save, $('.Results'));

    var phase = 0;

    if (level02.attr('phase')) {
        phase = parseInt(level02.attr('phase'))
    }

    if (level02.attr('limitexceeded')) {

        if (!level02.attr('completed')) {
            phase = checkFase(phase);
        }
        level02.attr('startphasedate', dateTimeFormat());
        level02.attr('correctivaction', 'correctivaction').attr('reaudit', 'reaudit').attr('reauditNumber', reauditNumber);
        level02.parents('.row').children('.userInfo').children('div').children('.btnReaudit').removeClass('hide').siblings('.reauditCount').removeClass('hide');
    }
    if (level02.attr('startreaudit')) {
        // phase = checkFase(phase);
        level02.attr('reaudit', 'reaudit').attr('reauditNumber', reauditNumber).removeAttr('startreaudit');
        level02.parents('.row').children('.userInfo').children('div').children('.reauditCount').children('button').text(reauditNumber);
    }


    var btnPhase = level02.parents('.row').children('.userInfo').children('div').children('.btnPhase');
    if (phase > 0) {

        //var reauditNumber = 0;
        btnPhase.removeClass('hide');
        btnPhase.children('button').children('.atualPhase').text(phase);
       
        var countPhase = level02.siblings('.userInfo').children('div').children('.reauditCount');

        var phaseConfiguation = $('.phasesreaudits .phase[number=' + phase + ']');

        if (phaseConfiguation.attr('reaudits') == "0" || phaseConfiguation.attr('reaudits') == "")
        {
            level02.siblings('.userInfo').children('div').children('.reauditCount').addClass('hide');
        }
        else
        {
            if(parseInt(level02.attr('phase')) != phase)
            {
                reauditNumber = 0;
                btnPhase.siblings('.btnAreaSave').removeClass('hide').siblings('.btnNotAvaliable').removeClass('hide');
            }

            if (reauditNumber == parseInt(phaseConfiguation.attr('reaudits')) && !level02.attr('limitexceeded'))
            {
                openMessageModal('Phase Completed', level02.children('.levelName').text() + " returned to phase 0");
                //reauditNumber = 0;
                phase = 0;
                level02.removeAttr('phase');
                btnPhase.addClass('hide');
                countPhase.addClass('hide');
            }
            else
            {
                countPhase.removeClass('hide').children('button').text(reauditNumber + '/' + phaseConfiguation.attr('reaudits'));

            }
            //verificar em qual faze estou
            //verificar se ja tem uma contagem de pahse e colocar o vsalor total dela no label

        }
        level02.attr('reauditnumber', reauditNumber);

    }
    level02.attr('phase', phase);

    if (level02.attr('prevphasexceeded')) {
        level02.removeAttr('prevphasexceeded').attr('limitexceeded', 'limitexceeded').parents('li').addClass('bgLimitExceeded');
    }

    level02Complete(level02);
    level02Return(level02);

    $(this).parents('.level03Group').children('div').children('.button-collapse').click();

    createFileResult();

});

function checkFase(phase) {
    if(phase == 0)
    {
        phase = 1
    }
    else if (phase == 1)
    {
        phase = 2;
    }
    else if(phase == 2)
    {
        phase = 3;
    }
    return phase;
}
$(document).on('mousedown', '#btnSave', function (e) {
    var that = $(this);
    that.addClass('disabled');
    setTimeout(function () {
        $('.level02Group:visible .level02Confirm').click();

        $('.level03Group:visible .level03Confirm').click();
        that.removeClass('disabled');
    }, 250);
});

$(document).on('change', 'select#selectPeriod', function (e) {

    level01Reset($('.level01'));
    $('.painelLevel02 select').val(0);
    $('.painelLevel02 input').val("");

    level02Reset($('.level02List .level02Group .level02'));

    $('.App').attr('period', $('#selectPeriod option:selected').val());
    $('span.period').html($("select#selectPeriod:visible :selected").text());

    periodReset();
});
function periodReset() {
    $('.totalDefects').text('0');

    var totalDefects = 0;

    //procurar reauditorias anteriores
    //procurar acoescorretivasanteriores


    var levels01Check = $('.level01List .level01').each(function (e) {
        var level01 = $(this);

        var corretivActionsNoCompleted = $('.level01Result[level01id=' + level01.attr('id') + '][date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][havecorrectiveaction]:last');
        var lastReaudit = $('.level01Result[level01id=' + level01.attr('id') + '][date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][havereaudit]:last');
        var btnReaudit = level01.parents('.row').children('.userInfo').children('div').children('.btnReaudit');



        if (corretivActionsNoCompleted.length) {
            level01.attr('correctivaction', 'correctivaction').parents('.row').children('.userInfo').children('.btnCorrectiveAction').removeClass('hide');
        }
        if (lastReaudit.length) {
            //se for completa verifica se o numero é igual o total de reauditoris se nao for soma -1
            //se a reautoria for igual ao totla de reautoriaas que tem que fazer libera a autoria
            btnReaudit.children('.reauditPeriod').text($('#selectPeriod option[value=' + lastReaudit.attr('period') + ']').text());
            //  btnReaudit.children('.reauditPeriod').text('aaaaaaaaaaaaaaaaaaaaaaa');
            level01.attr('reaudit', 'reaudit').parents('.row').children('.userInfo').children('.btnReaudit').removeClass('hide').siblings('.reauditCount').removeClass('hide');
        }

        var levelResultPeriod = $('.level01Result[level01id=' + level01.attr('id') + '][date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + '][reaudit=false]:last');
        if(levelResultPeriod.length)
        {
            if(levelResultPeriod.attr('completed'))
            {
                level01.attr('completed', 'completed');
            }
        }
    });
    configureLevel01();

}

$(document).on('change', 'select#selectCattleType', function (e) {
    $('span.cattleType').html("<strong>Cattle Type:</strong>" + $("select#selectCattleType :selected").text());
});
$(document).on('input', 'input#inputChainSpeed', function (e) {
    $('span.chainSpeed').html("<strong>Chain Speedy:</strong>" + $(this).val());
});
$(document).on('input', 'input#inputLotNumber', function (e) {
    $('span.lotNumber').html("<strong>Lot #:</strong>" + $(this).val());
});
$(document).on('input', 'input#inputMudScore', function (e) {
    $('span.mudScore').html("<strong>Mud Score:</strong>" + $(this).val());
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

    var level01 = $('.level01.selected');

    var reauditNumber = 0;
    var attrReaudit = "[reaudit=false]";
    
    var level01ResultGeraReaudit;
    if (level01.attr('reaudit'))
    {
        reauditNumber = reauditCount(level01);
        attrReaudit = "[reaudit=true]";
    }


    level01.attr('completed', 'completed');

    //colocar audit e reaudit para pesquisar e pesquisar o audit que gerou a reaudit
    
    var level01Result = $('.level01Result[level01id=' + level01.attr('id') + '][date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + ']' + attrReaudit + ":last");
    if(level01Result.attr('reaudit') == "true")
    {
        level01ResultGeraReaudit = $('.level01Result[level01id=' + level01.attr('id') + '][shift=' + $('.App').attr('shift') + '][havereaudit]');
    }

    //quando tiver reauditoria na reauditoria
    //if(level01Result.attr('havecorrectiveaction'))
    //{

    //}
    //if (level01.attr('reaudit')) {
    //    level01.removeAttr('correctivaction');
    //}
    //else {
    //    level01.removeAttr('correctivaction').removeAttr('reaudit').removeAttr('reauditNumber');
    //}
    
    var totalDefects = parseInt($('.painelLevel02 .totalDefects').text());
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

    level01Result.attr('completed', 'completed');


    if (level01.attr('startreaudit')) {

        level01.attr('reaudit', 'reaudit').attr('reauditNumber', reauditNumber).removeAttr('startreaudit');

        var resultStarReaudit = $('.level01Result[level01id=' + level01.attr('id') + '][shift=' + $('.App').attr('shift') + '][reaudit=false][havereaudit]');
        resultStarReaudit.attr('totalreaudits', reauditNumber);

        //if (reauditNumber == level01.attr('minreauditnumber')) {
        //}

        
        level01.parents('.row').children('.userInfo').children('div').children('.reauditCount').children('button').text(reauditNumber + '/' + level01.attr('minreauditnumber'));
        var minreauditNumber = parseInt(level01.attr('minreauditnumber'));
        if (reauditNumber >= minreauditNumber) {
            level01.removeAttr('reaudit').removeAttr('reauditnumber');
            resultStarReaudit.removeAttr('havereaudit').attr('completereaudit', 'completereaudit');
           
            var level01Buttons = level01.siblings('.userInfo').children('div');

            level01Buttons.children('.btnReaudit').addClass('hide').children('.reauditPeriod').text('');
            level01Buttons.children('.reauditCount').addClass('hide');

            if (resultStarReaudit.attr('period') != $('.App').attr('period')) {
                var resultAtualLevel = $('.level01Result[level01id=' + level01.attr('id') + '][date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + '][reaudit=false]');
                if(resultAtualLevel.length == 0)
                {
                    level01.attr('reauditnumber', '0').removeAttr('reaudit').removeAttr('reauditnumber').removeAttr('startreaudit').removeAttr('completed');
                    periodReset();
                }
            }

            openMessageModal('Re-audit completed', $('#selectPeriod option[value=' + resultStarReaudit.attr('period') + ']').text() + ' re-audit completed!');

        }
    }

    level01Return();



});
$(document).on('click', '#btnSalvarLevel02CFF', function (e) {

    var level01 = $('.level01.selected');

    var reauditNumber = 0;
    var attrReaudit = "[reaudit=false]";

    var level01ResultGeraReaudit;
    if (level01.attr('reaudit')) {
        reauditNumber = reauditCount(level01);
        attrReaudit = "[reaudit=true]";
    }

    level01.attr('completed', 'completed');

    var level01Result = $('.level01Result[level01id=' + level01.attr('id') + '][date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + ']' + attrReaudit + ":last");
    if (level01Result.attr('reaudit') == "true") {
        level01ResultGeraReaudit = $('.level01Result[level01id=' + level01.attr('id') + '][shift=' + $('.App').attr('shift') + '][havereaudit]');
    }

    if (level01.attr('reaudit')) {
        level01.removeAttr('correctivaction');
    }
    else {
        level01.removeAttr('correctivaction').removeAttr('reaudit').removeAttr('reauditNumber');
        level01Result.attr('havecorrectiveaction', 'havecorrectiveaction');
    }

        


    level01Result.attr('completed', 'completed');
    //if (level01.attr('reaudit') && !level01.attr('startreaudit'))
    //{
    //}

    

    if ($('.level02Group[level01id=' + level01.attr('id') + '] .level02[havereaudit]').length) {
        level01Result.attr('havereaudit', 'havereaudit');
        level01.attr('correctivaction', 'correctivaction').attr('reaudit', 'reaudit').attr('reauditNumber', reauditNumber);
    }
    else {
        level01.removeAttr('correctivaction').removeAttr('reaudit');
        level01Result.removeAttr('havereaudit').attr('completereaudit', 'completereaudit');
    }
    //if (level01.attr('startreaudit')) {
    //    level01.attr('reaudit', 'reaudit').attr('reauditNumber', reauditNumber).removeAttr('startreaudit');
    //    level01.parents('.row').children('.userInfo').children('div').children('.reauditCount').children('button').text(reauditNumber);
    //}

    if (level01.attr('startreaudit')) {

        level01.attr('reaudit', 'reaudit').attr('reauditNumber', reauditNumber).removeAttr('startreaudit');

        var resultStarReaudit = $('.level01Result[level01id=' + level01.attr('id') + '][shift=' + $('.App').attr('shift') + '][reaudit=false][havereaudit]');
        resultStarReaudit.attr('reauditNumber', reauditNumber);

        //if (reauditNumber == level01.attr('minreauditnumber')) {
        //}


        level01.parents('.row').children('.userInfo').children('div').children('.reauditCount').children('button').text(reauditNumber + '/' + level01.attr('minreauditnumber'));
        var minreauditNumber = parseInt(level01.attr('minreauditnumber'));
        if (reauditNumber >= minreauditNumber) {
            level01.removeAttr('reaudit').removeAttr('reauditnumber');
            resultStarReaudit.removeAttr('havereaudit').attr('completereaudit', 'completereaudit');

            var level01Buttons = level01.siblings('.userInfo').children('div');

            level01Buttons.children('.btnReaudit').addClass('hide').children('.reauditPeriod').text('');
            level01Buttons.children('.reauditCount').addClass('hide');

            if (resultStarReaudit.attr('period') != $('.App').attr('period')) {
                var resultAtualLevel = $('.level01Result[level01id=' + level01.attr('id') + '][date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + '][reaudit=false]');
                if (resultAtualLevel.length == 0) {
                    level01.attr('reauditnumber', '0').removeAttr('reaudit').removeAttr('reauditnumber').removeAttr('startreaudit').removeAttr('completed');
                    periodReset();
                }
            }

            openMessageModal('Re-audit completed', $('#selectPeriod option[value=' + resultStarReaudit.attr('period') + ']').text() + ' re-audit completed!');

        }
    }

    level01Return();
    //alert('salvar level02 completo');
});
$(document).on('click', '#btnSalvarLevel02HTP', function (e) {

    var level01 = $('.level01.selected');

    var reauditNumber = 0;
    var attrReaudit = "[reaudit=false]";

    var level01ResultGeraReaudit;
    if (level01.attr('reaudit')) {
        reauditNumber = reauditCount(level01);
        attrReaudit = "[reaudit=true]";
    }


    level01.attr('completed', 'completed');

    //colocar audit e reaudit para pesquisar e pesquisar o audit que gerou a reaudit

    var level01Result = $('.level01Result[level01id=' + level01.attr('id') + '][date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + ']' + attrReaudit + ":last");
    if (level01Result.attr('reaudit') == "true") {
        level01ResultGeraReaudit = $('.level01Result[level01id=' + level01.attr('id') + '][shift=' + $('.App').attr('shift') + '][havereaudit]');
    }

    if ($('.level02Group[level01id=' + level01.attr('id') + '] .level02[limitexceeded]').length) {
        level01.attr('correctivaction', 'correctivaction').attr('reaudit', 'reaudit').attr('reauditNumber', reauditNumber);
    }
    if (level01.attr('startreaudit')) {
        level01.attr('reaudit', 'reaudit').attr('reauditNumber', reauditNumber).removeAttr('startreaudit');
        level01.parents('.row').children('.userInfo').children('div').children('.reauditCount').children('button').text(reauditNumber);
    }
    level01Return();

    //alert('salvar level02 completo');
});
$(document).on('click', '.level01List .btnReaudit', function (e) {

    var level01 = $(this).parents('.row').children('.level01');

    if (level01.attr('correctivaction')) {
        //alert('Open Corrective Action');
        var title = "Warning";
        var content = "Fill the Corrective Action first.";
        openMessageModal(title, content);
        return true;
    }

    level01.attr('startReaudit', 'startReaudit').removeAttr('completed');

    //$('.level02List .level02Group[level01id=' + level01.attr('id') + '] .level02').removeAttr('completed').removeAttr('limitexceeded').removeAttr('notavaliable');
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

$(document).on('click', '.level02Group .btnPhase', function (e) {

    var level02 = $(this).parents('.row').children('.level02');
    startReauditOrPhases(level02);
    //nao é o 
    //level02Complete($('.level02List .level02Group[level01id=' + level01.attr('id') +'] .level02'));
    level02.click();
});
function startReauditOrPhases(level02) {
    level02.attr('reaudit', 'reaudit').attr('startReaudit', 'startReaudit').removeAttr('completed');
    level02.attr('defects', '0');
    $('.painelLevel03 .labelPhase').parents('.labelPainel').removeClass('hide');


    $('.painelLevel03 .labelPhase').text(level02.attr('phase'));

    //$('.level02List .level02Group[level01id=' + level01.attr('id') + '] .level02').removeAttr('completed').removeAttr('limitexceeded').removeAttr('notavaliable');
    //level02Reset($('.level02List .level02Group[level01id=' + level01.attr('id') + '] .level02'));
    level02Level03Reset(level02.parents('.level02Group'));
}
$(document).on('click', '.btnCorrectiveAction', function (e) {
    $('.btnCorrectiveAction').removeClass('selected');
    $(this).addClass('selected');
    var level01 = $(this).parents('.row').children('.level01');
    //$(this).addClass('hide');

    // level01.removeAttr('correctivaction');

    correctiveActionOpen(level01.attr('id'));
});

$(document).on('click', '#btnSignatureLogin', function (e) {
    if ($('#modalSignatureCorrectiveAction').attr('signature') == 'slaugther')
        getlogin($('#signatureLogin').val(), $('#signaturePassword').val(), slaugtherSignatureLogin);
    else
        getlogin($('#signatureLogin').val(), $('#signaturePassword').val(), techinicalSignatureLogin);
});
function slaugtherSignatureLogin(user) {
    $('#btnSignatureLogin').siblings('.modal-close-signature').click();
    $('.SlaugtherSignature').removeClass('hide');
    $('.SlaugtherSignature').children('.name').text(user.attr('username'));
    $('.SlaugtherSignature').children('.date').text(dateTimeFormat());
    $('.SlaugtherSignature').attr('userid', user.attr('userid'));
    $('.btnSignature.btnSlaugtherSignature').addClass('hide');
}
$(document).on('click', '#btnTechinicalLogin', function (e) {
    getlogin($('#techinicalLogin').val(), $('#techinicalPassword').val(), techinicalSignatureLogin);
});
function techinicalSignatureLogin(user) {
    $('#btnSignatureLogin').siblings('.modal-close-signature').click();
    $('.TechinicalSignature').removeClass('hide');
    $('.TechinicalSignature').children('.name').text(user.attr('username'));
    $('.TechinicalSignature').children('.date').text(dateTimeFormat());
    $('.TechinicalSignature').attr('userid', user.attr('userid'));
    $('.btnSignature.btnTechinicalSignature').addClass('hide');
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
function openMessageModal(title, content) {
  
    var mensagem = $('.message, .overlay');

    mensagem.children('.head').html(title);
    mensagem.children('.body').html(content);

    mensagem.fadeIn("fast");
    // var $modal = $('#modalMessage');
    // $modal.children('.modal-header').children('.messageHeader').text(title);
    // $modal.children('.modal-body').children('.message').html(content);
    // $modal.modal();
    ////$('.modal-scrollable, .modal-backdrop').removeClass('hide');
}

function appendDevice(obj, appendTo){
    var platform = device.platform;

    if(platform == 'windows'){
        MSApp.execUnsafeLocalFunction(function () {
            appendTo.append(obj);
        });
    }
    else
    {
        appendTo.append(obj);
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
    var auditorid = $('.App').attr('auditorid');
    var date = $('.App').attr('date');
    var shift = $('.App').attr('shift');
    var period = $('.App').attr('period');

    if (descriptionFailure == '')
    {
        message += 'The description of the failure is empty.<br>';
    }
    if ( immediateCorrectiveAction == '')
    {
        message += 'The immediate corrective action is empty.<br>';
    }
    if (productDisposition == '')
    {
        message += 'The product disposition is empty.<br>';
    }
    if($('.SlaugtherSignature').hasClass('hide'))
    {
        message += 'The slaughter signature has not inputed.<br>';
    }
    if($('.TechinicalSignature').hasClass('hide'))
    {
        message += 'The techinical signature has not inputed.<br>';
    }

    if(message != ''){
        openMessageModal('The following errors ocurred: ', message);
        return;
    }

    var level01Result = $('.level01Result[havecorrectiveaction]').attr('correctiveActionComplete', 'correctiveActionComplete').removeAttr('havecorrectiveaction');
    var level02Result = $('.level02Result[havecorrectiveaction]').attr('correctiveActionComplete', 'correctiveActionComplete').removeAttr('havecorrectiveaction');

    $('.correctiveaction').addClass('hide');
    var correctiveActionModal = $(this).parents('.modal-padrao');
    $('.btnCorrectiveAction.selected').addClass('hide').removeClass('selected');

    var corretiveActionResult = $(document.createElement('div'));
    corretiveActionResult.addClass('correctiveAction');
    corretiveActionResult.attr('slaugtherSignature', slaugtherSignature);
    corretiveActionResult.attr('techinicalSignature', techinicalSignature);
    corretiveActionResult.attr('preventativeMeasure', preventativeMeasure);
    corretiveActionResult.attr('unidadeid', unidadeid);
    corretiveActionResult.attr('auditorid', auditorid);
    corretiveActionResult.attr('date', date);
    corretiveActionResult.attr('shift', shift);
    corretiveActionResult.attr('period', period);

    var descriptionSpan = $(document.createElement('div'));
    
    descriptionSpan.addClass('descriptionFailure');
    descriptionSpan.text(descriptionFailure);

    var immediateCorrectiveActionSpan = $(document.createElement('span'));
    immediateCorrectiveActionSpan.addClass('immediateCorrectiveAction');
    immediateCorrectiveActionSpan.text(immediateCorrectiveAction);


    var preventativeMeasureSpan = $(document.createElement('span'));
    preventativeMeasureSpan.addClass('preventativeMeasure');
    preventativeMeasureSpan.text(preventativeMeasure);

    appendDevice(descriptionSpan, corretiveActionResult);
    appendDevice(immediateCorrectiveActionSpan, corretiveActionResult);
    appendDevice(preventativeMeasureSpan, corretiveActionResult);

    appendDevice(corretiveActionResult, level01Result);
    correctiveActionModal.fadeOut("fast", function (e) {
        $('.overlay').hide();
    });

});
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

//                        $("#shift").text('1');

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