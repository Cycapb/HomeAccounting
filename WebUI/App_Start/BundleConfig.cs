using System.Web.Optimization;


namespace WebUI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/bootstrap-material-kit").Include(
                "~/Content/assets/js/bootstrap.min.js",
                "~/Content/assets/js/material.min.js",
                "~/Content/assets/js/material-kit.js")
            );

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-datepicker").Include(
                "~/Content/assets/js/bootstrap.min.js",
                "~/Content/assets/js/bootstrap-datepicker.js",
                "~/Scripts/locales/bootstrap-datepicker.ru.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/jquery-material-kit-datepicker").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Content/assets/js/bootstrap.min.js",
                "~/Content/assets/js/material.min.js",
                "~/Content/assets/js/material-kit.js",
                "~/Content/assets/js/bootstrap-datepicker.js",
                "~/Scripts/locales/bootstrap-datepicker.ru.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                "~/Scripts/knockout-{version}.js",
                "~/Scripts/knockout.validation.js"));

            bundles.Add(new ScriptBundle("~/bundles/ajax").Include(
                "~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-ajax").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles.Add(new ScriptBundle("~/bundles/validation").Include(
                "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-loginPage").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.validate*",
                "~/Scripts/jquery.unobtrusive-ajax.js",
                "~/Scripts/jquery.blockUI.js",
                "~/Content/assets/js/bootstrap.min.js",
                "~/Content/assets/js/material.min.js",
                "~/Content/assets/js/material-kit.js",
                "~/Scripts/LoaderScripts.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-reportPage").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.unobtrusive-ajax.js",
                "~/Content/assets/js/bootstrap.min.js",
                "~/Content/assets/js/bootstrap-datepicker.js",
                "~/Scripts/locales/bootstrap-datepicker.ru.min.js",
                "~/Scripts/jquery.blockUI.js",
                "~/Content/assets/js/material-kit.js",
                "~/Scripts/CalendarScript.js",
                "~/Scripts/LoaderScripts.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-adminPage").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.unobtrusive-ajax.js",
                "~/Content/assets/js/bootstrap.min.js",
                "~/Content/assets/js/material.min.js",
                "~/Content/assets/js/material-kit.js",
                "~/Scripts/jquery.blockUI.js",
                "~/Scripts/LoaderScripts.js",
                "~/Scripts/Buttons.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-mainPage").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Content/assets/js/bootstrap.min.js",
                "~/Content/assets/js/material.min.js",
                "~/Content/assets/js/material-kit.js",
                "~/Content/assets/js/bootstrap-datepicker.js",
                "~/Scripts/locales/bootstrap-datepicker.ru.min.js",
                "~/Scripts/jquery.unobtrusive-ajax.js",
                "~/Scripts/jquery.blockUI.js",
                "~/Scripts/UpdateAccounts.js",
                "~/Scripts/ChangeButtonColor.js",
                "~/Scripts/CalendarScript.js",
                "~/Scripts/Buttons.js",
                "~/Scripts/LoaderScripts.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-validation").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.validate*"));

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
                "~/Content/bootstrap.min.css",
                "~/Content/bootstrap-datepicker.css",
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
                "~/Content/assets/css/bootstrap.min.css",
                "~/Content/assets/css/material-kit.css"
            ));
        }
    }
}