using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTClassLibrary.DataModel
{
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

    public class PasswordModel
    {
        public string Password { get; set; }
    }

    public class RegisterModel
    {
        string loginProp;
        string password;
        string confirmPassword;


        public RegisterModel()
        {
            loginProp = "";
            password = "";
            confirmPassword = "";

        }

        public void Clear()
        {
            loginProp = "";
            password = "";
            confirmPassword = "";

        }

        public string LoginProp
        {
            get { return loginProp; }
            set { loginProp = value; }
        }


        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set { confirmPassword = value; }
        }




    }
}
