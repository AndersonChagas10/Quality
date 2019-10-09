$(document).on('click', '.overlay', function (e) {
    if ($('.btnAreaSaveConfirm:visible').length) {
        $(this).hide();
        $('.btnAreaSaveConfirm:visible').addClass('hide').siblings('.btnAreaSave').removeAttr('disabled');
    }
});