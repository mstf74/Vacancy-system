using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.Dtos
{
    public class ShowApplicationDto
    {
        public string ApplicantId { get; set; }
        public string ApplicantName { get; set; }
        public int VacancyId { get; set; }
        public string VacancyName { get;set; }
        public DateTime ApplicationDate { get; set; }
        public string uploadedCV { get; set; }
    }
}
