
namespace WpfBLazorHybridClient.Client.Account.UserModel
{
    public class PasswordModel
    {
        public string Password { get; set; }
    }

    public class PasswordChangeModel
    {
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }
    }
}
