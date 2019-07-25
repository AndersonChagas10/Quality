//Select3.initialize(objFiltro);
/*

<button type="button" data-filtroselect3-btn
        data-filtroselect3-select="select[name=parLevel1_Ids]"
        data-filtroselect3-url="http://localhost/SgqSystem/api/Formulario/GetForm">
    Abrir
</button>
*/

/*
var objFiltro = {};

function atualizaObjFiltro() {
    objFiltro = $('#filterReports').serializeObject();
    $(Object.keys(objFiltro)).each(function (i, o) {
        if (o.indexOf("Date") < 0) {
            objFiltro[o] = Array.isArray(objFiltro[o]) ? objFiltro[o] : [objFiltro[o]];
        }
    });
}
*/

$('body').on('click', '[data-filtroselect3-btn]', function () {
    var btn = $(this);
    var select = $($(btn).attr('data-filtroselect3-select'));
    var name = $(select).attr('name');
    var url = $(btn).attr('data-filtroselect3-url');
    Select3.render(select, name, url);
});

var Select3 = {
    param: function () { return {} },
    objFiltroSelect3: {},
    callback: function () { },

    initialize: function (param, callback) {
        if (param)
            this.param = param;
        if (callback)
            this.callback = callback;
    },

    render: function (element, name, url) {

        $('#filtroSelect3').remove();

        if (!this.objFiltroSelect3['_' + name]) {
            this.objFiltroSelect3['_' + name] = [];
        }

        var html = `<div id="filtroSelect3" style="width:100%;height:100%;position: fixed;left:0;z-index:999998;background-color:rgba(0,0,0,.7);top: 0;">
			<div style="background:#ccc;
			height:450px;
			position:fixed;
			width:600px;
			left:50%;
			margin-top: 70px;
			margin-left:-300px;top :0; z-index:999999">
				<div class="col-md-12">
					<h4 class="text-center">Faça a busca abaixo</h4>
				</div>
				<div class="col-sm-12">
					<form>
						<input class="col-md-10 input" style="height:34px" name="txtFiltro">
						<button type="submit" class="col-md-2 btn btn-primary">Buscar</button>
						<button type="submit" class="col-md-2 btn btn-info hide">1000</button>
					</form>
				</div>
				<div class="col-sm-12" style="margin-top:20px;overflow-y:auto;height:320px">
					<table class="table table-striped">
						<thead>
							<tr>
								<th colspan="2">Itens filtrados: </th>
							</tr>
						</thead>
						<tbody>
						</tbody>
					</table>
				</div>
				<div class="col-sm-12">
					<button class ="pull-left btn btn-danger" id="filtroSelect3Limpar">Limpar Tudo</button>
					<button class ="pull-right btn btn-default" onclick="$('#filtroSelect3').remove();">Fechar</button>
				</div>
			</div>
			</div>`;

        $('body').append(html);

        $('#filtroSelect3 form').off('submit').on('submit', function (e) {
            e.preventDefault();

            $.ajax({
                url: url + "?search=" + $(this).serializeObject()['txtFiltro'],
                type: 'post',
                data: JSON.stringify(Select3.param()),
                dataType: "JSON",
                contentType: "APPLICATION/JSON; CHARSET=UTF-8",
                beforeSend: function () {
                }
            })
			.done(function (data) {
			    var lista = data;

			    var htmlLista = $.map(lista,
					function (o) {
					    var selected = $.grep(Select3.objFiltroSelect3['_' + name], function (s) { return s.Value == o.Id });
					    return `<tr>
						<td><input type="checkbox" id="${o.Id} - ${o.Name}" value="${o.Id}" data-text="${o.Id} - ${o.Name}"
						${selected.length > 0 ? ' checked' : ''}
						/></td>
						<td><label for="${o.Id} - ${o.Name}">${o.Id} - ${o.Name}</label></td>
						</tr>`;
					}
				);

			    $('#filtroSelect3 table tbody').html(htmlLista);
			})
			.fail(function (jqXHR, textStatus, msg) {
			    console.log(msg);
			});

        });

        $('body').off('click', '#filtroSelect3 #filtroSelect3Limpar').on('click', '#filtroSelect3 #filtroSelect3Limpar', function (e) {
            Select3.objFiltroSelect3['_' + name] = [];
            $('#filtroSelect3 [type=checkbox]:checked').trigger('click');

            element.html("");
            element.trigger('change');

            if (Select3.callback)
                Select3.callback();
        });

        $('body').off('change', '#filtroSelect3 [type=checkbox]').on('change', '#filtroSelect3 [type=checkbox]', function (e) {
            var checkbox = $(this);
            if (this.checked) {
                Select3.objFiltroSelect3['_' + name].push({ Value: checkbox.val(), Text: checkbox.data('text') });
            } else {
                Select3.objFiltroSelect3['_' + name] = $.grep(Select3.objFiltroSelect3['_' + name], function (o) { return o.Value != checkbox.val(); });
            }

            element.html($.map(Select3.objFiltroSelect3['_' + name],
				function (o) {
				    return `<option value="${o.Value}" selected>${o.Text}</option>`;
				}
			));
            element.trigger('change');

            if (Select3.callback)
                Select3.callback();

            console.table(Select3.objFiltroSelect3['_' + name]);
        });
    }
}
