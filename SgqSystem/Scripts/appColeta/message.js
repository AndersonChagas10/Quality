
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
        $('#btnLoginOffLine').button('reset');
        $('#btnLoginOnline').button('reset');
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


function wMessage(elemento, message) {
    elemento.children('.wMessage').text(message);
}

function openMessageModal(title, content, tipo, button) {

    if($(".level2Group:visible").length > 0){        
        $('.btnAreaSaveConfirm').addClass('hide');
        $('.btnAreaSave').removeAttr('disabled');
    }

    var mensagem = $('.message, .overlay');

    if (tipo == 'alerta') {
        $('.message').addClass('alertRed');
    }
    else {
        $('.message').removeClass('alertRed');
    }

    mensagem.children('.head').html(title);
    mensagem.children('.body').html(content);
    
    $('.btnMessage').removeClass('hide').show();

    mensagem.fadeIn("fast", function (e) {
        mensagem.find('button:last').focus();
    });
}

function openMessageConfirm(title, content, callback) {

    if($(".level2Group:visible").length > 0){        
        $('.btnAreaSaveConfirm').addClass('hide');
        $('.btnAreaSave').removeAttr('disabled');
    }

    $('#passMessageComfirm').val("");
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
        mensagem.fadeOut("fast", function (e) {
            $('#passMessageComfirm').removeClass('hide');
            $('#inputDate').addClass('hide');
        });
    });
}

function openMessageConfirmAtraso(callback, type) {

    var mensagem = $('.messageParReasonType' + type + ', .overlay');

    mensagem.fadeIn("fast");

    $('.messageParReasonType'+type+' #btnAtrasoOk').click(function () {
        mensagem.fadeOut("fast");
        callback();
    });
}

function openMessageSelecionarLevel3PorDepartamento(callback) {

    var mensagem = $('.messageSelecionarDepartamento, .overlay');

    mensagem.fadeIn("fast");

    $('.messageSelecionarDepartamento #btnAtrasoOk').click(function () {
        mensagem.fadeOut("fast");
        callback();
    });
}

function openMessageCA(title, content, callback) {

    if($(".level2Group:visible").length > 0){        
        $('.btnAreaSaveConfirm').addClass('hide');
        $('.btnAreaSave').removeAttr('disabled');
    }

    $('#passMessageComfirm').addClass('hide');
    var mensagem = $('.messageConfirm, .overlay');

    mensagem.children('.head').html(title);
    mensagem.children('.body').children('.txtMessage').html(content);
    $('#btnMessageYes').removeClass('hide');
    $('#btnMessageNo').removeClass('hide');

    mensagem.fadeIn("fast");
	
	mensagem.off("fast");
	$('#btnMessageYes').off('click');
	$('#btnMessageNo').off('click');

    $('#btnMessageYes').click(function () {
        mensagem.fadeOut("fast", function (e) {
            callback();
        });
    });
    $('#btnMessageNo').click(function () {
        mensagem.fadeOut("fast", function (e) {
        });
    });
}

function openSyncModal(title, content) {

    var mensagem = $('.message, .overlay');
    mensagem.children('.head').html(title);
    mensagem.children('.body').html(getResource("synchronizing")+"... ");
    //"<span class='perc'> 0</span> / " + ($('.level02Result[sync=false]').parents('.level01Result').length));
    //$('#btnMessageOk').addClass('hide');

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

$(document).on('click', '#btnMessageOk', function (e) {
    ////remover modal do body
    //remover modal do body
    var mensagem = $('.message');

    $('.overlay').hide();
    mensagem.fadeOut("fast");
    if($('.alertRed').length > 0)
        if($('.alertRed')[0].style.display != "none"){
            if(acaoCorretivaObrigatoria)
            {
                correctiveActionOpen(_level1.id, getCollectionDate(), $('.App').attr('shift'), $('.App').attr('period'));
                $('.modal-close-ca').hide();
            }
        }
});

function statusMessage(message) {
    $('.status').text(message);
    setTimeout(function () {
        $('.status').empty();
    }, 10000);
}

function getAlertMessage(tipoAlerta, alertaAtual, defeitosL1, metaTolerancia, defeitosLevel2, metaavaliacao){
    
    if($('.App').attr('local') == "brasil"){
        
        if (tipoAlerta == "a1" || tipoAlerta == "a2" || tipoAlerta == "a4") {
            switch(alertaAtual){
                case 0: 
                    return "Foi excedida a "+(alertaAtual+1)+"ª meta tolerância* ("+defeitosL1+" pontos NC de "+metaTolerancia+" pontos permitidos). <br /><br />"+
                            "O Supervisor da área será notificado e deverá tomar ações corretivas. <br /><br />"+
                            "* "+(alertaAtual+1)+"ª meta tolerância corresponde a "+(alertaAtual+1)+" terço da meta do indicador transformada em pontos.";
                    break;
                case 1: 
                    return "Foi excedida a "+(alertaAtual+1)+"ª meta tolerância* ("+defeitosL1+" pontos NC de "+metaTolerancia+" pontos permitidos) e meta avaliação** ("+defeitosLevel2+" pontos NC de "+metaavaliacao+" pontos permitidos por avaliação).<br /><br />"+
                            "O Supervisor e o Gerente da área serão notificados e deverão tomar ações corretivas. <br /><br />"+
                            "* "+(alertaAtual+1)+"ª meta tolerância corresponde a "+(alertaAtual+1)+" terços da meta do indicador transformada em pontos.<br />"+
                            "** Meta avaliação corresponde ao número de pontos permitidos por avaliação e é calculada através da divisão da meta do indicador transformada em pontos pelo número de avaliações por monitoramento.";
                    break;
                case 2: 
                    return "Foi excedida a "+(alertaAtual+1)+"ª meta tolerância* ("+defeitosL1+" pontos NC de "+metaTolerancia+" pontos permitidos) e meta avaliação** ("+defeitosLevel2+" pontos NC de "+metaavaliacao+" pontos permitidos por avaliação).<br /><br />"+
                            "O Supervisor, o Gerente e o Diretor da área serão notificados e deverão tomar ações corretivas. <br /><br />"+
                            "* "+(alertaAtual+1)+"ª meta tolerância corresponde a meta do indicador transformada em pontos.<br />"+
                            "** Meta avaliação corresponde ao número de pontos permitidos por avaliação e é calculada através da divisão da meta do indicador transformada em pontos pelo número de avaliações por monitoramento.";
                    break;
                default:
                    return "Foi excedida a "+(alertaAtual+1)+"ª meta tolerância* ("+defeitosL1+" pontos NC de "+metaTolerancia+" pontos permitidos) e meta avaliação** ("+defeitosLevel2+" pontos NC de "+metaavaliacao+" pontos permitidos por avaliação).<br /><br />"+
                            "O Supervisor e o Gerente da área serão notificados e deverão tomar ações corretivas. <br /><br />"+
                            "* "+(alertaAtual+1)+"ª meta tolerância corresponde a "+(alertaAtual+1)+" terços da meta do indicador transformada em pontos.<br />"+
                            "** Meta avaliação corresponde ao número de pontos permitidos por avaliação e é calculada através da divisão da meta do indicador transformada em pontos pelo número de avaliações por monitoramento.";

            }
        } else if (tipoAlerta == "a3") {
            switch(alertaAtual){
                case 0: 
                    return "Foi excedida a primeira meta tolerância* ("+defeitosL1+"% NC de "+metaTolerancia+"% meta indicador).<br /><br />"+
                            "O Supervisor da área será notificado e deverá tomar ações corretivas. <br /><br />"+
                            "* "+(alertaAtual+1)+"ª meta tolerância corresponde a "+(alertaAtual+1)+" terço da meta do indicador.";
                    break;
                case 1: 
                    return "Foi excedida a segunda meta tolerância* ("+defeitosL1+"% NC de "+metaTolerancia+"% meta indicador). <br /><br />"+
                            "O Supervisor e o Gerente da área serão notificados e deverão tomar ações corretivas. <br /><br />"+
                            "* "+(alertaAtual+1)+"ª meta tolerância corresponde a "+(alertaAtual+1)+" terços da meta do indicador.";
                    break;
                case 2: 
                    return "Foi excedida a "+(alertaAtual+1)+"ª meta tolerância* ("+defeitosL1+"% NC de "+metaTolerancia+"% meta indicador).<br /><br />"+
                            "O Supervisor, o Gerente e o Diretor da área serão notificados e deverão tomar ações corretivas. <br /><br />"+
                            "* "+(alertaAtual+1)+"ª meta tolerância corresponde a meta do indicador.";
                    break;
                default: 
                    return "Foi excedida a "+(alertaAtual+1)+"ª meta tolerância* ("+defeitosL1+"% NC de "+metaTolerancia+"% meta indicador).<br /><br />"+
                            "O Supervisor e o Gerente da área serão notificados e deverão tomar ações corretivas. <br /><br />"+
                            "* "+(alertaAtual+1)+"ª meta tolerância corresponde a "+(alertaAtual+1)+" terços da meta do indicador.";
                    
            }
        } else {
            return "Número de não conformidades excedido."
        }

    } else if($('.App').attr('local') == "eua"){

        if (tipoAlerta == "a1" || tipoAlerta == "a2" || tipoAlerta == "a4") {
            switch(alertaAtual){
                case 0: 
                    return "The first tolerance goal was exceeded ("+defeitosL1+" NC points of "+metaTolerancia+" allowed points). <br /><br />"+
                            "The supervisor will be notified and will take corrective actions.";
                    break;
                case 1: 
                    return "The second tolerance goal was exceeded ("+defeitosL1+" NC points of "+metaTolerancia+" allowed points) and the evaluation goal ("+defeitosLevel2+" NC points of "+metaavaliacao+" allowed points per evaluation).<br /><br />"+
                            "The supervidor and the area manager will be notified and will take corrective actions.";
                    break;
                case 2: 
                    return "The third tolerance goal was exceeded ("+defeitosL1+" NC points de "+metaTolerancia+" allowed points) and the evaluation goal ("+defeitosLevel2+" NC points of "+metaavaliacao+" allowed points per evaluation).<br /><br />"+
                            "The supervisor, the manager and the area manager will be notified and will take corrective actions.";
                    break;
                default:
                    return "The fourth tolerance goal was exceeded ("+defeitosL1+" NC points de "+metaTolerancia+" allowed points) and the evaluation goal ("+defeitosLevel2+" NC points of "+metaavaliacao+" allowed points per evaluation).<br /><br />"+
                            "The supervisor and the area manager will be notified and will take corrective actions.";

            }
        } else if (tipoAlerta == "a3") {
            switch(alertaAtual){
                case 0: 
                    return "The first tolerance goal was exceeded ("+defeitosL1+"% of NC "+metaTolerancia+"% indicator goal).<br /><br />"+
                            "The supervisor will be notified and will take corrective actions.";
                    break;
                case 1: 
                    return "The second tolerance goal was exceeded ("+defeitosL1+"% of NC "+metaTolerancia+"% indicator goal). <br /><br />"+
                            "The supervidor and the area manager will be notified and will take corrective actions.";
                    break;
                case 2: 
                    return "The third tolerance goal was exceeded ("+defeitosL1+"% of NC "+metaTolerancia+"% indicator goal).<br /><br />"+
                            "The supervisor, the manager and the area manager will be notified and will take corrective actions.";
                    break;
                default: 
                    return "The fourth tolerance goal was exceeded ("+defeitosL1+"% of NC "+metaTolerancia+"% indicator goal).<br /><br />"+
                            "The supervisor and the area manager will be notified and will take corrective actions.";
                    
            }
        } else {
            return "The number of not conformities is exceeded."
        }

    }

    
    
}