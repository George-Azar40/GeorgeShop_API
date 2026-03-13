using GeorgeShop.DAL.DTO.Request;
using GeorgeShop.DAL.DTO.Response;
using GeorgeShop.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeorgeShop.BLL.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        public AuthenticationService(UserManager<ApplicationUser> userManager , IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
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

            var EmailURL = $"https://localhost:7053/api/account/confirm?token={token}&id={user.Id}";

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
                Success = true
            };
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
    }
}
