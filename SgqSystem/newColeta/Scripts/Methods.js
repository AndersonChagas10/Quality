$(document).on('click', '#btnLogin', function (e) {
    e.preventDefault();
    $(this).parents('.login').fadeOut(function (e) {
        $(this).hide();
        $('.App').removeClass('hide');
        addBreadcrumb(0, 0, 'Slaugther');
    });
});

$(document).on('click', '.level01List .level01', function (e) {
    showLevel02($(this));
});

$(document).on('click', '.level02List .level02', function (e) {
    showLevel03($(this));    
});

function showLevel02(level01) {

    $('.level01').removeClass('selected');
    $('.painel .labelPainel').addClass('hide');
    level01.addClass('selected');

    $('.level01List').fadeOut(function (e) {
        level01.parents('.level01List').addClass('hide');

        var level02 = $('.level02List');
        level02.removeClass('hide');
        level02.children('.levelGroup02[level01id=' + level01.attr('id') + ']').removeClass('hide');
        $('.painel .labelPainel[level01Id=' + level01.attr('id') + ']').removeClass('hide');
        var id = '';
        var level = '';
        addBreadcrumb(id, level, $('.level01List .selected').text());
    });
}

function showLevel03(level02) {
    $('.level02').removeClass('selected');

    level02.addClass('selected');

    $('.level02List').fadeOut(function (e) {
        level02.parents('.level02List').addClass('hide');

        var level03 = $('.level03List');
        level03.removeClass('hide');
        level03.children('.level03Group[level01id=' + $('.level01.selected').attr('id') + ']').removeClass('hide');

        var id = '';
        var level = '';
        addBreadcrumb(id, level, $('.level02List .selected').text());
    });
}

function addBreadcrumb(id, level, value) {
    $('.breadcrumb').append("<li class='active'><a href='#' level='"+level+" 'id='"+id+"'>" + value + "</a></li>");
}

function removeBreadcrumb() {
    $('.breadcrumb').children().last().remove();
}
