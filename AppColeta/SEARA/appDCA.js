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

function validaParqualification(level1Id, level2Id, level3Id) {

    var listaParLevel3ValueFiltrada = [];
    var listaParQualificationxParLevel3Value = [];

    parametrization.listaParLevel3Value.forEach(function (o, i) {
        if (o.ParLevel1_Id == level1Id && o.ParLevel2_Id == level2Id && o.ParLevel3_Id == level3Id)
            listaParLevel3ValueFiltrada.push(o);
    });

    if (listaParLevel3ValueFiltrada.length > 0) {

        listaParQualificationxParLevel3Value =
            $.grep(parametrization.listaPargroupQualificationXParLevel3Value, function (element, index) {
                if (element.ParLevel3Value_Id == listaParLevel3ValueFiltrada[0].Id)
                    return element;
            });
    }

    return listaParQualificationxParLevel3Value;
}