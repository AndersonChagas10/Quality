using System.Web.Optimization;

namespace SgqSystem
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            //HighCharts
            bundles.Add(new ScriptBundle("~/bundles/hc")
                .Include("~/Scripts/highcharts/4.2.0/highcharts.js")
            //.Include("~/Scripts/highcharts/4.2.0/highcharts.src.js")
            );

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      //"~/Content/bootstrap-*",C:\Users\Celso\Source\Repos\ddd.bitbucket7\SgqSystem\Content\bootstrap-theme.css
                      "~/Content/bootstrap-theme.min.css.map", 
                      "~/Content/bootstrap-theme.css.map",
                      "~/Content/site.css"));


            bundles.Add(new ScriptBundle("~/bundles/DataTables").Include(
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
                     //"~/Scripts/jbs.jquery.dataTables.configuration.js",
                     //"~/Scripts/dataTable.CRUD.js",
                     //"~/Scripts/buttonsDataTable/buttons.html5.min.js",
                     //"~/Scripts/buttonsDataTable/buttons.flash.min.js",
                     //"~/Scripts/buttonsDataTable/buttons.print.min.js",
                     //"~/Scripts/buttonsDataTable/dataTables.buttons.min.js",
                     //"~/Scripts/buttonsDataTable/jszip.min.js",
                     //"~/Scripts/buttonsDataTable/pdfmake.min.js",
                     //"~/Scripts/buttonsDataTable/vfs_fonts.js",
                     //"~/Scripts/Moment/moment.min.js"
                     ));

            //bundles.Add(new StyleBundle("~/Content/DataTables").Include(
            //       "~/Content/DataTables/jquery.dataTables.min.css"));


        }
    }
}
