using System.Web;
using System.Web.Optimization;

namespace PlanoDeAcaoMVC
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            //HighCharts
            bundles.Add(new ScriptBundle("~/bundles/hc")
                .Include("~/Scripts/highcharts/4.2.0/highcharts.js")
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
                    "~/Scripts/select2.min.js"
                    , "~/Scripts/i18n/pt-BR.js"));

            bundles.Add(new StyleBundle("~/Content/select2css").Include(
                  "~/Content/css/select2.min.css"));

            #endregion
        }
    }
}
