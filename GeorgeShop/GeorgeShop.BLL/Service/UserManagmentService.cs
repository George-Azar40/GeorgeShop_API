using GeorgeShop.DAL.DTO.Response;
using GeorgeShop.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeorgeShop.BLL.Service
{
    public class UserManagmentService : IUserManagmentService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserManagmentService(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<bool> ChangeRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roleExist = await _roleManager.RoleExistsAsync(role);
            if(!roleExist) { return false; }

            // should remove user role
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            var result = await _userManager.AddToRoleAsync(user, role);
            return result.Succeeded;
        }

        public Task<bool> DeleteUser(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<UserListResponse>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            return users.Adapt<List<UserListResponse>>();
        }

        public async Task<UserDetailsResponse?> GetUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var roles = await _userManager.GetRolesAsync(user);
            var result = user.Adapt<UserDetailsResponse>();
            result.Role = roles.FirstOrDefault();

            return result;
        }

        public async Task<bool> ToogleBlockUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var IsBlocked = user.LockoutEnd != null && user.LockoutEnd > DateTime.UtcNow;
            if (IsBlocked)
            {
                await _userManager.SetLockoutEndDateAsync(user, null);
            }
            else
            {
                await _userManager.SetLockoutEnabledAsync(user, true);
                await _userManager.SetLockoutEndDateAsync(user, DateTime.UtcNow.AddDays(5));
            }

            return true;
        }
    }
}
