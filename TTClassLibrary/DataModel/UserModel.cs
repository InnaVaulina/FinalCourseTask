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


    public class UserEntryM
    {
        string login;
        string password;

        public string LoginProp
        {
            get { return login; }
            set { login = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public UserEntryM()
        {
            login = "";
            password = "";
        }



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


    public class ResetPasswordModel
    {
        string loginProp;
        string token;
        string password;

        public ResetPasswordModel()
        {
            loginProp = "";
            token = "";
            password = "";
        }

        public void Clear()
        {
            loginProp = "";
            token = "";
            password = "";
        }

        public string LoginProp
        {
            get { return loginProp; }
            set { loginProp = value; }
        }

        public string Token
        {
            get { return token; }
            set { token = value; }
        }

        public string NewPassword
        {
            get { return password; }
            set { password = value; }
        }


      

    }

}
