//enviar os defeitos
function sendTotalAvaliacoesPorIndicadorPorAvaliacao(callback) {
    // if (connectionServer && $('.App').attr('unidadeid') != undefined) {
    //     $.post(urlPreffix + "/api/Defect/MergeDefect",
    //         { '': _totalAvaliacoesPorIndicadorPorAvaliacao },
    //         function () {
    //             $.get(urlPreffix + "/api/Defect/GetDefects",
    //                { 'ParCompany_Id': parseInt($('.App').attr('unidadeid')) }, function (r) {
    //                    _totalAvaliacoesPorIndicadorPorAvaliacao = r;
    //                    writeTotalAvaliacoesPorIndicadorPorAvaliacao();
    //                });
    //         }
    //     );
    // } else {
    //     writeTotalAvaliacoesPorIndicadorPorAvaliacao();
    // }
    // if (callback)
    //     callback();
}

function writeTotalAvaliacoesPorIndicadorPorAvaliacao() {
    var divTotalAvaliacoesPorIndicadorPorAvaliacao = "";

    for (var i = 0; i < _totalAvaliacoesPorIndicadorPorAvaliacao.length; i++) {
        var div = '<div CurrentEvaluation= ' + _totalAvaliacoesPorIndicadorPorAvaliacao[i].CurrentEvaluation
                    + ' Date=' + encodeURI(_totalAvaliacoesPorIndicadorPorAvaliacao[i].Date)
                    + ' Level1_Id=' + _totalAvaliacoesPorIndicadorPorAvaliacao[i].ParLevel1_Id
                    + ' ParCompany_Id=' + _totalAvaliacoesPorIndicadorPorAvaliacao[i].ParCompany_Id
                    + ' Evaluations=' + _totalAvaliacoesPorIndicadorPorAvaliacao[i].Evaluations
                    + ' Defects=' + _totalAvaliacoesPorIndicadorPorAvaliacao[i].Defects + '></div>';

        divTotalAvaliacoesPorIndicadorPorAvaliacao += div;
    }

    _writeFile('totalAvaliacoesPorIndicadorPorAvaliacao.txt', divTotalAvaliacoesPorIndicadorPorAvaliacao);
}

function loadTotalAvaliacoesPorIndicadorPorAvaliacao() {
    _readFile('totalAvaliacoesPorIndicadorPorAvaliacao.txt', function (r) {
        _totalAvaliacoesPorIndicadorPorAvaliacao = [];
        $(r).each(function (index, self) {
            if (isInsideFrequency(new Date(Date.parse(decodeURI($(self).attr('Date')))), parseInt($('.level1[id=' + parseInt($(self).attr('Level1_Id')) + ']').attr('parfrequency_id')))) {
                _totalAvaliacoesPorIndicadorPorAvaliacao.push(DefectDTO = {
                    Date: decodeURI($(self).attr('Date')),
                    CurrentEvaluation: parseFloat($(self).attr('CurrentEvaluation')),
                    ParCompany_Id: parseInt($(self).attr('ParCompany_Id')),
                    ParLevel1_Id: parseInt($(self).attr('Level1_Id')),
                    Evaluations: parseFloat($(self).attr('Evaluations')),
                    Defects: parseFloat($(self).attr('Defects'))
                });
            }
        });

    });
}