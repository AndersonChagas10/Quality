﻿$(document).ready(function () {
    PeriodHTMLDAO.createTable();
    //PeriodHTMLDAO.selectTable(list);
});

$(document).on('click', '#btnLogin', function (e) {
    e.preventDefault();
    showLevel01();
    //auth();
});
$(window).bind('beforeunload', function () {
    return "Warning! If you proceed you will loose information";
});
$(document).on('click', '.level01List .level01', function (e) {
    var itensOk = checkInputsSelect();
    if (itensOk == false) {
        return false;
    }
    if ($(this).attr('reaudit') && !$(this).attr('reaudit'))
    {
        openMessageModal("Pending re-audit", 'Please, complete re-audit prior to move to next audit');
        return false;
    }

    showLevel02($(this));
});

$(document).on('click', '.level02List .level02', function (e) {
    showLevel03($(this));
});
//
//$(document).on('click', '.btnCA', function (e) {
//    $('.level01List').addClass('hide').hide();
//    $('.level02List').addClass('hide').hide();
//    $('.level03List').addClass('hide').hide();
//    $('.breadcrumb').addClass('hide').hide();

//    $('.correctiveaction').removeClass('hide').show();
//    $(this).addClass('hide');
//});
$(document).on('click', '.btnCA', function (e) {
    //  $('.level01List').addClass('hide').hide();
    // $('.level02List').addClass('hide').hide();
    // $('.level03List').addClass('hide').hide();
    // $('.breadcrumb').addClass('hide').hide();

    $('.correctiveaction').removeClass('hide').show();
    $(this).addClass('hide');

    var $modal = $('#correctiveActionModal');

    $modal.modal();

    var level01Id = parseInt($('.level01.selected').attr('id'));

    if (!$('.level01.selected').length) {

        level01Id = parseInt($('.btnCorrectiveAction.selected').parents('.row').children('.level01').attr('id'));
    }

    var description = "";

    $('.level02List .level02Group[level01id=' + level01Id + '] .level02[limitexceeded]').each(function (e) {

        var level02 = $(this);
        var level02Id = parseInt(level02.attr('id'));
        var level02Name = level02.children('span.levelName').text();
        var level02errorlimit = parseInt(level02.attr('levelerrorlimit'));

        description = description + level02Name + " error limit = " + level02errorlimit;

        $('.level03Group[level01id=' + level01Id + '] .level03').each(function (e) {

            var level03 = $(this);

            var level03Defects = level02.attr('level03' + level03.attr('id'));
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

});

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
            alert(mensagem);
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
            alert(mensagem);
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
                alert(mensagem);
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
        $('.atualDate').text($('.App').attr('logintime'));

        configureLevel01();
        level02Reset($('.level02List .level02Group .level02'));
        $('.App').removeClass('hide').show();
        breadCrumb();

        PeriodHTMLDAO.appendHTML();

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
    $('.level01[reaudit]').parents('li').removeClass('bgLimitExceeded').addClass('bgCompleted');
    $('.level01[correctivaction]').parents('li').addClass('bgLimitExceeded').removeClass('bgCompleted');

    $('.level01[reauditNumber]').parents('.row').children('.userInfo').children('div').children('.reauditCount').text($('.level01[reauditNumber]').attr('reauditCount')).removeClass('hide');
}
function showLevel02(level01) {

    $('.level01').removeClass('selected');
    $('.painel .labelPainel').addClass('hide');
    level01.addClass('selected');

    $('.level01List').fadeOut("fast", function (e) {
        level01.parents('.level01List').addClass('hide');

        $('span.auditReaudit').text('Audit');
        if (level01.attr('startreaudit'))
        {
            $('span.auditReaudit').text('Re-audit');
        }

        var level02 = $('.level02List');
        level02.removeClass('hide').show();
        $('.level02Group').addClass('hide');

        var level02Group = level02.children('.level02Group[level01id=' + level01.attr('id') + ']');

        level02Group.removeClass('hide');

        level02ButtonSave(level02Group);

        $('.level02').parents('li').children('.row').children('.level02').children('.icons').children('.areaComplete, .areaNotComplete').addClass('hide').siblings('.areaNotComplete').removeClass('hide');



        //var level02Complete = $()
        $('.level02[completed]').parents('li').children('.row').children('.level02').children('.icons').children('.areaComplete').removeClass('hide').siblings('.areaNotComplete').addClass('hide');
        $('.level02[completed]').parents('li').children('.row').children('.userInfo').children('div').children('.na').addClass('hide').siblings('.btnAreaSave').addClass('hide');
        //$('.level02[update]').removeAttr('update');

        // $('.level02[completed]').parents('li').children('.row').children('.userInfo').children('div').children('.areaComplete').removeClass('hide').siblings('.areaNotComplete, .na').addClass('hide').siblings('.btnAreaSave').addClass('hide');
        $('.level02[notavaliable]').parents('li').children('.row').children('.level02').children('.icons').children('.areaComplete').removeClass('hide').siblings('.areaNotComplete').addClass('hide');
        $('.level02[notavaliable]').parents('li').children('.row').children('.userInfo').children('div').children('.na').addClass('naSelected').removeClass('hide').siblings('.btnAreaSave').addClass('disabled').removeClass('hide');

        //$('.level02[notavaliable]').parents('li').children('.row').children('.userInfo').children('div').children('.btnAreaSave').addClass('hide').siblings('.btnNotAvaliable').addClass('hide').siblings('.btnReaudit').addClass('hide')
        $('.level02[reaudit]').parents('li').children('.row').children('.userInfo').children('div').children('.na').addClass('hide');

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
        $('.overlay').addClass('hide');
        $('.btnAreaSaveConfirm').addClass('hide').siblings('.btnAreaSave').removeClass('hide');
    }
}
$(document).on('click', '#btnShowImage', function (e) {
    imageShow();
});
function rightMenuShow() {
    $(".rightMenu").animate({
        right: "0px"
    }, "fast", function () {
        $('.overlay').removeClass('hide');
        $(this).addClass('visible');
    });
}
function imageShow() {
    $(".cffImage").animate({
        left: "0px"
    }, "fast", function () {
        $('.overlay').removeClass('hide');
        $(this).addClass('visible');
    });
}
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
            $('.overlay').addClass('hide');
        });
    }
}
function imageHide() {
    if ($('.cffImage').hasClass('visible')) {
        $(".cffImage").removeClass('visible').animate({ "left": "-256px" }, "fast", function (e) {
            $('.overlay').addClass('hide');
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
    $('.overlay').removeClass('hide');
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
    $('.level03Group[level01id=' + level02.parents('.level02Group').attr('level01id') + ']').children('.level03Confirm').click();
    $('.overlay').addClass('hide');
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
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] #inputChainSpeed, .level02Group[level01id=' + level02Group.attr('level01id') + '] #inputLotNumber, .level02Group[level01id=' + level02Group.attr('level01id') + '] #inputMudScore').val("");

    //CFF
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .setsDone').text('0');
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .sideWithErrors').text('0').parents('.labelPainel').removeClass('red');
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .more3Defects').text('0').parents('.labelPainel').removeClass('red');
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .setAtual').text('1');
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .sideAtual').text('1');
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .sideErros').text('0');


    $('span.auditReaudit').text('Audit');


    level02.removeAttr('completed').removeAttr('reaudit').removeAttr('notavaliable').removeAttr('limitexceeded').attr('defects', '0').parents('.row').children('.userInfo').children('div').children('.defects').text(level02.attr('defects'));
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

    var level02Saved = $('.level02Result[level01id=' + level01.attr('id') + '][level02id=' + level02.attr('id') + '][date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + ']');

    level02Saved.children('.level03Result').each(function (e) {
        var input = $('.level03Group[level01id=3] li#' + $(this).attr('level03id') + '.level03 input');
        input.val($(this).attr('value'));
        var valor = parseInt(input.val());
        if (valor > 0) {
            input.parents('li').addClass('bgAlert');
        }
    });

    //$('.level03Group[level01id=3] .level03').each(function (e) {
    //    var input = $('.level03Group[level01id=3] li#' + $(this).attr('id') + '.level03 input');
    //    if (level02.attr('level03' + $(this).attr('id'))) {

    //        input.val(level02.attr('level03' + $(this).attr('id')));

    //        var valor = parseInt(input.val());
    //        if (valor > 0) {
    //            input.parents('li').addClass('bgAlert');
    //        }
    //    }
    //    //level03AlertAdd(input);
    //});
});
$(document).on('click', '.level02Group[level01id=2] .level02', function (e) {

    var level02 = $('.level02.selected');
    var level02Group = level02.parents('.level02Group');

    $('.level03Group[level01id=2] .level03 span.response').text('Yes').attr('value', '1');
    $('.level03Group[level01id=2] .level03').removeClass('lightred').removeAttr('notconform');
    $('.level03Group[level01id=2]').children('div').children('.button-collapse').click();

    var auditReauditLabel = $('.painelLevel03 .labelPainel[level01id=' + level02Group.attr('level01id') + '] .auditReaudit');
    auditReauditLabel.text('Audit');
    if (level02.attr('startReaudit')) {
        auditReauditLabel.text('Reaudit');
    }

    $('.level03Group[level01id=2] .level03').each(function (e) {
        if (level02.attr('level03' + $(this).attr('id'))) {
            var valueResponse = level02.attr('level03' + $(this).attr('id'));
            var response = $('.level03Group[level01id=2] div#' + $(this).attr('id') + '.level03 .row div span.response');
            if (valueResponse == '0') {
                response.parents('.level03').click();
            }
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


    if ($('.painelLevel03 .consecutiveFailure:visible').length)
    {

        if (level02.attr('consecutivefailurelevel')) {
            consectiveFailureLevel = parseInt(level02.attr('consecutivefailurelevel'));
        }

        if (level02.attr('consecutivefailuretotal')) {
            consecFailureTotal = parseInt(level02.attr('consecutivefailuretotal'));
        }

        var defects = parseInt(level02.attr('defects'));
        var defectsLimit = parseInt(level02.attr('levelerrorlimit'));
        


        
        if (level02.attr('completed'))
        {
            if(defects > defectsLimit)
            {   
                if(consectiveFailureLevel == 0)
                {
                    consectiveFailureLevel = 1;
                    var level02Last = $('.level02Result[level01id=' + level01.attr('id') + '][level02id=' + level02.attr('id') + '][shift=' + $('.App').attr('shift') + '][period!=' + $('.App').attr('period') + ']:last');

                    var consecFailureLast = 0;
                    if (level02Last.attr('consecutivefailuretotal')) {
                        consecFailureLast = parseInt(level02Last.attr('consecutivefailuretotal'));
                    }
                    consecFailureTotal = consectiveFailureLevel + consecFailureLast;

                }
              
            }
            else if(defects <= defectsLimit)
            {
                consectiveFailureLevel = 0;
                consecFailureTotal = 0;
            }
            //se eu alterei e continua com mais defeitos nao faço nada
            //se eu alterei e nao tem defeitos irei zerar o contador ottal
            //se eu alterei e nao tem total de defeitos e estouruo os defietos
                //vreifica o numero de defeitos anterior e icremento mais 1 no defeitos total e 1 no defeito da areax   
        }
        else if (defects > defectsLimit && !level02.attr('completed')) {
            consectiveFailureLevel  = 1
            consecFailureTotal++
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

    var attrReaudit = "";
    var attrReauditNumber = "";


    var reauditNumber = 0;
    var reaudit = level01.attr('reaudit');
    if (reaudit == "reaudit") {

        if (level01.attr('reauditnumber')) {
            reauditNumber = parseInt(level01.attr('reauditnumber'));
            reauditNumber++;
        }

        attrReaudit = "[reaudit=reaudit]";
        attrReauditNumber = "[reauditnumber=0]";
    }

    var level01Save = $('.level01Result[level01Id=' + level01.attr('id') + '][date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + ']' + attrReaudit + attrReauditNumber);

    if (!level01Save.length) {
        level01Save = $(
                            saveLevel01(level01.attr('id'), $('.App').attr('date'), $('.App').attr('shift'), $('.App').attr('period'), reaudit, reauditNumber)
                        );
    }

    var level02Save = $('.level02Result[level01id=' + level01.attr('id') + '][level02id=' + level02.attr('id') + '][date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + ']' + attrReaudit + attrReauditNumber)
    if (level02Save.length)
    {
        level02Save.remove();
    }

   
    level02Save = 
                $(saveLevel02(
                              level01.attr('id'),
                              level02.attr('id'),
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

        level02Save.append(level03Save);

    });
    $('.level03Group[level01id=3] .level03 input').val('0').parents('li').removeClass('bgAlert');

    level01Save.append(level02Save);

    level02.parents('.level02Group').append(level01Save);

    if (consecFailureTotal == 3)
    {
        level02.attr('reaudit', 'reaudit');
        level02.parents('.row').children('.userInfo').children('.pull-right').children('.btnReaudit').removeClass('hide');
    }

    level02Complete(level02);
    level02.removeClass('selected');

    if ($('.breadcrumb li a').length > 1) {
        $('.breadcrumb li a:last').click();
    }
    else
    {
        showLevel02(level01);
    }


    //if (level01.attr('completed')) {
    //    level01.attr('update', 'update');
    //}
});



//function saveLevel01(Level01Id, date, shift, period, totalSets, totalSides, atualSet, atualSide, totalErros) {
//    return "<div class='level01Result' level01Id='" + Level01Id + "' date='" + date + "' shift='" + shift + "' period='" + period + "' totalSets='" + totalSets + "' totalSides='" + totalSide + "' atualSet='" + atualSet + "' atualSide='" + atualSide + "' totalerros='" + totalErros + "'></div>";
//}
function saveLevel01(Level01Id, date, shift, period, reaudit, reauditNumber) {

    if (reaudit == "reaudit")
    {
        reaudit = true
    }
    else
    {
        reaudit = false;
    }
    if (reauditNumber == null)
    {
        reauditNumber = 0;
    }
    
    return "<div class='level01Result' level01Id='" + Level01Id + "' date='" + $('.App').attr('date') + "' dateTime='" + dateTimeFormat() + "' shift='" + shift + "' period='" + period + "' reaudit='" + reaudit + "' reauditNumber='" + reauditNumber + "'></div>";
}
function saveLevel02(Level01Id, Level02Id, date, dateTime, auditorId, shift, period, evaluate, sample, defects, reaudit, reauditNumber, phase, startPhaseDate, cattleType,
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


    return "<div class='level02Result' level01Id='" + Level01Id + "' level02Id='" + Level02Id + "' date='" + date + "' dateTime='" + dateTime + "' auditorId='" + auditorId
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

 

    $('.level03Group[level01id=6] .level02').each(function (e) {

        var level02 = $(this);

        var level02Save =
                $(saveLevel02(
                              level01.attr('id'),
                              level02.attr('level02id'),
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

                                 //level02.attr('level03' + level03.attr('id'), $(this).val());

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

                                level02Save.append(level03Save);



                                    
                             });
                             level02Head.parents('.level02Group').append(level02Save);

    });


    level02Complete(level02Head);

    currentSide = currentSide + 1;

    $('.sideErros').text('0').parents('.labelPainel').removeClass('red');
    $('.level03Group:visible input').val(0).parents('.level03').removeClass('bgAlert');
    $('.painelLevel03 .sideAtual').text(currentSide);

    if (currentSide > sidesperset) {
        currentSet = currentSet + 1;
        setsDone = setsDone + 1;

        if (setsDone == totalsets) {
            level02Head.attr('completed', 'completed');
            level02Complete(level02Head);
        }
        $('.painelLevel03 .setAtual').text(currentSet);
        $('.setsDone').text(setsDone);
        //aumentar o set
        defectLimitCheck();
        level02Return();
        $('.painelLevel03 .sideAtual').text("1");
    }
    $(this).parents('.level03Group').children('div').children('.button-collapse').click();
    $(document).scrollTop(0);   
});
$(document).on('click', '#btnSalvarHTP', function (e) {

    var level01 = $('.level01.selected');
    var level02 = $('.level02.selected');

    var level02Save = $(saveLevel02(
                            level01.attr('id'),
                            level02.attr('id'),
                            dateReturn(),
                            dateTimeFormat(),
                            $('.App').attr('auditorid'),
                            $('.App').attr('shift'),
                            $('.App').attr('period'),
                            null,
                            null,
                            level02.attr('defects'),
                            level02.attr('reaudit'),
                            level02.attr('reauditnumber'),
                            level02.attr('phase'),
                            level02.attr('startPhaseDate')

                         ));

    var reauditNumber = reauditCount(level02);

    $('.level03Group[level01id=2] .level03 span.response').each(function (e) {

        var level03 = $(this).parents('.level03');

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

        level02Save.append(level03Save);
    });

    level02.parents('.level02Group').append(level02Save);
    var phase = 0;

    if (level02.attr('phase')) {
       phase = parseInt(level02.attr('phase'))
    }

    if (level02.attr('limitexceeded')) {

        if (!level02.attr('completed'))
        {
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

    level02.attr('phase', phase);

    var btnPhase = level02.parents('.row').children('.userInfo').children('div').children('.labelPhase');
    if (phase > 0)
    {
        btnPhase.removeClass('hide');
        btnPhase.children('button').children('.atualPhase').text(phase);
    }
    
    if (level02.attr('prevphasexceeded'))
    {
        level02.removeAttr('prevphasexceeded').attr('limitexceeded', 'limitexceeded').parents('li').addClass('bgLimitExceeded');
    }

    level02Complete(level02);
    level02Return(level02);

    $(this).parents('.level03Group').children('div').children('.button-collapse').click();
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
    $('.level02Group:visible .level02Confirm').click();

    $('.level03Group:visible .level03Confirm').click();
});


$(document).on('change', 'select#reaudit:visible', function (e) {
    $('span.auditReaudit').html($("select#reaudit:visible :selected").text());
});
$(document).on('change', 'select#selectPeriod', function (e) {


    level01Reset($('.level01'));
    $('.painelLevel02 select').val(0);
    $('.painelLevel02 input').val("");

    level02Reset($('.level02List .level02Group .level02'));

    $('.App').attr('period', $('#selectPeriod option:selected').val());
    $('span.period').html($("select#selectPeriod:visible :selected").text());
    $('.totalDefects').text('0');

    var totalDefects = 0;


    var resultsLevel01 = $('.level01Result[date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + ']');

    resultsLevel01.each(function (e) {

        var level01Result = $(this);
        var results = $('.level02Result[level01id=' + level01Result.attr('level01id') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + '][date=' + $('.App').attr('date') + ']');

        results.each(function (e) {

            var result = $(this);
            var level02 = $('.level02Group[level01id=' + results.attr('level01id') + '] .level02[id=' + result.attr('level02id') + ']');
            level02.attr('defects', result.attr('defects')).attr('completed', 'completed');
            level02.parents('.row').children('.userInfo').children('div').children('.defects').text(level02.attr('defects'));
            totalDefects = totalDefects + parseInt(level02.attr('defects'));
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
            level02Complete(level02);
            level02.removeClass('selected');
        });

        if (level01Result.attr('completed'))
        {
            $('.level01[id=' + level01Result.attr('level01id') + ']').addClass('selected');
            $('.level02Group[level01id=' + level01Result.attr('level01id') + '] .level02Confirm').click();
        }

    });

    //verificar se existe uma reaudit em aberto
            //se existir mostra um botao para selecionar reaudit ou o periodo
            //se o contador for zero e tiver uma...ele tem que ser obrigado a fazer uma auditoria caso contrario (maior que zero) adiciona auditoria -verificar o controle de auditorias
            //pode até ter um modal avisando as readutis que tem que fazer

    //getLastLavelResult CCA e CFF
    //if lastlavelresult CCA or CFF is erro and not reaudt ....open modal com mensagem



    //verificar se tem alguma informação nesse periodo para esse dia
    

});
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
    var reauditNumber = reauditCount(level01);

    level01.attr('completed', 'completed');

    if (level01.attr('reaudit')) {
        level01.removeAttr('correctivaction');
    }
    else {
        level01.removeAttr('correctivaction').removeAttr('reaudit').removeAttr('reauditNumber');
    }

    var totalDefects = parseInt($('.painelLevel02 .totalDefects').text());
    var haveConsecutiveFailures = false

    var levels02 = $('.level02Group[level01id=' + level01.attr('id') + '] .level02[consecutivefailuretotal]');
    
    
        levels02.each(function (e) {
        
        var consectiveFailure = parseInt($(this).attr('consecutivefailuretotal'));

        if(consectiveFailure > 2)
        {
            haveConsecutiveFailures = true;
        }
    });

    if(totalDefects > 22 || haveConsecutiveFailures == true)
    {
        level01.attr('correctivaction', 'correctivaction').attr('reaudit', 'reaudit').attr('reauditNumber', reauditNumber);
    }

    if (level01.attr('startreaudit')) {
        level01.attr('reaudit', 'reaudit').attr('reauditNumber', reauditNumber).removeAttr('startreaudit');
        level01.parents('.row').children('.userInfo').children('div').children('.reauditCount').children('button').text(reauditNumber);
    }

    var level01Result = $('.level01Result[level01id=' + level01.attr('id') + '][date=' + $('.App').attr('date') + '][shift=' + $('.App').attr('shift') + '][period=' + $('.App').attr('period') + ']');
    level01Result.attr('completed', 'completed');
    level01Return();



});
$(document).on('click', '#btnSalvarLevel02CFF', function (e) {

    var level01 = $('.level01.selected');

    var reauditNumber = reauditCount(level01);


    level01.attr('completed', 'completed');

    if (level01.attr('reaudit')) {
        level01.removeAttr('correctivaction');
    }
    else {
        level01.removeAttr('correctivaction').removeAttr('reaudit').removeAttr('reauditNumber');
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
$(document).on('click', '#btnSalvarLevel02HTP', function (e) {

    var level01 = $('.level01.selected');
    //colocar regra de audit e reaudit igual em uma funcao...ela se repete para salvar do level 02

    var reauditNumber = reauditCount(level01);
    level01.attr('completed', 'completed');

    if (level01.attr('reaudit')) {
        level01.removeAttr('correctivaction');
    }
    else {
        level01.removeAttr('correctivaction').removeAttr('reaudit').removeAttr('reauditNumber');
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


    //nao é o 

    //level02Complete($('.level02List .level02Group[level01id=' + level01.attr('id') +'] .level02'));

    level01.click();
});

$(document).on('click', '.level02Group .btnReaudit', function (e) {

    var level02 = $(this).parents('.row').children('.level02');
    level02.attr('startReaudit', 'startReaudit').removeAttr('completed');
    level02.attr('defects', '0');
    $('.painelLevel03 .labelPhase').parents('.labelPainel').addClass('hide');

    if (level02.attr('havephases') && level02.attr('limitexceeded'))
    {
        $('.painelLevel03 .labelPhase').parents('.labelPainel').removeClass('hide');
        level02.attr('prevphasexceeded', 'prevphasexceeded').removeAttr('limitexceeded');
        
    }
    $('.painelLevel03 .labelPhase').text(level02.attr('phase'));

    //$('.level02List .level02Group[level01id=' + level01.attr('id') + '] .level02').removeAttr('completed').removeAttr('limitexceeded').removeAttr('notavaliable');
    //level02Reset($('.level02List .level02Group[level01id=' + level01.attr('id') + '] .level02'));
    level02Level03Reset(level02.parents('.level02Group'));
    //nao é o 
    //level02Complete($('.level02List .level02Group[level01id=' + level01.attr('id') +'] .level02'));
    level02.click();
});


$(document).on('click', '.btnCorrectiveAction', function (e) {
    $('.btnCorrectiveAction').removeClass('selected');
    $(this).addClass('selected');
    var level01 = $(this).parents('.row').children('.level01');
    $(this).addClass('hide');

    level01.removeAttr('correctivaction');

    $('.btnCA').click();
});
$(document).on('click', '#btnMessageOk', function (e) {
    $('#modalMessage').modal('hide');

});
function areaImage(id) {
    showLevel03($("#" + id + ".level02"));
}
function openMessageModal(title, content) {
    var $modal = $('#modalMessage');
    $modal.children('.modal-header').children('.messageHeader').text(title);
    $modal.children('.modal-body').children('.message').html(content);
    $modal.modal();
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