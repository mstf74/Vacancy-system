using Business_Layer.Dtos;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.Validators
{
    public class VacancyApplicationValidator:AbstractValidator<VacancyApplicationDto>
    {
        string[] allowedExtensions = { ".pdf", ".doc", ".docs" };
        public VacancyApplicationValidator() 
        {
            RuleFor(a => a.VaccancyId).NotEmpty();
            RuleFor(a => a.ApplicantCV)
                .NotEmpty()
                .Must(ValidExtension);
        }
        public bool ValidExtension(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLower();
            if(! allowedExtensions.Contains(extension))
                return false;
            return true;
        }
    }
}
