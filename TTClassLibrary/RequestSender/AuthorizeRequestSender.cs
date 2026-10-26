using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using TTClassLibrary.Support;
using TTClassLibrary.DataModel;

namespace TTClassLibrary.RequestSender
{
    public static class AuthorizeRequestSender
    {
        public static string? DeserializeToken(this IHttpRequestSender requestSender, HttpResponseMessage response)
        {
            try
            {
                var serializer = new HttpResponseMessageDeserialize<TokenResponse>();
                var tokenResponse = serializer.DeserializeResponce(response);
                return tokenResponse.Token;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        public static async Task<HttpResponseMessage> Login(this IHttpRequestSender requestSender, JsonContent content)
        {
            var url = $"api/Auth/Login_";
            return await requestSender.Post(url, content);
        }

        public static async Task<HttpResponseMessage> Register(this IHttpRequestSender requestSender, JsonContent content)
        {
            var url = $"api/Auth/Register";
            return await requestSender.Post(url, content);
        }

        public static async Task<HttpResponseMessage> ResetUserPasswordWithToken(this IHttpRequestSender requestSender, JsonContent content)
        {
            var url = $"api/Auth/ResetUserPasswordWithToken";
            return await requestSender.Put(url, content);
        }

        public static async Task<HttpResponseMessage> ResetCurrentUserPassword(this IHttpRequestSender requestSender, JsonContent content)
        {
            var url = $"api/Auth/ChangeCurrentUserPassword";
            return await requestSender.Put(url, content);
        }

    }

    public class TokenResponse
    {
        public string? Token { get; set; }
    }
}
