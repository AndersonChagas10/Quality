function openColeta(levels) {
   
   var html = '';

   var coleta = '';

   levels.forEach(function (level1) {
      coleta += getLevel1(level1);
      level1.ParLevel2.forEach(function (level2) {
         coleta += getLevel2(level2);
         level2.ParLevel3.forEach(function (level3) {
            coleta += getInputLevel3(level3);
         });
      });
   });

   html = `
		${getHeader()}
		<div class="container">
			<div class="row">
				<div class="col-xs-12">
					<div class="panel panel-primary">
					  <div class="panel-heading">
						<h3 class="panel-title"><a onclick="listarParCargo(currentParCargo_Id);">Voltar</a></h3>
						<h3 class="panel-title" style="float:rigth">Tarefa</h3>
					  </div>
					  <div class="panel-body">
						${coleta}
					  </div>
               </div>		
            </div>
			</div>
		</div>
		`;

   $('div#app').html(html);


}

function getLevel1(level1) {
   return '<div class="col-sm-12 input-lg">' + level1.Name + '</div>';
}

function getLevel2(level2) {
   return '<div class="col-sm-12 input-lg">' + level2.Name + '</div>';
}

function getLevel3(level3) {
   return '<div class="col-sm-12">' + level3.Name + '</div>';
}

function getInputLevel3(level3) {

   var retorno = "";

   if (level3.ParLevel3InputType && level3.ParLevel3InputType.Id) {

      switch (level3.ParLevel3InputType.Id) {

         case 1: //Binário
            retorno = getBinario(level3);
            break;
         case 6: //BinárioComTexto
            retorno = getBinarioComTexto(level3);
            break;
         case 3: //Intervalo
            retorno = getIntervalo(level3);
            break;
         case 9: //IntervaloComObservacao
            retorno = getIntervaloComObservacao(level3);
            break;
         case 11: //Observacao
            retorno = getObservacao(level3)
            break;
         case 8: //Likert
            retorno = getLikert(level3)
            break;
         case 5: //Texto
            retorno = getTexto(level3)
            break;

         default://foda-se o resto
            retorno = ""
            break;
      }

   }

   return retorno;

}

function getBinario(level3) {

   var html = `<div class="col-sm-12" data-conforme>
		<div class="col-sm-4 input-sm">
			${level3.Name}
		</div>
		<div class="col-sm-4 input-sm">
		</div>
		<div class="col-sm-3">
			<button class="btn btn-default btn-sm btn-block" data-binario data-positivo="${level3.ParLevel3BoolTrue.Name}" data-negativo="${level3.ParLevel3BoolFalse.Name}">Conforme</button>
		</div>
		<div class="col-sm-1">
			<button class="btn btn-warning pull-right btn-sm" data-na>N/A</button>
		</div>
		<div class="clearfix"></div>
	</div>`;
   return html;
}

function getBinarioComTexto(level3) {

   var html = `<div class="col-sm-12" data-conforme>
		<div class="col-sm-4 input-sm">
         ${level3.Name}
		</div>
		<div class="col-sm-2">
		</div>
		<div class="col-sm-2">
			<input type="text" class="col-sm-12 input-sm"/>
		</div>
		<div class="col-sm-3">
			<button class="btn btn-default btn-sm btn-block" data-binario data-positivo="${level3.ParLevel3BoolTrue.Name}" data-negativo="${level3.ParLevel3BoolFalse.Name}">Sim</button>
		</div>
		<div class="col-sm-1">
			<button class="btn btn-warning pull-right btn-sm" data-na>N/A</button>
		</div>
		<div class="clearfix"></div>
	</div>`;
   return html;
}

function getIntervalo(level3) {

   var html = `<div class="col-sm-12" data-conforme data-min="${level3.ParLevel3Value.IntervalMin}" data-max="${level3.ParLevel3Value.IntervalMax}">
		<div class="col-sm-4 input-sm">
         ${level3.Name}
		</div>
		<div class="col-sm-4 input-sm">
			MIN: ${level3.ParLevel3Value.IntervalMin} | MAX: ${level3.ParLevel3Value.IntervalMax}
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

function getIntervaloComObservacao(level3) {

   var html = `<div class="col-sm-12" data-conforme data-min="${level3.ParLevel3Value.IntervalMin}" data-max="${level3.ParLevel3Value.IntervalMax}">
		<div class="col-sm-4 input-sm">
         ${level3.Name}
		</div>
		<div class="col-sm-2 input-sm">
			MIN: ${level3.ParLevel3Value.IntervalMin} | MAX: ${level3.ParLevel3Value.IntervalMax}
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

function getObservacao(level3) {

   var html = `<div class="col-sm-12" data-conforme>
		<div class="col-sm-4 input-sm">
         ${level3.Name}
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

function getTexto(level3) {

   var html = `<div class="col-sm-12" data-conforme>
		<div class="col-sm-4 input-sm">
         ${level3.Name}
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

function getLikert(level3) {

   var html = `<div class="col-sm-12" data-conforme data-min="${level3.ParLevel3Value.IntervalMin}" data-max="${level3.ParLevel3Value.IntervalMax}">
		<div class="col-sm-4 input-sm">
         ${level3.Name}
		</div>
		<div class="col-sm-4 input-sm">
			Escala: ${level3.ParLevel3Value.IntervalMin} - ${level3.ParLevel3Value.IntervalMax}
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

$('body').off('click', '[data-plus]').on('click', '[data-plus]', function (e) {
   var value = parseInt($(this).parent().find('input').val());
   if (isNaN(value))
      value = 1;
   else
      value += 1;
   var input = $(this).parent().find('input')
   input.val(value);
   input.trigger('change');

});

$('body').off('click', '[data-minus]').on('click', '[data-minus]', function (e) {
   var value = parseInt($(this).parent().find('input').val());
   if (isNaN(value))
      value = -1;
   else
      value -= 1;
   var input = $(this).parent().find('input')
   input.val(value);
   input.trigger('change');
});

$('body').off('click', '[data-na]').on('click', '[data-na]', function (e) {
   var linha = $(this).parents('[data-conforme]');
   if (typeof (linha.attr('data-conforme-na')) == 'undefined') {
      resetarLinha(linha);
      linha.addClass('alert-warning');
      linha.attr('data-conforme-na', '');
   } else {
      resetarLinha(linha);
      $(linha).find('input[data-valor]').trigger('change');
   }
});

$('body').off('click', '[data-binario]').on('click', '[data-binario]', function (e) {
   var linha = $(this).parents('[data-conforme]');
   if (linha.attr('data-conforme') == '1') {
      resetarLinha(linha);
      linha.attr('data-conforme', '0');
      $(this).text($(this).attr('data-positivo'))
      $(this).addClass('btn-default');
      $(this).removeClass('btn-danger');
   } else {
      resetarLinha(linha);
      linha.addClass('alert-danger');
      linha.attr('data-conforme', '1');
      $(this).text($(this).attr('data-negativo'))
      $(this).addClass('btn-danger');
      $(this).removeClass('btn-default');
   }
});

$('body').off('change', 'input[data-valor]').on('change', 'input[data-valor]', function (e) {
   var linha = $(this).parents('[data-conforme]');
   debugger
   if ($(this).val() >= $(linha).attr('data-min') && $(this).val() <= $(linha).attr('data-max')) {
      resetarLinha(linha);
      linha.attr('data-conforme', '0');
   } else {
      resetarLinha(linha);
      linha.addClass('alert-danger');
      linha.attr('data-conforme', '1');
   }
});

function resetarLinha(linha) {
   linha.attr('data-conforme', '0');
   linha.removeClass('alert-danger');
   linha.removeClass('alert-warning');
   linha.removeAttr('data-conforme-na');
}
