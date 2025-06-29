using Business_Layer.Dtos;
using Data_Access_Layer.Models;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.ServicesInterfaces
{
    public interface IVacancyApplicationService
    {
        // apply to a vacancy -> max number validation, and application per day validation
        OperationResut<VacancyApplication> Apply(int vacancyId, string applicantId, string cvpath);
        List<ShowApplicationDto> GetUserApplications(string applicantId);
        List<ShowApplicationDto> GetVacancyApplications(int vacancyId);

        // search for vacancy by name 
    }
}
