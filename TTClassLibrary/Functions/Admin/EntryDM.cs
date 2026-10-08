using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TTClassLibrary.Functions.Authorization;
using TTClassLibrary.Support;
using static TTClassLibrary.Functions.Authorization.AuthorizeRequestSender;

namespace TTClassLibrary.Functions.Admin
{
    public class EntryDM
    {
        UserManagerRequestSender requestSender;

        UserEntryM entry;

        public UserEntryM Entry
        {
            get { return entry; }
            set { entry = value; }
        }
        public EntryDM(UserManagerRequestSender requestSender)
        {
            this.requestSender = requestSender;
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
