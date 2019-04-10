function openParDepartment() {

	var html = '';

	$.ajax({
		data: {},
		url: urlPreffix + '/api/parDepartment',
		type: 'GET',
		success: function (data) {

			_writeFile("parDepartment.txt", JSON.stringify(data), function () {
				listarParDepartment(0);
			});
		},
		timeout: 600000,
		error: function () {
			$(this).html($(this).attr('data-initial-text'));
		}
	});


}

function listarParDepartment(id) {

	// _readFile("parDepartment.txt", function (data) {

	currentParDepartment_Id = id;

	var data = parametrization.listaParDepartment;

	var department = {};

	var htmlParDepartment = "";

	$(data).each(function (i, o) {

		if (parseInt(id) > 0 && id == o.Id) {
			department = o;
		}

		if ((id > 0 && id == o.Parent_Id) || ((id == 0 || id == null) && (o.Parent_Id == 0 || o.Parent_Id == null))) {

			htmlParDepartment += `
				<button type="button" class="list-group-item col-xs-12" data-par-department-id="${o.Id}" data-par-department-parend-id="${o.Parent_Id}">${o.Name}
					<span class="badge">14</span>
				</button>`;

		}

	});

	//caso for "" quer dizer que não tem mais filhos, então abre o próximo	
	if (htmlParDepartment == "") {
		listarParCargo();
		return;
	}

	var voltar = !!department.Id ? `<a onclick="listarParDepartment(${department.Parent_Id});">Voltar</a>` : `<a onclick="listarParFrequency();">Voltar</a>`;

	html = `
		${getHeader()}
		<div class="container">
			<div class="row">
				<div class="col-xs-12">

					<div class="panel panel-primary">
					  <div class="panel-heading">
						<h3 class="panel-title">${voltar}</h3>
					  </div>
					  <div class="panel-body">
						<div class="list-group">
							${htmlParDepartment}
						</div>
					  </div>
					</div>

				</div>
			</div>
		</div>
		`;

	$('div#app').html(html);
	// });
}

$('body').on('click', '[data-par-department-id]', function (e) {

	var parDepartment_Id = $(this).attr('data-par-department-id');

	if (parDepartment_Id) {
		listarParDepartment(parDepartment_Id);
	} else {
		currentParDepartment_Id = parDepartment_Id;
		openParCargo();
	}
});