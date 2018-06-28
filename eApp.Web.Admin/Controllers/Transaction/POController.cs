using eApp.Web.Admin.ADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eApp.Web.Admin.Controllers.Transaction
{
    public class POController : Controller
    {
        private dnbmssqlEntities db = new dnbmssqlEntities();

        private string getRegion()
        {
            var lRegg = User.Identity.Name.Length - 3;

            return User.Identity.Name.Substring(lRegg, 3);
        }

        // GET: PO
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PoList(string userid, string stats)
        {
            var curReg = getRegion();

            if(stats.Equals("expired"))
            {
                var getAllpo = db.ar_po.Where(s => s.sid.Equals(userid) && s.region.Equals(curReg)).ToList();
                ViewBag.Poes = getAllpo;
            }
            else
            {
                var getAllpo = db.poes.Where(s => s.sid.Equals(userid) && s.filestatus.Equals(stats) && s.region.Equals(curReg)).ToList();
                ViewBag.Poes = getAllpo;
            }

            ViewBag.Info = userid;

            return View();
        }

    }
}
