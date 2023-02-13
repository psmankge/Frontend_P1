using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace eRecruitment.BusinessDomain.DAL.Entities.AppModels
{
    public class ProfileViewModel
    {
        [Key]
        public int pkProfileID { get; set; }
        public string Userid { get; set; }

        [Required]
        public string IDNumber { get; set; }
        public string PassportNumber { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string FirstName { get; set; }

        public string DateOfBirth { get; set; }

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
        public string Disability { get; set; }
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
        [Required]
        public string TelNoDuringWorkingHours { get; set; }
        [Required]
        public int? MethodOfCommunicationID { get; set; }
        [Required]
        public string CorrespondanceDetails { get; set; }

        public int? TermsAndCondition { get; set; }
        public string Race { get; set; }
        public string Gender { get; set; }

        public string Matric { get; set; }
        public string DriversLicense { get; set; }

        public string Province { get; set; }
        //public int VacancyID { get; set; }
        public int ApplicationID { get; set; }

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
        public string ReEmployment { get; set; }
        public string PreviouslyEmployedDepartment { get; set; }
    }

}
