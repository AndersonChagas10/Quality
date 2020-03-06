function setBreadcrumbsDCA() {

    var breadcrumb = '<ol class="breadcrumb"><li><a onclick="validaRota(listarParLevel1,null)">Inicio</a></li>';
    var breadcrumbLi = "";
    var isCurrent = true;

    if (currentParLevel2_Id) {
        breadcrumbLi = getBreadcrumb($.grep(parametrization.listaParLevel2, function (item) {
            return item.Id == currentParLevel2_Id;
        })[0].Name, '', isCurrent) + breadcrumbLi;

        isCurrent = false;
    }

    if (currentFamiliaProdutoDCA_Id) {
        breadcrumbLi = getBreadcrumb(getParFamiliaProduto().Name, 'validaRota(listarParLevel2DCA)', isCurrent) + breadcrumbLi;
        isCurrent = false;
    }

    /*if (currentParLevel1_Id) {
        breadcrumbLi = getBreadcrumb($.grep(parametrization.listaParLevel1, function (item) {
            return item.Id == currentParLevel1_Id;
        })[0].Name, 'validaRota(listarFamiliaProdutoDCA)', isCurrent) + breadcrumbLi;

        isCurrent = false;
    }*/

    breadcrumb += breadcrumbLi + '</ol>';

    $('.panel-heading').prepend(breadcrumb);

}