using Business_Layer.Dtos;
using Business_Layer.ServicesInterfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.MicrosoftExtensions;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security.Claims;
using Vacancy_system.Helpers;

namespace Vacancy_system.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Employer")]
    public class VacanciesController : ControllerBase
    {
        private readonly IVacancyService _vacancyService;
        private readonly IValidator<VacancyDto> _VacancyDtoValidator;
        public VacanciesController(IVacancyService vacancyService, IValidator<VacancyDto> vacancyDtoValidator)
        {
            _vacancyService = vacancyService;
            _VacancyDtoValidator = vacancyDtoValidator;
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll()
        {
            var vacancies = _vacancyService.GetAll();
            if (vacancies is null)
            {

                var validationDetails = CreateVaidationErrorDetails.CreateVaidationDetails("vacancies", "can't load Vacancies");
                return BadRequest(validationDetails);
            }
            return Ok(vacancies);
        }
        [HttpPost]
        public IActionResult Add(VacancyDto vacancy)
        {
            var validationResult = _VacancyDtoValidator.Validate(vacancy);
            if (!validationResult.IsValid)
            {
                var validationDetails = new HttpValidationProblemDetails(validationResult.ToDictionary());
                return BadRequest(validationDetails);
            }
            var employerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = _vacancyService.AddVacancy(vacancy, employerId);
            if (!result)
            {
                var validationDetails = CreateVaidationErrorDetails.CreateVaidationDetails("vacancies", "can't load Vacancies");
                return BadRequest(validationDetails);
            }
            return Ok("Added sucessfuly");
        }
        [HttpPut]
        public IActionResult Update(int id, VacancyDto vacancy)
        {
            var validationResult = _VacancyDtoValidator.Validate(vacancy);
            if (!validationResult.IsValid)
            {
                var validationDetails = new ValidationProblemDetails(validationResult.ToDictionary());
                return BadRequest(validationDetails);
            }
            var employerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = _vacancyService.UpdateVacancy(id, employerId, vacancy);
            if (!result)
            {
                var error = CreateVaidationErrorDetails.CreateVaidationDetails("Update", "Can't update this vacancy");
                return BadRequest(error);
            }
            return Ok("Updated successfuly");
        }
        [HttpDelete]
        public IActionResult Remove(int id)
        {
            var employerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = _vacancyService.RemoveVacancy(id, employerId);
            if (!result)
            {
                var error = CreateVaidationErrorDetails.CreateVaidationDetails("Remove", "Can't remove this vacancy");
                return BadRequest(error);
            }
            return Ok("Removed successfuly");
        }
        [HttpPost]
        public IActionResult Deactive(int id)
        {
            var employerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = _vacancyService.DeActive(id, employerId);
            if (!result)
            {
                var error = CreateVaidationErrorDetails.CreateVaidationDetails("Deactive", "Can't Deactive this vacancy");
                return BadRequest(error);
            }
            return Ok("DeActived successfuly");
        }
    }
}
