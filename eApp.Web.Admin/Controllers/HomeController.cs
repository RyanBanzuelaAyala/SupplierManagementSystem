using eApp.Web.Admin.ADO;
using eApp.Web.Admin.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eApp.Web.Admin.Controllers
{
    [Authorize(Roles = "Administrator, Agent")]
    public class HomeController : Controller
    {
        dnbmssqlEntities db = new dnbmssqlEntities();

        UserManager<User> userManager = new UserManager<User>(new UserStore<User>(new ApplicationDbContext()));
        
        private User FindUser()
        {
            return userManager.FindById(User.Identity.GetUserId());
        }

        private string getRegion()
        {
            var lRegg = User.Identity.Name.Length - 3;

            return User.Identity.Name.Substring(lRegg, 3);
        }

        public ActionResult Index()
        {
            var curReg = getRegion();

            var user = FindUser();
            
            if(user == null)
                return Redirect("Account/Login");

            ViewBag.Role = user.Roles.FirstOrDefault().RoleId;

            ViewBag.Com = db.syssupcomps.Where(s => s.status.Equals("new") && s.userid.Contains(curReg) || s.status.Equals("replied") && s.userid.Contains(curReg)).Count();

            ViewBag.User = user;

            return View();
        }
        public ActionResult Startup()
        {
            return View();
        }

    }
}