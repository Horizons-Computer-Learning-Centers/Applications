using Horizons.Core.Auth.Dtos;
using Horizons.Core.Auth.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Horizons.Core.Auth.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        
        [HttpPost("register")]
        public async Task<RequestResponse> Register(RegistrationRequestDto registerDto)
        {

            return await _authService.Register(registerDto);
        }

        [HttpPost("login")]
        public async Task<RequestResponse> Login(LoginRequest loginDto)
        {
            return await _authService.Login(loginDto);
        }

        [HttpPost("assign-role")]
        public async Task<IList<string>> AssignRole(string email, List<string> roles)
        {
            return await _authService.AssignRole(email, roles);
        }

        [HttpPost("reset-password")]
        public async Task<RequestResponse> ResetPassword(ResetPasswordRequest model)
        {
            return await _authService.ResetPassword(model);
        }

        [HttpPost("forgot-password")]
        public async Task<RequestResponse> ForgotPassword(ForgotPasswordRequest model)
        {
            return await _authService.ForgotPassword(model);
        }

        [HttpPost("confirm-email")]
        public async Task<RequestResponse> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
        {
            return await _authService.ConfirmEmail(userId, token);
        }
    }
}
