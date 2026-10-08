using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers.UserModel
{
    public class UserModelForAdmin
    {

        public string Id { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
    }


    public class UserModel
    {

        public string Id { get; set; }
        public string UserName { get; set; }
        public List<UserRole> Roles { get; set; }
    }

    public class UserRole
    {
        public string Role { get; set; }

    }
}
