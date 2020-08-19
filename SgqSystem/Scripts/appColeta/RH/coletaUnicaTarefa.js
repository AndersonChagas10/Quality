
$(document).off('click', 'form[data-form-coleta] div[data-linha-coleta]')
    .on('click', 'form[data-form-coleta] div[data-linha-coleta]', function () {

        if ($(this).find('input, button').prop('disabled'))
            return;

        var isNA = $(this).attr('data-conforme-na') == "";

        if ($(this).attr('data-conforme') != $(this).attr('data-default-answer') || isNA) {

            $('form[data-form-coleta] div[data-linha-coleta]').not(this).css("background-color", "#999")
                .find('input, button').prop("disabled", true);
            $('form[data-form-coleta] div[data-linha-coleta]').not(this).addClass('naoSalvar');

        } else {

            $('form[data-form-coleta] div[data-linha-coleta]').css("background-color", "#FFFFFF")
                .find('input, button').prop("disabled", false);
            $('form[data-form-coleta] div[data-linha-coleta]').removeClass('naoSalvar');
            
        }
    })
