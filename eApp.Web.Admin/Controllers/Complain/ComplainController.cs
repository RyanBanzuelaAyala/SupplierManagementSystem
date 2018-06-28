using eApp.Web.Admin.ADO;
using eApp.Web.Admin.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eApp.Web.Admin.Controllers.Complain
{
    public class ComplainController : Controller
    {
        private dnbmssqlEntities db = new dnbmssqlEntities();
        UserManager<User> userManager = new UserManager<User>(new UserStore<User>(new ApplicationDbContext()));

        private string getRegion()
        {
            var lRegg = User.Identity.Name.Length - 3;

            return User.Identity.Name.Substring(lRegg, 3);
        }

        public ActionResult ComplainList()
        {
            var curReg = getRegion();

            ViewBag.RequestQQ = db.syssupcomps.Where(s => s.status.Equals("new") && s.userid.Contains(curReg) || s.status.Equals("replied") && s.userid.Contains(curReg)).ToList();

            return View();
        }
        
        public ActionResult ComplainInfo(string issueid)
        {
            ViewBag.Issueid = issueid;

            ViewBag.IssueStatus = db.syssupcomps.FirstOrDefault(s => s.issueid.Equals(issueid)).status;

            return View();
        }

        [HttpGet]
        public JsonResult ComplainInfoReplies(string issueid)
        {
            var objList = db.syssupcomprplies.Where(s => s.issueid.Equals(issueid)).ToList();

            return Json(objList, JsonRequestBehavior.AllowGet);
        }

        public bool ComplainInfoReply(string issueid, string respnse)
        {
            try
            {
                var currentUser = User.Identity.Name;

                db.syssupcomprplies.Add(new syssupcomprply
                {
                    agentid = currentUser,
                    userid = "",
                    issueid = issueid,
                    reply = respnse,
                    remarks = DateTime.Now.ToString("MM-dd-yyyy h:mmtt"),
                    status = "checking"
                });
                
                db.syssupcomps.FirstOrDefault(s => s.issueid.Equals(issueid)).status = "replied";

                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ComplainSolved(string issueid)
        {
            try
            {
                db.syssupcomps.FirstOrDefault(s => s.issueid.Equals(issueid)).status = "solved";

                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
        
        public ActionResult ComplainListofSolved()
        {
            var curReg = getRegion();

            ViewBag.RequestQQ = db.syssupcomps.Where(s => s.status.Equals("solved") && s.userid.Contains(curReg)).ToList();

            return View();
        }

        public ActionResult ComplainInfoSolved(string issueid)
        {
            ViewBag.RequestQQ = db.syssupcomprplies.Where(s => s.issueid.Equals(issueid)).ToList();

            return View();
        }
    }
}