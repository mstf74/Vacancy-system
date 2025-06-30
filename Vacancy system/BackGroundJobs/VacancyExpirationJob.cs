using Business_Layer.ServicesInterfaces;
using Quartz;

namespace Vacancy_system.BackGroundJobs
{
    public class VacancyExpirationJob : IJob
    {
        private readonly IVacancyService _vacancyService;

        public VacancyExpirationJob(IVacancyService vacancyService)
        {
            _vacancyService = vacancyService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _vacancyService.DeactiveExpirationVacancies();
        }
    }
}
