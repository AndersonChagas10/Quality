var callbackRota;
var parameterRota;

function validaRota(callback, parameter) {

    callbackRota = null;
    parameterRota = null;

    //verifica se esta na tela de coleta se sim informa que os dados serão perdidos, se nao executa o callback direto
    if ($('#divColeta').is(":visible")) {
        if (callback != null) {
            callbackRota = callback;
            parameterRota = parameter;
            var titulo1 = "Confirmação de saída";
            var mensagem1 = "Deseja sair sem salvar? <br/>";
            openMessageConfirm(titulo1, mensagem1, executeCallbackRota, closeMensagemImediatamente, "orange", "white");
        }
    } else {
        if (parameter != null)
            callback(parameterRota, true);
        else
            callback(true);
    }
}

function executeCallbackRota() {
    if (parameterRota != null)
        callbackRota(parameterRota);
    else
        callbackRota();
}