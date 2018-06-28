using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace eApp.Web.Admin
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // ---------------- SUPPLIER 

            #region SUPPLIER
            
            // -- POST

            routes.MapRoute(
                name: "AddSupplier",
                url: "Supplier/AddSupplier",
                defaults: new { controller = "Supplier", action = "AddSupplier" });


            // -- JSON

            routes.MapRoute(
                name: "GetSupplierCombiAll",
                url: "Supplier/GetSupplierCombiAll",
                defaults: new { controller = "Supplier", action = "GetSupplierCombiAll" });

            // -- VIEW

            routes.MapRoute(
                name: "SupplierList",
                url: "Supplier/SupplierList",
                defaults: new { controller = "Supplier", action = "SupplierList" });

            routes.MapRoute(
                name: "NewSupplier",
                url: "Supplier/NewSupplier",
                defaults: new { controller = "Supplier", action = "NewSupplier" });

            #endregion


            #region Monitoring Report

            routes.MapRoute(
                name: "SupplierLog",
                url: "Monitor/SupplierLog",
                defaults: new { controller = "Monitor", action = "SupplierLog" });

            routes.MapRoute(
                name: "SupplierActivities",
                url: "Monitor/SupplierActivities",
                defaults: new { controller = "Monitor", action = "SupplierActivities" });

            routes.MapRoute(
                name: "SupplierActivityInfo",
                url: "Monitor/SupplierActivityInfo",
                defaults: new { controller = "Monitor", action = "SupplierActivityInfo" });

            #endregion

            #region Purchasing Reporting

            routes.MapRoute(
               name: "SelectDatePO",
               url: "Report/SelectDatePO",
               defaults: new { controller = "Report", action = "SelectDatePO" });

            routes.MapRoute(
                name: "MonthlyPO",
                url: "Report/MonthlyPO",
                defaults: new { controller = "Report", action = "MonthlyPO" });

            routes.MapRoute(
                name: "OverAllPO",
                url: "Report/OverAllPO",
                defaults: new { controller = "Report", action = "OverAllPO" });
            
            routes.MapRoute(
                name: "DailyPO",
                url: "Report/DailyPO",
                defaults: new { controller = "Report", action = "DailyPO" });
            
            #region Agent

            routes.MapRoute(
                name: "SupplierReport",
                url: "Report/SupplierReport",
                defaults: new { controller = "Report", action = "SupplierReport" });

            routes.MapRoute(
                name: "SupplierPO",
                url: "Report/SupplierPO",
                defaults: new { controller = "Report", action = "SupplierPO" });

            routes.MapRoute(
                name: "SupplierPORangeDate",
                url: "Report/SupplierPORangeDate",
                defaults: new { controller = "Report", action = "SupplierPORangeDate" });

            #endregion

            #endregion

            #region Supplier Complain

            routes.MapRoute(
                name: "ComplainList",
                url: "Complain/ComplainList",
                defaults: new { controller = "Complain", action = "ComplainList" });
                      
            routes.MapRoute(
                name: "ComplainInfo",
                url: "Complain/ComplainInfo",
                defaults: new { controller = "Complain", action = "ComplainInfo" });

            routes.MapRoute(
                name: "ComplainInfoReplies",
                url: "Complain/ComplainInfoReplies",
                defaults: new { controller = "Complain", action = "ComplainInfoReplies" });

            routes.MapRoute(
                name: "ComplainInfoReply",
                url: "Complain/ComplainInfoReply",
                defaults: new { controller = "Complain", action = "ComplainInfoReply" });

            routes.MapRoute(
               name: "ComplainSolved",
               url: "Complain/ComplainSolved",
               defaults: new { controller = "Complain", action = "ComplainSolved" });
            
            routes.MapRoute(
               name: "ComplainListofSolved",
               url: "Complain/ComplainListofSolved",
               defaults: new { controller = "Complain", action = "ComplainListofSolved" });

            routes.MapRoute(
               name: "ComplainInfoSolved",
               url: "Complain/ComplainInfoSolved",
               defaults: new { controller = "Complain", action = "ComplainInfoSolved" });

            #endregion

            #region Ranking Request

            routes.MapRoute(
                name: "ViewRank",
                url: "Rank/Index",
                defaults: new { controller = "Rank", action = "Index" });

            routes.MapRoute(
                name: "RankList",
                url: "Rank/RankList",
                defaults: new { controller = "Rank", action = "RankList" });

            routes.MapRoute(
                name: "ProcessingList",
                url: "Rank/ProcessingList",
                defaults: new { controller = "Rank", action = "ProcessingList" });

            routes.MapRoute(
                name: "UpdateRanking",
                url: "Rank/UpdateRanking",
                defaults: new { controller = "Rank", action = "UpdateRanking" });

            #endregion

            #region Purchasing Order

            routes.MapRoute(
                name: "POIndex",
                url: "PO/Index",
                defaults: new { controller = "PO", action = "Index" });

            routes.MapRoute(
                name: "PoList",
                url: "PO/PoList",
                defaults: new { controller = "PO", action = "PoList" });


            #endregion

            #region New Supplier

            routes.MapRoute(
                name: "NewAccount",
                url: "Supplier/NewAccount",
                defaults: new { controller = "Supplier", action = "NewAccount" });

            routes.MapRoute(
                name: "RegisterNewAccount",
                url: "Supplier/RegisterNewAccount",
                defaults: new { controller = "Supplier", action = "RegisterNewAccount" });

            #endregion

            #region Supplier Information

            routes.MapRoute(
                name: "Search",
                url: "Supplier/Search",
                defaults: new { controller = "Supplier", action = "Search" });

            routes.MapRoute(
                name: "UserList",
                url: "Supplier/UserList",
                defaults: new { controller = "Supplier", action = "UserList" });
            
            routes.MapRoute(
                name: "UserRegion",
                url: "Supplier/UserRegion",
                defaults: new { controller = "Supplier", action = "UserRegion" });

            routes.MapRoute(
                name: "AddSupplierRegion",
                url: "Supplier/AddSupplierRegion",
                defaults: new { controller = "Supplier", action = "AddSupplierRegion" });

            routes.MapRoute(
                name: "UserEdit",
                url: "Supplier/UserEdit",
                defaults: new { controller = "Supplier", action = "UserEdit" });

            routes.MapRoute(
                name: "ResetPasswordSupplier",
                url: "Supplier/ResetPasswordSupplier",
                defaults: new { controller = "Supplier", action = "ResetPasswordSupplier" });

            routes.MapRoute(
                name: "UpdateInfo",
                url: "Supplier/UpdateInfo",
                defaults: new { controller = "Supplier", action = "UpdateInfo" });




            #endregion

            #region Agent Creation

            routes.MapRoute(
                name: "NewAgent",
                url: "Agent/NewAgent",
                defaults: new { controller = "Agent", action = "NewAgent" });

            routes.MapRoute(
                name: "AgentSubmit",
                url: "Agent/AgentSubmit",
                defaults: new { controller = "Agent", action = "AgentSubmit" });

            routes.MapRoute(
                name: "AgentStatus",
                url: "Agent/AgentStatus",
                defaults: new { controller = "Agent", action = "AgentStatus" });

            routes.MapRoute(
                name: "AgentList",
                url: "Agent/AgentList",
                defaults: new { controller = "Agent", action = "AgentList" });

            #endregion

            #region Authentication

            routes.MapRoute(
                name: "Login",
                url: "Account/Login",
                defaults: new { controller = "Account", action = "Login" });

            routes.MapRoute(
                name: "LogOff",
                url: "Account/LogOff",
                defaults: new { controller = "Account", action = "LogOff" });

            routes.MapRoute(
               name: "SessionExpired",
               url: "Account/SessionExpired",
               defaults: new { controller = "Account", action = "SessionExpired" });

            routes.MapRoute(
               name: "AddAgent",
               url: "Account/AddAgent",
               defaults: new { controller = "Account", action = "AddAgent" });

            routes.MapRoute(
              name: "SwitchRegion",
              url: "Account/SwitchRegion",
              defaults: new { controller = "Account", action = "SwitchRegion" });

            routes.MapRoute(
              name: "SwitchRegionProcess",
              url: "Account/SwitchRegionProcess",
              defaults: new { controller = "Account", action = "SwitchRegionProcess" });

            routes.MapRoute(
              name: "ResetMobile",
              url: "Account/ResetMobile",
              defaults: new { controller = "Account", action = "ResetMobile" });


            #endregion

            routes.MapRoute(
                name: "Startup",
                url: "Home/Startup",
                defaults: new { controller = "Home", action = "Startup" });
            
            routes.MapRoute(
                name: "Default",
                url: "{*url}",
                defaults: new { controller = "Home", action = "Index" });
            
        }
    }
}
