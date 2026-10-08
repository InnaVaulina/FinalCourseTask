using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTClassLibrary.DataModel;
using TTClassLibrary.Functions.Blog;
using TTClassLibrary.Support;

namespace TTClassLibrary.Functions.Admin
{
    public class UserManagerDM
    {
        UserManagerRequestSender requestSender;

        public UserManagerDM(UserManagerRequestSender _requestSender)
        {
            requestSender = _requestSender;
            list = new List<UserModel>();
        }

        List<UserModel> list;
        public List<UserModel> List { get { return list; } }

        public async Task SelectUsers()
        {
            list.Clear();
            var response = await requestSender.PullAdminList();
            var jsonSerializer = new HttpResponseMessageDeserialize<List<UserModel>>();
            list = await jsonSerializer.DeserealizeResultToContentAsync(response);
        }

        public UserItemDM CreateUserItemDM(UserModel user)
        {
            var dm = new UserItemDM(requestSender, user);
            return dm;
        }

        public RegistrationDM CreateNewUserDM()
        {
            var dm = new RegistrationDM(requestSender);
            return dm;
        }
    }
}
