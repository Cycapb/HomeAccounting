using System.Web.Optimization;


namespace WebUI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                "~/Scripts/knockout-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-ajax").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-val").Include(
                "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval-ajax").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.validate*",
                "~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles.Add(new ScriptBundle("~/bundles/datetime-datepicker").Include(
                "~/Scripts/bootstrap-datepicker.js",
                "~/Scripts/locales/bootstrap-datepicker.ru.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/material-kit").Include(
                "~/Content/assets/js/material.min.js",
                "~/Content/assets/js/material-kit.js"));

            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                "~/Scripts/UpdateAccounts.js",
                "~/Scripts/ChangeButtonColor.js",
                "~/Scripts/CalendarScript.js",
                "~/Scripts/Buttons.js"));

            //Use the development version of Modernizr to develop with and learn from. Then, when you're
            //ready for production, use the build tool at http://modernizr.com to pick only the tests you need.


            bundles.Add(new StyleBundle("~/bundles/bootstrap-datepicker/css").Include(
                "~/Content/bootstrap-theme.css",
                "~/Content/bootstrap-datepicker3.css",
                "~/Content/bootstrap.min.css",
                "~/Content/ErrorStyles.css")
                );

            bundles.Add(new StyleBundle("~/bundles/bootstrap/css").Include(
                "~/Content/bootstrap-theme.css",
                "~/Content/bootstrap.min.css",
                "~/Content/ErrorStyles.css",
                "~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/bundles/bootstrap-site/css").Include(
                "~/Content/bootstrap-theme.css",
                "~/Content/bootstrap.min.css",
                "~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/bundles/material-kit/css").Include(                
                "~/Content/assets/css/material-kit.css"));
        }
    }
}