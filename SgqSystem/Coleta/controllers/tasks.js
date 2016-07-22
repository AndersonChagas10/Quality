var storage = window.localStorage;
var indicatorId = storage.getItem("indicatorId");
var monitoringId = storage.getItem("monitoringId");
var monitoringName = storage.getItem("monitoringName");
var sidesErrors = storage.getItem("sides_with_errors");

var numero1_string = "numero1_"+indicatorId+"_"+monitoringId;
var numero2_string = "numero2_"+indicatorId+"_"+monitoringId;
var numero1 = storage.getItem(numero1_string);
var numero2 = storage.getItem(numero2_string);

$(function() {
    if(checkGroupsExist() == true){
		$("#showExpand").show();
		$("#showNa").hide();
	}else{
		$("#showExpand").hide();
		$("#showNa").show();
	}

	if(numero1 == null){
		storage.setItem(numero1_string, 1);
	}
	if(numero2 == null){
		storage.setItem(numero2_string, 1);
	}

	if(sidesErrors == null){
		storage.setItem("sides_with_errors", 0);
	}

	if(storage.getItem("side-errors") == null)
		storage.setItem("side-errors", 0);

	numero1 = storage.getItem(numero1_string);
    numero2 = storage.getItem(numero2_string);
	$("#numero1").text(numero1+"/5");
	$(".cont1").text(numero2+"/10");

	$("#carcassPanel").hide();
	$("#cffPanel").hide();
	$("#htpPanel").hide();

	switch (parseInt(indicatorId)) {
		case 3:
			$("#carcassPanel").show();
			var inspections = storage.getItem("inspections_"+indicatorId+"_"+monitoringId);
            var defects = storage.getItem("defects_"+indicatorId+"_"+monitoringId);
			numero2 = parseInt(inspections)+ 1;
			$("#inspections").text(inspections);
			$("#defects").text(defects);
			$(".total-sides-errors-label").text(storage.getItem("totalDefects"));
			break;
		case 6:
			$("#cffPanel").show();
			var sets = storage.getItem("sets_"+indicatorId+"_"+monitoringId);
            var sides = storage.getItem("sides_"+indicatorId+"_"+monitoringId);
			var cff_errors = storage.getItem("cff_errors");
			$("#sets").text(sets);
			$("#sides").text(sides);
			$("#side-errors").text(cff_errors);
			if(parseInt($('#side-errors').text()) >=6 ) {
				$('.total-sides-errors').addClass("divRed");
			}
			break;
	}
});

$("#monitoringName").text(monitoringName);

function checkGroupsExist(){
	var currentTasks = getCurrentTasks(tasks);
	for(var i = 0; i < currentTasks.length; i++){
		if(currentTasks[i].Group != null){
			return true;
		}
	}
	return false;
}

function showTasks(tasks, taskgroups) {
    var out = "";
    var i;
	
    for(i = 0; i<tasks.length; i++) {
		if(tasks[i].Indicator == indicatorId){
			if(tasks[i].Group == null)
			{
				out += lineTask(tasks[i]);
			}
			else
			{
				var group = getCurrentGroup(tasks[i].Group);
				showGroupTask(group);
				$('#group-task-body-'+group.Id).append(lineTask(tasks[i]));
			}
		}        	
    }
    $( ".tasks" ).append(out);
	
}

function showGroupTask(taskgroup){
	if(! $('#group-task-'+taskgroup.Id).length ) 
	{
		var out = lineGroupTask(taskgroup);
    	$( ".tasks" ).append(out);
	}	
}

function getCurrentTasks(tasks){
	var current = new Array();
	for(i = 0; i<tasks.length; i++) {
		if(tasks[i].Indicator == indicatorId)
        	current.push(tasks[i]);
    }
	return current;
}

function getCurrentGroup(Id){
	for(i = 0; i<taskgroups.length; i++) {
		if(taskgroups[i].Id == Id)
        	return taskgroups[i];
    }
	return null;
}

function saveResult(results) {

	var _source = [
        {
        'Id_Level3': 13,
        'Id_Level1': 2,
        'Id_Level2': 2,
        'Evaluate': 2,
        'NotConform': 2,
		'numero1': numero1,
		'numero2': numero2,
        'Id': 0,
        'AddDate': new Date().toISOString(),
        'AlterDate': new Date().toISOString(),
        },{
        'Id_Level3': 14,
        'Id_Level1': 2,
        'Id_Level2': 2,
        'Evaluate': 2,
        'NotConform': 2,
		'numero1': numero1,
		'numero2': numero2,
        'Id': 0,
        'AddDate': new Date().toISOString(),
        'AlterDate': new Date().toISOString(),
        }
      ]
	
	$.ajax({
        type: "POST",
        dataType: "json",
        url: urlSaveResultList,
        data: { listaResultado: results },
        success: function (data) {
            if(data.Mensagem != null){
                console.log(data.MensagemErro);
				reloadPage();
            }
            if(data.MensagemExcecao != null){
                console.log(data.MensagemExcecao);
            }
            if(data.Inner != null){
                console.log(data.Inner);
            }
            if(data.Retorno != null){
                console.log(data.Retorno);
            }

			if(data.Mensagem != null && data.MensagemExcecao == null)        
			{
				console.log(data.Mensagem);// Exiber mensagem para o Usuario
			}
			else 
				console.log(data.MensagemExcecao)// Quando ocorrer EXCECOES
			if(data.Inner != null)
				console.log(data.Inner) //NAO EXISBIR ALEM DO CONSOLE ESTA LINHA.
			if(data.Retorno != null)
				console.log(data.Retorno)//Utilizar esta situação quando a ação retornar um OBJETO.

        },
        error: function (error) {
            jsonValue = jQuery.parseJSON(error.responseText);
        },
        complete: function(){ 
            console.log('Aways Execute this one "Complete"');
        }
    });
}

$( "#btnCancel" ).mousedown(function() {
	
});

$("#btnCorrectiveAction").click(function () {
    $(location).attr('href', '../view/correctiveaction.html');
});


$( "#btnSave" ).mousedown(function() {


	var resultsArray = [];

	var currentTasks = getCurrentTasks(tasks);

	for(var i = 0; i < currentTasks.length; i++){
		var result = new Result();

		result.setId_Level3(currentTasks[i].Id);
		result.setId_Level1(indicatorId);
		result.setId_Level2(monitoringId);
		if(indicatorId == '6')
			result.setId_Level2(currentTasks[i].Monitoring);
		result.setId(0);
		result.setNumero1(numero1);
		result.setNumero2(numero2);
		result.setEvaluate(1);
		result.setPeriod(storage.getItem("periodo"));
		result.setReaudit();
		result.setAuditor(storage.getItem("userId"));
		var reauditValue = $('#reaudit').is(':checked') ? 1 : 0;
		result.setReaudit(reauditValue);

		if($($('.audit-item')[i]).length > 0){
			if($($('.audit-item')[i]).hasClass('disabled'))
				result.setEvaluate(0);
			else
				result.setEvaluate(1);
		}

		if($($('.audit-item')[i]).length > 0){
			result.setNotConform(parseInt($($('.audit-item')[i]).find('.audit-value').val()));
		}

		if($($('.item_data_answer')[i]).length > 0){
			if($($('.item_data_answer')[i]).text() == "No"){
				result.setNotConform(0);
			}else
				result.setNotConform(1);
		}		
		
		resultsArray.push(result.getObject());
		
	}

	saveResult(resultsArray);

});

function reloadPage(){

	switch (parseInt(indicatorId)) {
		case 3:
			$(location).attr('href', '../view/monitorings.html');
			break;
		case 6:
			var sets = storage.getItem("sets_"+indicatorId+"_"+monitoringId);
            var sides = storage.getItem("sides_"+indicatorId+"_"+monitoringId);
			storage.setItem("sides_"+indicatorId+"_"+monitoringId, parseInt(sides)+1);
			
			if(parseInt($($('.total-errors-label')).text()) > 0){
				storage.setItem("cff_errors", parseInt(storage.getItem("cff_errors"))+1);
			}

			 if(parseInt(sets) >= 5 && parseInt(sides) >= 10){
				storage.setItem("sets_"+indicatorId+"_"+monitoringId, 1);
				storage.setItem("sides_"+indicatorId+"_"+monitoringId, 1);
				storage.setItem("cff_errors", 0);
				$(location).attr('href', '../view/monitorings.html');
			}
			else if(parseInt(sides) >= 10){
				storage.setItem("sets_"+indicatorId+"_"+monitoringId, parseInt(sets)+1);
				storage.setItem("sides_"+indicatorId+"_"+monitoringId, 1);
				$(location).attr('href', '../view/monitorings.html');
			}else{
				location.reload();
			}
			break;
		case 2:
			$(location).attr('href', '../view/monitorings.html');
			break;
	}

	
	/*storage.setItem(numero2_string, parseInt(storage.getItem(numero2_string))+1);
	if(parseInt(storage.getItem(numero2_string)) >= 10){
		storage.setItem(numero1_string, parseInt(storage.getItem(numero1_string))+1);
		storage.setItem(numero2_string, 1);
	}

	$("#numero1").text(numero1+"/5");
	$("#numero2").text(numero2+"/10");
	if(parseInt($('.total-errors-label').text()) > 0){
		storage.setItem("sides_with_errors", parseInt(sidesErrors)+1);
		storage.setItem("cont1_"+indicatorId+"_"+monitoringId, 
			parseInt(storage.getItem("cont1_"+indicatorId+"_"+monitoringId))+1);
	}*/
		
}