using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTClassLibrary.DataModel
{
    public class User
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        public List<UserRole> UserRoles { get; set; }

        public string Token { get; set; }

        public User()
        {
            Token = string.Empty;
        }
    }


    public class PasswordChangeModel
    {
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }
    }
}
