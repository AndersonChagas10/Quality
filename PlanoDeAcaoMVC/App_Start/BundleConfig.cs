using System.Web.Optimization;

namespace PlanoDeAcaoMVC
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.moneymask.js",
                       
                        "~/Scripts/Angular/angular.min.js"));

             // Use the development version of Modernizr to develop with and learn from. Then, when you're
             // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
             bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/bootstrap-datepicker.min.js",
                      "~/Scripts/locales/bootstrap-datepicker.pt-BR.min.js"));

            bundles.Add(new StyleBundle("~/Content/cssBootstrap").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/site.css",
                      "~/Content/font-awesome.min.css"));

            //HighCharts
            bundles.Add(new ScriptBundle("~/bundles/hc")
                .Include("~/Scripts/highcharts/4.2.0/highcharts.js",
                "~/Scripts/highcharts/4.2.0/modules/no-data-to-display.src.js")
            );

            bundles.Add(new StyleBundle("~/bundles/hcstyle")
                .Include("~/Content/bootstrap-datepicker.min.css")
            );

            //Loading Overlay
            bundles.Add(new ScriptBundle("~/bundles/loadingoverlay")
                .Include("~/Scripts/loadingoverlay.min.js",//LoadingOverlay
                "~/Scripts/loadingoverlay_progress.min.js",//LoadingOverlay
                 "~/Scripts/moment-with-locales.min.js",//Moment
                "~/Scripts/bootbox.min.js",//BootBox
                 "~/Scripts/select2.min.js",//Select 2
                 "~/Scripts/i18n/pt-BR.js",//Select 2
                 "~/Scripts/daterangepicker.js",//Date RAnge Piker
                  "~/Scripts/DatePikerReady.js",//Date RAnge Piker
                "~/Scripts/inputmask/jquery.inputmask.bundle.js",//InputMask
                "~/Scripts/blur.js",//blur
                "~/Scripts/GuardJs.js")
            );

            //Reports
            bundles.Add(new StyleBundle("~/bundles/reportsstyle")
                .Include("~/Content/Reports/Reports1.css")
            );

            bundles.Add(new ScriptBundle("~/bundles/reportsscript")
                .Include("~/Scripts/Reports/Reports1.js",
                "~/Scripts/Reports/ReportsHC.js")
            );

            #region DataTable

            bundles.Add(new ScriptBundle("~/bundles/ScriptTables").Include(
                     //"~/Scripts/DataTables/datatables.min.js",
                     "~/Scripts/DataTables/jquery.dataTables.min.js",
                     "~/Scripts/DataTables/dataTables.buttons.min.js",
                     "~/Scripts/DataTables/buttons.flash.min.js",
                     "~/Scripts/DataTables/jszip.min.js",
                     "~/Scripts/DataTables/pdfmake.min.js",
                     "~/Scripts/DataTables/vfs_fonts.js",
                     "~/Scripts/DataTables/buttons.html5.min.js",
                     "~/Scripts/DataTables/buttons.print.min.js",
                     "~/Scripts/DataTables/buttons.colVis.min.js",
                     "~/Scripts/DataTables/buttons.print.min.js",
                     "~/Scripts/DataTables/dataTables.fixedHeader.min.js",
                     "~/Scripts/DataTables/dataTables.fixedColumns.min.js",
                     "~/Scripts/TableEditor/jquery.tabledit.min.js"
                     //"~/Scripts/jbs.jquery.dataTables.configuration.js",
                     //"~/Scripts/dataTable.CRUD.js",
                     ));

            bundles.Add(new StyleBundle("~/Content/Tables").Include(
                    "~/Content/DataTables/css/jquery.dataTables.min.css",
                    "~/Content/DataTables/css/buttons.dataTables.min.css",
                    "~/Content/DataTables/css/responsive.dataTables.min.css",
                    "~/Content/DataTables/css/fixedColumns.dataTables.min.css",
                    "~/Content/DataTables/css/fixedHeader.dataTables.min.css"
                ));

            #endregion

            #region Select2

            bundles.Add(new ScriptBundle("~/bundles/select2js").Include(
                   ));

            bundles.Add(new StyleBundle("~/Content/select2css").Include(
                  "~/Content/css/select2.min.css"));

            #endregion

            #region DateRangePicker

            bundles.Add(new StyleBundle("~/Content/DatePikerContentCss").Include(
                 "~/Content/daterangepicker.css"));

            bundles.Add(new ScriptBundle("~/bundles/DatePikerContent").Include(
                  ));

            #endregion

            #region Bundles PANovo

            //Loading Overlay
            bundles.Add(new ScriptBundle("~/bundles/panovoScriptBundle")
                .Include(
                "~/Scripts/PANovo/js/jquery-1.12.4.js",
                //"~/Scripts/PANovo/js/jquery-3.2.1.slim.min.js",
                "~/Scripts/PANovo/js/bootstrap.min.js",
                "~/Scripts/PANovo/js/underscore-min.js",
                "~/Scripts/PANovo/js/lodash.min.js",
                "~/Scripts/PANovo/js/popper.min.js",
                "~/Scripts/PANovo/js/jquery.dataTables.min.js",
                "~/Scripts/PANovo/js/highcharts.js",
                "~/Scripts/PANovo/js/treemap.js",
                "~/Scripts/PANovo/js/bootstrap-select.min.js",
                "~/Scripts/PANovo/js/defaults-pt_BR.min.js",
                "~/Scripts/PANovo/js/moment.min.js",
                "~/Scripts/PANovo/js/daterangepicker.js",
                "~/Scripts/PANovo/js/jquery.dataTables.min.js",
                "~/Scripts/PANovo/js/dataTables.buttons.min.js",
                "~/Scripts/PANovo/js/buttons.flash.min.js",
                "~/Scripts/PANovo/js/buttons.colVis.min.js",
                "~/Scripts/PANovo/js/jszip.min.js",
                "~/Scripts/PANovo/js/pdfmake.min.js",
                "~/Scripts/PANovo/js/vfs_fonts.js",
                "~/Scripts/PANovo/js/buttons.html5.min.js",
                "~/Scripts/PANovo/js/buttons.print.min.js",
                "~/Scripts/PANovo/js/dataTables.fixedColumns.min.js",
                "~/Scripts/PANovo/js/pt-br.js",
                 "~/Scripts/select2.min.js",//Select 2
                 "~/Scripts/i18n/pt-BR.js",//Select 2
                "~/Scripts/home2.js")
            );

            //Reports
            bundles.Add(new StyleBundle("~/bundles/panovoBundle")
                .Include("~/Content/Reports/Reports1.css",
                "~/Content/PANovo/css/style.css",
                "~/Content/PANovo/css/jquery.dataTables.min.css",
                "~/Content/PANovo/css/buttons.dataTables.min.css",
                "~/Content/PANovo/css/bootstrap-select.min.css",
                "~/Content/PANovo/css/daterangepicker.css",
                "~/Content/PANovo/css/bootstrap.min.css",
                "~/Content/PANovo/css/bootstrap-theme.min.css",
                "~/Content/PANovo/css/bootstrap-glyphicons.css",
                "~/Content/css/select2.min.css"
                )
            );
            #endregion

        }
    }
}
