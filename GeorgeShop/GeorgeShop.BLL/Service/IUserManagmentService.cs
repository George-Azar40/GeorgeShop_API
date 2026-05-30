using GeorgeShop.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeorgeShop.BLL.Service
{
    public interface IUserManagmentService
    {
        Task<List<UserListResponse>> GetAllUsers();
        Task<UserDetailsResponse?> GetUser(string userId);
        Task<bool> ChangeRole(string userId, string role);
        Task<bool> ToogleBlockUser(string userId);
        Task<bool> DeleteUser(string userId);

    }
}
