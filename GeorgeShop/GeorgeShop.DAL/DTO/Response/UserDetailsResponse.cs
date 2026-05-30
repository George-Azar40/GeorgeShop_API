using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeorgeShop.DAL.DTO.Response
{
    public class UserDetailsResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string IsBlocked { get; set; }
        public string Role { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        
    }
}
