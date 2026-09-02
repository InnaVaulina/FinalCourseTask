namespace TTClassLibrary.Functions.Authorization
{
    public class UserEntryM 
    {
        string login;
        string password;

        public string LoginProp
        {
            get { return login; }
            set { login = value; }
        }

        public string PassWord
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
}
