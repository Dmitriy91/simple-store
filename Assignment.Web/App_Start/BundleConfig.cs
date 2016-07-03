using System.Web;
using System.Web.Optimization;

namespace Assignment.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/vendors").Include(
                "~/Scripts/vendors/jquery-{version}.js",
                "~/Scripts/vendors/bootstrap.js",
                "~/Scripts/vendors/toastr.js",
                "~/Scripts/vendors/angular.js",
                "~/Scripts/vendors/angular-route.js",
                "~/Scripts/vendors/angular-cookies.js",
                "~/Scripts/vendors/angular-animate.js",
                "~/Scripts/vendors/loading-bar.js",
                "~/Scripts/vendors/bootstrap-datepicker.js"));

            bundles.Add(new ScriptBundle("~/Scripts/app").Include(
                "~/Scripts/app/app.js",
                "~/Scripts/app/modules/app.core.module.js",
                "~/Scripts/app/modules/app.layout.module.js",
                "~/Scripts/app/layout/top-bar.directive.js",
                "~/Scripts/app/services/data.service.js",
                "~/Scripts/app/services/notification.service.js",
                "~/Scripts/app/services/membership.service.js",
                "~/Scripts/app/home/root.controller.js",
                "~/Scripts/app/home/index.controller.js",
                "~/Scripts/app/account/login.controller.js",
                "~/Scripts/app/account/register.controller.js",
                "~/Scripts/app/products/products.controller.js",
                "~/Scripts/app/products/products-add.controller.js",
                "~/Scripts/app/products/products-details.controller.js",
                "~/Scripts/app/products/products-edit.controller.js",
                "~/Scripts/app/customers/customers.controller.js",
                "~/Scripts/app/customers/customers-add-juridical-person.controller.js",
                "~/Scripts/app/customers/customers-add-natural-person.controller.js",
                "~/Scripts/app/customers/customers-details-juridical-person.controller.js",
                "~/Scripts/app/customers/customers-details-natural-person.controller.js",
                "~/Scripts/app/customers/customers-edit-juridical-person.controller.js",
                "~/Scripts/app/customers/customers-edit-natural-person.controller.js",
                "~/Scripts/app/orders/orders.controller.js",
                "~/Scripts/app/orders/orders-add.controller.js",
                "~/Scripts/app/orders/orders-edit.controller.js",
                "~/Scripts/app/components/compare-to.directive.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/css/bootstrap-themes/bootstrap.flatly.css",
                "~/Content/css/bootstrap-datepicker3.standalone.css",
                "~/Content/css/toastr.css",
                "~/Content/css/loading-bar.css",
                "~/Content/css/site.css"));

            BundleTable.EnableOptimizations = false;
        }
    }
}
