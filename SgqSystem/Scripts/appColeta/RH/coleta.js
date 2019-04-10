function openColeta() {

	var html = '';
	
	html = `
		${getHeader()}
		<div class="container-fluid">
			<div class="row">
				<div class="col-xs-12">

					<div class="panel panel-primary">
					  <div class="panel-heading">
						<h3 class="panel-title">COLETA</h3>
					  </div>
					  <div class="panel-body">
						${getBinario()}
						${getBinarioComTexto()}
						${getIntervalo()}
						${getIntervaloComObservacao()}
						${getObservacao()}
						${getLikert()}
					  </div>
				    </div>		</div>


				</div>
			</div>
		`;

	$('div#app').html(html);


}

function getBinario(){
	
	var html = `<div class="col-sm-12" data-conforme>
		<div class="col-sm-4 input-sm">
			Binário
		</div>
		<div class="col-sm-4 input-sm">
		</div>
		<div class="col-sm-3">
			<button class="btn btn-default btn-sm btn-block" data-binario data-positivo="Conforme" data-negativo="Não Conforme">Conforme</button>
		</div>
		<div class="col-sm-1">
			<button class="btn btn-warning pull-right btn-sm" data-na>N/A</button>
		</div>
		<div class="clearfix"></div>
	</div>`;
	return html;
}

function getBinarioComTexto(){
	
	var html = `<div class="col-sm-12" data-conforme>
		<div class="col-sm-4 input-sm">
			Binário com texto
		</div>
		<div class="col-sm-2">
		</div>
		<div class="col-sm-2">
			<input type="text" class="col-sm-12 input-sm"/>
		</div>
		<div class="col-sm-3">
			<button class="btn btn-default btn-sm btn-block" data-binario data-positivo="Sim" data-negativo="Não">Sim</button>
		</div>
		<div class="col-sm-1">
			<button class="btn btn-warning pull-right btn-sm" data-na>N/A</button>
		</div>
		<div class="clearfix"></div>
	</div>`;
	return html;
}

function getIntervalo(){
	
	var html = `<div class="col-sm-12" data-conforme data-min="0" data-max="2">
		<div class="col-sm-4 input-sm">
			Intervalo
		</div>
		<div class="col-sm-4 input-sm">
			MIN: 0 | MAX: 2
		</div>
		<div class="col-sm-3">
			<button class="btn btn-sm btn-primary col-xs-2" data-minus>-</button>
			<input type="text" class="col-xs-8 input input-sm" data-valor/>
			<button class="btn btn-sm btn-primary col-xs-2" data-plus>+</button>
		</div>
		<div class="col-sm-1">
			<button class="btn btn-warning pull-right btn-sm" data-na>N/A</button>
		</div>
		<div class="clearfix"></div>
	</div>`;
	return html;
}

function getIntervaloComObservacao(){
	
	var html = `<div class="col-sm-12" data-conforme data-min="0" data-max="2">
		<div class="col-sm-4 input-sm">
			Intervalo Com Observacao
		</div>
		<div class="col-sm-2 input-sm">
			MIN: 0 | MAX: 2
		</div>
		<div class="col-sm-2">
			<input type="text" class="col-sm-12 input-sm"/>
		</div>
		<div class="col-sm-3">
			<button class="btn btn-sm btn-primary col-xs-2" data-minus>-</button>
			<input type="text" class="col-xs-8 input-sm" data-valor/>
			<button class="btn btn-sm btn-primary col-xs-2" data-plus>+</button>
		</div>
		<div class="col-sm-1">
			<button class="btn btn-warning pull-right btn-sm" data-na>N/A</button>
		</div>
		<div class="clearfix"></div>
	</div>`;
	return html;
}

function getObservacao(){
	
	var html = `<div class="col-sm-12" data-conforme>
		<div class="col-sm-4 input-sm">
			Observacao
		</div>
		<div class="col-sm-4">
		</div>
		<div class="col-sm-3">
			<input type="text" class="col-sm-12 input-sm"/>
		</div>
		<div class="col-sm-1">
			<button class="btn btn-warning pull-right btn-sm" data-na>N/A</button>
		</div>
		<div class="clearfix"></div>
	</div>`;
	return html;
}

function getLikert(){
	
	var html = `<div class="col-sm-12" data-conforme data-min="0" data-max="2">
		<div class="col-sm-4 input-sm">
			Escala Likert
		</div>
		<div class="col-sm-4 input-sm">
			Escala: 0 - 2
		</div>
		<div class="col-sm-3">
			<input type="text" class="col-sm-12 input-sm" data-valor/>
		</div>
		<div class="col-sm-1">
			<button class="btn btn-warning pull-right btn-sm" data-na>N/A</button>
		</div>
		<div class="clearfix"></div>
	</div>`;
	return html;
}

$('body').off('click','[data-plus]').on('click','[data-plus]',function(e){
	var value = parseInt($(this).parent().find('input').val());
	if(isNaN(value))
		value = 1;
	else
		value += 1;
	var input = $(this).parent().find('input')
	input.val(value);
	input.trigger('change');
	
});

$('body').off('click','[data-minus]').on('click','[data-minus]',function(e){
	var value = parseInt($(this).parent().find('input').val());
	if(isNaN(value))
		value = -1;
	else
		value -= 1;
	var input = $(this).parent().find('input')
	input.val(value);
	input.trigger('change');
});

$('body').off('click','[data-na]').on('click','[data-na]',function(e){
	var linha = $(this).parents('[data-conforme]');
	if(typeof(linha.attr('data-conforme-na')) == 'undefined'){
		resetarLinha(linha);
		linha.addClass('alert-warning');
		linha.attr('data-conforme-na','');
	}else{
		resetarLinha(linha);
		$(linha).find('input[data-valor]').trigger('change');
	}
});

$('body').off('click','[data-binario]').on('click','[data-binario]',function(e){
	var linha = $(this).parents('[data-conforme]');
	if(linha.attr('data-conforme') == '1'){
		resetarLinha(linha);
		linha.attr('data-conforme','0');
		$(this).text($(this).attr('data-positivo'))
		$(this).addClass('btn-default');
		$(this).removeClass('btn-danger');
	}else{
		resetarLinha(linha);
		linha.addClass('alert-danger');
		linha.attr('data-conforme','1');
		$(this).text($(this).attr('data-negativo'))
		$(this).addClass('btn-danger');
		$(this).removeClass('btn-default');
	}
});

$('body').off('change','input[data-valor]').on('change','input[data-valor]',function(e){
	var linha = $(this).parents('[data-conforme]');
	debugger
	if($(this).val() >= $(linha).attr('data-min') && $(this).val() <= $(linha).attr('data-max')){
		resetarLinha(linha);
		linha.attr('data-conforme','0');
	}else{
		resetarLinha(linha);
		linha.addClass('alert-danger');
		linha.attr('data-conforme','1');
	}
});

function resetarLinha(linha){
	linha.attr('data-conforme','0');
	linha.removeClass('alert-danger');
	linha.removeClass('alert-warning');
	linha.removeAttr('data-conforme-na');
}
