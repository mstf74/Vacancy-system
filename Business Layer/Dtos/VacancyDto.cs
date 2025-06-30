using System;
using System.Text;

namespace Business_Layer.Dtos
{
    public class VacancyDto
    {
        public string Name { get; set; }
        public int MaxNumber { get; set; }
        public string Description { get; set; }
        public DateTime ExpiryDate { get; set; }
    }

    public class ShowVacanyDto : VacancyDto
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public int RemainSeats { get; set; }
    }
}
