using Business_Layer.Dtos;
using Business_Layer.ServicesInterfaces;
using Data_Access_Layer.Models;
using Data_Access_Layer.RepositriesInterfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql.TypeMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.Services
{
    public class VacancyApplicationService : IVacancyApplicationService
    {
        private readonly IGenericRepo<VacancyApplication> _applicationRepo;
        private readonly IGenericRepo<Vacancy> _vacancyRepo;
        public VacancyApplicationService(IGenericRepo<VacancyApplication> applicationRepo, IGenericRepo<Vacancy> vacancyRepo)
        {
            _applicationRepo = applicationRepo;
            _vacancyRepo = vacancyRepo;
        }
        public OperationResut<VacancyApplication> Apply(int vacancyId, string applicantId, string cvpath)
        {
            var vacancy = _vacancyRepo.GetById(vacancyId);
            var errorMessage = ValidateApplicationRules(vacancy, applicantId);
            if(errorMessage is not null )
               return OperationResut<VacancyApplication>.FailureResult(errorMessage);
            var vacancyApplication = new VacancyApplication()
            {
                UserId = applicantId,
                VacancyId = vacancyId,
                ApplicationDate = DateTime.UtcNow,
                ApplicantCV = cvpath
            };
            bool result = _applicationRepo.Add(vacancyApplication);
            if(! result)
                return OperationResut<VacancyApplication>.FailureResult("Couldn't apply to the vacancy");
            CheckVacancyMaxNumber(vacancy);
            return OperationResut<VacancyApplication>.SuccessResult(vacancyApplication);
        }

        public List<ShowApplicationDto> GetUserApplications(string applicantId)
        {
            /* used double include here, i could get  the data from the repository itself
             as i did on GetVacancyApplications() function but i just wanted to try the both ways */
            var applications = _applicationRepo.GetFilter(filter: a => a.UserId == applicantId,
                include: a => a.Include(u => u.User).Include(u => u.Vacancy))
                .Select(a => new ShowApplicationDto()
                {
                    ApplicantName = a.User.UserName,
                    VacancyId=a.VacancyId,
                    VacancyName = a.Vacancy.Name,
                    ApplicationDate = a.ApplicationDate,
                    uploadedCV = a.ApplicantCV,
                }).ToList();

            return applications;
        }
        public List<ShowApplicationDto> GetVacancyApplications(int vacancyId)
        {
            var vacancy = _vacancyRepo.GetById(vacancyId);
            if (vacancy is null)
                return null;
                 return _applicationRepo.GetFilter(filter: a => a.VacancyId == vacancyId,
               include: a => a.Include(a => a.User))
                .Select(a => new ShowApplicationDto()
               {
                   ApplicantId = a.UserId,
                   ApplicantName = a.User.UserName,
                   VacancyId = vacancyId,
                   VacancyName = vacancy.Name,
                   ApplicationDate = a.ApplicationDate,
                   uploadedCV = a.ApplicantCV,
               }).ToList();
        }
        private void CheckVacancyMaxNumber(Vacancy vacancy)
        {
            var applicationsCount = _applicationRepo.GetFilter(filter: a => a.VacancyId == vacancy.Id).Count();
            if (applicationsCount == vacancy.MaxNumber)
            {
                vacancy.IsActive = false;
                _vacancyRepo.Update(vacancy);
            }
        }
        private string ValidateApplicationRules(Vacancy vacancy, string applicantId)
        {
            if (vacancy is null)
                return "vacancy doesn't exist";
            if (!vacancy.IsActive)
                return "the vacancy is no longer active";
            var applicationsCount = _applicationRepo.GetFilter(filter: a => a.VacancyId == vacancy.Id).Count();
            if (applicationsCount >= vacancy.MaxNumber)
                return "the vacancy has reached its maximum number of applications";
            var lastApplication = _applicationRepo.GetFilter(a => a.UserId == applicantId).LastOrDefault();
            if (lastApplication is not null &&
                lastApplication.ApplicationDate <= lastApplication.ApplicationDate.AddHours(24))
                return "you can't apply for more than one vacancy in a day";
            return null;
        }
    }
}
