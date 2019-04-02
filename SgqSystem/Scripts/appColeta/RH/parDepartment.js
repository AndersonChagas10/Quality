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

function listarParDepartment(id, parent_Id){
	
	_readFile("parDepartment.txt", function(data){
					
		data = JSON.parse(data);
		
		id = ((!!id && id.length > 0)? id.substring(parseInt(id.lastIndexOf('-')+1)):id);
		
		var htmlParDepartment = ""
		$(data).each(function( i,o ) {
			if((id > 0 && id == o.Parent_Id) || ((id == 0 || id == null) && (o.Parent_Id == 0 || o.Parent_Id == null))){
				var p_id = (!!parent_Id && parent_Id.length > 0 ? (parent_Id + "-"):"") + o.Parent_Id;
				htmlParDepartment += `<button type="button" class="list-group-item col-xs-12" data-par-department-id="${o.Id}" data-par-department-parend-id="${p_id}">${o.Name}
					<span class="badge">14</span>
				</button>`;
			}
		});
		
		parent_Id = (!!parent_Id && parent_Id.lastIndexOf('-') > 0 ? parent_Id.substring(0,parent_Id.lastIndexOf('-')) :parent_Id);
		var voltar = id > 0 ? `<a onclick="listarParDepartment(${parent_Id},${parent_Id})">Voltar</a>` : "";
		
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
	});
}

$('body').on('click','[data-par-department-id]',function(e){
	debugger
	listarParDepartment($(this).attr('data-par-department-id'),
	$(this).attr('data-par-department-parend-id'));
});