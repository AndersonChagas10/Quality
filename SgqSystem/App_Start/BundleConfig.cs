using System.Web.Optimization;

namespace SgqSystem
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region Scripts

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            //HighCharts
            bundles.Add(new ScriptBundle("~/bundles/hc")
                .Include("~/Scripts/highcharts/4.2.0/highcharts.js")
            );

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

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
                    "~/Scripts/DataTables/buttons.print.min.js"
                    //"~/Scripts/DataTables/dataTables.responsive.min.js"
                    //"~/Scripts/jbs.jquery.dataTables.configuration.js",
                    //"~/Scripts/dataTable.CRUD.js",
                    ));
            
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                      "~/Scripts/modernizr-*"));
            
            bundles.Add(new ScriptBundle("~/bundles/theme").Include(
                        "~/Content/bootstrap-switch/js/bootstrap-switch.min.js",
                        "~/Scripts/theme/js/app.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/select2js").Include(
                     "~/Scripts/select2.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/DatePikerContent").Include(
                    "~/Scripts/moment.min.js",
                    "~/Scripts/daterangepicker.js",
                    "~/Scripts/DatePikerReady.js"));

            #endregion

            #region Styles

            bundles.Add(new StyleBundle("~/Content/General").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-theme.min.css.map"));

            bundles.Add(new StyleBundle("~/Content/select2css").Include(
                   "~/Content/select2.min.css"));


            bundles.Add(new StyleBundle("~/Content/Tables").Include(
                    //"~/Content/datatables.min.css",
                    "~/Content/DataTables/css/jquery.dataTables.min.css",
                    "~/Content/DataTables/css/buttons.dataTables.min.css"
                ));

            bundles.Add(new StyleBundle("~/Content/Theme").Include(
                "~/Scripts/theme/css/components.min.css",
                "~/Scripts/theme/css/darkblue.min.css",
                "~/Scripts/theme/font-awesome-4.6.3/css/font-awesome.min.css",
                "~/Scripts/theme/css/layout.min.css",
                "~/Content/bootstrap-switch/css/bootstrap3/bootstrap-switch.min.css"
            ));

            bundles.Add(new StyleBundle("~/Content/Erp").Include(
                "~/Content/erp.css"
            ));

            bundles.Add(new StyleBundle("~/Content/DatePikerContentCss").Include(
                    "~/Content/daterangepicker-bs3.css"));

            #endregion


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.




            //bundles.Add(new StyleBundle("~/Content/DataTables").Include(
            //       "~/Content/DataTables/jquery.dataTables.min.css"));


        }
    }
}
