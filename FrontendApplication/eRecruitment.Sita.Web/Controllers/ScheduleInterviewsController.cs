using eRecruitment.Sita.Web.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eRecruitment.Sita.Web.Controllers
{
    public class ScheduleInterviewsController : Controller
    {
        eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext();
        eRecruitmentDataAccess _dal = new eRecruitmentDataAccess();
        //eRecruitment.Sita.Web.Notification.SendNotification notify = new eRecruitment.Sita.Web.Notification.SendNotification();
        Notification notify = new Notification();

        // GET: ScheduleInterviews
        public ActionResult ScheduleIndex()
        {
            ViewBag.AcceptedApplicationList = _dal.GetAcceptedApplicationsList();
            return View();
        }

  
        [HttpGet]
        public ActionResult ScheduleInterview()
        {

            ViewBag.InterviewType = _dal.GetInterviewType();
            ViewBag.Location = _dal.GetInterviewLocation();
            ViewBag.Category = _dal.GetInterviewCategory();
            ViewBag.InterviewStatus = _dal.GetInterviewStatus();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ScheduleInterview(int ID, ScheduleInterviews model)
        {
            ViewBag.InterviewType = _dal.GetInterviewType();
            ViewBag.Location = _dal.GetInterviewLocation();
            ViewBag.Category = _dal.GetInterviewLocation();
            ViewBag.InterviewStatus = _dal.GetInterviewStatus();

            return View();
        }
    }
}