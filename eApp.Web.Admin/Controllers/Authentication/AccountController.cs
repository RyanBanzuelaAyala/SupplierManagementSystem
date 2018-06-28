using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using eApp.Web.Admin.Models;
using eApp.Web.Admin.ADO;
using System.Collections.Generic;

namespace eApp.Web.Admin.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        dnbmssqlEntities db = new dnbmssqlEntities();

        #region Basic Setup 

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

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

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        #endregion

        #region Account Views

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [Authorize(Roles = "Administrator, Agent")]
        public ActionResult LogOff()
        {
            SignInManager.AuthenticationManager.SignOut();

            return View();
        }

        [Authorize(Roles = "Administrator, Agent")]
        public ActionResult SessionExpired()
        {
            SignInManager.AuthenticationManager.SignOut();

            return View();
        }

        [Authorize(Roles = "Administrator, Agent")]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize(Roles = "Administrator, Agent")]
        public ActionResult SwitchRegion()
        {   
            var iReg = User.Identity.Name.Substring(0, User.Identity.Name.Length - 3);

            var isExist = db.C_User.Where(s => s.UserName.Contains(iReg)).ToList();

            ViewBag.RequestQQ = isExist;

            ViewBag.CurrentReg = User.Identity.Name;

            return View();
        }

        public async Task<bool> SwitchRegionProcess(userX userX)
        {
            SignInManager.AuthenticationManager.SignOut();

            var result = await SignInManager.PasswordSignInAsync(userX.username, userX.password, false, false);

            switch (result)
            {
                case SignInStatus.Success:
                    var user = await UserManager.FindByNameAsync(userX.username);

                    var getL = user.UserName.Length - 3;

                    if (user.UserName.Substring(getL, 3) == userX.region)
                    {
                        var db = new dnbmssqlEntities();

                        var getSTatus = db.userapps.FirstOrDefault(s => s.userid.Equals(userX.username)).status;

                        if (getSTatus.Equals("deactivated"))
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid region.");
                        return false;
                    }

                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return false;
            }
            
        }

        #endregion

        #region Login

        [HttpPost]
        [AllowAnonymous]
        public async Task<bool> Login(nLoginViewModel model)
        {
            try
            {

                var result = await SignInManager.PasswordSignInAsync(model.Username, model.Password, false, false);

                switch(result)
                {
                    case SignInStatus.Success:
                        var user = await UserManager.FindByNameAsync(model.Username);

                        var getL = user.UserName.Length - 3;

                        if(user.UserName.Substring(getL, 3) == model.Region)
                        {
                            var db = new dnbmssqlEntities();

                            var getSTatus = db.userapps.FirstOrDefault(s => s.userid.Equals(model.Username)).status;
                            
                            if(getSTatus.Equals("deactivated"))
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid region.");
                            return false;
                        }

                    default:
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return false;
                }


            }
            catch(Exception)
            {
                return false;
            }

        }

        [Authorize(Roles = "Administrator, Agent")]
        [HttpPost]
        public async Task<bool> ResetPassword(nResetPasswordViewModel model)
        {
            try
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

                if(user == null)
                {
                    return false;
                }

                user.PasswordHash = UserManager.PasswordHasher.HashPassword(model.nPassword);

                var result = await UserManager.UpdateAsync(user);

                if(!result.Succeeded)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch(Exception)
            {
                return false;
            }

        }
        
        #endregion

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                if(_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if(_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }


        [AllowAnonymous]
        public async Task<string> AddAgent()
        {
            
            var user = new User { UserName = "devqq", Email = "ryan" + "@danubeco.com", Region = "JED" };

            var result = await UserManager.CreateAsync(user, "qwe123QQ@@");

            if(!result.Succeeded)
            {
                return "mali";
            }

            var currentUser = await UserManager.FindByEmailAsync("ryan@danubeco.com");

            await UserManager.AddToRolesAsync(currentUser.Id, "Administrator");

            try
            {
                db.userapps.Add(new userapp()
                {
                    userid = "devqq",
                    name = "Ryan Ayala",
                    role = "Administrator",
                    status = "activated",
                    login = DateTime.Today,
                    password = "qwe123QQ@@"

                });

                db.SaveChanges();

                return "ok";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            
        }

        #endregion


    }

    public class userX
    {
        public string username { get; set; }
        public string password { get; set; }
        public string region { get; set; }
    }

}
