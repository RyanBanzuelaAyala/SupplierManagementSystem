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
    [Authorize(Roles = "Administrator, Agent")]
    public class SupplierController : Controller
    {
        dnbmssqlEntities db = new dnbmssqlEntities();
        
        #region VIEW

        public ActionResult SupplierList()
        {
            return View();
        }

        public ActionResult NewSupplier()
        {
            return View();
        }

        #endregion

        #region JSON

        [HttpGet]
        public JsonResult GetSupplierCombiAll()
        {
            var db = new dnbmssqlEntities();

            var QQList = db.supplierregions.ToList();

            return Json(QQList, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region POST

        public async Task<bool> AddSupplier(rX supplier)
        {
            var db = new dnbmssqlEntities();

            NullFiller.FillNullFields<rX>(supplier);
            Capitalize.UppercaseClassFields<rX>(supplier);

            // Supplier Login
            var username = supplier.idd + supplier.region;

            var sup_user = db.C_User.FirstOrDefault(s => s.UserName.Equals(username));

            if (sup_user == null)
            {
                var user = new User { UserName = username, Email = username + "@danubeco.com", Region = supplier.region };

                try
                {
                    var result = await UserManager.CreateAsync(user, supplier.password);

                    if (!result.Succeeded)
                    {
                        return false;
                    }
                    else
                    {
                        SupplierInfo(supplier);

                        return true;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                var user = await UserManager.FindByIdAsync(sup_user.UserId);
                
                user.PasswordHash = UserManager.PasswordHasher.HashPassword(supplier.password);

                await UserManager.UpdateAsync(user);

                SupplierInfo(supplier);

                return true;

            }
        }

        public void SupplierInfo(rX supplier)
        {
            // Supplier 
            var supname = db.suppliers.FirstOrDefault(s => s.sid.Equals(supplier.idd));

            if (supname == null)
            {
                db.suppliers.Add(new ADO.supplier
                {
                    sid = supplier.idd,
                    name = supplier.name
                });

                db.SaveChanges();
            }

            SupplierRegion(supplier);
        }

        public void SupplierRegion(rX supplier)
        {
            // Supplier Region
            var sup = db.supplierregions.FirstOrDefault(s => s.sid.Equals(supplier.idd) && s.region.Equals(supplier.region));

            if (sup == null)
            {
                var isms = (string.IsNullOrEmpty(supplier.mobile)) ? "n" : "y";

                db.supplierregions.Add(new ADO.supplierregion
                {
                    sid = supplier.idd,
                    region = supplier.region,
                    password = supplier.password,
                    mobile = supplier.mobile,
                    sms = isms,
                    status = "A"
                });
                
            }
            else
            {
                sup.mobile = supplier.mobile;
                sup.password = supplier.password;                
            }

            db.SaveChanges();
        }


        #endregion

        #region Account Setup

        private ApplicationUserManager _userManager;

        UserManager<User> userManager = new UserManager<User>(new UserStore<User>(new ApplicationDbContext()));
        
        private User FindUser()
        {
            return userManager.FindById(User.Identity.GetUserId());
        }

        public SupplierController() { }

        public SupplierController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        #endregion
                
        #region New Account [Supplier]

        [Authorize(Roles = "Administrator, Agent")]
        public ActionResult NewAccount(string username)
        {
            if(!string.IsNullOrEmpty(username))
            {
                ViewBag.hasName = username;
            }
            else
            {
                ViewBag.hasName = "";
            }

            return View();
        }

        [Authorize(Roles = "Administrator, Agent")]
        [HttpPost]
        public bool RegisterNewAccount(supplier model)
        {
            try
            {
                var isExist = db.suppliers.Where(s => s.sid.Equals(model.sid)).FirstOrDefault();

                if(isExist == null)
                {
                    db.suppliers.Add(model);

                    db.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception)
            {
                return false;
            }

        }

        #endregion

        #region New Account [_User Identity]

        [HttpPost]
        public async Task<bool> AddSupplierRegion(RegisterViewModel model)
        {
            var iSms = (string.IsNullOrEmpty(model.Mobile)) ? "n" : "y";

            var user = new User { UserName = model.Username, Email = model.Email, Region = model.Region };

            try
            {
                var result = await UserManager.CreateAsync(user, model.Password);

                if(!result.Succeeded)
                    return false;

                var currentUser = await UserManager.FindByNameAsync(model.Username);

                await UserManager.AddToRolesAsync(currentUser.Id, "Supplier");

                db.supplierregions.Add(new supplierregion()
                {
                    sid = model.Username.Substring(0, 6),
                    region = model.Username.Substring(6, 3),
                    password = model.Password,
                    mobile = model.Mobile,
                    sms = iSms,
                    status = "A"
                });

                db.SaveChanges();

                return true;
            }
            catch(Exception)
            {
                return false;
            }

        }

        #endregion

        #region Search Supplier 

        private string getRegion()
        {
            var lRegg = User.Identity.Name.Length - 3;

            return User.Identity.Name.Substring(lRegg, 3);
        }

        public ActionResult Search()
        {
            return View();
        }

        public ActionResult UserList(string UserId)
        {
            var curReg = getRegion();

            var users = new List<User>();

            var isExist = db.suppliers.FirstOrDefault(s => s.sid.Equals(UserId));

            if(isExist != null)
            {                
                foreach(var user in UserManager.Users.Where(s => s.UserName.Contains(UserId)))
                {
                    if(user.UserName == UserId + curReg)
                    {
                        users.Add(user);
                    }                        
                    
                }

                if(users.Count() == 0)
                {
                    ViewBag.User = "NEW";
                    ViewBag.Info = UserId;
                }
                else
                {
                    ViewBag.User = users;
                    ViewBag.Info = UserId;
                }                
            }
            else
            {
                ViewBag.User = null;
                ViewBag.Info = UserId;
            }            

            return View();

        }
        
        public ActionResult UserRegion(string UserId)
        {
            ViewBag.Info = UserId;

            ViewBag.Region = getRegion();

            ViewBag.SuppName = getName(UserId);

            return View();
        }

        public ActionResult UserEdit(string userid)
        {
            var nSid = userid.Substring(0, 6);
            var nReg = userid.Substring(6, 3);

            var idd = db.C_User.FirstOrDefault(s => s.UserName.Equals(userid));

            ViewBag.User = db.supplierregions
                .Join(db.suppliers, u => u.sid, uir => uir.sid, (u, uir) => new { u, uir })
                .Where(s => s.u.sid.Equals(nSid) && s.u.region.Equals(nReg))
                .Select(m => new rX
                {
                    idd = idd.UserId,
                    name = m.uir.name,
                    region = m.u.region,
                    email = idd.Email,
                    mobile = m.u.mobile,
                    password = m.u.password

                }).FirstOrDefault();

            return View();
        }
        
        #region Change Password/Email

        [HttpPost]
        public async Task<bool> UpdateInfo(UpdateInfoViewModel model)
        {
            try
            {
                var user = await UserManager.FindByIdAsync(model.UserId);

                if(user == null)
                {
                    return false;
                }

                user.Email = model.Email;

                var result = await UserManager.UpdateAsync(user);

                if(!result.Succeeded)
                {
                    return false;
                }
                else
                {
                    var UserN = db.C_User.FirstOrDefault(s => s.UserId.Equals(model.UserId)).UserName.Substring(0, 6);
                    var UserR = db.C_User.FirstOrDefault(s => s.UserId.Equals(model.UserId)).UserName.Substring(6, 3);

                    db.supplierregions.FirstOrDefault(s => s.sid.Equals(UserN) && s.region.Equals(UserR)).mobile = model.Mobile;

                    db.SaveChanges();

                    return true;
                }

            }
            catch(Exception)
            {
                return false;
            }

        }


        [HttpPost]
        public async Task<bool> ResetPasswordSupplier(ResetPasswordViewModel model)
        {
            try
            {
                var user = await UserManager.FindByIdAsync(model.UserId);

                if(user == null)
                {
                    return false;
                }

                user.PasswordHash = UserManager.PasswordHasher.HashPassword(model.Password);

                var result = await UserManager.UpdateAsync(user);

                if(!result.Succeeded)
                {
                    return false;
                }
                else
                {
                    
                    var UserN = db.C_User.FirstOrDefault(s => s.UserId.Equals(model.UserId)).UserName.Substring(0, 6);
                    var UserR = db.C_User.FirstOrDefault(s => s.UserId.Equals(model.UserId)).UserName.Substring(6, 3);

                    db.supplierregions.FirstOrDefault(s => s.sid.Equals(UserN) && s.region.Equals(UserR)).password = model.Password;

                    db.SaveChanges();

                    return true;
                }

            }
            catch(Exception)
            {
                return false;
            }

        }

        #endregion

        #endregion

        #region Activate Supplier Account

        private string getName(string userid)
        {   
            var hasName = db.suppliers.FirstOrDefault(s => s.sid.Equals(userid));

            if (hasName != null)
            {
                return hasName.name;
            }
            else
            {
                return "";
            }
        }
        
        public async Task<ActionResult> UserArInfo(string userid)
        {
            string msg;

            string[] csvHandler;

            csvHandler = userid.Split('-');

            var iUsername = csvHandler[0] + csvHandler[1];

            var sms = (string.IsNullOrEmpty(csvHandler[3])) ? "no" : "yes";

            var user = new User { UserName = iUsername, Email = iUsername + "@danubeco.com", Region = csvHandler[1] };

            try
            {
                var result = await UserManager.CreateAsync(user, csvHandler[2]);

                if(!result.Succeeded)
                    msg = "Error adding account region.";

                var currentUser = await UserManager.FindByNameAsync(iUsername);

                await UserManager.AddToRolesAsync(currentUser.Id, "Supplier");

                msg = "Successfully added to new Database";
            }
            catch(Exception ex)
            {
                msg = ex.ToString();
            }

            ViewBag.response = msg;

            return View();

        }

        #endregion

        
    }
}


public partial class rX
{
    public string idd { get; set; }
    public string name { get; set; }
    public string region { get; set; }
    public string email { get; set; }
    public string mobile { get; set; }
    public string password { get; set; }
}
