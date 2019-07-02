
var trocarSequencial = false;
function getPCC1BNext() {
    
    var level1 = $('.level1.PCC1B');

    var alertaNivel1 = parseFloat(level1.attr('alertanivel1'));
    var alertaNivel2 = parseFloat(level1.attr('alertanivel2'));
    var alertaNivel3 = parseFloat(level1.attr('alertanivel3'));
    // var nctraseiro = parseInt($('.level2 .levelName:contains("Traseiro")').parent().attr('resultadodefeitos')) || 0;
    // var ncdianteiro = parseInt($('.level2 .levelName:contains("Dianteiro")').parent().attr('resultadodefeitos')) || 0;
    // var totalNC = nctraseiro + ncdianteiro;

    //connectionServer = false;

    //se tiver conexão tenta recuperar o próximo
    if (connectionServer && parseInt($(".banda:visible").val()) == 2) {

        if (trocarSequencial == true) {
            var sequencial = !parseInt($('.sequencial:visible').val()) == false ? parseInt($('.sequencial:visible').val()) : 1;

            var data = { Data: yyyyMMdd(), Unit: parseInt($('.App').attr('unidadeid')), ParLevel2: parseInt($('.level2.selected').attr('id')), sequencialAtual: sequencial };

            $.ajax({
                url: urlPreffix + "/api/PCC1B/Next",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify(data),
                type: 'POST',
                headers: token(),
                success: function (r) {
                    
                if (r.Sequential > 0 || r.Side > 0) {

                    // $('.Resultlevel2[level2id=' + $('.level2.selected').attr('id') + '][shift='+$('.App').attr('shift')+'][period='+$('.App').attr('period')+']')
                    //     .attr('Sequential', r.Sequential)
                    //     .attr('Side', r.Side);

                    //setPCC1BMonitoramento(r.Sequential, r.Side, alertaNivel1, alertaNivel2, alertaNivel3, totalNC, ncdianteiro, nctraseiro);
                    setPCC1BMonitoramento(r.Sequential, r.Side, alertaNivel1, alertaNivel2, alertaNivel3);

                    trocarSequencial = false;
                } else {
                    offLinePCC1B();
                }

                setTimeout(function () {
                    closeModalPCC1B();
                }, 300);
                
                },
                error: function (e) {
                    setTimeout(function () {
                        closeModalPCC1B();
                        openModalPCC1B();
                        offLinePCC1B();
                    }, 300);
                }
            });
        }       
        
    } else {
        if(trocarSequencial == true)
            offLinePCC1B();        
    }

    updatePCC1B("start");
        
}

function offLinePCC1B() {
    var level1 = $('.level1.PCC1B');

    var alertaNivel1 = parseFloat(level1.attr('alertanivel1'));
    var alertaNivel2 = parseFloat(level1.attr('alertanivel2'));
    var alertaNivel3 = parseFloat(level1.attr('alertanivel3'));
    // var nctraseiro = parseInt($('.level2 .levelName:contains("Traseiro")').parent().attr('resultadodefeitos')) || 0;
    // var ncdianteiro = parseInt($('.level2 .levelName:contains("Dianteiro")').parent().attr('resultadodefeitos')) || 0;
    // var totalNC = nctraseiro + ncdianteiro;

    var sequencial = 1;
    var banda = 0;

    var sequencialList = $(".Resultlevel2[collectiondate='" + getCollectionDate() + "'][level2id='" + $('.level2.selected').attr('id') + "'][shift="+$('.App').attr('shift')+"][period="+$('.App').attr('period')+"]").map(function () {
        return parseInt(this.getAttribute('sequential')) || 1;
    }).toArray();

    if (sequencialList.length > 0) {
        sequencial = Math.max.apply(Math, sequencialList) || 1;
    }

    var bandaList = $(".Resultlevel2[collectiondate='" + getCollectionDate() + "'][level2id='" + $('.level2.selected').attr('id') + "'][sequential='" + sequencial + "'][shift="+$('.App').attr('shift')+"][period="+$('.App').attr('period')+"]").map(function () {
        return parseInt(this.getAttribute('side')) || 1;
    }).toArray();

    if (bandaList.length > 0)
        banda = Math.max.apply(Math, bandaList) || 1;

    if (banda >= 2) {
        sequencial = sequencial + 1;
        banda = 1;
    } else {
        banda = banda + 1;
    }

    //setPCC1BMonitoramento(sequencial, banda, alertaNivel1, alertaNivel2, alertaNivel3, totalNC, ncdianteiro, nctraseiro);
    setPCC1BMonitoramento(sequencial, banda, alertaNivel1, alertaNivel2, alertaNivel3);
    updateDefectsCounters();

    setTimeout(function () {
        closeModalPCC1B();
    }, 1000);

    trocarSequencial = false;
    closeModalPCC1B();
}

function updatePCC1B(useReload, callback) {

    if ($('.level1.selected.PCC1B').length > 0 && $('.level02Result[sync=false]').length == 0) {
        var data = { HashKey: parseInt($(".level1.selected").attr('hashkey')), Data: yyyyMMdd(), Unit: parseInt($('.App').attr('unidadeid')), ParLevel2: parseInt($('.level2.selected').attr('id')) };

        var corrigido = false;

        var parLevel2IdDianteiro = $('.level2 .levelName:contains("Dianteiro")').parent().attr('id');
        var parLevel2IdTraseiro = $('.level2 .levelName:contains("Traseiro")').parent().attr('id');

        
        $.ajax({
            url: urlPreffix + "/api/PCC1B/TotalNC/" + parLevel2IdDianteiro + "/" + parLevel2IdTraseiro + "/" + $('.App').attr('shift'),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(data),
            type: 'POST',
            headers: token(),
            success: function (r) {
                
                pcc1bList = r;
                setPcc1bFile();
                updateDefectsCounters();
    
            
                if (useReload == undefined)
                setTimeout(updatePCC1B, 1000);

            if (callback) {
                callback();
            }
            },
            error: function (e) {
                if (useReload == undefined)
                    setTimeout(updatePCC1B, 1000);
    
                if (callback) {
                    callback();
                }
            }
        });
    }    
}

function closeModalPCC1B() {
    $('.modalPCC1B, .overlay').fadeOut('fast');
};

function openModalPCC1BProximoSequencial() {

    if ($('.level3Group.PCC1B').is(':visible')) {
        $('.modalPCC1B, .overlay').fadeIn('fast');

        $('.modalPCC1B').empty();

        var listHtml = $("<ul class='list-group'></ul>");

        var title = $("<li class='list-group-item active'>Próximo PCC1B</li>");
        appendDevice(title, listHtml);

        var label = $("<li class='list-group-item item'><div class='row'><div class='col-xs-2'><i class='fa fa-circle-o-notch fa-spin fa-2x fa-fw'></i>" +
                "</div><div class='col-xs-10'>"+ getResource("loading_next_sequential") +" </div></div>"
                + "</li>");
        appendDevice(label, listHtml);

        appendDevice(listHtml, $('.modalPCC1B'));

    }

};


function openModalPCC1B() {

    if ($('.level3.PCC1B').is(':visible')) {

        $('.modalPCC1B, .overlay').fadeIn('fast');

        $('.modalPCC1B').empty();

        var listHtml = $("<ul class='list-group'></ul>");

        var title = $("<li class='list-group-item active'>Próximo PCC1B</li>");
        appendDevice(title, listHtml);

        var label = $("<li class='list-group-item item'>" +
                       getResource("error_recovery_sequential")
                    + "</li>");
        appendDevice(label, listHtml);

        var btnManual = $("<button class='btn btn-default btn-block' id='btnPCC1BProximoManual'>Próximo Manual</button>");
        var btnAutomatico = $("<button class='btn btn-default btn-block' id='btnPCC1BAutomatico'>Automático</button>");

        appendDevice(btnManual, listHtml);
        appendDevice(btnAutomatico, listHtml);

        appendDevice(listHtml, $('.modalPCC1B'));

        $("#btnPCC1BProximoManual").click(function () {
            closeModalPCC1B();
        });
        $("#btnPCC1BAutomatico").click(function () {
            closeModalPCC1B();
            getPCC1BNext();
        });

    }
};


//function setPCC1BMonitoramento(sequencial, banda, nivel1, nivel2, nivel3, totalNC, ncdianteiro, nctraseiro) {
function setPCC1BMonitoramento(sequencial, banda, nivel1, nivel2, nivel3) {
    // if (totalNC != null)
    //     $('.totalnc:visible').text(totalNC);
    // if (ncdianteiro != null)
    //     $('.ncdianteiro:visible').text(ncdianteiro);
    // if (nctraseiro != null)
    //     $('.nctraseiro:visible').text(nctraseiro);

    volumePCC = parseFloat($(_level1).attr('volumealertaindicador').replace(',', '.')).toFixed(2);
    metaPCC = parseFloat($(_level1).attr('metaindicador').replace(',', '.')).toFixed(2);

    volumePCC = volumePCC * (metaPCC / 100);

    $('.nivel1:visible').text((volumePCC / 3).toFixed(2).toString().replace('.',','));
    $('.nivel2:visible').text((volumePCC / 3 * 2).toFixed(2).toString().replace('.', ','));
    $('.nivel3:visible').text((volumePCC).toFixed(2).toString().replace('.', ','));
    $('.sequencial:visible').val(sequencial);
    $('.banda:visible').val(banda);
};

var PCC1BSequence = '';
function addPCC1BSequence(sequence) {
    PCC1BSequence += sequence;

    if (PCC1BSequence == '12') {
        //showPCC1BStatus();
        PCC1BSequence = "";
    } else {
        switch (PCC1BSequence.length) {
            case 1:
                if (PCC1BSequence[0] != "1") {
                    PCC1BSequence = "";
                }
                break;
            case 2:
                if (PCC1BSequence[1] != "2") {
                    PCC1BSequence = "";
                }
                break;
        }
    }
};

//apagar o relatório
function apagarDIVPCC1bSIF(){
    $('.relatorioSIFPCC1b').remove()
}


$(document).off('click', '.level3Group.PCC1B .painel .btn').on('click', '.level3Group.PCC1B .painel .btn', function (e) {
   // if($('.totalnc:visible').parent().parent().parent().length > 0){
    //    hidePCC1BStatus();
   // }else{
        //cria a dIV do relatório
        $('.level3Group.PCC1B').not('.hide').prepend('<div class="relatorioSIFPCC1b" onClick="apagarDIVPCC1bSIF();" style="position: absolute; min-height:800px; width:100%; background-color:#f5f5f5; z-index:999999; left:0px;"></div>')

        //cria a tabela
        var tabelaResultadoSIFPCC1b = "<table border=1 align=center style='border: 1px solid #b8b8b8;'><tr><td style='padding:0px 10px; font-weight:bold; text-align:center'>Quarto</td><td style='padding:0px 10px; font-weight:bold; text-align:center'>Sequencial</td><td style='padding:0px 10px; font-weight:bold; text-align:center'>Banda</td><td style='padding:0px 10px; font-weight:bold; text-align:center'>Resultado</td></tr>";
           
       if(typeof(resultadoPCC1bSIF) != 'undefined'){
           
            //Preenche a tabela com os dados
            $(resultadoPCC1bSIF).each(
                function(i,o){ 
                    var resultadodefeitosPCC1b = "";
                    if($(o).attr("resultado") == 1){
                        resultadodefeitosPCC1b = "<td style='padding:0px 10px; text-align: center; background: red; color: white;'>1</td>"
                
                    }else if ($(o).attr("resultado") == 2){
                        resultadodefeitosPCC1b = "<td style='padding:0px 10px; text-align: center; background: black; color: white;'>NA</td>"
                
                    }else{
                        resultadodefeitosPCC1b = "<td style='padding:0px 10px; text-align: center;'>0</td>";
                
                    }
                    tabelaResultadoSIFPCC1b += "<tr>";
                    tabelaResultadoSIFPCC1b += "<td style='padding:0px 10px; text-align: center;'>" + $(o).attr("monitoramento") + "</td>";
                    tabelaResultadoSIFPCC1b += "<td style='padding:0px 10px; text-align: center;'>" + $(o).attr("sequential") + "</td>";
                    tabelaResultadoSIFPCC1b += "<td style='padding:0px 10px; text-align: center;'>" + $(o).attr("side") + "</td>";
                    tabelaResultadoSIFPCC1b += resultadodefeitosPCC1b;
                    tabelaResultadoSIFPCC1b += "</tr>"; 
                })

            tabelaResultadoSIFPCC1b += "</table>"; 

            //coloca na tela o relatório do SIF do PCC1b
            $(".relatorioSIFPCC1b").html("<h3><strong>Apontamentos do Indicador PCC1b</strong></h3> <br>" + tabelaResultadoSIFPCC1b)

        }

    //}
    
});

/*function hidePCC1BStatus() {
    $('.totalnc:visible').parent().parent().parent().fadeOut('fast');
    $('.ncdianteiro:visible').parent().parent().parent().fadeOut('fast');
    $('.nctraseiro:visible').parent().parent().parent().fadeOut('fast');
    $('.nivel1:visible').parent().parent().parent().fadeOut('fast');
}
function showPCC1BStatus() {
    $('.totalnc').parent().parent().parent().fadeIn('fast');
    $('.ncdianteiro').parent().parent().parent().fadeIn('fast');
    $('.nctraseiro').parent().parent().parent().fadeIn('fast');
    $('.nivel1').parent().parent().parent().fadeIn('fast');
}*/

var file_pcc1b = "pcc1b.json";
var pcc1bList = [];

function setPcc1bFile(){
    _writeFile(file_pcc1b, JSON.stringify(pcc1bList));
}

function getPcc1bFile(){
    _readFile(file_pcc1b, function(r){
        if(r){
            pcc1bList = JSON.parse(r);
        }else{
            pcc1bList = [];
        }
    });
}

function updateDefectsCounters(){
    var nctraseiro = 0;
    var ncdianteiro = 0;

    $.grep(pcc1bList, function (o, i) {
        if(o.CollectionDate == getCollectionDate() && o.UnitId == $('.App').attr('unidadeid')){
            if($('.level2 .levelName:contains("Traseiro")').parent().attr('id') == o.ParLevel2_Id){
                nctraseiro = nctraseiro + o.DefectsResult;
            }
            if($('.level2 .levelName:contains("Dianteiro")').parent().attr('id') == o.ParLevel2_Id){
                ncdianteiro = ncdianteiro + o.DefectsResult;
            }
        }
    });
    
    var totalNC = nctraseiro + ncdianteiro;

    $('.painelLevel03:visible .totalnc').text(totalNC);
    $('.painelLevel03:visible .ncdianteiro').text(ncdianteiro);
    $('.painelLevel03:visible .nctraseiro').text(nctraseiro);

    $('.level1.selected').attr('totaldefeitos', totalNC);
}

function setDefectsCounters(ParLevel2_Id, DefectsResult, Key, Sequential, Side){
    var obj = $.grep(pcc1bList, function (o, i) { 
        if(o.CollectionDate == getCollectionDate() && o.UnitId == $('.App').attr('unidadeid') && 
            ParLevel2_Id == o.ParLevel2_Id && Sequential == o.Sequential && Side == o.Side){ 
                return o;
        }
    });

    if (obj[0]) {
        obj[0].DefectsResult = parseInt(DefectsResult);
    } else {
        pcc1bList.push({
            CollectionDate : getCollectionDate(),
            DefectsResult : DefectsResult,
            Key : Key,
            ParLevel1_Id : $('.level1.selected').attr('id'),
            ParLevel2_Id : ParLevel2_Id,
            Sequential : Sequential,
            Side : Side,
            UnitId : $('.App').attr('unidadeid')
        });
    }

    updateDefectsCounters();
    setPcc1bFile();
}

var sincronizando = false;
function sincronizarResultadoPCC1B(){
    if(sincronizando == false){
        try {
            sincronizando = true;
            $.ajax({
                url: urlPreffix + "/api/RelatorioGenerico/reciveDataPCC1b2/" + $('.App').attr('unidadeid') + "/"
                 + getCollectionDate().substring(4, 8) + getCollectionDate().substring(0, 2) + getCollectionDate().substring(2, 4) + "/" 
                 + $('.App').attr('shift'),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: 'GET',
                headers: token(),
                success: function (data) {
                    resultadoPCC1bSIF = data;
                },
                error: function (e) {
                }
            });
        } catch (e) {
        }
        //Se der erro, irá liberar para sincronizar novamente após 2 minutos
        setTimeout(function() {
            sincronizando = false;
        }, 20000);
    }
}