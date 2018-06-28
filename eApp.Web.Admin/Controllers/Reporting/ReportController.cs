using eApp.Web.Admin.ADO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eApp.Web.Admin.Controllers.Reporting
{
    [Authorize]
    public class ReportController : Controller
    {
        private dnbmssqlEntities db = new dnbmssqlEntities();

        #region Administrator Reporting

        public ActionResult OverAllPO()
        {
            return View();
        }

        public ActionResult SelectDatePO(string xDate)
        {
            var cDate = Convert.ToDateTime(xDate).ToString("yyyy-MM-dd");

            ViewBag.CurrentDate = Convert.ToDateTime(xDate).ToString("MMMM dd, yyyy");

            var gg = db.poes.Count(s => s.region.Equals("JED") && s.released.Equals(cDate));
            ViewBag.jedOA = gg;

            var gg1 = db.poes.Count(s => s.region.Equals("RYD") && s.released.Equals(cDate));
            ViewBag.rydOA = gg1;

            var gg2 = db.poes.Count(s => s.region.Equals("KHB") && s.released.Equals(cDate));
            ViewBag.khbOA = gg2;

            ViewBag.OATotal = gg + gg1 + gg2;

            ViewBag.jedDL = db.poes.Count(s => s.region.Equals("JED") && s.released.Equals(cDate) && s.filestatus.Equals("Downloaded"));
            ViewBag.rydDL = db.poes.Count(s => s.region.Equals("RYD") && s.released.Equals(cDate) && s.filestatus.Equals("Downloaded"));
            ViewBag.khbDL = db.poes.Count(s => s.region.Equals("KHB") && s.released.Equals(cDate) && s.filestatus.Equals("Downloaded"));

            ViewBag.jedAV = db.poes.Count(s => s.region.Equals("JED") && s.released.Equals(cDate) && s.filestatus.Equals("Available"));
            ViewBag.rydAV = db.poes.Count(s => s.region.Equals("RYD") && s.released.Equals(cDate) && s.filestatus.Equals("Available"));
            ViewBag.khbAV = db.poes.Count(s => s.region.Equals("KHB") && s.released.Equals(cDate) && s.filestatus.Equals("Available"));
            
            ViewBag.jedDLU = db.poes.Count(s => s.region.Equals("JED") && s.released.Equals(cDate) && s.filestatus.Equals("Updated"));
            ViewBag.rydDLU = db.poes.Count(s => s.region.Equals("RYD") && s.released.Equals(cDate) && s.filestatus.Equals("Updated"));
            ViewBag.khbDLU = db.poes.Count(s => s.region.Equals("KHB") && s.released.Equals(cDate) && s.filestatus.Equals("Updated"));


            return View();
        }

        public ActionResult DailyPO()
        {
            var cDate = DateTime.Now.ToString("yyyy-MM-dd");

            var gg = db.poes.Count(s => s.region.Equals("JED") && s.released.Equals(cDate));
            ViewBag.jedOA = gg;

            var gg1 = db.poes.Count(s => s.region.Equals("RYD") && s.released.Equals(cDate));
            ViewBag.rydOA = gg1;

            var gg2 = db.poes.Count(s => s.region.Equals("KHB") && s.released.Equals(cDate));
            ViewBag.khbOA = gg2;

            ViewBag.OATotal = gg + gg1 + gg2;

            ViewBag.jedDL = db.poes.Count(s => s.region.Equals("JED") && s.released.Equals(cDate) && s.filestatus.Equals("Downloaded"));
            ViewBag.rydDL = db.poes.Count(s => s.region.Equals("RYD") && s.released.Equals(cDate) && s.filestatus.Equals("Downloaded"));
            ViewBag.khbDL = db.poes.Count(s => s.region.Equals("KHB") && s.released.Equals(cDate) && s.filestatus.Equals("Downloaded"));

            ViewBag.jedAV = db.poes.Count(s => s.region.Equals("JED") && s.released.Equals(cDate) && s.filestatus.Equals("Available"));
            ViewBag.rydAV = db.poes.Count(s => s.region.Equals("RYD") && s.released.Equals(cDate) && s.filestatus.Equals("Available"));
            ViewBag.khbAV = db.poes.Count(s => s.region.Equals("KHB") && s.released.Equals(cDate) && s.filestatus.Equals("Available"));

            ViewBag.jedDLU = db.poes.Count(s => s.region.Equals("JED") && s.released.Equals(cDate) && s.filestatus.Equals("Updated"));
            ViewBag.rydDLU = db.poes.Count(s => s.region.Equals("RYD") && s.released.Equals(cDate) && s.filestatus.Equals("Updated"));
            ViewBag.khbDLU = db.poes.Count(s => s.region.Equals("KHB") && s.released.Equals(cDate) && s.filestatus.Equals("Updated"));

            return View();
        }

        public ActionResult MonthlyPO(string mnth)
        {
            int monthInDigit = DateTime.ParseExact(mnth, "MMM", CultureInfo.InvariantCulture).Month;

            DateTime date = DateTime.Now;

            var firstDayOfMonth = new DateTime(date.Year, monthInDigit, 1);
            //var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var fdate = firstDayOfMonth.ToString("yyyy-MM");
            //var ldate= lastDayOfMonth.ToString("yyyy-MM");

            ViewBag.CMonth = fdate;

            //var gg = db.poes.Count(s => s.region.Equals("JED") && Convert.ToDateTime(s.released) >= firstDayOfMonth && Convert.ToDateTime(s.released) <= lastDayOfMonth);
            var gg = db.poes.Count(s => s.region.Equals("JED") && s.released.Contains(fdate));
            var ggg = db.ar_po.Count(s => s.region.Equals("JED") && s.released.Contains(fdate));
            ViewBag.jedOA = gg + ggg;

            var gg1 = db.poes.Count(s => s.region.Equals("RYD") && s.released.Contains(fdate));
            var gg2 = db.ar_po.Count(s => s.region.Equals("RYD") && s.released.Contains(fdate));
            ViewBag.rydOA = gg1 + gg2;

            var gg3 = db.poes.Count(s => s.region.Equals("KHB") && s.released.Contains(fdate));
            var gg4 = db.ar_po.Count(s => s.region.Equals("KHB") && s.released.Contains(fdate));
            ViewBag.khbOA = gg3 + gg4;

            ViewBag.OATotal = gg + ggg + gg1 + gg2 + gg3 + gg4;

            var j1 = db.poes.Count(s => s.region.Equals("JED") && s.released.Contains(fdate) && s.filestatus.Equals("Downloaded"));
            var j2 = db.ar_po.Count(s => s.region.Equals("JED") && s.released.Contains(fdate) && s.filestatus.Equals("Downloaded"));
            ViewBag.jedDL = j1 + j2;

            var r1 = db.poes.Count(s => s.region.Equals("RYD") && s.released.Contains(fdate) && s.filestatus.Equals("Downloaded"));
            var r2 = db.ar_po.Count(s => s.region.Equals("RYD") && s.released.Contains(fdate) && s.filestatus.Equals("Downloaded"));
            ViewBag.rydDL = r1 + r2;

            var k1 = db.poes.Count(s => s.region.Equals("KHB") && s.released.Contains(fdate) && s.filestatus.Equals("Downloaded"));
            var k2 = db.ar_po.Count(s => s.region.Equals("KHB") && s.released.Contains(fdate) && s.filestatus.Equals("Downloaded"));
            ViewBag.khbDL = k1 + k2;

            var jj1 = db.poes.Count(s => s.region.Equals("JED") && s.released.Contains(fdate) && s.filestatus.Equals("Available"));
            var jj2 = db.ar_po.Count(s => s.region.Equals("JED") && s.released.Contains(fdate) && s.filestatus.Equals("Available"));
            ViewBag.jedAV = jj1 + jj2;

            var rr1 = db.poes.Count(s => s.region.Equals("RYD") && s.released.Contains(fdate) && s.filestatus.Equals("Available"));
            var rr2 = db.ar_po.Count(s => s.region.Equals("RYD") && s.released.Contains(fdate) && s.filestatus.Equals("Available"));
            ViewBag.rydAV = rr1 + rr2;

            var kk1 = db.poes.Count(s => s.region.Equals("KHB") && s.released.Contains(fdate) && s.filestatus.Equals("Available"));
            var kk2 = db.ar_po.Count(s => s.region.Equals("KHB") && s.released.Contains(fdate) && s.filestatus.Equals("Available"));
            ViewBag.khbAV = kk1 + kk2;

            return View();
        }

        #endregion

        #region Agent Reporting
        
        private string getRegion()
        {
            var lRegg = User.Identity.Name.Length - 3;

            return User.Identity.Name.Substring(lRegg, 3);
        }
        
        public ActionResult SupplierReport()
        {
            return View();
        }

        public ActionResult SupplierPO(string sid, string sdate)
        {
            var curReg = getRegion();

            var cDate = Convert.ToDateTime(sdate).ToString("yyyy-MM-dd");

            ViewBag.CDate = cDate;
            ViewBag.Region = curReg;
            ViewBag.Sid = sid;

            ViewBag.OA = db.poes.Count(s => s.sid.Equals(sid) && s.region.Equals(curReg) && s.released.Equals(cDate));
                        
            ViewBag.AV = db.poes.Count(s => s.sid.Equals(sid) && s.region.Equals(curReg) && s.released.Equals(cDate) && s.filestatus.Equals("Available"));
            ViewBag.DL = db.poes.Count(s => s.sid.Equals(sid) && s.region.Equals(curReg) && s.released.Equals(cDate) && s.filestatus.Equals("Downloaded"));
            
            return View();
        }

        public ActionResult SupplierPORangeDate(string sid, string ixDate)
        {
            var curReg = getRegion();
            ViewBag.Region = curReg;
            ViewBag.Sid = sid;

            var _sDate = ixDate.Split('-');

            var aDate = Convert.ToDateTime(_sDate[0]);
            var bDate = Convert.ToDateTime(_sDate[1]);

            ViewBag.CDateS = aDate;
            ViewBag.CDateE = bDate;

            var po = new List<rpo1>();

            for(DateTime date = aDate; date <= bDate; date = date.AddDays(1))
            {
                var nNew = date.ToString("yyyy-MM-dd");
                po.Add(new rpo1()
                {
                    CDate = nNew,
                    Overall = db.poes.Count(s => s.sid.Equals(sid) && s.region.Equals(curReg) && s.released.Equals(nNew)).ToString(),
                    POAvailable = db.poes.Count(s => s.sid.Equals(sid) && s.region.Equals(curReg) && s.released.Equals(nNew) && s.filestatus.Equals("Available")).ToString(),
                    PODownloaded = db.poes.Count(s => s.sid.Equals(sid) && s.region.Equals(curReg) && s.released.Equals(nNew) && s.filestatus.Equals("Downloaded")).ToString()
                });
            }

            ViewBag.RequestQQ = po;

            return View();

        }
        #endregion
    }
}

public partial class rpo1
{
    public string CDate { get; set; }

    public string Overall { get; set; }

    public string POAvailable { get; set; }

    public string PODownloaded { get; set; }
}