using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTClassLibrary.DataModel;
using System.Net.Http.Json;

namespace TTClassLibrary.Functions.Admin
{
    public class UserItemDM
    {
        UserManagerRequestSender requestSender;

        public UserItemDM(UserManagerRequestSender _requestSender, UserModel _userContent)
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

        public async Task ChangeUserRoles(List<UserRole> newRoles)
        {
            var jsonContent = JsonContent.Create(newRoles);
            await requestSender.ChangeUserRole(userContent.Id, jsonContent);
        }
    }
}
