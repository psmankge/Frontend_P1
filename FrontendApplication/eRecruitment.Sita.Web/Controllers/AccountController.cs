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
using eRecruitment.Sita.Web.Models;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using eRecruitment.Sita.Web.App_Data.DAL;

namespace eRecruitment.Sita.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        eRecruitmentDataAccess _db = new eRecruitmentDataAccess();//Declare Data Access
        eRecruitmentDataClassesDataContext db = new eRecruitmentDataClassesDataContext(); //Declare DataContext

        private string fullname = string.Empty;
        private string surname = string.Empty;
        private string email = string.Empty;
        private string idNumber = string.Empty;
        private string passportNo = string.Empty;
        private string phoneNumber = string.Empty;

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

    //
    // GET: /Account/Login
    [AllowAnonymous]
    public async Task<ActionResult> Login(string returnUrl, string id)
    {

      string WebURL = Request.Url.ToString();
      //string webapiDev = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"];
      //string webapiDevProd = System.Configuration.ConfigurationManager.AppSettings["WebAPIURLProd"];
      //string newWebAPI = null;
      Session["sip"] = null;

      //if (WebURL.Contains("dev"))
      //{
      //     newWebAPI = string.Format(webapiDev, 158);
      //}
      //else if (WebURL.Contains("cit"))
      //{
      //    newWebAPI = string.Format(webapiDev, 208);
      //}
      //else if (WebURL.Contains("sit"))
      //{
      //    newWebAPI = string.Format(webapiDev, 237);
      //}
      //else if (WebURL.Contains("uat"))
      //{
      //    newWebAPI = string.Format(webapiDev, 140);
      //}
      //else if (WebURL.Contains("beta"))
      //{
      //    newWebAPI = string.Format(webapiDevProd, 57);
      //}
      //else
      //{
      //    newWebAPI = string.Format(webapiDevProd, 31);
      //}

      if (id != null)
      {
        HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"] + id);
        //HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(newWebAPI + id);

        myRequest.Timeout = 5000;
        HttpWebResponse response = (HttpWebResponse)myRequest.GetResponse();
        if (response.StatusCode == HttpStatusCode.OK)
        {
          Stream dataStream = response.GetResponseStream();
          // Open the stream using a StreamReader for easy access.
          StreamReader reader = new StreamReader(dataStream);
          // Read the content.
          string responseFromServer = reader.ReadToEnd();

          dynamic data = JsonConvert.DeserializeObject(responseFromServer);
          fullname = Convert.ToString(data["name"]);
          surname = Convert.ToString(data["surname"]);
          email = Convert.ToString(data["email"]);
          idNumber = Convert.ToString(data["idNumber"]);
          passportNo = Convert.ToString(data["passport"]);
          phoneNumber = Convert.ToString(data["phoneNumber"]);


          fullname = fullname.Trim();
          surname = surname.Trim();
          email = email.Trim();
          idNumber = idNumber.Trim();
          passportNo = passportNo.Trim();
          phoneNumber = phoneNumber.Trim();


          // Display the content.
          ViewBag.TestMessage = fullname;
          response.Close();
          //return true;
        }
        else
        {
          response.Close();
          //return false;
        }
      }
            //Console.Write("Data Returned");
            //name = "Ntshengedzeni";
            //surname = "Badamarema";
            //email = "nbadama@gmail.com";
            //idNumber = "7907265091081";
            //phoneNumber = "0725365413";


            //ML REDO//if (email != null || email != "")
            if (!string.IsNullOrEmpty(email))
            {
                var data = (dynamic)null;
                //var data = db.AspNetUsers.Where(x => x.Email == email).Count();
                if (!string.IsNullOrEmpty(idNumber))
                {

                    data = db.tblProfiles.Where(x => x.IDNumber == idNumber).Count();
                }
                else if (!string.IsNullOrEmpty(passportNo) && string.IsNullOrEmpty(idNumber))
                {

                    data = db.tblProfiles.Where(x => x.PassportNo == passportNo).Count();

                }
                if (data == 0)
                {
                    if (ModelState.IsValid)
                    {
                        var user = new ApplicationUser { UserName = email, Email = email };
                        var result = await UserManager.CreateAsync(user, "P@$$w0rd1");
                        if (result.Succeeded)
                        {
                            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                            if (!string.IsNullOrEmpty(idNumber))
                            {
                                passportNo = string.Empty;
                            }

                            //Insert User Profile
                            _db.CreateUserProfile(user.Id, idNumber, passportNo, surname, fullname, phoneNumber, email);

                            return RedirectToAction("Jobs", "Home");
                        }

                        AddErrors(result);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(idNumber))
                    {
                        passportNo = string.Empty;
                    }
                    db.sp_UpdateUserFromPortal(idNumber, phoneNumber, email, fullname, surname, passportNo);

                    var result = await SignInManager.PasswordSignInAsync(email, "P@$$w0rd1", false, shouldLockout: false);
                    switch (result)
                    {
                        case SignInStatus.Success:
                            //return RedirectToLocal(returnUrl);
                            //return this.RedirectToAction("PublishedDemand", "Demand");


                            //ML_20210217 REMOVED BECAUSE THEY SAID THAT THE CITIZEN DOES NOT GET ASSIGNED TO ANY ROLES

                            //string userid = SignInManager.AuthenticationManager.AuthenticationResponseGrant.Identity.GetUserId();
                            //var role = (from a in db.AspNetUserRoles
                            //            join b in db.AspNetRoles on a.RoleId equals b.Id
                            //            where a.UserId == userid
                            //            select new { b.Name }).FirstOrDefault();


                            //if (role != null)
                            //{
                            //    if (role.Name == "Recruiter" || role.Name == "Admin" || role.Name == "Approver")
                            //    {
                            //        return this.RedirectToAction("Jobs", "Home");
                            //    }
                            //    if (role.Name == "SysAdmin")
                            //    {
                            //        return this.RedirectToAction("Jobs", "Home");

                            //    }
                            //}


                            //ML ADDED IN CASE USER DID NOT HAVE A PROFILE
                            var userID = db.AspNetUsers.Where(w => w.Email.Equals(email)).Select(s => s.Id).FirstOrDefault();

                            var checkProfileExist = db.tblProfiles.Where(a => a.UserID == userID).Count();
                            if (checkProfileExist == 0)
                            {
                                if (!string.IsNullOrEmpty(idNumber))
                                {
                                    passportNo = string.Empty;
                                }
                                //Insert User Profile
                                _db.CreateUserProfile(userID, idNumber, passportNo, surname, fullname, phoneNumber, email);
                            }

                            return this.RedirectToAction("Jobs", "Home");

                        case SignInStatus.LockedOut:
                            return View("Lockout");
                        //case SignInStatus.RequiresVerification:
                        //    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                        case SignInStatus.Failure:
                        default:
                            ModelState.AddModelError("", "Invalid login attempt.");
                            //return View(model);
                            return View();
                    }
                }
            }
            ViewBag.ReturnUrl = returnUrl;
      return View();
    }

    //
    // POST: /Account/Login
    [HttpPost]
    [AllowAnonymous]
    //[ValidateAntiForgeryToken]
    public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }

      // This doesn't count login failures towards account lockout
      // To enable password failures to trigger account lockout, change to shouldLockout: true
      var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
      switch (result)
      {
        case SignInStatus.Success:

                    //ML_20210217 REMOVED BECAUSE THEY SAID THAT THE CITIZEN DOES NOT GET ASSIGNED TO ANY ROLES

                    //string userid = SignInManager.AuthenticationManager.AuthenticationResponseGrant.Identity.GetUserId();
                    //var data = (from a in db.AspNetUserRoles
                    //            join b in db.AspNetRoles on a.RoleId equals b.Id
                    //            where a.UserId == userid
                    //            select new { b.Name }).FirstOrDefault();
                    //if (data != null)
                    //{
                    //    if (data.Name == "Recruiter" || data.Name == "Admin" || data.Name == "Approver")
                    //    {
                    //        return this.RedirectToAction("Jobs", "Home");
                    //    }
                    //    if (data.Name == "SysAdmin")
                    //    {
                    //        return this.RedirectToAction("Jobs", "Home");
                    //    }
                    //}

                    // return RedirectToLocal(returnUrl);

                    return this.RedirectToAction("Jobs", "Home");

        case SignInStatus.LockedOut:
          return View("Lockout");
        case SignInStatus.RequiresVerification:
          return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
        case SignInStatus.Failure:
        default:
          ModelState.AddModelError("", "Invalid login attempt.");
          return View(model);
      }
    }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    _db.CreateUserProfile(user.Id, model.IDNumber, model.PassportNumber, model.Surname, model.FirstName, model.CellNo,model.Email);

                    string fileName = "NoImage.PNG";
                    string filepath = Server.MapPath( "~/Content/dist/img/" + fileName);

                    byte[] bytes = System.IO.File.ReadAllBytes(filepath);
                    _db.UpdateProfilePicture(user.Id, bytes);
                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Jobs", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {


            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult AssignUserRole(string id)
        {
            string userid = User.Identity.GetUserId();
            string role = null;
            if (User.IsInRole("Admin")) { role = "Admin"; }
            else if (User.IsInRole("SysAdmin")) { role = "SysAdmin"; }
            ViewBag.Users = _db.GetUserList(userid, role);
            ViewBag.Roles = _db.GetRoleList(userid,role);
            ViewBag.Organisation = _db.GetAllOrganisationList(userid, role);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignUserRole(ProfileModels model)
        {
            string userid = User.Identity.GetUserId();
            string role = null;
            if (User.IsInRole("Admin")) { role = "Admin"; }
            else if (User.IsInRole("SysAdmin")) { role = "SysAdmin"; }

            ViewBag.Users = _db.GetUserList(userid,role);
            ViewBag.Roles = _db.GetRoleList(userid, role);
            ViewBag.Organisation = _db.GetAllOrganisationList(userid, role);
            if (ModelState.IsValid)
            {
                _db.InsertUserRole(model.UserID, model.RoleID, model.Organisation);

                TempData["message"] = "You Have Successfully Assigned User Role";
            }

            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOut()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        public void GoToPortal()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["LogOutURL"].ToString());
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}