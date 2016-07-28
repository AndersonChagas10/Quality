$(document).on('click', '#btnLogin', function (e) {
    e.preventDefault();
    showLevel01();
    //auth();
});

$(document).on('click', '.level01List .level01', function (e) {
    showLevel02($(this));
});

$(document).on('click', '.level02List .level02', function (e) {
    showLevel03($(this));
});

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

    if (!$('.level01.selected').length)
    {
    
        level01Id = parseInt($('.btnCorrectiveAction.selected').parents('.row').children('.level01').attr('id'));
    }

    var description = "";

    $('.level02List .level02Group[level01id=' + level01Id + '] .level02[limitexceeded]').each(function (e) {

        var level02 = $(this);
        var level02Id = parseInt(level02.attr('id'));
        var level02Name = level02.children('span').text();
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
                    $('.App').attr('userId', data.Retorno.Id);
                    $('.App').attr('userName', data.Retorno.Name);
                    $('.App').attr('loginTime', dateTimeFormat());
                    $('.App').attr('shift', $('#shift').val());
                    $('.App').attr('shiftName', $('#shift option:selected').text());
                    $('.shift').text($('.App').attr('shiftName'));
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

$('body').css("font-family", "Gotham");

var saveBtn = [
    {
        "bgcolor": "orange",
        "icon": "<i class='fa fa-save'></i>"
    },
    {
        "id": "btnSave",
        "position": "up",
        "url": "",
        "bgcolor": "blue",
        "color": "white",
        "icon": "<i class='fa fa-check'></i>",
        "title": "Save"
    }
]
$('.kc_fab_wrapper.btnSave').kc_fab(saveBtn);

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
        configureLevel01();
        $('.App').removeClass('hide').show();
        breadCrumb();
    });
}
function configureLevel01() {


    $('.btnCA').addClass('hide');

    //resetLevel

    $('.level01').parents('.row').children('.userInfo').children('div').children('.areaComplete').addClass('hide');
    $('.level01').parents('li').removeClass('bgCompleted');

    $('.level01').parents('.row').children('.userInfo').children('div').children('.btnCorrectiveAction').addClass('hide');

    $('.level01').parents('.row').children('.userInfo').children('div').children('.btnReaudit').addClass('hide');
    $('.level01, .level01').parents('li').removeClass('bgLimitExceeded')

    $('.level01').parents('.row').children('.userInfo').children('div').children('.reauditCount').text($('.level01[reauditNumber]').attr('reauditCount')).addClass('hide');


    //configureLevel
    $('.level01[completed]').parents('.row').children('.userInfo').children('div').children('.areaComplete').removeClass('hide');
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

        var level02 = $('.level02List');
        level02.removeClass('hide').show();
        $('.level02Group').addClass('hide');

        var level02Group = level02.children('.level02Group[level01id=' + level01.attr('id') + ']');

        level02Group.removeClass('hide');
        
        level02ButtonSave(level02Group);

        $('.level02').parents('li').children('.row').children('.userInfo').children('div').children('.areaComplete, .areaNotComplete').addClass('hide').siblings('.areaNotComplete').removeClass('hide');
        $('.level02[completed]').parents('li').children('.row').children('.userInfo').children('div').children('.areaComplete').removeClass('hide').siblings('.areaNotComplete, .na').addClass('hide').siblings('.btnAreaSave').addClass('hide');
        $('.level02[notavaliable]').parents('li').children('.row').children('.userInfo').children('div').children('.areaComplete').removeClass('hide').siblings('.na').addClass('naSelected').siblings('.areaNotComplete, .na').addClass('hide').siblings('.btnAreaSave').addClass('disabled');

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
function showLevel03(level02) {
    if (level02.attr('notavaliable')) {
        return false;
    }
    buttonsLevel02Hide();
    $('.level02').removeClass('selected');

    level02.addClass('selected');

    if (!level02.attr('defects')) {
        level02.attr('defects', '0');
    }
    $('.level02List').fadeOut("fast", function (e) {
        level02.parents('.level02List').addClass('hide');

        var level03 = $('.level03List');
        level03.removeClass('hide').show();
        $('.level03Group').addClass('hide');
        $('.painel .defects').text(level02.attr('defects'));
        defectLimitCheck();

        level03.children('.level03Group[level01id=' + $('.level01.selected').attr('id') + ']').removeClass('hide');

        breadCrumb($('.level01List .selected').text(), $('.level02List .level02.selected span:first').text());
        $('.btnSave').removeClass('hide');

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
$(document).on('click', '#btnMore', function (e) {
    rightMenuShow();
});
$(document).on('click', function (e) {
    rightMenuHide();
    imageHide();
});

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
        $('.btnSave').addClass('hide');
        $('.level01List').removeClass('hide').show();
        $('.iconReturn').addClass('hide');
        breadCrumb();
    });

    $('.correctiveaction').addClass('hide');
}

function level02Return() {
    $('.level03List').fadeOut("fast", function (e) {
        $(this).addClass('hide');
        $('.btnSave').addClass('hide');
        $('.level02').removeClass('selected');
        $('.level02List').removeClass('hide').show();
        btnCorrectiveAction();
        buttonsLevel02Show($('.level01.selected'));
        breadCrumb($('.level01List .selected').text());
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

    level02.attr('level03' + level03.attr('id'), valorInputDefects);

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
    
function inputChangesUpdate2(input)
{
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
    if (total6Erros > 5 || total3Erros > 0)
    {
        level02.attr('defects', "1");
    }

    

    $('.painel .sideErros').text(level03Defects);

}
function defectLimitCheck() {
    var level02 = $('.level02.selected');

    var defectsLevel02 = parseInt(level02.attr('defects'));
    var defectsLimit = parseInt(level02.attr('levelerrorlimit'));
    var defects = $('.painelLevel03 .defects').parent('div');
    defects.removeClass('red');
    var btnNA = level02.siblings('.userInfo').children('div').children('.na');
    if (defectsLevel02 > defectsLimit) {
        defects.addClass('red');
        level02.attr('limitExceeded', 'limitExceeded');
        level02.parents('li').addClass('bgLimitExceeded');
       // $('.btnCA').removeClass('hide');
        btnNA.addClass('hide');

    }
    else {
        defects.removeClass('red');
        level02.removeAttr('limitExceeded');
        level02.parents('li').removeClass('bgLimitExceeded');
        //if (!$('.level02[limitexceeded]')) {
        //    $('.btnCA').addClass('hide');
        //}
        //if (!level02.attr('completed')) {
        //    btnNA.removeClass('hide');
        //}
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

    var botaoSalvar = $(this).siblings('.btnAreaSave');
    var iconCompleto = $(this).siblings('.areaComplete');
    var iconNaoCompleto = $(this).siblings('.areaNotComplete');
    var level02 = $(this).parents('li').children('.row').children('.level02');
    if ($(this).hasClass('naSelected')) {
        $(this).removeClass('naSelected');
        botaoSalvar.removeClass('disabled');
        iconCompleto.addClass('hide');
        iconNaoCompleto.removeClass('hide');
        level02.removeAttr('notavaliable');
        level02.parents('li').removeClass('bgNoAvaliable');
    }
    else {
        $(this).addClass('naSelected');
        botaoSalvar.addClass('disabled');
        iconCompleto.removeClass('hide');
        iconNaoCompleto.addClass('hide');
        level02.attr('notavaliable', 'notavaliable');
        level02.parents('li').addClass('bgNoAvaliable');
    }
});
$(document).on('click', '.btnAreaSave', function (e) {

    if ($(this).hasClass('disabled')) {
        return false;
    }
    var level02 = $(this).parents('li').children('.row').children('.level02');
    level02.addClass('selected');
    $('.level03Group[level01id=' + level02.parents('.level02Group').attr('level01id') + ']').children('.level03Confirm').click();
    //level02Complete(level02);

});
function level02Reset(level02) {

    var level02Group = level02.parents('.level02Group');
    
    level02Level03Reset(level02Group);

    //CCA
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .totalDefects').text('0');
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .defects').text('0');
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] #cattleType').val('0');
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] #chainSpeed, .level02Group[level01id=' + level02Group.attr('level01id') + '] #lotNumber, .level02Group[level01id=' + level02Group.attr('level01id') + '] #mudScore').val("");

    //CFF
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .setsDone').text('0');
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .sideWithErrors').text('0').parents('.labelPainel').removeClass('red');
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .more3Defects').text('0').parents('.labelPainel').removeClass('red');
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .setAtual').text('1');
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .sideAtual').text('1');
    $('.painel .labelPainel[level01id=' + level02Group.attr('level01id') + '] .sideErros').text('0');


    $('span.auditReaudit').text('Reaudit');

    
    level02.removeAttr('completed').removeAttr('limitexceeded').attr('defects', '0').parents('.row').children('.userInfo').children('div').children('.defects').text(level02.attr('defects'));
    level02.parents('li').removeClass('bgCompleted').removeClass('bgLimitExceeded');


    var botaoNa = level02.parents('.row').children('.userInfo').children('div').children('.na');
    var botaoSalvarLevel02 = level02.parents('.row').children('.userInfo').children('div').children('.btnAreaSave');
    var iconCompleto = level02.parents('.row').children('.userInfo').children('div').children('.areaComplete');
    var iconNaoCompleto = level02.parents('.row').children('.userInfo').children('div').children('.areaNotComplete');
 
    botaoNa.removeClass('hide');
    botaoSalvarLevel02.removeClass('hide');
    iconCompleto.addClass('hide');
    iconNaoCompleto.removeClass('hide');
    $('.btnSave, .btnCA').addClass('hide');


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
        var iconCompleto = level.parents('.row').children('.userInfo').children('div').children('.areaComplete');
        var iconNaoCompleto = level.parents('.row').children('.userInfo').children('div').children('.areaNotComplete');

        iconCompleto.removeClass('hide');
        iconNaoCompleto.addClass('hide');
        botaoNa.addClass('hide');
        botaoSalvarLevel02.addClass('hide');
        $('.btnSave').addClass('hide');
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
    if(level01.attr('saveLevel02') && (level02Group.children('li').children('.row').children('.level02[completed!=completed]').length ==  0 || (level01.attr('update') && level01.attr('completed'))))
    {
        $('.btnSave').removeClass('hide');
    }
}
$(document).on('click', '.level02Group[level01id=3] .level02', function (e) {
    var level02 = $('.level02.selected');
    $('.level03Group[level01id=3] .level03 input').val(0).parents('li').removeClass('bgAlert');

    $('.level03Group[level01id=3] .level03').each(function (e) {
        var input = $('.level03Group[level01id=3] li#' + $(this).attr('id') + '.level03 input');
        if (level02.attr('level03' + $(this).attr('id'))) {

            input.val(level02.attr('level03' + $(this).attr('id')));

            var valor = parseInt(input.val());
            if(valor > 0)
            {
                input.parents('li').addClass('bgAlert');
            }
        }
        //level03AlertAdd(input);
    });
});
$(document).on('click', '.level02Group[level01id=2] .level02', function (e) {
    var level02 = $('.level02.selected');
    var level02Group = level02.parents('.level02Group');

    $('.level03Group[level01id=2] .level03 span.response').text('Yes').attr('value', '1');
    $('.level03Group[level01id=2] .level03').removeClass('lightred').removeAttr('notconform');
    $('.level03Group[level01id=2]').children('div').children('.button-collapse').click();

    var auditReauditLabel = $('.painelLevel03 .labelPainel[level01id=' + level02Group.attr('level01id') + '] .auditReaudit');
    auditReauditLabel.text('Audit');
    if (level02.attr('startReaudit'))
    {
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

    $('.level03Group[level01id=3] .level03 input[value=0]').each(function (e) {

        var level03 = $(this).parents('.level03');
        level02.attr('level03' + level03.attr('id'), $(this).val());

    });
    
    if (level01.attr('completed'))
    {
        level01.attr('update', 'update');
    }
    $('.level03Group[level01id=3] .level03 input').val('0').parents('li').removeClass('bgAlert');
    level02Complete(level02);
    if ($('.breadcrumb li a').length > 1) {
        $('.breadcrumb li a:last').click();
    }
});
$(document).on('click', '#btnSalvarCFF', function (e) {

    var level02 = $('.level02.selected');
    var level02Group = $('.level03Group:visible');

    var currentSet = parseInt($('.painelLevel03 .setAtual').text());
    var currentSide = parseInt($('.painelLevel03 .sideAtual').text());

    var totalsets = parseInt(level02.attr('totalsets'));
    var totalsides = parseInt(level02.attr('totalsides'));
    var sidesperset = parseInt(level02.attr('sidesperset'));

    var returnlevel02endset = level02.attr('returnlevel02endset');

    var setsDone = parseInt($('.painelLevel02 .setsDone').text());

    currentSide = currentSide + 1;
    level02Group.removeAttr('firstErrorSide');
    level02Group.removeAttr('Error3MoreSide');


    //if ($('.painelLevel02 .sideWithErrors').parents('.labelPainel').hasClass('red') || $('.painelLevel03 .sideErros').parents('.labelPainel').hasClass('red'))
    //{
    //    $('.btnCA').removeClass('hide');
    //}

    if (currentSide > sidesperset)
    {
        currentSet = currentSet + 1;
        setsDone = setsDone + 1;
        
        
        if (setsDone == totalsets)
        {
            level02.attr('completed', 'completed');
            level02Complete(level02);

        }
        $('.painelLevel03 .setAtual').text(currentSet);
        $('.setsDone').text(setsDone);
        //aumentar o set
        defectLimitCheck();
        level02Return();
        $('.painelLevel03 .sideAtual').text("1");

    }
    else
    {
        $('.sideErros').text('0').parents('.labelPainel').removeClass('red');
        $('.level03Group:visible input').val(0).parents('.level03').removeClass('bgAlert');
        $('.painelLevel03 .sideAtual').text(currentSide);
    }

    //verificar o side atual 
    //adicionar + 1
    //verificar se ele é o ultimo
    //se ele for o o ultimo verifica se ele volta o level ou se atualiza o site



    $(this).parents('.level03Group').children('div').children('.button-collapse').click();
    $(document).scrollTop(0);

    //se o side dor igual o ultimo side..retorna
});
$(document).on('click', '#btnSalvarHTP', function (e) {

    var level02 = $('.level02.selected');
    $('.level03Group[level01id=2] .level03 span.response[value=1]').each(function (e) {
        level02.attr('level03' + $(this).parents('.level03').attr('id'), $(this).attr('value'));

    });

    //$(document).scrollTop(0);
    //if (level02.attr('reaudit'))
    //{
    //}
    var reauditNumber = reauditCount(level02);
    //if ($('.level02Group[level01id=' + level02.attr('id') + '] .level02[limitexceeded]').length) {
    if(level02.attr('limitexceeded'))
    {
        level02.attr('correctivaction', 'correctivaction').attr('reaudit', 'reaudit').attr('reauditNumber', reauditNumber);
        level02.parents('.row').children('.userInfo').children('div').children('.btnReaudit').removeClass('hide').siblings('.reauditCount').removeClass('hide');

    }
    if (level02.attr('startreaudit')) {
        level02.attr('reaudit', 'reaudit').attr('reauditNumber', reauditNumber).removeAttr('startreaudit');
        level02.parents('.row').children('.userInfo').children('div').children('.reauditCount').text(reauditNumber);
    }

    level02Complete(level02);
    level02Return(level02);
    
    $(this).parents('.level03Group').children('div').children('.button-collapse').click();

});
$(document).on('mousedown', '#btnSave', function (e) {
    $('.level02Group:visible .level02Confirm').click();

    $('.level03Group:visible .level03Confirm').click();
});


$(document).on('change', 'select#reaudit:visible', function (e) {
    $('span.auditReaudit').html($("select#reaudit:visible :selected").text());
});
$(document).on('change', 'select#selectPeriod:visible', function (e) {
    $('span.period').html($("select#selectPeriod:visible :selected").text());
});
$(document).on('change', 'select#cattleType', function (e) {
    $('span.cattleType').html("<strong>Cattle Type:</strong>" + $("select#cattleType :selected").text());
});
$(document).on('input', 'input#chainSpeed', function (e) {
    $('span.chainSpeed').html("<strong>Chain Speedy:</strong>" + $(this).val());
});
$(document).on('input', 'input#lotNumber', function (e) {
    $('span.lotNumber').html("<strong>Lot #:</strong>" + $(this).val());
});
$(document).on('input', 'input#mudScore', function (e) {
    $('span.mudScore').html("<strong>Mud Score:</strong>" + $(this).val());
});
$(document).on('change', 'select#biasedUnbiased', function (e) {
    $('span.biasedUnbiased').html($("select#biasedUnbiased :selected").text());
});
function reauditCount(level01) {
    var reauditNumber = 0;
    if (level01.attr('reauditNumber')) {
        reauditNumber = parseInt(level01.attr('reauditNumber'));
    }
    if (level01.attr('startreaudit')) {
        reauditNumber++;
    }
    return reauditNumber;
}
$(document).on('click', '#btnSalvarLevel02CCA', function (e) {

    var level01 = $('.level01.selected');
    //colocar regra de audit e reaudit igual em uma funcao...ela se repete para salvar do level 02
  
    var reauditNumber = reauditCount(level01);
    level01.attr('completed', 'completed');
    
    if (level01.attr('reaudit'))
    {
        level01.removeAttr('correctivaction');
    }
    else
    {
        level01.removeAttr('correctivaction').removeAttr('reaudit').removeAttr('reauditNumber');
    }

    if ($('.level02Group[level01id=' + level01.attr('id') + '] .level02[limitexceeded]').length)
    {
        level01.attr('correctivaction', 'correctivaction').attr('reaudit', 'reaudit').attr('reauditNumber', reauditNumber);
    }
    if (level01.attr('startreaudit'))
    {
        level01.attr('reaudit', 'reaudit').attr('reauditNumber', reauditNumber).removeAttr('startreaudit');
        level01.parents('.row').children('.userInfo').children('div').children('.reauditCount').text(reauditNumber);
    }
    level01Return();
    //alert('salvar level02 completo');
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
        level01.parents('.row').children('.userInfo').children('div').children('.reauditCount').text(reauditNumber);
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
        level01.parents('.row').children('.userInfo').children('div').children('.reauditCount').text(reauditNumber);
    }
    level01Return();
    //alert('salvar level02 completo');
});


$(document).on('click', '.level01List .btnReaudit', function (e) {
    
    var level01 = $(this).parents('.row').children('.level01');

    if (level01.attr('correctivaction'))
    {
        alert('Open Corrective Action');
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
function areaImage(id) {
    showLevel03($("#" + id + ".level02"));
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