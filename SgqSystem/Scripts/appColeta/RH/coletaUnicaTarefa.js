
$(document).on('keyup click change', 'form[data-form-coleta] div[data-linha-coleta]', function () {

    if ($(this).find('input, button').prop('disabled')
        || globalDicionarioEstatico.clustersIdsColetaUnicaTarefa.indexOf('|' + currentParCluster_Id + '|') < 0)
        return;

    var isNA = $(this).attr('data-conforme-na') == "";
        
    if (($(this).attr('data-input-type') == "11" && $(this).find('input[data-texto]').val() != 'undefined' && $(this).find('input[data-texto]').val() != '')
        || ($(this).find('input[data-valor]').length > 0 && $(this).find('input[data-valor]').val() != 'undefined' && $(this).find('input[data-valor]').val() != '')
        || ($(this).find('button[data-binario]').length > 0 && $(this).attr('data-conforme') != $(this).attr('data-default-answer'))
        || isNA) {

        $('form[data-form-coleta] div[data-linha-coleta]')
            .not(this)
            .css("background-color", "#999")
            .find('input, button')
            .prop("disabled", true);

        $('form[data-form-coleta] div[data-linha-coleta]')
            .not(this)
            .addClass('naoSalvar');
            
        $('.headerFieldL3').not('[parlevel1id="' + $(this).attr('data-level1') + '"][parlevel2id="' + $(this).attr('data-level2') + '"][parlevel3id="' + $(this).attr('data-level3') + '"]')
            .addClass('naoSalvar')
            .not(this)
            .css("background-color", "#999")
            .find('input, button').prop("disabled", true);

    } else {

        $('form[data-form-coleta] div[data-linha-coleta]').css("background-color", "#FFFFFF")
            .find('input, button')
            .prop("disabled", false);

        $('form[data-form-coleta] div[data-linha-coleta]')
            .removeClass('naoSalvar');

        $('.headerFieldL3')
            .removeClass('naoSalvar')
            .css("background-color", "#FFFFFF")
            .find('input, button').prop("disabled", false);
        
    }
})
