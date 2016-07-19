$('body').css("font-family", "Gotham");
var links = [
    {
        "bgcolor":"orange",
        "icon":"<i class='fa fa-save'></i>"
    },
    {
        "id": "btnCancel",
        "url":"",
        "bgcolor":"red",
        "color":"#fffff",
        "icon":"<i class='fa fa-close'></i>",
        "title": "Cancel"
    },
    {
        "id": "btnSave",
        "url":"",
        "bgcolor":"blue",
        "color":"white",
        "icon":"<i class='fa fa-check'></i>",
        "title": "Save"
    }
]
$('.kc_fab_wrapper').kc_fab(links);