using eRecruitment.Sita.Web.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.IO;
using Newtonsoft.Json;
using System.Web.Helpers;
using System.Text;

namespace eRecruitment.Sita.Web.Controllers
{
    public class HomeController : Controller
    {
        eRecruitment.Sita.Web.eRecruitmentDataAccess _dal = new eRecruitment.Sita.Web.eRecruitmentDataAccess();
        eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext();
        string monthDate = DateTime.Now.Month.ToString();
        [Authorize]
        public ActionResult Index()
        {
            string userid = User.Identity.GetUserId();
            int cid = _db.AspNetUserRoles.Where(x => x.UserId == userid).Select(x => x.OrganisationID).FirstOrDefault();
            int totalVacancy = 0;
            int totalApproved = 0;
            int totalRejected = 0;
            int totalWithdrawn = 0;

            if (User.IsInRole("Recruiter"))
            {
                //totalVacancy = _db.tblVacancies.Where(x => x.UserID == userid).Count();
                totalVacancy = (from a in _db.tblVacancies
                                from b in _db.tblFinYears
                                where a.UserID == userid
                                && a.CreatedDate >= b.StartDate && a.CreatedDate <= b.EndDate 
                                select a.ID).Count();

                //totalApproved = _db.tblVacancies.Where(x => x.UserID == userid && x.StatusID == 3).Count();
                totalApproved = (from a in _db.tblVacancies
                                 from b in _db.tblFinYears
                                 where a.UserID == userid
                                 && a.StatusID == 3
                                 && a.CreatedDate >= b.StartDate && a.CreatedDate <= b.EndDate
                                 select a.ID).Count();

                //totalRejected = _db.tblVacancies.Where(x => x.UserID == userid && x.StatusID == 4).Count();
                totalRejected = (from a in _db.tblVacancies
                                 from b in _db.tblFinYears
                                 where a.UserID == userid
                                 && a.StatusID == 4
                                 && a.CreatedDate >= b.StartDate && a.CreatedDate <= b.EndDate
                                 select a.ID).Count();

                //totalWithdrawn = _db.tblVacancies.Where(x => x.UserID == userid && x.StatusID == 5).Count();
                totalWithdrawn = (from a in _db.tblVacancies
                                 from b in _db.tblFinYears
                                 where a.UserID == userid
                                 && a.StatusID == 5
                                 && a.CreatedDate >= b.StartDate && a.CreatedDate <= b.EndDate
                                 select a.ID).Count();
            }
            else if (User.IsInRole("Approver"))
            {
                //totalVacancy = _db.tblVacancies.Where(x => x.OrganisationID == cid).Count();
                totalVacancy = (from a in _db.tblVacancies
                                from b in _db.tblFinYears
                                where a.OrganisationID == cid
                                && a.CreatedDate >= b.StartDate && a.CreatedDate <= b.EndDate
                                select a.ID).Count();

                //totalApproved = _db.tblVacancies.Where(x => x.OrganisationID == cid && x.StatusID == 3).Count();
                totalApproved = (from a in _db.tblVacancies
                                from b in _db.tblFinYears
                                where a.OrganisationID == cid && a.StatusID == 3
                                && a.CreatedDate >= b.StartDate && a.CreatedDate <= b.EndDate
                                select a.ID).Count();

                //totalRejected = _db.tblVacancies.Where(x => x.OrganisationID == cid && x.StatusID == 4).Count();
                totalRejected = (from a in _db.tblVacancies
                                 from b in _db.tblFinYears
                                 where a.OrganisationID == cid && a.StatusID == 4
                                 && a.CreatedDate >= b.StartDate && a.CreatedDate <= b.EndDate
                                 select a.ID).Count();

                //totalWithdrawn = _db.tblVacancies.Where(x => x.OrganisationID == cid && x.StatusID == 5).Count();
                totalWithdrawn = (from a in _db.tblVacancies
                                 from b in _db.tblFinYears
                                 where a.OrganisationID == cid && a.StatusID == 5
                                 && a.CreatedDate >= b.StartDate && a.CreatedDate <= b.EndDate
                                 select a.ID).Count();
            }
            ViewBag.TotalVacancy = totalVacancy;
            ViewBag.TotalApproved = totalApproved;
            ViewBag.TotalRejected = totalRejected;
            ViewBag.TotalWithdrawn = totalWithdrawn;

            return View();
        }

        public ActionResult CreatePie()
        {
            string userid = User.Identity.GetUserId();
            int cid = _db.AspNetUserRoles.Where(x => x.UserId == userid).Select(x => x.OrganisationID).FirstOrDefault();

            var TotalAsian = (from a in _db.tblCandidateVacancyApplications
                              join b in _db.tblProfiles on a.UserID equals b.UserID
                              where b.fkRaceID == 1 && a.ApplicationDate.Month.ToString() == monthDate
                              select new { totalAsian = b.fkRaceID }).Count();

            var TotalBlack = (from a in _db.tblCandidateVacancyApplications
                              join b in _db.tblProfiles on a.UserID equals b.UserID
                              where b.fkRaceID == 2 && a.ApplicationDate.Month.ToString() == monthDate
                              select new { totalBlack = b.fkRaceID }).Count();

            var TotalColoured = (from a in _db.tblCandidateVacancyApplications
                              join b in _db.tblProfiles on a.UserID equals b.UserID
                              where b.fkRaceID == 3 && a.ApplicationDate.Month.ToString() == monthDate
                              select new { totalColoured = b.fkRaceID }).Count();

            var TotalWhite = (from a in _db.tblCandidateVacancyApplications
                                 join b in _db.tblProfiles on a.UserID equals b.UserID
                                 where b.fkRaceID == 4 && a.ApplicationDate.Month.ToString() == monthDate
                                 select new { totalColoured = b.fkRaceID }).Count();

            var TotalOther = (from a in _db.tblCandidateVacancyApplications
                              join b in _db.tblProfiles on a.UserID equals b.UserID
                              where b.fkRaceID == 5 && a.ApplicationDate.Month.ToString() == monthDate
                              select new { totalOther = b.fkRaceID }).Count();

            string[] t1 = { "Asian", "Black", "Coloured", "White", "Other" };

            int[] t2 = { TotalAsian, TotalBlack, TotalColoured, TotalWhite, TotalOther};
            //int[] t2 = { 1, TotalBlack, TotalColoured, 1, 1 };

            //Create bar chart
            var chart = new Chart(width: 400, height: 400)
            .AddTitle("Ethnicity")
            .AddLegend("Race")
            .AddSeries(chartType: "pie",
                            axisLabel: "#VALX (#PERCENT{P0})",
                            xValue: t1,
                            yValues: t2)
                            .GetBytes("png");
            return File(chart, "image/bytes");

        }

        //Create Bar Graph For Applications Per Province 
        public ActionResult CreateBar()
        {
            string userid = User.Identity.GetUserId();
            int cid = _db.AspNetUserRoles.Where(x => x.UserId == userid).Select(x => x.OrganisationID).FirstOrDefault();

            var TotalEC = (from a in _db.tblCandidateVacancyApplications
                              join b in _db.tblProfiles on a.UserID equals b.UserID
                              where b.fkProvinceID == 1 && a.ApplicationDate.Month.ToString() == monthDate
                           select new { totalEC = b.fkProvinceID }).Count();

            var TotalFS = (from a in _db.tblCandidateVacancyApplications
                           join b in _db.tblProfiles on a.UserID equals b.UserID
                           where b.fkProvinceID == 2 && a.ApplicationDate.Month.ToString() == monthDate
                           select new { totalFS = b.fkProvinceID }).Count();

            var TotalGT = (from a in _db.tblCandidateVacancyApplications
                           join b in _db.tblProfiles on a.UserID equals b.UserID
                           where b.fkProvinceID == 3 && a.ApplicationDate.Month.ToString() == monthDate
                           select new { totalGT = b.fkProvinceID }).Count();

            var TotalKZN = (from a in _db.tblCandidateVacancyApplications
                           join b in _db.tblProfiles on a.UserID equals b.UserID
                           where b.fkProvinceID == 4 && a.ApplicationDate.Month.ToString() == monthDate
                           select new { totalKZN = b.fkProvinceID }).Count();

            var TotalLP = (from a in _db.tblCandidateVacancyApplications
                            join b in _db.tblProfiles on a.UserID equals b.UserID
                            where b.fkProvinceID == 5 && a.ApplicationDate.Month.ToString() == monthDate
                            select new { totalLP = b.fkProvinceID }).Count();

            var TotalMP = (from a in _db.tblCandidateVacancyApplications
                           join b in _db.tblProfiles on a.UserID equals b.UserID
                           where b.fkProvinceID == 6 && a.ApplicationDate.Month.ToString() == monthDate
                           select new { totalMP = b.fkProvinceID }).Count();

            var TotalNC = (from a in _db.tblCandidateVacancyApplications
                           join b in _db.tblProfiles on a.UserID equals b.UserID
                           where b.fkProvinceID == 7 && a.ApplicationDate.Month.ToString() == monthDate
                           select new { totalNC = b.fkProvinceID }).Count();

            var TotalNW = (from a in _db.tblCandidateVacancyApplications
                           join b in _db.tblProfiles on a.UserID equals b.UserID
                           where b.fkProvinceID == 8 && a.ApplicationDate.Month.ToString() == monthDate
                           select new { totalNW = b.fkProvinceID }).Count();

            var TotalWC = (from a in _db.tblCandidateVacancyApplications
                           join b in _db.tblProfiles on a.UserID equals b.UserID
                           where b.fkProvinceID == 9 && a.ApplicationDate.Month.ToString() == monthDate
                           select new { totalWC = b.fkProvinceID }).Count();

            string[] t1 = new[] { "Eastern Cape", "Free State", "Gauteng", "KwaZulu-Natal", "Limpopo", "Mpumalanga", "Northern Cape", "North West", "Western Cape" };

            int[] t2 = { TotalEC, TotalFS, TotalGT, TotalKZN, TotalLP, TotalMP, TotalNC, TotalNW, TotalWC };

            //Create bar chart
            var chart = new Chart(width: 400, height: 400)
            .AddTitle("Applications Per Province")
            .AddSeries(chartType: "bar",
                            xValue: t1,
                            yValues: t2)
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult CreatePieGender()
        {
            string userid = User.Identity.GetUserId();
            int cid = _db.AspNetUserRoles.Where(x => x.UserId == userid).Select(x => x.OrganisationID).FirstOrDefault();

            var TotalMale = (from a in _db.tblCandidateVacancyApplications
                           join b in _db.tblProfiles on a.UserID equals b.UserID
                           where b.fkGenderID == 1 && a.ApplicationDate.Month.ToString() == monthDate
                           select new { totalMale = b.fkGenderID }).Count();

            var TotalFemale = (from a in _db.tblCandidateVacancyApplications
                           join b in _db.tblProfiles on a.UserID equals b.UserID
                           where b.fkGenderID == 2 && a.ApplicationDate.Month.ToString() == monthDate
                           select new { totalFemale = b.fkGenderID }).Count();

            string[] t1 = new[] { "Male", "Female"};

            int[] t2 = { TotalMale, TotalFemale};

            //Create bar chart
            var chart = new Chart(width: 300, height: 300)
            .AddTitle("Gender")
            .AddLegend("Gender")
            .AddSeries(chartType: "pie",
                            xValue: t1,
                            yValues: t2)
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult CreateLine()
        {
            string userid = User.Identity.GetUserId();
            int cid = _db.AspNetUserRoles.Where(x => x.UserId == userid).Select(x => x.OrganisationID).FirstOrDefault();

            var TotalDisabled = (from a in _db.tblCandidateVacancyApplications
                             join b in _db.tblProfiles on a.UserID equals b.UserID
                             where b.fkDisabilityID == 1 && a.ApplicationDate.Month.ToString() == monthDate
                             select new { totalDisabled = b.fkDisabilityID }).Count();

            var TotalNotDisabled = (from a in _db.tblCandidateVacancyApplications
                               join b in _db.tblProfiles on a.UserID equals b.UserID
                               where b.fkDisabilityID == 2 && a.ApplicationDate.Month.ToString() == monthDate
                               select new { totalNotDisabled = b.fkDisabilityID }).Count();

            string[] t1 = new[] { "Disabled", "Not Disabled" };

            int[] t2 = { TotalDisabled, TotalNotDisabled };

            //Create bar chart
            var chart = new Chart(width: 300, height: 300)
            .AddTitle("Disability")
            .AddSeries(chartType: "line",
                            xValue: t1,
                           yValues: t2)
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult CreateBarMonth()
        {
            string userid = User.Identity.GetUserId();
            var App = _db.proc_eRecruitmentGetTotalApplicationsPerMonth(userid);
            foreach (var d in App)
            {
                int January = Convert.ToInt16(d.Jan);
                int February = Convert.ToInt16(d.Feb);
                int March = Convert.ToInt16(d.Mar);
                int April = Convert.ToInt16(d.Apr);
                int May = Convert.ToInt16(d.May);
                int June = Convert.ToInt16(d.Jun);
                int July = Convert.ToInt16(d.Jul);
                int August = Convert.ToInt16(d.Aug);
                int September = Convert.ToInt16(d.Sep);
                int October = Convert.ToInt16(d.Oct);
                int November = Convert.ToInt16(d.Nov);
                int December = Convert.ToInt16(d.Dec);

                System.Web.UI.DataVisualization.Charting.Chart chart = new System.Web.UI.DataVisualization.Charting.Chart();
                System.Web.UI.DataVisualization.Charting.ChartArea area = new System.Web.UI.DataVisualization.Charting.ChartArea();
                area.AxisX.IsLabelAutoFit = true;
                area.AxisX.LabelAutoFitStyle = System.Web.UI.DataVisualization.Charting.LabelAutoFitStyles.LabelsAngleStep30;
                area.AxisX.LabelStyle.Enabled = true;
                area.AxisX.Interval = 1;
                area.AxisY.Interval = 1;
                chart.ChartAreas.Add(area);
                chart.Width = 1065;
                chart.Height = 400;
                chart.BackColor = System.Drawing.Color.Azure;
                chart.BackGradientStyle = System.Web.UI.DataVisualization.Charting.GradientStyle.TopBottom;
                chart.BackSecondaryColor = System.Drawing.Color.White;
                chart.BorderColor = System.Drawing.Color.FromArgb(26, 59, 105);
                chart.BorderlineDashStyle = System.Web.UI.DataVisualization.Charting.ChartDashStyle.Solid;
                chart.BorderWidth = 2;
                chart.Palette = System.Web.UI.DataVisualization.Charting.ChartColorPalette.None;
                chart.PaletteCustomColors = new System.Drawing.Color[] { System.Drawing.Color.RoyalBlue };
                System.Web.UI.DataVisualization.Charting.Series mySeries = new System.Web.UI.DataVisualization.Charting.Series();
                mySeries.Points.DataBindXY(
                new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" },
                //new int[] { 1, 2, 3, 4, 5, 6, 7, August, September, 10, 11, 1 }
                new int[] { January, February, March, April, May, June, July, August, September, October, November, December }

                );
                chart.Series.Add(mySeries);
                chart.Titles.Add("Applications Per Month");
                var returnStream = new MemoryStream();
                chart.ImageType = System.Web.UI.DataVisualization.Charting.ChartImageType.Png;
                chart.SaveImage(returnStream);
                returnStream.Position = 0;
                return new FileStreamResult(returnStream, "image/png");
            }

            return View();
           
        }
    public ActionResult Jobs()
    {

      return View();
    }

        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            var url = System.Configuration.ConfigurationManager.AppSettings["LogOutURL"];
            return Redirect(url);
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult About()
        {
            using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
            {
                var result = (from skills in _db.lutGeneralQuestions select skills).ToList();
                if (result != null)
                {
                    ViewBag.mySkills = result.Select(N => new SelectListItem { Text = N.GeneralQuestionDesc, Value = N.id.ToString() });
                }
            }

            //FruitModel fruit = new FruitModel();
            //fruit.Fruits = PopulateFruits();
            return View();

            //ViewBag.QuetionBanks = _dal.GetGeneralQuestionsList();
            //ViewBag.Message = "Your application description page.";

            //return View();
        }

        private static List<SelectListItem> PopulateFruits()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = " SELECT id, GeneralQuestionDesc FROM lutGeneralQuestion";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            items.Add(new SelectListItem
                            {
                                Text = sdr["GeneralQuestionDesc"].ToString(),
                                Value = sdr["id"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }

            return items;
        }


        public ActionResult Blank()
        {

            char[] delimiterChars = { '1', '2', '3', '4', '5', '6','7','8','9' };
            StringBuilder rbuilder = new StringBuilder();

            string text = "SITA is Employment Equity employer and this position will be filled based on" +
                            " Employment Equity Plan.Correspondence will be limited to short listed candidates" +
                            " only. 1. It is the applicant`s responsibility to have foreign qualifications evaluated" +
                            " by the South African Qualifications Authority(SAQA). 2. Only candidate who meet" +
                            " the requirements should apply. 3. SITA reserves a right not to make an appointment." +
                            " 4. Appointment is subject to getting a positive security clearance, the signing of a" +
                            " balance score card contract, verification of the applicants documents(Qualifications)," +
                            " and reference checking. 5. Correspondence will be entered to with shortlisted" +
                            " candidates only. 6. Applications from Recruitment Agencies will not be accepted.";

            System.Diagnostics.Debug.WriteLine($"Original text: '{text}'");
           
            string[] words = text.Split(delimiterChars);
            System.Diagnostics.Debug.WriteLine($"{words.Length} words in text:");
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

            return View();
        }

        public JsonResult GetStatusList()
        {
            ViewBag.Message = "Your application description page.";
            var data = _dal.GetStatusList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public JsonResult EmploymentType()
        {
            ViewBag.Message = "Your application description page.";
            var data = _dal.GetEmploymentTypeList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult StartupPage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult StartupPage(FormCollection fc)
        {
            Session["sip"] = fc["signInOptions"];
            int sip = Convert.ToInt16(Session["sip"]);
            if (User.IsInRole("Admin") || User.IsInRole("SysAdmin"))
            {
                if (User.IsInRole("Admin") && Convert.ToInt16(Session["sip"]) == 1)
                {
                    return this.RedirectToAction("Jobs", "Home");
                }else if (User.IsInRole("Admin") && Convert.ToInt16(Session["sip"]) == 2)
                {
                    return this.RedirectToAction("AllAssignedUsers", "Admin");
                }
                else if (User.IsInRole("SysAdmin"))
                {
                    return this.RedirectToAction("AllAssignedUsers", "Admin");
                }
                
                //return this.RedirectToAction("StartupPage", "Home");
            }else if (User.IsInRole("Recruiter") && Convert.ToInt16(Session["sip"]) == 2)
            {
                return this.RedirectToAction("Index", "Home");
                //return this.RedirectToAction("StartupPage", "Home");
            }else if (User.IsInRole("Recruiter") && Convert.ToInt16(Session["sip"]) == 1)
            {
                return this.RedirectToAction("Jobs", "Home");
                //return this.RedirectToAction("StartupPage", "Home");
            }else if (User.IsInRole("Approver") && Convert.ToInt16(Session["sip"]) == 2)
            {
                return this.RedirectToAction("Index", "Home");
                //return this.RedirectToAction("StartupPage", "Home");
            }else if (User.IsInRole("Approver") && Convert.ToInt16(Session["sip"]) == 1)
            {
                return this.RedirectToAction("Jobs", "Home");
                //return this.RedirectToAction("StartupPage", "Home");
            }
            return View();
        }
    }
}