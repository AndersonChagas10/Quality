var executandoRegistrarLog = false;
var executandoLimpezaQuantidadeDeErros = false;

//A necessidade disso se dá para não ficar todo momento enviando log, supondo que entre num laço infinito, por exemplo.
var quantidadeMaximaDeErro = 10;
var quantidadeDeTempoLimite = 60000
var quantidadeDeErros = 0;

//Script para exibir erros no Mobile
window.onerror = function (msg, url, lineNo, columnNo, error) {
    quantidadeDeErros++;
    
    if(quantidadeDeErros <= quantidadeMaximaDeErro){
        var log = {
            ErrorMessage: msg,
            Line: lineNo,
            Controller: "AppColeta",
            Object: error,
            StackTrace: msg + " - " + url
        }
        enviarLog(log);
    }else{
        if(!executandoLimpezaQuantidadeDeErros){
            executandoLimpezaQuantidadeDeErros = true;
            setTimeout(function(){
                quantidadeDeErros = 0;
                executandoLimpezaQuantidadeDeErros = false;
            }, quantidadeDeTempoLimite);
        }
    }
}

function enviarLog(log){
    if(!executandoRegistrarLog){
        executandoRegistrarLog = true;
        $.ajax({
            data: JSON.stringify(log),
            url: urlPreffix + '/api/LogError/Registrar/',
            type: 'POST',
            contentType: "application/json",
            success: function () {
                executandoRegistrarLog = false;
            },
            timeout: 600000,
            error: function () {
                executandoRegistrarLog = false;
            }
        });
    }
}