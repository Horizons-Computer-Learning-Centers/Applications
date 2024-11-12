using Horizons.Core.Auth.Dtos;

namespace Horizons.Core.Auth.Service.Interface
{
    public interface IAuthService
    {
        Task<RequestResponse> Register(RegistrationRequest register);
        Task<bool> UserExists(string email);
        Task<RequestResponse> Login(LoginRequest? loginDto);
        Task<RequestResponse> ForgotPassword(ForgotPasswordRequest model);
        Task<RequestResponse> ResetPassword(ResetPasswordRequest model);
        Task<RequestResponse> ConfirmEmail(string userId, string token);
        Task<bool> ValidateToken(string token);
    }
}
