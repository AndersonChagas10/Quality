function breadCrumb(level01, level02) {
    var bdCrumb = $('.breadcrumb');

    bdCrumb.children('li:gt(0)').remove();

    var mainLevel = "<li><a href='#' class='main'>" + clusterAtivoName + "</a></li>"
    if(isEUA)
    {
        mainLevel = "<li><a href='#' class='main'>Audits</a></li>"
    }
    var level01Li = "";
    if (level01 != null && level01 != undefined) {

        level01Li = "<li><a href='#' class='level01'>" + level01 + "</a></li>";
        if (level02 == null || level02 == undefined) {
            level01Li = "<li class='active'>" + level01 + "</li>";
        }
    }

    var level02Li = "";
    if (level02 != null && level02 != undefined) {
        level02Li = "<li class='active'>" + level02 + "</li>";
    }

    var retroactiveDateClass = "";

    if ($('.App').attr('retroactivedata')) {
        retroactiveDateClass = " bgRed";
    }

    bdCrumb.html(
                    mainLevel +
                    level01Li +
                    level02Li 
                            );
}

$(document).on('click', '.breadcrumb .main', function (e) {
    if (!$('.level1List').is(':visible')) {
        level1Show(false, clusterAtivo);
    }
});

$(document).on('click', '.breadcrumb .level01', function (e) {
    openLevel2($('.level1.selected'));
});