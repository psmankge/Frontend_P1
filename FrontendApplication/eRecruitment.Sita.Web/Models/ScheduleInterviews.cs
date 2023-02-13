using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eRecruitment.Sita.Web.Models
{
    public class ScheduleInterviews
    {

        public int InterviewTypeID { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int LocationID { get; set; }
        public int CategoryID { get; set; }
        public int InterviewStatusID { get; set; }
        public string InternalContactName { get; set; }
        public string InternalContactNumber { get; set; }
        public IList<string> PrimaryInterviewerID { get; set; }
        public string NotetoCandidate { get; set; }
        public int ApplicantsID { get; set; }
    }
}