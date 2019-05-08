var sendVer = true;
var sendVerConf = true;
var vfCounter = setInterval(function (e) {
    sendVer = true;
}, 10000);

//sync verificacao da tipificacao
function sendVerificacaoTipificacao() {
    
    if (connectionServer && sendVer && sendVerConf) {

        var vfList = $('.VerificacaoTipificacao').children('.vf[sync=false]');

        if(vfList.length > 0) {

            //var VerificacaoTipificacao = [];
            //var VerificacaoTipificacaoResultados = [];



            vfList.each(function (index, self) {

                var VerificacaoTipificacao = [];
                var VerificacaoTipificacaoResultados = [];

                VerificacaoTipificacao.push({
                    Id: 0,
                    EvaluationNumber: $(self).attr('avaliacao'),
                    Sample: $(self).attr('amostra'),
                    Sequencial: $(self).attr('sequencial'),
                    Banda: $(self).attr('banda'),
                    DataHora: decodeURI($(self).attr('data')),
                    UnidadeId: $(self).attr('idunidade'),
                    Chave: $(self).attr('verificacaoTipificacaoChave'),
                    Status: 0,
                });

                $('.VerificacaoTipificacaoResultados')
                    .children('div[sync=false][chave=' + $(self).attr('verificacaoTipificacaoChave') + ']')
                    .each(function (index, self) {
                        var data = {
                            TarefaId: $(self).attr('tarefaid'),
                            CaracteristicaTipificacaoId: $(self).attr('caracteristicatipificacaoid'),
                            Chave: $(self).attr('chave'),
                            AreasParticipantesId: $(self).attr('areaparticipantesid')
                        };

                    VerificacaoTipificacaoResultados.push(data);
                }); 

                    var modelVerificacao = {
                        EmpresaId: 1,
                        DepartamentoId: 1,
                        TarefaId: 1,
                        MonitoramentoId: 1,
                        ProdutoId: 1,
                        UnidadeId: parseInt($('.App').attr('unidadeid')),
                        OperacaoId: 1,
                        VerificacaoTipificacao: VerificacaoTipificacao,
                        AuditorId: $('.App').attr('userid'),
                        VerificacaoTipificacaoResultados: VerificacaoTipificacaoResultados
                    };
                        
                    sendVer = false;
                    sendVerConf = false;
                    
                    var abc = function callback(){

                        sendVerConf = true;
                                
                        var chave = $(self).attr('verificacaoTipificacaoChave');
                        var dataChave = new Date(chave.substring(chave.length - 8, chave.length - 4), parseInt(chave.substring(chave.length - 4, chave.length - 2))-1, chave.substring(chave.length - 2, chave.length), 0, 0, 0, 0);

                        $(self).attr('sync', true);
                        $('.VerificacaoTipificacaoResultados').children('div[sync=false][chave=' + $(self).attr('verificacaoTipificacaoChave') + ']').attr('sync', true);

                        
                    
                        _writeFile("VerificacaoTipificacaoResultados.txt", $(".VerificacaoTipificacaoResultados").html());
                        _writeFile("VerificacaoTipificacao.txt", $(".VerificacaoTipificacao").html());

                    }

                    $.post(urlPreffix + "/api/VTVerificacaoTipificacao/Save", modelVerificacao, function () {

                        abc();

                    }).fail(function(){
                        sendVerConf = true;
                });
            });

            
        }

    }
}

$(document).on('click', '.level3.VF a', function (e) {
    e.preventDefault();

    var that = $(this);

    var type = $(this).parents('.level3.VF').find('.items').attr('listtype');
    $('.modalVF, .overlay').fadeIn('fast');

    $('.modalVF').empty();

    var listHtml = $("<ul class='list-group'></ul>");

    var title = $("<li class='list-group-item active'>" + $(this).text() + "</li>");
    appendDevice(title, listHtml);

    $(this).parents('.level3.VF').find('.items').children('div').each(function (index, self) {
        if (type == 'single') {
            var label = $("<li class='list-group-item item' id='" + $(self).attr('cNrCaracteristica') + "'><label class='radio-inline'><input type='radio' name='optradio'>" + $(self).text() + "</label></li>");
            appendDevice(label, listHtml);
        } else if (type == 'multiple') {
            var label = $("<li class='list-group-item item' id='" + $(self).attr('cNrCaracteristica') + "'><label class='checkbox-inline'><input type='checkbox' value=''>" + $(self).text() + "</label></li>");
            appendDevice(label, listHtml);
        }
    });

    var button = $("<button class='btn btn-primary btn-block btn-lg'>OK</button>");
    appendDevice(button, listHtml);

    appendDevice(listHtml, $('.modalVF'));

    $('.modalVF').find('.item').each(function () {
        $(this).find('input').prop('checked', !$('.level3.VF').find('.items').children('div[cnrcaracteristica=' + $(this).attr('id') + ']').hasClass('hide'));
    });

});

$(document).on('click', '.modalVF .btn', function (e) {
    e.preventDefault();

    $('.modalVF .item label input[type=radio]').each(function (index, self) {
        if ($(this).is(':checked')) {
            $('.VF').find('.items').children('div [cnrcaracteristica="' + $(this).parents('label').parents('.item').attr('id') + '"]').removeClass('hide');
        } else {
            $('.VF').find('.items').children('div [cnrcaracteristica="' + $(this).parents('label').parents('.item').attr('id') + '"]').addClass('hide');
        }
    });

    $('.modalVF .item label input[type=checkbox]').each(function (index, self) {
        if ($(this).is(':checked')) {
            $('.VF').find('.items').children('div [cnrcaracteristica="' + $(this).parents('label').parents('.item').attr('id') + '"]').removeClass('hide');
        } else {
            $('.VF').find('.items').children('div [cnrcaracteristica="' + $(this).parents('label').parents('.item').attr('id') + '"]').addClass('hide');
        }
    });

    var title = $('.modalVF .list-group-item.active').text();

    if (title.indexOf('Participantes') >= 0) {
        var ids = "";
        $('.modalVF .list-group-item[id] label input:checked').each(function (index, self) {
            ids += $(self).parent().parent().attr('id')+";";
        });
        localStorage.setItem("areasparticipantes", ids);
    }

    $('.modalVF, .overlay').fadeOut('fast');
});

function setAreasParticipantes() {
    var ids = localStorage.getItem("areasparticipantes");

    if (ids != null) {
        var array = ids.split(";");

        array.forEach(function (self, index) {

            if (self != "") {
                $('.modalVF .list-group-item[' + self + '] label input').val(true);
            }

            $('.VF').find('.items').children('div [cnrcaracteristica="' + self + '"]').removeClass('hide');
        });
    }
    
}

function updateVerificacaoHeaders() {
    var evaluateCurrent = 0;
    var sampleCurrent = 0;

    var avaliacaoList = $(".VerificacaoTipificacao .vf").map(function () {
        var chave = this.getAttribute('verificacaotipificacaochave');
        if (yyyyMMdd() == chave.substring(chave.length - 8, chave.length)) {
            return parseInt(this.getAttribute('avaliacao')) || 1;
        }
    }).toArray();
    if (avaliacaoList.length > 0) {
        evaluateCurrent = Math.max.apply(Math, avaliacaoList) || 1;
    }
    var amostraList = $(".VerificacaoTipificacao .vf").map(function () {
        var chave = this.getAttribute('verificacaotipificacaochave');
        if (yyyyMMdd() == chave.substring(chave.length - 8, chave.length)) {
            return parseInt(this.getAttribute('amostra')) || 1;
        }
    }).toArray();

    if (amostraList.length > 0)
        sampleCurrent = Math.max.apply(Math, amostraList) || 1;

    updateEvaluateSample($('.level2.selected'), $('.level3Group:visible'), evaluateCurrent, sampleCurrent);
}


function createVFConsolidation(unidade, operacao, sequencial, banda, data, tarefaId, monitoramentoId) {

    var chave = unidade + '' + operacao + '' + sequencial + '' + banda + '' + data;

    var items = $('.level3.VF').find('.items').children('div:visible');

    var consolidation = $("<div class='vf'></div>");

    consolidation.attr('sequencial', sequencial)
                 .attr('banda', banda)
                 .attr('idunidade', unidade)
                 .attr('data', encodeURI(getCollectionDateFormat().toUTCString()))
                 .attr('verificacaoTipificacaoChave', chave)
                 .attr('empresaId', 1)
                 .attr('monitoramentoId', monitoramentoId)
                 .attr('tarefaId', tarefaId)
                 .attr('produtoId', 1)
                 .attr('departamentoId', 1)
                 .attr('versaoApp', 1)
                 .attr('sync', false)
                 .attr('avaliacao', $('.level3Group:visible .painelLevel03 .evaluateCurrent').text())
                 .attr('amostra',  $('.level3Group:visible .painelLevel03 .sampleCurrent').text())
                 .attr('usuarioId', $('.App').attr('userid'));

    if ($('.VerificacaoTipificacao').children('.vf[verificacaotipificacaochave=' + chave + ']').length > 0){
        replaceWithDevice(consolidation, $('.VerificacaoTipificacao').children('.vf[verificacaotipificacaochave=' + chave + ']'));
    }
    else {
        appendDevice(consolidation, $('.VerificacaoTipificacao'));
    }

    if ($('.ResultsKeysVF').children('div[key=' + chave + ']').length == 0){
        var key = $("<div class='Key' date=\""+getCollectionDate()+"\" unidadeid=\""+$('.App').attr('unidadeid')+"\" key=\"" + chave + "\"></div>");
        appendDevice(key, $('.ResultsKeysVF'));
    }

}
function loginVerificacaoTipificacao() {
    _readFile("VerificacaoTipificacao.txt",
        function (r) {
            appendDevice($(r), $('.VerificacaoTipificacao'));
        }
    );
    _readFile("VerificacaoTipificacaoResultados.txt",
        function (r) {
            appendDevice($(r), $('.VerificacaoTipificacaoResultados'));
        }
    );
}

function loginVerificacaoKeys(){
    _readFile("VerificacaoTipificacaoKeys.txt",
        function (r) {
            appendDevice($(r), $('.ResultsKeysVF'));
            _writeFile("VerificacaoTipificacaoKeys.txt", $('.ResultsKeysVF').html());
        }
    );
}

function getAllVF(){
    if($('.ResultsKeysVF').length == 0)
        $('.ResultsKeys').after($('<div class="ResultsKeysVF"></div>'));

    $.ajax({
        headers: token(),
        url: urlPreffix + "/api/VTVerificacaoTipificacao/GetAll/"+getCollectionDate()+"/"+$('.App').attr('unidadeid'),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'GET',
        success: function (data,status,xhr) {
            if(status == "success"){
                if(data){
                    $('.ResultsKeysVF').remove();
                    var keys = $('<div class="ResultsKeysVF"></div>').append($(data));
                    $('.ResultsKeys').after(keys);
                }
            } else {
                loginVerificacaoKeys();
            }
        },
        error: function (e) {
            loginVerificacaoKeys();
        }
    });
}
