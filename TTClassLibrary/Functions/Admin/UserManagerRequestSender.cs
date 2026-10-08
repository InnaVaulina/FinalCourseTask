using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TTClassLibrary.Functions.Authorization;
using TTClassLibrary.IServices;

namespace TTClassLibrary.Functions.Admin
{
    public class UserManagerRequestSender
    {
        protected IHttpRequestSender requestSender;
        public UserManagerRequestSender(IHttpRequestSender _requestSender)
        {
            requestSender = _requestSender;
        }

        public async Task<HttpResponseMessage> Register(JsonContent content)
        {
            var url = $"api/Account/Register";
            return await requestSender.Post(url, content);
        }

        public async Task<HttpResponseMessage> Login(JsonContent content)
        {
            var url = $"api/Account/Login_";
            return await requestSender.Post(url, content);
        }

        public async Task<HttpResponseMessage> PullAdminList()
        {
            var url = $"api/Account/GetUserList";
            return await requestSender.Get(url);
        }

        public async Task<HttpResponseMessage> DeleteUser(string id)
        {
            var url = $"api/Account/DeleteUser?id={id}";
            return await requestSender.Delete(url);
        }

        public async Task<HttpResponseMessage> ChangeUserRole(string id, JsonContent content)
        {
            var url = $"api/Account/ChangeUserRole?id={id}";
            return await requestSender.Put(url, content);
        }

        public async Task<HttpResponseMessage> ChangeCurrentUserPassword(JsonContent content)
        {
            var url = $"api/Account/ChangeCurrentUserPassword";
            return await requestSender.Put(url, content);
        }

        public async Task<HttpResponseMessage> ResetUserPassword(string userId, JsonContent content)
        {
            var url = $"api/Account/ChangeUserPassword?id={userId}";
            return await requestSender.Put(url, content);
        }
    }
}
