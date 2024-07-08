using Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Application.Contracts.Services
{
    public interface IUserService : IService<User, UserDto>
    {
        Task<UserDto> GetByEmail(string email);
        Task<bool> IsValidIdentifier(Guid guid);
        Task<UserDto> GetByIdentifier(Guid guid);
    }
}
