
using System.Net.Http.Headers;
using System.Net.Http;
using WpfBLazorHybridClient.DataModel;
using WpfBLazorHybridClient.Client.Account.UserModel;
using System.Net.Http.Json;


namespace WpfBLazorHybridClient.Client.Account
{
    public class AccountClientExt : AccountClientBase
    {
        public AccountClientExt(User user) : base()
        {
            this.user = user;
        }

        User user;

        public async Task<HttpResponseMessage> PullAdminList()
        {
            using (var client = new HttpClient())
                try
                {
                    var url = $"{baseAddress}/api/Account/GetUserList";
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
                    HttpResponseMessage result = await client.GetAsync(url);
                    return result;
                }
                catch (Exception ex)
                {

                }

            return null;
        }

        public async Task<HttpResponseMessage> DeleteUser(UserModelForAdmin usermodel)
        {
            using (var client = new HttpClient())
                try
                {
                    var url = $"{baseAddress}/api/Account/DeleteUser?id={usermodel.Id}";
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
                    HttpResponseMessage result = await client.DeleteAsync(url);
                    return result;
                }
                catch (Exception ex)
                {

                }

            return null;

        }

        public async Task<HttpResponseMessage> ChangeUserRole(UserModelForAdmin usermodel)
        {
            using (var client = new HttpClient())
                try
                {
                    var url = $"{baseAddress}/api/Account/ChangeUserRole?id={usermodel.Id}";
                    JsonContent content = JsonContent.Create(usermodel.UserRoles);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
                    HttpResponseMessage result = await client.PutAsync(url, content);
                    return result;
                }
                catch (Exception ex)
                {

                }

            return null;

        }

        public async Task<HttpResponseMessage> ChangeCurrentUserPassword(PasswordChangeModel model)
        {
            using (var client = new HttpClient())
                try
                {
                    var url = $"{baseAddress}/api/Account/ChangeCurrentUserPassword";
                    JsonContent content = JsonContent.Create(model);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
                    HttpResponseMessage result = await client.PostAsync(url, content);
                    return result;
                }
                catch (Exception ex)
                {

                }

            return null;
        }


        public async Task<HttpResponseMessage> ResetUserPassword(string userId, PasswordModel model)
        {
            using (var client = new HttpClient())
                try
                {
                    var url = $"{baseAddress}/api/Account/ChangeUserPassword?id={userId}";
                    JsonContent content = JsonContent.Create(model);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
                    HttpResponseMessage result = await client.PostAsync(url, content);
                    return result;
                }
                catch (Exception ex)
                {

                }

            return null;
        }

    }
}
