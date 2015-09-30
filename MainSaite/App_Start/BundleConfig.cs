using System.Web;
using System.Web.Optimization;

namespace MainSaite
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
						"~/Scripts/bootstrap.js",
						"~/Scripts/bootstrap-filestyle.js"));  

			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

			bundles.Add(new ScriptBundle("~/bundles/coordinates").Include(
						"~/Scripts/coordinates.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

			bundles.Add(new ScriptBundle("~/bundles/tabHelper").Include(
						"~/Scripts/tabCorrector.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));
			
			bundles.Add(new ScriptBundle("~/bundles/inputmask").Include(
			"~/Scripts/jquery.inputmask/inputmask.js",
			"~/Scripts/jquery.inputmask/jquery.inputmask.js",
						"~/Scripts/jquery.inputmask/inputmask.extensions.js",
						"~/Scripts/jquery.inputmask/inputmask.date.extensions.js",
				//and other extensions you want to include
						"~/Scripts/jquery.inputmask/inputmask.numeric.extensions.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Scripts/modernizr-*", "~/Scripts/jquery-1.11.3.js", "~/Scripts/bootstrap.js"));

			bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/bootstrap.css",
																 "~/Content/shop-item.css",
																 "~/Content/DriverMenu.css"));

			bundles.Add(new ScriptBundle("~/bundles/datetime").Include(
																	"~/Scripts/moment*",
																	"~/Scripts/bootstrap-datetimepicker*"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css",
						"~/Content/bootstrap.css",
						"~/Content/BootstrapCallout.css"));
        }
    }
}