using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eRecruitment.Sita.Web.Models
{
    public class VacancyListModels
    {
        //,,,,, ,
        public int? ID { get; set; }
        public string ReferenceNo { get; set; }
       // public string VacancyName { get; set; }
        public string JobTitle { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }
        public string EmploymentType { get; set; }
        public string Organisation { get; set; }
        public string CreatedDate { get; set; }
        public string ModifyDate { get; set; }
        public string ClosingDate { get; set; }
        public string Salary { get; set; }
        public string Status { get; set; }
        public int NumberOfOpenings { get; set; }
    }

    public class VacancyModels
    {
        [Key]
        public int ID { get; set; }
      //  [Required]
        [Display(Name = "Reference No")]
        public string ReferenceNo { get; set; }
        public string BPSNumber { get; set; }
        //  [Required]
        [Display(Name = "Vacancy Name")]
        public string VacancyName { get; set; }
        
        public string JobTitle { get; set; }
        public string JobLevel { get; set; }
        public int JobLevelID { get; set; }
        [Required]
        public int OrganisationID { get; set; }
        public string OrganisationName { get; set; }
        public string Recruiter { get; set; }
        //public string VacancyQuestionID { get; set; }
        public IList<string> VacancyQuestionID { get; set; }
        [Required]
        [EmailAddress]
        public string RecruiterEmail { get; set; }
        [Required]
        public string RecruiterTel { get; set; }
        public int StatusID { get; set; }
        public string Location { get; set; }
       
        public string Manager { get; set; }
        [Required]
        public int EmploymentTypeID { get; set; }
        public string EmploymentType { get; set; }
        public string ContractDuration { get; set; }
       
        public string Salary { get; set; }
        public string SalaryRange { get; set; }
        [Required]
        [Display(Name = "Responsibilities")]
        public string VacancyPurpose { get; set; }
        public string Responsibility { get; set; }

        public string Race { get; set; }

        public string Gender { get; set; }

        public string Deviation { get; set; }
        public string QualificationsandExperience { get; set; }

        public string TechnicalCompetenciesDescription { get; set; }
        public string OtherSpecialRequirements { get; set; }
        [Required]
        [Display(Name = "Closing Date")]
       // [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ClosingDate { get; set; }

        [Display(Name = "Creation Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Updated Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ModifyDate { get; set; }

        public string Disclaimer { get; set; }
        public string WithdrawalReason { get; set; }
        public string RetractReason { get; set; }
        public int NumberOfOpenings { get; set; }

        public int VacancyNameID { get; set; }
        public int VacancyProfileID { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int DivisionID { get; set; }
        public string DivisionName { get; set; }
        public int VancyTypeID { get; set; }
        public string VacancyTypeName { get; set; }

        public string ClosingDateS { get; set; }

        public byte[] FileData { get; set; } 

        public string FileName { get; set; }
      
        public string Knowledge { get; set; }
    }

    public class VacancyNameModel
    {
        [Key]
        public int VacancyNameID { get; set; }
        public string VacancyName { get; set; }
     
    }

    public class VacancyProfileModels
    {

        [Key]
        public int VacancyProfileID { get; set; }
        [Display(Name = "Vacancy Name")]
        [Required(ErrorMessage = "Please enter Vacancy Name")]
        public string VacancyName { get; set; }
        [Display(Name = "Vacancy Purpose")]
        [Required(ErrorMessage = "Please enter Vacancy Purpose")]
        public string VacancyPurpose { get; set; }
        [Display(Name = "Responsibility")]
        [Required(ErrorMessage = "Please enter Key Responsibility")]
        public string Responsibility { get; set; }
        [Display(Name = "Qualifications and Experience")]
        [Required(ErrorMessage = "Please enter Qualifications and Experience")]
        public string QualificationsandExperience { get; set; }
        [Display(Name = "Technical Competencies Description")]
        [Required(ErrorMessage = "Please enter Technical Competencies Description")]
        public string TechnicalCompetenciesDescription { get; set; }
        [Display(Name = "Other Special Requirements")]
        [Required(ErrorMessage = "Please enter Other Special Requirements")]
        public string OtherSpecialRequirements { get; set; }
        [Display(Name = "Disclaimer")]
        [Required(ErrorMessage = "Please enter Disclaimer")]
        public string Disclaimer { get; set; }
        [Display(Name = "Creation Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }
        public string CreatedDateView { get; set; }
        [Display(Name = "Updated Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ModifyDate { get; set; }
        public string ModifyDateView { get; set; }
        public int OrganisationID { get; set; }
        public string OrganisationName { get; set; }

    }
    public class CandidateScreeningModel
    {
        [Key]
        public int VacancyNameID { get; set; }
        public int ProvinceID { get; set; }
        public string AgeRange { get; set; }
        public int GenderID { get; set; }
        public int RaceID { get; set; }

    }
    public class CandidateListToExcelModel
    {
        public string IDNumber { get; set; }
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public string DateOfBirth { get; set; }
        public string CellNo { get; set; }
        public string EmailAddress { get; set; }
        public string RaceName { get; set; }
        public string Gender { get; set; }
    }

    public class VacancyTypeModel
    {
        public int VancyTypeID { get; set; }
        public string VacancyTypeName { get; set; }
    }

    public class JobLevelModel
    {
        public int JobLevelID { get; set; }
        public int OrganisationID { get; set; }
        public string Descr { get; set; }
    }

    public class SalaryRangeModel
    {
        public int SalaryRangeID { get; set; }
        public int JobLevelID { get; set; }
        public int OrganisationID { get; set; }
        public string SalaryRange { get; set; }
    }

    public class SalaryRangeViewModel
    {
        public int SalaryRangeID { get; set; }
        public int JobLevelID { get; set; }
        public string OrganisationName { get; set; }
        public string Descr { get; set; }
        public string SalaryRange { get; set; }
    }

    public class JobLevelViewModel
    {
        public int JobLevelID { get; set; }
        public int OrganisationID { get; set; }
        public string Descr { get; set; }
        public string SalaryRange { get; set; }
        public string OrganisationName { get; set; }
    }

    public class UserStatusModel
    {
        public int UserStatusID { get; set; }
        public string UserStatus { get; set; }
    }
}
