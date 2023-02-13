using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eRecruitment.Sita.Web.Models
{
    public class OrganisationModels
    {
        public int OrganisationID { get; set; }
        [Required]
        public string OrganisationCode { get; set; }
        [Required]
        public string OrganisationName { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] FileData { get; set; }
    }
}