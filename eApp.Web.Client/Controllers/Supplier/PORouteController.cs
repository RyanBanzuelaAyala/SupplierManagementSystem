using eApp.Web.Client.ADO;
using eApp.Web.Client.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eApp.Web.Client.Controllers.Supplier
{
    [Authorize(Roles = "Supplier")]
    public class PORouteController : Controller
    {
        #region VIEW
        
        public ActionResult PO()
        {
            dnbmssqlEntities db = new dnbmssqlEntities();

            var reg = User.Identity.GetUserName().Substring(6, 3);
            var id = User.Identity.GetUserName().Substring(0, 6);

            var getall = db.poes.Where(s => s.sid.Equals(id)
                                                && s.region.Equals(reg));

            ViewBag.POA = getall.Where(s => s.filestatus.Equals("Available")).Count();
            ViewBag.POD = getall.Where(s => s.filestatus.Equals("Downloaded")).Count();

            var getallar = db.ar_po.Where(s => s.sid.Equals(id)
                                                && s.region.Equals(reg));

            ViewBag.POE = getallar.Count();

            return View();
            
        }
        
        #endregion

        #region POST

        [HttpPost]
        public bool SubmitPO(string pono)
        {
            dnbmssqlEntities db = new dnbmssqlEntities();
            UserManager<User> userManager = new UserManager<User>(new UserStore<User>(new ApplicationDbContext()));

            var currentUser = User.Identity.Name.Substring(0, 6);
            
            var objPO = db.poes.Where(e => e.sid.Equals(currentUser) && e.pono.Contains(pono)).FirstOrDefault();

            objPO.filestatus = "Downloaded";

            db.SaveChanges();

            sysLog("Download PO : " + pono, "Purchasing Order");

            return true;
        }

        #endregion
               
        #region FUNCTION

        private void sysLog(string msg, string action)
        {
            dnbmssqlEntities db = new dnbmssqlEntities();
            
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

        #endregion
        
        #region JSON


        [HttpGet]
        public JsonResult GetAllPO(string status)
        {
            dnbmssqlEntities db = new dnbmssqlEntities();
            UserManager<User> userManager = new UserManager<User>(new UserStore<User>(new ApplicationDbContext()));

            var currentUser = User.Identity.Name.Substring(0, 6);
            var currentRegion = User.Identity.Name.Substring(6, 3);

            if(status == "Available")
            {
                var QQList = db.poes.Where(s => s.sid.Equals(currentUser) && s.region.Equals(currentRegion))
                                    .Where(s => s.filestatus.Equals("Available") || s.filestatus.Equals("Updated"))
                                    .OrderBy(s => s.released);

                return Json(QQList, JsonRequestBehavior.AllowGet);
            }
            else if(status == "Downloaded")
            {
                var QQList = db.poes.Where(s => s.sid.Equals(currentUser) && s.region.Equals(currentRegion) && s.filestatus.Equals("Downloaded"))
                                   .OrderBy(s => s.released);

                return Json(QQList, JsonRequestBehavior.AllowGet);
            }
            else if (status == "Expired")
            {
                var QQList = db.ar_po.Where(s => s.sid.Equals(currentUser) && s.region.Equals(currentRegion));

                return Json(QQList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion

    }
}