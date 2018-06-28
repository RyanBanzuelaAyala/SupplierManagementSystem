using eApp.Web.Client.ADO;
using eApp.Web.Client.Models;
using eApp.Web.Client.Resources.LibraryClass;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eApp.Web.Client.Controllers.Supplier
{
    [Authorize(Roles = "Supplier")]
    public class ComplainController : Controller
    {
        dnbmssqlEntities db = new dnbmssqlEntities();
        UserManager<User> userManager = new UserManager<User>(new UserStore<User>(new ApplicationDbContext()));

        private void sysLog(string msg, string action)
        {
            var newObj = new syssuplog
            {
                userid = User.Identity.GetUserId(),
                sysaction = msg,
                sysdate = DateTime.Now.ToString(),
                status = action
            };

            db.syssuplogs.Add(newObj);

            db.SaveChanges();
        }

        #region VIEW

        public ActionResult Issue()
        {
            return View();
        }

        public ActionResult IssueList()
        {
            var currentUser = User.Identity.Name;

            var getall = db.syssupcomps.Where(s => s.userid.Equals(currentUser))
                                    .OrderBy(s => s.issuedate);

            ViewBag.RequestQQ = getall;

            //sysLog("View List of Complain", "Complain");

            return View();

        }

        public ActionResult IssueListInfo(string issueid)
        {
            ViewBag.cUser = User.Identity.Name;

            ViewBag.IssueStatus = db.syssupcomps.FirstOrDefault(s => s.issueid.Equals(issueid)).status;

            ViewBag.RequestQQ = db.syssupcomprplies.Where(s => s.issueid.Equals(issueid)).ToList();

            //sysLog("View Complain Info : " + issueid, "Complain");  

            return View();
        }
        
        #endregion

        #region POST

        [HttpPost]
        public bool SubmitCompDL(CompIssue issue)
        {
            var currentUser = User.Identity.Name;
            var newId = new NewId()._GenerateId("syssupcomp");

            var objIssue = db.syssupcomps.Where(e => e.userid.Equals(currentUser) && e.issuename.Contains(issue.issuename)).FirstOrDefault();

            if (objIssue == null)
            {
                var IssueID = "TCK" + newId + DateTime.Now.ToString("yyyyMMdd");

                db.syssupcomps.Add(new syssupcomp
                {
                    userid = currentUser,
                    issueid = IssueID,
                    issuename = issue.issuename,
                    issuetype = issue.issuetype,
                    comment = issue.comment,
                    issuedate = DateTime.Now.ToString("MM-dd-yyyy h:mmtt"),
                    remarks = DateTime.Now.ToString("MM-dd-yyyy h:mmtt"),
                    status = "new"
                });

                db.syssupcomprplies.Add(new syssupcomprply
                {
                    agentid = currentUser,
                    userid = "",
                    issueid = IssueID,
                    reply = issue.comment,
                    remarks = DateTime.Now.ToString("MM-dd-yyyy h:mmtt"),
                    status = "checking"
                });

                db.SaveChanges();

                sysLog("Sent Complain", "Complain");

                return true;
            }
            else
            {

                return false;
            }
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

                db.syssupcomps.FirstOrDefault(s => s.issueid.Equals(issueid)).remarks = DateTime.Now.ToString("MM-dd-yyyy h:mmtt");

                db.SaveChanges();

                sysLog("Replied Complain Info : " + issueid, "Complain");

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region GET

        [HttpGet]
        public JsonResult ComplainInfoReplies(string issueid)
        {
            var objList = db.syssupcomprplies.Where(s => s.issueid.Equals(issueid)).ToList();

            return Json(objList, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult GetEvents()
        {
            var currentUser = User.Identity.Name;
            var eventList = new List<EventsCom>();

            var getall = db.syssupcomps.Where(s => s.userid.Equals(currentUser));

            var random = new Random();
            
            foreach (var item in getall)
            {
                var iend = "";

                var color = String.Format("#{0:X6}", random.Next(0x1000000)); // = "#A197B9"

                if (item.status != "solved")
                {
                    if (item.remarks == "new")
                    {
                        iend = Convert.ToDateTime(item.issuedate).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        iend = Convert.ToDateTime(item.remarks).ToString("yyyy-MM-dd");
                    }
                }
                else
                {
                    if(item.remarks == "new")
                    {
                        iend = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        iend = Convert.ToDateTime(item.remarks).ToString("yyyy-MM-dd");
                    }                     
                }

                var newEvent = new EventsCom
                {
                    id = item.Id.ToString(),
                    title = item.issuename,
                    desc = item.issuetype,
                    start = Convert.ToDateTime(item.issuedate).ToString("yyyy-MM-dd"),
                    end = iend,
                    color = color,
                    allDay = false,
                    status = item.status,
                    issueid = item.issueid                  
                };


                eventList.Add(newEvent);

            }
            
            return Json(eventList.ToArray(), JsonRequestBehavior.AllowGet);
        }
    
        #endregion

    }
}


public partial class CompIssue
{
    public string issuename { get; set; }
    public string issuetype { get; set; }
    public string comment { get; set; }
    
}



public class EventsCom
{
    public string id { get; set; }
    public string title { get; set; }
    public string desc { get; set; }
    public string start { get; set; }
    public string end { get; set; }
    public string color { get; set; }
    public bool allDay { get; set; }
    public string status { get; set; }
    public string issueid { get; set; }
}
