using Application.Contracts.Services;
using Application.DTOs.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Web.Http.Filters;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        public UserController(
            IUserService userService,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _userService = userService;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> Get(int id)
        {
            var user = await _userService.Get(id);

            return Ok(user);
        }

        [HttpPost]
        public async Task<bool> Add(UserDto user)
        {
            return await _userService.Add(user);
        }

        [HttpGet("valid-identifier/{identifier}")]
        public async Task<bool> IsValidIdentifier(Guid identifier)
        {
            return await _userService.IsValidIdentifier(identifier);
        }

        [HttpGet("identifier/{identifier}")]
        public async Task<ActionResult<UserDto>> GetByIdentifier(Guid identifier)
        {
            return await _userService.GetByIdentifier(identifier);
        }
    }
}
