using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eRecruitment.Sita.Web.Models
{
    public class DivisionModel
    {
        [Key]
        public int DivisionID { get; set; }
        public string DivisionDiscription { get; set; }
        public int OrganisationID { get; set; }
        public string OrganisationName { get; set; }
    }
}