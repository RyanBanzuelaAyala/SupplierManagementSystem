using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace eApp.Web.Client
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            #region Performance

            routes.MapRoute(
                name: "Summary",
                url: "Performance/Summary",
                defaults: new { controller = "Performance", action = "Summary" });

            routes.MapRoute(
                name: "GetAllPOSummary",
                url: "Performance/GetAllPOSummary",
                defaults: new { controller = "Performance", action = "GetAllPOSummary" });

            routes.MapRoute(
                name: "GetAllPOSummaryByRegion",
                url: "Performance/GetAllPOSummaryByRegion",
                defaults: new { controller = "Performance", action = "GetAllPOSummaryByRegion" });

            routes.MapRoute(
                name: "GetAllPOSummaryByBranchRegion",
                url: "Performance/GetAllPOSummaryByBranchRegion",
                defaults: new { controller = "Performance", action = "GetAllPOSummaryByBranchRegion" });

            routes.MapRoute(
                name: "GetAllPOSummaryByPOPerBranchRegion",
                url: "Performance/GetAllPOSummaryByPOPerBranchRegion",
                defaults: new { controller = "Performance", action = "GetAllPOSummaryByPOPerBranchRegion" });

            #endregion

            #region Supplier Complains

            routes.MapRoute(
                name: "Issue",
                url: "Complain/Issue",
                defaults: new { controller = "Complain", action = "Issue" });

            routes.MapRoute(
                name: "SubmitCompDL",
                url: "Complain/SubmitCompDL",
                defaults: new { controller = "Complain", action = "SubmitCompDL" });

            routes.MapRoute(
                name: "IssueList",
                url: "Complain/IssueList",
                defaults: new { controller = "Complain", action = "IssueList" });

            routes.MapRoute(
                name: "IssueListInfo",
                url: "Complain/IssueListInfo",
                defaults: new { controller = "Complain", action = "IssueListInfo" });

            routes.MapRoute(
                name: "ComplainInfoReplies",
                url: "Complain/ComplainInfoReplies",
                defaults: new { controller = "Complain", action = "ComplainInfoReplies" });

            routes.MapRoute(
                name: "ComplainInfoReply",
                url: "Complain/ComplainInfoReply",
                defaults: new { controller = "Complain", action = "ComplainInfoReply" });

            routes.MapRoute(
                name: "ComplainGetEvents",
                url: "Complain/GetEvents",
                defaults: new { controller = "Complain", action = "GetEvents" });

            #endregion

            #region Profile Setting

            routes.MapRoute(
                name: "ChangePassword",
                url: "Account/ChangePassword",
                defaults: new { controller = "Account", action = "ChangePassword" });

            routes.MapRoute(
                name: "ResetPassword",
                url: "Account/ResetPassword",
                defaults: new { controller = "Account", action = "ResetPassword" });
            
            #endregion

            #region Ranking Request

            #region Brand

            routes.MapRoute(
                name: "Brand",
                url: "RankRoute/Brand",
                defaults: new { controller = "RankRoute", action = "Brand" });

            routes.MapRoute(
                name: "searchCSV",
                url: "RankRoute/searchCSV/{brandcode}",
                defaults: new { controller = "RankRoute", action = "searchCSV", brandcode = UrlParameter.Optional });

            routes.MapRoute(
                name: "searchCSVB",
                url: "RankRoute/searchCSVB/{brandcode}",
                defaults: new { controller = "RankRoute", action = "searchCSVB", brandcode = UrlParameter.Optional });

            routes.MapRoute(
                name: "SubmitBrand",
                url: "RankRoute/SubmitBrand",
                defaults: new { controller = "RankRoute", action = "SubmitBrand" });

            routes.MapRoute(
                name: "GetAllRanking",
                url: "RankRoute/GetAllRanking",
                defaults: new { controller = "RankRoute", action = "GetAllRanking" });

            #endregion

            #region SKU

            routes.MapRoute(
                name: "SKU",
                url: "RankRoute/SKU",
                defaults: new { controller = "RankRoute", action = "SKU" });

            routes.MapRoute(
                name: "SubmitSKU",
                url: "RankRoute/SubmitSKU",
                defaults: new { controller = "RankRoute", action = "SubmitSKU" });

            #endregion

            #region Department

            routes.MapRoute(
                name: "viewDeptSupp",
                url: "RankRoute/viewDeptSupp",
                defaults: new { controller = "RankRoute", action = "viewDeptSupp" });

            routes.MapRoute(
                name: "Department",
                url: "RankRoute/Department",
                defaults: new { controller = "RankRoute", action = "Department" });

            routes.MapRoute(
                name: "deptsearch",
                url: "RankRoute/searchDept/{dept}/{sdept}/{iclass}/{siclass}/",
                defaults: new
                {
                    controller = "RankRoute",
                    action = "searchDept",
                    dept = UrlParameter.Optional,
                    sdept = UrlParameter.Optional,
                    iclass = UrlParameter.Optional,
                    siclass = UrlParameter.Optional
                });

            routes.MapRoute(
                name: "SubmitDept",
                url: "RankRoute/SubmitDept",
                defaults: new { controller = "RankRoute", action = "SubmitDept" });
            
            #endregion

            routes.MapRoute(
                name: "RequestList",
                url: "RankRoute/RequestList",
                defaults: new { controller = "RankRoute", action = "RequestList" });

            routes.MapRoute(
                name: "SubmitRankDL",
                url: "RankRoute/SubmitRankDL/{reqid}",
                defaults: new { controller = "RankRoute", action = "SubmitRankDL", reqid = UrlParameter.Optional });
            
            #endregion

            #region Authentication

            routes.MapRoute(
                name: "Login",
                url: "Account/Login",
                defaults: new { controller = "Account", action = "Login" });

            routes.MapRoute(
                name: "Register",
                url: "Account/Register",
                defaults: new { controller = "Account", action = "Register" });

            routes.MapRoute(
                name: "ResendCode",
                url: "Account/ResendCode",
                defaults: new { controller = "Account", action = "ResendCode" });

            routes.MapRoute(
                name: "ResetMobileActivation",
                url: "Account/ResetMobileActivation",
                defaults: new { controller = "Account", action = "ResetMobileActivation" });

            routes.MapRoute(
                name: "CodeValidate",
                url: "Account/CodeValidate",
                defaults: new { controller = "Account", action = "CodeValidate" });

            routes.MapRoute(
                name: "LogOff",
                url: "Account/LogOff",
                defaults: new { controller = "Account", action = "LogOff" });

            routes.MapRoute(
                name: "SessionExpired",
                url: "Account/SessionExpired",
                defaults: new { controller = "Account", action = "SessionExpired" });

            routes.MapRoute(
                name: "RegisterAll",
                url: "Account/RegisterAll",
                defaults: new { controller = "Account", action = "RegisterAll" });
            
            routes.MapRoute(
                name: "SwitchRegion",
                url: "Account/SwitchRegion",
                defaults: new { controller = "Account", action = "SwitchRegion" });

            routes.MapRoute(
                name: "SwitchRegionProcess",
                url: "Account/SwitchRegionProcess",
                defaults: new { controller = "Account", action = "SwitchRegionProcess" });


            #endregion

            #region Purchasing Order

            routes.MapRoute(
                name: "PO",
                url: "PORoute/PO",
                defaults: new { controller = "PORoute", action = "PO" });

            routes.MapRoute(
                name: "SubmitPO",
                url: "PORoute/SubmitPO",
                defaults: new { controller = "PORoute", action = "SubmitPO" });
            
            routes.MapRoute(
                name: "GetAllPO",
                url: "PORoute/GetAllPO",
                defaults: new { controller = "PORoute", action = "GetAllPO" });

            routes.MapRoute(
                name: "_Template",
                url: "PORoute/_tempdl",
                defaults: new { controller = "PORoute", action = "_tempdl" });

            #endregion

            routes.MapRoute(
                name: "Startup",
                url: "Home/Startup",
                defaults: new { controller = "Home", action = "Startup" });

            routes.MapRoute(
                name: "GetEvents",
                url: "Home/GetEvents",
                defaults: new { controller = "Home", action = "GetEvents" });

            routes.MapRoute(
                name: "GetStatistic",
                url: "Home/GetStatistic",
                defaults: new { controller = "Home", action = "GetStatistic" });

            routes.MapRoute(
                name: "Default",
                url: "{*url}",
                defaults: new { controller = "Home", action = "Index" });

        }
    }
}
