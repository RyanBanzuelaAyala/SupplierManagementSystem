using eApp.Web.Client.Resources.LibraryClass;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eApp.Web.Client.Controllers.Performance
{
    public class PerformanceController : Controller
    {

        #region VIEW

        public ActionResult Summary()
        {
            
            return View();
        }

        #endregion
        
        #region JSON
        
        [HttpGet]
        public JsonResult GetAllPOSummary(string daterange)
        {
            var reg = User.Identity.GetUserName().Substring(6, 3);
            var id = User.Identity.GetUserName().Substring(0, 6);

            char[] delimiterChars = { '-' };
            string[] drdate = daterange.Split(delimiterChars);

            var sdate = Convert.ToDateTime(drdate[0]);
            var edate = Convert.ToDateTime(drdate[1]);
            
            var ilist = new List<SupplierPerformance>();
            var summary = new xCSVPerformance();
            
            var requestQQ = summary.generateSummary(summary.generateSummaryRangeDate(summary.getSupplierData(id, reg, Server.MapPath("~/SPData/sadata.csv"), Server.MapPath("~/SPData/region.csv")), sdate, edate));

            foreach (var item in requestQQ)
            {
                ilist.Add(new xFormula(item, reg).performace());
            }

            return Json(ilist, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetAllPOSummaryByBranchRegion(string daterange)
        {
            var reg = User.Identity.GetUserName().Substring(6, 3);  
            var id = User.Identity.GetUserName().Substring(0, 6);

            char[] delimiterChars = { '-' };
            string[] drdate = daterange.Split(delimiterChars);

            var sdate = Convert.ToDateTime(drdate[0]);
            var edate = Convert.ToDateTime(drdate[1]);

            var summary = new xCSVPerformance();
            var requestQQ = summary.generateSummaryBranch(summary.generateSummaryBranchRangeDate(summary.getSupplierDataByBranch(id, reg, Server.MapPath("~/SPData/sadata.csv"), Server.MapPath("~/SPData/region.csv")), sdate, edate));

            return Json(requestQQ, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetAllPOSummaryByPOPerBranchRegion(string store, string daterange)
        {
            var reg = User.Identity.GetUserName().Substring(6, 3);
            var id = User.Identity.GetUserName().Substring(0, 6);


            char[] delimiterChars = { '-' };
            string[] drdate = daterange.Split(delimiterChars);

            var sdate = Convert.ToDateTime(drdate[0]);
            var edate = Convert.ToDateTime(drdate[1]);

            var summary = new xCSVPerformance();

            var requestQQ = summary.getSupplierDataByBranchStoreRangeDate(summary.getSupplierDataByBranchStore(id, reg, store, Server.MapPath("~/SPData/sadata.csv"), Server.MapPath("~/SPData/region.csv")), sdate, edate);

            return Json(requestQQ, JsonRequestBehavior.AllowGet);

        }
        
        #endregion

    }
}