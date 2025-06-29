using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Business_Layer.Dtos
{
    public class VacancyApplicationDto
    {
        public int VaccancyId { get; set; }
        public IFormFile ApplicantCV { get; set; }
    }
}
