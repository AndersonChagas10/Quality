var storage = window.localStorage;
var indicatorId = storage.getItem("indicatorId");
var indicatorName = storage.getItem("indicatorName");
var indicatorImage = JSON.parse(storage.getItem("indicatorImage"));

var date = new Date();
var year  = date.getFullYear();
var month = date.getMonth() + 1;
var day   = date.getDate();
var yyyymmdd = year + ("0" + month).slice(-2) +""+ ("0" + day).slice(-2);


$("#indicatorName").text(indicatorName);

if(indicatorImage){
    $("#indicatorImage").show();
}else{
    $("#indicatorImage").hide();
}

$(function() {
    $(".carcassMenu").hide();
	$(".cffMenu").hide();
	$(".htpMenu").hide();

	switch (parseInt(indicatorId)) {
		case 3:
			$(".carcassMenu").show();
			break;
		case 6:
			$(".cffMenu").show();
			break;
	}

    $('.monitoramento-item').children('.row').children('.defects').text('0')

    $.each($('.monitoramento-item'), function(e, d){

        $.get(urlTarefa, { indicadorId: indicatorId, idMonitoramento: $(d).attr("Id"), dateInit: yyyymmdd, dateEnd: yyyymmdd}, function (r) { 
 
                $.each($('.monitoramento-item'), function(e, d){
                    storage.setItem("defects_"+indicatorId+"_"+d.id, 0);
                    var totalDoMonitoramento = 0;
                    
                    $.each(r.Retorno,function(a,b){ 
                        if(b.Id_Monitoramento == d.id){
                            totalDoMonitoramento += b.NotConform;
                            
                            $('#'+b.Id_Monitoramento).children('.row').children('.defects').text(totalDoMonitoramento.toString())
                            var defects = storage.setItem("defects_"+indicatorId+"_"+d.id, totalDoMonitoramento);
                        }
                    });
                    
                });

                

        });

    });
    
    //var maior = 0;
    //var menor=0;
    //verificar a maior avaliacao...
    //pegar menores avaliacaoes e bloquear
    //pegar menores avaliacoes e zera defects
    //fazer a conta do total defects

    $.get(urlNcMonitoramento , { idIndicador: indicatorId, dateInit: yyyymmdd, dateEnd: yyyymmdd}, function(r){

  //  var totalDefects = 0;

    $('.monitoramento-item').children('.row').children('.inspections').children('.actualInspection').text('0')
    $.each($('.monitoramento-item'), function(e, d){
            storage.setItem("inspections_"+indicatorId+"_"+d.id, '0/10');
            $.each(r.Retorno,function(a,b){ 

                if(b.Id_Monitoramento == d.id){
                  
                    /*
                    if(b.Evaluate >= maior){
                          maior = b.Evaluate;
                    }
                    else
                    {
                          menor = b.Evaluate;
                    }
                    */

                    $('#'+b.Id_Monitoramento).children('.row').children('.inspections').children('.actualInspection').text((b.Evaluate%10)).attr('atInsp', b.Evaluate%10)
                    storage.setItem("inspections_"+indicatorId+"_"+b.Id_Monitoramento, (b.Evaluate%10)+'/10');
                }
            });
        });
        
        /*
        $('.actualInspection[atinsp="' + maior +  '"]').parents('.monitoramento-item').addClass('disabled')
        $('.actualInspection[atinsp="' + menor +  '"]').parents('.monitoramento-item').children('.row').children('.defects').text('0');
        if($('.actualInspection[atinsp="' + maior +  '"]').length == $('.monitoramento-item').length )
        {
            $('.actualInspection[atinsp="' + maior +  '"]').parents('.monitoramento-item').removeClass('disabled');
        }
        */

        var totalDefects = 0;
        $('.defects').each(function(e){
            $that = $(this).text();
            totalDefects = totalDefects + parseInt($(this).text());
        });
        $("#totalDefects").text(totalDefects);
        storage.setItem("totalDefects", totalDefects);
    });

     


    // var url =  "/SGQDevInterno/api/RelatorioBetaApi/GetNcPorMonitoramentoJelsafa"
    // $.get(url , { idIndicador: 3, dateInit: '20160712', dateEnd: '20160712'}, function(r){

    // $('.monitoramento-item').children('.row').children('.inspections').text('0/10')
    // $.each($('.monitoramento-item'), function(e, d){

    //         $.each(r.Retorno,function(a,b){ 

    //             if(b.Id_Monitoramento == d.id)
    //                 $('#'+b.Id_Monitoramento).children('.row').children('.inspections').text(b.Evaluate.toString()+'/10')
    //         });
    //     });
    // });
});

function openMonitoring(monitoringId){
    if(!$('#'+monitoringId).hasClass('disabled')){
        storage.setItem("monitoringId", monitoringId);
        storage.setItem("monitoringName", getMonitoring(monitoringId).Name);
        window.location="../view/tasks.html";
    }
    
}

function backIndicators(){
    $(location).attr('href', '../view/indicators.html');
}

function showMonitorings(monitorings) {
    var out = "";
    var i;
    for(i = 0; i<monitorings.length; i++) {
		if(monitorings[i].Indicator == indicatorId){
			out += lineMonitoring(monitorings[i]);
		}
    }
    $( ".monitorings" ).append(out);
}

function getMonitoring(Id){
    for(var i = 0; i < monitorings.length; i++){
        if(monitorings[i].Id == Id){
            return monitorings[i];
        }
    }
}

function getCurrentMonitorings(monitorings){
	var current = new Array();
	for(i = 0; i<monitorings.length; i++) {
		if(monitorings[i].Indicator == indicatorId)
        	current.push(monitorings[i]);
    }
	return current;
}

function getNotConformMonitorings(){
    
    $.get(urlMonitoramento, { idIndicador: 3, dateInit: '20160712', dateEnd: '20160712' }, function (Monitoramentos) { 
        ms = Monitoramentos.Retorno;
    });
}

function getCountNC(){
    
}

