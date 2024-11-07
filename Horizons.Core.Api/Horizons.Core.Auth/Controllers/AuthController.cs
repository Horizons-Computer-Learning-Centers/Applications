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
        public async Task<ResponseDto> Register(RegistrationRequestDto registerDto)
        {
            return await _authService.Register(registerDto);
        }

        [HttpPost("login")]
        public async Task<ResponseDto> Login(LoginRequestDto loginDto)
        {
            return await _authService.Login(loginDto);
        }

        [HttpPost("assign-role")]
        public async Task<IList<string>> AssignRole(string email, List<string> roles)
        {
            return await _authService.AssignRole(email, roles);
        }
    }
}
