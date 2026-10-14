using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TTClassLibrary.DataModel;
using TTClassLibrary.RequestSender;

namespace TTClassLibrary.Functions.Admin
{
    public class UserItemDM
    {
        IHttpRequestSender requestSender;

        public UserItemDM(IHttpRequestSender _requestSender, UserModel _userContent)
        {
            requestSender = _requestSender;
            userContent = _userContent;
        }

        UserModel userContent;
        public UserModel UserContent
        {
            get { return userContent; }
            set
            {
                userContent = value;
            }
        }

        public async Task DeleteUser()
        {
            await requestSender.DeleteUser(userContent.Id);
        }

        public async Task ResetPassword(PasswordModel passwordModel)
        {
            var jsonContent = JsonContent.Create(passwordModel);
            await requestSender.ResetUserPassword(userContent.Id, jsonContent);
        }

        public async Task<HttpResponseMessage> ChangeUserRoles(List<string> newRoles)
        {
            var jsonContent = JsonContent.Create(newRoles);
            return await requestSender.ChangeUserRole(userContent.Id, jsonContent);
        }
    }
}
