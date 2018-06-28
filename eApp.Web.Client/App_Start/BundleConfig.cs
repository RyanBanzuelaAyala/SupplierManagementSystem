using System.Web;
using System.Web.Optimization;

namespace eApp.Web.Client
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/dnb")
                .Include("~/Scripts/Factories/DataFactory.js")
                .Include("~/Scripts/Factories/ConfirmFactory.js")
                .Include("~/Scripts/Factories/ValidateFactory.js")
                .Include("~/Scripts/Factories/GenerateFactory.js")
                .Include("~/Scripts/Factories/InfoFactory.js")
                .Include("~/Scripts/Factories/UserFactory.js")
                .Include("~/Scripts/Controllers/Complain/rComplainController.js")
                .Include("~/Scripts/Controllers/Complain/ComplainController.js")
                .Include("~/Scripts/Controllers/Account/ResetController.js")                
                .Include("~/Scripts/Controllers/Purchasing/PoController.js")

                .Include("~/Scripts/Controllers/Ranking/RankController.js")
                .Include("~/Scripts/Controllers/Ranking/RankListController.js")
                .Include("~/Scripts/Controllers/Performance/PerformanceController.js")

                

                //.Include("~/Scripts/Controllers/Ranking/DeptController.js")
                //.Include("~/Scripts/Controllers/Ranking/SkuController.js")
                //.Include("~/Scripts/Controllers/Ranking/BrandController.js")
                .Include("~/Scripts/Controllers/Profile/LandingPageController.js")
                .Include("~/Scripts/Factories/Profile/AuthHttpResponseInterceptor.js")
                .Include("~/Scripts/Services/dnbservices.js")
                .Include("~/Scripts/Module/dnb.js"));

            bundles.Add(new ScriptBundle("~/bundles/Login")
                .Include("~/Scripts/Services/dnbservices.js")
                .Include("~/Scripts/Controllers/Account/LoginController.js")                
                .Include("~/Scripts/Module/Login.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/site.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
