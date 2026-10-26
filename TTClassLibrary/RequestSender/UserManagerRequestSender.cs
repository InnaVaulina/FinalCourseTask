using System.Net.Http.Json;


namespace TTClassLibrary.RequestSender
{
    public static class UserManagerRequestSender
    {

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

        public static async Task<HttpResponseMessage> GetTokenForChangingPassword(this IHttpRequestSender requestSender, string id)
        {
            var url = $"api/Account/GeneratePasswordResetToken?id={id}";
            return await requestSender.Get(url);
        }
    }
}
