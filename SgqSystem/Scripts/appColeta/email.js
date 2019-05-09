function setEnviaEmail() {
    $.ajax({
        type: 'POST'
        , url: urlPreffix + '/api/SyncServiceApi/sendEmailAlerta'
        , headers: token()
        , contentType: 'application/json; charset=utf-8'
        , dataType: 'json'
        //, data: '{' + "insertDeviation: '" + deviationsSend + "'" + '}'
        //, data: '{' + "obj: '" + objectSend + "', collectionDate : '" + level02.attr('datetime') + "', level01Id: '" + level01.attr('level01Id') + "', level02Id: '" + level02.attr('level02id') + "', unitId: '" + level01.attr('unidadeid') + "', period: '" + level01.attr('period') + "', shift: '" + level01.attr('shift') + "', device: '123', version: '" + versao + "', ambient: '" + baseAmbiente + "'" + '}'
        , async: true
        , error: function (XMLHttpRequest, textStatus, errorThrown) {
            //createLog(XMLHttpRequest.responseText);
            console.log(XMLHttpRequest.responseText);
        }
    });
}