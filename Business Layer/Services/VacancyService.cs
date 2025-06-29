using Business_Layer.Dtos;
using Business_Layer.ServicesInterfaces;
using Data_Access_Layer.Models;
using Data_Access_Layer.RepositriesInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Business_Layer.Services
{
    public class VacancyService : IVacancyService
    {
        private readonly IGenericRepo<Vacancy> _vacancyRepo;
        private readonly IGenericRepo<VacancyApplication> _applicationRepo;
        public VacancyService(IGenericRepo<Vacancy> vacancyRepo, IGenericRepo<VacancyApplication> applicationRepo)
        {
            _vacancyRepo = vacancyRepo;
            _applicationRepo = applicationRepo;
        }
        public List<ShowVacanyDto> GetAll()
        {
            var ExistedVacanciesList = _vacancyRepo.GetFilter(include: q => q.Include( v=> v.Employer));
            if (ExistedVacanciesList is null)
                return null;
            var takenSeats = VacanciesApplicationsCount();
            var vacancies = ExistedVacanciesList.Select(v => new ShowVacanyDto()
            {
                Id = v.Id,
                Name = v.Name,
                Description = v.Description,
                MaxNumber = v.MaxNumber,
                ExpiryDate = v.ExpiryDate,
                RemainSeats = v.MaxNumber - (takenSeats.TryGetValue(v.Id , out int count)? count : 0),
                IsActive = v.IsActive && v.ExpiryDate > DateTime.UtcNow,
                CreatedBy = v.Employer.UserName
            }).ToList();
            return vacancies;
        }
        public ShowVacanyDto GetById(int id)
        {
            var existedVacancy = _vacancyRepo.GetFilter(
                filter : v => v.Id == id,
                include : q => q.Include(v => v.Employer)
                ).FirstOrDefault();
            if (existedVacancy is null)
                return null;
            var seats = _applicationRepo.GetFilter(filter: a=> a.VacancyId == id).Count();
            var vacancy = new ShowVacanyDto()
            {
                Id = existedVacancy.Id,
                Name = existedVacancy.Name,
                Description = existedVacancy.Description,
                MaxNumber = existedVacancy.MaxNumber,
                RemainSeats = existedVacancy.MaxNumber - seats,
                ExpiryDate = existedVacancy.ExpiryDate,
                IsActive = existedVacancy.IsActive,
                CreatedBy = existedVacancy.Employer.UserName
            };
            return vacancy;
        }
        public List<ShowVacanyDto> GetByName(string name)
        {

            return _vacancyRepo.GetFilter(filter: a => a.Name.Contains(name), include: a => a.Include(v => v.Employer))
                .Select(v => new ShowVacanyDto()
                {
                    Name = v.Name,
                    Id = v.Id,
                    CreatedBy = v.Employer.UserName,
                    MaxNumber = v.MaxNumber,
                    IsActive = v.IsActive,
                    Description = v.Description,
                    ExpiryDate = v.ExpiryDate,
                }).ToList();
        }
        public bool AddVacancy(VacancyDto vacancyDto, string EmployerId)
        {
            // vacancy dto validated using the validator;
            var vacancyModel = MapDtoToModel(vacancyDto);
            vacancyModel.EmployerId = EmployerId;
            bool result = _vacancyRepo.Add(vacancyModel);
            return result;
        }
        public bool UpdateVacancy(int id,string employerId, VacancyDto vacancyDto)
        {
            var existingVacancy = GetOwnedVacancy(id, employerId);
            if(existingVacancy is null)
                return false;
            existingVacancy.Name = vacancyDto.Name;
            existingVacancy.Description = vacancyDto.Description;
            existingVacancy.ExpiryDate = vacancyDto.ExpiryDate;
            existingVacancy.MaxNumber = vacancyDto.MaxNumber;
            bool result = _vacancyRepo.Update(existingVacancy);
            return result;
        }
        public bool RemoveVacancy(int id, string employerId)
        {
            var existingVacancy = GetOwnedVacancy(id, employerId);
            if(existingVacancy is null)
                return false ;
            bool result = _vacancyRepo.Delete(id);
            return result;
        }
        public bool DeActive(int id, string employerId)
        {
            var existingVacancy = GetOwnedVacancy(id, employerId);
            if (existingVacancy is null)
                return false;
            existingVacancy.IsActive = false;
            bool result = _vacancyRepo.Update(existingVacancy);
            return result ;
        }
        private Dictionary<int,int> VacanciesApplicationsCount()
        {
            var counts = _applicationRepo.GetAll()
                .GroupBy(v => v.VacancyId)
                .ToDictionary(v => v.Key, v => v.Count()); 
            return counts;
        }
        private Vacancy MapDtoToModel(VacancyDto vacancyDto) 
        {
            var vacancyModel = new Vacancy()
            {
                Name = vacancyDto.Name,
                Description = vacancyDto.Description,
                MaxNumber = vacancyDto.MaxNumber,
                ExpiryDate = vacancyDto.ExpiryDate,
                IsActive = true,
            };
            return vacancyModel;
        }
        private Vacancy GetOwnedVacancy(int id, string employerId)
        {
            var ExistingVacancy = _vacancyRepo.GetById(id);
            if (ExistingVacancy == null || ExistingVacancy.EmployerId != employerId)
                return null;
            return ExistingVacancy;   
        }
    }
}
