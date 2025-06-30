using System;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Business_Layer.Dtos
{
    public class VacancyApplicationDto
    {
        public int VaccancyId { get; set; }
        public IFormFile ApplicantCV { get; set; }
    }
}
