using eApp.Web.Client.ADO;
using eApp.Web.Client.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eApp.Web.Client.Controllers
{
    [Authorize(Roles = "Supplier")]
    public class HomeController : Controller
    {
        #region Setup

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

        private User FindUser()
        {
            return userManager.FindById(User.Identity.GetUserId());
        }

        #endregion

        #region VIEW

        private ApplicationSignInManager _signInManager;
        
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ActionResult Index()
        {
            //sysLog("Account Login", "Login");

            var reg = User.Identity.GetUserName().Substring(6, 3);
            var id = User.Identity.GetUserName().Substring(0, 6);

            var newMob = db.suppliermobileactivtions.FirstOrDefault(s => s.sid.Equals(id) && s.region.Equals(reg));

            if (newMob == null)
            {
                SignInManager.AuthenticationManager.SignOut();

                return Redirect("Account/Login");
            }
            else
            {
                if(newMob.status == "pending")
                {
                    SignInManager.AuthenticationManager.SignOut();

                    return Redirect("Account/Login");
                }
            }
                
            
            var user = FindUser();

            ViewBag.CurrentUser = user;
            
            var getall = db.poes.Where(s => s.sid.Equals(id)
                                                && s.region.Equals(reg));

            ViewBag.POA = getall.Where(s => s.filestatus.Equals("Available")).Count();
            ViewBag.POD = getall.Where(s => s.filestatus.Equals("Downloaded")).Count();

            var getallar = db.ar_po.Where(s => s.sid.Equals(id)
                                                && s.region.Equals(reg));

            ViewBag.POE = getallar.Count();

            var getrk = db.reqlists.Where(s => s.sid.Equals(user.UserName));

            ViewBag.RKA = getrk.Where(s => s.sts.Equals("processed")).Count();
            ViewBag.RKD = getrk.Where(s => s.sts.Equals("downloaded")).Count();

            ViewBag.RKR = db.ar_reqlist.Count(s => s.sid.Equals(user.UserName));

            var getrv = db.rtvs.Where(s => s.sid.Equals(id)
                                                && s.region.Equals(reg));

            ViewBag.RVA = getrv.Where(s => s.filestatus.Equals("Available")).Count();
            ViewBag.RVD = getrv.Where(s => s.filestatus.Equals("Downloaded")).Count();

            ViewBag.CMPL = db.syssupcomps.Where(s => s.status.Equals("replied") && s.userid.Equals(user.UserName) || s.status.Equals("new") && s.userid.Equals(user.UserName)).Count();

            if (user == null)
                return Redirect("Account/Login");

            if (user.Roles.FirstOrDefault().RoleId != "1")
            {
                return Redirect("Account/LogOff");
            }
            else
            {
                return View(ViewBag);
            }
        }
        
        public ActionResult Startup()
        {
            var user = FindUser();

            ViewBag.PO = db.poes.Count(s => s.sid.Equals(user.UserName.Substring(0, 6)) && s.region.Equals(user.Region)) + db.ar_po.Count(s => s.sid.Equals(user.UserName.Substring(0, 6)) && s.region.Equals(user.Region));

            ViewBag.Rank = db.reqlists.Count(s => s.sid.Equals(user.UserName)) + db.ar_reqlist.Count(s => s.sid.Equals(user.UserName));

            var SupplierMobile = db.supplierregions.FirstOrDefault(s => s.sid.Equals(user.UserName.Substring(0, 6)) && s.region.Equals(user.Region)).mobile;

            var SupplierName = db.suppliers.FirstOrDefault(s => s.sid.Equals(user.UserName.Substring(0, 6))).name;

            ViewBag.SupplierName = SupplierName;

            ViewBag.SupplierMobile = SupplierMobile;

            ViewBag.User = user;
            
            return View();
        }

        public ActionResult GetEvents()
        {
            var user = FindUser();

            List<Events> eventList = new List<Events>();
            
            var getAllPOA = db.poes.Where(s => s.sid.Equals(user.UserName.Substring(0, 6)) && s.region.Equals(user.Region) && s.filestatus.Equals("available")).ToList();

            if(getAllPOA != null)
            {
                foreach (var item in getAllPOA)
                {
                    var newEvent = new Events
                    {
                        id = item.Id.ToString(),
                        title = "PO : " + item.pono,
                        desc = "PAvailable",
                        start = item.released,
                        end = item.released,
                        color = "green",
                        allDay = false,
                        link = item.link,
                        status = item.filestatus
                    };

                    eventList.Add(newEvent);

                }
            }
            

            var getAllPOD = db.poes.Where(s => s.sid.Equals(user.UserName.Substring(0, 6)) && s.region.Equals(user.Region) && s.filestatus.Equals("downloaded")).ToList();

            if (getAllPOD != null)
            {

                foreach (var item in getAllPOD)
                {
                    var newEvent = new Events
                    {
                        id = item.Id.ToString(),
                        title = "PO : " + item.pono,
                        desc = "PDownloaded",
                        start = item.released,
                        end = item.released,
                        color = "red",
                        allDay = false,
                        link = item.link,
                        status = item.filestatus
                    };

                    eventList.Add(newEvent);

                }

            }

            var getAllPOE = db.ar_po.Where(s => s.sid.Equals(user.UserName.Substring(0, 6)) && s.region.Equals(user.Region)).ToList();

            if (getAllPOE != null)
            {

                foreach (var item in getAllPOE)
                {
                    var newEvent = new Events
                    {
                        id = item.Id.ToString(),
                        title = "PO : " + item.pono,
                        desc = "PArchived",
                        start = item.released,
                        end = item.released,
                        color = "gray",
                        allDay = false,
                        link = item.link,
                        status = "expired"
                    };

                    eventList.Add(newEvent);

                }

            }

            var getAllrank = db.reqlists.Where(s => s.sid.Equals(user.UserName) && s.sts.Equals("processed")).ToList();

            if (getAllrank != null)
            {

                foreach (var item in getAllrank)
                {
                    var newEvent = new Events
                    {
                        id = item.Id.ToString(),
                        title = "RANK :" + item.reqid,
                        desc = "Processed",
                        start = item.dreq,
                        end = item.dreq,
                        color = "orange",
                        allDay = false,
                        link = item.lnk,
                        status = item.sts
                    };

                    eventList.Add(newEvent);

                }

            }


            var getAllrankD = db.reqlists.Where(s => s.sid.Equals(user.UserName) && s.sts.Equals("downloaded")).ToList();

            if (getAllrankD != null)
            {

                foreach (var item in getAllrank)
                {
                    var newEvent = new Events
                    {
                        id = item.Id.ToString(),
                        title = "RANK :" + item.reqid,
                        desc = "Downloaded",
                        start = item.dreq,
                        end = item.dreq,
                        color = "blue",
                        allDay = false,
                        link = item.lnk,
                        status = item.sts
                    };

                    eventList.Add(newEvent);

                }

            }

            var getAllrankar = db.ar_reqlist.Where(s => s.sid.Equals(user.UserName)).ToList();

            if (getAllrankar != null)
            {
                foreach (var item in getAllrankar)
                {
                    var newEvent = new Events
                    {
                        id = item.Id.ToString(),
                        title = "RANK :" + item.reqid,
                        desc = "Archived",
                        start = item.dreq,
                        end = item.dreq,
                        color = "pink",
                        allDay = false,
                        link = item.lnk,
                        status = "expired"
                    };

                    eventList.Add(newEvent);

                }
            }

            

            return Json(eventList.ToArray(), JsonRequestBehavior.AllowGet);
        }
                
        public JsonResult GetStatistic(string cmonth)
        {
            var user = FindUser();

            if(string.IsNullOrEmpty(cmonth))
            {
                cmonth = DateTime.Now.ToString("yyyy-MM");
            }
            
            var getall = db.poes.Where(s => s.sid.Equals(user.UserName.Substring(0, 6))
                                                && s.region.Equals(user.UserName.Substring(6, 3)) && s.released.Contains(cmonth));
            
            var getallar = db.ar_po.Where(s => s.sid.Equals(user.UserName.Substring(0, 6))
                                                && s.region.Equals(user.UserName.Substring(6, 3)) && s.released.Contains(cmonth));
            
            var getrk = db.reqlists.Where(s => s.sid.Equals(user.UserName) && s.dreq.Contains(cmonth));
            
            var obj = new Statistic
            {
                POA = getall.Where(s => s.filestatus.Equals("Available")).Count(),
                POD = getall.Where(s => s.filestatus.Equals("Downloaded")).Count(),
                POE = getallar.Count(),
                RKA = getrk.Count(s => s.sts.Equals("processed") && s.dreq.Contains(cmonth)),
                RKD = getrk.Count(s => s.sts.Equals("downloaded") && s.dreq.Contains(cmonth)),
                RKR = db.ar_reqlist.Count(s => s.sid.Equals(user.UserName) && s.dreq.Contains(cmonth))
            };

            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        
        #endregion

    }
}


public class Events
{
    public string id { get; set; }
    public string title { get; set; }
    public string desc { get; set; }
    public string start { get; set; }
    public string end { get; set; }
    public string color { get; set; }
    public bool allDay { get; set; }
    public string link { get; set; }
    public string status { get; set; }
}

public class Statistic
{
    public int POA { get; set; }
    public int POD { get; set; }
    public int POE { get; set; }
    public int RKA { get; set; }
    public int RKD { get; set; }
    public int RKR { get; set; }
    
}