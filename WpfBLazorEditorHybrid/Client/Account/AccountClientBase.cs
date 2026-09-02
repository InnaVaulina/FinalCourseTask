using System;
using System.Configuration;
using System.Net.Http.Json;
using System.Net.Http;
using WpfBLazorHybridClient.Client.Account.UserModel;
using TTClassLibrary.Support;




namespace WpfBLazorHybridClient.Client.Account
{
    public class AccountClientBase
    {
        public string baseAddress = ConfigurationManager.AppSettings["ApiUrl"];

        public class TokenResponse
        {
            public string? Token { get; set; }
        }

        TTClassLibrary.Support.HttpResponseMessageDeserialize<TokenResponse> serializer;


        public AccountClientBase() 
        {
            serializer = new TTClassLibrary.Support.HttpResponseMessageDeserialize<TokenResponse>();
        }



        public async Task<HttpResponseMessage> Login(UserEntryM model)
        {
            using (var client = new HttpClient())
                try
                {
                    var url = $"{baseAddress}/api/Account/Login_";
                    JsonContent content = JsonContent.Create(model);
                    HttpResponseMessage result = await client.PostAsync(url, content);
                    return result;
                }
                catch (Exception ex)
                {

                }
            return null;
        }


        public async Task<HttpResponseMessage> Register(RegisterM model)
        {
            using (var client = new HttpClient())
                try
                {
                    var url = $"{baseAddress}/api/Account/Register";
                    JsonContent content = JsonContent.Create(model);
                    HttpResponseMessage result = await client.PostAsync(url, content);
                    return result;
                }
                catch (Exception ex)
                {

                }

            return null;
        }



        public string? DeserializeToken(HttpResponseMessage response)
        {
            try
            {
                var tokenResponse = serializer.DeserializeResponce(response);
                return tokenResponse.Token;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


    }
}
