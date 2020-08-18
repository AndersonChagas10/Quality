
$(document).off('click', 'form[data-form-coleta] div[data-linha-coleta]')
    .on('click', 'form[data-form-coleta] div[data-linha-coleta]', function () {

        if ($(this).find('input, button').prop('disabled'))
            return;

        if ($(this).hasClass('alert-secundary')) {
            $('form[data-form-coleta] div[data-linha-coleta]').css("background-color", "#FFFFFF")
                .find('input, button').prop("disabled", false);
        } else {
            $('form[data-form-coleta] div[data-linha-coleta]').not(this).css("background-color", "#999")
                .find('input, button').prop("disabled", true);
        }
    })
