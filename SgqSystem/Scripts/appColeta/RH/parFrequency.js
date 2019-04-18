function openParFrequency() {

	var html = '';

	$.ajax({
		data: {},
		url: urlPreffix + '/api/parFrequency',
		type: 'GET',
		success: function (data) {

			_writeFile("parFrequency.txt", JSON.stringify(data), function () {
				listarParFrequency();
			});
		},
		timeout: 600000,
		error: function () {
			$(this).html($(this).attr('data-initial-text'));
		}
	});

}

function listarParFrequency() {

    cleanGlobalVarParFrequency();

	_readFile("parFrequency.txt", function (data) {

		data = JSON.parse(data);

		var frequency = {};

        var htmlParFrequency = "";         

		$(data).each(function (i, o) {

				htmlParFrequency += '<button type="button" class="list-group-item col-xs-12" data-par-frequency-id="'+o.Id+'">'+o.Name+
                    '<span class="badge">14</span>'+
                    '<span class="badge"><i class="fa fa-download"></i></span>'+
				'</button>';
        });
        
        var voltar = "";

		html = getHeader()+
		'<div class="container">                               '+
		'	<div class="row">                                  '+
		'		<div class="col-xs-12">                        '+
		'			<div class="panel panel-primary">          '+
		'			  <div class="panel-heading">              '+
		'				<h3 class="panel-title">Renã</h3>      '+
		'			  </div>                                   '+
		'			  <div class="panel-body">                 '+
		'				<div class="list-group">               '+
		htmlParFrequency+
		'				</div>                                 '+
		'			  </div>                                   '+
		'			</div>                                     '+
		'		</div>                                         '+
		'	</div>                                             '+
		'</div>';

		$('div#app').html(html);
	});
}

function cleanGlobalVarParFrequency(){
    currentParDepartment_Id = null;
    currentParCargo_Id = null;
    currentParFrequency_Id = null;
}

$('body').on('click', '[data-par-frequency-id]', function (e) {   

    currentParFrequency_Id = parseInt($(this).attr('data-par-frequency-id'));

    getAppParametrization();

});