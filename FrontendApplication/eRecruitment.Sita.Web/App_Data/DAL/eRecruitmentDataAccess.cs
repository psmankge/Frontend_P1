//==============================Peter - 20221014================================
using eRecruitment.BusinessDomain.DAL.Entities.AppModels;
//==============================================================================
using eRecruitment.Sita.Web.App_Data.DAL;
using eRecruitment.Sita.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eRecruitment.Sita.Web
{
  public class eRecruitmentDataAccess
  {
    eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext();


    // Get Vacancy Profile
    //public List<VacancyProfileModels> GetVacancyProfile(int id)
    //{
    //    var p = new List<VacancyProfileModels>();
    //    using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
    //    {
    //        var data = from a in _db.tblVacancyProfiles
    //                   join b in _db.tblKeyResponsibilityAreas on a.VacancyProfileID equals b.VacancyNameID
    //                   join c in _db.tblVacancyPurposes on a.VacancyProfileID equals c.VacancyNameID
    //                   where a.VacancyProfileID == id
    //                   select new
    //                   {
    //                       VacancyPurpose = c.PurposeDiscription,
    //                       TechnicalCompetenciesDescription = b.KeyResponsibilityAreas
    //                       ,
    //                       Disclaimer = a.Disclaimer
    //                       ,
    //                       Responsibility = a.Responsibility
    //                       ,
    //                       QualificationsandExperience = a.QualificationsandExperience
    //                       ,
    //                       OtherSpecialRequirements = a.OtherSpecialRequirements
    //                   };



    //        foreach (var d in data)
    //        {
    //            VacancyProfileModels e = new VacancyProfileModels();
    //            e.VacancyPurpose = Convert.ToString(d.VacancyPurpose);
    //            e.TechnicalCompetenciesDescription = Convert.ToString(d.TechnicalCompetenciesDescription);
    //            e.Disclaimer = Convert.ToString(d.Disclaimer);
    //            e.QualificationsandExperience = Convert.ToString(d.QualificationsandExperience);
    //            e.Responsibility = Convert.ToString(d.Responsibility);
    //            e.OtherSpecialRequirements = Convert.ToString(d.OtherSpecialRequirements);
    //            p.Add(e);
    //        }
    //        return p;
    //    }
    //}

    //Get Vacancy Type List
    public List<VacancyTypeModel> GetVacancyTypeList()
    {
      var p = new List<VacancyTypeModel>();
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        var VacancyType = _db.lutVacancyTypes.ToList();

        foreach (var d in VacancyType)
        {
          VacancyTypeModel e = new VacancyTypeModel();
          e.VancyTypeID = Convert.ToInt32(d.VancyTypeID);
          e.VacancyTypeName = Convert.ToString(d.VacancyTypeName);
          p.Add(e);
        }
        return p;
      }
    }





    // Get Skills For User Profile
    public List<SkillsModel> SkillsProf(string userid)
    {
      var p = new List<SkillsModel>();
      var skillsData = (from a in _db.tblSkillsProfiles
                       join b in _db.lutSkills on a.skillID equals b.skillID
                       join c in _db.tblProfiles on a.profileID equals c.pkProfileID
                       where c.UserID == userid
                       select new
                       {
                         SkillName = b.skillName
                       }).Take(10);
      if (skillsData != null)
      {
        foreach (var d in skillsData)
        {
          SkillsModel e = new SkillsModel();
          e.skillName = Convert.ToString(d.SkillName);

          p.Add(e);
        }
      }
      return p;
    }


    // Get Salary Range 
    public List<SalaryRangeViewModel> GetSalaryRange(int id)
    {
      var p = new List<SalaryRangeViewModel>();
      var salaryRangeData = from a in _db.lutJobLevels
                            join b in _db.lutOrganisations on a.OrganisationID equals b.OrganisationID
                            join c in _db.tblSalaryRanges on a.JobLevelID equals c.JobLevelID
                            where c.JobLevelID == id
                            select new
                            {
                              SalaryRangeID = c.SalaryRangeID,
                              SalaryRange = c.SalaryRange

                            };
      foreach (var d in salaryRangeData)
      {
        SalaryRangeViewModel e = new SalaryRangeViewModel();
        e.SalaryRangeID = Convert.ToInt32(d.SalaryRangeID);
        e.SalaryRange = Convert.ToString(d.SalaryRange);
        p.Add(e);
      }
      return p;
    }

    //Get Van Name
    public List<CandidateScreeningVacancyModels> GetVacancyName(int id)
    {
      var p = new List<CandidateScreeningVacancyModels>();
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        var data = from a in _db.tblVacancies
                   join b in _db.tblVacancyProfiles
                   on a.VacancyProfileID equals b.VacancyProfileID
                   where a.ID == id
                   select new { a.ID, a.ReferenceNo, VacancyName = b.VacancyName + " - (" + a.ReferenceNo + ")" };

        foreach (var d in data)
        {
          CandidateScreeningVacancyModels e = new CandidateScreeningVacancyModels();
          e.VacancyID = Convert.ToInt32(d.ID);
          e.VacancyName = Convert.ToString(d.VacancyName);
          e.ReferenceNumber = Convert.ToString(d.ReferenceNo);
          p.Add(e);
        }
        return p;
      }
    }

    // Get Get Candidate List For Export
    public List<CandidateListToExcelModel> GetCandidateListForExport(int id)
    {
      var p = new List<CandidateListToExcelModel>();
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        var data = _db.tblScreenedCandidates.Where(x => x.VacancyID == id).ToList();

        foreach (var d in data)
        {
          CandidateListToExcelModel e = new CandidateListToExcelModel();
          e.IDNumber = Convert.ToString(d.IDNumber);
          e.Surname = Convert.ToString(d.Surname);
          e.FirstName = Convert.ToString(d.FirstName);
          e.DateOfBirth = Convert.ToDateTime(d.DateOfBirth).ToString("d");
          e.CellNo = Convert.ToString(d.CellNo);
          e.EmailAddress = Convert.ToString(d.EmailAddress);
          e.RaceName = Convert.ToString(d.RaceName);
          e.Gender = Convert.ToString(d.Gender);
          p.Add(e);
        }
        return p;
      }
    }


    // Get Reject Reason List 
    public List<RejectReasonModel> GetRejectReasonList()
    {
      var p = new List<RejectReasonModel>();
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        var data = _db.lutRejectReasons.ToList();

        foreach (var d in data)
        {
          RejectReasonModel e = new RejectReasonModel();
          e.RejectReasonID = Convert.ToInt32(d.RejectReasonID);
          e.RejectReason = Convert.ToString(d.RejectReason);
          p.Add(e);
        }
        return p;
      }
    }

    // Get Retract Reason List 
    public List<RetractReasonModel> GetRetractReasonList()
    {
      var p = new List<RetractReasonModel>();
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        var data = _db.lutRetractReasons.ToList();

        foreach (var d in data)
        {
          RetractReasonModel e = new RetractReasonModel();
          e.RetractReasonID = Convert.ToInt32(d.RetractReasonID);
          e.RetractReason = Convert.ToString(d.RetractReason);
          p.Add(e);
        }
        return p;
      }
    }

    // Get Withdrawal Reason List 
    public List<WithdrawalReasonModel> GetWithdrawalReasonList()
    {
      var p = new List<WithdrawalReasonModel>();
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        var data = _db.lutWithdrawalReasons.ToList();

        foreach (var d in data)
        {
          WithdrawalReasonModel e = new WithdrawalReasonModel();
          e.WithdrawalReasonID = Convert.ToInt32(d.WithdrawalReasonID);
          e.WithdrawalReason = Convert.ToString(d.WithdrawalReason);
          p.Add(e);
        }
        return p;
      }
    }


    // Get Vacancy Division
    public List<VacancyDivisionModel> GetVacancyDivision()
    {
      var p = new List<VacancyDivisionModel>();
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        var Roles = _db.lutDivisions.ToList();

        foreach (var d in Roles)
        {
          VacancyDivisionModel e = new VacancyDivisionModel();
          e.DivisionID = Convert.ToInt32(d.DivisionID);
          e.DivisionDiscription = Convert.ToString(d.DivisionDiscription);
          p.Add(e);
        }
        return p;
      }
    }

    public List<MethodOfCummunicationModel> GetMethodOfCommunication()
    {
      var p = new List<MethodOfCummunicationModel>();
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        var Roles = _db.lutMethodOfCommunications.ToList();

        foreach (var d in Roles)
        {
          MethodOfCummunicationModel e = new MethodOfCummunicationModel();
          e.MethodID = Convert.ToInt32(d.MethodID);
          e.MethodName = Convert.ToString(d.MethodOfCommunication);
          p.Add(e);
        }
        return p;
      }
    }

    public int GetTotalCandidateVacancyApplications(string uid)
    {
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        var data = (from a in _db.tblCandidateVacancyApplications
                    join b in _db.tblVacancies on a.VacancyID equals b.ID
                    join e in _db.lutJobTitles on b.JobTitleID equals e.JobTitleID
                    join d in _db.lutOrganisations on b.OrganisationID equals d.OrganisationID
                    where a.UserID == uid
                    select a.ApplicationID).Count();

        return data;
      }
    }

    public List<CandidateJobApplication> GetCandidateVacancyApplications(string uid)
    {
      var p = new List<CandidateJobApplication>();
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        var data = (from a in _db.tblCandidateVacancyApplications
                    join b in _db.tblVacancies on a.VacancyID equals b.ID
                    join e in _db.lutJobTitles on b.JobTitleID equals e.JobTitleID
                    join d in _db.lutOrganisations on b.OrganisationID equals d.OrganisationID
                    where a.UserID == uid
                    select new { a.ApplicationID, e.JobTitle, a.ApplicationDate, b.ReferenceNo, b.ID, d.OrganisationName }).ToList();

        foreach (var d in data)
        {
          CandidateJobApplication e = new CandidateJobApplication();
          e.ApplicationID = Convert.ToInt32(d.ApplicationID);
          e.VacancyName = Convert.ToString(d.JobTitle);
          e.ApplicationDate = Convert.ToString(d.ApplicationDate);
          e.Refno = Convert.ToString(d.ReferenceNo);
          e.VacancyID = Convert.ToInt32(d.ID);
          e.OrganisationName = Convert.ToString(d.OrganisationName);
          p.Add(e);
        }
        return p;
      }
    }

    //Get Application Status

    public List<ApplicationStatusModel> GetApplicatinStatusList()
    {
      var p = new List<ApplicationStatusModel>();
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        var Roles = _db.lutApplicantStatus;

        foreach (var d in Roles)
        {
          ApplicationStatusModel e = new ApplicationStatusModel();
          e.StatusID = Convert.ToInt32(d.StatusID);
          e.StatusDescription = Convert.ToString(d.StatusDescription);
          p.Add(e);
        }
        return p;
      }
    }

    public List<ScreenedCandidateModel> GetScreenedCandidateList(string VacancyID, string ProvinceID, string GenderID
        , string RaceID, int Disability, int @CVAttached, int IDAttached, int AgeRange, string questionBank)

    {
      var p = new List<ScreenedCandidateModel>();
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        var data = _db.proc_eRecruitmentGetScreenedCandidateList(VacancyID, ProvinceID, GenderID, RaceID, Disability
            , CVAttached, IDAttached, AgeRange, questionBank).ToList();

        foreach (var d in data)
        {
          ScreenedCandidateModel e = new ScreenedCandidateModel();
          e.VacancyID = Convert.ToInt32(d.VacancyID);
          e.ApplicationID = Convert.ToInt32(d.ApplicationID);
          e.UserID = Convert.ToString(d.UserID);
          e.IDNumber = Convert.ToString(d.IDNumber);
          e.Surname = Convert.ToString(d.Surname);
          e.FirstName = Convert.ToString(d.FirstName);
          e.DateOfBirth = Convert.ToString(d.DateOfBirth);
          e.CellNo = Convert.ToString(d.CellNo);
          e.EmailAddress = Convert.ToString(d.EmailAddress);
          e.RaceName = Convert.ToString(d.RaceName);
          e.Gender = Convert.ToString(d.Gender);
          p.Add(e);
        }
        return p;
      }
    }

    //GetStatusList
    public List<StatusModel> GetStatusList()
    {
      var p = new List<StatusModel>();
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        var Roles = _db.lutStatus.ToList();

        foreach (var d in Roles)
        {
          StatusModel e = new StatusModel();
          e.StatusID = Convert.ToInt32(d.StatusID);
          e.StatusDescription = Convert.ToString(d.StatusDescription);
          p.Add(e);
        }
        return p;
      }
    }
    //Get Interview Category

    public List<InterviewCategory> GetInterviewCategory()
    {
      var p = new List<InterviewCategory>();
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        var Roles = _db.lutInterviewCategories.ToList();

        foreach (var d in Roles)
        {
          InterviewCategory e = new InterviewCategory();
          e.InterviewCatID = Convert.ToInt32(d.InterviewCatID);
          e.InterviewCatDescription = Convert.ToString(d.InterviewCatDescription);
          p.Add(e);
        }
        return p;
      }
    }

    //Get Interview Type
    public List<InterviewType> GetInterviewType()
    {
      var p = new List<InterviewType>();
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        var Roles = _db.lutInterviewTypes.ToList();

        foreach (var d in Roles)
        {
          InterviewType e = new InterviewType();
          e.InterviewTypeID = Convert.ToInt32(d.InterviewTypeID);
          e.InterviewTypeDescription = Convert.ToString(d.InterviewTypeDescription);
          p.Add(e);
        }
        return p;
      }
    }
    //Get Interview Status
    public List<InterviewStatus> GetInterviewStatus()
    {
      var p = new List<InterviewStatus>();
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        var Roles = _db.lutInterviewStatus.ToList();

        foreach (var d in Roles)
        {
          InterviewStatus e = new InterviewStatus();
          e.InterviewStatusID = Convert.ToInt32(d.InterviewStatusID);
          e.InterviewStatusDescription = Convert.ToString(d.InterviewStatusDescription);
          p.Add(e);
        }
        return p;
      }
    }

    //Get Location 

    public List<Location> GetInterviewLocation()
    {
      var p = new List<Location>();
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        var Roles = _db.lutLocations.ToList();

        foreach (var d in Roles)
        {
          Location e = new Location();
          e.LocationID = Convert.ToInt32(d.LocationID);
          e.LocationDiscription = Convert.ToString(d.LocationDiscription);
          p.Add(e);
        }
        return p;
      }
    }
    public List<UserRoleModel> GetRoleList()
    {
      var p = new List<UserRoleModel>();

      var Roles = _db.AspNetRoles.ToList();
      foreach (var d in Roles)
      {
        UserRoleModel e = new UserRoleModel();
        e.RoleID = Convert.ToInt32(d.Id);
        e.RoleName = Convert.ToString(d.Name);
        p.Add(e);
      }
      return p;

    }

    public List<UserListModel> GetUserList()
    {
      var p = new List<UserListModel>();

      var Roles = from a in _db.AspNetUsers
                  join b in _db.tblProfiles on a.Id equals b.UserID
                  select new { UserID = a.Id, FullNames = b.FirstName + ' ' + b.Surname, EmailAddress = a.Email };

      foreach (var d in Roles)
      {
        //if ()
        UserListModel e = new UserListModel();
        e.UserID = Convert.ToString(d.UserID);
        e.FullName = Convert.ToString(d.FullNames);
        e.EmailAddress = Convert.ToString(d.EmailAddress);
        p.Add(e);
      }
      return p;

    }


    //GetUserList
    public List<UserListModel> GetUserList(string userid, string role)
    {
      var p = new List<UserListModel>();

      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        var data = (dynamic)null;
        int orgid = _db.AspNetUserRoles.Where(x => x.UserId == userid).Select(x => x.OrganisationID).FirstOrDefault();

        if (role == "Admin")
        {
          data = from a in _db.AspNetUsers
                 join b in _db.tblProfiles on a.Id equals b.UserID
                 join c in _db.AspNetUserRoles on a.Id equals c.UserId
                 where b.UserID != userid && c.OrganisationID == orgid
                 select new { UserID = a.Id, FullNames = b.FirstName + ' ' + b.Surname, EmailAddress = a.Email };
        }
        else if (role == "SysAdmin")
        {
          data = from a in _db.AspNetUsers
                 join b in _db.tblProfiles on a.Id equals b.UserID
                 where b.UserID != userid
                 select new { UserID = a.Id, FullNames = b.FirstName + ' ' + b.Surname, EmailAddress = a.Email };
        }



        foreach (var d in data)
        {
          UserListModel e = new UserListModel();
          e.UserID = Convert.ToString(d.UserID);
          e.FullName = Convert.ToString(d.FullNames);
          e.EmailAddress = Convert.ToString(d.EmailAddress);
          p.Add(e);
        }
        return p;
      }
    }

    //Get All Users 
    public List<ProfileViewModels> GetAllUsers()
    {
      var p = new List<ProfileViewModels>();


      var data = from a in _db.AspNetUsers
                 join b in _db.tblProfiles on a.Id equals b.UserID
                 //join c in _db.AspNetUserRoles on a.Id equals c.UserId
                 //where Convert.ToInt32(c.RoleId) != 1 && Convert.ToInt32(c.RoleId) != 2 && Convert.ToInt32(c.RoleId) != 3
                 //&& Convert.ToInt32(c.RoleId) != 5 && Convert.ToInt32(c.RoleId) != 6
                 select new
                 {
                   UserID = a.Id,
                   Fullname = b.FirstName,
                   Surname = b.Surname,
                   Email = b.EmailAddress,
                   //RoleName = c.Name,

                 };

      foreach (var d in data)
      {
        ProfileViewModels e = new ProfileViewModels();
        e.UserID = Convert.ToString(d.UserID);
        e.FullName = Convert.ToString(d.Fullname);
        e.Surname = Convert.ToString(d.Surname);
        e.Email = Convert.ToString(d.Email);
        //e.RoleName = Convert.ToString(d.RoleName);
        p.Add(e);
      }
      return p;

    }

    public List<CandidateEmailListModel> GetCandidateEmailList(string userid)
    {
      int profileid = GetProfileID(userid);
      var p = new List<CandidateEmailListModel>();
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        var data = _db.tblEmails.Where(x => x.ProfileID == profileid).ToList();

        foreach (var d in data)
        {
          CandidateEmailListModel e = new CandidateEmailListModel();
          e.EmailID = Convert.ToInt32(d.pkEmailID);
          e.FromEmail = Convert.ToString("e-Recruitment Team");
          e.EmailSubject = Convert.ToString(d.sEmailSubject);
          e.EmailDate = Convert.ToString(d.CreatedOn);
          e.StatusID = Convert.ToInt32(d.StatusID);
          e.EmailTypeID = Convert.ToInt32(d.EmailTypeID);
          p.Add(e);
        }
        return p;
      }
    }

    //GetOrganisationList
    public List<OrganisationModel> GetOrganisationList(string userid)
    {
      var p = new List<OrganisationModel>();
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        //var data = _db.lutOrganisations.ToList();
        var data = from a in _db.lutOrganisations
                   join b in _db.AspNetUserRoles on a.OrganisationID equals b.OrganisationID
                   where b.UserId == userid
                   select a;

        foreach (var d in data)
        {
          OrganisationModel e = new OrganisationModel();
          e.OrganisationID = Convert.ToInt32(d.OrganisationID);
          e.OrganisationCode = Convert.ToString(d.OrganisationCode);
          e.OrganisationName = Convert.ToString(d.OrganisationName);
          p.Add(e);
        }
        return p;
      }
    }
    public List<OrganisationModel> GetOrganisationList()
    {
      var p = new List<OrganisationModel>();
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        var data = _db.lutOrganisations.ToList();
        foreach (var d in data)
        {
          OrganisationModel e = new OrganisationModel();
          e.OrganisationID = Convert.ToInt32(d.OrganisationID);
          e.OrganisationCode = Convert.ToString(d.OrganisationCode);
          e.OrganisationName = Convert.ToString(d.OrganisationName);
          p.Add(e);
        }
        return p;
      }
    }

    public List<DepartmentManagerModel> GetDepartmentalManagerList(int orgid)
    {
      var p = new List<DepartmentManagerModel>();
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        //var data = _db.lutOrganisations.ToList();
        var data = from a in _db.AspNetUsers
                   join b in _db.AspNetUserRoles on a.Id equals b.UserId
                   join c in _db.tblProfiles on a.Id equals c.UserID
                   where b.OrganisationID == orgid //&& b.RoleId == "2"
                   select new
                   {
                     UserID = a.Id,
                     FullName = c.FirstName + " " + c.Surname
                   };

        foreach (var d in data)
        {
          DepartmentManagerModel e = new DepartmentManagerModel();
          e.UserID = Convert.ToString(d.UserID);
          e.ManagerName = Convert.ToString(d.FullName);
          p.Add(e);
        }
        return p;
      }
    }

    //Get Quetions banks Per ORG
    public List<QuetionBanksModel> GetQuestionBanksList(string userid)
    {
      var p = new List<QuetionBanksModel>();
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {

        var data = from a in _db.lutGeneralQuestions
                   join b in _db.AspNetUserRoles on a.OrganisationID equals b.OrganisationID
                   join c in _db.lutOrganisations on a.OrganisationID equals c.OrganisationID
                   where b.UserId == userid
                   select new
                   {
                     id = a.id,
                     GeneralQuestionDesc = a.GeneralQuestionDesc,
                     OrganisationName = c.OrganisationName
                   };

        foreach (var d in data)
        {
          QuetionBanksModel e = new QuetionBanksModel();
          e.id = Convert.ToInt32(d.id);
          e.GeneralQuestionDesc = Convert.ToString(d.GeneralQuestionDesc);
          e.OrganisationName = Convert.ToString(d.OrganisationName);
          p.Add(e);
        }
        return p;
      }
    }


    public List<QuetionBanksModel> GetVacancyQuestionBanksListByID(int id)
    {
      var p = new List<QuetionBanksModel>();
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        var data = _db.proc_eRecruitmentGetVacancyQuestion(id);
        foreach (var d in data)
        {
          QuetionBanksModel e = new QuetionBanksModel();
          e.id = Convert.ToInt32(d.QuestionID);
          e.GeneralQuestionDesc = Convert.ToString(d.GeneralQuestionDesc);
          p.Add(e);
        }
        return p;
      }
    }

    //Get Division
    public List<DivisionModel> GetDivisionList(string userid)
    {
      var p = new List<DivisionModel>();
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {

        var data = from a in _db.lutDivisions
                   join b in _db.AspNetUserRoles on a.OrganisationID equals b.OrganisationID
                   join c in _db.lutOrganisations on a.OrganisationID equals c.OrganisationID
                   where b.UserId == userid
                   select new
                   {
                     DivisionID = a.DivisionID,
                     DivitionName = a.DivisionDiscription,
                     OrganisationName = c.OrganisationName
                   };

        foreach (var d in data)
        {
          DivisionModel e = new DivisionModel();
          e.DivisionID = Convert.ToInt32(d.DivisionID);
          e.DivisionDiscription = Convert.ToString(d.DivitionName);
          e.OrganisationName = Convert.ToString(d.OrganisationName);
          p.Add(e);
        }
        return p;
      }
    }
    //GetAllOrganisationList
    public List<OrganisationModel> GetAllOrganisationList(string userid, string role)
    {
      var p = new List<OrganisationModel>();
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        var data = (dynamic)null;

        if (role == "Admin")
        {
          data = from a in _db.lutOrganisations
                 join b in _db.AspNetUserRoles on a.OrganisationID equals b.OrganisationID
                 where b.UserId == userid
                 select a;
        }
        else if (role == "SysAdmin")
        {
          data = _db.lutOrganisations.Where(x => x.OrganisationID != 0).ToList();
        }


        //var data = _db.lutOrganisations.Where(x=>x.OrganisationID != 0).ToList();

        foreach (var d in data)
        {
          OrganisationModel e = new OrganisationModel();
          e.OrganisationID = Convert.ToInt32(d.OrganisationID);
          e.OrganisationCode = Convert.ToString(d.OrganisationCode);
          e.OrganisationName = Convert.ToString(d.OrganisationName);
          e.FileName = Convert.ToString(d.fileName);
          p.Add(e);
        }
        return p;
      }
    }
    //GetGeneralQuestionsList
    public List<GeneralQuestionsModel> GetGeneralQuestionsList()
    {
      var p = new List<GeneralQuestionsModel>();
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        var data = _db.lutGeneralQuestions.ToList();
        foreach (var d in data)
        {
          GeneralQuestionsModel e = new GeneralQuestionsModel();
          e.QuestionID = Convert.ToInt32(d.id);
          e.QuestionDesc = Convert.ToString(d.GeneralQuestionDesc);
          //e.TypeID = Convert.ToInt32(d.TypeID);
          p.Add(e);
        }
        return p;
      }
    }

    //GetApprovedVacancyListForCandidates
    public List<VacancyListModels> GetApprovedVacancyListForCandidates(int id)
    {
      var p = new List<VacancyListModels>();
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        var data = _db.GetApprovedVacancyListForCandidates(id);

        // search for vacancy that match the item and add to list --- p
        foreach (var d in data)
        {
          VacancyListModels e = new VacancyListModels();
          e.ID = Convert.ToInt32(d.ID);
          e.ReferenceNo = Convert.ToString(d.ReferenceNo);
          e.JobTitle = Convert.ToString(d.JobTitle);
          e.Department = Convert.ToString(d.DepartmentDiscription);
          e.Location = Convert.ToString(d.LocationDiscription);
          e.EmploymentType = Convert.ToString(d.EmploymentType);
          e.Salary = Convert.ToString(d.SalaryRange);
          e.CreatedDate = Convert.ToDateTime(d.CreatedDate).ToShortDateString();
          e.ClosingDate = Convert.ToDateTime(d.ClosingDate).ToShortDateString();
          p.Add(e);
        }

        return p;
      }
    }

    public int GetTotalApprovedVacancyListForCandidates(int id)
    {
      
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        var d = _db.GetTotalApprovedVacancyListForCandidates(id).FirstOrDefault();

        int totalJobs = (int)d.TotalJobs;

        return totalJobs;

      }
    }


    //Get Job Level List per ORG 
    // A user belongs To a certain Organisation
    public List<JobLevelViewModel> GetJobLevelPerOrgList(string userid)
    {
      var p = new List<JobLevelViewModel>();
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {

        var data = from a in _db.lutJobLevels
                   join b in _db.AspNetUserRoles on a.OrganisationID equals b.OrganisationID
                   join c in _db.lutOrganisations on a.OrganisationID equals c.OrganisationID
                   where b.UserId == userid
                   select new
                   {
                     JobLevelID = a.JobLevelID,
                     //   Descr = a.Descr

                   };

        foreach (var d in data)
        {
          JobLevelViewModel e = new JobLevelViewModel();
          e.JobLevelID = Convert.ToInt32(d.JobLevelID);
          // e.Descr = Convert.ToString(d.Descr);
          p.Add(e);
        }
        return p;
      }
    }


    // Get Vacancy Profile
    public List<ProfileViewModels> GetAllAssignedUsers(string userid, string role)
    {
      var p = new List<ProfileViewModels>();
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        var data = (dynamic)null;

        if (role == "Admin")
        {
          data = from a in _db.AspNetUserRoles
                 join b in _db.AspNetUsers on a.UserId equals b.Id
                 join c in _db.AspNetRoles on a.RoleId equals c.Id
                 join d in _db.tblProfiles on a.UserId equals d.UserID
                 join e in _db.lutOrganisations on a.OrganisationID equals e.OrganisationID
                 where b.Id != userid && c.Name != "SysAdmin" && a.UserStatusID == 1
                 select new
                 {
                   UserID = a.UserId,
                   Fullname = d.FirstName + ' ' + d.Surname,
                   RoleName = c.Name,
                   OrganisationName = e.OrganisationName
                 };
        }
        else if (role == "SysAdmin")
        {
          data = from a in _db.AspNetUserRoles
                 join b in _db.AspNetUsers on a.UserId equals b.Id
                 join c in _db.AspNetRoles on a.RoleId equals c.Id
                 join d in _db.tblProfiles on a.UserId equals d.UserID
                 join e in _db.lutOrganisations on a.OrganisationID equals e.OrganisationID
                 where c.Name != "SysAdmin" && a.UserStatusID == 1
                 select new
                 {
                   UserID = a.UserId,
                   Fullname = d.FirstName + ' ' + d.Surname,
                   RoleName = c.Name,
                   OrganisationName = e.OrganisationName
                 };
        }

        foreach (var d in data)
        {
          ProfileViewModels e = new ProfileViewModels();
          e.UserID = Convert.ToString(d.UserID);
          e.FullName = Convert.ToString(d.Fullname);
          e.RoleName = Convert.ToString(d.RoleName);
          e.Organisation = Convert.ToString(d.OrganisationName);
          p.Add(e);
        }
        return p;
      }
    }


    public List<ProfileViewModels> GetAllDeactivatedUsers(string userid, string role)
    {
      var p = new List<ProfileViewModels>();
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        var data = (dynamic)null;

        if (role == "Admin")
        {
          data = from a in _db.AspNetUserRoles
                 join b in _db.AspNetUsers on a.UserId equals b.Id
                 join c in _db.AspNetRoles on a.RoleId equals c.Id
                 join d in _db.tblProfiles on a.UserId equals d.UserID
                 join e in _db.lutOrganisations on a.OrganisationID equals e.OrganisationID
                 where b.Id != userid && c.Name != "SysAdmin" && a.UserStatusID == 2
                 select new
                 {
                   UserID = a.UserId,
                   Fullname = d.FirstName,
                   Surname = d.Surname,
                   Email = d.EmailAddress,
                   RoleName = c.Name,
                   OrganisationName = e.OrganisationName
                 };
        }
        else if (role == "SysAdmin")
        {
          data = from a in _db.AspNetUserRoles
                 join b in _db.AspNetUsers on a.UserId equals b.Id
                 join c in _db.AspNetRoles on a.RoleId equals c.Id
                 join d in _db.tblProfiles on a.UserId equals d.UserID
                 join e in _db.lutOrganisations on a.OrganisationID equals e.OrganisationID
                 where b.Id != userid && c.Name != "SysAdmin" && a.UserStatusID == 2
                 select new
                 {
                   UserID = a.UserId,
                   Fullname = d.FirstName,
                   Surname = d.Surname,
                   Email = d.EmailAddress,
                   RoleName = c.Name,
                   OrganisationName = e.OrganisationName,
                   UserStatusID = a.UserStatusID
                 };
          int test = data.UserStatusID;
        }

        foreach (var d in data)
        {
          ProfileViewModels e = new ProfileViewModels();
          e.UserID = Convert.ToString(d.UserID);
          e.FullName = Convert.ToString(d.Fullname);
          e.Surname = d.Surname;
          e.Email = d.Email;
          e.RoleName = Convert.ToString(d.RoleName);
          e.Organisation = Convert.ToString(d.OrganisationName);
          p.Add(e);
        }
        return p;
      }
    }

    //Insert Quetions Bank
    public void InsertIntoQuestionBanks(string GeneralQuestionDesc, int OrganisationID)
    {
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentAddQuestionBank(GeneralQuestionDesc, OrganisationID);
      }
    }
    //Update Quetions Bank
    public void UpdateIntoQuetionsBank(int id, string GeneralQuestionDesc, int OrganisationID)
    {
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentUpdateQuestionBank(id, GeneralQuestionDesc, OrganisationID);
      }
    }
    //Delete Quetion Bank
    public void DeleteIntoGeneralQuestion(int id)
    {
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentDeleteQuetionBank(id);
      }
    }

    //Insert Job Level
    public void InsertIntoJobLevel(int OrganisationID, string Descr)
    {
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentAddJobLevel(OrganisationID, Descr);
      }
    }

    //Delete Job Level
    public void DeleteJobLevel(int id)
    {
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentDeleteJobLevel(id);
      }
    }

    //Insert Salary Bank
    public void InsertIntoSalaryBank(int JobLevelID, string SalaryRange)
    {
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentAddSalaryBank(JobLevelID, SalaryRange);
      }
    }

    //Update Salary Bank
    public void UpdateIntoSalaryBank(int salaryRangeID, int JobLevelID, string SalaryRange)
    {
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentUpdateSalaryBank(salaryRangeID, JobLevelID, SalaryRange);
      }
    }

    //Delete Salary Bank
    public void DeleteIntoSalaryBank(int salaryRangeID)
    {
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentDeleteSalaryBank(salaryRangeID);
      }
    }

    //Insert Division
    public void InsertIntoDivision(string Division, int OrganisationID)
    {
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentAddDivision(Division, OrganisationID);
      }
    }

    //Update Division
    public void UpdateIntoDivision(int id, string DivisionName, int OrganisationID)
    {
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentUpdateDivision(id, DivisionName, OrganisationID);
      }
    }

    //Delete Division
    public void DeleteIntoDivision(int id)
    {
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentDeleteDivision(id);
      }
    }
    //Insert DepartmentName
    public void InsertIntoDepartment(string DepartmentName, int OrganisationID, string managerID)
    {
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentAddDepartment(DepartmentName, OrganisationID, managerID);
      }
    }

    //Update DepartmentName
    public void UpdateIntoDepartment(int id, string DepartmentName, int OrganisationID, string ManagerID)
    {
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentUpdateDepartment(id, DepartmentName, OrganisationID, ManagerID);
      }
    }

    //Delete DepartmentName
    public void DeleteIntoDepartment(int id)
    {
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentDeleteDepartment(id);
      }
    }
    //Get Department List Per ORg
    public List<DepartmentModel> GetDepartmentForEdit(int id)
    {
      var p = new List<DepartmentModel>();
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {

        var data = _db.lutDepartments.Where(x => x.DepartmentID == id).ToList();

        foreach (var d in data)
        {
          DepartmentModel e = new DepartmentModel();
          e.DepartmentID = Convert.ToInt32(d.DepartmentID);
          e.DepartmentName = Convert.ToString(d.DepartmentDiscription);
          //e.OrganisationID = Convert.ToInt32(d.);
          //e.ManagerID = Convert.ToString(d.man);
          p.Add(e);
        }
        return p;
      }
    }

    //get Division per ORG
    public List<DivisionModel> GetDivisionForEdit(int id)
    {
      var p = new List<DivisionModel>();
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {

        var data = _db.lutDivisions.Where(x => x.DivisionID == id).ToList();

        foreach (var d in data)
        {
          DivisionModel e = new DivisionModel();
          e.DivisionID = Convert.ToInt32(d.DivisionID);
          e.DivisionDiscription = Convert.ToString(d.DivisionDiscription);
          e.OrganisationID = Convert.ToInt32(d.OrganisationID);
          p.Add(e);
        }
        return p;
      }
    }

    //Get QuestionBank For Edit
    public List<QuetionBanksModel> GetQuestionBankForEdit(int ID)
    {
      var p = new List<QuetionBanksModel>();
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        var data = _db.lutGeneralQuestions.Where(x => x.id == ID).ToList();
        foreach (var d in data)
        {
          QuetionBanksModel e = new QuetionBanksModel();
          e.id = Convert.ToInt32(d.id);
          e.GeneralQuestionDesc = Convert.ToString(d.GeneralQuestionDesc);
          e.OrganisationID = Convert.ToInt32(d.OrganisationID);
          p.Add(e);
        }
        return p;
      }
    }

    //Get SalaryBank For Edit
    public List<SalaryRangeModel> GetSalaryBankForEdit(int salaryRangeID)
    {
      var p = new List<SalaryRangeModel>();
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        var data = _db.tblSalaryRanges.Where(x => x.SalaryRangeID == salaryRangeID).ToList();
        foreach (var d in data)
        {
          SalaryRangeModel e = new SalaryRangeModel();
          e.SalaryRangeID = Convert.ToInt32(d.SalaryRangeID);
          e.SalaryRange = Convert.ToString(d.SalaryRange);
          e.JobLevelID = Convert.ToInt32(d.JobLevelID);
          p.Add(e);
        }
        return p;
      }
    }

    //Insert Vacancy Profile
    public void InsertIntoVacancyProfile(string VacancyName, string VacancyPurpose, string Responsibility, string QualificationsandExperience, string TechnicalCompetenciesDescription, string OtherSpecialRequirements, string Disclaimer, int OrganisationID)
    {
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentInsertIntoVacancyProfile(VacancyName, VacancyPurpose, Responsibility, QualificationsandExperience, TechnicalCompetenciesDescription, OtherSpecialRequirements, Disclaimer, OrganisationID);
      }
    }

    //Get Vacancy Profile For Edit
    //public List<VacancyProfileModels> GetVacancyProfileForEdit(int id)
    //{
    //    var p = new List<VacancyProfileModels>();
    //    using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
    //    {

    //        var data = _db.tblVacancyProfiles.Where(x => x.VacancyProfileID == id).ToList();

    //        foreach (var d in data)
    //        {
    //            VacancyProfileModels e = new VacancyProfileModels();
    //            e.VacancyProfileID = Convert.ToInt32(d.VacancyProfileID);
    //            e.VacancyName = Convert.ToString(d.VacancyName);
    //            e.VacancyPurpose = Convert.ToString(d.VacancyPurpose);
    //            e.Responsibility = Convert.ToString(d.Responsibility);
    //            e.QualificationsandExperience = Convert.ToString(d.QualificationsandExperience);
    //            e.TechnicalCompetenciesDescription = Convert.ToString(d.TechnicalCompetenciesDescription);
    //            e.OtherSpecialRequirements = Convert.ToString(d.OtherSpecialRequirements);
    //            e.Disclaimer = Convert.ToString(d.Disclaimer);
    //            e.OrganisationID = Convert.ToInt32(d.OrganasationID);
    //            p.Add(e);
    //        }
    //        return p;
    //    }
    //}

    //Delete Vacancy Profile
    public void DeleteVacancyProfile(int id)
    {
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentDeleteVacancyProfile(id);
      }
    }

    //Update Vacancy Profile
    public void UpdateVacancyProfile(int VacancyProfileID, string VacancyName, string VacancyPurpose, string Responsibility, string QualificationsandExperience, string TechnicalCompetenciesDescription, string OtherSpecialRequirements, string Disclaimer, int OrganisationID)
    {
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentUpdateVacancyProfile(VacancyProfileID, VacancyName, VacancyPurpose, Responsibility, QualificationsandExperience, TechnicalCompetenciesDescription, OtherSpecialRequirements, Disclaimer, OrganisationID);
      }
    }

    //GetRoleList
    public List<UserRoleModel> GetRoleList(string userid, string role)
    {
      var p = new List<UserRoleModel>();
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        var data = (dynamic)null;

        if (role == "Admin")
        {
          data = _db.AspNetRoles.Where(x => x.Name != "SysAdmin" && x.Name != "Citizen" && x.Name != "Admin").ToList();
        }
        else if (role == "SysAdmin")
        {
          data = _db.AspNetRoles.Where(x => x.Name != "SysAdmin" && x.Name != "Citizen").ToList();
        }

        //var Roles = _db.AspNetRoles.Where(x=>x.Name != "SysAdmin" && x.Name != "Citizen").ToList();
        foreach (var d in data)
        {
          UserRoleModel e = new UserRoleModel();
          e.RoleID = Convert.ToInt32(d.Id);
          e.RoleName = Convert.ToString(d.Name);
          p.Add(e);
        }
        return p;
      }
    }
    //GetEmploymentTypeList
    public List<EmploymentTypeModel> GetEmploymentTypeList()
    {
      var p = new List<EmploymentTypeModel>();
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        var Roles = _db.lutEmployementTypes.ToList();
        foreach (var d in Roles)
        {
          EmploymentTypeModel e = new EmploymentTypeModel();
          e.EmploymentTypeID = Convert.ToInt32(d.id);
          e.EmploymentType = Convert.ToString(d.EmploymentType);
          p.Add(e);
        }
        return p;
      }
    }
    //GetVacancyListByUser
    public List<VacancyListModels> GetVacancyListByUser(string userid)
    {
      var p = new List<VacancyListModels>();
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        var vacancy = from a in _db.tblVacancies
                      join b in _db.lutStatus on a.StatusID equals b.StatusID
                      join c in _db.lutEmployementTypes on a.EmploymentTypeID equals c.id
                      join d in _db.lutOrganisations on a.OrganisationID equals d.OrganisationID
                      join e in _db.lutDivisions on a.DivisionID equals e.DivisionID
                      join f in _db.lutDepartments on a.DepartmentID equals f.DepartmentID
                      join g in _db.tblVacancyProfiles on a.VacancyProfileID equals g.VacancyProfileID
                      where a.UserID == userid && a.ClosingDate >= DateTime.Now
                      orderby a.CreatedDate descending
                      select new
                      {
                        ID = a.ID,
                        ReferenceNo = a.ReferenceNo,
                        //VacancyName = a.VacancyName,
                        JobTitle = g.VacancyName,
                        EmploymentType = c.EmploymentType,
                        Organisation = d.OrganisationName,
                        CreatedDate = a.CreatedDate,
                        ModifyDate = a.ModifyDate,
                        ClosingDate = a.ClosingDate,
                        Status = b.StatusDescription,
                        NumberOfOpenings = a.NumberOfOpenings

                      };
        foreach (var d in vacancy)
        {
          VacancyListModels e = new VacancyListModels();
          e.ID = Convert.ToInt32(d.ID);
          e.ReferenceNo = Convert.ToString(d.ReferenceNo);
          // e.VacancyName = Convert.ToString(d.VacancyName);
          e.JobTitle = Convert.ToString(d.JobTitle);
          e.EmploymentType = Convert.ToString(d.EmploymentType);
          e.Organisation = Convert.ToString(d.Organisation);
          e.CreatedDate = Convert.ToDateTime(d.CreatedDate).ToShortDateString();
          e.ModifyDate = Convert.ToDateTime(d.ModifyDate).ToShortDateString();
          e.ClosingDate = Convert.ToDateTime(d.ClosingDate).ToShortDateString();
          e.Status = Convert.ToString(d.Status);
          e.NumberOfOpenings = Convert.ToInt32(d.NumberOfOpenings);
          p.Add(e);
        }
        return p;
      }
    }


    //GEt Active Applications List
    public List<CandidateVacancyApplication> GetApplicationsList()
    {
      var p = new List<CandidateVacancyApplication>();
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        var Applications = from a in _db.tblCandidateVacancyApplications
                           join b in _db.tblProfiles on a.UserID equals b.UserID
                           join c in _db.tblVacancies on a.VacancyID equals c.ID
                           join h in _db.lutGenders on b.fkGenderID equals h.GenderID
                           join l in _db.lutJobTitles on c.JobTitleID equals l.JobTitleID
                           join k in _db.lutApplicantStatus on a.ApplicationStatusID equals k.StatusID

                           where a.ApplicationStatusID == 1
                           select new
                           {
                             ApplicationID = a.ApplicationID,
                             profileID = b.pkProfileID,
                             UserID = a.UserID,
                             FirstName = b.FirstName,
                             Surname = b.Surname,
                             IDNumber = b.IDNumber,
                             Gender = b.fkGenderID,
                             DateofBirth = b.DateOfBirth,
                             Refno = c.ReferenceNo,
                             JobTitle = l.JobTitle,
                             //Joblevel = c.JobLevelID,
                             //Salary = c.SalaryRange,
                             Location = c.Location,
                             Status = k.StatusDescription,
                             DateOfBirthApplicationDate = a.ApplicationDate,

                           };
        foreach (var d in Applications)
        {
          CandidateVacancyApplication e = new CandidateVacancyApplication();
          e.ApplicationID = Convert.ToInt32(d.ApplicationID);
          e.profileID = Convert.ToInt32(d.profileID);
          e.UserID = Convert.ToString(d.UserID);
          e.FirstName = Convert.ToString(d.FirstName);
          e.Surname = Convert.ToString(d.Surname);
          e.IDNumber = Convert.ToString(d.IDNumber);
          e.DateofBirth = Convert.ToDateTime(d.DateofBirth).ToShortDateString();
          e.Gender = Convert.ToString(d.Gender);
          e.JobTitle = Convert.ToString(d.JobTitle);
          e.Location = Convert.ToString(d.Location);
          e.Refno = Convert.ToString(d.Refno);
          e.JobTitle = Convert.ToString(d.JobTitle);
          //e.Joblevel = Convert.ToString(d.Joblevel);
          //e.Salary = Convert.ToString(d.Salary);
          e.Location = Convert.ToString(d.Location);
          e.status = Convert.ToString(d.Status);
          e.ApplicationDate = Convert.ToDateTime(d.DateOfBirthApplicationDate).ToShortDateString();


          p.Add(e);


        }
        return p;
      }
    }


    //GEt Application Detailes List
    public List<CandidateVacancyApplication> ApplicationDetailes(int id)
    {
      var p = new List<CandidateVacancyApplication>();
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        var Applications = from a in _db.tblCandidateVacancyApplications
                           join b in _db.tblProfiles on a.UserID equals b.UserID
                           join c in _db.tblVacancies on a.VacancyID equals c.ID
                           join d in _db.lutJobTitles on c.JobTitleID equals d.JobTitleID
                           join h in _db.lutGenders on b.fkGenderID equals h.GenderID

                           where a.ApplicationID == id
                           select new
                           {
                             ApplicationID = a.ApplicationID,
                             profileID = b.pkProfileID,
                             FirstName = b.FirstName,
                             Surname = b.Surname,
                             IDNumber = b.IDNumber,
                             Gender = b.fkGenderID,
                             DateofBirth = b.DateOfBirth,
                             Refno = c.ReferenceNo,
                             JobTitle = d.JobTitle,
                             //Joblevel = c.JobLevelID,
                             //Salary = c.SalaryRange,
                             Location = c.Location,

                             DateOfBirthApplicationDate = a.ApplicationDate,
                           };
        foreach (var d in Applications)
        {
          CandidateVacancyApplication e = new CandidateVacancyApplication();

          e.ApplicationID = Convert.ToInt32(d.ApplicationID);
          e.profileID = Convert.ToInt32(d.profileID);
          e.FirstName = Convert.ToString(d.FirstName);
          e.Surname = Convert.ToString(d.Surname);
          e.IDNumber = Convert.ToString(d.IDNumber);
          e.DateofBirth = Convert.ToDateTime(d.DateofBirth).ToShortDateString();
          e.Gender = Convert.ToString(d.Gender);
          e.JobTitle = Convert.ToString(d.JobTitle);
          e.Location = Convert.ToString(d.Location);
          e.Refno = Convert.ToString(d.Refno);
          e.JobTitle = Convert.ToString(d.JobTitle);
          //e.Joblevel = Convert.ToString(d.Joblevel);
          //e.Salary = Convert.ToString(d.Salary);
          e.Location = Convert.ToString(d.Location);
          e.ApplicationDate = Convert.ToDateTime(d.DateOfBirthApplicationDate).ToShortDateString();


          p.Add(e);


        }
        return p;
      }
    }
    //Get Shortlisted Candidate
    public List<CandidateVacancyApplication> GetShortlistedCandidtes()
    {
      var p = new List<CandidateVacancyApplication>();
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        var Applications = from a in _db.tblCandidateVacancyApplications
                           join b in _db.tblProfiles on a.UserID equals b.UserID
                           join c in _db.tblVacancies on a.VacancyID equals c.ID
                           join h in _db.lutGenders on b.fkGenderID equals h.GenderID
                           join i in _db.lutJobTitles on c.JobTitleID equals i.JobTitleID
                           join k in _db.lutApplicantStatus on a.ApplicationStatusID equals k.StatusID
                           where a.ApplicationStatusID == 2
                           select new
                           {
                             ApplicationID = a.ApplicationID,
                             profileID = b.pkProfileID,
                             FirstName = b.FirstName,
                             Surname = b.Surname,
                             IDNumber = b.IDNumber,
                             Gender = b.fkGenderID,
                             DateofBirth = b.DateOfBirth,
                             Refno = c.ReferenceNo,
                             JobTitle = i.JobTitleID,
                             //Joblevel = c.JobLevelID,
                             //Salary = c.SalaryRange,
                             Location = c.Location,
                             Status = k.StatusDescription,
                             DateOfBirthApplicationDate = a.ApplicationDate,
                           };
        foreach (var d in Applications)
        {
          CandidateVacancyApplication e = new CandidateVacancyApplication();

          e.ApplicationID = Convert.ToInt32(d.ApplicationID);
          e.profileID = Convert.ToInt32(d.profileID);
          e.FirstName = Convert.ToString(d.FirstName);
          e.Surname = Convert.ToString(d.Surname);
          e.IDNumber = Convert.ToString(d.IDNumber);
          e.DateofBirth = Convert.ToDateTime(d.DateofBirth).ToShortDateString();
          e.Gender = Convert.ToString(d.Gender);
          e.JobTitle = Convert.ToString(d.JobTitle);
          e.Location = Convert.ToString(d.Location);
          e.Refno = Convert.ToString(d.Refno);
          e.JobTitle = Convert.ToString(d.JobTitle);
          //e.Joblevel = Convert.ToString(d.Joblevel);
          //e.Salary = Convert.ToString(d.Salary);
          e.Location = Convert.ToString(d.Location);
          e.status = Convert.ToString(d.Status);
          e.ApplicationDate = Convert.ToDateTime(d.DateOfBirthApplicationDate).ToShortDateString();


          p.Add(e);


        }
        return p;
      }
    }

    //get Accepted Application List
    public List<CandidateVacancyApplication> GetAcceptedApplicationsList()
    {
      var p = new List<CandidateVacancyApplication>();
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        var Applications = from a in _db.tblCandidateVacancyApplications
                           join b in _db.tblProfiles on a.UserID equals b.UserID
                           join c in _db.tblVacancies on a.VacancyID equals c.ID
                           join h in _db.lutGenders on b.fkGenderID equals h.GenderID
                           join l in _db.lutJobTitles on c.JobTitleID equals l.JobTitleID
                           join k in _db.lutApplicantStatus on a.ApplicationStatusID equals k.StatusID
                           where a.ApplicationStatusID == 5
                           select new
                           {
                             ApplicationID = a.ApplicationID,
                             profileID = b.pkProfileID,
                             UserID = a.UserID,
                             FirstName = b.FirstName,
                             Surname = b.Surname,
                             IDNumber = b.IDNumber,
                             Gender = b.fkGenderID,
                             DateofBirth = b.DateOfBirth,
                             Refno = c.ReferenceNo,
                             JobTitle = l.JobTitle,
                             //Joblevel = c.JobLevelID,
                             //Salary = c.SalaryRange,
                             Location = c.Location,
                             Status = k.StatusDescription,
                             DateOfBirthApplicationDate = a.ApplicationDate,

                           };
        foreach (var d in Applications)
        {
          CandidateVacancyApplication e = new CandidateVacancyApplication();
          e.ApplicationID = Convert.ToInt32(d.ApplicationID);
          e.profileID = Convert.ToInt32(d.profileID);
          e.UserID = Convert.ToString(d.UserID);
          e.FirstName = Convert.ToString(d.FirstName);
          e.Surname = Convert.ToString(d.Surname);
          e.IDNumber = Convert.ToString(d.IDNumber);
          e.DateofBirth = Convert.ToDateTime(d.DateofBirth).ToShortDateString();
          e.Gender = Convert.ToString(d.Gender);
          e.JobTitle = Convert.ToString(d.JobTitle);
          e.Location = Convert.ToString(d.Location);
          e.Refno = Convert.ToString(d.Refno);
          e.JobTitle = Convert.ToString(d.JobTitle);
          // e.Joblevel = Convert.ToString(d.Joblevel);
          //e.Salary = Convert.ToString(d.Salary);
          e.Location = Convert.ToString(d.Location);
          e.status = Convert.ToString(d.Status);
          e.ApplicationDate = Convert.ToDateTime(d.DateOfBirthApplicationDate).ToShortDateString();


          p.Add(e);


        }
        return p;
      }
    }

    //GetApprovedVacancyList
    public List<VacancyListModels> GetApprovedVacancyList(string userid)
    {
      var p = new List<VacancyListModels>();
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        var data = _db.AspNetUserRoles.Where(x => x.UserId == userid).FirstOrDefault();
        int orgid = data.OrganisationID;

        var vacancy = from a in _db.tblVacancies
                      join b in _db.lutStatus on a.StatusID equals b.StatusID
                      join c in _db.lutEmployementTypes on a.EmploymentTypeID equals c.id
                      join d in _db.lutOrganisations on a.OrganisationID equals d.OrganisationID
                      join e in _db.lutDivisions on a.DivisionID equals e.DivisionID
                      join f in _db.lutDepartments on a.DepartmentID equals f.DepartmentID
                      join g in _db.tblVacancyProfiles on a.VacancyProfileID equals g.VacancyProfileID
                      where a.StatusID == 2 && a.OrganisationID == orgid //&& f.ManagerID == userid
                      && a.ClosingDate >= DateTime.Now
                      orderby a.CreatedDate descending
                      select new
                      {
                        ID = a.ID,
                        ReferenceNo = a.ReferenceNo,
                        // VacancyName = a.VacancyName,
                        JobTitle = g.VacancyName,
                        EmploymentType = c.EmploymentType,
                        Organisation = d.OrganisationName,
                        CreatedDate = a.CreatedDate,
                        ClosingDate = a.ClosingDate,
                        Status = b.StatusDescription
                      };
        foreach (var d in vacancy)
        {
          VacancyListModels e = new VacancyListModels();
          e.ID = Convert.ToInt32(d.ID);
          e.ReferenceNo = Convert.ToString(d.ReferenceNo);
          //e.VacancyName = Convert.ToString(d.VacancyName);
          e.JobTitle = Convert.ToString(d.JobTitle);
          e.EmploymentType = Convert.ToString(d.EmploymentType);
          e.Organisation = Convert.ToString(d.Organisation);
          e.CreatedDate = Convert.ToDateTime(d.CreatedDate).ToShortDateString();
          e.ClosingDate = Convert.ToDateTime(d.ClosingDate).ToShortDateString();
          e.Status = Convert.ToString(d.Status);
          p.Add(e);
        }
        return p;
      }
    }
    //CreateUserProfile

    public void CreateUserProfile(string userID, string IDNumber, string passportNo, string Surname, string FirstName, string CellNo, string emailAddress)
    {
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentCreateUserProfile(userID, IDNumber, passportNo, Surname, FirstName, CellNo, emailAddress);
      }

      //using (SqlConnection sCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
      //{
      //    string sSQL = string.Empty;
      //    sSQL = "Update tblProfile Set PassportNo = '" + passportNo + "' Where UserID = '" + userID + "'";
      //    using (SqlCommand oCommand = new SqlCommand(sSQL, sCon))
      //    {
      //        oCommand.CommandType = CommandType.Text;
      //        //oCommand.Parameters.AddWithValue("@passportNo", passportNo);
      //        //oCommand.Parameters.AddWithValue("@UserID", userID);
      //        sCon.Open();
      //        oCommand.ExecuteNonQuery();
      //    }
      //}

    }
    //DeleteOrganisation
    public void DeleteOrganisation(int id)
    {
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentDeleteOrganisation(id);
      }
    }
    //UpdateOrganisation
    public void UpdateOrganisation(string organisationCode, string organisationName, string fileName, byte[] fileData, string contentType, int organisationID)
    {
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentUpdateOrganisation(organisationCode, organisationName, fileName, fileData, contentType, organisationID);
      }
    }
    //InsertIntoOrganisation
    public void InsertIntoOrganisation(string organisationCode, string organisationName, string fileName, byte[] fileData, string contentType)
    {
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentInsertIntoOrganisation(organisationCode, organisationName, fileName, fileData, contentType);
      }
    }

    //Insert Vacancy Name
    public void InsertIntoVacancyName(string VacancyName)
    {
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentInsertIntoVacancyName(VacancyName);
      }
    }
    // InsertUserRole
    public void InsertUserRole(string useid, int roleid, int organisationid)
    {
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentInsertUserRole(useid, roleid, organisationid);
      }
    }
    //UpdateVacancyStatus
    public bool UpdateVacancyStatus(int statusid, int id)
    {
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentUpdateVacancyStatus(statusid, id);
        return true;
      }
    }

    //Update Application Status
    public void UpdateApplicationStatus(int statusid, int id)
    {
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentUpdateApplicationStatus(statusid, id);
      }
    }
    //UpdateRejectionReason

    public void UpdateRejectionReason(string reason, int id, string userid, int rejectReasonID)
    {
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentUpdateRejectionReason(reason, id, userid, rejectReasonID);
      }
    }
    //InsertVacancyDocument
    public void InsertVacancyDocument(int vacancyID, string sFileName, byte[] FileData, string ContentType)
    {
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentInsertVacancyDocument(vacancyID, sFileName, FileData, ContentType);
      }
    }
        //InsertCandidateVacancyApplication
        //================Peter commented this on 20221024================
        //public bool InsertCandidateVacancyApplication(string userID, int vacancyid, int OrganisationID, string IDNumber )
        //================================================================
        //================Peter added this on 20221024====================
        public bool InsertCandidateVacancyApplication(string userID, int vacancyid, int OrganisationID, string IDNumber, int Declaration)
        //================================================================
        {
            using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
          {
            _db.proc_eRecruitmentInsertIntoCandidateVacancyApplication(userID, vacancyid, OrganisationID, IDNumber, Declaration); //Peter added Declaration
                return true;
          }
        }
    //UpdateWithdrawalReason
    public void UpdateWithdrawalReason(string reason, int id, int withdrawalReasonID)
    {
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentUpdateWithdrawalReason(reason, id, withdrawalReasonID);
      }
    }

    //Update Retraced Reason
    public void UpdateRetractReason(string reason, int id, int retractReasonID)
    {
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentUpdateRetractedReason(reason, id, retractReasonID);
      }
    }
    //Insert Vacancy
    public int InsertVacancy(string UserID, string JobTitle, int JobLevelID
        , int OrganisationID, string Recruiter, string Location, string ContractDuration, string Manager, int EmploymentTypeID, string SalaryRange, string Responsibility
        , string QualificationsandExperience, string TechnicalCompetenciesDescription, string OtherSpecialRequirements, string ClosingDate, string Disclaimer, string VacancyPurpose, string RecruiterEmail, string RecruiterTel
        , int NumberOfOpenings, int VacancyNameID, int DepartmentID, int DivisionID, int VancyTypeID)
    {
      int id = 0;

      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        var vacancy = _db.proc_eRecruitmentInsertVacancy(UserID, JobTitle, JobLevelID, OrganisationID
            , Recruiter, Location, ContractDuration, Manager, EmploymentTypeID, SalaryRange, Responsibility, QualificationsandExperience
            , TechnicalCompetenciesDescription, OtherSpecialRequirements, ClosingDate, Disclaimer, VacancyPurpose, RecruiterEmail, RecruiterTel
            , NumberOfOpenings, VacancyNameID, DepartmentID, DivisionID, VancyTypeID);
        foreach (var d in vacancy)
        {
          id = (int)d.VacancyID;
        }

      }
      return id;
    }
    //UPdate Vacancy
    public int UpdateVacancy(int id, string UserID, string ReferenceNo, string VacancyName, string JobTitle, int JobLevelID
          , int OrganisationID, string Recruiter, string Location, string ContractDuration, string Manager, int EmploymentTypeID, string SalaryRange, string Responsibility
          , string QualificationsandExperience, string TechnicalCompetenciesDescription, string OtherSpecialRequirements, string ClosingDate, string Disclaimer, string VacancyPurpose,
        string RecruiterEmail, string RecruiterTel, int NumberOfOpenings, int VacancyNameID, int DepartmentID, int DivisionID, int VancyTypeID, DateTime ModifyDate)
    {
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        var vacancy = _db.proc_eRecruitmentUpdateVacancy(id, UserID, ReferenceNo, VacancyName, JobTitle, JobLevelID
            , OrganisationID, Recruiter, Location, ContractDuration, Manager, EmploymentTypeID, SalaryRange, Responsibility, QualificationsandExperience
            , TechnicalCompetenciesDescription, OtherSpecialRequirements, ClosingDate, Disclaimer, VacancyPurpose, RecruiterEmail, RecruiterTel, NumberOfOpenings, VacancyNameID, DepartmentID, DivisionID, VancyTypeID, ModifyDate);

      }
      return id;
    }


    //Update Candidate Personal Info to Profile Table

    public void UpdateProfileInfo(string Userid, string IDNumber, string Surname, string FirstName, string DateOfBirth, int fkRaceID, int fkGenderID, string CellNo,
                                string AlternativeNo, string EmailAddress, string UnitNo, string ComplexName, string StreetNo, string StreetName, string SuburbName, string City,
                                string PostalCode, int fkDisabilityID, int NatureOfDisability, string OtherNatureOfDisability, int SACitizen, int fkNationalityID, int fkProvinceID, int fkWorkPermitID, string WorkPermitNo,
                                int pkCriminalOffenseID, int fkLanguageForCorrespondenceID, string TelNoDuringWorkingHours,
                                int MethodOfCommunicationID, string CorrespondanceDetails, int TermsAndConditions, int ProfessionallyRegisteredID,
                                string RegistrationDate, string RegistrationNumber, string RegistrationBody,
                                int PreviouslyEmployedPS, string ReEmployment, string PreviouslyEmployedDepartment, int DriversLicenseID, int MatricID, int ConditionsThatPreventsID)
    {
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        if (RegistrationDate == "")
        {
          RegistrationDate = null;
        }
        _db.UpdateProfile(Userid, IDNumber, Surname, FirstName, Convert.ToDateTime(DateOfBirth), fkRaceID, fkGenderID, CellNo,
            AlternativeNo, EmailAddress, UnitNo, ComplexName, StreetNo, StreetName, SuburbName, City,
            PostalCode, fkDisabilityID, NatureOfDisability, OtherNatureOfDisability, SACitizen, fkNationalityID, fkProvinceID, fkWorkPermitID, WorkPermitNo,
            pkCriminalOffenseID, fkLanguageForCorrespondenceID, TelNoDuringWorkingHours, MethodOfCommunicationID, CorrespondanceDetails, TermsAndConditions,
            ProfessionallyRegisteredID, RegistrationDate, RegistrationNumber, RegistrationBody,
            PreviouslyEmployedPS, ReEmployment, PreviouslyEmployedDepartment, DriversLicenseID, MatricID, ConditionsThatPreventsID);
      }
    }
    //InsertUpdateVacancyQuestion
    public void InsertUpdateVacancyQuestion(int vacancyid, string questionid)
    {
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentInsertUpdateVacancyQuestion(vacancyid, questionid);
      }
    }
    //InsertCandidateVacancyResponseQuestion
    public void InsertCandidateVacancyResponseQuestion(string userid, int vacancyid, int questionid)
    {
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentInsertCandidateVacancyResponseQuestion(userid, vacancyid, questionid);
      }
    }

    //Insert Email
    public void InsertEmail(string userID, int profileID, string sFromEmail, string sToEmail, string sEmailSubject, string sEmailBody, int emailTypeID)
    {
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentInsertEmail(userID, profileID, sFromEmail, sToEmail, sEmailSubject, sEmailBody, emailTypeID);
      }
    }

    //Add Education
    public void AddEducation(string institutionName, string qualificationName, int QualificationTypeID, string certificateNumber, DateTime startDate, DateTime endDate, int profileID)
    {
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentAddEducation(institutionName, qualificationName, QualificationTypeID, certificateNumber, startDate, endDate, profileID);
      }
    }

    //Update Education
    public void EditEducation(int qualificationID, string institutionName, string qualificationName, int QualificationTypeID, string certificateNumber, DateTime startDate, DateTime endDate)
    {
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentUpdateEducation(qualificationID, institutionName, qualificationName, QualificationTypeID, certificateNumber, startDate, endDate);
      }
    }

    //Delete Education
    public void DeleteEducation(int qualificationID)
    {
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentDeleteEducation(qualificationID);
      }
    }

    //Add Work History
    public void AddWorkHistory(string companyName, string jobTitle, string positionHeld, string department, DateTime startDate, DateTime endDate, string reasonForLeaving, int previouslyEmployedPS,
                                string reEmployment, string previouslyEmployedDepartment, int profileID)
    {


      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentAddWorkHistory(companyName, jobTitle, positionHeld, department, startDate, endDate, reasonForLeaving, previouslyEmployedPS,
                           reEmployment, previouslyEmployedDepartment, profileID);
      }
    }

    //Update Work History
    public void EditWorkHistory(string companyName, string jobTitle, string positionHeld, string department, DateTime startDate, string endDate, string reasonForLeaving, int previouslyEmployedPS,
                                string reEmployment, string previouslyEmployedDepartment, int workHistoryID)
    {
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentUpdateWorkHistory(companyName, jobTitle, positionHeld, department, startDate, endDate, reasonForLeaving, previouslyEmployedPS,
                           reEmployment, previouslyEmployedDepartment, workHistoryID);
      }
    }

    //Delete Work History
    public void DeleteWorkHistory(int workHistoryID)
    {
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentDeleteWorkHistory(workHistoryID);
      }
    }

    //Add Skills
    public void AddSkills(int profileID, int skillID, int SkillProficiencyID)
    {
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentAddSkills(profileID, skillID, SkillProficiencyID);
      }
    }

    //Delete Skills_Profile
    public void DeleteCandidateSkillsProfile(int skillsProfileID)
    {
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentDeleteSkillsProfile(skillsProfileID);
      }
    }

    //Add Languages
    public void AddLanguages(int profileID, int languageID, int LanguageProficiencyID)
    {
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentAddLanguage(profileID, languageID, LanguageProficiencyID);
      }
    }

    //Delete DeleteProfile_Langauage
    public void DeleteCandidateProfileLangauage(int profileLanguageID)
    {
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentDeleteProfileLangauage(profileLanguageID);
      }
    }

    //Add References
    public void AddReferences(string refName, string companyName, string positionHeld, string telNo, string emailAddress, int profileID)
    {
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentAddReference(refName, companyName, positionHeld, telNo, emailAddress, profileID);
      }
    }

    //Update References
    public void EditReference(string refName, string companyName, string positionHeld, string telNo, string emailAddress, int referrenceID)
    {
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentUpdateReference(refName, companyName, positionHeld, telNo, emailAddress, referrenceID);
      }
    }

    //Delete Reference
    public void DeleteReference(int referrenceID)
    {
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentDeleteReferenceByID(referrenceID);
      }
    }


    //Add Attachments 
    public void AddAttachments(int profileID, string fileName, byte[] fileData, string contentType,string FileExtension)
    {
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        _db.AddAttachments(profileID, fileName, fileData, contentType, FileExtension);
      }
    }

    public void DeleteUserRole(string userid)
    {
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        var data = from a in _db.AspNetUserRoles
                   where a.UserId == userid
                   select a;
        _db.AspNetUserRoles.DeleteAllOnSubmit(data);
        _db.SubmitChanges();
      }
    }

    public void DeleteCandidateEmail(int id)
    {
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        var data = from a in _db.tblEmails
                   where a.pkEmailID == id
                   select a;
        _db.tblEmails.DeleteAllOnSubmit(data);
        _db.SubmitChanges();
      }
    }


    //Update User Role
    public void UpdateUserRole(string userId, string roleId, int organisationID)
    {
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentUpdateUserRole(userId, roleId, organisationID);
      }
    }

    //Reassign User Role
    public void ReassignUserRole(string userId, string roleId, int organisationID, int userStatusID)
    {
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentReassignUserRole(userId, roleId, organisationID, userStatusID);
      }
    }

    //Deactivate User Role
    public void DeactivateUserRole(string userId, string roleId, int organisationID, int userStatusID)
    {
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentDeactivateUserRole(userId, roleId, organisationID, userStatusID);
      }
    }

    //Get User Status List
    public List<UserStatusModel> GetUserStatusList()
    {
      var p = new List<UserStatusModel>();
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        var UserStatus = _db.lutUserStatus.ToList();

        foreach (var d in UserStatus)
        {
          UserStatusModel e = new UserStatusModel();
          e.UserStatusID = Convert.ToInt32(d.UserStatusID);
          e.UserStatus = Convert.ToString(d.UserStatus);
          p.Add(e);
        }
        return p;
      }
    }

    public void UpdateProfilePicture(string userid, byte[] fileData)
    {
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentUpdateProfilePic(userid, fileData);
      }
    }

    public List<CandidateScreeningVacancyModels> GetCandidateScreeningVacancy(string userid)
    {
      var p = new List<CandidateScreeningVacancyModels>();
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        var data = from a in _db.tblVacancies
                   join b in _db.tblVacancyProfiles
                   on a.VacancyProfileID equals b.VacancyProfileID
                   where a.UserID == userid
                   orderby a.CreatedDate descending
                   select new { a.ID, a.ReferenceNo, VacancyName = b.VacancyName + " - (" + a.ReferenceNo + ")" };

        foreach (var d in data)
        {
          CandidateScreeningVacancyModels e = new CandidateScreeningVacancyModels();
          e.VacancyID = Convert.ToInt32(d.ID);
          e.VacancyName = Convert.ToString(d.VacancyName);
          e.ReferenceNumber = Convert.ToString(d.ReferenceNo);
          p.Add(e);
        }
        return p;
      }
    }

    public void UpdateAttachments(int profileID, string fileName, byte[] fileData, string contentType, string FileExtension)
    {
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        _db.proc_eRecruitmentUpdateAttachment(profileID, fileName, fileData, contentType, FileExtension);
      }
    }

    //Delete Attachment
    public void DeleteAttachment(int attachmentID)
    {
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        _db.DeleteAttachments(attachmentID);
      }
    }

    public void UpdateTableDateColumn(int id)
    {
      SqlConnection SqlConn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DefaultConnection"]);
      SqlCommand SqlComm = new SqlCommand();
      SqlComm = new SqlCommand("Update tblWorkHistory Set endDate = null where workHistoryID = @ID;", SqlConn);
      SqlComm.Parameters.AddWithValue("@ID", id);
      SqlComm.ExecuteNonQuery();
    }

    //Get ProfileID
    public int GetProfileID(string userId)
    {
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        int pid = 0;
        var profile = from a in _db.tblProfiles
                      join b in _db.AspNetUsers
                      on a.UserID equals b.Id
                      where b.Id == userId
                      select new { ProfileID = a.pkProfileID };
        foreach (var d in profile)
        {
          pid = d.ProfileID;
        }
        return pid;
      }
    }

    public int GetNumberofQuetions(string userId, int vanID)
    {
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        //int pid = 0;
        //var profile = from a in _db.tblVacancyQuestions
        //              join b in _db.tblCandidateResponses
        //              on a.VacancyID equals b.VacancyID
        //              where b. == userId
        //              select new { ProfileID = a.pkProfileID };
        //foreach (var d in profile)
        //{
        //    pid = d.ProfileID;
        //}
        return 0;
      }
    }

    //Get Race List
    public List<RaceModel> GetRaceList()
    {
      var p = new List<RaceModel>();
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        var Race = _db.lutRaces.Where(x => x.RaceID != 6).ToList();

        foreach (var d in Race)
        {
          RaceModel e = new RaceModel();
          e.RaceID = Convert.ToInt32(d.RaceID);
          e.RaceName = Convert.ToString(d.RaceName);
          p.Add(e);
        }
        return p;
      }
    }

    //Get Gender List
    public List<GenderModel> GetGenderList()
    {
      var p = new List<GenderModel>();
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {

        var Gender = _db.lutGenders.Where(x => x.GenderID != 3).OrderBy(x => x.Gender).ToList(); //Peter added order by - 20230315
        //var Gender = _db.lutGenders.Where(x => x.GenderID != 3).ToList();

        foreach (var d in Gender)
        {
          GenderModel e = new GenderModel();
          e.GenderID = Convert.ToInt32(d.GenderID);
          e.Gender = Convert.ToString(d.Gender);
          p.Add(e);
        }
        return p;
      }
    }

    //Get Province List
    public List<ProvinceModel> GetProvinceList()
    {
      var p = new List<ProvinceModel>();
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        var Province = _db.lutProvinces.Where(x => x.ProvinceID != 10).ToList();

        foreach (var d in Province)
        {
          ProvinceModel e = new ProvinceModel();
          e.ProvinceID = Convert.ToInt32(d.ProvinceID);
          e.ProvinceName = Convert.ToString(d.ProvinceName);
          p.Add(e);
        }
        return p;
      }
    }

    //Get Yes or No List
    public List<YesorNoModel> GetYesorNoList()
    {
      var p = new List<YesorNoModel>();
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        var YesorNo = _db.lutYesorNos.ToList();

        foreach (var d in YesorNo)
        {
          YesorNoModel e = new YesorNoModel();
          e.AnswerID = Convert.ToInt32(d.AnswerID);
          e.Answer = Convert.ToString(d.Answer);
          p.Add(e);
        }
        return p;
      }
    }

        //=======================================Peter 20221014===========================================
        //Get GetCandidateProfileInfo
        public List<ProfileViewModel> GetCandidateProfileInfo(string ID)
        {
            var p = new List<ProfileViewModel>();
            _db = new eRecruitmentDataClassesDataContext();

            
           var data =  from b in _db.tblProfiles 
                       join c in _db.lutRaces on b.fkRaceID equals c.RaceID
                       join d in _db.lutGenders on b.fkGenderID equals d.GenderID
                       join e in _db.lutProvinces on b.fkProvinceID equals e.ProvinceID
                       join f in _db.lutDisabilities on b.NatureOfDisability equals f.DisabilityID
                       join g in _db.lutYesorNos on b.MatricID equals g.AnswerID
                       join h in _db.lutYesorNos on b.DriversLicenseID equals h.AnswerID
                       where b.UserID == ID
                       select new
                       {
                           IDNumber = b.IDNumber,
                           b.PassportNo,
                           Surname = b.Surname,
                           FirstName = b.FirstName,
                           DateOfBirth = b.DateOfBirth,
                           fkRaceID = c.RaceID,
                           Race = c.RaceName,
                           fkGenderID = d.GenderID,
                           Gender = d.Gender,
                           CellNo = b.CellNo,
                           AlternativeNo = b.AlternativeNo,
                           EmailAddress = b.EmailAddress,
                           Matric = g.Answer,
                           DriversLicense = h.Answer,

                           UnitNo = b.UnitNo,
                           StreetNo = b.StreetNo,
                           StreetName = b.StreetName,
                           SuburbName = b.SuburbName,
                           fkProvinceID = b.fkProvinceID,
                           PostalCode = b.PostalCode,
                           fkDisabilityID = b.fkDisabilityID,

                           NatureOfDisability = b.NatureOfDisability,
                           DisabilityDesc = f.Disability,
                           OtherNatureOfDisability = b.OtherNatureOfDisability,
                           SACitizen = b.SACitizen,
                           fkNationalityID = b.fkNationalityID,
                           fkWorkPermitID = b.fkWorkPermitID,
                           WorkPermitNo = b.WorkPermitNo,
                           pkCriminalOffenseID = b.pkCriminalOffenseID,
                           fkLanguageForCorrespondenceID = b.fkLanguageForCorrespondenceID,
                           TelNoDuringWorkingHours = b.TelNoDuringWorkingHours,

                           MethodOfCommunicationID = b.MethodOfCommunicationID,
                           CorrespondanceDetails = b.CorrespondanceDetails,


                           ProfessionallyRegisteredID = b.ProfessionallyRegisteredID,
                           RegistrationDate = b.RegistrationDate,
                           RegistrationNumber = b.RegistrationNumber,
                           RegistrationBody = b.RegistrationBody,
                           PreviouslyEmployedPS = b.PreviouslyEmployedPS,
                           b.ConditionsThatPreventsReEmploymentID,
                           ReEmployment = b.ReEmployment,
                           PreviouslyEmployedDepartment = b.PreviouslyEmployedDepartment,

                       };

            foreach (var d in data)
            {
                ProfileViewModel e = new ProfileViewModel();
                e.IDNumber = Convert.ToString(d.IDNumber);
                e.PassportNumber = Convert.ToString(d.PassportNo);
                e.Surname = Convert.ToString(d.Surname);
                e.FirstName = Convert.ToString(d.FirstName);
                e.DateOfBirth = Convert.ToDateTime(d.DateOfBirth).ToShortDateString();
                e.Race = Convert.ToString(d.Race);
                e.Gender = Convert.ToString(d.Gender);
                e.CellNo = Convert.ToString(d.CellNo);
                e.AlternativeNo = Convert.ToString(d.AlternativeNo);
                e.EmailAddress = Convert.ToString(d.EmailAddress);
                e.Matric = Convert.ToString(d.Matric);
                e.DriversLicense = Convert.ToString(d.DriversLicense);
                e.UnitNo = Convert.ToString(d.UnitNo);
                e.StreetNo = Convert.ToString(d.StreetNo);
                e.StreetName = Convert.ToString(d.StreetName);
                e.SuburbName = Convert.ToString(d.SuburbName);
                e.fkProvinceID = Convert.ToInt32(d.fkProvinceID);
                e.PostalCode = Convert.ToString(d.PostalCode);
                e.fkDisabilityID = Convert.ToInt32(d.fkDisabilityID);
                e.SACitizen = Convert.ToInt32(d.SACitizen);
                e.NatureOfDisability = Convert.ToInt32(d.NatureOfDisability);
                e.OtherNatureOfDisability = Convert.ToString(d.OtherNatureOfDisability);
                e.Disability = Convert.ToString(d.DisabilityDesc);
                e.fkNationalityID = Convert.ToInt32(d.fkNationalityID);
                e.fkWorkPermitID = Convert.ToInt32(d.fkWorkPermitID);
                e.WorkPermitNo = Convert.ToString(d.WorkPermitNo);
                e.pkCriminalOffenseID = Convert.ToInt32(d.pkCriminalOffenseID);
                e.fkLanguageForCorrespondenceID = Convert.ToInt32(d.fkLanguageForCorrespondenceID);
                e.TelNoDuringWorkingHours = Convert.ToString(d.TelNoDuringWorkingHours);
                e.EmailAddress = Convert.ToString(d.EmailAddress);

                e.MethodOfCommunicationID = Convert.ToInt32(d.MethodOfCommunicationID);
                e.CorrespondanceDetails = Convert.ToString(d.CorrespondanceDetails);

                e.ProfessionallyRegisteredID = Convert.ToInt32(d.ProfessionallyRegisteredID);
                e.RegistrationDate = Convert.ToDateTime(d.RegistrationDate);
                e.RegistrationNumber = Convert.ToString(d.RegistrationNumber);
                e.RegistrationBody = Convert.ToString(d.RegistrationBody);
                e.PreviouslyEmployedPS = Convert.ToInt32(d.PreviouslyEmployedPS);
                e.ConditionsThatPreventsReEmploymentID = Convert.ToInt32(d.ConditionsThatPreventsReEmploymentID);
                e.ReEmployment = Convert.ToString(d.ReEmployment);
                e.PreviouslyEmployedDepartment = Convert.ToString(d.PreviouslyEmployedDepartment);
                p.Add(e);
            }
            return p;
        }
        //================================================================================================
        //=======================================Peter 20221014===========================================
        //Get Get Candidate Education List
        public List<EducationModel> GetEducationList(string ID)
        {
            var p = new List<EducationModel>();
            _db = new eRecruitmentDataClassesDataContext();

            var data = from b in _db.tblProfiles 
                       join c in _db.tblCandidateEducations on b.pkProfileID equals c.ProfileID
                       join d in _db.lutQualificationTypes on c.QualificationTypeID equals d.QualificationTypeID

                       where b.UserID == ID
                       select new
                       {
                           institutionName = c.InstitutionName,
                           qualificationName = c.QualificationName,
                           QualificationTypeName = d.QualificationTypeName,
                           CertificateNumber = c.CertificateNumber,
                           startDate = c.StartDate,
                           endDate = c.EndDate

                       };

            foreach (var d in data)
            {
                EducationModel e = new EducationModel();
                e.institutionName = Convert.ToString(d.institutionName);
                e.qualificationName = Convert.ToString(d.qualificationName);
                e.QualificationTypeName = Convert.ToString(d.QualificationTypeName);
                e.certificateNumber = Convert.ToString(d.CertificateNumber);
                e.startDate = Convert.ToDateTime(d.startDate).ToShortDateString();
                e.endDate = Convert.ToDateTime(d.endDate).ToShortDateString();
                p.Add(e);
            }
            return p;
        }
        //================================================================================================
        //=======================================Peter 20221014===========================================
        //Get Get Candidate Work History List
        public List<WorkHistoryModel> GetWorkHistoryList(string ID)
        {
            var p = new List<WorkHistoryModel>();
            _db = new eRecruitmentDataClassesDataContext();

            var data = from b in _db.tblProfiles
                       join c in _db.tblWorkHistories on b.pkProfileID equals c.profileID

                       where b.UserID == ID
                       select new
                       {
                           companyName = c.companyName,
                           jobTitle = c.jobTitle,
                           positionHeld = c.positionHeld,
                           department = c.department,
                           startDate = c.startDate,
                           endDate = c.endDate,
                           reasonForLeaving = c.reasonForLeaving,

                       };

            foreach (var d in data)
            {
                WorkHistoryModel e = new WorkHistoryModel();
                e.companyName = Convert.ToString(d.companyName);
                e.jobTitle = Convert.ToString(d.jobTitle);
                string text = Convert.ToString(d.positionHeld);

                string heldPosition = Convert.ToString(MvcHtmlString.Create(HttpUtility.HtmlEncode(text.Replace("•", ".").Replace("’", "'").Replace("“", "\"").Replace("”", "\"").Replace("–", "-"))));
                e.positionHeld = Convert.ToString(this.RemoveSpecialCharacters(heldPosition));
                e.department = Convert.ToString(d.department);
                e.startDate = Convert.ToDateTime(d.startDate).ToShortDateString();
                e.endDate = Convert.ToDateTime(d.endDate).ToShortDateString();
                e.reasonForLeaving = Convert.ToString(d.reasonForLeaving);
                p.Add(e);
            }
            return p;
        }
        //================================================================================================
        //=======================================Peter 20221014===========================================
        //Get Get Candidate Skill List
        public List<CandidateSkillsProfileModel> GetCandidateSkillList(string ID)
        {
            var p = new List<CandidateSkillsProfileModel>();
            _db = new eRecruitmentDataClassesDataContext();

            var data = from b in _db.tblProfiles
                       join c in _db.tblSkillsProfiles on b.pkProfileID equals c.profileID
                       join d in _db.lutSkills on c.skillID equals d.skillID
                       join e in _db.lutSkill_Proficiencies on c.SkillProficiencyID equals e.SkillProficiencyID

                       where b.UserID == ID
                       select new
                       {
                           skillName = d.skillName,
                           SkillProficiency = e.SkillProficiency

                       };

            foreach (var d in data)
            {
                CandidateSkillsProfileModel e = new CandidateSkillsProfileModel();
                e.skillName = Convert.ToString(d.skillName);
                e.SkillProficiency = Convert.ToString(d.SkillProficiency);
                p.Add(e);
            }
            return p;
        }
        //================================================================================================
        //=======================================Peter 20221014===========================================
        //Get Get Candidate Language List
        public List<CandidateLanguageProfileModel> GetCandidateLanguageList(string ID)
        {
            var p = new List<CandidateLanguageProfileModel>();
            _db = new eRecruitmentDataClassesDataContext();

            var data = from b in _db.tblProfiles
                       join c in _db.tblProfileLangauages on b.pkProfileID equals c.profileID
                       join d in _db.lutLanguages on c.languageID equals d.languageID
                       join e in _db.lutLaguage_Proficiencies on c.LanguageProficiencyID equals e.LanguageProficiencyID

                       where b.UserID == ID
                       select new
                       {
                           LanguageName = d.LanguageName,
                           LanguageProficiency = e.LanguageProficiency

                       };

            foreach (var d in data)
            {
                CandidateLanguageProfileModel e = new CandidateLanguageProfileModel();
                e.LanguageName = Convert.ToString(d.LanguageName);
                e.LanguageProficiency = Convert.ToString(d.LanguageProficiency);
                p.Add(e);
            }
            return p;
        }
        //================================================================================================
        //=======================================Peter 20221014===========================================
        //Get Get Candidate Reference List
        public List<ReferenceModel> GetReferenceList(string ID)
        {
            var p = new List<ReferenceModel>();
            _db = new eRecruitmentDataClassesDataContext();

            var data = from b in _db.tblProfiles 
                       join c in _db.tblReferences on b.pkProfileID equals c.ProfileID

                       where b.UserID == ID
                       select new
                       {
                           RefName = c.RefName,
                           CompanyName = c.CompanyName,
                           PositionHeld = c.PositionHeld,
                           TelNo = c.TelNo,
                           EmailAddress = c.EmailAddress

                       };

            foreach (var d in data)
            {
                ReferenceModel e = new ReferenceModel();
                e.refName = Convert.ToString(d.RefName);
                e.companyName = Convert.ToString(d.CompanyName);
                e.positionHeld = Convert.ToString(d.PositionHeld);
                e.telNo = Convert.ToString(d.TelNo);
                e.emailAddress = Convert.ToString(d.EmailAddress);
                p.Add(e);
            }
            return p;
        }
        //================================================================================================
        //=======================================Peter 20221014===========================================
        private string RemoveSpecialCharacters(string value)
        {
            return value.Replace("\u0095", ".").Replace("\u0092", "'").Replace("\u0096", "-").Replace("•", ".").Replace("amp;amp;amp;", "").Replace("&#39;", "'").Replace("&amp;amp;amp;", "&").Replace("‘", "'").Replace("’", "'").Replace("#39", "'").Replace("&quot;", "\"").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "&").Trim();
        }
        //================================================================================================

        //Get Country List
        public List<CountryModel> GetCountryList()
    {
      var p = new List<CountryModel>();
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        var Country = _db.lutCountries.ToList();

        foreach (var d in Country)
        {
          CountryModel e = new CountryModel();
          e.CountryID = Convert.ToInt32(d.CountryID);
          e.CountryName = Convert.ToString(d.CountryName);
          p.Add(e);
        }
        return p;
      }
    }

    //Get Language List
    public List<LanguageModel> GetLanguageList()
    {
      var p = new List<LanguageModel>();
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        var Language = _db.lutLanguages.ToList().OrderBy(x => x.LanguageName);

        foreach (var d in Language)
        {
          LanguageModel e = new LanguageModel();
          e.LanguageID = Convert.ToInt32(d.languageID);
          e.LanguageName = Convert.ToString(d.LanguageName);
          p.Add(e);
        }
        return p;
      }
    }


    //Get Language List
    public List<LanguageModel> GetLanguageCorrList()
    {
      var p = new List<LanguageModel>();
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        var Language = _db.lutLanguages.Where(x => x.languageID == 4);

        foreach (var d in Language)
        {
          LanguageModel e = new LanguageModel();
          e.LanguageID = Convert.ToInt32(d.languageID);
          e.LanguageName = Convert.ToString(d.LanguageName);
          p.Add(e);
        }
        return p;
      }
    }

    //Get Disability List
    public List<DisabilityModel> GetDisabilityList()
    {
      var p = new List<DisabilityModel>();
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        var Disability = _db.lutDisabilities.ToList();

        foreach (var d in Disability)
        {
          DisabilityModel e = new DisabilityModel();
          e.DisabilityID = Convert.ToInt32(d.DisabilityID);
          e.Disability = Convert.ToString(d.Disability);
          p.Add(e);
        }
        return p;
      }
    }

    //Get Skills List
    public List<SkillsModel> GetSkillsList()
    {
      var p = new List<SkillsModel>();
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {

        //============Peter 20221028============
        var Skills = _db.lutSkills.OrderBy(x => x.skillName.Trim()).ToList();
        //======================================
        //var Skills = _db.lutSkills.ToList();

        foreach (var d in Skills)
        {
          SkillsModel e = new SkillsModel();
          e.skillID = Convert.ToInt32(d.skillID);
          e.skillName = Convert.ToString(d.skillName);
          p.Add(e);
        }
        return p;
      }
    }

    //Get Skills List By Profile
    public List<CandidateSkillsProfileModel> GetSkillsByProfileID(int profileID)
    {
      List<CandidateSkillsProfileModel> skillList = new List<CandidateSkillsProfileModel>();
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {

        //============Peter 20221123============
        var profileSkill = _db.proc_eRecruitmentGetSkillsProficirnyByProfileID(profileID).OrderBy(x => x.SkillName.Trim()).ToList();
        //======================================
        //var profileSkill = _db.proc_eRecruitmentGetSkillsProficirnyByProfileID(profileID).ToList();

        foreach (var item in profileSkill)
        {
          CandidateSkillsProfileModel skillProf = new CandidateSkillsProfileModel();
          skillProf.skillsProfileID = Convert.ToInt32(item.skillsProfileID);
          skillProf.skillName = Convert.ToString(item.SkillName);
          skillProf.SkillProficiency = Convert.ToString(item.SkillProficiency);
          skillList.Add(skillProf);
        }

      }
      return skillList;
    }

    //Get Skills List By ID
    public List<CandidateSkillsProfileModel> GetSkillsByID(int id)
    {
      List<CandidateSkillsProfileModel> skillList = new List<CandidateSkillsProfileModel>();
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        var profileSkill = _db.proc_GetSkillsProficienyByID(id).ToList();

        foreach (var item in profileSkill)
        {
          CandidateSkillsProfileModel skillProf = new CandidateSkillsProfileModel();
          skillProf.skillsProfileID = Convert.ToInt32(item.skillsProfileID);
          skillProf.skillName = Convert.ToString(item.SkillName);
          skillProf.SkillProficiency = Convert.ToString(item.SkillProficiency);
          skillList.Add(skillProf);
        }

      }
      return skillList;
    }

    //Get Language List By Profile
    public List<CandidateLanguageProfileModel> GetLanguagesByProfileID(int profileID)
    {
      List<CandidateLanguageProfileModel> languageList = new List<CandidateLanguageProfileModel>();
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {


        //============Peter 20221124============
        var profileLanguage = _db.GetLanguageProficienyByProfileID(profileID).OrderBy(x => x.LanguageName).ToList();
        //======================================
        //var profileLanguage = _db.GetLanguageProficienyByProfileID(profileID).ToList();

        //var profileLanguage = _db.Profile_Langauages.Where(t => t.profileID == profileID).ToList();

        foreach (var item in profileLanguage)
        {
          CandidateLanguageProfileModel languagesProf = new CandidateLanguageProfileModel();
          languagesProf.profileLanguageID = (int)Convert.ToInt64(item.profileLanguageID);
          languagesProf.LanguageName = Convert.ToString(item.LanguageName);
          languagesProf.LanguageProficiency = Convert.ToString(item.LanguageProficiency);
          languageList.Add(languagesProf);
        }

      }
      return languageList;
    }

    //Get Education List
    public List<CandidateEducationModel> GetCandidateQualificationByProfileID(string Userid)
    {
      var p = new List<CandidateEducationModel>();
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        int profileID = GetProfileID(Userid);

        var education = (from a in _db.tblCandidateEducations
                         join b in _db.lutQualificationTypes
                         on a.QualificationTypeID equals b.QualificationTypeID
                         where a.ProfileID == profileID
                         //============Peter 20221124============
                         orderby a.StartDate descending
                         //======================================
                         select new
                         {
                           a.QualificationID,
                           a.ProfileID,
                           a.InstitutionName,
                           a.QualificationName,
                           b.QualificationTypeName,
                           a.CertificateNumber,
                           a.StartDate,
                           a.EndDate
                         }).ToList();

        foreach (var d in education)
        {
          CandidateEducationModel e = new CandidateEducationModel();
          e.qualificationID = Convert.ToInt32(d.QualificationID);
          e.profileID = Convert.ToInt32(d.ProfileID);
          e.institutionName = Convert.ToString(d.InstitutionName);
          e.qualificationName = Convert.ToString(d.QualificationName);
          e.QualificationTypeName = Convert.ToString(d.QualificationTypeName);
          e.certificateNumber = Convert.ToString(d.CertificateNumber);
          e.startDate = Convert.ToDateTime(d.StartDate).ToString("d");
          e.endDate = Convert.ToDateTime(d.EndDate).ToString("d");
          p.Add(e);
        }
        return p;
      }
    }

    //Get Language List By ID
    public List<CandidateLanguageProfileModel> GetLanguageProficienyByID(int id)
    {
      List<CandidateLanguageProfileModel> languageList = new List<CandidateLanguageProfileModel>();
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        var profileLanguage = _db.GetLanguageProficienyByID(id).ToList();

        foreach (var item in profileLanguage)
        {
          CandidateLanguageProfileModel languagesProf = new CandidateLanguageProfileModel();
          languagesProf.profileLanguageID = Convert.ToInt32(item.profileLanguageID);
          languagesProf.LanguageName = Convert.ToString(item.LanguageName);
          languagesProf.LanguageProficiency = Convert.ToString(item.LanguageProficiency);
          languageList.Add(languagesProf);
        }

      }
      return languageList;
    }

    //Get Education List By ID
    //Modified By Ntshengedzeni
    public List<CandidateEducationModel> GetCandidateEducationForEditByID(int id)
    {
      var p = new List<CandidateEducationModel>();
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        var data = _db.tblCandidateEducations.Where(x => x.QualificationID == id).FirstOrDefault();
        CandidateEducationModel e = new CandidateEducationModel();
        e.qualificationID = Convert.ToInt32(data.QualificationID);
        e.profileID = Convert.ToInt32(data.ProfileID);
        e.institutionName = Convert.ToString(data.InstitutionName);
        e.qualificationName = Convert.ToString(data.QualificationName);
        e.QualificationTypeName = Convert.ToString(data.QualificationTypeID);
        e.certificateNumber = Convert.ToString(data.CertificateNumber);
        e.startDate = Convert.ToDateTime(data.StartDate).ToString("d");
        DateTime sDate = Convert.ToDateTime(data.StartDate);
        e.startDateDay = Convert.ToString(sDate.Day);
        e.startDateMonth = Convert.ToString(sDate.Month);
        e.startDateYear = Convert.ToString(sDate.Year);
        e.endDate = Convert.ToDateTime(data.EndDate).ToString("d");
        DateTime eDate = Convert.ToDateTime(data.EndDate);
        e.endDateDay = Convert.ToString(eDate.Day);
        e.endDateMonth = Convert.ToString(eDate.Month);
        e.endDateYear = Convert.ToString(eDate.Year);
        p.Add(e);

        return p;
      }
    }

    public List<MonthListModel> GetMonthNameList()
    {
      var p = new List<MonthListModel>();
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        var data = _db.proc_eRecruitmentGetAllMonths().ToList();

        foreach (var d in data)
        {
          MonthListModel e = new MonthListModel();
          e.MonthNo = Convert.ToInt32(d.MonthNo);
          e.MonthName = Convert.ToString(d.sMonthName);
          p.Add(e);
        }
        return p;
      }
    }

    public List<YearListModel> GetYearNumList()
    {
      var p = new List<YearListModel>();
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        var data = _db.proc_eRecruitmentGetYearsList().ToList();

        foreach (var d in data)
        {
          YearListModel e = new YearListModel();
          e.YearNum = Convert.ToInt32(d.year);
          p.Add(e);
        }
        return p;
      }
    }

    public List<DayListModel> GetAllDaysList()
    {
      var p = new List<DayListModel>();
      var _days = (from a in Enumerable.Range(1, 31) select a).ToArray();
      foreach (var d in _days)
      {
        DayListModel e = new DayListModel();
        e.DayNum = Convert.ToInt32(d);
        p.Add(e);
      }
      return p;
    }

    //Get Work History By id
    public List<WorkHistoryModel> GetWorkHistoryByID(int id)
    {
      var p = new List<WorkHistoryModel>();
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {

        var workHistory = _db.tblWorkHistories.Where(x => x.workHistoryID == id).ToList();

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
          e.reasonForLeaving = Convert.ToString(d.reasonForLeaving);
          e.previouslyEmployedPS = Convert.ToInt32(d.previouslyEmployedPS);
          e.reEmployment = Convert.ToString(d.reEmployment);
          e.previouslyEmployedDepartment = Convert.ToString(d.previouslyEmployedDepartment);
          p.Add(e);
        }
        return p;
      }
    }

    //Get Work History List
    public List<WorkHistoryModel> GetWorkHistoryByProfileID(string Userid)
    {
      var p = new List<WorkHistoryModel>();
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        var id = GetProfileID(Userid);

        //============Peter 20221124============
        var workHistory = _db.tblWorkHistories.Where(x => x.profileID == id).OrderByDescending(x => x.startDate).ToList();
        //======================================
        //var workHistory = _db.tblWorkHistories.Where(x => x.profileID == id).ToList();

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
          if (Convert.ToString(d.reasonForLeaving) == "current")
          {
            e.reasonForLeaving = string.Empty;
          }
          else
          {
            e.reasonForLeaving = Convert.ToString(d.reasonForLeaving);
          }

          p.Add(e);
        }
        return p;
      }
    }

    //Get Reference List
    public List<ReferenceModel> GetReferenceByProfileID(string Userid)
    {
      var p = new List<ReferenceModel>();
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        int id = GetProfileID(Userid);

        //============Peter 20221124============
        var Reference = _db.tblReferences.Where(x => x.ProfileID == id).OrderBy(x => x.CreateOn).ToList();
        //======================================
        //var Reference = _db.tblReferences.Where(x => x.ProfileID == id).ToList();

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
        return p;
      }
    }


    public List<ReferenceModel> GetReferenceByID(int id)
    {
      var p = new List<ReferenceModel>();
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        var Reference = _db.tblReferences.Where(x => x.ReferrenceID == id).ToList();

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
        return p;
      }
    }

    //Get Reference List
    public List<AttachmentModel> GetAttachmentByProfileID(string Userid)
    {
      var p = new List<AttachmentModel>();
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        var id = GetProfileID(Userid);
        var Attachment = _db.Attachments.Where(x => x.ProfileID == id && x.fileName != "Curriculum Vitae").ToList();

        foreach (var d in Attachment)
        {
          AttachmentModel e = new AttachmentModel();
          e.attachmentID = Convert.ToInt32(d.AttachmentID);
          e.profileID = Convert.ToInt32(d.ProfileID);
          e.fileName = Convert.ToString(d.fileName);
          e.contentType = Convert.ToString(d.contentType);
          e.contentType = Convert.ToString(d.contentType);
          e.createdon = Convert.ToString(d.ModifiedOn).ToString();
          //e.createdon = DateTime.Now.ToString();
          p.Add(e);
        }
        return p;
      }
    }

    //Get Language Proficiency List
    public List<LaguageProficiencyModel> GetLanguageProficiencyList()
    {
      var p = new List<LaguageProficiencyModel>();
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        var LanguageProficiency = _db.lutLaguage_Proficiencies.ToList();

        foreach (var d in LanguageProficiency)
        {
          LaguageProficiencyModel e = new LaguageProficiencyModel();
          e.LanguageProficiencyID = (int)Convert.ToInt64(d.LanguageProficiencyID);
          e.LanguageProficiency = Convert.ToString(d.LanguageProficiency);
          p.Add(e);
        }
        return p;
      }
    }

    //Get Skill Proficiency List
    public List<SkillProficiencyModel> GetSkillProficiencyList()
    {
      var p = new List<SkillProficiencyModel>();
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        var SkillProficiency = _db.lutSkill_Proficiencies.ToList();

        foreach (var d in SkillProficiency)
        {
          SkillProficiencyModel e = new SkillProficiencyModel();
          e.SkillProficiencyID = Convert.ToInt32(d.SkillProficiencyID);
          e.SkillProficiency = Convert.ToString(d.SkillProficiency);
          p.Add(e);
        }
        return p;
      }
    }

    //Get Qualification Type List
    public List<QualificationTypeModel> GetQualificationTypeList()
    {
      var p = new List<QualificationTypeModel>();
      using (eRecruitmentDataClassesDataContext _db = new eRecruitmentDataClassesDataContext())
      {
        var QualificationType = _db.lutQualificationTypes.ToList();
        foreach (var d in QualificationType)
        {
          QualificationTypeModel e = new QualificationTypeModel();
          e.QualificationTypeID = Convert.ToInt32(d.QualificationTypeID);
          e.QualificationTypeName = Convert.ToString(d.QualificationTypeName);
          p.Add(e);
        }
        return p;
      }
    }

    //Get ProfileIDNumber
    public string GetProfileIDNumber(string uid)
    {
      using (eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext _db = new eRecruitment.Sita.Web.App_Data.DAL.eRecruitmentDataClassesDataContext())
      {
        string IDNumbers = "";
        var IDNumber = from a in _db.tblProfiles
                       where a.UserID == uid
                       select new { IDNumber = a.IDNumber };
        foreach (var d in IDNumber)
        {
          IDNumbers = d.IDNumber;
        }
        return IDNumbers;
      }
    }


    public int GetOrganisationID(string userid)
    {
      return _db.AspNetUserRoles.Where(x => x.UserId == userid).Select(x => x.OrganisationID).FirstOrDefault();
    }


    //GetGeneralQuestionsList
    public List<GeneralQuestionsModel> GetGeneralQuestionsList(int id)
    {
      var p = new List<GeneralQuestionsModel>();
      _db = new eRecruitmentDataClassesDataContext();
      var data = from a in _db.lutOrganisations
                 join b in _db.lutGeneralQuestions on a.OrganisationID equals b.OrganisationID
                 join c in _db.lutQuestionCatergories on b.QCategoryID equals c.QCategoryID
                 join d in _db.tblVacancyQuestions on b.id equals d.QuestionID
                 where d.VacancyID == id && c.QCategoryDescr == "Qualification"
               /*  orderby b.QualificationTypeID ascending*/ //Peter added Orderby on 20221005
                 select new
                 {
                   QuestionID = b.id,
                   QuestionDesc = b.GeneralQuestionDesc,
                   QCategoryID = c.QCategoryID,
                   QCategoryDescr = c.QCategoryDescr,

                 };

      foreach (var d in data)
      {
        GeneralQuestionsModel e = new GeneralQuestionsModel();
        e.QuestionID = Convert.ToInt32(d.QuestionID);
        e.QuestionDesc = Convert.ToString(d.QuestionDesc);
        e.QCategoryID = Convert.ToInt32(d.QCategoryID);
        e.QCategoryDescr = Convert.ToString(d.QCategoryDescr);
        p.Add(e);
      }
      return p;

    }

    public List<GeneralQuestionsModel> GetGeneralQuestionsExperienceList(int id)
    {
      var p = new List<GeneralQuestionsModel>();
      _db = new eRecruitmentDataClassesDataContext();
      var data = from a in _db.lutOrganisations
                 join b in _db.lutGeneralQuestions on a.OrganisationID equals b.OrganisationID
                 join c in _db.lutQuestionCatergories on b.QCategoryID equals c.QCategoryID
                 join d in _db.tblVacancyQuestions on b.id equals d.QuestionID
                 where d.VacancyID == id && c.QCategoryDescr == "Experience"

                 select new
                 {
                   QuestionID = b.id,
                   QuestionDesc = b.GeneralQuestionDesc,
                   QCategoryID = c.QCategoryID,
                   QCategoryDescr = c.QCategoryDescr,

                 };

      foreach (var d in data)
      {
        GeneralQuestionsModel e = new GeneralQuestionsModel();
        e.QuestionID = Convert.ToInt32(d.QuestionID);
        e.QuestionDesc = Convert.ToString(d.QuestionDesc);
        e.QCategoryID = Convert.ToInt32(d.QCategoryID);
        e.QCategoryDescr = Convert.ToString(d.QCategoryDescr);
        p.Add(e);
      }
      return p;

    }

    public List<GeneralQuestionsModel> GetGeneralQuestionsCertificationList(int id)
    {
      var p = new List<GeneralQuestionsModel>();
      _db = new eRecruitmentDataClassesDataContext();
      var data = from a in _db.lutOrganisations
                 join b in _db.lutGeneralQuestions on a.OrganisationID equals b.OrganisationID
                 join c in _db.lutQuestionCatergories on b.QCategoryID equals c.QCategoryID
                 join d in _db.tblVacancyQuestions on b.id equals d.QuestionID
                 where d.VacancyID == id && c.QCategoryDescr == "Certification"

                 select new
                 {
                   QuestionID = b.id,
                   QuestionDesc = b.GeneralQuestionDesc,
                   QCategoryID = c.QCategoryID,
                   QCategoryDescr = c.QCategoryDescr,

                 };

      foreach (var d in data)
      {
        GeneralQuestionsModel e = new GeneralQuestionsModel();
        e.QuestionID = Convert.ToInt32(d.QuestionID);
        e.QuestionDesc = Convert.ToString(d.QuestionDesc);
        e.QCategoryID = Convert.ToInt32(d.QCategoryID);
        e.QCategoryDescr = Convert.ToString(d.QCategoryDescr);
        p.Add(e);
      }
      return p;

    }

    public List<GeneralQuestionsModel> GetGeneralQuestionsAnnualSalaryList(int id)
    {
      var p = new List<GeneralQuestionsModel>();
      _db = new eRecruitmentDataClassesDataContext();
      var data = from a in _db.lutOrganisations
                 join b in _db.lutGeneralQuestions on a.OrganisationID equals b.OrganisationID
                 join c in _db.lutQuestionCatergories on b.QCategoryID equals c.QCategoryID
                 join d in _db.tblVacancyQuestions on b.id equals d.QuestionID
                 where d.VacancyID == id && c.QCategoryDescr == "Annual Salary"

                 select new
                 {
                   QuestionID = b.id,
                   QuestionDesc = b.GeneralQuestionDesc,
                   QCategoryID = c.QCategoryID,
                   QCategoryDescr = c.QCategoryDescr,

                 };

      foreach (var d in data)
      {
        GeneralQuestionsModel e = new GeneralQuestionsModel();
        e.QuestionID = Convert.ToInt32(d.QuestionID);
        e.QuestionDesc = Convert.ToString(d.QuestionDesc);
        e.QCategoryID = Convert.ToInt32(d.QCategoryID);
        e.QCategoryDescr = Convert.ToString(d.QCategoryDescr);
        p.Add(e);
      }
      return p;

    }

    public List<GeneralQuestionsModel> GetGeneralQuestionsNoticePeriodList(int id)
    {
      var p = new List<GeneralQuestionsModel>();
      _db = new eRecruitmentDataClassesDataContext();
      var data = from a in _db.lutOrganisations
                 join b in _db.lutGeneralQuestions on a.OrganisationID equals b.OrganisationID
                 join c in _db.lutQuestionCatergories on b.QCategoryID equals c.QCategoryID
                 join d in _db.tblVacancyQuestions on b.id equals d.QuestionID
                 where d.VacancyID == id && c.QCategoryDescr == "Notice Period "

                 select new
                 {
                   QuestionID = b.id,
                   QuestionDesc = b.GeneralQuestionDesc,
                   QCategoryID = c.QCategoryID,
                   QCategoryDescr = c.QCategoryDescr,

                 };

      foreach (var d in data)
      {
        GeneralQuestionsModel e = new GeneralQuestionsModel();
        e.QuestionID = Convert.ToInt32(d.QuestionID);
        e.QuestionDesc = Convert.ToString(d.QuestionDesc);
        e.QCategoryID = Convert.ToInt32(d.QCategoryID);
        e.QCategoryDescr = Convert.ToString(d.QCategoryDescr);
        p.Add(e);
      }
      return p;

    }

    //===============================Peter - 20221115===========================
    public List<JobSpecificQuestionsModel> GetGeneralQuestionsJobSpecificList(int? JobTitleid)
    {
        var p = new List<JobSpecificQuestionsModel>();
        _db = new eRecruitmentDataClassesDataContext();
        var data = from a in _db.lutJobSpecificQuestions
                    where a.JobTitleID == JobTitleid

                    select new
                    {
                        JobSpecQID = a.JobSpecificeQuestionID,
                        JobSpecQDesc = a.JobSpecificeQuestionDesc,
                    };
        foreach (var d in data)
        {
            JobSpecificQuestionsModel e = new JobSpecificQuestionsModel();
            e.JobSpecificeQuestionID = Convert.ToInt32(d.JobSpecQID);
            e.JobSpecificeQuestionDesc = Convert.ToString(d.JobSpecQDesc);
            p.Add(e);
        }
        return p;
    }
    //==========================================================================

        public List<GeneralQuestionsModel> GetGeneralQuestionsExperienceint()
    {
      var p = new List<GeneralQuestionsModel>();
      _db = new eRecruitmentDataClassesDataContext();
      var data = from a in _db.lutOrganisations
                 join b in _db.lutGeneralQuestions on a.OrganisationID equals b.OrganisationID
                 join c in _db.lutQuestionCatergories on b.QCategoryID equals c.QCategoryID
                 //join d in _db.tblVacancyQuestions on b.id equals d.QuestionID
                 where c.QCategoryID == 2

                 select new
                 {
                   QuestionID = b.id,
                   QuestionDesc = b.GeneralQuestionDesc,
                   QCategoryID = c.QCategoryID,
                   QCategoryDescr = c.QCategoryDescr,

                 };

      foreach (var d in data)
      {
        GeneralQuestionsModel e = new GeneralQuestionsModel();
        e.QuestionID = Convert.ToInt32(d.QuestionID);
        e.QuestionDesc = Convert.ToString(d.QuestionDesc);
        e.QCategoryID = Convert.ToInt32(d.QCategoryID);
        e.QCategoryDescr = Convert.ToString(d.QCategoryDescr);
        p.Add(e);
      }
      return p;

    }

    public OrganisationModel GetOrganisationIDByVacanyID(int id)
    {
      var p = new OrganisationModel();
      _db = new eRecruitmentDataClassesDataContext();
      var data = from a in _db.lutOrganisations
                 join b in _db.tblVacancies on a.OrganisationID equals b.OrganisationID
                 where b.ID == id

                 select new
                 {
                   OrganisationID = b.OrganisationID


                 };

      OrganisationModel e = new OrganisationModel();
      foreach (var d in data)
      {

        e.OrganisationID = Convert.ToInt32(d.OrganisationID);
      }
      return e;

    }

    public OrganisationModel GetOrganisationIDPerVacancy(int id)
    {
      var p = new OrganisationModel();
      _db = new eRecruitmentDataClassesDataContext();
      var data = from a in _db.lutOrganisations
                 join b in _db.tblVacancies on a.OrganisationID equals b.OrganisationID
                 where b.ID == id

                 select new
                 {
                   OrganisationID = b.OrganisationID


                 };

      OrganisationModel e = new OrganisationModel();
      foreach (var d in data)
      {

        e.OrganisationID = Convert.ToInt32(d.OrganisationID);
      }
      return e;

    }

    public VacancyModels GetVacancyHistoryIDPerVacancy(int id)
    {
      var p = new VacancyModels();
      _db = new eRecruitmentDataClassesDataContext();
      var data = from a in _db.tblVacancies
                 join b in _db.tblVacancyHistories on a.ID equals b.ID
                 where b.ID == id

                 select new
                 {
                   VacancyHistoryID = b.VacancyHistoryID


                 };

      VacancyModels e = new VacancyModels();
      foreach (var d in data)
      {

        e.ID = Convert.ToInt32(d.VacancyHistoryID);
      }
      return e;

    }

    public VacancyModels GetVacancyQuestionIDPerVacancy(int id)
    {
      var p = new VacancyModels();
      _db = new eRecruitmentDataClassesDataContext();
      var data = from a in _db.tblVacancies
                 join b in _db.tblVacancyQuestions on a.ID equals b.VacancyID
                 where b.VacancyID == id

                 select new
                 {
                   VacancyQuestionID = b.VacancyID


                 };

      VacancyModels e = new VacancyModels();
      foreach (var d in data)
      {

        e.ID = Convert.ToInt32(d.VacancyQuestionID);
      }
      return e;

    }

    public VacancyModels GetVacancyIDPerVacancyHistory(int id)
    {
      var p = new VacancyModels();
      _db = new eRecruitmentDataClassesDataContext();
      var data = from a in _db.tblVacancies
                 join b in _db.tblVacancyHistories on a.ID equals b.ID
                 where b.VacancyHistoryID == id

                 select new
                 {
                   VacancyID = b.ID

                 };

      VacancyModels e = new VacancyModels();
      foreach (var d in data)
      {

        e.ID = Convert.ToInt32(d.VacancyID);
      }
      return e;

    }

    //===================Peter 20221014=======================
    public VacancyModels GetVacancyID(int id, string userid)
    {
        var p = new VacancyModels();
        _db = new eRecruitmentDataClassesDataContext();
        var data = from a in _db.tblCandidateVacancyApplications
                   join b in _db.tblProfiles on a.UserID equals b.UserID
                    where a.VacancyID == id && b.UserID == userid
 
                    select new
                    {
                        ApplicationID = a.ApplicationID 
                    };
        VacancyModels e = new VacancyModels();
        foreach (var d in data)
        {
            e.ID = Convert.ToInt32(d.ApplicationID);
        }
        return e;
    }
    //=======================================================


        public List<TechnicalCompetencyModel> GetTechnicalCompList(int VacancyProfileID)
    { 
      _db = new eRecruitmentDataClassesDataContext();

      var data = (from jp in _db.tblJobProfiles
                  join jptc in _db.JobProfileTechComps on jp.JobProfileID equals jptc.JobProfileID
                  join tc in _db.TechnicalCompetencies on jptc.TechnicalCompetencyID equals tc.TechnicalCompetencyID
                  orderby tc.TechnicalComp
                  where jp.JobProfileID == VacancyProfileID
                  select new TechnicalCompetencyModel()
                  {
                    TechnicalCompetencyID = tc.TechnicalCompetencyID,
                    TechnicalComp = tc.TechnicalComp,
                    TechnicalCompDesc = tc.TechnicalCompDesc
                  }).ToList();

      return data;
    }

    public List<LeadershipCompetencyModel> GetLeadershipCompList(int VacancyProfileID)
    {
      _db = new eRecruitmentDataClassesDataContext();

      var data = (from jp in _db.tblJobProfiles
                  join jplc in _db.JobProfileLeadComps on jp.JobProfileID equals jplc.JobProfileID
                  join lc in _db.LeadershipCompetencies on jplc.LeadershipCompetencyID equals lc.LeadershipCompetencyID
                  orderby lc.LeadershipComp
                  where jp.JobProfileID == VacancyProfileID
                  select new LeadershipCompetencyModel()
                  {
                    LeadershipCompetencyID = lc.LeadershipCompetencyID,
                    LeadershipComp = lc.LeadershipComp,
                    LeadershipCompDesc = lc.LeadershipCompDesc
                  }).ToList();

      return data;
    }

    public List<BehaviouralCompetencyModel> GetBehaviouralCompList(int VacancyProfileID)
    {
      _db = new eRecruitmentDataClassesDataContext();

      var data = (from jp in _db.tblJobProfiles
                  join jpbc in _db.JobProfileBehaveComps on jp.JobProfileID equals jpbc.JobProfileID
                  join bc in _db.BehaviouralCompetencies on jpbc.BehaviouralCompetencyID equals bc.BehaviouralCompetencyID
                  orderby bc.BehaviouralComp
                  where jp.JobProfileID == VacancyProfileID
                  select new BehaviouralCompetencyModel()
                  {
                    BehaviouralCompetencyID = bc.BehaviouralCompetencyID,
                    BehaviouralComp = bc.BehaviouralComp,
                    BehaviouralCompDesc = bc.BehaviouralCompDesc
                  }).ToList();

      return data;
    }


    public List<SkillsModel> GetSelectedSkillsPerCatergiryListForJobProfile(int id)
    {
      var p = new List<SkillsModel>();
      _db = new eRecruitmentDataClassesDataContext();

      var data = from a in _db.tblVacancyProfileSkills
                 join b in _db.lutSkillsCategories on a.CategoryID equals b.CategoryID
                 join c in _db.lutSkills on a.SkillID equals c.skillID
                 join d in _db.tblJobProfiles on a.JobProfileID equals d.JobProfileID

                 where a.JobProfileID == id
                 select new
                 {
                   SkillID = c.skillID,
                   SkillName = c.skillName,

                 };

      foreach (var d in data)
      {
        SkillsModel e = new SkillsModel();
        e.skillID = Convert.ToInt32(d.SkillID);
        e.skillName = Convert.ToString(d.SkillName);
        p.Add(e);
      }
      return p;

    }

  }
}
