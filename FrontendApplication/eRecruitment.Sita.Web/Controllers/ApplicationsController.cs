using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eRecruitment.Sita.Web.Models;
using Microsoft.AspNet.Identity;

namespace eRecruitment.Sita.Web.Controllers
{
    public class ApplicationsController : Controller
    {
        eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext();
        eRecruitmentDataAccess _dal = new eRecruitmentDataAccess();
        //eRecruitment.Sita.Web.Notification.SendNotification notify = new eRecruitment.Sita.Web.Notification.SendNotification();
        Notification notify = new Notification();
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Applications
        public ActionResult Index()
        {
            string userid = User.Identity.GetUserId();
            ViewBag.ApplicationList = _dal.GetApplicationsList();
            return View();
        }

        //View Received Application Detailes
        public ActionResult ViewReceivedApplicationDetailes(int id)
        {
            ViewBag.Gender = _dal.GetGenderList();
            string useri = User.Identity.GetUserId();
            var Ap = _dal.ApplicationDetailes(id);
            ViewBag.Vacancy = Ap;
            return View(Ap);

        }

        //DownLoad Attachments
        [HttpGet]
        public FileResult DownLoadAttachements(int id)
        {
            var doc = _db.Attachments.Where(x => x.AttachmentID == id).FirstOrDefault();
            return File(doc.fileData.ToArray(), doc.contentType, doc.fileName);
        }

        //Shortlist Candidate
        public ActionResult shortlist(int id)
        {
            //2 is Shortlisted
            _dal.UpdateApplicationStatus(2, Convert.ToInt16(id));
            return RedirectToAction("Index");
        }
   
    }
}
