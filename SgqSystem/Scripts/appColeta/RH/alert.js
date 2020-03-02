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
    var modal = "";
    var body = "";
    var salvaAcaoCorretiva = "";
    var corpo = "";
    var display = "";
    var btnShowAcaoCorretiva = "";
    var numeroDeAlertas = n;

    setTimeout(function () {

        var alerta = 
            '<div style="background-color:red; padding:10px;">' +
            '<div>' +
            '<p>Alerta ' + numeroDeAlertas + ' (' + exists[0].Name + ') foi disparado.</p>' +
            '</div>' +
            '<div style="text-align:center">' +
            '</div>' +
            '</div>';

        if(exists[0].HasCorrectiveAction){
            display = 'block';
            fillAcaocorretiva();
        }
        else
        {
            display = 'none';
            btnShowAcaoCorretiva = '<div>' +
                '<button class="btn btn-secundary" data-id style="float:left;">Mostrar Ação Corretiva</button>' +
                '</div>';
            fillAcaocorretiva();
        }
        
        function fillAcaocorretiva(){
            modal = '<h3 style="font-weight:bold; display:' + display + ';">Ação Corretiva</h3>';
            body = '<div class="form-group" style="display:' + display + ';">' +
                '<div class="form-group col-xs-12">' +
                '<strong>Informações</strong>' +
                '<small><br/>Data/Hora: ' + currentCollectDate.toLocaleDateString() + ' ' + currentCollectDate.toLocaleTimeString() +
                '<br/>Monitor: ' + currentLogin.Name +
                '<br/>Tarefa: ' + $.grep(parametrization.listaParLevel3, function (o, i) { return o.Id == coleta.ParLevel3_Id; })[0].Name +
                '<br/>Frequência: ' + $.grep(parametrization.listaParFrequency, function (item) { return item.Id == parametrization.currentParFrequency_Id; })[0].Name +
                '</small></div>' +
            
                '<div class="form-group col-xs-12">' +
                '<label>Descrição da Falha:</label>' +
                '<input name="DescriptionFailure" id="descriptionFailure" class="col-sx-12 form-control" style="height: 80px;">' +
                '</div>' +
                '<div class="form-group col-xs-12">' +
                '<label for="email">Ação Corretiva Imediata:</label>' +
                '<input name="ImmediateCorrectiveAction" id="immediateCorrectiveAction" class="form-control" style="height: 80px;">' +
                '</div>' +
                '<div class="form-group col-xs-12">' +
                '<label for="email">Ação Preventiva:</label>' +
                '<input name="PreventativeMeasure" id="preventativeMeasure" class="form-control" style="height: 80px;">' +
                '</div>';
            salvaAcaoCorretiva = '<button class="btn btn-primary" id="btnSendCA" style="display:' + display + ';">Salvar Ação Corretiva</button>';
            
            corpo = 
                '<div class="container">' +
                '<div class="row" style="overflow:auto">' +
                '<div>' +
                alerta +                
                '</div>' +
                btnShowAcaoCorretiva +
                '<hr>' +
                modal +
                '<hr>' +
                '<div>' +
                body +
                '</div>' +
                '<hr>' +
                '<div class="form-group col-xs-6">' +
                salvaAcaoCorretiva +
                '</div>' +
                '</div>' +
                '</div>';
        }

        openModal(corpo, 'white', 'black');

        $('body').off('click', '[data-id]').on('click', '[data-id]', function (e) {
            display = 'block';
            fillAcaocorretiva();
            openModal(corpo, 'white', 'black');
        });

        // openMensagem('Alerta ' + numeroDeAlertas + ' (' + exists[0].Name + ') foi disparado.', 'red', 'white');
        // //closeMensagem(3000);

        // if (exists[0].HasCorrectiveAction) {
        //     //Verificar se disparou alerta e se existe ação corretiva - Caso existir, abre o modal - após salvar a ação corretiva abre a função abaixo;
        //     setTimeout(function () {
        //         OpenCorrectiveAction(coleta);
        //     }, 3100);
        // }
    }, 3500);
}