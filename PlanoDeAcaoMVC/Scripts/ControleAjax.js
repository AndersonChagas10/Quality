/**
 * jquery-loading-overlay
 *
 * https://gasparesganga.com/labs/jquery-loading-overlay/
 */

/*Utillizado para um unico ajax

    //$(document).ajaxStart(function () {
    //    $.LoadingOverlay("show");
    //    counter++;
    //});
    
    //$(document).ajaxStop(function () {
    //    $.LoadingOverlay("hide");
    //    counter--;
    //    HideForOperacional();
    //});
    //
*/

/*Utillizado para multiplos ajax concatenados*/
$(document).ajaxSend(function () {
    $.LoadingOverlay("show");
});

$(document).ajaxComplete(function () {
    $.LoadingOverlay("hide");
});
/**/

$(document).ajaxError(function (e, h, x) {
    console.log(e);
    console.log(h);
    console.log(x);
    $.LoadingOverlay("hide");
});