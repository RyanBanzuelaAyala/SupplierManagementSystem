using eApp.Web.Admin.ADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eApp.Web.Admin.Controllers.Reporting
{
    public class MonitorController : Controller
    {
        dnbmssqlEntities db = new dnbmssqlEntities();

        #region Supplier Who Visit [ Report ]

        private string getRegion()
        {
            var lRegg = User.Identity.Name.Length - 3;

            return User.Identity.Name.Substring(lRegg, 3);
        }

        public ActionResult SupplierLog()
        {
            return View();
        }

        public ActionResult SupplierActivities(string ixdate)
        {
            var nDate = Convert.ToDateTime(ixdate).ToString("M/d/yyyy");

            var reg = getRegion();

            ViewBag.RequestQQ = db.syssuplogs
               .Join(db.C_User, u => u.userid, uir => uir.UserId, (u, uir) => new { u, uir })
               .Where(s => s.u.sysdate.Contains(nDate) && s.uir.UserName.Contains(reg))
               .Select(m => new rXX()
               {
                   combi = m.uir.UserName,
                   sid = m.uir.UserName.Substring(0, 6),
                   region = m.uir.UserName.Substring(6, 3),
                   date = m.u.sysdate,
                   userid = m.uir.UserId

               }).ToList().GroupBy(p => p.combi).Select(grp => grp.First());

            ViewBag.cdate = ixdate;

            return View();
        }

        public ActionResult SupplierActivityInfo(string sid, string ixdate)
        {
            var nDate = Convert.ToDateTime(ixdate).ToString("M/d/yyyy");

            ViewBag.RequestQQ = db.syssuplogs.Where(s => s.userid.Equals(sid) && s.sysdate.Contains(nDate)).ToList();
            
            return View();
        }

        #endregion
    }
}


public partial class rXX
{
    public string combi { get; set; }
    public string sid { get; set; }
    public string region { get; set; }    
    public string date { get; set; }
    public string userid { get; set; }

}

