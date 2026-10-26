namespace WebApplication1.Controllers.UserModel
{
    public class PasswordModel
    {
        public string Password {  get; set; }
    }

    public class PasswordChangeModel
    {
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }
    }

    public class PasswordResetModel
    {
        public string LoginProp { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
