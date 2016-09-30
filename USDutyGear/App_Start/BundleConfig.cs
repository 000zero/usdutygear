using System.Web.Optimization;

namespace USDutyGear
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                      "~/Scripts/knockout-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/usdutygear").Include(
                        "~/Scripts/underscore-min.js",
                        "~/Scripts/moment.min.js",
                        "~/js/__constants.js",
                        "~/js/__usdutygear.js"));

            bundles.Add(new ScriptBundle("~/bundles/product").Include(
                        "~/Scripts/jquery.zoom.min.js",
                        "~/js/__product.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/home").Include(
                        "~/Content/home.css"));

            bundles.Add(new StyleBundle("~/Content/earth2").Include(
                        "~/Content/earth2_home.css"));

            bundles.Add(new StyleBundle("~/Content/product").Include(
                        "~/Content/product.css"));

            bundles.Add(new StyleBundle("~/Content/cart").Include(
                        "~/Content/cart.css"));

            bundles.Add(new StyleBundle("~/Content/about").Include(
                        "~/Content/about.css"));

            bundles.Add(new StyleBundle("~/Content/ie").Include(
                        "~/Content/ie-10-11.css",
                        "~/Content/ie-12.css"));
        }
    }
}
