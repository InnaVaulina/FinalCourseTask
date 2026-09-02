using WpfBLazorHybridClient.DataModel;

namespace WpfBLazorHybridClient.Client.Account.UserModel
{
    public class UserModelForAdmin
    {
        string id;
        public string Id { get { return id; } set { id = value; } }

        string userName;
        public string UserName { get { return userName; } set { userName = value;  } }

        List<UserRole> userRoles;
        public List<UserRole> UserRoles { get { return userRoles; } set { userRoles = value; } }



        
    }


    
}
