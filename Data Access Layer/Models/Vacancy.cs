using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Models
{
    public class Vacancy
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(40)]
        public string Name { get; set; }
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }
        [Required]
        public bool IsActive { get; set; } = true;
        [Required]
        [Range(1,20)]
        public int MaxNumber { get; set; }
        [Required]
        [Column(TypeName = "timestamptz")]
        public DateTime ExpiryDate {  get; set; }
        [Required]
        [ForeignKey("Employer")]
        public virtual string EmployerId { get; set; }
        public virtual List<VacancyApplication> VacancyApplications { get; set; }
        public virtual ApplicationUser Employer { get; set; }
    }
}
