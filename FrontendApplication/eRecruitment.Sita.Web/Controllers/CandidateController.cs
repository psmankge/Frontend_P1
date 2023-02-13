using eRecruitment.Sita.Web.App_Data.DAL;
using eRecruitment.Sita.Web.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using eRecruitment.Sita.Web.IdClass;
using System.Text.RegularExpressions;

namespace eRecruitment.Sita.Web.Controllers
{
    public class CandidateController : Controller
    {
        eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext();
        eRecruitmentDataAccess _dal = new eRecruitmentDataAccess();
        // GET: Candidate
        public ActionResult Index()
        {
            ViewBag.Days = from a in Enumerable.Range(1, 31) select a;
            DateTime dt = Convert.ToDateTime( "2018/01/01");
            ViewBag.Month = from b in Enumerable.Range(dt.Month, 12) select b;
            //ViewBag.Year = from c in Enumerable.Range(1999,)
            return View();
        }

        public ActionResult MailBox()
        {
            string userid = User.Identity.GetUserId();
            ViewBag.EmailList = _dal.GetCandidateEmailList(userid);
            return View();
        }
        public JsonResult FileUpload(FormCollection fc, HttpPostedFileBase postedFile)
        {
            string filename = fc["attachmentName"];
   
            var data = "";
            return Json(data,JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadProfilePicture(HttpPostedFileBase postedFile)
        {
            string uid = User.Identity.GetUserId();
            if (postedFile != null && postedFile.ContentLength > 00)
            {
                var fileExt = System.IO.Path.GetExtension(postedFile.FileName).Substring(1);

                if ( fileExt != "tiff" && fileExt != "png" && fileExt != "gif" && fileExt != "jpeg" && fileExt != "jpg")
                {
                    ModelState.AddModelError("", "File Extension Is InValid - Only Upload Images");
                }
                else 
                {
                    if (postedFile != null && postedFile.ContentLength > 00)
                    {
                        var FileExt = System.IO.Path.GetExtension(postedFile.FileName).Substring(1);
                        byte[] bytes;

                        using (BinaryReader br = new BinaryReader(postedFile.InputStream))
                        {
                            bytes = br.ReadBytes(postedFile.ContentLength);
                        }
                        _dal.UpdateProfilePicture(uid, bytes);
                    }
                    else
                    {
                        _dal.UpdateProfilePicture(uid, null);
                    }
                }
            }

  

            return RedirectToAction("MyProfile", "Candidate");
        }

     
        public ActionResult MyProfile()
    {
      //ViewBag.Month = _dal.GetMonthNameList();
      //ViewBag.Bada = _dal.GetAllDaysList();
      //ViewBag.Year = _dal.GetYearNumList();
      //ViewBag.Days = from a in Enumerable.Range(1, 31) select a;
      //DateTime dt = Convert.ToDateTime("2018/01/01");
      //ViewBag.Month = from b in Enumerable.Range(dt.Month, 12) select b;

      string userid = User.Identity.GetUserId();



      var citizen = new List<CitizenshipModel>
            {
                new CitizenshipModel {CitizenID = -1, SACitizen = "Unknown", },
                new CitizenshipModel {CitizenID = 1, SACitizen = "Yes", },
                new CitizenshipModel {CitizenID = 2, SACitizen = "No", }
            };

      ViewBag.Citizenship = citizen;


      //var skillsData = from a in _db.tblSkillsProfiles
      //                 join b in _db.lutSkills on a.skillID equals b.skillID
      //                 join c in _db.tblProfiles on a.profileID equals c.pkProfileID
      //                 where c.UserID == userid
      //                 select new
      //                 {
      //                     SkillName = b.skillName

      //                 };

      ViewBag.SkillsProf = _dal.SkillsProf(userid);

      ViewBag.Race = _dal.GetRaceList();

      ViewBag.Gender = _dal.GetGenderList();
      ViewBag.Province = _dal.GetProvinceList();
      ViewBag.YesorNo = _dal.GetYesorNoList();

      ViewBag.Country = _dal.GetCountryList();
      ViewBag.Disability = _dal.GetDisabilityList();
      ViewBag.QualificationType = _dal.GetQualificationTypeList();
      ViewBag.Language = _dal.GetLanguageList();
      ViewBag.LanguageCorr = _dal.GetLanguageCorrList();
      ViewBag.LanguageProficiency = _dal.GetLanguageProficiencyList();
      ViewBag.SkillProficiency = _dal.GetSkillProficiencyList();
      ViewBag.Skills = _dal.GetSkillsList();

      //ViewBag.Education = _dal.GetQualificationByProfileID(userid);
      //ViewBag.WorkHistory = _dal.GetWorkHistoryByProfileID(userid);
      //ViewBag.References = _dal.GetReferenceByProfileID(userid);
      //ViewBag.Attachments = _dal.GetAttachmentByProfileID(userid);
      //ViewBag.SkillsForProfile = _dal.GetSkillsByProfileID(profileID);
      //ViewBag.LanguagesForProfile = _dal.GetLanguagesByProfileID(profileID);
      ViewBag.CommunicationMethod = _dal.GetMethodOfCommunication();
      ViewBag.Note = "NB: All Attached Documents Are Saved On The Attachments Tab. And must be less 5mb";

      var P = new List<ProfileModel>();
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        var d = _db.tblProfiles.Where(x => x.UserID == userid).FirstOrDefault();
        if (d != null)
        {
          ProfileModel e = new ProfileModel();
          e.IDNumber = Convert.ToString(d.IDNumber);
          e.PassportNumber = Convert.ToString(d.PassportNo);
          e.Surname = Convert.ToString(d.Surname);
          e.FirstName = Convert.ToString(d.FirstName);
          e.DateOfBirth = d.DateOfBirth;
          e.fkRaceID = Convert.ToInt32(d.fkRaceID);
          e.fkGenderID = Convert.ToInt32(d.fkGenderID);
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
          e.fkDisabilityID = Convert.ToInt32(d.fkDisabilityID);
          e.NatureOfDisability = Convert.ToInt32(d.NatureOfDisability);
          e.OtherNatureOfDisability = Convert.ToString(d.OtherNatureOfDisability);
          e.SACitizen = Convert.ToInt32(d.SACitizen);
          e.fkNationalityID = Convert.ToInt32(d.fkNationalityID);
          e.fkProvinceID = Convert.ToInt32(d.fkProvinceID);
          e.fkWorkPermitID = Convert.ToInt32(d.fkWorkPermitID);
          e.WorkPermitNo = Convert.ToString(d.WorkPermitNo);
          e.pkCriminalOffenseID = Convert.ToInt32(d.pkCriminalOffenseID);
          e.fkLanguageForCorrespondenceID = Convert.ToInt32(d.fkLanguageForCorrespondenceID);
          e.TelNoDuringWorkingHours = Convert.ToString(d.TelNoDuringWorkingHours);
          e.MethodOfCommunicationID = Convert.ToInt32(d.MethodOfCommunicationID);
          e.CorrespondanceDetails = Convert.ToString(d.CorrespondanceDetails);
          e.ProfessionallyRegisteredID = Convert.ToInt32(d.ProfessionallyRegisteredID);
          e.RegistrationDate = d.RegistrationDate;
          e.RegistrationNumber = Convert.ToString(d.RegistrationNumber);
          e.RegistrationBody = Convert.ToString(d.RegistrationBody);
          e.PreviouslyEmployedPS = Convert.ToInt32(d.PreviouslyEmployedPS);
          e.ReEmploy = Convert.ToString(d.ReEmployment);
          e.PreviouslyEmployedDepartment = Convert.ToString(d.PreviouslyEmployedDepartment);
          e.ConditionsThatPreventsReEmploymentID = Convert.ToInt32(d.ConditionsThatPreventsReEmploymentID);
          //e.DriversLicenseID = Convert.ToInt32(d.DriversLicenseID);
          //e.MatricID = Convert.ToInt32(d.MatricID);
          e.DriversLicenseID = d.DriversLicenseID;
          e.MatricID = d.MatricID;

          P.Add(e);
        }
        else
        {
          var model = new List<ProfileModel>();
          return View(model);
        }
      }

      return View(P);
    }


        [HttpPost]
        public ActionResult UpdateProfile(ProfileModel item, HttpPostedFileBase postedCV, HttpPostedFileBase postedID, FormCollection fc)
       {

            int termsAndConditions = fc["TermsAndConditions"] != null ? 1 : 0;

            //var ds = item.DateOfBirth;
            //var d = fc["item.DateOfBirth"];
            string dob = string.Empty;

            if (item.DateOfBirth == null)
            {
                DateTime date2 = DateTime.ParseExact(fc["item.DateOfBirth"], "dd/MM/yyyy", null);
                dob = String.Format("{0:MM/dd/yyyy}", date2);
            }

            ViewBag.CommunicationMethod = _dal.GetMethodOfCommunication();
            string Userid;
            Userid = User.Identity.GetUserId();

            string IDNumber = item.IDNumber;
            string Surname = item.Surname;
            string FirstName = item.FirstName;
            string DateOfBirth = string.Empty;

            if (item.DateOfBirth != null)
            {
                DateOfBirth = Convert.ToString(item.DateOfBirth);
            }
            else
            {
                DateOfBirth = dob;
            }

            
            int fkRaceID = Convert.ToInt32(item.fkRaceID);
            int fkGenderID = Convert.ToInt32(item.fkGenderID);
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
            int fkDisabilityID = Convert.ToInt32(item.fkDisabilityID);
            int NatureOfDisability = Convert.ToInt32(item.NatureOfDisability);
            string OtherNatureOfDisability = item.OtherNatureOfDisability;
            int SACitizen = Convert.ToInt32(item.SACitizen);
            int fkNationalityID = Convert.ToInt32(item.fkNationalityID);
            int fkProvinceID = Convert.ToInt32(item.fkProvinceID);
            int fkWorkPermitID = Convert.ToInt32(item.fkWorkPermitID);
            string WorkPermitNo = item.WorkPermitNo;
            int pkCriminalOffenseID = Convert.ToInt32(item.pkCriminalOffenseID);
            int fkLanguageForCorrespondenceID = Convert.ToInt32(item.fkLanguageForCorrespondenceID);
            string TelNoDuringWorkingHours = item.TelNoDuringWorkingHours;
            int MethodOfCommunicationID = Convert.ToInt32(item.MethodOfCommunicationID);
            string CorrespondanceDetails = item.CorrespondanceDetails;
            int? ProfessionallyRegisteredID = item.ProfessionallyRegisteredID;
            string RegistrationDate = Convert.ToString(item.RegistrationDate);
            string RegistrationNumber = item.RegistrationNumber;
            string RegistrationBody = item.RegistrationBody;
            int? PreviouslyEmployedPS = item.PreviouslyEmployedPS;
            string ReEmployment = item.ReEmploy;
            string PreviouslyEmployedDepartment = item.PreviouslyEmployedDepartment;
            int DriversLicenseID = Convert.ToInt32(item.DriversLicenseID);
            int MatricID = Convert.ToInt32(item.MatricID);
            int ConditionsThatPrevents = Convert.ToInt32(item.ConditionsThatPreventsReEmploymentID);

            //TempData["Message"] = "Operation successful!";
            _dal.UpdateProfileInfo(Userid, IDNumber, Surname, FirstName, DateOfBirth, (int)fkRaceID
                , fkGenderID, CellNo, AlternativeNo, EmailAddress, UnitNo, ComplexName, StreetNo, StreetName, SuburbName
                , City, PostalCode, (int)fkDisabilityID, NatureOfDisability, OtherNatureOfDisability, SACitizen
                , (int)fkNationalityID, (int)fkProvinceID, (int)fkWorkPermitID, WorkPermitNo, pkCriminalOffenseID
                , (int)fkLanguageForCorrespondenceID, TelNoDuringWorkingHours, MethodOfCommunicationID, CorrespondanceDetails
                , termsAndConditions, (int)ProfessionallyRegisteredID, Convert.ToString(RegistrationDate), RegistrationNumber, RegistrationBody
                , (int)PreviouslyEmployedPS, ReEmployment, PreviouslyEmployedDepartment, (int)DriversLicenseID, (int)MatricID, (int)ConditionsThatPrevents);

            int profileID = _dal.GetProfileID(User.Identity.GetUserId());

            if (postedCV != null && postedCV.ContentLength > 00)
            {
                var fileExt = System.IO.Path.GetExtension(postedCV.FileName).Substring(1);

               // var filesize = 60000;
                if (fileExt != "pdf" && fileExt != "doc"  && fileExt != "docx")
                {
                    //ModelState.AddModelError("", "File Extension Is InValid - Only Upload Pdf/Word File");
                    TempData["Message"] = "File Extension Is InValid - Only Upload Pdf/Word File";

                    return RedirectToAction("MyProfile", "Candidate");
                }
                //else if (postedCV.ContentLength > (filesize))
                //{
                    //ModelState.AddModelError("", "File size Should Be UpTo " + filesize + "mB");
                    //TempData["Message"] = "File size Should Be UpTo  5 MB";

                    //return RedirectToAction("MyProfile", "Candidate");
                    //ModelState.AddModelError("", "File size Should Be UpTo 5MB");
               // }
            }

            if (postedID != null && postedID.ContentLength > 00)
            {
                var fileExt = System.IO.Path.GetExtension(postedID.FileName).Substring(1);

               // var filesize = 80000;
                if (fileExt != "pdf" && fileExt != "tiff" && fileExt != "png" && fileExt != "gif" && fileExt != "jpeg" && fileExt != "jpg")
                {
                    TempData["Message"] = "File Extension Is InValid - Only Upload Pdf/Images File";

                    return RedirectToAction("MyProfile", "Candidate");
                }
              //  else if (postedID.ContentLength > (filesize))
                //{//
                    //TempData["Message"] = "File size Should Be UpTo 5 MB";

                    //return RedirectToAction("MyProfile", "Candidate");
                    //ModelState.AddModelError("", "File size Should Be UpTo " + filesize + "KB");
                    //ModelState.AddModelError("", "File size Should Be UpTo 5MB");
              //
            }

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
            TempData["Message"] = "Personal Information successfully saved.";

            return RedirectToAction("MyProfile", "Candidate");
        }

        [HttpGet]
        public JsonResult GetCandidateEducationList()
        {
            string uid = User.Identity.GetUserId();
            var data = _dal.GetCandidateQualificationByProfileID(uid);
            return Json(data,JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCandidateAttachmentList()
        {
            string uid = User.Identity.GetUserId();
            var data = _dal.GetAttachmentByProfileID(uid);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCandidateWorkHistoryForEdit(string list)
        {
            dynamic info = JsonConvert.DeserializeObject(list);
            int Wid = info.WID;
            var p = new List<WorkHistoryModel>();
            try { 
            var data = _dal.GetWorkHistoryByID(Wid);
            
            foreach (var d in data)
            {
                WorkHistoryModel e = new WorkHistoryModel();
                e.workHistoryID = Convert.ToInt32(d.workHistoryID);
                e.profileID = Convert.ToInt32(d.profileID);
                e.companyName = Convert.ToString(d.companyName);
                e.jobTitle = Convert.ToString(d.jobTitle);
                //String text = this.RemoveSpecialCharacters(d.positionHeld);
                String text = d.positionHeld;
                string positionHeld = Convert.ToString(MvcHtmlString.Create(HttpUtility.HtmlEncode(text.Replace("•", ".").Replace("&amp;","&").Replace("’", "'").Replace("–", "-").Replace("“", "\"").Replace("”", "\""))));
                e.positionHeld = this.RemoveSpecialCharacters(positionHeld);
                //e.positionHeld =  Convert.ToString(MvcHtmlString.Create(HttpUtility.HtmlEncode(text.Replace("?", "•").Replace("’", "'").Replace("“", "\"").Replace("”", "\"").Replace("–", "-"))));
                //e.positionHeld = Convert.ToString(text);
                e.department = Convert.ToString(d.department);
                e.startDate = Convert.ToDateTime(d.startDate).ToString("d");
                e.endDate = Convert.ToDateTime(d.endDate).ToString("d");
                e.reasonForLeaving = Convert.ToString(d.reasonForLeaving);
                e.previouslyEmployedPS = Convert.ToInt32(d.previouslyEmployedPS);
                e.reEmployment = Convert.ToString(d.reEmployment);
                e.previouslyEmployedDepartment = Convert.ToString(d.previouslyEmployedDepartment);
                 p.Add(e);
                }
              

            }
            catch(Exception e) { }
            return Json(p, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateWorkHistoryInfo(string list)
        {
            dynamic data = JsonConvert.DeserializeObject(list);
            string Enddate = string.Empty;

            if (data.reasonForLeaving == null)
            {
                data.reasonForLeaving = "current";
            }
            else
            {
                data.reasonForLeaving = data.reasonForLeaving;
            }

            string CompanyName = data.companyName;
            string JobTitle = data.jobTitle;
            String text = data.positionHeld;
            string positionHeld1 = Convert.ToString(MvcHtmlString.Create(HttpUtility.HtmlEncode(text.Replace("•", ".").Replace("&amp;", "&").Replace("’", "'").Replace("–", "-"))));
            string PositionHeld = this.RemoveSpecialCharacters(positionHeld1);
            //String text = this.RemoveSpecialCharacters(PositionHeld1);
            // string PositionHeld = Convert.ToString(MvcHtmlString.Create(HttpUtility.HtmlEncode(text.Replace("•", ".").Replace("’", "'").Replace("“", "\"").Replace("”", "\"").Replace("–", "-"))));
            string Department = data.department;
            //string StartDate = data.startdate.ToString("d");
            string StartDate = Convert.ToDateTime(data.startdate).AddHours(2).ToString("d");
            if (data.reasonForLeaving == "current")
            {
                Enddate = string.Empty;
            }
            else
            {
                Enddate = Convert.ToDateTime(data.enddate).AddHours(2).ToString("d");
            }
           
            string ReasonForLeaving = data.reasonForLeaving;
            int PreviouslyEmployedPS = /*data.previouslyEmployedPS*/ 0;
            string ReEmployment = /*data.reEmployment*/"NULL";
            string PreviouslyEmployedDepartment =/* data.previouslyEmployedDepartment*/"NULL";
            int id = data.WID;
            try {
            _dal.EditWorkHistory(CompanyName, JobTitle, PositionHeld, Department, Convert.ToDateTime(StartDate), Enddate,
                ReasonForLeaving, PreviouslyEmployedPS, ReEmployment, PreviouslyEmployedDepartment, id);
            }
            catch(Exception e)
            {

            }
            var work = "Work history successfully updated";
            return Json(work, JsonRequestBehavior.AllowGet); 
        }
  
        public JsonResult AddWorkHistory(string list)
        {
            dynamic info = JsonConvert.DeserializeObject(list);

            if (info.reasonForLeaving == null)
            {
                info.reasonForLeaving = "current";
            }
            else
            {
                info.reasonForLeaving = info.reasonForLeaving;
            }
            string CompanyName = info.companyName;
            string JobTitle = info.jobTitle;
            string PositionHeld1 = info.positionHeld;
            String text = info.positionHeld;
            string positionHeld1 = Convert.ToString(MvcHtmlString.Create(HttpUtility.HtmlEncode(text.Replace("•", ".").Replace("&amp;", "&").Replace("’", "'").Replace("–", "-"))));
            string PositionHeld = this.RemoveSpecialCharacters(positionHeld1);
            string Department = info.department;
            string startDate = info.startdate;
            string enddate = info.enddate;
            string ReasonForLeaving = info.reasonForLeaving;
            int PreviouslyEmployedPS = /*info.previouslyEmployedPS*/0;
            string ReEmployment = /*info.reEmployment*/"NULL";
            string PreviouslyEmployedDepartment = /*info.previouslyEmployedDepartment*/"NULL";
            int profileID = _dal.GetProfileID(User.Identity.GetUserId());

            _dal.AddWorkHistory(CompanyName, JobTitle, PositionHeld, Department, Convert.ToDateTime(startDate), Convert.ToDateTime(enddate)
                                ,ReasonForLeaving, PreviouslyEmployedPS, ReEmployment, PreviouslyEmployedDepartment, profileID);

            var data = "Work history successfully saved.";
            TempData["Message"] = "Profile successfully saved.";
            return Json(data, JsonRequestBehavior.AllowGet); ;
        }
        private string RemoveSpecialCharacters(string value)
        {
            //return Regex.Replace(value, @"[^0-9A-Za-z ,]", " ").Replace("&#61623", ".").Replace(" 61623", ".").Replace("&#61553", ".").Replace(" 61553", ".").Replace(" ", " ").Trim();
            //return Regex.Replace(value, @"[^0-9A-Za-z ,]", ".").Replace("\u0095", ".").Replace("\u0092", "'").Trim();
            return value.Replace("\u0095", ".").Replace("\u0092", "'").Replace("\u0096", "-").Replace("?", ".").Replace("&amp;", "&").Replace("amp;amp;", "&").Replace("amp;amp;amp;#39;", "'").Replace("&#39;", "'").Replace("&quot;", "\"").Replace("&lt", "<").Replace("&gt", ">").Trim();



        }

        //private string RemoveSpecialCharacters(string value) 
        // {

        //     var data = _db.SpecialCharacters.ToList(); 
        //     foreach (var d in data) 
        //     {
        //         //value = Regex.Replace(value, d.KeyAscii, d.KeyAsciiReplacement, RegexOptions.IgnoreCase);
        //      
        //         //value = Convert.ToString(MvcHtmlString.Create(HttpUtility.HtmlEncode(Regex.Replace(value, d.SC_Unicode, d.SC_UnicodeReplacementValue, RegexOptions.IgnoreCase))));
        //     } 
        //  return value.ToString(); 

        // }


        public JsonResult GetCandidateWorkHistoryList()
        {
            string uid = User.Identity.GetUserId();
            var data = _dal.GetWorkHistoryByProfileID(uid);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateEducation(string list)
        {
            dynamic info = JsonConvert.DeserializeObject(list);
            string institutionName = info.IName;
            string qualificationName = info.QName;
            string OtherEdu = info.OName;
            int qTypeID = info.QTypeID;
            string certificateNumber = info.CNumber;
            //string startDate = info.startdate.ToString("d");
            //string enddate = info.enddate.ToString("d");
            string startDate = Convert.ToDateTime(info.startdate).AddHours(2).ToString("d");
            string enddate = Convert.ToDateTime(info.enddate).AddHours(2).ToString("d");
            int qid = info.QID;

            _dal.EditEducation((int)Convert.ToInt64(qid), institutionName, qualificationName, Convert.ToInt32(qTypeID), certificateNumber, Convert.ToDateTime(startDate), Convert.ToDateTime(enddate));

            var data = "Education successfully Updated";
            return Json(data, JsonRequestBehavior.AllowGet); ;
        }

        public JsonResult UpdateReferenceInfo(string list)
        {
            dynamic data = JsonConvert.DeserializeObject(list);
            string refName = data.refName;
            string companyName = data.companyName;
            string positionHeld = data.positionHeld;
            string telNo = data.telNo;
            string emailAddress = data.emailAddress;
            int qid = data.RID;

            _dal.EditReference(refName, companyName, positionHeld, telNo, emailAddress, qid);

            var res = "OK";
            return Json(res, JsonRequestBehavior.AllowGet); ;
        }

        public JsonResult DeleteReferenceInfo(string list)
        {
            dynamic data = JsonConvert.DeserializeObject(list);
            int qid = data.RID;
            _dal.DeleteReference(qid);
            var res = "OK";
            return Json(res, JsonRequestBehavior.AllowGet); ;
        }

        public JsonResult DeleteEducationInfo(string list)
        {
            dynamic data = JsonConvert.DeserializeObject(list);
            int qid = data.QID;
            _dal.DeleteEducation(qid);
            var res = "OK";
            return Json(res, JsonRequestBehavior.AllowGet); ;
        }

        public JsonResult DeleteCandidateWorkHistory(string list)
        {
            dynamic data = JsonConvert.DeserializeObject(list);
            int qid = data.QID;
            _dal.DeleteWorkHistory(qid);
            var res = "OK";
            return Json(res, JsonRequestBehavior.AllowGet); ;
        }

        public JsonResult DeleteCandidateSkillsByID(string list)
        {
            dynamic data = JsonConvert.DeserializeObject(list);
            int qid = data.QID;
            _dal.DeleteCandidateSkillsProfile(qid);
            var res = "OK";
            return Json(res, JsonRequestBehavior.AllowGet); ;
        }

        public JsonResult DeleteCandidateLanguageByID(string list)
        {
            dynamic data = JsonConvert.DeserializeObject(list);
            int qid = data.QID;
            _dal.DeleteCandidateProfileLangauage(qid);
            var res = "OK";
            return Json(res, JsonRequestBehavior.AllowGet); ;
        }

        public JsonResult AddLanguage(string list)
        {
            dynamic info = JsonConvert.DeserializeObject(list);
            int LanguageID = info.LanguageID;
            int LanguageProficiencyID = info.LanguageProficiencyID;
            int profileID = _dal.GetProfileID(User.Identity.GetUserId());

            var data1 = _db.tblProfileLangauages.Where(x => x.languageID == LanguageID &&  x.profileID == profileID).Count();

            if (data1 > 0)
            {
                var data = "Warning";
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                _dal.AddLanguages(profileID, LanguageID, LanguageProficiencyID);
                var data = "Languages successfully saved.";
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetLanguageProficiencyList()
        {
            var data = _dal.GetLanguageProficiencyList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetLanguagesList()
        {
            var data = _dal.GetLanguageList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCandidateLanguagesList()
        {
            string uid = User.Identity.GetUserId();
            int profileID = _dal.GetProfileID(uid);
            var data = _dal.GetLanguagesByProfileID(profileID);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCandidateSkillsList()
        {
            string uid = User.Identity.GetUserId();
            int profileID = _dal.GetProfileID(uid);
            var data = _dal.GetSkillsByProfileID(profileID);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddSkills(string list)
        {
            dynamic info = JsonConvert.DeserializeObject(list);
            int SkillID = info.skillID;
            int SkillProficiencyID = info.SkillProficiencyID;

            int profileID = _dal.GetProfileID(User.Identity.GetUserId());


            var data1 = _db.tblSkillsProfiles.Where(x => x.skillID == SkillID && x.profileID == profileID).Count();

            if (data1 > 0)
            {
                var data = "Warning";
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                _dal.AddSkills(profileID, SkillID, SkillProficiencyID);

                string userid = User.Identity.GetUserId();
              
                ViewBag.SkillsProf = _dal.SkillsProf(userid);

                var data = "Skills successfully saved";
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SkillsTypeList()
        {
            var data = _dal.GetSkillsList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SkillProficiencyList()
        {
            var data = _dal.GetSkillProficiencyList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public FileResult DownloadAttachment(string list)
        {
            dynamic d = JsonConvert.DeserializeObject(list);
            int id = d.AID;
            var doc = _db.Attachments.Where(x => x.AttachmentID == id).FirstOrDefault();
            return File(doc.fileData.ToArray(), doc.contentType, doc.fileName);
        }

        public JsonResult AddReferenceInfo(string list)
        { 
            
            //{ "refName":"xcxcvxcxc","companyName":"xccxcxcx","positionHeld":"cxcxcxcxcx","telNo":"0123652843","emailAddress":"info@naro.co.za"}
            dynamic info = JsonConvert.DeserializeObject(list);
            string refName = info.refName;
            string companyName = info.companyName;
            string positionHeld = info.positionHeld;
            string telNo = info.telNo;
            string emailAddress = info.emailAddress;
            int profileID = _dal.GetProfileID(User.Identity.GetUserId());

            _dal.AddReferences( refName,companyName, positionHeld, telNo,emailAddress,profileID);

            var data = "Reference successfully saved.";
            return Json(data, JsonRequestBehavior.AllowGet); ;

        }

        public JsonResult AddEducation(string list)
        {
            dynamic info = JsonConvert.DeserializeObject(list);
            string institutionName = info.IName;
            string qualificationName = info.QName;
           
            int qTypeID = info.QTypeID;
            string certificateNumber = info.CNumber;
            string startDate = info.startdate.ToString("d");
            string enddate = info.enddate.ToString("d");
            int profileID = _dal.GetProfileID(User.Identity.GetUserId());

            _dal.AddEducation(institutionName, qualificationName,  Convert.ToInt32(qTypeID), certificateNumber
                , Convert.ToDateTime(startDate), Convert.ToDateTime(enddate), profileID);

            var data = "Education successfully saved.";
            //TempData["Message"] = "Profile successfully saved.";
            return Json(data, JsonRequestBehavior.AllowGet); ;
        }

        public JsonResult GetCandidateEducationForEdit(string list)
        {
            dynamic info = JsonConvert.DeserializeObject(list);
            int qid = info.QID;
            var data = _dal.GetCandidateEducationForEditByID(qid);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetCandidateReferenceForEdit(string list)
        {
            dynamic info = JsonConvert.DeserializeObject(list);
            int rid = info.RID;
            var data = _dal.GetReferenceByID(rid);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCandidateReferenceList()
        {
            string uid = User.Identity.GetUserId();
            var data = _dal.GetReferenceByProfileID(uid);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetQualificationTypeList()
        {
            var data = _dal.GetQualificationTypeList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDaysList()
        {
            var data = _dal.GetAllDaysList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMonthList()
        {
            var data = _dal.GetMonthNameList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetYearList()
        {
            var data = _dal.GetYearNumList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportToExel(FormCollection fc, string[] VacancyQuestionID)
        {
            //if (VacancyQuestionID == null)
            //{
            //    ViewBag.ErrorMessage = "You must select one of the Question Banks provided!";
            //    ModelState.AddModelError("", "You must select one of the Question Banks provided");
            //}

            string VacancyQuestionBank = null;
            string vacancyID = fc["VacancyID"];
            string provinceID = fc["ProvinceID"];
            int ageRange = int.Parse(fc["AgeRange"]);
            string genderID = fc["GenderID"];
            string raceID = fc["RaceID"];
            int attachedCV = fc["chkWithAttachedCV"] != null ? 1 : 0;
            int attachedID = fc["chkWithAttachedID"] != null ? 1 : 0;
            int withDisabilities = fc["chkWithDisabilities"] != null ? 1 : 0;

            if (VacancyQuestionID != null)
            {
                VacancyQuestionBank = string.Join(";", VacancyQuestionID);
            }
            else
            {
                VacancyQuestionBank = null;
            }

            if (ModelState.IsValid)
            {

            }
            _dal.GetScreenedCandidateList(vacancyID, provinceID, genderID, raceID, withDisabilities, attachedCV, attachedID, ageRange, VacancyQuestionBank);

        
            int id = int.Parse(vacancyID);
			
			var vName = (from a in _db.tblVacancies
                         join b in _db.tblVacancyProfiles on a.VacancyProfileID equals b.VacancyProfileID
                         where a.ID == id
                         select new
                         {
                             VacancyName = b.VacancyName + " - (" + a.ReferenceNo + ")",
                         }).FirstOrDefault();
     


            string provinceName = null;
            string AgeRange = null;
            string gender = null;
            string race = null;
            string AttachedCV = null;
            string AttachedID = null;
            string WithDisabilities = null; 

            //var vName = _db.tblVacancies.Where(x => x.ID == vid).FirstOrDefault();
            //string VancancyName = vName.VacancyName + " - ( " + vName.ReferenceNo + ")";

            switch (provinceID)
            {
                case "10":
                    provinceName = "All";
                    break;
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                    int pid = int.Parse(provinceID);
                    provinceName = _db.lutProvinces.Where(x => x.ProvinceID == pid).Select(x => x.ProvinceName).FirstOrDefault();
                    break;
                default:
                    break;
            }

            switch (genderID)
            {
                case "0":
                    gender = "All";
                    break;
                case "1":
                case "2":
                    int pid = int.Parse(genderID);
                    gender = _db.lutGenders.Where(x => x.GenderID == pid).Select(x => x.Gender).FirstOrDefault();
                    break;
                default:
                    break;
            }

            switch (raceID)
            {
                case "0":
                    race = "All";
                    break;
                case "1":
                case "2":
                    int pid = int.Parse(raceID);
                    race = _db.lutRaces.Where(x => x.RaceID == pid).Select(x => x.RaceName).FirstOrDefault();
                    break;
                default:
                    break;
            }

            switch (ageRange)
            {
                case 0:
                    AgeRange = "Any";
                    break;
                case 1:
                    AgeRange = "18 - 25";
                    break;
                case 2:
                    AgeRange = "25 - 35";
                    break;
                default:
                    break;
            }

            switch (attachedCV)
            {
                case 0:
                    AttachedCV = "No";
                    break;
                case 1:
                    AttachedCV = "Yes";
                    break;
                default:
                    break;
            }

            switch (attachedID)
            {
                case 0:
                    AttachedID = "No";
                    break;
                case 1:
                    AttachedID = "Yes";
                    break;
                default:
                    break;
            }


            switch (withDisabilities)
            {
                case 0:
                    WithDisabilities = "No";
                    break;
                case 1:
                    WithDisabilities = "Yes";
                    break;
                default:
                    break;

            }

            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("CandidateList");


            //ws.Cell(6, 1).Value = "Application";

         
            ws.Cell(3, 2).Value = "Gender";
            ws.Cell(3, 3).Value = "Race";
            ws.Cell(3, 4).Value = "Province";
            ws.Cell(3, 5).Value = "Age Range";
            ws.Cell(3, 6).Value = "Disability";
            ws.Cell(3, 7).Value = "Attached CV";
            ws.Cell(3, 8).Value = "Attached ID";

            ws.Cell(4, 1).Value = "Selected Criteria";

            //Insert
            ws.Cell(2, 2).Value = vName.VacancyName;
            ws.Cell(4, 2).Value = gender;
            ws.Cell(4, 3).Value = race;
            ws.Cell(4, 4).Value = provinceName;
            ws.Cell(4, 5).Value = AgeRange;
            ws.Cell(4, 6).Value = WithDisabilities;
            ws.Cell(4, 7).Value = AttachedCV;
            ws.Cell(4, 8).Value = AttachedID;
            //*****************************************************************


            ws.Cell(7, 1).Value = "ID Number";
            ws.Cell(7, 2).Value = "Surname";
            ws.Cell(7, 3).Value = "Name";
            ws.Cell(7, 4).Value = "Date Of Birth";
            ws.Cell(7, 5).Value = "Contact Number";
            ws.Cell(7, 6).Value = "Email";
            ws.Cell(7, 7).Value = "Race";
            ws.Cell(7, 8).Value = "Gender";
            ws.Range(6, 1, 6, 8).Merge().AddToNamed("Titles");
            ws.Range(2, 2, 2, 8).Merge().AddToNamed("Titles");
            ws.Range(7, 1, 7, 8).AddToNamed("Titles");
            ws.Range(3, 1, 3, 8).AddToNamed("Titles");


            // From a Database
            var data = _dal.GetCandidateListForExport(id);
            var rangeWithData = ws.Cell(8, 1).InsertData(data);

            // Prepare the style for the titles
            var titlesStyle = wb.Style;
            titlesStyle.Font.Bold = true;
            titlesStyle.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            titlesStyle.Fill.BackgroundColor = XLColor.LightBlue;

            // Format all titles in one shot
            wb.NamedRanges.NamedRange("Titles").Ranges.Style = titlesStyle;

            ws.Columns().AdjustToContents();

            // wb.SaveAs("InsertingData.xlsx");

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=\"Screened_Candidates_" + DateTime.Now.ToShortDateString() + ".xlsx\"");

            // Flush the workbook to the Response.OutputStream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                wb.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                memoryStream.Close();
            }

            Response.End();
            return View();
        }

        [HttpGet]
        public FileResult DownLoadFile(int id)
        {
            var doc = _db.Attachments.Where(x => x.AttachmentID == id).FirstOrDefault();
            return File(doc.fileData.ToArray(), doc.contentType, doc.fileName);
        }

    }
}