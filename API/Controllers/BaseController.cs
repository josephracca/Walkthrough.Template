using Application.Constant;
using Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public int UserId { get; set; }

        public BaseController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _ = int.TryParse(_httpContextAccessor.HttpContext.User.FindFirst(UserClaim.Id)?.Value, out int uid);
            UserId = uid;
        }

        private string GetClaim(string claimType)
        {
            var claim = _httpContextAccessor.HttpContext.User.FindFirst(f => f.Type == claimType);
            if (claim == null)
            {
                throw new CustomException(404, "User not found");
            }

            return claim.Value;
        }
    }
}

