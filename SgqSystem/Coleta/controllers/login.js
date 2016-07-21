var storage = window.localStorage;

var Geral = {
 exibirMensagemAlerta: function (mensagem, url, container) {
        var page = $("html, body");
        Geral.esconderMensagem();
        $('#divMensagemAlerta').hide().find('span').text('');
        if (url == undefined || url.length == 0) {
            container = container || '';
            var $divMensagem = $(container + '#divMensagemAlerta');
            $divMensagem.find('span').text(mensagem);
            $divMensagem.show();
        } else {
            alert(mensagem);
            location.href = url;
        }
        $('html,body').animate({ scrollTop: 0 }, 'slow');
    },

    exibirMensagemErro: function (mensagem, url, container) {
        Geral.esconderMensagem();
        $('#divMensagemErro').hide().find('span').text('');
        if (url == undefined || url.length == 0) {
            container = container || '';
            var $divMensagem = $(container + '#divMensagemErro');
            $divMensagem.find('#mensagemErro').text(mensagem);
            $divMensagem.show();
        } else {
            alert(mensagem);
            location.href = url;
        }
        $('html,body').animate({ scrollTop: 0 }, 'slow');
    },

    esconderMensagem: function () {
        $('#divMensagemErro').hide();
        $('#divMensagemAlerta').hide();
        $('#divMensagemSucesso').hide();
    },

    exibirMensagemSucesso: function (mensagem, url, container) {
        if (mensagem == undefined || mensagem.length == 0) {
            Geral.esconderMensagem();
        } else {
            $('#divMensagemSucesso').hide().find('span').text('');
            if (url == undefined || url.length == 0) {
                container = container || '';
                var $divMensagem = $(container + '#divMensagemSucesso');
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

$("#message").append('<div id="divMensagemErro" style="display: none" class="alert alert-danger" role="alert">    <span class="icon-remove-sign"></span>    <strong>Erro! </strong><span id="mensagemErro"> </span></div><div id="divMensagemAlerta" style="display: none" class="alert alert-info" role="alert">    <span id="mensagemAlerta" class="icon-info-sign"></span></div><div id="divMensagemSucesso" style="display: none" class="alert alert-success" role="alert">   <span id="mensagemSucesso" class="icon-ok-circle"></span></div>')


function auth()
{
    var userTest = new User($("#user").val(), $("#password").val());

    $.ajax({
        type: "POST",
        dataType: "json",
        url: urlGetUser,
        data: userTest,
        success: function (data) {
            if(!!data.MensagemErro){
                Geral.exibirMensagemErro(data.MensagemErro);
            }
            if(!!data.MensagemAlerta){
                Geral.exibirMensagemAlerta(data.MensagemAlerta);
            }
            if(!!data.MensagemExcecao){
                Geral.exibirMensagemErro(data.MensagemExcecao);
            }
            if (!!data.Retorno) {
                storage.setItem("userId", data.Retorno.Id);
                //Geral.exibirMensagemAlerta(data.Retorno);
                $(location).attr('href', 'view/indicators.html');
            }
            if(!!data.listRetorno && data.listRetorno.length > 0){
                Geral.exibirMensagemAlerta(data.listRetorno);
            }
        },
        error: function (error) {
            jsonValue = jQuery.parseJSON(error.responseText);
        },
        complete: function(){ 
            console.log('Aways Execute this one "Complete"');
            $( "#login" ).button('reset');
        }
    });

}

$( "#login" ).click(function() {
    
	$(this).button('loading');
    auth();
});

$("#password").keypress(function(e) {

    if(e.which == 13) {
        $("#login").button('loading');
        auth();
    }
});

$("#user").keypress(function(e) {

    if(e.which == 13) {
        $("#password").focus();
    }
});
