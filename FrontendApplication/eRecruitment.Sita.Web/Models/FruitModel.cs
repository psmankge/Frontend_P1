using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eRecruitment.Sita.Web.Models
{
    public class FruitModel
    {
        public List<SelectListItem> Fruits { get; set; }
        public int[] FruitIds { get; set; }
    }

}