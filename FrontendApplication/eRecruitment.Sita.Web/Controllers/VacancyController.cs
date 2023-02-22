using eRecruitment.Sita.Web.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace eRecruitment.Sita.Web.Controllers
{

  
    public class VacancyController : Controller
    {
        eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext();
        eRecruitmentDataAccess _dal = new eRecruitmentDataAccess();
        //eRecruitment.Sita.Web.Notifications.SendNotification notify = new eRecruitment.Sita.Web.Notifications.SendNotification();
        Notification notify = new Notification();
            
        // GET: Vacancy
        [Authorize]
        public ActionResult Index()
        {
            string userid = User.Identity.GetUserId();
            ViewBag.VacancyList = _dal.GetVacancyListByUser(userid);
            ViewBag.RetractList = _dal.GetRetractReasonList();
            ViewBag.WithdrawalList = _dal.GetWithdrawalReasonList();

            return View();
        }

        public JsonResult GetVacancyQuestionBank(string list)
        {
            try
            {
                dynamic rec = JsonConvert.DeserializeObject(list);
                int vid = Convert.ToInt16(rec.VID);
                var data = _dal.GetVacancyQuestionBanksListByID(Convert.ToInt16(vid));
                ViewBag.VacancyQuetionBanks = data.ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message, results = "" }, JsonRequestBehavior.AllowGet);
            }

        }
     
        public JsonResult GetSalaryRangeByID(string vid)
        {
            try
            {
                //dynamic data = JsonConvert.DeserializeObject(list);
                //int PID = Convert.ToInt16(data);
                //var data = _dal.GetVacancyProfile(Convert.ToInt16(vid));
                var data = _dal.GetSalaryRange(Convert.ToInt16(vid));
                return Json(data, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message, results = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        //get all Approve Vacancy
        [Authorize]
        public ActionResult ApproveVacancy()
        {
            string userid = User.Identity.GetUserId();
            ViewBag.ApprovedVacancy = _dal.GetApprovedVacancyList(userid);
            return View();
        }
        //Download Vacancy Ad
        public ActionResult DownloadVacancyAd(int id)
        {
            var data = _db.sp_GetVacancyAdDetail(id).FirstOrDefault();

            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {

                Document document = new Document(PageSize.A4, 10, 10, 10, 10);
                //Document document = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
                Font courier = new Font(Font.FontFamily.HELVETICA, 9f);

                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();
                //document.Add(new Paragraph("\n"));

                //var logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Content/dist/img/sita.png"));

                var logo = Image.GetInstance((byte[])data.FileData.ToArray());
                logo.Alignment = Element.ALIGN_CENTER;
                logo.ScaleToFit(120f, 80f);

                document.Add(logo);
                //document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());

                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL);

                ////Insert Image
                //string imagepath = Server.MapPath("img");
                //Image gif = Image.GetInstance(imagepath + "/eclogoAIM.png");
                //gif.Alignment = Element.ALIGN_LEFT;
                ////Resize image depend upon your need
                //gif.ScaleToFit(250f, 200f);
                ////Give space before image
                //gif.SpacingBefore = 10f;
                ////Give some space after the image
                //gif.SpacingAfter = 1f;
                //document.Add(gif);
                ////End Image

                Paragraph paraHead = new Paragraph("VACANCY ADVERTISEMENT");
                paraHead.Alignment = Element.ALIGN_CENTER;
                paraHead.Font = FontFactory.GetFont("dax-black", 10, Font.BOLD);
                //cellVacancyPurpose.BackgroundColor = BaseColor.LIGHT_GRAY;

                document.Add(paraHead);
                document.Add(new Paragraph("\n"));

                PdfPTable table = new PdfPTable(2);

                //PdfPCell cell = new PdfPCell(new Phrase("Vacancy Information Download"));
                PdfPCell cell = new PdfPCell(new Phrase("Vacancy Information Download",font));
                cell.Colspan = 2;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.Border = 0; //Added this for testing
                table.AddCell(cell);

                PdfPCell emptyCell = new PdfPCell(new Phrase(" ",font));
                emptyCell.Colspan = 2;
                emptyCell.Border = 0;
                table.AddCell(emptyCell);

                //Reference Number
                //table.AddCell("Reference Number:");
                //table.AddCell(data.ReferenceNo);

                PdfPCell cellReferenceNo = new PdfPCell(new Phrase("Reference Number:", font));
                cellReferenceNo.Colspan = 1;
                cellReferenceNo.HorizontalAlignment = Element.ALIGN_LEFT;
                //cellReferenceNo.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cellReferenceNo);

                PdfPCell cellReferenceNoContent = new PdfPCell(new Phrase(data.ReferenceNo.ToString(), font));
                cellReferenceNoContent.Colspan = 1;
                cellReferenceNoContent.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                table.AddCell(cellReferenceNoContent);

                PdfPCell cellBPSNo = new PdfPCell(new Phrase("BPS Vacancy Number:", font));
                cellBPSNo.Colspan = 1;
                cellBPSNo.HorizontalAlignment = Element.ALIGN_LEFT;
                //cellReferenceNo.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cellBPSNo);

                PdfPCell cellBPSNoContent = new PdfPCell(new Phrase(data.VacancyNo.ToString(), font));
                cellBPSNoContent.Colspan = 1;
                cellBPSNoContent.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                table.AddCell(cellBPSNoContent);


                //Job Title
                PdfPCell cellJobTitle = new PdfPCell(new Phrase("Job Title:", font));
                cellJobTitle.Colspan = 1;
                cellJobTitle.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cellJobTitle);

                PdfPCell cellJobTitleContent = new PdfPCell(new Phrase(data.JobTitle.ToString(), font));
                cellJobTitleContent.Colspan = 1;
                cellJobTitleContent.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                table.AddCell(cellJobTitleContent);


                //Job Level
                PdfPCell cellJobLevel = new PdfPCell(new Phrase("Job Level:", font));
                cellJobLevel.Colspan = 1;
                cellJobLevel.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cellJobLevel);

                PdfPCell cellJobLevelContent = new PdfPCell(new Phrase(data.JobLevel.ToString(), font));
                cellJobLevelContent.Colspan = 1;
                cellJobLevelContent.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                table.AddCell(cellJobLevelContent);


                //Vacancy Type
                PdfPCell cellVacancyType = new PdfPCell(new Phrase("Vacancy Type:", font));
                cellVacancyType.Colspan = 1;
                cellVacancyType.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cellVacancyType);

                PdfPCell cellVacancyTypeContent = new PdfPCell(new Phrase(data.VacancyTypeName.ToString(), font));
                cellVacancyTypeContent.Colspan = 1;
                cellVacancyTypeContent.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                table.AddCell(cellVacancyTypeContent);

                //Salary Range
                PdfPCell cellSalaryRange = new PdfPCell(new Phrase("Salary Range:", font));
                cellSalaryRange.Colspan = 1;
                cellSalaryRange.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cellSalaryRange);

                PdfPCell cellSalaryRangeContent = new PdfPCell(new Phrase(data.Salary.ToString(), font));
                cellSalaryRangeContent.Colspan = 1;
                cellSalaryRangeContent.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                table.AddCell(cellSalaryRangeContent);

                //Organisation
                PdfPCell cellOrganisationName = new PdfPCell(new Phrase("Organisation Name:", font));
                cellOrganisationName.Colspan = 1;
                cellOrganisationName.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cellOrganisationName);

                PdfPCell cellOrganisationNameContent = new PdfPCell(new Phrase(data.OrganisationName.ToString(), font));
                cellOrganisationNameContent.Colspan = 1;
                cellOrganisationNameContent.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                table.AddCell(cellOrganisationNameContent);

                //table.AddCell("Report To: ");
                //table.AddCell(data.Manager);

                //Division
                PdfPCell cellDivisionName = new PdfPCell(new Phrase("Division:", font));
                cellDivisionName.Colspan = 1;
                cellDivisionName.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cellDivisionName);

                PdfPCell cellDivisionNameContent = new PdfPCell(new Phrase(data.DivisionName.ToString(), font));
                cellDivisionNameContent.Colspan = 1;
                cellDivisionNameContent.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                table.AddCell(cellDivisionNameContent);

                //Department
                PdfPCell cellDepartmentName = new PdfPCell(new Phrase("Department:", font));
                cellDepartmentName.Colspan = 1;
                cellDepartmentName.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cellDepartmentName);

                PdfPCell cellDepartmentNameContent = new PdfPCell(new Phrase(data.DepartmentName.ToString(), font));
                cellDepartmentNameContent.Colspan = 1;
                cellDepartmentNameContent.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                table.AddCell(cellDepartmentNameContent);

                //Employment Type
                PdfPCell cellEmploymentType = new PdfPCell(new Phrase("Employment Type:", font));
                cellEmploymentType.Colspan = 1;
                cellEmploymentType.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cellEmploymentType);

                PdfPCell cellEmploymentTypeContent = new PdfPCell(new Phrase(data.EmploymentType.ToString(), font));
                cellEmploymentTypeContent.Colspan = 1;
                cellEmploymentTypeContent.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                table.AddCell(cellEmploymentTypeContent);


                //Contract Duiration
                if (data.EmploymentType != null)
                {
                    if (data.EmploymentType != "Permanent")
                    {
                        PdfPCell cellContractDuration = new PdfPCell(new Phrase("Contract Duration:", font));
                        cellContractDuration.Colspan = 1;
                        cellContractDuration.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.AddCell(cellContractDuration);

                        string contractDuration = string.Empty;
                        if (data.ContractDuration != null) { contractDuration = data.ContractDuration.ToString(); } else { contractDuration = string.Empty; }

                        PdfPCell cellContractDurationContent = new PdfPCell(new Phrase(contractDuration, font));
                        cellContractDurationContent.Colspan = 1;
                        cellContractDurationContent.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                        table.AddCell(cellContractDurationContent);
                    }
                }

                //Location
                PdfPCell cellLocation = new PdfPCell(new Phrase("Location:", font));
                cellLocation.Colspan = 1;
                cellLocation.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cellLocation);

                PdfPCell cellLocationContent = new PdfPCell(new Phrase(data.Location.ToString(), font));
                cellLocationContent.Colspan = 1;
                cellLocationContent.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                table.AddCell(cellLocationContent);

                //table.AddCell("Recruiter: ");
                //table.AddCell(data.Recruiter);

                //table.AddCell("Reply To: ");
                //table.AddCell(data.ReplyTo);

                //Number or Opening
                PdfPCell cellNumberOfOpenings = new PdfPCell(new Phrase("Number Of Openings:", font));
                cellNumberOfOpenings.Colspan = 1;
                cellNumberOfOpenings.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cellNumberOfOpenings);

                PdfPCell cellNumberOfOpeningsContent = new PdfPCell(new Phrase(data.NumberOfOpenings.ToString(), font));
                cellNumberOfOpeningsContent.Colspan = 1;
                cellNumberOfOpeningsContent.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                table.AddCell(cellNumberOfOpeningsContent);

                table.AddCell(emptyCell);

                //Vacancy Purpose
                PdfPCell cellVacancyPurpose = new PdfPCell(new Phrase("Purpose of Job:",font));
                cellVacancyPurpose.Colspan = 2;
                cellVacancyPurpose.HorizontalAlignment = Element.ALIGN_LEFT;
                cellVacancyPurpose.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cellVacancyPurpose);

                PdfPCell cellVacancyPurposeContent = new PdfPCell(new Phrase(data.VacancyPurpose,font));
                cellVacancyPurposeContent.Colspan = 2;
                cellVacancyPurposeContent.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                table.AddCell(cellVacancyPurposeContent);
                //End Vacancy Purpose

                table.AddCell(emptyCell);

                //Responsibilities
                PdfPCell cellResponsibilities = new PdfPCell(new Phrase("Responsibilities:", font));
                cellResponsibilities.Colspan = 2;
                cellResponsibilities.HorizontalAlignment = Element.ALIGN_LEFT;
                cellResponsibilities.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cellResponsibilities);

                PdfPCell cellResponsibilitiesContent = new PdfPCell(new Phrase(data.Responsibility, font));
                cellResponsibilitiesContent.Colspan = 2;
                cellResponsibilitiesContent.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                table.AddCell(cellResponsibilitiesContent);
                //End Responsibilities

                //table.AddCell("Responsibilities: ");
                //table.AddCell(data.Responsibility);

                table.AddCell(emptyCell);

                //Qualifications and Experience
                PdfPCell cellQualificationsandExperience = new PdfPCell(new Phrase("Qualifications and Experience:",font));
                cellQualificationsandExperience.Colspan = 2;
                cellQualificationsandExperience.HorizontalAlignment = Element.ALIGN_LEFT;
                cellQualificationsandExperience.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cellQualificationsandExperience);

                PdfPCell cellQualificationsandExperienceContent = new PdfPCell(new Phrase(data.QualificationsandExperience,font));
                cellQualificationsandExperienceContent.Colspan = 2;
                cellQualificationsandExperienceContent.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                table.AddCell(cellQualificationsandExperienceContent);

                //table.AddCell("Qualifications and Experience: ");
                //table.AddCell(data.QualificationsandExperience);
                //End Qualifications and Experience
                table.AddCell(emptyCell);

                //Knowledge
                PdfPCell cellKnowledge = new PdfPCell(new Phrase("Knowledge:", font));
                cellKnowledge.Colspan = 2;
                cellKnowledge.HorizontalAlignment = Element.ALIGN_LEFT;
                cellKnowledge.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cellKnowledge);

                PdfPCell cellKnowledgeContent = new PdfPCell(new Phrase(data.Knowledge,font));
                cellKnowledgeContent.Colspan = 2;
                cellKnowledgeContent.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                table.AddCell(cellKnowledgeContent);
                //table.AddCell("Knowledge: ");
                //table.AddCell(data.Knowledge);
                //End Knowledge


                table.AddCell(emptyCell);
                //Technical Competency
                //StringBuilder technicalCompBuilder = new StringBuilder();
                //int technicalCompCounter = 0;
                //var technicalComp = _dal.GetTechnicalCompList((int)data.JobProfileID);

                //foreach (var item in technicalComp)
                //{
                //    technicalCompCounter += 1;
                //    technicalCompBuilder.AppendLine(technicalCompCounter.ToString() + ". " + item.TechnicalComp + " - " + item.TechnicalCompDesc);
                //}

                PdfPCell cellTechnicalComp = new PdfPCell(new Phrase("Technical Competencies",font));
                cellTechnicalComp.Colspan = 2;
                cellTechnicalComp.HorizontalAlignment = Element.ALIGN_LEFT;
                cellTechnicalComp.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cellTechnicalComp);

                //PdfPCell cellTechnicalCompContent = new PdfPCell(new Phrase(technicalCompBuilder.ToString(),font));
                string tcomp = string.Empty;
                if (data.TechnicalCompetenciesDescription != null) { tcomp = data.TechnicalCompetenciesDescription.ToString(); } else { tcomp = string.Empty; }
                PdfPCell cellTechnicalCompContent = new PdfPCell(new Phrase(tcomp, font));
                cellTechnicalCompContent.Colspan = 2;
                cellTechnicalCompContent.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                table.AddCell(cellTechnicalCompContent);

                //Leadership Competency
                if (data.JobLevelID >= 17) //From the tblJob Level We sorted the Job Level with ID and 17 is the D4 and Above
                {
                    //StringBuilder leadershipCompBuilder = new StringBuilder();
                    //int leadershipCompCounter = 0;
                    //var leadershipComp = _dal.GetLeadershipCompList((int)data.JobProfileID);

                    //foreach (var item in leadershipComp)
                    //{
                    //    leadershipCompCounter += 1;
                    //    leadershipCompBuilder.AppendLine(leadershipCompCounter.ToString() + ". " + item.LeadershipComp + " - " + item.LeadershipCompDesc);
                    //}

                    PdfPCell cellLeadershipComp = new PdfPCell(new Phrase("Leadership Competencies",font));
                    cellLeadershipComp.Colspan = 2;
                    cellLeadershipComp.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellLeadershipComp.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cellLeadershipComp);

                    //PdfPCell cellLeadershipCompContent = new PdfPCell(new Phrase(leadershipCompBuilder.ToString(),font));
                    string lcomp = string.Empty;
                    if (data.LeadershipCompetencies != null) { lcomp = data.LeadershipCompetencies.ToString(); } else { lcomp = string.Empty; }
                    PdfPCell cellLeadershipCompContent = new PdfPCell(new Phrase(lcomp.ToString(), font));
                    cellLeadershipCompContent.Colspan = 2;
                    cellLeadershipCompContent.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    table.AddCell(cellLeadershipCompContent);
                }

                //Behavioural Competency
                ////StringBuilder behaviouralCompBuilder = new StringBuilder();
                ////int behaviouralCompCounter = 0;
                //var behaviouralComp = _dal.GetBehaviouralCompList((int)data.JobProfileID);

                //foreach (var item in behaviouralComp)
                //{
                //    behaviouralCompCounter += 1;
                //    behaviouralCompBuilder.AppendLine(behaviouralCompCounter.ToString() + ". " + item.BehaviouralComp + " - " + item.BehaviouralCompDesc);
                //}

                table.AddCell(emptyCell);
                PdfPCell cellBehaviouralComp = new PdfPCell(new Phrase("Interpersonal and Behavioural Competencies",font));
                cellBehaviouralComp.Colspan = 2;
                cellBehaviouralComp.HorizontalAlignment = Element.ALIGN_LEFT;
                cellBehaviouralComp.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cellBehaviouralComp);

                string bcomp = string.Empty;
                if (data.BehaviouralCompetency != null) { bcomp = data.BehaviouralCompetency.ToString(); } else { bcomp = string.Empty; }

                PdfPCell cellBehaviouralCompContent = new PdfPCell(new Phrase(bcomp.ToString(),font));
                cellBehaviouralCompContent.Colspan = 2;
                cellBehaviouralCompContent.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                table.AddCell(cellBehaviouralCompContent);



                //var skillsCategoryList = _dal.GetSelectedSkillsPerCatergiryListForJobProfile((int)data.JobProfileID);
                //foreach (var item in skillsCategoryList)
                //{
                //  table.AddCell("Skills: ");
                //  table.AddCell(item.skillName);
                //}
                table.AddCell(emptyCell);
                //Additional Requirements
                PdfPCell cellOtherSpecialRequirements = new PdfPCell(new Phrase("Additional Requirements",font));
                cellOtherSpecialRequirements.Colspan = 2;
                cellOtherSpecialRequirements.HorizontalAlignment = Element.ALIGN_LEFT;
                cellOtherSpecialRequirements.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cellOtherSpecialRequirements);

                PdfPCell cellOtherSpecialRequirementsContent = new PdfPCell(new Phrase(data.OtherSpecialRequirements,font));
                cellOtherSpecialRequirementsContent.Colspan = 2;
                cellOtherSpecialRequirementsContent.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                table.AddCell(cellOtherSpecialRequirementsContent);

                //table.AddCell("Additional Requirements: ");
                //table.AddCell(data.OtherSpecialRequirements);
                //End Additional Requirements

                //table.AddCell("Recruiter ");
                //table.AddCell(data.Recruiter);

                //table.AddCell("Email your query to ");
                //table.AddCell(data.RecruiterEmail);
                table.AddCell(emptyCell);

                PdfPCell cellHowToApply = new PdfPCell(new Phrase("How to apply", font));
                cellHowToApply.Colspan = 2;
                cellHowToApply.HorizontalAlignment = Element.ALIGN_LEFT;
                cellHowToApply.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cellHowToApply);

                Font LinkFont = FontFactory.GetFont(FontFactory.COURIER, 9, iTextSharp.text.Font.UNDERLINE, BaseColor.BLUE);

                //Get List of all support stuff
                var support = from a in _db.SupportStaffs 
                               join b in _db.AspNetUsers on a.UserID equals b.Id
                              where a.IsActive == true
                              select b.Email;

                var result = (dynamic)null;
                List<string> list = new List<string>();

                foreach (var sf in support)
                {
                    //result = String.Join("; ", sf.ToString());
                  
                    list.Add(sf.ToString());
         
                }
                String[] str = list.ToArray();
                result = string.Join(" , ", str);

                //End Get List of all support stuff

                PdfPCell cellcellHowToApplyContent = new PdfPCell(new Phrase("To apply please log onto the e-Government Portal: "
                                                    + new Uri(System.Configuration.ConfigurationManager.AppSettings["LogOutURL"])
                                                    + " and follow the following process;"
                                                    + Environment.NewLine + "1. Register using your ID and personal information;"
                                                    + Environment.NewLine + "2. Use received one-time pin to complete the registration;"
                                                    + Environment.NewLine + "3. Log in using your username and password;"
                                                    + Environment.NewLine + "4. Click on “Employment & Labour”; "
                                                    + Environment.NewLine + "5. Click on “Recruitment Citizen” to create profile, update profile, browse and apply for jobs;"
                                                    + Environment.NewLine
                                                    + Environment.NewLine + "Or, if candidate has registered on eservices portal, access "
                                                    + new Uri(System.Configuration.ConfigurationManager.AppSettings["LogOutURL"])
                                                    + ", then follow the below steps:"
                                                    + Environment.NewLine + "1. Click on “Employment & Labour”;"
                                                    + Environment.NewLine + "2. Click on “Recruitment Citizen”;"
                                                    + Environment.NewLine + "3. Log in using your username and password;"
                                                    + Environment.NewLine + "4. Click on “Recruitment Citizen” to create profile, update profile, browse and apply for jobs; "
                                                    + Environment.NewLine + Environment.NewLine
                                                    + "For support, please send an email to: "+System.Configuration.ConfigurationManager.AppSettings["SupportEmailAddress"], font));


                cellcellHowToApplyContent.Colspan = 2;
                cellcellHowToApplyContent.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                table.AddCell(cellcellHowToApplyContent);


                //        table.AddCell("HOW TO APPLY ");
                //table.AddCell(System.Configuration.ConfigurationManager.AppSettings["LogOutURL"]);

                table.AddCell(emptyCell);
                //Closing Date
                PdfPCell cellClosingDate = new PdfPCell(new Phrase("Closing Date : " + Convert.ToDateTime(data.ClosingDate).ToString("dd MMM yyyy"), font));
                cellClosingDate.Colspan = 2;
                cellClosingDate.HorizontalAlignment = Element.ALIGN_LEFT;

                cellClosingDate.BackgroundColor = BaseColor.LIGHT_GRAY;
                cellClosingDate.Border = 0; //Added this for testing
                table.AddCell(cellClosingDate);
                //End Closing Date

                table.AddCell(emptyCell);

                char[] delimiterChars = { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                StringBuilder rbuilder = new StringBuilder();

                string text = data.Disclaimer.Replace("\r\n", "");

                //System.Diagnostics.Debug.WriteLine($"Original text: '{text}'");

                string[] words = text.Split(delimiterChars);
                //System.Diagnostics.Debug.WriteLine($"{words.Length} words in text:");
                int myVal = 0;
                foreach (var word in words)
                {
                    if (word != null || word != "")
                    {
                        myVal += 1;
                        if (myVal - 1 == 0)
                        {
                            rbuilder.Append(word + Environment.NewLine);
                            //rbuilder.AppendLine();
                            //System.Diagnostics.Debug.WriteLine($"<{word + Environment.NewLine}>");
                        }
                        else
                        {
                            rbuilder.Append(myVal - 1 + word + Environment.NewLine);
                            //System.Diagnostics.Debug.WriteLine($"<{myVal - 1 + word}>"); 
                        }
                    }
                }

                string disclaimerData = rbuilder.ToString();

                PdfPCell cellDisclaimer = new PdfPCell(new Phrase("Disclaimer", font));
                cellDisclaimer.Colspan = 2;
                cellDisclaimer.HorizontalAlignment = Element.ALIGN_LEFT;
                cellDisclaimer.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cellDisclaimer);

               //string myDisclaimer =  string.Join(Environment.NewLine, data.Disclaimer.ToCharArray().Where(Char.IsDigit));

                //PdfPCell cellDisclaimerContent = new PdfPCell(new Phrase(myDisclaimer));
                //PdfPCell cellDisclaimerContent = new PdfPCell(new Phrase(data.Disclaimer));
                PdfPCell cellDisclaimerContent = new PdfPCell(new Phrase(disclaimerData,font));
                cellDisclaimerContent.Colspan = 2;
                cellDisclaimerContent.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                table.AddCell(cellDisclaimerContent);

                //table.DefaultCell.Border = Rectangle.NO_BORDER;
                document.Add(table);

                document.Add(new Paragraph("\n"));


                //paragraph.Add(text);
                Paragraph paraEnd = new Paragraph("******NB: EMAILED CV'S WILL NOT BE ACCEPTED******",font);
                paraEnd.Alignment = Element.ALIGN_CENTER;
                paraHead.Font = FontFactory.GetFont("dax-black", 10, Font.ITALIC);
                document.Add(paraEnd);

                document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());

                document.Close();
                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();
                Response.Clear();
                Response.ContentType = "application/pdf";

                string pdfName = data.ReferenceNo.Replace("/", "_");
                string jobTitle = data.JobTitle.Replace(":"," ") + ".pdf";
                string fileName = data.ReferenceNo.Replace("/", "_") + ".pdf";

                //Response.AddHeader("content-disposition", "attachment;filename=" + string.Format("{}_{}", jobTitle,fileName));
                Response.AddHeader("content-disposition", "attachment;filename=" + jobTitle);

                //Response.AddHeader("Content-Disposition", @"attachment; filename=" + pdfName + ".pdf");
                Response.ContentType = "application/pdf";
                Response.Buffer = true;
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                Response.BinaryWrite(bytes);
                Response.End();
                Response.Close();

            }

            return View();
        }

        //===============================Peter - 20221014==============================
        [HttpGet]
        public ActionResult ApplyForVacancy(int? id, string userid)
        {
           
             return View();
            //return RedirectToAction("ApplyForVacancy", new { id = id });
        }
        //=============================================================================
        //==============================Peter - 20221014===============================
        public ActionResult DownloadCandidateProfile(string id)
        {
            //int appID = Convert.ToInt32(Request["appid"]);
            int profileId = _db.tblProfiles.Where(x => x.UserID == id).Select(x => x.pkProfileID).FirstOrDefault();
            var profileData = _dal.GetCandidateProfileInfo(id).FirstOrDefault();
            var educationData = _dal.GetEducationList(id).ToList();
            var workHistoryData = _dal.GetWorkHistoryList(id);
            var skillsData = _dal.GetCandidateSkillList(id);
            var languageData = _dal.GetCandidateLanguageList(id);
            var referenceData = _dal.GetReferenceList(id);

            //var data = _db.sp_GetVacancyAdDetail(id).FirstOrDefault();

            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {

                //Document document = new Document(PageSize.A4, 10, 10, 10, 10);
                Document document = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
                //Document document = new Document(new Rectangle(288f, 144f), 10, 10, 10, 10);
                //document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                Font courier = new Font(Font.FontFamily.HELVETICA, 9f);

                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();
                //document.Add(new Paragraph("\n"));

                Paragraph paraHead = new Paragraph("Candidate Profile of ".ToUpper() + string.Format("{0} {1}", profileData.Surname, profileData.FirstName).ToUpper());
                paraHead.Alignment = Element.ALIGN_CENTER;
                paraHead.Font = FontFactory.GetFont("dax-black", 20, Font.BOLD);
                //cellVacancyPurpose.BackgroundColor = BaseColor.LIGHT_GRAY;

                document.Add(paraHead);
                document.Add(new Paragraph("\n"));

                PdfPTable table = new PdfPTable(2);

                //PdfPCell cell = new PdfPCell(new Phrase("Vacancy Information Download"));
                PdfPCell cell = new PdfPCell(new Phrase("Personal Information"));
                cell.Colspan = 2;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.Border = 0; //Added this for testing
                table.AddCell(cell);

                PdfPCell emptyCell = new PdfPCell(new Phrase(" "));
                emptyCell.Colspan = 2;
                emptyCell.Border = 0;

                table.AddCell(emptyCell);
                table.SetWidths(new int[] { 1, 2 });
                PdfPCell cellP = null;

                cellP = new PdfPCell(new Phrase("ID Number: "));
                cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cellP);
                table.AddCell(profileData.IDNumber);

                cellP = new PdfPCell(new Phrase("Passport Number: "));
                cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cellP);
                table.AddCell(profileData.PassportNumber);

                cellP = new PdfPCell(new Phrase("Surname: "));
                cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cellP);
                table.AddCell(profileData.Surname);

                cellP = new PdfPCell(new Phrase("First Name: "));
                cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cellP);
                table.AddCell(profileData.FirstName);

                cellP = new PdfPCell(new Phrase("Date of Birth: "));
                cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cellP);
                table.AddCell(profileData.DateOfBirth);

                cellP = new PdfPCell(new Phrase("Race: "));
                cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cellP);
                table.AddCell(profileData.Race);

                cellP = new PdfPCell(new Phrase("Gender: "));
                cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cellP);
                table.AddCell(profileData.Gender);

                cellP = new PdfPCell(new Phrase("Contact No: "));
                cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cellP);
                table.AddCell(profileData.CellNo);

                cellP = new PdfPCell(new Phrase("Alternative No: "));
                cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cellP);
                table.AddCell(profileData.AlternativeNo);

                cellP = new PdfPCell(new Phrase("Email Address: "));
                cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cellP);
                table.AddCell(profileData.EmailAddress);

                cellP = new PdfPCell(new Phrase("Matric: "));
                cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cellP);
                table.AddCell(profileData.Matric);

                cellP = new PdfPCell(new Phrase("Drivers License: "));
                cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cellP);
                table.AddCell(profileData.DriversLicense);

                cellP = new PdfPCell(new Phrase("Unit No: "));
                cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cellP);
                table.AddCell(profileData.UnitNo);

                cellP = new PdfPCell(new Phrase("Complex Name: "));
                cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cellP);
                table.AddCell(profileData.ComplexName);

                cellP = new PdfPCell(new Phrase("Street No: "));
                cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cellP);
                table.AddCell(profileData.StreetNo);

                cellP = new PdfPCell(new Phrase("Street Name: "));
                cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cellP);
                table.AddCell(profileData.StreetName);

                cellP = new PdfPCell(new Phrase("Suburb Name: "));
                cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cellP);
                table.AddCell(profileData.SuburbName);

                cellP = new PdfPCell(new Phrase("City: "));
                cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cellP);
                table.AddCell(profileData.City);

                cellP = new PdfPCell(new Phrase("Province: "));
                cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cellP);
                table.AddCell(profileData.Province);

                cellP = new PdfPCell(new Phrase("ID Number: "));
                cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cellP);
                table.AddCell(profileData.PostalCode);

                if (profileData.ProfessionallyRegisteredID == 1)
                {
                    cellP = new PdfPCell(new Phrase("Are You Professionally Registered?"));
                    cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell("Are You Professionally Registered? ");
                    table.AddCell("Yes");

                    cellP = new PdfPCell(new Phrase("Registration Date: "));
                    cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cellP);

                    if (profileData.RegistrationDate.ToString().Contains("0001/01/01"))
                    {
                        table.AddCell(string.Empty);
                    }
                    else { table.AddCell(profileData.RegistrationDate.ToString()); }

                    cellP = new PdfPCell(new Phrase("Registration Number: "));
                    cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cellP);
                    if (profileData.RegistrationNumber != null) { table.AddCell(profileData.RegistrationNumber.ToString()); } else { table.AddCell(string.Empty); }

                    cellP = new PdfPCell(new Phrase("Registration Body: "));
                    cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cellP);
                    if (profileData.RegistrationBody != null) { table.AddCell(profileData.RegistrationBody.ToString()); } else { table.AddCell(string.Empty); }
                    //table.AddCell(profileData.RegistrationBody.ToString());
                }
                else
                {
                    cellP = new PdfPCell(new Phrase("Are You Professionally Registered? "));
                    cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cellP);
                    table.AddCell("No");
                }

                cellP = new PdfPCell(new Phrase("Were you previously employed in the Public Service? "));
                cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cellP);
                if (profileData.PreviouslyEmployedPS == 1)
                {
                    table.AddCell("Yes");
                }
                else
                {
                    table.AddCell("No");
                }

                if (profileData.ConditionsThatPreventsReEmploymentID == 1)
                {
                    cellP = new PdfPCell(new Phrase("Are there any conditions that prevents your re-employment?"));
                    cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cellP);
                    table.AddCell("Yes");

                    cellP = new PdfPCell(new Phrase("If Yes please specify:"));
                    cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cellP);
                    table.AddCell(profileData.ReEmployment);

                    cellP = new PdfPCell(new Phrase("Provide the name of the previous employing department:"));
                    cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cellP);
                    table.AddCell(profileData.PreviouslyEmployedDepartment);
                }
                else
                {
                    cellP = new PdfPCell(new Phrase("Are there any conditions that prevents your re-employment?"));
                    cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cellP);
                    table.AddCell("No");
                }

                if (profileData.fkDisabilityID == 1)
                {
                    cellP = new PdfPCell(new Phrase("Disability:"));
                    cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cellP);
                    table.AddCell("Yes");

                    cellP = new PdfPCell(new Phrase("Disability:"));
                    cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cellP);
                    table.AddCell(profileData.Disability);

                    //if (profileData.NatureOfDisability == 2)
                    //{
                    //    table.AddCell("Nature of Disability: ");
                    //    table.AddCell(profileData.OtherNatureOfDisability);
                    //}

                    cellP = new PdfPCell(new Phrase("Nature of Disability: "));
                    cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cellP);
                    table.AddCell(profileData.OtherNatureOfDisability);

                }
                else
                {
                    cellP = new PdfPCell(new Phrase("Disability: "));
                    cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cellP);
                    table.AddCell("No");
                }

                if (profileData.SACitizen == 1)
                {
                    cellP = new PdfPCell(new Phrase("RSA Citizen: "));
                    cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cellP);
                    table.AddCell("Yes");
                }
                else
                {
                    cellP = new PdfPCell(new Phrase("RSA Citizen: "));
                    cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cellP);
                    table.AddCell("No");
                }

                if (profileData.pkCriminalOffenseID == 1)
                {
                    cellP = new PdfPCell(new Phrase("Criminal Offence: "));
                    cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cellP);
                    table.AddCell("Yes");
                }
                else
                {
                    cellP = new PdfPCell(new Phrase("Criminal Offence: "));
                    cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cellP);
                    table.AddCell("No");
                }

                string lang = _db.lutLanguages.Where(x => x.languageID == profileData.fkLanguageForCorrespondenceID).Select(x => x.LanguageName).FirstOrDefault();
                cellP = new PdfPCell(new Phrase("Preferred Language For Correspondence: "));
                cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cellP);
                table.AddCell(lang);

                cellP = new PdfPCell(new Phrase("Telephone No During Working Hours: "));
                cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cellP);
                table.AddCell(profileData.TelNoDuringWorkingHours);

                string commMethod = _db.lutMethodOfCommunications.Where(x => x.MethodID == profileData.MethodOfCommunicationID).Select(x => x.MethodOfCommunication).FirstOrDefault();
                cellP = new PdfPCell(new Phrase("Preferred Way Of Correspondence: "));
                cellP.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cellP);
                table.AddCell(commMethod);

                table.AddCell(emptyCell);

                PdfPTable tableEducation = new PdfPTable(6);
                PdfPCell cellEducationHistory = new PdfPCell(new Phrase("Education History"));
                cellEducationHistory.Colspan = 6;
                cellEducationHistory.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cellEducationHistory.BackgroundColor = BaseColor.GRAY;
                tableEducation.AddCell(cellEducationHistory);


                PdfPCell cellQualificationName = new PdfPCell(new Phrase("Qualification Name"));
                cellQualificationName.HorizontalAlignment = Element.ALIGN_LEFT;
                cellQualificationName.BackgroundColor = BaseColor.LIGHT_GRAY;
                tableEducation.AddCell(cellQualificationName);

                PdfPCell cellInstitutionName = new PdfPCell(new Phrase("Institution Name"));
                cellInstitutionName.HorizontalAlignment = Element.ALIGN_LEFT;
                cellInstitutionName.BackgroundColor = BaseColor.LIGHT_GRAY;
                tableEducation.AddCell(cellInstitutionName);

                PdfPCell cellQualificationTypeName = new PdfPCell(new Phrase("Qualification Type"));
                cellQualificationTypeName.HorizontalAlignment = Element.ALIGN_LEFT;
                cellQualificationTypeName.BackgroundColor = BaseColor.LIGHT_GRAY;
                tableEducation.AddCell(cellQualificationTypeName);

                PdfPCell cellCertificateNumber = new PdfPCell(new Phrase("Certificate Number"));
                cellCertificateNumber.HorizontalAlignment = Element.ALIGN_LEFT;
                cellCertificateNumber.BackgroundColor = BaseColor.LIGHT_GRAY;
                tableEducation.AddCell(cellCertificateNumber);

                PdfPCell cellEducationStartDate = new PdfPCell(new Phrase("Start Date"));
                cellEducationStartDate.HorizontalAlignment = Element.ALIGN_LEFT;
                cellEducationStartDate.BackgroundColor = BaseColor.LIGHT_GRAY;
                tableEducation.AddCell(cellEducationStartDate);


                PdfPCell cellEducationEndDate = new PdfPCell(new Phrase("End Date"));
                cellEducationEndDate.HorizontalAlignment = Element.ALIGN_LEFT;
                cellEducationEndDate.BackgroundColor = BaseColor.LIGHT_GRAY;
                tableEducation.AddCell(cellEducationEndDate);

                foreach (var d in educationData)
                {
                    tableEducation.AddCell(d.qualificationName);
                    tableEducation.AddCell(d.institutionName);
                    tableEducation.AddCell(d.QualificationTypeName);
                    tableEducation.AddCell(d.certificateNumber);
                    tableEducation.AddCell(d.startDate);
                    tableEducation.AddCell(d.endDate);
                }

                //Work History
                PdfPTable tableWorkHistory = new PdfPTable(6);
                //tableWorkHistory.SetWidths(new int[] { 1, 1, 2, 1, 1, 1, 1 });
                PdfPCell cellWorkHistory = new PdfPCell(new Phrase("Work History"));
                cellWorkHistory.Colspan = 6;
                cellWorkHistory.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cellWorkHistory.BackgroundColor = BaseColor.GRAY;
                tableWorkHistory.AddCell(cellWorkHistory);

                PdfPCell cellCompanyName = new PdfPCell(new Phrase("Company Name"));
                cellCompanyName.HorizontalAlignment = Element.ALIGN_LEFT;
                cellCompanyName.BackgroundColor = BaseColor.LIGHT_GRAY;
                tableWorkHistory.AddCell(cellCompanyName);

                PdfPCell cellJobTitle = new PdfPCell(new Phrase("Job Title"));
                cellJobTitle.HorizontalAlignment = Element.ALIGN_LEFT;
                cellJobTitle.BackgroundColor = BaseColor.LIGHT_GRAY;
                tableWorkHistory.AddCell(cellJobTitle);

                PdfPCell cellDepartment = new PdfPCell(new Phrase("Department"));
                cellDepartment.HorizontalAlignment = Element.ALIGN_LEFT;
                cellDepartment.BackgroundColor = BaseColor.LIGHT_GRAY;
                tableWorkHistory.AddCell(cellDepartment);

                PdfPCell cellStartDate = new PdfPCell(new Phrase("Start Date"));
                cellStartDate.HorizontalAlignment = Element.ALIGN_LEFT;
                cellStartDate.BackgroundColor = BaseColor.LIGHT_GRAY;
                tableWorkHistory.AddCell(cellStartDate);

                PdfPCell cellEndDate = new PdfPCell(new Phrase("End Date"));
                cellEndDate.HorizontalAlignment = Element.ALIGN_LEFT;
                cellEndDate.BackgroundColor = BaseColor.LIGHT_GRAY;
                tableWorkHistory.AddCell(cellEndDate);

                PdfPCell cellReasonForLeaving = new PdfPCell(new Phrase("Reason For Leaving"));
                cellReasonForLeaving.HorizontalAlignment = Element.ALIGN_LEFT;
                cellReasonForLeaving.BackgroundColor = BaseColor.LIGHT_GRAY;
                tableWorkHistory.AddCell(cellReasonForLeaving);

                foreach (var d in workHistoryData)
                {
                    tableWorkHistory.AddCell(d.companyName);
                    tableWorkHistory.AddCell(d.jobTitle);
                    tableWorkHistory.AddCell(d.department);
                    tableWorkHistory.AddCell(d.startDate);
                    if (d.reasonForLeaving == "current") { tableWorkHistory.AddCell(string.Empty); } else { tableWorkHistory.AddCell(d.endDate); }
                    if (d.reasonForLeaving == "current") { tableWorkHistory.AddCell(string.Empty); } else { tableWorkHistory.AddCell(d.reasonForLeaving); }

                    PdfPCell cellDutiesHeading = new PdfPCell(new Phrase("Duties"));
                    cellDutiesHeading.Colspan = 6;
                    cellDutiesHeading.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    cellDutiesHeading.BackgroundColor = BaseColor.LIGHT_GRAY;
                    tableWorkHistory.AddCell(cellDutiesHeading);

                    if (d.positionHeld != null && d.positionHeld != "")
                    {

                        string text = d.positionHeld;
                        string duties1 = this.RemoveSpecialCharacters(text);

                        PdfPCell cellDuties = new PdfPCell(new Phrase(duties1));
                        cellDuties.Colspan = 6;
                        cellDuties.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                                            //cellDuties.BackgroundColor = BaseColor.GRAY;
                                                            //cellDuties.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                        tableWorkHistory.AddCell(cellDuties);
                    }
                    else
                    {
                        PdfPCell cellDuties = new PdfPCell(new Phrase("No duties specified."));
                        cellDuties.Colspan = 6;
                        cellDuties.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                                            //cellDuties.BackgroundColor = BaseColor.GRAY;
                        tableWorkHistory.AddCell(cellDuties);
                    }
                }

                //Candidate Skills
                PdfPTable tableSkills = new PdfPTable(2);
                PdfPCell cellSkills = new PdfPCell(new Phrase("Skills"));
                cellSkills.Colspan = 2;
                cellSkills.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cellSkills.BackgroundColor = BaseColor.GRAY;
                tableSkills.AddCell(cellSkills);

                PdfPCell cellSkillName = new PdfPCell(new Phrase("Skill Name"));
                cellSkillName.HorizontalAlignment = Element.ALIGN_LEFT;
                cellSkillName.BackgroundColor = BaseColor.LIGHT_GRAY;
                tableSkills.AddCell(cellSkillName);

                PdfPCell cellSkillProficiency = new PdfPCell(new Phrase("Skill Proficiency"));
                cellSkillProficiency.HorizontalAlignment = Element.ALIGN_LEFT;
                cellSkillProficiency.BackgroundColor = BaseColor.LIGHT_GRAY;
                tableSkills.AddCell(cellSkillProficiency);

                foreach (var d in skillsData)
                {
                    tableSkills.AddCell(d.skillName);
                    tableSkills.AddCell(d.SkillProficiency);

                }

                //Candidate Languages
                PdfPTable tableLanguage = new PdfPTable(2);
                PdfPCell cellLanguage = new PdfPCell(new Phrase("Languages"));
                cellLanguage.Colspan = 2;
                cellLanguage.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cellLanguage.BackgroundColor = BaseColor.GRAY;
                tableLanguage.AddCell(cellLanguage);

                PdfPCell cellLanguageName = new PdfPCell(new Phrase("Language Name"));
                cellLanguageName.HorizontalAlignment = Element.ALIGN_LEFT;
                cellLanguageName.BackgroundColor = BaseColor.LIGHT_GRAY;
                tableLanguage.AddCell(cellLanguageName);

                PdfPCell cellLanguageProficiency = new PdfPCell(new Phrase("Language Proficiency"));
                cellLanguageProficiency.HorizontalAlignment = Element.ALIGN_LEFT;
                cellLanguageProficiency.BackgroundColor = BaseColor.LIGHT_GRAY;
                tableLanguage.AddCell(cellLanguageProficiency);

                foreach (var d in languageData)
                {
                    tableLanguage.AddCell(d.LanguageName);
                    tableLanguage.AddCell(d.LanguageProficiency);
                }

                //Candidate Reference
                PdfPTable tableReference = new PdfPTable(5);
                PdfPCell cellReference = new PdfPCell(new Phrase("References"));
                cellReference.Colspan = 5;
                cellReference.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cellReference.BackgroundColor = BaseColor.GRAY;
                tableReference.AddCell(cellReference);

                PdfPCell cellFullNames = new PdfPCell(new Phrase("Full Names"));
                cellFullNames.HorizontalAlignment = Element.ALIGN_LEFT;
                cellFullNames.BackgroundColor = BaseColor.LIGHT_GRAY;
                tableReference.AddCell(cellFullNames);

                PdfPCell cellRefCompanyName = new PdfPCell(new Phrase("Company Name"));
                cellRefCompanyName.HorizontalAlignment = Element.ALIGN_LEFT;
                cellRefCompanyName.BackgroundColor = BaseColor.LIGHT_GRAY;
                tableReference.AddCell(cellRefCompanyName);

                PdfPCell cellRefPositionHeld = new PdfPCell(new Phrase("Position Held"));
                cellRefPositionHeld.HorizontalAlignment = Element.ALIGN_LEFT;
                cellRefPositionHeld.BackgroundColor = BaseColor.LIGHT_GRAY;
                tableReference.AddCell(cellRefPositionHeld);

                PdfPCell cellTelephoneNo = new PdfPCell(new Phrase("Telephone No"));
                cellTelephoneNo.HorizontalAlignment = Element.ALIGN_LEFT;
                cellTelephoneNo.BackgroundColor = BaseColor.LIGHT_GRAY;
                tableReference.AddCell(cellTelephoneNo);

                PdfPCell cellEmailAddress = new PdfPCell(new Phrase("Email Address"));
                cellEmailAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                cellEmailAddress.BackgroundColor = BaseColor.LIGHT_GRAY;
                tableReference.AddCell(cellEmailAddress);

                foreach (var d in referenceData)
                {
                    tableReference.AddCell(d.refName);
                    tableReference.AddCell(d.companyName);
                    tableReference.AddCell(d.positionHeld);
                    tableReference.AddCell(d.telNo);
                    tableReference.AddCell(d.emailAddress);
                }


                document.Add(table);
                document.Add(tableEducation);
                document.Add(new Paragraph("\n"));
                document.Add(tableWorkHistory);
                document.Add(new Paragraph("\n"));
                document.Add(tableSkills);
                document.Add(new Paragraph("\n"));
                document.Add(tableLanguage);
                document.Add(new Paragraph("\n"));
                document.Add(tableReference);

                //paragraph.Add(text);
                Paragraph paraEnd = new Paragraph("******NB: End of Candidate Profile******");
                paraEnd.Alignment = Element.ALIGN_CENTER;
                paraHead.Font = FontFactory.GetFont("dax-black", 20, Font.ITALIC);
                document.Add(paraEnd);

                document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());

                document.Close();
                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();
                Response.Clear();

                Response.ContentType = "application/pdf";

                //string pdfName = data.FirstName.Replace("/", "_");
                string fileName = String.Format("{0}_{1}_Profile", profileData.FirstName.Trim(), profileData.Surname.Trim()) + ".pdf";
                //string fileName = "_Profile.pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + fileName);

                //Response.AddHeader("Content-Disposition", @"attachment; filename=" + pdfName + ".pdf");
                Response.ContentType = "application/pdf";
                Response.Buffer = true;
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                Response.BinaryWrite(bytes);
                Response.End();
                Response.Close();

            }

            return View();
        }
        //=============================================================================
        //===========================Peter 20221014====================================
        private string RemoveSpecialCharacters(string value)
        {
            return value.Replace("\u0095", ".").Replace("\u0092", "'").Replace("\u0096", "-").Replace("•", ".").Replace("amp;amp;amp;", "").Replace("&#39;", "'").Replace("&amp;amp;amp;", "&").Replace("‘", "'").Replace("’", "'").Replace("#39", "'").Replace("&quot;", "\"").Replace("&lt", "<").Replace("&gt", ">").Replace("&amp", "&").Trim();
        }
        //=============================================================================

        //Apply For Vacancy
        public ActionResult ApplyForVacancy(int? id, string[] CandidateVacancyResponseID, string chkDeclaration)
        {
            string uid = User.Identity.GetUserId();
            var IDNumber = _dal.GetProfileIDNumber(uid);

            //================Peter 20220922====================
            Session["sessUserIDnumber"] = IDNumber;
            //==================================================
            

            OrganisationModel organisation = new OrganisationModel();
            organisation = _dal.GetOrganisationIDPerVacancy((int)id);
            int OrganisationID = organisation.OrganisationID;


            //=============================================Peter - 20220913=================================================
            var profileComplete = _db.proc_eRecruitmentCheckCompleteProfile(uid).ToList();
            if (profileComplete == null || profileComplete.Count == 0)
            {
                Session["ProfileNotComplete"] = "true";
                return RedirectToAction("ViewVacancyForApplying", new { id = id });
            }
            //==============================================================================================================

            //=============================================Peter - 20220913=================================================
            string getLastEight = (Convert.ToString(IDNumber).Substring(11, 1));
            if (Convert.ToInt32(getLastEight) != 8)
            {
                Session["NotSAIDnumber"] = "true";
                return RedirectToAction("ViewVacancyForApplying", new { id = id });
            }
            //==============================================================================================================

            //===========================Peter - 20221104 Profile against Work Qualifications================================
            if (CandidateVacancyResponseID != null)
            {
                int prfleID = (from a in _db.tblProfiles where a.UserID == uid select a.pkProfileID).FirstOrDefault();
                var educationQualTypefID = _db.tblCandidateEducations.Where(x => x.ProfileID != 1 && x.ProfileID == prfleID).Select(x => x.QualificationTypeID).OrderBy(n => n).Distinct().ToList();
                List<int?> list = new List<int?>();
                int i, p, CountChecked = 0;
                bool isMatched = false;
                for (i = 0; i < educationQualTypefID.Count(); i++)
                {
                    //Get the QualificationTypeID of the selected Education Checkboxes and put them in the list
                    for (p = 0; p < Convert.ToInt32(CandidateVacancyResponseID.Count()); p++)
                    {
                        var eduQualTypeID_Checked = from b in _db.lutGeneralQuestions
                                                    where (b.id == Convert.ToInt32(CandidateVacancyResponseID[p]) && (b.QualificationTypeID != null))
                                                    select new { QTypeId = b.QualificationTypeID };
                        foreach (var d in eduQualTypeID_Checked)
                        {
                            list.Add(d.QTypeId);
                        }
                    }
                    foreach (var d in list)
                    {
                        if (educationQualTypefID[i] == d.Value)
                        {
                            //if (CountChecked == educationQualTypefID.Count()) //Commented on 20230221, while testing it was stuck on this error of Qualifications
                            //{
                            isMatched = true; break;
                            //}
                            //CountChecked += 1;
                        }
                    }
                }

                if (isMatched == false)
                {
                    Session["QualificationNotMatchProfile"] = "true";
                    return RedirectToAction("ViewVacancyForApplying", new { id = id });
                }
            }
            //==============================================================================================================
            //===========================Peter - 20221104 Profile against Work Exeperience==================================
            if (CandidateVacancyResponseID != null)
            {
                string prfleID = (from a in _db.tblProfiles where a.UserID == uid select a.pkProfileID).FirstOrDefault().ToString();
                var intWorkExperience = _db.proc_eRecruitmentWorkExperienceVSProfile(prfleID).ToString();
                List<int?> lstExperience = new List<int?>();
                bool isMatched = false;

                for (int i = 1; i < 10; i++) {
                    if (intWorkExperience == Convert.ToString(i))
                    {
                        var mylistIDs = _db.lutGeneralQuestions.Where(x => x.Experience.Contains(intWorkExperience) && x.Experience != null && x.QCategoryID == 2).Select(x => x.id).OrderBy(n => n).Distinct().ToList();
                        if (mylistIDs.Count() > 0){
                            foreach (var d in mylistIDs) { lstExperience.Add(d); }
                            foreach (var d in mylistIDs){ if (CandidateVacancyResponseID[i] == d.ToString()) {isMatched = true; break;}}
                            break;
                        }
                    }
                    if (Convert.ToInt32(intWorkExperience) >= 10)
                    {
                        var mylistIDs = _db.lutGeneralQuestions.Where(x => x.Experience.Contains("10") && x.Experience != null && x.QCategoryID == 2).Select(x => x.id).OrderBy(n => n).Distinct().ToList();
                        if (mylistIDs.Count() > 0){
                            foreach (var d in mylistIDs) { lstExperience.Add(d); }
                            foreach (var d in mylistIDs) { if (CandidateVacancyResponseID[i] == d.ToString()) { isMatched = true; break;}}
                            break;
                        }
                    }
                }
                if (isMatched == false)
                {
                    Session["WorkExperienceNotMatchProfile"] = "true";
                    return RedirectToAction("ViewVacancyForApplying", new { id = id });
                }
            }
            //==============================================================================================================

            //=============================================Peter - 20221012=================================================
            if (chkDeclaration != "1" && chkDeclaration != "on")
            {
                Session["DeclarationTicked"] = "false";
                return RedirectToAction("ViewVacancyForApplying", new { id = id });
            }
            //==============================================================================================================



            //VacancyModels vacancyHistory = new VacancyModels();
            //vacancyHistory = _dal.GetVacancyHistoryIDPerVacancy((int)id);
            //int VacancyHistoryID = vacancyHistory.ID;

            VacancyModels vacancyQuestion = new VacancyModels();
            vacancyQuestion = _dal.GetVacancyQuestionIDPerVacancy((int)id);
            int VacancyQuestionID = vacancyQuestion.ID;

            if (CandidateVacancyResponseID != null)
            {
                foreach (string d in CandidateVacancyResponseID)
                {
                    //==========================Peter commented TonInt16 and added ToInt64===========================
                    //int questionid = Convert.ToInt16(d);
                    int questionid = (int)Convert.ToInt64(d);
                    //_dal.InsertCandidateVacancyResponseQuestion(uid, (int)id, Convert.ToInt16(questionid));
                    _dal.InsertCandidateVacancyResponseQuestion(uid, (int)id, (int)Convert.ToInt64(questionid));
                    //===============================================================================================

                    TempData["message"] = /*" Reference No: " + ReferenceNo + */"You Have Successfully Applied For This Position"; 
                }
            }
            else if (VacancyQuestionID == 0)
            {
                int questionid = 0;
                _dal.InsertCandidateVacancyResponseQuestion(uid, (int)id, Convert.ToInt16(questionid));

                TempData["message"] = /*" Reference No: " + ReferenceNo + */"You Have Successfully Applied For This Position";
            }
            else if (CandidateVacancyResponseID == null)
            {
                ViewBag.ErrorMessage = "You must select one of the Question Banks provided!";
                ModelState.AddModelError("", "You must select one of the Question Banks provided");
            }

            if (!ModelState.IsValid)
            {
                string msg = "You must select one of the Question Banks provided!";
                return RedirectToAction("ViewVacancyForApplying", new { id = id, msg = msg });
            }


        //if (_dal.InsertCandidateVacancyApplication(uid, (int)id, OrganisationID, Convert.ToString(IDNumber)) == true)
        //Peter added the chkDeclaration field on 20221024 and commented the code above===========
        if (chkDeclaration == "on") { chkDeclaration = "1"; } else { chkDeclaration = "0"; } 
        if (_dal.InsertCandidateVacancyApplication(uid, (int)id, OrganisationID, Convert.ToString(IDNumber), Convert.ToInt32(chkDeclaration)))
        {

        StringBuilder rbuilder = new StringBuilder();
        string FullName = null;
        string JobApplied = null;
        string email = null;
        string PhoneNumber = null;
        string recruiteremail = null;
        string organisationName = null;
        Nullable<int> profileID = null;

        var candidate = from a in _db.tblProfiles
                        join b in _db.tblCandidateVacancyApplications on a.IDNumber equals b.IDNumber
                        join c in _db.tblVacancies on b.VacancyID equals c.ID
                        join d in _db.lutJobTitles on c.JobTitleID equals d.JobTitleID
                        join e in _db.lutSalaryStructures on c.JobTitleID equals e.JobTitleID
                        join f in _db.lutJobLevels on e.JobLevelID equals f.JobLevelID
                        join g in _db.lutOrganisations on c.OrganisationID equals g.OrganisationID
                        where a.UserID == uid && c.ID == id

                        select new
                        {
                          ProfileID = a.pkProfileID,
                          FullName = a.FirstName + " " + a.Surname,
                          email = a.EmailAddress,
                          PhoneNumber = a.CellNo,
                          RecruiterEmail = c.RecruiterEmail,
                          JobApplied = d.JobTitle + " - " + " Job Level: " + f.JobLevelName + " Reference No: " + c.ReferenceNo,
                          OrganisationName = g.OrganisationName + " (" + g.OrganisationCode + ")"
                        };

        foreach (var d in candidate)
        {
          profileID = d.ProfileID;
          FullName = d.FullName;
          JobApplied = d.JobApplied;
          email = d.email;
          PhoneNumber = d.PhoneNumber;
          recruiteremail = d.RecruiterEmail;
          organisationName = d.OrganisationName;
        }

        rbuilder.Append("Dear: " + FullName + "<br/>");
        rbuilder.AppendLine();
        rbuilder.Append("<br/>");
        rbuilder.Append("Thank you for your application and interest shown for the position of " + JobApplied + " . "
                        + "Correspondence will be limited to short listed candidates only. Should you not hear from us within two months of the closing date, please consider your application as unsuccessful.<br/> ");
        rbuilder.Append("<br/>");
        rbuilder.Append("<ul><b><li>It is the applicant`s responsibility to have foreign qualifications evaluated by the South African Qualifications Authority (SAQA).</li></b>");
        rbuilder.Append("<b><li>" + organisationName + " reserves the right not to make an appointment.</li></b>");
        rbuilder.Append("<b><li>Appointment is subject to getting a positive security clearance, the signing of a balance score card contract, verification of the applicants documents (Qualifications), and reference checking.</li></b></ul>");
        rbuilder.Append("<br/><br/>");
        rbuilder.Append("Thank you for your patience while we process all applications.");
        rbuilder.Append("<br/><br/>");
        rbuilder.Append("Kind Regards<br/>E-Recruitment Team");
        string candidateEmail = rbuilder.ToString();

        notify.SendEmail(email, "e-Recruitment Notification", candidateEmail);
        notify.SendSMS(PhoneNumber, candidateEmail);
        //Insert Into Email Table
        _dal.InsertEmail(uid, Convert.ToInt32(profileID), recruiteremail, email, "Job Application Notification", candidateEmail, 1);
      }


            //3 is Approved
            //_dal.UpdateVacancyStatus(3, Convert.ToInt16(id));
            var data = _db.tblVacancies.Where(x => x.ID == id).FirstOrDefault();
            int orgid = (int)data.OrganisationID;
            return RedirectToAction("BrowseVacancy", new { id = orgid });
            //return View();
        }

        //Approve Vacancy
        public ActionResult Approve(int id)
        {
            //3 is Approved

            StringBuilder rbuilder = new StringBuilder();

            var RecruiterEmail = _db.tblVacancies.Where(x => x.ID == id).Select(x => x.RecruiterEmail).FirstOrDefault();
            var info = (from a in _db.tblVacancies
                        join b in _db.tblProfiles on a.UserID equals b.UserID
                        join c in _db.tblVacancyProfiles on a.VacancyProfileID equals c.VacancyProfileID
                        where a.ID == id
                        select new
                        {
                            b.EmailAddress
                                , b.Surname
                                , b.FirstName
                                , a.ReferenceNo
                                , c.VacancyName
                        }).FirstOrDefault();

            _dal.UpdateVacancyStatus(3,Convert.ToInt16(id));

            rbuilder.Append("Dear : <b>" + info.FirstName + " " + info.Surname + "</b>" + "<br/>");
            rbuilder.AppendLine("Vacancy Name: " + info.VacancyName + ", Reference No: " + info.ReferenceNo + "<br/>");
            rbuilder.Append("Has been successfully approved and will now be available to the public.");
            rbuilder.Append("");
            rbuilder.Append("<br/>");

            rbuilder.Append("Kind Regards");

            string aprovedemail = rbuilder.ToString();

            notify.SendEmail(RecruiterEmail, "e-Recruitment Notification", aprovedemail);

            return RedirectToAction("ApproveVacancy", new { id = User.Identity.GetUserId() });
        }
    public ActionResult LonglistCandidates()
    {

      return View();
    }

    //View Vacancy For Applying
    public ActionResult ViewVacancyForApplying(int id, string msg)
    {
      OrganisationModel Organisation = new OrganisationModel();
      string userid = User.Identity.GetUserId();
      Organisation = _dal.GetOrganisationIDByVacanyID(id);
      ViewBag.Questions = _dal.GetGeneralQuestionsList();
      ViewBag.Id = Organisation.OrganisationID;
            
      //==================Peter - 20221014===========
      Session["sessUserID"] = userid;
      Session["sessApplicationID"] = _dal.GetVacancyID(id, userid); 
      //=============================================

//VacancyModels vacancyID = new VacancyModels();
//vacancyID = _dal.GetVacancyIDPerVacancyHistory((int)id);
            int VacancyID = id;
      ViewBag.VacancyID = VacancyID;

      ViewBag.QuetionBanks = _dal.GetGeneralQuestionsList(VacancyID);
      ViewBag.QuetionExperienceCat = _dal.GetGeneralQuestionsExperienceList(VacancyID);
      ViewBag.QuetionExperienceInt = _dal.GetGeneralQuestionsExperienceint();
      ViewBag.QuetionCertificationCat = _dal.GetGeneralQuestionsCertificationList(VacancyID);
      ViewBag.QuetionAnnualSalaryCat = _dal.GetGeneralQuestionsAnnualSalaryList(VacancyID);
      ViewBag.QuetionNoticePeriodCat = _dal.GetGeneralQuestionsNoticePeriodList(VacancyID);
      //==================Peter - 20221115=================
      var JobTitleid = _db.tblVacancies.Where(x => x.ID == VacancyID).Select(x => x.JobTitleID).FirstOrDefault();
      //ViewBag.myJobTitleID = JobTitleid;
      ViewBag.QuestionJobSpecific = _dal.GetGeneralQuestionsJobSpecificList(JobTitleid);
      //===================================================

      if (msg != null)
      {
        ViewBag.ErrorMessage = "You must select one of the Question Banks provided!";
      }
      else { ViewBag.ErrorMessage = null; }

      //var p = new List<VacancyModels>();

      VacancyModels vacancy;

      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        var data = _db.sp_GetVacancyAdDetail(id).FirstOrDefault();

                //Technical Competency
                //StringBuilder technicalCompBuilder = new StringBuilder();
                //int technicalCompCounter = 0;
                //var technicalComp = _dal.GetTechnicalCompList((int)data.JobProfileID);

                //foreach (var item in technicalComp)
                //{
                //  technicalCompCounter += 1;
                //  technicalCompBuilder.AppendLine(technicalCompCounter.ToString() + ". " + item.TechnicalComp + " - " + item.TechnicalCompDesc);
                //}
                //ViewBag.TechnicalComp = technicalCompBuilder.ToString();
                string tcomp = string.Empty;
                if (data.TechnicalCompetenciesDescription != null) { tcomp = data.TechnicalCompetenciesDescription.ToString(); } else { tcomp = string.Empty; }
                ViewBag.TechnicalComp = tcomp;

                //Leadership Competency
                //        StringBuilder leadershipCompBuilder = new StringBuilder();
                //int leadershipCompCounter = 0;
                //var leadershipComp = _dal.GetLeadershipCompList((int)data.JobProfileID);

                //foreach (var item in leadershipComp)
                //{
                //  leadershipCompCounter += 1;
                //  leadershipCompBuilder.AppendLine(leadershipCompCounter.ToString() + ". " + item.LeadershipComp + " - " + item.LeadershipCompDesc);
                //}
                //ViewBag.LeadershipComp = leadershipCompBuilder.ToString();
                string lcomp = string.Empty;
                if (data.LeadershipCompetencies != null) { lcomp = data.LeadershipCompetencies.ToString(); } else { lcomp = string.Empty; }
                ViewBag.LeadershipComp = lcomp;

                //Behavioural Competency
                //        StringBuilder behaviouralCompBuilder = new StringBuilder();
                //int behaviouralCompCounter = 0;
                //var behaviouralComp = _dal.GetBehaviouralCompList((int)data.JobProfileID);

                //foreach (var item in behaviouralComp)
                //{
                //  behaviouralCompCounter += 1;
                //  behaviouralCompBuilder.AppendLine(behaviouralCompCounter.ToString() + ". " + item.BehaviouralComp + " - " + item.BehaviouralCompDesc);
                //}
                //ViewBag.BehaviouralComp = behaviouralCompBuilder.ToString();
                string bcomp = string.Empty;
                if (data.BehaviouralCompetency != null) { bcomp = data.BehaviouralCompetency.ToString(); } else { bcomp = string.Empty; }
                ViewBag.BehaviouralComp = bcomp;

                var skills = _dal.GetSelectedSkillsPerCatergiryListForJobProfile((int)data.jobprofileid);
        ViewBag.Skills = skills;

        vacancy = new VacancyModels()
        {
          ID = id,
          ReferenceNo = data.ReferenceNo,
            BPSNumber = data.VacancyNo,
            JobTitle = data.JobTitle,
          JobLevel = data.JobLevel,
          JobLevelID = (int)data.JobLevelID,
          Salary = data.Salary,
          OrganisationName = data.OrganisationName,
          Manager = data.Manager,
          Race = data.Race,
          Gender = data.Gender,
          ////Deviation = Convert.ToString(a.DeligationReasons,
          DivisionName = data.DivisionName,
          DepartmentName = data.DepartmentName,
          EmploymentType = data.EmploymentType,
          ContractDuration = data.ContractDuration,
          ClosingDate = data.ClosingDate,
          Location = data.Location,
          //Recruiter = data.Recruiter,
          //RecruiterEmail = data.ReplyTo,
          NumberOfOpenings = (int)data.NumberOfOpenings,
          VacancyTypeName = data.VacancyTypeName,
          VacancyPurpose = data.VacancyPurpose,
          Responsibility = data.Responsibility,
          QualificationsandExperience = data.QualificationsandExperience,
          TechnicalCompetenciesDescription = data.TechnicalCompetenciesDescription,
          OtherSpecialRequirements = data.OtherSpecialRequirements,
          Disclaimer = data.Disclaimer,
          Knowledge = data.Knowledge          
        };
      }

      return View(vacancy);
    }

    

        //Reject Vacancy
        public ActionResult RejectVacancy(FormCollection fc)
        {
            string reason = fc["RejectReason"];

            int id = Convert.ToInt16(fc["hdnVacancyID"]);

            int rejectReasonID = Convert.ToInt16(fc["RejectReasonID"]);

            string userid = User.Identity.GetUserId();

            _dal.UpdateRejectionReason(reason, id, userid, rejectReasonID);

            StringBuilder rbuilder = new StringBuilder();

            var data = _db.tblVacancies.Where(x => x.ID == id).FirstOrDefault();
            var info = (from a in _db.tblVacancies join b in _db.tblProfiles on a.UserID equals b.UserID
                        join c in _db.tblVacancyProfiles on a.VacancyProfileID equals c.VacancyProfileID
                        where a.ID == id
                        select new
                        {
                            b.EmailAddress
                                , b.Surname
                                , b.FirstName
                                , a.ReferenceNo
                                , c.VacancyName
                        }).FirstOrDefault();


            StringBuilder sBuilder = new StringBuilder();
            rbuilder.Append("<ul><li>" + reason + "</li></ul>");

            string rejectreason = rbuilder.ToString();

            _dal.UpdateVacancyStatus(4, Convert.ToInt16(id));

            rbuilder.Append("Dear : <b>" + info.FirstName + " " + info.Surname + "</b>" + "<br/>");

            rbuilder.AppendLine();

            rbuilder.Append("Your Vacancy has been rejected with the following reason: <br/><br/>" + rejectreason + "<br/>");

            rbuilder.Append("<br/>");

            rbuilder.Append("Kind Regards");

            string aprovedemail = rbuilder.ToString();

            notify.SendEmail(data.RecruiterEmail, "e-Recruitment Notification", aprovedemail);

            return RedirectToAction("ApproveVacancy", new { id = User.Identity.GetUserId() });
        }
        //Withdraw vacancy
        public ActionResult WithdrawVacancy(FormCollection fc)
        {
            int withdrawalReasonID = int.Parse(fc["WithdrawalReasonID"]);
            string reason = fc["WithdrawalReason"];
            int id = Convert.ToInt16(fc["hdnVacancyID"]);
            _dal.UpdateWithdrawalReason(reason, id, withdrawalReasonID);
            return RedirectToAction("Index", new { id = User.Identity.GetUserId() });
        }
        //Retract vacancy
        public ActionResult RetractVacancy(FormCollection hc)
        {
            int retractReasonID = int.Parse(hc["RetractReasonID"]);
            string reason = hc["RetractReason"];
            int id = Convert.ToInt16(hc["hdnVacancyIDFor"]);
            _dal.UpdateRetractReason(reason, id, retractReasonID);
            return RedirectToAction("Index", new { id = User.Identity.GetUserId() });

        }

        [AllowAnonymous]
        public ActionResult BrowseVacancy(int id = 0)
        {
          List<VacancyListModels> model;

          if (id > 0)
          {
            model = _dal.GetApprovedVacancyListForCandidates(id);
                //ViewBag.Id = id;
                //ViewBag.VacancyID = model.ID;

                //==========Peter created the following sessions variables ==========
                Session["ProfileNotComplete"] = "";
                Session["NotSAIDnumber"] = "";
                Session["QualificationNotMatchProfile"] = "";
                Session["WorkExperienceNotMatchProfile"] = "";
                Session["DeclarationTicked"] = "";
                //===================================================================

                return View(model);
          }
          else
          {
            model = new List<VacancyListModels>();
            return View(model);
          }
    
        }

        [HttpGet]
        public ActionResult CandidateScreening()
        {
            //ViewBag.CandidateScreening = _dal.GetScreenedCandidateList();
            return View();
        }

        public JsonResult GetCandidateScreeningVacancy()
        {
            var data = _dal.GetCandidateScreeningVacancy(User.Identity.GetUserId());
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProvinceList()
        {
            var data = _dal.GetProvinceList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGenderList()
        {
            var data = _dal.GetGenderList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRaceList()
        {
            var data = _dal.GetRaceList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

         //Edit Vacancy
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditVacancy(VacancyModels item, int id, string[] VacancyQuestionID)
        {
            string userID = User.Identity.GetUserId();

            if (item.ClosingDate <= DateTime.Now)
            {
                ModelState.AddModelError("", "Closing date cannot be less or equals to Today's Date");
            }

            if (ModelState.IsValid)
            {

                int? vacancyid = _dal.UpdateVacancy(id, userID, item.ReferenceNo, item.VacancyName, item.JobTitle, item.JobLevelID, item.OrganisationID,
                 item.Recruiter, item.Location, item.ContractDuration, item.Manager, item.EmploymentTypeID, item.SalaryRange, item.Responsibility
                , item.QualificationsandExperience, item.TechnicalCompetenciesDescription, item.OtherSpecialRequirements, Convert.ToString(item.ClosingDate), item.Disclaimer, item.VacancyPurpose, item.RecruiterEmail, item.RecruiterTel
                ,item.NumberOfOpenings,item.VacancyProfileID, item.DepartmentID,item.DivisionID,item.VancyTypeID, DateTime.Now);
                if (vacancyid != null)
                {
                    if(VacancyQuestionID != null)
                    {
                        string vqid = null;
                        vqid = string.Join(";",VacancyQuestionID);
                        _dal.InsertUpdateVacancyQuestion((int)vacancyid, Convert.ToString(vqid));
                        //foreach (string d in VacancyQuestionID)
                        //{
                        //    int questionid = Convert.ToInt16(d);
                        //   
                        //}
                    }
                    _dal.UpdateVacancyStatus(7, Convert.ToInt16(id));
                }
                TempData["message"] = "Vacancy Updated Successfully";
                return RedirectToAction("Index");
            }

            return View(item);

        }

        [HttpGet]
        public FileResult DownLoadFile(int id)
        {
            var doc = _db.tblVacancyDocuments.Where(x => x.id == id).FirstOrDefault();
            return File(doc.FileData.ToArray(), doc.ContentType, doc.sFileName);
        }

        public static string ExcelContentType
        {
            get
            { return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; }
        }

        public static DataTable ListToDataTable<T>(List<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable dataTable = new DataTable();

            for (int i = 0; i < properties.Count; i++)
            {
                PropertyDescriptor property = properties[i];
                dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            }

            object[] values = new object[properties.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = properties[i].GetValue(item);
                }

                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public static byte[] ExportExcel(DataTable dataTable, string heading = "", bool showSrNo = false, params string[] columnsToTake)
        {

            byte[] result = null;
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(String.Format("{0} Data", heading));
                int startRowFrom = String.IsNullOrEmpty(heading) ? 1 : 3;

                if (showSrNo)
                {
                    DataColumn dataColumn = dataTable.Columns.Add("#", typeof(int));
                    dataColumn.SetOrdinal(0);
                    int index = 1;
                    foreach (DataRow item in dataTable.Rows)
                    {
                        item[0] = index;
                        index++;
                    }
                }


                // add the content into the Excel file  
                workSheet.Cells["A" + startRowFrom].LoadFromDataTable(dataTable, true);

                // autofit width of cells with small content  
                int columnIndex = 1;
                foreach (DataColumn column in dataTable.Columns)
                {
                    ExcelRange columnCells = workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.End.Row, columnIndex];
                    int maxLength = columnCells.Max(cell => cell.Value.ToString().Count());
                    if (maxLength < 150)
                    {
                        workSheet.Column(columnIndex).AutoFit();
                    }


                    columnIndex++;
                }

                // format header - bold, yellow on black  
                using (ExcelRange r = workSheet.Cells[startRowFrom, 1, startRowFrom, dataTable.Columns.Count])
                {
                    r.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    r.Style.Font.Bold = true;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#1fb5ad"));
                }

                // format cells - add borders  
                using (ExcelRange r = workSheet.Cells[startRowFrom + 1, 1, startRowFrom + dataTable.Rows.Count, dataTable.Columns.Count])
                {
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                }

                // removed ignored columns  
                for (int i = dataTable.Columns.Count - 1; i >= 0; i--)
                {
                    if (i == 0 && showSrNo)
                    {
                        continue;
                    }
                    if (!columnsToTake.Contains(dataTable.Columns[i].ColumnName))
                    {
                        workSheet.DeleteColumn(i + 1);
                    }
                }

                if (!String.IsNullOrEmpty(heading))
                {
                    workSheet.Cells["A1"].Value = heading;
                    workSheet.Cells["A1"].Style.Font.Size = 20;

                    workSheet.InsertColumn(1, 1);
                    workSheet.InsertRow(1, 1);
                    workSheet.Column(1).Width = 5;
                }

                result = package.GetAsByteArray();
            }

            return result;
        }

        public static byte[] ExportExcel<T>(List<T> data, string Heading = "", bool showSlno = false, params string[] ColumnsToTake)
        {
            return ExportExcel(ListToDataTable<T>(data), Heading, showSlno, ColumnsToTake);
        }

        public FileContentResult ExportToExcel(int id)
        {
            List<CandidateListToExcelModel> technologies = _dal.GetCandidateListForExport(id);
            string[] columns = { "ID Number", "Surname", "First Name", "Cellphone", "Email Address", "Race", "Gender" };
            byte[] filecontent = ExportExcel(technologies, "Technology", true, columns);
            return File(filecontent, ExcelContentType, "Technologies.xlsx");
        }
    }
}