using eApp.Web.Admin.ADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eApp.Web.Admin.Controllers.Transaction
{
    public class RankController : Controller
    {
        private dnbmssqlEntities db = new dnbmssqlEntities();

        private string getRegion()
        {
            var lRegg = User.Identity.Name.Length - 3;

            return User.Identity.Name.Substring(lRegg, 3);
        }


        // GET: Rank
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RankList(string userid, string stats)
        {
            var gUser = userid + getRegion();

            if(stats.Equals("expired"))
            {
                var getAllrank = db.ar_reqlist.Where(s => s.sid.Equals(gUser)).ToList();
                ViewBag.Ranks = getAllrank;
            }
            else
            {
                var getAllrank = db.reqlists.Where(s => s.sid.Equals(gUser) && s.sts.Equals(stats)).ToList();
                ViewBag.Ranks = getAllrank;
            }
            
            ViewBag.Info = gUser;

            return View();
        }

        public ActionResult ProcessingList()
        {
            var curReg = getRegion();

            var getAllrank = db.reqlists.Where(s => s.sts.Equals("processing") && s.sid.Contains(curReg)).ToList();

            ViewBag.Ranks = getAllrank;

            return View();
        }

        public bool DeleteRanking(string reqid)
        {
            try
            {
                var rnk = db.reqlists.FirstOrDefault(s => s.reqid.Equals(reqid));

                db.reqlists.Remove(rnk);

                db.SaveChanges();

                return true;
            }
            catch(Exception)
            {
                return false;
            }

        }

        public bool UpdateRanking(string reqid)
        {
            try
            {
                var obj = db.reqlists.Where(s => s.reqid.Equals(reqid)).FirstOrDefault();

                obj.sts = "process";

                db.SaveChanges();

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}