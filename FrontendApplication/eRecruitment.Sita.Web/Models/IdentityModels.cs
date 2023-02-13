using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace eRecruitment.Sita.Web.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<eRecruitment.Sita.Web.Models.EducationModel> EducationModels { get; set; }

        public System.Data.Entity.DbSet<eRecruitment.Sita.Web.Models.WorkHistoryModel> WorkHistoryModels { get; set; }

        public System.Data.Entity.DbSet<eRecruitment.Sita.Web.Models.Skills_ProfileModel> Skills_ProfileModel { get; set; }

        public System.Data.Entity.DbSet<eRecruitment.Sita.Web.Models.Profile_LangauageModel> Profile_LangauageModel { get; set; }

        public System.Data.Entity.DbSet<eRecruitment.Sita.Web.Models.ReferenceModel> ReferenceModels { get; set; }

        public System.Data.Entity.DbSet<eRecruitment.Sita.Web.Models.AttachmentModel> AttachmentModels { get; set; }

        public System.Data.Entity.DbSet<eRecruitment.Sita.Web.Models.VacancyModels> VacancyModels { get; set; }

        public System.Data.Entity.DbSet<eRecruitment.Sita.Web.Models.CandidateVacancyApplication> CandidateVacancyApplications { get; set; }

        public System.Data.Entity.DbSet<eRecruitment.Sita.Web.Models.VacancyProfileModels> VacancyProfileModels { get; set; }
    }
}