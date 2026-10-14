using System.Net.Http.Json;


namespace TTClassLibrary.RequestSender
{
    public static class UserManagerRequestSender
    {

        public static async Task<HttpResponseMessage> Register(this IHttpRequestSender requestSender, JsonContent content)
        {
            var url = $"api/Auth/Register";
            return await requestSender.Post(url, content);
        }

        public static async Task<HttpResponseMessage> Login(this IHttpRequestSender requestSender, JsonContent content)
        {
            var url = $"api/Auth/Login_";
            return await requestSender.Post(url, content);
        }

        public static async Task<HttpResponseMessage> PullAdminList(this IHttpRequestSender requestSender)
        {
            var url = $"api/Account/GetUserList";
            return await requestSender.Get(url);
        }

        public static async Task<HttpResponseMessage> DeleteUser(this IHttpRequestSender requestSender, string id)
        {
            var url = $"api/Account/DeleteUser?id={id}";
            return await requestSender.Delete(url);
        }

        public static async Task<HttpResponseMessage> ChangeUserRole(this IHttpRequestSender requestSender, string id, JsonContent content)
        {
            var url = $"api/Account/ChangeUserRole?id={id}";
            return await requestSender.Put(url, content);
        }

        public static async Task<HttpResponseMessage> ChangeCurrentUserPassword(this IHttpRequestSender requestSender, JsonContent content)
        {
            var url = $"api/Auth/ChangeCurrentUserPassword";
            return await requestSender.Put(url, content);
        }

        public static async Task<HttpResponseMessage> ResetUserPassword(this IHttpRequestSender requestSender, string userId, JsonContent content)
        {
            var url = $"api/Account/ChangeUserPassword?id={userId}";
            return await requestSender.Put(url, content);
        }

        public static async Task<HttpResponseMessage> ResetCurrentUserPassword(this IHttpRequestSender requestSender, JsonContent content)
        {
            var url = $"api/Account/ChangeUserPassword";
            return await requestSender.Put(url, content);
        }
    }
}
