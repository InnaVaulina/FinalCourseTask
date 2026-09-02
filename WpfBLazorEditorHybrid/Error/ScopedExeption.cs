using System;
using System.IO;

namespace WpfBLazorHybridClient.Error
{
    public class UserInfo 
    {
        public string Id { get; set; }
        public string UserName { get; set; }
    }
    public class ScopedExeption: Exception
    {
        public UserInfo? UserInfo { get; }
        public string Action { get; }
        public string OccurredAt { get; }

        public ScopedExeption(UserInfo? _userInfo, string _action, Exception exception)
            : base(exception?.Message, exception)
        {
            UserInfo = _userInfo;
            Action = _action ?? string.Empty;
            OccurredAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public override string ToString()
        {
            var userPart = UserInfo is null
                ? "User: null"
                : $"User: {UserInfo.UserName}, Id: {UserInfo.Id}";

            var ex = this.InnerException;
            var exPart = ex == null
                ? $"Exception: {Message}"
                : $"Exception: {ex.GetType().FullName}: {ex.Message}{Environment.NewLine}{ex.StackTrace}";

            return $"{OccurredAt} | Action: {Action} | {userPart}{Environment.NewLine} | {exPart}";
        }
    }
}
