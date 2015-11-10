using System.Web;
using System.Web.Optimization;

namespace ClientSite
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

			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
						"~/Scripts/jquery.unobtrusive*",
						"~/Scripts/jquery.validate*"));

			bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/bootstrap.css",
																 "~/Content/shop-item.css",
																 "~/Content/DriverMenu.css"));

			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Scripts/modernizr-*", "~/Scripts/jquery-1.11.3.js", "~/Scripts/bootstrap.js"));

			bundles.Add(new ScriptBundle("~/bundles/handlebars").Include(
																	"~/Scripts/handlebars.js"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
						"~/Scripts/bootstrap.js",
						"~/Scripts/bootstrap-filestyle.js"));

			bundles.Add(new ScriptBundle("~/bundles/handlebars").Include(
																	"~/Scripts/handlebars.js"));

			bundles.Add(new ScriptBundle("~/bundles/inputmask").Include(
			"~/Scripts/jquery.inputmask/inputmask.js",
			"~/Scripts/jquery.inputmask/jquery.inputmask.js",
						"~/Scripts/jquery.inputmask/inputmask.extensions.js",
						"~/Scripts/jquery.inputmask/inputmask.date.extensions.js",
				//and other extensions you want to include
						"~/Scripts/jquery.inputmask/inputmask.numeric.extensions.js"));

			bundles.Add(new ScriptBundle("~/bundles/datetime").Include(
																	"~/Scripts/moment*",
																	"~/Scripts/bootstrap-datetimepicker*"));

			bundles.Add(new ScriptBundle("~/bundles/GoogleMapJS/GoogleMapUserPage").Include(
						"~/Scripts/GoogleMapJS/GoogleMapUserPage.js"));

			bundles.Add(new StyleBundle("~/Content/UserMenu/UserMenuMaps").Include(
						"~/Content/UserMenu/UserMenuMaps.css"));


		}
	}
}
