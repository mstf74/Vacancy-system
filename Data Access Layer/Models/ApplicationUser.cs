using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Enums;
using Microsoft.AspNetCore.Identity;

namespace Data_Access_Layer.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Rules Rule { get; set; } = Rules.Applicant;
        public List<VacancyApplication> UserApplications { get; set; }
        public List<Vacancy> VacanciesCreated { get; set; }
    }
}
