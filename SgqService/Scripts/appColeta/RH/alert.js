function processAlertRole(coletaJson) {

    if (coletaJson.length == 0)
        return;

    var listaParAlertPreFiltrada = $.grep(parametrization.listaParAlert, function (o, i) {
        return (o.ParDepartment_Id == coletaJson[0].ParDepartment_Id || o.ParDepartment_Id == null)
            && (o.ParCargo_Id == coletaJson[0].ParCargo_Id || o.ParCargo_Id == null)
    });

    for (var i = 0; i < coletaJson.length; i++) {

        var coleta = coletaJson[i];

        if (coleta.IsConform)
            continue;

        //retorna se existe alguem alerta vigente para este cenario
        var exists = $.grep(listaParAlertPreFiltrada, function (o, i) {
            return (o.ParCompany_Id == coleta.ParCompany_Id || o.ParCompany_Id == null)
                && (o.ParLevel1_Id == coleta.ParLevel1_Id || o.ParLevel1_Id == null)
                && (o.ParLevel2_Id == coleta.ParLevel2_Id || o.ParLevel2_Id == null)
                && o.ParLevel3_Id == coleta.ParLevel3_Id
                && o.ParAlertType_Id == 1;
        });

        //retorna todos os alertas para somar o numero do alerta
        var numeroDeAlertas = $.grep(currentAlertsAgrupados, function (o, i) {
            return (o.ParDepartment_Id == coleta.ParDepartment_Id || o.ParDepartment_Id == null)
                && (o.ParCargo_Id == coleta.ParCargo_Id || o.ParCargo_Id == null)
                && (o.ParCompany_Id == coleta.ParCompany_Id || o.ParCompany_Id == null)
                && (o.ParLevel1_Id == coleta.ParLevel1_Id || o.ParLevel1_Id == null)
                && (o.ParLevel2_Id == coleta.ParLevel2_Id || o.ParLevel2_Id == null)
                && o.ParLevel3_Id == coleta.ParLevel3_Id
        }).length;

        if (exists.length > 0) {

            numeroDeAlertas++;

            setTimeoutOpenCorrectiveAction(exists,coleta,numeroDeAlertas)

            currentAlertsAgrupados.push({
                ParDepartment_Id: coleta.ParDepartment_Id,
                ParCargo_Id: coleta.ParCargo_Id,
                ParCompany_Id: coleta.ParCompany_Id,
                ParLevel1_Id: coleta.ParLevel1_Id,
                ParLevel2_Id: coleta.ParLevel2_Id,
                ParLevel3_Id: coleta.ParLevel3_Id,
                Number: numeroDeAlertas
            });
        }
    }
}

function setTimeoutOpenCorrectiveAction(e,c,n){
    var exists = e;
    var coleta = c;
    var numeroDeAlertas = n;

    setTimeout(function () {
        openMensagem('Alerta ' + numeroDeAlertas + ' (' + exists[0].Name + ') foi disparado.', 'red', 'white');
        closeMensagem(3000);

        if (exists[0].HasCorrectiveAction) {
            //Verificar se disparou alerta e se existe ação corretiva - Caso existir, abre o modal - após salvar a ação corretiva abre a função abaixo;
            setTimeout(function () {
                OpenCorrectiveAction(coleta);
            }, 3100);
        }
    }, 3500);
}