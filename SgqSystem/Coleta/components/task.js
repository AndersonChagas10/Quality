function getTaskGroup(task, taskgroups){
	for(var i = 0; i < taskgroups.length; i++)
		if(taskgroups[i].Id == task.Group)
			return taskgroups[i];
	return 	
}

function lineTask(task, taskgroups) {

    var out = '';
    switch(task.Type) {
        case 0:
            out = "<a id='task-"+task.Id+"' class='list-group-item audit-item task'>"+
					"<div class='row'>"+
						"<div class='col-xs-6'>"+
							"<h4 class='list-group-item-heading audit-desc'>"+task.Name+" </h4>"+
						"</div>"+
						"<div class='col-xs-4 text-center'>"+
							"<div class='input-group input-group-sm'>"+
								"<span class='input-group-btn btn-minus'>"+
									"<button class='btn btn-default' type='button'>"+
										"<i class='fa fa-minus' aria-hidden='true'></i>"+
									"</button>"+
								"</span>"+
								"<input value='0' min='0' type='number' class='form-control text-center audit-value'>"+
								"<span class='input-group-btn'>"+
									"<button class='btn btn-default btn-plus' type='button'>"+
										"<i class='fa fa-plus' aria-hidden='true'></i>"+
									"</button>"+
								"</span>"+
							"</div>"+
						"</div>"+
						"<div class='col-xs-2 text-center'>"+
							"<button class='btn btn-default audit-na' type='button'>"+
								"N/A"+
							"</button>"+
						"</div>"+
					"</div>"+	
				"</a>";
            break;
		case 1:
			out += "<div id='task-"+task.Id+"' class='panel-body task'>"+
						"<div class='row'>"+
							"<div class='col-xs-6 col-sm-7 col-md-8 col-lg-9'>"+
								"<p>"+task.Name+"</p>"+
							"</div>"+
							"<div class='col-xs-6 col-sm-5 col-md-4 col-lg-3 text-right'>"+
								"<div class='input-group input-group-sm audit-item'>"+
									"<span class='input-group-btn'>"+
										"<button class='btn btn-default btn-minus' type='button'>"+
											"<i class='fa fa-minus' aria-hidden='true'></i>"+
										"</button>"+
									"</span>"+
									"<input type='number' value='0' min='0' class='form-control text-center audit-value'>"+
									"<span class='input-group-btn'>"+
										"<button class='btn btn-default btn-plus' type='button'>"+
											"<i class='fa fa-plus' aria-hidden='true'></i>"+
										"</button>"+
									"</span>"+
								"</div>"+
							"</div>"+
						"</div>"+
					"</div>";
            break;
		case 2:
            out = "<div id='task-"+task.Id+"' class='panel-body item_data task'>"+
						"<div class='row'>"+
							"<div class='col-xs-9' id='task-name'>"+
								task.Name+
							"</div>"+
							"<div class='col-xs-1 text-right item_data_answer'>"+
								"No"+
							"</div>"+
							"<div class='col-xs-2 text-right'>"+
								"<button data-toggle='modal' data-target='#helpModal' class='btn btn-default button-help'><i class='fa fa-question' aria-hidden='true'></i></button>"+
							"</div>"+
						"</div>"+
					"</div>";
			break;
        default:
            out = '<a class="Level2-item list-group-item"><div class="row">Invalid indicator type</div></a>';
    }
    return out;
}

$(function(){
			
	var errors = 3;

	$('.btn-minus').click(function(e){
		e.preventDefault();					
		$that = $(this);
		
		if(parseInt($that.parents(".audit-item").find(".audit-value").val()) != 0){
			$that.parents(".audit-item").find(".audit-value").val(parseInt($that.parents(".audit-item").find(".audit-value").val())-1);
		}
		
		totalErrors($(this));
	});

	$('.button-help').click(function(e){
		e.preventDefault();		

		$(this).parents('.item_data').children('.row').children('#task-name').text();
		
		$('#helpModal-content').text($(this).parents('.row').find('#task-name').text());
	});

	$('.btn-plus').click(function(e){
		e.preventDefault();					
		$that = $(this); 
		
		$that.parents(".audit-item").find(".audit-value").val(parseInt($that.parents(".audit-item").find(".audit-value").val())+1);
		
		totalErrors($(this));
	});

	$('.audit-na').click(function(e){
		
		e.preventDefault();					
		$that = $(this);
		
		if($that.parents('.audit-item').hasClass('disabled')){
			$that.parents('.audit-item').removeClass('disabled');	
			$that.parents('.audit-item').find('.audit-core').text('Yes');
			$that.parents('.audit-item').find('.audit-others').text('Yes');
			$that.parents(".audit-item").find(".audit-value").attr("readonly", false);
		}else{
			$that.parents('.audit-item').addClass('disabled');
			$that.parents('.audit-item').find('.audit-core').text('');
			$that.parents('.audit-item').find('.audit-others').text('');
			$that.parents(".audit-item").find(".audit-value").attr("readonly", true);
			$that.parents(".audit-item").find(".audit-value").val(0);
		}
		
		totalErrors($(this));
		
	})

	function changeInputBackground(){
		if(parseInt($that.parents(".audit-item").find(".audit-value").val()) >= errors){
			$that.parents(".audit-item").find(".audit-value").css({'background-color' : '#ffb2b2'});
		}else{
			$that.parents(".audit-item").find(".audit-value").css({'background-color' : 'white'});
		}
	}

	function totalErrors(element){
		var i, $auditvalue = $('.audit-value');
		var sumErrors = 0, sumErrosSides = 0;
		
		for (i=0; i<$auditvalue.length; i++){
			if(parseInt($auditvalue.eq(i).val()) > 0){
				sumErrors = sumErrors + parseInt($auditvalue.eq(i).val());
			}
		}
		
		$('.total-errors-label').text(sumErrors);
		
		if(sumErrors > 0){
			sumErrosSides = 1;
		}else{
			sumErrosSides = 0;
		}
		$(element).parents(".panel-body").addClass('taskYellow');

		var divCor = $(element).parents("a.task, div.task");
		var input = parseInt($(element).parents("a.task, div.task").find("input").val());

	    if(input == 0){
			divCor.removeClass('taskYellow');
		}

		else if(input > 0){
			divCor.addClass('taskYellow');
		}

		$('.total-errors').removeClass("divRed");

		if($('.taskYellow').length >= 2 || input > 2)
		{
			//fica vermelho	
			$('.total-errors').addClass("divRed");
		}

		if(parseInt($('#side-errors').text()) >=6 ) {
			$('.total-sides-errors').addClass("divRed");
		}

		// if(sumErrors >= 2){
		// 	$('.total-errors').css({'background-color' : '#ffb2b2'});
		// }else{
		// 	$('.total-errors').css({'background-color' : 'white'});
		// }
		
		//$('.total-sides-errors-label').text(sumErrosSides);
	}

	function totalErrorsItemData(){
			var i, $auditvalue = $('.item_data');
			var sumErrors = 0, sumErrosSides = 0;
			
			for (i=0; i<$auditvalue.length; i++){
				if($auditvalue.eq(i).find(".item_data_answer").text() == "Yes"){
					sumErrors = sumErrors + 1;
				}
			}
			
			$('.total-errors-label').text(sumErrors);
			
			if(sumErrors > 0){
				sumErrosSides = 1;
			}else{
				sumErrosSides = 0;
			}
			
			if(sumErrors >= 2){
				$('.total-errors').css({'background-color' : '#ffb2b2'});
			}else{
				$('.total-errors').css({'background-color' : 'white'});
			}
			
			$('.total-sides-errors-label').text(sumErrosSides);

			
		}

	$('.item_data').click(function(e){
					
		e.preventDefault();					
		$that = $(this);

		var $target = $(event.target);

		if(!$target.is(".btn") && !$target.is(".fa")) {
			var color = "rgb(253, 160, 160)";
			var background_color = $that.css("background-color");
			var evaluation = $that.find(".item_data_answer").text();

			if(evaluation == "Yes"){
				$that.css('background-color', 'white');
				$that.find(".item_data_answer").text("No");
			}else{
				$that.css('background-color', color);
				$that.find(".item_data_answer").text("Yes");
			}

			totalErrorsItemData();
		}
		
	})

	$(".button-collapse").click(function(e){
		e.preventDefault();					
		$that = $(this);
		$(".collapse").collapse('hide');
	});	

	$(".button-expand").click(function(e){
		e.preventDefault();					
		$that = $(this);
		$(".collapse").collapse('show');
	});	

})



