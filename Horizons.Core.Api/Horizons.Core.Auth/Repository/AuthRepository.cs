using System.Net;
using System.Web;
using Horizons.Core.Auth.Constants;
using Horizons.Core.Auth.Dtos;
using Horizons.Core.Auth.Identity.Interface;
using Horizons.Core.Auth.Models;
using Horizons.Core.Auth.Repository.Interface;
using Horizons.Core.Auth.Service.Interface;
using Microsoft.AspNetCore.Identity;

namespace Horizons.Core.Auth.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtProvider _jwtProviders;
        private readonly IMailSender _mailSender;

        public AuthRepository(
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtProvider jwtProviders, 
            IMailSender mailSender)
        {
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
                    Message = result.Errors.First().Description,
                    IsSuccess = false
                };
            }
            
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebUtility.UrlEncode(token);

            await _mailSender.SendEmailAsync(
                user.Email, "Email Confirmation",
                "Please confirm your email by clicking this " +
                "link: <a href='https://horizon-centers.com/confirm-email/" + user.Id + "/" + token +
                "'>Confirm Email</a>");

            // Ensure the user role exists
            if (!await _roleManager.RoleExistsAsync(HorizonsCoreAuthRoles.UserRole))
            {
                var roleCreationResult = await _roleManager.CreateAsync(new IdentityRole(HorizonsCoreAuthRoles.UserRole));
                if (!roleCreationResult.Succeeded)
                {
                    return new RequestResponse
                    {
                        Message = roleCreationResult.Errors.First().Description,
                        IsSuccess = false
                    };
                }
            }

            // Assign user to role
            await _userManager.AddToRoleAsync(user, HorizonsCoreAuthRoles.UserRole);

            return new RequestResponse
            {
                Message = "User was created",
                IsSuccess = true
            };
        }

        public async Task<bool> UserExists(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<RequestResponse> Login(LoginRequest? loginDto)
        {
            // Validate input
            if (loginDto == null || string.IsNullOrWhiteSpace(loginDto.Email) || string.IsNullOrWhiteSpace(loginDto.Password))
            {
                return new RequestResponse
                {
                    IsSuccess = false,
                    Message = "Invalid login request"
                };
            }

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return new RequestResponse
                {
                    IsSuccess = false,
                    Message = "User not found"
                };
            }

            // Check password validity
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordValid)
            {
                return new RequestResponse
                {
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
                Message = "Login successful"
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
            // Here you would typically send the token to the user's email.
            // For example, you might use an email service to send the token.

            return new RequestResponse { IsSuccess = true, Message = "Password reset token generated", Result = token };
        }

        public async Task<RequestResponse> ResetPassword(ResetPasswordRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                return new RequestResponse
                {
                    IsSuccess = false,
                    Message = "User does not exist"
                };
            }

            var purpose = _userManager.Options.Tokens.PasswordResetTokenProvider;
            bool isTokenValid = await _userManager.VerifyUserTokenAsync(user, purpose, "ResetPassword", model.Token);

            if (!isTokenValid)
            {
                return new RequestResponse
                {
                    IsSuccess = false,
                    Message = "Invalid or expired token"
                };
            }
            
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (result.Succeeded)
            {
                return new RequestResponse { IsSuccess = true, Message = "Password reset successful" };
            }

            return new RequestResponse { IsSuccess = false, Message = "Password reset failed" };

        }

        public async Task<RequestResponse> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new RequestResponse { IsSuccess = false, Message = "User not found" };
            }

            token = WebUtility.UrlDecode(token);
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return new RequestResponse { IsSuccess = true, Message = "Email confirmed successfully" };
            }

            return new RequestResponse { IsSuccess = false, Message = result.Errors.First().Description };
        }

        private async Task CreateRole(List<string> roles)
        {
            foreach (var role in roles)
            {
                if (!_roleManager.RoleExistsAsync(role).GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
