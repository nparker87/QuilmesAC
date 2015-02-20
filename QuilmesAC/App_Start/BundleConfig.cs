namespace QuilmesAC.App_Start
{
    using Helpers;
    using System.Web.Optimization;

    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
#if !DEBUG
            BundleTable.EnableOptimizations = true;
#endif

            bundles.Add(new ScriptBundle("~/Scripts").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery-ui-{version}.js",
                "~/Scripts/jquery.jqGrid.src.js",
                "~/Scripts/i18n/grid.locale-en.js",
                "~/Scripts/jquery.validate.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/less-{version}.js",
                "~/Scripts/Global.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/themes/custom/jquery-ui.theme.css",
                "~/Content/jquery.jqGrid/ui.jqgrid.css"));

            var lessBundle = new Bundle("~/Content/less").Include("~/Content/Less/Site.less");
            lessBundle.Transforms.Add(new LessTransform());
            bundles.Add(lessBundle);
        }
    }
}