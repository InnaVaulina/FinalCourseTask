using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TTClassLibrary.Support;
using TTClassLibrary.RequestSender;
using static TTClassLibrary.RequestSender.AuthorizeRequestSender;
using TTClassLibrary.DataModel;

namespace TTClassLibrary.Functions.Authorization
{
    public class EntryDM
    {
        IHttpRequestSender requestSender;

        UserEntryM entry;

        public UserEntryM Entry
        {
            get { return entry; }
            set { entry = value; }
        }
        public EntryDM(IHttpRequestSender _requestSender)
        {
            this.requestSender = _requestSender;
            entry = new UserEntryM();
        }

        public async Task<HttpResponseMessage> LoginNewUser()
        {
            var jsonContent = JsonContent.Create(entry);
            return await requestSender.Login(jsonContent);
        }

        public string? DeserializeToken(HttpResponseMessage response)
        {
            var serializer = new HttpResponseMessageDeserialize<TokenResponse>();
            var tokenResponse = serializer.DeserializeResponce(response);
            return tokenResponse.Token;
        }
    }
}
