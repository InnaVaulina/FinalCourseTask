using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TTClassLibrary.IServices;
using TTClassLibrary.Support;

namespace TTClassLibrary.Functions.Authorization
{
    public class AuthorizeRequestSender
    {
        public class TokenResponse
        {
            public string? Token { get; set; }
        }

        HttpResponseMessageDeserialize<TokenResponse> serializer;
        IHttpRequestSender requestSender;

        public AuthorizeRequestSender(IHttpRequestSender _requestSender)
        {
            this.requestSender = _requestSender;
            serializer = new HttpResponseMessageDeserialize<TokenResponse>();
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


        public async Task<HttpResponseMessage> Login(UserEntryM model)
        {
            var url = $"api/Account/Login_";
            try
            {
                JsonContent content = JsonContent.Create(model);
                return await requestSender.Post(url, content);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
