using eApp.Web.Admin.ADO;
using eApp.Web.Admin.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace eApp.Web.Admin.Controllers.Stakeholder
{
    [Authorize(Roles = "Administrator")]
    public class AgentController : Controller
    {
        dnbmssqlEntities db = new dnbmssqlEntities();

        #region Account Setup

        private ApplicationUserManager _userManager;

        UserManager<User> userManager = new UserManager<User>(new UserStore<User>(new ApplicationDbContext()));

        private User FindUser()
        {
            return userManager.FindById(User.Identity.GetUserId());
        }

        public AgentController() { }

        public AgentController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        #endregion

        #region Create New Agent
                        
        public ActionResult NewAgent()
        {
            return View();
        }

        [HttpPost]        
        public async Task<bool> AgentSubmit(rPerson model)
        {
            var uUser = model.name + model.region;

            var isExist = db.userapps.FirstOrDefault(s => s.userid.Equals(uUser));

            if(isExist != null)
            {
                return false;
            }

            var newAID = model.name + model.region;

            var user = new User { UserName = newAID, Email = newAID + "@danubeco.com", Region = model.region };

            var result = await UserManager.CreateAsync(user, model.password);

            if(!result.Succeeded)
            {
                return false;
            }

            var currentUser = await UserManager.FindByEmailAsync(newAID + "@danubeco.com");

            await UserManager.AddToRolesAsync(currentUser.Id, model.role);
            
            db.userapps.Add(new userapp()
            {
              userid = newAID,
              name = model.name,
              role = model.role,
              status = "activated",
              login = DateTime.Today,
              password = model.password

            });

            db.SaveChanges();
            
            return true;
        }

        #endregion

        #region List of Agent
        
        public ActionResult AgentList()
        {
            ViewBag.RequestQQ = db.userapps.ToList();

            return View();
        }

        public ActionResult AgentStatus(string stats, string userid)
        {
            var isExist = db.userapps.FirstOrDefault(s => s.userid.Equals(userid));

            if(isExist == null)
            {
                ViewBag.isError = true;

                ViewBag.Result = "Agent not existing..";
            }
            else
            {
                isExist.status = stats;

                db.SaveChanges();

                ViewBag.isError = false;

                ViewBag.Result = "Successfully Activated...";
            }
            
            return View();
        }

        #endregion
    }
}

public partial class rPerson
{
    public string name { get; set; }
    public string password { get; set; }
    public string region { get; set; }
    public string role { get; set; }
}
