using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Contracts.Persistence;
using Application.Contracts.Services;
using Application.DTOs.User;
using Application.Models;
using Domain;

namespace Persistence.Services
{
    public class UserService : Service<User, UserDto>, IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository, IMapper mapper, IUnitOfWork unitOfWork) : base(userRepository, mapper, unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public async Task<UserDto> GetByEmail(string email)
        {
            var entity = await _userRepository.GetByEmail(email);
            return _mapper.Map<UserDto>(entity);
        }

        public async Task<UserDto> GetByIdentifier(Guid guid)
        {
            var entity = await _userRepository.GetByIdentifier(guid);
            return _mapper.Map<UserDto>(entity);
        }

        public async Task<bool> IsValidIdentifier(Guid guid)
        {
            return await _userRepository.IsValidIdentifier(guid);
        }
    }
}
