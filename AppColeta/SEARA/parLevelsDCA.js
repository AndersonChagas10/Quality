function listarParLevelsDCA() {

    var levels = GetLevelsDCA();

    openColetaDCA(levels);

}

function GetLevelsDCA() {

    level1List = [];

    montarLevel1(level1List);

    level1List = retornaLevels1Planejados(level1List);

    return level1List;
}

function montarLevel1DCA(level1List) {

    var parVinculos = $.grep(parametrization.listaParVinculoPeso, function (obj) {
        return obj.ParLevel1_Id == currentParLevel1_Id && obj.ParLevel2_Id == currentParLevel2_Id;
    });

    var level1_Ids_Aux = $.map(parVinculos, function (obj) {
        return obj.ParLevel1_Id;
    });

    //Força que o ID seja unico
    var level1_Ids = [];

    level1_Ids_Aux.forEach(function (level1_Id) {

        if (level1_Ids.indexOf(level1_Id) < 0) {
            level1_Ids.push(level1_Id);
        }

    });

    level1_Ids.forEach(function (level1_Id) {

        var levels1 = $.grep(parametrization.listaParLevel1, function (obj) {
            return obj.Id == level1_Id;
        });

        levels1.forEach(function (parLevel1) {

            level1List.push(parLevel1);

            montarLevel2(parLevel1, parVinculos);

        });

    });
}

function montarLevel2DCA(parLevel1,parVinculos) {

    var level2_Ids = $.map(parVinculos, function (obj) {
        return obj.ParLevel2_Id;
    });

    level2_Ids = $.unique(level2_Ids.sort());

    var level2List = [];

    level2_Ids.forEach(function (parLevel2_Id) {

        var Level2 = $.grep(parametrization.listaParLevel2, function (parLevel2) {
            return parLevel2.Id == parLevel2_Id;
        });

        Level2.forEach(function (parLevel2, index) {
            level2List.push(parLevel2);
            montarLevel3(parLevel1, parLevel2, parVinculos);
        });

    });

    parLevel1['ParLevel2'] = level2List;

}

var parVinculosMontarLevel3 = [];

function montarLevel3DCA(parLevel1, parLevel2, parVinculos) {

    parVinculosMontarLevel3 = $.grep(parVinculos, function (obj) {
        return true;
    });

    var level3_Ids = $.map(parVinculosMontarLevel3, function (obj) {
        return obj.ParLevel3_Id;
    });

    level3_Ids = $.unique(level3_Ids.sort());

    var level3List = [];

    level3_Ids.forEach(function (parLevel3_Id) {

        var Level3 = $.grep(parametrization.listaParLevel3, function (parLevel3) {
            return parLevel3.Id == parLevel3_Id && vinculoPesoIsValid(parLevel1, parLevel2, parLevel3, parVinculosMontarLevel3);
        });

        Level3.forEach(function (level3) {

            var vinculo = $.grep(parVinculosMontarLevel3, function (obj) {
                return level3.Id == obj.ParLevel3_Id;
            });

            level3["Peso"] = vinculo[0].Peso;

            level3["ParLevel3InputType"] = getInputType(level3, parLevel2, parLevel1);

            level3["ParLevel3Value"] = getParLevel3Value(level3, parLevel2, parLevel1);

            level3["ParLevel3BoolTrue"] = getParLevel3BoolTrue(level3.ParLevel3Value);

            level3["ParLevel3BoolFalse"] = getParLevel3BoolFalse(level3.ParLevel3Value);

            level3["ParLevel3XHelp"] = getParLevel3XHelp(level3);

            level3List.push(level3);

        });

    });

    parLevel2.ParLevel3 = level3List;
}