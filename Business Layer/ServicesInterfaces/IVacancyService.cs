using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Layer.Dtos;

namespace Business_Layer.ServicesInterfaces
{
    public interface IVacancyService
    {
        // check if the max number is exceeded
        List<ShowVacanyDto> GetAll();
        ShowVacanyDto GetById(int id);
        List<ShowVacanyDto> GetByName(string name);
        bool AddVacancy(VacancyDto vacancy, string EmployerId);
        bool UpdateVacancy(int id, string employerId, VacancyDto vacancyDto);
        bool RemoveVacancy(int id, string employerId);
        bool DeActive(int id, string employerId);
        Task DeactiveExpirationVacancies();
    }
}
