using Data_Access_Layer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.Dtos
{
    public class RegisterDto
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string password { get; set; }
        public string phonenumber { get; set; }
        public Rules Rule { get; set; }
    }
}
