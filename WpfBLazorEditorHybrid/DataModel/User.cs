using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBLazorHybridClient.DataModel
{
    public class User
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        public List<UserRole> UserRoles { get; set; }

        public string Token { get; set; }

        public User()
        {
        }
    }


    public class UserRole
    {
        public string Role { get; set; }


    }
}
