using GeorgeShop.DAL.DTO.Request;
using GeorgeShop.DAL.DTO.Response;
using GeorgeShop.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GeorgeShop.BLL.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthenticationService(UserManager<ApplicationUser> userManager ,
            IEmailSender emailSender,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

      

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            var user = request.Adapt<ApplicationUser>();

            var result = await _userManager.CreateAsync(user , request.Password);


            if(!result.Succeeded) 
            return new RegisterResponse
            {
                Success = false,
                Message = string.Join(", ", result.Errors.Select(e => e.Description))
            };

            await _userManager.AddToRoleAsync(user, "User");
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = Uri.EscapeDataString(token);

            var EmailURL = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/account/confirm?token={token}&id={user.Id}";

            await _emailSender.SendEmailAsync(user.Email, "Welcome",
                $"< h1 > Welcome {request.UserName}</ h1 >"
                + $"" +
                $"<a href='{EmailURL}'> Confirm </a>"

                );

            return new RegisterResponse
            {
                Success = true,
                Message = "Success"
            };
        }
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if(user is null)
            {
                return new LoginResponse()
                {
                    Message = "Invalid Email",
                    Success = false
                };
            }

            if(!await _userManager.IsEmailConfirmedAsync(user))
            {
                return new LoginResponse()
                {
                    Message = "Please Confirm Your Email",
                    Success = false
                };
            }

           var result = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!result)
            {
                return new LoginResponse()
                {
                    Message = "Invalid Password",
                    Success = false
                };
            }

            return new LoginResponse()
            {
                Message = $"Welcome {user.UserName} Login Successfully",
                Success = true,
                AccessToken = await GenerateAccessToken(user)
            };
        }

        private async Task<string> GenerateAccessToken(ApplicationUser user)
        {
            

            var userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name , user.UserName),
                new Claim(ClaimTypes.Email , user.Email)
            };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: userClaims,
            expires: DateTime.Now.AddDays(5),
            signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> confirmEmailAsync(string token , string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user is null) 
                return false;   

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if(!result.Succeeded)
                return false;
            return true;

        }

        public async Task<ForgetPasswordResponse> RequestPasswordResetAsync(ForgetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if(user is null)
            {
                return new ForgetPasswordResponse
                {
                    Message = "Invalid Email",
                    Success = false
                };
            }

            var random = new Random();
            var code = random.Next(1000,9999).ToString();

            user.CodeResetPassword = code;
            user.PasswordResetCodeExpiry = DateTime.UtcNow.AddMinutes(3);

            await _userManager.UpdateAsync(user);

            await _emailSender.SendEmailAsync(request.Email, "Reset Password", $"<p>Your Reset Code is{code}</p>");

            return new ForgetPasswordResponse
            {
                Message = "Code Sent to your Email",
                Success = true
            };
        }

        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new ResetPasswordResponse
                {
                    Message = "Invalid Email",
                    Success = false
                };
            } else if(user.CodeResetPassword != request.Code)
            {
                return new ResetPasswordResponse
                {
                    Message = "Invalid Code",
                    Success = false
                };
            }else if(user.PasswordResetCodeExpiry < DateTime.UtcNow)
            {
                return new ResetPasswordResponse
                {
                    Message = "Code Expired",
                    Success = false
                };
            }

            var IsSamePassword = await _userManager.CheckPasswordAsync(user, request.NewPassword);

            if (IsSamePassword)
            {
                return new ResetPasswordResponse
                {
                    Message = "new password must be different",
                    Success = false
                };
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            
            var result = await _userManager.ResetPasswordAsync(user , token, request.NewPassword);

            if (!result.Succeeded)
            {
                return new ResetPasswordResponse
                {
                    Message = "Password Reset Failed",
                    Success = false
                };
            }
            await _emailSender.SendEmailAsync(request.Email , "change password" , "<p>Your Password was Recently Changed</p>");
            return new ResetPasswordResponse
            {
                Message = "Password Reset Success",
                Success = true
            };
        } 
    }
}
