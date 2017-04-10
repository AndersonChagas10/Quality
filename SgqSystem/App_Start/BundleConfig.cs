using System.Web.Optimization;

namespace SgqSystem
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region Scripts

            bundles.Add(new ScriptBundle("~/bundles/jquery")
                .Include("~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery-ui-1.12.1.min.js"
                ));

            //HighCharts
            bundles.Add(new ScriptBundle("~/bundles/hc")
                .Include("~/Scripts/highcharts/4.2.0/highcharts.js")
            );

            bundles.Add(new ScriptBundle("~/bundles/Guard")
             .Include("~/Scripts/GuardJs.js")
            );

            bundles.Add(new ScriptBundle("~/bundles/mask")
             .Include("~/Scripts/inputmask/min/jquery.inputmask.bundle.min.js")
            );

            bundles.Add(new ScriptBundle("~/bundles/bootstrap")
                .Include("~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/bootbox.min.js"));

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

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                      "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/theme").Include(
                        "~/Content/bootstrap-switch/js/bootstrap-switch.min.js",
                        "~/Scripts/theme/js/app.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/select2js").Include(
                     "~/Scripts/select2.min.js"
                     , "~/Scripts/i18n/pt-BR.js"));

            bundles.Add(new ScriptBundle("~/bundles/DatePikerContent").Include(
                    //"~/Scripts/moment.min.js",
                    "~/Scripts/moment-with-locales.min.js",
                    "~/Scripts/inputmask/jquery.inputmask.bundle.js",
                    "~/Scripts/daterangepicker.js",
                    "~/Scripts/DatePikerReady.js"));

            #endregion

            #region Styles

            bundles.Add(new StyleBundle("~/Content/General").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-theme.min.css.map",
                      "~/Content/jquery-ui.min.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/select2css").Include(
                   "~/Content/css/select2.min.css"));

            bundles.Add(new StyleBundle("~/Content/Tables").Include(
                    "~/Content/DataTables/css/jquery.dataTables.min.css",
                    "~/Content/DataTables/css/buttons.dataTables.min.css",
                    "~/Content/DataTables/css/responsive.dataTables.min.css",
                    "~/Content/DataTables/css/fixedColumns.dataTables.min.css",
                    "~/Content/DataTables/css/fixedHeader.dataTables.min.css"
                ));

            bundles.Add(new StyleBundle("~/Content/Theme").Include(
                "~/Scripts/theme/css/components.min.css",
                //"~/Scripts/theme/css/darkblue.min.css",
                "~/Scripts/theme/font-awesome-4.7.0/css/font-awesome.min.css",
                "~/Scripts/theme/css/layout.min.css",
                "~/Content/bootstrap-switch/css/bootstrap3/bootstrap-switch.min.css"
            ));

            bundles.Add(new StyleBundle("~/Content/Erp").Include(
                "~/Content/erp.css"
            ));

            bundles.Add(new StyleBundle("~/Content/DatePikerContentCss").Include(
                    "~/Content/daterangepicker.css"));

            //bundles.Add(new StyleBundle("~/Content/Hc").Include(
            //      "~/Content/daterangepicker.css"));
            #endregion

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.

            //bundles.Add(new StyleBundle("~/Content/DataTables").Include(
            //       "~/Content/DataTables/jquery.dataTables.min.css"));

        }
    }
}
