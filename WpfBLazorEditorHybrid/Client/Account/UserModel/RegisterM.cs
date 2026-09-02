

namespace WpfBLazorHybridClient.Client.Account.UserModel
{
    public class RegisterM 
    {
        string loginProp;
        string password;
        string confirmPassword;
       

        public RegisterM()
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
