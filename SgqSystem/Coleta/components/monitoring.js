function lineMonitoring(monitoring, defect, inspection) {

    var out = '';
	var storage = window.localStorage;
	
    switch(monitoring.Indicator) {
        case 3:
			var cont1 = storage.getItem("defects_"+monitoring.Indicator+"_"+monitoring.Id);
			var cont2 = storage.getItem("inspections_"+monitoring.Indicator+"_"+monitoring.Id);

			if(cont1 == null || cont1 == 'NaN'){
				cont1 = 0;
				storage.setItem("defects_"+monitoring.Indicator+"_"+monitoring.Id, 0);
			}
			if(cont2 == null || cont2 == 'NaN'){
				cont2 = 0;
				storage.setItem("inspections_"+monitoring.Indicator+"_"+monitoring.Id, 0);
			}
            out = "<a id="+monitoring.Id+" class='Level2-item list-group-item' onclick='openMonitoring("+monitoring.Id+")'>"+
					"<div class='row'>"+
						"<div class='col-xs-8'>"+
							"<h4 class='list-group-item-heading'>"+monitoring.Name+" </h4>"+
						"</div>"+
						"<div class='col-xs-2 text-center defects'></div>"+
						"<div class='col-xs-2 text-center inspections'><span class='actualInspection'></span>/<span class='totalInspection'>10</span></div>"+
					"</div>"+
				"</a>";
            break;
        case 6:			
            var cont1 = storage.getItem("sets_"+monitoring.Indicator+"_"+monitoring.Id);
			var cont2 = storage.getItem("sides_"+monitoring.Indicator+"_"+monitoring.Id);
			var errors = storage.getItem("cff_errors");

			if(cont1 == null || cont1 == 'NaN'){
				cont1 = 1;
				storage.setItem("sets_"+monitoring.Indicator+"_"+monitoring.Id, 1);
			}
			if(cont2 == null || cont2 == 'NaN'){
				cont2 = 1;
				storage.setItem("sides_"+monitoring.Indicator+"_"+monitoring.Id, 1);
			}
			if(errors == null || errors == 'NaN'){
				errors = 0;
				storage.setItem("cff_errors", 0);
			}

            out = "<a id="+monitoring.Id+" class='Level2-item list-group-item audit-item' onclick='openMonitoring("+monitoring.Id+")'>"+
					"<div class='row'>"+
						"<div class='col-xs-8'>"+
							"<h4 class='list-group-item-heading audit-desc'>"+monitoring.Name+"</h4>"+
						"</div>"+						
						"<div class='col-xs-2 text-center'>"+cont1+"</div>"+
						"<div class='col-xs-2 text-center'>"+cont2+"</div>"+
					"</div>"+	
				"</a>";
            break;
		case 2:
            out = "<a id="+monitoring.Id+" class='Level2-item list-group-item audit-item' onclick='openMonitoring("+monitoring.Id+")'>"+
					"<div class='row'>"+
						"<div class='col-xs-8'>"+
							"<h4 class='list-group-item-heading audit-desc'>"+monitoring.Name+"</h4>"+
						"</div>"+						
					"</div>"+	
				"</a>";
            break;
        default:
            out = '<a class="Level2-item list-group-item"><div class="row">Invalid indicator type</div></a>';
    }
    return out;
}

function getDefect(p_indicator, p_monitoring, p_date){
	var retorno = "";

	var func = function(r){
		var a =  r.Retorno.filter(function (n) {
						return n.Id_Level2 == p_monitoring;
					});
					return a.length
	}

	$.when(
		/* AJAX requests */
			$.get(urlLevel2, { idIndicador: p_indicator, dateInit: p_date, dateEnd: p_date }, func)
	).then(function(m) {
		/* Run after all AJAX */
		var a =  m.Retorno.filter(function (n) {
						return n.Id_Level2 == p_monitoring;
					});
			console.log(m.length);
			//return a.length
	});
	
}