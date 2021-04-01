$(document)
    .off('click change keyup blur', 'form[data-form-coleta] div[data-linha-coleta] button, form[data-form-coleta] div[data-linha-coleta] input')
    .on('click change keyup blur', 'form[data-form-coleta] div[data-linha-coleta] button, form[data-form-coleta] div[data-linha-coleta] input', function () {
        var tag = $(this).prop("tagName");
        var linhaColeta = $(this).parents('[data-linha-coleta]')[0];
        if (globalDicionarioEstatico) {
            if (globalDicionarioEstatico.clustersIdsColetaUnicaTarefa == undefined) {
                globalDicionarioEstatico = JSON.parse(globalDicionarioEstatico);
            }
        } else {
            openMessageConfirm(
                "Objeto de coleta inválido",
                "Entre em contato com o responsável do SESMT da sua Unidade! Não atualizar a página e tirar print da tela por favor.",
                closeMensagemImediatamente,
                closeMensagemImediatamente,
                "red",
                "white");
        }
        if ($(linhaColeta).hasClass('naoSalvar')
            || globalDicionarioEstatico.clustersIdsColetaUnicaTarefa.indexOf('|' + currentParCluster_Id + '|') < 0)
            return;
        var isNA = $(linhaColeta).attr('data-conforme-na') == "";

        if ( (tag == "BUTTON" && $(linhaColeta).find('input[data-texto]').length > 0
            && $(linhaColeta).find('input[data-texto]').val() == ''
            && $(linhaColeta).attr('data-conforme') != $(linhaColeta).attr('data-default-answer'))
            || (tag == "INPUT" && $(linhaColeta).find('input[data-texto]').length > 0
            && ($(linhaColeta).find('input[data-texto]').val() != '') || $(linhaColeta).attr('data-conforme') != $(linhaColeta).attr('data-default-answer')))
            {

            $('form[data-form-coleta] div[data-linha-coleta]')
                .not(linhaColeta)
                .addClass('naoSalvar');

            $('form[data-form-coleta] div[data-linha-coleta]')
                .not(linhaColeta)
                .css("background-color", "#999");

            $('form[data-form-coleta] div[data-linha-coleta]')
                .not(linhaColeta)
                .find('input, button')
                .prop("disabled", true);

            $('.headerFieldL3, .headerFieldL3 input, .headerFieldL3 select')
                .not('[parlevel1id="' + $(linhaColeta).attr('data-level1') + '"][parlevel2id="' + $(linhaColeta).attr('data-level2') + '"][parlevel3id="' + $(linhaColeta).attr('data-level3') + '"]')
                .addClass('naoSalvar')
                .not(linhaColeta)
                .css("background-color", "#999")
                .find('input, button')
                .prop("disabled", true);

            if ($(this).val().length > 0 && $(linhaColeta).attr('data-conforme') == $(linhaColeta).attr('data-default-answer')) {
                setBinaryFieldProperties($(linhaColeta), $(linhaColeta).find('button[data-binario]'));
                $('[data-salvar]').prop('disabled', false);
            }
        }
        else {
            $('form[data-form-coleta] div[data-linha-coleta]')
                .removeClass('naoSalvar');
            $('form[data-form-coleta] div[data-linha-coleta]')
                .css("background-color", "#FFFFFF")
            $('form[data-form-coleta] div[data-linha-coleta]')
                .find('input, button')
                .prop("disabled", false);
            $('.headerFieldL3, .headerFieldL3 input, .headerFieldL3 select')
                .removeClass('naoSalvar')
                .css("background-color", "#FFFFFF")
                .find('input, button')
                .prop("disabled", false);
            
            if ($(this).val().length == 0 && (tag == "INPUT" || tag == "BUTTON") && $(linhaColeta).attr('data-conforme') != $(linhaColeta).attr('data-default-answer')) {
                $(linhaColeta).find('button[data-binario]').trigger('click');
                $('[data-salvar]').prop('disabled', true);
            }
        }
    });