function getRotina(that) {

    if (!online) {
        openMessageModal(getResource("warning"), getResource("you_are_not_online"));
        return false;
    }

    var headerFieldsParams = $(that).attr('data-headerfields').split('|');
    var $btn = $(that);
    var headerFieldsList = {};
    var headerFieldsParamsClean = $(that).attr('data-headerfieldsClean').split('|')

    headerFieldsParams.forEach(function (headerFieldParam) {

        var self = $('input[data-param=' + headerFieldParam + ']').parents('.header');
        var value = $('input[data-param=' + headerFieldParam + ']').val();
        var isRequired = $(self).prop('required');

        if (isRequired && !value) {

            $(self).addClass("has-warning");
            openMessageModal(getResource("warning"), getResource("fill_header_fields"));
            return;

        } else {

            headerFieldsList[headerFieldParam] = value;
        }
    });

    if (Object.keys(headerFieldsList).length > 0) {

        var obj = {
            IdUsuario: $('.App').attr('userid'),
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

            openMessageModal(getResource("error"), getResource("unable_to_complete_data_request"));
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