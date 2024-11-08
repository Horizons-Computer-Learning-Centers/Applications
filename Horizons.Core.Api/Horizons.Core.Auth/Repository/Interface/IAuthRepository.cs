using Horizons.Core.Auth.Dtos;

namespace Horizons.Core.Auth.Repository.Interface
{
    public interface IAuthRepository
    {
        Task<RequestResponse> Register(RegistrationRequest register);
        Task<bool> UserExists(string email);
        Task<RequestResponse> Login(LoginRequest loginDto);
        Task<RequestResponse> ForgotPassword(ForgotPasswordRequest model);
        Task<RequestResponse> ResetPassword(ResetPasswordRequest model);
        Task<RequestResponse> ConfirmEmail(string userId, string token);
    }
}
