var datatableGRT = {

    idTabela: "resultsApontamentos",
    listaDeDados: [],
    definicaoColuna: [],
    colunaDosDados: [],
    linguagem: {
        "search": 'Buscar',
        "lengthMenu": "_MENU_",
        "zeroRecords": 'Sem dados',
        "paginate": {
            "previous": 'Anterior',
            "next": 'Proximo',
        }
    },
    numeroLinhasNaTabela: 25,
    aplicarResponsividade: true,
    tamanhosDoMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "-"]],
    loadingRecords: true,
    destroy: true,
    info: true,
    initComplete: function () { },
    createdRow: function (row, data, index) { },

    Inicializar: function (config) {
        if ($.fn.DataTable.isDataTable('#' + config.idTabela) && $('#' + config.idTabela).html().length > 0) {
            $('#' + config.idTabela).DataTable().destroy();
        }

        if (config.idTabela != undefined)
            this.idTabela = config.idTabela;
        if (config.listaDeDados != undefined)
            this.listaDeDados = config.listaDeDados;
        if (config.colunaDosDados != undefined)
            this.colunaDosDados = config.colunaDosDados;
        if (config.numeroLinhasNaTabela != undefined)
            this.numeroLinhasNaTabela = config.numeroLinhasNaTabela;
        if (config.initComplete != undefined)
            this.initComplete = config.initComplete;
        if (config.createdRow != undefined)
            this.createdRow = config.createdRow;
        if (config.tamanhosDoMenu != undefined)
            this.tamanhosDoMenu = config.tamanhosDoMenu;
        if (config.definicaoColuna != undefined)
            this.definicaoColuna = config.definicaoColuna;

        $('#' + config.idTabela).empty()
        return $('#' + config.idTabela).DataTable({
            data: this.listaDeDados,
            "columnDefs": this.definicaoColuna,
            columns: this.colunaDosDados,
            info: true,
            "pageLength": this.numeroLinhasNaTabela,
            responsive: true,
            "scrollX": true,
            destroy: this.destroy,
            lengthMenu: this.tamanhosDoMenu,
            "language": this.linguagem,
            initComplete: this.initComplete,
            dom: 'Blfrtip',
            buttons: {
                buttons: [{
                    extend: 'excel',
                    text: '<i class="fa fa-file-excel-o" title="Excel"></i>',
                    title: "",
                    exportOptions: {
                        columns: ':visible'
                    },
                    footer: true,
                    autoPrint: false
                }, {
                    extend: 'copy',
                    text: '<i class="fa fa-copy" title="Copy"></i>',
                    exportOptions: {
                        modifier: {
                            page: ':visible'
                        }
                    }
                }],
                dom: {
                    container: {
                        className: 'dt-buttons'
                    },
                    button: {
                        className: 'btn btn-default'
                    }
                }
            },
            "drawCallback": function (settings) {

                var tfoot = "<tfoot><tr class='search-input-tfoot-tr'>";
                $('#' + settings.nTableWrapper.id + ' .dataTables_scrollHead thead th').each(function (i) {
                    var th = $('#' + settings.nTableWrapper.id + ' .dataTables_scrollHead thead th').eq($(this).index());
                    var td = $('#' + settings.nTableWrapper.id + ' .dataTables_scrollBody tbody > tr:first > td').eq($(this).index());

                    var width = th.outerWidth();
                    if (width < td.outerWidth() &&
                    $('#' + settings.nTableWrapper.id + ' .dataTables_scrollBody tbody:first > tr:first > td').length == $('#' + settings.nTableWrapper.id + ' .dataTables_scrollHead thead:first th').length) {
                        width = td.outerWidth();
                    }

                    var tfootAtual = $('#' + settings.nTableWrapper.id + ' .dataTables_scrollHead table').find('tfoot');

                    if (tfootAtual.length > 0) {
                        $(tfootAtual).find('input[data-index=' + i + ']').css('width', (width - 10) + 'px');
                    } else {
                        var title = th.text();
                        if (title.length > 0)
                            tfoot += '<td style="padding-left:5px !important; padding-right:5px !important;"><input type="text" class="search-input-tfoot" style="width:' + (width - 10) + 'px !important" placeholder="' + title + '" data-index="' + i + '" /></td>';
                        else
                            tfoot += '<td style="padding-left:5px !important; padding-right:5px !important;"><span type="text" style="display:block;width:' + (width - 10) + 'px !important"></span></td>';
                    }
                });
                tfoot += "</tr></tfoot>";

                if (!($('#' + settings.nTableWrapper.id + ' .dataTables_scrollHead table').find('tfoot').length > 0))
                    $('#' + settings.nTableWrapper.id + ' .dataTables_scrollHead table').append(tfoot);
            }
        });

        $(table.table().container()).on('keyup', 'tfoot input', function () {
            table
                .column($(this).data('index'))
                .search(this.value)
                .draw();
        });

        table.draw();
    }
};