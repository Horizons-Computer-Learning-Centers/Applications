using System.Net;
using Horizons.Core.Auth.Configuration;
using Horizons.Core.Auth.Constants;
using Horizons.Core.Auth.Dtos;
using Horizons.Core.Auth.Enums;
using Horizons.Core.Auth.Identity.Interface;
using Horizons.Core.Auth.Models;
using Horizons.Core.Auth.Repository.Interface;
using Horizons.Core.Auth.Service.Interface;
using Microsoft.AspNetCore.Identity;

namespace Horizons.Core.Auth.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppUrlSettings _appUrlSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtProvider _jwtProviders;
        private readonly IMailSender _mailSender;

        public AuthRepository(
            AppUrlSettings appUrlSettings,
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtProvider jwtProviders, 
            IMailSender mailSender)
        {
            _appUrlSettings = appUrlSettings;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _jwtProviders = jwtProviders;
            _mailSender = mailSender;
        }

        public async Task<RequestResponse> Register(RegistrationRequest register)
        {
            // Check if user already exists
            if (await UserExists(register.Email))
            {
                return new RequestResponse
                {
                    ResponseType = ResponseTypeEnum.UserAlreadyExists,
                    Message = "User already exists",
                    IsSuccess = false
                };
            }

            // Create new user instance
            ApplicationUser user = new ApplicationUser
            {
                UserName = register.Email,
                Email = register.Email,
                PhoneNumber = register.PhoneNumber,
                FirstName = register.FirstName,
                LastName = register.LastName
            };

            // Create user
            var result = await _userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded)
            {
                return new RequestResponse
                {
                    ResponseType = ResponseTypeEnum.UserNotCreated,
                    Message = result.Errors.First().Description,
                    IsSuccess = false
                };
            }
            
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebUtility.UrlEncode(token);

            var message = $"Please confirm your email by clicking this link: <a href='{_appUrlSettings.Frontend}/confirm-email/{user.Id}/{token}'>Confirm Email</a>";
            await _mailSender.SendEmailAsync(user.Email, "Email Confirmation", message);

            // Ensure the user role exists
            if (!await _roleManager.RoleExistsAsync(HorizonsCoreAuthRoles.UserRole))
            {
                var roleCreationResult = await _roleManager.CreateAsync(new IdentityRole(HorizonsCoreAuthRoles.UserRole));
                if (!roleCreationResult.Succeeded)
                {
                    return new RequestResponse
                    {
                        ResponseType = ResponseTypeEnum.RoleNotCreated,
                        Message = roleCreationResult.Errors.First().Description,
                        IsSuccess = false
                    };
                }
            }

            // Assign user to role
            await _userManager.AddToRoleAsync(user, HorizonsCoreAuthRoles.UserRole);

            return new RequestResponse
            {
                ResponseType = ResponseTypeEnum.UserAddedToRole,
                Message = "User was created",
                IsSuccess = true
            };
        }

        public async Task<bool> UserExists(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<RequestResponse> Login(LoginRequest loginDto)
        {
            // Validate input
            if (loginDto == null || string.IsNullOrWhiteSpace(loginDto.Email) || string.IsNullOrWhiteSpace(loginDto.Password))
            {
                return new RequestResponse
                {
                    ResponseType = ResponseTypeEnum.InvalidLogin,
                    IsSuccess = false,
                    Message = "Invalid login request"
                };
            }

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return new RequestResponse
                {
                    ResponseType = ResponseTypeEnum.UserNotFound,
                    IsSuccess = false,
                    Message = "User not found"
                };
            }

            // Check if email is confirmed
            var emailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            if (!emailConfirmed)
            {
                return new RequestResponse
                {
                    ResponseType = ResponseTypeEnum.EmailNotConfirmed,
                    IsSuccess = false,
                    Message = "Email not confirmed"
                };
            }

            // Check password validity
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordValid)
            {
                return new RequestResponse
                {
                    ResponseType = ResponseTypeEnum.InvalidPassword,
                    IsSuccess = false,
                    Message = "Incorrect password"
                };
            }

            // Get user roles and generate token
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtProviders.GenerateJwtToken(user, roles);

            return new RequestResponse
            {
                Result = new
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Token = token
                },
                IsSuccess = true,
                Message = "Login successful",
                ResponseType = ResponseTypeEnum.LoginSuccess,
            };
        }
        
        public async Task<RequestResponse> ForgotPassword(ForgotPasswordRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new RequestResponse { IsSuccess = false, Message = "User not found" };
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = WebUtility.UrlEncode(token);

            var message = $"Please reset your password by clicking this link: <a href='{_appUrlSettings.Frontend}/reset-password/{user.Email}/{token}'>Reset Password</a>";
            await _mailSender.SendEmailAsync(user.Email, "Reset Password", message);

            return new RequestResponse
            {
                IsSuccess = true,
                Message = "Password reset token generated",
                ResponseType = ResponseTypeEnum.PasswordResetTokenGenerated,
                Result = null
            };
        }

        public async Task<RequestResponse> ResetPassword(ResetPasswordRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                return new RequestResponse
                {
                    IsSuccess = false,
                    Message = "User does not exist",
                    ResponseType = ResponseTypeEnum.UserNotFound,
                };
            }

            var purpose = _userManager.Options.Tokens.PasswordResetTokenProvider;
            bool isTokenValid = await _userManager.VerifyUserTokenAsync(user, purpose, "ResetPassword", model.Token);

            if (!isTokenValid)
            {
                return new RequestResponse
                {
                    IsSuccess = false,
                    Message = "Invalid or expired token",
                    ResponseType = ResponseTypeEnum.PasswordResetTokenInvalid,
                };
            }
            
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (result.Succeeded)
            {
                return new RequestResponse
                {
                    IsSuccess = true,
                    Message = "Password reset successful",
                    ResponseType = ResponseTypeEnum.PasswordResetSuccess,
                };
            }

            return new RequestResponse
            {
                IsSuccess = false, 
                Message = "Password reset failed",
                ResponseType = ResponseTypeEnum.PasswordResetFailed,
            };

        }

        public async Task<RequestResponse> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new RequestResponse
                {
                    IsSuccess = false,
                    Message = "User not found",
                    ResponseType = ResponseTypeEnum.UserNotFound,
                };
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return new RequestResponse
                {
                    IsSuccess = true,
                    Message = "Email confirmed successfully",
                    ResponseType = ResponseTypeEnum.EmailConfirmed,
                };
            }

            return new RequestResponse
            {
                IsSuccess = false,
                Message = result.Errors.First().Description,
                ResponseType = ResponseTypeEnum.EmailNotConfirmed,
            };
        }
    }
}
