using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eRecruitment.Sita.Web.Models
{
    public class DepartmentModel
    {
        [Key]
        public int? DepartmentID { get; set; }

        [Required]
        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; }

        [Required]
        [Display(Name = "Organisation Name")]
        public int OrganisationID { get; set; }
        public string OrganisationName { get; set; }

        [Required]
        [Display(Name = "Department Manager Name")]
        public string ManagerID { get; set; }
        public string ManagerName { get; set; }
    }

    public class DepartmentManagerModel
    {
        [Key]
        public string UserID { get; set; }
        public string ManagerName { get; set; }

    }
}