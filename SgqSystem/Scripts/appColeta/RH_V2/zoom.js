function configureZoom() {

    if ($('.zooms').length == 0) {

        $('body').append(
            '<div class="zooms">' +
            '<button class="btn btn-default btn-lg btn-zoom" id="zoomPlus">' +
            '<i class="fa fa-search-plus" aria-hidden="true"></i>' +
            '</button>' +
            '<button class="btn btn-default btn-lg btn-zoom" id="zoomMinus">' +
            '<i class="fa fa-search-minus" aria-hidden="true"></i>' +
            '</button>' +
            '</div>'
        );
    }

    applyZoom();
}

function applyZoom(zoom) {

    if (localStorage.zoom == undefined) {
        zoom = 1 + zoom;
    }

    if (!localStorage.getItem('zoom') && zoom) {
        localStorage.setItem('zoom', zoom);
    } else if (!zoom) {
        zoom = parseFloat(localStorage.getItem('zoom'));
    } else {
        localStorage.setItem('zoom', parseFloat(localStorage.getItem('zoom')) + zoom);
    }
    $('html').css('zoom', parseFloat(localStorage.getItem('zoom')));
}

$(document).off('click', '#zoomPlus').on('click', '#zoomPlus', function () {
    applyZoom(0.1);
});

$(document).off('click', '#zoomMinus').on('click', '#zoomMinus', function () {
    applyZoom(-0.1);
});

configureZoom();