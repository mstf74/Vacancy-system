using Business_Layer.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.Validators
{
    public class VacancyDtovalidator: AbstractValidator<VacancyDto>
    {
        public VacancyDtovalidator()
        {
            RuleFor(v => v.Name)
                .NotEmpty()
                .MaximumLength(40);
            RuleFor(v => v.Description)
                .NotEmpty()
                .MaximumLength(500);
            RuleFor(v => v.MaxNumber)
                .NotEmpty()
                .InclusiveBetween(1, 20);
            RuleFor(v => v.ExpiryDate)
                .NotEmpty()
                .Must(FutureDate).WithMessage("the expiration date should be at least 1 day");
        }
        private bool FutureDate(DateTime date)
        {
            return date >= DateTime.UtcNow.AddDays(1);
        } 
    }
}
