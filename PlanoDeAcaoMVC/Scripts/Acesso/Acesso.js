/*Aux*/
var checkSessionControl;
var identy = {};
/*Aux END*/

/*Config Modal*/
var modal = bootbox.dialog({
    message: $(".form-content").html(),
    title: "Autenticação",
    buttons: [
      {
          label: "Entrar",
          className: "btn btn-primary pull-left",
          callback: function () {
              var user = $('#formLogin form').serializeObject();
              if (!user.Name.length) {
                  $('#formLogin #resultado').text('O campo nome é obrigatório.').fadeIn().fadeOut(2000, "swing");
              }
              if (!user.Password.length) {
                  $('#formLogin #resultado1').text('O senha nome é obrigatório.').fadeIn().fadeOut(2000, "swing");
              }
              if (user.Name.length && user.Password.length) {
                  loginPaSgq(user);
              }
              return false;
          }
      },
    ],
    show: false,
    onEscape: function () {
        //modal.modal("hide");
    },
    closeButton: false,
});
modal.attr("id", "formLogin");
modal.modal("show");
/*Config Modal END*/

/*Trigger*/
function checkSession() {
    if (!getCookie('webControlCookie')) {
        modal.modal("show");
        //clearInterval(checkSessionControl);
    }
    //else
    //    console.log('logado')
}
/*Trigger END*/

/*PA*/
var urlCookie = '@Url.Action("GetUserCookie", "api/Pa_User")';
var urlMaster = '@Html.Raw(Conn.SgqHost)';
var isSgqIntegrado = '@Html.Raw(Conn.isSgqIntegrado)';

function loginPaSgq(user) {
    $.LoadingOverlay('show');
    if (isSgqIntegrado) {
        $.post(urlMaster, user, function (r) {
            $.LoadingOverlay('hide')
            if (r.Mensagem == null && r.MensagemExcecao == null && r.Retorno != null) {
                //console.log(r);
                modal.modal("hide");
                createCookie(r.Retorno);
                identy = user;
            }
            else {
                $('#formLogin #resultado').text('Usuário ou Senha inválidos.').fadeIn().fadeOut(2000, "swing");
                console.log(r);
            }
        });
        //} else {

        //}
    }
}

function createUpdateCookie(dto) {
    $.post(urlCookie, dto, function (r, a, xhr) {
        //console.log(r)
        xhr.getResponseHeader('Set-Cookie');
        checkSessionControl = setInterval(checkSession, 3000);
    });
}