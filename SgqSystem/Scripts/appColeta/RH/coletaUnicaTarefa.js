
$(document).on('keyup click change', 'form[data-form-coleta] div[data-linha-coleta]', function () {

    if (globalDicionarioEstatico) {
        if (globalDicionarioEstatico.clustersIdsColetaUnicaTarefa == undefined) {
            globalDicionarioEstatico = JSON.parse(globalDicionarioEstatico);
        }
    } else {
        openMessageConfirm(
            "Objeto de coleta inválido",
            "Entre em contato com o responsável do SESMT da sua Unidade! Não atualizar a página e tirar prints da tela por favor.",
            closeMensagemImediatamente,
            closeMensagemImediatamente,
            "red",
            "white");
    }

    if ($(this).hasClass('naoSalvar')
        || globalDicionarioEstatico.clustersIdsColetaUnicaTarefa.indexOf('|' + currentParCluster_Id + '|') < 0)
        return;

    var isNA = $(this).attr('data-conforme-na') == "";

    if (($(this).attr('data-input-type') == "11" && $(this).find('input[data-texto]').val() && $(this).find('input[data-texto]').val() != '')
        || ($(this).find('input[data-valor]').length > 0 && $(this).find('input[data-valor]').val() && $(this).find('input[data-valor]').val() != '')
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

        $('.headerFieldL3, .headerFieldL3 input, .headerFieldL3 select').not('[parlevel1id="' + $(this).attr('data-level1') + '"][parlevel2id="' + $(this).attr('data-level2') + '"][parlevel3id="' + $(this).attr('data-level3') + '"]')
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

        $('.headerFieldL3, .headerFieldL3 input, .headerFieldL3 select')
            .removeClass('naoSalvar')
            .css("background-color", "#FFFFFF")
            .find('input, button').prop("disabled", false);

    }
});
