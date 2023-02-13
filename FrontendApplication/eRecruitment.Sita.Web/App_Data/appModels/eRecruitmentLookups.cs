using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eRecruitment.Sita.Web
{
  public class eRecruitmentLookups
  {
  }

  public class VacancyListNameModel
  {
    public int VacancyNameID { get; set; }
    public string VacancyName { get; set; }

  }

  public class RejectReasonModel
  {
    public int RejectReasonID { get; set; }
    public string RejectReason { get; set; }

  }

  public class RetractReasonModel
  {
    public int RetractReasonID { get; set; }
    public string RetractReason { get; set; }

  }

  public class WithdrawalReasonModel
  {
    public int WithdrawalReasonID { get; set; }
    public string WithdrawalReason { get; set; }

  }

  public class CitizenshipModel
  {
    public int CitizenID { get; set; }
    public string SACitizen { get; set; }

  }

  public class CandidateEmailListModel
  {
    public int EmailID { get; set; }
    public string FromEmail { get; set; }
    public string EmailSubject { get; set; }
    public string EmailDate { get; set; }
    public int StatusID { get; set; }
    public int EmailTypeID { get; set; }
  }

  public class MonthListModel
  {
    public int MonthNo { get; set; }
    public string MonthName { get; set; }
  }

  public class YearListModel
  {
    public int YearNum { get; set; }
  }

  public class DayListModel
  {
    public int DayNum { get; set; }
  }

  public class MethodOfCummunicationModel
  {
    public int MethodID { get; set; }
    public string MethodName { get; set; }

  }

  public class VacancyDivisionModel
  {
    public int DivisionID { get; set; }
    public string DivisionDiscription { get; set; }

  }

  public class VacancySITADepartmentModel
  {
    public int DepartmentID { get; set; }
    public string DepartmentDiscription { get; set; }

  }
  public class GeneralQuestionsModel
  {
    public int QuestionID { get; set; }
    public string QuestionDesc { get; set; }
    public int TypeID { get; set; }
    public int QCategoryID { get; set; }

    public string QCategoryDescr { get; set; }
  }

    //======================Peter 20221115=====================
    public class JobSpecificQuestionsModel
    {
      public int JobSpecificeQuestionID { get; set; }
      public int JobTitleID { get; set; }
      public string JobSpecificeQuestionDesc { get; set; }
      public string CreatedDate { get; set; }
      public string CreatedBy { get; set; }
      public string ModifyDate { get; set; }
      public string ModifiedBy { get; set; }
    }
    //=========================================================

    public class OrganisationModel
  {
    public int OrganisationID { get; set; }
    public string OrganisationCode { get; set; }
    public string OrganisationName { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public byte[] FileData { get; set; }
  }

  public class StatusModel
  {
    public int StatusID { get; set; }
    public string StatusDescription { get; set; }
  }

  public class ApplicationStatusModel
  {
    public int StatusID { get; set; }
    public string StatusDescription { get; set; }
  }

  public class UserListModel
  {
    public string UserID { get; set; }
    public string FullName { get; set; }
    public string EmailAddress { get; set; }

  }

  //public class LanguageModel
  //{
  //    public int LanguageID { get; set; }
  //    public string Language { get; set; }
  //}

  public class UserModel
  {
    public int UserID { get; set; }
    public string FullName { get; set; }
  }

  public class UserRoleModel
  {
    public int RoleID { get; set; }
    public string RoleName { get; set; }
  }

  public class EmploymentTypeModel
  {
    public int EmploymentTypeID { get; set; }
    public string EmploymentType { get; set; }
  }

  public class InterviewCategory
  {
    public int InterviewCatID { get; set; }
    public string InterviewCatDescription { get; set; }
  }

  public class InterviewStatus
  {
    public int InterviewStatusID { get; set; }
    public string InterviewStatusDescription { get; set; }
  }

  public class InterviewType
  {
    public int InterviewTypeID { get; set; }
    public string InterviewTypeDescription { get; set; }
  }

  public class ApplicantType
  {
    public int ApplicantTypeID { get; set; }
    public string ApplicantTypeDiscription { get; set; }
  }

  public class Location
  {
    public int LocationID { get; set; }
    public string LocationDiscription { get; set; }
  }

  public class CandidateScreeningVacancyModels
  {
    public int VacancyID { get; set; }
    public string ReferenceNumber { get; set; }
    public string VacancyName { get; set; }

  }

  public class TechnicalCompetencyModel
  {
    public int TechnicalCompetencyID { get; set; }
    public string TechnicalComp { get; set; }
    public string TechnicalCompDesc { get; set; }
  }

  public class LeadershipCompetencyModel
  {
    public int LeadershipCompetencyID { get; set; }
    public string LeadershipComp { get; set; }
    public string LeadershipCompDesc { get; set; }
  }

  public class BehaviouralCompetencyModel
  {
    public int BehaviouralCompetencyID { get; set; }
    public string BehaviouralComp { get; set; }
    public string BehaviouralCompDesc { get; set; }
  }


}