using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Models
{
    public class VacancyApplication
    {
        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }
        [Required]
        [ForeignKey("Vacancy")]
        public int VacancyId { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTime ApplicationDate { get; set; } = DateTime.Now;
        public string ApplicantCV { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Vacancy Vacancy { get; set; }
    }
}
