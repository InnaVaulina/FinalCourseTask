using TTClassLibrary.DataModel;
using TTClassLibrary.Functions.Authorization;
using TTClassLibrary.RequestSender;
using TTClassLibrary.Support;

namespace TTClassLibrary.Functions.Admin
{
    public class UserManagerDM
    {
        IHttpRequestSender requestSender;

        public UserManagerDM(IHttpRequestSender _requestSender)
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
