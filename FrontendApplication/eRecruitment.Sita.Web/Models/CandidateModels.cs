using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace eRecruitment.Sita.Web.Models
{
    [Table("tblProfile")]
    public class ProfileModel
    {
        [Key]
        public int pkProfileID { get; set; }
        public string Userid { get; set; }

        //[Required]
        public string IDNumber { get; set; }
   
        public string PassportNumber { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string FirstName { get; set; }

        //[Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DateOfBirth { get; set; }

        [Required]
        public int? fkRaceID { get; set; }
        [Required]
        public int fkGenderID { get; set; }
        [Required]
        public string CellNo { get; set; }
       
        public string AlternativeNo { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        public string UnitNo { get; set; }
        public string ComplexName { get; set; }
        public string StreetNo { get; set; }
        public string StreetName { get; set; }
        public string SuburbName { get; set; }
        [Required]
        public string City { get; set; }
        public string PostalCode { get; set; }
        [Required]
        public int? fkDisabilityID { get; set; }
        public int NatureOfDisability { get; set; }
        public string OtherNatureOfDisability { get; set; }
        [Required]
        public int SACitizen { get; set; }
        public int? fkNationalityID { get; set; }
        [Required]
        public int? fkProvinceID { get; set; }
        public int? fkWorkPermitID { get; set; }
        public string WorkPermitNo { get; set; }
        [Required]
        public int pkCriminalOffenseID { get; set; }
        [Required]
        public int? fkLanguageForCorrespondenceID { get; set; }
        [Required(ErrorMessage = "Please enter Tel Number During Working Hours")]
        public string TelNoDuringWorkingHours { get; set; }
        [Required]
        public int? MethodOfCommunicationID { get; set; }
        [Required]
        public string CorrespondanceDetails { get; set; }

        public int? TermsAndCondition { get; set; }

        [Required]
        public int? ProfessionallyRegisteredID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> RegistrationDate { get; set; }
        //public DateTime RegistrationDate { get; set; }
        public string RegistrationNumber { get; set; }
        public string RegistrationBody { get; set; }
        [Required]
        public int? PreviouslyEmployedPS { get; set; }
        public int? ConditionsThatPreventsReEmploymentID { get; set; }
        public string ReEmploy { get; set; }
        public string PreviouslyEmployedDepartment { get; set; }
        [Required]
        public int? DriversLicenseID { get; set; }
        [Required]
        public int? MatricID { get; set; }

    }


    [Table("Education")]
    public class EducationModel
    {
        [Key]
        public int qualificationID { get; set; }
        [Required]
        [Display(Name = "Institution Name")]
        public string institutionName { get; set; }
        [Required]
        [Display(Name = "Qualification Name")]
        public string qualificationName { get; set; }
        //===================Peter - 20221014===========================
        public string QualificationTypeName { get; set; }
        //==============================================================

        [Display(Name = "Other Qualification Name")]
        public string otherEdu { get; set; }
        [Required]
        [Display(Name = "Qualification Type")]
        public int QualificationTypeID { get; set; }
        [Required]
        [Display(Name = "Certificate Number")]
        public string certificateNumber { get; set; }
        [Required]
        [Display(Name = "Start Date")]
        public string startDate { get; set; }
        [Required]
        [Display(Name = "End Date")]
        public string endDate { get; set; }
        public int profileID { get; set; }
    }

    public class CandidateEducationModel
    {
        [Key]
        public int qualificationID { get; set; }
        [Required]
        [Display(Name = "Institution Name")]
        public string institutionName { get; set; }
        [Required]
        [Display(Name = "Qualification Name")]
        public string qualificationName { get; set; }

        [Display(Name = "Other Qualification")]
        public string OtherEdu { get; set; }
        [Required]
        [Display(Name = "Qualification Type")]
        public string QualificationTypeName { get; set; }
        [Required]
        [Display(Name = "Certificate Number")]
        public string certificateNumber { get; set; }
        [Required]
        [Display(Name = "Start Date")]
        public string startDate { get; set; }
        [Required]
        [Display(Name = "End Date")]
        public string endDate { get; set; }
        public int profileID { get; set; }

        public string startDateDay { get; set; }
        public string startDateMonth { get; set; }
        public string startDateYear { get; set; }

        public string endDateDay { get; set; }
        public string endDateMonth { get; set; }
        public string endDateYear { get; set; }

    }

    [Table("WorkHistory")]
    public class WorkHistoryModel
    {
        [Key]
        public int workHistoryID { get; set; }
        [Required]
        [Display(Name = "Company Name")]
        public string companyName { get; set; }
        [Required]
        [Display(Name = "Job Title")]
        public string jobTitle { get; set; }
        [Required]
        [Display(Name = "Position Held")]
        public string positionHeld { get; set; }
        [Required]
        [Display(Name = "Department")]
        public string department { get; set; }
        [Required]
        [Display(Name = "Start Date")]
        public string startDate { get; set; }
        [Required]
        [Display(Name = "End Date")]
        public string endDate { get; set; }
        public int profileID { get; set; }
        public string reasonForLeaving { get; set; }
        public int previouslyEmployedPS { get; set; }
        public string reEmployment { get; set; }
        public string previouslyEmployedDepartment { get; set; }

    }

    [Table("Referrence")]
    public class ReferrenceModel
    {
        [Key]
        public int referrenceID { get; set; }
        [Required]
        [Display(Name = "Reference Full Name")]
        public string refName { get; set; }
        [Required]
        [Display(Name = "Company Name")]
        public string companyName { get; set; }
        [Required]
        [Display(Name = "Position Held")]
        public string positionHeld { get; set; }
        [Required]
        [Display(Name = "Tel No")]
        public string telNo { get; set; }
        
        [Display(Name = "Email Address")]
        public string emailAddress { get; set; }
        public int profileID { get; set; }
    }

    [Table("lutSkill")]
    public class SkillsModel
    {
        [Key]
        public int skillID { get; set; }
        public string skillName { get; set; }

    }

    [Table("Profile_Langauage")]
    public class Profile_LangauageModel
    {
        [Key]
        public int profileLanguageID { get; set; }
        public int profileID { get; set; }
        public int languageID { get; set; }
        public int LanguageProficiencyID { get; set; }
    }

    public class CandidateLanguageProfileModel
    {
        [Key]
        public int profileLanguageID { get; set; }
        [Display(Name = "Language Name")]
        public string LanguageName { get; set; }
        [Display(Name = "Language Proficiency")]
        public string LanguageProficiency { get; set; }
    }

    [Table("Skills_Profile")]
    public class Skills_ProfileModel
    {
        [Key]
        public int skillsProfileID { get; set; }
        public int profileID { get; set; }
        public int skillID { get; set; }
        public int SkillProficiencyID { get; set; }
    }

    public class CandidateSkillsProfileModel
    {
        [Key]
        public int skillsProfileID { get; set; }
        [Display(Name = "Skill Name")]
        public string skillName { get; set; }
        [Display(Name = "Skill Proficiency")]
        public string SkillProficiency { get; set; }
    }

    [Table("Reference")]
    public class ReferenceModel
    {
        [Key]
        public int referrenceID { get; set; }
        [Required]
        [Display(Name = "Reference Name")]
        public string refName { get; set; }
        [Required]
        [Display(Name = "Company Name")]
        public string companyName { get; set; }
        [Required]
        [Display(Name = "Position Held")]
        public string positionHeld { get; set; }
        [Required]
        [Display(Name = "Tel No")]
        public string telNo { get; set; }
        [Required]
        [Display(Name = "Email Address")]
        public string emailAddress { get; set; }
        public int profileID { get; set; }
    }

    public class AttachmentModel
    {
        [Key]
        public int attachmentID { get; set; }
        public int profileID { get; set; }
        public string fileName { get; set; }
        public byte[] fileData { get; set; }
        public string contentType { get; set; }
        public string createdon { get; set; }
    }

    //DropDowns or Lookup tables

    [Table("lutRace")]
    public class RaceModel
    {
        [Key]
        public int RaceID { get; set; }
        public string RaceName { get; set; }
    }

    [Table("lutGender")]
    public class GenderModel
    {
        [Key]
        public int GenderID { get; set; }
        public string Gender { get; set; }
    }

    [Table("lutProvince")]
    public class ProvinceModel
    {
        [Key]
        public int ProvinceID { get; set; }
        public string ProvinceName { get; set; }
    }

    [Table("lutYesorNo")]
    public class YesorNoModel
    {
        [Key]
        public int AnswerID { get; set; }
        public string Answer { get; set; }
    }

    [Table("lutCountry")]
    public class CountryModel
    {
        [Key]
        public int CountryID { get; set; }
        public string CountryName { get; set; }
    }

    [Table("lutLanguage")]
    public class LanguageModel
    {
        [Key]
        public int LanguageID { get; set; }
        public string LanguageName { get; set; }
    }

    [Table("lutDisability")]
    public class DisabilityModel
    {
        [Key]
        public int DisabilityID { get; set; }
        public string Disability { get; set; }
    }

    [Table("lutSkill_Proficiency")]
    public class SkillProficiencyModel
    {
        [Key]
        public int SkillProficiencyID { get; set; }
        public string SkillProficiency { get; set; }

    }

    [Table("lutLaguage_Proficiency")]
    public class LaguageProficiencyModel
    {
        [Key]
        public int LanguageProficiencyID { get; set; }
        public string LanguageProficiency { get; set; }
    }

    [Table("lutQualificationType")]
    public class QualificationTypeModel
    {
        [Key]
        public int QualificationTypeID { get; set; }
        public string QualificationTypeName { get; set; }
    }
}