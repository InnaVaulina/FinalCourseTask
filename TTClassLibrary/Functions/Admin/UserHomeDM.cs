using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TTClassLibrary.DataModel;
using TTClassLibrary.RequestSender;
namespace TTClassLibrary.Functions.Admin
{
    public class UserHomeDM
    {
        IHttpRequestSender requestSender;

        public UserHomeDM(IHttpRequestSender _requestSender, User _user)
        {
            requestSender = _requestSender;
            userContent = _user;
        }

        User userContent;
        public User UserContent
        {
            get { return userContent; }
            set
            {
                userContent = value;
            }
        }

        public async Task ResetPassword(PasswordChangeModel passwordModel)
        {
            var jsonContent = JsonContent.Create(passwordModel);
            await requestSender.ResetCurrentUserPassword(jsonContent);
        }

        
    }
}
