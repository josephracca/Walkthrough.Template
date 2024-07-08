using Application.DTOs.Auth;
using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Identity
{
    public interface IAuthService
    {
        Task<LoginResponseDto> Login(UserLoginDto auth);
        Task<bool> CreatePassword(string password, Guid guid);
        Task<bool> ForgotUsername(string email);
        Task<bool> ForgotPassword(string email);
        Task UpdatePassword(UpdatePasswordDto PasswordDto);
        Task<bool> ChangePassword(string password, Guid guid);
        Task<bool> IsCaptchaValid(string token);
    }
}
