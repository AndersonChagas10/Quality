

function lineIndicator(indicator) {

    var out = '';
    switch(indicator.Type) {
        case 0:
            out = '<a class="Level2-item list-group-item" onclick="openIndicator('+indicator.Id+')">'+
					'<div class="row">'+
						'<div class="col-xs-offset-1 col-xs-11">'+
							'<h4 class="list-group-item-heading">'+indicator.Name+' </h4>'+
						'</div>'+
					'</div>'+	
                '</a>';
            break;
        default:
            out = '<a class="Level2-item list-group-item"><div class="row">Invalid indicator type</div></a>';
    }
    return out;
}