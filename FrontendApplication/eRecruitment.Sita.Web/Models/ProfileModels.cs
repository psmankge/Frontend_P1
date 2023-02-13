using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eRecruitment.Sita.Web.Models
{
    public class ProfileModels
    {
        [Required]
        [Display(Name = "Role")]
        public int RoleID { get; set; }
        [Required]
        [Display(Name = "Organisation")]
        public int Organisation { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string UserID { get; set; }

        [Required]
        [Display(Name = "User Status")]
        public int UserStatusID { get; set; }
    }

    public class ProfileViewModels
    {
        public string UserID { get; set; }
        public String FullName { get; set; }
        public String Surname { get; set; }
        public String Email { get; set; }
        public string Organisation { get; set; }
        public string RoleName { get; set; }
    }
}