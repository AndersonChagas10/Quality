function listarParCargo() {

    if (!parametrization.listaParCargoXDepartment.length > 0) {
        return false;
    }

    var listaParCargoXDepartment = $.grep(parametrization.listaParCargoXDepartment, function (item) {
        return item.ParDepartment_Id == currentParDepartment_Id;
    });

    var listaParCargo = [];

    $(listaParCargoXDepartment).each(function (item, obj) {

        var listaParCargoFilter = $.grep(parametrization.listaParCargo, function (parCargo) {
            return parCargo.Id == obj.ParCargo_Id;
        });

        listaParCargoFilter.forEach(function(item){
            listaParCargo.push(item);
        });
        
    });

    var htmlParCargo = "";

    $(listaParCargo).each(function (i, o) {

        htmlParCargo += `<button type="button" class="list-group-item col-xs-12" data-par-cargo-id="${o.Id}">${o.Name}
                <span class="badge">14</span>
            </button>`;
    });

    var voltar = `<a onclick="listarParDepartment(0);">Voltar</a>`;

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
                        ${htmlParCargo}
                    </div>
                  </div>
                </div>
            </div>
        </div>
    </div>`;

    $('div#app').html(html);

}

$('body').on('click', '[data-par-cargo-id]', function (e) {   

    currentParCargo_Id = parseInt($(this).attr('data-par-cargo-id'));

    //listarLevels();

});