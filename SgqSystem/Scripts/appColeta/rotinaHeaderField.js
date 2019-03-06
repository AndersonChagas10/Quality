function getRotina(that) {

    var headerFields_Ids = $(that).attr('data-headerfields').split('|');

    var headerFieldsList = [];

    headerFields_Ids.forEach(function (headerFieldId) {

        var key = [headerFieldId];
        var value = $('input[data-param=' + headerFieldId + ']').val();

        if (key && value) {
            var headerFieldsSend = { [headerFieldId]: $('input[data-param=' + headerFieldId + ']').val() };
            headerFieldsList.push(headerFieldsSend);
        }

    });

    if (headerFieldsList.length > 0) {

        var obj = {
            IdRotina: $(that).attr('data-id-rotina'),
            Params: headerFieldsList
        }

        getDynamicValues(obj);

    } else {

        openMessageModal("Prencha o cabeçalho", "Preencha o cabeçalho para buscar os dados");

    }

}

function getDynamicValues(obj) {

    $.ajax({
        data: obj,
        url: urlPreffix + '/api/RetornaQueryRotinaApi/RetornaQueryRotina',
        type: 'POST',
        success: function (data) {
            debugger
            if (data)
                setDynamicValues(data);

        },
        timeout: 600000,
        error: function () {

        }
    });

}

function setDynamicValues(obj) {

    Object.entries(obj).forEach(function ([key, value]) {

        var input = $('input[data-din=' + key + ']');

        if (input)
            $(input).val(value);

    });
}