var callbackRota;
var parameterRota;

function validaRota(callback, parameter) {

    callbackRota = null;
    parameterRota = null;
    
    //verifica se esta na tela de coleta se sim informa que os dados serão perdidos, se nao executa o callback direto
    if ($('#divColeta').is(":visible") && interacaoComFormulario > 0) {
        deparment = "";
        if (callback != null) {
            callbackRota = callback;

            parameterRota = parameter;
            var titulo1 = "Confirmação de saída";
            var mensagem1 = "Deseja sair sem salvar? <br/>";
            openMessageConfirm(titulo1, mensagem1, executeCallbackRota, closeMensagemImediatamente, "orange", "white");
        }
    } else {
        deparment = "";
        if (parameter != null) {
            parameterRota = parameter;
            if (validateSyncCount()) {
                callback(parameterRota, true);
            } else {
                setMessageSyncNotFinished();
            }
        } else {
            if (validateSyncCount()) {
                callback(true);
            } else {
                setMessageSyncNotFinished();
            }
        }
    }
}

function setMessageSyncNotFinished() {
    openMensagem("Aguarde enquanto as coletas são sincronizadas", "blue", "white");
    setTimeout(function () {
        closeMensagem();
    }, 8000);
}

function validateSyncCount() {
    if (appIsOnline) {
        if (globalColetasRealizadas.length > 0) {
            return false;
        }
        else {
            return true;
        }
    } else
        return true;
}

function executeCallbackRota() {
    if (parameterRota != null)
        callbackRota(parameterRota, true);
    else
        callbackRota(true);
}