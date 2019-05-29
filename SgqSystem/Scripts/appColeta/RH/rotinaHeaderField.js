function getRotina(that) {

    if (!appIsOnline) {
        openModal("Voçê precisa estar online");
        closeModal(2000);
        return false;
    }

    var headerFieldsParams = $(that).attr('data-headerfields').split('|');
    var $btn = $(that);
    var headerFieldsList = {};
    var headerFieldsParamsClean = $(that).attr('data-headerfieldsClean').split('|')

    headerFieldsParams.forEach(function (headerFieldParam) {

        var self = $('input[data-param=' + headerFieldParam + ']');
        var value = $('input[data-param=' + headerFieldParam + ']').val();
        var isRequired = $(self).attr('data-required') == "true" ? true : false;

        $(self).css("background-color", "");

        if (isRequired && !value) {

            $(self).css("background-color", "#ffc1c1");
            openModal("Preencha os cabeçalhos", "blue", "white");
            closeModal(2000);
            return;

        } else {

            headerFieldsList[headerFieldParam] = value;

        }

    });

    if (Object.keys(headerFieldsList).length > 0) {

        var obj = {
            IdUsuario: currentLogin.Id,
            IdRotina: $(that).attr('data-id-rotina'),
            Params: headerFieldsList,
            HeaderFieldsParamsClean: headerFieldsParamsClean
        }

        getDynamicValues(obj, $btn);

    }
}

function getDynamicValues(obj, $btn) {

    $btn.button('loading');

    $.ajax({
        data: JSON.stringify(obj),
        url: urlPreffix + '/api/RetornaQueryRotinaApi/RetornaQueryRotina',
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        success: function (data) {

            if (data)
                setDynamicValues(data);
            else
                cleanInputHeaderFields(obj);


        },
        timeout: 10000,
        error: function () {
            openModal("Não foi possível buscar os dados", "blue", "white");
            closeModal(2000);
            cleanInputHeaderFields(obj);
        },
        complete: function () {
            $btn.button('reset');
        }

    });
}

function setDynamicValues(obj) {

    $.each(obj, function (key, value) {

        var input = $('input[data-din=' + key + ']');

        if (input)
            $(input).val(value);

    });
}

function cleanInputHeaderFields(obj) {

    if (obj.HeaderFieldsParamsClean && obj.HeaderFieldsParamsClean.length > 0)
        obj.HeaderFieldsParamsClean.forEach(function (key, i) {

            var input = $('input[data-din=' + key + ']');

            if (input)
                $(input).val("");
        });

}