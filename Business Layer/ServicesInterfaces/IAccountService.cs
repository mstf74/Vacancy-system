using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Layer.Dtos;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Identity;

namespace Business_Layer.ServicesInterfaces
{
    public interface IAccountService
    {
        Task<IdentityResult> Register(RegisterDto userDto);
        Task<IdentityResult> Login(LoginDto userDto);
        Task<string> GeneratToken(string email);
    }
}
