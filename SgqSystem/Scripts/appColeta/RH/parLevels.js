function listarParLevels() {

    var levels = GetLevels();

    openColeta(levels);

}

function GetLevels() {

    level1List = [];

    montarLevel1(level1List);

    montarLevel2(level1List);

    montarLevel3(level1List);

    return level1List;
}

function montarLevel1(level1List) {

    var parVinculos = $.grep(parametrization.listaParVinculoPeso, function (obj) {
        return (obj.ParDepartment_Id == currentParDepartment_Id || obj.ParDepartment_Id == null) && (obj.ParCargo_Id == currentParCargo_Id || obj.ParCargo_Id == null);
    });

    var level1_Ids = $.map(parVinculos, function (obj) {
        return obj.ParLevel1_Id;
    });

    level1_Ids = $.unique(level1_Ids);

    level1_Ids.forEach(function (level1_Id) {

        var levels1 = $.grep(parametrization.listaParLevel1, function (obj) {
            return obj.Id == level1_Id;
        });

        levels1.forEach(function (obj) {
            level1List.push(obj);
        });

    });
}

function montarLevel2(level1List) {

    level1List.forEach(function (parLevel1) {

        var parVinculos = $.grep(parametrization.listaParVinculoPeso, function (obj) {
            return (obj.ParDepartment_Id == currentParDepartment_Id || obj.ParDepartment_Id == null) &&
                (obj.ParCargo_Id == currentParCargo_Id || obj.ParCargo_Id == null) && obj.ParLevel1_Id == parLevel1.Id;
        });

        var level2_Ids = $.map(parVinculos, function (obj) {
            return obj.ParLevel2_Id;
        });

        level2_Ids = $.unique(level2_Ids);

        var level2List = [];

        level2_Ids.forEach(function (parLevel2_Id) {

            var Level2 = $.grep(parametrization.listaParLevel2, function (parLevel2) {
                return parLevel2.Id == parLevel2_Id;
            });

            Level2.forEach(function (item) {
                level2List.push(item);
            });

        });

        parLevel1['ParLevel2'] = level2List;
    });

}

function montarLevel3(level1List) {

    level1List.forEach(function (parLevel1) {

        parLevel1.ParLevel2.forEach(function (parLevel2, index) {

            var parVinculos = $.grep(parametrization.listaParVinculoPeso, function (obj) {
                return (obj.ParDepartment_Id == currentParDepartment_Id || obj.ParDepartment_Id == null) &&
                    (obj.ParCargo_Id == currentParCargo_Id || obj.ParCargo_Id == null) &&
                    obj.ParLevel1_Id == parLevel1.Id && obj.ParLevel2_Id == parLevel2.Id;
            });

            var level3_Ids = $.map(parVinculos, function (obj) {
                return obj.ParLevel3_Id;
            });

            level3_Ids = $.unique(level3_Ids);

            var level3List = [];

            level3_Ids.forEach(function (parLevel3_Id) {

                var Level3 = $.grep(parametrization.listaParLevel3, function (parLevel3) {
                    return parLevel3.Id == parLevel3_Id;
                });

                Level3.forEach(function (level3) {

                    level3["ParLevel3InputType"] = getInputType(level3, parLevel2, parLevel1);

                    level3["ParLevel3Value"] = getParLevel3Value(level3, parLevel2, parLevel1);

                    level3["ParLevel3BoolTrue"] = getParLevel3BoolTrue(level3.ParLevel3Value);

                    level3["ParLevel3BoolFalse"] = getParLevel3BoolFalse(level3.ParLevel3Value);

                    level3List.push(level3);
                });

            });

            parLevel1.ParLevel2[index].ParLevel3 = level3List;

        });
    });
}

function getParLevel3BoolTrue(parLevel3Value) {

    if (parLevel3Value && parLevel3Value.ParLevel3BoolTrue_Id)
        return $.grep(parametrization.listaParLevel3BoolTrue, function (item) { return item.Id == parLevel3Value.ParLevel3BoolTrue_Id })[0];
}

function getParLevel3BoolFalse(parLevel3Value) {

    if (parLevel3Value && parLevel3Value.ParLevel3BoolFalse_Id)
        return $.grep(parametrization.listaParLevel3BoolFalse, function (item) { return item.Id == parLevel3Value.ParLevel3BoolFalse_Id })[0];

}

function getParLevel3Value(level3, parLevel2, parLevel1) {

    var level3Values = $.grep(parametrization.listaParLevel3Value, function (parLevel3Value) {
        return (parLevel3Value.ParLevel3_Id == level3.Id && parLevel3Value.ParLevel2_Id == parLevel2.Id && parLevel3Value.ParLevel1_Id == parLevel1.Id)
    });

    if (level3Values && level3Values.length > 0)

        return level3Values[0];

}

function getInputType(level3, parLevel2, parLevel1) {

    var level3Values = getParLevel3Value(level3, parLevel2, parLevel1);

    if (level3Values)

        var parLevel3InputTypes = $.grep(parametrization.listaParLevel3InputType, function (level3InputType) {
            return (level3InputType.Id == level3Values.ParLevel3InputType_Id)
        });

    if (parLevel3InputTypes && parLevel3InputTypes.length > 0)

        return parLevel3InputTypes[0];

}