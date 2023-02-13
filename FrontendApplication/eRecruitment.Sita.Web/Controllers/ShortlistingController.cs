using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eRecruitment.Sita.Web.Controllers
{
    public class ShortlistingController : Controller
    {
        eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext();
        eRecruitmentDataAccess _dal = new eRecruitmentDataAccess();
        //eRecruitment.Sita.Web.Notification.SendNotification notify = new eRecruitment.Sita.Web.Notification.SendNotification();      
        Notification notify = new Notification();
        //GET : Shortlisted Candidate

        public ActionResult Shortlisted()
        {
            ViewBag.Shortlisted = _dal.GetShortlistedCandidtes();
            return View();
        }
        //View Short listed Application Detailes
        public ActionResult ViewShortlistedApplicationDetailes(int id)
        {
            ViewBag.Gender = _dal.GetGenderList();
            string useri = User.Identity.GetUserId();
            var Ap = _dal.ApplicationDetailes(id);
            ViewBag.Vacancy = Ap;
            return View(Ap);

        }
        public ActionResult AcceptedForInterview(int id)
        {
            _dal.UpdateApplicationStatus(5, Convert.ToInt16(id));
            return RedirectToAction("Index");
        }


    }
}