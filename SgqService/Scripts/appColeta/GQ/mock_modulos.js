var level01ListClone;

var modulos = {
    quente : [
        '(%) NC nas Operações de Esfola',
        '(%) NC PCC 1B',
        '(%) NC Uso de Bastão Elétrico em Animais',
        '(%) NC Animais Mal Insensibilizados na Calha de Sangria',
        '(%) NC Banho de Aspersão',
        '(%) NC Análises Laboratoriais de Subprodutos',
        '(%) NC CEP Miúdos',
        '(%) NC CEP Carnes Industriais',
        '(%) Verificação da Tipificação',
        '(%) Frequência de Verificação da Tipificação',
        '(%) Conformidade no IQM',
        '(%) NC PPHO',
        '(%) NC Análise de Água (Físico-Químicas)',
        '(%) NC Análise de Água (Microbiológicos)',
        '(%) NC Contaminantes Físicos do Abate',
        '(%) NC Escorregões de Animais - Desembarque/Manejo',
        '(%) NC Quedas de Animais - Desembarque/Manejo',
        'Auditoria Miúdos',
        'Auditoria Subprodutos',
        '% NC GRD Etiquetas'
    ],
    fria : [
        '(%) NC CEP Desossa',
        '(%) NC Temperatura',
        '(%) NC Vácuo - Processo',
        '(%) NC Vácuo - Equipamentos',
        '(%) NC CEP Carnes Industriais',
        '(%) Conformidade no IQM',
        '(%) NC PPHO',
        '(%) NC Higienização das Câmaras',
        '(%) NC no Controle da Aspersão',
        '(%) NC Análise Microbiológica de Produto Final',
        '(%) NC no Monitoramento de Carne com Osso',
        '(%) NC CEP Recortes',
        '(%) NC Espaçamento Entre Meias-Carcaças',
        '(%) NC Análise Microbiológica em Carcaça',
        '(%) NC Análise Microbiológica em Produto (Desossa)',
        '(%) NC Expedição',
        'Auditoria Diferença de Peso',
        'Auditoria Embarque',
        'Gestão de Produtos Bloqueados',
        'Qualidade da Matéria Prima (Nº de ocorrência)',
        '% NC GRD Etiquetas'

    ],
    manutencao : [
        '(%) NC em Auditoria Mensal de Manutenção'
    ],
    grd : [
        '(%) NC CEP Vácuo GRD'
    ]
};

function loadInd(){

    level01ListClone = $('.level1List').clone();
    
    $('body .level1List').remove();
    $('.breadcrumb').remove();

    level01ListClone.find('.userInfo').empty();
    //appendDevice($('<input type="radiobutton" class="check pull-right">'), level01ListClone.find('.userInfo'));

    
    appendDevice($('<div class=\"modalSyncInd\" style=\"display:none; margin-bottom: 50px;\"></div>'), $('.App'));

    var div =   $('<ul class="list-group ">'+
                    '<li id="" class="row list-group-item">'+
                        '<a id="" href="#" class="col-xs-7" >'+
                            '<span class="levelName"style="">Indicadores da Área Quente</span>'+
                        '</a>'+
                        '<div id="" class="userInfo col-xs-5">'+
                            '<input class="pull-right modulo check quente" name="modulos" type="radio"/>'+
                        '</div>'+
                    '</li>'+
                    '<li id="" class="row list-group-item">'+
                        '<a id="" href="#" class="col-xs-7" >'+
                            '<span class="levelName" style="">Indicadores da Área Fria</span>'+
                        '</a>'+
                        '<div id="" class="userInfo col-xs-5">'+
                            '<input class="pull-right modulo check fria" name="modulos" type="radio"/>'+
                        '</div>'+
                    '</li>'+
                    '<li id="" class="row list-group-item">'+
                        '<a id="" href="#" class="col-xs-7" >'+
                            '<span class="levelName" style="">Auditoria Mensal de Manutenção</span>'+
                        '</a>'+
                        '<div id="" class="userInfo col-xs-5">'+
                            '<input class="pull-right modulo check manutencao" name="modulos" type="radio"/>'+
                        '</div>'+
                    '</li>'+
                    '<li id="" class="row list-group-item">'+
                        '<a id="" href="#" class="col-xs-7" >'+
                            '<span class="levelName" style="">CEP Vácuo GRD</span>'+
                        '</a>'+
                        '<div id="" class="userInfo col-xs-5">'+
                            '<input class="pull-right modulo check grd" name="modulos" type="radio"/>'+
                        '</div>'+
                    '</li>'+
                '</ul>');

    appendDevice(div, $('.modalSyncInd'));    

    appendDevice($('<button class="btn-block btn btn-primary btnConfirmSyncInd">Confirmar</button>'), $('.modalSyncInd'));

    $('.modalSyncInd').fadeIn('fast');

    $('.modalSyncInd .level1').off();

    //$('.check').attr('checked', true);
    
}

var listIndUpdate = '';
$(document).on('click', '.btnConfirmSyncInd', function(){

    if($('.modulo.check:checked').length == 0){
        openMessageModal("Selecione um módulo para continuar.", "");
        return;
    }

    level01ListClone.find('.level1 .levelName').each(function(i, o){
        if($('.modulo.check.quente:checked').length == 1){
            $.grep(modulos.quente, function(m) {
                if(m == $(o).text())
                    listIndUpdate += $(o).parent().attr('id')+",";
            });
        }
        if($('.modulo.check.fria:checked').length == 1){
            $.grep(modulos.fria, function(m) {
                if(m == $(o).text())
                    listIndUpdate += $(o).parent().attr('id')+",";
            });
        }
        if($('.modulo.check.manutencao:checked').length == 1){
            $.grep(modulos.manutencao, function(m) {
                if(m == $(o).text())
                    listIndUpdate += $(o).parent().attr('id')+",";
            });
        }
        if($('.modulo.check.grd:checked').length == 1){
            $.grep(modulos.grd, function(m) {
                if(m == $(o).text())
                    listIndUpdate += $(o).parent().attr('id')+",";
            });
        }
    });

    // $('.modalSyncInd .check:checked').parents('.userInfo').siblings('.level1').each(function(i, o){
    //     listIndUpdate += $(o).attr('id') + ',';
    // });

    ping(paramsUpdate_OnLine, paramsUpdate_OffLine);

    $('.modalSyncInd, .overlay').fadeOut('fast');

    loadFirst = false;
});

