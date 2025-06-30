using System.Runtime.CompilerServices;
using System.Security.Claims;
using Business_Layer.Dtos;
using Business_Layer.ServicesInterfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Vacancy_system.Helpers;

namespace Vacancy_system.Controllers
{
    [Route("api/Vacancy/Application")]
    [ApiController]
    public class VacancyApplicationController : ControllerBase
    {
        private readonly IVacancyApplicationService _applicationService;
        private readonly IValidator<VacancyApplicationDto> _applicationValidator;
        private readonly string _uploadsPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot",
            "uploads"
        );

        public VacancyApplicationController(
            IVacancyApplicationService applicationService,
            IValidator<VacancyApplicationDto> applicationValidator
        )
        {
            _applicationService = applicationService;
            _applicationValidator = applicationValidator;
        }

        [Authorize(Roles = "Applicant")]
        [HttpPost]
        public IActionResult Apply([FromForm] VacancyApplicationDto application)
        {
            var validationRestult = _applicationValidator.Validate(application);
            if (!validationRestult.IsValid)
            {
                var errorDetails = new HttpValidationProblemDetails(
                    validationRestult.ToDictionary()
                );
                return BadRequest(errorDetails);
            }
            var applicantId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var uploadPath = UploadFile(application.ApplicantCV);
            var result = _applicationService.Apply(application.VaccancyId, applicantId, uploadPath);
            if (!result.Success)
            {
                var errordetails = CreateVaidationErrorDetails.CreateVaidationDetails(
                    "vacancyApplication",
                    result.Error
                );
                return BadRequest(errordetails);
            }
            return Created("api/VacancyApplicationController/Apply", " Applied sucessfully");
        }

        [Authorize(Roles = "Applicant")]
        [HttpGet("User")]
        public IActionResult GetUserApplications()
        {
            var applicantId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var applications = _applicationService.GetUserApplications(applicantId);
            if (applications is null)
                return NotFound();
            return Ok(applications);
        }

        [HttpGet(":id")]
        [Authorize(Roles = "Employer")]
        public IActionResult GetVacancyApplications([FromRoute] int vacancyId)
        {
            var applications = _applicationService.GetVacancyApplications(vacancyId);
            if (applications is null)
                return NotFound();
            return Ok(applications);
        }

        private string UploadFile(IFormFile file)
        {
            if (!Directory.Exists(_uploadsPath))
            {
                Directory.CreateDirectory(_uploadsPath);
            }
            string fileExtension = Path.GetExtension(file.FileName);
            var fileName = Guid.NewGuid().ToString() + fileExtension;
            string filePath = Path.Combine(_uploadsPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return filePath;
        }
    }
}
