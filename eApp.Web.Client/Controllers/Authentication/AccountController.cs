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
using eApp.Web.Client.Models;
using System.IO;
using eApp.Web.Client.ADO;
using eApp.Web.Client.Resources.LibraryClass;

namespace eApp.Web.Client.Controllers
{
    [Authorize(Roles = "Supplier")]
    public class AccountController : Controller
    {
        dnbmssqlEntities db = new dnbmssqlEntities();

        #region Basic Setup 

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
                
        public ActionResult LogOff()
        {
            sysLog("Account LogOff", "Login");

            SignInManager.AuthenticationManager.SignOut();

            return View();
        }

        public ActionResult SessionExpired()
        {
            sysLog("Account Session Expired", "Login");

            SignInManager.AuthenticationManager.SignOut();

            return View();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }
        
        public ActionResult SwitchRegion()
        {
            var iReg = User.Identity.Name.Substring(0, User.Identity.Name.Length - 3);

            var isExist = db.C_User.Where(s => s.UserName.Contains(iReg)).ToList();

            ViewBag.RequestQQ = isExist;

            ViewBag.CurrentReg = User.Identity.Name;

            return View();
        }

        public ActionResult SwitchRegionProcess(userX userX)
        {
            if(userX.password != null)
            {
                SignInManager.AuthenticationManager.SignOut();

                var result = SignInManager.PasswordSignInAsync(userX.username, userX.password, false, false).Result;

                switch (result)
                {
                    case SignInStatus.Success:

                        var user = UserManager.FindByNameAsync(userX.username).Result;

                        if (user.UserName.Substring(6, 3) == userX.region)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid region.");
                            return RedirectToAction("Login", "Account");
                        }

                    default:
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return RedirectToAction("Login", "Account");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");

            }           

        }
        
        #endregion

        #region Login

        [HttpPost]
        [AllowAnonymous]
        public async Task<bool> Login(LoginViewModel model)
        {
            try
            {

                var result = await SignInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

                switch (result)
                {
                    case SignInStatus.Success:
                        var user = await UserManager.FindByNameAsync(model.Username);

                        if (user.UserName.Substring(6, 3) == model.Region)
                        {
                            return true;
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
            catch (Exception)
            {
                return false;
            }

        }
        
        [HttpPost]
        public async Task<bool> ResetPassword(ResetPasswordViewModel model)
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
                    sysLog("Account Password Changed", "Login");

                    return true;
                }

            }
            catch(Exception)
            {
                return false;
            }

        }

        [HttpPost]
        public bool ResetMobile(string mobile)
        {
            var reg = User.Identity.GetUserName().Substring(6, 3);
            var id = User.Identity.GetUserName().Substring(0, 6);

            var newMob = db.supplierregions.FirstOrDefault(s => s.sid.Equals(id) && s.region.Equals(reg));

            newMob.mobile = mobile;

            db.SaveChanges();

            sysLog("Account Mobile Changed", "Login");

            return true;
        }
        
        #endregion

        #region Mobile Activation
        
        public ActionResult Register()
        {
            var reg = User.Identity.GetUserName().Substring(6, 3);
            var id = User.Identity.GetUserName().Substring(0, 6);

            var isvalid = db.suppliermobileactivtions.FirstOrDefault(s => s.sid.Equals(id) && s.region.Equals(reg));

            if (isvalid != null)
            {
                if (isvalid.status == "validated")
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }

            #region MyRegion


            //var reg = User.Identity.GetUserName().Substring(6, 3);
            //var id = User.Identity.GetUserName().Substring(0, 6);

            //var isvalid = db.suppliermobileactivtions.FirstOrDefault(s => s.sid.Equals(id) && s.region.Equals(reg));

            //if (isvalid != null)
            //{
            //    if (isvalid.status == "validated")
            //    {
            //        var a = DateTime.ParseExact(isvalid.datetimesent, "dd-MM-yyyy hh:mm:ss", null);

            //        // get current hours if we need to send SMS should be 6hrs
            //        var days = Math.Floor(System.Math.Abs(DateTime.Now.Subtract(a).TotalDays));

            //        if (days == 4)
            //        {
            //            var sms = new xSms().GetCode(6);

            //            isvalid.codesms = sms;
            //            isvalid.status = "pending";
            //            isvalid.datetimesent = string.Format("{0:dd-MM-yyyy hh:mm:ss}", DateTime.Now);

            //            db.SaveChanges();

            //            //new xSms().SendSMS("Requested an activation code " + sms + " for mobile registration.", isvalid.mobile);

            //            return RedirectToAction("CodeValidate", "Account");
            //        }
            //        else
            //        {
            //            return RedirectToAction("Index", "Home");
            //        }
            //    }
            //    else
            //    {
            //        return RedirectToAction("CodeValidate", "Account");
            //    }
            //}
            //else
            //{
            //    return View();
            //}

            #endregion

        }
        
        [HttpPost]
        public bool Register(string mobile)
        {
            var reg = User.Identity.GetUserName().Substring(6, 3);
            var id = User.Identity.GetUserName().Substring(0, 6);

            var newMob = db.suppliermobileactivtions.FirstOrDefault(s => s.sid.Equals(id) && s.region.Equals(reg));
            var newsuppmob = db.supplierregions.FirstOrDefault(s => s.sid.Equals(id) && s.region.Equals(reg));

            //var sms = new xSms().GetCode(6);

            if (newMob == null)
            {
                db.suppliermobileactivtions.Add(new suppliermobileactivtion
                {
                    sid = id,
                    region = reg,
                    password = "",
                    mobile = mobile,
                    emailadd = "",
                    codesms = "123456",
                    status = "validated",
                    datetimesent = string.Format("{0:dd-MM-yyyy hh:mm:ss}", DateTime.Now)
                });

            }
            else
            {
                newMob.mobile = mobile;
                newMob.codesms = "123456";
                newMob.status = "validated";
                newMob.datetimesent = string.Format("{0:dd-MM-yyyy hh:mm:ss}", DateTime.Now);
            }

            newsuppmob.mobile = mobile;

            db.SaveChanges();

            //new xSms().SendSMS("Requested an activation code " + sms + " for supplier management portal.", mobile);

            return true;
        }
        
        public ActionResult CodeValidate()
        {
            var reg = User.Identity.GetUserName().Substring(6, 3);
            var id = User.Identity.GetUserName().Substring(0, 6);

            ViewBag.Mobile = db.suppliermobileactivtions.FirstOrDefault(s => s.sid.Equals(id) && s.region.Equals(reg)).mobile;

            return View();
        }
        
        public bool ResendCode()
        {
            var reg = User.Identity.GetUserName().Substring(6, 3);
            var id = User.Identity.GetUserName().Substring(0, 6);

            var newMob = db.suppliermobileactivtions.FirstOrDefault(s => s.sid.Equals(id) && s.region.Equals(reg));

            if (newMob != null)
            {

                var sms = new xSms().GetCode(6);

                newMob.codesms = sms;
                newMob.status = "pending";
                newMob.datetimesent = string.Format("{0:dd-MM-yyyy hh:mm:ss}", DateTime.Now);

                db.SaveChanges();

                //new xSms().SendSMS("Requested an activation code " + sms + " for mobile registration.", newMob.mobile);

                return true;

            }
            else
            {
                return false;
            }

        }
        
        [HttpPost]
        public bool CodeValidate(string code)
        {
            var reg = User.Identity.GetUserName().Substring(6, 3);
            var id = User.Identity.GetUserName().Substring(0, 6);

            var newMob = db.suppliermobileactivtions.FirstOrDefault(s => s.sid.Equals(id) && s.region.Equals(reg));

            if (newMob != null)
            {
                if (newMob.codesms == code)
                {
                    newMob.status = "validated";
                    newMob.datetimesent = string.Format("{0:dd-MM-yyyy hh:mm:ss}", DateTime.Now.AddDays(1));

                    var suppliernew = db.supplierregions.FirstOrDefault(s => s.sid.Equals(id) && s.region.Equals(reg));

                    if (suppliernew != null)
                    {
                        suppliernew.mobile = newMob.mobile;
                    }

                    db.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }

        }
        
        public ActionResult ResetMobileActivation()
        {
            var reg = User.Identity.GetUserName().Substring(6, 3);
            var id = User.Identity.GetUserName().Substring(0, 6);

            var ireset = db.suppliermobileactivtions.FirstOrDefault(s => s.sid.Equals(id) && s.region.Equals(reg));

            if(ireset != null)
            {
                db.suppliermobileactivtions.Remove(ireset);

                db.SaveChanges();

                return RedirectToAction("Register", "Account");
            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }            
        }


        #endregion

        #region Account Logoff/Session Expiration

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


        //[AllowAnonymous]
        //public async Task<string> RegisterAll()
        //{

        //    var user = new User { UserName = "330018JED", Email = "330018JED@danubeco.com", Region = "JED" };

        //    var result = await UserManager.CreateAsync(user, "330018");

        //    if(result.Succeeded)
        //    {
        //        var currentUser = await UserManager.FindByEmailAsync("330018JED@danubeco.com");

        //        await UserManager.AddToRolesAsync(currentUser.Id, "Supplier");
                                
        //        db.supplierregions.Add(new supplierregion
        //        {
        //            sid = "330018",
        //            region = "JED",
        //            password = "330018",
        //            mobile = "0564433271",
        //            sms = "y",
        //            status = "A"
        //        });

        //        db.suppliers.Add(new supplier
        //        {
        //            sid = "330018",
        //            name = "THE COCA COLA BOTTLING CO OF SA LTD"
        //        });

        //        db.SaveChanges();

        //        return "ok";
        //    }
        //    else
        //    {
        //        return "nah!";

        //    }

        //}

        //[AllowAnonymous]
        //public async Task<string> qweqwe()
        //{
        //    string csvHandlerLine = "";

        //    string[] csvHandler;

        //    using(StreamReader CsvReader = new StreamReader(Server.MapPath("~/CSV/dnbSup.csv")))
        //    {
        //        string inputLine = "";

        //        while((inputLine = CsvReader.ReadLine()) != null)
        //        {
        //            csvHandlerLine = inputLine.Trim().Replace("'", "");

        //            char[] delimiterChars = { ',', '\"' };

        //            csvHandler = inputLine.Split(delimiterChars);

        //            var c1 = csvHandler[11].Trim().ToString(); // UserName
        //            var c2 = csvHandler[12].Trim().ToString(); // Region
        //            var c3 = csvHandler[13].Trim().ToString(); // Name
        //            var c4 = csvHandler[14].Trim().ToString(); // Mobile
        //            var c5 = csvHandler[15].Trim().ToString(); // Password
        //            var c6 = csvHandler[16].Trim().ToString(); // IsSms

        //            var eEmail = c1 + "@danube.com";

        //            var user = new User { UserName = c1, Email = eEmail, Region = c2 };

        //            if(!db.C_User.Any(e => e.UserName.Equals(c1)))
        //            {
        //                var result = await UserManager.CreateAsync(user, c5);

        //                if(result.Succeeded)
        //                {
        //                    var currentUser = await UserManager.FindByEmailAsync(eEmail);

        //                    await UserManager.AddToRolesAsync(currentUser.Id, "Supplier");

        //                    var hasSMS = (c6.Equals("yes")) ? "y" : "n";

        //                    db.supplierregions.Add(new supplierregion
        //                    {
        //                        sid = c1.Substring(0, 6),
        //                        region = c2,
        //                        password = c5,
        //                        mobile = c4,
        //                        sms = hasSMS,
        //                        status = "A"
        //                    });

        //                    db.suppliers.Add(new supplier
        //                    {
        //                        sid = c1.Substring(0, 6),
        //                        name = c3
        //                    });

        //                    db.SaveChanges();
        //                }
        //            }
        //        }

        //        CsvReader.Close();
        //    }

        //    return "nah2";

        //}

        #endregion
    }


    public class userX
    {
        public string username { get; set; }
        public string password { get; set; }
        public string region { get; set; }
    }

}