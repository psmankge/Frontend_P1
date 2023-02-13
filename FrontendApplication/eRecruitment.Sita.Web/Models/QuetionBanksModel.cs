using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eRecruitment.Sita.Web.Models
{
    public class QuetionBanksModel
    {
        public int id { get; set; }
        [Required]
        public string GeneralQuestionDesc { get; set; }
        public int TypeID { get; set; }
        public int OrganisationID { get; set; }
        public string OrganisationName { get; set; }

    }
}