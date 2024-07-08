using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Persistence.Repositories;
using System.IdentityModel.Tokens.Jwt;
using Application.Contracts.Identity;
using Application.Contracts.Infrastructure;
using Application.Contracts.Persistence;
using Application.DTOs.Auth;
using Application.DTOs.User;
using Application.Exceptions;
using Application.Helper;
using Application.Models;

namespace Persistence.Identity
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JwtSettings _jwtSettings;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private string encryptionKey;
        private readonly IEmailSender _emailSender;
        private string _baseUrl;
        private const string GOOGLE_SITE_VERIFY_URL = "https://www.google.com/recaptcha/api/siteverify";
        private readonly Captcha _captcha;
        private string baseUrl;
        private readonly IStorageService _storageService;

        public AuthService(
            IHttpContextAccessor httpContextAccessor,
            IOptions<JwtSettings> jwtSettings,
            IUserRepository userRepository,
            IConfiguration configuration,
            IEmailSender emailSender,
            IUnitOfWork unitOfWork,
            IOptions<Captcha> captcha,
            IMapper mapper,
            IStorageService storageService)
        {
            _httpContextAccessor = httpContextAccessor;
            _jwtSettings = jwtSettings.Value;
            _userRepository = userRepository;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _mapper = mapper;
            encryptionKey = _jwtSettings.Key;
            baseUrl = _configuration["BaseUrl"];
            _captcha = captcha.Value;
            _storageService = storageService;
        }

        public async Task<bool> CreatePassword(string password, Guid guid)
        {
            password = AuthHelper.DecryptPassword(password, encryptionKey);

            var user = await _userRepository.GetByIdentifier(guid);

            if (user == null)
                throw new CustomException(403, $"Access is forbidden");
            if (user.IdentifierExpirationDate < DateTime.UtcNow)
                throw new CustomException(400, $"Exceeded the time limit");

            HashSaltDto hashSalt = AuthHelper.GenerateSaltedHash(password);
            user.Password = hashSalt.Hash;
            user.Salt = hashSalt.Salt;
            user.Identifier = null;
            user.IdentifierExpirationDate = null;

            await _userRepository.Update(user);
            return await _unitOfWork.Save(user.Id) > 0;
        }

        public async Task<bool> ChangePassword(string password, Guid guid)
        {
            password = AuthHelper.DecryptPassword(password, encryptionKey);

            var user = await _userRepository.GetByIdentifier(guid);

            if (user == null)
                throw new CustomException(403, $"Access is forbidden");
            if (user.Password != null && AuthHelper.VerifyPassword(user.Password, password, user.Salt))
                throw new CustomException(403, $"Your new password can not be your current password.");
            if (user.IdentifierExpirationDate < DateTime.UtcNow)
                throw new CustomException(400, $"Exceeded the time limit");

            HashSaltDto hashSalt = AuthHelper.GenerateSaltedHash(password);
            user.Password = hashSalt.Hash;
            user.Salt = hashSalt.Salt;
            user.Identifier = null;
            user.IdentifierExpirationDate = null;

            await _userRepository.Update(user);
            return await _unitOfWork.Save(user.Id) > 0;
        }

        public Task<bool> ForgotPassword(string email)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ForgotUsername(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsCaptchaValid(string token)
        {
            using var client = new HttpClient();
            var response = await client.PostAsync($"{GOOGLE_SITE_VERIFY_URL}?secret={_captcha.SecretKey}&response={token}", null);
            var jsonString = await response.Content.ReadAsStringAsync();
            var captchaVerification = JsonConvert.DeserializeObject<CaptchaVerificationResponse>(jsonString);
            return captchaVerification.Success;
        }

        public async Task<LoginResponseDto> Login(UserLoginDto auth)
        {
            auth.Password = AuthHelper.DecryptPassword(auth.Password, encryptionKey);
            var user = await _userRepository.GetByEmail(auth.EmailAddress);
            if (user == null)
                throw new CustomException(403, "Access is forbidden");

            if (!AuthHelper.VerifyPassword(user.Password, auth.Password, user.Salt))
            {
                throw new CustomException(400, "Invalid Password");
            }
            var userDto = _mapper.Map<UserDto>(user);
            var handler = new JwtSecurityTokenHandler();
            var token = AuthHelper.GenerateJWT(user.Id, _jwtSettings);

            return new LoginResponseDto
            {
                Token = handler.WriteToken(token),
                UserId = userDto.Id,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                EmailAddress = userDto.EmailAddress,
            };
        }

        public Task UpdatePassword(UpdatePasswordDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
