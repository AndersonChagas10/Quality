$(document).ready(function () {
    $("body").off("keypress", "input.mask-decimal").on("keypress", "input.mask-decimal", function (e) {

        var ponto = 46;
        var virgula = 44;

        if (e.which !== 8 && e.which !== 0 && (e.which < 48 || e.which > 57) && e.which !== ponto && e.which !== virgula) {
            return false;
        }

        var numeroCaracteres = $(e.target).val().length;
        var valorDentroDoInput = $(e.target).val();

        if ((e.which == ponto || e.which == virgula)
            && (valorDentroDoInput.indexOf('.') >= 0
                || valorDentroDoInput.indexOf(',') >= 0)) {
            return false;
        }
    });

    $("body").off("keyup", "input.mask-decimal").on("keyup", "input.mask-decimal", function (e) {

        var valorDentroDoInput = $(e.target).val().replace(',', '.');

        $(e.target).val(valorDentroDoInput);
    });
});