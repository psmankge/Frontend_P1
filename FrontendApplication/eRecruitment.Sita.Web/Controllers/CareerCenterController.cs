using eRecruitment.Sita.Web.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Text;

namespace eRecruitment.Sita.Web.Controllers
{
    public class CareerCenterController : Controller
    {
        eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext();
        eRecruitmentDataAccess _dal = new eRecruitmentDataAccess();

        // GET: CareerCenter
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MyProfile(string tabname)
        {
            string Userid = User.Identity.GetUserId();
            int profileID = 0;
            profileID = _dal.GetProfileID(User.Identity.GetUserId());

            ViewBag.Race = _dal.GetRaceList();
            ViewBag.Gender = _dal.GetGenderList();
            ViewBag.Province = _dal.GetProvinceList();
            ViewBag.YesorNo = _dal.GetYesorNoList();
            ViewBag.Country = _dal.GetCountryList();
            ViewBag.Disability = _dal.GetDisabilityList();
            ViewBag.QualificationType = _dal.GetQualificationTypeList();
            ViewBag.Language = _dal.GetLanguageList();
            ViewBag.LanguageProficiency = _dal.GetLanguageProficiencyList();
            ViewBag.SkillProficiency = _dal.GetSkillProficiencyList();
            ViewBag.Skills = _dal.GetSkillsList();

            //ViewBag.Education = _dal.GetQualificationByProfileID(Userid);
            ViewBag.WorkHistory = _dal.GetWorkHistoryByProfileID(Userid);
            ViewBag.References = _dal.GetReferenceByProfileID(Userid);
            ViewBag.Attachments = _dal.GetAttachmentByProfileID(Userid);
            ViewBag.SkillsForProfile = _dal.GetSkillsByProfileID(profileID);
            ViewBag.LanguagesForProfile = _dal.GetLanguagesByProfileID(profileID);
            ViewBag.CommunicationMethod = _dal.GetMethodOfCommunication();

            var P = new List<ProfileModel>();
            using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
            {
                var prof = _db.tblProfiles.Where(x => x.UserID == Userid).ToList();

                foreach (var d in prof)
                {
                    ProfileModel e = new ProfileModel();
                    e.IDNumber = Convert.ToString(d.IDNumber);
                    e.Surname = Convert.ToString(d.Surname);
                    e.FirstName = Convert.ToString(d.FirstName);
                    e.DateOfBirth = Convert.ToDateTime(d.DateOfBirth);
                    e.fkRaceID = Convert.ToInt16(d.fkRaceID);
                    e.fkGenderID = Convert.ToInt16(d.fkGenderID);
                    e.CellNo = Convert.ToString(d.CellNo);
                    e.AlternativeNo = Convert.ToString(d.AlternativeNo);
                    e.EmailAddress = Convert.ToString(d.EmailAddress);
                    e.UnitNo = Convert.ToString(d.UnitNo);
                    e.ComplexName = Convert.ToString(d.ComplexName);
                    e.StreetNo = Convert.ToString(d.StreetNo);
                    e.StreetName = Convert.ToString(d.StreetName);
                    e.SuburbName = Convert.ToString(d.SuburbName);
                    e.City = Convert.ToString(d.City);
                    e.PostalCode = Convert.ToString(d.PostalCode);
                    e.fkDisabilityID = Convert.ToInt16(d.fkDisabilityID);
                    e.NatureOfDisability = Convert.ToInt16(d.NatureOfDisability);
                    e.SACitizen = Convert.ToInt16(d.SACitizen);
                    e.fkNationalityID = Convert.ToInt16(d.fkNationalityID);
                    e.fkProvinceID = Convert.ToInt16(d.fkProvinceID);
                    e.fkWorkPermitID = Convert.ToInt16(d.fkWorkPermitID);
                    e.WorkPermitNo = Convert.ToString(d.WorkPermitNo);
                    e.pkCriminalOffenseID = Convert.ToInt16(d.pkCriminalOffenseID);
                    e.fkLanguageForCorrespondenceID = Convert.ToInt16(d.fkLanguageForCorrespondenceID);
                    e.TelNoDuringWorkingHours = Convert.ToString(d.TelNoDuringWorkingHours);
                    e.MethodOfCommunicationID = Convert.ToInt16(d.MethodOfCommunicationID);
                    e.CorrespondanceDetails = Convert.ToString(d.CorrespondanceDetails);
                    //e.TermsAndConditions = Convert.ToBoolean(d.TermsAndConditions);
                    P.Add(e);
                }
            }
            return View(P);

        }

        public ActionResult MyApplications()
        {
            ViewBag.MyApplications = _dal.GetCandidateVacancyApplications(User.Identity.GetUserId());
            return View();
        }

        [HttpPost]
        public ActionResult UpdateProfile(ProfileModel item, HttpPostedFileBase postedCV, HttpPostedFileBase postedID)
        {
            ViewBag.CommunicationMethod = _dal.GetMethodOfCommunication();
            string Userid;
            Userid = User.Identity.GetUserId();

            string IDNumber = item.IDNumber;
            string Surname = item.Surname;
            string FirstName = item.FirstName;
            string DateOfBirth = Convert.ToString(item.DateOfBirth);
            int fkRaceID = Convert.ToInt16(item.fkRaceID);
            int fkGenderID = Convert.ToInt16(item.fkGenderID);
            string CellNo = item.CellNo;
            string AlternativeNo = item.AlternativeNo;
            string EmailAddress = item.EmailAddress;
            string UnitNo = item.UnitNo;
            string ComplexName = item.ComplexName;
            string StreetNo = item.StreetNo;
            string StreetName = item.StreetName;
            string SuburbName = item.SuburbName;
            string City = item.City;
            string PostalCode = item.PostalCode;
            int fkDisabilityID = Convert.ToInt16(item.fkDisabilityID);
            int NatureOfDisability = Convert.ToInt16(item.NatureOfDisability);
            string OtherNatureOfDisability = item.OtherNatureOfDisability;
            int SACitizen = Convert.ToInt16(item.SACitizen);
            int fkNationalityID = Convert.ToInt16(item.fkNationalityID);
            int fkProvinceID = Convert.ToInt16(item.fkProvinceID);
            int fkWorkPermitID = Convert.ToInt16(item.fkWorkPermitID);
            string WorkPermitNo = item.WorkPermitNo;
            int pkCriminalOffenseID = Convert.ToInt16(item.pkCriminalOffenseID);
            int fkLanguageForCorrespondenceID = Convert.ToInt16(item.fkLanguageForCorrespondenceID);
            string TelNoDuringWorkingHours = item.TelNoDuringWorkingHours;
            int MethodOfCommunicationID = Convert.ToInt16(item.MethodOfCommunicationID);
            string CorrespondanceDetails = item.CorrespondanceDetails;
            int? ProfessionallyRegisteredID = item.ProfessionallyRegisteredID;
            string RegistrationDate = Convert.ToString(item.RegistrationDate);
            string RegistrationNumber = item.RegistrationNumber;
            string RegistrationBody = item.RegistrationBody;
            int? PreviouslyEmployedPS = item.PreviouslyEmployedPS;
            string ReEmployment = item.ReEmploy;
            string PreviouslyEmployedDepartment = item.PreviouslyEmployedDepartment;
            int DriversLicenseID = Convert.ToInt16(item.DriversLicenseID);
            int MatricID = Convert.ToInt16(item.MatricID);
            int ConditionsThatPreventsID = Convert.ToInt16(item.ConditionsThatPreventsReEmploymentID);

            _dal.UpdateProfileInfo(Userid, IDNumber, Surname, FirstName, Convert.ToString(DateOfBirth), (int)fkRaceID
                 , fkGenderID, CellNo, AlternativeNo, EmailAddress, UnitNo, ComplexName, StreetNo, StreetName, SuburbName
                 , City, PostalCode, (int)fkDisabilityID, NatureOfDisability, OtherNatureOfDisability, SACitizen
                 , (int)fkNationalityID, (int)fkProvinceID, (int)fkWorkPermitID, WorkPermitNo, pkCriminalOffenseID
                 , (int)fkLanguageForCorrespondenceID, TelNoDuringWorkingHours, MethodOfCommunicationID, CorrespondanceDetails
                 , Convert.ToInt32(1), (int)ProfessionallyRegisteredID, Convert.ToString(RegistrationDate), RegistrationNumber, RegistrationBody
                 , (int)PreviouslyEmployedPS, ReEmployment, PreviouslyEmployedDepartment, (int)DriversLicenseID, (int)MatricID,(int)ConditionsThatPreventsID);

            int profileID = 0;

            profileID = _dal.GetProfileID(User.Identity.GetUserId());

            if (postedCV != null && postedCV.ContentLength > 00)
            {
                var FileExt = System.IO.Path.GetExtension(postedCV.FileName).Substring(1);
                byte[] bytes;

                using (BinaryReader br = new BinaryReader(postedCV.InputStream))
                {
                    bytes = br.ReadBytes(postedCV.ContentLength);
                }

                //string fileName = Path.GetFileName(postedCV.FileName);
                string fileName = "Curriculum Vitae";
                string ContentType = postedCV.ContentType;
                var count = _db.Attachments.Where(x => x.ProfileID == profileID && x.fileName == fileName).Count();
                if (count == 0)
                {
                    _dal.AddAttachments(profileID, fileName, bytes, ContentType, System.IO.Path.GetExtension(postedCV.FileName));
                }
                else
                {
                    _dal.UpdateAttachments(profileID, fileName, bytes, ContentType, System.IO.Path.GetExtension(postedCV.FileName));
                }
             
            }

            if (postedID != null && postedID.ContentLength > 00)
            {
                var FileExt = System.IO.Path.GetExtension(postedID.FileName).Substring(1);
                byte[] bytes;

                using (BinaryReader br = new BinaryReader(postedID.InputStream))
                {
                    bytes = br.ReadBytes(postedID.ContentLength);
                }

                //string fileName = Path.GetFileName(postedID.FileName);
                string fileName = "Identity Document";
                string ContentType = postedID.ContentType;
                var count = _db.Attachments.Where(x => x.ProfileID == profileID && x.fileName == fileName).Count();
                if (count == 0)
                {
                    _dal.AddAttachments(profileID, fileName, bytes, ContentType, System.IO.Path.GetExtension(postedID.FileName));
                }
                else
                {
                    _dal.UpdateAttachments(profileID, fileName, bytes, ContentType, System.IO.Path.GetExtension(postedID.FileName));
                }
   
            }

            return RedirectToAction("MyProfile", "CareerCenter");
        }

        [HttpGet]
        public ActionResult AddEducation()
        {
            string Userid = User.Identity.GetUserId();
            ViewBag.QualificationType = _dal.GetQualificationTypeList();
            //ViewBag.Education = _dal.GetQualificationByProfileID(Userid);
            return View();
        }
        [HttpGet]
        public ActionResult getEducation(string id)
        {
            ViewBag.QualificationType = _dal.GetQualificationTypeList();
            id = User.Identity.GetUserId();
            //ViewBag.Education = _dal.GetQualificationByProfileID(id);

            if (ViewBag.Education.Count == 0)
            {
                return RedirectToAction("AddEducation", "CareerCenter");
            }
            else
            {
                var p = new List<EducationModel>();
                using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
                {
                    var id1 = _dal.GetProfileID(User.Identity.GetUserId());
                    var d = _db.tblCandidateEducations.Where(x => x.ProfileID == id1).FirstOrDefault();

                    EducationModel e = new EducationModel();
                    e.qualificationID = Convert.ToInt32(d.QualificationID);
                    e.profileID = Convert.ToInt32(d.ProfileID);
                    e.institutionName = Convert.ToString(d.InstitutionName);
                    e.qualificationName = Convert.ToString(d.QualificationName);
                    e.QualificationTypeID = Convert.ToInt16(d.QualificationTypeID);
                    e.certificateNumber = Convert.ToString(d.CertificateNumber);
                    e.startDate = Convert.ToDateTime(d.StartDate).ToString("d");
                    e.endDate = Convert.ToDateTime(d.EndDate).ToString("d");
                    p.Add(e);

                }
                return View(p);
            }

        }
        [HttpPost]
        public ActionResult AddEducation(EducationModel education, HttpPostedFileBase postedFile)
        {
            ViewBag.QualificationType = _dal.GetQualificationTypeList();

            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Content("Sorry, this method can't be called only from AJAX.");
            }
       
            try
            {
                if (Convert.ToDateTime(education.endDate) <= Convert.ToDateTime(education.startDate))
                {
                    ModelState.AddModelError("", "Closing date cannot be less or equals to Today's Date");
                    TempData["message"] = "End Date Cannot Be Less or Equals to Start Date";
                }

                if (ModelState.IsValid)
                {
                    string Userid;
                    Userid = User.Identity.GetUserId();

                    int profileID = _dal.GetProfileID(User.Identity.GetUserId());
                    education.profileID = profileID;

                    _dal.AddEducation(education.institutionName, education.qualificationName, education.QualificationTypeID, education.certificateNumber, Convert.ToDateTime(education.startDate), Convert.ToDateTime(education.endDate), education.profileID);

                    if (postedFile != null && postedFile.ContentLength > 00)
                    {
                        var FileExt = System.IO.Path.GetExtension(postedFile.FileName).Substring(1);
                        byte[] bytes;

                        using (BinaryReader br = new BinaryReader(postedFile.InputStream))
                        {
                            bytes = br.ReadBytes(postedFile.ContentLength);
                        }

                        string fileName = Path.GetFileName(postedFile.FileName);
                        string ContentType = postedFile.ContentType;
                        _dal.AddAttachments(profileID, fileName, bytes, ContentType, System.IO.Path.GetExtension(postedFile.FileName));
                    }

                    //ViewBag.Education = _dal.GetQualificationByProfileID(Userid);
                    return Content(Userid.ToString());
                }
                else
                {
                    StringBuilder strB = new StringBuilder(500);
                    foreach (ModelState modelState in ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            strB.Append(error.ErrorMessage + "Closing date cannot be less or equals to Today's Date");
                        }
                    }
                    Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                    return Content(strB.ToString());
                }
            }
            catch (Exception ee)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Content(ee.Message);
            }

        }

        [HttpGet]
        public ActionResult EditEducation()
        {

            ViewBag.QualificationType = _dal.GetQualificationTypeList();
            int id = int.Parse(Request.QueryString["id"]);

            EducationModel e = new EducationModel();

            using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
            {
                var d = _db.GetEducationListByID(id).FirstOrDefault();

                e.qualificationID = Convert.ToInt32(d.qualificationID);
                e.profileID = Convert.ToInt32(d.profileID);
                e.institutionName = Convert.ToString(d.institutionName);
                e.qualificationName = Convert.ToString(d.qualificationName);
                e.QualificationTypeID = Convert.ToInt32(d.QualificationTypeID);
                e.certificateNumber = Convert.ToString(d.certificateNumber);
                e.startDate = Convert.ToDateTime(d.startDate).ToString("d");
                e.endDate = Convert.ToDateTime(d.endDate).ToString("d");

            }
            return PartialView(e);
        }

        [HttpPost]
        public ActionResult EditEducation(EducationModel model)
        {
            string id1 = User.Identity.GetUserId();

            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Content("Sorry, this method can't be called only from AJAX.");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    int qualificationID = model.qualificationID;
                    string institutionName = model.institutionName;
                    
                    string qualificationName = model.qualificationName;
                    int QualificationTypeID = model.QualificationTypeID;
                    string certificateNumber = model.certificateNumber;
                    string startDate = model.startDate;
                    string endDate = model.endDate;

                    _dal.EditEducation(Convert.ToInt16(qualificationID), institutionName, qualificationName, Convert.ToInt16(QualificationTypeID), certificateNumber, Convert.ToDateTime(startDate), Convert.ToDateTime(endDate));

                    return Content(qualificationID.ToString());
                }
                else
                {
                    StringBuilder strB = new StringBuilder(500);
                    foreach (ModelState modelState in ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            strB.Append(error.ErrorMessage + ".");
                        }
                    }
                    Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                    return Content(strB.ToString());
                }
            }
            catch (Exception ee)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Content(ee.Message);
            }
        }

        [HttpGet]
        public ActionResult EducationDelete()
        {
            int id = int.Parse(Request.QueryString["id"]);

            CandidateEducationModel e = new CandidateEducationModel();

            using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
            {
                var d = _db.GetEducationListByID(id).FirstOrDefault();

                e.qualificationID = Convert.ToInt32(d.qualificationID);
                e.profileID = Convert.ToInt32(d.profileID);
                e.institutionName = Convert.ToString(d.institutionName);
                e.qualificationName = Convert.ToString(d.qualificationName);
                e.QualificationTypeName = Convert.ToString(d.QualificationTypeName);
                e.certificateNumber = Convert.ToString(d.certificateNumber);
                e.startDate = Convert.ToDateTime(d.startDate).ToString("d");
                e.endDate = Convert.ToDateTime(d.endDate).ToString("d");

            }
            return PartialView(e);
        }

        [HttpPost]
        public ActionResult DeleteEducation()
        {
            int id = int.Parse(Request.QueryString["id"]);

            string id1 = User.Identity.GetUserId();
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Content("Sorry, this method can't be called only from AJAX.");
            }
            try
            {
                if (ModelState.IsValid)
                {
                    _dal.DeleteEducation(id);
                    return Content(Convert.ToString(id1));
                }
                else
                {
                    StringBuilder strB = new StringBuilder(500);
                    foreach (ModelState modelState in ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            strB.Append(error.ErrorMessage + ".");
                        }
                    }
                    Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                    return Content(strB.ToString());
                }
            }
            catch (Exception ee)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Content(ee.Message);
            }

        }

        [HttpGet]
        public ActionResult AddWorkHistory()
        {

            return View();
        }

        [HttpGet]
        public ActionResult getWorkHistory(string id)
        {
            id = User.Identity.GetUserId();
            ViewBag.WorkHistory = _dal.GetWorkHistoryByProfileID(id);

            if (ViewBag.WorkHistory.Count == 0)
            {
                return RedirectToAction("AddWorkHistory", "CareerCenter");
            }
            else
            {
                var p = new List<WorkHistoryModel>();
                using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
                {
                    var id1 = _dal.GetProfileID(User.Identity.GetUserId());
                    var workHistory = _db.tblWorkHistories.Where(x => x.profileID == id1).ToList();

                    foreach (var d in workHistory)
                    {
                        WorkHistoryModel e = new WorkHistoryModel();
                        e.workHistoryID = Convert.ToInt32(d.workHistoryID);
                        e.profileID = Convert.ToInt32(d.profileID);
                        e.companyName = Convert.ToString(d.companyName);
                        e.jobTitle = Convert.ToString(d.jobTitle);
                        e.positionHeld = Convert.ToString(d.positionHeld);
                        e.department = Convert.ToString(d.department);
                        e.startDate = Convert.ToDateTime(d.startDate).ToString("d");
                        e.endDate = Convert.ToDateTime(d.endDate).ToString("d");
                        p.Add(e);
                    }
                }
                return View(p);
            }
        }

        [HttpPost]
        public ActionResult AddWorkHistory(WorkHistoryModel workHistory, HttpPostedFileBase postedFile)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Content("Sorry, this method can't be called only from AJAX.");
            }
            try
            {
                //if (DateTime.Parse( workHistory.startDate) > Convert.ToDateTime( workHistory.endDate))
                //{
                //    ModelState.AddModelError("","Start Date cannot be after end date!");
                //}

                if (ModelState.IsValid)
                {
                    string Userid;
                    Userid = User.Identity.GetUserId();

                    int profileID = 0;

                    profileID = _dal.GetProfileID(User.Identity.GetUserId());
                    workHistory.profileID = profileID;

                    _dal.AddWorkHistory(workHistory.companyName, workHistory.jobTitle, workHistory.positionHeld, workHistory.department, Convert.ToDateTime(workHistory.startDate), Convert.ToDateTime(workHistory.endDate),
                                        workHistory.reasonForLeaving, workHistory.previouslyEmployedPS, workHistory.reEmployment, workHistory.previouslyEmployedDepartment, workHistory.profileID);

                    ViewBag.WorkHistory = _dal.GetWorkHistoryByProfileID(Userid);

                    if (postedFile != null && postedFile.ContentLength > 00)
                    {
                        var FileExt = System.IO.Path.GetExtension(postedFile.FileName).Substring(1);
                        byte[] bytes;

                        using (BinaryReader br = new BinaryReader(postedFile.InputStream))
                        {
                            bytes = br.ReadBytes(postedFile.ContentLength);
                        }

                        string fileName = Path.GetFileName(postedFile.FileName);
                        string ContentType = postedFile.ContentType;
                        _dal.AddAttachments(profileID, fileName, bytes, ContentType, System.IO.Path.GetExtension(postedFile.FileName));
                    }

                    //ViewBag.Education = _dal.GetQualificationByProfileID(Userid);
                    ViewBag.TabPage = "timeline";
                    return Content(Userid.ToString());
                }
                else
                {
                    StringBuilder strB = new StringBuilder(500);
                    foreach (ModelState modelState in ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            strB.Append(error.ErrorMessage + ".");
                        }
                    }
                    Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                    return Content(strB.ToString());
                }
            }
            catch (Exception ee)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Content(ee.Message);
            }
        }
        [HttpGet]
        public ActionResult EditWorkHistory()
        {
            int id = int.Parse(Request.QueryString["id"]);

            WorkHistoryModel e = new WorkHistoryModel();

            using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
            {
                var d = _db.tblWorkHistories.Where(x => x.workHistoryID == id).FirstOrDefault();

                e.workHistoryID = Convert.ToInt32(d.workHistoryID);
                e.profileID = Convert.ToInt32(d.profileID);
                e.companyName = Convert.ToString(d.companyName);
                e.jobTitle = Convert.ToString(d.jobTitle);
                e.positionHeld = Convert.ToString(d.positionHeld);
                e.department = Convert.ToString(d.department);
                e.startDate = Convert.ToDateTime(d.startDate).ToString("d");
                e.endDate = Convert.ToDateTime(d.endDate).ToString("d");

            }
            return PartialView(e);
        }

        [HttpPost]
        public ActionResult EditWorkHistory(WorkHistoryModel model)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Content("Sorry, this method can't be called only from AJAX.");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    int workHistoryID = model.workHistoryID;
                    string companyName = model.companyName;
                    string jobTitle = model.jobTitle;
                    string positionHeld = model.positionHeld;
                    string department = model.department;
                    string startDate = model.startDate;
                    string endDate = model.endDate;
                    string ReasonForLeaving = model.reasonForLeaving;
                    int PreviouslyEmployedPS = model.previouslyEmployedPS;
                    string ReEmployment = model.reEmployment;
                    string PreviouslyEmployedDepartment = model.previouslyEmployedDepartment;

                    _dal.EditWorkHistory(companyName, jobTitle, positionHeld, department, Convert.ToDateTime(startDate), endDate,
                                         ReasonForLeaving, PreviouslyEmployedPS, ReEmployment, PreviouslyEmployedDepartment, Convert.ToInt16(workHistoryID));
                    return Content(workHistoryID.ToString());
                }
                else
                {
                    StringBuilder strB = new StringBuilder(500);
                    foreach (ModelState modelState in ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            strB.Append(error.ErrorMessage + ".");
                        }
                    }
                    Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                    return Content(strB.ToString());
                }
            }
            catch (Exception ee)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Content(ee.Message);
            }
        }

        [HttpGet]
        public ActionResult WorkHistoryDelete()
        {
            int id = int.Parse(Request.QueryString["id"]);

            WorkHistoryModel e = new WorkHistoryModel();

            using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
            {
                var d = _db.tblWorkHistories.Where(x => x.workHistoryID == id).FirstOrDefault();

                e.workHistoryID = Convert.ToInt32(d.workHistoryID);
                e.profileID = Convert.ToInt32(d.profileID);
                e.companyName = Convert.ToString(d.companyName);
                e.jobTitle = Convert.ToString(d.jobTitle);
                e.positionHeld = Convert.ToString(d.positionHeld);
                e.department = Convert.ToString(d.department);
                e.startDate = Convert.ToDateTime(d.startDate).ToString("d");
                e.endDate = Convert.ToDateTime(d.endDate).ToString("d");

            }
            return PartialView(e);
        }

        [HttpPost]
        public ActionResult DeleteWorkHistory()
        {
            int id = int.Parse(Request.QueryString["id"]);

            string id1 = User.Identity.GetUserId();
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Content("Sorry, this method can't be called only from AJAX.");
            }
            try
            {
                if (ModelState.IsValid)
                {
                    _dal.DeleteWorkHistory(id);
                    return Content(Convert.ToString(id1));
                }
                else
                {
                    StringBuilder strB = new StringBuilder(500);
                    foreach (ModelState modelState in ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            strB.Append(error.ErrorMessage + ".");
                        }
                    }
                    Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                    return Content(strB.ToString());
                }
            }
            catch (Exception ee)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Content(ee.Message);
            }

        }

        [HttpGet]
        public ActionResult AddSkills()
        {
            ViewBag.SkillProficiency = _dal.GetSkillProficiencyList();
            ViewBag.Skills = _dal.GetSkillsList();

            int profileID = _dal.GetProfileID(User.Identity.GetUserId());

            ViewBag.SkillsForProfile = _dal.GetSkillsByProfileID(profileID);

            return View();

        }

        [HttpGet]
        public ActionResult getSkills(string id)
        {
            ViewBag.SkillProficiency = _dal.GetSkillProficiencyList();
            ViewBag.Skills = _dal.GetSkillsList();

            id = User.Identity.GetUserId();
            int profileID = _dal.GetProfileID(User.Identity.GetUserId());
            ViewBag.SkillsForProfile = _dal.GetSkillsByProfileID(profileID);


            if (ViewBag.SkillsForProfile.Count == 0)
            {
                return RedirectToAction("AddSkills", "CareerCenter");
            }
            else
            {
                ViewBag.SkillsForProfile = _dal.GetSkillsByProfileID(profileID);
                return View();
            }
        }


        [HttpPost]
        public ActionResult AddSkills(Skills_ProfileModel skillsProfile, HttpPostedFileBase postedFile)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Content("Sorry, this method can't be called only from AJAX.");
            }
            try
            {
                if (ModelState.IsValid)
                {
                    string Userid;
                    Userid = User.Identity.GetUserId();

                    int profileID = _dal.GetProfileID(User.Identity.GetUserId());
                    skillsProfile.profileID = profileID;

                    _dal.AddSkills(skillsProfile.profileID, skillsProfile.skillID, skillsProfile.SkillProficiencyID);

                    ViewBag.SkillsForProfile = _dal.GetSkillsByProfileID(profileID);

                    return Content(Userid.ToString());
                }
                else
                {
                    StringBuilder strB = new StringBuilder(500);
                    foreach (ModelState modelState in ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            strB.Append(error.ErrorMessage + ".");
                        }
                    }
                    Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                    return Content(strB.ToString());
                }
            }
            catch (Exception ee)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Content(ee.Message);
            }

        }
        [HttpGet]
        public ActionResult DeleteSkill_Profile()
        {
            int id = int.Parse(Request.QueryString["id"]);

            CandidateSkillsProfileModel e = new CandidateSkillsProfileModel();

            using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
            {
                var d = _db.proc_GetSkillsProficienyByID(id).FirstOrDefault();

                e.skillsProfileID = Convert.ToInt16(d.skillsProfileID);
                e.skillName = Convert.ToString(d.SkillName);
                e.SkillProficiency = Convert.ToString(d.SkillProficiency);

            }
            return PartialView(e);
        }

        [HttpPost]
        public ActionResult DeleteSkills_Profile()
        {
            int id = int.Parse(Request.QueryString["id"]);

            string id1 = User.Identity.GetUserId();
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Content("Sorry, this method can't be called only from AJAX.");
            }
            try
            {
                if (ModelState.IsValid)
                {

                    _dal.DeleteCandidateSkillsProfile(id);

                    return Content(Convert.ToString(id1));
                }
                else
                {
                    StringBuilder strB = new StringBuilder(500);
                    foreach (ModelState modelState in ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            strB.Append(error.ErrorMessage + ".");
                        }
                    }
                    Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                    return Content(strB.ToString());
                }
            }
            catch (Exception ee)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Content(ee.Message);
            }
        }



        [HttpGet]
        public ActionResult AddLanguages()
        {
            ViewBag.Language = _dal.GetLanguageList();
            ViewBag.LanguageProficiency = _dal.GetLanguageProficiencyList();

            int profileID = _dal.GetProfileID(User.Identity.GetUserId());

            ViewBag.LanguagesForProfile = _dal.GetLanguagesByProfileID(profileID);

            return View();
        }

        [HttpGet]
        public ActionResult getLanguages(string id)
        {
            ViewBag.Language = _dal.GetLanguageList();
            ViewBag.LanguageProficiency = _dal.GetLanguageProficiencyList();

            id = User.Identity.GetUserId();
            int profileID = _dal.GetProfileID(User.Identity.GetUserId());
            ViewBag.LanguagesForProfile = _dal.GetLanguagesByProfileID(profileID);

            if (ViewBag.LanguagesForProfile.Count == 0)
            {
                return RedirectToAction("AddLanguages", "CareerCenter");
            }
            else
            {
                ViewBag.LanguagesForProfile = _dal.GetLanguagesByProfileID(profileID);
                return View();
            }
        }


        [HttpPost]
        public ActionResult AddLanguages(Profile_LangauageModel profileLanguage, HttpPostedFileBase postedFile)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Content("Sorry, this method can't be called only from AJAX.");
            }
            try
            {
                if (ModelState.IsValid)
                {
                    string Userid;
                    Userid = User.Identity.GetUserId();

                    int profileID = 0;

                    profileID = _dal.GetProfileID(User.Identity.GetUserId());
                    profileLanguage.profileID = profileID;

                    _dal.AddLanguages(profileLanguage.profileID, profileLanguage.languageID, profileLanguage.LanguageProficiencyID);

                    ViewBag.LanguagesForProfile = _dal.GetLanguagesByProfileID(profileID);

                    return Content(Userid.ToString());
                }
                else
                {
                    StringBuilder strB = new StringBuilder(500);
                    foreach (ModelState modelState in ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            strB.Append(error.ErrorMessage + ".");
                        }
                    }
                    Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                    return Content(strB.ToString());
                }
            }
            catch (Exception ee)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Content(ee.Message);
            }
        }

        [HttpGet]
        public ActionResult DeleteProfil_Langauage()
        {
            int id = int.Parse(Request.QueryString["id"]);

            CandidateLanguageProfileModel e = new CandidateLanguageProfileModel();

            using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
            {
                var d = _db.GetLanguageProficienyByID(id).FirstOrDefault();


                e.profileLanguageID = Convert.ToInt16(d.profileLanguageID);
                e.LanguageName = Convert.ToString(d.LanguageName);
                e.LanguageProficiency = Convert.ToString(d.LanguageProficiency);

            }
            return PartialView(e);
        }

        [HttpPost]
        public ActionResult DeleteProfile_Langauage()
        {
            int id = int.Parse(Request.QueryString["id"]);

            string id1 = User.Identity.GetUserId();
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Content("Sorry, this method can't be called only from AJAX.");
            }
            try
            {
                if (ModelState.IsValid)
                {
                    _dal.DeleteCandidateProfileLangauage(id);
                    return Content(Convert.ToString(id1));
                }
                else
                {
                    StringBuilder strB = new StringBuilder(500);
                    foreach (ModelState modelState in ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            strB.Append(error.ErrorMessage + ".");
                        }
                    }
                    Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                    return Content(strB.ToString());
                }
            }
            catch (Exception ee)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Content(ee.Message);
            }
        }

        [HttpGet]
        public ActionResult AddReferences()
        {

            return View();
        }

        [HttpGet]
        public ActionResult getReferences(string id)
        {
            id = User.Identity.GetUserId();
            ViewBag.References = _dal.GetReferenceByProfileID(id);

            if (ViewBag.References.Count == 0)
            {
                return RedirectToAction("AddReferences", "CareerCenter");
            }
            else
            {

                var p = new List<ReferenceModel>();
                using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
                {
                    var id1 = _dal.GetProfileID(User.Identity.GetUserId());
                    var Reference = _db.tblReferences.Where(x => x.ProfileID == id1).ToList();

                    foreach (var d in Reference)
                    {
                        ReferenceModel e = new ReferenceModel();
                        e.referrenceID = Convert.ToInt32(d.ReferrenceID);
                        e.profileID = Convert.ToInt32(d.ProfileID);
                        e.refName = Convert.ToString(d.RefName);
                        e.companyName = Convert.ToString(d.CompanyName);
                        e.positionHeld = Convert.ToString(d.PositionHeld);
                        e.telNo = Convert.ToString(d.TelNo);
                        e.emailAddress = Convert.ToString(d.EmailAddress);
                        p.Add(e);
                    }
                }

                return View(p);
            }
        }

        [HttpPost]
        public ActionResult AddReferences(ReferenceModel reference, HttpPostedFileBase postedFile)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Content("Sorry, this method can't be called only from AJAX.");
            }
            try
            {
                if (ModelState.IsValid)
                {
                    string Userid;
                    Userid = User.Identity.GetUserId();

                    int profileID = _dal.GetProfileID(User.Identity.GetUserId());
                    reference.profileID = profileID;

                    _dal.AddReferences(reference.refName, reference.companyName, reference.positionHeld, reference.telNo, reference.emailAddress, reference.profileID);

                    ViewBag.References = _dal.GetReferenceByProfileID(Userid);

                    return Content(Userid.ToString());
                }
                else
                {
                    StringBuilder strB = new StringBuilder(500);
                    foreach (ModelState modelState in ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            strB.Append(error.ErrorMessage + ".");
                        }
                    }
                    Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                    return Content(strB.ToString());
                }
            }
            catch (Exception ee)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Content(ee.Message);
            }
        }

        [HttpGet]
        public ActionResult EditReference()
        {
            int id = int.Parse(Request.QueryString["id"]);

            ReferenceModel e = new ReferenceModel();

            using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
            {
                var d = _db.tblReferences.Where(x => x.ReferrenceID == id).FirstOrDefault();

                e.referrenceID = Convert.ToInt32(d.ReferrenceID);
                e.profileID = Convert.ToInt32(d.ProfileID);
                e.refName = Convert.ToString(d.RefName);
                e.companyName = Convert.ToString(d.CompanyName);
                e.positionHeld = Convert.ToString(d.PositionHeld);
                e.telNo = Convert.ToString(d.TelNo);
                e.emailAddress = Convert.ToString(d.EmailAddress);
            }
            return PartialView(e);
        }

        [HttpPost]
        public ActionResult EditReference(ReferenceModel model)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Content("Sorry, this method can't be called only from AJAX.");
            }

            try
            {
                if (ModelState.IsValid)
                {

                    int referrenceID = model.referrenceID;
                    string refName = model.refName;
                    string companyName = model.companyName;
                    string positionHeld = model.positionHeld;
                    string telNo = model.telNo;
                    string emailAddress = model.emailAddress;

                    _dal.EditReference(refName, companyName, positionHeld, telNo, emailAddress, Convert.ToInt16(referrenceID));
                    return Content(referrenceID.ToString());
                }
                else
                {
                    StringBuilder strB = new StringBuilder(500);
                    foreach (ModelState modelState in ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            strB.Append(error.ErrorMessage + ".");
                        }
                    }
                    Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                    return Content(strB.ToString());
                }
            }
            catch (Exception ee)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Content(ee.Message);
            }
        }

        [HttpGet]
        public ActionResult DeleteReferenc()
        {
            int id = int.Parse(Request.QueryString["id"]);

            ReferenceModel e = new ReferenceModel();

            using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
            {
                var d = _db.tblReferences.Where(x => x.ReferrenceID == id).FirstOrDefault();

                e.referrenceID = Convert.ToInt32(d.ReferrenceID);
                e.profileID = Convert.ToInt32(d.ProfileID);
                e.refName = Convert.ToString(d.RefName);
                e.companyName = Convert.ToString(d.CompanyName);
                e.positionHeld = Convert.ToString(d.PositionHeld);
                e.telNo = Convert.ToString(d.TelNo);
                e.emailAddress = Convert.ToString(d.EmailAddress);
            }
            return PartialView(e);
        }

        [HttpPost]
        public ActionResult DeleteReference()
        {
            int id = int.Parse(Request.QueryString["id"]);

            string id1 = User.Identity.GetUserId();
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Content("Sorry, this method can't be called only from AJAX.");
            }
            try
            {
                if (ModelState.IsValid)
                {
                    _dal.DeleteReference(id);

                    return Content(Convert.ToString(id1));
                }
                else
                {
                    StringBuilder strB = new StringBuilder(500);
                    foreach (ModelState modelState in ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            strB.Append(error.ErrorMessage + ".");
                        }
                    }
                    Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                    return Content(strB.ToString());
                }
            }
            catch (Exception ee)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Content(ee.Message);
            }
        }

        [HttpGet]
        public ActionResult AddAttachments()
        {

            return View();
        }
        [HttpGet]
        public ActionResult getAttachments(string id)
        {
            id = User.Identity.GetUserId();
            ViewBag.Attachments = _dal.GetAttachmentByProfileID(id);
            return View();
        }

        [HttpPost]
        public ActionResult AddAttachments(AttachmentModel attachment, HttpPostedFileBase postedFile)
        {
            string Userid;
            Userid = User.Identity.GetUserId();

            int profileID = 0;

            profileID = _dal.GetProfileID(User.Identity.GetUserId());
            attachment.profileID = profileID;

            if (postedFile != null && postedFile.ContentLength > 00)
            {
                var FileExt = System.IO.Path.GetExtension(postedFile.FileName).Substring(1);
                byte[] bytes;

                using (BinaryReader br = new BinaryReader(postedFile.InputStream))
                {
                    bytes = br.ReadBytes(postedFile.ContentLength);
                }

                string fileName = Path.GetFileName(postedFile.FileName);
                string ContentType = postedFile.ContentType;
                _dal.AddAttachments(attachment.profileID, fileName, bytes, ContentType, System.IO.Path.GetExtension(postedFile.FileName));
            }

            ViewBag.Attachments = _dal.GetAttachmentByProfileID(Userid);

            return RedirectToAction("MyProfile", "CareerCenter");
        }

        [HttpPost]
        public ActionResult DeleteAttachment(int id)
        {
            string id1 = User.Identity.GetUserId();
            _dal.DeleteAttachment(id);

            return Content(Convert.ToString(id1));
        }

    }
}