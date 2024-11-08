using Horizons.Core.Auth.Dtos;
using Horizons.Core.Auth.Repository.Interface;
using Horizons.Core.Auth.Service.Interface;

namespace Horizons.Core.Auth.Service
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<IList<string>> AssignRole(string email, List<string> roles)
        {
            return await _authRepository.AssignRole(email, roles);
        }

        public async Task<RequestResponse> ForgotPassword(ForgotPasswordRequest model)
        {
            var response = await _authRepository.ForgotPassword(model);
            return response;
        }

        public async Task<RequestResponse> ResetPassword(ResetPasswordRequest model)
        {
            return await _authRepository.ResetPassword(model);
        }

        public async Task<RequestResponse> ConfirmEmail(string userId, string token)
        {
            return await _authRepository.ConfirmEmail(userId, token);
        }

        public async Task<RequestResponse> Register(RegistrationRequestDto registerDto)
        {
            return await _authRepository.Register(registerDto);
        }

        public Task<bool> UserExists(string email)
        {
            return _authRepository.UserExists(email);
        }

        public async Task<RequestResponse> Login(LoginRequest loginRequestDto)
        {
            return await _authRepository.Login(loginRequestDto);
        }

    }
}
