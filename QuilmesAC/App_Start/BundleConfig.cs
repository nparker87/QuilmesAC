namespace QuilmesAC.App_Start
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
#if !DEBUG
            BundleTable.EnableOptimizations = true;
#endif

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/themes/custom/jquery-ui.theme.css",
                "~/Content/jquery.jqGrid/ui.jqgrid.css",
                "~/Content/Less/Site.less"));

            bundles.Add(new ScriptBundle("~/Scripts").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery-ui-{version}.js",
                "~/Scripts/jquery.jqGrid.src.js",
                "~/Scripts/i18n/grid.locale-en.js",
                "~/Scripts/jquery.validate.js",
                "~/Scripts/jquery.validate.unobtrusive.js",
                "~/Scripts/less-{version}.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/Global.js"));
        }
    }
}