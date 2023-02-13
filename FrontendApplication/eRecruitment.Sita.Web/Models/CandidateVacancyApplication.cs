using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eRecruitment.Sita.Web.Models
{
    public class CandidateVacancyApplication
    {
        [Key]
        public int ApplicationID { get; set; }
        public int profileID { get; set; }
        public string UserID { get; set; }
        public int VacancyID { get; set; }
        public string ApplicationDate { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string IDNumber { get; set; }
        public string Gender { get; set; }
        public string DateofBirth { get; set; }
        public string Refno { get; set; }
        public string JobTitle { get; set; }
        public string Joblevel { get; set; }
        public string Salary { get; set; }
        public string Location { get; set; }
        public string status { get; set; }

        //Peter 20221014
        public string Declaration { get; set; }

    }

    public class CandidateJobApplication
    {
        public int ApplicationID { get; set; }
        public int VacancyID { get; set; }
        public string VacancyName { get; set; }
        public string OrganisationName { get; set; }
        public string ApplicationDate { get; set; }
        public string Refno { get; set; }

    }

    public class ScreenedCandidateModel
    {
        public int ApplicationID { get; set; }
        public int VacancyID { get; set; }
        public string UserID { get; set; }
        public string IDNumber { get; set; }
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public string DateOfBirth { get; set; }
        public string CellNo { get; set; }
        public string EmailAddress { get; set; }
        public string RaceName { get; set; }
        public string Gender { get; set; }

        public string JobTitle { get; set; }
    }
}