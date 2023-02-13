using eRecruitment.Sita.Web.App_Data.DAL;
using eRecruitment.Sita.Web.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.ComponentModel;
using System.Data;
using System.Text;


namespace eRecruitment.Sita.Web.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext();
        eRecruitmentDataAccess _dal = new eRecruitmentDataAccess();
        //eRecruitment.Sita.Web.Notifications.SendNotification notify = new eRecruitment.Sita.Web.Notifications.SendNotification();
        Notification notify = new Notification();

        // GET: Admin
        public ActionResult Index()
        {
            string userid = User.Identity.GetUserId();
            string role = GetUserRole();

            ViewBag.Organisation = _dal.GetAllOrganisationList(userid, role);
            return View();
        }
   

        public ActionResult AllAssignedUsers()
        {
            string userid = User.Identity.GetUserId();
            string role = GetUserRole();

            ViewBag.AllAssignedUsers = _dal.GetAllAssignedUsers(userid, role);
           // ViewBag.Users = _dal.GetUserList(userid, role);
            return View();
        }

        public ActionResult AllDeactivatedUsers()
        {
            string userid = User.Identity.GetUserId();
            string role = GetUserRole();

            ViewBag.AllDeactivatedUsers = _dal.GetAllDeactivatedUsers(userid, role);
            // ViewBag.Users = _dal.GetUserList(userid, role);
            return View();
        }

        public ActionResult TestUserRole(string userID)
        {
            ViewBag.userID = userID;
            return View();
        }

        //Assign User Role Changes 
        [Authorize]
        [HttpGet]
        public ActionResult AssignUserRole(string userId)
        {

            ViewBag.Users = _dal.GetUserList();
            
            ViewBag.Roles = _dal.GetRoleList();
            ViewBag.Organisation = _dal.GetOrganisationList();


            var data = _db.AspNetUsers.Where(x => x.Id == userId).FirstOrDefault();


                var p = new List<ProfileModels>();
                ProfileModels e = new ProfileModels();
                e.UserID = Convert.ToString(data.Id);
                //e.RoleID = Convert.ToInt32(data.RoleId);
                p.Add(e);

                //ViewBag.UserRole = data;
            return View(p);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignUserRole(string userId, FormCollection fc)
        {
            int roleID = int.Parse(fc["item.RoleID"]);
            int organisationID = int.Parse(fc["item.Organisation"]);
            string uid = User.Identity.GetUserId();

            ViewBag.Users = _dal.GetUserList();
            ViewBag.Roles = _dal.GetRoleList();
            ViewBag.Organisation = _dal.GetOrganisationList();
            if (ModelState.IsValid)
            {
                 _dal.InsertUserRole(userId,roleID,organisationID);
                SendNotification(userId);

                TempData["message"] = "You Have Successfully Assigned User Role";
            }
            //return View();
            return RedirectToAction("AllAssignedUsers", "Admin");
        }

        private void SendNotification(string userID)
        {
            string Senderid = User.Identity.GetUserId();
            Notification notify = new Notification();


            StringBuilder sbEmail = new StringBuilder();

            var UserDetails = _db.tblProfiles.Where(x => x.UserID == userID).FirstOrDefault();
            string UserRoleID = _db.AspNetUserRoles.Where(x => x.UserId == userID).Select(a => a.RoleId).FirstOrDefault();
            string SenderRoleID = _db.AspNetUserRoles.Where(x => x.UserId == Senderid).Select(a => a.RoleId).FirstOrDefault();


            string UserRoleName = _db.AspNetRoles.Where(x => x.Id == UserRoleID).Select(r => r.Name).FirstOrDefault();
            string SenderRoleName = _db.AspNetRoles.Where(x => x.Id == SenderRoleID).Select(r => r.Name).FirstOrDefault();
            string userid = UserDetails.UserID.ToString();
            string To = UserDetails.EmailAddress;


            int Status = 1;


            sbEmail.Append("Dear : <b>" + UserDetails.FirstName + " " + UserDetails.Surname + "</b>" + "<br/>");
            sbEmail.AppendLine();
            sbEmail.Append("<br/>");
            sbEmail.Append("You Have Been Successfully Asigned A User Role As A (<b>" + UserRoleName + "</b>) .<br/> ");
            sbEmail.Append("<br/>");
            //sbEmail.Append("Please log on");
            sbEmail.Append("<br/>");
            sbEmail.AppendLine();
            sbEmail.Append("<br/>");
            sbEmail.Append("Kind Regards<br/>E-Recruitment Team");

            string UserDetailsEmail = sbEmail.ToString();

            notify.SendEmail(To, "e-Recruitment Notification", UserDetailsEmail);

            //Insert Into Email Table
            _dal.InsertEmail(userid, Convert.ToInt32(UserDetails.pkProfileID), "e-Recruitment Team", To, "e-Recruitment Notification", UserDetailsEmail, Status);

        }

        //Get All Users 
        [HttpGet]
        public ActionResult AllUsers()
        {
          
            ViewBag.AllUsers = _dal.GetAllUsers();
            return View();
        }

        private string GetUserRole()
        {
            string role = null;
            if (User.IsInRole("Admin")) { role = "Admin"; }
            else if (User.IsInRole("SysAdmin")) { role = "SysAdmin"; }
            return role;
        }

        [Authorize]
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var p = new List<OrganisationModels>();
            using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
            {
                var data = _db.lutOrganisations.Where(x => x.OrganisationID == id).ToList();
                foreach (var d in data)
                {
                    OrganisationModels e = new OrganisationModels();
                    e.OrganisationID = Convert.ToInt32(d.OrganisationID);
                    e.OrganisationName = Convert.ToString(d.OrganisationName);
                    e.OrganisationCode = Convert.ToString(d.OrganisationCode);
                    p.Add(e);
                }

            }
            ViewBag.BID = id;
            return View(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OrganisationModels item, HttpPostedFileBase postedFile, int id, int bid)
        {
            byte[] bytes = null;
            string filePath = null;
            string ContentType = null;
            string fileName = null;
            int OrganisationID = bid;

            if (postedFile != null && postedFile.ContentLength > 00)
            {
                var fileExt = System.IO.Path.GetExtension(postedFile.FileName).Substring(1);

                var filesize = 5;
                if (fileExt != "png" && fileExt != "gif" && fileExt != "jpeg" && fileExt != "jpg")
                {
                    ModelState.AddModelError("", "File Extension Is InValid - Only Upload Images File");
                }
                else if (postedFile.ContentLength > (filesize * 1024))
                {
                    //ModelState.AddModelError("", "File size Should Be UpTo " + filesize + "KB");
                    //ModelState.AddModelError("", "File size Should Be UpTo 5MB");
                }
            }

            if (ModelState.IsValid)
            {
                if (postedFile != null && postedFile.ContentLength > 00)
                {
                    using (BinaryReader br = new BinaryReader(postedFile.InputStream))
                    {
                        bytes = br.ReadBytes(postedFile.ContentLength);
                    }
                    filePath = Path.GetFileName(postedFile.FileName);
                    fileName = filePath.Remove(filePath.LastIndexOf("."));
                    ContentType = postedFile.ContentType;
                }
                else
                {
                    var bus = _db.lutOrganisations.Where(x => x.OrganisationID == bid).FirstOrDefault();
                    ContentType = bus.contentType;
                    bytes = bus.fileData.ToArray();
                }

                _dal.UpdateOrganisation(item.OrganisationCode, item.OrganisationName, fileName, bytes, ContentType, id);
                return RedirectToAction("Index");
            }
            ViewBag.BID = id;
            return View(item);
        }

        public ActionResult Delete(int id)
        {
            _dal.DeleteOrganisation(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(OrganisationModels model, HttpPostedFileBase postedFile)
        {
            byte[] bytes = null;
            string filePath = null;
            string ContentType = null;
            string fileName = null;
            string userid = User.Identity.GetUserId();
            ViewBag.Organisation = _dal.GetOrganisationList(userid);

            var data = _db.lutOrganisations.Where(x => x.OrganisationName == model.OrganisationName && x.OrganisationCode == model.OrganisationCode && x.OrganisationID == model.OrganisationID).Count();

            if (data > 0)
            {
                ModelState.AddModelError(" ", "Record is already Exist");
            }
            else
            {
                if (postedFile != null && postedFile.ContentLength > 00)
                {
                    var fileExt = System.IO.Path.GetExtension(postedFile.FileName).Substring(1);

                    var filesize = 5;
                    if (fileExt != "png" && fileExt != "gif" && fileExt != "jpeg" && fileExt != "jpg")
                    {
                        ModelState.AddModelError("", "File Extension Is InValid - Only Upload Images File");
                        //ViewBag.Error("File Extension Is InValid - Only Upload Images File");
                    }
                    else if (postedFile.ContentLength > (filesize * 1024))
                    {
                        //ModelState.AddModelError("", "File size Should Be UpTo " + filesize + "KB");
                        //ModelState.AddModelError("", "File size Should Be UpTo 5MB");
                    }
                }
                if (postedFile == null && postedFile.ContentLength <= 0)
                {
                    ModelState.AddModelError("", "Organisation Logo cannot be empty");

                }
            }
            if (ModelState.IsValid)
            {
                if (postedFile != null && postedFile.ContentLength > 00)
                {
                    using (BinaryReader br = new BinaryReader(postedFile.InputStream))
                    {
                        bytes = br.ReadBytes(postedFile.ContentLength);
                    }
                    filePath = Path.GetFileName(postedFile.FileName);
                    fileName = filePath.Remove(filePath.LastIndexOf("."));
                    ContentType = postedFile.ContentType;
                }

                _dal.InsertIntoOrganisation(model.OrganisationCode, model.OrganisationName, fileName, bytes, ContentType);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult AddVacancyProfile()
        {
            string userid = User.Identity.GetUserId();

            ViewBag.Organisation = _dal.GetOrganisationList(userid);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddVacancyProfile(VacancyProfileModels model)
        {
            string userid = User.Identity.GetUserId();
            ViewBag.Organisation = _dal.GetOrganisationList(userid);

            var data = _db.tblVacancyProfiles.Where(x => x.VacancyName == model.VacancyName && x.OrganasationID == model.OrganisationID).Count();

            if (data > 0)
            {
                ModelState.AddModelError(" ", "Record is already Exist");
            }
            if (ModelState.IsValid)
            {
                _dal.InsertIntoVacancyProfile(model.VacancyName, model.VacancyPurpose, model.Responsibility, model.QualificationsandExperience, model.TechnicalCompetenciesDescription, model.OtherSpecialRequirements, model.Disclaimer, model.OrganisationID);

                TempData["message"] = "You Have Successfully Created A Vancy Profile";

                return RedirectToAction("VacancyProfileList", "Admin");
            }
            return View(model);
        }

    

        [HttpGet]
        public ActionResult EditUserRole(string UserID)
        {
            string userid = User.Identity.GetUserId();
            string role = GetUserRole();
      
            ViewBag.Users = _dal.GetUserList(userid, role);
            ViewBag.Roles = _dal.GetRoleList(userid, role);
            ViewBag.Organisation = _dal.GetAllOrganisationList(userid, role);
            var data = _db.AspNetUserRoles.Where(x => x.UserId == UserID).FirstOrDefault();

            var p = new List<ProfileModels>();
            ProfileModels e = new ProfileModels();
            e.UserID = Convert.ToString(data.UserId);
            e.RoleID = Convert.ToInt32(data.RoleId);
            e.Organisation = Convert.ToInt32(data.OrganisationID);
            p.Add(e);

            //ViewBag.UserRole = data;
            return View(p);
        }

        [HttpPost]
        public ActionResult EditUserRole(string UserID, FormCollection fc)
        {
            string userid = User.Identity.GetUserId();
            string role = GetUserRole();

            ViewBag.Users = _dal.GetUserList(userid, role);
            ViewBag.Roles = _dal.GetRoleList(userid, role);
            ViewBag.Organisation = _dal.GetAllOrganisationList(userid, role);
            int organisationID = int.Parse(fc["item.Organisation"]);
            int roleID = int.Parse(fc["item.RoleID"]);

            //Remeber to check Validation - Ntshengedzeni
            _dal.UpdateUserRole(UserID, Convert.ToString(roleID), organisationID);

            TempData["message"] = " User Role Edited Successfully";

            return RedirectToAction("AllAssignedUsers", "Admin");
        }

        [HttpGet]
        public ActionResult ReassignUserRole(string userId)
        {
            string userid = User.Identity.GetUserId();
            string role = GetUserRole();

            ViewBag.Users = _dal.GetUserList(userid, role);
            ViewBag.Roles = _dal.GetRoleList(userid, role);
            ViewBag.Organisation = _dal.GetAllOrganisationList(userid, role);
            ViewBag.UserStatus = _dal.GetUserStatusList();
            var data = _db.AspNetUserRoles.Where(x => x.UserId == userId).FirstOrDefault();

            var p = new List<ProfileModels>();
            ProfileModels e = new ProfileModels();
            e.UserID = Convert.ToString(data.UserId);
            e.RoleID = Convert.ToInt32(data.RoleId);
            e.Organisation = Convert.ToInt32(data.OrganisationID);
            e.UserStatusID = Convert.ToInt32(data.UserStatusID);
            p.Add(e);

            return View(p);
        }

        [HttpPost]
        public ActionResult ReassignUserRole(string UserID, FormCollection fc)
        {
            string userid = User.Identity.GetUserId();
            string role = GetUserRole();

            ViewBag.Users = _dal.GetUserList(userid, role);
            ViewBag.Roles = _dal.GetRoleList(userid, role);
            ViewBag.Organisation = _dal.GetAllOrganisationList(userid, role);
            ViewBag.UserStatus = _dal.GetUserStatusList();
            int organisationID = int.Parse(fc["item.Organisation"]);
            int roleID = int.Parse(fc["item.RoleID"]);
            int userStatusID = int.Parse(fc["item.UserStatusID"]);

            //Remeber to check Validation - Ntshengedzeni
            _dal.ReassignUserRole(UserID, Convert.ToString(roleID), organisationID, userStatusID);

            TempData["message"] = " User Role Reassigned Successfully";

            return RedirectToAction("AllAssignedUsers", "Admin");
        }

        public ActionResult DeleteUserRole(string id)
        {
            _dal.DeleteUserRole(id);
            return RedirectToAction("AllAssignedUsers", "Admin");
        }

        [HttpGet]
        public ActionResult DeactivateUserRole(string idUSer)
        {
            string userid = User.Identity.GetUserId();
            string role = GetUserRole();

            ViewBag.Users = _dal.GetUserList(userid, role);
            ViewBag.Roles = _dal.GetRoleList(userid, role);
            ViewBag.Organisation = _dal.GetAllOrganisationList(userid, role);
            ViewBag.UserStatus = _dal.GetUserStatusList();
            var data = _db.AspNetUserRoles.Where(x => x.UserId == idUSer).FirstOrDefault();

            var p = new List<ProfileModels>();
            ProfileModels e = new ProfileModels();
            e.UserID = Convert.ToString(data.UserId);
            e.RoleID = Convert.ToInt32(data.RoleId);
            e.Organisation = Convert.ToInt32(data.OrganisationID);
            p.Add(e);

            //ViewBag.UserRole = data;
            return View(p);
        }


        [HttpPost]
        public ActionResult DeactivateUserRole(string idUSer, FormCollection fc)
        {
            string userid = User.Identity.GetUserId();
            string role = GetUserRole();

            ViewBag.Users = _dal.GetUserList(userid, role);
            ViewBag.Roles = _dal.GetRoleList(userid, role);
            ViewBag.Organisation = _dal.GetAllOrganisationList(userid, role);
            int organisationID = int.Parse(fc["item.Organisation"]);
            int roleID = int.Parse(fc["item.RoleID"]);
            int userStatusID = int.Parse(fc["item.UserStatusID"]);

            _dal.DeactivateUserRole(idUSer, Convert.ToString(roleID), organisationID, userStatusID);

            TempData["message"] = "User Role Deactivated Successfully";

            return RedirectToAction("AllAssignedUsers", "Admin");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditVacancyProfile(VacancyProfileModels item, int id)
        {

            if (ModelState.IsValid)
            {
                _dal.UpdateVacancyProfile(id, item.VacancyName, item.VacancyPurpose, item.Responsibility, item.QualificationsandExperience, item.TechnicalCompetenciesDescription, item.OtherSpecialRequirements, item.Disclaimer, item.OrganisationID);

                TempData["message"] = "Vacancy Profile Successfully Edited";

                return RedirectToAction("VacancyProfileList", "Admin");
            }
            return View();
        }

        //public ActionResult DeleteVacancyProfile(int id)
        //{
        //    string userid = User.Identity.GetUserId();

        //    ViewBag.Organisation = _dal.GetOrganisationList(userid);
        //    var vanP = _dal.GetVacancyProfileForEdit(id);
        //    ViewBag.Vacancy = vanP;
        //    return View(vanP);
        //}
        public ActionResult DeleteVacancyProfileConfirmed(int id)
        {
            _dal.DeleteVacancyProfile(id);
            return RedirectToAction("VacancyProfileList", "Admin");
        }

        //public ActionResult DepartmentList()
        //{
        //    string userid = User.Identity.GetUserId();
        //    ViewBag.Department = _dal.GetDepartmentList(userid);
        //    return View();
        //}

        [Authorize]
        [HttpGet]
        public ActionResult AddDepartment()
        {
            string userid = User.Identity.GetUserId();
            int orgid = _db.AspNetUserRoles.Where(x=>x.UserId==userid).Select(x=>x.OrganisationID).FirstOrDefault();
            ViewBag.ManagerList = _dal.GetDepartmentalManagerList(orgid);
            ViewBag.Organisation = _dal.GetOrganisationList(userid);
            return View();
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDepartment(DepartmentModel item, int id)
        {
            string userid = User.Identity.GetUserId();
            ViewBag.Organisation = _dal.GetOrganisationList(userid);
            int orgid = _db.AspNetUserRoles.Where(x => x.UserId == userid).Select(x => x.OrganisationID).FirstOrDefault();
            ViewBag.ManagerList = _dal.GetDepartmentalManagerList(orgid);
            if (ModelState.IsValid)
            {
                _dal.UpdateIntoDepartment(id, item.DepartmentName, item.OrganisationID, item.ManagerID);

                TempData["message"] = "Department Edited Successfully";

                return RedirectToAction("DepartmentList", "Admin");
            }
            return View();
        }

        public ActionResult DeleteDepartment(int id)
        {
            _dal.DeleteIntoDepartment(id);
            return RedirectToAction("DepartmentList", "Admin");
        }

        public ActionResult DivisionList()
        {
            string userid = User.Identity.GetUserId();
            ViewBag.Division = _dal.GetDivisionList(userid);
            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult AddDivision()
        {
            string userid = User.Identity.GetUserId();
            ViewBag.Organisation = _dal.GetOrganisationList(userid);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDivision(DivisionModel model)
        {
            string userid = User.Identity.GetUserId();
            ViewBag.Organisation = _dal.GetOrganisationList(userid);

            var data = _db.lutDivisions.Where(x => x.DivisionDiscription == model.DivisionDiscription && x.OrganisationID == model.OrganisationID).Count();

            if (data > 0)
            {
                ModelState.AddModelError(" ", "Record is already Exist");
            }
            if (ModelState.IsValid)
            {
                _dal.InsertIntoDivision(model.DivisionDiscription, model.OrganisationID);

                TempData["message"] = "You Have Successfully Created A Division";

                return RedirectToAction("DivisionList", "Admin");
            }
            return View(model);
        }


        [Authorize]
        [HttpGet]
        public ActionResult EditDivision(int id)
        {

            string userid = User.Identity.GetUserId();
            ViewBag.Organisation = _dal.GetOrganisationList(userid);
            var Div = _dal.GetDivisionForEdit(id);
        
            return View(Div);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDivision(DivisionModel item, int id)
        {
            string userid = User.Identity.GetUserId();
            ViewBag.Organisation = _dal.GetOrganisationList(userid);

            if (ModelState.IsValid)
            {
                _dal.UpdateIntoDivision(id, item.DivisionDiscription, item.OrganisationID);

                TempData["message"] = "Division Successfully Edited";

                return RedirectToAction("DivisionList", "Admin");
            }
            return View();
        }

        public ActionResult DeleteDivision(int id)
        {
            _dal.DeleteIntoDivision(id);
            return RedirectToAction("DivisionList", "Admin");
        }

        public ActionResult QuestionBanksList()
        {
            string userid = User.Identity.GetUserId();
            ViewBag.QuestionBank = _dal.GetQuestionBanksList(userid);
            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult AddQuestionBanks()
        {
            string userid = User.Identity.GetUserId();
            ViewBag.Organisation = _dal.GetOrganisationList(userid);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddQuestionBanks(QuetionBanksModel model)
        {
            string userid = User.Identity.GetUserId();
            ViewBag.Organisation = _dal.GetOrganisationList(userid);

            var data = _db.lutGeneralQuestions.Where(x => x.GeneralQuestionDesc == model.GeneralQuestionDesc && x.OrganisationID == model.OrganisationID).Count();

            if (data > 0)
            {
                ModelState.AddModelError(" ", "Record is already Exist");
            }

            if (ModelState.IsValid)
            {
                _dal.InsertIntoQuestionBanks(model.GeneralQuestionDesc, model.OrganisationID);

                TempData["message"] = "You Have Successfully Created A Question Bank";

                return RedirectToAction("QuestionBanksList", "Admin");
            }
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditQuestionBanks(int id)
        {
            string userid = User.Identity.GetUserId();
            ViewBag.Organisation = _dal.GetOrganisationList(userid);
            var Q = _dal.GetQuestionBankForEdit(id);

            return View(Q);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditQuestionBanks(QuetionBanksModel item, int id)
        {
            string userid = User.Identity.GetUserId();
            ViewBag.Organisation = _dal.GetOrganisationList(userid);

            if (ModelState.IsValid)
            {
                _dal.UpdateIntoQuetionsBank(id, item.GeneralQuestionDesc, item.OrganisationID);

                TempData["message"] = "Question Bank Successfully Edited";

                return RedirectToAction("QuestionBanksList", "Admin");
            }
            return View();
        }
        public ActionResult DeleteQuestionBank(int id)
        {
            _dal.DeleteIntoGeneralQuestion(id);
            return RedirectToAction("QuestionBanksList", "Admin");
        }

    
  

     


        public ActionResult DeleteSalaryBank(int id)
        {
            _dal.DeleteIntoSalaryBank(id);
            TempData["message"] = "Salary Bank Successfully Deleted";
            return RedirectToAction("SalaryBanksList", "Admin");
        }


        [HttpGet]
        public ActionResult AddJobLevelList()
        {
            string userid = User.Identity.GetUserId();
            ViewBag.Organisation = _dal.GetOrganisationList(userid);
            return View();
        }


        public ActionResult DeleteJobLevel(int id)
        {
            _dal.DeleteJobLevel(id);
            return RedirectToAction("JobLevelList", "Admin");
        }
    }
}