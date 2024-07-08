using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Auth
{
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        [NotMapped]
        public string? FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
    }
}
